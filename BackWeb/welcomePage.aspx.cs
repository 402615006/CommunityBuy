using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommunityBuy.BackWeb
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string machinename = "";
        public string userHostAddress = "";
        public string browser = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            machinename = Page.Server.MachineName;
            userHostAddress = Page.Request.UserHostAddress;
            browser = Request.Browser.Browser;

            //浏览器类型：Request.Browser.Browser;
            //浏览器标识：Request.Browser.Id;
            //浏览器版本号：Request.Browser.Version;

            string browserid = Request.Browser.Id;
            string browerVersion = Request.Browser.Version;
            string count = Session.Contents.Count.ToString();
            //bll.DownloadFullSynchronize("000");
            //bll.DownloadUpdateSynchronize("000", "Employee");
        }
    }
}