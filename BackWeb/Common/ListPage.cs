using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.WebControl;
using Aspose.Cells;
using Wuqi.Webdiyer;

namespace CommunityBuy.BackWeb.Common
{
    public class ListPage : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                Control controlGrid = Page.FindControl("anp_top");
                if (controlGrid != null)
                {
                    AspNetPager Grid = (AspNetPager)controlGrid;
                    if (Grid.PageSize == 10)
                    {
                        Grid.PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
                    }
                }
            }
            base.OnLoad(e);
        }

        protected void VisibleToolBar(string linkname, bool Flag)
        {
            Control ToolBar = Page.FindControl("ToolBar1");
            if (ToolBar != null)
            {
                Control lt = ToolBar.FindControl(linkname);
                if (lt != null)
                {
                    LinkButton linkb = (LinkButton)lt;
                    linkb.Visible = Flag;
                }
            }
        }

        protected void SearchToolBarAddJSClick(string linkname, string clickName)
        {
            Control ToolBar = Page.FindControl("ToolBar1");
            if (ToolBar != null)
            {
                Control lt = ToolBar.FindControl(linkname);
                if (lt != null)
                {
                    LinkButton linkb = (LinkButton)lt;
                    linkb.OnClientClick = clickName;
                }
            }
        }

        /// <summary>
        /// CustDataGrid数据列表 获取选中的checkbox
        /// </summary>
        /// <returns></returns>
        public string GetSelectStr(CustDataGrid grid)
        {
            StringBuilder Selected = new StringBuilder();
            CustDataGrid cgrid = (CustDataGrid)Page.FindControl("gv_list");
            if (cgrid != null)
            {
                foreach (DataGridItem item in grid.Items)
                {
                    CheckBox cb = (CheckBox)item.Cells[0].FindControl("CB_Select");
                    if (cb != null)
                    {
                        if (cb.Checked)
                        {
                            Selected.Append(((HiddenField)item.Cells[0].FindControl("HD_Key")).Value + ",");
                        }
                    }
                }
            }
            return Selected.ToString().TrimEnd(',');
        }


        /// <summary>
        /// CustDataGrid数据列表 获取选中的checkbox 
        /// </summary>
        /// <param name="keyisVarchar"> 隐藏列在数据库是否是varchar 类型</param>
        /// <returns></returns>
        public string GetSelectStr(CustDataGrid grid, bool keyisVarchar = false)
        {
            StringBuilder Selected = new StringBuilder();
            CustDataGrid cgrid = (CustDataGrid)Page.FindControl("gv_list");
            if (cgrid != null)
            {
                foreach (DataGridItem item in grid.Items)
                {
                    CheckBox cb = (CheckBox)item.Cells[0].FindControl("CB_Select");
                    if (cb != null)
                    {
                        if (cb.Checked && keyisVarchar)
                        {

                            Selected.Append("'" + ((HiddenField)item.Cells[0].FindControl("HD_Key")).Value + "',");
                        }
                    }
                }
            }
            return Selected.ToString().TrimEnd(',');
        }


        /// <summary>
        /// CustDataGrid 符合条件的列表
        /// </summary>
        /// <param name="grid">grid</param>
        /// <param name="chk">选取 选中的记录 true 还是未选中的记录 false </param>
        /// <param name="rows">如果多选,请输入条数</param>
        /// <returns></returns>
        public string GetnotSelectStr(CustDataGrid grid, bool chk = true, int rows = 1)
        {
            StringBuilder Selected = new StringBuilder();
            CustDataGrid cgrid = (CustDataGrid)Page.FindControl("gv_list");
            if (cgrid != null)
            {
                int count = 0;
                foreach (DataGridItem item in grid.Items)
                {
                    CheckBox cb = (CheckBox)item.Cells[0].FindControl("CB_Select");
                    if (cb != null)
                    {
                        if (chk == cb.Checked)
                        {
                            if (count > rows)
                            {
                                break;
                            }
                            count++;
                            Selected.Append(((HiddenField)item.Cells[0].FindControl("HD_Key")).Value + ",");
                        }
                    }
                }
            }
            return Selected.ToString().TrimEnd(',');
        }


        /// <summary>
        /// 获取拼接IN
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public string GetWhereStr(CustDataGrid grid)
        {
            string strwhere = GetSelectStr(grid);
            if (strwhere.Length > 0)
            {
                return "'" + strwhere.Replace(",", "','") + "'";
            }
            return strwhere;
        }

        /// <summary>
        /// 获取拼接IN
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public string GetWhereStrs(string strwhere, string mark)
        {
            if (strwhere.Length > 0)
            {
                return "'" + strwhere.Replace(mark, "'" + mark + "'") + "'";
            }
            return strwhere;
        }

        public List<DataGridItem> GetSelectRow(CustDataGrid grid)
        {
            List<DataGridItem> lst = new List<DataGridItem>();
            CustDataGrid cgrid = (CustDataGrid)Page.FindControl("gv_list");
            if (cgrid != null)
            {
                foreach (DataGridItem item in grid.Items)
                {
                    CheckBox cb = (CheckBox)item.Cells[0].FindControl("CB_Select");
                    if (cb != null)
                    {
                        if (cb.Checked)
                        {
                            lst.Add(item);
                        }
                    }
                }
            }
            return lst;
        }

        /// <summary>
        /// DataGrid排序事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gv_list_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.Trim().Length > 0)
            {
                HtmlInputHidden hdSortExpression = (HtmlInputHidden)Page.FindControl("HidSortExpression");
                HtmlInputHidden hdOrder = (HtmlInputHidden)Page.FindControl("HidOrder");
                if (hdSortExpression != null && hdOrder != null)
                {

                    if (hdSortExpression.Value != e.SortExpression)
                    {
                        hdSortExpression.Value = e.SortExpression;
                        hdOrder.Value = "DESC";
                    }
                    else
                    {
                        if (hdOrder.Value.ToUpper() == "ASC" || hdOrder.Value.ToUpper().Length == 0)
                        {
                            hdOrder.Value = "DESC";
                        }
                        else
                        {
                            hdOrder.Value = "ASC";
                        }
                    }
                    BindGridView();
                }
            }
        }

        #region 分页事件
        protected virtual void BindGridView()
        {
            //SELECT Stock.*,A.matname,A.securityamount FROM  Stock ,(SELECT matcode,matname,securityamount FROM dbo.StockMaterial WHERE mattypecode IN (SELECT mcode FROM  dbo.StockMateType WHERE mcode='02')) AS A WHERE A.matcode=dbo.Stock.matcode
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        protected void anp_top_PageChanging(object src, PageChangingEventArgs e)
        {
            Control controlGrid = Page.FindControl("anp_top");
            if (controlGrid != null)
            {
                AspNetPager Grid = (AspNetPager)controlGrid;
                Grid.CurrentPageIndex = e.NewPageIndex;
                BindGridView();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        protected void gpager_view_PageChanged(object src, PageChangedEventArgs e)
        {
            Control controlGrid = Page.FindControl("gpager_view");
            if (controlGrid != null)
            {
                GridPager Grid = (GridPager)controlGrid;
                Grid.CurrentPageIndex = e.NewPageIndex;
                BindGridView();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        protected void gpager_view_PageSizeChanged(object src, PageSizeChangedEventArgs e)
        {
            Control controlGrid = Page.FindControl("gpager_view");
            if (controlGrid != null)
            {
                GridPager Grid = (GridPager)controlGrid;
                Grid.PageSize = e.NewPageSize;
                BindGridView();
            }
        }
        #endregion

        public bool ExportReportToExcel(System.Data.DataTable dt, string fileName, Dictionary<string, object> filter)
        {

            bool succeed = false;
            if (dt != null)
            {
                try
                {
                    //Aspose.Cells.License li = new Aspose.Cells.License();
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                    Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];

                    cellSheet.Name = dt.TableName;

                    int rowIndex = 0;
                    int colIndex = 0;
                    int colCount = dt.Columns.Count;
                    int rowCount = dt.Rows.Count;
                    if (colCount >= filter.Count)
                    {
                        //列名的处理
                        foreach (KeyValuePair<string, object> de0 in filter)
                        {
                            cellSheet.Cells[rowIndex, colIndex].PutValue(de0.Key.ToString());//dt.Columns[i].ColumnName
                            Aspose.Cells.Style style = new Aspose.Cells.Style();
                            style.Font.IsBold = true;
                            style.Font.Name = "宋体";
                            cellSheet.Cells[rowIndex, colIndex].SetStyle(style);
                            colIndex++;
                        }

                        Aspose.Cells.Style style2 = new Aspose.Cells.Style();
                        style2.Font.Name = "Arial";
                        style2.Font.Size = 10;
                        Aspose.Cells.StyleFlag styleFlag = new Aspose.Cells.StyleFlag();
                        cellSheet.Cells.ApplyStyle(style2, styleFlag);

                        rowIndex++;

                        for (int i = 0; i < rowCount; i++)
                        {
                            colIndex = 0;

                            foreach (KeyValuePair<string, object> de1 in filter)
                            {
                                cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Rows[i][de1.Value.ToString()].ToString());
                                colIndex++;
                            }
                            rowIndex++;
                        }
                        cellSheet.AutoFitColumns();

                        //path = System.IO.Path.GetFullPath(fileName);
                        //workbook.Save(path);

                        cellSheet.AutoFitColumns();//让各列自适应宽度，这个很有用。        
                        //Response.Clear();
                        //Response.Charset = "UTF8";
                        //Response.ContentEncoding = System.Text.Encoding.UTF8;
                        //Response.HeaderEncoding = System.Text.Encoding.UTF8;
                        //Response.ContentType = "application/ms-excel";

                        HttpResponse response = Page.Response;
                        response.Clear();
                        response.ContentType = "application/octet-stream";
                        //使用UTF-8对文件名进行编码
                        fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);

                        response.ContentType = "application/ms-excel;";
                        //workbook.Save(fileName, Aspose.Cells.FileFormatType.Default, Aspose.Cells.SaveType.OpenInExcel, response);
                        workbook.Save(fileName, FileFormatType.Xlsx);
                        succeed = true;

                    }

                }
                catch (Exception ex)
                {
                    succeed = false;
                }
            }

            return succeed;
        }
    }
}