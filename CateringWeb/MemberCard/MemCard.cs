using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy
{
    public class MemCard
    {
        //参数
        public string GUID;
        public string USER_ID;
        public string BusCode;
        public string StoCode;
        public string Mac;

        public MemCard(string guid, string userid, string buscode, string stocode, string mac)
        {
            this.GUID = guid;
            this.USER_ID = userid;
            this.BusCode = buscode;
            this.StoCode = stocode;
            this.Mac = mac;
        }

        //接口默认根地址
        public static string ServiceUrl = Helper.GetAppSettings("ServiceUrl");
        /// <summary>
        /// 会员卡开卡
        /// </summary>
        /// <param name="OrderCode">订单编号</param>
        ///  <param name="invoice">发票金额</param>
        /// <returns></returns>
        public DataTable SYOpenCard(string PayOrderCode, string OrderCode, decimal invoice, ref string Mes)
        {
            DataSet ds = new DataSet();
            DataTable dto = new DataTable();
            ds = new bllopencardinfo().GetOpendCardInfo(OrderCode);
            if (ds != null)
            {
                string memcode = string.Empty;
                string cardcode = string.Empty;
                string regamount = string.Empty;
                string freeamount = string.Empty;
                string cardcost = string.Empty;
                string payamount = string.Empty;
                string validate = string.Empty;
                string password = string.Empty;
                string nowritedoc = string.Empty;
                string cname = string.Empty;
                string mobile = string.Empty;
                string sex = string.Empty;
                string idtype = string.Empty;
                string IDNO = string.Empty;
                string ucode = string.Empty;
                string uname = string.Empty;
                string isopen = "0";
                string isgive = "0";
                string isneedpwd = "0";
                string levelcode = "";
                if (invoice > 0)
                {
                    isopen = "1";
                }
                //终端编号/单号
                string terminalType = Mac;

                //赠送优惠券资料
                string opencardcoupon = string.Empty;

                DataTable dtcard = ds.Tables[0];
                DataTable dtcoupon = ds.Tables[1];
                if (dtcard != null && dtcard.Rows.Count > 0)
                {
                    DataRow dr = dtcard.Rows[0];
                    terminalType = PayOrderCode;
                    memcode = dr["memcode"].ToString();
                    cardcode = dr["cardcode"].ToString();
                    regamount = dr["regamount"].ToString();
                    freeamount = dr["freeamount"].ToString();
                    cardcost = dr["cardcost"].ToString();
                    payamount = dr["payamount"].ToString();
                    validate = dr["validate"].ToString();
                    password = dr["password"].ToString();
                    nowritedoc = dr["nowritedoc"].ToString();
                    cname = dr["cname"].ToString();
                    mobile = dr["mobile"].ToString();
                    idtype = dr["idtype"].ToString();
                    IDNO = dr["IDNO"].ToString();
                    ucode = dr["ucode"].ToString();
                    uname = dr["uname"].ToString();
                    sex = dr["sex"].ToString();
                    isneedpwd = dr["bak1"].ToString();
                    levelcode = dr["levelcode"].ToString();
                }

                if (dtcoupon != null && dtcoupon.Rows.Count > 0)
                {
                    isgive = "1";
                    for (int i = 0; i < dtcoupon.Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            opencardcoupon += ";";
                        }
                        opencardcoupon += dtcoupon.Rows[i]["pcode"] + "," + dtcoupon.Rows[i]["sumcode"] + "," + dtcoupon.Rows[i]["mccode"] + "," + dtcoupon.Rows[i]["num"];
                    }
                }

                //发卡操作
                //通过接口获取卡信息，以及优惠方案信息
                string InterfaceUrl = ServiceUrl + "/memberCard/WSMemCard.ashx";
                StringBuilder postStr = new StringBuilder();
                //{ "GUID", "USER_ID", "buscode", "stocode", "memcode", "cardcode", "regamount", "freeamount", "cardcost", "payamount", "validate", "password", "nowritedoc", "cname", "mobile", "idtype", "IDNO", "ucode", "uname", "isopen", "isgive", "terminalType", "opencardcoupon" };
                string getopenmemcardinfo = "actionname={0}&parameters={{\"GUID\":\"{1}\", \"USER_ID\":\"{2}\", \"buscode\":\"{3}\", \"stocode\":\"{4}\", \"memcode\":\"{5}\", \"cardcode\":\"{6}\", \"regamount\":\"{7}\", \"freeamount\":\"{8}\", \"cardcost\":\"{9}\", \"payamount\":\"{10}\", \"validate\":\"{11}\", \"password\":\"{12}\", \"nowritedoc\":\"{13}\", \"cname\":\"{14}\", \"mobile\":\"{15}\", \"idtype\":\"{16}\", \"IDNO\":\"{17}\", \"ucode\":\"{18}\", \"uname\":\"{19}\", \"isopen\":\"{20}\", \"isgive\":\"{21}\", \"terminalType\":\"{22}\", \"sex\":\"{23}\", \"opencardcoupon\":\"{24}\", \"isneedpwd\":\"{25}\", \"levelcode\":\"{26}\"}}";
                postStr.Append(string.Format(getopenmemcardinfo, "syopencard", GUID, USER_ID, BusCode, StoCode, memcode, cardcode, regamount, freeamount, cardcost, payamount, validate, password, nowritedoc, cname, mobile, idtype, IDNO, ucode, uname, isopen, isgive, terminalType, sex, opencardcoupon, isneedpwd, levelcode));
                string result = Helper.HttpWebRequestByURL(InterfaceUrl, postStr);
                if (string.IsNullOrEmpty(result) || result.Trim() == "")
                {
                    Mes = "调用会员卡接口失败，请检查网络！";
                    return null;
                }

                string mes = string.Empty;
                string status = string.Empty;

                ds = JsonHelper.JsonToDataSet(result, out status, out mes);
                if (status.Trim() != "0")
                {
                    if (string.IsNullOrEmpty(mes) || mes.Trim() == "")
                    {
                        mes = "调用会员卡接口失败，请检查网络！";
                    }
                    Mes = mes;
                    return null;
                }
                if (ds != null && ds.Tables["data"] != null && ds.Tables["data"].Rows.Count > 0)
                {
                    dto = ds.Tables["data"];
                    DataRow dr = dto.Rows[0];
                    string oldcardcodes = "";
                    string oldbalance = "0";
                    string balance = dto.Rows[0]["balance"].ToString();
                    string pledge = dto.Rows[0]["pledge"].ToString();
                    string newcardCode = dto.Rows[0]["cardCode"].ToString();
                    new bllopencardinfo().Update(OrderCode, oldcardcodes, oldbalance, balance, dto.Rows[0]["memcname"].ToString(), dto.Rows[0]["mob"].ToString(), dto.Rows[0]["pname"].ToString(), pledge, "", "", newcardCode);
                    dto = new bllopencardinfo().GetCardOrderInfo(OrderCode);
                }
            }
            return dto;
        }

        /// <summary>
        /// 会员卡充值
        /// </summary>
        /// <param name="OrderCode">订单编号</param>
        /// <param name="invoice">发票金额</param>
        /// <returns></returns>
        public DataTable CardRecharge(string PayOrderCode, string OrderCode, decimal invoice, ref string Mes)
        {
            DataSet ds = new DataSet();
            DataTable dto = new DataTable();
            ds = new bllopencardinfo().GetCardRechageInfo(OrderCode);
            if (ds != null)
            {
                string cardcode = string.Empty;
                string regamount = string.Empty;
                string freeamount = string.Empty;
                string payamount = string.Empty;
                string ucode = string.Empty;
                string uname = string.Empty;
                string isopen = "0";
                string isgive = "0";
                if (invoice > 0)
                {
                    isopen = "1";
                }
                //终端编号
                string terminalType = this.Mac;

                //赠送优惠券资料
                string opencardcoupon = string.Empty;

                DataTable dtcard = ds.Tables[0];
                DataTable dtcoupon = ds.Tables[1];
                if (dtcard != null && dtcard.Rows.Count > 0)
                {
                    DataRow dr = dtcard.Rows[0];
                    terminalType = PayOrderCode;
                    cardcode = dr["cardcode"].ToString();
                    regamount = dr["regamount"].ToString();
                    freeamount = dr["freeamount"].ToString();
                    payamount = dr["payamount"].ToString();
                    ucode = dr["ucode"].ToString();
                    uname = dr["uname"].ToString();
                }

                if (dtcoupon != null && dtcoupon.Rows.Count > 0)
                {
                    isgive = "1";
                    for (int i = 0; i < dtcoupon.Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            opencardcoupon += ";";
                        }
                        opencardcoupon += dtcoupon.Rows[i]["pcode"] + "," + dtcoupon.Rows[i]["sumcode"] + "," + dtcoupon.Rows[i]["mccode"] + "," + dtcoupon.Rows[i]["num"];
                    }
                }
                //发卡操作
                //通过接口获取卡信息，以及优惠方案信息
                string InterfaceUrl = ServiceUrl + "/memberCard/WSMemCard.ashx";
                StringBuilder postStr = new StringBuilder();
                string getopenmemcardinfo = "actionname={0}&parameters={{\"GUID\":\"{1}\", \"USER_ID\":\"{2}\", \"buscode\":\"{3}\", \"stocode\":\"{4}\", \"cardcode\":\"{5}\", \"regamount\":\"{6}\", \"freeamount\":\"{7}\", \"payamount\":\"{8}\", \"ucode\":\"{9}\", \"uname\":\"{10}\", \"isopen\":\"{11}\", \"isgive\":\"{12}\", \"terminalType\":\"{13}\", \"opencardcoupon\":\"{14}\"}}";
                postStr.Append(string.Format(getopenmemcardinfo, "membercardrecharge", this.GUID, this.USER_ID, this.BusCode, this.StoCode, cardcode, regamount, freeamount, payamount, ucode, uname, isopen, isgive, terminalType, opencardcoupon));
                string result = Helper.HttpWebRequestByURL(InterfaceUrl, postStr);
                if (string.IsNullOrEmpty(result) || result.Trim() == "")
                {
                    Mes = "调用会员卡接口失败，请检查网络！";
                    return null;
                }

                string mes = string.Empty;
                string status = string.Empty;

                ds = JsonHelper.JsonToDataSet(result, out status, out mes);
                if (status != "0")
                {
                    if (string.IsNullOrEmpty(mes) || mes.Trim() == "")
                    {
                        mes = "调用会员卡接口失败，请检查网络！";
                    }
                    Mes = mes;
                    return null;
                }
                if (ds != null && ds.Tables["data"] != null && ds.Tables["data"].Rows.Count > 0)
                {
                    dto = ds.Tables["data"];
                    DataRow dr = dto.Rows[0];
                    string oldcardcodes = "";
                    string oldbalance = dto.Rows[0]["oldbalance"].ToString();
                    string balance = dto.Rows[0]["balance"].ToString();
                    string pledge = dto.Rows[0]["pledge"].ToString();
                    new bllopencardinfo().Update(OrderCode, oldcardcodes, oldbalance, balance, dto.Rows[0]["memcname"].ToString(), dto.Rows[0]["mob"].ToString(), dto.Rows[0]["pname"].ToString(), pledge, "", "");
                    dto = new bllopencardinfo().GetCardOrderInfo(OrderCode);
                }
            }
            return dto;
        }

        /// <summary>
        /// 会员卡补卡
        /// </summary>
        /// <param name="OrderCode">订单编号</param>
        /// <returns></returns>
        public DataTable MemCardSupple(string PayOrderCode, string OrderCode, ref string Mes)
        {
            return MemCardSupple(PayOrderCode, OrderCode, SystemEnum.MemCardStatus.Change, ref Mes);
        }

        /// <summary>
        /// 会员卡换卡
        /// </summary>
        /// <param name="OrderCode">订单编号</param>
        /// <returns></returns>
        public DataTable MemCardReplace(string PayOrderCode, string OrderCode, ref string Mes)
        {
            return MemCardSupple(PayOrderCode, OrderCode, SystemEnum.MemCardStatus.Replace, ref Mes);
        }

        /// <summary>
        /// 会员卡补换卡
        /// </summary>
        /// <param name="OrderCode">订单编号</param>
        /// <returns></returns>
        private DataTable MemCardSupple(string PayOrderCode, string OrderCode, SystemEnum.MemCardStatus ctype, ref string Mes)
        {
            DataTable dt = new DataTable();
            dt = new bllopencardinfo().GetCardOrderInfo(OrderCode);
            if (dt != null)
            {
                string cardcode = string.Empty;
                string stocode = string.Empty;
                string payamount = string.Empty;
                string ucode = string.Empty;
                string uname = string.Empty;
                string oldcardcode = string.Empty;
                string validity = string.Empty;

                //终端编号
                string terminalType = this.Mac;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    terminalType = PayOrderCode;
                    cardcode = dr["cardcode"].ToString();
                    stocode = dr["stocode"].ToString();
                    payamount = dr["payamount"].ToString();
                    ucode = dr["ucode"].ToString();
                    uname = dr["uname"].ToString();
                    oldcardcode = dr["remark"].ToString();
                    validity = dr["memcode"].ToString();
                }

                //发卡操作
                //通过接口获取卡信息，以及优惠方案信息
                string InterfaceUrl = ServiceUrl + "/memberCard/WSMemCard.ashx";
                StringBuilder postStr = new StringBuilder();
                string actionname = "repaircard";
                if (ctype == SystemEnum.MemCardStatus.Replace)
                {
                    actionname = "replace";
                }
                string reqParameters = "actionname={0}&parameters={{\"GUID\":\"{1}\", \"USER_ID\":\"{2}\", \"stocode\":\"{3}\", \"ousercode\":\"{4}\", \"ousername\":\"{5}\", \"cardcode\":\"{6}\", \"oldcardcode\":\"{7}\", \"validity\":\"{8}\", \"paymoney\":\"{9}\", \"paycode\":\"{10}\"}}";
                string[] arrPar = new string[] { actionname, this.GUID, this.USER_ID, stocode, ucode, uname, cardcode, oldcardcode, validity, payamount, terminalType };
                postStr.Append(string.Format(reqParameters, arrPar));//键值对
                string result = Helper.HttpWebRequestByURL(InterfaceUrl, postStr);
                if (string.IsNullOrEmpty(result) || result.Trim() == "")
                {
                    Mes = "调用会员卡接口失败，请检查网络！";
                    return null;
                }

                string mes = string.Empty;
                string status = string.Empty;

                DataSet ds = JsonHelper.JsonToDataSet(result, out status, out mes);
                if (status != "0")
                {
                    if (string.IsNullOrEmpty(mes) || mes.Trim() == "")
                    {
                        mes = "调用会员卡接口失败，请检查网络！";
                    }
                    Mes = mes;
                    return null;
                }
                if (ds != null && ds.Tables["data"] != null && ds.Tables["data"].Rows.Count > 0)
                {
                    dt = ds.Tables["data"];
                    string oldcardcodes = dt.Rows[0]["oldcardcodes"].ToString();
                    string oldbalance = dt.Rows[0]["oldbalance"].ToString();
                    string balance = dt.Rows[0]["balance"].ToString();
                    new bllopencardinfo().Update(OrderCode, oldcardcodes, oldbalance, balance, "", "", "", "", "", "");
                }
                dt = new bllopencardinfo().GetCardOrderInfo(OrderCode);
            }
            return dt;
        }

    }
}
