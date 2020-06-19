using System;
using System.Data;
using System.Web.UI;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using Sam.WebControl;
using CommunityBuy.Model;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Security;
using System.Runtime.CompilerServices;

namespace CommunityBuy.BackWeb.Common
{
    public class LoginedUserEntity
    {

        public string UserID { get; set; }
        public string Pwd { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Roleids { get; set; }
        public string Functions { get; set; }
        public string GUID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CookieData"></param>
        /// <returns></returns>
        public LoginedUserEntity()
        {
            string CookieData = GetFromCookieData();
            string[] DataList = CookieData.Split('|');
            this.UserID = DataList[0];
            this.Pwd = DataList[1];
            this.Name = DataList[2];
            this.Mobile = DataList[3];
            this.Roleids = DataList[4];
            this.Functions = DataList[5];
            this.GUID = DataList[6];
        }

        /// <summary>
        /// 用户登录信息
        /// </summary>
        private void SetLoginCookie()
        {

            HttpCookie hcCurrent = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (hcCurrent != null)
            {
                HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            }
            string sData =this.UserID+"|"+ this.Pwd + "|"+this.Name + "|"+this.Mobile + "|"+this.UserID + "|"+this.Roleids + "|"+this.Functions + "|"+this.GUID;
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
        private string GetFromCookieData()
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
        LoginedUserEntity LoginedUser = new LoginedUserEntity();

        /// <summary>
        /// 检测cookie是否过期
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
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

            string Fcode = pathBar.PageCode;
            if (Fcode == "StockSemiProOutlist")
            {
                if (Request["type"] == "1")
                {
                    Fcode = "StockSemiProOutlist";
                }
                else if (Request["type"] == "2")
                {
                    Fcode = "StockSalelist";
                }
                else
                {
                    Fcode = "StockWineOutList";
                }
            }
            else if (Fcode == "StockSemiPro")
            {
                if (Request["type"] == "1")
                {
                    Fcode = "StockSemiProlist";
                }
                else
                {
                    Fcode = "StockWineList";
                }
            }
            string Ptype = pathBar.PageType.ToString().ToLower();
            if (Ptype == "normal" || Ptype == "referer")//不验证权限
            {
                return;
            }
            int intCount;
            int intPagenums;
            bllFUNMAS _bll = new bllFUNMAS();
            DataTable dt = new DataTable();
            if (WebCache.Get("RoleInfo_BackWeb_" + LoginedUser)!=null)
            {
                dt = (DataTable)WebCache.Get("RoleInfo_BackWeb_" + LoginedUser);
            }
            else
            {
                dt = _bll.GetPagingInfo(10000, 1, "ftype=1 and id in(select funid from rolefunction where  roleid in(" + LoginedUser.Roleids + "))", " order by orders asc", out intCount, out intPagenums);
                WebCache.Insert("RoleInfo_BackWeb_" + LoginedUser, dt);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                //获取一级菜单
                DataRow[] drs = dt.Select(" code='" + Fcode + "'");
                if (drs.Length == 0)
                {
                    Response.Write("<script> top.location.href='" + Helper.GetAppSettings("NoAuthorizationPageUrl") + "';</script>");
                    return;
                }
                switch (Ptype)
                {
                    case "add":
                    case "edit":
                    case "found":
                    case "sendbatchcard":  //批量发卡
                        drs = dt.Select(" code='" + Fcode + "' and btnname='" + Ptype.ToString() + "'");
                        if (drs.Length == 0)
                        {
                            Response.Write("<script> top.location.href='" + Helper.GetAppSettings("NoAuthorizationPageUrl") + "';</script>");
                            return;
                        }
                        break;
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

    }
}