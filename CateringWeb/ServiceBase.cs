using CommunityBuy.BLL;
using CommunityBuy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    public class ServiceBase : IHttpHandler
    {
        public HttpContext Pagcontext = null;
        public string actionname = string.Empty;
        public string usercode = string.Empty; //授权用户账号
        public operatelogEntity entity = null;
        public virtual void ProcessRequest(HttpContext context)
        {
        }

        public string GetPriKey(DataTable dt, string key)
        {
            string strReturn = string.Empty;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strReturn += dt.Rows[i][key].ToString() + ",";
                }
            }

            return "'" + strReturn.TrimEnd(',').Replace(",", "','") + "'";
        }

        /// <summary>
        /// 检测接口必要参数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected bool CheckParameters(HttpContext context)
        {
            Pagcontext = context;
            bool Flag = true;
            string mes = string.Empty;
            actionname = context.Request["actionname"];
            //if (CheckAuthorization() == true)
            //{
            if (actionname == null)
            {
                mes += "actionname,";
                Flag = false;
            }
            string parameters = context.Request["parameters"];
            if (parameters == null)
            {
                mes += "parameters,";
                Flag = false;
            }
            if (!Flag)
            {
                context.Response.Write("{\"status\":\"2\",\"mes\":\"缺少" + mes.TrimEnd(',') + "参数\"}");
            }
            //}
            //else
            //{
            //    context.Response.Write("{\"status\":\"2\",\"mes\":\"授权已过期或总控程序未启动\"}");
            //}
            return Flag;
        }

        /// <summary>
        /// 检测授权程序是否开启
        /// </summary>
        /// <returns></returns>
        private bool CheckAuthorization()
        {
            bool result = false;
            if (Process.GetProcessesByName("CateringServerForm").Length > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 检测接口必要参数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected bool CheckCouponParameters(HttpContext context)
        {
            Pagcontext = context;
            bool Flag = true;
            string mes = string.Empty;
            actionname = context.Request["actionname"];
            if (actionname == null)
            {
                mes += "actionname,";
                Flag = false;
            }
            string parameters = context.Request["parameters"];
            if (parameters == null)
            {
                mes += "parameters,";
                Flag = false;
            }
            //授权用户账号
            usercode = context.Request["usercode"];
            if (usercode == null)
            {
                mes += "usercode,";
                Flag = false;
            }

            if (!Flag)
            {
                context.Response.Write("{\"status\":\"2\",\"mes\":\"缺少" + mes.TrimEnd(',') + "参数\"}");
            }
            return Flag;
        }

        /// <summary>
        /// 获取json参数信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected Dictionary<string, object> GetParameters()
        {
            Dictionary<string, object> dicPar = new Dictionary<string, object>();
            string parameters = Pagcontext.Request["parameters"];
            if (parameters.Length > 0)
            {
                try
                {
                    //string decparameters = OEncryp.Decrypt(parameters);
                    string decparameters = parameters;
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    object obj = jss.DeserializeObject(decparameters);
                    dicPar = (Dictionary<string, object>)obj;
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                    Pagcontext.Response.Write("{\"code\":\"2\",\"msg\":\"参数解析错误\"}");
                    return null;
                }
            }
            return dicPar;
        }

        /// <summary>
        /// 检测调用参数是否合法
        /// </summary>
        /// <param name="dicPar"></param>
        /// <param name="liPra"></param>
        /// <returns></returns>
        protected bool CheckActionParameters(Dictionary<string, object> dicPar, List<string> liPra)
        {
            string mes = string.Empty;
            bool Flag = true;
            if (liPra == null)
            {
                Pagcontext.Response.Write("{\"code\":\"2\",\"msg\":\"List参数不合法\"}");
                return false;
            }
            if (dicPar == null)
            {
                Pagcontext.Response.Write("{\"code\":\"2\",\"msg\":\"parameters参数解析错误\"}");
                return false;
            }
            foreach (string str in liPra)
            {
                if (!dicPar.ContainsKey(str))
                {
                    mes += str + ",";
                    Flag = false;
                }
            }
            if (!Flag)
            {
                Pagcontext.Response.Write("{\"code\":\"2\",\"msg\":\"缺少" + mes.TrimEnd(',') + "参数\"}");
            }
            return Flag;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        protected void ToCustomerJson(string status, string mes)
        {
            Pagcontext.Response.Write(JsonHelper.ToJson(status, mes));
        }

        protected void ToErrorJson()
        {
            string mes = ErrMessage.GetMessageInfoByCode("Err_004").Body;
            Pagcontext.Response.Write(JsonHelper.ToErrorJson("1", mes));
        }

        protected void ToTipsJson()
        {
            Pagcontext.Response.Write("{\"status\":\"2\",\"mes\":\"审核成功\"}");
        }

        protected void ToSucessJson()
        {
            Pagcontext.Response.Write("{\"status\":\"0\",\"mes\":\"操作成功\"}");
        }

        protected void ToNullJson()
        {
            string mes = ErrMessage.GetMessageInfoByCode("Err_003").Body;
            Pagcontext.Response.Write(JsonHelper.ToErrorJson("1", mes));
        }

        protected void ToJsonStr(string jsonstr)
        {
            Pagcontext.Response.Write(jsonstr);
        }

        /// <summary>
        /// 返回执行json
        /// </summary>
        /// <param name="dt"></param>
        protected void ReturnJson(DataTable dt)
        {
            string type;
            string mes;
            Helper.GetDataTableToResult(dt, out type, out mes);
            Pagcontext.Response.Write(JsonHelper.ToJson(type, mes));
        }

        /// <summary>
        /// 返回单条json
        /// </summary>
        /// <param name="dt"></param>
        protected void ReturnListJson(DataTable dt)
        {
            ReturnListJson(dt, null, null, null, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="status"></param>
        /// <param name="mes"></param>
        protected void ReturnJsonByT<T>(T t)
        {
            Pagcontext.Response.Write(JsonHelper.ObjectToJson<T>(t));
        }

        /// <summary>
        /// 返回列表json
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="currentPage"></param>
        /// <param name="totalPage"></param>
        protected void ReturnListJson(DataTable dt, int? pageSize, long? recordCount, int? currentPage, int? totalPage)
        {
            string type;
            string mes;
            Helper.GetDataTableToResult(dt, out type, out mes);
            if (type != "0")
            {
                Pagcontext.Response.Write(JsonHelper.ToJson(type, mes));
            }
            else
            {
                Pagcontext.Response.Write(JsonHelper.ToJson(type, mes, new ArrayList() { dt }, new string[] { "data" }, pageSize, recordCount, currentPage, totalPage));
            }
        }


        /// <summary>
        /// Json 返回多对象数据
        /// </summary>
        /// <param name="type">是否成功标识</param>
        /// <param name="mes">提示信息</param>
        /// <param name="list">多对象</param>
        /// <param name="Names">多对象返回名称</param>
        /// <param name="pageSize">页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="currentPage">当前页数</param>
        /// <param name="totalPage">总页数</param>
        protected void ReturnListJson(string type, string mes, ArrayList list, string[] Names, int? pageSize, long? recordCount, int? currentPage, int? totalPage)
        {
            if (type != "0")
            {
                Pagcontext.Response.Write(JsonHelper.ToJson(type, mes));
            }
            else
            {
                Pagcontext.Response.Write(JsonHelper.ToJson(type, mes, list, Names, pageSize, recordCount, currentPage, totalPage));
            }
        }

        protected void ReturnListJson(string type, string mes, ArrayList list, string[] Names)
        {
            if (type != "0")
            {
                Pagcontext.Response.Write(JsonHelper.ToJson(type, mes));
            }
            else
            {
                Pagcontext.Response.Write(JsonHelper.ToJson(type, mes, list, Names, null, null, null, null));
            }
        }

        /// <summary>
        /// 设置用户权限门店编号到缓存
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoleCode"></param>
        public void SetUserLoginRoleCodeToCache(string UserId, string RoleCode)
        {
            Hashtable ht = new Hashtable();
            if (WebCache.IsExist(UserId + "CommunityBuy_LoginInfo"))
            {
                ht = (Hashtable)WebCache.GetCache(UserId + "CommunityBuy_LoginInfo");
                if (ht[UserId] == null)
                {
                    ht.Add(UserId, RoleCode);
                    WebCache.Insert(UserId + "CommunityBuy_LoginInfo", ht);
                }
            }
            else
            {
                ht.Add(UserId, RoleCode);
                WebCache.Insert(UserId + "CommunityBuy_LoginInfo", ht);
            }
        }

        #region 缓存当前登陆用户的商户编号
        /// <summary>
        /// 缓存登陆用户的商户编号
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="buscode"></param>
        public void SetUserBusCodeToCache(string userid, string buscode)
        {
            WebCache.Insert(userid + "_buscode", buscode);
        }

        /// <summary>
        /// 获取指定用户的商户编号
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetCacheToUserBusCode(string userid)
        {
            if (WebCache.IsExist(userid + "_buscode"))
            {
                return WebCache.GetCache(userid + "_buscode").ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion


        /// <summary>
        /// 获取权限范围条件
        /// </summary>
        /// <param name="ColName">查询列名</param>
        /// <param name="RoleStoCode">拥有权限的门店Codes,多个逗号隔开</param>
        /// <returns></returns>
        protected string GetAuthoritywhere(string ColName, string UserId)
        {
            string where = string.Empty;
            string RoleStoCode = string.Empty;

            Hashtable ht = new Hashtable();
            if (WebCache.IsExist(UserId + "CommunityBuy_LoginInfo"))
            {
                ht = (Hashtable)WebCache.GetCache(UserId + "CommunityBuy_LoginInfo");
                if (ht[UserId] == null)
                {
                    RoleStoCode = GetUserRoleCodes(UserId);
                    ht.Add(UserId, RoleStoCode);
                    WebCache.Insert(UserId + "CommunityBuy_LoginInfo", ht);
                }
                else
                {
                    RoleStoCode = ht[UserId].ToString();
                }
            }
            else
            {
                RoleStoCode = GetUserRoleCodes(UserId);
                ht.Add(UserId, RoleStoCode);
                WebCache.Insert(UserId + "CommunityBuy_LoginInfo", ht);
            }
            if (RoleStoCode.Trim().Length > 0)
            {
                //if (!ColName.Contains("."))
                //{
                //    where = " and " + ColName + " in('" + RoleStoCode.Replace(",", "','") + "') and buscode='" + GetCacheToUserBusCode(UserId) + "' ";
                //}
                //else
                //{
                //    string asname = ColName.Substring(0, ColName.IndexOf("."));
                //    where = " and " + ColName + " in('" + RoleStoCode.Replace(",", "','") + "') and " + asname + ".buscode='" + GetCacheToUserBusCode(UserId) + "' ";
                //}
                if (!ColName.Contains("."))
                {
                    where = " and " + ColName + " in('" + RoleStoCode.Replace(",", "','") + "') ";
                }
                else
                {
                    string asname = ColName.Substring(0, ColName.IndexOf("."));
                    where = " and " + ColName + " in('" + RoleStoCode.Replace(",", "','") + "') ";
                }
            }
            return where;
        }

        /// <summary>
        /// 获取BusCode条件
        /// </summary>
        /// <param name="dicPar">dicPar键值对</param>
        /// <param name="filter">已有条件</param>
        /// <param name="ColName"></param>
        /// <returns></returns>
        protected string GetBusCodeWhere(Dictionary<string, object> dicPar,string filter,string ColName)
        {
            string where = string.Empty;
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                if(dicPar["BusCode"].ToString()!= "undefined")
                {
                    BusCode = dicPar["BusCode"].ToString();
                }
            }
            if (!filter.ToLower().Contains("buscode"))
            {
                if (!string.IsNullOrEmpty(BusCode))
                {
                    if (string.IsNullOrEmpty(filter))
                    {
                        where = " where "+ ColName + "='" + BusCode + "'";
                    }
                    else
                    {
                        where += " and "+ ColName + "='" + BusCode + "'";
                    }
                }
            }
            return filter+where;
        }

        /// <summary>
        /// 调用连锁端接口获取用户权限门店编号
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <returns></returns>
        public string GetUserRoleCodes(string UserId)
        {
            string rolecode = "x#";
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            UserId = Helper.ReplaceString(UserId);
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/WSadmins.ashx";
            postStr.Append("actionname=getrolestocoe&parameters={" +
                                           string.Format("'userid':'{0}'", UserId) +
                                   "}");//键值对
            string strJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            if (strJson.Length > 0)
            {
                string status = "";
                string mes = "";
                DataSet ds = JsonHelper.NewJsonToDataSet(strJson, out status, out mes);
                if (status == "0")
                {
                    DataTable dtAdmin = ds.Tables["data"];
                    if (dtAdmin != null && dtAdmin.Rows.Count > 0)
                    {
                        DataRow dr = dtAdmin.Rows[0];
                        rolecode = dtAdmin.Rows[0]["rolecode"].ToString();

                    }
                }
            }
            return rolecode;
        }


        #region 缓存商户门店信息 包括门店编号、门店名称
        /// <summary>
        /// 根据登陆的用户的编号缓存该商户所有门店信息
        /// </summary>
        /// <param name="userid">用户id</param>
        public void SetStoreToCache(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            DataTable dtStore = null;
            if (!WebCache.IsExist(busCode + "_sto"))
            {
                dtStore = GetAllStoreList(busCode);
                WebCache.Insert(busCode + "_sto", dtStore,5);
            }
        }

        /// <summary>
        /// 根据用户编号从缓存中取门店信息
        /// </summary>
        /// <param name="userid">用户id</param>
        public DataTable GetCacheToStore(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            DataTable dtStore = null;
            if (WebCache.IsExist(busCode+"_sto"))
            {
                dtStore = (DataTable)WebCache.GetCache(busCode + "_sto");
            }
            else
            {
                dtStore = GetAllStoreList(busCode);
                WebCache.Insert(busCode + "_sto", dtStore,5);
            }
            if(dtStore==null)
            {
                dtStore = GetAllStoreList(busCode);
                WebCache.Insert(busCode + "_sto", dtStore, 5);
            }
            return dtStore;
        }

        /// <summary>
        /// 根据商户编号获取门店信息
        /// </summary>
        /// <param name="buscode">商户编号</param>
        /// <returns></returns>
        private DataTable GetAllStoreList(string busCode)
        {
            DataTable dt = new bllStore().GetBusCodeToAllStore(busCode);
            return dt;
        }

        #endregion

        #region 缓存商户部门信息 包括部门编号、部门名称
        /// <summary>
        /// 根据登陆的用户的编号缓存该商户所有部门信息
        /// </summary>
        /// <param name="userid">用户id</param>
        public void SetDepartmentToCache(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            DataTable dtStore = null;
            if (!WebCache.IsExist(busCode + "_dep"))
            {
                dtStore = GetAllDepartmentList(busCode);
                WebCache.Insert(busCode + "_dep", dtStore,5);
            }
        }

        /// <summary>
        /// 根据用户编号从缓存中取部门信息
        /// </summary>
        /// <param name="userid">用户id</param>
        public DataTable GetCacheToDepartment(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            if (string.IsNullOrEmpty(busCode))
            {
                busCode = Helper.GetAppSettings("BusCode").ToString();
            }
            DataTable dtDepartment = null;
            if (WebCache.IsExist(busCode + "_dep"))
            {
                dtDepartment = (DataTable)WebCache.GetCache(busCode + "_dep");
            }
            else
            {
                dtDepartment = GetAllDepartmentList(busCode);
                WebCache.Insert(busCode + "_dep", dtDepartment,5);
            }
            if(dtDepartment==null)
            {
                dtDepartment = GetAllDepartmentList(busCode);
                WebCache.Insert(busCode + "_dep", dtDepartment, 5);
            }
            return dtDepartment;
        }
        /// <summary>
        /// 根据商户编号获取部门信息
        /// </summary>
        /// <param name="buscode">商户编号</param>
        /// <returns></returns>
        private DataTable GetAllDepartmentList(string busCode)
        {
            DataTable dt = new bllDepartment().GetBusCodeToAllDep(busCode);
            return dt;
        }
        #endregion

        #region 缓存商户的财务类别 包括财务类别编号、财务类别名称
        /// <summary>
        /// 根据登陆的用户的编号缓存该商户所有财务类别信息
        /// </summary>
        /// <param name="userid">用户id</param>
        public void SetFinTypeToCache(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);

            DataTable dtStore = null;
            if (!WebCache.IsExist(busCode + "_finType"))
            {
                dtStore = GetAllFinTypeList(busCode);
                WebCache.Insert(busCode + "_finType", dtStore,5);
            }
        }

        /// <summary>
        /// 根据用户编号从缓存中取财务类别信息
        /// </summary>
        /// <param name="userid">用户id</param>
        public DataTable GetCacheToFinType(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            DataTable dtFinType = null;
            if (WebCache.IsExist(busCode + "_finType"))
            {
                dtFinType = (DataTable)WebCache.GetCache(busCode + "_finType");
            }
            else
            {
                dtFinType = GetAllFinTypeList(busCode);
                WebCache.Insert(busCode + "_finType", dtFinType,5);
            }
            if(dtFinType==null)
            {
                dtFinType = GetAllFinTypeList(busCode);
                WebCache.Insert(busCode + "_finType", dtFinType, 5);
            }
            return dtFinType;
        }
        /// <summary>
        /// 根据商户编号获取财务类别信息
        /// </summary>
        /// <param name="buscode">商户编号</param>
        /// <returns></returns>
        private DataTable GetAllFinTypeList(string busCode)
        {
            DataTable dt = new bllFinanceType().GetBusCodeToAllFin(busCode);
            return dt;
        }
        #endregion

        #region 缓存商户的员工信息,包括门店、姓名、员工编号、部门编号
        /// <summary>
        /// 根据登陆的用户的编号缓存该商户所有财务类别信息
        /// </summary>
        /// <param name="userid">用户id</param>
        public void SetEmployeeToCache(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            DataTable dtStore = null;
            if (!WebCache.IsExist(busCode + "_Employee"))
            {
                dtStore = GetAllDepartmentList(busCode);
                WebCache.Insert(busCode + "_Employee", dtStore,5);
            }
        }

        /// <summary>
        /// 根据用户编号从缓存中取员工信息
        /// </summary>
        /// <param name="userid">用户id</param>
        public DataTable GetCacheToEmployee(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            DataTable dtFinType = null;
            if (WebCache.IsExist(busCode + "_Employee"))
            {
                dtFinType = (DataTable)WebCache.GetCache(busCode + "_Employee");
            }
            else
            {
                dtFinType = GetAllEmployeeList(busCode);
                WebCache.Insert(busCode + "_Employee", dtFinType,5);
            }
            if(dtFinType==null)
            {
                dtFinType = GetAllEmployeeList(busCode);
                WebCache.Insert(busCode + "_Employee", dtFinType, 5);
            }
            return dtFinType;
        }
        /// <summary>
        /// 根据商户编号获取财务类别信息
        /// </summary>
        /// <param name="buscode">商户编号</param>
        /// <returns></returns>
        private DataTable GetAllEmployeeList(string busCode)
        {
            DataTable dt = new bllEmployee().GetBusCodeToAllEmp(busCode);
            return dt;
        }
        #endregion

        #region 缓存总部的下的计量单位信息;
        /// <summary>
        /// 综合版获取总部的下的计量单位信息;
        /// </summary>
        /// <param name="userid">用户id</param>
        public void SetDictsToCache(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            DataTable dtStore = null;
            if (!WebCache.IsExist(busCode + "_Dicts"))
            {
                dtStore = GetAllDepartmentList(busCode);
                WebCache.Insert(busCode + "_Dicts", dtStore,5);
            }
        }

        /// <summary>
        /// 综合版获取总部的下的计量单位信息;
        /// </summary>
        /// <param name="userid">用户id</param>
        public DataTable GetCacheToDicts(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            DataTable dtDicts = null;
            if (WebCache.IsExist(busCode + "_Dicts"))
            {
                dtDicts = (DataTable)WebCache.GetCache(busCode + "_Dicts");
            }
            else
            {
                dtDicts = GetAllDictsList(busCode);
                WebCache.Insert(busCode + "_Dicts", dtDicts,5);
            }
            if(dtDicts==null)
            {
                dtDicts = GetAllDictsList(busCode);
                WebCache.Insert(busCode + "_Dicts", dtDicts, 5);
            }
            return dtDicts;
        }
        /// <summary>
        /// 综合版获取总部的下的计量单位信息;
        /// </summary>
        /// <param name="buscode">商户编号</param>
        /// <returns></returns>
        private DataTable GetAllDictsList(string busCode)
        {
            DataTable dt = new bllDicts().GetBusCodeToDicts(busCode);
            return dt;
        }
        #endregion

        #region 缓存会员卡等级信息;
        /// <summary>
        /// 综合版获取会员卡等级信息;
        /// </summary>
        /// <param name="userid">用户id</param>
        public void SetMemcardLevelToCache(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            DataTable dtStore = null;
            if (!WebCache.IsExist(busCode + "_memcardtypeList"))
            {
                dtStore = GetAllDepartmentList(busCode);
                WebCache.Insert(busCode + "_memcardtypeList", dtStore,5);
            }
        }

        /// <summary>
        /// 综合版获取会员卡等级信息;
        /// </summary>
        /// <param name="userid">用户id</param>
        public memcardtypeEntityList GetCacheToMemcardLevel(string userid)
        {
            string busCode = GetCacheToUserBusCode(userid);
            memcardtypeEntityList dtDicts = null;
            if (WebCache.IsExist(busCode + "_memcardtypeList"))
            {
                dtDicts = (memcardtypeEntityList)WebCache.GetCache(busCode + "_memcardtypeList");
            }
            else
            {
                dtDicts = GetAllMemcardLevelList(busCode);
                WebCache.Insert(busCode + "_memcardtypeList", dtDicts,5);
            }
            if(dtDicts==null)
            {
                dtDicts = GetAllMemcardLevelList(busCode);
                WebCache.Insert(busCode + "_memcardtypeList", dtDicts, 5);
            }
            return dtDicts;
        }
        /// <summary>
        /// 综合版获取会员卡等级信息;
        /// </summary>
        /// <param name="buscode">商户编号</param>
        /// <returns></returns>
        private memcardtypeEntityList GetAllMemcardLevelList(string buscode)
        {
            return new bllMemcardLevel().GetMemcardLevel(buscode);
        }
        #endregion
               
    }
}