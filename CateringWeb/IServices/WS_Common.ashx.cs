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
    /// <summary>
    /// WS_TB_Admins 的摘要说明
    /// </summary>
    public class WS_Common : ServiceBase
    {

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
                    logentity.module = "用户信息表";
                    switch (actionname.ToLower())
                    {
                        case "sendmessage"://列表
                            SendPhoneMessage(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 免登陆验证
        /// </summary>
        /// <param name="dicPar"></param>
        private void SendPhoneMessage(Dictionary<string, object> dicPar)
        {
            try
            {
                //要检测的参数信息
                List<string> pra = new List<string>() { "buscode", "stocode", "mobile", "vercode", "descr" };
                //检测方法需要的参数
                if (!CheckActionParameters(dicPar, pra))
                {
                    return;
                }
                
                StringBuilder postStr = new StringBuilder();
                //获取参数信息  "mobile", "vercode", "descr", "buscode", "stocode"

                string buscode = dicPar["buscode"].ToString();
                string stocode = dicPar["stocode"].ToString();
                string mobile = dicPar["mobile"].ToString();
                string vercode = dicPar["vercode"].ToString();
                string descr = dicPar["descr"].ToString();
                if (mobile.Length == 0)
                {
                    ToCustomerJson("1", "手机号不能为空");
                    return;
                }
                else
                {
                    if (!RegularExpressions.IsRegExpType(mobile, RegularExpressions.RegExpType.Mobile))
                    {
                        ToCustomerJson("1", "手机号格式不正确请检查！");
                        return;
                    }
                }

                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/IsystemSet/WSAliyunSendMsg.ashx";
                postStr.Append("actionname=sendmsg&parameters={" +
                                               string.Format("'buscode':'{0}'", buscode) +
                                               string.Format(",'stocode': '{0}'", stocode) +
                                               string.Format(",'mobile': '{0}'", mobile) +
                                               string.Format(",'vercode':'{0}'", vercode) +
                                               string.Format(",'descr':'{0}'", descr) +
                                       "}");//键值对
                string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
                {
                    string status=JsonHelper.GetJsonValByKey(strAdminJson,"status");
                    string mes= JsonHelper.GetJsonValByKey(strAdminJson, "mes");
                    if (status == "0")
                    {
                        ReturnListJson(status, mes, null, null);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}