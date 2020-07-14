using CommunityBuy.BLL;
using System;
using System.Web.Security;
using CommunityBuy.CommonBasic;
using CommunityBuy.BackWeb.Common;

namespace CommunityBuy.BackWeb
{
    public partial class logout :System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                    FormsAuthentication.SignOut();
                    Session.Clear();
                ClientScript.RegisterStartupScript(this.GetType(), "logout", "logout()", true);
            }
        }
    }
}