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
    /// ts_Dicts 的摘要说明
    /// </summary>
    public class ts_Dicts : ServiceBase
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
                    logentity.module = "系统字典";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetAllDictsList(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 综合版获取总部的下的计量单位信息;
        /// </summary>
        /// <param name="buscode">商户编号</param>
        /// <returns></returns>
        private void GetAllDictsList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "BusCode", "lng" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string BusCode = dicPar["BusCode"].ToString();
            string lng = dicPar["lng"].ToString();
            StringBuilder postStr = new StringBuilder();
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/IStore/WSsto_ts_Dicts.ashx";
            postStr.Append("actionname=getnewalllistinfo&parameters={" +
                                           string.Format("\"buscode\": \"{0}\",", BusCode) +
                                           string.Format("\"stocode\": \"{0}\",", Helper.GetAppSettings("StoCode")) +
                                           string.Format("\"lng\": \"{0}\"", lng) +
                                   "}");//键值对
            string strDictsJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            if (!string.IsNullOrEmpty(strDictsJson) && strDictsJson.Trim() != "")
            {
                string code = "";
                string msg = "";
                string limit = "";
                string count = "";
                string curpage = "";
                string totpage = "";
                DataSet ds = JsonHelper.NewLinJsonToDataSet(strDictsJson, out code, out msg, out limit, out count, out curpage, out totpage);
                if (code != "0")
                {
                    ToCustomerJson("2", "获取失败 x001");
                    return;
                }
                ReturnListJson(ds.Tables[0], Helper.StringToInt(limit), Helper.StringToInt(count), Helper.StringToInt(curpage), Helper.StringToInt(totpage));
            }
            else
            {
                ToCustomerJson("2", "获取失败 x001");
                return;
            }
        }

    }
}