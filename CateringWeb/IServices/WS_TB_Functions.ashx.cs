using CommunityBuy.BLL;
using CommunityBuy.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CommunityBuy.CommonBasic;
using System.Collections;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_TB_Functions 系统功能 的摘要说明
    /// </summary>
    public class WS_TB_Functions : ServiceBase
    {
        bllTB_Functions bll = new bllTB_Functions();
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
            List<string> pra = new List<string>() { "GUID", "userid", "page", "limit", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            int pageSize = StringHelper.StringToInt(dicPar["limit"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
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
            dt = bll.GetPagingListInfo(GUID, userid, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt,pageSize, recordCount, currentPage,totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "Id", "BusCode", "StoCode", "CCname", "TStatus", "FType", "ParentId", "Code", "Cname", "BtnCode", "Orders", "ImgName", "Url", "Level", "Descr", "CCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
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
            bll.Add(GUID, userid, Id, BusCode, StoCode, CCname, TStatus, FType, ParentId, Code, Cname, BtnCode, Orders, ImgName, Url, Level, Descr, CCode);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "Id", "BusCode", "StoCode", "CCname", "TStatus", "FType", "ParentId", "Code", "Cname", "BtnCode", "Orders", "ImgName", "Url", "Level", "Descr", "CCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
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
            TB_FunctionsEntity UEntity = bll.GetEntitySigInfo(" where id="+ Id);
            UEntity.Cname = Cname;


             bll.Update(GUID, userid, UEntity);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "Id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string Id = dicPar["Id"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, userid, "where Id=" + Id);
            ReturnListJson(dt,null,null,null,null);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "Id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string Id = dicPar["Id"].ToString();
            //调用逻辑

            bll.Delete(GUID, userid, Id);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "ids", "tstatus" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string ids = dicPar["ids"].ToString();
            string status = dicPar["tstatus"].ToString();

            string Id = dicPar["ids"].ToString().Trim(',');
            TB_FunctionsEntity UEntity = bll.GetEntitySigInfo(" where id=" + ids);
            UEntity.TStatus = status;

            bll.Update(GUID, userid, UEntity);

            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        /// <summary>
        /// 获取二级三级权限（根据一级）
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetFunctionListByParentIdAndStocode(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "parentid"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string parentid = dicPar["parentid"].ToString();
            string stocode = string.Empty;
            dt = bll.GetFunctionListByParentId(GUID, userid, parentid, stocode);
            ReturnListJson(dt,null,null,null,null);
        }
    }
}