using CommunityBuy.BLL;
using CommunityBuy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    public class WS_TB_MpCoupon : ServiceBase
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
                    logentity.module = "影城卖品券";
                    switch (actionname.ToLower())
                    {
                        case "getinfo"://获取券信息
                            GetInfo(dicPar);
                            break;
                        case "checkcoupon"://消券
                            CheckCoupon(dicPar);
                            break;
                        case "cancelcoupon"://取消使用
                            CancelCoupon(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取卖品券信息
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetInfo(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "checkcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string guid = dicPar["GUID"].ToString();
            string uid = dicPar["USER_ID"].ToString();
            string checkcode = dicPar["checkcode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            //获取卖品券
            string mpUrl = Helper.GetAppSettings("MpUrl") + "/AboutForm.ashx";
            string mpParameters = "actionname=getorderinfo&parameters={";
            mpParameters += "'GUID':'" + guid + "',";
            mpParameters += "'USER_ID':'" + uid + "',";
            mpParameters += "'stocode':'" + stocode + "',";
            mpParameters += "'checkcode':'" + checkcode + "'";
            mpParameters += "}";
            string mpResponse = Helper.HttpWebRequestByURL(mpUrl, mpParameters);
            ToJsonStr(mpResponse);
        }

        /// <summary>
        /// 使用卖品券
        /// </summary>
        /// <param name="dicPar"></param>
        private void CheckCoupon(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "checkcode", "usercode", "username","money","code" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string guid = dicPar["GUID"].ToString();
            string uid = dicPar["USER_ID"].ToString();
            string checkcode = dicPar["checkcode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string usercode = dicPar["usercode"].ToString();
            string username = dicPar["username"].ToString();
            string money= dicPar["money"].ToString();
            string code= dicPar["code"].ToString();

            //获取卖品券
            string mpUrl = Helper.GetAppSettings("MpUrl") + "/AboutForm.ashx";
            string mpParameters = "actionname=modifyorderstatus&parameters={";
            mpParameters += "'GUID':'" + guid + "',";
            mpParameters += "'USER_ID':'" + uid + "',";
            mpParameters += "'stocode':'" + stocode + "'";
            mpParameters += ",'checkcode':'" + checkcode + "'";
            mpParameters += ",'usercode':'" + usercode + "'";
            mpParameters += ",'username':'" + username + "'";
            mpParameters += ",'pickupmoney':'" + money + "'";
            mpParameters += ",'pickupcode':'" + code + "'";
            mpParameters += "}";
            string mpResponse = Helper.HttpWebRequestByURL(mpUrl, mpParameters);
            ToJsonStr(mpResponse);
        }


        /// <summary>
        /// 取消使用卖品券
        /// </summary>
        /// <param name="dicPar"></param>
        private void CancelCoupon(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "checkcode", "usercode", "username" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string guid = dicPar["GUID"].ToString();
            string uid = dicPar["USER_ID"].ToString();
            string checkcode = dicPar["checkcode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string usercode = dicPar["usercode"].ToString();
            string username = dicPar["username"].ToString();

            //获取卖品券
            string mpUrl = Helper.GetAppSettings("MpUrl") + "/AboutForm.ashx";
            string mpParameters = "actionname=cancelorderstatus&parameters={";
            mpParameters += "'GUID':'" + guid + "',";
            mpParameters += "'USER_ID':'" + uid + "',";
            mpParameters += "'stocode':'" + stocode + "'";
            mpParameters += ",'checkcode':'" + checkcode + "'";
            mpParameters += ",'usercode':'" + usercode + "'";
            mpParameters += ",'username':'" + username + "'";
            mpParameters += "}";
            string mpResponse = Helper.HttpWebRequestByURL(mpUrl, mpParameters);
            ToJsonStr(mpResponse);
        }
    }
}