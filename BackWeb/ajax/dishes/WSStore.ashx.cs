using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.ajax.dishes
{
    /// <summary>
    /// WSStore 的摘要说明
    /// </summary>
    public class WSStore : ServiceBase
    {
        bllStore bll = new bllStore();
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
                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "pageSize", "currentPage", "filter", "order" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}

            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            int pageSize = StringHelper.StringToInt(dicPar["pageSize"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["currentPage"].ToString());

            string filter = dicPar["filter"] == null ? "" : dicPar["filter"].ToString(); //(dicPar["filter"].ToString()).Replace('’', '\'').Replace('‘', '\'');
            //查询门店编号为传递过来的条件  预定状态为1 预定类型包含3的数据
            string sql = string.Format(" buscode ='{0}' ", Helper.GetAppSettings("BusCode"));
            if (filter.Length == 0)
            {
                filter = sql;
            }
            else
            {
                filter = filter + " and" + sql;
            }
            string order = dicPar["order"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stoid", "comcode", "buscode", "stocode", "cname", "sname", "bcode", "indcode", "provinceid", "cityid", "areaid", "address", "stoprincipal", "stoprincipaltel", "tel", "stoemail", "logo", "backgroundimg", "descr", "stourl", "stocoordx", "stocoordy", "netlinklasttime", "calcutime", "busHour", "recommended", "remark", "status", "cuser", "uuser", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stoid = dicPar["stoid"].ToString();
            string comcode = dicPar["comcode"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string cname = dicPar["cname"].ToString();
            string sname = dicPar["sname"].ToString();
            string bcode = dicPar["bcode"].ToString();
            string indcode = dicPar["indcode"].ToString();
            string provinceid = dicPar["provinceid"].ToString();
            string cityid = dicPar["cityid"].ToString();
            string areaid = dicPar["areaid"].ToString();
            string address = dicPar["address"].ToString();
            string stoprincipal = dicPar["stoprincipal"].ToString();
            string stoprincipaltel = dicPar["stoprincipaltel"].ToString();
            string tel = dicPar["tel"].ToString();
            string stoemail = dicPar["stoemail"].ToString();
            string logo = dicPar["logo"].ToString();
            string backgroundimg = dicPar["backgroundimg"].ToString();
            string descr = dicPar["descr"].ToString();
            string stourl = dicPar["stourl"].ToString();
            string stocoordx = dicPar["stocoordx"].ToString();
            string stocoordy = dicPar["stocoordy"].ToString();
            string netlinklasttime = dicPar["netlinklasttime"].ToString();
            string calcutime = dicPar["calcutime"].ToString();
            string busHour = dicPar["busHour"].ToString();
            string recommended = dicPar["recommended"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string cuser = dicPar["cuser"].ToString();
            string uuser = dicPar["uuser"].ToString();
            //调用逻辑
            bll.Add(GUID, USER_ID, stoid, comcode, buscode, stocode, cname, sname, bcode, indcode, provinceid, cityid, areaid, address, stoprincipal, stoprincipaltel, tel, stoemail, logo, backgroundimg, "", "", descr, stourl, stocoordx, stocoordy, recommended, remark, status, cuser,  "", "");
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stoid", "comcode", "buscode", "stocode", "cname", "sname", "bcode", "indcode", "provinceid", "cityid", "areaid", "address", "stoprincipal", "stoprincipaltel", "tel", "stoemail", "logo", "backgroundimg", "descr", "stourl", "stocoordx", "stocoordy", "netlinklasttime", "calcutime", "busHour", "recommended", "remark", "status", "cuser", "uuser", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stoid = dicPar["stoid"].ToString();
            string comcode = dicPar["comcode"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string cname = dicPar["cname"].ToString();
            string sname = dicPar["sname"].ToString();
            string bcode = dicPar["bcode"].ToString();
            string indcode = dicPar["indcode"].ToString();
            string provinceid = dicPar["provinceid"].ToString();
            string cityid = dicPar["cityid"].ToString();
            string areaid = dicPar["areaid"].ToString();
            string address = dicPar["address"].ToString();
            string stoprincipal = dicPar["stoprincipal"].ToString();
            string stoprincipaltel = dicPar["stoprincipaltel"].ToString();
            string tel = dicPar["tel"].ToString();
            string stoemail = dicPar["stoemail"].ToString();
            string logo = dicPar["logo"].ToString();
            string backgroundimg = dicPar["backgroundimg"].ToString();
            string descr = dicPar["descr"].ToString();
            string stourl = dicPar["stourl"].ToString();
            string stocoordx = dicPar["stocoordx"].ToString();
            string stocoordy = dicPar["stocoordy"].ToString();
            string netlinklasttime = dicPar["netlinklasttime"].ToString();
            string busHour = dicPar["busHour"].ToString();
            string recommended = dicPar["recommended"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string cuser = dicPar["cuser"].ToString();
            string uuser = dicPar["uuser"].ToString();

            //调用逻辑
            StoreEntity UEntity = bll.GetEntitySigInfo("where id="+ stoid);
            UEntity.address = address;
            UEntity.areaid =StringHelper.StringToInt(areaid);
            UEntity.backgroundimg = backgroundimg;
            UEntity.bcode = bcode;
            UEntity.buscode = buscode;
            UEntity.cityid = StringHelper.StringToInt(cityid);
            UEntity.cname = cname;
            UEntity.comcode = comcode;
            UEntity.descr = descr;

            UEntity.indcode = indcode;
            UEntity.logo = logo;
            UEntity.provinceid = StringHelper.StringToInt(provinceid);
            UEntity.recommended = recommended;
            UEntity.remark = remark;
            UEntity.sname = sname;
            UEntity.status = status;
            UEntity.stocode = stocode;
            UEntity.stocoordx = stocoordx;
            UEntity.stocoordy = stocoordy;
            UEntity.stoemail = stoemail;
            UEntity.stoprincipal = stoprincipal;
            UEntity.stoprincipaltel = stoprincipaltel;
            UEntity.stourl = stourl;
            UEntity.tel = tel;
            bll.Update(GUID, USER_ID, UEntity);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "stocode='" + stocode + "'");
            ReturnListJson(dt,1,1,1,1);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stoid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stoid = dicPar["stoid"].ToString();
            //调用逻辑

            bll.Delete(GUID, USER_ID, stoid);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
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
            string status = dicPar["status"].ToString();

            string stoid = dicPar["id"].ToString().Trim(',');
            bll.UpdateStatus(GUID, USER_ID, stoid, status);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }
    }
}