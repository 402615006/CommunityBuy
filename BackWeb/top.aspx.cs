using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using System;

namespace CommunityBuy.BackWeb
{
    public partial class top : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //判断cookie是否过期
                if (base.LoginedUser == null)
                {
                    Response.Write("<script> top.location.href='" + Helper.GetAppSettings("HomePageUrl") + "';</script>");
                    return;
                }
                AdminName.InnerText = LoginedUser.Name;
            }
            catch { Response.Write("<script> top.location.href='" + Helper.GetAppSettings("HomePageUrl") + "';</script>"); }
        }
    }
}