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

namespace CommunityBuy.BackWeb
{
    public partial class Login : System.Web.UI.Page
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
            string username = Helper.ReplaceString(UserName.Value);
            string password = Helper.ReplaceString(UserPwd.Value);
            string usercode = Helper.ReplaceString(UserCode.Value);

            if (username.Length == 0)
            {
                Message.InnerHtml = ErrMessage.GetMessageInfoByCode("admins_007").Body;
                return;
            }

            if (password.Length == 0)
            {
                Message.InnerHtml = ErrMessage.GetMessageInfoByCode("admins_010").Body;
                return;
            }

            if (usercode.Length == 0)
            {
                Message.InnerHtml = ErrMessage.GetMessageInfoByCode("admins_028").Body;
                return;
            }
            if (Session["ValidateCode"] == null)
            {
                Message.InnerHtml = ErrMessage.GetMessageInfoByCode("admins_029").Body;
                return;
            }
            if (Session["ValidateCodeDateTime"] == null)
            {
                Message.InnerHtml = ErrMessage.GetMessageInfoByCode("admins_029").Body;
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
                    Message.InnerHtml = ErrMessage.GetMessageInfoByCode("admins_029").Body;
                    return;
                }
            }

            if (code.ToLower() == usercode.ToLower())
            {
                CheckUser(username, password);
            }
            else
            {
                Message.InnerHtml = ErrMessage.GetMessageInfoByCode("admins_033").Body;
            }
        }

        private void CheckUser(string username, string password)
        {
            bllAdmins bll = new bllAdmins();

            string userpwd = OEncryp.Encrypt(password);
            DataTable dt = bll.LoginCheck(" WHERE (uname='" + username + "' or empid='" + username + "') AND upwd='" + userpwd + "'");

            if (dt != null && dt.Rows.Count > 0)
            {
                //赋值加入缓存
                LoginUserInfo loginUserInfo = new LoginUserInfo();
                loginUserInfo.Id = StringHelper.StringToLong(dt.Rows[0]["userid"].ToString());
                loginUserInfo.umobile = Convert.ToString(dt.Rows[0]["umobile"]);
                loginUserInfo.pwd = Convert.ToString(password);
                loginUserInfo.username = Convert.ToString(dt.Rows[0]["uname"]);
                loginUserInfo.empcode = Convert.ToString(dt.Rows[0]["empcode"]);
                loginUserInfo.cname = Convert.ToString(dt.Rows[0]["realname"]);
                loginUserInfo.status = Convert.ToString(dt.Rows[0]["status"]);
                loginUserInfo.Language = language.Value;
                loginUserInfo.Scope_ID = dt.Rows[0]["scope"].ToString();
                string empstocode = dt.Rows[0]["empstocode"].ToString();
                if (loginUserInfo.status == "1")
                {
                    ////获取角色信息
                    bllURRMAS urr = new bllURRMAS();
                    DataRow drUrr = urr.GetRoleInfoByMemID(loginUserInfo.Id);
                    if (drUrr != null)
                    {
                        loginUserInfo.Rol_ID = drUrr["roleid"].ToString();
                        loginUserInfo.Rol_Name = drUrr["cname"].ToString();
                        loginUserInfo.stocode = empstocode;
                        if (loginUserInfo.Scope_ID == "2")
                        {
                            loginUserInfo.rolstocode = empstocode;
                        }
                        else
                        {
                            loginUserInfo.rolstocode = dt.Rows[0]["stocode"].ToString();
                        }
                        loginUserInfo.LoginGuid = dt.Rows[0]["userid"].ToString();
                        loginUserInfo.buscode = dt.Rows[0]["buscode"].ToString();
                    }
                    LoginedUser.UserInfo = loginUserInfo;

                    //将登录信息写日志
                    try
                    {
                        tl_loginlogEntity login = new tl_loginlogEntity();
                        string IP = IPHelp.GetClientIP();
                        login.userid = loginUserInfo.Id;
                        login.strcode = empstocode;
                        login.ip = IP;
                        login.cname = loginUserInfo.cname;
                        login.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("admins_051").Body, loginUserInfo.cname, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        blltl_loginlog log = new blltl_loginlog();
                        log.Add(login);
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.WriteErrorMessage(ex);
                    }
                    //登陆信息存入缓存 slw
                    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script type='text/javascript'>set_loginfo('logininfo'," + loginUserInfo + ");</script>", true);
                    //this.Page.Response.Write("<script type='text/javascript'>set_loginfo('logininfo'," + loginUserInfo + ");</script>");
                    dt.Columns.Add("GUID", typeof(string));
                    dt.Rows[0]["GUID"] = Guid.NewGuid().ToString();
                    string appJson = JsonHelper.DataTableToJSON(dt);
                    appJson = appJson.TrimStart('[');
                    appJson = appJson.TrimEnd(']');
                    Response.Redirect("/LAYUIHTML/login2.html?user=" + appJson);
                    //Response.Redirect("index.html");
                }
                else
                {
                    Message.InnerHtml = ErrMessage.GetMessageInfoByCode("admins_031").Body;
                }
            }
            else
            {
                Message.InnerHtml = ErrMessage.GetMessageInfoByCode("admins_032").Body;
            }
        }
    }
}