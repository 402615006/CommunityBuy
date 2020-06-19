using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using System.Data;
using System.Web;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// leftlist 的摘要说明
    /// </summary>
    public class leftlist : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string parentid = context.Request["parid"] == null ? "0" : context.Request["parid"].ToString();
            int intCount;
            int pagenums;
            bllFUNMAS bll = new bllFUNMAS();
            string userid = LoginedUser.UserInfo.Rol_ID.ToString();

            DataTable dt = bll.GetPagingInfo(10000, 1, "status='1' and ftype!=2 and id in(select funid from rolefunction where roleid in (" + LoginedUser.UserInfo.Rol_ID + ")) and parentid=" + parentid, " order by orders asc", out intCount, out pagenums);

            string json = Helper.DataTableToJson("leftlist",dt);
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