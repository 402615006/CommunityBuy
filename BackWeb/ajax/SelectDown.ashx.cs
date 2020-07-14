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
    public class SelectDown : ServiceBase
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
            }
            context.Response.Write(result);
            context.Response.End();
        }

        public string GetSto()//获取门店
        {
            string strWhere = string.Empty;

            DataTable dtStore = new bllStore().GetPagingListInfo("0", "0", int.MaxValue, 1, "where status='1' " + strWhere + "", "cname desc", out int recount, out int pagenums);
            List<StoreEntity> stolist = EntityHelper.GetEntityListByDR<StoreEntity>(dtStore.Select(), null);
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