using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using CommunityBuy.IServices;

namespace CateringWeb.IServices
{
	/// <summary>
    /// 系统功能管理接口类
    /// </summary>
    public class WSTB_Functions : ServiceBase
    {
        bllTB_Functions bll = new bllTB_Functions();
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
					logentity.module = "系统功能管理";
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
                        case "getfunctionlistbyparentidandstocode":
                            GetFunctionListByParentIdAndStocode(dicPar);
                            break;
                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
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
            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt);
        } 
        
        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCname", "TStatus", "FType", "ParentId", "Code", "Cname", "BtnCode", "Orders", "ImgName", "Url", "Level", "Descr","CCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
			string Id = dicPar["Id"].ToString();
			string BusCode = dicPar["BusCode"].ToString();
			string StoCode = dicPar["StoCode"].ToString();
			string CCname = dicPar["CCname"].ToString();
			string TStatus = dicPar["TStatus"].ToString();
			string FType = dicPar["FType"].ToString();
			string ParentId = dicPar["ParentId"].ToString();
			string Code = dicPar["Code"].ToString();
			string Cname = dicPar["Cname"].ToString();
			string BtnCode = dicPar["BtnCode"].ToString();
			string Orders = dicPar["Orders"].ToString();
			string ImgName = dicPar["ImgName"].ToString();
			string Url = dicPar["Url"].ToString();
			string Level = dicPar["Level"].ToString();
			string Descr = dicPar["Descr"].ToString();
            string CCode = dicPar["CCode"].ToString();
            //调用逻辑
            logentity.pageurl ="TB_FunctionsEdit.html";
			logentity.logcontent = "新增系统功能管理信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.Add(GUID, USER_ID, out  Id, BusCode, StoCode, CCname, TStatus, FType, ParentId, Code, Cname, BtnCode, Orders, ImgName, Url, Level, Descr,CCode, entity);
			
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id", "BusCode", "StoCode", "CCname", "TStatus", "FType", "ParentId", "Code", "Cname", "BtnCode", "Orders", "ImgName", "Url", "Level", "Descr", "CCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
			string Id = dicPar["Id"].ToString();
			string BusCode = dicPar["BusCode"].ToString();
			string StoCode = dicPar["StoCode"].ToString();
			string CCname = dicPar["CCname"].ToString();
			string TStatus = dicPar["TStatus"].ToString();
			string FType = dicPar["FType"].ToString();
			string ParentId = dicPar["ParentId"].ToString();
			string Code = dicPar["Code"].ToString();
			string Cname = dicPar["Cname"].ToString();
			string BtnCode = dicPar["BtnCode"].ToString();
			string Orders = dicPar["Orders"].ToString();
			string ImgName = dicPar["ImgName"].ToString();
			string Url = dicPar["Url"].ToString();
			string Level = dicPar["Level"].ToString();
			string Descr = dicPar["Descr"].ToString();
            string CCode = dicPar["CCode"].ToString();
            //调用逻辑
            logentity.pageurl ="TB_FunctionsEdit.html";
			logentity.logcontent = "修改id为:"+Id+"的系统功能管理信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID,  Id, BusCode, StoCode, CCname, TStatus, FType, ParentId, Code, Cname, BtnCode, Orders, ImgName, Url, Level, Descr,CCode, entity);
            
            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string Id = dicPar["Id"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where Id=" + Id);
            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "Id"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string Id = dicPar["Id"].ToString();
            //调用逻辑
			logentity.pageurl ="TB_FunctionsList.html";
			logentity.logcontent = "删除id为:"+Id+"的系统功能管理信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.Delete(GUID, USER_ID, Id, entity);
            ReturnListJson(dt);
        }

		/// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "ids", "tstatus" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ids = dicPar["ids"].ToString();
            string status = dicPar["tstatus"].ToString();

            string Id = dicPar["ids"].ToString().Trim(',');
            logentity.pageurl ="TB_FunctionsList.html";
			logentity.logcontent = "修改状态id为:"+Id+"的系统功能管理信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, Id, status);

            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取二级三级权限（根据一级）
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetFunctionListByParentIdAndStocode(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "parentid", "stocode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string parentid = dicPar["parentid"].ToString();
            string stocode = dicPar["stocode"].ToString();

            dt = bll.GetFunctionListByParentId(GUID, USER_ID, parentid, stocode);
            ReturnListJson(dt);
        }

    }
}