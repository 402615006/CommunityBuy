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
        /// 获取中英文混排字符串的实际长度(字节数)
        /// </summary>
        /// <param name="str">要获取长度的字符串</param>
        /// <returns>字符串的实际长度值（字节数）</returns>
        public static int getStringLength(string str)
        {
            if (str.Equals(string.Empty))
                return 0;
            int strlen = 0;
            ASCIIEncoding strData = new ASCIIEncoding();
            //将字符串转换为ASCII编码的字节数字
            byte[] strBytes = strData.GetBytes(str);
            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                    strlen++;
                strlen++;
            }
            return strlen;
        }

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


        #region 字符串格式转换
        /// <summary>
        /// SQL字符串过滤
        /// </summary>
        /// <param name="Text">待过滤的内容</param>
        /// <returns>返回：过滤后字符串</returns>
        public static string ReplaceString(string Text)
        {
            if (Text != null)
            {
                return Text.Trim().Replace("'", "‘").Replace(@"\", @"");
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 字符串转Int
        /// </summary>
        /// <param name="Text">带转换的内容</param>
        /// <returns>返回：Int结果，非法返回0</returns>
        public static int StringToInt(string Text)
        {
            if (Text != null)
            {
                int temp;
                int.TryParse(Text, out temp);

                #region 处理字符串是1.00是返回来是0 tsg 2017-05-06
                if (temp == 0)
                {
                    try
                    {
                        string[] arrText = Text.Split('.');
                        int iResult = Convert.ToInt32(arrText[0]);
                        if (iResult > 0)
                        {
                            temp = iResult;
                        }
                    }
                    catch { }
                }
                #endregion

                return temp;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 字符串转Long
        /// </summary>
        /// <param name="Text">带转换的内容</param>
        /// <returns>返回：Long结果，非法返回0</returns>
        public static long StringToLong(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                string value = System.Text.RegularExpressions.Regex.Replace(Text, @"[^\d]*", "");
                long temp;
                long.TryParse(value, out temp);
                return temp;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 字符串转Double
        /// </summary>
        /// <param name="Text">带转换的内容</param>
        /// <returns>返回：Double结果，非法返回0</returns>
        public static double StringToDouble(string Text)
        {
            if (Text != null)
            {
                double temp;
                double.TryParse(Text, out temp);
                return temp;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 字符串转Decimal
        /// </summary>
        /// <param name="Text">带转换的内容</param>
        /// <returns>返回：Decimal结果，非法返回0</returns>
        public static decimal StringToDecimal(string Text)
        {
            if (Text != null)
            {
                decimal temp;
                decimal.TryParse(Text, out temp);
                return temp;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 字符串转Float
        /// </summary>
        /// <param name="Text">带转换的内容</param>
        /// <returns>返回：Float结果，非法返回0</returns>
        public static float StringToFloat(string Text)
        {
            if (Text != null)
            {
                float temp;
                float.TryParse(Text, out temp);
                return temp;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 字符串转DateTime
        /// </summary>
        /// <param name="Text">带转换的内容</param>
        /// <returns>返回：DateTime结果，非法返回1900-01-01</returns>
        public static DateTime StringToDateTime(string Text)
        {
            if (Text != null && Text.Length > 0)
            {
                DateTime temp;
                DateTime.TryParse(Text, out temp);
                return temp;
            }
            else
            {
                return DateTime.Parse("1900-01-01");
            }
        }

        /// <summary>
        /// Base64字符串转Byte
        /// </summary>
        /// <param name="Text">带转换的内容</param>
        /// <returns>返回：Byte结果，非法返回null</returns>
        public static byte[] Base64StringToByte(string Text)
        {
            if (Text != null && Text.Length > 0)
            {
                byte[] buf = Convert.FromBase64String(Text);//把字符串读到字节数组中
                return buf;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Byte[]转Image
        /// </summary>
        /// <param name="by">字节数组</param>
        /// <returns>返回：Image对象</returns>
        public static System.Drawing.Image ByteToImage(byte[] by)
        {
            if (by != null)
            {
                MemoryStream ms = new MemoryStream(by);
                return System.Drawing.Image.FromStream(ms);
            }
            return null;
        }

        /// <summary>
        /// Image转Byte[]
        /// </summary>
        /// <param name="by">Image对象</param>
        /// <returns>返回：Byte[]对象</returns>
        public static byte[] ImageToByte(System.Drawing.Image image)
        {
            if (image != null)
            {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, (object)image);
                ms.Close();
                return ms.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Image转Base64String
        /// </summary>
        /// <param name="image">Image对象</param>
        /// <returns>返回：Base64String</returns>
        public static string ImageToBase64String(System.Drawing.Image image)
        {
            if (image != null)
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                string mes = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return mes;
            }
            return string.Empty;
        }

        /// <summary>
        /// byte[]转Base64String
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>返回：Base64String</returns>
        public static string ByteToBase64String(byte[] bytes)
        {
            if (bytes != null)
            {
                string mes = Convert.ToBase64String(bytes);
                return mes;
            }
            return string.Empty;
        }

        /// <summary>
        /// byte[]转String
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>返回：String</returns>
        public static string ByteToString(byte[] bytes)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (byte bt in bytes)
            {
                strBuilder.AppendFormat("{0:X2}", bt);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// Stream转Bytes
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        # endregion

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
        public static void AddAppSetting(string key,string value)
        {
            XmlDocument xmlDoc = new System.Xml.XmlDocument();
            string FullFilePath = AppDomain.CurrentDomain.BaseDirectory +AppDomain.CurrentDomain.FriendlyName+ ".config";
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
        /// datatable 数据求和
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colNames"></param>
        /// <param name="expression"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static decimal SumDataTableColumn(DataTable dt, string[] colNames, string expression, string filters)
        {
            decimal sumVal = 0;
            try
            {
                DataTable dtSum = dt.Clone();
                if (dt.Rows.Count > 0)
                {
                    foreach (string colName in colNames)
                    {
                        dtSum.Columns[colName].DataType = typeof(decimal);
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow drAdd = dtSum.NewRow();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string colName = dt.Columns[i].ColumnName;
                            if (colNames.Contains(colName))
                            {
                                drAdd[colName] = StringToDecimal(dr[colName].ToString());
                            }
                            else
                            {
                                drAdd[colName] = dr[colName];
                            }
                        }
                        dtSum.Rows.Add(drAdd);
                    }
                sumVal = (decimal)dtSum.Compute(expression, filters);
                }
            }
            catch (Exception ex)
            {

            }
            return sumVal;
        }

        /// <summary>
        /// 字符串转二进制
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToBinary(string str)
        {
            string rel = "";
            byte[] data = Encoding.Unicode.GetBytes(str);
            StringBuilder sb = new StringBuilder(data.Length * 8);
            foreach (byte item in data)
            {
                sb.Append(Convert.ToString(item, 2).PadLeft(8, '0'));
                rel = sb.ToString();
            }
            return rel;
        }
        /// <summary>
        /// 二进制转字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string BinaryToString(string str)
        {
            System.Text.RegularExpressions.CaptureCollection cs = System.Text.RegularExpressions.Regex.Match(str, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// 字符串转16进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += "%" + Convert.ToString(b[i], 16);
            }
            return result;
        }

        /// <summary>
        /// 16进制转字符串
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string HexStringToString(string hs, Encoding encode)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }


        /// <summary>
        /// bitmap转换为base64
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static  string BitmapToBase64(Bitmap bmp)
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
}