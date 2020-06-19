using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.BackWeb.UserControls;

namespace CommunityBuy.BackWeb
{
    public partial class DishesFeedRateList : Common.ListPage
    {
        bllDishesMate bll = new bllDishesMate();
        bool status = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                System.Web.UI.WebControls.ListItem itemDefault = new System.Web.UI.WebControls.ListItem();
                itemDefault.Text = "--请选中--";
                itemDefault.Value = "-1";
                itemDefault.Selected = true;
                int recount;
                int pagenums;
                //部门下拉框
                string strWhere = "where status='1'";
                if (LoginedUser.UserInfo.rolstocode.Length > 0)
                {
                    strWhere += " and stocode in('" + LoginedUser.UserInfo.rolstocode.Replace(",", "','") + "')";
                }
                DataTable dtStore = new bllStore().GetPagingListInfo("0", "0", int.MaxValue, 1, strWhere, "cname desc", out recount, out pagenums);
                ddl_stocode.DataTextField = "cname";
                ddl_stocode.DataValueField = "stocode";
                ddl_stocode.DataSource = dtStore;
                ddl_stocode.DataBind();
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    ddl_stocode.SelectedIndex = 0;
                }
                //GotoSearch();
                //BindGridView();
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected override void BindGridView()
        {
            status = true;
            int recount;
            int pagenums;
            string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
            if (HidSortExpression.Value == "")
            {
                order = " t1.ctime desc";
            }
            DataTable dt = bll.GetPagingListInfo5("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recount, out pagenums);

            if (dt != null && dt.Rows.Count > 0)
            {

                string where = "where 1=1";
                string discodes = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    discodes += "'" + dr["discode"].ToString() + "',";
                }
                where += " and A.discode in (" + discodes.TrimEnd(',') + ")";
                if (ddl_stocode.SelectedValue != "-1")
                {
                    where += " and A.stocode='" + ddl_stocode.SelectedValue + "'";
                }
                where += " and A.discode<>'' and B.matname<>''";
                DataTable dt1 = new blldishes().getDataTableBySql("SELECT *,'' as jlvs,[dbo].[fnGetDisheDisidByDiscode](A.discode,A.stocode) as disid,[dbo].[fnGetDisheWareHouseByDiscode](A.discode,A.stocode) as warname,[dbo].[fnGetDisheNameByDiscode](A.discode,A.stocode) as disname,[dbo].[fnGetDisheUnitByDiscode](A.discode,A.stocode) as disnuitname, unitname=dbo.f_Get_DictsName(unitcode),B.matname,B.smxprice,matclassname=dbo.fnGetStockMateClassLongNameByMatcode(A.matcode) FROM DishesMate A left join catering_stock.dbo.StockMaterial B on A.matcode=B.matcode and A.buscode=B.buscode " + where);
                foreach (DataRow dr1 in dt.Rows)
                {


                    DataRow dr2 = dt1.NewRow();
                    if (dr1["disname"].ToString() != "")
                    {
                        if (dt1.Select("discode='" + dr1["discode"] + "'").Length == 0)
                        {
                            //不存在配料信息
                            dr2["warname"] = dr1["warname"];
                            dr2["discode"] = dr1["discode"];
                            dr2["disname"] = dr1["disname"];
                            dr2["disnuitname"] = dr1["unit"];
                            dr2["disid"] = dr1["lsid"];
                        }
                        else
                        {
                            dr2["disid"] = dr1["lsid"];
                        }
                        dr2["stocode"] = dr1["stocode"];
                        dt1.Rows.Add(dr2);

                    }
                }
                foreach (DataRow dr3 in dt1.Rows)
                {
                    if (!string.IsNullOrEmpty(dr3["jlv"].ToString()))
                    {
                        decimal jlv = decimal.Parse(dr3["jlv"].ToString());
                        if (jlv == 100)
                        {
                            dr3["jlvs"] = decimal.Parse(dr3["jlv"].ToString()).ToString("f2") + "%";
                        }
                        else
                        {
                            dr3["jlvs"] = (decimal.Parse(dr3["jlv"].ToString()) * 100).ToString("f2") + "%";
                        }

                    }
                }
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    gv_list.DataSource = dt1;
                    gv_list.DataBind();
                    anp_top.RecordCount = recount;
                }
                else
                {
                    gv_list.DataSource = null;
                    gv_list.DataBind();
                    anp_top.RecordCount = recount;
                }
            }
            else
            {
                gv_list.DataSource = null;
                gv_list.DataBind();
                anp_top.RecordCount = recount;
            }
        }

        /// <summary>
        /// ToolBar所有按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToolBar1_Click(object sender, ToolBarEventArgs e)
        {
            DataTable dt = new DataTable();
            if (e != null)
            {
                string Selected = string.Empty;
                switch (((ToolBarEventArgs)(e)).btntype.ToLower())
                {
                    //搜索事件代码
                    case "search":
                        GotoSearch();
                        break;
                    //刷新事件代码
                    case "refresh":
                        BindGridView();
                        break;
                    case "delete":

                        break;
                    //有效
                    case "active":

                        break;
                    //无效
                    case "invalid":

                        break;
                    //导出事件代码
                    case "export":
                        int recount;
                        int pagenums;
                        string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                        dt = bll.GetPagingListInfo5("", "0", 50000, 1, HidWhere.Value, order, out recount, out pagenums);
                        string where = "where 1=1";
                        string discodes = string.Empty;
                        foreach (DataRow dr in dt.Rows)
                        {
                            discodes += "'" + dr["discode"].ToString() + "',";
                        }
                        where += " and A.discode in (" + discodes.TrimEnd(',') + ")";
                        if (ddl_stocode.SelectedValue != "-1")
                        {
                            where += " and A.stocode='" + ddl_stocode.SelectedValue + "'";
                        }
                        where += " order by [dbo].[fnGetDisheNameByDiscode](A.discode,A.stocode)";
                        DataTable dt1 = new blldishes().getDataTableBySql("SELECT *,[dbo].[fnGetDisheDisidByDiscode](A.discode,A.stocode) as disid,[dbo].[fnGetDisheWareHouseByDiscode](A.discode,A.stocode) as warname,[dbo].[fnGetDisheNameByDiscode](A.discode,A.stocode) as disname,[dbo].[fnGetDisheUnitByDiscode](A.discode,A.stocode) as disnuitname, unitname=dbo.f_Get_DictsName(unitcode),B.matname,B.smxprice,matclassname=dbo.fnGetStockMateClassLongNameByMatcode(A.matcode) FROM DishesMate A left join catering_stock.dbo.StockMaterial B on A.matcode=B.matcode and A.buscode=B.buscode " + where);
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            DataRow dr2 = dt1.NewRow();
                            if (dr1["disname"].ToString() != "")
                            {
                                if (dt1.Select("discode='" + dr1["discode"] + "'").Length == 0)
                                {
                                    //不存在配料信息
                                    dr2["warname"] = dr1["warname"];
                                    dr2["discode"] = dr1["discode"];
                                    dr2["disname"] = dr1["disname"];
                                    dr2["disnuitname"] = dr1["unit"];
                                    dr2["disid"] = dr1["lsid"];
                                    dt1.Rows.Add(dr2);
                                }
                                else
                                {
                                    dr2["disid"] = dr1["lsid"];
                                }
                            }
                        }
                        string html = string.Empty;
                        html += "<table><tbody>";
                        html += "<tr>";
                        html += "<th>菜品名称</th>";
                        html += "<th>单位</th>";
                        html += "<th>仓库</th>";
                        html += "<th>物料名称</th>";
                        html += "<th>规格</th>";
                        html += "<th>基本单位</th>";
                        html += "<th>用量</th>";
                        html += "<th>净菜率</th>";
                        html += "<th>用量</th>";
                        html += "</tr>";
                        string discodes1 = string.Empty;
                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {

                                string discode = dt1.Rows[i]["discode"].ToString();
                                if (discodes1.Contains(discode + ","))
                                {
                                    continue;
                                }

                                DataRow[] rows = dt1.Select(" discode='" + discode + "'");
                                if (rows.Length > 1)
                                {
                                    for (int j = 0; j < rows.Length; j++)
                                    {
                                        html += "<tr>";
                                        if (j == 0)
                                        {
                                            html += "<td rowspan='" + rows.Length + "'>" + rows[j]["disname"].ToString() + "</td>";
                                        }

                                        html += "<td>" + rows[j]["disnuitname"].ToString() + "</td>";
                                        html += "<td>" + rows[j]["warname"].ToString() + "</td>";
                                        html += "<td>" + rows[j]["matname"].ToString() + "</td>";
                                        html += "<td>" + rows[j]["spec"].ToString() + "</td>";
                                        html += "<td>" + rows[j]["unitname"].ToString() + "</td>";
                                        html += "<td>" + rows[j]["jlnum"].ToString() + "</td>";
                                        html += "<td>" + rows[j]["jlv"].ToString() + "</td>";
                                        html += "<td>" + rows[j]["mlnum"].ToString() + "</td>";
                                        html += "</tr>";
                                    }
                                    i = i + (rows.Length - 1);
                                }
                                else
                                {
                                    if (rows.Length > 0)
                                    {
                                        html += "<tr>";
                                        html += "<td>" + rows[0]["disname"].ToString() + "</td>";
                                        html += "<td>" + rows[0]["disnuitname"].ToString() + "</td>";
                                        html += "<td>" + rows[0]["warname"].ToString() + "</td>";
                                        html += "<td>" + rows[0]["matname"].ToString() + "</td>";
                                        html += "<td>" + rows[0]["spec"].ToString() + "</td>";
                                        html += "<td>" + rows[0]["unitname"].ToString() + "</td>";
                                        html += "<td>" + rows[0]["jlnum"].ToString() + "</td>";
                                        html += "<td>" + rows[0]["jlv"].ToString() + "</td>";
                                        html += "<td>" + rows[0]["mlnum"].ToString() + "</td>";
                                        html += "</tr>";
                                    }
                                }
                                discodes1 += discode + ",";
                            }
                        }
                        html += "</tbody></table>";
                        Response.ContentType = "application/force-download";
                        Response.AddHeader("content-disposition",
                            "attachment; filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                        Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                        Response.Write("<head>");
                        Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                        Response.Write("<!--[if gte mso 9]><xml>");
                        Response.Write("<x:ExcelWorkbook>");
                        Response.Write("<x:ExcelWorksheets>");
                        Response.Write("<x:ExcelWorksheet>");
                        Response.Write("<x:Name>Sheet1</x:Name>");
                        Response.Write("<x:WorksheetOptions>");
                        Response.Write("<x:Print>");
                        Response.Write("<x:ValidPrinterInfo/>");
                        Response.Write("</x:Print>");
                        Response.Write("</x:WorksheetOptions>");
                        Response.Write("</x:ExcelWorksheet>");
                        Response.Write("</x:ExcelWorksheets>");
                        Response.Write("</x:ExcelWorkbook>");
                        Response.Write("</xml>");
                        Response.Write("<![endif]--> ");
                        Response.Write(html);//HTML
                        Response.Flush();
                        Response.End();




                        string fileName = string.Format(ErrMessage.GetMessageInfoByCode("DishesMate_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        string strColumnName = "菜品名称,单位,仓库,物料名称,规格,基本单位,用量,净菜率,用量";
                        string ColumnCode = "disname,disnuitname,warname,matname,spec,unitname,jlnum,jlv,mlnum";
                        ExcelsHelp.ExportExcelFileB(dt, System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8), strColumnName.Split(','), ColumnCode.Split(','));
                        break;
                }
            }
        }

        /// <summary>
        /// 搜索按钮拼接Where条件
        /// </summary>
        public void GotoSearch()
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" where 1=1 ");
            //拼接Where条件
            if (ddl_stocode.SelectedValue != "-1")
            {
                Where.Append(" and t1.stocode='" + ddl_stocode.SelectedValue + "'");
            }
            if (!string.IsNullOrEmpty(ddl_Menu_list.SelectedValue) && ddl_Menu_list.SelectedValue != "-1")
            {
                Where.Append(" and t1.warcode='" + ddl_Menu_list.SelectedValue + "'");
            }
            if (!string.IsNullOrEmpty(txt_disname.Value))
            {
                Where.Append(" and t1.disname like '%" + txt_disname.Value + "%'");
            }
            Where.Append(" and t1.discode<>''");
            //Where.Append(" and (select count(0) from [dbo].[DishesMate] t where t.discode=t1.discode and t.stocode=t1.stocode)>0");
            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }

        protected void ddl_stocode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取仓库
            System.Web.UI.WebControls.ListItem itemDefault = new System.Web.UI.WebControls.ListItem();
            itemDefault.Text = "全部";
            itemDefault.Value = "-1";
            itemDefault.Selected = true;
            int recount;
            int pagenums;
            //部门下拉框
            ddl_Menu_list.DataSource = null;
            DataTable dtHouse = new bllStockWareHouse().GetPagingListInfo("0", "0", int.MaxValue, 1, "where buscode='" + Helper.GetAppSettings("BusCode") + "' and stocode='" + ddl_stocode.SelectedValue + "'", "warname desc", out recount, out pagenums);
            ddl_Menu_list.DataTextField = "warname";
            ddl_Menu_list.DataValueField = "warcode";
            ddl_Menu_list.DataSource = dtHouse;
            ddl_Menu_list.DataBind();
            ddl_Menu_list.Items.Add(itemDefault);
        }

        protected void gv_list_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" <TR align=\"center\" style=\"BACKGROUND-COLOR:#EEEEEE;COLOR:white;FONT-SIZE:18px\"> ");
                sb.Append(" <TD colSpan=\"3\"><font size=\"3\">菜品信息</font></TD> ");
                sb.Append(" <TD colSpan=\"3\"><font size=\"3\">物料信息</font></TD> ");
                sb.Append(" <TD colSpan=\"2\"><font size=\"3\">净料</font></TD> ");
                sb.Append(" <TD><font size=\"3\">毛料</font></TD> ");
                sb.Append(" </TR> ");
                sb.Append(" <TR align=\"center\" style=\"BACKGROUND-COLOR:#EEEEEE;COLOR:white; FONT-SIZE:18px\"> ");
                sb.Append(" <TD width=\"180px;\"><font size=\"3\">菜品名称</font></TD> ");
                sb.Append(" <TD width=\"180px;\"><font size=\"3\">单位</font></TD> ");
                sb.Append(" <TD width=\"180px;\"><font size=\"3\">仓库</font></TD> ");
                sb.Append(" <TD width=\"180px;\"><font size=\"3\">物料名称</font></TD> ");
                sb.Append(" <TD width=\"180px;\"><font size=\"3\">规格</font></TD> ");
                sb.Append(" <TD width=\"180px;\"><font size=\"3\">基本单位</font></TD> ");
                sb.Append(" <TD width=\"180px;\"><font size=\"3\">用量</font></TD> ");
                sb.Append(" <TD width=\"180px;\"><font size=\"3\">净料率</font></TD> ");
                sb.Append(" <TD width=\"180px;\"><font size=\"3\">用量</font></TD> ");
                sb.Append(" </TR> ");

                gv_list.Caption = " </caption> " + sb.ToString() + " <caption> ";
                gv_list.ShowHeader = false;
            }
        }

        /// <summary>
        /// 合并第一列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_list_PreRender(object sender, EventArgs e)
        {
            if (status)
            {
                if (this.gv_list.Items.Count <= 1)
                {
                    return;
                }
                int col = 1;
                TableCell oldtc = this.gv_list.Items[0].Cells[col];
                TableCell oldtc1 = this.gv_list.Items[0].Cells[0];
                for (int i = 1; i < this.gv_list.Items.Count; i++)
                {
                    oldtc.Visible = false;
                    TableCell tc = this.gv_list.Items[i].Cells[col];
                    TableCell tc1 = this.gv_list.Items[i].Cells[0];
                    if (tc.Text == "" || tc.Text == "&nbsp;")
                    {
                        this.gv_list.Items[i].Visible = false;
                        continue;
                    }
                    else if (tc.Text == oldtc.Text)
                    {
                        tc.Visible = false;
                        tc1.Visible = false;
                        oldtc.Visible = false;
                        if (oldtc1.RowSpan == 0)
                        {
                            oldtc1.RowSpan = 1;
                        }
                        oldtc1.RowSpan = oldtc1.RowSpan + 1;
                        oldtc1.VerticalAlign = VerticalAlign.Middle;
                        oldtc.Visible = false;
                    }
                    else
                    {
                        oldtc1 = this.gv_list.Items[i].Cells[0];
                        oldtc = tc;
                        if (i == this.gv_list.Items.Count - 1)
                        {
                            oldtc.Visible = false;
                        }

                    }
                }
                status = false;
            }
        }

    }
}