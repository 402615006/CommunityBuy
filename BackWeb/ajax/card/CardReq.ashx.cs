using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.ajax.card
{
    /// <summary>
    /// C_CardType 的摘要说明
    /// </summary>
    public class CardReq : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Charset = "utf-8";
            string type = Helper.ReplaceString(context.Request["way"]);
            string parids = string.Empty;
            if (context.Request["ids"] != null)
            {
                parids = Helper.ReplaceString(context.Request["ids"]);
            }

            int intCount;
            int pagenums;
            string json = string.Empty;
            DataTable dt = new DataTable();

            switch (type)
            {
                case "cardType":// 卡类型
                    dt = new bllmemcardlevel().GetPagingListInfo("0", "0", 10000, 1, "", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "cardTypelist" });
                    break;
                case "coupon":// 优惠活动  优惠券
                    dt = new bllmaincouponN().GetPagingListInfo("0", "0", 10000, 1, "", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "couponlist" });
                    break;

                case "ctype":
                    string fiter = " status=1   ";
                    dt = new BLL.bllmemcardtype().GetPagingListInfo("0", "", 10000, 1, fiter, "mctcode", out intCount, out pagenums);
                    json = Helper.DataTableToJson("ctypelist", dt);
                    break;
                case "level": //会员卡等级
                    fiter = string.Format(" status=1  and   mctcode = '{0}' ", parids);
                    dt = new BLL.bllmemcardlevel().GetPagingListInfo("0", "", 10000, 1, fiter, "levelcode", out intCount, out pagenums);
                    json = Helper.DataTableToJson("ctypelist", dt);
                    break;

                case "getcardnum":

                    string[] array = parids.Split(',');
                    fiter = string.Format("(cardCode = '{0}'  or cardCode='{1}')  and status = '0' ", array[0], array[1]);
                    dt = new BLL.bllmemprebatch().GetPagingListInfo("0", "", 10000, 1, fiter, " identityNum  desc ", out intCount, out pagenums);
                    json = Helper.DataTableToJson("data", dt);
                    break;



            }
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}