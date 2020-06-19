
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using CommunityBuy.CommonBasic;
using System.Text;
using System.IO;
using System.Drawing;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// uploads 的摘要说明
    /// </summary>
    public class uploads : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "uft-8";
            string type = context.Request["type"];
            string typename = "";
            switch (type)
            {
                case "1":
                    typename = "wx";//案例
                    break;
                case "2":
                    typename = "coupon";//优惠券
                    break;
                case "3":
                    typename = "wx";
                    break;
                case "4":
                    typename = "htmls";//网页
                    break;
                case "5":
                    typename = "htmls";//layui网页
                    break;
            }
            HttpPostedFile file = context.Request.Files["FileData"];
            string filelen = file.ContentLength.ToString();
            string extension = file.FileName.Substring(file.FileName.LastIndexOf("."), (file.FileName.Length - file.FileName.LastIndexOf(".")));
            string fileName = MakeFileRndName(extension);
            string uploadPath = HttpContext.Current.Server.MapPath(@context.Request["folder"]) + "\\" + typename + "\\";
            if (file != null)
            {
                if (!System.IO.Directory.Exists(uploadPath))
                {
                    System.IO.Directory.CreateDirectory(uploadPath);
                }
                file.SaveAs(uploadPath + fileName);
                string redultName = context.Request["folder"] + "/" + typename + "/" + fileName + "";
                if (type == "1")
                {
                    context.Response.Write(redultName);
                }
                else if (type == "3")
                {
                    #region 3
                    try
                    {
                        string strLength = context.Request["fileSizeLimit"];
                        if (!string.IsNullOrWhiteSpace(strLength))
                        {
                            float fLength = float.Parse(strLength);
                            FileInfo info = new FileInfo(uploadPath + fileName);
                            if (info.Length / 1024.0 > fLength)
                            {
                                context.Response.Write("-2");
                                return;
                            }
                        }
                        string width = context.Request["width"];
                        string height = context.Request["height"];
                        if (!string.IsNullOrWhiteSpace(width) && !string.IsNullOrWhiteSpace(height))
                        {
                            int nwidth = int.Parse(width);
                            int nheight = int.Parse(height);
                            byte[] bytes = File.ReadAllBytes(uploadPath + fileName);
                            Image bit = Image.FromStream(new MemoryStream(bytes));
                            if (bit.Width < nwidth || bit.Height < nheight)
                            {
                                File.Delete(uploadPath + fileName);
                                context.Response.Write("-1");//文件长宽不合格
                            }
                            else if (bit.Width == nwidth && bit.Height == nheight)
                            {
                                context.Response.Write(redultName);
                            }
                            else
                            {
                                File.Move(uploadPath + fileName, uploadPath + "bak" + fileName);
                                UploadPicture.MakeThumbnail(uploadPath + "bak" + fileName, uploadPath + fileName, nwidth, nheight, "Cut");
                                File.Delete(uploadPath + "bak" + fileName);
                                context.Response.Write(redultName);
                            }
                            bit.Dispose();
                        }
                        else
                        {
                            context.Response.Write(redultName);
                        }
                    }
                    catch
                    {
                        context.Response.Write("-1");
                    }
                    #endregion
                }
                else if (type == "4")
                {
                    context.Response.Write(context.Request.Url.GetLeftPart(UriPartial.Authority) + redultName);
                }
                else if (type == "5")
                {
                    var url = context.Request.Url.GetLeftPart(UriPartial.Authority) + "/ajax" + redultName;
                    context.Response.Write("{\"code\":\"0\",\"mes\":\"操作成功\",\"data\":{\"src\":\"" + url + "\"}}");
                }
            }
            else
            {
                context.Response.Write("-1");
            }
        }
        public bool IsValidEmail(string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public bool IsValidZipCode(string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^[0-9][0-9]{5}$");
        }

        public bool IsValidTelphone(string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^(?:13\d|15\d|18\d)\d{5}(\d{3}|\*{3})$");
        }

        public static string MakeFileRndName(string extension)
        {
            return (DateTime.Now.ToString("yyyyMMddHHmmss") + MakeRandomString("0123456789", 4)) + extension;
        }


        public static string MakeRandomString(string pwdchars, int pwdlen)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                int num = random.Next(pwdchars.Length);
                builder.Append(pwdchars[num]);
            }
            return builder.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}