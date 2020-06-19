using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class MemCardLists : Common.ListPage
    {
        bllMemCard bll = new bllMemCard();
        private string mobilNum = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    mobilNum = Request["id"];
                    BindGridView();
                }
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
            //todo 
            string sql = HidWhere.Value != "" ? HidWhere.Value + " amd memcode IN (SELECT memcode FROM dbo.members WHERE mobile='" + mobilNum + "') and status != '0'" + mobilNum + "'" : " WHERE memcode IN (SELECT memcode FROM dbo.members WHERE mobile='" + mobilNum + "') and status != '0'";
            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, sql, order, out recount, out pagenums);
            if (dt != null)
            {
                dt.Columns.Add("statusname", typeof(string));
                dt.Columns.Add("affiliatedname", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.MemCardStatus), dt.Rows[i]["status"].ToString());
                    dt.Rows[i]["affiliatedname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.AffiliatedType), dt.Rows[i]["affiliated"].ToString());
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
                            logentity.module = ErrMessage.GetMessageInfoByCode("MemCard_Menu").Body;
                            logentity.pageurl = "MemCardEdit.aspx";
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
                                logentity.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("MemCard_961").Body, LoginedUser.UserInfo.cname, Selected);
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
                        string fileName = string.Format(ErrMessage.GetMessageInfoByCode("MemCard_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        string strColumnName = ErrMessage.GetMessageInfoByCode("admins_Export").Body;
                        string ColumnCode = "列名1,列名2";
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
            Where.Append(GetAuthoritywhere("stocode"));


            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }
    }
}