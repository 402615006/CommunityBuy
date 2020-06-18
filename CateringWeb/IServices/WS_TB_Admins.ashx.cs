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
    /// WS_TB_Admins 的摘要说明
    /// </summary>
    public class WS_TB_Admins : ServiceBase
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
                    logentity.module = "用户信息表";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "login"://登录
                            Login(dicPar);
                            break;
                        case "loginbypassword"://登录
                            LoginByPassword(dicPar);
                            break;
                        case "detail"://修改时获取明细，包含门店角色信息
                            Detail(dicPar);
                            break;
                        case "getlistall":
                            GetListAll(dicPar);
                            break;
                        case "authlogin":
                            AuthLoginByPassword(dicPar);
                            break;
                        case "getlistsearch"://提成人
                            GetListSearch(dicPar);
                            break;
                    }
                }
            }
        }

        private void GetListSearch(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "limit","page", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            int pageSize = StringHelper.StringToInt(dicPar["limit"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            string order = "";
            string name = "";
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
                            case "name":
                                name= dr["filter"].ToString();
                                break;
                        }
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(filter))
            {
                filter = "where 1=1 ";
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                filter +=string.Format(" and (PY like('%{0}%') or realname like('%{0}%')) ", name);
            }
            filter = GetBusCodeWhere(dicPar, filter, "buscode");

            filter += " and status='1' ";
            order = " py asc";
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
           dt =new bllEmployee().GetCustomerManager(currentPage,pageSize, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }


        /// <summary>
        /// 免登陆验证
        /// </summary>
        /// <param name="dicPar"></param>
        private void Login(Dictionary<string, object> dicPar)
        {
            try
            {
                //要检测的参数信息
                List<string> pra = new List<string>() { "uname", "userid", "tertype" };
                //检测方法需要的参数
                if (!CheckActionParameters(dicPar, pra))
                {
                    return;
                }
                StringBuilder postStr = new StringBuilder();
                //获取参数信息
                string uname = dicPar["uname"].ToString();
                string userid = dicPar["userid"].ToString();
                string tertype = dicPar["tertype"].ToString();
                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/WSadmins.ashx";
                postStr.Append("actionname=login&parameters={" +
                                               string.Format("'uname':'{0}'", uname) +
                                               string.Format(",'userid': '{0}'", userid) +
                                               string.Format(",'tertype': '{0}'", tertype) +
                                       "}");//键值对
                string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
                {
                    string status = "";
                    string mes = "";
                    DataSet ds = JsonHelper.NewJsonToDataSet(strAdminJson, out status, out mes);
                    if (status != "0")
                    {
                        ToCustomerJson("2", "验证失败");
                        return;
                    }
                    DataTable dtAdmin = ds.Tables["data"];
                    if (dtAdmin != null && dtAdmin.Rows.Count > 0)
                    {
                        DataRow dr = dtAdmin.Rows[0];
                        dtAdmin.Rows[0]["GUID"] = Guid.NewGuid().ToString();
                        dtAdmin.Rows[0]["upwd"] = "";
                        dtAdmin.AcceptChanges();

                        //清除之前的缓存信息
                        if (HttpContext.Current.Cache.Get(dr["userid"].ToString()+"CommunityBuy_LoginInfo")!=null)
                        {
                            HttpContext.Current.Cache.Remove(dr["userid"].ToString()+"CommunityBuy_LoginInfo");
                        }
                        if (HttpContext.Current.Cache.Get(dr["userid"].ToString() + tertype)!=null)
                        {
                            HttpContext.Current.Cache.Remove(dr["userid"].ToString() + tertype);
                        }
                        if (HttpContext.Current.Cache.Get(dr["userid"].ToString() + "_buscode")!=null)
                        {
                            HttpContext.Current.Cache.Remove(dr["userid"].ToString() + "_buscode");
                        }
                        SetUserLoginRoleCodeToCache(dr["userid"].ToString(), dr["rolecode"].ToString());//缓存用户门店权限
                        //获取用户的门店下的角色
                        DataTable dtUserRole = new bllTB_UserRole().GetUserToRole(dr["userid"].ToString());
                        string RoleIds = string.Empty;
                        if (dtUserRole != null && dtUserRole.Rows.Count > 0)
                        {
                            RoleIds = dtUserRole.Rows[0]["roleids"].ToString();
                        }
                        HttpContext.Current.Cache.Insert(dr["userid"].ToString() + tertype, RoleIds);//保存用户角色ID到缓存中，多个使用，分隔
                        SetUserBusCodeToCache(dr["userid"].ToString(), dr["buscode"].ToString());//缓存用户商户编号
                        ReturnListJson(dtAdmin);
                    }
                    else
                    {
                        ToCustomerJson("2", "验证失败");
                        return;
                    }
                }
                else
                {
                    ToCustomerJson("2", "验证失败");
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                ToCustomerJson("2", ex.Message);
                return;
            }

        }

        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="dicPar"></param>
        private void LoginByPassword(Dictionary<string, object> dicPar)
        {
            try
            {
                //要检测的参数信息
                List<string> pra = new List<string>() { "uname", "password","depart","strcode" };
                //检测方法需要的参数
                if (!CheckActionParameters(dicPar, pra))
                {
                    return;
                }
                StringBuilder postStr = new StringBuilder();
                //获取参数信息
                string uname = Helper.ReplaceString(dicPar["uname"].ToString());
                string password = dicPar["password"].ToString();
                string depart = dicPar["depart"].ToString();
                string stocode= dicPar["strcode"].ToString();
                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/WSadmins.ashx";
                postStr.Append("actionname=loginbypassword&parameters={" +
                                                string.Format("'GUID':'{0}'", "") +
                                                string.Format(",'USER_ID': '{0}'", "") +
                                                string.Format(",'uname': '{0}'", uname) +
                                                string.Format(",'password': '{0}'", password) +
                                        "}");//键值对
                string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
                {
                    string status = "";
                    string mes = "";
                    DataSet ds = JsonHelper.NewJsonToDataSet(strAdminJson, out status, out mes);
                    if (status != "0")
                    {
                        ToCustomerJson("2", "验证失败");
                        return;
                    }
                    DataTable dtAdmin = ds.Tables["data"];
                    DataTable dtReturn = dtAdmin.Clone();
                    dtReturn.Columns.Add("RoleType");
                    if (dtAdmin != null && dtAdmin.Rows.Count > 0)
                    {
                        string adminStocode = "";
                        //判断用户的门店
                        if (dtAdmin.Rows[0]["scope"].ToString() == "2")
                        {
                            //从emp表中获取权限门店
                            int nums = 0;
                            DataTable dtEmployee = new bllEmployee().GetEmpStoList(dtAdmin.Rows[0]["GUID"].ToString(), dtAdmin.Rows[0]["userid"].ToString(), int.MaxValue, 1, "t.userid=" + dtAdmin.Rows[0]["userid"].ToString(), "", out nums, out nums);
                            if (dtEmployee != null && dtEmployee.Rows.Count > 0)
                            {
                                adminStocode = dtEmployee.Rows[0]["empstocode"].ToString();
                            }
                        }
                        else
                        {
                            adminStocode = dtAdmin.Rows[0]["stocode"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(stocode) &&!adminStocode.Contains(stocode))
                        {
                            ToCustomerJson("3", "该用户没有该门店的权限");
                            return;
                        }


                        DataRow dr = dtAdmin.Rows[0];
                        dtAdmin.Rows[0]["GUID"] = Guid.NewGuid().ToString();

                        SetUserLoginRoleCodeToCache(dr["userid"].ToString(), dr["rolecode"].ToString());//缓存用户门店权限
                        SetUserBusCodeToCache(dr["userid"].ToString(), dr["buscode"].ToString());//缓存用户商户编号

                        //添加登陆记录，如果存在则为修改，主要作用与单点登陆
                        string id = "0";
                        new bllTB_SingleLogin().Add(dtAdmin.Rows[0]["GUID"].ToString(), "", out id, dr["buscode"].ToString(), stocode, dtAdmin.Rows[0]["uname"].ToString(), dtAdmin.Rows[0]["uname"].ToString(), "1", depart, null);
                        if (HttpContext.Current.Cache.IsExist("empcodesing" +stocode+ depart + dtAdmin.Rows[0]["uname"].ToString()))
                        {
                            HttpContext.Current.Cache.Remove("empcodesing" + stocode + depart + dtAdmin.Rows[0]["uname"].ToString());
                        }
                        HttpContext.Current.Cache.Insert("empcodesing"+ stocode + depart + dtAdmin.Rows[0]["uname"].ToString(), dtAdmin.Rows[0]["GUID"].ToString());
                        
                        //获取用户的门店下的角色
                        DataTable dtUserRole = new bllTB_UserRole().GetUserStoreRole(dr["userid"].ToString());
                        string RoleIds = string.Empty;
                        string RoleTypes = string.Empty;
                        if (dtUserRole != null)
                        {
                            foreach (DataRow drRole in dtUserRole.Rows)
                            {
                                RoleIds += drRole["id"].ToString() + ",";
                                RoleTypes += "," + drRole["RoleType"].ToString() + ",";
                            }
                        }
                        DataRow drAdd = dtReturn.NewRow();
                        foreach (DataColumn dc in dtAdmin.Columns)
                        {
                            drAdd[dc.ColumnName] = dr[dc.ColumnName];
                        }
                        drAdd["RoleType"] = RoleTypes;
                        dtReturn.Rows.Add(drAdd);
                        if (string.IsNullOrEmpty(RoleIds))
                        {
                            ToCustomerJson("3", "该用户没有角色信息，无法登陆");
                            return;
                        }


                        HttpContext.Current.Cache.Insert(dr["userid"].ToString() + "1", RoleIds);//保存用户角色ID到缓存中，多个使用，分隔
                        SetUserBusCodeToCache(dr["userid"].ToString(), dr["buscode"].ToString());//缓存用户商户编号
                        ReturnListJson(dtReturn);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                ToCustomerJson("2", ex.Message);
                return;
            }
        }

        /// <summary>
        /// 获取连锁端用户信息-区分门店的
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "page", "limit", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
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
                            case "sig":
                                filter += " and (select count(1) from TB_UserSigScheme where UserCode=t.userid)>0";
                                break;
                            case "DisCount":
                                filter += " and (select count(1) from TB_UserDiscountScheme where UserCode=t.userid and and len(isnull(InsideCode,''))=0)>0";
                                break;
                        }
                    }
                }
                if (!filter.Contains("strcode"))
                {
                    filter += GetAuthoritywhere("e.strcode", userid);
                }
                filter += " and t.status='1' ";
            }
            else
            {
                filter = "where t.status='1'";
            }
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            filter = GetBusCodeWhere(dicPar, filter, "t.buscode");

            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            if (string.IsNullOrEmpty(order))
            {
                order = " order by t.ctime desc";
            }
            int recordCount = 0;
            int totalPage = 0;
            dt = new bllEmployee().GetEmpStoList(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            DataTable dtLocalUserRoleName = new bllTB_UserRole().GetUserRoleNameList(BusCode);
            DataTable dtLocalUserUserDiscountScheme = new bllTB_DiscountScheme().GetUserDiscountScheme(BusCode);
            if (dt != null && dt.Rows.Count > 0 && dtLocalUserRoleName != null && dtLocalUserRoleName.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["rolename"] = "";
                    string auserid = dr["userid"].ToString();
                    DataRow[] rdrs = dtLocalUserRoleName.Select("UserId='" + auserid + "'");
                    foreach (DataRow rdr in rdrs)
                    {
                        dr["rolename"] += rdr["RoleName"].ToString() + "、";
                    }
                    dr["rolename"] = dr["rolename"].ToString().TrimEnd('、');
                }
            }
            if (dt != null && dt.Rows.Count > 0 && dtLocalUserUserDiscountScheme != null && dtLocalUserUserDiscountScheme.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string auserid = dr["userid"].ToString();
                    DataRow[] rdrs = dtLocalUserUserDiscountScheme.Select("usercode='" + auserid + "'");
                    foreach (DataRow rdr in rdrs)
                    {
                        dr["RoleDisCount"] += rdr["DisCountCode"].ToString() + ",";
                        dr["RoleDisCountName"] += rdr["SchName"].ToString() + ",";
                    }
                    dr["RoleDisCount"] = dr["RoleDisCount"].ToString().TrimEnd(',');
                    dr["RoleDisCountName"] = dr["RoleDisCountName"].ToString().TrimEnd(',');
                }
            }
            if(dt!=null)
            {
                dt.AcceptChanges();
            }

            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        /// <summary>
        /// 获取连锁端用户信息
        /// </summary>
        /// <param name="dicPar"></param>
        private void Detail(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid"};

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            int recordCount = 0;
            int totalPage = 0;
            dt = new bllEmployee().GetEmpStoList(GUID, USER_ID, 1, 1, "where t.userid=" + userid + "", "", out recordCount, out totalPage);

            #region 信息
            DataTable dtUserRole = new bllTB_UserRole().GetRoleListUser(userid, "");
            DataTable dtStore = GetCacheToStore(userid);
            if (dtUserRole != null && dtUserRole.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUserRole.Rows)
                {
                    #region 门店
                    if (dtStore != null && dtStore.Rows.Count > 0)
                    {
                        string sto = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + sto + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + sto + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }
                    }
                    #endregion
                }
                dtUserRole.AcceptChanges();
            }
            #endregion

            #region 签送方案
            int recordCount1 = 0;
            int totalPage1 = 0;
            DataTable dtuser = new bllEmployee().GetEmpStoList("", "", 1, 1, "where t.userid=" + userid, "", out recordCount1, out totalPage1);
            decimal TotalMoney = StringHelper.StringToDecimal(dtuser.Rows[0]["msigmoney"].ToString());
            DataTable dt_Sig = new bllTB_UserRole().GetUserSigList(" where uss.usercode='" + userid + "'");
            if (dt_Sig != null && dt_Sig.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_Sig.Rows)
                {
                    dr["TotalMoney"] = TotalMoney;
                }
                dt_Sig.AcceptChanges();
            }

            #endregion

            #region 折扣方案
            DataTable dtLocalUserUserDiscountScheme = new bllTB_DiscountScheme().GetUserDiscountScheme(BusCode);
            if (dt != null && dt.Rows.Count > 0 && dtLocalUserUserDiscountScheme != null && dtLocalUserUserDiscountScheme.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string auserid = dr["userid"].ToString();
                    DataRow[] rdrs = dtLocalUserUserDiscountScheme.Select("usercode='" + auserid + "'");
                    foreach (DataRow rdr in rdrs)
                    {
                        dr["RoleDisCount"] += rdr["DisCountCode"].ToString() + ",";
                        dr["RoleDisCountName"] += rdr["SchName"].ToString() + ",";
                    }
                    dr["RoleDisCount"] = dr["RoleDisCount"].ToString().TrimEnd(',');
                    dr["RoleDisCountName"] = dr["RoleDisCountName"].ToString().TrimEnd(',');
                }
            }
            #endregion

            DataTable dtLocalUserRoleName = new bllTB_UserRole().GetUserRoleNameList(BusCode);
            if (dt != null && dt.Rows.Count > 0 && dtLocalUserRoleName != null && dtLocalUserRoleName.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["rolename"] = "";
                    string auserid = dr["userid"].ToString();
                    DataRow[] rdrs = dtLocalUserRoleName.Select("UserId='" + auserid + "'");
                    foreach (DataRow rdr in rdrs)
                    {
                        dr["rolename"] += rdr["RoleName"].ToString() + "、";
                    }
                    dr["rolename"] = dr["rolename"].ToString().TrimEnd('、');
                }
            }
            dt.AcceptChanges();
            ArrayList arrData = new ArrayList();
            string[] arrTBName = new string[3] { "data1", "data2", "data3" };
            arrData.Add(dt);
            arrData.Add(dtUserRole);
            arrData.Add(dt_Sig);
            ReturnListJson("0", "", arrData, arrTBName);
        }


        /// <summary>
        /// 员工卡登录
        /// </summary>
        /// <param name="dicPar">门店、设备ip、用户名、卡号、设备编号Mac、计算机名称</param>
        private void LoginByCard(Dictionary<string, object> dicPar)
        {
            try
            {
                //要检测的参数信息
                List<string> pra = new List<string>() { "stocode", "stip", "uname", "cardcode", "devicecode", "hostname" };
                //检测方法需要的参数
                if (!CheckActionParameters(dicPar, pra))
                {
                    return;
                }
                StringBuilder postStr = new StringBuilder();
                //获取参数信息
                string uname = Helper.ReplaceString(dicPar["uname"].ToString());
                string stip = Helper.ReplaceString(dicPar["stip"].ToString());
                string stocode = Helper.ReplaceString(dicPar["stocode"].ToString());
                string cardcode = Helper.ReplaceString(dicPar["cardcode"].ToString());
                string devicecode = dicPar["devicecode"].ToString();
                string hostname = dicPar["hostname"].ToString();
                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/WSadmins.ashx";
                postStr.Append("actionname=loginbycard&parameters={" +
                                               string.Format(",'uname': '{0}'", uname) +
                                                string.Format(",'stip': '{0}'", stip) +
                                                 string.Format(",'stocode': '{0}'", stocode) +
                                                  string.Format(",'stocode': '{0}'", stocode) +
                                                   string.Format(",'devicecode': '{0}'", devicecode) +
                                                string.Format(",'hostname': '{0}'", hostname) +
                                       "}");//键值对
                string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
                {
                    string status = "";
                    string mes = "";
                    DataSet ds = JsonHelper.NewJsonToDataSet(strAdminJson, out status, out mes);
                    if (status != "0")
                    {
                        ToCustomerJson("2", "验证失败");
                        return;
                    }
                    DataTable dtAdmin = ds.Tables["data"];
                    if (dtAdmin != null && dtAdmin.Rows.Count > 0)
                    {
                        DataRow dr = dtAdmin.Rows[0];
                        dtAdmin.Rows[0]["GUID"] = Guid.NewGuid().ToString();
                        SetUserLoginRoleCodeToCache(dr["userid"].ToString(), dr["rolecode"].ToString());//缓存用户门店权限
                        SetUserBusCodeToCache(dr["userid"].ToString(), dr["buscode"].ToString());//缓存用户商户编号
                        DataTable dtUserRole = new bllTB_UserRole().GetUserToRole(dr["userid"].ToString());
                        string RoleIds = string.Empty;
                        if (dtUserRole != null && dtUserRole.Rows.Count > 0)
                        {
                            RoleIds = dtUserRole.Rows[0]["roleids"].ToString();
                        }
                        HttpContext.Current.Cache.Insert(dr["userid"].ToString() + "1", RoleIds);//保存用户角色ID到缓存中，多个使用，分隔
                        SetUserBusCodeToCache(dr["userid"].ToString(), dr["buscode"].ToString());//缓存用户商户编号
                        ReturnListJson(dtAdmin);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                ToCustomerJson("2", ex.Message);
                return;
            }
        }


        /// <summary>
        /// 获取连锁端用户信息-不分门店
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetListAll(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "page", "limit", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
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
            }
            else
            {
                filter = string.Empty;
            }
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            filter = GetBusCodeWhere(dicPar, filter, "t.buscode");

            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            int recordCount = 0;
            int totalPage = 0;
            dt = new bllEmployee().GetEmpStoList(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            DataTable dtLocalUserRoleName = new bllTB_UserRole().GetUserRoleNameList(BusCode);
            if (dt != null && dt.Rows.Count > 0 && dtLocalUserRoleName != null && dtLocalUserRoleName.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["rolename"] = "";
                    string auserid = dr["userid"].ToString();
                    DataRow[] rdrs = dtLocalUserRoleName.Select("UserId='" + auserid + "'");
                    foreach (DataRow rdr in rdrs)
                    {
                        dr["rolename"] += rdr["RoleName"].ToString() + "、";
                    }
                    dr["rolename"] = dr["rolename"].ToString().TrimEnd('、');
                }
            }
            dt.AcceptChanges();
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        /// <summary>
        /// 用户授权登录
        /// </summary>
        /// <param name="dicPar"></param>
        private void AuthLoginByPassword(Dictionary<string, object> dicPar)
        {
            try
            {
                //要检测的参数信息
                List<string> pra = new List<string>() { "uname", "password", "depart", "strcode" };
                //检测方法需要的参数
                if (!CheckActionParameters(dicPar, pra))
                {
                    return;
                }
                StringBuilder postStr = new StringBuilder();
                //获取参数信息
                string uname = dicPar["uname"].ToString();
                string password = dicPar["password"].ToString();
                string depart = dicPar["depart"].ToString();
                string stocode = dicPar["strcode"].ToString();
                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/WSadmins.ashx";
                postStr.Append("actionname=loginbypassword&parameters={" +
                                               string.Format("'GUID':'{0}'", "") +
                                               string.Format(",'USER_ID': '{0}'", "") +
                                               string.Format(",'uname': '{0}'", uname) +
                                                string.Format(",'password': '{0}'", password) +
                                       "}");//键值对
                string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
                {
                    string status = "";
                    string mes = "";
                    DataSet ds = JsonHelper.NewJsonToDataSet(strAdminJson, out status, out mes);
                    if (status != "0")
                    {
                        ToCustomerJson("2", "验证失败");
                        return;
                    }
                    DataTable dtAdmin = ds.Tables["data"];
                    DataTable dtReturn = dtAdmin.Clone();
                    dtReturn.Columns.Add("RoleType");
                    if (dtAdmin != null && dtAdmin.Rows.Count > 0)
                    {
                        DataRow dr = dtAdmin.Rows[0];
                        dtAdmin.Rows[0]["GUID"] = Guid.NewGuid().ToString();
                        //获取用户的门店下的角色
                        DataTable dtUserRole = new bllTB_UserRole().GetUserStoreRole(dr["userid"].ToString());
                        string RoleIds = string.Empty;
                        string RoleTypes = string.Empty;
                        if (dtUserRole != null)
                        {
                            foreach (DataRow drRole in dtUserRole.Rows)
                            {
                                string rolestore = drRole["RoleStore"].ToString();
                                bool contains = false;

                                string[] userstores = stocode.Split(',');
                                string[] rolestores = rolestore.Split(',');
                                foreach (string us in userstores)
                                {
                                    foreach (string rs in rolestores)
                                    {
                                        if (us == rs)
                                        {
                                            contains = true;
                                            break;
                                        }
                                    }
                                    if (contains)
                                    {
                                        break;
                                    }
                                }
                                if (contains)
                                {
                                    RoleIds += drRole["id"].ToString() + ",";
                                    RoleTypes += "," + drRole["RoleType"].ToString() + ",";
                                }
                            }
                        }
                        DataRow drAdd = dtReturn.NewRow();
                        foreach (DataColumn dc in dtAdmin.Columns)
                        {
                            drAdd[dc.ColumnName] = dr[dc.ColumnName];
                        }
                        drAdd["RoleType"] = RoleTypes;
                        dtReturn.Rows.Add(drAdd);

                        HttpContext.Current.Cache.Insert(dr["userid"].ToString() + "1", RoleIds);//保存用户角色ID到缓存中，多个使用，分隔
                        ReturnListJson(dtReturn);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                ToCustomerJson("2", ex.Message);
                return;
            }
        }

    }
}