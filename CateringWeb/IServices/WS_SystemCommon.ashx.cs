using CommunityBuy.BLL;
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
    /// WSSystemCommon 公共的一些接口 的摘要说明
    /// </summary>
    public class WS_SystemCommon : ServiceBase
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
                    switch (actionname.ToLower())
                    {
                        case "getenumbycode"://获取枚举信息
                            GetEnumByCode(dicPar);
                            break;
                        case "getpermission"://获取页面权限按钮信息
                            GetPermission(dicPar);
                            break;
                        case "getsystime"://获取页面权限按钮信息
                            GetSysTime(dicPar);
                            break;
                        case "checkmaincontrol"://检查总控连接
                            CheckMainControl(dicPar);
                            break;
                        case "getformsysversion"://获取系统的版本号
                            GetFormSysVersion(dicPar);
                            break;
                    }
                }
            }
        }
        private void GetPermission(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "pagecode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string pagecode = dicPar["pagecode"].ToString();

            //调用逻辑
            DataTable dt = new bllTB_Functions().GetFunctionsButtonByPageCode(GUID, userid, pagecode);
            ReturnListJson(dt);
        }

        private void GetEnumByCode(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "enumcode", "lng" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string enumcode = dicPar["enumcode"].ToString();
            string lng = dicPar["lng"].ToString();

            //调用逻辑
            DataTable dt = new DataTable();
            ReturnListJson(dt);
        }

        private void GetSysTime(Dictionary<string, object> dicPar)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string json="{\"time\":\""+ time + "\"}";
            //DataTable dt = JsonHelper.JsonToDataTable(json);
            ToCustomerJson("0", time);
        }
        
        /// <summary>
        /// 检查总控是否启动
        /// </summary>
        /// <param name="dicPar"></param>
        private void CheckMainControl(Dictionary<string, object> dicPar)
        {
            StringBuilder postStr = new StringBuilder();
            //获取参数信息 "GUID", "USER_ID", "pageSize", "currentPage", "filter", "order" 
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string terminalcode = dicPar["terminalcode"].ToString();
            string remark = dicPar["remark"].ToString();

            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/IStore/WSts_ManAuthorization.ashx";
            postStr.Append("actionname=checkloginbyterminalcode&parameters={" +
                                           string.Format("\"GUID\":\"{0}\"", GUID) +
                                           string.Format(",\"USER_ID\": \"{0}\"", USER_ID) +
                                           string.Format(",\"buscode\": \"{0}\"", buscode) +
                                           string.Format(",\"stocode\": \"{0}\"", stocode) +
                                           string.Format(",\"terminalcode\": \"{0}\"", terminalcode) +
                                           string.Format(",\"remark\": \"{0}\"", remark) +
                                   "}");//键值对
            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            ToJsonStr(strAdminJson);
        }

        /// <summary>
        /// 获取系统的版本号
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetFormSysVersion(Dictionary<string, object> dicPar)
        {
            StringBuilder postStr = new StringBuilder();
            //获取参数信息 "GUID", "USER_ID", "pageSize", "currentPage", "filter", "order" 
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/WSSoftVersion.ashx";
            postStr.Append("actionname=getlist&parameters={" +
                                           string.Format("\"GUID\":\"{0}\"", GUID) +
                                           string.Format(",\"USER_ID\": \"{0}\"", USER_ID) +
                                           string.Format(",\"pageSize\": \"{0}\"", "1") +
                                           string.Format(",\"currentPage\": \"{0}\"", "1") +
                                           string.Format(",\"filter\": \"{0}\"", " Softname='NewCateringForm' ") +
                                           string.Format(",\"order\": \"{0}\"", "") +
                                   "}");//键值对
            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            ToJsonStr(strAdminJson);
        }

    }
}