using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.IServices;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.WeiXinSercices
{
	/// <summary>
    /// 美食-首页
    /// </summary>
    public class WXsqinfo : ServiceBase
    {
        bllWXsqinfo bll = new bllWXsqinfo();
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
					logentity.module = "";
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
                        case "getgpslist"://更具定位获取
                            GetGPSList(dicPar);
                            break;
                        case "getnewgpslist"://新更具定位获取
                            GetNewGPSList(dicPar);
                            break;
                        case "getsqstolist":
                            GetSQStoList(dicPar);
                            break;
                        case "getnewsqstolist":
                            GetNewSQStoList(dicPar);//新
                            break;
                        case "getunimages"://获取模块轮播图
                            GetUnImage(dicPar);
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
            List<string> pra = new List<string>() { "GUID", "USER_ID", "id", "sqcode", "sqname", "city", "jwcodes", "cuser", "status", "uuser", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
			string id = dicPar["id"].ToString();
			string sqcode = dicPar["sqcode"].ToString();
			string sqname = dicPar["sqname"].ToString();
			string city = dicPar["city"].ToString();
			string jwcodes = dicPar["jwcodes"].ToString();
			string cuser = dicPar["cuser"].ToString();
			string status = dicPar["status"].ToString();
			string uuser = dicPar["uuser"].ToString();
            //调用逻辑
			logentity.pageurl ="sqinfoEdit.html";
			logentity.logcontent = "新增信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.Add(GUID, USER_ID, out  id, sqcode, sqname, city, jwcodes, cuser, status, uuser, entity);
			
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "id", "sqcode", "sqname", "city", "jwcodes", "cuser", "status", "uuser", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
			string id = dicPar["id"].ToString();
			string sqcode = dicPar["sqcode"].ToString();
			string sqname = dicPar["sqname"].ToString();
			string city = dicPar["city"].ToString();
			string jwcodes = dicPar["jwcodes"].ToString();
			string cuser = dicPar["cuser"].ToString();
			string status = dicPar["status"].ToString();
			string uuser = dicPar["uuser"].ToString();

            //调用逻辑
			logentity.pageurl ="sqinfoEdit.html";
			logentity.logcontent = "修改id为:"+id+"的信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID,  id, sqcode, sqname, city, jwcodes, cuser, status, uuser, entity);
            
            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string id = dicPar["id"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where id=" + id);
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
            string id = dicPar["id"].ToString();
            //调用逻辑
			logentity.pageurl ="sqinfoList.html";
			logentity.logcontent = "删除id为:"+id+"的信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
			logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.Delete(GUID, USER_ID, id, entity);
            ReturnListJson(dt);
        }

		/// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "ids", "status" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ids = dicPar["ids"].ToString();
            string status = dicPar["status"].ToString();

            string id = dicPar["ids"].ToString().Trim(',');
            logentity.pageurl ="sqinfoList.html";
			logentity.logcontent = "修改状态id为:"+id+"的信息";
			logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, id, status);

            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取商圈信息
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetGPSList(Dictionary<string, object> dicPar)
        {
            dt = bll.GetGPSList();
            foreach(DataRow dr in dt.Rows)
            {
                if(!string.IsNullOrEmpty(dr["jwcodes"].ToString()))
                {
                    string code = dr["jwcodes"].ToString();
                    dr["jcode"] = code.Split(',')[0];
                    dr["wcode"] = code.Split(',')[1];
                }
            }
            dt.AcceptChanges();
            ReturnListJson(dt);
        }

        /// <summary>
        /// 根据类型获取商圈信息
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetNewGPSList(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "typecode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string typecode = dicPar["typecode"].ToString();
            dt = bll.GetNewGPSList(typecode);
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr["jwcodes"].ToString()))
                {
                    string code = dr["jwcodes"].ToString();
                    if(code.Split(',').Length==2)
                    {
                        dr["jcode"] = code.Split(',')[0];
                        dr["wcode"] = code.Split(',')[1];
                    }
                }
            }
            dt.AcceptChanges();
            ReturnListJson(dt);
        }

        private void GetSQStoList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() {"page", "limit", "filters"};

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            int pageSize = Helper.StringToInt(dicPar["limit"].ToString());
            int currentPage = Helper.StringToInt(dicPar["page"].ToString());
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0 && filter != "[]" &&filter!="\"\"")
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
                filter += " and s.status='1'";
            }
            else
            {
                filter = " where s.status='1'";
            }
            string order = string.Empty;

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetSQStoList(pageSize, currentPage, filter, order, out recordCount, out totalPage);
            //获取门店预定设置
            DataTable dtYD = bll.GetPresetSettings();
            //获取可排队的门店
            DataTable dtPD = bll.GetQueuingSettings();
            foreach(DataRow dr in dt.Rows)
            {
                dr["isdc"] = Helper.GetAppSettings("isAppOrder");//1为开启小程序点单  0为屏蔽小程序点单
                //判断预定
                if (dtYD.Select("stocode='"+dr["stocode"]+"'").Length>0)
                {
                    dr["isyuding"] = "1";
                }
                if (dtPD.Select("stocode='" + dr["stocode"] + "'").Length > 0)
                {
                    dr["ispaidui"] = "1";
                }
                if(!string.IsNullOrEmpty(dr["logo"].ToString()))
                {
                    dr["logo"] = Helper.GetAppSettings("WXStoBackRoot") + dr["logo"].ToString();
                }
            }
            dt.AcceptChanges();

            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void GetNewSQStoList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "page", "limit", "filters", "x", "y" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            int pageSize = Helper.StringToInt(dicPar["limit"].ToString());
            int currentPage = Helper.StringToInt(dicPar["page"].ToString());
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            string x = dicPar["x"].ToString();//经度
            string y = dicPar["y"].ToString();//纬度
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0 && filter != "[]" && filter != "\"\"")
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
                filter += " and s.status='1'";
            }
            else
            {
                filter = " where s.status='1'";
            }
            string order = string.Empty;

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetNewSQStoList(pageSize, currentPage, filter, order, out recordCount, out totalPage, x, y);
            //获取门店预定设置
            DataTable dtYD = bll.GetPresetSettings();
            //获取可排队的门店
            DataTable dtPD = bll.GetQueuingSettings();
            foreach (DataRow dr in dt.Rows)
            {
                dr["isdc"] = Helper.GetAppSettings("isAppOrder");//1为开启小程序点单  0为屏蔽小程序点单
                //判断预定
                if (dtYD.Select("stocode='" + dr["stocode"] + "'").Length > 0)
                {
                    dr["isyuding"] = "1";
                }
                if (dtPD.Select("stocode='" + dr["stocode"] + "'").Length > 0)
                {
                    dr["ispaidui"] = "1";
                }
                if (!string.IsNullOrEmpty(dr["logo"].ToString()))
                {
                    dr["logo"] = Helper.GetAppSettings("WXStoBackRoot") + dr["logo"].ToString();
                }
            }
            dt.AcceptChanges();

            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        /// <summary>
        /// 根据不同的模块获取首页的轮播图
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetUnImage(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "modelcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string modelcode = dicPar["modelcode"].ToString();
            DataTable dt = bll.GetUnImage(GUID, USER_ID, modelcode);
            foreach(DataRow dr in dt.Rows)
            {
                if(!string.IsNullOrEmpty(dr["smallimg"].ToString()))
                {
                    dr["smallimg"] = Helper.GetAppSettings("StoBackRoot") + "/uploads/movie/" + dr["smallimg"].ToString();
                }
            }
            dt.AcceptChanges();
            ReturnListJson(dt);
        }

    }
}