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
            int pageSize = StringHelper.StringToInt(dicPar["limit"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0 && filter != "[]")
            {
                filter = JsonHelper.JsonToFilterByString(filter, out dtFilter);
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
            DataTable dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
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
            bll.Add(GUID, USER_ID, Id, BusCode, StoCode, CCname, PKKCode, PKCode, TypeName, Sort, TStatus, CCode);

            ReturnResultJson(bll.oResult.Code,bll.oResult.Msg);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode","PKCode", "TypeName", "Sort", "TStatus" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string TypeName = dicPar["TypeName"].ToString();
            string Sort = dicPar["Sort"].ToString();
            //调用逻辑
            TB_DishTypeEntity UEntity = bll.GetEntitySigInfo(" where pkcode='"+ PKCode + "'");
            UEntity.TypeName = TypeName;
            UEntity.Sort =StringHelper.StringToInt(Sort);
            bll.Update(GUID, USER_ID, UEntity);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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

            DataTable dt = bll.GetPagingSigInfo(GUID, USER_ID, "where PKCode='" + PKCode + "' and stocode='"+stocode+"'");

            ReturnListJson(dt,null,null,null,null);
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
            bll.Delete(GUID, USER_ID, PKCode);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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

            TB_DishTypeEntity UEntity = bll.GetEntitySigInfo(" where pkcode='" + PKCode + "'");
            UEntity.TStatus = status;
            bll.Update(GUID, USER_ID, UEntity);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }
    }
}