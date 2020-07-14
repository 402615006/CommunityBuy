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
        bllAdmins bll = new bllAdmins();
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
            //DataTable dt = bll.getUserRole(GUID, USER_ID, userid, pagecode);
            //ReturnListJson(dt);
        }
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="dicPar"></param>
        private void LoginOut(Dictionary<string, object> dicPar)
        {
            
        }


        /// <summary>
        /// 登录("uname", "password", "cardcode", "devicecode")
        /// </summary>
        /// <param name="dicPar"></param>
        private void Login(Dictionary<string, object> dicPar)
        {

            string strUser = string.Empty;
            //要检测的参数信息
            List<string> pra = new List<string>() { "uname", "password" };
            //检测方法需要的参数

            string Uname = dicPar["uname"].ToString();//用户名
            string Password = dicPar["password"].ToString();//密码

            bool checkinput = (Helper.CheckSqlInjection(Uname) && Helper.CheckSqlInjection(Password));
            if (!checkinput)
            {
                ReturnResultJson("-2", "您的输入格式不正确");
                return;            
            }
            string filter = " uname='"+Uname+"' and upwd='"+ Password + "'";
            AdminsEntity loginEntity = bll.GetEntitySigInfo(filter);
            if (loginEntity == null)
            {
                ReturnResultJson("-1", "登录失败");
                return;
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
            bll.ResetPwd(Guid, USER_ID, userID, NewPassword);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "uname", "upwd", "realname", "umobile", "remark", "status", "cuser", "uuser", "role", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string uname = dicPar["uname"].ToString();
            string upwd = dicPar["upwd"].ToString();
            string realname = dicPar["realname"].ToString();
            string umobile = dicPar["umobile"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string cuser = dicPar["cuser"].ToString();
            string uuser = dicPar["uuser"].ToString();
            string role = dicPar["role"].ToString();
            //调用逻辑
            bll.Add(GUID, USER_ID, userid,uname, upwd, realname, umobile, remark, status, cuser, uuser, role);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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
            string uname = dicPar["uname"].ToString();
            string realname = dicPar["realname"].ToString();
            string umobile = dicPar["umobile"].ToString();
            string remark = dicPar["remark"].ToString();
            string role = dicPar["role"].ToString();
            //调用逻辑

            AdminsEntity UEntity = bll.GetEntitySigInfo(" where userid="+userid);
            if (UEntity.userid > 0)
            {
                UEntity.uname = uname;
                UEntity.realname = realname;
                UEntity.umobile = umobile;
                UEntity.remark = remark;
                bll.Update(GUID, USER_ID, UEntity,role);
            }
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

            ReturnListJson(dt,1,1,1,1);
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
            bll.Delete(GUID, USER_ID, userid);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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
            bll.UpdateStatus(GUID, USER_ID, userid, status);

            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }
    }
}