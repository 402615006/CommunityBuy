using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.App
{
    /// <summary>
    /// 菜品信息接口类
    /// </summary>
    public class WS_Order :IServices.ServiceBase
    {
        bllTB_Dish bll = new bllTB_Dish();
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
                    logentity.module = "菜品信息";
                    switch (actionname.ToLower())
                    {
                        case "i_addorder"://下单
                            AddOrder(dicPar);
                            break;
                        case "i_getorder"://获取订单
                            GetOrder(dicPar);
                            break;
                        //case "i_cancelorder"://取消订单
                        //    CancelOrder(dicPar);
                        //    break;
                    }
                }
            }
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dicPar"></param>
        public void AddOrder(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "opencode", "disnum", "distypenum", "remark", "ordermoney", "memcode", "dishlist", "departcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string opencode = dicPar["opencode"].ToString();
            string disnum = dicPar["disnum"].ToString();
            string distypenum = dicPar["distypenum"].ToString();
            string remark = dicPar["remark"].ToString();
            string ordermoney = dicPar["ordermoney"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string dishlist = new JavaScriptSerializer().Serialize(dicPar["dishlist"]);
            string departcode = dicPar["departcode"].ToString();


            string ordercode = "";

            logentity.pageurl = "WS_Order.ashx";
            logentity.logcontent = "小程序下单";
            logentity.cuser = StringHelper.StringToLong("0");
            logentity.otype = SystemEnum.LogOperateType.Add;

            //判断门店是先支付，还是后支付
            DataTable dtStore = GetCacheToStore("");
            string stoname = "";
            string paytype = "";
            foreach (DataRow drStore in dtStore.Select("stocode='" + stocode + "'"))
            {
                stoname = drStore["cname"].ToString();
                paytype = drStore["ptype"].ToString();
                if (!string.IsNullOrWhiteSpace(stoname))
                {
                    break;
                }
            }
            //后支付、下单状态是挂单
            string orderstatus = "1";
            string ordertype = "4";
            if (paytype == "1")
            {
                orderstatus = "0";
                ordertype = "5";
            }

            DataTable dt = new bllTB_Order().Add(dishlist, "", "", "", buscode, stocode, memcode, "小程序", orderstatus, out ordercode, opencode, ordermoney, disnum, distypenum, remark, "", "", ordertype, departcode, logentity);
            ReturnJson(dt);
            if (dt.Rows[0]["type"].ToString() == "0")
            {
                //设置桌台状态
                if (orderstatus =="0")
                {
                    new bllTB_OpenTable().UpdateStatus("", "", stocode, opencode, "6");
                }
                //下单成功，打印制作单
                //int num;
                //string filter = " PKCode='" + opencode + "' and stocode='"+ stocode + "' ";
                //DataTable dtOPenTable = new bllTB_OpenTable().GetPagingListInfo("", "", 1, 1, filter, "", out num, out num);
                //if (paytype == "1")
                //{
                //    PrintCookPaper(stocode, ordercode, dishlist, dtOPenTable);
                //}
            }
        }

        /// <summary>
        /// 获取订餐和菜品
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetOrder(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "opencode"};

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string opencode = dicPar["opencode"].ToString();
            string filter = " o.stocode='"+ stocode + "' and o.opencodelist='"+ opencode + "'";
            string order = "";
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = new bllTB_Order().GetOrderDisPagingListInfo("", "", 999999, 1, filter, order, out recordCount, out totalPage);

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
            dtResult.Columns.Add("status");
            if (dt != null)
            {

                DataTable dtOrder = dt.DefaultView.ToTable(true, "code");
                foreach (DataRow dr in dtOrder.Rows)
                {
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
                        dtDish.Rows.Add(drAdd);
                    }
                    string jsonDish = JsonHelper.DataTableToJSON(dtDish);

                    DataRow drResult = dtResult.NewRow();

                    DataRow drOrder = dt.Select("code='" + dr["code"] + "'")[0];
                    drResult["code"] = drOrder["code"];
                    drResult["ctime"] = StringHelper.StringToDateTime(drOrder["ctime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    drResult["money"] = drOrder["money"];
                    drResult["disnum"] = drOrder["disnum"];
                    drResult["distypenum"] = drOrder["distypenum"];
                    drResult["ordertype"] = drOrder["ordertype"];
                    drResult["tablename"] = drOrder["tablename"];
                    drResult["cusnum"] = drOrder["cusnum"];
                    drResult["opentime"] = drOrder["opentime"];
                    drResult["openman"] = drOrder["openman"];
                    drResult["status"] = drOrder["status"];
                    drResult["dishList"] = jsonDish;
                    dtResult.Rows.Add(drResult);
                }
            }
            ReturnListJson(dtResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicPar"></param>
        //public void CancelOrder(Dictionary<string, object> dicPar)
        //{
        //    string stocode = dicPar["stocode"].ToString();
        //    string ordercode= dicPar["ordercode"].ToString();

        //    //获取门店的付款方式
        //    DataRow drStore = null;
        //    DataTable dtStore = GetCacheToStore("");
        //    if (dtStore != null && dtStore.Rows.Count > 0)
        //    {
        //        DataRow[] drs= dtStore.Select("stocode='" + stocode + "'");
        //        if (drs != null && drs.Length > 0)
        //        {
        //            drStore = drs[0];
        //        }
        //    }
        //    if (drStore != null)
        //    {
        //        string ptype = drStore["ptype"].ToString();
        //    }
        //}

        /// <summary>
        /// 打印制作单
        /// </summary>
        public void PrintCookPaper(string stocode,string ordercode, string dishlist, DataTable dtOpenTable)
        {
            try
            {
                DataTable dtStore = GetCacheToStore("");
                string stoname="";

                foreach (DataRow dr in dtStore.Rows)
                {
                    if (dr["stocode"].ToString() == stocode)
                    {
                        stoname = dr["cname"].ToString();
                        break;
                    }
                }

                //BusCode(商户编号),StoCode(门店编号),StoName(门店名称),OrderCode(订单编号),TableCode(桌台编号),TableName(桌台名称),CCname(操作人),CTime(操作时间),CusNum(客人数量),KitCode(厨房编号),TypeCode(菜品类别编号),DisName(菜品名称),DisNum(菜品数量)
                DataTable dtPrint = new DataTable();
                dtPrint.Columns.Add("BusCode");
                dtPrint.Columns.Add("StoCode");
                dtPrint.Columns.Add("StoName");
                dtPrint.Columns.Add("OrderCode");
                dtPrint.Columns.Add("TableCode");
                dtPrint.Columns.Add("TableName");
                dtPrint.Columns.Add("CCode");
                dtPrint.Columns.Add("CCname");
                dtPrint.Columns.Add("CTime");
                dtPrint.Columns.Add("CusNum");
                dtPrint.Columns.Add("KitCode");
                dtPrint.Columns.Add("TypeCode");
                dtPrint.Columns.Add("DisName");
                dtPrint.Columns.Add("DisNum");
                dtPrint.Columns.Add("DisCode");
                DataTable dtDish = JsonHelper.JsonToDataTable(dishlist);
                if (dtDish != null)
                {
                    string cusNum = dtOpenTable.Rows[0]["cusnum"].ToString();
                    string tablecode = dtOpenTable.Rows[0]["tablecode"].ToString();
                    string tablename = dtOpenTable.Rows[0]["tablename"].ToString();
                    foreach (DataRow dr in dtDish.Rows)
                    {
                        if (StringHelper.StringToDecimal(dr["disnum"].ToString()) > 0)
                        {
                            DataRow drAdd = dtPrint.NewRow();
                            drAdd["BusCode"] = "88888888";
                            drAdd["StoCode"] = stocode;
                            drAdd["StoName"] = stoname;
                            drAdd["OrderCode"] = ordercode;
                            drAdd["TableCode"] = tablecode;
                            drAdd["TableName"] = tablename;
                            drAdd["CCode"] = "0";
                            drAdd["CCname"] = "小程序";
                            drAdd["CTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            drAdd["CusNum"] = cusNum;
                            drAdd["KitCode"] = dr["kitcode"];
                            drAdd["TypeCode"] = dr["typecode"];
                            drAdd["DisName"] = dr["disname"];
                            drAdd["DisNum"] = dr["disnum"];
                            drAdd["DisCode"] = dr["discode"];
                            dtPrint.Rows.Add(drAdd);
                        }
                    }
                    PrintAdapter printAdapter = new PrintAdapter();
                    printAdapter.InsertProNote(dtPrint);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                return;
            }

        }

    }
}