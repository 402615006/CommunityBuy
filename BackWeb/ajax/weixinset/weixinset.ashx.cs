using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.ajax.weixinset
{
    /// <summary>
    /// weixinset 的摘要说明
    /// </summary>
    public class weixinset : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            string type =context.Request["way"];
            switch (type)
            {
                default:
                    context.Response.Write("");
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class KeyValue
        {
            public string key { get; set; }
            public string value { get; set; }
        }
    }
}