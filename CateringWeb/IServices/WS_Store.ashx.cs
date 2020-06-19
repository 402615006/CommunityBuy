using CommunityBuy.BLL;
using CommunityBuy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_Store 的摘要说明
    /// </summary>
    public class WS_Store : ServiceBase
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
                        case "getalllist"://获取连锁端门店信息,不分页
                            GetAllList(dicPar);
                            break;
                        case "getbuscodetostolist"://获取连锁端门店信息,不分页
                            GetBusCodeToStoList(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取连锁端门店信息,不分页
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetAllList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "BusCode", "StoNmae" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string busCode = dicPar["BusCode"].ToString();
            string userid = dicPar["userid"].ToString();
            //string where = "where 1=1 and (buscode='" + busCode + "' or ''='" + busCode + "') " + GetAuthoritywhere("stocode", userid);
            string where = "where 1=1 ";
            if (!string.IsNullOrEmpty(busCode) && busCode != "undefined")
            {
                where += " and buscode='" + busCode + "'";
            }
            if (dicPar["StoNmae"] != null)
            {
                if (!string.IsNullOrEmpty(dicPar["StoNmae"].ToString()) && dicPar["StoNmae"].ToString() != "")
                {
                    where += " and (cname like '%" + dicPar["StoNmae"].ToString() + "%' or sname like '%" + dicPar["StoNmae"].ToString() + "%') ";
                }
            }
            dt = new bllStore().GetPagingListInfo(GUID, USER_ID, int.MaxValue, 1, where, "", out int recnums, out int pagenums);
            ReturnListJson(dt,null,null,null,null);
        }

        /// <summary>
        /// 获取连锁端门店信息,不分页
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetBusCodeToStoList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string BusCode = string.Empty;
            if (dicPar.Keys.Contains("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            string where = "where 1=1";
            if (dicPar.ContainsKey("BusCode"))
            {
                where += "and buscode='" + dicPar["BusCode"].ToString() + "'";
            }
            if (dicPar.ContainsKey("StoNmae"))
            {
                if (!string.IsNullOrEmpty(dicPar["StoNmae"].ToString()))
                {
                    where += " and (cname like '%" + dicPar["StoNmae"].ToString() + "%' or sname like '%" + dicPar["StoNmae"].ToString() + "%') ";
                }
            }
            dt = new bllStore().GetPagingListInfo(GUID,USER_ID,int.MaxValue,1,where,"",out int recnums,out int pagenums);
            ReturnListJson(dt,1,recnums,1,pagenums);
        }

    }
}