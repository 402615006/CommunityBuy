using System;
using System.Data;
using System.Web.UI;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.WebControl;
using CommunityBuy.Model;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Security;
using System.Runtime.CompilerServices;
using System.Web.UI.WebControls;
using NPOI.HPSF;
using NPOI.HSSF.Util;

namespace CommunityBuy.BackWeb.Common
{
    public class LoginedUserEntity
    {

        public string UserID { get; set; }
        public string Pwd { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public DataTable Permission { get; set; }
        public string GUID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CookieData"></param>
        /// <returns></returns>
        public LoginedUserEntity()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CookieData"></param>
        /// <returns></returns>
        public LoginedUserEntity(string userid,string pwd,string name,string mobile)
        {
            this.UserID = userid;
            this.Pwd = pwd;
            this.Name = name;
            this.Mobile = mobile;
            this.GUID =Guid.NewGuid().ToString();
        }

        public void LoginFromCookie()
        {
            string CookieData = GetFromCookieData();
            if (!string.IsNullOrWhiteSpace(CookieData))
            {
                string[] DataList = CookieData.Split('|');
                this.UserID = DataList[0];
                this.Pwd = DataList[1];
                this.Name = DataList[2];
                this.Mobile = DataList[3];
                this.GUID = DataList[5];
            }
        }

        /// <summary>
        /// 用户登录信息
        /// </summary>
        public void SetLoginCookie()
        {
            HttpCookie hcCurrent = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (hcCurrent != null)
            {
                HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            }
            string sData =this.UserID+"|"+ this.Pwd + "|"+this.Name + "|"+this.Mobile + "|"+this.UserID + "|"+this.GUID;
            sData = OEncryp.Encrypt(sData);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(this.GUID, false);
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(authTicket.Version, authTicket.Name, authTicket.IssueDate, authTicket.IssueDate.AddMinutes(360), authTicket.IsPersistent, sData, authTicket.CookiePath);
            authCookie.Value = FormsAuthentication.Encrypt(newTicket);
            authCookie.Secure = false;
            authCookie.Expires = newTicket.Expiration;
            authCookie.Domain = FormsAuthentication.CookieDomain;
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }
        public string GetFromCookieData()
        {
                string sCookieName = FormsAuthentication.FormsCookieName;
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[sCookieName];
                if (authCookie == null)
                {
                    //跳转到登录页
                    try
                    {
                        System.Web.UI.Page page = (System.Web.UI.Page)System.Web.HttpContext.Current.Handler;
                        if (page != null)
                        {
                            page.Response.Write("<script> top.location.href='" + Helper.GetAppSettings("HomePageUrl") + "';</script>");
                        }
                        return null;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
                FormsAuthenticationTicket authTicket = null;
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket == null)
                {
                    HttpContext.Current.Response.Clear();
                    FormsAuthentication.RedirectToLoginPage();
                    HttpContext.Current.Response.End();
                    return null;
                }

                if (!authTicket.Expired && FormsAuthentication.SlidingExpiration)
                {
                    FormsAuthenticationTicket newTicket = FormsAuthentication.RenewTicketIfOld(authTicket);
                    if (authTicket.Expiration != newTicket.Expiration)
                    {
                        string encTicket = FormsAuthentication.Encrypt(newTicket);
                        authCookie.Value = encTicket;
                        authCookie.Expires = newTicket.Expiration;
                        authCookie.Domain = FormsAuthentication.CookieDomain;
                        HttpContext.Current.Response.Cookies.Remove(sCookieName);
                        HttpContext.Current.Response.Cookies.Add(authCookie);
                    }
                }
                string sData = authTicket.UserData;
                sData = OEncryp.Decrypt(sData);
                return sData;
            }
     }
    public class BasePage : System.Web.UI.Page
    {

        Cache WebCache = HttpContext.Current.Cache;
        public LoginedUserEntity LoginedUser = null;

        /// <summary>
        /// 检测cookie是否过期
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            LoginedUser = new LoginedUserEntity();
            LoginedUser.LoginFromCookie();
            if (!string.IsNullOrWhiteSpace(LoginedUser.UserID))
            {
                LoginedUser =(LoginedUserEntity)Context.Cache.Get("logincache_"+LoginedUser.UserID);
            }
            //判断cookie是否过期
            if (LoginedUser == null)
            {
                Response.Write("<script> top.location.href='" + Helper.GetAppSettings("HomePageUrl") + "';</script>");
                return;
            }
            CheckPageAuthorization();
            base.OnInit(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// 检测用户页面访问权限
        /// </summary>
        protected void CheckPageAuthorization()
        {
            //获取页面Code
            CPathBar pathBar = (CPathBar)Page.FindControl("pathBar");
            if (pathBar == null)
            {
                Response.Write("<script> top.location.href='" + Helper.GetAppSettings("NoAuthorizationPageUrl") + "';</script>");
                return;
            }
            string Ptype = pathBar.PageType.ToString().ToLower();
            if (Ptype == "normal" || Ptype == "referer")//不验证权限
            {
                return;
            }

            if (WebCache.Get("RoleInfo_BackWeb_" + LoginedUser.UserID)!=null)
            {
                LoginedUser = (LoginedUserEntity)WebCache.Get("RoleInfo_BackWeb_" + LoginedUser.UserID);
                //获取一级菜单
                if (LoginedUser.Permission.Rows.Count== 0)
                {
                    Response.Write("<script> top.location.href='" + Helper.GetAppSettings("NoAuthorizationPageUrl") + "';</script>");
                    return;
                }
            }
        }

        /// <summary>
        /// 执行JS代码
        /// </summary>
        /// <param name="page"></param>
        /// <param name="msg"></param>
        public static void Script(Page page, string msg)
        {
            ScriptManager.RegisterStartupScript(page, typeof(Page), System.DateTime.Now.Ticks.ToString(), msg, true);
        }

        public static void Script(Control control, string msg)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), System.DateTime.Now.Ticks.ToString(), msg, true);
        }

        /// <summary>
        /// 显示操作结果
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="errormessage"></param>
        protected bool ShowResult(string code,string msg, HtmlGenericControl errormessage)
        {
            bool Flag = false;
            switch (code)
            {
                case "1"://成功
                    Flag = true;
                    errormessage.InnerHtml = msg;
                    break;
                case "-2"://参数错误

                    break;
                case "2"://其他错误
                    errormessage.InnerHtml = msg;
                    break;
                default:
                    errormessage.InnerHtml = "无法链接到服务器，请检查网络";
                    break;
            }
            return Flag;
        }

        /// <summary>
        /// 绑定数据到DDL
        /// </summary>
        /// <param name="DDL"></param>
        /// <param name="dtData"></param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        /// <param name="SelectOrALL"></param>
        protected  static void BindDropDownListInfo(DropDownList DDL, DataTable dtData, string TextField, string ValueField, int SelectOrALL)
        {
            if (dtData != null)
            {
                try
                {
                    if (DDL != null)
                    {
                        DDL.Items.Clear();
                        DDL.DataSource = dtData;
                        DDL.DataTextField = TextField;
                        DDL.DataValueField = ValueField;
                        DDL.DataBind();
                    }
                }
                catch
                { }
            }
            switch (SelectOrALL)
            {
                case 1:
                    DDL.Items.Insert(0, new ListItem("--无--", ""));
                    break;
                case 2:
                    DDL.Items.Insert(0, new ListItem("--全部--", ""));
                    break;
                case 3:
                    DDL.Items.Insert(0, new ListItem("--全部--", "0"));
                    break;
            }
        }
    }
}