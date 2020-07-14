using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// Ajax 的摘要说明
    /// </summary>
    public class Ajax : IHttpHandler
    {
        bllPaging bll = new bllPaging();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string actionname =context.Request["actionname"];
            switch (actionname.ToLower())
            {
            }
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