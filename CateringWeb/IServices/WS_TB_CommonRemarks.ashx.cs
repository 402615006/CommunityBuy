using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.IServices
{
    /// <summary>
    /// 常用备注管理接口类
    /// </summary>
    public class WS_TB_CommonRemarks : ServiceBase
    {
        bllTB_CommonRemarks bll = new bllTB_CommonRemarks();
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
                    logentity.module = "常用备注管理";
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

            }
            else
            {
                filter = "";
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
            DataTable dtType = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.NoteType));
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dtType != null && dtType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string typecode = dr["RType"].ToString();
                        if (dtType.Select("enumcode='" + typecode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtType.Select("enumcode='" + typecode + "'")[0];
                            dr["RTypeName"] = dr_sto["enumname"].ToString();
                        }
                    }
                }
                dt.AcceptChanges();
            }
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);

        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BusCode", "StoCode", "CCname", "TStatus", "Sort", "PKCode", "Remark", "RType", "CCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = Helper.GetAppSettings("StoCode");
            string CCname = dicPar["CCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string Sort = dicPar["Sort"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string Remark = dicPar["Remark"].ToString();
            string RType = dicPar["RType"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_CommonRemarksEdit.html";
            logentity.logcontent = "新增常用备注管理信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Add(GUID, USER_ID, BusCode, StoCode, CCname, TStatus, Sort, PKCode, Remark, RType, CCode, logentity);

            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCname", "TStatus", "Sort", "PKCode", "Remark", "RType", "CCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = Helper.GetAppSettings("StoCode");
            string CCname = dicPar["CCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string Sort = dicPar["Sort"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string Remark = dicPar["Remark"].ToString();
            string RType = dicPar["RType"].ToString();

            //调用逻辑
            logentity.pageurl = "TB_CommonRemarksEdit.html";
            logentity.logcontent = "修改id为:" + PKCode + "的常用备注管理信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Update(GUID, USER_ID, BusCode, StoCode, CCname, TStatus, Sort, PKCode, Remark, RType, CCode, logentity);

            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "PKCode" };
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
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where PKCode='" + PKCode + "'");
            DataTable dtType = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.NoteType));
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
                }
                if (dtType != null && dtType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string typecode = dr["RType"].ToString();
                        if (dtType.Select("enumcode='" + typecode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtType.Select("enumcode='" + typecode + "'")[0];
                            dr["RTypeName"] = dr_sto["enumname"].ToString();
                        }
                    }
                }
                dt.AcceptChanges();
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
            logentity.pageurl = "TB_CommonRemarksList.html";
            logentity.logcontent = "删除id为:" + PKCode + "的常用备注管理信息";
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
            logentity.pageurl = "TB_CommonRemarksList.html";
            logentity.logcontent = "修改状态id为:" + PKCode + "的常用备注管理信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, PKCode, status);

            ReturnListJson(dt);
        }
    }
}