using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.WebControl
{
    [ToolboxData("<{0}:CPageTitle runat=server></{0}:CPageTitle>")]
    sealed public class CPageTitle:Label
    {
        public CPageTitle()
		{
        }
        private string _Menu;
        [Browsable(true), Category("自定义属性"), Description("功能菜单")]
        public string Menu
        {
            get
            {
                return _Menu;
            }
            set
            {
                _Menu = value;
            }
        }
        private string _Operate;
        [Browsable(true), Category("自定义属性"), Description("操作")]
        public string Operate
        {
            get
            {
                return _Operate;
            }
            set
            {
                _Operate = value;
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.Write("<span data-code=\"Menu\" >" + this.Menu + "</span> - " + this.Operate);
            base.RenderEndTag(writer);
        }
    }
}