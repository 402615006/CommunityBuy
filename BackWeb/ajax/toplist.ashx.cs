using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using System.Data;
using System.Web;
using System.Web.Caching;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// toplist 的摘要说明
    /// </summary>
    public class toplist : ServiceBase
    {
        public override  void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            context.Response.ContentType = "text/plain";
            DataTable dt =base.LoginedUser.Permission.Select("parentid=0").CopyToDataTable();
            string json = JsonHelper.DataTableToJSON(dt);
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