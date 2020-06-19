using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// uploadDisPic 的摘要说明
    /// </summary>
    public class uploadDisPic : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "uft-8";
            string type = context.Request["type"];
            switch (type)
            {
                case "1":
                    handle1(context);
                    break;
                case "2":
                    handle2(context);
                    break;
                case "-1":
                    handle_1(context);//删除
                    break;
            }
        }

        private void handle_1(HttpContext context)//删除图片
        {
            string strPath = context.Request["path"].Replace("/", "\\");
            string[] strArr = strPath.Split('|');
            foreach (string str in strArr)
            {
                string FilePath = CommunityBuy.BackWeb.Common.StrHelp.GetPhysicalPath() + str;
                if (File.Exists(FilePath))
                {
                    FileInfo delFile = new FileInfo(FilePath);
                    try
                    {
                        delFile.Delete();
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException(e.Message);
                    }
                }
            }
            context.Response.Write("1");
        }

        private void handle1(HttpContext context)//上传图片
        {
            string typename = "weixin";
            HttpPostedFile file = context.Request.Files["FileData"];
            string stocode = context.Request["stocode"];
            string discode = context.Request["discode"];
            string filelen = file.ContentLength.ToString();
            string extension = file.FileName.Substring(file.FileName.LastIndexOf("."), (file.FileName.Length - file.FileName.LastIndexOf(".")));
            string fileName1 = string.Format("b_{0}_{1}{2}", stocode, discode, extension);
            string fileName2 = string.Format("t_{0}_{1}{2}", stocode, discode, extension);
            string fileName3 = string.Format("c_{0}_{1}{2}", stocode, discode, extension);
            string uploadPath = HttpContext.Current.Server.MapPath(@context.Request["folder"]) + "\\" + typename + "\\";
            string swidth = context.Request["width"];
            string sheight = context.Request["height"];
            if (file != null)
            {
                try
                {
                    int nwidth = int.Parse(swidth);
                    int nheight = int.Parse(sheight);
                    if (!System.IO.Directory.Exists(uploadPath))
                    {
                        System.IO.Directory.CreateDirectory(uploadPath);
                    }
                    if (File.Exists(uploadPath + fileName1))
                    {
                        File.Delete(uploadPath + fileName1);
                    }
                    file.SaveAs(uploadPath + fileName1);
                    //
                    string strLength = context.Request["fileSizeLimit"];
                    if (!string.IsNullOrWhiteSpace(strLength))
                    {
                        float fLength = float.Parse(strLength);
                        FileInfo info = new FileInfo(uploadPath + fileName1);
                        if (info.Length / 1024.0 > fLength)
                        {
                            context.Response.Write("-2");
                            return;
                        }
                    }
                    //
                    byte[] bytes = File.ReadAllBytes(uploadPath + fileName1);
                    Image bit = Image.FromStream(new MemoryStream(bytes));
                    if (bit.Width < nwidth || bit.Height < nheight)
                    {
                        File.Delete(uploadPath + fileName1);
                        context.Response.Write("-1");
                        bit.Dispose();
                        return;
                    }
                    else
                    {
                        if (bit.Width > nwidth || bit.Height > nheight)//裁剪
                        {
                            File.Move(uploadPath + fileName1, uploadPath + "bak" + fileName1);
                            UploadPicture.MakeThumbnail(uploadPath + "bak" + fileName1, uploadPath + fileName1, nwidth, nheight, "Cut");
                            File.Delete(uploadPath + "bak" + fileName1);
                        }
                        ////
                        if (File.Exists(uploadPath + fileName2))
                        {
                            File.Delete(uploadPath + fileName2);
                        }
                        UploadPicture.MakeThumbnail(uploadPath + fileName1, uploadPath + fileName2, 270, 180, "HW");
                        //file.SaveAs(uploadPath + fileName2);
                        if (File.Exists(uploadPath + fileName3))
                        {
                            File.Delete(uploadPath + fileName3);
                        }
                        UploadPicture.MakeThumbnail(uploadPath + fileName1, uploadPath + fileName3, 160, 120, "HW");
                        //file.SaveAs(uploadPath + fileName3);
                        string redultName = context.Request["folder"] + "/" + typename + "/" + fileName1 + "";
                        context.Response.Write(redultName);
                        bit.Dispose();
                    }
                }
                catch
                {
                    context.Response.Write("-1");
                    return;
                }
            }
            else
            {
                context.Response.Write("-1");
            }
        }

        private void handle2(HttpContext context)//上传图片
        {
            string typename = "weixin";
            HttpPostedFile file = context.Request.Files["FileData"];
            string stocode = context.Request["stocode"];
            string discode = context.Request["discode"];
            string filelen = file.ContentLength.ToString();
            string extension = file.FileName.Substring(file.FileName.LastIndexOf("."), (file.FileName.Length - file.FileName.LastIndexOf(".")));
            string fileName = string.Format("tc_{0}_{1}{2}", stocode, discode, extension);
            string uploadPath = HttpContext.Current.Server.MapPath(@context.Request["folder"]) + "\\" + typename + "\\";
            string swidth = context.Request["width"];
            string sheight = context.Request["height"];
            string strLength = context.Request["fileSizeLimit"];
            if (file != null)
            {
                try
                {
                    int nwidth = int.Parse(swidth);
                    int nheight = int.Parse(sheight);
                    if (!System.IO.Directory.Exists(uploadPath))
                    {
                        System.IO.Directory.CreateDirectory(uploadPath);
                    }
                    if (File.Exists(uploadPath + fileName))
                    {
                        File.Delete(uploadPath + fileName);
                    }
                    file.SaveAs(uploadPath + fileName);
                    //
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
                    //
                    byte[] bytes = File.ReadAllBytes(uploadPath + fileName);
                    Image bit = Image.FromStream(new MemoryStream(bytes));
                    if (bit.Width < nwidth || bit.Height < nheight)
                    {
                        File.Delete(uploadPath + fileName);
                        context.Response.Write("-1");
                        bit.Dispose();
                        return;
                    }
                    else
                    {
                        File.Move(uploadPath + fileName, uploadPath + "bak" + fileName);
                        UploadPicture.MakeThumbnail(uploadPath + "bak" + fileName, uploadPath + fileName, nwidth, nheight, "Cut");
                        File.Delete(uploadPath + "bak" + fileName);
                        bit.Dispose();
                    }
                }
                catch
                {
                    context.Response.Write("-1");
                    return;
                }
                string redultName = context.Request["folder"] + "/" + typename + "/" + fileName + "";
                context.Response.Write(redultName);
            }
            else
            {
                context.Response.Write("-1");
            }
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