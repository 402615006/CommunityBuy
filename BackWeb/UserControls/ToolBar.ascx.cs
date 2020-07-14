using CommunityBuy.WebControl;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using System.Web;
using System.Web.Caching;

namespace CommunityBuy.BackWeb.UserControls
{
    public class ToolBarEventArgs : EventArgs
    {

        public ToolBarEventArgs(string strbtntype)
            : base()
        {
            _btntype = strbtntype;
        }
        private string _btntype;
        /// <summary>
        /// 按钮类型
        /// </summary>
        public string btntype
        {
            set { _btntype = value; }
            get { return _btntype; }
        }
    }
    public delegate void ToolBarClickHandler(object sender, ToolBarEventArgs e);
    public partial class ToolBar : System.Web.UI.UserControl
    {
        Cache WebCache = HttpContext.Current.Cache;

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                base.OnInit(e);
            }
            else
            {
                if (ViewState["setLastborder"] != null)
                {
                  WebHelper.WriteScriptMessage(ViewState["setLastborder"].ToString(),this.Page);
                }
            }
        }

        public event ToolBarClickHandler ToolBarClick;
        protected void OnToolBar_Click(object sender, ToolBarEventArgs e)
        {
            if (ToolBarClick != null)
            {
                if (this.Parent.FindControl("sp_showmes") != null)
                {
                    HtmlGenericControl show = (HtmlGenericControl)this.Parent.FindControl("sp_showmes");
                    show.InnerHtml = "";
                }
                ToolBarClick(this, e);
            }
        }

        /// <summary>
        /// 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToolBar_Click(object sender, EventArgs e)
        {
            string BtnType = ((System.Web.UI.WebControls.LinkButton)(sender)).CommandName;
            OnToolBar_Click(this, new ToolBarEventArgs(BtnType));
        }
    }
}