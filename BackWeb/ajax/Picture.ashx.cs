using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// Picture 的摘要说明
    /// </summary>
    public class Picture : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                HttpPostedFile file = context.Request.Files["Filedata"];
                string uploadPath = HttpContext.Current.Server.MapPath("/uploads/stock") + "\\";
                if (file != null)
                {
                    //获取文件后缀名
                    string extension = file.FileName.Substring(file.FileName.LastIndexOf("."), (file.FileName.Length - file.FileName.LastIndexOf(".")));
                    //按年月生成文件夹
                    string folderName = StrHelp.MakeFolderName();
                    //生成新文件名
                    string fileName = StrHelp.MakeFileRndName() + extension.ToLower();
                    if (!System.IO.Directory.Exists(uploadPath + "original\\" + folderName))
                    {
                        System.IO.Directory.CreateDirectory(uploadPath + "\\original\\" + folderName);
                    }
                    //保存原图
                    file.SaveAs(string.Format("{0}original\\{1}\\{2}", uploadPath, folderName, fileName));

                    //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                    context.Response.Write(string.Format("{0}/{1}", folderName, fileName));
                }
                else
                {
                    context.Response.Write("0");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog,ex.ToString());
                context.Response.Write("0");
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