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
                        case "getorderdishlist"://订单带菜品列表
                            GetOrderDIshList(dicPar);
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
                    drResult["ctime"] = StringHelper.StringToDateTime(drOrder["ctime"].ToString()).ToString("HH:mm");
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
            ReturnListJson(dtResult,null,null,null,null);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCode", "CCname", "TStatus", "PKCode", "OpenCodeList", "OrderMoney", "DisNum", "DisTypeNum", "Remar", "CheckTime", "BillCode"};
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
			string OrderMoney = dicPar["OrderMoney"].ToString();
			string Remar = dicPar["Remar"].ToString();
			string CheckTime = dicPar["CheckTime"].ToString();
			string BillCode = dicPar["BillCode"].ToString();

            //调用逻辑

            TB_OrderEntity UEntity = bll.GetEntitySigInfo("where pkcode='"+PKCode+"'");
            UEntity.Remar = Remar;
            UEntity.TStatus = TStatus;


            bll.Update(GUID, USER_ID, UEntity);
            
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
            string pkcode = dicPar["pkcode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            //调用逻辑			
            dt =bll.GetPagingSigInfo(GUID, USER_ID, "where PKcode='" + pkcode+"' and StoCode='"+ stocode + "'");
            ReturnListJson(dt,null,null,null,null);
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

            bll.Delete(GUID, USER_ID, pkcode,stocode);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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

            //DataTable dt = bll.UpdateStatus(GUID, USER_ID, ids, status, stocode);

            //ReturnListJson(dt);
        }

    }
}