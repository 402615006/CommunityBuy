
using System;
using System.Web;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// uploads 的摘要说明
    /// </summary>
    public class UploadFile:ServiceBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                context.Response.Charset = "uft-8";
                string type = context.Request["filetype"].ToString();
                string pwd = context.Request["pwd"].ToString();
                if (pwd == null || pwd != "combuy")
                {   
                    context.Response.Write("-2");
                    return;
                }
                HttpPostedFile file = context.Request.Files["file"];
                string filelen = file.ContentLength.ToString();
                string extension = file.FileName.Substring(file.FileName.LastIndexOf("."), (file.FileName.Length - file.FileName.LastIndexOf(".")));
                string fileName = Guid.NewGuid().ToString() + extension;
                string uploadPath = HttpContext.Current.Server.MapPath("/UploadFiles") + "\\" + type + "\\";
                if (file != null)
                {
                    if (!System.IO.Directory.Exists(uploadPath))
                    {
                        System.IO.Directory.CreateDirectory(uploadPath);
                    }
                    file.SaveAs(uploadPath + fileName);
                    string redultName = context.Request["folder"] + "/" + type + "/" + fileName + "";
                    context.Response.Write("{\"name\":\""+ redultName + "\"}");
                }
                else
                {
                    context.Response.Write("-1");
                }
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
                context.Response.Write(msg);
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