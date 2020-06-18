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
    /// WS_TB_DiscountScheme 折扣方案 的摘要说明
    /// </summary>
    public class WS_TB_DiscountScheme : ServiceBase
    {
        bllTB_DiscountScheme bll = new bllTB_DiscountScheme();
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
                    logentity.module = "折扣方案";
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
                        case "getstolslist":
                            GetStoLSList(dicPar);
                            break;
                        case "getmemcardtype"://获取会员卡
                            GetMemCardType(dicPar);
                            break;
                        case "detailbycode"://根据编号获取详细
                            DetailByCode(dicPar);
                            break;
                        case "getselectuserdiscountlist":
                            GetSelectUserDiscountList(dicPar);
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
            if (filter.Length > 0 && filter != "[]")
            {
                filter = JsonHelper.JsonToFilterByString(filter, out dtFilter);
                if (dtFilter != null)
                {
                    DataRow[] drArr = dtFilter.Select("cus<>''");
                    foreach (DataRow dr in drArr)
                    {
                        string col = dr["cus"].ToString();
                        string fil = dr["filter"].ToString();
                        switch (col)
                        {
                            case "templatetype":
                                switch (fil)
                                {
                                    case "1":
                                        filter += " and len(isnull(InsideCode,''))=0";
                                        break;
                                    case "2":
                                        filter += " and len(isnull(InsideCode,''))>0";
                                        break;
                                }
                                break;
                        }
                    }
                    if (!filter.Contains("StoCode") && !filter.Contains("stocode"))
                    {
                        filter += GetAuthoritywhere("stocode", userid);
                    }
                }
            }
            else
            {
                filter = "where 1=1" + GetAuthoritywhere("stocode", userid);
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
            dt = bll.GetPagingListInfo(GUID, userid, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            if (dt != null && dt.Rows.Count > 0)

            {
                DataTable dtLSDiscountpackage = new DataTable();
                #region 获取连锁端的折扣方案做比对
                StringBuilder postStr = new StringBuilder();
                //获取参数信息
                string busCode = GetCacheToUserBusCode(userid);
                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/IStore/WSdiscountpackage.ashx";
                postStr.Append("actionname=getnewbuslist&parameters={" +
                                               string.Format("\"BusCode\":\"{0}\"", busCode) +
                                       "}");//键值对
                string strStoreJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                if (!string.IsNullOrEmpty(strStoreJson) && strStoreJson.Trim() != "")
                {
                    string code = "";
                    string msg = "";
                    DataSet ds = JsonHelper.NewJsonToDataSet(strStoreJson, out code, out msg);
                    if (code == "0")
                    {
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            dtLSDiscountpackage = ds.Tables[0];
                        }
                    }
                }
                if (dtLSDiscountpackage != null && dtLSDiscountpackage.Rows.Count > 0)//判断连锁端折扣方案是否为空
                {
                    foreach (DataRow dr in dt.Rows)//循环本地的折扣方案
                    {
                        string stocode = dr["InsideCode"].ToString();//取出内部编码，如果不为空，则代表该方案基本信息取连锁端的
                        if (!string.IsNullOrEmpty(stocode) && dtLSDiscountpackage.Select("dispcode='" + stocode + "'").Length > 0)
                        {
                            //将本地存储的信息部分基本信息改为连锁云的
                            DataRow dr_sto = dtLSDiscountpackage.Select("dispcode='" + stocode + "'")[0];
                            dr["LevelCode"] = dr_sto["cracodes"].ToString();//会员卡等级
                            dr["DiscountRate"] = dr_sto["usdiscountrate"].ToString();//折扣率
                            dr["TStatus"] = dr_sto["status"].ToString();//状态

                            dr["SchName"] = dr_sto["dispname"].ToString();//方案名称
                            dr["TStatusName"] = dr_sto["status"].ToString() == "0" ? "无效" : "有效";//状态
                        }
                    }
                }

                #endregion
                DataTable dtStore = GetCacheToStore(userid);
                memcardtypeEntityList meList = GetCacheToMemcardLevel(userid);
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string stocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + stocode + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }
                    }
                }

                #region 取会员卡
                foreach (DataRow dr in dt.Rows)
                {
                    //会员卡
                    if (!string.IsNullOrEmpty(dr["LevelCode"].ToString()) && meList != null && meList.data.Count > 0)
                    {
                        string[] lelist = dr["LevelCode"].ToString().Split(',');
                        foreach (string l in lelist)
                        {
                            if (!string.IsNullOrEmpty(l))
                            {
                                foreach (memcardtypeEntity m in meList.data)
                                {
                                    if (m.levels != null && m.levels.Count > 0)
                                    {
                                        memcardlevelEntity lv = m.levels.SingleOrDefault(a => a.levelcode.ToString() == l);
                                        if (lv != null)
                                        {
                                            dr["LevelCodeName"] += lv.levelname + "、";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dr["LevelCodeName"] = dr["LevelCodeName"].ToString().TrimEnd('、');
                }
                #endregion
                dt.AcceptChanges();
            }
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "BusCode", "StoCode", "CCname", "UCname", "TStatus", "Sort", "PKCode", "InsideCode", "DiscountRate", "MenuCode", "LevelCode", "SchName", "CCode", "UCode", "discountschemerateJson" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string CCname = dicPar["CCname"].ToString();
            string UCname = dicPar["UCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string Sort = dicPar["Sort"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string InsideCode = dicPar["InsideCode"].ToString();
            string DiscountRate = dicPar["DiscountRate"].ToString();
            string MenuCode = dicPar["MenuCode"].ToString();
            string LevelCode = dicPar["LevelCode"].ToString();
            string SchName = dicPar["SchName"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string UCode = dicPar["UCode"].ToString();
            string discountschemerateJson = dicPar["discountschemerateJson"].ToString();
            //调用逻辑
            dt = bll.Add(GUID, userid, out PKCode, BusCode, StoCode, CCname, UCname, TStatus, Sort, InsideCode, DiscountRate, MenuCode, LevelCode, SchName, CCode, UCode, discountschemerateJson);

            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "BusCode", "StoCode", "CCname", "UCname", "TStatus", "Sort", "PKCode", "InsideCode", "DiscountRate", "MenuCode", "LevelCode", "SchName", "CCode", "UCode", "discountschemerateJson" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string CCname = dicPar["CCname"].ToString();
            string UCname = dicPar["UCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string Sort = dicPar["Sort"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string InsideCode = dicPar["InsideCode"].ToString();
            string DiscountRate = dicPar["DiscountRate"].ToString();
            string MenuCode = dicPar["MenuCode"].ToString();
            string LevelCode = dicPar["LevelCode"].ToString();
            string SchName = dicPar["SchName"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string UCode = dicPar["UCode"].ToString();
            string discountschemerateJson = dicPar["discountschemerateJson"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_DiscountSchemeEdit.html";
            logentity.logcontent = "修改id为:" + PKCode + "的折扣方案信息";
            logentity.cuser = StringHelper.StringToLong(userid);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Update(GUID, userid, BusCode, StoCode, CCname, UCname, TStatus, Sort, PKCode, InsideCode, DiscountRate, MenuCode, LevelCode, SchName, CCode, UCode, discountschemerateJson, logentity);

            ReturnListJson(dt);
        }

        private void DetailByCode(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "PKCode", "Type" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            string Type = dicPar["Type"].ToString();
            DataTable dt = new DataTable();
            if (Type == "1")
            {
                dt = bll.GetDiscountSchemeRateByLevelCode(GUID, userid, stocode, PKCode);
            }
            else
            {
                dt = bll.GetDiscountSchemeRateByCode(GUID, userid, stocode, PKCode);
            }

            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "PKCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string PKCode = dicPar["PKCode"].ToString();
            //调用逻辑			
            DataTable dt = bll.GetPagingSigInfo(GUID, userid, "where PKCode='" + PKCode + "'");
            DataTable dtRate = new DataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                dtRate = bll.GetDiscountSchRate(dt.Rows[0]["PKCode"].ToString(), dt.Rows[0]["StoCode"].ToString());
                DataTable dtStore = GetCacheToStore(userid);
                memcardtypeEntityList meList = GetCacheToMemcardLevel(userid);
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string stocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + stocode + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }
                        //会员卡
                        if (!string.IsNullOrEmpty(dr["LevelCode"].ToString()) && meList != null && meList.data.Count > 0)
                        {
                            string[] lelist = dr["LevelCode"].ToString().Split(',');
                            foreach (string l in lelist)
                            {
                                foreach (memcardtypeEntity m in meList.data)
                                {
                                    if (m.levels != null && m.levels.Count > 0)
                                    {
                                        memcardlevelEntity lv = m.levels.SingleOrDefault(a => a.levelcode.ToString() == l);
                                        if (lv != null)
                                        {
                                            dr["LevelCodeName"] += lv.levelname + "、";
                                        }
                                    }
                                }
                            }
                        }
                        dr["LevelCodeName"] = dr["LevelCodeName"].ToString().TrimEnd('、');
                    }
                    dt.AcceptChanges();
                }
            }
            ArrayList arrData = new ArrayList();
            string[] arrTBName = new string[2] { "data1", "data2" };
            arrData.Add(dt);
            arrData.Add(dtRate);
            ReturnListJson("0", "", arrData, arrTBName);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string PKCode = dicPar["id"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_DiscountSchemeList.html";
            logentity.logcontent = "删除id为:" + PKCode + "的折扣方案信息";
            logentity.cuser = StringHelper.StringToLong(userid);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Delete(GUID, userid, PKCode, logentity);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "id", "status" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string ids = dicPar["id"].ToString();
            string status = dicPar["status"].ToString();

            string PKCode = dicPar["id"].ToString().Trim(',');
            logentity.pageurl = "TB_DiscountSchemeList.html";
            logentity.logcontent = "修改状态id为:" + PKCode + "的折扣方案信息";
            logentity.cuser = StringHelper.StringToLong(userid);
            DataTable dt = bll.UpdateStatus(GUID, userid, PKCode, status);

            ReturnListJson(dt);
        }

        /// <summary>
        /// 获取连锁端门店折扣方案信息,不分页
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetStoLSList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息

            List<string> pra = new List<string>() { "BusCode", "StoCode", "userid" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string busCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string userid = dicPar["userid"].ToString();
            memcardtypeEntityList mel = GetCacheToMemcardLevel(userid);
            dt = new blldiscountpackage().GetStoDisCountPackage(busCode, StoCode);
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr["cracodes"].ToString()))
                {
                    string[] cracodes = dr["cracodes"].ToString().Split(',');
                    foreach (string code in cracodes)
                    {
                        if (string.IsNullOrEmpty(code))
                        {
                            continue;
                        }
                        foreach (memcardtypeEntity memcard in mel.data)
                        {
                            if (memcard.levels == null || memcard.levels.Count <= 0)
                            {
                                continue;
                            }
                            memcardlevelEntity m = memcard.levels.SingleOrDefault(a => a.levelcode == code);
                            if (m != null)
                            {
                                dr["CardName"] += m.levelname + ",";
                                break;
                            }
                        }
                    }
                    dr["CardName"] = dr["CardName"].ToString().TrimEnd(',');
                }
            }
            dt.AcceptChanges();
            ReturnListJson(dt);
        }

        //获取会员卡类别等级
        private void GetMemCardType(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "userid" };
            string userid = dicPar["userid"].ToString();
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            memcardtypeEntityList mel = GetCacheToMemcardLevel(userid);
            if (mel == null)
            {
                ToCustomerJson("2", "暂无信息");
            }
            else
            {
                string objjson = JsonHelper.ObjectToJson<memcardtypeEntityList>(mel);
                Pagcontext.Response.Write(objjson);
            }
        }

        /// <summary>
        /// 系统用户获取折扣方案列表-置加载手动折扣
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetSelectUserDiscountList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "page", "limit", "filters", "orders", "stocodes" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string stocodes = dicPar["stocodes"].ToString();
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
                        string col = dr["cus"].ToString();
                        string fil = dr["filter"].ToString();
                        switch (col)
                        {
                            case "":
                                filter += "";
                                break;
                        }
                    }
                }
                filter += "and len(isnull(InsideCode,''))=0";//只加载手动模板
                if (!string.IsNullOrEmpty(stocodes))
                {
                    filter += " and stocode in(";
                    foreach (string stocode in stocodes.Split(','))
                    {
                        filter += "'" + stocode + "',";
                    }
                    filter = filter.TrimEnd(',') + ")";
                }
            }
            else
            {
                filter = "where len(isnull(InsideCode,''))=0";
            }
            filter = GetBusCodeWhere(dicPar, filter, "buscode");

            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, userid, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtStore = GetCacheToStore(userid);
                memcardtypeEntityList meList = GetCacheToMemcardLevel(userid);
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string stocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + stocode + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }

                    }
                }
                dt.AcceptChanges();
            }
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }
    }
}