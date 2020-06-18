using CommunityBuy.BLL;
using CommunityBuy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_Store 的摘要说明
    /// </summary>
    public class WS_Store : ServiceBase
    {
        DataTable dt = new DataTable();
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
                    logentity.module = "门店信息表";
                    switch (actionname.ToLower())
                    {
                        case "getalllist"://获取连锁端门店信息,不分页
                            GetAllList(dicPar);
                            break;
                        case "getbuscodetostolist"://获取连锁端门店信息,不分页
                            GetBusCodeToStoList(dicPar);
                            break;
                        case "getuseridtosto"://获取两个用户交集的门店
                            GetUserIntersectSto(dicPar);
                            break;
                        case "getuserroleintersectsto"://获取两个用户交集的门店
                            GetUserRoleIntersectSto(dicPar);
                            break;
                        case "checkstoreauthcode"://检查门店的授权码是否正确
                            GetStoreAuth(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取连锁端门店信息,不分页
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetAllList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "BusCode", "StoNmae" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string busCode = dicPar["BusCode"].ToString();
            string userid = dicPar["userid"].ToString();
            //string where = "where 1=1 and (buscode='" + busCode + "' or ''='" + busCode + "') " + GetAuthoritywhere("stocode", userid);
            string where = "where 1=1 " + GetAuthoritywhere("stocode", userid);
            if (!string.IsNullOrEmpty(busCode) && busCode != "undefined")
            {
                where += " and buscode='" + busCode + "'";
            }
            if (dicPar["StoNmae"] != null)
            {
                if (!string.IsNullOrEmpty(dicPar["StoNmae"].ToString()) && dicPar["StoNmae"].ToString() != "")
                {
                    where += " and (cname like '%" + dicPar["StoNmae"].ToString() + "%' or sname like '%" + dicPar["StoNmae"].ToString() + "%') ";
                }
            }
            dt = new bllStore().GetAllStore(where);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取交集的签送门店
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetUserIntersectSto(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "UserCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string touserid = dicPar["UserCode"].ToString();//取交集的用户id
            string userid = dicPar["userid"].ToString();//当前登陆用户id
            int recordCount = 0;
            int totalPage = 0;
            DataTable dtuser = new bllEmployee().GetEmpStoList("", "", 1, 1, "where t.userid=" + touserid, "", out recordCount, out totalPage);
            string loginUserRoleStoCode = ((Hashtable)HttpContext.Current.Cache.GetCache(userid + "CommunityBuy_LoginInfo"))[userid].ToString();
            string userRoleStoCode = string.Empty;
            if (dtuser.Rows[0]["sigstocodes"] != null)
            {
                userRoleStoCode = dtuser.Rows[0]["sigstocodes"].ToString();
            }
            if (string.IsNullOrEmpty(loginUserRoleStoCode))
            {
                loginUserRoleStoCode = userRoleStoCode;
            }
            string[] x = loginUserRoleStoCode.Split(',');
            string[] y = userRoleStoCode.Split(',');
            var stocode = x.Intersect(y).ToArray();
            if (stocode.Length > 0)
            {
                string filter = "where stocode in(";
                foreach (string code in stocode)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        filter += "'" + code + "',";
                    }
                }
                filter = filter.TrimEnd(',');
                filter += ")";
                dt = new bllStore().GetAllStore(filter);
            }

            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取连锁端门店信息,不分页
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetUserRoleIntersectSto(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "UserCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string touserid = dicPar["UserCode"].ToString();//取交集的用户id
            string userid = dicPar["userid"].ToString();//当前登陆用户id
            int recordCount = 0;
            int totalPage = 0;
            DataTable dtuser = new bllEmployee().GetEmpStoList("", "", 1, 1, "where t.userid=" + touserid, "", out recordCount, out totalPage);
            //string loginUserRoleStoCode = ((Hashtable)HttpContext.Current.Cache.GetCache(userid + "CommunityBuy_LoginInfo"))[userid].ToString();
            string userRoleStoCode = GetUserRoleCodes(touserid);
            //if (string.IsNullOrEmpty(loginUserRoleStoCode))
            //{
            //    loginUserRoleStoCode = userRoleStoCode;
            //}
            //string[] x = loginUserRoleStoCode.Split(',');
            //string[] y = userRoleStoCode.Split(',');
            //var stocode = x.Intersect(y).ToArray();
            //if (stocode.Length > 0)
            //{

            //    string filter = "where stocode in(";
            //    foreach (string code in stocode)
            //    {
            //        if (!string.IsNullOrEmpty(code))
            //        {
            //            filter += "'" + code + "',";
            //        }
            //    }
            //    filter = filter.TrimEnd(',');
            //    filter += ")";
            //    dt = new bllStore().GetAllStore(filter);
            //}
            if (!string.IsNullOrEmpty(userRoleStoCode))
            {
                string filter = "where stocode in(";
                foreach (string code in userRoleStoCode.Split(','))
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        filter += "'" + code + "',";
                    }
                }
                filter = filter.TrimEnd(',');
                filter += ")";
                dt = new bllStore().GetAllStore(filter);
            }
            else
            {
                dt = new bllStore().GetAllStore("where status='1'");
            }
            ReturnListJson(dt);
        }


        /// <summary>
        /// 获取连锁端门店信息,不分页
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetBusCodeToStoList(Dictionary<string, object> dicPar)
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
            string BusCode = string.Empty;
            if (dicPar.Keys.Contains("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            string where = "where 1=1";
            if (dicPar.ContainsKey("BusCode"))
            {
                where += "and buscode='" + dicPar["BusCode"].ToString() + "'";
            }
            if (dicPar.ContainsKey("StoNmae"))
            {
                if (!string.IsNullOrEmpty(dicPar["StoNmae"].ToString()))
                {
                    where += " and (cname like '%" + dicPar["StoNmae"].ToString() + "%' or sname like '%" + dicPar["StoNmae"].ToString() + "%') ";
                }
            }
            dt = new bllStore().GetAllStore(where);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取连锁端用户信息
        /// </summary>
        /// <param name="dicPar"></param>
        private void Detail(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "page", "limit", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            int pageSize = StringHelper.StringToInt(dicPar["limit"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0 && filter != "[]")
            {
                filter = JsonHelper.JsonToFilterByString(filter, out dtFilter);
                if (dtFilter != null)
                {
                    DataRow[] drArr = dtFilter.Select("cus<>''");
                    foreach (DataRow dr in drArr)
                    {
                        string col = dr["col"].ToString();
                        switch (col)
                        {
                            case "":
                                filter += "";
                                break;
                        }
                    }
                }
                filter += GetAuthoritywhere("a.stocode", userid);
            }
            else
            {
                filter = "where 1=1" + GetAuthoritywhere("a.stocode", userid);
            }
            filter = GetBusCodeWhere(dicPar, filter, "a.buscode");
            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/WSadmins.ashx";
            postStr.Append("actionname=getcateringweblist&parameters={" +
                                           string.Format("\"GUID\":\"{0}\"", GUID) +
                                           string.Format(",\"userid\": \"{0}\"", userid) +
                                           string.Format(",\"page\": \"{0}\"", currentPage) +
                                           string.Format(",\"limit\": \"{0}\"", pageSize) +
                                           string.Format(",\"filters\": \"{0}\"", filter) +
                                           string.Format(",\"orders\":\"{0}\"", order) +
                                   "}");//键值对
            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
            {
                string code = "";
                string msg = "";
                DataSet ds = JsonHelper.NewJsonToDataSet(strAdminJson, out code, out msg);
                if (code != "0")
                {
                    ToCustomerJson("2", "获取连锁端用户失败 x001");
                    return;
                }
                Pagcontext.Response.Write(strAdminJson);
            }
            else
            {
                ToCustomerJson("2", "获取连锁端用户失败 x003");
                return;
            }
        }

        private void GetStoreAuth(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "buscode", "stocode", "terminalcode", "remark" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string terminalcode = dicPar["terminalcode"].ToString();
            string remark= dicPar["remark"].ToString();
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/IStore/WSts_ManAuthorization.ashx";
            postStr.Append("actionname=checkloginbyterminalcode&parameters={" +
                                           string.Format("\"GUID\":\"{0}\"", GUID) +
                                           string.Format(",\"USER_ID\": \"{0}\"", userid) +
                                           string.Format(",\"buscode\": \"{0}\"", buscode) +
                                           string.Format(",\"stocode\": \"{0}\"", stocode) +
                                           string.Format(",\"terminalcode\": \"{0}\"", terminalcode) +
                                           string.Format(",\"remark\":\"{0}\"", remark) +
                                   "}");//键值对
            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            if (string.IsNullOrWhiteSpace(strAdminJson))
            {
                ToCustomerJson("-1", "联锁端检查失败");
                return;
            }
            string code = "";
            string msg = "";
            DataSet ds = JsonHelper.JsonToDataSet(strAdminJson, out code, out msg);
            ToCustomerJson(code, msg);
        }
    }
}