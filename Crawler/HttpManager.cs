using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Crawler
{
    class HttpManager
    {
        private WebClient webClient;

        public HttpManager()
        {
            webClient = new WebClient();
        }

        public void GetHttpString(string url)
        {
            //string htmlString = webClient.DownloadString(url);
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.Navigate(url);
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
            //return htmlString;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = (WebBrowser)sender;
            string htmlString=browser.Document.Body.InnerHtml;
        }

        public void SaveHttpFile(string url, string directoryName, string name)
        {
            string filePath = ConfigurationManager.AppSettings["savepath"] + directoryName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fileName = filePath + "\\" + name;
            webClient.DownloadString(url);
        }

    }
}
