using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using System;

namespace CommunityBuy.BackWeb
{
    public partial class top : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //判断cookie是否过期
                if (LoginedUser.UserInfo == null)
                {
                    Response.Write("<script> top.location.href='" + Helper.GetAppSettings("HomePageUrl") + "';</script>");
                    return;
                }
                AdminName.InnerText = LoginedUser.UserInfo.cname;
            }
            catch { Response.Write("<script> top.location.href='" + Helper.GetAppSettings("HomePageUrl") + "';</script>"); }
        }
    }
}