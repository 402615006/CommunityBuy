using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.ajax.system
{
    /// <summary>
    /// globalreq 的摘要说明
    /// </summary>
    public class S_Province : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string type =context.Request["way"];
            string parids = string.Empty;
            if (context.Request["ids"] != null)
            {
                parids =context.Request["ids"];
            }
            int intCount;
            int pagenums;
            string json = string.Empty;
            DataTable dt = new DataTable();
            switch (type)
            {
                //case "provice":
                //    bllprovinces bllprovinces = new bllprovinces();
                //    dt = bllprovinces.GetPagingListInfo("0", "0", 100, 1, "", "", out intCount, out pagenums);
                //    json = Helper.DataTableToJson("provincelist", dt);
                //    break;
                //case "city":
                //    bllcitys bll = new bllcitys();
                //    dt = bll.GetPagingListInfo("0", "0", 100, 1, " where parentid=" + parids, "", out intCount, out pagenums);
                //    json = Helper.DataTableToJson("citylist", dt);
                //    break;
                //case "area":
                //    bllareas bllarea = new bllareas();
                //    dt = bllarea.GetPagingListInfo("0", "0", 100, 1, " where parentid=" + parids, "", out intCount, out pagenums);
                //    json = Helper.DataTableToJson("arealist", dt);
                //    break;
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