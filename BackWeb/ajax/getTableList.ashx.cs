using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityBuy.CommonBasic;
using CommunityBuy.BLL;
using System.Data;
using System.Collections;


namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// getTableList 的摘要说明
    /// </summary>
    public class getTableList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = Helper.ReplaceString(context.Request["way"]);
            string parids = string.Empty;
            string tbname = string.Empty;
            string idname = string.Empty;
            string cname = string.Empty;
            int currentpage = 1;
            int pageSize = 10;
            string filter = "";
            if (context.Request["ids"] != null)
            {
                parids = Helper.ReplaceString(context.Request["ids"]);
            }
            if (context.Request["tbname"] != null) //表名称
            {
                tbname = Helper.ReplaceString(context.Request["tbname"]);
            }
            if (context.Request["idname"] != null)//id列名
            {
                idname = Helper.ReplaceString(context.Request["idname"]);
            }
            if (context.Request["cname"] != null) //name列名
            {
                cname = Helper.ReplaceString(context.Request["cname"]);
            }
            if (context.Request["currentpage"] != null)
            {
                try
                {
                    currentpage = int.Parse(Helper.ReplaceString(context.Request["currentpage"]));
                    pageSize = int.Parse(Helper.ReplaceString(context.Request["pagesize"]));
                }
                catch { }
            }
            if (context.Request["filter"] != null)
            {
                filter = Helper.ReplaceString(context.Request["filter"]);
            }
            string[] ids = parids.TrimEnd(',').TrimStart(',').Split(',');
            string codes = string.Empty;
            if (context.Request["codes"] != null && !string.IsNullOrWhiteSpace(context.Request["codes"].ToString()))
            {
                codes = context.Request["codes"].ToString();
            }
            else
            {
                foreach (string item in ids)
                {
                    codes += "'" + item + "',";
                }
                codes = codes.TrimEnd(',');
            }
            int intCount;
            int pagenums;
            string json = string.Empty;
            DataTable dt = new DataTable();

            switch (type)
            {
                case "admins"://用户
                    dt = new bllAdmins().GetPagingListInfo("0", "0", 10000, 1, " where userid in(" + codes + ") ", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "adminslist" });
                    break;
                case "adminsbyempid"://用户
                    dt = new bllAdmins().GetPagingListInfoByempid("0", "0", 10000, 1, " where a.empid in(select col from dbo.fn_StringSplit('" + codes.Replace("'", "") + "',',') where len(col)>0)", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "adminslist" });
                    break;
                case "financetype"://财务类别
                    dt = new bllFinanceType().GetPagingListInfo("0", "0", 10000, 1, " where fincode in(select col from dbo.fn_StringSplit('" + codes.Replace("'", "") + "',',') where len(col)>0) ", " order by lsid ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "financetypelist" });
                    break;
                case "paymethod"://支付方式
                    dt = new bllpaymethod().GetPagingListInfo("0", "0", 10000, 1, " where pmcode in(select col from dbo.fn_StringSplit('" + codes.Replace("'", "") + "',',') where len(col)>0) ", " order by pmcode ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "paymethod" });
                    break;
                case "costtype"://费用类别
                    dt = new bllCostType().GetPagingListInfo("0", "0", 10000, 1, " where coscode in(select col from dbo.fn_StringSplit('" + codes.Replace("'", "") + "',',') where len(col)>0) ", " order by coscode ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "costtype" });
                    break;
                case "store"://门店
                    dt = new bllStore().GetPagingListInfo("0", "0", 10000, 1, " where stocode in(" + codes + ") ", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "storelist" });
                    break;
                case "dishes"://菜品信息
                    dt = new blldishes().GetPagingListInfo("0", "0", 10000, 1, " where discode in(" + codes + ") ", " order by ctime desc ", string.Empty, string.Empty, 0, out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "dishes1"://菜品信息1
                    string stocode = "";
                    if (context.Request["stocode"] != null)
                    {
                        stocode = context.Request["stocode"].ToString();
                    }
                    dt = new blldishes().GetPagingListInfo1("0", "0", 10000, 1, " where discode in(" + codes + ") and stocode='" + stocode + "' ", " order by ctime desc ", string.Empty, string.Empty, 0, out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "dishes2"://菜品信息2
                    var code = codes.Replace("'", ""); ;
                    dt = new blldishes().GetPagingListInfo("0", "0", 10000, 1, " where buscode in ( select  col1 from [dbo].[fn_StringSplit3]('" + code + "',',','|')) and stocode in ( select  col2 from [dbo].[fn_StringSplit3]('" + code + "',',','|')) and discode in ( select  col3 from [dbo].[fn_StringSplit3]('" + code + "',',','|')) ", " order by ctime desc ", string.Empty, string.Empty, 0, out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "subcard"://次卡
                    dt = new bllTimesCard().GetPagingListInfo("0", "0", 10000, 1, " where actcode in(" + codes + ") ", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "sumcoupon": //选择优惠券
                    dt = new bllmaincouponN().GetPagingCouponListInfo("0", "0", 10000, 1, " where mccode in(" + codes + ") ", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "marketingN": //选择活动方案
                    dt = new bllmarketingN().GetPagingListInfo("0", "0", 10000, 1, " where pcode in(" + codes + ") ", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "mvgoods"://选择卖品
                    dt = new bllMVgoods().GetPagingListInfo("0", "0", 10000, 1, " where goodsid in(" + codes + ") ", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "other"://动态查询
                    string sql = "select {0},{1} from {2} where {0} in({3})";
                    dt = new bllPaging().GetDataTableInfoBySQL(string.Format(sql, idname, cname, tbname, codes));
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "members"://用户
                    dt = new bllmembers().GetPagingListInfo("0", "0", 10000, 1, " where memcode in(" + codes + ") ", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "memberslist" });
                    break;
                case "memcardcode"://用户
                    codes = codes.TrimStart('\'').TrimEnd('\'');
                    dt = new bllMemCard().GetPCardCodeByMemCode("0", "0", codes);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "memcard" });
                    break;
                case "warehouse"://仓库
                    dt = new bllStockWareHouse().GetPagingListInfo("0", "0", 10000, 1, " where warcode in(" + codes + ") ", " order by warcode asc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "list" });
                    break;
                case "allstore"://动态查询,供影院调用
                    filter = OEncryp.DecodeBase64(filter);
                    if (string.IsNullOrWhiteSpace(filter))
                    {
                        filter = " where stocode in ('07','10','15','HMHXYY')";
                    }
                    else
                    {
                        filter += " and stocode in ('07','10','15','HMHXYY')";
                    }
                    dt = new bllStore().GetPagingListInfo("0", "0", pageSize, currentpage, filter, " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" }, pageSize, intCount, currentpage, pagenums);
                    break;
                case "allbussiness"://动态查询,供影院调用
                    dt = new bllPaging().GetDataTableInfoBySQL("select buscode,cname from Business where status='1';");
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "dishesbystocode"://菜品信息,供影院调用
                    filter = OEncryp.DecodeBase64(filter);
                    dt = new blldishes().GetPagingListInfo1("0", "0", pageSize, currentpage, filter, " order by ctime desc ", string.Empty, string.Empty, 0, out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" }, pageSize, intCount, currentpage, pagenums);
                    break;
                case "filmcoupons"://影城优惠券,供影院调用
                    filter = OEncryp.DecodeBase64(filter);
                    dt = new bllmaincouponN().GetFilmCoupons("0", "0", pageSize, currentpage, filter, " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" }, pageSize, intCount, currentpage, pagenums);
                    break;
                case "membindingstat":
                    filter = OEncryp.DecodeBase64(filter);
                    dt = new bllMVmemBinding().GetPagingListInfo("0", "0", pageSize, currentpage, filter, " order by StatDate desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" }, pageSize, intCount, currentpage, pagenums);
                    break;
                case "activity":
                    filter = OEncryp.DecodeBase64(filter);
                    dt = new bllMVActivity().GetPagingListInfo("0", "0", pageSize, currentpage, filter, " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" }, pageSize, intCount, currentpage, pagenums);
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