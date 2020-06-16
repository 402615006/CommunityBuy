using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.IServices
{
    /// <summary>
    /// 系统用户操作日志接口类
    /// </summary>
    public class WS_tl_operatelog : ServiceBase
    {
        blltl_operatelog bll = new blltl_operatelog();
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
                    logentity.module = "系统用户操作日志";
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
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "page", "limit", "filters", "orders" };

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
            DataTable dtemp = new bllEmployee().GetAllEmp();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["cusername"] = "";
                    string auserid = dr["opeuserid"].ToString();
                    if (dtemp.Select("userid='" + auserid + "'").Length > 0)
                    {
                        DataRow[] rdrs = dtemp.Select("userid='" + auserid + "'");
                        foreach (DataRow rdr in rdrs)
                        {
                            dr["cusername"] = dtemp.Select("userid='" + auserid + "'")[0]["empcodename"].ToString();
                        }
                    }
                }
                dt.AcceptChanges();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("otypeName", typeof(string));//操作类型
                foreach (DataRow dr in dt.Rows)
                {
                    dr["otypeName"] = Helper.GetEnumNameByValue(typeof(SystemEnum.LogOperateType), dr["otype"].ToString());//操作类型
                }
            }
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "opeid", "buscode", "strcode", "module", "pageurl", "otype", "logcontent", "ip", "opeuserid", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string opeid = dicPar["opeid"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string strcode = dicPar["strcode"].ToString();
            string module = dicPar["module"].ToString();
            string pageurl = dicPar["pageurl"].ToString();
            string otype = dicPar["otype"].ToString();
            string logcontent = dicPar["logcontent"].ToString();
            string ip = dicPar["ip"].ToString();
            string opeuserid = dicPar["opeuserid"].ToString();
            //调用逻辑
            logentity.pageurl = "tl_operatelogEdit.html";
            logentity.logcontent = "新增系统用户操作日志信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.Add(GUID, USER_ID, out opeid, buscode, strcode, module, pageurl, otype, logcontent, ip, opeuserid, entity);

            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "opeid", "buscode", "strcode", "module", "pageurl", "otype", "logcontent", "ip", "opeuserid", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string opeid = dicPar["opeid"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string strcode = dicPar["strcode"].ToString();
            string module = dicPar["module"].ToString();
            string pageurl = dicPar["pageurl"].ToString();
            string otype = dicPar["otype"].ToString();
            string logcontent = dicPar["logcontent"].ToString();
            string ip = dicPar["ip"].ToString();
            string opeuserid = dicPar["opeuserid"].ToString();

            //调用逻辑
            logentity.pageurl = "tl_operatelogEdit.html";
            logentity.logcontent = "修改id为:" + opeid + "的系统用户操作日志信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID, opeid, buscode, strcode, module, pageurl, otype, logcontent, ip, opeuserid, entity);

            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "opeid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string opeid = dicPar["opeid"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where opeid=" + opeid);
            StringBuilder postStr = new StringBuilder();
            string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/WSadmins.ashx";
            postStr.Append("actionname=getempstolist&parameters={" +
                                           string.Format("\"GUID\":\"{0}\"", GUID) +
                                           string.Format(",\"USER_ID\": \"{0}\"", USER_ID) +
                                           string.Format(",\"page\": \"{0}\"", 5000) +
                                           string.Format(",\"limit\": \"{0}\"", 1) +
                                           string.Format(",\"filters\": \"{0}\"", string.Empty) +
                                           string.Format(",\"orders\":\"{0}\"", string.Empty) +
                                   "}");//键值对
            string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
            if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
            {
                string code = "";
                string msg = "";
                string limit = "";
                string count = "";
                string curpage = "";
                string totpage = "";
                DataSet ds = JsonHelper.NewLinJsonToDataSet(strAdminJson, out code, out msg, out limit, out count, out curpage, out totpage);
                if (code == "0")
                {
                    if (dt != null && ds.Tables[0].Rows.Count > 0 && ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["cusername"] = "";
                            string auserid = dr["cuser"].ToString();
                            if (ds.Tables[0].Select("userid='" + auserid + "'").Length > 0)
                            {
                                DataRow[] rdrs = ds.Tables[0].Select("userid='" + auserid + "'");
                                foreach (DataRow rdr in rdrs)
                                {
                                    dr["cusername"] = ds.Tables[0].Select("userid='" + auserid + "'")[0]["realname"].ToString();
                                }
                            }
                        }
                        dt.AcceptChanges();
                    }
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("otypeName", typeof(string));//操作类型
                foreach (DataRow dr in dt.Rows)
                {
                    dr["otypeName"] = Helper.GetEnumNameByValue(typeof(SystemEnum.LogOperateType), dr["otype"].ToString());//操作类型
                }
            }
            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "opeid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string opeid = dicPar["opeid"].ToString();
            //调用逻辑
            logentity.pageurl = "tl_operatelogList.html";
            logentity.logcontent = "删除id为:" + opeid + "的系统用户操作日志信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.Delete(GUID, USER_ID, opeid, entity);
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

            string opeid = dicPar["ids"].ToString().Trim(',');
            logentity.pageurl = "tl_operatelogList.html";
            logentity.logcontent = "修改状态id为:" + opeid + "的系统用户操作日志信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, opeid, status);

            ReturnListJson(dt);
        }
    }
}