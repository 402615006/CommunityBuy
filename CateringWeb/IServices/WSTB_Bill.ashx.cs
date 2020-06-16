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
    public class WSTB_Bill : ServiceBase
    {
        bllTB_Bill bll = new bllTB_Bill();
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
					logentity.module = "账单";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "getpaymethodlist"://列表
                            GetPayMethodList(dicPar);
                            break;
                        case "add"://添加							
                            Add(dicPar);
                            break;
                        case "detail"://详细
                            Detail(dicPar);
                            break;
                        case "memdetail"://详细
                            MemDetail(dicPar);
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
                        case "finish"://完成账单
                            Finish(dicPar);
                            break;
                        case "unfinish"://反结账单
                            UnFinish(dicPar);
                            break;
                        case "getunstockdish"://获取未出库的菜品
                            GetUnStockBillDish(dicPar);
                            break;
                        case "clearunusebill"://清理不用的订单
                            ClearUnuseBillByTable(dicPar);
                            break;
                        case "printdetail":
                            PrintDetail(dicPar);
                            break;
                        case "getappunprintbill":
                            GetAppUnPrintBill(dicPar);
                            break;
                        case "addbillorder":
                            AddBillOrder(dicPar);
                            break;
                        case "updateappprintbillstatus"://更新小程序结账单的打印状态
                            UpdateAppBillPrintStatus(dicPar);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 更新小程序结账单的打印状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateAppBillPrintStatus(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "StoCode","BillCode","Status", "Ptimes" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string BillCode = dicPar["BillCode"].ToString();
            string Status = dicPar["Status"].ToString();
            string Ptimes = dicPar["Ptimes"].ToString();
            //调用逻辑
            DataTable dt = new bllTR_BillPrint().UpdateStatus(GUID, USER_ID, StoCode, BillCode, Status,Ptimes);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetPayMethodList(Dictionary<string, object> dicPar)
        {
            try
            {
                //要检测的参数信息
                List<string> pra = new List<string>() { "GUID", "USER_ID", "page", "pagesize", "stocode", "order" };
                //检测方法需要的参数
                if (!CheckActionParameters(dicPar, pra))
                {
                    return;
                }
                StringBuilder postStr = new StringBuilder();
                //获取参数信息
                string GUID = Helper.ReplaceString(dicPar["GUID"].ToString());
                string USER_ID = Helper.ReplaceString(dicPar["USER_ID"].ToString());
                string page = Helper.ReplaceString(dicPar["page"].ToString());
                string pagesize = Helper.ReplaceString(dicPar["pagesize"].ToString());
                string stocode = Helper.ReplaceString(dicPar["stocode"].ToString());
                string order = Helper.ReplaceString(dicPar["order"].ToString());

                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/Istore/WSpaymethod.ashx";
                // "GUID", "USER_ID", "pageSize", "currentPage", "filter", "order" 
                postStr.Append("actionname=getlist&parameters={" +
                                               string.Format("'GUID':'{0}'", GUID) +
                                               string.Format(",'USER_ID': '{0}'", USER_ID) +
                                               string.Format(",'pageSize': '{0}'", pagesize) +
                                               string.Format(",'currentPage': '{0}'", page) +
                                               string.Format(",'filter': '{0}'", "status=1" ) +
                                               string.Format(",'order': '{0}'", order) +
                                       "}");//键值对

                string strWebJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                if (!string.IsNullOrEmpty(strWebJson) && strWebJson.Trim() != "")
                {
                    string status = "";
                    string mes = "";
                    JsonHelper.JsonToMessage(strWebJson, out status, out mes);
                    DataTable dt = JsonHelper.JsonToDataTable(strWebJson);
                    if (status == "0")
                    {
                        //获取本地禁用的支付方式
                        bllTB_PayMethod bllPM = new bllTB_PayMethod();
                        string filter= "TStatus='1' and Stocode='"+ stocode + "'";
                        int num = 0;
                        DataTable dtPM = bllPM.GetPagingListInfo(GUID, USER_ID, int.MaxValue, 1, filter, "", out num, out num);
                        if (dtPM != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow drNo in dtPM.Rows)
                            {
                                for(int i=dt.Rows.Count-1;i>0;i--)
                                {
                                    if (dt.Rows[i]["pmcode"].ToString() == drNo["PKCode"].ToString())
                                    {
                                        dt.Rows.Remove(dt.Rows[i]);
                                    }
                                }
                            }
                            dt.AcceptChanges();
                        }
                        ReturnListJson(dt);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                ToCustomerJson("2", ex.Message);
                return;
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
                        string val = dr["filter"].ToString();
                        switch (col)
                        {
                            case "TStatus":
                                
                                if (val == "1")
                                {
                                    filter += " and Tstatus in('0','2','3') ";
                                }
                                if (val == "2")
                                {
                                    filter += " and Tstatus='5' ";
                                }
                                break;
                            case "ftime":
                                    filter += " and (select count(*) from tb_billpay where billcode=b.pkcode and stocode=b.stocode)>0";
                                break;
                            case "OrderCodeList":
                                filter += " and charindex('"+ val + "',OrderCodeList)>0";
                                break;
                        }
                    }
                }
            }
            filter = GetBusCodeWhere(dicPar, filter, "b.buscode");
            //dicPar["orders"] = " ftime desc ";
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
            List<string> pra = new List<string>() { "GUID", "USER_ID",  "BusCode", "StoCode", "CCode", "CCname", "OrderCodeList",  "ShiftCode","BillType" };
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
            string TStatus = "0";
			string OrderCodeList = dicPar["OrderCodeList"].ToString();
            string PKCode = "";
            string BillMoney = "0";
            string PayMoney = "0";
            string ZeroCutMoney = "0";
			string ShiftCode = dicPar["ShiftCode"].ToString();
			string Remar = "";
            string FTime = "";
            string OpenDate = "";
            string DiscountName = "";
            string DiscountMoney = "";
            string AUCode = "";
            string AUName = "";
            string PointMoney = "0";
			string VirMoney = "0";
            string BillType= dicPar["BillType"].ToString();
            string PayWay = dicPar["PayWay"].ToString();
            string CStatus = dicPar["CStatus"].ToString();
            //调用逻辑
            logentity.pageurl ="TB_BillEdit.html";
			logentity.logcontent = "新增账单信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.Add(GUID, USER_ID, out  Id, BusCode, StoCode, CCode, CCname, TStatus, OrderCodeList, PKCode, BillMoney, PayMoney, ZeroCutMoney, ShiftCode, Remar, FTime, OpenDate, DiscountName, DiscountMoney, AUCode, AUName, PointMoney, VirMoney, BillType, PayWay, CStatus, entity);
			
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCode", "CCname", "TStatus", "OrderCodeList", "PKCode", "BillMoney", "PayMoney", "ZeroCutMoney", "ShiftCode", "Remar", "FTime", "OpenDate", "DiscountName", "DiscountMoney", "AUCode", "AUName", "PointMoney", "VirMoney", };
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
			string OrderCodeList = dicPar["OrderCodeList"].ToString();
			string PKCode = dicPar["PKCode"].ToString();
			string BillMoney = dicPar["BillMoney"].ToString();
			string PayMoney = dicPar["PayMoney"].ToString();
			string ZeroCutMoney = dicPar["ZeroCutMoney"].ToString();
			string ShiftCode = dicPar["ShiftCode"].ToString();
			string Remar = dicPar["Remar"].ToString();
			string FTime = dicPar["FTime"].ToString();
			string OpenDate = dicPar["OpenDate"].ToString();
			string DiscountName = dicPar["DiscountName"].ToString();
			string DiscountMoney = dicPar["DiscountMoney"].ToString();
			string AUCode = dicPar["AUCode"].ToString();
			string AUName = dicPar["AUName"].ToString();
			string PointMoney = dicPar["PointMoney"].ToString();
			string VirMoney = dicPar["VirMoney"].ToString();
            string BillType = dicPar["BillType"].ToString();
            string PayWay = dicPar["PayWay"].ToString();
            string CStatus= dicPar["CStatus"].ToString();
            //调用逻辑
            logentity.pageurl ="TB_BillEdit.html";
			logentity.logcontent = "修改id为:"+Id+"的账单信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID,  Id, BusCode, StoCode, CCode, CCname, TStatus, OrderCodeList, PKCode, BillMoney, PayMoney, ZeroCutMoney, ShiftCode, Remar, FTime, OpenDate, DiscountName, DiscountMoney, AUCode, AUName, PointMoney, VirMoney,BillType,PayWay, CStatus, entity);
            
            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BillCode","StoCode","UserCode" };
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
            DataSet  ds= bll.GetDetail(GUID, USER_ID, BillCode, StoCode, UserCode);

            if (ds != null && ds.Tables.Count >= 4)
            {
                DataTable dtBill = ds.Tables[0];
                DataTable dtPayMethod = ds.Tables[1];
                DataTable dtOpenTable = ds.Tables[3];
                DataTable dtBillCoupon = ds.Tables[4];

                //将退菜内容加入到Dish表
                DataTable dtDish = ds.Tables[2].Clone();
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    if (dr["DiscountType"].ToString() == "6")
                    {
                        dr["DisName"] = dr["DisName"] + "(赠)";
                    }
                    DataRow drAdd = dtDish.NewRow();
                    foreach (DataColumn dc in dtDish.Columns)
                    {
                        drAdd[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    drAdd["disnum"] = Helper.StringToDecimal(dr["disnum"].ToString()) + Helper.StringToDecimal(dr["returnnum"].ToString());
                    drAdd["totalmoney"]=(Helper.StringToDecimal(drAdd["discountprice"].ToString()) * Helper.StringToDecimal(drAdd["disnum"].ToString())).ToString("f2");
                    dtDish.Rows.Add(drAdd);
                    decimal returnnum = Helper.StringToDecimal(dr["returnnum"].ToString());
                    if (returnnum > 0)
                    {
                        DataRow drBack= dtDish.NewRow();
                        foreach (DataColumn dc in dtDish.Columns)
                        {
                            drBack[dc.ColumnName] = dr[dc.ColumnName];
                        }
                        drBack["disnum"] = "-" + dr["returnnum"].ToString();
                        drBack["totalmoney"] = (-1*Helper.StringToDecimal(dr["discountprice"].ToString()) * Helper.StringToDecimal(dr["returnnum"].ToString())).ToString("f2");
                        dtDish.Rows.Add(drBack);
                    }
                }

                ArrayList dtArray = new ArrayList() { dtBill, dtPayMethod, dtDish , dtOpenTable };
                string[] tablenames = { "BillList", "PayMethodList","DishList","OpenTableList" };
                string json = JsonHelper.ToJson("0", "获取成功", dtArray, tablenames);
                Pagcontext.Response.Write(json);
            }
        }

        private void MemDetail(Dictionary<string, object> dicPar)
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
            //调用逻辑		
            DataSet ds = bll.GetMemDetail(GUID, USER_ID, BillCode, StoCode);

            if (ds != null && ds.Tables.Count >=2)
            {
                DataTable dtBill = ds.Tables[0];
                DataTable dtPayMethod = ds.Tables[1];

                ArrayList dtArray = new ArrayList() { dtBill, dtPayMethod};
                string[] tablenames = { "BillList", "PayMethodList"};
                string json = JsonHelper.ToJson("0", "获取成功", dtArray, tablenames);
                Pagcontext.Response.Write(json);
            }
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
			logentity.pageurl ="TB_BillList.html";
			logentity.logcontent = "删除id为:"+Id+"的账单信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
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
            List<string> pra = new List<string>() { "GUID", "USER_ID", "billCode", "status","stoCode" };
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
            string status = dicPar["status"].ToString();

            logentity.pageurl ="TB_BillList.html";
			logentity.logcontent = "修改状态编号为:"+ billCode + "的账单信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, billCode,stoCode, status);

            ReturnListJson(dt);
        }

        /// <summary>
        /// 完成账单
        /// </summary>
        /// <param name="dicPar"></param>
        private void Finish(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BillCode","StoCode" };
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
            //调用逻辑
            DataSet ds = bll.Finish(GUID, USER_ID, BillCode, StoCode);
            ArrayList arrayTable = new ArrayList() { ds.Tables[0],ds.Tables[1],ds.Tables[2], ds.Tables[3], ds.Tables[4],ds.Tables[5],ds.Tables[6]};
            string[] names = {"opentable","bill","dish","pay","coupon","memcardorder","paydetail"};
            ReturnListJson("0", "获取成功", arrayTable, names);
        }

        /// <summary>
        /// 获取账单打印详情数据
        /// </summary>
        /// <param name="dicPar"></param>
        private void PrintDetail(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BillCode", "StoCode" };
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
            //调用逻辑
            DataSet ds = bll.PrintDetail(GUID, USER_ID, BillCode, StoCode);
            ArrayList arrayTable = new ArrayList() { ds.Tables[0], ds.Tables[1], ds.Tables[2], ds.Tables[3], ds.Tables[4], ds.Tables[5],ds.Tables[6]};
            string[] names = { "opentable", "bill", "dish", "pay", "coupon", "memcardorder","paydetail" };
            ReturnListJson("0", "获取成功", arrayTable, names);
        }

        /// <summary>
        /// 反结账单
        /// </summary>
        /// <param name="dicPar"></param>
        private void UnFinish(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BillCode", "StoCode" };
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
            //调用逻辑
            int rv = 1;
            DataSet ds = bll.UnFinish(GUID, USER_ID, BillCode, StoCode,out rv);
            if (rv == 0)
            {
                ArrayList arrayTable = new ArrayList() { ds.Tables[0], ds.Tables[1], ds.Tables[2], ds.Tables[3], ds.Tables[4] };
                string[] names = { "opentable", "bill", "dish", "pay", "coupon" };
                ReturnListJson("0", "获取成功", arrayTable, names);
            }
            else if (rv == 2)
            {
                DataTable dt = ds.Tables[0];
                string msg = "";
                foreach (DataRow dr in dt.Rows)
                {
                    msg += dr[0].ToString() + ",";
                }
                msg = msg.TrimEnd(',');
                ToCustomerJson("2", msg+"桌台已被占用，无法反结");
            }
            else if (rv == 1)
            {
                ToCustomerJson("1", "当前账单无法反结");
            }
           
        }

        /// <summary>
        /// 获取未出库的菜品
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetUnStockBillDish(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "StoCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            //调用逻辑
            DataTable dt = bll.GetUnStockBillDish(GUID, USER_ID, StoCode);
            ReturnListJson(dt);
        }

        private void ClearUnuseBillByTable(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "OpenCode","StoCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string OpenCode = dicPar["OpenCode"].ToString();
            string StoCode= dicPar["StoCode"].ToString();
            //调用逻辑
            DataTable dt = bll.ClearUnuseBillByTable(GUID, USER_ID, OpenCode,StoCode);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取门店的没有打印过的小程序下的订单
        /// </summary>
        /// <param name="dicPa"></param>
        private void GetAppUnPrintBill(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "StoCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            int page = 1;
            int pagesize = int.MaxValue;
            //调用逻辑
            string tablename = "TR_BillPrint";
            string filter = " stocode='"+ StoCode + "' and Tstatus='0'";
            string group = "";
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = new bllPaging().GetPagingInfo(tablename,"id","*",pagesize,page, filter, group,"ctime asc",out recnums,out pagenums);
            ReturnListJson(dt, pagesize, recnums, page, pagenums);
        }

        /// <summary>
        /// 给账单添加新的订单
        /// </summary>
        /// <param name="dicPar"></param>
        private void AddBillOrder(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BusCode", "StoCode","OrderCodeList", "BillCode" };
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
            string OrderCodeList = dicPar["OrderCodeList"].ToString();
            string BillCode = dicPar["BillCode"].ToString();
            //调用逻辑
            dt = bll.AddOrder(GUID, USER_ID, OrderCodeList, BusCode, StoCode, BillCode);
            ReturnListJson(dt);
        }
    }
}