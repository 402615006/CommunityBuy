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
                    ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
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

        protected void ReturnResultJson(string status, string mes)
        {
            Pagcontext.Response.Write(JsonHelper.ToJson(status, mes));
        }

        protected void ReturnJsonStr(string jsonstr)
        {
            Pagcontext.Response.Write(jsonstr);
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
            if (dt != null && dt.Rows.Count > 0)
            {
                type = "1";
                mes = "获取成功";
                Pagcontext.Response.Write(JsonHelper.ToJson(type, mes, new ArrayList() { dt }, new string[] { "data" }, pageSize, recordCount, currentPage, totalPage));
            }
            else
            {
                ReturnResultJson("0", "获取失败");
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
        protected void ReturnManyListJson(string type, string mes, ArrayList list, string[] Names, int? pageSize, long? recordCount, int? currentPage, int? totalPage)
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
    }
}