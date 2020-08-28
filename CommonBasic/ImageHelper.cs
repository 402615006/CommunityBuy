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
    public sealed class ImageHelper
    {
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
        #endregion
    }
}