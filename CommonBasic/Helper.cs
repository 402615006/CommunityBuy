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
    public sealed class Helper
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
        /// ��ȡApp.config�ļ�Key��ֵ
        /// </summary>
        /// <param name="strKey">Key����</param>
        /// <returns>Value</returns>
        public static string GetAppSettings(string strKey)
        {
            try
            {
                string strValue = System.Configuration.ConfigurationManager.AppSettings[strKey];
                if (strValue != null)
                    return strValue.ToString();
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// ���һ��AppSettting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSetting(string key, string value)
        {
            XmlDocument xmlDoc = new System.Xml.XmlDocument();
            string FullFilePath = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName + ".config";
            if (File.Exists(FullFilePath))
            {
                xmlDoc.Load(FullFilePath);
                XmlNode AppSettingNode = xmlDoc.SelectNodes("configuration/appSettings")[0];
                XmlElement AddElement = xmlDoc.CreateElement("add");
                AddElement.SetAttribute("key", key);
                AddElement.SetAttribute("value", value);
                AppSettingNode.AppendChild(AddElement);
                xmlDoc.Save(FullFilePath);
            }
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


        #region ͼ���ȡ
        /// <summary>
        /// ͼ��תΪ�ַ���
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string ImageToString(System.Drawing.Image img)
        {
            if (img == null) return string.Empty;
            string strImg = string.Empty;
            MemoryStream st = new MemoryStream();
            img.Save(st, System.Drawing.Imaging.ImageFormat.Jpeg);
            strImg = Convert.ToBase64String(st.ToArray());
            return strImg;
        }

        /// <summary>
        /// �ַ���ת��Ϊͼ��
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static System.Drawing.Image StringToImage(string base64Str)
        {
            System.Drawing.Bitmap bitmap = null;
            System.Drawing.Image img = null;
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = Convert.FromBase64String(base64Str);
                ms.Write(buffer, 0, buffer.Length);
                try
                {
                    img = System.Drawing.Image.FromStream(ms);
                    if (img != null)
                    {
                        bitmap = new System.Drawing.Bitmap(img.Width, img.Height);
                        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                        {
                            g.DrawImage(img, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height));
                        }
                    }
                }
                catch { }
            }
            return bitmap;
        }

        /// <summary>
        /// ��ȡURL���ӵ�ͼƬ
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static System.Drawing.Image GetImageFromURL(string URL)
        {
            System.Drawing.Image image = null;
            try
            {
                Random seed = new Random();
                WebRequest webreq = WebRequest.Create(URL);
                WebResponse webres = webreq.GetResponse();
                Stream stream = webres.GetResponseStream();
                image = System.Drawing.Image.FromStream(stream);
                stream.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
            }
            return image;

        }

        #endregion

        /// <summary>
        /// ��ȡ����ʱ��֮���ʱ��
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public static string getDuration(DateTime timeStart, DateTime timeEnd)
        {
            string strDuration = string.Empty;
            int intDay = 0;
            int intHour = 0;
            int intMM = 0;
            int intSS = 0;

            TimeSpan ts = timeEnd.Subtract(timeStart).Duration();
            intDay = ts.Days;
            intHour = ts.Hours;
            intMM = ts.Minutes;
            intSS = ts.Seconds;

            if (intDay > 0)
            {
                strDuration = intDay.ToString() + "��";
            }
            if (intHour > 0)
            {
                strDuration += intHour.ToString() + "ʱ";
            }
            if (intMM > 0)
            {
                strDuration += intMM.ToString() + "��";
            }
            if (intSS > 0)
            {
                strDuration += intSS.ToString() + "��";
            }

            return strDuration;
        }


        /// <summary> 
        /// ת������Ҵ�С��� 
        /// </summary> 
        /// <param name="num">���</param> 
        /// <returns>���ش�д��ʽ</returns> 
        public static string CmycurD(decimal num)
        {
            string str1 = "��Ҽ��������½��ƾ�";            //0-9����Ӧ�ĺ��� 
            string str2 = "��Ǫ��ʰ��Ǫ��ʰ��Ǫ��ʰԪ�Ƿ�"; //����λ����Ӧ�ĺ��� 
            string str3 = "";    //��ԭnumֵ��ȡ����ֵ 
            string str4 = "";    //���ֵ��ַ�����ʽ 
            string str5 = "";  //����Ҵ�д�����ʽ 
            int i;    //ѭ������ 
            int j;    //num��ֵ����100���ַ������� 
            string ch1 = "";    //���ֵĺ������ 
            string ch2 = "";    //����λ�ĺ��ֶ��� 
            int nzero = 0;  //����������������ֵ�Ǽ��� 
            int temp;            //��ԭnumֵ��ȡ����ֵ 

            num = Math.Round(Math.Abs(num), 2, MidpointRounding.AwayFromZero);    //��numȡ����ֵ����������ȡ2λС�� 
            str4 = ((long)(num * 100)).ToString();        //��num��100��ת�����ַ�����ʽ 
            j = str4.Length;      //�ҳ����λ 
            if (j > 15) { return "���"; }
            str2 = str2.Substring(15 - j);   //ȡ����Ӧλ����str2��ֵ���磺200.55,jΪ5����str2=��ʰԪ�Ƿ� 

            //ѭ��ȡ��ÿһλ��Ҫת����ֵ 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //ȡ����ת����ĳһλ��ֵ 
                temp = Convert.ToInt32(str3);      //ת��Ϊ���� 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //����ȡλ����ΪԪ�����ڡ������ϵ�����ʱ 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "��" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //��λ�����ڣ��ڣ���Ԫλ�ȹؼ�λ 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "��" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //�����λ����λ��Ԫλ�������д�� 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //���һλ���֣�Ϊ0ʱ�����ϡ����� 
                    str5 = str5 + '��';
                }
            }
            if (num == 0)
            {
                str5 = "��Ԫ��";
            }
            return str5;
        }

        /// <summary>
        /// ��ȡö���������
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];//DescriptionAttribute
            return da.Description;
        }

        /// <summary>
        /// bitmapת��Ϊbase64
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static string BitmapToBase64(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return strbaser64;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// base64תbitmap
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static Bitmap Base64StringToBitmap(string inputStr)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(inputStr);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// ���sqlע��
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static bool CheckSqlInjection(string inputString)
        {
            string SqlStr = @"and|or|exec|execute|insert|select|delete|update|alter|create|drop|count|\*|chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
            try
            {
                if ((inputString != null) && (inputString != String.Empty))
                {
                    string str_Regex = @"\b(" + SqlStr + @")\b";

                    Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                    //string s = Regex.Match(inputString).Value; 
                    if (true == Regex.IsMatch(inputString))
                        return false;

                }
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}