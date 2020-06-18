using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.IServices
{
    /// <summary>
    /// 菜品信息接口类
    /// </summary>
    public class WS_TB_Dish : ServiceBase
    {
        bllTB_Dish bll = new bllTB_Dish();
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
                    logentity.module = "菜品信息";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "getlistbyservice"://列表
                            GetListForIService(dicPar);
                            break;
                        case "add"://添加							
                            Add(dicPar);
                            break;
                        case "detail"://详细
                            Detail(dicPar);
                            break;
                        case "webdetail"://详细
                            WebDetail(dicPar);
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
                        case "getdishtypeppkdata"://获取一级菜品类别
                            GetDishTypePPKData(dicPar);
                            break;
                        case "getdishtypeppktopkdata"://根据一级菜品类别获取旗下二级类别
                            GetDishTypePPKToPKData(dicPar);
                            break;
                        case "getdishselectalldata"://通过门店编号获取菜品一级类别、菜谱、财务类别信息集
                            GetDishSelectAllData(dicPar);
                            break;
                        case "getadddishselectalldata"://添加菜品或修改菜品用,通过门店编号获取菜谱、财务类别、仓库、厨房信息集合
                            GetAddDishSelectAllData(dicPar);
                            break;
                        case "getoutdepcodetokitandemp"://获取制作厨师
                            GetOutDepCodeToKitAndEmp(dicPar);
                            break;
                        case "getstockmateclass"://获取原物料类别树
                            GetStockMateClass(dicPar);
                            break;
                        case "getstockmaterial"://查询原物料
                            GetStockMaterial(dicPar);
                            break;
                        case "uploaddishimages":
                            UpLoadDishImages(context);
                            break;
                        case "downtemp":
                            GetImportTemplate(dicPar);
                            break;
                        case "dishesexport":
                            DishesExport(dicPar);
                            break;
                        case "uploaddishesbytemplate"://导入菜品数据校正
                            UploadDishesByTemplate(dicPar);
                            break;
                        case "uploaddishesbytemplatepost"://正常导入菜品
                            UploadDishesByTemplatePost(dicPar);
                            break;
                        case "getcombotypetodish":
                            GetComboTypeToDish(dicPar);
                            break;
                        case "getfintype":
                            GetFinType(dicPar);
                            break;
                        case "getdishebycodes":
                            GetDisheByCodes(dicPar);
                            break;
                        case "outdishstock"://菜品出库
                            DishOutStock(dicPar);
                            break;
                        case "indishstock"://菜品入库
                            DishInStock(dicPar);
                            break;
                        case "getdisheinfo":
                            GetDisheInfo(dicPar);
                            break;
                        case "getdishesbyname":
                            GetDishesByName(dicPar);
                            break;
                        case "getdisheinfonew":
                            GetDisheInfoNew(dicPar);
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
            string userid = dicPar["userid"].ToString();
            int pageSize = StringHelper.StringToInt(dicPar["limit"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            string depart = "";
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            string opencode = "";
            object opencodeObj = "";
            string channel = "";
            dicPar.TryGetValue("opencode", out opencodeObj);
            if (opencodeObj != null)
            {
                opencode = opencodeObj.ToString();
            }
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0 && filter != "[]")
            {
                filter = JsonHelper.JsonToFilterByString(filter, out dtFilter);
                if (!filter.Contains("StoCode") && !filter.Contains("stocode"))
                {
                    filter += GetAuthoritywhere("dis.stocode", userid);
                }
                if (dtFilter != null)
                {
                    DataRow[] drArr = dtFilter.Select("cus<>''");
                    foreach (DataRow dr in drArr)
                    {
                        string col = dr["col"].ToString();
                        switch (col)
                        {
                            case "sing":
                                string singCode = dr["filter"].ToString();
                                filter += " and (dis.DisCode in(select DisCode from TR_SignDish where SchCode='" + singCode + "') or dis.TypeCode in(select DisTypeCode from TR_SignDish where SchCode='" + singCode + "'))";
                                break;
                            case "dis.DisName":
                                string filter1 = filter.Replace("where", "");
                                filter1 = filter1.Replace("DisName", "QuickCode");
                                filter += " or (" + filter1 + ")";
                                filter += " or (" + filter1.Replace("QuickCode", "CusDisCode") + ")";
                                filter += " or (" + filter1.Replace("QuickCode", "QRCode") + ")";
                                break;
                            case "Depart":
                                depart = dr["filter"].ToString();
                                break;
                            case "Channel":
                                channel = dr["filter"].ToString();
                                break;
                            case "TypeCode":
                                string type = dr["cus"].ToString();
                                string value = dr["filter"].ToString();
                                if (type == "1")
                                {
                                    filter += " and (dis.typecode='" + value + "' or dis.typecode in(select PKCode from [dbo].[TB_DishType] where PKKCode='" + value + "'))";
                                }
                                else
                                {
                                    filter += " and dis.typecode='" + value + "'";
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                filter = "where 1=1" + GetAuthoritywhere("dis.stocode", userid);
            }
            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            if (string.IsNullOrEmpty(order))
            {
                order = " order by dis.ctime desc";
            }
            if (!string.IsNullOrWhiteSpace(depart))
            {
                filter += " and dis.MenuCode in(select PKCode from TB_DishMenu where IsDefault='1' and stocode=dis.stocode and DepCode='" + depart + "') ";
            }
            if (!string.IsNullOrWhiteSpace(channel))
            {
                filter += " and charindex('" + channel + "',dis.ChannelCodeList)>0 ";
            }


            if (!dicPar.ContainsKey("HT"))//门店后台独有参数
            {
                if (string.IsNullOrWhiteSpace(depart))
                {
                    filter += " and dis.MenuCode in(select PKCode from TB_DishMenu where IsDefault='1' and stocode=dis.stocode) ";
                }
                else
                {
                    filter += " and dis.MenuCode in(select PKCode from TB_DishMenu where IsDefault='1' and stocode=dis.stocode and DepCode='" + depart + "') ";
                }
            }
            filter = GetBusCodeWhere(dicPar, filter, "dis.buscode");

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, opencode, out recordCount, out totalPage);
            if (dt != null && dt.Rows.Count > 0)
            {

                #region 门店名称
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
                        if (!string.IsNullOrEmpty(dr["ChannelCodeList"].ToString()))
                        {
                            foreach (string ccode in dr["ChannelCodeList"].ToString().Split(','))
                            {
                                if (!string.IsNullOrEmpty(ccode))
                                {
                                    if (!string.IsNullOrEmpty(ccode))
                                    {
                                        dr["ChannelCodeListName"] += dtchannel.Select("enumcode='" + ccode + "'")[0]["enumname"] + ",";
                                    }
                                }
                            }
                            dr["ChannelCodeListName"] = dr["ChannelCodeListName"].ToString().TrimEnd(',');
                        }

                        //ChannelCodeListName

                    }
                }
                #endregion
                #region 制作厨师
                DataTable dtEmp = GetCacheToEmployee(userid);
                if (dtEmp != null && dtEmp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string empCode = dr["CookerCode"].ToString();
                        if (dtEmp.Select("ecode='" + empCode + "'").Length > 0)
                        {
                            DataRow dr_emp = dtEmp.Select("ecode='" + empCode + "'")[0];
                            dr["CookerCodeName"] = dr_emp["cname"].ToString();
                        }
                    }
                }
                #endregion
                #region 财务类别
                DataTable dtFinType = GetCacheToFinType(userid);
                if (dtFinType != null && dtFinType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string fincode = dr["FinCode"].ToString();
                        if (dtFinType.Select("fincode='" + fincode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtFinType.Select("fincode='" + fincode + "'")[0];
                            dr["FinTypeName"] = dr_sto["finname" +
                                ""].ToString();
                        }
                    }
                }
                #endregion
                #region 原物料
                DataTable dtMate = new bllStockMaterial().GetStockMaterial(string.Empty, GetCacheToUserBusCode(userid), string.Empty);
                if (dtMate != null && dtMate.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string MatCode = dr["MatCode"].ToString();
                        if (dtMate.Select("matcode='" + MatCode + "'").Length > 0)
                        {
                            DataRow dr_mat = dtMate.Select("matcode='" + MatCode + "'")[0];
                            dr["MatCodeName"] = dr_mat["matname"].ToString();
                        }
                    }
                }
                #endregion
                #region 仓库
                DataTable dtWareHouse = new bllStockWareHouse().GetStoWareHouseAllList("where 1=1 " + GetAuthoritywhere("stocode", userid));
                if (dtWareHouse != null && dtWareHouse.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string stocode = dr["StoCode"].ToString();
                        string WarCode = dr["WarCode"].ToString();
                        if (dtWareHouse.Select("stocode='" + stocode + "' and warcode='" + WarCode + "'").Length > 0)
                        {
                            DataRow dr_war = dtWareHouse.Select("stocode='" + stocode + "' and warcode='" + WarCode + "'")[0];
                            dr["WarCodeName"] = dr_war["warname"].ToString();
                        }
                    }
                }
                #endregion
                dt.AcceptChanges();
            }
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void GetListForIService(Dictionary<string, object> dicPar)
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
            string userid = dicPar["userid"].ToString();
            int pageSize = StringHelper.StringToInt(dicPar["limit"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            string depart = "";
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            string opencode = "";
            object opencodeObj = "";
            string channel = "";
            dicPar.TryGetValue("opencode", out opencodeObj);
            if (opencodeObj != null)
            {
                opencode = opencodeObj.ToString();
            }
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0 && filter != "[]")
            {
                filter = JsonHelper.JsonToFilterByString(filter, out dtFilter);
                if (!filter.Contains("StoCode") && !filter.Contains("stocode"))
                {
                    filter += GetAuthoritywhere("dis.stocode", userid);
                }
                if (dtFilter != null)
                {
                    DataRow[] drArr = dtFilter.Select("cus<>''");
                    foreach (DataRow dr in drArr)
                    {
                        string col = dr["col"].ToString();
                        switch (col)
                        {
                            case "sing":
                                string singCode = dr["filter"].ToString();
                                filter += " and (dis.DisCode in(select DisCode from TR_SignDish where SchCode='" + singCode + "') or dis.TypeCode in(select DisTypeCode from TR_SignDish where SchCode='" + singCode + "'))";
                                break;
                            case "DisName":
                                string filter1 = filter.Replace("where", "");
                                filter1 = filter1.Replace("DisName", "QuickCode");
                                filter += " or (" + filter1 + ")";
                                filter += " or (" + filter1.Replace("QuickCode", "CusDisCode") + ")";
                                break;
                            case "Depart":
                                depart = dr["filter"].ToString();
                                break;
                            case "Channel":
                                channel = dr["filter"].ToString();
                                break;
                            case "TypeCode":
                                string type = dr["cus"].ToString();
                                string value = dr["filter"].ToString();
                                if (type == "1")
                                {
                                    filter += " and (dis.typecode='" + value + "' or dis.typecode in(select PKCode from [dbo].[TB_DishType] where PKKCode='" + value + "'))";
                                }
                                else
                                {
                                    filter += " and dis.typecode='" + value + "'";
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                filter = "where 1=1" + GetAuthoritywhere("dis.stocode", userid);
            }
            filter = GetBusCodeWhere(dicPar, filter, "dis.buscode");

            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            if (string.IsNullOrEmpty(order))
            {
                order = " order by dis.ctime desc";
            }
            if (!string.IsNullOrWhiteSpace(depart))
            {
                filter += " and dis.MenuCode in(select PKCode from TB_DishMenu where IsDefault='1' and stocode=dis.stocode and DepCode='" + depart + "') ";
            }
            if (!string.IsNullOrWhiteSpace(channel))
            {
                filter += " and charindex('" + channel + "',dis.ChannelCodeList)>0 ";
            }


            if (!dicPar.ContainsKey("HT"))//门店后台独有参数
            {
                if (string.IsNullOrWhiteSpace(depart))
                {
                    filter += " and dis.MenuCode in(select PKCode from TB_DishMenu where IsDefault='1' and stocode=dis.stocode) ";
                }
                else
                {
                    filter += " and dis.MenuCode in(select PKCode from TB_DishMenu where IsDefault='1' and stocode=dis.stocode and DepCode='" + depart + "') ";
                }
            }

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, opencode, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "BusCode", "StoCode", "CCname", "UCname", "TStatus", "ChannelCodeList", "DisCode", "DisName", "OtherName", "TypeCode", "QuickCode", "CusDisCode", "Unit", "Price", "MenuCode", "MemPrice", "CostPrice", "RoyMoney", "ExtCode", "FinCode", "KitCode", "CookerCode", "MakeTime", "QRCode", "WarCode", "MatCode", "Descript", "IsCount", "DefCount", "CountPrice", "IsVarPrice", "IsWeight", "IsMethod", "IsStock", "IsPoint", "IsMemPrice", "IsCoupon", "IsKeep", "IsCombo", "CCode", "UCode", "ImageName", "dishesMethodsJson", "dishescombosJson", "dishescomboinfoJson" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string userid = dicPar["userid"].ToString();
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string CCname = dicPar["CCname"].ToString();
            string UCname = dicPar["UCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string ChannelCodeList = dicPar["ChannelCodeList"].ToString();
            if (ChannelCodeList == "0")
            {
                ChannelCodeList = "1,2";
            }
            string DisCode = dicPar["DisCode"].ToString();
            string DisName = dicPar["DisName"].ToString();
            string OtherName = dicPar["OtherName"].ToString();
            string TypeCode = dicPar["TypeCode"].ToString();
            string QuickCode = dicPar["QuickCode"].ToString();
            string CusDisCode = dicPar["CusDisCode"].ToString();
            string Unit = dicPar["Unit"].ToString();
            string Price = dicPar["Price"].ToString();
            string MenuCode = dicPar["MenuCode"].ToString();
            string MemPrice = dicPar["MemPrice"].ToString();
            string CostPrice = dicPar["CostPrice"].ToString();
            string RoyMoney = dicPar["RoyMoney"].ToString();
            string ExtCode = dicPar["ExtCode"].ToString();
            string FinCode = dicPar["FinCode"].ToString();
            string KitCode = dicPar["KitCode"].ToString();
            string CookerCode = dicPar["CookerCode"].ToString();
            string MakeTime = dicPar["MakeTime"].ToString();
            string QRCode = dicPar["QRCode"].ToString();
            string WarCode = dicPar["WarCode"].ToString();
            string MatCode = dicPar["MatCode"].ToString();
            string Descript = dicPar["Descript"].ToString();
            string IsCount = dicPar["IsCount"].ToString();
            string DefCount = dicPar["DefCount"].ToString();
            string CountPrice = dicPar["CountPrice"].ToString();
            string IsVarPrice = dicPar["IsVarPrice"].ToString();
            string IsWeight = dicPar["IsWeight"].ToString();
            string IsMethod = dicPar["IsMethod"].ToString();
            string IsStock = dicPar["IsStock"].ToString();
            string IsPoint = dicPar["IsPoint"].ToString();
            string IsMemPrice = dicPar["IsMemPrice"].ToString();
            string IsCoupon = dicPar["IsCoupon"].ToString();
            string IsKeep = dicPar["IsKeep"].ToString();
            string IsCombo = dicPar["IsCombo"].ToString();
            string UCode = dicPar["UCode"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string FinTypeName = string.Empty;
            if (string.IsNullOrEmpty(QuickCode))
            {
                QuickCode = Others.GetChineseSpell(DisName);
            }
            if (!string.IsNullOrEmpty(FinCode))
            {
                DataTable dtFinType = GetCacheToFinType(userid);
                DataRow[] fin = dtFinType.Select("fincode='" + FinCode + "'");
                FinTypeName = fin[0]["finname"].ToString();
            }
            string ImageName = dicPar["ImageName"].ToString();
            string dishesMethodsJson = dicPar["dishesMethodsJson"].ToString();// 菜品做法加价
            string dishescombosJson = dicPar["dishescombosJson"].ToString(); // 套餐菜品
            string dishescomboinfoJson = dicPar["dishescomboinfoJson"].ToString(); // 套餐组别信息
            //调用逻辑
            logentity.pageurl = "TB_DishEdit.html";
            logentity.logcontent = "新增菜品信息信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Add(GUID, USER_ID, BusCode, StoCode, CCname, UCname, TStatus, ChannelCodeList, DisCode, DisName, OtherName, TypeCode, QuickCode, CusDisCode, Unit, Price, MenuCode, MemPrice, CostPrice, RoyMoney, ExtCode, FinCode, KitCode, CookerCode, MakeTime, QRCode, WarCode, MatCode, Descript, IsCount, DefCount, CountPrice, IsVarPrice, IsWeight, IsMethod, IsStock, IsPoint, IsMemPrice, IsCoupon, IsKeep, IsCombo, CCode, UCode, ImageName, dishesMethodsJson, dishescombosJson, dishescomboinfoJson, FinTypeName, logentity);

            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "BusCode", "StoCode", "CCname", "UCname", "TStatus", "ChannelCodeList", "DisCode", "DisName", "OtherName", "TypeCode", "QuickCode", "CusDisCode", "Unit", "Price", "MenuCode", "MemPrice", "CostPrice", "RoyMoney", "ExtCode", "FinCode", "KitCode", "CookerCode", "MakeTime", "QRCode", "WarCode", "MatCode", "Descript", "IsCount", "DefCount", "CountPrice", "IsVarPrice", "IsWeight", "IsMethod", "IsStock", "IsPoint", "IsMemPrice", "IsCoupon", "IsKeep", "IsCombo", "CCode", "UCode", "ImageName", "dishesMethodsJson", "dishescombosJson", "dishescomboinfoJson" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string userid = dicPar["userid"].ToString();
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string BusCode = dicPar["BusCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string CCname = dicPar["CCname"].ToString();
            string UCname = dicPar["UCname"].ToString();
            string TStatus = dicPar["TStatus"].ToString();
            string ChannelCodeList = dicPar["ChannelCodeList"].ToString();
            if (ChannelCodeList == "0")
            {
                ChannelCodeList = "1,2";
            }
            string DisCode = dicPar["DisCode"].ToString();
            string DisName = dicPar["DisName"].ToString();
            string OtherName = dicPar["OtherName"].ToString();
            string TypeCode = dicPar["TypeCode"].ToString();
            string QuickCode = dicPar["QuickCode"].ToString();
            string CusDisCode = dicPar["CusDisCode"].ToString();
            string Unit = dicPar["Unit"].ToString();
            string Price = dicPar["Price"].ToString();
            string MenuCode = dicPar["MenuCode"].ToString();
            string MemPrice = dicPar["MemPrice"].ToString();
            string CostPrice = dicPar["CostPrice"].ToString();
            string RoyMoney = dicPar["RoyMoney"].ToString();
            string ExtCode = dicPar["ExtCode"].ToString();
            string FinCode = dicPar["FinCode"].ToString();
            string KitCode = dicPar["KitCode"].ToString();
            string CookerCode = dicPar["CookerCode"].ToString();
            string MakeTime = dicPar["MakeTime"].ToString();
            string QRCode = dicPar["QRCode"].ToString();
            string WarCode = dicPar["WarCode"].ToString();
            string MatCode = dicPar["MatCode"].ToString();
            string Descript = dicPar["Descript"].ToString();
            string IsCount = dicPar["IsCount"].ToString();
            string DefCount = dicPar["DefCount"].ToString();
            string CountPrice = dicPar["CountPrice"].ToString();
            string IsVarPrice = dicPar["IsVarPrice"].ToString();
            string IsWeight = dicPar["IsWeight"].ToString();
            string IsMethod = dicPar["IsMethod"].ToString();
            string IsStock = dicPar["IsStock"].ToString();
            string IsPoint = dicPar["IsPoint"].ToString();
            string IsMemPrice = dicPar["IsMemPrice"].ToString();
            string IsCoupon = dicPar["IsCoupon"].ToString();
            string IsKeep = dicPar["IsKeep"].ToString();
            string IsCombo = dicPar["IsCombo"].ToString();
            string UCode = dicPar["UCode"].ToString();
            string CCode = dicPar["CCode"].ToString();
            if (string.IsNullOrEmpty(QuickCode))
            {
                QuickCode = Others.GetChineseSpell(DisName);
            }
            string FinTypeName = string.Empty;
            if (!string.IsNullOrEmpty(FinCode))
            {
                DataTable dtFinType = GetCacheToFinType(userid);
                DataRow[] fin = dtFinType.Select("fincode='" + FinCode + "'");
                FinTypeName = fin[0]["finname"].ToString();
            }
            else
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"财务类别不能为空\"}");
                return;
            }
            string ImageName = dicPar["ImageName"].ToString();
            string dishesMethodsJson = dicPar["dishesMethodsJson"].ToString();// 菜品做法加价
            string dishescombosJson = dicPar["dishescombosJson"].ToString(); // 套餐菜品
            string dishescomboinfoJson = dicPar["dishescomboinfoJson"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_DishEdit.html";
            logentity.logcontent = "修改id为:" + DisCode + "的菜品信息信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Update(GUID, USER_ID, BusCode, StoCode, CCname, UCname, TStatus, ChannelCodeList, DisCode, DisName, OtherName, TypeCode, QuickCode, CusDisCode, Unit, Price, MenuCode, MemPrice, CostPrice, RoyMoney, ExtCode, FinCode, KitCode, CookerCode, MakeTime, QRCode, WarCode, MatCode, Descript, IsCount, DefCount, CountPrice, IsVarPrice, IsWeight, IsMethod, IsStock, IsPoint, IsMemPrice, IsCoupon, IsKeep, IsCombo, CCode, UCode, ImageName, dishesMethodsJson, dishescombosJson, dishescomboinfoJson, FinTypeName, logentity);

            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "DisCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string DisCode = dicPar["DisCode"].ToString();
            string userid = dicPar["userid"].ToString();
            //调用逻辑			
            DataTable dtDish = bll.GetPagingSigInfo(GUID, USER_ID, "where dis.id='" + DisCode + "'");//菜品信息
            DataTable dtDishImage = new DataTable();
            ArrayList arrData = new ArrayList();
            string[] arrTBName = new string[5] { "dishes", "Method", "Combo", "Image", "ComboInfo" };
            arrData.Add(dtDish);
            arrData.Add(dtDishImage);
            ReturnListJson("0", "", arrData, arrTBName);
        }

        private void WebDetail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "DisCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string DisCode = dicPar["DisCode"].ToString();
            string userid = dicPar["userid"].ToString();
            //调用逻辑			
            DataTable dtDish = bll.GetDisForCombo(GUID, USER_ID, "where dis.id='" + DisCode + "'");//菜品信息
            DataTable dtDishMethods = new DataTable();
            DataTable dtForCombo = new DataTable();
            DataTable dtDishImage = new DataTable();
            if (dtDish != null && dtDish.Rows.Count > 0)
            {
                #region 门店名称
                DataTable dtStore = GetCacheToStore(userid);
                if (dtStore != null && dtStore.Rows.Count > 0)
                {
                    string stocode = dtDish.Rows[0]["StoCode"].ToString();
                    if (dtStore.Select("stocode='" + stocode + "'").Length > 0)
                    {
                        DataRow dr_sto = dtStore.Select("stocode='" + stocode + "'")[0];
                        dtDish.Rows[0]["StoName"] = dr_sto["cname"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dtDish.Rows[0]["ChannelCodeList"].ToString()))
                    {
                        foreach (string ccode in dtDish.Rows[0]["ChannelCodeList"].ToString().Split(','))
                        {
                            if (!string.IsNullOrEmpty(ccode))
                            {
                                if (!string.IsNullOrEmpty(ccode))
                                {
                                    dtDish.Rows[0]["ChannelCodeListName"] += dtchannel.Select("enumcode='" + ccode + "'")[0]["enumname"] + ",";
                                }
                            }
                        }
                        dtDish.Rows[0]["ChannelCodeListName"] = dtDish.Rows[0]["ChannelCodeListName"].ToString().TrimEnd(',');
                    }
                }
                #endregion
                #region 制作厨师
                DataTable dtEmp = GetCacheToEmployee(userid);
                if (dtEmp != null && dtEmp.Rows.Count > 0)
                {
                    string empCode = dtDish.Rows[0]["CookerCode"].ToString();
                    if (dtEmp.Select("ecode='" + empCode + "'").Length > 0)
                    {
                        DataRow dr_emp = dtEmp.Select("ecode='" + empCode + "'")[0];
                        dtDish.Rows[0]["CookerCodeName"] = dr_emp["cname"].ToString();
                    }
                }
                #endregion
                #region 原物料
                if (!string.IsNullOrEmpty(dtDish.Rows[0]["MatCode"].ToString()))
                {
                    DataTable dtMate = new bllStockMaterial().GetStockMaterial(" where A.matcode='" + dtDish.Rows[0]["MatCode"].ToString() + "'", GetCacheToUserBusCode(userid), string.Empty);
                    if (dtMate != null && dtMate.Rows.Count > 0)
                    {
                        string MatCode = dtDish.Rows[0]["MatCode"].ToString();
                        if (dtMate.Select("matcode='" + MatCode + "'").Length > 0)
                        {
                            DataRow dr_mat = dtMate.Select("matcode='" + MatCode + "'")[0];
                            dtDish.Rows[0]["MatCodeName"] = dr_mat["matname"].ToString();
                        }
                    }
                }
                #endregion

                dtDish.AcceptChanges();
                //做法加价信息 
                dtDishMethods = bll.GetDisMethods(dtDish.Rows[0]["BusCode"].ToString(), dtDish.Rows[0]["StoCode"].ToString(), dtDish.Rows[0]["DisCode"].ToString());
                //套餐内菜品信息
                if (dtDish.Rows[0]["IsCombo"].ToString() == "1")//套餐
                {
                    dtForCombo = bll.GetDisForCombo(dtDish.Rows[0]["BusCode"].ToString(), dtDish.Rows[0]["StoCode"].ToString(), dtDish.Rows[0]["DisCode"].ToString());
                    dtComboInfo = bll.GetDisForComboInfo(dtDish.Rows[0]["BusCode"].ToString(), dtDish.Rows[0]["StoCode"].ToString(), dtDish.Rows[0]["DisCode"].ToString());
                }
                //菜品图片信息
                dtDishImage = bll.GetDisImages(dtDish.Rows[0]["BusCode"].ToString(), dtDish.Rows[0]["StoCode"].ToString(), dtDish.Rows[0]["DisCode"].ToString());
                if (dtDishImage != null && dtDishImage.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDishImage.Rows)
                    {
                        dr["ImageUrl"] = Helper.GetAppSettings("StoBackRoot") + dr["ImageName"].ToString();
                    }
                    dtDishImage.AcceptChanges();
                }
            }
            ArrayList arrData = new ArrayList();
            string[] arrTBName = new string[5] { "dishes", "Method", "Combo", "Image", "ComboInfo" };
            arrData.Add(dtDish);
            arrData.Add(dtDishMethods);
            arrData.Add(dtForCombo);
            arrData.Add(dtDishImage);
            arrData.Add(dtComboInfo);
            ReturnListJson("0", "", arrData, arrTBName);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string DisCode = dicPar["id"].ToString();
            //调用逻辑
            logentity.pageurl = "TB_DishList.html";
            logentity.logcontent = "删除id为:" + DisCode + "的菜品信息信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Delete(GUID, USER_ID, DisCode, logentity);
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

            string DisCode = dicPar["id"].ToString().Trim(',');
            logentity.pageurl = "TB_DishList.html";
            logentity.logcontent = "修改状态id为:" + DisCode + "的菜品信息信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, DisCode, status);

            ReturnListJson(dt);
        }


        #region 菜品列表的接口
        /// <summary>
        /// 获取菜品类别一级类别
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetDishTypePPKData(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
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
                filter += GetAuthoritywhere("stocode", userid);
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
            //调用逻辑
            dt = new bllTB_DishType().GetDisTypeOneListInfo(GUID, USER_ID, filter, order);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 根据一级菜品类别获取旗下二级类别
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetDishTypePPKToPKData(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "PKKCode", "StoCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string PPKCode = dicPar["PKKCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            string where = "";
            if(!string.IsNullOrEmpty(BusCode))
            {
                where = " and buscode='" + BusCode + "'";
            }
            //调用逻辑
            dt = new bllTB_DishType().GetDishTypePPKToPKData(GUID, USER_ID, "where PKKCode='" + PPKCode + "' and StoCode='" + StoCode + "'"+where, "");
            ReturnListJson(dt);
        }

        /// <summary>
        /// 通过门店编号获取菜品一级类别、菜谱、财务类别信息集
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetDishSelectAllData(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "StoCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string stocode = dicPar["StoCode"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            string where = "";
            if (!string.IsNullOrEmpty(BusCode))
            {
                where = " and buscode='" + BusCode + "'";
            }
            //调用逻辑
            //获取菜谱类别一级分类
            DataTable dtDishType = new bllTB_DishType().GetDisTypeOneListInfo(GUID, USER_ID, "where stocode='" + stocode + "'"+where, "");
            //获取菜谱
            DataTable dtDishMenu = new bllTB_DishMenu().GetStoDishMenu(GUID, USER_ID, "where stocode='" + stocode + "'"+where, "");
            #region 获取财务类别
            DataTable dtFinType = GetCacheToFinType(userid);
            #endregion
            ArrayList arrData = new ArrayList();
            string[] arrTBName = new string[3] { "data1", "data2", "data3" };
            arrData.Add(dtDishType);
            arrData.Add(dtDishMenu);
            arrData.Add(dtFinType);
            ReturnListJson("0", "", arrData, arrTBName);
        }


        /// <summary>
        /// 查询原物料
        /// </summary>
        /// <param name="dicPar">filters为XX.matname原料名称、XX.matclscode分类编号</param>
        private void GetStockMaterial(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "filters", "StoCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
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
            filter = GetBusCodeWhere(dicPar, filter, "A.buscode");

            filter = filter.Replace("XX.", "A.");
            DataTable dtMate = new bllStockMaterial().GetStockMaterial(filter, GetCacheToUserBusCode(userid), StoCode);
            ReturnListJson(dtMate);
        }

        /// <summary>
        /// 上传菜品图片
        /// </summary>
        /// <param name="context"></param>
        private void UpLoadDishImages(HttpContext context)
        {
            string img_url = "";
            try
            {
                int count = context.Request.Files.Count;
                if (count > 0)
                {
                    HttpPostedFile file = context.Request.Files[0];
                    string filename = file.FileName;
                    if (filename != "")
                    {
                        string ext = filename.Substring(filename.LastIndexOf(".")).ToLower();
                        string path = "";
                        filename = Guid.NewGuid().ToString() + ext;
                        if (!Directory.Exists(context.Server.MapPath("~/uploads/UpDishImages/")))
                        {
                            Directory.CreateDirectory(context.Server.MapPath("~/uploads/UpDishImages/"));
                        }
                        path = context.Server.MapPath("~/uploads/UpDishImages/" + filename + "");
                        img_url = filename;
                        bool b = File.Exists(path);
                        if (b)
                        {
                            File.Delete(path);
                            file.SaveAs(path);
                        }
                        else
                        {
                            file.SaveAs(path);
                        }
                    }
                }
                DataTable dtImage = new DataTable();
                dtImage.Columns.Add("ImageName", typeof(string));
                dtImage.Columns.Add("ImageUrl", typeof(string));
                DataRow dr = dtImage.NewRow();
                dr["ImageName"] = "/uploads/UpDishImages/" + img_url;
                dr["ImageUrl"] = Helper.GetAppSettings("StoBackRoot") + "/uploads/UpDishImages/" + img_url;
                dtImage.Rows.Add(dr);
                ReturnListJson(dtImage);
            }
            catch (Exception e)
            {
                ToCustomerJson("2", e.Message);
            }
        }

        /// <summary>
        /// 新增或修改套餐时查询标配或选配菜品
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetComboTypeToDish(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "filters" };
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
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
                    if (filter.Contains("dis.MenuCode"))
                    {
                        DataRow[] drs = dtFilter.Select("col='dis.MenuCode'");
                        string menucodes = drs[0]["filter"].ToString();
                        string where = "dis.MenuCode in (";
                        string[] codes = menucodes.Split(',');
                        foreach (string c in codes)
                        {
                            where += "'" + c + "',";
                        }
                        where = where.TrimEnd(',') + ")";
                        filter = filter.Replace("dis.MenuCode = '" + menucodes + "'", where);
                    }
                }
                if (!filter.Contains("StoCode") && !filter.Contains("stocode"))
                {
                    filter += GetAuthoritywhere("dis.stocode", userid);
                }
            }
            else
            {
                filter = "where 1=1" + GetAuthoritywhere("dis.stocode", userid);
            }
            filter = GetBusCodeWhere(dicPar, filter, "dis.buscode");

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetComboTypeToDish(GUID, USER_ID, 1000, 1, filter, "", out recordCount, out totalPage);
            ReturnListJson(dt);
        }

        private DataRow SetResultRow(DataTable dt, string no, string disname, string err)
        {
            DataRow newdr = dt.NewRow();
            newdr["no"] = no;
            newdr["disname"] = disname;
            newdr["err"] = err;
            return newdr;
        }

        #endregion
    }
}