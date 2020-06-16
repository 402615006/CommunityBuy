using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using System;
using System.Collections;
using System.Text;
using CommunityBuy.IServices;

namespace CommunityBuy.WeiXinSercices
{
    /// <summary>
    /// 商家门店信息接口类
    /// </summary>
    public class WXStore : ServiceBase
    {
        bllWXStore bll = new bllWXStore();
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
                    logentity.module = "商家门店信息";
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
                        case "getwxstodetail"://小程序获取门店详情
                            GetWXStoDetail(dicPar);
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
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stoid", "comcode", "buscode", "stocode", "cname", "sname", "bcode", "indcode", "provinceid", "cityid", "areaid", "address", "stoprincipal", "stoprincipaltel", "tel", "stoemail", "logo", "backgroundimg", "descr", "stourl", "stocoordx", "stocoordy", "netlinklasttime", "calcutime", "busHour", "recommended", "remark", "status", "cuser", "uuser", "btime", "etime", "TerminalNumber", "ValuesDate", "isfood", "pstocode", "sqcode", "storetype", "jprice", };
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
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            string TerminalNumber = dicPar["TerminalNumber"].ToString();
            string ValuesDate = dicPar["ValuesDate"].ToString();
            string isfood = dicPar["isfood"].ToString();
            string pstocode = dicPar["pstocode"].ToString();
            string sqcode = dicPar["sqcode"].ToString();
            string storetype = dicPar["storetype"].ToString();
            string jprice = dicPar["jprice"].ToString();
            //调用逻辑
            logentity.pageurl = "StoreEdit.html";
            logentity.logcontent = "新增商家门店信息信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.Add(GUID, USER_ID, out stoid, comcode, buscode, stocode, cname, sname, bcode, indcode, provinceid, cityid, areaid, address, stoprincipal, stoprincipaltel, tel, stoemail, logo, backgroundimg, descr, stourl, stocoordx, stocoordy, netlinklasttime, calcutime, busHour, recommended, remark, status, cuser, uuser, btime, etime, TerminalNumber, ValuesDate, isfood, pstocode, sqcode, storetype, jprice, entity);

            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stoid", "comcode", "buscode", "stocode", "cname", "sname", "bcode", "indcode", "provinceid", "cityid", "areaid", "address", "stoprincipal", "stoprincipaltel", "tel", "stoemail", "logo", "backgroundimg", "descr", "stourl", "stocoordx", "stocoordy", "netlinklasttime", "calcutime", "busHour", "recommended", "remark", "status", "cuser", "uuser", "btime", "etime", "TerminalNumber", "ValuesDate", "isfood", "pstocode", "sqcode", "storetype", "jprice", };
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
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            string TerminalNumber = dicPar["TerminalNumber"].ToString();
            string ValuesDate = dicPar["ValuesDate"].ToString();
            string isfood = dicPar["isfood"].ToString();
            string pstocode = dicPar["pstocode"].ToString();
            string sqcode = dicPar["sqcode"].ToString();
            string storetype = dicPar["storetype"].ToString();
            string jprice = dicPar["jprice"].ToString();

            //调用逻辑
            logentity.pageurl = "StoreEdit.html";
            logentity.logcontent = "修改id为:" + stoid + "的商家门店信息信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.Update(GUID, USER_ID, stoid, comcode, buscode, stocode, cname, sname, bcode, indcode, provinceid, cityid, areaid, address, stoprincipal, stoprincipaltel, tel, stoemail, logo, backgroundimg, descr, stourl, stocoordx, stocoordy, netlinklasttime, calcutime, busHour, recommended, remark, status, cuser, uuser, btime, etime, TerminalNumber, ValuesDate, isfood, pstocode, sqcode, storetype, jprice, entity);

            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
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
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where stoid=" + stoid);
            ReturnListJson(dt);
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
            logentity.pageurl = "StoreList.html";
            logentity.logcontent = "删除id为:" + stoid + "的商家门店信息信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.Delete(GUID, USER_ID, stoid, entity);
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

            string stoid = dicPar["ids"].ToString().Trim(',');
            logentity.pageurl = "StoreList.html";
            logentity.logcontent = "修改状态id为:" + stoid + "的商家门店信息信息";
            logentity.cuser = Helper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, stoid, status);

            ReturnListJson(dt);
        }

        /// <summary>
        /// 根据门店编号获取门店详情
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetWXStoDetail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "membercode", "phone" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string membercode = dicPar["membercode"].ToString();
            string phone = dicPar["phone"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where s.stocode='" + stocode + "'");
            DataTable dtYDRemark = new DataTable();//获取指定门店的预定备注
            DataTable dtPDCusNum = new DataTable();//可排队的人数
            DataTable dtQueue = new DataTable();//排队信息
            DataTable dtDis = bll.GetTopProduct(stocode);
            if (dtDis != null && dtDis.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDis.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["ImgUrl"].ToString()))
                    {
                        dr["ImgUrl"] = Helper.GetAppSettings("WXStoBackRoot") + dr["ImgUrl"].ToString();
                    }
                }
            }
            //获取轮播图
            foreach (DataRow dr in dt.Rows)
            {
                dr["isdc"] = Helper.GetAppSettings("isAppOrder");//1为开启小程序点单  0为屏蔽小程序点单
                if (!string.IsNullOrEmpty((dr["stopath"].ToString())))
                {
                    string[] path = dr["stopath"].ToString().Split(',');
                    foreach (string p in path)
                    {
                        dr["stopathname"] += Helper.GetAppSettings("WXStoBackRoot") + p + ",";
                    }
                    dr["stopathname"] = dr["stopathname"].ToString().TrimEnd(',');
                }
                if (!string.IsNullOrEmpty(dr["logo"].ToString()))
                {
                    dr["logo"] = Helper.GetAppSettings("WXStoBackRoot") + dr["logo"].ToString();
                }

                string btime = dr["btime"].ToString();
                string etime = dr["etime"].ToString();
                DateTime bdatetime = DateTime.Parse("1900-01-01 " + btime);
                DateTime edatetime = DateTime.Parse("1900-01-01 " + etime);
                DateTime bdatetime1 = DateTime.Parse("1900-01-01 " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                if (bdatetime > edatetime)
                {
                    edatetime = edatetime.AddDays(1);
                }
                if (bdatetime <= bdatetime1 && edatetime >= bdatetime1)
                {
                    dr["sstatus"] = "营业中";
                }
                else
                {
                    dr["sstatus"] = "未营业";
                }
                dr["iscoupon"] = "0";//无优惠券可用,调用肖洋优惠券接口，返回可用优惠券集合


                //获取门店预定设置
                DataTable dtYD = new bllWXsqinfo().GetPresetSettings();
                //获取可排队的门店
                DataTable dtPD = new bllWXsqinfo().GetQueuingSettings();
                if(dtYD!=null)
                {
                    //判断预定
                    if (dtYD.Select("stocode='" + dr["stocode"] + "'").Length > 0)
                    {
                        dr["isyuding"] = "1";//可预定
                    }
                }
                if(dtPD!=null)
                {
                    if (dtPD.Select("stocode='" + dr["stocode"] + "'").Length > 0)
                    {
                        dr["ispaidui"] = "1";//可排队
                    }
                }
               
                //可排队的人数选择
                dtPDCusNum = new bllWXsqinfo().GetQueuing(stocode);
                if(dtPDCusNum==null)
                {
                    dtPDCusNum = new DataTable();
                }
                //获取指定门店的预定备注
                dtYDRemark = new bllWXsqinfo().GetReservationRemark(stocode);
                if(dtYDRemark==null)
                {
                    dtYDRemark = new DataTable();
                }
                //如果有排队获取用户排队信息
                dtQueue = new bllWXsqinfo().GetQueue(stocode, phone);
            }

            dt.AcceptChanges();
            ArrayList arrData = new ArrayList();
            string[] arrTBName = new string[5] { "data1", "data2", "data3", "data4", "data5" };
            arrData.Add(dt);
            arrData.Add(dtYDRemark);
            arrData.Add(dtPDCusNum);
            if(dtQueue==null)
            {
                dtQueue = new DataTable();
            }
            arrData.Add(dtQueue);
            if(dtDis==null)
            {
                dtDis = new DataTable();
            }
            arrData.Add(dtDis);
            ReturnListJson("0", "", arrData, arrTBName);
        }

        

    }
}