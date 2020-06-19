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
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
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
                order = " a.ctime desc";
            }

            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recount, out pagenums);

            if (dt != null)
            {
                dt.Columns.Add("scopename", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["scopename"] = Helper.GetEnumNameByValue(typeof(SystemEnum.AccScope), dt.Rows[i]["scope"].ToString());
                }
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
                            logentity.module = ErrMessage.GetMessageInfoByCode("admins_Menu").Body;
                            logentity.pageurl = "adminsedit.aspx";
                            logentity.otype = SystemEnum.LogOperateType.Delete;
                            logentity.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());

                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_005").Body;
                                return;
                            }
                            string[] arrSel = Selected.Split(',');
                            for (int i = 0; i < arrSel.Length; i++)
                            {
                                logentity.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("admins_961").Body, LoginedUser.UserInfo.cname, Selected);
                                bll.Delete("0", "0", Selected, logentity);
                            }
                            sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_001").Body;
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetWhereStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_005").Body;
                            return;
                        }
                        else
                        {
                            dt = bll.UpdateStatus("", "0", Selected, SystemEnum.Status.Valid.ToString("D"));
                            if (ShowResult(dt, sp_showmes))
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
                            sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_005").Body;
                        }
                        else
                        {
                            dt = bll.UpdateStatus("", "0", Selected, SystemEnum.Status.Invalid.ToString("D"));
                            if (ShowResult(dt, sp_showmes))
                            {
                                BindGridView();
                            }
                        }
                        break;
                    //导出事件代码
                    case "export":
                        int recount;
                        int pagenums;
                        string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                        dt = bll.GetPagingListInfo("", "0", 50000, 1, HidWhere.Value, order, out recount, out pagenums);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dt.Columns.Add("statusname", typeof(string));
                            dt.Columns.Add("scopename", typeof(string));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                                dt.Rows[i]["scopename"] = Helper.GetEnumNameByValue(typeof(SystemEnum.AccScope), dt.Rows[i]["scope"].ToString());
                            }
                        }
                        string fileName = string.Format(ErrMessage.GetMessageInfoByCode("admins_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        //字段1,字段2
                        string strColumnName = ErrMessage.GetMessageInfoByCode("admins_Export").Body;
                        string ColumnCode = "uname,empmob,empcode,scopename,storename,empcodename,rolename,statusname";
                        ExcelsHelp.ExportExcelFileB(dt, fileName, strColumnName.Split(','), ColumnCode.Split(','));
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
            string struname = Helper.ReplaceString(txt_uname.Value);
            if (struname.Length > 0)
            {
                Where.Append(" and a.uname like '%" + struname + "%' ");
            }

            string strrealname = Helper.ReplaceString(txt_realname.Value);
            if (strrealname.Length > 0)
            {
                Where.Append(" and dbo.fnGetEmployeeCname(empcode) like '%" + strrealname + "%' ");
            }
            string umobile = Helper.ReplaceString(txt_umobile.Value);
            if (umobile.Length > 0)
            {
                Where.Append(" and umobile like '%" + umobile + "%' ");
            }
            string strempcode = Helper.ReplaceString(txt_empcode.Value);
            if (strempcode.Length > 0)
            {
                Where.Append(" and empcode like '%" + strempcode + "%' ");
            }
            string status = Helper.ReplaceString(ddl_status.SelectedValue);
            if (status.Length > 0)
            {
                Where.Append(" and a.status='" + status + "'");
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