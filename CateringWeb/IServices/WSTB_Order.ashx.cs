using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.BLL;
using CommunityBuy.IServices;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using System.Linq;
namespace CommunityBuy.WServices
{
	/// <summary>
    /// 订单接口类
    /// </summary>
    public class WSTB_Order : ServiceBase
    {
        bllTB_Order bll = new bllTB_Order();
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
					logentity.module = "订单";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "getorderdishlist"://订单带菜品列表
                            GetOrderDIshList(dicPar);
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
                        case "getmemorders"://修改状态
                            GetMemOrderList(dicPar);
                            break;
                        case "getunfinishorder"://获取未结账单
                            GetUnFinishOrder(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取订单菜品
        /// </summary>
        /// <param name="dicPar"></param>
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

        
        private void GetOrderDIshList(Dictionary<string, object> dicPar)
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
            if (filter.Length > 0)
            {
                filter = JsonHelper.JsonToFilterByString(filter, out dtFilter);
                if (dtFilter != null)
                {
                    DataRow[] drArr = dtFilter.Select("cus<>''");
                    foreach (DataRow dr in drArr)
                    {
                        string col = dr["col"].ToString();
                        string cus = dr["cus"].ToString();
                        string colFilter= dr["filter"].ToString();
                        switch (col)
                        {
                            case "o.TStatus":
                                if (cus == "in")
                                {
                                    filter += " and o.TStatus in("+ colFilter + ") ";
                                }
                                break;
                        }
                    }
                }
            }
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            if (!filter.ToLower().Contains("buscode"))
            {
                if (!string.IsNullOrEmpty(BusCode))
                {
                    if (string.IsNullOrEmpty(filter))
                    {
                        filter = "where o.buscode='" + BusCode + "'";
                    }
                    else
                    {
                        filter += " and o.buscode='" + BusCode + "'";
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
            dt = bll.GetOrderDisPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("code");
            dtResult.Columns.Add("ctime");
            dtResult.Columns.Add("money");
            dtResult.Columns.Add("disnum");
            dtResult.Columns.Add("distypenum");
            dtResult.Columns.Add("ordertype");
            dtResult.Columns.Add("dishList");
            dtResult.Columns.Add("tablename");
            dtResult.Columns.Add("cusnum");
            dtResult.Columns.Add("opentime");
            dtResult.Columns.Add("openman");
            dtResult.Columns.Add("tstatus");
            if (dt != null)
            {
               
                DataTable dtOrder = dt.DefaultView.ToTable(true, "code");
                foreach (DataRow dr in dtOrder.Rows)
                {
                    DataRow drResult = dtResult.NewRow();
                    DataRow drOrder = dt.Select("code='" + dr["code"] + "'")[0];
                    drResult["code"] = drOrder["code"];
                    drResult["ctime"] = Helper.StringToDateTime(drOrder["ctime"].ToString()).ToString("HH:mm");
                    drResult["money"] = drOrder["money"];
                    drResult["disnum"] = drOrder["disnum"];
                    drResult["distypenum"] = drOrder["distypenum"];
                    drResult["ordertype"] = drOrder["ordertype"];
                    drResult["tablename"] = drOrder["tablename"];
                    drResult["cusnum"] = drOrder["cusnum"];
                    drResult["opentime"] = drOrder["opentime"];
                    drResult["openman"] = drOrder["openman"];
                    drResult["tstatus"] = drOrder["status"];

                    DataTable dtDish = new DataTable();
                    dtDish.Columns.Add("discode");
                    dtDish.Columns.Add("disname");
                    dtDish.Columns.Add("price");
                    dtDish.Columns.Add("extraMoney");
                    dtDish.Columns.Add("disnum");
                    dtDish.Columns.Add("totalMoney");
                    dtDish.Columns.Add("uptype");
                    dtDish.Columns.Add("returnnum");
                    dtDish.Columns.Add("DiscountType");
                    dtDish.Columns.Add("DiscountPrice");
                    dtDish.Columns.Add("MemPrice");
                    dtDish.Columns.Add("CookMoney");
                    dtDish.Columns.Add("FinCode");
                    dtDish.Columns.Add("DisTypeCode");
                    dtDish.Columns.Add("CookName");
                    dtDish.Columns.Add("OrderCode");
                    dtDish.Columns.Add("kitcode");
                    dtDish.Columns.Add("typecode");
                    dtDish.Columns.Add("ispackage");
                    dtDish.Columns.Add("pdiscode");
                    dtDish.Columns.Add("IsMemPrice");
                    dtDish.Columns.Add("orderdiscode");
                    dtDish.Columns.Add("IsCoupon");
                    dtDish.Columns.Add("ItemNum");
                    dtDish.Columns.Add("ItemPrice");
                    dtDish.Columns.Add("IsMp");
                    dtDish.Columns.Add("MpCheckCode");
                    dtDish.Columns.Add("Tstatus");

                    foreach (DataRow drTemp in dt.Select("code='" + dr["code"] + "'"))
                    {
                        DataRow drAdd = dtDish.NewRow();
                        drAdd["discode"] = drTemp["discode"];
                        drAdd["disname"] = drTemp["disname"];
                        drAdd["price"] = drTemp["price"];
                        drAdd["extraMoney"] = drTemp["extraMoney"];
                        drAdd["disnum"] = drTemp["disnum"];
                        drAdd["totalMoney"] = drTemp["totalmoney"];
                        drAdd["uptype"] = drTemp["uptype"];
                        drAdd["returnnum"] = drTemp["returnnum"];
                        drAdd["DiscountType"] = drTemp["DiscountType"];
                        drAdd["DiscountPrice"] = drTemp["DiscountPrice"];
                        drAdd["MemPrice"] = drTemp["MemPrice"];
                        drAdd["CookMoney"] = drTemp["CookMoney"];
                        drAdd["FinCode"] = drTemp["FinCode"];
                        drAdd["DisTypeCode"] = drTemp["DisTypeCode"];
                        drAdd["CookName"] = drTemp["CookName"];
                        drAdd["OrderCode"] = drTemp["OrderCode"];
                        drAdd["kitcode"] = drTemp["kitcode"];
                        drAdd["typecode"] = drTemp["typecode"];
                        drAdd["ispackage"] = drTemp["ispackage"];
                        drAdd["pdiscode"] = drTemp["pdiscode"];
                        drAdd["IsMemPrice"] = drTemp["IsMemPrice"];
                        drAdd["orderdiscode"] = drTemp["PkCode"];
                        drAdd["IsCoupon"] = drTemp["IsCoupon"];
                        drAdd["ItemNum"] = drTemp["ItemNum"];
                        drAdd["ItemPrice"] = drTemp["ItemPrice"];
                        drAdd["IsMp"] = drTemp["IsMp"];
                        drAdd["MpCheckCode"] = drTemp["MpCheckCode"];
                        drAdd["tstatus"] = "1";
                        if (drOrder["status"].ToString() == "0")
                        {
                            drAdd["tstatus"] = "0";
                        }

                        dtDish.Rows.Add(drAdd);
                    }
                    string jsonDish = JsonHelper.DataTableToJSON(dtDish);


                    drResult["dishList"] = jsonDish;
                    dtResult.Rows.Add(drResult);
                }
            }
            ReturnListJson(dtResult);
        }

        /// <summary>
        /// 获取会员卡账单
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetMemOrderList(Dictionary<string, object> dicPar)
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
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            if (!filter.ToLower().Contains("buscode"))
            {
                if (!string.IsNullOrEmpty(BusCode))
                {
                    if (string.IsNullOrEmpty(filter))
                    {
                        filter = "where buscode='" + BusCode + "'";
                    }
                    else
                    {
                        filter += " and buscode='" + BusCode + "'";
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
            dt = new bllmemcardorders().GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            foreach (DataRow dr in dt.Rows)
            {
                dr["otype"] = Helper.GetEnumNameByValue(typeof(SystemEnum.MemCardStatus), dr["otype"].ToString());
            }
            ReturnListJson(dt);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID",  "BusCode", "StoCode", "CCode", "CCname", "OpenCodeList", "OrderMoney", "DisNum", "DisTypeNum", "Remar","OrderDishList","TStatus" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
			string Id ="0";
			string BusCode = dicPar["BusCode"].ToString();
			string StoCode = dicPar["StoCode"].ToString();
			string CCode = dicPar["CCode"].ToString();
			string CCname = dicPar["CCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string PKCode = "";
			string OpenCodeList = dicPar["OpenCodeList"].ToString();
			string OrderMoney = dicPar["OrderMoney"].ToString();
			string DisNum = dicPar["DisNum"].ToString();
			string DisTypeNum = dicPar["DisTypeNum"].ToString();
			string Remar = dicPar["Remar"].ToString();
            string CheckTime = "";
            string BillCode = "";

            string dishlistBin = dicPar["OrderDishList"].ToString();
            string dishlistJson = Helper.BinaryToString(dishlistBin);

            string OrderDishListJson = dishlistJson;
            string OrderType = dicPar["OrderType"].ToString();
            string DepartCode= dicPar["DepartCode"].ToString();
            //调用逻辑
            logentity.pageurl ="TB_OrderEdit.html";
			logentity.logcontent = "新增订单信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.Add(OrderDishListJson,GUID, USER_ID,Id, BusCode, StoCode, CCode, CCname, TStatus, out PKCode, OpenCodeList, OrderMoney, DisNum, DisTypeNum, Remar, CheckTime, BillCode,OrderType,DepartCode, logentity);
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCode", "CCname", "TStatus", "PKCode", "OpenCodeList", "OrderMoney", "DisNum", "DisTypeNum", "Remar", "CheckTime", "BillCode", };
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
			string TStatus = dicPar["TStatus"].ToString();
			string PKCode = dicPar["PKCode"].ToString();
			string OpenCodeList = dicPar["OpenCodeList"].ToString();
			string OrderMoney = dicPar["OrderMoney"].ToString();
			string DisNum = dicPar["DisNum"].ToString();
			string DisTypeNum = dicPar["DisTypeNum"].ToString();
			string Remar = dicPar["Remar"].ToString();
			string CheckTime = dicPar["CheckTime"].ToString();
			string BillCode = dicPar["BillCode"].ToString();
            string OrderType = dicPar["OrderType"].ToString();
            string DepartCode = dicPar["DepartCode"].ToString();

            //调用逻辑
            logentity.pageurl ="TB_OrderEdit.html";
			logentity.logcontent = "修改id为:"+Id+"的订单信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID,  Id, BusCode, StoCode, CCode, CCname, TStatus, PKCode, OpenCodeList, OrderMoney, DisNum, DisTypeNum, Remar, CheckTime, BillCode, OrderType, DepartCode, entity);
            
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
            string pkcode = dicPar["pkcode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where PKcode='" + pkcode+"' and StoCode='"+ stocode + "'");
            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "StoCode", "OrderCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string pkcode = dicPar["OrderCode"].ToString();
            string stocode = dicPar["StoCode"].ToString();
            //调用逻辑
            logentity.pageurl ="TB_OrderList.html";
			logentity.logcontent = "删除编号为:"+ pkcode + "的订单信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.Delete(GUID, USER_ID, pkcode,stocode, logentity);
            ReturnListJson(dt);
        }

		/// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "ids", "status","stocode" };
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

            string stocode = dicPar["stocode"].ToString().Trim(',');
            logentity.pageurl ="TB_OrderList.html";
			logentity.logcontent = "修改状态id为:"+ ids + "的订单信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, ids, status, stocode);

            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取未结账单列表
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetUnFinishOrder(Dictionary<string, object> dicPar)
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
            string bcode = dicPar["bcode"].ToString();
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
                        }
                    }
                }
            }
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            if (!filter.ToLower().Contains("buscode"))
            {
                if (!string.IsNullOrEmpty(BusCode))
                {
                    if (string.IsNullOrEmpty(filter))
                    {
                        filter = "where o.buscode='" + BusCode + "'";
                    }
                    else
                    {
                        filter += " and o.buscode='" + BusCode + "'";
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
            filter += " and  (TStatus not in('0','3','5') or BillCode in(select PKCode from TB_Bill where Stocode=o.StoCode and TStatus not in('4','5')))";
            dt = bll.GetUnFinishPagingListInfo(GUID, USER_ID, int.MaxValue, 1, filter, order, out recordCount, out totalPage);

            //linq
            //var linqdt = dt.Columns.Add("allmoney").Table.AsEnumerable().OrderByDescending(r => r.Field<DateTime>("ctime")).Where(r =>string.IsNullOrWhiteSpace(r.Field<string>("bccode")) || r.Field<string>("bccode") == bcode);
            //var summoney = linqdt.Sum(r => r.Field<decimal>("ordermoney"));
            //recordCount = linqdt.Count();
            //totalPage = (int)(Math.Ceiling((decimal)recordCount / pageSize));
            //DataTable pagedt = linqdt.Skip(((currentPage - 1) * pageSize)).Take(pageSize).Select(r => { r.SetField<decimal>("allmoney", summoney); return r; }).ToArray().CopyToDataTable();
            //ReturnListJson(pagedt, pageSize, recordCount, currentPage, totalPage);
            //end


            DataView dv = dt.DefaultView;
            dv.Sort = "ctime desc";
            DataTable dtSort = dv.ToTable();
            DataTable dtReturn = dt.Clone();
            foreach (DataRow dr in dtSort.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["BCCode"].ToString()))
                {
                    if (dr["BCCode"].ToString() == bcode)
                    {
                        dtReturn.ImportRow(dr);
                    }
                }
                else
                {
                    dtReturn.ImportRow(dr);
                }
            }
            decimal summoney = Helper.SumDataTableColumn(dtReturn, new string[] { "ordermoney" }, "sum(ordermoney)", "");
            dtReturn.Columns.Add("allmoney");


            foreach (DataRow dr in dtReturn.Rows)
            {
                dr["allmoney"] = summoney.ToString("f2");
            }
            var rows = dtReturn.Rows.Cast<DataRow>();
            var curRows = rows.Skip(((currentPage - 1) * pageSize)).Take(pageSize).ToArray();
            DataTable dtPage = new DataTable();
            if (curRows.Length > 0)
            {
                dtPage = curRows.CopyToDataTable();
            }
            totalPage = (int)(Math.Ceiling((decimal)dtReturn.Rows.Count / pageSize));
            ReturnListJson(dtPage, pageSize, dtReturn.Rows.Count, currentPage, totalPage);
        }
    }
}