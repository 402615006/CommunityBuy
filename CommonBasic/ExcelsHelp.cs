using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using Aspose.Cells;
using NPOI.XSSF.UserModel;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.CommonBasic
{
    /// <summary>
    /// 功能描述：关于Excel的操作
    /// 创建者：CGD
    /// 建立时间：2014-1-12
    /// </summary>
    public class ExcelsHelp : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        public ExcelsHelp()
            : this(ExcelsHelp.FontSize.small)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fs"></param>
        public ExcelsHelp(FontSize fs)
        {
            this._FontSize = fs;
            _strXmlLeft = "<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\"";
            _strXmlLeft += "xmlns=\"http://www.w3.org/TR/REC-html40\">";
            _strXmlLeft += "";
            _strXmlLeft += "<head>";
            _strXmlLeft += "<meta http-equiv='Content-Type' content='text/html; charset=gb2312'>";
            _strXmlLeft += "<style type=\"text/css\">";
            _strXmlLeft += "<!--tr";
            _strXmlLeft += "	{mso-height-source:auto;}";
            _strXmlLeft += "td";
            _strXmlLeft += "	{white-space:nowrap;}";
            _strXmlLeft += ".wc4680812";
            _strXmlLeft += "	{white-space:nowrap;";
            _strXmlLeft += "	font-family:宋体;";
            _strXmlLeft += "	mso-number-format:General;";
            //_strXmlLeft		+=	"	font-size:10pt;";
            _strXmlLeft += "	font-size:" + this._xmlFontSize + ";";
            _strXmlLeft += "	font-weight:auto;";
            _strXmlLeft += "	font-style:auto;";
            _strXmlLeft += "	text-decoration:auto;";
            _strXmlLeft += "	mso-background-source:auto;";
            _strXmlLeft += "	mso-pattern:auto;";
            _strXmlLeft += "	mso-color-source:auto;";
            _strXmlLeft += "	text-align:general;";
            _strXmlLeft += "	vertical-align:bottom;";
            _strXmlLeft += "	border-top:none;";
            _strXmlLeft += "	border-left:none;";
            _strXmlLeft += "	border-right:none;";
            _strXmlLeft += "	border-bottom:none;";
            _strXmlLeft += "	mso-protection:locked;}";
            _strXmlLeft += "-->";
            _strXmlLeft += "</style>";
            _strXmlLeft += "</head>";
            _strXmlLeft += "";
            _strXmlLeft += "<body>";
            _strXmlLeft += "<!--[if gte mso 9]><xml>";
            _strXmlLeft += " <x:ExcelWorkbook>";
            _strXmlLeft += "  <x:ExcelWorksheets>";
            _strXmlLeft += "   <x:ExcelWorksheet>";
            _strXmlLeft += "    <x:OWCVersion>9.0.0.3821</x:OWCVersion>";
            _strXmlLeft += "    <x:Label Style='border-top:solid .5pt silver;border-left:solid .5pt silver;";
            _strXmlLeft += "     border-right:solid .5pt silver;border-bottom:solid .5pt silver'>";
            _strXmlLeft += "     <x:Caption>Microsoft Office Spreadsheet</x:Caption>";
            _strXmlLeft += "    </x:Label>";
            _strXmlLeft += "    <x:Name>Sheet1</x:Name>";
            _strXmlLeft += "    <x:WorksheetOptions>";
            _strXmlLeft += "     <x:Selected/>";
            //_strXmlLeft		+=	"     <x:Height>7620</x:Height>";
            _strXmlLeft += "     <x:Height>" + this._xmlX_Height + "</x:Height>";
            //_strXmlLeft		+=	"     <x:Width>15240</x:Width>";
            _strXmlLeft += "     <x:Width>" + this._xmlX_Width + "</x:Width>";
            _strXmlLeft += "     <x:TopRowVisible>0</x:TopRowVisible>";
            _strXmlLeft += "     <x:LeftColumnVisible>0</x:LeftColumnVisible>";
            _strXmlLeft += "     <x:ProtectContents>False</x:ProtectContents>";
            //_strXmlLeft		+=	"     <x:DefaultRowHeight>210</x:DefaultRowHeight>";
            _strXmlLeft += "     <x:DefaultRowHeight>" + this._xmlX_DefaultRowHeight + "</x:DefaultRowHeight>";
            _strXmlLeft += "     <x:StandardWidth>2389</x:StandardWidth>";
            _strXmlLeft += "    </x:WorksheetOptions>";
            _strXmlLeft += "   </x:ExcelWorksheet>";
            _strXmlLeft += "  </x:ExcelWorksheets>";
            _strXmlLeft += "  <x:MaxHeight>80%</x:MaxHeight>";
            _strXmlLeft += "  <x:MaxWidth>80%</x:MaxWidth>";
            _strXmlLeft += " </x:ExcelWorkbook>";
            _strXmlLeft += "</xml><![endif]-->";

            _strXmlRight = "</table>";
            _strXmlRight += "";
            _strXmlRight += "</body>";
            _strXmlRight += "";
            _strXmlRight += "</html>";


            //_strXmlRowLeft		=	"<tr height=\"22\">";			
            _strXmlRowLeft = "<tr height=\"" + this._xmlTr_height + "\">";
            _strXmlRowRight = "</tr>";

            _strXmlCellLeft = "<td width=\"30\">";
            _strXmlCellRight = "</td>";

            _strXmlCellLeft_Number = "<td x:num=\"";

            _strXmlCellRight_Number = "\"></td>";

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="captions"></param>
        /// <returns></returns>
        public string BindExcelXML(DataTable dt, string[] captions)
        {
            for (int i = 0; i < dt.Columns.Count && i < captions.Length; i++)
            {
                dt.Columns[i].Caption = captions[i];
            }
            return BindExcelXML(dt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string BindExcelXML(DataTable dt)
        {
            int i, j;
            DataRow dr;
            DateTime dtTemp;
            StringBuilder sbTemp = new StringBuilder();

            if (dt.Columns.Count == 0)
            {
                return getEmptyXML();
            }
            else if (dt.Rows.Count == 0)
            {
                sbTemp.Append(strXmlLeft);
                sbTemp.Append(getTableDefine(dt.Columns.Count));
                sbTemp.Append(strXmlRowLeft);

                for (i = 0; i < dt.Columns.Count; i++)
                {
                    sbTemp.Append(strXmlCellLeft);
                    sbTemp.Append(dt.Columns[i].Caption);
                    sbTemp.Append(strXmlCellRight);
                }
                sbTemp.Append(strXmlRowRight);
                sbTemp.Append(strXmlRight);

                return sbTemp.ToString();
            }
            else
            {
                sbTemp.Append(strXmlLeft);
                sbTemp.Append(getTableDefine(dt.Columns.Count));
                sbTemp.Append(strXmlRowLeft);

                for (i = 0; i < dt.Columns.Count; i++)
                {
                    sbTemp.Append(strXmlCellLeft);
                    sbTemp.Append(dt.Columns[i].Caption);
                    sbTemp.Append(strXmlCellRight);
                }
                sbTemp.Append(strXmlRowRight);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    sbTemp.Append(strXmlRowLeft);
                    for (j = 0; j < dt.Columns.Count; j++)
                    {
                        string type = dr[j].GetType().ToString();
                        if (type == "System.DateTime")
                        {
                            dtTemp = (DateTime)dr[j];
                            sbTemp.Append(strXmlCellLeft);
                            sbTemp.Append(dtTemp.ToShortDateString());
                            sbTemp.Append(strXmlCellRight);
                        }
                        else if (type == "System.Int32" || type == "System.Decimal")
                        {
                            sbTemp.Append(_strXmlCellLeft_Number);
                            sbTemp.Append(dr[j].ToString());
                            sbTemp.Append(_strXmlCellRight_Number);
                        }
                        else
                        {
                            sbTemp.Append(strXmlCellLeft);
                            sbTemp.Append(dr[j].ToString());
                            sbTemp.Append(strXmlCellRight);
                        }
                    }
                    sbTemp.Append(strXmlRowRight);
                }
                sbTemp.Append(strXmlRight);
                return sbTemp.ToString();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColName"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string BindExcelXML(DataTable dt, string[] ColName, string[] col)
        {
            int i;
            DataRow dr;
            DateTime dtTemp;
            StringBuilder sbTemp = new StringBuilder();

            if (dt.Columns.Count == 0)
            {
                return getEmptyXML();
            }
            else if (dt.Rows.Count == 0)
            {
                sbTemp.Append(strXmlLeft);
                sbTemp.Append(getTableDefine(dt.Columns.Count));
                sbTemp.Append(strXmlRowLeft);

                for (i = 0; i < ColName.Length; i++)
                {
                    sbTemp.Append(strXmlCellLeft);
                    sbTemp.Append(ColName[i]);
                    sbTemp.Append(strXmlCellRight);
                }
                sbTemp.Append(strXmlRowRight);
                sbTemp.Append(strXmlRight);

                return sbTemp.ToString();
            }
            else
            {
                sbTemp.Append(strXmlLeft);
                sbTemp.Append(getTableDefine(dt.Columns.Count));
                sbTemp.Append(strXmlRowLeft);

                for (i = 0; i < ColName.Length; i++)
                {
                    sbTemp.Append(strXmlCellLeft);
                    sbTemp.Append(ColName[i]);
                    sbTemp.Append(strXmlCellRight);
                }
                sbTemp.Append(strXmlRowRight);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    sbTemp.Append(strXmlRowLeft);

                    foreach (string caption in col)
                    {
                        string type = dr[caption].GetType().ToString();
                        if (type == "System.DateTime")
                        {
                            dtTemp = (DateTime)dr[caption];
                            sbTemp.Append(strXmlCellLeft);
                            sbTemp.Append(dtTemp.ToString("yyyy-MM-dd HH:mm:ss"));
                            sbTemp.Append(strXmlCellRight);
                        }
                        else if (type == "System.Int32" || type == "System.Decimal")
                        {
                            sbTemp.Append(_strXmlCellLeft_Number);
                            sbTemp.Append(dr[caption].ToString());
                            sbTemp.Append(_strXmlCellRight_Number);
                        }
                        //else if (type == "System.Int64")
                        //{
                        //    sbTemp.Append("<td class=xl26>");
                        //    sbTemp.Append(dr[caption].ToString());
                        //    sbTemp.Append("</td>");
                        //}
                        else
                        {
                            sbTemp.Append(strXmlCellLeft);
                            sbTemp.Append((dr[caption].ToString()));
                            sbTemp.Append(strXmlCellRight);
                        }
                    }

                    sbTemp.Append(strXmlRowRight);
                }
                sbTemp.Append(strXmlRight);
                return sbTemp.ToString();
            }

        }

        /// <summary>
        /// 需要把Excel文件保存的
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        /// <param name="ColName"></param>
        /// <param name="col"></param>
        public void BindExcelXML(DataTable dt, string path, string[] ColName, string[] col)
        {
            int i;
            DataRow dr;
            DateTime dtTemp;
            StringBuilder sbTemp = new StringBuilder();
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("gb2312"));

            if (dt.Columns.Count == 0)
            {
                getEmptyXML();
            }
            else if (dt.Rows.Count == 0)
            {
                sbTemp.Append(strXmlLeft);
                sbTemp.Append(getTableDefine(dt.Columns.Count));
                sbTemp.Append(strXmlRowLeft);

                for (i = 0; i < ColName.Length; i++)
                {
                    sbTemp.Append(strXmlCellLeft);
                    sbTemp.Append(ColName[i]);
                    sbTemp.Append(strXmlCellRight);
                }
                sbTemp.Append(strXmlRowRight);
                sbTemp.Append(strXmlRight);

                sbTemp.ToString();
            }
            else
            {
                sbTemp.Append(strXmlLeft);
                sbTemp.Append(getTableDefine(dt.Columns.Count));
                sbTemp.Append(strXmlRowLeft);

                for (i = 0; i < ColName.Length; i++)
                {
                    sbTemp.Append(strXmlCellLeft);
                    sbTemp.Append(ColName[i]);
                    sbTemp.Append(strXmlCellRight);
                }
                sbTemp.Append(strXmlRowRight);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    sbTemp.Append(strXmlRowLeft);

                    foreach (string caption in col)
                    {
                        string type = dr[caption].GetType().ToString();
                        if (type == "System.DateTime")
                        {
                            dtTemp = (DateTime)dr[caption];
                            sbTemp.Append(strXmlCellLeft);
                            sbTemp.Append(dtTemp.ToShortDateString());
                            sbTemp.Append(strXmlCellRight);
                        }
                        else if (type == "System.Int32" || type == "System.Decimal")
                        {
                            sbTemp.Append(_strXmlCellLeft_Number);
                            sbTemp.Append(dr[caption].ToString());
                            sbTemp.Append(_strXmlCellRight_Number);
                        }
                        else
                        {
                            sbTemp.Append(strXmlCellLeft);
                            sbTemp.Append(dr[caption].ToString());
                            sbTemp.Append(strXmlCellRight);
                        }
                    }

                    sbTemp.Append(strXmlRowRight);
                }
                sbTemp.Append(strXmlRight);
                //				return sbTemp.ToString();
                //sw
                sw.WriteLine(sbTemp);
            }
            sw.Close();
        }

        //Added By Chengguodong 2008-9-3 
        public string BindExcelXML(DataTable dt, string[] ColName, string[] col, string[,] HeadCustomData, int Width)
        {
            int i;
            DataRow dr;
            DateTime dtTemp;
            StringBuilder sbTemp = new StringBuilder();

            if (dt.Columns.Count == 0)
            {
                return getEmptyXML();
            }
            else if (dt.Rows.Count == 0)
            {
                sbTemp.Append(strXmlLeft);
                sbTemp.Append(getTableDefine(dt.Columns.Count));

                //在头部添加自定义数据
                for (i = 0; i < HeadCustomData.Length / 2; i++)
                {
                    string Str = "<tr height=\"22\">";
                    string colspan = "";
                    if (i == 0)
                    {
                        colspan = HeadCustomData[i, 1];
                        Str += "<td Width=\"" + Width + "\" colspan=\"" + colspan + "\" align=\"center\">" + HeadCustomData[i, 0] + "</td></tr>";
                        Str += "<tr height=\"22\"><td colspan=\"" + colspan + "\" align=\"center\"> </td>";
                    }
                    else
                    {
                        string[] ArrVal = HeadCustomData[i, 0].Split(',');
                        string[] ArrCol = HeadCustomData[i, 1].Split(',');
                        for (int j = 0; j < ArrVal.Length; j++)
                        {
                            Str += "<td colspan=\"" + ArrCol[j] + "\" align=\"center\">" + ArrVal[j] + "</td>";
                        }
                    }
                    Str += "</tr>";

                    sbTemp.Append(Str);
                }

                sbTemp.Append(strXmlRowLeft);
                for (i = 0; i < ColName.Length; i++)
                {
                    sbTemp.Append(strXmlCellLeft);
                    sbTemp.Append(ColName[i]);
                    sbTemp.Append(strXmlCellRight);
                }
                sbTemp.Append(strXmlRowRight);

                sbTemp.Append(strXmlRight);

                return sbTemp.ToString();
            }
            else
            {
                sbTemp.Append(strXmlLeft);
                sbTemp.Append(getTableDefine(dt.Columns.Count));

                //在头部添加自定义数据
                for (i = 0; i < HeadCustomData.Length / 2; i++)
                {
                    string Str = "<tr height=\"22\">";
                    string colspan = "";
                    if (i == 0)
                    {
                        colspan = HeadCustomData[i, 1];
                        Str += "<td Width=\"" + Width + "\" style=\"font-size:18px; font-weight:bold;\" colspan=\"" + colspan + "\" align=\"center\">" + HeadCustomData[i, 0] + "</td></tr>";
                        Str += "<tr height=\"22\"><td colspan=\"" + colspan + "\" align=\"left\"> </td>";
                    }
                    else
                    {
                        string[] ArrVal = HeadCustomData[i, 0].Split(',');
                        string[] ArrCol = HeadCustomData[i, 1].Split(',');
                        for (int j = 0; j < ArrVal.Length; j++)
                        {
                            Str += "<td colspan=\"" + ArrCol[j] + "\" align=\"center\">" + ArrVal[j] + "</td>";
                        }
                    }
                    Str += "</tr>";

                    sbTemp.Append(Str);
                }

                sbTemp.Append(strXmlRowLeft);
                for (i = 0; i < ColName.Length; i++)
                {
                    sbTemp.Append(strXmlCellLeft);
                    sbTemp.Append(ColName[i]);
                    sbTemp.Append(strXmlCellRight);
                }

                sbTemp.Append(strXmlRowRight);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    sbTemp.Append(strXmlRowLeft);

                    foreach (string caption in col)
                    {
                        string type = dr[caption].GetType().ToString();
                        if (type == "System.DateTime")
                        {
                            dtTemp = (DateTime)dr[caption];
                            sbTemp.Append(strXmlCellLeft);
                            sbTemp.Append(dtTemp.ToShortDateString());
                            sbTemp.Append(strXmlCellRight);
                        }
                        else if (type == "System.Int32" || type == "System.Decimal")
                        {
                            sbTemp.Append(_strXmlCellLeft_Number);
                            sbTemp.Append(dr[caption].ToString());
                            sbTemp.Append(_strXmlCellRight_Number);
                        }
                        else
                        {
                            sbTemp.Append(strXmlCellLeft);
                            sbTemp.Append((dr[caption].ToString()));
                            sbTemp.Append(strXmlCellRight);
                        }
                    }

                    sbTemp.Append(strXmlRowRight);
                }
                sbTemp.Append(strXmlRight);
                return sbTemp.ToString();
            }

        }
        //Added End



        /// <summary>
        /// 
        /// </summary>
        public enum FontSize
        {
            /// <summary>
            /// 
            /// </summary>
            small,
            /// <summary>
            /// 
            /// </summary>
            normal,
            /// <summary>
            /// 
            /// </summary>
            large
        }

        private enum DataType
        {
            OWC_str,
            OWC_num,
            OWC_none
        }

        private ExcelsHelp.FontSize _FontSize;

        private string _strXmlLeft;

        private string _strXmlRight;

        private string _strXmlRowLeft;

        private string _strXmlRowRight;

        private string _strXmlCellLeft;

        private string _strXmlCellRight;

        private string _strXmlCellLeft_Number;

        private string _strXmlCellRight_Number;

        private string GetDataTypeString(DataType dt)
        {
            string temp;
            switch (dt)
            {
                case DataType.OWC_str:
                    temp = "x:str";
                    break;
                case DataType.OWC_num:
                    temp = "x:num";
                    break;
                default:
                    temp = "";
                    break;
            }
            return temp;
        }

        private string _xmlFontSize
        {
            get
            {
                switch (this._FontSize)
                {
                    case ExcelsHelp.FontSize.small:
                        return "12pt";
                    case ExcelsHelp.FontSize.normal:
                        return "16pt";
                    case ExcelsHelp.FontSize.large:
                        return "20pt";
                    default:
                        return "12pt";
                }

            }
        }

        private string _xmlX_Height
        {
            get
            {
                switch (this._FontSize)
                {
                    case ExcelsHelp.FontSize.small:
                        return "7620";
                    case ExcelsHelp.FontSize.normal:
                        return "10583";
                    case ExcelsHelp.FontSize.large:
                        return "10583";
                    default:
                        return "7620";
                }
            }
        }

        private string _xmlX_Width
        {
            get
            {
                switch (this._FontSize)
                {
                    case ExcelsHelp.FontSize.small:
                        return "15240";
                    case ExcelsHelp.FontSize.normal:
                        return "18521";
                    case ExcelsHelp.FontSize.large:
                        return "18521";
                    default:
                        return "15240";
                }

            }
        }

        private string _xmlX_DefaultRowHeight
        {
            get
            {
                switch (this._FontSize)
                {
                    case ExcelsHelp.FontSize.small:
                        return "210";
                    case ExcelsHelp.FontSize.normal:
                        return "360";
                    case ExcelsHelp.FontSize.large:
                        return "465";
                    default:
                        return "210";
                }
            }
        }

        private string _xmlTr_height
        {
            get
            {
                switch (this._FontSize)
                {
                    case ExcelsHelp.FontSize.small:
                        return "18";
                    case ExcelsHelp.FontSize.normal:
                        return "24";
                    case ExcelsHelp.FontSize.large:
                        return "31";
                    default:
                        return "18";
                }

            }
        }

        private string strXmlLeft
        {
            get
            {
                return _strXmlLeft;
            }
        }

        private string strXmlRight
        {
            get
            {
                return _strXmlRight;
            }
        }

        private string strXmlRowLeft
        {
            get
            {
                return _strXmlRowLeft;
            }
        }

        private string strXmlRowRight
        {
            get
            {
                return _strXmlRowRight;
            }
        }

        private string strXmlCellLeft
        {
            get
            {
                return _strXmlCellLeft;
            }
        }

        private string strXmlCellRight
        {
            get
            {
                return _strXmlCellRight;
            }
        }

        private string strXmlCellLeft_Number
        {
            get
            {
                return _strXmlCellLeft_Number;
            }
        }

        private string strXmlCellRight_Number
        {
            get
            {
                return _strXmlCellRight_Number;
            }
        }

        private string getTableDefine(int intCols)
        {
            return this.getTableDefine(intCols, DataType.OWC_str);
        }

        private string getTableDefine(int intCols, DataType dataType)
        {
            string strTemp;
            int i;

            strTemp = "<table class=wc4680812 " + this.GetDataTypeString(dataType) + ">";
            for (i = 0; i < intCols; i++)
            {
                strTemp += " <col class=wc4680812 width=\"72\">";
            }
            return strTemp;
        }

        private string getEmptyXML()
        {
            string strTemp;

            strTemp = strXmlLeft;
            strTemp += getTableDefine(1);
            strTemp += strXmlRowLeft;
            strTemp += strXmlCellLeft;
            strTemp += strXmlCellRight;
            strTemp += strXmlRowRight;
            strTemp += strXmlRight;

            return strTemp;
        }

        /// <summary>
        /// 不保存文件的
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void ExportExcelFileA(DataTable dt, string fileName)
        {
            ExcelsHelp owc = new ExcelsHelp();
            string excel = owc.BindExcelXML(dt);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");

            response.ContentType = "application/vnd.ms-excel";
            response.Charset = "gb2312";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            response.Write(excel);
            response.End();
        }
        /// <summary>
        /// 不保存Excel文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="colName"></param>
        /// <param name="col"></param>
        public static void ExportExcelFileB(DataTable dt, string fileName, string[] colName, string[] col)
        {
            ExcelsHelp owc = new ExcelsHelp();
            string excel = owc.BindExcelXML(dt, colName, col);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");

            response.ContentType = "application/vnd.ms-excel";
            response.Charset = "gb2312";
            //fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            response.Write(excel);
            response.End();
        }
        /// <summary>
        /// 保存Excel文件的
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="colName"></param>
        /// <param name="col"></param>
        public static void ExportExcelFileA(DataTable dt, string fileName, string[] colName, string[] col)
        {
            ExcelsHelp owc = new ExcelsHelp();
            owc.BindExcelXML(dt, fileName, colName, col);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");

            response.ContentType = "application/vnd.ms-excel";
            response.Charset = "gb2312";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            response.WriteFile(fileName);
            response.End();
        }

        public static void SaveToExcelFile(DataTable dt, string fileName, string[] colName, string[] col)
        {
            ExcelsHelp owc = new ExcelsHelp();
            owc.BindExcelXML(dt, fileName, colName, col);
        }

        //Added By Chengguodong 2008-9-3
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="colName"></param>
        /// <param name="col"></param>
        /// <param name="HeadCustomData"></param>
        /// <param name="Width"></param>
        public static void ExportExcelFileB(DataTable dt, string fileName, string[] colName, string[] col, string[,] HeadCustomData, int Width)
        {
            ExcelsHelp owc = new ExcelsHelp();
            string excel = owc.BindExcelXML(dt, colName, col, HeadCustomData, Width);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");

            response.ContentType = "application/vnd.ms-excel";
            response.Charset = "gb2312";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            response.Write(excel);

            response.End();
        }
        //Added End

        public static string GetConnectionExcelString(string FileName)
        {
            string ConnectionStr = String.Empty;
            if (FileName.Length > 0)
            {
                switch (Path.GetExtension(FileName))
                {
                    case ".xls":
                        ConnectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;";
                        ConnectionStr += "Data Source=";
                        ConnectionStr += FileName;
                        ConnectionStr += ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
                        break;
                    case ".xlsx":
                        ConnectionStr = "Provider=Microsoft.ACE.OleDb.12.0;";
                        ConnectionStr += "Data Source=";
                        ConnectionStr += FileName;
                        ConnectionStr += ";Extended Properties=\"Excel 12.0;HDR=YES\"";
                        break;
                }
            }
            return ConnectionStr;
        }

        /// <summary>
        /// 获取上传Excel文件的指定表名数据
        /// </summary>
        /// <param name="FileName">上传文件的完全路径</param>
        /// <param name="TableName">工作表名</param>
        /// <returns>返回数据集</returns>
        public static DataTable GetUploadExcelFileInfo(string FileName, string TableName, string WhereStr)
        {
            if (FileName.Length > 0)
            {
                string ConnectionStr = GetConnectionExcelString(FileName);
                if (ConnectionStr.Length > 0)
                {
                    DataSet Ds = new DataSet();
                    DataTable TablesName = new DataTable();
                    OleDbConnection oleDBConn = new OleDbConnection(ConnectionStr);
                    try
                    {
                        oleDBConn.Open();
                        string SqlStr = " SELECT *  FROM [" + TableName + "$] " + WhereStr;
                        OleDbDataAdapter oleDa = new OleDbDataAdapter(SqlStr, oleDBConn);
                        oleDa.Fill(Ds);
                        return Ds.Tables[0];

                    }
                    catch (Exception ex)
                    {
                        ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                        return null;
                    }
                    finally
                    {
                        oleDBConn.Close();
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fupload">控件</param>
        /// <param name="FilePath">保存文件路径</param>
        /// <returns></returns>
        public static bool SaveUploadExcelFile(FileUpload fupload, string FilePath, string FileName)
        {
            bool Flag = false;
            System.Web.HttpPostedFile postedFile = fupload.PostedFile;
            int fileLength = postedFile.ContentLength;
            byte[] buff = new byte[fileLength];
            Stream filestream = postedFile.InputStream;//建立文件流对象
            filestream.Read(buff, 0, fileLength);//读取流内容			
            DirectoryInfo dir = new DirectoryInfo(FilePath);
            if (!dir.Exists)
            {
                dir.Create();
            }
            FileStream fs = new FileStream(FilePath + FileName, FileMode.Create);
            try
            {
                fs.Write(buff, 0, buff.Length);
                Flag = true;
            }
            catch
            {
            }
            finally
            {
                fs.Close();
            }
            return Flag;
        }

        /// <summary>
        /// 将指定文件输出到页面
        /// </summary>
        /// <param name="FilePath">文件物理完全路径</param>
        public static void DwonLoadFileToPage(string FilePath)
        {
            if (FilePath.Length > 0)
            {
                string FileName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
                FileInfo flinfo = new FileInfo(FilePath);

                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.BufferOutput = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
                System.Web.HttpContext.Current.Response.AddHeader("Content-Length", flinfo.Length.ToString());
                System.Web.HttpContext.Current.Response.WriteFile(FilePath);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
                System.Web.HttpContext.Current.Response.Close();
            }
        }

        /// <summary>
        /// 导出数据到本地
        /// </summary>
        /// <param name="dt">要导出的数据</param>
        /// <param name="Title">表格标题</param>
        /// <param name="path">保存路径</param>
        public static void OutFileToDisk(DataTable dt, string Title, string path)
        {
            Workbook workbook = new Workbook(); //工作簿
            Worksheet sheet = workbook.Worksheets[0]; //工作表
            Cells cells = sheet.Cells;//单元格

            //为标题设置样式    
            Aspose.Cells.Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
            styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            styleTitle.Font.Name = "宋体";//文字字体
            styleTitle.Font.Size = 16;//文字大小
            styleTitle.Font.IsBold = true;//粗体

            //列样式
            Aspose.Cells.Style styleCol = workbook.Styles[workbook.Styles.Add()];//新增样式
            styleCol.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            styleCol.Font.Name = "宋体";//文字字体
            styleCol.Font.Size = 12;//文字大小
            styleCol.Font.IsBold = true;//粗体
            styleCol.IsTextWrapped = true;//单元格内容自动换行
            styleCol.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleCol.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleCol.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleCol.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            //数据样式
            Aspose.Cells.Style styleContent = workbook.Styles[workbook.Styles.Add()];//新增样式
            styleContent.HorizontalAlignment = TextAlignmentType.Left;//文字居左
            styleCol.IsTextWrapped = true;//单元格内容自动换行
            styleContent.Font.Name = "宋体";//文字字体
            styleContent.Font.Size = 12;//文字大小
            styleContent.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleContent.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleContent.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleContent.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            int Colnum = dt.Columns.Count;//表格列数
            int Rownum = dt.Rows.Count;//表格行数

            int NowCount = 0;
            //生成行1 标题行
            if (Title.Length > 0)
            {
                cells.Merge(0, 0, 1, Colnum);//合并单元格
                cells[NowCount, 0].PutValue(Title);//填写内容
                cells[NowCount, 0].SetStyle(styleTitle);
                cells.SetRowHeight(0, 38);
                NowCount++;
            }

            //生成行2 列名行
            for (int i = 0; i < Colnum; i++)
            {
                cells[NowCount + 0, i].PutValue(dt.Columns[i].ColumnName);
                cells[NowCount + 0, i].SetStyle(styleCol);
                cells.SetRowHeight(NowCount + 0, 25);
                if (dt.Columns[i].ColumnName.Length > 0)
                {
                    cells.SetColumnWidth(NowCount + i, dt.Columns[i].ColumnName.Length * 3);
                }
            }
            NowCount++;
            //生成数据行
            for (int i = 0; i < Rownum; i++)
            {
                for (int k = 0; k < Colnum; k++)
                {
                    cells[NowCount + i, k].PutValue(dt.Rows[i][k].ToString());
                    cells[NowCount + i, k].SetStyle(styleContent);
                }
                cells.SetRowHeight(NowCount + i, 22);
            }
            workbook.Save(path);
        }

        public static DataTable GetExeclToDataTable_template(string FileName, string TableName, out string ErrorStr)
        {
            ErrorStr = String.Empty;
            DataTable dt = new DataTable();
            Aspose.Cells.Workbook workbook = new Workbook();
            try
            {
                workbook.Open(FileName);
            }
            catch (Exception e)
            {
                switch (e.Message)
                {
                    case "This Excel files contains (Excel3.0 or earlier file format) records.":
                        ErrorStr = "您上传的Excel文件包含更早的版本的记录，请重新选择要上传的文件。";
                        break;
                    case "This Excel files contains (Excel4.0 or earlier file format) records.":
                        ErrorStr = "您上传的Excel文件包含更早的版本的记录，请重新选择要上传的文件。";
                        break;
                    case "This file's format is not supported or you don't specify a correct format.":
                        ErrorStr = "您上传的文件不是Excel格式，请重新选择要上传的文件。";
                        break;
                }
                dt = null;
            }
            bool Flag = false;
            string result = "";
            for (int i = 0; i < 1; i++)
            {
                Flag = true;
                Worksheet wst = workbook.Worksheets[i];
                int MaxR = wst.Cells.MaxRow;
                int MaxC = wst.Cells.MaxColumn;
                if (MaxR > 0 && MaxC > 0)
                {
                    dt = wst.Cells.ExportDataTableAsString(0, 0, MaxR + 1, MaxC + 1, true);
                    //string email = dt.Rows[0][1].ToString();
                    //string pwd = Back.CommonBasic.Helper.StringToMD5(dt.Rows[1][1].ToString());
                    //string province = dt.Rows[2][1].ToString();
                    //string city = dt.Rows[3][1].ToString();
                    //string county = dt.Rows[4][1].ToString();
                    //string linknames = dt.Rows[5][1].ToString();
                    //string units = dt.Rows[6][1].ToString();
                    //string department = dt.Rows[7][1].ToString();
                    //string address = dt.Rows[8][1].ToString();
                    //string zipcode = dt.Rows[9][1].ToString();
                    //string Fixednum = dt.Rows[10][1].ToString();
                    //string faxnum = dt.Rows[22][1].ToString();
                }
                else
                {
                    ErrorStr = "您上传的Excel文件没有数据，请重新选择要上传的文件。";
                    dt = null;
                }
            }
            if (!Flag)
            {
                ErrorStr = "您上传的Excel文件不存在指定的工作表，请重新选择要上传的文件。";
                dt = null;
            }
            return dt;
        }

        public static DataTable GetExeclToDataTable_template(string FileName, string TableName, int index, out string ErrorStr)
        {
            ErrorStr = String.Empty;
            DataTable dt = new DataTable();
            Aspose.Cells.Workbook workbook = new Workbook();
            try
            {
                workbook.Open(FileName);
            }
            catch (Exception e)
            {
                switch (e.Message)
                {
                    case "This Excel files contains (Excel3.0 or earlier file format) records.":
                        ErrorStr = "您上传的Excel文件包含更早的版本的记录，请重新选择要上传的文件。";
                        break;
                    case "This Excel files contains (Excel4.0 or earlier file format) records.":
                        ErrorStr = "您上传的Excel文件包含更早的版本的记录，请重新选择要上传的文件。";
                        break;
                    case "This file's format is not supported or you don't specify a correct format.":
                        ErrorStr = "您上传的文件不是Excel格式，请重新选择要上传的文件。";
                        break;
                }
                dt = null;
            }
            bool Flag = false;
            string result = "";

            Flag = true;
            Worksheet wst = workbook.Worksheets[index];
            int MaxR = wst.Cells.MaxRow;
            int MaxC = wst.Cells.MaxColumn;
            if (MaxR > 0 && MaxC > 0)
            {
                dt = wst.Cells.ExportDataTableAsString(0, 0, MaxR + 1, MaxC + 1, true);
                //string email = dt.Rows[0][1].ToString();
                //string pwd = Back.CommonBasic.Helper.StringToMD5(dt.Rows[1][1].ToString());
                //string province = dt.Rows[2][1].ToString();
                //string city = dt.Rows[3][1].ToString();
                //string county = dt.Rows[4][1].ToString();
                //string linknames = dt.Rows[5][1].ToString();
                //string units = dt.Rows[6][1].ToString();
                //string department = dt.Rows[7][1].ToString();
                //string address = dt.Rows[8][1].ToString();
                //string zipcode = dt.Rows[9][1].ToString();
                //string Fixednum = dt.Rows[10][1].ToString();
                //string faxnum = dt.Rows[22][1].ToString();
            }
            else
            {
                ErrorStr = "您上传的Excel文件没有数据，请重新选择要上传的文件。";
                dt = null;
            }
            if (!Flag)
            {
                ErrorStr = "您上传的Excel文件不存在指定的工作表，请重新选择要上传的文件。";
                dt = null;
            }
            return dt;
        }

        //李超
        public static void DataTabletoExcel(DataTable tmpDataTable, string strFileName, string[] colName)
        {
            if (tmpDataTable == null)
                return;
            int rowNum = tmpDataTable.Rows.Count;
            int columnNum = tmpDataTable.Columns.Count;
            int rowIndex = 1;
            int columnIndex = 0;

            //Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            //xlApp.DefaultFilePath = "";
            //xlApp.DisplayAlerts = true;
            //xlApp.SheetsInNewWorkbook = 1;
            //Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);

            //将DataTable的列名导入Excel表第一行
            foreach (string dc in colName)
            {
                columnIndex++;
                //xlApp.Cells[rowIndex, columnIndex] = dc;
            }

            //将DataTable中的数据导入Excel中
            for (int i = 0; i < rowNum; i++)
            {
                rowIndex++;
                columnIndex = 0;
                for (int j = 0; j < columnNum; j++)
                {
                    columnIndex++;
                    //xlApp.Cells[rowIndex, columnIndex] = tmpDataTable.Rows[i][j].ToString();
                }
            }
            //xlBook.SaveAs(strFileName, -4143);
        }

        /// <summary>
        /// Excel文件导成Datatable
        /// </summary>
        /// <param name="strFilePath">Excel文件目录地址</param>
        /// <param name="strTableName">Datatable表名</param>
        /// <param name="iSheetIndex">Excel sheet index</param>
        /// <returns></returns>
        public static DataTable XlSToDataTable(string strFilePath, string strTableName, int iSheetIndex)
        {
            string strExtName = Path.GetExtension(strFilePath);

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(strTableName))
            {
                dt.TableName = strTableName;
            }

            if (strExtName.Equals(".xls") || strExtName.Equals(".xlsx"))
            {
                using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(iSheetIndex);

                    //列头
                    foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                    {
                        dt.Columns.Add(item.ToString(), typeof(string));
                    }

                    //写入内容
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    while (rows.MoveNext())
                    {
                        IRow row = (XSSFRow)rows.Current;
                        if (row.RowNum == sheet.FirstRowNum)
                        {
                            continue;
                        }

                        DataRow dr = dt.NewRow();
                        foreach (ICell item in row.Cells)
                        {
                            item.SetCellType(CellType.String);
                            switch (item.CellType)
                            {
                                case CellType.Boolean:
                                    dr[item.ColumnIndex] = item.BooleanCellValue;
                                    break;
                                case CellType.Error:
                                    dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                    break;
                                case CellType.Formula:
                                    switch (item.CachedFormulaResultType)
                                    {
                                        case CellType.Boolean:
                                            dr[item.ColumnIndex] = item.BooleanCellValue;
                                            break;
                                        case CellType.Error:
                                            dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(item))
                                            {
                                                dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(item.StringCellValue))
                                                {
                                                    dr[item.ColumnIndex] = item.NumericCellValue;
                                                }
                                                else
                                                {
                                                    dr[item.ColumnIndex] = 0;
                                                }
                                            }
                                            break;
                                        case CellType.String:
                                            string str = item.StringCellValue;
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                dr[item.ColumnIndex] = str.ToString();
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = null;
                                            }
                                            break;
                                        case CellType.Unknown:
                                        case CellType.Blank:
                                        default:
                                            dr[item.ColumnIndex] = string.Empty;
                                            break;
                                    }
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(item))
                                    {
                                        dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(item.StringCellValue))
                                        {
                                            dr[item.ColumnIndex] = item.NumericCellValue;
                                        }
                                        else
                                        {
                                            dr[item.ColumnIndex] = 0;
                                        }
                                    }
                                    break;
                                case CellType.String:
                                    string strValue = item.StringCellValue;
                                    if (!string.IsNullOrEmpty(strValue))
                                    {
                                        dr[item.ColumnIndex] = strValue.ToString();
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = null;
                                    }
                                    break;
                                case CellType.Unknown:
                                case CellType.Blank:
                                default:
                                    dr[item.ColumnIndex] = string.Empty;
                                    break;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        public static DataTable XlSToDataTable97(string strFilePath, string strTableName, int iSheetIndex)
        {
            string strExtName = Path.GetExtension(strFilePath);

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(strTableName))
            {
                dt.TableName = strTableName;
            }

            if (strExtName.Equals(".xls") || strExtName.Equals(".xlsx"))
            {
                using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        HSSFWorkbook workbook = new HSSFWorkbook(file);
                        ISheet sheet = workbook.GetSheetAt(iSheetIndex);

                        //列头
                        foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                        {
                            dt.Columns.Add(item.ToString(), typeof(string));
                        }

                        //写入内容
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        while (rows.MoveNext())
                        {
                            IRow row = (HSSFRow)rows.Current;
                            if (row.RowNum == sheet.FirstRowNum)
                            {
                                continue;
                            }

                            DataRow dr = dt.NewRow();
                            foreach (ICell item in row.Cells)
                            {
                                item.SetCellType(CellType.String);
                                switch (item.CellType)
                                {
                                    case CellType.Boolean:
                                        dr[item.ColumnIndex] = item.BooleanCellValue;
                                        break;
                                    case CellType.Error:
                                        dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                        break;
                                    case CellType.Formula:
                                        switch (item.CachedFormulaResultType)
                                        {
                                            case CellType.Boolean:
                                                dr[item.ColumnIndex] = item.BooleanCellValue;
                                                break;
                                            case CellType.Error:
                                                dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                                break;
                                            case CellType.Numeric:
                                                if (DateUtil.IsCellDateFormatted(item))
                                                {
                                                    dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                                }
                                                else
                                                {
                                                    dr[item.ColumnIndex] = item.StringCellValue;
                                                }
                                                break;
                                            case CellType.String:
                                                string str = item.StringCellValue;
                                                if (!string.IsNullOrEmpty(str))
                                                {
                                                    dr[item.ColumnIndex] = str.ToString();
                                                }
                                                else
                                                {
                                                    dr[item.ColumnIndex] = null;
                                                }
                                                break;
                                            case CellType.Unknown:
                                            case CellType.Blank:
                                            default:
                                                dr[item.ColumnIndex] = string.Empty;
                                                break;
                                        }
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(item))
                                        {
                                            dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                        }
                                        else
                                        {
                                            dr[item.ColumnIndex] = item.StringCellValue;
                                        }
                                        break;
                                    case CellType.String:
                                        string strValue = item.StringCellValue;
                                        if (!string.IsNullOrEmpty(strValue))
                                        {
                                            dr[item.ColumnIndex] = strValue.ToString();
                                        }
                                        else
                                        {
                                            dr[item.ColumnIndex] = null;
                                        }
                                        break;
                                    case CellType.Unknown:
                                    case CellType.Blank:
                                    default:
                                        dr[item.ColumnIndex] = string.Empty;
                                        break;
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                    }
                }
            }
            return dt;
        }
    }
}
