using System;
using System.Collections;
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
    /// 账单接口类
    /// </summary>
    public class WSTB_OnlineBill : ServiceBase
    {
        bllTB_Bill bll = new bllTB_Bill();
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
                    logentity.module = "账单";
                    switch (actionname.ToLower())
                    {
                        case "getpayorderlist"://获取待接单
                            GetList(dicPar);
                            break;
                        case "getBillDetail"://获取账单详情
                            Detail(dicPar);
                            break;
                        case "getallpayorderlist"://获取全部线上订单
                            GetList(dicPar);
                            break;
                        case "makefood"://制作
                            UpdateStatus(dicPar);
                            break;
                        case "takefood"://取餐
                            UpdateStatus(dicPar);
                            break;
                        case "refundorder"://退款
                            UnFinish(dicPar);
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
                filter = JsonHelper.JsonToFilterByString1(filter, out dtFilter);
                if (dtFilter != null)
                {
                    DataRow[] drArr = dtFilter.Select("cus<>''");
                    foreach (DataRow dr in drArr)
                    {
                        string col = dr["col"].ToString();
                        switch (col)
                        {
                            case "CStatus"://待接单
                                filter += " and CStatus in('0','1')";
                                filter += " and TStatus in('1','5')";
                                break;
                            case "TStatus"://全部订单
                                filter += " and TStatus in('1','3','5')";
                                break;
                        }
                    }
                }
            }
            filter = GetBusCodeWhere(dicPar, filter, "b.buscode");
            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            //ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, "aa:" + filter);
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        /// <summary>
        /// 获取账单详细
        /// </summary>
        /// <param name="dicPar"></param>
        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BillCode", "StoCode", "UserCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string BillCode = dicPar["BillCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string UserCode = dicPar["UserCode"].ToString();
            //调用逻辑		
            DataSet ds = bll.GetDetail(GUID, USER_ID, BillCode, StoCode, UserCode);
            if (ds != null && ds.Tables.Count >= 4)
            {
                DataTable dtBill = ds.Tables[0];
                DataTable dtPayMethod = ds.Tables[1];
                DataTable dtDish = ds.Tables[2];
                DataTable dtOpenTable = ds.Tables[3];

                ArrayList dtArray = new ArrayList() { dtBill, dtPayMethod, dtDish, dtOpenTable };
                string[] tablenames = { "BillList", "PayMethodList", "DishList", "OpenTableList" };
                string json = JsonHelper.ToJson("0", "获取成功", dtArray, tablenames);
                Pagcontext.Response.Write(json);
            }
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "billCode", "stoCode", "CStatus", "ccode", "ccname" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string billCode = dicPar["billCode"].ToString();
            string stoCode = dicPar["stoCode"].ToString();
            string CStatus = dicPar["CStatus"].ToString();
            string ccode = dicPar["ccode"].ToString();
            string ccname = dicPar["ccname"].ToString();

            logentity.pageurl = "onlinebill.html";
            logentity.logcontent = "修改取餐状态编号为:" + billCode + "的账单信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateCStatus(GUID, USER_ID, billCode, stoCode, CStatus, ccode, ccname);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["type"].ToString() == "0")//成功
                {
                    if (CStatus == "1")//制作，获取订单信息
                    {
                        //调用逻辑		
                        DataSet ds = bll.GetDetail(GUID, USER_ID, billCode, stoCode, "");
                        DataSet ds1 = new bllTB_Bill().PrintDetail(GUID, USER_ID, billCode, stoCode);
                        if (ds != null && ds.Tables.Count >= 5 && ds1 != null && ds1.Tables.Count >= 7)
                        {
                            DataTable opentable = ds1.Tables[0];
                            DataTable bill = ds1.Tables[1];
                            DataTable dish = ds1.Tables[2];
                            DataTable pay = ds1.Tables[3];
                            DataTable coupon = ds1.Tables[4];
                            DataTable memcardorder = ds1.Tables[5];
                            DataTable paydetail = ds1.Tables[6];

                            DataTable DishList = ds.Tables[2];
                            DataTable OpenTableList = ds.Tables[3];

                            ArrayList dtArray = new ArrayList() { opentable, bill, dish, pay, coupon, memcardorder, paydetail, DishList, OpenTableList };
                            string[] tablenames = { "opentable", "bill", "dish", "pay", "coupon", "memcardorder", "paydetail", "DishList", "OpenTableList" };
                            string json = JsonHelper.ToJson("0", "获取成功", dtArray, tablenames);
                            Pagcontext.Response.Write(json);
                        }
                    }
                    else
                    {
                        ToCustomerJson("0", "获取成功");
                    }
                }
                else
                {
                    ReturnListJson(dt);
                }
            }
            else
            {
                ToErrorJson();
            }
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="dicPar"></param>
        private void UnFinish(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BillCode", "BusCode", "StoCode", "CCode", "CCname" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string BillCode = dicPar["BillCode"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string CCname = dicPar["CCname"].ToString();
            //调用逻辑
            string mescode = string.Empty;
            DataTable dt = bll.BillReturn(GUID, USER_ID, BusCode, StoCode, BillCode, CCode, CCname);
            ReturnListJson(dt);
        }
    }
}