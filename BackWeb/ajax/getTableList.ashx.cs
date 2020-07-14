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
            string type =context.Request["way"];
            string parids = string.Empty;
            string tbname = string.Empty;
            string idname = string.Empty;
            string cname = string.Empty;
            int currentpage = 1;
            int pageSize = 10;
            string filter = "";
            if (context.Request["ids"] != null)
            {
                parids =context.Request["ids"];
            }
            if (context.Request["tbname"] != null) //表名称
            {
                tbname =context.Request["tbname"];
            }
            if (context.Request["idname"] != null)//id列名
            {
                idname =context.Request["idname"];
            }
            if (context.Request["cname"] != null) //name列名
            {
                cname =context.Request["cname"];
            }
            if (context.Request["currentpage"] != null)
            {
                try
                {
                    currentpage =StringHelper.StringToInt(context.Request["currentpage"]);
                    pageSize = StringHelper.StringToInt(context.Request["pagesize"]);
                }
                catch { }
            }
            if (context.Request["filter"] != null)
            {
                filter =context.Request["filter"];
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
                case "store"://门店
                    dt = new bllStore().GetPagingListInfo("0", "0", 10000, 1, " where stocode in(" + codes + ") ", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "storelist" });
                    break;
                case "dishes"://菜品信息
                    dt = new bllTB_Dish().GetPagingListInfo("0", "0", 10000, 1, " where discode in(" + codes + ") ", " order by ctime desc ",out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "members"://用户
                    dt = new bllmembers().GetPagingListInfo("0", "0", 10000, 1, " where memcode in(" + codes + ") ", " order by ctime desc ", out intCount, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "memberslist" });
                    break;

                case "allbussiness"://动态查询,供影院调用
                    dt = new bllPaging().GetDataTableInfoBySQL("select buscode,cname from Business where status='1';");
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
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