using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class tl_loginlogList : Common.ListPage
    {
        blltl_loginlog bll = new blltl_loginlog();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                BindGridView();
            }
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
                            logentity.module = ErrMessage.GetMessageInfoByCode("tl_loginlog_Menu").Body;
                            logentity.pageurl = "tl_loginlogEdit.aspx";
                            logentity.otype = SystemEnum.LogOperateType.Delete;
                            logentity.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());
                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_005").Body;
                            }
                            string[] arrSel = Selected.Split(',');
                            for (int i = 0; i < arrSel.Length; i++)
                            {
                                logentity.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("tl_loginlog_961").Body, LoginedUser.UserInfo.cname, Selected);
                                bll.Delete("0", "0", Selected, logentity);
                            }
                            sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_001").Body;
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_005").Body;
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
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请选择要操作记录！";
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
                        string fileName = string.Format(ErrMessage.GetMessageInfoByCode("tl_loginlog_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        string strColumnName = ErrMessage.GetMessageInfoByCode("tl_loginlog_Export").Body;
                        string ColumnCode = "cname,cusername,logcontent,ctime";
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

            string strcname = Helper.ReplaceString(txt_cname.Value);
            if (strcname.Length > 0)
            {
                Where.Append(" and cname like '%" + strcname + "%'");
            }
            string ctime = Helper.ReplaceString(txt_ctime.Value);
            string ctimeend = Helper.ReplaceString(this.txt_ctimeend.Value);
            if (ctime.Length > 0 && ctimeend.Length > 0)
            {
                Where.AppendFormat(" and ctime>= '{0}' and ctime <'{1}' ", ctime, ctimeend);
            }

            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }
    }
}