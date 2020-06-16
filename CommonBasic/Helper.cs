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
        /// ��ȡ��Ӣ�Ļ����ַ�����ʵ�ʳ���(�ֽ���)
        /// </summary>
        /// <param name="str">Ҫ��ȡ���ȵ��ַ���</param>
        /// <returns>�ַ�����ʵ�ʳ���ֵ���ֽ�����</returns>
        public static int getStringLength(string str)
        {
            if (str.Equals(string.Empty))
                return 0;
            int strlen = 0;
            ASCIIEncoding strData = new ASCIIEncoding();
            //���ַ���ת��ΪASCII������ֽ�����
            byte[] strBytes = strData.GetBytes(str);
            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //���Ķ�������ΪASCII����63,��"?"��
                    strlen++;
                strlen++;
            }
            return strlen;
        }

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


        #region �ַ�����ʽת��
        /// <summary>
        /// SQL�ַ�������
        /// </summary>
        /// <param name="Text">�����˵�����</param>
        /// <returns>���أ����˺��ַ���</returns>
        public static string ReplaceString(string Text)
        {
            if (Text != null)
            {
                return Text.Trim().Replace("'", "��").Replace(@"\", @"");
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// �ַ���תInt
        /// </summary>
        /// <param name="Text">��ת��������</param>
        /// <returns>���أ�Int������Ƿ�����0</returns>
        public static int StringToInt(string Text)
        {
            if (Text != null)
            {
                int temp;
                int.TryParse(Text, out temp);

                #region �����ַ�����1.00�Ƿ�������0 tsg 2017-05-06
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
        /// �ַ���תLong
        /// </summary>
        /// <param name="Text">��ת��������</param>
        /// <returns>���أ�Long������Ƿ�����0</returns>
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
        /// �ַ���תDouble
        /// </summary>
        /// <param name="Text">��ת��������</param>
        /// <returns>���أ�Double������Ƿ�����0</returns>
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
        /// �ַ���תDecimal
        /// </summary>
        /// <param name="Text">��ת��������</param>
        /// <returns>���أ�Decimal������Ƿ�����0</returns>
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
        /// �ַ���תFloat
        /// </summary>
        /// <param name="Text">��ת��������</param>
        /// <returns>���أ�Float������Ƿ�����0</returns>
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
        /// �ַ���תDateTime
        /// </summary>
        /// <param name="Text">��ת��������</param>
        /// <returns>���أ�DateTime������Ƿ�����1900-01-01</returns>
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
        /// Base64�ַ���תByte
        /// </summary>
        /// <param name="Text">��ת��������</param>
        /// <returns>���أ�Byte������Ƿ�����null</returns>
        public static byte[] Base64StringToByte(string Text)
        {
            if (Text != null && Text.Length > 0)
            {
                byte[] buf = Convert.FromBase64String(Text);//���ַ��������ֽ�������
                return buf;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Byte[]תImage
        /// </summary>
        /// <param name="by">�ֽ�����</param>
        /// <returns>���أ�Image����</returns>
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
        /// ImageתByte[]
        /// </summary>
        /// <param name="by">Image����</param>
        /// <returns>���أ�Byte[]����</returns>
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
        /// ImageתBase64String
        /// </summary>
        /// <param name="image">Image����</param>
        /// <returns>���أ�Base64String</returns>
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
        /// byte[]תBase64String
        /// </summary>
        /// <param name="bytes">�ֽ�����</param>
        /// <returns>���أ�Base64String</returns>
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
        /// byte[]תString
        /// </summary>
        /// <param name="bytes">�ֽ�����</param>
        /// <returns>���أ�String</returns>
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
        /// StreamתBytes
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // ���õ�ǰ����λ��Ϊ���Ŀ�ʼ 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        # endregion

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
        /// datatable �������
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
        /// �ַ���ת������
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
        /// ������ת�ַ���
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
        /// �ַ���ת16����
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//����ָ�����뽫string����ֽ�����
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//���ֽڱ�Ϊ16�����ַ�����%����
            {
                result += "%" + Convert.ToString(b[i], 16);
            }
            return result;
        }

        /// <summary>
        /// 16����ת�ַ���
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string HexStringToString(string hs, Encoding encode)
        {
            //��%�ָ��ַ�������ȥ�����ַ�
            string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //����ַ���Ϊ16�����ֽ�����
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }
            //����ָ�����뽫�ֽ������Ϊ�ַ���
            return encode.GetString(b);
        }


        /// <summary>
        /// bitmapת��Ϊbase64
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
}