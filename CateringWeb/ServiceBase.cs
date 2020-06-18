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
            if (HttpContext.Current.Cache.IsExist(UserId + "CommunityBuy_LoginInfo"))
            {
                ht = (Hashtable)HttpContext.Current.Cache.GetCache(UserId + "CommunityBuy_LoginInfo");
                if (ht[UserId] == null)
                {
                    ht.Add(UserId, RoleCode);
                    HttpContext.Current.Cache.Insert(UserId + "CommunityBuy_LoginInfo", ht);
                }
            }
            else
            {
                ht.Add(UserId, RoleCode);
                HttpContext.Current.Cache.Insert(UserId + "CommunityBuy_LoginInfo", ht);
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
            HttpContext.Current.Cache.Insert(userid + "_buscode", buscode);
        }

        /// <summary>
        /// 获取指定用户的商户编号
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetCacheToUserBusCode(string userid)
        {
            if (HttpContext.Current.Cache.IsExist(userid + "_buscode"))
            {
                return HttpContext.Current.Cache.GetCache(userid + "_buscode").ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion


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
    }
}