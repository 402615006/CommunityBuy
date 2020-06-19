using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string qid = context.Request.QueryString["q"] ?? "";
            string name = context.Request.QueryString["n"] ?? "";
            try
            {
                if (!string.IsNullOrEmpty(qid) && !string.IsNullOrEmpty(name))
                {
                    UploadPicture.DelPic(string.Format("\\original\\{0}", name));
                    context.Response.Write(qid);
                }
                else
                {
                    context.Response.Write("0");
                }
            }
            catch { context.Response.Write("0"); }
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