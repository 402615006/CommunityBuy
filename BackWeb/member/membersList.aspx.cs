using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using Wuqi.Webdiyer;
namespace CommunityBuy.BackWeb
{
    public partial class membersList : Common.ListPage
    {
        bllmembers bll = new bllmembers();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = "列表页";
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
            DataTable dt = bll.GetPagingInfoByList("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recount, out pagenums);

            if (dt != null)
            {
                dt.Columns.Add("statusname", typeof(string));
                dt.Columns.Add("sexname", typeof(string));
                dt.Columns.Add("bigcustomername", typeof(string));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["statusname"] = dt.Rows[i]["status"].ToString() == "1" ? "有效" : "无效";
                    dt.Rows[i]["sexname"] = dt.Rows[i]["sex"].ToString()== "1" ? "男" : "女";
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
                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = "请至少选择一项";
                                return;
                            }
                            string[] coldelist = Selected.Split(',');
                            if (coldelist.Length > 1)
                            {
                                sp_showmes.InnerText = "一次只能删除一条数据";
                                return;
                            }

                            bll.Delete("0", "0", coldelist[0]);
                            sp_showmes.InnerText = bll.oResult.Msg;
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetSelectStr(gv_list, true);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请至少选择一项";
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
                        Selected = GetSelectStr(gv_list, true);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请选择要操作记录！";
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
                    case "memeditphone":
                         Selected = GetSelectStr(gv_list, true);
                         if (Selected.Length == 0)
                         {
                             sp_showmes.InnerText = "请选择要操作记录！";
                         }
                         else
                         {
                             var dto = bll.GetEntitySigInfo(string.Format("memcode ={0}", Selected));
                             Script(this.Page, "ShowOpenpage('修改手机号', '/memberCard/membersPhoneEdit.aspx?id=" + dto.memcode + "', '100%', '100%', true, true);");
                         }
                        break;
                    //导出事件代码
                    case "export":
                        int recount;
                        int pagenums;
                        string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                        dt = bll.GetPagingInfoByList("", "0", 50000, 1, HidWhere.Value, order, out recount, out pagenums);
                        string fileName = "会员信息.xls";
                        string strColumnName = "编号,姓名,性别,电话,证件类型,证件号码,状态";
                        string ColumnCode = "memcode,cname,sexname,mobile,idtypeName,IDNO,statusname";
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
            Where.AppendFormat(" where  1=1 ");
            //拼接Where条件
            string memcode =txt_memcode.Value;
            if (memcode.Length > 0)
            {
                Where.Append(" and memcode =  '" + memcode + "'");
            }
            string strcname =txt_cname.Value;
            if (strcname.Length > 0)
            {
                Where.Append(" and cname = '" + strcname + "'");

            }
            string strmobile =txt_mobile.Value;
            if (strmobile.Length > 0)
            {
                Where.Append(" and mobile = '" + strmobile + "'");

            }
            string strIDNO =txt_IDNO.Value;
            if (strIDNO.Length > 0)
            {
                Where.Append(" and IDNO = '" + strIDNO + "'");
            }

            anp_top.CurrentPageIndex = 1;
        }
    }
}