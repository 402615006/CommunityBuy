using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.manage
{
    public partial class adminslist : ListPage
    {
        bllAdmins bll = new bllAdmins();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = "列表";
            if (!IsPostBack)
            {
                BindRole();
                this.ddl_status.SelectedValue = "1";
                GotoSearch();
            }
        }

        public void BindRole()
        {
            DataTable dt = new bllPaging().GetDataTableInfoBySQL("select roleid,cname from roles where status='1';");
            this.ddl_rolename.DataSource = dt;
            this.ddl_rolename.DataTextField = "cname";
            this.ddl_rolename.DataValueField = "roleid";
            this.ddl_rolename.DataBind();

            this.ddl_rolename.Items.Insert(0, new ListItem("全部", ""));
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected override void BindGridView()
        {
            int recount;
            int pagenums;
            string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
            if (HidSortExpression.Value == "")
            {
                order = " ctime desc";
            }

            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recount, out pagenums);

            if (dt != null)
            {
                gv_list.DataSource = dt;
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
                        {
                            //日志信息
                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = "请选择操作项";
                                return;
                            }

                                bll.Delete("0", "0", Selected);
                            sp_showmes.InnerText = "操作成功";
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetWhereStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请选择操作项";
                            return;
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
                        Selected = GetWhereStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请选择操作项";
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
                    ////导出事件代码
                    //case "export":
                    //    int recount;
                    //    int pagenums;
                    //    string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                    //    dt = bll.GetPagingListInfo("", "0", 50000, 1, HidWhere.Value, order, out recount, out pagenums);

                    //    string fileName = string.Format(ErrMessage.GetMessageInfoByCode("admins_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                    //    //字段1,字段2
                    //    string strColumnName = ErrMessage.GetMessageInfoByCode("admins_Export").Body;
                    //    string ColumnCode = "uname,empmob,empcode,scopename,storename,empcodename,rolename,statusname";
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
            string struname =txt_uname.Value;
            if (struname.Length > 0)
            {
                Where.Append(" and uname like '%" + struname + "%' ");
            }
            string strrealname =txt_realname.Value;

            string umobile =txt_umobile.Value;
            if (umobile.Length > 0)
            {
                Where.Append(" and umobile like '%" + umobile + "%' ");
            }
            string status =ddl_status.SelectedValue;
            if (status.Length > 0)
            {
                Where.Append(" and status='" + status + "'");
            }
            string rolename = ddl_rolename.SelectedValue;
            if (rolename.Length > 0)
            {
                Where.Append(" and CHARINDEX('," + rolename + ",',','+dbo.f_GetRoleID(userid)+',')>0 ");
            }
            HidWhere.Value = Where.ToString();
            this.anp_top.CurrentPageIndex = 1;
        }
    }
}