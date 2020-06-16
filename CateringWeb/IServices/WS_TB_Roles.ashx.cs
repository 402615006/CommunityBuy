using CommunityBuy.BLL;
using CommunityBuy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_TB_Roles 角色管理 的摘要说明
    /// </summary>
    public class WS_TB_Roles : ServiceBase
    {
        bllTB_Roles bll = new bllTB_Roles();
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
                    logentity.module = "门店角色信息";
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
                        case "getaddloaddata":
                            GetAddLoadData(dicPar);
                            break;
                        case "getbtnroledata": //根据角色获取按钮权限
                            GetbtnRoleData(dicPar);
                            break;

                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            try
            {
                //要检测的参数信息
                List<string> pra = new List<string>() { "GUID", "userid", "page", "limit", "filters", "orders" };

                //检测方法需要的参数
                if (!CheckActionParameters(dicPar, pra))
                {
                    return;
                }

                //获取参数信息
                string GUID = dicPar["GUID"].ToString();
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
                dt = bll.GetPagingListInfo(GUID, userid, pageSize, currentPage, filter, order, out recordCount, out totalPage);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dtRoleType = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.RolesType));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["TerminalTypeName"] = Enum.GetName(typeof(SystemEnum.SysTerminalType), Helper.StringToInt(dr["TerminalType"].ToString()));
                        if (dtRoleType.Select("enumcode='" + Helper.StringToInt(dr["RoleType"].ToString()) + "'").Length > 0)
                        {
                            dr["RoleTypeName"] = dtRoleType.Select("enumcode='" + Helper.StringToInt(dr["RoleType"].ToString()) + "'")[0]["enumname"].ToString();
                        }

                    }
                    dt.AcceptChanges();
                }
                ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
            }
            catch (Exception ex)
            {
                ToCustomerJson("2", ex.Message);
                return;
            }

        }

        /// <summary>
        /// 新增角色及角色功能
        /// </summary>
        /// <param name="dicPar"></param>
        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "Id", "BusCode", "StoCode", "CCname", "TStatus", "UCname", "SCope", "RoleName", "RoleParent", "RoleDescr", "RoleDisCount", "RoleEnable", "MaxDiffPrice", "MaxPrefePrice", "IsSig", "Sigcredit", "RoleType", "TerminalType", "CCode", "UCode", "FunctionIds" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string Id = dicPar["Id"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = Helper.GetAppSettings("StoCode");
            string CCname = dicPar["CCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string UCname = dicPar["UCname"].ToString();
            string SCope = dicPar["SCope"].ToString();
            string RoleName = dicPar["RoleName"].ToString();
            string RoleParent = dicPar["RoleParent"].ToString();
            string RoleDescr = dicPar["RoleDescr"].ToString();
            string RoleDisCount = dicPar["RoleDisCount"].ToString().Trim(',');
            string RoleEnable = dicPar["RoleEnable"].ToString();
            string MaxDiffPrice = dicPar["MaxDiffPrice"].ToString();
            string MaxPrefePrice = dicPar["MaxPrefePrice"].ToString();
            string IsSig = dicPar["IsSig"].ToString();
            string Sigcredit = dicPar["Sigcredit"].ToString();
            string RoleType = dicPar["RoleType"].ToString();
            string TerminalType = dicPar["TerminalType"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string UCode = dicPar["UCode"].ToString();
            string FunctionIds = string.Empty;
            if (dicPar["FunctionIds"] != null)
            {
                FunctionIds = dicPar["FunctionIds"].ToString().Trim(',');
            }
            //调用逻辑
            logentity.pageurl = "TB_RolesEdit.html";
            logentity.logcontent = "新增门店角色信息信息";
            logentity.cuser = Helper.StringToLong(userid);
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Add(GUID, userid, out Id, BusCode, StoCode, CCname, TStatus, UCname, SCope, RoleName, RoleParent, RoleDescr, RoleDisCount, RoleEnable, MaxDiffPrice, MaxPrefePrice, IsSig, Sigcredit, RoleType, TerminalType, CCode, UCode, FunctionIds, logentity);
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "Id", "BusCode", "StoCode", "CCname", "TStatus", "UCname", "SCope", "RoleName", "RoleParent", "RoleDescr", "RoleDisCount", "RoleEnable", "MaxDiffPrice", "MaxPrefePrice", "IsSig", "Sigcredit", "RoleType", "TerminalType", "CCode", "UCode", "FunctionIds" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string Id = dicPar["Id"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = Helper.GetAppSettings("StoCode");
            string CCname = dicPar["CCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string UCname = dicPar["UCname"].ToString();
            string SCope = dicPar["SCope"].ToString();
            string RoleName = dicPar["RoleName"].ToString();
            string RoleParent = dicPar["RoleParent"].ToString();
            string RoleDescr = dicPar["RoleDescr"].ToString();
            string RoleDisCount = dicPar["RoleDisCount"].ToString().Trim(',');
            string RoleEnable = dicPar["RoleEnable"].ToString();
            string MaxDiffPrice = dicPar["MaxDiffPrice"].ToString();
            string MaxPrefePrice = dicPar["MaxPrefePrice"].ToString();
            string IsSig = dicPar["IsSig"].ToString();
            string Sigcredit = dicPar["Sigcredit"].ToString();
            string RoleType = dicPar["RoleType"].ToString();
            string TerminalType = dicPar["TerminalType"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string UCode = dicPar["UCode"].ToString();
            string FunctionIds = string.Empty;
            if (dicPar["FunctionIds"] != null)
            {
                FunctionIds = dicPar["FunctionIds"].ToString().TrimEnd(',');
            }
            //调用逻辑
            logentity.pageurl = "TB_RolesEdit.html";
            logentity.logcontent = "修改id为:" + Id + "的门店角色信息信息";
            logentity.cuser = Helper.StringToLong(userid);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Update(GUID, userid, Id, BusCode, StoCode, CCname, TStatus, UCname, SCope, RoleName, RoleParent, RoleDescr, RoleDisCount, RoleEnable, MaxDiffPrice, MaxPrefePrice, IsSig, Sigcredit, RoleType, TerminalType, CCode, UCode, FunctionIds, logentity);

            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "Id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string Id = dicPar["Id"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, userid, "where Id=" + Id);
            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string Id = dicPar["id"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_RolesList.html";
            logentity.logcontent = "删除id为:" + Id + "的门店角色信息信息";
            logentity.cuser = Helper.StringToLong(userid);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Delete(GUID, userid, Id, logentity);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "ids", "tstatus" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string ids = dicPar["ids"].ToString();
            string status = dicPar["tstatus"].ToString();

            string Id = dicPar["ids"].ToString().Trim(',');
            logentity.pageurl = "TB_RolesList.html";
            logentity.logcontent = "修改状态id为:" + Id + "的门店角色信息信息";
            logentity.cuser = Helper.StringToLong(userid);
            DataTable dt = bll.UpdateStatus(GUID, userid, Id, status);

            ReturnListJson(dt);
        }

        /// <summary>
        /// 添加角色或修改角色时获取相关信息
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetAddLoadData(Dictionary<string, object> dicPar)
        {
            try
            {
                List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "id" };
                if (!CheckActionParameters(dicPar, pra))
                {
                    return;
                }
                string GUID = dicPar["GUID"].ToString();
                string userid = dicPar["userid"].ToString();
                string roleid = dicPar["id"].ToString();
                string USER_ID = dicPar["USER_ID"].ToString();
                DataTable data1 = new bllTB_RoleFunction().GetRoleFunctionInfoList(GUID, userid, roleid);//获取该角色的功能
                DataTable data2 = bll.GetPagingSigInfo(GUID, userid, "where Id=" + roleid);//获取角色信息
                //获取折扣方案和获取门店名称,门店信息需要从连锁端获取

                //对商品管理下的功能层级做特殊处理               
                DataRow[] drs = data1.Select("id in ('2002002','2002003','2002004','2002005','2002008','2002011','2002006','2002007')");
                foreach (DataRow dr in drs)
                {
                    dr["pId"] = "2002001";
                }
                data1.AcceptChanges();

                DataTable data3 = new DataTable();

                //data3.Columns.Add("RoleTypeName", typeof(string));
                //data3.Columns.Add("TerminalTypeName", typeof(string));
                ArrayList arrData = new ArrayList();
                string[] arrTBName = new string[3] { "data1", "data2", "data3" };
                arrData.Add(data1);
                arrData.Add(data2);
                arrData.Add(data3);
                ReturnListJson("0", "", arrData, arrTBName);
            }
            catch (Exception ex)
            {
                ToCustomerJson("2", ex.Message);
                return;
            }

        }

        /// <summary>
        /// 获取按钮权限
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetbtnRoleData(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["USER_ID"].ToString();
            //调用逻辑			
            dt = bll.GetbtnRoleData(GUID, userid);
            ReturnListJson(dt);
        }
    }
}