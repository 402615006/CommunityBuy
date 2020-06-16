using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CommunityBuy.CommonBasic
{
    public class pwHelper
    {
        private static string key_32 = "T3S3GGX6905a4skU190ZFl316ikTSGw8"; //密匙，使用32位密匙足矣

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string Encode(string toEncodeString) // 传入一个待加密的字符串，返回加密后的字符串
        {

            byte[] keyArray = ASCIIEncoding.ASCII.GetBytes(key_32);
            byte[] toEncryptArray = ASCIIEncoding.ASCII.GetBytes(toEncodeString);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static string Decode(string toDecodeString)
        {
            byte[] keyArray = ASCIIEncoding.ASCII.GetBytes(key_32);
            byte[] toEncryptArray = Convert.FromBase64String(toDecodeString);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
