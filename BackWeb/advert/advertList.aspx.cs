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
    public partial class advertList : Common.ListPage
    {
        bllActivityAd bll = new bllActivityAd();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = "广告列表";
            if (!IsPostBack)
            {
                BindBusInfo();
                GotoSearch();
                BindGridView();
            }
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        public void BindBusInfo()
        {
            ListItem itemDefault = new ListItem();
            itemDefault.Text = "--请选择--";
            itemDefault.Value = "-1";
            itemDefault.Selected = true;

            ListItem item1= new ListItem();
            item1.Text = "首页轮播";
            item1.Value = "1";

            ListItem item2 = new ListItem();
            item2.Text = "首页通知";
            item2.Value = "2";

            ddl_selDisheType.Items.Add(item1);
            ddl_selDisheType.Items.Add(item2);
            ddl_selDisheType.Items.Add(itemDefault);
        }

        protected override void BindGridView()
        {
            int recount;
            int pagenums;
            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, "", out recount, out pagenums);
           
            if (dt != null)
            {
                gv_list.DataSource = dt;
                gv_list.DataBind();
                anp_top.RecordCount = recount;
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
                        GotoSearch();
                        break;
                    //刷新事件代码
                    case "refresh":
                        Response.Redirect(Request.Url.ToString());
                        break;
                    case "delete":
                        {
                            //日志信息

                            Selected = GetSelectStr(gv_list);
                           
                            string[] arrSel = Selected.Split(',');
                            if (arrSel.Length != 1)
                            {
                                sp_showmes.InnerText = "请选择一项进行操作";
                            }
                            bll.Delete("0", "0", arrSel[0]);
                            sp_showmes.InnerText = bll.oResult.Msg;
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请至少选择一项进行操作";
                        }

                        bll.UpdateStatus("", "0",Selected,"1");
                    if (ShowResult(bll.oResult.Code,bll.oResult.Msg, sp_showmes))
                    {
                        BindGridView();
                    }
                        break;
                    //无效
                    case "invalid":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请至少选择一项进行操作";
                        }

                        bll.UpdateStatus("", "0", Selected, "0");
                        if (ShowResult(bll.oResult.Code, bll.oResult.Msg, sp_showmes))
                        {
                            BindGridView();
                        }
                        break;
                }
            }
        }

        public void GotoSearch()
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" where 1=1");
            //拼接Where条件

            //类型

            string TB_DishType =this.ddl_selDisheType.SelectedValue;
            if (!string.IsNullOrEmpty(TB_DishType) && TB_DishType != "-1")
            {
                Where.Append(" and Type="+ TB_DishType);
            }

            string status =this.ddl_status.SelectedValue;
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                Where.Append(" and status='" + status + "'");

            }
            string disname = txt_disname.Value;
            if (!string.IsNullOrEmpty(disname))
            {
                Where.Append(" and (disname like '%" + disname + "%' or quickcode like '%" + disname + "%' or customcode like '%" + disname + "%')");
            }

            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }
    }
}