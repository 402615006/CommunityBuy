using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// getuser 的摘要说明
    /// </summary>
    public class getuser : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = LoginedUser.UserInfo.cname.ToString();
            context.Response.Write("{\"uname\":\"" + username + "\"}");
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