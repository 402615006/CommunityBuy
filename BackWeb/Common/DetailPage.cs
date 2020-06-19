using System;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.Common
{
    public class DetailPage : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.DesignMode)
            {
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
            }
            base.OnLoad(e);
        }
    }        
}