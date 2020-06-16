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
    /// 账单支付接口类
    /// </summary>
    public class WSTB_BillPay : ServiceBase
    {
        bllTB_BillPay bll = new bllTB_BillPay();
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
					logentity.module = "账单支付";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "add"://添加							
                            Add(dicPar);
                            break;
                        case "back"://反结							
                            Back(dicPar);
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
                        case "checkmemberpayresult"://检查会员卡支付结果
                            CheckMemberPayResult(dicPar);
                            break;
                        case "cancelmemberscanpay"://取消会员扫码支付
                            CancelMemberScanPay(dicPar);
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
            int pageSize = Helper.StringToInt(dicPar["limit"].ToString());
            int currentPage = Helper.StringToInt(dicPar["page"].ToString());
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            string billcode = dicPar["billcode"].ToString();

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
            //string order = dicPar["orders"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //根据账单，去查询未成功的单据，对比网上的数据是否成功

            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
           
            ReturnListJson(dt);
        } 
        
        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BusCode", "StoCode", "CCode", "CCname", "TStatus",  "BillCode",
                "PayMoney", "PayMethodName", "PayMethodCode", "Remar", "OutOrderCode", "PPKCode","PointUse","PointGive","MemCard","Coupons","DiscountMoney","DiscountName","ZeroCutMoney","CardPayCode","MemberName","MemberBalance","MemberLeve","MemberDiscount"};
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
			string TStatus = dicPar["TStatus"].ToString();
            string PKCode = "0";
			string BillCode = dicPar["BillCode"].ToString();
			string PayMoney = dicPar["PayMoney"].ToString();
			string PayMethodName = dicPar["PayMethodName"].ToString();
			string PayMethodCode = dicPar["PayMethodCode"].ToString();
			string Remar = dicPar["Remar"].ToString();
			string OutOrderCode = dicPar["OutOrderCode"].ToString();
			string PPKCode = dicPar["PPKCode"].ToString();

            //账单信息
            string DiscountMoney = dicPar["DiscountMoney"].ToString();
            string DiscountName = dicPar["DiscountName"].ToString();
            string ZeroCutMoney = dicPar["ZeroCutMoney"].ToString();
            string AuthCode= dicPar["AuCode"].ToString();
            string AuthName = dicPar["AuName"].ToString();
            //会员卡
            string PointUse = dicPar["PointUse"].ToString();//抵扣积分
            string PointGive = dicPar["PointGive"].ToString();//积分
          
            string Coupons = dicPar["Coupons"].ToString();//会员卡优惠券
            string CardPayCode= dicPar["CardPayCode"].ToString();//会员卡读卡方式
            string MemCard = dicPar["MemCard"].ToString();//会员卡
            string MemberName = dicPar["MemberName"].ToString();//会员名称
            string MemberBalance = dicPar["MemberBalance"].ToString();//会员卡余额
            string CardType = dicPar["CLType"].ToString();//会员卡类型0年费卡1Plus卡2次卡
            if (string.IsNullOrWhiteSpace(MemberBalance))
            {
                MemberBalance = "0";
            }
            string MemberLeve = dicPar["MemberLeve"].ToString();//会员卡等级
            string MemberDiscount = dicPar["MemberDiscount"].ToString();//会员卡折扣方案名称
            if (!string.IsNullOrWhiteSpace(CardPayCode)) {
                OutOrderCode = CardPayCode;
            }

            //调用逻辑
            logentity.pageurl ="TB_BillPayEdit.html";
			logentity.logcontent = "新增账单支付信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Add;
            if (CardType == "0")
            {
                Remar = "nfk";
            }
                dt = bll.Add(GUID, USER_ID, Id, BusCode, StoCode, CCode, CCname, TStatus, out PKCode, BillCode, PayMoney, PayMethodName, PayMethodCode, Remar, OutOrderCode, PPKCode,DiscountName,DiscountMoney,ZeroCutMoney, AuthCode, AuthName
               , MemCard, MemberName, MemberBalance, MemberLeve, MemberDiscount, logentity);
            //
            if (PKCode.Length > 4)
            {
                switch (PayMethodCode)
                {
                    case "1"://支付宝支付
                        string AliUrl = Helper.GetAppSettings("AliPayUrl") + "/NetworkPay/WsAliPay.ashx";
                        StringBuilder aliPostStr = new StringBuilder();
                        // "GUID", "USER_ID", "buscode", "stocode", "orderno", "money", "devicecode", "tablecode", "paycode", "remark"
                        aliPostStr.Append("actionname=barcodepay&parameters={" +
                                                       string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'orderno': '{0}'", PKCode) + //支付单号
                                                        string.Format(",'money': '{0}'", PayMoney) + //订单金额
                                                        string.Format(",'devicecode': '{0}'", "1") + //设备号
                                                        string.Format(",'tablecode': '{0}'", "") + //桌台号
                                                        string.Format(",'paycode': '{0}'", Remar) + //付款码
                                                        string.Format(",'remark': '{0}'", Remar) + //备注
                                                "}");//键值对
                        string aliResultJson = Helper.HttpWebRequestByURL(AliUrl, aliPostStr.ToString());
                        string aliPayStatus = JsonHelper.GetJsonValByKey(aliResultJson, "status");
                        string aliPayMsg = JsonHelper.GetJsonValByKey(aliResultJson, "mes");
                        if (aliPayStatus == "0")
                        {
                            bll.UpdateStatus(GUID, USER_ID, StoCode, PKCode, "1", DiscountName, DiscountMoney, ZeroCutMoney, AuthCode, AuthName);
                            DataRow dr = dt.NewRow();
                            dr["type"] = "0";
                            dr["mes"] = "支付宝支付成功";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["type"] = "-1";
                            dr["mes"] = "请求联锁端支付宝支付失败";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        break;
                    case "2"://微信支付 
                        string WxUrl =Helper.GetAppSettings("WxPayUrl") + "/NetworkPay/WsWxPay.ashx";
                        StringBuilder wxPostStr = new StringBuilder();
                        wxPostStr.Append("actionname=micropaypage&parameters={" +
                                                        string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'orderno': '{0}'", PKCode) + //支付单号
                                                        string.Format(",'money': '{0}'", PayMoney) + //订单金额
                                                        string.Format(",'devicecode': '{0}'", "1") + //设备号
                                                        string.Format(",'tablecode': '{0}'", "") + //桌台号
                                                        string.Format(",'paycode': '{0}'", Remar) + //付款码
                                                        string.Format(",'remark': '{0}'", Remar) + //付款码
                                                "}");//键值对
                        string wxResultJson = Helper.HttpWebRequestByURL(WxUrl, wxPostStr.ToString());
                        string wxPayStatus = JsonHelper.GetJsonValByKey(wxResultJson, "status");
                        string wxPayMsg = JsonHelper.GetJsonValByKey(wxResultJson, "mes");
                        if (wxPayStatus == "0")
                        {
                            bll.UpdateStatus(GUID, USER_ID, StoCode, PKCode, "1",DiscountName,DiscountMoney,ZeroCutMoney,AuthCode,AuthName);
                            DataRow dr = dt.NewRow();
                            dr["type"] = "0";
                            dr["mes"] = "微信支付成功";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["type"] = "-1";
                            dr["mes"] = "请求联锁端微信支付失败";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        break;
                    case "7"://会员卡支付
                       
                            //"GUID", "USER_ID" , "buscode"  , "stocode"  , "terminalType" , "ucode", "uname", "paycode" , "vicepaycode" , "couponcode", "cardcode","expend","isopen", "invamount","addint" , "deductionint" 
                            string MemUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
                            StringBuilder memPostStr = new StringBuilder();
                            memPostStr.Append("actionname=cardpaynew&parameters={" +
                                                            string.Format("'GUID':'{0}'", GUID) +
                                                            string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                            string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                            string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                            string.Format(",'terminalType': '{0}'", "PC") + //终端编号
                                                            string.Format(",'ucode': '{0}'", CCode) + //操作人编号
                                                            string.Format(",'uname': '{0}'", CCname) + //操作人姓名
                                                            string.Format(",'paycode': '{0}'", BillCode) + //主支付编号
                                                            string.Format(",'vicepaycode': '{0}'", PKCode) + //副支付编号
                                                            string.Format(",'couponcode': '{0}'", "") + //优惠券编号（多个以英文逗号分隔）
                                                            string.Format(",'cardcode': '{0}'", MemCard) + //卡号
                                                            string.Format(",'expend': '{0}'", PayMoney) + //消费金额
                                                            string.Format(",'isopen': '{0}'", "0") + //是否开票
                                                            string.Format(",'invamount': '{0}'", "0") + //开票金额
                                                            string.Format(",'addint': '{0}'", PointGive) + //可积积分
                                                            string.Format(",'deductionint': '{0}'", PointUse) + //抵扣积分 
                                                    "}");//键值对

                            if (!string.IsNullOrWhiteSpace(CardPayCode))//会员卡扫码支付
                            {
                                memPostStr = new StringBuilder();
                               //GUID,USER_ID,buscode,stocode,terminalType,ucode,uname,paycode,vicepaycode,couponcode,expend,isopen,invamount,addint,deductionint,cardpaycode,remark
                                memPostStr.Append("actionname=scanpaycode&parameters={" +
                                                                string.Format("'GUID':'{0}'", GUID) +
                                                                string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                                string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                                string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                                string.Format(",'terminalType': '{0}'", "PC") + //终端编号
                                                                string.Format(",'ucode': '{0}'", CCode) + //操作人编号
                                                                string.Format(",'uname': '{0}'", CCname) + //操作人姓名
                                                                string.Format(",'paycode': '{0}'", BillCode) + //主支付编号
                                                                string.Format(",'vicepaycode': '{0}'", PKCode) + //副支付编号
                                                                string.Format(",'couponcode': '{0}'", "") + //优惠券编号（多个以英文逗号分隔）
                                                                string.Format(",'cardpaycode': '{0}'", CardPayCode) + //卡号
                                                                string.Format(",'expend': '{0}'", PayMoney) + //消费金额
                                                                string.Format(",'isopen': '{0}'", "0") + //是否开票
                                                                string.Format(",'invamount': '{0}'", "0") + //开票金额
                                                                string.Format(",'addint': '{0}'", PointGive) + //可积积分
                                                                string.Format(",'deductionint': '{0}'", PointUse) + //抵扣积分 
                                                                string.Format(",'remark': '{0}'", "小程序付款码支付") + //备注 
                                                        "}");//键值对
                            }

                            string memResultJson = Helper.HttpWebRequestByURL(MemUrl, memPostStr.ToString());
                            string memPayStatus = JsonHelper.GetJsonValByKey(memResultJson, "status");
                            string memPayMsg= JsonHelper.GetJsonValByKey(memResultJson, "mes");
                            if (CardType == "0")
                            {
                                memPayStatus = "0";
                            }
                            if (memPayStatus == "0")
                            {
                                bll.UpdateStatus(GUID, USER_ID, StoCode, PKCode, "1", DiscountName, DiscountMoney, ZeroCutMoney, AuthCode, AuthName);
                                DataRow dr = dt.NewRow();
                                dr["type"] = "0";
                                dr["mes"] = "会员卡支付成功";
                                dt.Rows.Clear();
                                dt.Rows.Add(dr);
                                dt.AcceptChanges();
                            }
                            else if(memPayStatus == "1" && !string.IsNullOrWhiteSpace(CardPayCode))
                            {
                                DataRow dr = dt.NewRow();
                                dr["type"] = "9";
                                dr["mes"] = "支付中";
                                dt.Rows.Clear();
                                dt.Rows.Add(dr);
                                dt.AcceptChanges();
                            }
                            else if (memPayStatus == "2"&&!string.IsNullOrWhiteSpace(CardPayCode))
                                {
                                bll.UpdateStatus(GUID, USER_ID, StoCode, PKCode, "1", DiscountName, DiscountMoney, ZeroCutMoney, AuthCode, AuthName);
                                DataRow dr = dt.NewRow();
                                    dr["type"] = "0";
                                    dr["mes"] = "会员卡支付成功";
                                    dt.Rows.Clear();
                                    dt.Rows.Add(dr);
                                    dt.AcceptChanges();
                                }
                        else
                            {
                                DataRow dr = dt.NewRow();
                                dr["type"] = "-1";
                                dr["mes"] = memPayMsg;
                                dt.Rows.Clear();
                                dt.Rows.Add(dr);
                                dt.AcceptChanges();
                            }
                        break;
                    case "4"://签单
                        //"GUID", "USER_ID" , "buscode"  , "stocode"  , "terminalType" , "ucode", "uname", "paycode" , "vicepaycode" , "couponcode", "cardcode","expend","isopen", "invamount","addint" , "deductionint" 
                        string SignUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
                        StringBuilder signPostStr = new StringBuilder();
                        signPostStr.Append("actionname=sigcreditpay&parameters={" +
                                                        string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'cardcode': '{0}'", MemCard) + //卡号
                                                        string.Format(",'expend': '{0}'", PayMoney) + //消费金额
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'terminalType': '{0}'", "CS") + //终端编号
                                                        string.Format(",'ucode': '{0}'", CCode) + //操作人编号
                                                        string.Format(",'uname': '{0}'", CCname) + //操作人姓名
                                                        string.Format(",'paycode': '{0}'", BillCode) + //主支付编号
                                                "}");//键值对

                      string signResultJson = Helper.HttpWebRequestByURL(SignUrl, signPostStr.ToString());
                        string signPayStatus = JsonHelper.GetJsonValByKey(signResultJson, "status");
                        string signPayMsg= JsonHelper.GetJsonValByKey(signResultJson, "mes");
                        if (signPayStatus == "0")
                        {
                            bll.UpdateStatus(GUID, USER_ID, StoCode, PKCode, "1", DiscountName, DiscountMoney, ZeroCutMoney, AuthCode, AuthName);
                            DataRow dr = dt.NewRow();
                            dr["type"] = "0";
                            dr["mes"] = "签单成功";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["type"] = "-1";
                            dr["mes"] = signPayMsg;
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        break;
                    case "5"://挂账
                        //"GUID", "USER_ID" , "buscode"  , "stocode"  , "terminalType" , "ucode", "uname", "paycode" , "vicepaycode" , "couponcode", "cardcode","expend","isopen", "invamount","addint" , "deductionint" 
                        string CreditUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
                        StringBuilder creditPostStr = new StringBuilder();
                        creditPostStr.Append("actionname=creditlinepay&parameters={" +
                                                        string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'cardcode': '{0}'", MemCard) + //卡号
                                                        string.Format(",'expend': '{0}'", PayMoney) + //消费金额
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'terminalType': '{0}'", "CS") + //终端编号
                                                        string.Format(",'ucode': '{0}'", CCode) + //操作人编号
                                                        string.Format(",'uname': '{0}'", CCname) + //操作人姓名
                                                        string.Format(",'paycode': '{0}'", BillCode) + //主支付编号
                                                "}");//键值对


                       string creditResultJson = Helper.HttpWebRequestByURL(CreditUrl, creditPostStr.ToString());
                        string creditPayStatus = JsonHelper.GetJsonValByKey(creditResultJson, "status");
                        string creditPayMsg= JsonHelper.GetJsonValByKey(creditResultJson, "mes");
                        if (creditPayStatus == "0")
                        {
                            bll.UpdateStatus(GUID, USER_ID, StoCode, PKCode, "1", DiscountName, DiscountMoney, ZeroCutMoney, AuthCode, AuthName);
                            DataRow dr = dt.NewRow();
                            dr["type"] = "0";
                            dr["mes"] = "挂账成功";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["type"] = "-1";
                            dr["mes"] = creditPayMsg;
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        break;
                }
            }


            ReturnListJson(dt);
        }

        private void Back(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BusCode", "StoCode", "CCode", "CCname", "TStatus",  "PayMoney","PPKCode","Remar"};
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
            string CCode = dicPar["CCode"].ToString();
            string CCname = dicPar["CCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string PKCode = "0";
            string PayMoney = dicPar["PayMoney"].ToString();
            string Remar = dicPar["Remar"].ToString();
            string PPKCode = dicPar["PPKCode"].ToString();
            string OutOrderCode = "";
            string PayMethodCode = "";
            string BillCode = dicPar["BillCode"].ToString();

            //会员卡

            string MemCard = Remar;


            //调用逻辑
            logentity.pageurl = "TB_BillPayEdit.html";
            logentity.logcontent = "新增账单支付信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.Back(GUID, USER_ID, BusCode, StoCode, CCode, CCname, TStatus, out PKCode, PayMoney, Remar, out OutOrderCode, PPKCode, logentity,out PayMethodCode);

            if (PKCode.Length > 4)
            {
                switch (PayMethodCode)
                {
                    case "1"://支付宝支付
                        string AliUrl = Helper.GetAppSettings("AliPayUrl") + "/NetworkPay/WsAliPay.ashx";
                        StringBuilder aliPostStr = new StringBuilder();
                        // "GUID", "USER_ID", "buscode", "stocode", "orderno", "money", "devicecode", "tablecode", "paycode", "remark"
                        aliPostStr.Append("actionname=refund&parameters={" +
                                                       string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'orderno': '{0}'", PPKCode) + //支付单号
                                                        string.Format(",'money': '{0}'", PayMoney) + //订单金额
                                                        string.Format(",'returnno': '{0}'", PKCode) + //桌台号
                                                        string.Format(",'reason': '{0}'", Remar) + //付款码
                                                        string.Format(",'remark': '{0}'", Remar) + //付款码
                                                "}");//键值对
                        string aliResultJson = Helper.HttpWebRequestByURL(AliUrl, aliPostStr.ToString());
                        string aliPayStatus = JsonHelper.GetJsonValByKey(aliResultJson, "status");
                        if (aliPayStatus == "0")
                        {
                            DataRow dr = dt.NewRow();
                            dr["type"] = "0";
                            dr["mes"] = "支付宝支付退款成功";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        else
                        {
                            bll.Delete(GUID, USER_ID, PKCode, StoCode, entity);
                            DataRow dr = dt.NewRow();
                            dr["type"] = "-1";
                            dr["mes"] = "支付宝退款失败";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        break;
                    case "2"://微信支付 "GUID", "USER_ID", "buscode", "stocode", "orderno", "transactionid", "money", "returnmoney", "remark"
                        string Url = Helper.GetAppSettings("WxPayUrl") + "/NetworkPay/WsWxPay.ashx";
                        StringBuilder postStr = new StringBuilder();
                        postStr.Append("actionname=refundpage&parameters={" +
                                                        string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'orderno': '{0}'", PPKCode) + //支付单号
                                                        string.Format(",'transactionid': ''") + //微信订单号
                                                        string.Format(",'money': '{0}'", PayMoney) + //订单总金额
                                                        string.Format(",'returnmoney': '{0}'", PayMoney) + //退款金额
                                                        string.Format(",'remark': '{0}'","") + //
                                                "}");//键值对
                        string strResultJson = Helper.HttpWebRequestByURL(Url, postStr.ToString());
                        string wxPayStatus  = JsonHelper.GetJsonValByKey(strResultJson, "status");
                        if (wxPayStatus== "0")
                        {
                            DataRow dr = dt.NewRow();
                            dr["type"] = "0";
                            dr["mes"] = "请求微信退款成功";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        else
                        {
                            bll.Delete(GUID, USER_ID, PKCode, StoCode, entity);
                            DataRow dr = dt.NewRow();
                            dr["type"] = "-1";
                            dr["mes"] = "请求微信退款失败";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        break;
                    case "7"://会员卡支付
                        string MemUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardreturn.ashx";
                        StringBuilder memPostStr = new StringBuilder();
                        memPostStr.Append("actionname=cardpayreturnnew&parameters={" +
                                                        string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'terminalType': '{0}'", "CS") + //终端编号
                                                        string.Format(",'ucode': '{0}'", CCode) + //操作人编号
                                                        string.Format(",'uname': '{0}'", CCname) + //操作人姓名
                                                        string.Format(",'paycode': '{0}'", BillCode) + //主支付编号
                                                        string.Format(",'vicepaycode': '{0}'", PKCode) + //副支付编号
                                                        string.Format(",'rvicepaycode': '{0}'", PPKCode) + //待反结副支付编号
                                                        string.Format(",'couponcode': '{0}'", "") + //优惠券编号（多个以英文逗号分隔）
                                                        string.Format(",'cardcode': '{0}'", MemCard) + //卡号
                                                        string.Format(",'expend': '{0}'", PayMoney) + //消费金额
                                                        string.Format(",'isopen': '{0}'", "0") + //是否开票
                                                        string.Format(",'invamount': '{0}'", "0") + //开票金额
                                                        string.Format(",'addint': '{0}'", "0") + //可积积分
                                                        string.Format(",'deductionint': '{0}'", "0") + //抵扣积分 
                                                "}");//键值对
                        string memResultJson = Helper.HttpWebRequestByURL(MemUrl, memPostStr.ToString());
                        string memPayStatus = JsonHelper.GetJsonValByKey(memResultJson, "status");
                        //获取支付的卡类型
                        TB_BillPayEntity payEntity=new bllTB_BillPay().GetEntitySigInfo("PKCode='"+ PPKCode + "'");
                        if (payEntity.Remar == "nfk")
                        {
                            memPayStatus = "0";
                        }
                        if (memPayStatus == "0")
                        {
                            DataRow dr = dt.NewRow();
                            dr["type"] = "0";
                            dr["mes"] = "会员卡反结成功";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        else
                        {
                            bll.Delete(GUID, USER_ID, PKCode, StoCode, entity);
                            DataRow dr = dt.NewRow();
                            dr["type"] = "-1";
                            dr["mes"] = "会员卡反结失败";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        break;
                    case "4"://签单
                        //"GUID", "USER_ID", "cardcode", "expend", "buscode", "stocode", "ucode", "uname", "paycode" 令牌，操作人id,卡号，签单金额，商户号，门店编号，操作人编号，操作人姓名
                        string SignUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardreturn.ashx";
                        StringBuilder signPostStr = new StringBuilder();
                        signPostStr.Append("actionname=sigcreditpayreturn&parameters={" +
                                                        string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'cardcode': '{0}'", MemCard) + //卡号
                                                        string.Format(",'expend': '{0}'", PayMoney) + //签单金额
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'ucode': '{0}'", CCode) + //操作人编号
                                                        string.Format(",'uname': '{0}'", CCname) + //操作人姓名
                                                        string.Format(",'paycode': '{0}'", BillCode) + //主支付编号
                                                "}");//键值对

                        string signResultJson = Helper.HttpWebRequestByURL(SignUrl, signPostStr.ToString());
                        string signPayStatus = JsonHelper.GetJsonValByKey(signResultJson, "status");
                        if (signPayStatus == "0")
                        {
                            DataRow dr = dt.NewRow();
                            dr["type"] = "0";
                            dr["mes"] = "签单反结成功";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        else
                        {
                            bll.Delete(GUID, USER_ID, PKCode, StoCode, entity);
                            DataRow dr = dt.NewRow();
                            dr["type"] = "-1";
                            dr["mes"] = "签单反结失败";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        break;
                    case "5"://挂账
                        //"GUID", "USER_ID", "cardcode", "expend", "buscode", "stocode", "ucode", "uname", "paycode"  令牌，操作人id,卡号，签单金额，商户号，门店编号，操作人编号，操作人姓名
                        string CreditUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardreturn.ashx";
                        StringBuilder creditPostStr = new StringBuilder();
                        creditPostStr.Append("actionname=creditlinepayreturn&parameters={" +
                                                        string.Format("'GUID':'{0}'", GUID) +
                                                        string.Format(",'USER_ID': '{0}'", USER_ID) +
                                                        string.Format(",'cardcode': '{0}'", MemCard) + //卡号
                                                        string.Format(",'expend': '{0}'", PayMoney) + //消费金额
                                                        string.Format(",'buscode': '{0}'", BusCode) + //商户编号
                                                        string.Format(",'stocode': '{0}'", StoCode) + //门店编号
                                                        string.Format(",'terminalType': '{0}'", "CS") + //终端编号
                                                        string.Format(",'ucode': '{0}'", CCode) + //操作人编号
                                                        string.Format(",'uname': '{0}'", CCname) + //操作人姓名
                                                        string.Format(",'paycode': '{0}'", BillCode) + //主支付编号
                                                "}");//键值对


                        string creditResultJson = Helper.HttpWebRequestByURL(CreditUrl, creditPostStr.ToString());
                        string creditPayStatus = JsonHelper.GetJsonValByKey(creditResultJson, "status");
                        if (creditPayStatus == "0")
                        {
                            DataRow dr = dt.NewRow();
                            dr["type"] = "0";
                            dr["mes"] = "挂账反结成功";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        else
                        {
                            bll.Delete(GUID, USER_ID, PKCode, StoCode, entity);
                            DataRow dr = dt.NewRow();
                            dr["type"] = "-1";
                            dr["mes"] = "挂账反结失败";
                            dt.Rows.Clear();
                            dt.Rows.Add(dr);
                            dt.AcceptChanges();
                        }
                        break;
                }
            }


            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCode", "CCname", "TStatus", "PKCode", "BillCode", "PayMoney", "PayMethodName", "PayMethodCode", "Remar", "OutOrderCode", "PPKCode", };
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
			string BillCode = dicPar["BillCode"].ToString();
			string PayMoney = dicPar["PayMoney"].ToString();
			string PayMethodName = dicPar["PayMethodName"].ToString();
			string PayMethodCode = dicPar["PayMethodCode"].ToString();
			string Remar = dicPar["Remar"].ToString();
			string OutOrderCode = dicPar["OutOrderCode"].ToString();
			string PPKCode = dicPar["PPKCode"].ToString();

            //调用逻辑
			logentity.pageurl ="TB_BillPayEdit.html";
			logentity.logcontent = "修改id为:"+Id+"的账单支付信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID,  Id, BusCode, StoCode, CCode, CCname, TStatus, PKCode, BillCode, PayMoney, PayMethodName, PayMethodCode, Remar, OutOrderCode, PPKCode, entity);
            
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
            List<string> pra = new List<string>() { "GUID", "USER_ID", "PKCode","StoCode"};
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
			logentity.pageurl ="TB_BillPayList.html";
			logentity.logcontent = "删除id为:"+Id+"的账单支付信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.Delete(GUID, USER_ID, Id,"", entity);
            ReturnListJson(dt);
        }

		/// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "StoCode", "PKCode", "status" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string StoCode= dicPar["StoCode"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string status = dicPar["status"].ToString();

            string Id = dicPar["ids"].ToString().Trim(',');
            logentity.pageurl ="TB_BillPayList.html";
			logentity.logcontent = "修改状态id为:"+Id+"的账单支付信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, StoCode, PKCode, status,"","","","","");
            ReturnListJson(dt);
        }

        /// <summary>
        /// 检查会员支付结果
        /// </summary>
        /// <param name="dicPar"></param>
        private void CheckMemberPayResult(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "CardPayCode","StoCode"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string cardpaycode = dicPar["CardPayCode"].ToString();
            string stocode = dicPar["StoCode"].ToString();

            string MemUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
            StringBuilder memPostStr = new StringBuilder();
            //{ "GUID":"","USER_ID":"","cardpaycode":"1911202057601"}
            memPostStr.Append("actionname=querypaycode&parameters={" +
                                            string.Format("'GUID':'{0}'", GUID) +
                                            string.Format(",'USER_ID': '{0}'", USER_ID) +
                                            string.Format(",'cardpaycode': '{0}'", cardpaycode) + //商户编号
                                    "}");//键值对
            string memResultJson = Helper.HttpWebRequestByURL(MemUrl, memPostStr.ToString());
            string payStatus = JsonHelper.GetJsonValByKey(memResultJson, "status");
            if (payStatus == "2")
            {
                //更新支付信息
                bll.UpdateStatusByOutOrderCode(GUID, USER_ID, stocode, cardpaycode, "1");
            }
            ToJsonStr(memResultJson);
        }

        /// <summary>
        /// 检查会员支付结果
        /// </summary>
        /// <param name="dicPar"></param>
        private void CancelMemberScanPay(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "CardPayCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode= dicPar["stocode"].ToString();
            string cardpaycode = dicPar["cardpaycode"].ToString();

            string MemUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
            StringBuilder memPostStr = new StringBuilder();
            //{ "GUID":"","USER_ID":"","cardpaycode":"1911202057601"}
            memPostStr.Append("actionname=cancelpaycode&parameters={" +
                                            string.Format("'GUID':'{0}'", GUID) +
                                            string.Format(",'USER_ID': '{0}'", USER_ID) +
                                            string.Format(",'buscode': '{0}'", cardpaycode) + //商户编号
                                    "}");//键值对
            string memResultJson = Helper.HttpWebRequestByURL(MemUrl, memPostStr.ToString());
            string status = JsonHelper.GetJsonValByKey(memResultJson, "status");
            if (status == "0")
            {
                ToCustomerJson("0", "取消成功");
                //bll.UpdateStatusByOutOrderCode(GUID, USER_ID, stocode, cardpaycode, "0");
            }
            else
            {
                ToCustomerJson("1", "无法取消");
            }
        }

    }
}