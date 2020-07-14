using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.manage
{
    public partial class ResetPwd : EditPage
    {
        bllAdmins bll = new bllAdmins();
        string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                SetPage();
            }
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage()
        {

            txt_uname.Text = base.LoginedUser.Name.ToString();
            this.tex_oldpwd.Text = "";
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string pwd =tex_oldpwd.Text;
            string newpwd =this.txt_pwd.Text;
            string newpwdconfirm =this.txt_repwd.Text;
            pwd = OEncryp.Encrypt(pwd);
            if (newpwd.Length < 6)
            {
                Script(this.Page, "pcLayerMsg('密码格式为6-16位数字或字母组合！');");
                return;
            }
            if (newpwd != newpwdconfirm)
            {
                Script(this.Page, "pcLayerMsg('两次输入密码不一致');");
                return;
            }
            newpwd = OEncryp.Encrypt(newpwd);
            bll.ResetPwd("0", "0", base.LoginedUser.UserID.ToString(),newpwd);
            Script(this.Page, "pcLayerMsg('" +bll.oResult.Msg + "');");
        }
    }
}