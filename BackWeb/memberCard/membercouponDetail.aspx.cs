using Sam.WebControl;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class membercouponDetail : ListPage
    {
        public string cardcode;
        public string memcode;
        public string status;
        public string sumcode;
        public string mccode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                VisibleToolBarTrue("LinkRefresh");
                VisibleToolBarTrue("LinkSearch");
                bindWarInfo();
                BindSYStatus();
                BindGridView();
            }
        }

        /// <summary>
        /// 绑定是否使用 信息
        /// </summary>
        private void BindSYStatus()
        {
            this.ddl_sfsy.Items.Add(new ListItem("--全部--", ""));
            this.ddl_sfsy.Items.Add(new ListItem("未使用", "0"));
            this.ddl_sfsy.Items.Add(new ListItem("已使用", "1"));
            this.ddl_sfsy.Items.Add(new ListItem("作废", "2"));
        }

        //绑定门店信息
        protected void bindWarInfo()
        {
            string buscode = Helper.GetAppSettings("BusCode");
            DataTable dt = new bllPaging().GetDataTableInfoBySQL("select stocode,cname from Store where buscode='" + buscode + "' and stocode not in (select stocode from store where isfood='1' and pstocode='')");
            Helper.BindDropDownListForSearch(ddl_store, dt, "cname", "stocode", 2);
        }

        protected override void BindGridView()
        {
            if (Request["cardcode"] != null && Request["memcode"] != null && Request["status"] != null && Request["sumcode"] != null && Request["mccode"] != null)
            {
                cardcode = Request["cardcode"].ToString();
                memcode = Request["memcode"].ToString();
                status = Request["status"].ToString();
                sumcode = Request["sumcode"].ToString();
                mccode = Request["mccode"].ToString();

                string filter = "membercoupon.memcode='" + memcode + "' and membercoupon.cardcode='" + cardcode + "' and N_maincoupon.sumcode='" + sumcode + "' and N_maincoupon.mccode='" + mccode + "'";
                if (status.Length > 0)
                {
                    filter += " and coupon.status='" + status + "'";
                }

                //会员优惠券 
                if (string.IsNullOrEmpty(HidWhere.Value))
                {
                    HidWhere.Value = " where " + filter;
                }
                else
                {
                    HidWhere.Value += " and " + filter;
                }

                int recount;
                int pagenums;
                DataTable dtcoupon = new bllmembers().GetRefPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, "", out recount, out pagenums);
                if (dtcoupon != null)
                {
                    dtcoupon.Columns.Add("statusname", typeof(string));
                    for (int i = 0; i < dtcoupon.Rows.Count; i++)
                    {
                        string storename = dtcoupon.Rows[i]["storename"].ToString();
                        dtcoupon.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.CouponStatus), dtcoupon.Rows[i]["status"].ToString()); ;
                        if (storename.Length == 0)
                        {
                            dtcoupon.Rows[i]["storename"] = "不限";
                        }
                    }
                    gv_list.DataSource = dtcoupon;
                    gv_list.DataBind();
                    anp_top.RecordCount = recount;
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
            string sfsy = ddl_sfsy.SelectedValue;   //是否使用
            string fastore = ddl_store.SelectedValue; //发放门店
            string stime = Helper.ReplaceString(txt_stime.Value); //开始生效时间
            string etime = Helper.ReplaceString(txt_etime.Value);//结束过期时间
            string ffpeople = Helper.ReplaceString(txt_ffuser.Value); //发放人
            string ffstime = Helper.ReplaceString(txt_ffstime.Value);   //发放时间
            string ffetime = Helper.ReplaceString(txt_ffetime.Value);
            if (sfsy.Length > 0)
            {
                Where.Append(" and coupon.status='" + sfsy + "' ");
            }

            if (fastore.Length > 0)
            {
                Where.Append(" and coupon.prostocode='" + fastore + "'");
            }

            if (stime.Length > 0)
            {
                Where.Append(" and membercoupon.sdate>='" + stime + "'");
            }

            if (etime.Length > 0)
            {
                Where.Append(" and membercoupon.edate<'" + StringHelper.StringToDateTime(etime).AddDays(1).ToString() + "'");
            }

            if (ffpeople.Length > 0)
            {
                Where.Append(" and coupon.puser like '%" + ffpeople + "%'");
            }

            if (ffstime.Length > 0)
            {
                Where.Append(" and coupon.ptime>='" + ffstime + "'");
            }

            if (ffetime.Length > 0)
            {
                Where.Append(" and coupon.ptime<'" + StringHelper.StringToDateTime(ffetime).AddDays(1).ToString() + "'");
            }

            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }
    }
}