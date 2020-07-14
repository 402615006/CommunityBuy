using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
using CommunityBuy.BackWeb.Common;

namespace CommunityBuy.BackWeb
{
    public partial class couponcheck : EditPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        /// <summary>
        /// 查看优惠券信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void readCard_Click(object sender, EventArgs e)
        {
            errormessage.InnerHtml = string.Empty;
            if (txt_coupon.Text.Length == 0)
            {
                errormessage.InnerHtml = "请输入优惠券券码！";
                return;
            }
            var checkcode = txt_coupon.Text.Replace(" ", ""); ;
            string newCode = string.Format("{0}-{1}-{2}-{3}", checkcode.Substring(0, 4), checkcode.Substring(4, 4), checkcode.Substring(8, 4), checkcode.Substring(12, 4));
            string MemcardUrl = Helper.GetAppSettings("MemberCardUrl") + "/coupon/WScheckcoupon.ashx";
            StringBuilder postStr = new StringBuilder();
            postStr.Append("actionname=getcoupondetail&usercode=" + base.LoginedUser.empcode + "&parameters={\"GUID\":\"\",\"USER_ID\":\"\",\"buscode\":\"" + Helper.GetAppSettings("BusCode") + "\",\"usercode\":\"" + base.LoginedUser.empcode + "\",\"couponcode\":\"" + newCode + "\",\"stocode\":\"" + base.LoginedUser.stocode + "\",\"way\":\"PC\"}");//键值对

            string strAdminJson = Helper.HttpWebRequestByURL(MemcardUrl, postStr);
            if (!string.IsNullOrEmpty(strAdminJson))
            {
                string msg = string.Empty;
                string status = string.Empty;
                DataSet ds = JsonHelper.JsonToDataSet(strAdminJson, out status, out msg);
                if (status == "0")
                {
                    if (ds.Tables.Count >= 1)
                    {
                        DataTable dtCoupon = ds.Tables[0];
                        hidcoupons.Value = dtCoupon.Rows[0]["checkcode"].ToString();

                        lbshowname.Text = dtCoupon.Rows[0]["couname"].ToString() + "（" + StringHelper.StringToDateTime(dtCoupon.Rows[0]["btime"].ToString()).ToString("yyyy.MM.dd") + "-" + StringHelper.StringToDateTime(dtCoupon.Rows[0]["etime"].ToString()).ToString("yyyy.MM.dd") + "）";
                        this.Free_btn.Visible = true;
                    }
                    else
                    {
                        errormessage.InnerHtml = "获取优惠券数据网络异常，请检查！";
                        return;
                    }
                }
                else
                {
                    errormessage.InnerHtml = msg;
                    return;
                }

            }
        }

        /// <summary>
        /// 优惠券消券
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            string MemcardUrl = Helper.GetAppSettings("MemberCardUrl") + "/coupon/WScheckcoupon.ashx";
            StringBuilder postStr = new StringBuilder();
            postStr.Append("actionname=couponrecoverynew&usercode=" + base.LoginedUser.empcode + "&parameters={\"GUID\":\"\",\"USER_ID\":\"\",\"buscode\":\"" + Helper.GetAppSettings("BusCode") + "\",\"couponcode\":\"" + hidcoupons.Value + "\",\"stocode\":\"" + base.LoginedUser.stocode + "\",\"way\":\"PC\",\"username\":\"" + base.LoginedUser.Name + "\",\"usercode\":\"" + base.LoginedUser.empcode + "\",\"orderno\":\"\"}");
            string strAdminJson = Helper.HttpWebRequestByURL(MemcardUrl, postStr);
            if (!string.IsNullOrEmpty(strAdminJson))
            {
                string msg = string.Empty;
                string status = string.Empty;
                DataSet ds = JsonHelper.JsonToDataSet(strAdminJson, out status, out msg);
                if (status == "0")
                {
                    if (ds.Tables.Count >= 1)
                    {
                        int num = 0;
                        DataTable dtCouponList = ds.Tables[0];
                        string tbhtml = "<table id='tbcoupon'><tr><td>序号</td><td>券名称</td><td>券码</td><td>过期时间</td><td>金额</td></tr>{0}</table>";
                        string trstring = "<tr {4}><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{5}</td></tr>";
                        StringBuilder sbTR = new StringBuilder();
                        if (dtCouponList.Rows.Count > 0)
                        {
                            //加载历史记录
                            if (!string.IsNullOrEmpty(hidId.Value))
                            {
                                string[] coupon = hidId.Value.Split(',');
                                for (int i = 0; i < coupon.Length; i++)
                                {
                                    if (coupon[i].Length > 0)
                                    {
                                        string[] content = coupon[i].Split('|');
                                        sbTR.Append(string.Format(trstring, (i + 1).ToString(), content[0], content[1], content[2], "class='trtoday'", content[3]));
                                    }
                                }
                            }
                            //加载新消券记录
                            for (int i = 0; i < dtCouponList.Rows.Count; i++)
                            {
                                string couname = dtCouponList.Rows[i]["couname"].ToString();
                                string checkcode = dtCouponList.Rows[i]["checkcode"].ToString();
                                string time = StringHelper.StringToDateTime(dtCouponList.Rows[0]["btime"].ToString()).ToString("yyyy.MM.dd") + "-" + StringHelper.StringToDateTime(dtCouponList.Rows[0]["etime"].ToString()).ToString("yyyy.MM.dd");
                                string singlemoney = dtCouponList.Rows[i]["singlemoney"].ToString();
                                hidId.Value = hidId.Value + ',' + couname + '|' + checkcode + '|' + time + '|' + singlemoney;

                                sbTR.Append(string.Format(trstring, (i + 1).ToString(), couname, checkcode, time, "class='trtoday'", singlemoney));

                            }
                        }
                        tb_freeinfo.InnerHtml = string.Format(tbhtml, sbTR.ToString());

                        hidcoupons.Value = string.Empty;
                        this.Free_btn.Visible = false;
                        this.errormessage.InnerHtml = "消券成功！";
                    }
                }
            }
        }


    }

}