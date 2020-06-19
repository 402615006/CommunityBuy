using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using System.Data;
using System.Web;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// toplist 的摘要说明
    /// </summary>
    public class toplist : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int intCount;
            int pagenums;
            bllFUNMAS bll = new bllFUNMAS();
            string userid = LoginedUser.UserInfo.Rol_ID.ToString();

            DataTable dt = bll.GetPagingInfo(10000, 1, "status='1' and level='1' and parentid=0  and id in(select funid from rolefunction where roleid in (" + LoginedUser.UserInfo.Rol_ID + ")) ", " orders asc", out intCount, out pagenums);

            string json = Helper.DataTableToJson("authlist", dt);

     

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