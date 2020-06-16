using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.IServices;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using System.Text;
using System.Collections;
namespace CommunityBuy.WServices
{
	/// <summary>
    /// 账单优惠券接口类
    /// </summary>
    public class WSTB_BillCoupon : ServiceBase
    {
        bllTB_BillCoupon bll = new bllTB_BillCoupon();
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
					logentity.module = "账单优惠券";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "add"://使用优惠券						
                            Add(dicPar);
                            break;
                        case "detail"://详细
                            Detail(dicPar);
                            break;
                        case "cancel"://取消使用优惠券
                            Cancel(dicPar);
                            break;
                         case "getcoupondetail"://获取优惠券详细
                            GetCouponDetail(dicPar);
                            break;
                        case "getcouponbymem"://获取会员卡优惠券
                            GetCouponByMemCard(dicPar);
                            break;
                        case "getgivecoupons"://获取赠送方案
                            GetGiveCouponSch(dicPar);
                            break;
                        case "givecoupon"://赠送优惠券
                            GiveCoupon(dicPar);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 使用优惠券
        /// </summary>
        /// <param name="dicPar"></param>
        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "BusCode", "StoCode", "CCode", "CCname", "BillCode", "CouponCode", "CouponMoney", "MemberCardCode", "RealPay", "VIMoney", "Remark", "UseType", "ShiftCode", "CouponName", "OrderDishId", "DiscountPrice", "McCode","TicType","TicWay" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
			string Id = string.Empty;
			string BusCode = dicPar["BusCode"].ToString();
			string StoCode = dicPar["StoCode"].ToString();
			string CCode = dicPar["CCode"].ToString();
			string CCname = dicPar["CCname"].ToString();
			string BillCode = dicPar["BillCode"].ToString();
			string CouponCode = dicPar["CouponCode"].ToString();
			string CouponMoney = dicPar["CouponMoney"].ToString();
			string MemberCardCode = dicPar["MemberCardCode"].ToString();
			string RealPay = dicPar["RealPay"].ToString();
			string VIMoney = dicPar["VIMoney"].ToString();
			string Remark = dicPar["Remark"].ToString();
			string UseType = dicPar["UseType"].ToString();
            string ShiftCode = dicPar["ShiftCode"].ToString();
            string CouponName = dicPar["CouponName"].ToString();
            string OrderDishId = dicPar["OrderDishId"].ToString();
            string McCode = dicPar["McCode"].ToString();
            decimal DiscountPrice = Helper.StringToDecimal(dicPar["DiscountPrice"].ToString());
            string tictype= dicPar["TicType"].ToString();
            string ticway = dicPar["TicWay"].ToString();
            //调用逻辑
            logentity.pageurl ="TB_BillCouponEdit.html";
			logentity.logcontent = "新增账单优惠券信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Add;

            if (ticway != "1")//如果是卖品券，不用再调用网络验券接口
            {
                //调用验券接口
                string MemcardUrl = Helper.GetAppSettings("ServiceUrl") + "/coupon/WScheckcoupon.ashx";
                StringBuilder postStr = new StringBuilder();
                postStr.Append("actionname=couponrecoverynew&usercode=" + CCode + "&parameters={\"GUID\":\"" + GUID + "\",\"USER_ID\":\"" + GUID + "\",\"buscode\":\"" + BusCode + "\",\"couponcode\":\"" + CouponCode + "\",\"stocode\":\"" + StoCode + "\",\"way\":\"PC\",\"username\":\"" + CCname + "\",\"usercode\":\"" + CCode + "\",\"orderno\":\"" + BillCode + "\"}");
                string strAdminJson = Helper.HttpWebRequestByURL(MemcardUrl, postStr.ToString());
                if (!string.IsNullOrEmpty(strAdminJson))
                {
                    string msg = string.Empty;
                    string status = string.Empty;
                    DataSet ds = JsonHelper.JsonToDataSet(strAdminJson, out status, out msg);
                    if (status == "0")
                    {
                        //添加优惠券信息
                        dt = bll.Add(GUID, USER_ID, out Id, BusCode, StoCode, CCode, CCname, "0", BillCode, CouponCode, CouponMoney, MemberCardCode, RealPay, VIMoney, Remark, UseType, ShiftCode, CouponName, McCode, tictype, ticway, entity);
                        ReturnListJson(dt);
                    }
                    else
                    {
                        ToCustomerJson(status, msg);
                    }
                }
                else
                {
                    ToCustomerJson("2", "获取优惠券数据网络异常，请检查！");
                    return;
                }
            }
            else
            {
                //保存卖品券使用记录
                dt = bll.Add(GUID, USER_ID, out Id, BusCode, StoCode, CCode, CCname, "0", BillCode, CouponCode, CouponMoney, MemberCardCode, RealPay, VIMoney, Remark, UseType, ShiftCode, CouponName, McCode, tictype, ticway, entity);
                ReturnListJson(dt);
            }
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCode", "CCname", "TStatus", "BillCode", "CouponCode", "CouponMoney", "MemberCardCode", "RealPay", "VIMoney", "Remark", "UseType", "ShiftCode", };
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
			string BillCode = dicPar["BillCode"].ToString();
			string CouponCode = dicPar["CouponCode"].ToString();
			string CouponMoney = dicPar["CouponMoney"].ToString();
			string MemberCardCode = dicPar["MemberCardCode"].ToString();
			string RealPay = dicPar["RealPay"].ToString();
			string VIMoney = dicPar["VIMoney"].ToString();
			string Remark = dicPar["Remark"].ToString();
			string UseType = dicPar["UseType"].ToString();
            string ShiftCode = dicPar["ShiftCode"].ToString();
            string CouponName = dicPar["CouponName"].ToString();
            string McCode = dicPar["McCode"].ToString();

            //调用逻辑
            logentity.pageurl ="TB_BillCouponEdit.html";
			logentity.logcontent = "修改id为:"+Id+"的账单优惠券信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID, Id, BusCode, StoCode, CCode, CCname, TStatus, BillCode, CouponCode, CouponMoney, MemberCardCode, RealPay, VIMoney, Remark, UseType, ShiftCode, CouponName, McCode, "","",entity);
            
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

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="dicPar"></param>
        private void Cancel(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID","BusCode","StoCode","CCode","CCName", "couponcode","way","BillCode"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string couponcode = dicPar["couponcode"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string CCName= dicPar["CCName"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string Way = dicPar["way"].ToString();
            string BillCode= dicPar["BillCode"].ToString();

            //本地获取优惠券信息，查看是否是卖品券
            TB_BillCouponEntity coupon=bll.GetEntitySigInfo(string.Format("CouponCode='{0}' and billcode='{1}' and Stocode='{2}'", couponcode, BillCode, StoCode));
            if (coupon.TicWay != "1")
            {
                //联锁端取消
                string MemcardUrl = Helper.GetAppSettings("ServiceUrl") + "/coupon/WScheckcoupon.ashx";
                StringBuilder postStr = new StringBuilder();
                postStr.Append("actionname=couponrecoverycancelnew&usercode=" + CCode + "&parameters=" +
                    "{\"GUID\":\"" + GUID + "\"," +
                    "\"USER_ID\":\"" + GUID + "\"," +
                    "\"buscode\":\"" + BusCode + "\"," +
                    "\"couponcode\":\"" + couponcode + "\"," +
                    "\"stocode\":\"" + StoCode + "\"," +
                    "\"username\":\"" + CCName + "\"," +
                    "\"usercode\":\"" + CCode + "\"," +
                    "\"way\":\"" + Way + "\"}");
                string strAdminJson = Helper.HttpWebRequestByURL(MemcardUrl, postStr.ToString());
                if (!string.IsNullOrEmpty(strAdminJson))
                {
                    string status = "";
                    string mgs = "";
                    JsonHelper.JsonToMessage(strAdminJson, out status, out mgs);
                    if (status == "0")
                    {
                        //本地删除
                        DataSet ds = bll.Cancel(GUID, USER_ID, StoCode, BillCode, couponcode, "1");
                        ArrayList arrayTables = new ArrayList();
                        arrayTables.Add(ds.Tables[0]);
                        arrayTables.Add(ds.Tables[1]);
                        string[] names = { "BillCoupon", "OrderDish" };
                        ReturnListJson("0", "取消成功", arrayTables, names);
                    }
                }
            }
            else {
                //本地删除
                DataSet ds = bll.Cancel(GUID, USER_ID, StoCode, BillCode, couponcode, "1");
                ArrayList arrayTables = new ArrayList();
                arrayTables.Add(ds.Tables[0]);
                arrayTables.Add(ds.Tables[1]);
                string[] names = { "BillCoupon", "OrderDish" };
                ReturnListJson("0", "取消成功", arrayTables, names);
            }
        }

        /// <summary>
        /// 获取账单优惠券信息
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetList(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "StoCode", "BillCode","page","pagesize"};
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
            int page=Helper.StringToInt(dicPar["page"].ToString());
            int pagesize = Helper.StringToInt(dicPar["pagesize"].ToString());
            int recunms = 0;
            int pagenums = 0;
            //调用逻辑			
            dt = bll.GetCouponInfoByBillCode(GUID, USER_ID, StoCode, BillCode,page,pagesize,out recunms,out pagenums);
            ReturnListJson(dt, pagesize, recunms, page, pagenums);
        }

        /// <summary>
        /// 获取优惠券详细
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetCouponDetail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            //"GUID":"","USER_ID":"","buscode":"88888888","couponcode":"100K-0016-JG68-7P50","stocode":"15","way":"PC"
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "couponcode", "way","usercode"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string couponcode = dicPar["couponcode"].ToString();
            string way = dicPar["way"].ToString();
            string usercode = dicPar["usercode"].ToString();

            string MemcardUrl = Helper.GetAppSettings("ServiceUrl") + "/coupon/WScheckcoupon.ashx";
            StringBuilder postStr = new StringBuilder();
            postStr.Append("actionname=getcoupondetail&usercode="+ usercode + "&parameters={\"GUID\":\"" + GUID + "\",\"USER_ID\":\"" + GUID + "\",\"buscode\":\"" + buscode + "\",\"couponcode\":\"" + couponcode + "\",\"stocode\":\"" + stocode + "\",\"way\":\"" + way + "\"}");//键值对

            string strAdminJson = Helper.HttpWebRequestByURL(MemcardUrl, postStr.ToString());
            if (!string.IsNullOrEmpty(strAdminJson))
            {
                string msg = string.Empty;
                string status  = string.Empty;
                DataSet ds = JsonHelper.JsonToDataSet(strAdminJson, out status, out msg);
                if (status == "0")
                {
                    if (ds.Tables.Count >= 1)
                    {
                        DataTable dtCoupon = ds.Tables[0];
                        ReturnListJson(dtCoupon);
                    }
                    else
                    {
                        ToCustomerJson("2", "获取优惠券数据网络异常，请检查！");
                    }
                }
                else
                {
                    ToCustomerJson(status, msg);
                }
            }
            else
            {
                ToCustomerJson("2", "获取优惠券数据网络异常，请检查！");
                return;
            }
        }
        
        /// <summary>
        /// 根据会员卡获取优惠券
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetCouponByMemCard(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "memcode", "stocode", "way", "usercode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string stocode= dicPar["stocode"].ToString();
            string way = dicPar["way"].ToString();
            string usercode = dicPar["usercode"].ToString();

            //"GUID", "USER_ID", "memcode"
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/coupon/WScheckcoupon.ashx";
            postStr.Append("actionname=getvalidatecouponlist&usercode="+ usercode + "&parameters={" +
                                           string.Format("\"GUID\":\"{0}\"", GUID) +
                                           string.Format(",\"USER_ID\": \"{0}\"", USER_ID) +
                                           string.Format(",\"cardcode\": \"{0}\"", memcode) +
                                           string.Format(",\"stocode\": \"{0}\"", stocode) +
                                           string.Format(",\"way\": \"{0}\"", way) +
                                   "}");//键值对

            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
            {
                string msg = "";
                string status = JsonHelper.GetJsonValByKey(strAdminJson, "status");
                msg= JsonHelper.GetJsonValByKey(strAdminJson, "mes");
                if (status == "0")
                {
                    ToJsonStr(strAdminJson);
                }
                else
                {
                    ToCustomerJson("1", msg);
                    return;
                }

            }
            else
            {
                ToCustomerJson("2", "接口读取失败");
                return;
            }
        }

        /// <summary>
        /// 获取优惠券赠送方案
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetGiveCouponSch(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "cardcode", "phone", "money" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string cardcode = dicPar["cardcode"].ToString();
            string phone = dicPar["phone"].ToString();
            string money = dicPar["money"].ToString();

            //"GUID", "USER_ID", "memcode"
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
            postStr.Append("actionname=getmemcouponslist&parameters={" +
                                           string.Format("\"GUID\":\"{0}\"", GUID) +
                                           string.Format(",\"USER_ID\": \"{0}\"", USER_ID) +
                                           string.Format(",\"buscode\": \"{0}\"", buscode) +
                                           string.Format(",\"stocode\": \"{0}\"", stocode) +
                                           string.Format(",\"cardcode\": \"{0}\"", cardcode) +
                                           string.Format(",\"phone\": \"{0}\"", phone) +
                                           string.Format(",\"money\": \"{0}\"", money) +
                                   "}");//键值对

            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            ToJsonStr(strAdminJson);
        }

        /// <summary>
        /// 赠送优惠券
        /// </summary>
        /// <param name="dicPar"></param>
        private void GiveCoupon(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "cardcode", "phone", "coupons", "ucode", "uname", "remark", "pcode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string cardcode = dicPar["cardcode"].ToString();
            string phone = dicPar["phone"].ToString();
            string coupons = dicPar["coupons"].ToString();
            string ucode = dicPar["ucode"].ToString();
            string uname = dicPar["uname"].ToString();
            string remark = dicPar["remark"].ToString();
            string pcode = dicPar["pcode"].ToString();
            string billcode= dicPar["billcode"].ToString();

            coupons = "[" + coupons.Replace("\"","'")+ "]";

            //判断账单是否已赠送过
            DataTable dtBill = new bllTB_Bill().GetPagingSigInfo(GUID, USER_ID, " PKCode='"+ billcode + "' and StoCode='"+ stocode + "'");
            if (dtBill != null && dtBill.Rows.Count > 0)
            {
                string gavecoupon = dtBill.Rows[0]["GiveCoupons"].ToString();
                if (!string.IsNullOrWhiteSpace(gavecoupon))
                {
                    ToCustomerJson("1", "该账单已经赠送过了！");
                    return;
                }
            }

            //"GUID", "USER_ID", "memcode"
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
            postStr.Append("actionname=givecouponslist&parameters={" +
                                           string.Format("\"GUID\":\"{0}\"", GUID) +
                                           string.Format(",\"USER_ID\": \"{0}\"", USER_ID) +
                                           string.Format(",\"buscode\": \"{0}\"", buscode) +
                                           string.Format(",\"stocode\": \"{0}\"", stocode) +
                                           string.Format(",\"cardcode\": \"{0}\"", cardcode) +
                                           string.Format(",\"phone\": \"{0}\"", phone) +
                                           string.Format(",\"coupons\": \"{0}\"", coupons) +
                                           string.Format(",\"ucode\": \"{0}\"", ucode) +
                                           string.Format(",\"uname\": \"{0}\"", uname) +
                                           string.Format(",\"remark\": \"{0}\"", remark) +
                                           string.Format(",\"pcode\": \"{0}\"", pcode) +
                                   "}");//键值对

            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            string status = JsonHelper.GetJsonValByKey(strAdminJson, "status");
            if (status == "0")
            {
                //更新账单赠送信息
                new bllTB_Bill().UpdateGiveCoupons(GUID, USER_ID, billcode,stocode,coupons);
            }

            ToJsonStr(strAdminJson);
        }
    }
}