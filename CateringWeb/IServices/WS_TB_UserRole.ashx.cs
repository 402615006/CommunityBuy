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
                        case "getroleusernamelist"://获取指定门店指定角色类型的用户，不分页
                            GetRoleUserNameList(dicPar);
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
             bll.Add(GUID, USER_ID, out Id, BusCode, StoCode, StrRoleId, UserId, RealName, EmpCode, sigJson, RoleDisCount);

            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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
            //调用逻辑

            TB_UserRoleEntity UEntity = bll.GetEntitySigInfo(" where id=" + Id);
            UEntity.RealName = RealName;

            bll.Update(GUID, USER_ID,UEntity);

            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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
            ReturnListJson(dt,null,null,null,null);
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
            bll.Delete(GUID, USER_ID, Id);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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
            ReturnListJson(dt,null,null,null,null);
        }
    }
}