﻿using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.IServices
{
	/// <summary>
    /// 购物车设置接口类
    /// </summary>
    public class WS_TB_ShoppingCart : ServiceBase
    {
        bllTB_ShoppingCart bll = new bllTB_ShoppingCart();
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
					logentity.module = "购物车设置";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "add"://添加							
                            Add(dicPar);
                            break;
                        case "detail"://详细
                            Detail(dicPar);
                            break;
                        case "update"://修改							
                            Update(dicPar);
                            break;
                        case "delete"://删除
                            Delete(dicPar);
                            break;
						 case "updatestatus"://修改状态
                            UpdateStatus(dicPar);
							break;
                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID","userid","page", "limit", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            int pageSize = StringHelper.StringToInt(dicPar["limit"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0 && filter != "[]")
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
            if (string.IsNullOrEmpty(order))
            {
                order = " order by ctime desc";
            }
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        } 
        
        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "CCode", "BusCode", "CCname", "TStatus", "PKCode", "AutoDelTime", "IsAutoDelete", "MaxNum" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
			string CCode = dicPar["CCode"].ToString();
			string BusCode = dicPar["BusCode"].ToString();
			string CCname = dicPar["CCname"].ToString();
			string TStatus = dicPar["TStatus"].ToString();
			string PKCode = dicPar["PKCode"].ToString();
			string AutoDelTime = "1900-01-01 " + dicPar["AutoDelTime"].ToString();
			string IsAutoDelete = dicPar["IsAutoDelete"].ToString();
			string MaxNum = dicPar["MaxNum"].ToString();
            //调用逻辑
			logentity.pageurl ="TB_ShoppingCartEdit.html";
			logentity.logcontent = "新增购物车设置信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Add(GUID, USER_ID,BusCode, CCname, TStatus, PKCode, AutoDelTime, IsAutoDelete, MaxNum, CCode, logentity);
			
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "CCode", "BusCode", "CCname", "TStatus", "PKCode", "AutoDelTime", "IsAutoDelete", "MaxNum" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
			string CCode = dicPar["CCode"].ToString();
			string BusCode = dicPar["BusCode"].ToString();
			string CCname = dicPar["CCname"].ToString();
			string TStatus = dicPar["TStatus"].ToString();
			string PKCode = dicPar["PKCode"].ToString();
            string AutoDelTime = "1900-01-01 " + dicPar["AutoDelTime"].ToString();
            string IsAutoDelete = dicPar["IsAutoDelete"].ToString();
			string MaxNum = dicPar["MaxNum"].ToString();

            //调用逻辑
			logentity.pageurl ="TB_ShoppingCartEdit.html";
			logentity.logcontent = "修改PKCode为:"+PKCode+"的购物车设置信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Edit;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Update(GUID, USER_ID,BusCode, CCname, TStatus, PKCode, AutoDelTime, IsAutoDelete, MaxNum, CCode, logentity);
            
            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "PKCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where PKCode='" + PKCode+"'");
            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "id"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string PKCode = dicPar["id"].ToString();
            //调用逻辑
			logentity.pageurl ="TB_ShoppingCartList.html";
			logentity.logcontent = "删除PKCode为:"+PKCode+"的购物车设置信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Delete;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Delete(GUID, USER_ID, PKCode, logentity);
            ReturnListJson(dt);
        }

		/// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "id", "status" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ids = dicPar["id"].ToString();
            string status = dicPar["status"].ToString();

            string PKCode = dicPar["id"].ToString().Trim(',');
            logentity.pageurl ="TB_ShoppingCartList.html";
			logentity.logcontent = "修改状态PKCode为:"+PKCode+"的购物车设置信息";
			logentity.cuser = StringHelper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, PKCode, status);

            ReturnListJson(dt);
        }
    }
}