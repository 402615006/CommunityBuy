using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace CommunityBuy.WebControl
{
    [ToolboxData("<{0}:CTextBox runat=server></{0}:CTextBox>")]
    sealed public class CTextBox : TextBox
    {
        [Description("文本框类型")]
        public enum eType
        {
            [CAttribute(Name = "默认格式")]
            Normal,
            [CAttribute(Name = "手机格式")]
            Mobile,
            [CAttribute(Name = "短日期格式")]
            ShortDate,
            [CAttribute(Name = "长日期格式")]
            LongDate,
            [CAttribute(Name = "时间格式")]
            Time,
            [CAttribute(Name = "时间格式")]
            HourM,
            [CAttribute(Name = "时间格式")]
            Hour,
            [CAttribute(Name = "邮箱地址格式")]
            Email,
            [CAttribute(Name = "电话格式")]
            Tel,
            [CAttribute(Name = "身份证格式")]
            ChineseID,
            [CAttribute(Name = "整形格式")]
            Int,
            [CAttribute(Name = "小数格式")]
            Decimal,
            [CAttribute(Name = "用户名格式")]
            User,
            [CAttribute(Name = "密码格式")]
            Pwd,
            [CAttribute(Name = "数字+,")]
            IntD,
            [CAttribute(Name = "正小数格式")]
            Decimal1,
            [CAttribute(Name = "Float格式")]
            Float,
            [CAttribute(Name = "正Float格式")]
            Float1
        }
        public CTextBox()
        {
            _TextType = eType.Normal;
            _IsRequired = false;
            _IsALPHANUM = false;
            this.Init += new EventHandler(CTextBox_Init);
        }
        private eType _TextType;
        [Browsable(true), Category("自定义属性"), Description("文本框类型，默认普通文本框。")]
        public eType TextType
        {
            get { return _TextType; }
            set { _TextType = value; }
        }

        private bool _IsALPHANUM;
        [Browsable(true), Category("自定义属性"), Description("是否字母数字输入法，默认普通。")]
        public bool IsAlphanum
        {
            get { return _IsALPHANUM; }
            set { _IsALPHANUM = value; }
        }

        private bool _IsRequired;
        [Browsable(true), Category("自定义属性"), Description("是否必填项，默认非必填项。")]
        public bool IsRequired
        {
            get { return _IsRequired; }
            set { _IsRequired = value; }
        }

        private string _placeholder;
        [Browsable(true), Category("自定义属性"), Description("输入提示信息，默认非必填项。")]
        public string placeholder
        {
            get { return _placeholder; }
            set { _placeholder = value; }
        }

        private void CTextBox_Init(object sender, EventArgs e)
        {
            int intMaxLength = 0;
            string strToolTip = string.Empty;
            this.Attributes.Add("TextType", _TextType.ToString());
            switch (_TextType)
            {
                case eType.Mobile:
                    this.Attributes.Add("onkeypress", "return OnKeyPressCheck(this,'Mobile');");
                    strToolTip = "不超过11位手机号码";
                    intMaxLength = 11;
                    break;
                case eType.Tel:
                    this.Attributes.Add("onkeypress", "return OnKeyPressCheck(this,'Tel');");
                    strToolTip = "不超过20位的电话号码";
                    intMaxLength = 20;
                    break;
                case eType.ChineseID:
                    this.Attributes.Add("onkeypress", "return OnKeyPressCheck(this,'ChineseID');");
                    strToolTip = "18位身份证号码";
                    intMaxLength = 18;
                    break;
                case eType.Int:
                    this.Attributes.Add("onkeypress", "return OnKeyPressCheck(this,'Int');");
                    strToolTip = "不超过9位的整数";
                    intMaxLength = 8;
                    break;
                case eType.Decimal:
                    this.Attributes.Add("onkeypress", "return OnKeyPressCheck(this,'Decimal');");
                    strToolTip = "不超过18位的小数或整数";
                    intMaxLength = 18;
                    break;
                case eType.Decimal1:
                    this.Attributes.Add("onkeypress", "return OnKeyPressCheck(this,'Decimal1');");
                    strToolTip = "不超过18位的正小数或正整数";
                    intMaxLength = 18;
                    break;
                case eType.Float:
                    this.Attributes.Add("onkeypress", "return OnKeyPressCheck(this,'Float');");
                    strToolTip = "不超过18位的小数或整数";
                    intMaxLength = 18;
                    break;
                case eType.Float1:
                    this.Attributes.Add("onkeypress", "return OnKeyPressCheck(this,'Float1');");
                    strToolTip = "不超过18位的正小数或正整数";
                    intMaxLength = 18;
                    break;
                case eType.ShortDate:
                    this.Width = 70;
                    intMaxLength = 10;
                    this.ReadOnly = true;
                    this.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    //this.Attributes.Add("onfocus", "WebCalendar.timeShowType = '0';calendar();");
                    break;
                case eType.LongDate:
                    intMaxLength = 19;
                    this.ReadOnly = true;
                    this.Text = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                    //this.Attributes.Add("onfocus", "WebCalendar.timeShowType = '1';calendar();");
                    break;
                case eType.Time:
                    this.Width = 50;
                    intMaxLength = 8;
                    this.ReadOnly = true;
                    this.Text = DateTime.Now.ToString("HH:00:00");
                    //this.Attributes.Add("onfocus", "WebCalendar.timeShowType = '2';calendar();");
                    break;
            }
            if (_placeholder == null)
            {
                this.Attributes.Add("placeholder", strToolTip);
            }
            else
            {
                if (_placeholder.Length == 0)
                {
                    this.Attributes.Add("placeholder", strToolTip);
                }
                else
                {
                    this.Attributes.Add("placeholder", this.placeholder);
                }
            }

            switch (_TextType)
            {
                case eType.Int:
                case eType.Decimal:
                case eType.Decimal1:
                case eType.ChineseID:
                case eType.Mobile:
                case eType.Email:
                case eType.User:
                case eType.Pwd:
                case eType.Tel:
                case eType.Float:
                case eType.Float1:
                    //禁止粘贴
                    this.Attributes.Add("onpaste", "return false");
                    //转换到默认输入法
                    this.Style.Add("ime-mode", "disabled");
                    break;
            }
            if (strToolTip.Length > 0)
            {
                this.ToolTip = strToolTip;
            }
            //用户有定义长度，则不改变用户定义的长度
            if (this.MaxLength <= 0)
            {
                if (intMaxLength > 0)
                {
                    this.MaxLength = intMaxLength;
                }
            }
            if (this.Width.Value <= 0)
            {
                this.Width = 170;
            }
            if (_IsRequired)
            {
                this.CssClass = "reqtxtstyle";
                this.Attributes.Add("IsRequired", "true");
            }
            else
            {
                this.CssClass = "txtstyle";
                this.Attributes.Add("IsRequired", "false");
            }
            if (_IsALPHANUM)
            {
                this.Attributes.Add("onkeyup", "value=value.replace(/[\\W]/g,'');");
            }
            this.Attributes.Add("onmousemove", "SetTextCSS('" + this.ID + "',2);");
            this.Attributes.Add("onmouseout", "SetTextCSS('" + this.ID + "',1);");
        }
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            switch (_TextType)
            {
                case eType.LongDate:
                case eType.ShortDate:
                case eType.Time:
                    writer.Write("<input type=\"text\" id=\"" + this.ID + "\" onfocus=\"WdatePicker({isShowClear:true,readOnly:true,maxDate:'#F{$dp.$D\'%y-%M-%d\'}'})\" class=\"wheredate\" runat=\"server\" />");
                    break;
            }

            base.RenderEndTag(writer);
        }
    }
}