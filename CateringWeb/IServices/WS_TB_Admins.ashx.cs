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
                    switch (actionname.ToLower())
                    {
                        case "loginbypassword"://登录
                            LoginByPassword(dicPar);
                            break;
                        case "detail"://修改时获取明细，包含门店角色信息
                            Detail(dicPar);
                            break;
                        case "getlistall":
                            GetListAll(dicPar);
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
            filter += " and status='1' ";
            order = " py asc";
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
           //dt =new bllEmployee().GetCustomerManager(currentPage,pageSize, filter, order, out recordCount, out totalPage);
           // ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
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
                string uname = dicPar["uname"].ToString();
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
                        ReturnResultJson("2", "验证失败");
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
                            DataTable dtEmployee = new bllAdmins().GetPagingListInfo(dtAdmin.Rows[0]["GUID"].ToString(), dtAdmin.Rows[0]["userid"].ToString(), int.MaxValue, 1, "t.userid=" + dtAdmin.Rows[0]["userid"].ToString(), "", out nums, out nums);
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
                            ReturnResultJson("3", "该用户没有该门店的权限");
                            return;
                        }
                        DataRow dr = dtAdmin.Rows[0];
                        dtAdmin.Rows[0]["GUID"] = Guid.NewGuid().ToString();
                        //添加登陆记录，如果存在则为修改，主要作用与单点登陆
                        string id = "0";
                        if (HttpContext.Current.Cache.Get("empcodesing" +stocode+ depart + dtAdmin.Rows[0]["uname"].ToString())!=null)
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
                            ReturnResultJson("3", "该用户没有角色信息，无法登陆");
                            return;
                        }
                        HttpContext.Current.Cache.Insert(dr["userid"].ToString() + "1", RoleIds);//保存用户角色ID到缓存中，多个使用，分隔
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                ReturnResultJson("2", ex.Message);
                return;
            }
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
            dt = new bllAdmins().GetPagingListInfo(GUID, USER_ID, 1, 1, "where t.userid=" + userid, "", out recordCount, out totalPage);

            #region 信息
            DataTable dtUserRole = new bllTB_UserRole().GetRoleListUser(userid, "");
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
            string[] arrTBName = new string[2] { "data1", "data2"};
            arrData.Add(dt);
            arrData.Add(dtUserRole);
            ReturnManyListJson("0", "", arrData, arrTBName,null,null,null,null);
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

            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            int recordCount = 0;
            int totalPage = 0;
            dt = new bllAdmins().GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
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

    }
}