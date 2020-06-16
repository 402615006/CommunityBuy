using CommunityBuy.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// 退款操作
    /// </summary>
    public class WSTB_BillReturn : ServiceBase
    {
        DataTable dt = new DataTable();
        operatelogEntity logentity = new operatelogEntity();

        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    logentity.module = "账单退款";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "refundcouponpay":
                            RefundCouponAndPayMethod(dicPar);
                            break;
                    }
                }
            }
        }

        //获取待退款列表
        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            string stocode = dicPar["stocode"].ToString();
            dt = new BLL.bllPaging().GetDataTableInfoBySQL("select BusCode,BillCode,PKCode,PayMoney,OutOrderCode,paymethodcode,Remar,'1' as rtype from dbo.TB_BillPay where billcode in(select BillCode from dbo.TB_BillReturn where TStatus in('0','1') and Ctime<=DATEADD(minute,-2,getdate()) and StoCode='" + stocode + "') and TStatus='1' and PayMethodCode in ('1','2','7') and stocode='" + stocode + "' union all select BusCode,BillCode,'' as PKCode,0 as PayMoney,'' as OutOrderCode,'' as paymethodcode,'' as Remar,'2' as rtype from TB_BillCoupon where billcode in(select BillCode from dbo.TB_BillReturn where TStatus in('0','1') and Ctime<=DATEADD(minute,-2,getdate()) and StoCode='" + stocode + "') and TStatus='1' and stocode='" + stocode + "';");

            if (dt != null && dt.Rows.Count > 0)
            {
                ReturnListJson(dt);
            }
            else
            {
                ToCustomerJson("1", "暂无数据");
            }
        }

        //反结优惠券及（新增反结记录），更新反结状态
        private void RefundCouponAndPayMethod(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "billcode", "pkcode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            string billcode = dicPar["billcode"].ToString();
            string pkcode = dicPar["pkcode"].ToString();
            string sqlStr = string.Empty;
            string couponcodes = string.Empty;
            string buscode = string.Empty;
            string stocode = string.Empty;
            dt = new BLL.bllPaging().GetDataTableInfoBySQL("select BusCode,StoCode,CouponCode from TB_BillCoupon where TStatus='1' and BillCode='" + billcode + "';");
            if (dt != null && dt.Rows.Count > 0)
            {
                buscode = dt.Rows[0]["BusCode"].ToString();
                stocode = dt.Rows[0]["StoCode"].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    couponcodes += dr["CouponCode"].ToString() + ",";
                }
            }
            //(多)优惠券(代金券)取消回收
            if (!string.IsNullOrEmpty(couponcodes))
            {
                couponcodes = couponcodes.TrimEnd(',');
                string InterfaceUrl = Helper.GetAppSettings("ServiceUrl") + "/coupon/WScheckcoupon.ashx";
                string ListParameters = "actionname={0}&parameters={{\"GUID\":\"\",\"USER_ID\":\"0\",\"buscode\":\"{1}\",\"stocode\":\"{2}\",\"couponcodes\":\"{3}\",\"username\":\"{4}\",\"usercode\":\"{5}\",\"way\":\"{6}\"}}&usercode={7}";
                StringBuilder postStr = new StringBuilder();
                postStr.Append(string.Format(ListParameters, "couponsrecoverycancelnew", buscode, stocode, couponcodes, "总控端", "ZK", "1", "ZK"));//键值对
                string jsonStr = Helper.HttpWebRequestByURL(InterfaceUrl, postStr);
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    string status = string.Empty;
                    string mes = string.Empty;
                    JsonHelper.JsonToDataSet(jsonStr, out status, out mes);
                    //退款成功
                    if (status == "0")
                    {
                        //反结优惠券
                        sqlStr += "update TB_BillCoupon set TStatus='2' where BillCode='" + billcode + "' and TStatus='1';";
                    }
                }
            }

            //插入支付反结数据
            if (!string.IsNullOrEmpty(pkcode))
            {
                sqlStr += " INSERT INTO dbo.TB_BillPay(BusCode,StoCode,CCode,CCname,CTime,TStatus,PKCode,BillCode,PayMoney,PayMethodName,PayMethodCode,Remar,OutOrderCode,PPKCode) select BusCode,StoCode,CCode,CCname,CTime,'2',PKCode+'_1',BillCode,PayMoney,PayMethodName,PayMethodCode,Remar,billcode,PKCode from dbo.TB_BillPay where billcode='" + billcode + "' and pkcode='" + pkcode + "' and TStatus='1' and PayMethodCode in('1','2','7');";

                //更新之前支付记录反结状态为取消=4
                sqlStr += "update dbo.TB_BillPay set TStatus='4' where billcode='" + billcode + "' and pkcode='" + pkcode + "' and TStatus='1' and PayMethodCode in ('1','2','7');";
            }
            sqlStr += "update TB_BillReturn set tstatus='2' where billcode='" + billcode + "';";
            int count = new BLL.bllPaging().ExecuteNonQueryBySQL(sqlStr);
            if (count >= 0)
            {
                ToCustomerJson("0", "操作成功");
            }
            else
            {
                ToCustomerJson("1", "操作失败");
            }
        }
    }
}