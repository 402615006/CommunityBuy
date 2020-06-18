using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.IServices;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.WServices
{
	/// <summary>
    /// 退单信息接口类
    /// </summary>
    public class WSTB_BackOrder : ServiceBase
    {
        bllTB_BackOrder bll = new bllTB_BackOrder();
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
					logentity.module = "退单信息";
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
            filter = GetBusCodeWhere(dicPar, filter, "buscode");
            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt);
        } 
        
        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID","BusCode", "StoCode", "CCode", "CCname", "AuthCode", "AuthName", "OrderCode", "OrderDisCode", "ReasonCode", "ReasonName", "Remar", "BackNum","discounttype" };
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
			string CCode = dicPar["CCode"].ToString();
			string CCname = dicPar["CCname"].ToString();
			string AuthCode = dicPar["AuthCode"].ToString();
			string AuthName = dicPar["AuthName"].ToString();
            string TStatus = "1";
			string OrderCode = dicPar["OrderCode"].ToString();
			string OrderDisCode = dicPar["OrderDisCode"].ToString();
			string ReasonCode = dicPar["ReasonCode"].ToString();
			string ReasonName = dicPar["ReasonName"].ToString();
			string Remar = dicPar["Remar"].ToString();
			string BackNum = dicPar["BackNum"].ToString();
            string discounttype = dicPar["discounttype"].ToString();
            //调用逻辑
            logentity.pageurl ="TB_BackOrderEdit.html";
			logentity.logcontent = "新增退单信息信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Add;

            //如果是签送，返还签送额度
            if (discounttype == "6")
            {
                //"GUID":"","USER_ID":"","buscode":"88888888","stocode":"12","orderno":"10001"
                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/usersign/usersign.ashx";
                StringBuilder postStr = new StringBuilder();
                postStr.Append("actionname=usersignreturn&parameters={" +
                                                string.Format("'GUID':'{0}'", "") +
                                                string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                string.Format(",'buscode': '{0}'", BusCode) +
                                                string.Format(",'stocode': '{0}'", StoCode) +
                                                string.Format(",'orderno': '{0}'", OrderDisCode) +
                                        "}");//键值对
                string strMoneytJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                string status = "";
                string msg = "";
                JsonHelper.JsonToMessage(strMoneytJson, out status, out msg);
                if (status == "0")
                {
                    dt = bll.Add(GUID, USER_ID, Id, BusCode, StoCode, CCode, CCname, AuthCode, AuthName, TStatus, OrderCode, OrderDisCode, ReasonCode, ReasonName, Remar, BackNum, logentity);
                }
                else
                {
                    ReturnListJson("1", "反签送失败", null, null);
                    return;
                }
            }
            else
            {
                dt = bll.Add(GUID, USER_ID, Id, BusCode, StoCode, CCode, CCname, AuthCode, AuthName, TStatus, OrderCode, OrderDisCode, ReasonCode, ReasonName, Remar, BackNum, logentity);
            }
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCode", "CCname", "AuthCode", "AuthName", "TStatus", "OrderCode", "OrderDisCode", "ReasonCode", "ReasonName", "Remar", "BackNum", };
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
			string CCode = dicPar["CCode"].ToString();
			string CCname = dicPar["CCname"].ToString();
			string AuthCode = dicPar["AuthCode"].ToString();
			string AuthName = dicPar["AuthName"].ToString();
			string TStatus = dicPar["TStatus"].ToString();
			string OrderCode = dicPar["OrderCode"].ToString();
			string OrderDisCode = dicPar["OrderDisCode"].ToString();
			string ReasonCode = dicPar["ReasonCode"].ToString();
			string ReasonName = dicPar["ReasonName"].ToString();
			string Remar = dicPar["Remar"].ToString();
			string BackNum = dicPar["BackNum"].ToString();

            //调用逻辑
			logentity.pageurl ="TB_BackOrderEdit.html";
			logentity.logcontent = "修改id为:"+Id+"的退单信息信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID,  Id, BusCode, StoCode, CCode, CCname, AuthCode, AuthName, TStatus, OrderCode, OrderDisCode, ReasonCode, ReasonName, Remar, BackNum, entity);
            
            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
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
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where Id=" + Id);
            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id"};
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
			logentity.pageurl ="TB_BackOrderList.html";
			logentity.logcontent = "删除id为:"+Id+"的退单信息信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.Delete(GUID, USER_ID, Id, entity);
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
            logentity.pageurl ="TB_BackOrderList.html";
			logentity.logcontent = "修改状态id为:"+Id+"的退单信息信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, Id, status);

            ReturnListJson(dt);
        }
    }
}