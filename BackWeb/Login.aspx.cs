using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
using CommunityBuy.BackWeb.Common;
using System.Web.UI;

namespace CommunityBuy.BackWeb
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ImportDishes();
            if (!IsPostBack)
            {
                
            }
        }

        protected void CheckLogin_Click(object sender, EventArgs e)
        {
            string username =UserName.Value;
            string password = UserPwd.Value;
            string usercode =UserCode.Value;
            if (username.Length == 0)
            {
                Message.InnerHtml = " 用户名不能为空";
                return;
            }
            if (password.Length == 0)
            {
                Message.InnerHtml = "密码不能为空!";
                return;
            }
            if (usercode.Length == 0)
            {
                Message.InnerHtml = "验证码不能为空!";
                return;
            }
            if (Session["ValidateCode"] == null)
            {
                Message.InnerHtml = "验证码失效!";
                return;
            }
            if (Session["ValidateCodeDateTime"] == null)
            {
                Message.InnerHtml = "验证码失效!";
                return;
            }
            string code = string.Empty;
            DateTime dtime;
            if (Session["ValidateCode"] != null)
            {
                code = Session["ValidateCode"].ToString();
            }
            if (Session["ValidateCodeDateTime"] != null)
            {
                dtime = StringHelper.StringToDateTime(Session["ValidateCodeDateTime"].ToString());
                if ((DateTime.Now - dtime).Minutes > 2)
                {
                    Message.InnerHtml = "验证码失效!";
                    return;
                }
            }

            if (code.ToLower() == usercode.ToLower())
            {
                CheckUser(username, password);
            }
            else
            {
                Message.InnerHtml = "验证码不正确!";
            }
        }

        private void CheckUser(string username, string password)
        {
            bllAdmins bll = new bllAdmins();
            string userpwd = OEncryp.Encrypt(password);
            AdminsEntity adminEntity = bll.GetEntitySigInfo(" WHERE uname='" + username + "' AND upwd='" + userpwd + "'");
            if (adminEntity.userid>0)
            {
                LoginedUserEntity loginedUser = new LoginedUserEntity(adminEntity.userid.ToString(), adminEntity.upwd, adminEntity.uname, adminEntity.umobile);
                DataTable dtPermission=bll.GetPermissionInfo(loginedUser.GUID, loginedUser.UserID, loginedUser.UserID);
                if(dtPermission!=null&&dtPermission.Rows.Count>0)
                {
                    loginedUser.Permission = dtPermission;
                    loginedUser.SetLoginCookie();
                    //string appJson = JsonHelper.ObjectToJSON(loginedUser);
                    Context.Cache.Insert("logincache_"+loginedUser.UserID, loginedUser);
                    loginedUser.SetLoginCookie();
                    Response.Redirect("index.html");
                }
                else
                {
                    Message.InnerHtml = "您的帐号无效，请联系管理员！";
                }
            }
            else
            {
                Message.InnerHtml = "用户或密码错误！";
            }
        }
    }
}