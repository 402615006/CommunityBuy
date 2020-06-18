using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.IServices;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
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
					logentity.module = "订单菜品";
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
            filter = GetBusCodeWhere(dicPar, filter, "od.buscode");
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
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCode", "CCname", "OrderCode", "FinCode", "DisTypeCode", "DisCode", "DisName", "MemPrice", "Price", "DisUite", "DisNum", "ReturnNum", "IsPackage", "PDisCode", "Remar", "PKCode", "DiscountPrice", "DiscountRemark", "DiscountType", "DisCase", "Favor", "ItemNum", "ItemPrice", "CookName", "CookMoney", "TotalMoney", };
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
			string OrderCode = dicPar["OrderCode"].ToString();
			string FinCode = dicPar["FinCode"].ToString();
			string DisTypeCode = dicPar["DisTypeCode"].ToString();
			string DisCode = dicPar["DisCode"].ToString();
			string DisName = dicPar["DisName"].ToString();
			string MemPrice = dicPar["MemPrice"].ToString();
			string Price = dicPar["Price"].ToString();
			string DisUite = dicPar["DisUite"].ToString();
			string DisNum = dicPar["DisNum"].ToString();
			string ReturnNum = dicPar["ReturnNum"].ToString();
			string IsPackage = dicPar["IsPackage"].ToString();
			string PDisCode = dicPar["PDisCode"].ToString();
			string Remar = dicPar["Remar"].ToString();
			string PKCode = dicPar["PKCode"].ToString();
			string DiscountPrice = dicPar["DiscountPrice"].ToString();
			string DiscountRemark = dicPar["DiscountRemark"].ToString();
			string DiscountType = dicPar["DiscountType"].ToString();
			string DisCase = dicPar["DisCase"].ToString();
			string Favor = dicPar["Favor"].ToString();
			string ItemNum = dicPar["ItemNum"].ToString();
			string ItemPrice = dicPar["ItemPrice"].ToString();
			string CookName = dicPar["CookName"].ToString();
			string CookMoney = dicPar["CookMoney"].ToString();
			string TotalMoney = dicPar["TotalMoney"].ToString();
            //调用逻辑
			logentity.pageurl ="TB_OrderDishEdit.html";
			logentity.logcontent = "新增订单菜品信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.Add(GUID, USER_ID, out  Id, BusCode, StoCode, CCode, CCname, OrderCode, FinCode, DisTypeCode, DisCode, DisName, MemPrice, Price, DisUite, DisNum, ReturnNum, IsPackage, PDisCode, Remar, PKCode, DiscountPrice, DiscountRemark, DiscountType, DisCase, Favor, ItemNum, ItemPrice, CookName, CookMoney, TotalMoney, entity);
			
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCode", "CCname", "OrderCode", "FinCode", "DisTypeCode", "DisCode", "DisName", "MemPrice", "Price", "DisUite", "DisNum", "ReturnNum", "IsPackage", "PDisCode", "Remar", "PKCode", "DiscountPrice", "DiscountRemark", "DiscountType", "DisCase", "Favor", "ItemNum", "ItemPrice", "CookName", "CookMoney", "TotalMoney", };
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
			string OrderCode = dicPar["OrderCode"].ToString();
			string FinCode = dicPar["FinCode"].ToString();
			string DisTypeCode = dicPar["DisTypeCode"].ToString();
			string DisCode = dicPar["DisCode"].ToString();
			string DisName = dicPar["DisName"].ToString();
			string MemPrice = dicPar["MemPrice"].ToString();
			string Price = dicPar["Price"].ToString();
			string DisUite = dicPar["DisUite"].ToString();
			string DisNum = dicPar["DisNum"].ToString();
			string ReturnNum = dicPar["ReturnNum"].ToString();
			string IsPackage = dicPar["IsPackage"].ToString();
			string PDisCode = dicPar["PDisCode"].ToString();
			string Remar = dicPar["Remar"].ToString();
			string PKCode = dicPar["PKCode"].ToString();
			string DiscountPrice = dicPar["DiscountPrice"].ToString();
			string DiscountRemark = dicPar["DiscountRemark"].ToString();
			string DiscountType = dicPar["DiscountType"].ToString();
			string DisCase = dicPar["DisCase"].ToString();
			string Favor = dicPar["Favor"].ToString();
			string ItemNum = dicPar["ItemNum"].ToString();
			string ItemPrice = dicPar["ItemPrice"].ToString();
			string CookName = dicPar["CookName"].ToString();
			string CookMoney = dicPar["CookMoney"].ToString();
			string TotalMoney = dicPar["TotalMoney"].ToString();

            //调用逻辑
			logentity.pageurl ="TB_OrderDishEdit.html";
			logentity.logcontent = "修改id为:"+Id+"的订单菜品信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID,  Id, BusCode, StoCode, CCode, CCname, OrderCode, FinCode, DisTypeCode, DisCode, DisName, MemPrice, Price, DisUite, DisNum, ReturnNum, IsPackage, PDisCode, Remar, PKCode, DiscountPrice, DiscountRemark, DiscountType, DisCase, Favor, ItemNum, ItemPrice, CookName, CookMoney, TotalMoney, entity);
            
            ReturnListJson(dt);
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
            ToCustomerJson("0", "操作已完成");
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
			logentity.pageurl ="TB_OrderDishList.html";
			logentity.logcontent = "删除id为:"+Id+"的订单菜品信息";
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
            logentity.pageurl ="TB_OrderDishList.html";
			logentity.logcontent = "修改状态id为:"+Id+"的订单菜品信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, Id, status);

            ReturnListJson(dt);
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

            logentity.pageurl = "TB_OrderDishList.html";
            logentity.logcontent = "编号为:" + orderdishcode + "的订单菜品退菜";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            DataTable dt = bll.BackOrderDish(GUID, USER_ID, stocode, orderdishcode, logentity);
            ReturnListJson(dt);
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

            ReturnListJson(dataTableDistinct);
        }

        private void UpDish(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "orderdiscode","stocode"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string orderdiscode = dicPar["orderdiscode"].ToString();
            string stocode = dicPar["stocode"].ToString();

            //int rel=new bllPaging().UpdateData

            ReturnListJson(dt);
        }
    }
}