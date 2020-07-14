using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace CommunityBuy.WebControl
{
    [ToolboxData("<{0}:CButton runat=server></{0}:CButton>")]
    sealed public class CButton : Button
    {
        public CButton()
		{
            this.Init += new EventHandler(CButton_Init);
        }
        private bool _IsFormValidation = true;
        [Browsable(true), Category("自定义属性"), Description("是否验证表单数据，默认验证表单数据。")]
        public bool IsFormValidation
        {
            get { return _IsFormValidation; }
            set { _IsFormValidation = value; }
        }

        private bool _IsSaveAdd = true;
        [Browsable(true), Category("自定义属性"), Description("是否保存并新建按钮")]
        public bool IsSaveAdd
        {
            get { return _IsSaveAdd; }
            set { _IsSaveAdd = value; }
        }

        private void CButton_Init(object sender, EventArgs e)
        {
            if (_IsFormValidation)
            {
                this.OnClientClick = "return FormDataValidationCheck();";
            }
            else
            {
                if (_IsSaveAdd)
                {
                    this.OnClientClick = " $('#" + this.ID + "').click();";
                }
            }
        }

        /*
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (_IsFormValidation)
            {
                //writer.Write("<input type=\"submit\" name=\"" + this.ID + "\" value=\"\" onclick=\"return FormDataValidationCheck();\" id=\"" + this.ID + "\" style=\"display: none\" />");
            }
            if (_IsSaveAdd)
            {
                writer.Write("<input type=\"submit\" data-isadd=\"1\" name=\"" + this.ID + "Add\" value=\"\" onclick=\"return FormDataSaveAdd('" + this.ID + "');\" id=\"" + this.ID + "Add\" style=\"display: none\" />");
                writer.Write("<script>AddSaveAddbutton('" + this.ID + "');</script>");
            }
           
            base.RenderEndTag(writer);
        }
        */
    }
}
