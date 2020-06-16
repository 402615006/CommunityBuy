using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.IServices
{
    /// <summary>
    /// 用户角色关系表接口类
    /// </summary>
    public class WS_TB_UserRole : ServiceBase
    {
        bllTB_UserRole bll = new bllTB_UserRole();
        DataTable dt = new DataTable();
        operatelogEntity logentity = new operatelogEntity();
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
                    logentity.module = "用户角色关系表";
                    switch (actionname.ToLower())
                    {
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
                        case "delete"://删除
                            Delete(dicPar);
                            break;
                        case "updatestatus"://修改状态
                            UpdateStatus(dicPar);
                            break;
                        case "getuseridtorole"://用户绑定角色时用
                            GetUserIdToRole(dicPar);
                            break;
                        case "getuseridtosig"://用户绑定角色时用
                            GetUserIdToSig(dicPar);
                            break;
                        case "getroleusernamelist"://获取指定门店指定角色类型的用户，不分页
                            GetRoleUserNameList(dicPar);
                            break;
                        case "getroleusernamereportlist":
                            GetRoleUserNameReportList(dicPar);
                            break;
                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "page", "limit", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            int pageSize = Helper.StringToInt(dicPar["limit"].ToString());
            int currentPage = Helper.StringToInt(dicPar["page"].ToString());
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
            filter = GetBusCodeWhere(dicPar, filter, "buscode");
            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            if (string.IsNullOrEmpty(order))
            {
                order = " order by ctime desc";
            }
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "RoleId", "UserId", "RealName", "EmpCode", "sigJson","RoleDisCount" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string Id = dicPar["Id"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string StrRoleId = dicPar["RoleId"].ToString();
            string UserId = dicPar["UserId"].ToString();
            string RoleDisCount = dicPar["RoleDisCount"].ToString();
            string sigJson = dicPar["sigJson"].ToString();
            if (UserId == "1")
            {
                Pagcontext.Response.Write(JsonHelper.ToJson("0", "操作成功"));
            }
            string RealName = dicPar["RealName"].ToString();
            string EmpCode = dicPar["EmpCode"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_UserRoleEdit.html";
            logentity.logcontent = "新增用户角色关系表信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Add(GUID, USER_ID, out Id, BusCode, StoCode, StrRoleId, UserId, RealName, EmpCode, sigJson, RoleDisCount, logentity);

            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "RoleId", "UserId", "RealName", "EmpCode", "RoleDisCount" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string Id = dicPar["Id"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string RoleId = dicPar["RoleId"].ToString();
            string UserId = dicPar["UserId"].ToString();
            string RealName = dicPar["RealName"].ToString();
            string EmpCode = dicPar["EmpCode"].ToString();
            string RoleDisCount = dicPar["RoleDisCount"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_UserRoleEdit.html";
            logentity.logcontent = "修改id为:" + Id + "的用户角色关系表信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Update(GUID, USER_ID, Id, BusCode, StoCode, RoleId, UserId, RealName, EmpCode, RoleDisCount, logentity);

            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "userid", "TotalMoney" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string Id = dicPar["Id"].ToString();
            string userid = dicPar["userid"].ToString();
            string TotalMoney = dicPar["TotalMoney"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where Id=" + Id);
            //获取当前用户的额角色
            DataTable dt_role = bll.GetRoleListUser(userid);
            DataTable dtStore = GetCacheToStore(userid);
            if (dt_role != null && dt_role.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_role.Rows)
                {
                    #region 获取折扣方案
                    DataTable dtDiscountScheme = new bllTB_DiscountScheme().GetTableInfo(GUID, userid, dr["StoCode"].ToString());
                    if (dr["RoleDisCount"] != null && !string.IsNullOrEmpty(dr["RoleDisCount"].ToString()))
                    {
                        string[] roleDisPKCode = dr["RoleDisCount"].ToString().Split(',');
                        string disWhere = string.Empty;
                        foreach (string pkcode in roleDisPKCode)
                        {
                            disWhere += "'" + pkcode + "',";
                        }
                        disWhere = disWhere.Trim(',');
                        DataRow[] drDiss = dtDiscountScheme.Select("PKCode in(" + disWhere + ")");
                        foreach (DataRow dr1 in drDiss)
                        {
                            dr["RoleDisCountName"] += dr["RoleDisCountName"].ToString() + "、" + dr1["SchName"].ToString();
                        }
                        dr["RoleDisCountName"] = dr["RoleDisCountName"].ToString().Trim('、');
                    }
                    #endregion
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
                dt_role.AcceptChanges();
            }
            //获取当前用户的签送方案
            DataTable dt_Sig = bll.GetUserSigList(" where uss.usercode='" + userid + "'");
            if (dt_Sig != null && dt_Sig.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_Sig.Rows)
                {
                    dr["TotalMoney"] = TotalMoney;
                }
                dt_Sig.AcceptChanges();
            }
            ArrayList arrData = new ArrayList();
            string[] arrTBName = new string[3] { "data1", "data2", "data3" };
            arrData.Add(dt);
            arrData.Add(dt_role);
            arrData.Add(dt_Sig);
            ReturnListJson("0", "", arrData, arrTBName);
            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string Id = dicPar["Id"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_UserRoleList.html";
            logentity.logcontent = "删除id为:" + Id + "的用户角色关系表信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Delete(GUID, USER_ID, Id, logentity);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "ids", "status" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ids = dicPar["ids"].ToString();
            string status = dicPar["status"].ToString();

            string Id = dicPar["ids"].ToString().Trim(',');
            logentity.pageurl = "TB_UserRoleList.html";
            logentity.logcontent = "修改状态id为:" + Id + "的用户角色关系表信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, Id, status);

            ReturnListJson(dt);
        }

        /// <summary>
        /// 用户绑定角色时使用
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetUserIdToRole(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "UserCode", "userid", "StoCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["UserCode"].ToString();
            string loginid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string filter = "where r.stocode ='" + StoCode + "'";
            dt = bll.GetRoleListUser(userid, filter);
            DataTable dtStore = GetCacheToStore(loginid);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    #region 获取折扣方案
                    DataTable dtDiscountScheme = new bllTB_DiscountScheme().GetTableInfo(GUID, loginid, dr["StoCode"].ToString());
                    if (dr["RoleDisCount"] != null && !string.IsNullOrEmpty(dr["RoleDisCount"].ToString()))
                    {
                        string[] roleDisPKCode = dr["RoleDisCount"].ToString().Split(',');
                        string disWhere = string.Empty;
                        foreach (string pkcode in roleDisPKCode)
                        {
                            disWhere += "'" + pkcode + "',";
                        }
                        disWhere = disWhere.Trim(',');
                        DataRow[] drDiss = dtDiscountScheme.Select("PKCode in(" + disWhere + ")");
                        foreach (DataRow dr1 in drDiss)
                        {
                            dr["RoleDisCountName"] += dr["RoleDisCountName"].ToString() + "、" + dr1["SchName"].ToString();
                        }
                        dr["RoleDisCountName"] = dr["RoleDisCountName"].ToString().Trim('、');
                    }
                    #endregion
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
                dt.AcceptChanges();
            }
            ReturnListJson(dt);
        }

        /// <summary>
        /// 修改时获取该用户的签送方案
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetUserIdToSig(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "userid", "UserCode", "StoCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string userid = dicPar["userid"].ToString();
            string buscode = GetCacheToUserBusCode(userid);
            string usercode = dicPar["UserCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            DataTable dtuser = new bllEmployee().GetEmpStoList("", "", 1, 1, "where t.userid=" + usercode, "", out recordCount, out totalPage);
            decimal TotalMoney = Helper.StringToDecimal(dtuser.Rows[0]["msigmoney"].ToString());
            string filter = "where ss.stocode ='" + StoCode + "'";
            dt = bll.GetUserSigList(filter);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["TotalMoney"] = TotalMoney;
                }
                dt.AcceptChanges();
            }
            ReturnListJson(dt);
        }

        private void GetRoleUserNameList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "stocode", "roletype"};

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string stocode = dicPar["stocode"].ToString();
            string roletype = dicPar["roletype"].ToString();
            //调用逻辑
            dt = bll.GetRoleUserNameList(stocode,roletype);
            if(dt!=null && dt.Rows.Count>0)
            {
                DataTable dtUser = new bllEmployee().GetAllAdmin();
                foreach (DataRow dr in dt.Rows)
                {
                    string userid = dr["UserId"].ToString();
                    if (dtUser.Select("userid='" + userid + "'").Length > 0)
                    {
                        DataRow dr_sto = dtUser.Select("userid='" + userid + "'")[0];
                        dr["ucname"] = dr_sto["uname"].ToString();
                    }
                }
            }
            ReturnListJson(dt);
        }

        /// <summary>
        /// 班次统计报表获取收银员,包含系统默认的小程序
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetRoleUserNameReportList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "stocode", "roletype" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string stocode = dicPar["stocode"].ToString();
            string roletype = dicPar["roletype"].ToString();
            //调用逻辑
            dt = bll.GetRoleUserNameList(stocode, roletype);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtUser = new bllEmployee().GetAllAdmin();
                foreach (DataRow dr in dt.Rows)
                {
                    string userid = dr["UserId"].ToString();
                    if (dtUser.Select("userid='" + userid + "'").Length > 0)
                    {
                        DataRow dr_sto = dtUser.Select("userid='" + userid + "'")[0];
                        dr["ucname"] = dr_sto["uname"].ToString();
                    }
                }
                DataRow newdr = dt.NewRow();
                newdr["UserId"] = "0";
                newdr["RealName"] = "小程序";
                newdr["RoleName"] = "小程序";
                newdr["ucname"] = "小程序";
                dt.Rows.Add(newdr);
            }
            ReturnListJson(dt);
        }

    }
}