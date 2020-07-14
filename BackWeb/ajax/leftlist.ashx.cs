using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using System.Data;
using System.Web;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// leftlist 的摘要说明
    /// </summary>
    public class leftlist : ServiceBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            context.Response.ContentType = "text/plain";
            string parentid = context.Request["parid"] == null ? "0" : context.Request["parid"].ToString();
            string json = "";
            if (base.LoginedUser.Permission.Select("parentid=" + parentid).Length > 0)
            {
                DataTable dt = base.LoginedUser.Permission.Select("parentid=" + parentid).CopyToDataTable();
                json = JsonHelper.DataTableToJSON(dt);
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