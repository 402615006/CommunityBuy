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
            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
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

           bll.Add(GUID, USER_ID, Id, BusCode, StoCode, CCode, CCname, AuthCode, AuthName, TStatus, OrderCode, OrderDisCode, ReasonCode, ReasonName, Remar, BackNum);
            ReturnResultJson(bll.oResult.Code,bll.oResult.Msg);
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
            ReturnListJson(dt,null,null,null,null);
        }
    }
}