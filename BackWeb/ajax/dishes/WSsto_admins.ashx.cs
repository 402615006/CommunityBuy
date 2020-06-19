using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.ajax.dishes
{
    /// <summary>
    /// WSsto_admins 的摘要说明
    /// </summary>
    public class WSsto_admins : ServiceBase
    {
        DataTable dt = new DataTable();
        string notstart = Helper.GetAppSettings("notstart");
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    logentity.module = "系统用户表";
                    switch (actionname.ToLower())
                    {
                        case "getuserrole"://列表
                            getUserRole(dicPar);
                            break;
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "add"://添加							
                            Add(dicPar);
                            break;
                        case "detail"://详细
                            Detail(dicPar);
                            break;
                        case "update"://修改							
                            Update(dicPar);
                            break;
                        //case "delete"://删除
                        //    Delete(dicPar);
                        //    break;
                        case "updatestatus"://修改状态
                            UpdateStatus(dicPar);
                            break;
                        case "updatepwd"://修改密码
                            UpdatePwd(dicPar);
                            break;
                        case "login"://登录
                            Login(dicPar);
                            break;
                        case "delete"://删除
                            Delete(dicPar);
                            break;
                        case "checkoldpwd": //检查旧密码
                            CheckOldPwd(dicPar);
                            break;
                        case "loginout"://退出系统
                            LoginOut(dicPar);
                            break;
                    }
                }
            }
        }


        private void getUserRole(Dictionary<string, object> dicPar)
        {
            string strUser = string.Empty;
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "pagecode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();//用户名
            string pagecode = dicPar["pagecode"].ToString();//密码
            DataTable dt = bll.getUserRole(GUID, USER_ID, userid, pagecode);
            ReturnListJson(dt);
        }
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="dicPar"></param>
        private void LoginOut(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "strcode", "stip", "uname", "devicecode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string Strcode = dicPar["strcode"].ToString();//门店编号
            string Uname = dicPar["uname"].ToString();//用户名
            string Devicecode = dicPar["devicecode"].ToString();//设备编号Mac
            string buscode = Helper.GetAppSettings("BusCode");//商户编号
            string stip = dicPar["stip"].ToString();//IP地址

            DataTable dtSet = new bllsetterminal().Add(buscode, Strcode, Devicecode, "", stip, "0", Uname);
            ReturnJson(dt);
        }

        public bool StartProcess(string filename, string[] args)
        {
            try
            {
                string s = "";
                foreach (string arg in args)
                {
                    s = s + arg + " ";
                }
                s = s.Trim();
                Process myprocess = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(filename, s);
                myprocess.StartInfo = startInfo;
                myprocess.StartInfo.UseShellExecute = false;
                myprocess.Start();
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        /// <summary>
        /// 登录("uname", "password", "cardcode", "devicecode")
        /// </summary>
        /// <param name="dicPar"></param>
        private void Login(Dictionary<string, object> dicPar)
        {
            //判断是否启动总控程序
            if (notstart != "1")
            {
                bool Isopen = false;
                Process[] vProcesses = Process.GetProcesses();
                foreach (Process vProcess in vProcesses)
                {
                    if (vProcess.ProcessName.Equals("CateringServerForm", StringComparison.OrdinalIgnoreCase))
                    {
                        Isopen = true;
                        break;
                    }
                }
                if (!Isopen)
                {
                    ErrorLog.WriteErrorMessage("总控程序未启动，请检查！");
                    ToCustomerJson("-1", "总控程序未启动，请检查！");
                    return;
                }
                else
                {
                    ErrorLog.WriteErrorMessage("总控程序已启动！");
                }
            }

            string strUser = string.Empty;
            //要检测的参数信息
            List<string> pra = new List<string>() { "strcode", "stip", "uname", "password", "cardcode", "devicecode", "hostname" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            string Strcode = dicPar["strcode"].ToString();//门店编号
            string Uname = dicPar["uname"].ToString();//用户名
            string Password = dicPar["password"].ToString();//密码
            string Cardcode = dicPar["cardcode"].ToString();//卡号
            string Devicecode = dicPar["devicecode"].ToString();//设备编号Mac
            string buscode = Helper.GetAppSettings("BusCode");//商户编号
            string stip = dicPar["stip"].ToString();//IP地址
            string hostname = dicPar["hostname"].ToString();//计算机名称
            string terminaltype = string.Empty;
            if (dicPar["terminaltype"] != null && dicPar["terminaltype"].ToString().Trim() != "")
            {
                terminaltype = dicPar["terminaltype"].ToString();//终端类型
            }
            DataTable dtSet = new DataTable();

            DataSet dsAdmin = bll.Login2(Uname, Password, Cardcode, Strcode, terminaltype);
            sto_adminsEntity objadminsEntity = null;
            if (dsAdmin != null && dsAdmin.Tables.Count > 0 && dsAdmin.Tables[0].Rows.Count > 0)
            {
                if (Cardcode.Length > 0)
                {
                    dtSet = new bllsetterminal().Add(buscode, Strcode, Devicecode, hostname, stip, "1", Cardcode);
                }
                else
                {
                    dtSet = new bllsetterminal().Add(buscode, Strcode, Devicecode, hostname, stip, "1", Uname);
                }

                objadminsEntity = EntityHelper.GetEntityByDR<sto_adminsEntity>(dsAdmin.Tables[0].Rows[0], null);
                if (objadminsEntity != null)
                {
                    if (dsAdmin.Tables.Count > 1 && dsAdmin.Tables[1].Rows.Count > 0)
                    {
                        objadminsEntity.dtFunctions = dsAdmin.Tables[1];
                        string[] arrRoles = objadminsEntity.roleids.Split(',');
                        if (arrRoles != null && arrRoles.Length > 0)
                        {
                            if (objadminsEntity.Functions == null)
                            {
                                objadminsEntity.Functions = new List<sto_functionsEntity>();
                            }
                            Dictionary<string, string> dicFunctions = new Dictionary<string, string>();
                            for (int i = 0; i < arrRoles.Length; i++)
                            {
                                if (arrRoles[i].Trim() != "")
                                {

                                    DataRow[] arrDR = dsAdmin.Tables[1].Select(string.Format(" roleids like '%,{0},%' ", arrRoles[i].Trim()));
                                    if (arrDR != null && arrDR.Length > 0)
                                    {
                                        List<sto_functionsEntity> lstFunctions = EntityHelper.GetEntityListByDR<sto_functionsEntity>(arrDR, null);
                                        if (lstFunctions != null && lstFunctions.Count > 0)
                                        {
                                            for (int j = 0; j < lstFunctions.Count; j++)
                                            {
                                                if (!dicFunctions.ContainsKey(lstFunctions[j].code + "_" + lstFunctions[j].btnname + "_" + lstFunctions[j].level.ToString()))
                                                {
                                                    objadminsEntity.Functions.Add(lstFunctions[j]);
                                                    dicFunctions.Add(lstFunctions[j].code + "_" + lstFunctions[j].btnname + "_" + lstFunctions[j].level.ToString(), lstFunctions[j].btnname);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            dicFunctions = null;

                        }
                    }
                    objadminsEntity.LoginTime = System.DateTime.Now;//登录时间
                    try
                    {
                        if (dsAdmin.Tables.Count > 2)
                        {
                            if (dsAdmin.Tables[2] != null && dsAdmin.Tables[2].Rows.Count > 0)
                            {
                                objadminsEntity.strcode = dsAdmin.Tables[2].Rows[0]["stocode"].ToString();
                                objadminsEntity.strname = dsAdmin.Tables[2].Rows[0]["cname"].ToString();
                                objadminsEntity.strtel = dsAdmin.Tables[2].Rows[0]["tel"].ToString();
                            }
                        }
                    }
                    catch { }
                }
                createtb(buscode, Strcode);
                string jsonstr = "{\"status\":\"0\",\"mes\":\"获取数据成功\",\"data\":[" + Newtonsoft.Json.JsonConvert.SerializeObject(objadminsEntity) + "]}";
                Pagcontext.Response.Write(jsonstr);
            }
            else
            {
                ToCustomerJson("-1", "用户名或密码错误，请检查！");
            }
        }

        //创建上传数据临时保存的路径表
        private void createtb(string buscode, string stocode)
        {
            try
            {
                if (string.IsNullOrEmpty(buscode) || string.IsNullOrEmpty(stocode)) return;
                string DataPath = HttpRuntime.AppDomainAppPath + "uploadsql\\";
                DataPath = DataPath.Replace(@"\\", @"\");

                if (!Directory.Exists(DataPath))
                {
                    Directory.CreateDirectory(DataPath);
                }
                string sql = " IF not exists (select 1 from  sysobjects where  id = object_id('StoreUploaddatapath') and   type = 'U') " +
                            " begin " +
                            " create table StoreUploaddatapath ( " +
                               " stuid                int                  identity, " +
                               " buscode              varchar(16)          null, " +
                               " stocode              varchar(8)           null, " +
                               " datapath             varchar(128)         null, " +
                               " utime                datetime             null, " +
                               " extc1                varchar(64)          null, " +
                               " extc2                varchar(64)          null, " +
                               " constraint PK_STOREUPLOADDATAPATH primary key (stuid) " +
                            " ) " +
                            " END ;";

                sql += string.Format(" IF not exists (select 1 from  StoreUploaddatapath where buscode='{0}' and stocode='{1}' ) " +
                       " begin " +
                        " insert into StoreUploaddatapath (buscode,stocode,datapath,utime ) values ('{0}','{1}','{2}',getdate()) ;" +
                       " end " +
                       " else " +
                       " begin " +
                            " update StoreUploaddatapath set datapath='{2}',utime=getdate() where  buscode='{0}' and stocode='{1}' ; " +
                       " end "
                       , buscode, stocode, DataPath)
                       ;
                bll.ExecuteDataSetByTran2(sql);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage("createtb错误:" + ex.Message);
            }



        }

        //修改密码
        private void UpdatePwd(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "uname", "OldPassword", "NewPassword" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string Guid = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userID = dicPar["USER_ID"].ToString();
            string uname = dicPar["uname"].ToString();
            string OldPassword = dicPar["OldPassword"].ToString();
            string NewPassword = dicPar["NewPassword"].ToString();
            NewPassword = OEncryp.Encrypt(NewPassword);
            dt = bll.stoCheckResetPwd(Guid, USER_ID, userID, OldPassword, uname, NewPassword);
            ReturnListJson(dt);
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "pageSize", "currentPage", "filter", "order" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            int pageSize = StringHelper.StringToInt(dicPar["pageSize"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["currentPage"].ToString());
            string filter = dicPar["filter"].ToString();
            //filter = CombinationFilter(new List<string>() { "userid","buscode","strcode","uname","upwd","realname","umobile","empcode","remark","status","cuser","uuser" }, dicPar, typeof(string), filter);
            string order = dicPar["order"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "buscode", "strcode", "uname", "upwd", "realname", "umobile", "empcode", "remark", "status", "cuser", "uuser", "role", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string strcode = dicPar["strcode"].ToString();
            string uname = dicPar["uname"].ToString();
            string upwd = dicPar["upwd"].ToString();
            string realname = dicPar["realname"].ToString();
            string umobile = dicPar["umobile"].ToString();
            string empcode = dicPar["empcode"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string cuser = dicPar["cuser"].ToString();
            string uuser = dicPar["uuser"].ToString();
            string role = dicPar["role"].ToString();
            //调用逻辑
            logentity.pageurl = "sto_adminsEdit.html";
            logentity.logcontent = "新增门店用户表信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.Add(GUID, USER_ID, strcode, ref userid, uname, upwd, realname, umobile, empcode, remark, status, cuser, uuser, role, entity);
            //table添加一个id，防止多次提交
            dt.Columns.Add("id", typeof(int));
            if (!string.IsNullOrEmpty(userid))
            {
                dt.Rows[0]["id"] = userid;
            }
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "buscode", "strcode", "uname", "upwd", "realname", "umobile", "empcode", "remark", "status", "cuser", "uuser", "role", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string strcode = dicPar["strcode"].ToString();
            string uname = dicPar["uname"].ToString();
            string upwd = dicPar["upwd"].ToString();
            string realname = dicPar["realname"].ToString();
            string umobile = dicPar["umobile"].ToString();
            string empcode = dicPar["empcode"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string cuser = dicPar["cuser"].ToString();
            string uuser = dicPar["uuser"].ToString();
            string role = dicPar["role"].ToString();
            //调用逻辑
            logentity.pageurl = "sto_adminsEdit.html";
            logentity.logcontent = "修改id为:" + userid + "的系统用户表信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID, strcode, userid, uname, upwd, realname, umobile, empcode, remark, status, cuser, uuser, role, logentity);
            //table添加一个id，防止多次提交
            dt.Columns.Add("id", typeof(int));
            dt.Rows[0]["id"] = userid;
            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            //调用逻辑		
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where userid=" + userid);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Rows[0]["upwd"] = OEncryp.Decrypt(dt.Rows[0]["upwd"].ToString());
            }


            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            //调用逻辑
            logentity.pageurl = "sto_adminsList.html";
            logentity.logcontent = "删除id为:" + userid + "的系统用户表信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.Delete(GUID, USER_ID, userid, entity);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "status" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string status = dicPar["status"].ToString();

            string userid = dicPar["userid"].ToString().Trim(',');
            logentity.pageurl = "sto_adminsList.html";
            logentity.logcontent = "修改状态id为:" + userid + "的系统用户表信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, userid, status);

            ReturnListJson(dt);
        }
    }
}