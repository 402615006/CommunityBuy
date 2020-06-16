using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Crwaler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetWebBrowserFeatures();
            this.webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = (WebBrowser)sender;
            string htmlString = browser.Document.Body.InnerHtml;
        }

        /// <summary>
        /// 解析页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //获取章节名称
            string chapterName = webBrowser1.Document.Body.GetElementsByTagName("h1")[0].InnerText;
            txtChaperName.Text = chapterName;
            //获取图片的html，分析正则表达式
            StringBuilder sbImages = new StringBuilder();
            IEnumerator enumerator = webBrowser1.Document.Body.GetElementsByTagName("img").GetEnumerator();
            while (enumerator.MoveNext())
            {
                HtmlElement imgElement = enumerator.Current as HtmlElement;
                string imgStr =imgElement.OuterHtml.ToString();
                sbImages.AppendLine(imgStr);
            }
            txtImages.Text = sbImages.ToString();
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this.folderBrowserDialog1.SelectedPath.Trim() != "")
                {
                    string pathName = this.folderBrowserDialog1.SelectedPath +"\\"+ this.txtChaperName.Text;
                    Regex regHtml = new Regex("(?<img><img\\s.*?data-src=\"(?<url>.*?)\">)", RegexOptions.Compiled);
                    int i = 0;
                    MatchCollection matchCollection = regHtml.Matches(txtImages.Text);
                    progressBar1.Maximum = matchCollection.Count;
                    //显示下载进度
                    lblImagNums.Text = i.ToString()+"/" + matchCollection.Count.ToString();
                    foreach (Match match in matchCollection)
                    {
                        string downloadurl = match.Groups["url"].Value;
                        if (!string.IsNullOrWhiteSpace(downloadurl))
                        {
                            i++;
                            DownImage(downloadurl, pathName, i.ToString()+ ".jpg");
                            lblImagNums.Text = i.ToString() + "/" + matchCollection.Count.ToString();
                            progressBar1.Value = i;
                            lblImagNums.Refresh();
                            progressBar1.Refresh();
                        }
                    }
                    
                }  
            }
        }

        /// <summary>
        /// 设置浏览器版本为IE11
        /// </summary>
        static void SetWebBrowserFeatures()
        {
            // don't change the registry if running in-proc inside Visual Studio  
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
            //获取程序及名称  
            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //得到浏览器的模式的值  
            UInt32 ieMode = 11000;
            var featureControlRegKey = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\";
            //设置浏览器对应用程序（appName）以什么模式（ieMode）运行  
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION",
                appName, ieMode, RegistryValueKind.DWord);
            // enable the features which are "On" for the full Internet Explorer browser  
            //不晓得设置有什么用  
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION",
                appName, 1, RegistryValueKind.DWord);
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            this.webBrowser1.Navigate(url);
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        private void DownImage(string imageUrl, string directoryName, string name)
        {
            try
            {
                string filePath = ConfigurationManager.AppSettings["savepath"] + directoryName;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileName = filePath + "\\" + name;

                HttpWebRequest request = HttpWebRequest.Create(imageUrl) as HttpWebRequest;
                HttpWebResponse response = null;
                response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode != HttpStatusCode.OK)
                    return;
                Stream reader = response.GetResponseStream();
                FileStream writer = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buff = new byte[512];
                int c = 0; //实际读取的字节数
                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                {
                    writer.Write(buff, 0, c);
                }
                writer.Close();
                writer.Dispose();
                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
