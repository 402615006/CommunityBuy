using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.memberCard
{
    public partial class membersPhoneEdit : EditPage
    {
        public string memcode;
        bllmembers bll = new bllmembers();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    memcode = Request["id"].ToString().Trim(',');
                    hidId.Value = memcode;
                    SetPage(hidId.Value);
                    this.PageTitle.Operate = 修改
                }
            }
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where memcode='" + id + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                string mobile = dr["mobile"].ToString();
                hidoldphone.Value = mobile;
                if (mobile.Length == 11)
                {
                    mobile = mobile.Substring(0, 3) + "****" + mobile.Substring(7);
                }
                txt_oldphone.Text = dr["mobile"].ToString();
                if (mobile.Length <= 0)
                {
                    Script(this.Page, "hideold();");
                }
            }
            txt_oname.InnerText = LoginedUser.UserInfo.cname + "(" + LoginedUser.UserInfo.empcode + ")";
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string newphone = Helper.ReplaceString(txt_newphone.Text);
            string oldphone = Helper.ReplaceString(hidoldphone.Value);
            string img1 = hid_topImg1.Value;
            string img2 = hid_topImg2.Value;
            string newCode = Helper.ReplaceString(txt_newYZM.Text);
            string oldCode = Helper.ReplaceString(txt_oldYZM.Text);
            if (hidSFZ.Value != "1")//旧手机可以接收短信
            {
                if (oldphone.Length > 0)
                {
                    if (Session["code" + oldphone] == null || oldCode != Session["code" + oldphone].ToString())
                    {
                        errormessage.InnerHtml = "旧手机验证码验证失败";
                        return;
                    }
                }
            }
            else//旧手机不能接收短信
            {
                if (string.IsNullOrWhiteSpace(img1))
                {
                    errormessage.InnerHtml = "请上传身份证正面";
                    return;
                }
                if (string.IsNullOrWhiteSpace(img2))
                {
                    errormessage.InnerHtml = "请上传身份证背面";
                    return;
                }
            }
            if (Session["code" + newphone] == null || newCode != Session["code" + newphone].ToString())
            {
                errormessage.InnerHtml = "新手机验证码验证失败";
                return;
            }
            //Session.Remove("code" + newphone);
            //Session.Remove("code" + oldphone);
            //日志信息
            memberslogEntity entity = new memberslogEntity();
            entity.operatetype = "修改";
            entity.memcode = hidId.Value;
            entity.oname = LoginedUser.UserInfo.cname.ToString();
            entity.ocode = LoginedUser.UserInfo.Id.ToString();
            entity.logcontext = "修改memcode为" + hidId.Value + "的手机号信息：" + oldphone + "->" + newphone + ",附件：" + img1 + ";" + img2;
            entity.stocode = LoginedUser.UserInfo.stocode;

            DataTable dt = new DataTable();
            dt = bll.UpdatePhone(LoginedUser.UserInfo.LoginGuid, LoginedUser.UserInfo.Id.ToString(), hidId.Value, newphone, LoginedUser.UserInfo.Id.ToString(), entity);
            this.PageTitle.Operate = 修改
            //显示结果
            ShowResult(dt, errormessage);
            savebtn1.Visible = false;
            Script(this.Page, "refresh();");
        }

    }
}