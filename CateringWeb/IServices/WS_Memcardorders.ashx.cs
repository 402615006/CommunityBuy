using CommunityBuy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_Memcardorders 的摘要说明
    /// </summary>
    public class WS_Memcardorders : ServiceBase
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
                    logentity.module = "门店信息表";
                    switch (actionname.ToLower())
                    {
                        case "opencard":
                            OpenCard(dicPar);
                            break;
                        case "openmemcard":
                            OpenMemCard(dicPar);
                            break;
                        case "chargecard":
                            ChargeCard(dicPar);
                            break;
                        case "returncard":
                            ReturnCard(dicPar);
                            break;
                        case "mergecard":
                            MergeCard(dicPar);
                            break;
                        case "returnmemcard":
                            ReturnMemCard(dicPar);
                            break;
                        case "refundmemcard":
                            RefundMemCard(dicPar);
                            break;
                        case "getopendcardinfo":
                            GetOpendCardInfo(dicPar);
                            break;
                        case "updateopendcardinfo":
                            UpdateOpendCardInfo(dicPar);
                            break;
                        case "getcardorderinfo":
                            GetCardOrderInfo(dicPar);
                            break;
                        case "getcardrechageinfo":
                            GetCardRechageInfo(dicPar);
                            break;
                        case "updatecardorder":
                            UpdateCardOrder(dicPar);
                            break;
                        case "updatecardmerge":
                            UpdateMemcardMerg(dicPar);
                            break;
                        case "getlist":
                            GetBillList(dicPar);
                            break;
                        case "getreportlist"://获取储值报表
                            GetReportList(dicPar);
                            break;
                        case "getreportexport"://导出储值报表
                            GetReportExport(dicPar);
                            break;
                        case "getmemcardtypelist":
                            GetMemCardTypeList(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取卡类型
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetMemCardTypeList(Dictionary<string, object> dicPar)
        {
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            DataTable dt = new BLL.bllmemcardorders().GetMemCardTypeList(BusCode);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 实物卡开卡
        /// </summary>
        /// <param name="dicPar"></param>
        private void OpenCard(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "memcode", "cardtype", "cardcode", "otype", "regamount", "freeamount", "cardcost", "payamount", "remark", "status", "ucode", "uname", "paystatus", "pushemcode", "pushname", "shiftid", "pcname", "bak2", "bak3", "opencardinfo", "opencardcoupon" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            string CardCode = dicPar["cardcode"].ToString();
            string otype = dicPar["otype"].ToString();
            string regamount = dicPar["regamount"].ToString();
            string freeamount = dicPar["freeamount"].ToString();
            string cardcost = dicPar["cardcost"].ToString();
            string payamount = dicPar["payamount"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string ucode = dicPar["ucode"].ToString();
            string uname = dicPar["uname"].ToString();
            string ordercode = "";
            string paystatus = dicPar["paystatus"].ToString();
            string tcrCode = dicPar["pushemcode"].ToString();
            string tcrName = dicPar["pushname"].ToString();
            string shiftid = dicPar["shiftid"].ToString();
            string pcname = dicPar["pcname"].ToString();
            string bak2 = dicPar["bak2"].ToString();
            string bak3 = dicPar["bak3"].ToString();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string cardinfo = jss.Serialize(dicPar["opencardinfo"]);
            string coupons = jss.Serialize(dicPar["opencardcoupon"]);
            string ID = "";
            List<opencardcouponEntity> opencardcoupon = JsonHelper.AnalysisJson<List<opencardcouponEntity>>(coupons);
            opencardinfoEntity opencardinfo = JsonHelper.AnalysisJson<opencardinfoEntity>(cardinfo);

            DataTable dt = new BLL.bllmemcardorders().Add(GUID, USER_ID, out ID, buscode, stocode, memcode, cardtype, CardCode, otype, regamount, freeamount, cardcost, payamount, remark, status, ucode, uname, ref ordercode, paystatus, tcrCode, tcrName, shiftid, pcname, bak2, "", opencardinfo, opencardcoupon);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("ID");
                dt.Columns.Add("ordercode");
                dt.Rows[0]["ID"] = ID;
                dt.Rows[0]["ordercode"] = ordercode;
            }
            ReturnListJson(dt);
        }


        /// <summary>
        /// 会员卡开卡
        /// </summary>
        /// <param name="dicPar"></param>
        private void OpenMemCard(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "memcode", "cardtype", "levelcode", "otype", "regamount", "freeamount", "cardcost", "payamount", "remark", "status", "ucode", "uname", "paystatus", "pushemcode", "pushname", "shiftid", "pcname", "bak2", "bak3", "opencardinfo", "opencardcoupon" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            string levelcode = dicPar["levelcode"].ToString();
            string otype = dicPar["otype"].ToString();
            string regamount = dicPar["regamount"].ToString();
            string freeamount = dicPar["freeamount"].ToString();
            string cardcost = dicPar["cardcost"].ToString();
            string payamount = dicPar["payamount"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string ucode = dicPar["ucode"].ToString();
            string uname = dicPar["uname"].ToString();
            string ordercode = "";
            string paystatus = dicPar["paystatus"].ToString();
            string tcrCode = dicPar["pushemcode"].ToString();
            string tcrName = dicPar["pushname"].ToString();
            string shiftid = dicPar["shiftid"].ToString();
            string pcname = dicPar["pcname"].ToString();
            string bak2 = dicPar["bak2"].ToString();
            string bak3 = dicPar["bak3"].ToString();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string cardinfo = jss.Serialize(dicPar["opencardinfo"]);
            string coupons = jss.Serialize(dicPar["opencardcoupon"]);
            string ID = "";
            List<opencardcouponEntity> opencardcoupon = JsonHelper.AnalysisJson<List<opencardcouponEntity>>(coupons);
            opencardinfoEntity opencardinfo = JsonHelper.AnalysisJson<opencardinfoEntity>(cardinfo);

            DataTable dt = new BLL.bllmemcardorders().AddmemCard(GUID, USER_ID, out ID, buscode, stocode, memcode, cardtype, levelcode, otype, regamount, freeamount, cardcost, payamount, remark, status, ucode, uname, ref ordercode, paystatus, tcrCode, tcrName, shiftid, pcname, bak2, "", opencardinfo, opencardcoupon);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("ID");
                dt.Columns.Add("ordercode");
                dt.Rows[0]["ID"] = ID;
                dt.Rows[0]["ordercode"] = ordercode;
            }
            ReturnListJson(dt);
        }
        /// <summary>
        /// 会员卡换卡
        /// </summary>
        /// <param name="dicPar"></param>
        private void ChargeCard(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "memcode", "cardtype", "cardcode", "otype", "regamount", "freeamount", "cardcost", "payamount", "remark", "status", "ucode", "uname", "paystatus", "pushemcode", "pushname", "shiftid", "pcname", "bak2", "bak3", "opencardcoupon" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            string CardCode = dicPar["cardcode"].ToString();
            string otype = dicPar["otype"].ToString();
            string regamount = dicPar["regamount"].ToString();
            string freeamount = dicPar["freeamount"].ToString();
            string cardcost = dicPar["cardcost"].ToString();
            string payamount = dicPar["payamount"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string ucode = dicPar["ucode"].ToString();
            string uname = dicPar["uname"].ToString();
            string ordercode = "";
            string paystatus = dicPar["paystatus"].ToString();
            string tcrCode = dicPar["pushemcode"].ToString();
            string tcrName = dicPar["pushname"].ToString();
            string shiftid = dicPar["shiftid"].ToString();
            string pcname = dicPar["pcname"].ToString();
            string bak2 = dicPar["bak2"].ToString();
            string bak3 = dicPar["bak3"].ToString();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string coupons = jss.Serialize(dicPar["opencardcoupon"]);
            string ID = "";
            List<opencardcouponEntity> opencardcoupon = JsonHelper.AnalysisJson<List<opencardcouponEntity>>(coupons);

            DataTable dt = new BLL.bllmemcardorders().Recharge("", "0", out ID, buscode, stocode, memcode, cardtype, CardCode, otype, regamount, freeamount, cardcost, payamount, remark, status, ucode, uname, ref ordercode, paystatus, tcrCode, tcrName, shiftid, pcname, bak2, "", opencardcoupon);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("ID");
                dt.Columns.Add("ordercode");
                dt.Rows[0]["ID"] = ID;
                dt.Rows[0]["ordercode"] = ordercode;
            }
            ReturnListJson(dt);
        }

        private void ReturnCard(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "memcode", "cardtype", "cardcode", "otype", "regamount", "freeamount", "cardcost", "payamount", "remark", "status", "ucode", "uname", "paystatus", "shiftid", "postmac", "userid", "terminal" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            string CardCode = dicPar["cardcode"].ToString();
            string otype = dicPar["otype"].ToString();
            string regamount = dicPar["regamount"].ToString();
            string freeamount = dicPar["freeamount"].ToString();
            string cardcost = dicPar["cardcost"].ToString();
            string payamount = dicPar["payamount"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string ucode = dicPar["ucode"].ToString();
            string uname = dicPar["uname"].ToString();
            string ordercode = "";
            string paystatus = dicPar["paystatus"].ToString();
            string shiftid = dicPar["shiftid"].ToString();
            string postmac = dicPar["postmac"].ToString();
            string userid = dicPar["userid"].ToString();
            string terminal = dicPar["terminal"].ToString();
            string ID = "";
            string Paycode = "";

            DataTable dt = new BLL.bllmemcardorders().CardReturn("", "0", out ID, buscode, stocode, memcode, cardtype, CardCode, otype, regamount, freeamount, cardcost, payamount, remark, status, ucode, uname, ref ordercode, paystatus, shiftid);
            if (!string.IsNullOrEmpty(ordercode))
            {
                int n = new BLL.bllmemcardorders().RefundMemcard(buscode, stocode, CardCode, cardtype, otype, payamount, postmac, ordercode, shiftid, userid, terminal, ref Paycode);

                ToJsonStr("{\"code\":\"0\",\"mes\":\"操作成功\",\"count\":" + n + ",\"paycode\":\"" + Paycode + "\"}");
            }
            else
            {
                ToJsonStr("{\"code\":\"1\",\"mes\":\"操作失败，请稍后再试\"}");
            }
        }

        private void MergeCard(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "memcode", "cardtype", "cardcode", "otype", "regamount", "freeamount", "cardcost", "payamount", "remark", "status", "ucode", "uname", "paystatus", "shiftid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            string CardCode = dicPar["cardcode"].ToString();
            string otype = dicPar["otype"].ToString();
            string regamount = dicPar["regamount"].ToString();
            string freeamount = dicPar["freeamount"].ToString();
            string cardcost = dicPar["cardcost"].ToString();
            string payamount = dicPar["payamount"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string ucode = dicPar["ucode"].ToString();
            string uname = dicPar["uname"].ToString();
            string ordercode = "";
            string paystatus = dicPar["paystatus"].ToString();
            string shiftid = dicPar["shiftid"].ToString();
            string ID = "";

            DataTable dt = new BLL.bllmemcardorders().Add("", "0", out ID, buscode, stocode, "", "", CardCode, SystemEnum.MemCardStatus.Merge.ToString("D"), "0", "0", "0", "0", "", status, ucode, uname, ref ordercode, paystatus, "", "", shiftid, "", "", "", null, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("ID");
                dt.Columns.Add("ordercode");
                dt.Rows[0]["ID"] = ID;
                dt.Rows[0]["ordercode"] = ordercode;
            }
            ReturnListJson(dt);
        }

        private void ReturnMemCard(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "cardtype", "cardcode", "otype", "refundmoney", "postmac", "ordercode", "shiftid", "userid", "terminal" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            string CardCode = dicPar["cardcode"].ToString();
            string otype = dicPar["otype"].ToString();
            string refundmoney = dicPar["refundmoney"].ToString();
            string postmac = dicPar["postmac"].ToString();
            string ordercode = dicPar["ordercode"].ToString();
            string shiftid = dicPar["shiftid"].ToString();
            string userid = dicPar["userid"].ToString();
            string terminal = dicPar["terminal"].ToString();
            string Paycode = "";

            int n = new BLL.bllmemcardorders().RefundMemcard(buscode, stocode, CardCode, cardtype, otype, refundmoney, postmac, ordercode, shiftid, userid, terminal, ref Paycode);

            ToJsonStr("{\"code\":\"0\",\"mes\":\"操作成功\",\"count\":" + n + ",\"paycode\":\"" + Paycode + "\"}");
        }


        private void RefundMemCard(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "cardtype", "cardcode", "otype", "refundmoney", "postmac", "ordercode", "shiftid", "userid", "terminal" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            string CardCode = dicPar["cardcode"].ToString();
            string otype = dicPar["otype"].ToString();
            string refundmoney = dicPar["refundmoney"].ToString();
            string postmac = dicPar["postmac"].ToString();
            string ordercode = dicPar["ordercode"].ToString();
            string shiftid = dicPar["shiftid"].ToString();
            string userid = dicPar["userid"].ToString();
            string terminal = dicPar["terminal"].ToString();
            string Paycode = "";

            int n = new BLL.bllmemcardorders().RefundMemcard(buscode, stocode, CardCode, cardtype, otype, refundmoney, postmac, ordercode, shiftid, userid, terminal, ref Paycode);

            ToJsonStr("{\"code\":\"0\",\"mes\":\"操作成功\",\"count\":" + n + ",\"ordercode\":\"" + Paycode + "\",\"data\":[{\"code\":\"0\",\"mes\":\"操作成功\",\"count\":" + n + ",\"ordercode\":\"" + Paycode + "\"}]}");
        }

        private void GetOpendCardInfo(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "ordercode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ordercode = dicPar["ordercode"].ToString();
            BLL.bllmemcardorders bll = new BLL.bllmemcardorders();
            DataSet set = bll.GetOpendCardInfo(GUID, USER_ID, ordercode);
            if (set != null && set.Tables.Count == 2)
            {
                ArrayList arrData = new ArrayList();
                string[] arrTBName = new string[2] { "Table1", "Table2" };
                arrData.Add(set.Tables[0]);
                arrData.Add(set.Tables[1]);
                ReturnListJson("0", "操作成功！", arrData, arrTBName);
            }
            else
            {
                ToErrorJson();
            }
        }

        private void UpdateOpendCardInfo(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "ordercode", "oldcardcodes", "oldbalance", "balance" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ordercode = dicPar["ordercode"].ToString();
            string oldcardcodes = dicPar["oldcardcodes"].ToString();
            string oldbalance = dicPar["oldbalance"].ToString();
            string balance = dicPar["balance"].ToString();
            BLL.bllopencardinfo bll = new BLL.bllopencardinfo();
            bll.Update(ordercode, oldcardcodes, oldbalance, balance, "", "", "", "", "", "");
            ToSucessJson();
        }

        private void GetCardOrderInfo(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "ordercode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ordercode = dicPar["ordercode"].ToString();
            BLL.bllmemcardorders bll = new BLL.bllmemcardorders();
            DataTable dt = bll.GetCardOrderInfo(GUID, USER_ID, ordercode);
            if (dt != null)
            {
                ArrayList arrData = new ArrayList();
                string[] arrTBName = new string[1] { "data" };
                arrData.Add(dt);
                ReturnListJson("0", "操作成功！", arrData, arrTBName);
            }
            else
            {
                ToErrorJson();
            }
        }

        private void GetCardRechageInfo(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "ordercode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ordercode = dicPar["ordercode"].ToString();
            BLL.bllmemcardorders bll = new BLL.bllmemcardorders();
            DataSet set = bll.GetCardRechageInfo(GUID, USER_ID, ordercode);
            if (set != null && set.Tables.Count == 2)
            {
                ArrayList arrData = new ArrayList();
                string[] arrTBName = new string[2] { "Table1", "Table2" };
                arrData.Add(set.Tables[0]);
                arrData.Add(set.Tables[1]);
                ReturnListJson("0", "操作成功！", arrData, arrTBName);
            }
            else
            {
                ToErrorJson();
            }
        }

        /// <summary>
        /// 更新一条数据,bak1-存放押金
        /// </summary>
        public void UpdateCardOrder(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "ordercode", "oldcardcodes", "oldbalance", "balance", "memcname", "mob", "pcname", "bak1", "bak2", "bak3" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string ordercode = dicPar["ordercode"].ToString();
            string oldcardcodes = dicPar["oldcardcodes"].ToString();
            string oldbalance = dicPar["oldbalance"].ToString();
            string balance = dicPar["balance"].ToString();
            string memcname = dicPar["memcname"].ToString();
            string mob = dicPar["mob"].ToString();
            string pcname = dicPar["pcname"].ToString();
            string bak1 = dicPar["bak1"].ToString();
            string bak2 = dicPar["bak2"].ToString();
            string bak3 = dicPar["bak3"].ToString();
            string cardcode = "";
            if (dicPar.ContainsKey("cardcode"))
            {
                cardcode = dicPar["cardcode"].ToString();
            }
            BLL.bllmemcardorders bll = new BLL.bllmemcardorders();
            int result = bll.UpdateCardOrder(cardcode, ordercode, oldcardcodes, oldbalance, balance, memcname, mob, pcname, bak1, bak2, bak3);
            ToSucessJson();
        }

        /// <summary>
        /// 更新一条数据,bak1-存放押金
        /// </summary>
        public void UpdateMemcardMerg(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "ordercode", "oldcardcodes", "oldbalance", "remark" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string ordercode = dicPar["ordercode"].ToString();
            string oldcardcodes = dicPar["oldcardcodes"].ToString();
            string oldbalance = dicPar["oldbalance"].ToString();
            string remark = dicPar["remark"].ToString();
            BLL.bllmemcardorders bll = new BLL.bllmemcardorders();
            int result = bll.UpdateMemcardMerg(ordercode, oldcardcodes, oldbalance, remark);
            ToSucessJson();
        }

        /// <summary>
        /// 获取会员卡账单
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetBillList(Dictionary<string, object> dicPar)
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
            dt = new BLL.bllmemcardorders().GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);

            foreach (DataRow dr in dt.Rows)
            {
                string otype = dr["otype"].ToString();
                dr["otype"] = Helper.GetEnumNameByValue(typeof(SystemEnum.MemCardStatus), otype);
            }


            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        /// <summary>
        /// 储值报表
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetReportList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "page", "limit", "stocode", "bdate", "edate", "cardtype" };

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
            string stocode = dicPar["stocode"].ToString();
            string bdate = dicPar["bdate"].ToString();
            string edate = dicPar["edate"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            if((DateTime.Parse(edate)-DateTime.Parse(bdate)).TotalDays>31)
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"开始日期与结束日期不能超过31天\"}");
                return;
            }
            int recordCount = 0;
            int totalPage = 0;
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            //调用逻辑
            DataTable dtData = new DataTable();
            DataSet ds = new BLL.bllmemcardorders().GetCardRecharge(stocode, bdate, edate, cardtype,BusCode, currentPage, pageSize, out recordCount, out totalPage);
            if (ds != null && ds.Tables.Count >= 2)
            {
                dtData = ds.Tables[0];
                if (dtData != null)
                {
                    dtData.Columns.Add("otypename", typeof(string));
                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        if (dtData.Rows[i]["otype"].ToString() == "3")
                        {
                            dtData.Rows[i]["otypename"] = "补卡";
                        }
                        else if (dtData.Rows[i]["otype"].ToString() == "4")
                        {
                            dtData.Rows[i]["otypename"] = "换卡";
                        }
                        else if (dtData.Rows[i]["otype"].ToString() == "9")
                        {
                            dtData.Rows[i]["otypename"] = "退卡减少";
                        }
                        else
                        {
                            dtData.Rows[i]["otypename"] = "账户充值";
                        }
                    }
                }
                DataTable dtTotal = ds.Tables[1];
                if (dtTotal != null && dtTotal.Rows.Count >= 1)
                {
                    DataRow row = dtData.NewRow();
                    row["PKCode"] = "合计：";
                    row["cardcost"] = Helper.StringToDecimal(dtTotal.Rows[0]["cardcost"].ToString());
                    row["regamount"] = Helper.StringToDecimal(dtTotal.Rows[0]["regamount"].ToString());
                    row["freeamount"] = Helper.StringToDecimal(dtTotal.Rows[0]["freeamount"].ToString());
                    row["balance"] = Helper.StringToDecimal(dtTotal.Rows[0]["balance"].ToString());
                    row["totalamount"] = Helper.StringToDecimal(dtTotal.Rows[0]["totalamount"].ToString());
                    dtData.Rows.Add(row);
                }
            }

            ReturnListJson(dtData, pageSize, recordCount, currentPage, totalPage);
        }

        DataTable dtRemark = new DataTable();
        /// <summary>
        /// 导出储值报表
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetReportExport(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "page", "limit", "stocode", "bdate", "edate", "cardtype" };

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
            string stocode = dicPar["stocode"].ToString();
            string bdate = dicPar["bdate"].ToString();
            string edate = dicPar["edate"].ToString();
            string cardtype = dicPar["cardtype"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            //调用逻辑
            DataSet dsData = new BLL.bllmemcardorders().GetCardRechargeExport(stocode, bdate, edate, cardtype,BusCode);
            if (dsData != null && dsData.Tables.Count >= 2)
            {
                DataTable dtData = dsData.Tables[0];
                dtRemark = dsData.Tables[1];
                if (dtData != null)
                {
                    dtData.Columns.Add("otypename", typeof(string));
                    dtData.Columns.Add("payremark", typeof(string));
                    string strorderno = string.Empty;
                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        strorderno = dtData.Rows[i]["PKCode"].ToString();
                        if (dtData.Rows[i]["otype"].ToString() == "3")
                        {
                            dtData.Rows[i]["otypename"] = "补卡";
                        }
                        else if (dtData.Rows[i]["otype"].ToString() == "4")
                        {
                            dtData.Rows[i]["otypename"] = "换卡";
                        }
                        else if (dtData.Rows[i]["otype"].ToString() == "9")
                        {
                            dtData.Rows[i]["otypename"] = "退卡减少";
                        }
                        else
                        {
                            dtData.Rows[i]["otypename"] = "账户充值";
                        }
                        dtData.Rows[i]["payremark"] = GetPayRemark(strorderno);
                    }
                }
                dtData.TableName = "s";
                DataSet ds = new DataSet();
                ds.Tables.Add(dtData.Copy());

                DataTable detail = new DataTable();
                detail.Columns.Add("date", typeof(string));
                DataRow dr = detail.NewRow();
                dr["date"] = bdate.Replace("-", ".") + "-" + edate.Replace("-", ".");
                detail.Rows.Add(dr);

                string fileName = "会员充值流水";
                AsponseHelper asponse = new AsponseHelper();
                string tempFile = HttpContext.Current.Request.PhysicalApplicationPath + "\\templateData\\cardrechage.xlsx";
                string doloafile = Helper.GetAppSettings("StoBackRoot");
                doloafile += asponse.GetExportOPenfileByFilePath(fileName, tempFile, ds, detail);
                ToCustomerJson("0", doloafile);
            }
            else
            {
                ToErrorJson();
            }
        }

        private string GetPayRemark(string orderno)
        {
            string strReturn = string.Empty;
            if (dtRemark != null)
            {
                DataRow[] drs = dtRemark.Select("BillCode='" + orderno + "'");
                if (drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        strReturn += drs[i]["PayMethodName"].ToString() + ":" + drs[i]["PayMoney"].ToString() + ";";
                    }
                }
            }
            return strReturn;
        }
    }
}