using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.WebControl;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CommunityBuy.BackWeb.Common
{
    public class EditPage : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            if (Request["id"] != null)
            {
                CPathBar pathBar = (CPathBar)Page.FindControl("pathBar");
                //pathBar.PageType =CommunityBuy.WebControl.CPathBar.PType.Edit;
            }
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


        /// <summary>
        /// 控件禁用
        /// </summary>
        /// <param name="page"></param>
        public static void SetControlReadOnly(Page page)
        {

            foreach (Control ctrl in page.Controls)
            {
                SetControlReadOnly(ctrl);

            }
        }

        public static void SetControlReadOnly(Control ctr)
        {
            if (ctr is TextBox)
            {
                TextBox txtControl = (TextBox)ctr;
                txtControl.ReadOnly = true;
                txtControl.Enabled = false;

            }
            else if (ctr is Button)
            {
                Button btn = (Button)ctr;
                btn.Visible = false;

            }
            else if (ctr is RadioButton)
            {
                RadioButton btn = (RadioButton)ctr;
                btn.Enabled = false;

            }
            else if (ctr is RadioButtonList)
            {
                RadioButtonList btn = (RadioButtonList)ctr;
                btn.Enabled = false;
            }

            else if (ctr is CheckBox)
            {
                CheckBox cb = (CheckBox)ctr;
                cb.Enabled = false;
            }
            else if (ctr is DropDownList)
            {
                DropDownList list = (DropDownList)ctr;
                list.Enabled = false;
            }

            else if (ctr is HtmlTextArea)
            {
                HtmlTextArea cb = (HtmlTextArea)ctr;
                cb.Attributes.Add("readonly", "");
                cb.Disabled = true;
            }
            else if (ctr is HtmlSelect)
            {
                HtmlSelect rb = (HtmlSelect)ctr;
                rb.Disabled = true;
            }

            else if (ctr is HtmlInputCheckBox)
            {
                HtmlInputCheckBox rb = (HtmlInputCheckBox)ctr;
                rb.Disabled = true;
            }
            else if (ctr is HtmlInputRadioButton)
            {
                HtmlInputRadioButton rb = (HtmlInputRadioButton)ctr;
                rb.Disabled = true;
            }
            else if (ctr is HtmlInputText)
            {
                HtmlInputControl input = (HtmlInputControl)ctr;
                input.Attributes.Add("readonly", "");
                input.Disabled = true;
            }
            else
                foreach (Control ctr1 in ctr.Controls)
                {
                    SetControlReadOnly(ctr1);
                }
        }

    }
}

