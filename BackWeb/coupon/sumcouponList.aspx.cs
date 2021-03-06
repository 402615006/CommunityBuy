﻿using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class sumcouponList : Common.ListPage
    {
        bllsumcoupon bll = new bllsumcoupon();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = "列表";
            if (!IsPostBack)
            {
                //BindStoreInfo();
                BindStatus();
                GotoSearch();
            }
        }



        /// <summary>
        /// 绑定状态信息
        /// </summary>
        private void BindStatus()
        {
            ddl_zt.Items.Insert(0, new ListItem("--全部--", ""));
            ddl_zt.Items.Insert(1, new ListItem("正常", "1"));
            ddl_zt.Items.Insert(2, new ListItem("作废", "2"));
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
                dt.Columns.Add("btimename", typeof(string));
                dt.Columns.Add("ctypename", typeof(string));
                dt.Columns.Add("initypename", typeof(string));
                dt.Columns.Add("statusname", typeof(string));
                dt.Columns.Add("audstatusname", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["btimename"] = StringHelper.StringToDateTime(dt.Rows[i]["btime"].ToString()).ToString("yyyy.MM.dd") + "-" + StringHelper.StringToDateTime(dt.Rows[i]["etime"].ToString()).ToString("yyyy.MM.dd");
                    dt.Rows[i]["ctypename"] = "";
                    dt.Rows[i]["initypename"] = "";
                    dt.Rows[i]["statusname"] = "";
                    dt.Rows[i]["audstatusname"] = "";
                }
                gv_list.DataSource = dt;
                gv_list.DataBind();
                anp_top.RecordCount = recount;
            }
            else
            {
                DataTable dt1 = gv_list.DataSource as DataTable;
                if (dt1 != null)
                {
                    dt1.Rows.Clear();

                    gv_list.DataSource = dt1;
                    gv_list.DataBind();
                    anp_top.RecordCount = 0;
                }
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
                                sp_showmes.InnerText = "";
                            }
                            else
                            {
                                int recnums = 0;
                                int pagenums = 0;
                                dt = bll.GetPagingListInfo("", "0", 50000, 1, " where sumid in (" + Selected + ")", "", out recnums, out pagenums);
                                string auditstatus = "0";
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["audstatus"].ToString() != "0")
                                        {
                                            auditstatus = "1";
                                        }
                                    }
                                }
                                if (auditstatus == "1")
                                {
                                    sp_showmes.InnerText = "";
                                }
                                else
                                {
                                    bll.Delete("0", "0", Selected);
                                    sp_showmes.InnerText = "";
                                    anp_top.CurrentPageIndex = 1;
                                }
                            }
                        }
                        break;
                    case "edit":
                        {
                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = "";
                            }
                            else
                            {
                                string[] sel = Selected.Split(',');
                                if (sel.Length > 1)
                                {
                                    sp_showmes.InnerText = "";
                                }
                                else
                                {
                                    dt = bll.GetPagingSigInfo("", "0", " where sumid =" + Selected);
                                    string auditstatus = "0";
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            if (dt.Rows[i]["audstatus"].ToString() != "0")
                                            {
                                                auditstatus = "1";
                                            }
                                        }
                                    }
                                    if (auditstatus == "1")
                                    {
                                        sp_showmes.InnerText = "";
                                    }
                                    else
                                    {
                                        Response.Redirect("/coupon/sumcouponEdit.aspx?id=" + Selected);
                                    }
                                }
                            }
                        }
                        break;
                    //作废
                    case "nullify":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "";
                        }
                        else
                        {
                            bll.UpdateStatus("", "0", Selected, "1");

                            sp_showmes.InnerText = "";
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //作废未发放
                    case "nullifynotuse":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "";
                        }
                        else
                        {
                            dt = bll.GetPagingSigInfo("", "0", " where sumid =" + Selected);
                            string auditstatus = "0";
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                auditstatus = dt.Rows[0]["audstatus"].ToString();
                                if (auditstatus == "0" || auditstatus == "2")
                                {
                                    sp_showmes.InnerText = "";
                                }
                                else
                                {
                                    bll.UpdateStatusNotSend("", "0", Selected);

                                    sp_showmes.InnerText = "";
                                    anp_top.CurrentPageIndex = 1;
                                }
                            }
                            else
                            {
                                sp_showmes.InnerText = "";
                            }
                        }
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
            Where.Append(" where 1=1 and ctype<>'2' ");
            //拼接Where条件
            string strcname = txt_cname.Value;
            if (strcname.Length > 0)
            {
                Where.Append(" and cname like '%" + strcname + "%'");
            }

            string strctype = ddl_ctype.SelectedValue;
            if (strctype.Length > 0)
            {
                Where.Append(" and ctype= '" + strctype + "'");

            }
            string strinitype = ddl_initype.SelectedValue;
            if (strinitype.Length > 0)
            {
                Where.Append(" and initype= '" + strinitype + "'");

            }
            string strstatus = ddl_zt.SelectedValue;
            if (strstatus.Length > 0)
            {
                Where.Append(" and status= '" + strstatus + "'");

            }
            string straudstatus = ddl_audstatus.SelectedValue;
            if (straudstatus.Length > 0)
            {
                Where.Append(" and audstatus= '" + straudstatus + "'");

            }
            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }
    }
}