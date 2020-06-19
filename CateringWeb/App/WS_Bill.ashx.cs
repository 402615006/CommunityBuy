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
using System.Runtime.Remoting.Messaging;
using System.Resources;

namespace CommunityBuy
{
    /// <summary>
    /// 菜品信息接口类
    /// </summary>
    public class WS_Bill :IServices.ServiceBase
    {
        bllTB_Bill bll = new bllTB_Bill();
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
                        case "i_addbill"://添加账单
                            AddBill(dicPar);
                            break;
                        case "i_getbill"://列表
                            GetBill(dicPar);
                            break;
                        case "i_finishbill"://完成结账
                            FinishBill(dicPar);
                            break;
                        case "i_usecoupon"://使用优惠券
                            AddCoupon(dicPar);
                            break;
                        case "i_cancelcoupon"://取消优惠券
                            CancelCoupon(dicPar);
                            break;
                        case "i_wxpay"://请求微信支付的参数
                            AddWxPay(dicPar);
                            break;
                        case "i_refreshwxpay"://联锁端更新微信支付的账单
                            UpdateWxPay(dicPar);
                            break;
                        case "i_refreshbill"://手动刷新账单
                            RefreshBill(dicPar);
                            break;
                        case "i_cancelpay"://反结算
                            CancelPay(dicPar);
                            break;
                        case "i_cancelbill"://取消账单
                            CancelBill(dicPar);
                            break;

                    }
                }
            }
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dicPar"></param>
        public void AddBill(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "ordercodelist", "memcode"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = "";
            string USER_ID = "";
            string Id = "0";
            string BusCode = dicPar["buscode"].ToString();
            string StoCode = dicPar["stocode"].ToString();
            string CCode = dicPar["memcode"].ToString();
            string CCname = "小程序";
            string TStatus = "0";
            string OrderCodeList = dicPar["ordercodelist"].ToString();
            string PKCode = "";
            string BillMoney = "0";
            string PayMoney = "0";
            string ZeroCutMoney = "0";
            string ShiftCode = dicPar["shiftcode"].ToString();
            string Remar = "";
            string FTime = "";
            string OpenDate = "";
            string DiscountName = "";
            string DiscountMoney = "";
            string AUCode = "";
            string AUName = "";
            string PointMoney = "0";
            string VirMoney = "0";
            string BillType = "1";
            string PayWay = "2";
            string CStatus = "0";

            bll.Add(GUID, USER_ID, Id, BusCode, StoCode, CCode, CCname, TStatus, OrderCodeList, PKCode, BillMoney, PayMoney, ZeroCutMoney, ShiftCode, Remar, FTime, OpenDate, DiscountName, DiscountMoney, AUCode, AUName, PointMoney, VirMoney, BillType, PayWay, CStatus);
            ReturnJsonByT<BLL.OperateResult>(bll.oResult);
        }

        /// <summary>
        /// 获取订餐和菜品
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetBill(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "billcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string billcode = dicPar["billcode"].ToString();
            string GUID = "";
            string USER_ID = "";
            int pageSize = 99999999;
            int currentPage = 1;
            string filter =  " PKCode='"+ billcode + "' ";
            string order = "";

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑

            DataTable dt = new bllTB_Bill().GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt,pageSize,recordCount,currentPage,totalPage);
        }

        /// <summary>
        /// 完成结账
        /// </summary>
        /// <param name="dicPar"></param>
        public void FinishBill(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "billcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string guid = "";
            string uid = "";
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string billcode = dicPar["billcode"].ToString();
            //调用逻辑
            DataSet ds = new bllTB_Bill().Finish(guid, uid, billcode, stocode);
            ArrayList arrayTable = new ArrayList() { ds.Tables[0], ds.Tables[1], ds.Tables[2], ds.Tables[3], ds.Tables[4], ds.Tables[5] };
            string[] names = { "opentable", "bill", "dish", "pay", "coupon", "memcardorder" };
           //打印结账单
            ReturnManyListJson("0", "账单支付完成", arrayTable, names,null,null,null,null);
        }

        /// <summary>
        /// 刷新账单信息
        /// </summary>
        /// <param name="billcode"></param>
        /// <param name="stocode"></param>
        public void RefreshBill(string billcode, string stocode,string msg)
        {
            DataSet ds = new bllTB_Bill().GetDetail("", "", billcode, stocode, "");
            DataTable dtBill = ds.Tables[0];


            DataTable dtPay = ds.Tables[1];
            DataTable dtDish = ds.Tables[2];
            DataTable dtTable = ds.Tables[3];
            DataTable dtCoupon = ds.Tables[4];
            ArrayList arr = new ArrayList();
            arr.Add(dtBill);
            arr.Add(dtPay);
            arr.Add(dtDish);
            arr.Add(dtTable);
            arr.Add(dtCoupon);
            string[] names = { "bill", "pay", "dish", "table", "coupon" };
            if (msg == "反结成功")
            {
                //更新菜品的折扣信息
                new bllTB_OrderDish().UpdateByTable("", "", stocode, dtDish);
            }
            if (string.IsNullOrWhiteSpace(msg))
            {
                msg = "成功";
            }
            ReturnManyListJson("0", msg, arr, names,null,null,null,null);

            string billstatus = dtBill.Rows[0]["Tstatus"].ToString();
        }

        /// <summary>
        /// 刷新账单信息
        /// </summary>
        /// <param name="billcode"></param>
        /// <param name="stocode"></param>
        public void RefreshBill(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "billcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string stocode = dicPar["stocode"].ToString();
            string billcode = dicPar["billcode"].ToString();
            DataSet ds = new bllTB_Bill().GetDetail("", "", billcode, stocode, "");
            DataTable dtBill = ds.Tables[0];
            dtBill.Columns.Add("StoreName");
            dtBill.Columns.Add("PayType");
            string stoname = "";
            string paytype = "";

            foreach (DataRow drBill in dtBill.Rows)
            {
                drBill["StoreName"] = stoname;
                drBill["PayType"] = paytype;
                if (drBill["Tstatus"].ToString() == "5")
                {
                    drBill["Tstatus"] = "1";
                }
            }
            dtBill.AcceptChanges();

            DataTable dtPay = ds.Tables[1];
            DataTable dtDish = ds.Tables[2];
            DataTable dtTable = ds.Tables[3];
            DataTable dtCoupon = ds.Tables[4];

            ArrayList arr = new ArrayList();
            arr.Add(dtBill);
            arr.Add(dtPay);
            arr.Add(dtDish);
            arr.Add(dtTable);
            arr.Add(dtCoupon);
            string[] names = { "bill", "pay", "dish", "table", "coupon" };
            string billstatus = dtBill.Rows[0]["Tstatus"].ToString();
            ReturnManyListJson("0", "成功", arr, names,null,null,null,null);
        }

        /// <summary>
        /// 从缓存更新账单信息
        /// </summary>
        public void RefreshBillFromCache(DataTable dtBill, DataTable dtPay,DataTable dtDish, DataTable dtTable, DataTable dtCoupon)
        {
            ArrayList arr = new ArrayList();
            arr.Add(dtBill);
            arr.Add(dtPay);
            arr.Add(dtDish);
            arr.Add(dtTable);
            arr.Add(dtCoupon);
            string[] names = { "bill", "pay", "dish", "table", "coupon" };
            ReturnManyListJson("0", "成功", arr, names,null,null,null,null);
        }

        /// <summary>
        /// 更新账单和菜品信息
        /// </summary>
        /// <param name="dtBill"></param>
        /// <param name="dtDish"></param>
        public void UpdateBillAndDish(DataTable dtBill, DataTable dtDish)
        {
            new bllApp().UpdateBillAndDish(dtBill,dtDish);
        }

        /// <summary>
        /// 使用优惠券
        /// </summary>
        /// <param name="dicPar"></param>
        public void AddCoupon(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "billcode", "couponinfo" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string guid = "1";
            string uid = "1";
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string billcode = dicPar["billcode"].ToString();
            //string couponJson = dicPar["couponinfo"].ToString();
            string memcode = "";
            dynamic couponObj = dicPar["couponinfo"];
            string paymethod = couponObj["usepaytype"].ToString();
            //获取账单详情、菜品详情
            DataSet ds = new bllTB_Bill().GetDetail("", "", billcode, stocode, "");
            DataTable dtBill = ds.Tables[0];
            DataTable dtPay = ds.Tables[1];
            DataTable dtDish = ds.Tables[2];
            DataTable dtCoupon = ds.Tables[4];
            
            //使用优惠券
            if (dtBill != null && dtBill.Rows.Count > 0)
            {
                string BillCode = dtBill.Rows[0]["PKCode"].ToString();
                string ShiftCode = dtBill.Rows[0]["ShiftCode"].ToString();
                memcode = dtBill.Rows[0]["CCode"].ToString();
                decimal ToPayMoney = StringHelper.StringToDecimal(dtBill.Rows[0]["ToPayMoney"].ToString());
                decimal PayMoney = StringHelper.StringToDecimal(dtBill.Rows[0]["PayMoney"].ToString());
                if (PayMoney > 0)
                {
                    ReturnResultJson("1", "账单已有支付，无法使用优惠券");
                    return;
                }
                if (ToPayMoney <= 0)
                {
                    ReturnResultJson("1", "付款金额为0，无法使用优惠券。");
                    return;
                }

                //使用优惠券
                TB_BillCouponEntity CouponEntity = new TB_BillCouponEntity();
                CouponEntity.BusCode = buscode;
                CouponEntity.StoCode = stocode;
                CouponEntity.BillCode = billcode;
                CouponEntity.ShiftCode = ShiftCode;
                CouponEntity.TicType = "0";
                CouponEntity.TicWay = "";
                string msg = OrderHelper.UseCoupon(billcode, couponObj, ToPayMoney, ref dtDish, ref CouponEntity, dtCoupon);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    //线上验券
                    string MemcardUrl = Helper.GetAppSettings("ServiceUrl") + "/coupon/WScheckcoupon.ashx";
                    StringBuilder postStr = new StringBuilder();
                    postStr.Append("actionname=couponrecoverynew&usercode=" + uid + "&parameters={\"GUID\":\"" + guid + "\",\"USER_ID\":\"" + guid + "\",\"buscode\":\"" + buscode + "\",\"couponcode\":\"" + CouponEntity.CouponCode + "\",\"stocode\":\"" + stocode + "\",\"way\":\"PC\",\"username\":\"线上点餐\",\"usercode\":\"" + memcode + "\",\"orderno\":\"" + billcode + "\"}");
                    string strAdminJson = Helper.HttpWebRequestByURL(MemcardUrl, postStr.ToString());
                    if (!string.IsNullOrEmpty(strAdminJson))
                    {
                        string checkmsg = string.Empty;
                        string status = string.Empty;
                        DataSet dsCheckCcoupon = JsonHelper.JsonToDataSet(strAdminJson, out status, out checkmsg);
                        if (status == "0")
                        {
                            //添加使用券记录
                            string id = "";
                            DataTable dtAdd = new BLL.bllTB_BillCoupon().Add("", "", id, buscode, stocode, "", "线上点餐", "1", CouponEntity.BillCode, CouponEntity.CouponCode, CouponEntity.CouponMoney.ToString(), "", CouponEntity.RealPay.ToString(), CouponEntity.VIMoney.ToString(), CouponEntity.Remark, CouponEntity.UseType, ShiftCode, CouponEntity.CouponName, CouponEntity.McCode, CouponEntity.TicType,CouponEntity.TicWay);
                            string restatus = "1";
                            string remsg = "优惠券使用失败";
                            if (dtAdd != null && dtAdd.Rows.Count > 0)
                            {
                                restatus = "0";
                                remsg = "优惠券使用成功";
                                //更新菜品和账单信息
                                dtCoupon.Rows.Add(CouponEntity.CouponName, CouponEntity.CouponCode, CouponEntity.CouponMoney, CouponEntity.RealPay, CouponEntity.VIMoney,CouponEntity.BillCode,CouponEntity.McCode);
                                dtCoupon.AcceptChanges();
                                OrderHelper.ResetBillMoney(ref dtBill, dtDish, dtCoupon);
                                dtBill.Rows[0]["PayMethod"] = paymethod;
                                UpdateBillAndDish(dtBill, dtDish);
                                RefreshBill(billcode, stocode,"");
                                return;
                            }
                            if (restatus != "0")
                            {
                                //执行取消优惠券接口
                                postStr = new StringBuilder();
                                postStr.Append("actionname=couponrecoverycancelnew&usercode=" + uid + "&parameters=" +
                                    "{\"GUID\":\"" + guid + "\"," +
                                    "\"USER_ID\":\"" + guid + "\"," +
                                    "\"buscode\":\"" + buscode + "\"," +
                                    "\"couponcode\":\"" + CouponEntity.CouponCode + "\"," +
                                    "\"stocode\":\"" + stocode + "\"," +
                                    "\"username\":\"线上点餐\"," +
                                    "\"usercode\":\"" + memcode + "\"," +
                                    "\"way\":\"App\"}");
                                Helper.HttpWebRequestByURL(MemcardUrl, postStr.ToString());
                            }
                            ReturnResultJson(restatus, checkmsg);
                            return;
                        }
                    }
                    ReturnResultJson("1", "线上消券错误");
                    return;
                }
                else
                {
                    ReturnResultJson("1", msg);
                    return;
                }
            }
            else
            {
                ReturnResultJson("1", "账单获取错误，请重试");
            }
        }

        /// <summary>
        /// 取消使用优惠券
        /// </summary>
        /// <param name="dicPar"></param>
        public void CancelCoupon(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "billcode", "couponcode","memcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string key= dicPar["key"].ToString();
            string couponcode = dicPar["couponcode"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string billcode = dicPar["billcode"].ToString();
            //联锁端取消
            string MemcardUrl = Helper.GetAppSettings("ServiceUrl") + "/coupon/WScheckcoupon.ashx";
            StringBuilder postStr = new StringBuilder();
            postStr.Append("actionname=couponrecoverycancelnew&usercode=" + memcode + "&parameters=" +
                "{\"GUID\":\"" + memcode + "\"," +
                "\"USER_ID\":\"" + memcode + "\"," +
                "\"buscode\":\"" + buscode + "\"," +
                "\"couponcode\":\"" + couponcode + "\"," +
                "\"stocode\":\"" + stocode + "\"," +
                "\"username\":\"" + memcode + "\"," +
                "\"usercode\":\"" + memcode + "\"," +
                "\"way\":\"App\"}");
            string strAdminJson = Helper.HttpWebRequestByURL(MemcardUrl, postStr.ToString());
            if (!string.IsNullOrEmpty(strAdminJson))
            {
                string status = "";
                string mgs = "";
                JsonHelper.JsonToMessage(strAdminJson, out status, out mgs);
                if (status == "0")
                {
                    //本地删除
                    DataSet ds =new  bllTB_BillCoupon().Cancel(memcode, memcode, stocode, billcode, couponcode, "1");
                    //刷新账单
                    RefreshBill(billcode, stocode,"");
                }
            }
        }

        /// <summary>
        ///添加支付信息 
        /// </summary>
        /// <param name="dicPar"></param>
        public void AddWxPay(Dictionary<string, object> dicPar)
        {

            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "memcode", "billcode", "paymoney","openid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = "";
            string USER_ID = "";
            string Id = "0";
            string BusCode = dicPar["buscode"].ToString();
            string StoCode = dicPar["stocode"].ToString();
            string CCode = dicPar["memcode"].ToString();
            string CCname = dicPar["memcode"].ToString();
            string TStatus = "0";
            string PKCode = "0";
            string BillCode = dicPar["billcode"].ToString();
            string PayMoney = dicPar["paymoney"].ToString();
            string PayMethodName = "微信支付";
            string PayMethodCode = "2";
            string Remar = "";
            string OutOrderCode = "";
            string PPKCode = "";
            string openid = dicPar["openid"].ToString();

            //获取账单详情、菜品详情
            DataSet ds = new bllTB_Bill().GetDetail("", "", BillCode, StoCode, "");
            DataTable dtBill = ds.Tables[0];
            DataTable dtPay = ds.Tables[1];
            DataTable dtDish = ds.Tables[2];
            DataTable dtTable = ds.Tables[3];
            DataTable dtCoupon = ds.Tables[4];
            if (dtBill == null || dtBill.Rows.Count < 1)
            {
                ReturnResultJson("1", "账单信息获取失败");
                return;
            }
            //账单信息
            string DiscountMoney = dtBill.Rows[0]["DiscountMoney"].ToString();
            string DiscountName = dtBill.Rows[0]["DiscountName"].ToString();
            string ZeroCutMoney = dtBill.Rows[0]["ZeroCutMoney"].ToString();
            string AuthCode = "";
            string AuthName = "";
            //调用逻辑
            new bllTB_BillPay().Add(GUID, USER_ID, Id, BusCode, StoCode, CCode, CCname, TStatus,  PKCode, BillCode, PayMoney, PayMethodName, PayMethodCode, Remar, OutOrderCode, PPKCode, DiscountName, DiscountMoney, ZeroCutMoney, AuthCode, AuthName ,"","","0","","");
            if (PKCode.Length > 4)
            {
                switch (PayMethodCode)
                {
                    case "2"://微信支付 
                        string WxUrl = Helper.GetAppSettings("ServiceUrl") + "/networkpay/WxPayEx.ashx";
                        StringBuilder wxPostStr = new StringBuilder();
                        wxPostStr.Append("actionname=getpayparms&parameters={" +
                                                        string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'openid': '{0}'", openid) + //openid
                                                        string.Format(",'orderno': '{0}'", PKCode) + //支付单号
                                                        string.Format(",'money': '{0}'", PayMoney) + //订单金额
                                                        string.Format(",'callback': '{0}'",HttpUtility.UrlEncode(Helper.GetAppSettings("StoBackRoot") +"/App/Ws_Bill.ashx?actionname=i_refreshWxPay&parameters={\"payno\":\"" + PKCode + "\",\"buscode\":\"" + BusCode + "\",\"stocode\":\"" + StoCode + "\"}")) + 
                                                "}");//键值对
                        string wxResultJson = Helper.HttpWebRequestByURL(WxUrl, wxPostStr.ToString());
                        string wxPayStatus =JsonHelper.GetJsonValByKey (wxResultJson,"status");
                        if (wxPayStatus == "0")
                        {
                            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(wxResultJson);
                            ReturnJsonStr("{\"code\":0,\"msg\":" + jsonObj["mes"].ToString() + "}");
                        }
                        else
                        {
                            ReturnResultJson("1", "请求微信支付失败");
                        }
                        break;
                }
            }
            else
            {
                ReturnResultJson("1", "支付失败");
            }
        }

        /// <summary>
        /// 联锁端请求微信支付的结果
        /// </summary>
        /// <param name="dicPar"></param>
        public void UpdateWxPay(Dictionary<string, object> dicPar)
        {
            bllTB_BillPay bllPay = new bllTB_BillPay();
            //要检测的参数信息
            List<string> pra = new List<string>() { "buscode", "stocode", "payno"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = "";
            string USER_ID = "";
            string BusCode = dicPar["buscode"].ToString();
            string StoCode = dicPar["stocode"].ToString();
            string PKCode = dicPar["payno"].ToString();

            TB_BillPayEntity UEntity = bllPay.GetEntitySigInfo(" where pkcode='"+ PKCode + "'");
            UEntity.TStatus = "1";
            bllPay.Update(GUID,USER_ID,UEntity);
            string type = bllPay.oResult.Code;
            if (type != "1")
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, "支付单号：" + PKCode + "支付回调失败");
            }
            ReturnResultJson(bll.oResult.Code,bll.oResult.Msg);

        }
        
        /// <summary>
        /// 取消支付
        /// </summary>
        /// <param name="dicPar"></param>
        public void CancelPay(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "billcode","memcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string billcode = dicPar["billcode"].ToString();
            string memcode= dicPar["memcode"].ToString();
            //检查账单的状态，是否能进行反结
            TB_BillEntity billEntity=new bllTB_Bill().GetEntitySigInfo(" where PKCode='" + billcode + "' and StoCode='" + stocode + "'");
            if (billEntity == null||billEntity.TStatus!="2")
            {
                ReturnResultJson("1", "账单无法取消支付！");
                return;
            }
            //获取改账单的支付方式
            int nums = 0;
            string filter = " where BillCode='"+ billcode + "' and PayMethodCode='7'  and TStatus='1' ";
            DataTable dtBillPay=new bllTB_BillPay().GetPagingListInfo("", "", int.MaxValue, 1, filter, "", out nums, out nums);
            //反结
            string msg = "反结成功";
            if (dtBillPay != null && dtBillPay.Rows.Count > 0)
            {
                foreach (DataRow dr in dtBillPay.Rows)
                {
                    string PKCode = "";
                    string PayMoney = dr["PayMoney"].ToString();
                    string Remar = dr["Remar"].ToString();
                    string OutOrderCode = "";
                    string PPKCode= dr["PKCode"].ToString();
                    string PayMethodCode = "";
                    string memcard = dr["MemberCard"].ToString();

                    new bllTB_BillPay().Back("", "", buscode, stocode, memcode, "小程序", "2", out PKCode, PayMoney, Remar, out OutOrderCode, PPKCode, out PayMethodCode);
                    if (PKCode.Length > 4)
                    {
                        string MemUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardreturn.ashx";
                        StringBuilder memPostStr = new StringBuilder();
                        memPostStr.Append("actionname=cardpayreturnnew&parameters={" +
                                                        string.Format("'GUID':'{0}'", memcode) +
                                                        string.Format(",'USER_ID': '{0}'", memcode) +
                                                        string.Format(",'buscode': '{0}'", buscode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", stocode) + //门店编号
                                                        string.Format(",'terminalType': '{0}'", "App") + //终端编号
                                                        string.Format(",'ucode': '{0}'", memcode) + //操作人编号
                                                        string.Format(",'uname': '{0}'", "小程序") + //操作人姓名
                                                        string.Format(",'paycode': '{0}'", billcode) + //主支付编号
                                                        string.Format(",'vicepaycode': '{0}'", PKCode) + //副支付编号
                                                        string.Format(",'rvicepaycode': '{0}'", PPKCode) + //待反结副支付编号
                                                        string.Format(",'couponcode': '{0}'", "") + //优惠券编号（多个以英文逗号分隔）
                                                        string.Format(",'cardcode': '{0}'", memcard) + //卡号
                                                        string.Format(",'expend': '{0}'", PayMoney) + //消费金额
                                                        string.Format(",'isopen': '{0}'", "0") + //是否开票
                                                        string.Format(",'invamount': '{0}'", "0") + //开票金额
                                                        string.Format(",'addint': '{0}'", "0") + //可积积分
                                                        string.Format(",'deductionint': '{0}'", "0") + //抵扣积分 
                                                "}");//键值对
                        string memResultJson = Helper.HttpWebRequestByURL(MemUrl, memPostStr.ToString());
                        string memPayStatus = JsonHelper.GetJsonValByKey(memResultJson, "status");
                        if (memPayStatus != "0")
                        {
                            msg = "有支付反结失败，请重试";
                            new bllTB_BillPay().Delete(memcode, memcode, PKCode, stocode);
                            break;
                        }
                    }
                }
            }
            RefreshBill(billcode, stocode, msg);
        }

        /// <summary>
        /// 先付款，取消账单
        /// </summary>
        /// <param name="dicPar"></param>
        public void CancelBill(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "billcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string billcode = dicPar["billcode"].ToString();
            //检查账单的状态，是否能进行反结
            TB_BillEntity billEntity = new bllTB_Bill().GetEntitySigInfo(" where PKCode='" + billcode + "' and StoCode='" + stocode + "' and TStatus in('0','3')");
            if (billEntity == null||string.IsNullOrWhiteSpace(billEntity.PKCode))
            {
                ReturnResultJson("1", "账单已支付无法取消！");
                return;
            }
            //获取改账单的支付方式
            int rel=new bllApp().CancelBill(stocode,billcode);
            if (rel >= 0)
            {
                ReturnResultJson("0", "账单已取消！");
            }
            else
            {
                ReturnResultJson("1", "账单取消失败！");
            }
        }
    }
}