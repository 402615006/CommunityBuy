using CommunityBuy.BLL;
using System;
using System.Web.Security;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LoginedUser.UserInfo != null)
                {
                    FormsAuthentication.SignOut();
                    Session.Clear();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "logout", "logout()", true);
            }
        }
    }
}