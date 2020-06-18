using Aspose.Cells;
using Aspose.Cells.Rendering;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace CommunityBuy.CommonBasic
{
    /// <summary>
    /// 
    /// </summary>
    public class AsponseHelper
    {
        private bool _IsPrint = false;
        private string _PrintName = string.Empty;
        private string _IP = string.Empty;
        private int _Port = 9100;
        private string _VPath = string.Empty;
        private string _DPath = string.Empty;
        private string _FileName = string.Empty;

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint
        {
            get { return _IsPrint; }
            set { _IsPrint = value; }
        }

        /// <summary>
        /// 打印机名称，不传默认打印机
        /// </summary>
        public string PrintName
        {
            get { return _PrintName; }
            set { _PrintName = value; }
        }

        /// <summary>
        /// 打印机IP
        /// </summary>
        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

        /// <summary>
        /// 打印机端口
        /// </summary>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        /// <summary>
        /// 文件物理路径
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
        }

        /// <summary>
        /// 文件虚拟路径
        /// </summary>
        public string VPath
        {
            get { return _VPath; }
        }

        /// <summary>
        /// 文件物理路径
        /// </summary>
        public string DPath
        {
            get { return _DPath; }
        }

        WorkbookDesigner designer = null;

        public AsponseHelper()
        {
            string strPath = string.Empty;
            if (HttpContext.Current != null)
            {

                strPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\print\\" + DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                strPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName + "\\print\\" + DateTime.Now.ToString("yyyyMMdd");
            }
            DeleteFile(strPath);
        }

        /// <summary>
        /// 根据模板和数据获取解析后的HTML字符
        /// </summary>
        /// <param name="TemplateFilePath">模板文件绝对路径</param>
        /// <param name="ds">循环数据集</param>
        /// <param name="dt">单字段数据表</param>
        /// <returns></returns>
        public string OPenfileByFilePath(string TemplateFilePath, DataSet ds, DataTable dt)
        {
            string strhtml = string.Empty;
            CreateExcelByData(TemplateFilePath, ds, dt, string.Empty);

            try
            {
                Workbook workbook = OPenFile(_DPath);
                if (workbook != null)
                {
                    MemoryStream stream = new MemoryStream();
                    workbook.Save(stream, FileFormatType.Html);
                    strhtml = StreamToString(stream);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }

            return strhtml;
        }

        /// <summary>
        /// 生成excel文件，路径从对象取值
        /// </summary>
        /// <param name="ChineseName"></param>
        /// <param name="TemplateFilePath"></param>
        /// <param name="ds"></param>
        /// <param name="dt"></param>
        public void OPenfileByFilePath(string ChineseName, string TemplateFilePath, DataSet ds, DataTable dt)
        {
            CreateExcelByData(TemplateFilePath, ds, dt, string.Empty);
        }

        /// <summary>
        /// 生成Excel文档并打印
        /// </summary>
        /// <param name="ChineseName"></param>
        /// <param name="TemplateFilePath"></param>
        /// <param name="ds"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetExportOPenfileByFilePath(string ChineseName, string TemplateFilePath, DataSet ds, DataTable dt)
        {
            return GetExportOPenfileByFilePath(ChineseName, TemplateFilePath, ds, dt, 1);
        }

        /// <summary>
        /// 生成Excel文档并打印
        /// </summary>
        /// <param name="ChineseName"></param>
        /// <param name="TemplateFilePath"></param>
        /// <param name="ds"></param>
        /// <param name="dt"></param>
        /// <param name="isPrint">是否打印</param>
        /// <returns></returns>
        public string GetExportOPenfileByFilePath(string ChineseName, string TemplateFilePath, DataSet ds, DataTable dt, bool isPrint)
        {
            return GetExportOPenfileByFilePath(ChineseName, TemplateFilePath, ds, dt, isPrint, 1);
        }

        /// <summary>
        /// 生成Excel文档并打印
        /// </summary>
        /// <param name="ChineseName"></param>
        /// <param name="TemplateFilePath"></param>
        /// <param name="ds"></param>
        /// <param name="dt"></param>
        /// <param name="isPrint">是否打印</param>
        /// <returns></returns>
        public string GetExportOPenfileByFilePath(string ChineseName, string TemplateFilePath, DataSet ds, DataTable dt, bool isPrint, int PrintCount)
        {
            string strhtml = string.Empty;

            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Open(TemplateFilePath);

            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    designer.SetDataSource(ds.Tables[i]);
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    designer.SetDataSource(dt.Columns[k].ToString(), dr[k].ToString());
                }
            }
            designer.Process();

            //新工作表
            string file = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + "_" + ChineseName + ".xlsx";
            string strPath = string.Empty;
            string strLocalPath = "";
            if (HttpContext.Current != null)
            {

                strPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\print\\" + DateTime.Now.ToString("yyyyMMdd");
                strLocalPath += "/print/" + DateTime.Now.ToString("yyyyMMdd") + "/" + file;
            }
            else
            {
                strPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName + "\\print\\" + DateTime.Now.ToString("yyyyMMdd");
                strLocalPath += Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName + "\\print\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + file;
            }
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            strPath += "\\" + file;

            try
            {
                designer.Workbook.Save(strPath, FileFormatType.Xlsx);
                if (_IsPrint && isPrint)
                {
                    PrintExcelBySheet(designer.Workbook.Worksheets[0], _PrintName, file, PrintCount);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog,ex.ToString());
            }
            return strLocalPath;
        }
        /// <summary>
        /// 生成Excel文档并打印
        /// </summary>
        /// <param name="ChineseName"></param>
        /// <param name="TemplateFilePath"></param>
        /// <param name="ds"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetExportOPenfileByFilePath(string ChineseName, string TemplateFilePath, DataSet ds, DataTable dt, int PrintCount)
        {
            string strhtml = string.Empty;

            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Open(TemplateFilePath);

            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    designer.SetDataSource(ds.Tables[i]);
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    designer.SetDataSource(dt.Columns[k].ToString(), dr[k].ToString());
                }
            }
            designer.Process();

            //新工作表
            string file = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + "_" + ChineseName + ".xlsx";
            string strPath = string.Empty;
            string strLocalPath = "";
            if (HttpContext.Current != null)
            {

                strPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\print\\" + DateTime.Now.ToString("yyyyMMdd");
                strLocalPath += "/print/" + DateTime.Now.ToString("yyyyMMdd") + "/" + file;
            }
            else
            {
                strPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName + "\\print\\" + DateTime.Now.ToString("yyyyMMdd");
                strLocalPath += Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName + "\\print\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + file;
            }
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            strPath += "\\" + file;

            try
            {
                designer.Workbook.Save(strPath, FileFormatType.Xlsx);
                if (_IsPrint)
                {
                    PrintExcelBySheet(designer.Workbook.Worksheets[0], _PrintName, file, PrintCount);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
            return strLocalPath;
        }



        /// <summary>
        /// 打印excel文档
        /// </summary>
        /// <param name="worksheet">工作表</param>
        /// <param name="PrinterName">打印机名称，为空默认打印机</param>
        public void PrintExcelBySheet(Worksheet worksheet, string PrinterName, string FileName, int PrintCount)
        {
            worksheet.PageSetup.Orientation = PageOrientationType.Portrait;
            worksheet.PageSetup.Zoom = 100;//打印时页面设置,缩放比例
            worksheet.PageSetup.LeftMargin = 0;
            worksheet.PageSetup.RightMargin = 0;
            worksheet.PageSetup.BottomMargin = 0;
            worksheet.PageSetup.TopMargin = 0;

            Aspose.Cells.Rendering.ImageOrPrintOptions options = new Aspose.Cells.Rendering.ImageOrPrintOptions();
            options.PrintWithStatusDialog = false;
            options.OnePagePerSheet = true;
            options.PrintingPage = PrintingPageType.IgnoreBlank;
            options.ImageFormat = ImageFormat.Emf;
            SheetRender sr = new SheetRender(worksheet, options);
            string strPrinterName = PrinterName;
            if (PrinterName.Length == 0)
            {
                System.Drawing.Printing.PrinterSettings printSettings = new System.Drawing.Printing.PrinterSettings();
                strPrinterName = printSettings.PrinterName;
            }
            if (FileName.Length == 0)
            {
                FileName = "document";
            }
            //sr.ToImage(0, @"d:\" + FileName + ".jpg");
            for (int i = 0; i < PrintCount; i++)
            {
                sr.ToPrinter(strPrinterName, FileName);
            }
        }

        /// <summary>
        /// 将excel文件导出为图片
        /// </summary>
        /// <param name="worksheet">工作表</param>
        /// <param name="PrinterName">打印机名称，为空默认打印机</param>
        public Bitmap BitmapExcelBySheet(string TemplateFilePath, DataSet ds, DataTable dt)
        {
            Bitmap bmp = null;
            CreateExcelByData(TemplateFilePath, ds, dt, string.Empty);
            designer.Workbook.Worksheets[0].PageSetup.Orientation = PageOrientationType.Portrait;
            designer.Workbook.Worksheets[0].PageSetup.Zoom = 100;//打印时页面设置,缩放比例
            designer.Workbook.Worksheets[0].PageSetup.LeftMargin = 0;
            designer.Workbook.Worksheets[0].PageSetup.RightMargin = 0;
            designer.Workbook.Worksheets[0].PageSetup.BottomMargin = 0;
            designer.Workbook.Worksheets[0].PageSetup.TopMargin = 0;

            Aspose.Cells.Rendering.ImageOrPrintOptions options = new Aspose.Cells.Rendering.ImageOrPrintOptions();
            options.PrintWithStatusDialog = false;
            options.OnePagePerSheet = true;
            options.PrintingPage = PrintingPageType.IgnoreBlank;
            options.ImageFormat = ImageFormat.Emf;
            SheetRender sr = new SheetRender(designer.Workbook.Worksheets[0], options);
            bmp=sr.ToImage(0);
            return bmp;
        }



        /// <summary>
        /// 导出数据生成Exce弹出页面保存提示
        /// </summary>
        /// <param name="ChineseName">文件名称</param>
        /// <param name="TemplateFilePath">模板路径</param>
        /// <param name="ds"></param>
        /// <param name="dt"></param>
        public void ExportOPenfileByFilePath(string ChineseName, string TemplateFilePath, DataSet ds, DataTable dt)
        {
            CreateExcelByData(TemplateFilePath, ds, dt, ChineseName);

            //try
            //{
            //    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            //    response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            //    response.ContentType = "application/vnd.ms-excel";
            //    response.Charset = "gb2312";
            //    response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(_FileName, System.Text.Encoding.UTF8));
            //    response.WriteFile(_DPath);
            //    response.End();
            //}
            //catch (Exception ex)
            //{
            //    ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
            //}
        }

        /// <summary>
        /// 生成Excel文档并打印
        /// </summary>
        /// <param name="ChineseName"></param>
        /// <param name="TemplateFilePath"></param>
        /// <param name="ds"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool PrintOPenfileByFilePath(string ChineseName, string TemplateFilePath, DataSet ds, DataTable dt, int PrintCount)
        {
            CreateExcelByData(TemplateFilePath, ds, dt, ChineseName);
            try
            {
                if (_IsPrint)
                {
                    if (_IP.Length > 0 && _Port > 0)
                    {
                        PrintExcelInternetBySheet(designer.Workbook.Worksheets[0], _FileName, PrintCount);
                    }
                    else
                    {
                        PrintExcelBySheet(designer.Workbook.Worksheets[0], _FileName, PrintCount);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 打印excel文档
        /// </summary>
        /// <param name="worksheet">工作表</param>
        /// <param name="PrinterName">打印机名称，为空默认打印机</param>
        private void PrintExcelBySheet(Worksheet worksheet, string FileName, int PrintCount)
        {
            worksheet.PageSetup.Orientation = PageOrientationType.Portrait;
            worksheet.PageSetup.Zoom = 100;//打印时页面设置,缩放比例
            worksheet.PageSetup.LeftMargin = 0;
            worksheet.PageSetup.RightMargin = 0;
            worksheet.PageSetup.BottomMargin = 0;
            worksheet.PageSetup.TopMargin = 0;

            Aspose.Cells.Rendering.ImageOrPrintOptions options = new Aspose.Cells.Rendering.ImageOrPrintOptions();
            options.PrintWithStatusDialog = false;
            options.OnePagePerSheet = true;
            options.PrintingPage = PrintingPageType.IgnoreBlank;
            options.ImageFormat = ImageFormat.Emf;
            SheetRender sr = new SheetRender(worksheet, options);
            string strPrinterName = _PrintName;
            if (_PrintName.Length == 0)
            {
                System.Drawing.Printing.PrinterSettings printSettings = new System.Drawing.Printing.PrinterSettings();
                strPrinterName = printSettings.PrinterName;
            }
            if (FileName.Length == 0)
            {
                FileName = "document";
            }
            //sr.ToImage(0, @"d:\" + FileName + ".jpg");
            for (int i = 0; i < PrintCount; i++)
            {
                sr.ToPrinter(strPrinterName, FileName);
            }
        }


        /// <summary>
        /// 打印excel文档
        /// </summary>
        /// <param name="worksheet">工作表</param>
        /// <param name="PrinterName">打印机名称，为空默认打印机</param>
        private void PrintExcelInternetBySheet(Worksheet worksheet, string FileName, int PrintCount)
        {
            if (IP.Length == 0 || Port <= 0)
            {
                return;
            }
            worksheet.PageSetup.Orientation = PageOrientationType.Portrait;
            worksheet.PageSetup.Zoom = (int)(((decimal)200 / 85) * 100);//打印时页面设置,缩放比例,佳博打印机
            //worksheet.PageSetup.Zoom = 200;
            worksheet.PageSetup.LeftMargin = 0;
            worksheet.PageSetup.RightMargin = 0;
            worksheet.PageSetup.BottomMargin = 0;
            worksheet.PageSetup.TopMargin = 0;

            Aspose.Cells.Rendering.ImageOrPrintOptions options = new Aspose.Cells.Rendering.ImageOrPrintOptions();
            options.PrintWithStatusDialog = false;
            options.OnePagePerSheet = true;
            options.PrintingPage = PrintingPageType.IgnoreBlank;
            options.ImageFormat = ImageFormat.MemoryBmp;
            options.ChartImageType = ImageFormat.MemoryBmp;
            options.Quality = 100;
            SheetRender sr = new SheetRender(worksheet, options);
            string strPrinterName = _PrintName;
            if (_PrintName.Length == 0)
            {
                System.Drawing.Printing.PrinterSettings printSettings = new System.Drawing.Printing.PrinterSettings();
                strPrinterName = printSettings.PrinterName;
            }
            if (FileName.Length == 0)
            {
                FileName = "document";
            }
            Bitmap bmp = sr.ToImage(0);
            for (int i = 0; i < PrintCount; i++)
            {
                string printMessage = "";
                IpPrinter ipp = new IpPrinter(IP, Port);
                ipp.PrintBitmap(bmp, out printMessage);
            }
        }


        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <param name="TemplateFilePath">物理模板路径</param>
        /// <param name="ds"></param>
        /// <param name="dt"></param>
        /// <param name="ChineseName"></param>
        /// <returns></returns>
        private string CreateExcelByData(string TemplateFilePath, DataSet ds, DataTable dt, string ChineseName)
        {
            string strLocalPath = string.Empty;
            designer = new WorkbookDesigner();
            if (File.Exists(TemplateFilePath))
            {
                designer.Open(TemplateFilePath);
                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        designer.SetDataSource(ds.Tables[i]);
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        designer.SetDataSource(dt.Columns[k].ToString(), dr[k].ToString());
                    }
                }
                designer.Process();

                //新工作表
                string file = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                if (ChineseName.Length > 0)
                {
                    file += "_" + ChineseName + ".xlsx";
                }
                else
                {
                    file += ".xlsx";
                }
                _FileName = file;

                string strPath = string.Empty;

                if (HttpContext.Current != null)
                {

                    strPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\print\\" + DateTime.Now.ToString("yyyyMMdd");
                    strLocalPath += "/print/" + DateTime.Now.ToString("yyyyMMdd") + "/" + file;
                }
                else
                {
                    strPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName + "\\print\\" + DateTime.Now.ToString("yyyyMMdd");
                    strLocalPath += Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName + "\\print\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + file;
                }
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                strPath += "\\" + file;

                try
                {
                    designer.Workbook.Save(strPath, FileFormatType.Xlsx);
                    _DPath = strPath;
                    _VPath = strLocalPath;
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                }
            }

            return strLocalPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private Workbook OPenFile(string FilePath)
        {
            Workbook workbook = new Workbook();
            try
            {
                workbook.Open(FilePath);
            }
            catch (Exception ex)
            {

            }
            return workbook;
        }

        private string StreamToString(MemoryStream stream)
        {
            byte[] b = stream.ToArray();
            string s = System.Text.Encoding.UTF8.GetString(b, 0, b.Length);
            return s;
        }


        private static void DeleFile(string DirectoryName)
        {
            try
            {
                if (Directory.Exists(DirectoryName))
                {
                    Directory.Delete(DirectoryName);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void DeleteFile(string Path)
        {
            try
            {
                if (System.IO.Directory.Exists(Path))
                {
                    int nowtime = StringHelper.StringToInt(DateTime.Now.AddDays(-3).ToString("yyyyMMdd"));
                    DirectoryInfo Dir = new DirectoryInfo(Path);
                    DirectoryInfo[] dis = Dir.GetDirectories();

                    foreach (DirectoryInfo di in dis)
                    {
                        if (StringHelper.StringToInt(di.Name) < nowtime)
                        {
                            di.Delete(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
        }
    }
}
