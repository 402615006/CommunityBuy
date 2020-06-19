using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// deletefile 的摘要说明
    /// </summary>
    public class deletefile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string filepath = context.Request["filepath"];
            context.Response.Write("Hello World");
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