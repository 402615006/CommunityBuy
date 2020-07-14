using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

namespace CommunityBuy.WebControl
{
    [ToolboxData("<{0}:CPathBar runat=server></{0}:CPathBar>")]
    sealed public class CPathBar : Label
    {
        public CPathBar()
        {
        }

        /// <summary>
        /// 页面类型
        /// </summary>
        public enum PType
        {
            //列表页
            List = 0,
            //新增页
            Add = 1,
            //编辑页
            Edit = 2,
            //详细页
            Detail = 3,
            //参照页
            Referer = 4,
            //不验证
            Normal = 5,
            //发布  微信
            Found = 6,
            //批量发卡  会员卡
            Sendbatchcard = 7,
            //上传
            Upload = 8,
            //付款
            Pay = 9,
            //退款
            Backpay = 10,
            //审核
            Audit = 11
        }

        private PType _PageType;
        [Browsable(true), Category("自定义属性"), Description("页面类型")]
        public PType PageType
        {
            get { return _PageType; }
            set { _PageType = value; }
        }

        private string _MainMenu;
        [Browsable(true), Category("自定义属性"), Description("一级菜单名称")]
        public string MainMenu
        {
            get
            {
                return _MainMenu;
            }
            set
            {
                _MainMenu = value;
            }
        }
        private string _SubMenu;
        [Browsable(true), Category("自定义属性"), Description("二级菜单名称")]
        public string SubMenu
        {
            get
            {
                return _SubMenu;
            }
            set
            {
                _SubMenu = value;
            }
        }

        private string _PageCode;
        [Browsable(true), Category("自定义属性"), Description("页面代码")]
        public string PageCode
        {
            get { return _PageCode; }
            set { _PageCode = value; }
        }

        private bool _IsNoLoad = true;
        [Browsable(true), Category("自定义属性"), Description("是否执行权限")]
        public bool IsNoLoad
        {
            get { return _IsNoLoad; }
            set { _IsNoLoad = value; }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.Write("<span data-code=\"position\" class=\"CPathBar\">您当前位置：</span><span data-code=\"MainMenu\" class=\"CPathBar\">" + this.MainMenu + "</span> >> <span data-code=\"SubMenu\" class=\"CPathBar\">" + this.SubMenu + "</span>");
            base.RenderEndTag(writer);
        }
    }
}