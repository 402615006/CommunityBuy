using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.ajax.memberCard
{
    /// <summary>
    /// getmemberCard 的摘要说明
    /// </summary>
    public class getmemberCard : ServiceBase, System.Web.SessionState.IRequiresSessionState
    {
        bllEmployee blle = new bllEmployee();
        DataTable dt = new DataTable();
        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    logentity.module = "门店_原料分类";
                    switch (actionname.ToLower())
                    {
                        case "memcardstoragelist":
                            memCardStoragelist(dicPar);
                            break;
                        case "cvstorageauditing":
                            CVStorageAuditing(dicPar);
                            break;
                        case "getcustomerslist":
                            GetCustomersList(dicPar);
                            break;
                        case "getcvsalemateslist":
                            GetCVSaleMatesList(dicPar);
                            break;
                        case "getcvsalepaymentlist":
                            GetCVSalePaymentList(dicPar);
                            break;
                        case "cvsalesauditing":
                            CVSalesAuditing(dicPar);
                            break;
                        case "getcardlevelinfo":
                            getCardLevelInfo(dicPar);
                            break;
                        case "getcardstatusbycardcode":
                            GetCardStatusByCardCode(dicPar);
                            break;
                        case "getcardlist":
                            GetCardList(dicPar);
                            break;
                        case "checkmembermobileandidno":
                            CheckMemberMobileAndIDNO(dicPar);
                            break;
                        case "checkemployeemobileandidno":
                            CheckEmployeeMobileAndIDNO(dicPar);
                            break;
                        case "checkemployeemobileandidno1":
                            CheckEmployeeMobileAndIDNO1(dicPar);
                            break;
                        case "getmemcardaccountstoreslist":
                            GetMemCardAccountStoresList(dicPar);
                            break;
                        case "getmaincouponinfo":
                            GetMainCouponInfo(dicPar);
                            break;
                        case "delaycoupon":
                            DelayCoupon(dicPar);
                            break;
                        case "checkcvsales":
                            CheckCVSales(dicPar);
                            break;
                        case "buildk3sr":
                            BuildK3Sr(dicPar);
                            break;
                        case "memmateriareferlist":
                            memMateriaReferList(dicPar);
                            break;
                        case "sendmessage"://手机号发送验证码
                            SendMessage(dicPar);
                            break;
                    }
                }
            }
        }

        public void SendMessage(Dictionary<string, object> dicPar)
        {
            string phone = dicPar["phone"].ToString();
            string type = dicPar["type"].ToString();
            string ServiceUrl = Helper.GetAppSettings("MemberCardUrl");
            string InterfaceUrl = ServiceUrl.TrimEnd('/') + "/IsystemSet/WSAliyunSendMsg.ashx";
            StringBuilder postStr = new StringBuilder();

            string getosendYZM = "actionname={0}&parameters={{\"mobile\":\"{1}\", \"descr\":\"{2}\",\"vercode\":\"1\",\"buscode\":\"{3}\",\"stocode\":\"{4}\"}}";
            postStr.Append(string.Format(getosendYZM, "sendmsg", phone, "会员修改手机号码验证", "88888888", LoginedUser.UserInfo.stocode));
            string result = Helper.HttpWebRequestByURL(InterfaceUrl, postStr);
            if (!string.IsNullOrWhiteSpace(result))
            {
                string status = JsonHelper.GetJsonValByKey(result, "status");
                string mes = JsonHelper.GetJsonValByKey(result, "mes");
                if (status == "0")
                {
                    try
                    {
                        Pagcontext.Session["code" + phone] = mes;
                    }
                    catch (Exception ex)
                    {
                    }
                    string flag = "";
                    if (type == "1")//会员新手机号
                    {
                        flag = new bllPaging().ExecuteScalarBySQL("select memcode from members where mobile='" + phone + "'");
                    }
                    Pagcontext.Response.Write("{\"status\":\"0\",\"mes\":\"" + flag + "\"}");
                    return;
                }
            }
            ToErrorJson();
        }

        public void memMateriaReferList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "warcode", "proapplycode", "matcodes" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string warcode = dicPar["warcode"].ToString();
            string proapplycode = dicPar["proapplycode"].ToString();
            string matcodes = dicPar["matcodes"].ToString();

            string buscode = Helper.GetAppSettings("BusCode");

            string stocode = LoginedUser.UserInfo.stocode;
            if (string.IsNullOrEmpty(stocode))
            {
                stocode = Helper.GetAppSettings("Stocode");
            }
            else
            {
                string[] stocode_arr = stocode.Split(',');
                stocode = stocode_arr[0];
            }

            string filter = " where matcode in (select col from dbo.fn_StringSplit('" + matcodes.Trim(',') + "',','))";
            int recnums = 0;
            int pagenums = 0;

            DataTable dt = new bllPaging("StockConnectionString").GetPagingInfo("StockMaterial", "matid", "matid,matcode,matname,dbo.fnGetStockNum('" + warcode + "',matcode,dbo.fnGetCYUnitCode(matcode)) as stocknum,dbo.fnGetSQStockNum('" + buscode + "',matcode,'" + warcode + "','" + proapplycode + "',dbo.fnGetCYUnitCode(matcode)) as sqnum,matunitname=dbo.fnGetUnitName(matid),smunitname1=dbo.fnGetunitcode(matid),lsprice=dbo.fnGetTrueSmsalprice(matcode,'" + stocode + "'),spec", 10000, 1, filter, "", "", out recnums, out pagenums);

            if (dt != null && dt.Rows.Count > 0)
            {
                ReturnListJson(dt);
            }
            else
            {
                ToErrorJson();
            }
        }

        public void BuildK3Sr(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "date", "stocode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string date = dicPar["date"].ToString();
            string stocode = dicPar["stocode"].ToString();

            string rescode = string.Empty;

            bllk3.CheckK3SrManage(date, stocode, ref rescode);

            if (rescode == "0")
            {
                ToSucessJson();
            }
            else
            {
                ToErrorJson();
            }

        }

        //获取优惠活动优惠券信息
        public void GetMainCouponInfo(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "mcid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string mcid = dicPar["mcid"].ToString();
            dt = new bllPaging().GetDataTableInfoBySQL("select couname,convert(varchar(10),btime,120) as btime,convert(varchar(10),etime,120) as etime from N_maincoupon where mcid=" + mcid);
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        //优惠券延期
        public void DelayCoupon(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "mcid", "etime" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string mcid = dicPar["mcid"].ToString();
            string etime = dicPar["etime"].ToString();

            bllc.DelayCoupon(mcid, etime);
            ToSucessJson();
        }

        /// <summary>
        /// 获取采购申请信息
        /// </summary>
        /// <param name="dicPar"></param>
        public void memCardStoragelist(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "applyid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string applyid = dicPar["applyid"].ToString();
            dt = new bllPaging("StockConnectionString").GetDataTableInfoBySQL("select sam.matcode,sam.unitname,sam.realproxprice as proxprice,sam.realprocount as procount from CVStorageMates sam left join CVStorage saly on sam.storagecode=saly.storagecode where saly.sstorid = " + applyid);
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        //获取卡状态信息
        public void GetCardStatusByCardCode(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "CardCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string CardCode = dicPar["CardCode"].ToString();
            dt = bllmc.GetCardStatusByCardNo("", "0", CardCode);
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        //获取会员卡信息
        public void GetCardList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "CardCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string CardCode = dicPar["CardCode"].ToString();
            dt = bllmc.GetPagingListInfo("", "0", " where mc.cardcode='" + CardCode + "'");
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        //会员信息 检测手机号和证件号是否存在
        public void CheckMemberMobileAndIDNO(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "mobile", "idtype", "idno" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string mobile = dicPar["mobile"].ToString();
            string idtype = dicPar["idtype"].ToString();
            string idno = dicPar["idno"].ToString();

            dt = bllm.MembersCheck("", "0", mobile, idtype, idno);
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        //员工信息 检测手机号和证件号是否存在
        public void CheckEmployeeMobileAndIDNO(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "mobile", "idtype", "idno" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string mobile = dicPar["mobile"].ToString();
            string idtype = dicPar["idtype"].ToString();
            string idno = dicPar["idno"].ToString();

            dt = blle.EmployeeCheck("", "0", mobile, idtype, idno);
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        //员工信息 检测手机号和证件号是否存在（修改状态使用)
        public void CheckEmployeeMobileAndIDNO1(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "mobile", "idtype", "idno", "empid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string mobile = dicPar["mobile"].ToString();
            string idtype = dicPar["idtype"].ToString();
            string idno = dicPar["idno"].ToString();
            string empid = dicPar["empid"].ToString();

            dt = blle.EmployeeCheck1("", "0", mobile, idtype, idno, empid);
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        //根据会员卡号获取可以签单（挂账）的门店列表信息
        public void GetMemCardAccountStoresList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "cardcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string cardcode = dicPar["cardcode"].ToString();

            dt = new bllPaging().GetDataTableInfoBySQL("select cardcode,ctype,stocode,stoname=dbo.fnGetStoreName(stocode) from memcardaccountstores where cardcode='" + cardcode + "';");
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        /// <summary>
        /// 根据会员卡类型获取会员卡级别信息
        /// </summary>
        /// <param name="dicPar"></param>
        public void getCardLevelInfo(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "mctcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string mctcode = dicPar["mctcode"].ToString();
            dt = new bllPaging().GetDataTableInfoBySQL(" select levelcode,levelname from memcardlevel where mctcode='" + mctcode + "';");

            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetCustomersList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "uode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string uode = dicPar["uode"].ToString();
            dt = new bllPaging().GetDataTableInfoBySQL("select companycode,comname,linkman,tel,address from memcustomer where companycode='" + uode + "';");
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取卡券销售货品信息
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetCVSaleMatesList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "cvsalescode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string cvsalescode = dicPar["cvsalescode"].ToString();
            dt = new bllPaging("StockConnectionString").GetDataTableInfoBySQL("select matcode,dbo.f_GetStockMaterialName(matcode) as matname,unitname as unitcode,dbo.fnGetDistsName(unitname) as unitname,averagecost,mcount,protype,dbo.fnGetDistsName(protype) as protypename,salestype,dbo.fnGetDistsName(salestype) as salestypename,moneytype,dbo.fnGetDistsName(moneytype) as moneytypename,saletype,dbo.fnGetDistsName(saletype) as saletypename,remark,dbo.fnGetSpec(matcode) as spec,batchno,searnostart,searnoend from CVSalesMates where cvsalescode='" + cvsalescode + "';");
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取卡券支付方式信息
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetCVSalePaymentList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "cvsalescode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string cvsalescode = dicPar["cvsalescode"].ToString();
            dt = new bllPaging("StockConnectionString").GetDataTableInfoBySQL("select pmcode,averagecost,dbo.fnGetSumPrice(cvsalescode,'Sales') as saleprice,dbo.fnGetSumPrice(cvsalescode,'Gives') as giveprice from CVSalesPayment where cvsalescode = '" + cvsalescode + "';");
            if (dt == null)
            {
                ToErrorJson();
                return;
            }

            ReturnListJson(dt);
        }

        //卡券入库 复核
        private void CVStorageAuditing(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "storagecode", "status", "remark", "eid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string storagecode = dicPar["storagecode"].ToString();
            string status = dicPar["status"].ToString();
            string remark = dicPar["remark"].ToString();
            string eid = dicPar["eid"].ToString();
            int count = bllcs.CVStorageAudit(storagecode, eid, status, remark);
            if (count == 0)
            {
                ToSucessJson();
                return;
            }
            else
            {
                ToErrorJson();
                return;
            }
        }

        //卡券销售 审核
        private void CVSalesAuditing(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "applycode", "eid", "status", "remark", "empcode", "filepath" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string applycode = dicPar["applycode"].ToString();
            string status = dicPar["status"].ToString();
            string remark = dicPar["remark"].ToString();
            string empcode = dicPar["empcode"].ToString();
            string eid = dicPar["eid"].ToString();
            string rescode = string.Empty;
            string resmes = string.Empty;
            string filepath = dicPar["filepath"].ToString();

            int count = bllcvs.CVSaleAudit(applycode, eid, status, remark, empcode, filepath);

            if (count >= 0)
            {
                ToSucessJson();
            }
            else
            {
                ToErrorJson();
            }
        }

        //卡券销售 检测（批次号 序号区间）
        private void CheckCVSales(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "batchno", "searnostart", "searnoend" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string batchno = dicPar["batchno"].ToString();
            string searnostart = dicPar["searnostart"].ToString();
            string searnoend = dicPar["searnoend"].ToString();
            string rescode = string.Empty;
            bllcvs.CheckCVSales(batchno, searnostart, searnoend, ref rescode);
            switch (rescode)
            {
                case "0":
                    ToCustomerJson(rescode, "操作成功");
                    break;
                case "1":
                    ToCustomerJson(rescode, "批次号不存在");
                    break;
                case "2":
                    ToCustomerJson(rescode, "序号开始号大于结束号");
                    break;
                case "3":
                    ToCustomerJson(rescode, "序号结束号大于优惠券实际发放结束号");
                    break;
                case "4":
                    ToCustomerJson(rescode, "选择序号优惠券存在非未使用状态");
                    break;
                case "5":
                    ToCustomerJson(rescode, "序号序号区间的优惠券已发放给其他客户");
                    break;
                case "6":
                    ToCustomerJson(rescode, "优惠券所属活动未审核通过");
                    break;
                default:
                    ToCustomerJson(rescode, "数据错误");
                    break;
            }
        }
    }
}