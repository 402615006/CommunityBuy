using System.Web;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// getuser 的摘要说明
    /// </summary>
    public class getuser : ServiceBase
    {

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            context.Response.ContentType = "text/plain";
            context.Response.Write("{\"uname\":\"" + base.LoginedUser.Name + "\"}");
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