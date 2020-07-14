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
    /// Helper帮助类
    /// </summary>
    public sealed class Helper
    {
        /// <summary>
        /// Web请求
        /// </summary>
        /// <param name="serverUrl">地址</param>
        /// <param name="postStr">参数信息</param>
        /// <returns></returns>
        public static string HttpWebRequestByURL(string serverUrl, string postStr)
        {
            string result = string.Empty;
            byte[] postBin;
            HttpWebRequest request;
            HttpWebResponse response;
            Stream ioStream;
            postBin = Encoding.UTF8.GetBytes(postStr);//注意提交到的网站的编码，现在是gb2312的

            try
            {
                request = WebRequest.Create(serverUrl) as HttpWebRequest;
                request.Timeout = 50000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";

                request.Method = "post";
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;//不允许重定向
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
        /// 获取App.config文件Key的值
        /// </summary>
        /// <param name="strKey">Key名称</param>
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
        /// 添加一项AppSettting
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
        /// pingIP是否通
        /// </summary>
        /// <param name="IP">IP地址</param>
        /// <returns></returns>
        public static bool GetPingIPStatus(string IP)
        {
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(IP, 1000);//第一个参数为ip地址，第二个参数为ping的时间 
            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }


        #region 图像获取
        /// <summary>
        /// 图像转为字符串
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
        /// 字符串转换为图像
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
        /// 获取URL链接的图片
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
        /// 获取两个时间之间的时长
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
                strDuration = intDay.ToString() + "天";
            }
            if (intHour > 0)
            {
                strDuration += intHour.ToString() + "时";
            }
            if (intMM > 0)
            {
                strDuration += intMM.ToString() + "分";
            }
            if (intSS > 0)
            {
                strDuration += intSS.ToString() + "秒";
            }

            return strDuration;
        }


        /// <summary> 
        /// 转换人民币大小金额 
        /// </summary> 
        /// <param name="num">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            num = Math.Round(Math.Abs(num), 2, MidpointRounding.AwayFromZero);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
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
                            ch1 = "零" + str1.Substring(temp * 1, 1);
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
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
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
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /// <summary>
        /// 获取枚举项的描述
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
        /// bitmap转换为base64
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
        /// base64转bitmap
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
        /// 检查sql注入
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