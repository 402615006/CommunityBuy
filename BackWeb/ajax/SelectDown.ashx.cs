using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// SelectDown 的摘要说明
    /// </summary>
    public class SelectDown : IHttpHandler
    {
        JavaScriptSerializer json = new JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string code = context.Request["code"].ToString();
            string result = string.Empty;
            switch (code)
            {
                case "stocode"://门店
                    result = GetSto();
                    break;
                case "war"://获取仓库
                    if (context.Request["stocode"] != null)
                    {
                        result = GetWar(context.Request["stocode"].ToString());
                    }
                    else
                    {
                        result = GetWar("");
                    }
                    break;
                case "pd"://日周盘点
                    result = GetCtype(context.Request["stocode"].ToString(), context.Request["warcode"].ToString());
                    break;
                case "bz":
                    result = GetStructural(context.Request["stocode"].ToString());
                    break;
                case "zp":
                    result = GetZBCtype(context.Request["stocode"].ToString(), context.Request["warcode"].ToString(), context.Request["scoutime"].ToString(), context.Request["ctype"].ToString());
                    break;
                case "wartype"://获取仓库
                    if (context.Request["stocode"] != null)
                    {
                        if (!string.IsNullOrEmpty(context.Request["type"].ToString()))
                        {
                            result = GetWarType(context.Request["stocode"].ToString(), context.Request["type"].ToString());
                        }
                        else
                        {
                            result = GetWarType(context.Request["stocode"].ToString(), "");
                        }
                    }
                    else
                    {
                        result = GetWar("");
                    }
                    break;
                case "yyzx"://运营中心
                    result = GetYyzxList();
                    break;
                case "yyzxsto"://运营中心
                    result = GetYyzxStoList(context.Request["dcode"].ToString());
                    break;
            }
            context.Response.Write(result);
            context.Response.End();
        }

        public string GetSto()//获取门店
        {
            int recount = 0;
            int pagenums = 0;

            string strWhere = string.Empty;
            if (LoginedUser.UserInfo.rolstocode.Length > 0)
            {
                strWhere = " and stocode in('" + LoginedUser.UserInfo.rolstocode.Replace(",", "','") + "')";
            }

            DataTable dtStore = new bllStore().GetPagingListInfo("0", "0", int.MaxValue, 1, "where status='1' " + strWhere + "", "cname desc", out recount, out pagenums);
            List<StoreEntity> stolist = EntityHelper.GetEntityListByDR<StoreEntity>(dtStore.Select(), null);
            return json.Serialize(stolist);
        }

        public string GetWar(string stocode)//获取仓库
        {
            int recount = 0;
            int pagenums = 0;
            DataTable dtWar = new DataTable();
            if (!string.IsNullOrEmpty(stocode))
            {
                dtWar = new bllStockWareHouse().GetPagingListInfo("0", "0", int.MaxValue, 1, "where stocode='" + stocode + "' and status='1'", "warname desc", out recount, out pagenums);
            }
            else
            {
                dtWar = new bllStockWareHouse().GetPagingListInfo("0", "0", int.MaxValue, 1, "where status='1'", "warname desc", out recount, out pagenums);
            }
            List<StockWareHouseEntity> stolist = EntityHelper.GetEntityListByDR<StockWareHouseEntity>(dtWar.Select(), null);
            return json.Serialize(stolist);
        }

        public string GetCtype(string stocode, string warcode)
        {
            bllStockCounting bll = new bllStockCounting();
            DataTable dtCtype = new DataTable();
            string sql = "select CONVERT(varchar(100), scoutime, 120)+(case ctype when 0 then '日' when 1 then '周' end) as buscode,scoutime as stocode from catering_stock.dbo.stockcountingday where stocode='" + stocode + "' and warcode='" + warcode + "' and audstatus='1' and ctype in('0','1')";
            DataTable dt = bll.GetSelectCountingData(sql);
            List<StockCountingDayEntity> stolist = EntityHelper.GetEntityListByDR<StockCountingDayEntity>(dt.Select(), null);
            return json.Serialize(stolist);
        }

        public string GetStructural(string stocode)
        {
            bllStockCounting bll = new bllStockCounting();
            DataTable dtCtype = new DataTable();
            string sql = "select mll,djl from StoStructuralLog where stocode='" + stocode + "'";
            DataTable dt = bll.GetSelectCountingData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                string _mll = dt.Rows[0]["mll"].ToString();
                string _djl = dt.Rows[0]["djl"].ToString();
                return json.Serialize(new { mll = _mll, djl = _djl });
            }
            return "";
        }

        public string GetZBCtype(string stocode, string warcode, string scoutime, string ctype)
        {
            bllStockCounting bll = new bllStockCounting();
            DataTable dtCtype = new DataTable();
            string sql = "select top 1 scoutime from catering_stock.dbo.stockcountingday where stocode='" + stocode + "' and warcode='" + warcode + "' and scoutime<'" + scoutime + "' and audstatus='1' and ctype='" + ctype + "' order by scoutime desc";
            DataTable dt = bll.GetSelectCountingData(sql);
            List<StockCountingDayEntity> stolist = EntityHelper.GetEntityListByDR<StockCountingDayEntity>(dt.Select(), null);
            return json.Serialize(stolist);
        }

        public string GetWarType(string stocode, string wartype)//获取仓库
        {
            int recount = 0;
            int pagenums = 0;
            DataTable dtWar = new DataTable();
            if (!string.IsNullOrEmpty(stocode))
            {
                if(!string.IsNullOrEmpty(wartype))
                {
                    dtWar = new bllStockWareHouse().GetPagingListInfo("0", "0", int.MaxValue, 1, "where stocode='" + stocode + "' and status='1' and warkittype='" + wartype + "'", "warname desc", out recount, out pagenums);
                }
                else
                {
                    dtWar = new bllStockWareHouse().GetPagingListInfo("0", "0", int.MaxValue, 1, "where stocode='" + stocode + "' and status='1' and warkittype in('1','2')", "warname desc", out recount, out pagenums);
                }
            }
            else
            {
                dtWar = new bllStockWareHouse().GetPagingListInfo("0", "0", int.MaxValue, 1, "where status='1'", "warname desc", out recount, out pagenums);
            }
            List<StockWareHouseEntity> stolist = EntityHelper.GetEntityListByDR<StockWareHouseEntity>(dtWar.Select(), null);
            return json.Serialize(stolist);
        }

        public string GetYyzxList()
        {
            bllStockCounting bll = new bllStockCounting();
            DataTable dtCtype = new DataTable();
            string sql = "select dcode,dname from Department where status=1 and stocode='' and dcode not in('D0000018','D0000019','D0000020','D0000021')";
            DataTable dt = bll.GetSelectCountingData(sql);
            List<DepartmentEntity> stolist = EntityHelper.GetEntityListByDR<DepartmentEntity>(dt.Select(), null);
            return json.Serialize(stolist);
        }

        public string GetYyzxStoList(string pdcode)
        {
            bllStockCounting bll = new bllStockCounting();
            DataTable dtCtype = new DataTable();
            string strWhere = string.Empty;
            if (LoginedUser.UserInfo.rolstocode.Length > 0)
            {
                strWhere = " and stocode in('" + LoginedUser.UserInfo.rolstocode.Replace(",", "','") + "')";
            }
            if(pdcode== "D0000008")
            {
                if (LoginedUser.UserInfo.rolstocode.Length > 0)
                {
                    if (LoginedUser.UserInfo.rolstocode.Contains("12,"))
                    {
                        strWhere += " and stocode = '12'";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    strWhere += " and stocode = '12'";
                }
            }
            string sql = "select stocode,dname from Department where status=1 and dcode not in('D0000018','D0000019','D0000020','D0000021') and pdcode='"+ pdcode + "'"+strWhere;
            DataTable dt = bll.GetSelectCountingData(sql);
            List<DepartmentEntity> stolist = EntityHelper.GetEntityListByDR<DepartmentEntity>(dt.Select(), null);
            return json.Serialize(stolist);
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