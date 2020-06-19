using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.IServices;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using System.Web.UI;

namespace CommunityBuy.WServices
{
	/// <summary>
    /// 订单菜品接口类
    /// </summary>
    public class WSTB_OrderDish : ServiceBase
    {
        bllTB_OrderDish bll = new bllTB_OrderDish();
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

                        case "detail"://详细
                            Detail(dicPar);
                            break;

                        case "delete"://删除
                            Delete(dicPar);
                            break;
                        case "backorderdish"://退菜
                            BackOrderDish(dicPar);
                            break;
                        case "gettabledishes"://获取桌台已点菜品
                            GetListByTable(dicPar);
                            break;
                        case "updatelist"://更新桌台菜品
                            UpdateList(dicPar);
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
                            case "od.OrderCode":
                                string ordercodes = dr["filter"].ToString();
                                //string ordercodefilters = "";
                                //foreach (string od in ordercodes.Split(','))
                                //{
                                //    ordercodefilters += string.Format("'{0}',", od);
                                //}
                                //ordercodefilters.TrimEnd();
                                filter += " and od.OrderCode in(select col from [dbo].[fn_StringSplit]('"+ ordercodes + "',','))";
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
            ReturnListJson(dt,pageSize,recordCount, currentPage, totalPage);
        } 

        private void UpdateList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BusCode", "StoCode", "OrderDishList" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string OrderDishList = dicPar["OrderDishList"].ToString();

            DataTable dtDish = JsonHelper.JsonToDataTable(OrderDishList);
            if (dtDish != null && dtDish.Rows.Count > 0)
            {
                bll.UpdateByTable(GUID, USER_ID, StoCode, dtDish);
            }
            ReturnResultJson("0", "操作已完成");
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

             bll.Delete(GUID, USER_ID, Id);
            ReturnResultJson(bll.oResult.Code,bll.oResult.Msg);
        }

        private void BackOrderDish(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode","orderdishcode"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string orderdishcode = dicPar["orderdishcode"].ToString();

            bll.BackOrderDish(GUID, USER_ID, stocode, orderdishcode);
            ReturnResultJson(bll.oResult.Code,bll.oResult.Msg);
        }

        private void GetListByTable(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "page", "limit", "StoCode","TableCode" };

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
            string stocode =dicPar["StoCode"].ToString();
            string tablecode = dicPar["TableCode"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetOrderDishByTable(GUID, USER_ID, stocode, tablecode, pageSize, currentPage,out recordCount,out totalPage);
            DataView dataView = dt.DefaultView;
            DataTable dataTableDistinct = dataView.ToTable(true, "DisCode");

            ReturnListJson(dataTableDistinct,null,null,null,null);
        }
    }
}