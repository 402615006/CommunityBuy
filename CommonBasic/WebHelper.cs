using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.CommonBasic
{
    /// <summary>
    /// Helper������
    /// </summary>
    public sealed class WebHelper
    {
        /// <summary>
        /// Web����
        /// </summary>
        /// <param name="serverUrl">��ַ</param>
        /// <param name="postStr">������Ϣ</param>
        /// <returns></returns>
        public static string HttpWebRequestByURL(string serverUrl, string postStr)
        {
            string result = string.Empty;
            byte[] postBin;
            HttpWebRequest request;
            HttpWebResponse response;
            Stream ioStream;
            postBin = Encoding.UTF8.GetBytes(postStr);//ע���ύ������վ�ı��룬������gb2312��

            try
            {
                request = WebRequest.Create(serverUrl) as HttpWebRequest;
                request.Timeout = 50000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";

                request.Method = "post";
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;//�������ض���
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBin.Length;

                ioStream = request.GetRequestStream();
                ioStream.Write(postBin, 0, postBin.Length);
                ioStream.Flush();
                ioStream.Close();
                response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, serverUrl + ";" + postStr.ToString());
            }
            return result;
        }

        /// <summary>
        /// pingIP�Ƿ�ͨ
        /// </summary>
        /// <param name="IP">IP��ַ</param>
        /// <returns></returns>
        public static bool GetPingIPStatus(string IP)
        {
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(IP, 1000);//��һ������Ϊip��ַ���ڶ�������Ϊping��ʱ�� 
            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ִ��JS�ű�
        /// </summary>
        /// <param name="Mes"></param>
        public static void WriteScriptMessage(string Mes, System.Web.UI.Page page)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "PageScript", "<script type='text/javascript'>" + Mes + "</script>");
        }

    }
}