using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.manage
{
    public partial class rolefunctionlist : ListPage
    {
        bllTB_Roles bll = new bllTB_Roles();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = "";
            if (!IsPostBack)
            {
                if (base.LoginedUser != null)
                {
                    BindGridView();
                }
            }
        }
        protected override void BindGridView()
        {
            int recnums = 0;
            int pagenums = 0;
            if (HidSortExpression.Value == "")
            {
                HidSortExpression.Value = "ctime";
                HidOrder.Value = "desc";
            }
            //根据查询条件获取数据列表
            string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recnums, out pagenums);
            if (dt != null)
            {
                gv_list.DataSource = dt;
                gv_list.DataBind();
                anp_top.RecordCount = recnums;
            }
        }

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
                        this.anp_top.CurrentPageIndex = 1;
                        GotoSearch();
                        break;
                    //刷新事件代码
                    case "refresh":
                        BindGridView();
                        break;
                    case "delete":
                        {
                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = "请选择一项操作";
                            }
                            else
                            {
                                string mes = string.Empty;
                                bll.Delete("","",Selected);
                                ShowResult(bll.oResult.Code, bll.oResult.Msg, sp_showmes);
                                anp_top.CurrentPageIndex = 1;
                            }
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请选择一项操作";
                        }
                        else
                        {
                            bll.UpdateStatus("", "0", Selected, "1");
                            if (ShowResult(bll.oResult.Code,bll.oResult.Msg, sp_showmes))
                            {
                                BindGridView();
                            }
                        }
                        break;
                    //无效
                    case "invalid":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请选择一项操作";
                        }
                        else
                        {
                            bll.UpdateStatus("", "0", Selected, "0");
                            if (ShowResult(bll.oResult.Code, bll.oResult.Msg, sp_showmes))
                            {
                                BindGridView();
                            }
                        }
                        break;
                    //导出事件代码
                    //case "export":
                    //    int recount;
                    //    int pagenums;
                    //    string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                    //    dt = bll.GetPagingListInfo("", "0", 50000, 1, HidWhere.Value, order, out recount, out pagenums);
                    //    if (dt != null)
                    //    {
                    //        dt.Columns.Add("rolestatus", typeof(string));
                    //        dt.Columns.Add("scopename", typeof(string));
                    //        for (int i = 0; i < dt.Rows.Count; i++)
                    //        {
                    //            dt.Rows[i]["rolestatus"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                    //            dt.Rows[i]["scopename"] = Helper.GetEnumNameByValue(typeof(SystemEnum.AccScope), dt.Rows[i]["scope"].ToString());
                    //        }
                    //    }
                    //    string fileName = string.Format(ErrMessage.GetMessageInfoByCode("roles_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                    //    string strColumnName = ErrMessage.GetMessageInfoByCode("roles_Export").Body;
                    //    string ColumnCode = "cname,rolestatus,descr,ctime";
                    //    ExcelsHelp.ExportExcelFileB(dt, fileName, strColumnName.Split(','), ColumnCode.Split(','));
                    //    break;
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
            string Name =username.Value;
            if (Name.Length > 0)
            {
                Where.Append(" and cname like'%" + Name + "%'");
            }
            string status = sel_status.SelectedValue;
            if (status.Length > 0)
            {
                Where.Append(" and status='" + status + "'");
            }
            HidWhere.Value = Where.ToString();
            BindGridView();
        }
    }
}

