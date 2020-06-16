using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WSCreateQRCode 的摘要说明
    /// </summary>
    public class WSCreateQRCode : ServiceBase
    {

        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    switch (actionname.ToLower())
                    {
                        case "createqr"://生成图片
                            CreateQRCode(dicPar);
                            break;
                        case "createzip": //生成所有桌台的图片并压缩
                            CreateImgByStoCode(dicPar);
                            break;
                    }
                }
            }
        }

        private void CreateQRCode(Dictionary<string, object> dicPar)
        {
            try
            {
                //要检测的参数信息
                List<string> pra = new List<string>() { "para", "imgname" };
                //检测方法需要的参数
                if (!CheckActionParameters(dicPar, pra))
                {
                    return;
                }

                var para = dicPar["para"].ToString();
                var imgname = dicPar["imgname"].ToString();
                var url = "packageFood/pages/stocode/stocode" + para;

                var con = HttpContext.Current;
                var result = MPTools.CreateQRCode(con.Server.MapPath(@"~/uploads/qrimg/"), url, "/uploads/qrimg/", imgname);
                ToJsonStr(result);
            }
            catch (Exception ex)
            {
                ToJsonStr("{\"code\":\"-1\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        /// <summary>
        /// 批量生成图片
        /// </summary>
        /// <param name="dicPar"></param>
        private void CreateImgByStoCode(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "stocode", "stoname" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            var stocode = dicPar["stocode"].ToString();
            var stoname = dicPar["stoname"].ToString();

            //   var postString = "{\"path\":\"packageFood/stocode/stocode?scene=14-B31534\"}";          
            var con = HttpContext.Current;
            var dt = new BLL.bllPaging().GetDataTableInfoBySQL("SELECT PKCode,TableName FROM dbo.TB_Table WHERE StoCode='" + stocode + "' and TStatus=1");
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    var path = "/uploads/qrimg/" + stocode + "/";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var imgname = stoname + "_" + dt.Rows[i]["TableName"].ToString();
                        var url = "packageFood/pages/stocode/stocode?scene=" + stocode + "-" + dt.Rows[i]["PKCode"].ToString();
                        MPTools.CreateQRCode(con.Server.MapPath(@"~" + path), url, path, imgname);
                    }

                    var outpath = path.Substring(0, path.Length - 1);
                    var rname = ZipMultiFile(HttpContext.Current.Server.MapPath(@"~" + outpath));
                    var returnstr = "{\"code\":\"0\",\"msg\":\"" + outpath + ".zip" + "\"}";
                    ToJsonStr(returnstr);
                }
                catch (Exception ex)
                {
                    ToJsonStr("{\"code\":\"-1\",\"msg\":\"" + ex.Message + "\"}");
                }


            }
        }

        #region 压缩文件夹
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="ZipFromFileDictory">文件路径</param>
        public string ZipMultiFile(string ZipFromFileDictory)
        {
            string[] strFiles = Directory.GetFiles(ZipFromFileDictory);
            string strZipFileName = ZipFromFileDictory + ".zip";

            if (File.Exists(strZipFileName))
            {
                File.Delete(strZipFileName);
            }
            string name = strZipFileName.Substring(strZipFileName.LastIndexOf("\\") + 1);
            if (name == ".zip")
            {

            }
            else
            {
                //需要压缩文件的个数
                string[] strFilePaths = new string[strFiles.Length];

                MemoryStream oMemoryStream = new MemoryStream();

                ZipOutputStream oZipStream = new ZipOutputStream(File.Create(strZipFileName));

                for (int i = 0; i <= strFiles.Length - 1; i++)
                {
                    FileStream oReadFileStream = File.OpenRead(strFiles[i]);
                    byte[] btFile = new byte[oReadFileStream.Length];
                    oReadFileStream.Read(btFile, 0, btFile.Length);


                    string strCurrentFileName = Path.GetFileName(strFiles[i]);
                    strFilePaths[i] = ZipFromFileDictory + "/" + strCurrentFileName;

                    ZipEntry oZipEntry = new ZipEntry(strCurrentFileName);

                    oZipEntry.DateTime = DateTime.Now;
                    oZipEntry.Size = oReadFileStream.Length;

                    Crc32 oCrc32 = new Crc32();
                    oCrc32.Reset();
                    oCrc32.Update(btFile);


                    oZipEntry.Crc = oCrc32.Value;

                    oZipStream.PutNextEntry(oZipEntry);
                    oZipStream.Write(btFile, 0, btFile.Length);

                    oReadFileStream.Close();
                }
                oZipStream.Finish();
                oZipStream.Close();
                //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "Online; filename=" + name);
                //System.Web.HttpContext.Current.Response.ContentType = "text/plain";
                //System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                //System.Web.HttpContext.Current.Response.WriteFile(strZipFileName);
                //System.Web.HttpContext.Current.Response.End();
            }

            return strZipFileName;
        }
        #endregion
    }
}