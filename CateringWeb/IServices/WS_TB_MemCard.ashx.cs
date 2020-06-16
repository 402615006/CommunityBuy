using CommunityBuy.BLL;
using CommunityBuy.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CommunityBuy.CommonBasic;
using System.Collections;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_TB_Admins 的摘要说明
    /// </summary>
    public class WS_TB_MemCard : ServiceBase
    {

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
                    logentity.module = "用户信息表";
                    switch (actionname.ToLower())
                    {
                        case "getinfo"://列表
                            GetInfo(dicPar);
                            break;
                        case "pay"://结账
                            //Login(dicPar);
                            break;
                        case "back"://反结
                            //LoginByPassword(dicPar);
                            break;
                        case "add"://开卡
                            AddMemberCard(dicPar);
                            break;
                        case "recharge"://充值
                            RechargeMemCard(dicPar);
                            break;
                        case "repaire"://补卡
                            RepaireMemCard(dicPar);
                            break;
                        case "change"://换卡
                            ChangeMemCard(dicPar);
                            break;
                        case "退卡"://换卡
                            BackMemCard(dicPar);
                            break;
                        case "checkmembymob"://换卡
                            CheckIsWxMem(dicPar);
                            break;
                    }
                }
            }
        }



        /// <summary>
        /// 获取连锁端用户信息
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetInfo(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID","USER_ID", "stocode", "cardcode", "cardtype","paycode"};

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string cardcode = dicPar["cardcode"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            string paycode = dicPar["paycode"].ToString();
            //"GUID", "USER_ID", "stocode", "cardcode", "cardtype"
            string ShortMesUrl = "";
            if (!string.IsNullOrWhiteSpace(paycode))
            {
                ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
                postStr.Append("actionname=getmemcardinfobypaycode&parameters={" +
                                               string.Format("\"GUID\":\"{0}\"", GUID) +
                                               string.Format(",\"USER_ID\": \"{0}\"", USER_ID) +
                                               string.Format(",\"stocode\": \"{0}\"", stocode) +
                                               string.Format(",\"paycode\": \"{0}\"", paycode) +
                                       "}");//键值对
            }
            else
            {
                ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
                postStr.Append("actionname=getmemcardinfobycardcode&parameters={" +
                                               string.Format("\"GUID\":\"{0}\"", GUID) +
                                               string.Format(",\"USER_ID\": \"{0}\"", USER_ID) +
                                               string.Format(",\"stocode\": \"{0}\"", stocode) +
                                               string.Format(",\"cardcode\": \"{0}\"", cardcode) +
                                               string.Format(",\"cardtype\": \"{0}\"", cardtype) +
                                               string.Format(",\"way\": \"{0}\"", "PC") +
                                       "}");//键值对
            }
            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString()); 
            if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
            {
                string msg = "";
                string status = JsonHelper.GetJsonValByKey(strAdminJson, "status");
                msg = JsonHelper.GetJsonValByKey(strAdminJson, "mes");
                if (status == "0")
                {
                    ToJsonStr(strAdminJson);
                }
                else
                {
                    ToCustomerJson("2", msg);
                    return;
                }
                    
            }
            else
            {
                ToCustomerJson("2", "读卡失败");
                return;
            }
        }

        /// <summary>
        /// 获取折扣金额
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetDiscountMoney(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "billcode", "levelcode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string billcode = dicPar["billcode"].ToString();
            string levelcode = dicPar["levelcode"].ToString();

            //调用逻辑
            string sql =string.Format("select [dbo].[p_getMemcardDiscount]('{0}','{1}','{2}')",stocode,billcode,levelcode);
            string dmoney = new bllPaging().ExecuteScalarBySQL(sql);

            ToCustomerJson("0", dmoney);
        
        }

        /// <summary>
        /// 会员卡开卡
        /// </summary>
        /// <param name="dicPar"></param>
        private void AddMemberCard(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode","stocode", "ordercode", "invoice","mac","payordercode" };

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
            string ordercode = dicPar["ordercode"].ToString();
            string mac = dicPar["mac"].ToString();
            decimal invoice = Helper.StringToDecimal(dicPar["invoice"].ToString());
            string msg = "";

            MemCard memCard = new MemCard(GUID, USER_ID, buscode, stocode, mac);
            DataTable dt= memCard.SYOpenCard("", ordercode, invoice, ref msg);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 会员卡充值
        /// </summary>
        /// <param name="dicPar"></param>
        private void RechargeMemCard(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "ordercode", "invoice", "mac" };

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
            string ordercode = dicPar["ordercode"].ToString();
            string mac = dicPar["mac"].ToString();
            decimal invoice = Helper.StringToDecimal(dicPar["invoice"].ToString());
            string msg = "";

            MemCard memCard = new MemCard(GUID, USER_ID, buscode, stocode, mac);
            DataTable dt = memCard.CardRecharge("", ordercode, invoice, ref msg);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 会员卡补卡
        /// </summary>
        /// <param name="dicPar"></param>
        private void RepaireMemCard(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "ordercode", "mac" };

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
            string ordercode = dicPar["ordercode"].ToString();
            string mac = dicPar["mac"].ToString();
            string msg = "";

            MemCard memCard = new MemCard(GUID, USER_ID, buscode, stocode, mac);
            DataTable dt = memCard.MemCardSupple("", ordercode, ref msg);
            ReturnListJson(dt);
        }


        /// <summary>
        /// 会员卡换卡
        /// </summary>
        /// <param name="dicPar"></param>
        private void ChangeMemCard(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "ordercode", "mac" };

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
            string ordercode = dicPar["ordercode"].ToString();
            string mac = dicPar["mac"].ToString();
            string msg = "";

            MemCard memCard = new MemCard(GUID, USER_ID, buscode, stocode, mac);
            DataTable dt = memCard.MemCardReplace("", ordercode, ref msg);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 会员卡退看
        /// </summary>
        /// <param name="dicPar"></param>
        private void BackMemCard(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "ordercode", "mac" };

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
            string ordercode = dicPar["ordercode"].ToString();
            string mac = dicPar["mac"].ToString();
            string msg = "";

            MemCard memCard = new MemCard(GUID, USER_ID, buscode, stocode, mac);
            DataTable dt = memCard.MemCardReplace("", ordercode, ref msg);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 判断用户是否已经关注小程序
        /// </summary>
        /// <param name="dicPar"></param>
        private void CheckIsWxMem(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "mob" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string mob = dicPar["mob"].ToString();


            //"GUID", "USER_ID", "memcode"
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/memcardpay/memcardpay.ashx";
            postStr.Append("actionname=checkinfobymob&parameters={" +
                                           string.Format("\"GUID\":\"{0}\"", GUID) +
                                           string.Format(",\"USER_ID\": \"{0}\"", USER_ID) +
                                           string.Format(",\"mob\": \"{0}\"", mob) +
                                   "}");//键值对

            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            ToJsonStr(strAdminJson);
        }
    }
}