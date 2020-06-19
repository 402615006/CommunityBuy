using System;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


namespace CommunityBuy.CommonBasic
{
    /// <summary>
    /// Helper������
    /// </summary>
    public sealed class StringHelper
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
        /// ��ȡ�ַ���������ĸƴ��
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        static public string GetChineseSpell(string strText)
        {

            int len = strText.Length;
            string myStr = "";

            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }

        static public string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) }).ToUpper();
                    }
                }
                return "*";
            }
            else return cnChar.ToUpper();
        }
    }
}