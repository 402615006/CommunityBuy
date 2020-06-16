using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.IServices
{
    /// <summary>
    /// 菜品类别表接口类
    /// </summary>
    public class WS_TB_DishType : ServiceBase
    {
        bllTB_DishType bll = new bllTB_DishType();
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
                    logentity.module = "菜品类别表";
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
                        case "getdistypetreelistinfo"://菜品类别管理树型数据获取
                            GetDisTypeTreeListInfo(dicPar);
                            break;
                        case "getdistypeonelistinfo":
                            GetDisTypeOneListInfo(dicPar);//获取一级类别
                            break;
                        case "getlistforservice":
                            GetListForService(dicPar);
                            break;
                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "page", "limit", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
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
                        string val = dr["filter"].ToString();
                        switch (col)
                        {
                            case "IsKeep":
                                filter += " and pkcode in (select distinct typecode from tb_dish where iskeep='1')";
                                break;
                            case "Depart":
                                filter += " and PKCode in(select distinct TypeCode from TB_Dish where MenuCode in(select PKCode from TB_DishMenu where DepCode = '"+ val + "')) or PKCode in(select distinct[dbo].[fn_GetDisParentTypeCode](DisCode, StoCode)  from TB_Dish where MenuCode in(select PKCode from TB_DishMenu where DepCode = '"+ val + "'))";
                                break;
                        }
                    }
                }
                if (!filter.Contains("StoCode") && !filter.Contains("stocode"))
                {
                    filter += GetAuthoritywhere("stocode", userid);
                }
            }
            else
            {
                filter = "where 1=1" + GetAuthoritywhere("stocode", userid);
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
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtStore = GetCacheToStore(userid);
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string stocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + stocode + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }

                    }
                    dt.AcceptChanges();
                }
            }
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void GetListForService(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "stocode", "depcode","iskeep" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string depcode = dicPar["depcode"].ToString();
            string iskeep = dicPar["iskeep"].ToString();

            //调用逻辑
            string sql = "dbo.p_getDisTypeInfo";
            SqlParameter[] sqlParameters =
            {
                    new SqlParameter("@stocode ", stocode),
                    new SqlParameter("@depcode",depcode),
                    new SqlParameter("@isstock ", iskeep)
             };
            dt = new bllPaging().GetDataTableInfoByProcedure(sql, sqlParameters);
            ReturnListJson(dt, 1, 0, 1, 0);
        }



        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCname", "PKKCode", "PKCode", "TypeName", "Sort", "TStatus", "CCode" };
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
            string CCname = dicPar["CCname"].ToString();
            string PKKCode = dicPar["PKKCode"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string TypeName = dicPar["TypeName"].ToString();
            string Sort = dicPar["Sort"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string CCode = dicPar["CCode"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_DishTypeEdit.html";
            logentity.logcontent = "新增菜品类别表信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Add(GUID, USER_ID, Id, BusCode, StoCode, CCname, PKKCode, PKCode, TypeName, Sort, TStatus, CCode, logentity);

            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCname", "PKKCode", "PKCode", "TypeName", "Sort", "TStatus", "CCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string Id = "0";
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string CCname = dicPar["CCname"].ToString();
            string PKKCode = dicPar["PKKCode"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string TypeName = dicPar["TypeName"].ToString();
            string Sort = dicPar["Sort"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string CCode = dicPar["CCode"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_DishTypeEdit.html";
            logentity.logcontent = "修改id为:" + PKCode + "的菜品类别表信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Update(GUID, USER_ID, Id, BusCode, StoCode, CCname, PKKCode, PKCode, TypeName, Sort, TStatus, CCode, logentity);

            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "PKCode","stocode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where PKCode='" + PKCode + "' and stocode='"+stocode+"'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtStore = GetCacheToStore(userid);
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string _stocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + _stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + _stocode + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }

                    }
                    dt.AcceptChanges();
                }
            }
            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string PKCode = dicPar["id"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_DishTypeList.html";
            logentity.logcontent = "删除id为:" + PKCode + "的菜品类别表信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Delete(GUID, USER_ID, PKCode, logentity);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "id", "status" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ids = dicPar["id"].ToString();
            string status = dicPar["status"].ToString();

            string PKCode = dicPar["id"].ToString().Trim(',');
            logentity.pageurl = "TB_DishTypeList.html";
            logentity.logcontent = "修改状态id为:" + PKCode + "的菜品类别表信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, PKCode, status);

            ReturnListJson(dt);
        }

        /// <summary>
        /// 菜品类别管理树型数据获取
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetDisTypeTreeListInfo(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0)
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
                if (!filter.Contains("StoCode") && !filter.Contains("stocode"))
                {
                    filter += GetAuthoritywhere("stocode", userid);
                }
            }
            else
            {
                filter = "where 1=1" + GetAuthoritywhere("stocode", userid);
            }
            filter = GetBusCodeWhere(dicPar, filter, "buscode");

            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            //调用逻辑
            dt = bll.GetDisTypeTreeListInfo(GUID, USER_ID, filter, order);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtStore = GetCacheToStore(userid);
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string stocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + stocode + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }

                    }
                    dt.AcceptChanges();
                }
            }
            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取类别一级类别
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetDisTypeOneListInfo(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0)
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
                if (!filter.Contains("StoCode") && !filter.Contains("stocode"))
                {
                    filter += GetAuthoritywhere("stocode", userid);
                }
            }
            else
            {
                filter = "where 1=1" + GetAuthoritywhere("stocode", userid);
            }
            filter = GetBusCodeWhere(dicPar, filter, "buscode");

            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }

            //调用逻辑
            dt = bll.GetDisTypeOneListInfo(GUID, USER_ID, filter, order);
            ReturnListJson(dt);
        }
    }
}