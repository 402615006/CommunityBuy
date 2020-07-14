using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class ts_syssetList : Common.ListPage
    {
        bllts_sysset bll = new bllts_sysset();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageTitle.Operate = "列表";
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
                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = "请至少选择一项";
                            }
                            string[] arrSel = Selected.Split(',');
                            for (int i = 0; i < arrSel.Length; i++)
                            {
                                bll.Delete("0", "0", Selected);
                            }
                            sp_showmes.InnerText = bll.oResult.Msg;
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = sp_showmes.InnerText = "请至少选择一项";
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
                            sp_showmes.InnerText = "请选择要操作记录！";
                        }
                        else
                        {
                            bll.UpdateStatus("", "0", Selected, "0");
                            if (ShowResult(bll.oResult.Code,bll.oResult.Msg, sp_showmes))
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
                    //    string fileName = string.Format(ErrMessage.GetMessageInfoByCode("ts_sysset_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                    //    string strColumnName = ErrMessage.GetMessageInfoByCode("admins_Export").Body;
                    //    string ColumnCode = "列名1,列名2";
                    //    ExcelsHelp.ExportExcelFileB(dt, System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8), strColumnName.Split(','), ColumnCode.Split(','));
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

            string strkey =txt_key.Value;
            if (strkey.Length > 0)
            {
                Where.AppendFormat(" and [key] like  '%{0}%'", strkey);

            }
            string strval =txt_val.Value;
            if (strval.Length > 0)
            {

                Where.AppendFormat(" and [val] like  '%{0}%'", strval);
            }

            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }
    }
}