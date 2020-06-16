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
            int pageSize = Helper.StringToInt(dicPar["limit"].ToString());
            int currentPage = Helper.StringToInt(dicPar["page"].ToString());
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
                DataTable dtchannel = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DisChannel));
                DataTable dtStore = GetCacheToStore(userid);
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
            int pageSize = Helper.StringToInt(dicPar["limit"].ToString());
            int currentPage = Helper.StringToInt(dicPar["page"].ToString());
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
            if (dt != null && dt.Rows.Count > 0)
            {

                #region 门店名称
                DataTable dtchannel = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DisChannel));
                DataTable dtStore = GetCacheToStore(userid);
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
                #endregion
                dt.AcceptChanges();
            }
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
            logentity.cuser = Helper.StringToLong(USER_ID);
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
            logentity.cuser = Helper.StringToLong(USER_ID);
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
            DataTable dtDishMethods = new DataTable();
            DataTable dtForCombo = new DataTable();
            DataTable dtDishImage = new DataTable();
            DataTable dtComboInfo = new DataTable();
            DataTable dtchannel = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DisChannel));
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
                        dr["ImageUrl"] = Helper.GetAppSettings("StoDisImgUrl") + dr["ImageName"].ToString();
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
            DataTable dtDish = bll.GetWebPagingSigInfo(GUID, USER_ID, "where dis.id='" + DisCode + "'");//菜品信息
            DataTable dtDishMethods = new DataTable();
            DataTable dtForCombo = new DataTable();
            DataTable dtDishImage = new DataTable();
            DataTable dtComboInfo = new DataTable();
            DataTable dtchannel = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DisChannel));
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
            logentity.cuser = Helper.StringToLong(USER_ID);
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
            logentity.cuser = Helper.StringToLong(USER_ID);
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
        /// 根据厨房PKCode获取制作人
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetOutDepCodeToKitAndEmp(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "PKCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string KitCode = dicPar["PKCode"].ToString();
            string StoCode = dicPar["StoCode"].ToString();

            //调用逻辑

            TB_KitchenEntity Kit = new bllTB_Kitchen().GetEntitySigInfo("where PKCode='" + KitCode + "' and StoCode='" + StoCode + "'");
            //员工信息stocode,dcode,ecode,cname
            DataTable dtEmp = new DataTable();
            dtEmp.Columns.Add("ecode", typeof(string));
            dtEmp.Columns.Add("cname", typeof(string));
            dtEmp.Columns.Add("stocode", typeof(string));
            dtEmp.Columns.Add("dcode", typeof(string));
            if (GetCacheToEmployee(userid) != null)
            {
                DataRow[] dremps = ((DataTable)GetCacheToEmployee(userid)).Select("dcode='" + Kit.SubDepartment + "'");
                if (dremps.Length > 0)
                {
                    foreach (DataRow dr in dremps)
                    {
                        DataRow dremp = dtEmp.NewRow();
                        dremp["ecode"] = dr["ecode"].ToString();
                        dremp["cname"] = dr["cname"].ToString();
                        dremp["stocode"] = dr["stocode"].ToString();
                        dremp["dcode"] = dr["dcode"].ToString();
                        dtEmp.Rows.Add(dremp);
                    }
                }
            }
            ReturnListJson(dtEmp);
        }

        /// <summary>
        /// 添加菜品或修改菜品用,通过门店编号获取菜谱、财务类别、仓库、厨房信息集合
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetAddDishSelectAllData(Dictionary<string, object> dicPar)
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
            string StoCode = dicPar["StoCode"].ToString();
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
            string filter = "where stocode='" + StoCode + "'"+where;

            //获取菜谱
            DataTable dtDishMenu = new bllTB_DishMenu().GetStoDishMenu(GUID, USER_ID, filter, "");
            #region 获取财务类别
            DataTable dtFinType = GetCacheToFinType(userid);
            #endregion
            #region 获取门店仓库
            DataTable dtWareHouse = new bllStockWareHouse().GetStoWareHouseAllList(filter);

            #endregion
            DataTable dtKit = new bllTB_Kitchen().GetOutDepCodeToKit(GUID, USER_ID, StoCode);
            ArrayList arrData = new ArrayList();
            string[] arrTBName = new string[4] { "data1", "data2", "data3", "data4" };
            arrData.Add(dtDishMenu);
            arrData.Add(dtFinType);
            arrData.Add(dtWareHouse);
            arrData.Add(dtKit);
            ReturnListJson("0", "", arrData, arrTBName);
        }

        /// <summary>
        /// //根据门店编号获取原物料类别树
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetStockMateClass(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            string where = "";
            if (!string.IsNullOrEmpty(BusCode))
            {
                where = " where buscode='" + BusCode + "'";
            }
            if(string.IsNullOrEmpty(where))
            {
                where = " where 1=1 ";
            }
            DataTable dtMateClass = new bllWSStockMateClass().GetStockMateClass(where);
            ReturnListJson(dtMateClass);
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

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetImportTemplate(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "StoCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["StoCode"].ToString();
            string userid = dicPar["userid"].ToString();
            DataTable dtSto = new DataTable();//获取部门信息
            dtSto.Columns.Add("StoCode", typeof(string));
            DataRow newstodr = dtSto.NewRow();
            newstodr["StoCode"] = stocode;
            dtSto.Rows.Add(newstodr);
            dtSto.TableName = "rep";
            DataTable dtcDep = GetCacheToDepartment(userid);
            DataTable dtDep = new DataTable();//获取部门信息
            dtDep.Columns.Add("dcode", typeof(string));
            dtDep.Columns.Add("dname", typeof(string));
            if (dtcDep != null)
            {
                DataRow[] drsDep = dtcDep.Select("stocode='" + stocode + "' and status='1'");
                foreach (DataRow dr in drsDep)
                {
                    DataRow newdr = dtDep.NewRow();
                    newdr["dcode"] = dr["dcode"].ToString();
                    newdr["dname"] = dr["dname"].ToString();
                    dtDep.Rows.Add(newdr);
                }
            }



            DataTable dtKit = new bllTB_Kitchen().GetOutDepCodeToKit(GUID, USER_ID, stocode);//获取厨房
            DataTable dtDishMenu = new bllTB_DishMenu().GetStoDishMenu(GUID, USER_ID, "where StoCode='" + stocode + "' and tstatus='1'", "");//菜谱
            DataTable dtDishType = new bllTB_DishType().GetDishTypePPKToPKData(GUID, USER_ID, "where StoCode='" + stocode + "' and tstatus='1'", "");//菜品类别
            DataTable dtFin = GetCacheToFinType(userid).Copy();
            DataTable dtDic = GetCacheToDicts(userid).Copy();
            DataTable dtWareHouse = new bllStockWareHouse().GetStoWareHouseAllList("where stocode='" + stocode + "'");
            DataSet ds = new DataSet();
            ds.Tables.Add(dtDep);
            ds.Tables[0].TableName = "table";
            ds.Tables.Add(dtKit);
            ds.Tables[1].TableName = "table1";
            ds.Tables.Add(dtDishMenu);
            ds.Tables[2].TableName = "table2";
            ds.Tables.Add(dtDishType);
            ds.Tables[3].TableName = "table3";
            ds.Tables.Add(dtFin);
            ds.Tables[4].TableName = "table4";
            ds.Tables.Add(dtDic);
            ds.Tables[5].TableName = "table5";
            ds.Tables.Add(dtWareHouse);
            ds.Tables[6].TableName = "table6";
            ds.Tables.Add(dtSto);
            if (ds != null && ds.Tables.Count == 8)
            {
                AsponseHelper asponse = new AsponseHelper();
                string tempPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\templateData\\dishtemplate.xlsx";
                asponse.OPenfileByFilePath("菜品模板", tempPath, ds, dtSto);
                string path = asponse.VPath;
                ToJsonStr("{\"code\":\"0\",\"msg\":\"操作成功\",\"data\":[{\"filepath\":\"" + Helper.GetAppSettings("StoBackRoot") + path + "\"}]}");
            }
            else
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"暂无数据\"}");
            }
        }

        /// <summary>
        /// 菜品导出
        /// </summary>
        /// <param name="dicPar"></param>
        private void DishesExport(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "page", "limit", "filters", "orders", "type" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string type = dicPar["type"].ToString();
            int pageSize = Helper.StringToInt(dicPar["limit"].ToString());
            int currentPage = Helper.StringToInt(dicPar["page"].ToString());
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

            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, string.Empty, out recordCount, out totalPage);
            if (dt != null && dt.Rows.Count > 0)
            {

                #region 门店名称
                DataTable dtchannel = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DisChannel));
                DataTable dtStore = GetCacheToStore(userid);
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
                    }
                }
                #endregion
                #region 财务类别名称
                DataTable dtFin = GetCacheToFinType(userid);
                if (dtFin != null && dtFin.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string stocode = dr["FinCode"].ToString();
                        if (dtFin.Select("fincode='" + stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtFin.Select("fincode='" + stocode + "'")[0];
                            dr["FinTypeName"] = dr_sto["finname"].ToString();
                        }
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
                #region 原物料
                StringBuilder filters = new StringBuilder();
                filters.Append(" where A.matcode in(");
                foreach (DataRow dr1 in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr1["matcode"].ToString()))
                    {
                        filters.Append("'" + dr1["matcode"] + "',");
                    }
                }

                DataTable dtMate = new bllStockMaterial().GetStockMaterial(filters.ToString().TrimEnd(',') + ")", GetCacheToUserBusCode(userid), string.Empty);
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
                DataTable dtWareHouse = new bllStockWareHouse().GetStoWareHouseAllList(GetAuthoritywhere("stocode", userid));
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
                AsponseHelper asponse = new AsponseHelper();
                string tempFile = HttpContext.Current.Request.PhysicalApplicationPath + "\\templateData\\";
                if (type == "2")
                {
                    tempFile += "dishePackageInfo.xlsx";
                }
                else
                {
                    tempFile += "dishesInfo.xlsx";
                }
                DataSet ds = new DataSet();
                dt.TableName = "rep";
                ds.Tables.Add(dt);
                string html = string.Empty;
                if (type == "2")
                {
                    string path = asponse.VPath;
                    asponse.OPenfileByFilePath("菜品套餐", tempFile, ds, dt);
                }
                else
                {
                    asponse.OPenfileByFilePath("菜品信息", tempFile, ds, dt);
                }
                html = asponse.VPath;
                ToJsonStr("{\"code\":\"0\",\"msg\":\"操作成功\",\"data\":[{\"filepath\":\"" + Helper.GetAppSettings("StoBackRoot") + html + "\"}]}");
            }
            else
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"暂无数据\"}");
            }
        }

        /// <summary>
        /// 菜品导入
        /// </summary>
        private void UploadDishesByTemplate(Dictionary<string, object> dicPar)
        {
            if (this.Pagcontext.Request.Files.Count == 0)
            {
                this.Pagcontext.Response.Write(JsonHelper.ToJson("1", "没有接收到任何文件！"));
                return;
            }
            DataTable dtResult = new DataTable();//获取部门信息
            dtResult.Columns.Add("no", typeof(string));
            dtResult.Columns.Add("disname", typeof(string));
            dtResult.Columns.Add("err", typeof(string));
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "CCname" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                this.Pagcontext.Response.Write(JsonHelper.ToJson("1", "请求参数错误！"));
                return;
            }
            try
            {
                string GUID = dicPar["GUID"].ToString();
                string USER_ID = dicPar["USER_ID"].ToString();
                string stocode = string.Empty;
                //if (string.IsNullOrEmpty(stocode))
                //{
                //    this.Pagcontext.Response.Write(JsonHelper.ToJson("1", "请选择门店！"));
                //    return;
                //}
                string CCname = dicPar["CCname"].ToString();
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
                HttpPostedFile file = this.Pagcontext.Request.Files[0];
                string filename = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                string uploadPath = HttpContext.Current.Server.MapPath("/uploadtemplate/");
                if (file != null)
                {
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    file.SaveAs(uploadPath + filename);
                    //开始导入菜品
                    string importMessage = "";
                    DataTable dtImportDishes = ExcelsHelp.GetExeclToDataTable_template(uploadPath + filename, "TB_Dish", 1, out importMessage);
                    if (dtImportDishes == null || dtImportDishes.Rows.Count == 0)
                    {
                        ToJsonStr("{\"code\":\"1\",\"msg\":\"模板文件上传失败\"}");
                        return;
                    }
                    string userid = dicPar["userid"].ToString();
                    stocode = dtImportDishes.Rows[0]["门店编号"].ToString();
                    DataTable dtcDep = GetCacheToDepartment(userid);
                    DataRow[] drsDep = dtcDep.Select("stocode='" + stocode + "' and status='1'");
                    DataTable dtDep = new DataTable();//获取部门信息
                    dtDep.Columns.Add("dcode", typeof(string));
                    dtDep.Columns.Add("dname", typeof(string));
                    foreach (DataRow dr in drsDep)
                    {
                        DataRow newdr = dtDep.NewRow();
                        newdr["dcode"] = dr["dcode"].ToString();
                        newdr["dname"] = dr["dname"].ToString();
                        dtDep.Rows.Add(newdr);
                    }

                    //获取该门店的所有菜品
                    int recordCount = 0;
                    int totalPage = 0;
                    DataTable dtDis = bll.GetPagingListInfo(GUID, USER_ID, 5000, 1, "where dis.stocode='" + stocode + "'", string.Empty, string.Empty, out recordCount, out totalPage);
                    DataTable dtKit = new bllTB_Kitchen().GetOutDepCodeToKit(GUID, USER_ID, stocode);//获取厨房
                    DataTable dtDishMenu = new bllTB_DishMenu().GetStoDishMenu(GUID, USER_ID, "where StoCode='" + stocode + "' and tstatus='1'"+where, "");//菜谱
                    DataTable dtDishType = new bllTB_DishType().GetDishTypePPKToPKData(GUID, USER_ID, "where StoCode='" + stocode + "' and tstatus='1'"+where, "");//菜品类别
                    DataTable dtFin = GetCacheToFinType(userid).Copy();
                    DataTable dtDic = GetCacheToDicts(userid).Copy();
                    DataTable dtWareHouse = new bllStockWareHouse().GetStoWareHouseAllList("where stocode='" + stocode + "'"+where);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtDep);
                    ds.Tables[0].TableName = "table";
                    ds.Tables.Add(dtKit);
                    ds.Tables[1].TableName = "table1";
                    ds.Tables.Add(dtDishMenu);
                    ds.Tables[2].TableName = "table2";
                    ds.Tables.Add(dtDishType);
                    ds.Tables[3].TableName = "table3";
                    ds.Tables.Add(dtFin);
                    ds.Tables[4].TableName = "table4";
                    ds.Tables.Add(dtDic);
                    ds.Tables[5].TableName = "table5";
                    ds.Tables.Add(dtWareHouse);
                    ds.Tables[6].TableName = "table6";
                    if (ds != null && ds.Tables.Count == 7)
                    {

                        string errmessage = string.Empty;
                        for (int i = 0; i < dtImportDishes.Rows.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(dtImportDishes.Rows[i][0].ToString()))
                            {
                                errmessage = CheckImportRow(ds, dtImportDishes.Rows[i]);
                                if (!string.IsNullOrWhiteSpace(errmessage))
                                {
                                    dtResult.Rows.Add(SetResultRow(dtResult, (i + 1).ToString(), dtImportDishes.Rows[i]["菜品名称"].ToString(), "第" + (i + 1) + "行，" + errmessage + "-导入失败  "));
                                }
                            }
                        }

                        string DisNames = string.Empty;
                        string QRCode = string.Empty;
                        string ewm = string.Empty;

                        //上传菜品
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.Append(" declare @DisCode varchar(32);");
                        foreach (DataRow dr in dtImportDishes.Rows)
                        {
                            if (!string.IsNullOrWhiteSpace(dr["序号"].ToString()))
                            {

                                if (!string.IsNullOrEmpty(dr["菜品名称"].ToString()))
                                {
                                    if (DisNames.Contains(dr["菜品名称"].ToString() + ",") || dtDis.Select("disname='" + dr["菜品名称"].ToString() + "'").Length > 0)
                                    {
                                        dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), "菜品名称:" + dr["菜品名称"].ToString() + "重复-序号" + dr["序号"].ToString() + "导入失败  "));
                                    }
                                    else
                                    {
                                        DisNames += dr["菜品名称"].ToString() + ",";
                                    }
                                }
                                else
                                {
                                    dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), "菜品名称:不能为空-序号" + dr["序号"].ToString() + "导入失败  "));
                                }
                                if (!string.IsNullOrEmpty(dr["菜品二维码"].ToString()))
                                {
                                    if (QRCode.Contains(dr["菜品二维码"].ToString() + ",") || dtDis.Select("QRCode='" + dr["菜品二维码"].ToString() + "'").Length > 0)
                                    {
                                        dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), "菜品二维码:" + dr["菜品二维码"].ToString() + "重复-序号" + dr["序号"].ToString() + "导入失败  "));
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(dr["菜品二维码"].ToString()))
                                        {
                                            QRCode += dr["菜品二维码"].ToString() + ",";
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(dr["售价"].ToString()))
                                {
                                    try
                                    {
                                        decimal value = decimal.Parse(dr["售价"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "售价:" + dr["售价"].ToString() + "格式无效-序号" + dr["序号"].ToString() + "导入失败  "));
                                    }

                                }
                                else
                                {
                                    dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "售价:不能为空-导入失败  "));
                                }
                                if (!string.IsNullOrEmpty(dr["成本价"].ToString()))
                                {
                                    try
                                    {
                                        decimal value = decimal.Parse(dr["成本价"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "成本价:" + dr["成本价"].ToString() + "格式无效-序号" + dr["序号"].ToString() + "导入失败  "));
                                    }

                                }
                                else
                                {
                                    dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "成本价:不能为空-序号" + dr["序号"].ToString() + "导入失败  "));
                                }
                                if (!string.IsNullOrEmpty(dr["会员价"].ToString()))
                                {
                                    try
                                    {
                                        decimal value = decimal.Parse(dr["会员价"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "会员价:" + dr["会员价"].ToString() + "格式无效-序号" + dr["序号"].ToString() + "导入失败  "));
                                    }

                                }
                                else
                                {
                                    dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "会员价:不能为空-序号" + dr["序号"].ToString() + "导入失败  "));
                                }
                                if (!string.IsNullOrEmpty(dr["提成金额"].ToString()))
                                {
                                    try
                                    {
                                        decimal value = decimal.Parse(dr["提成金额"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "提成金额:" + dr["提成金额"].ToString() + "格式无效-序号" + dr["序号"].ToString() + "导入失败  "));
                                    }

                                }
                                else
                                {
                                    SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "提成金额:不能为空-序号" + dr["序号"].ToString() + "导入失败  ");
                                }
                                if (dr["是否烟酒可入库"].ToString() == "是")
                                {
                                    if (string.IsNullOrEmpty(dr["所属原料编号"].ToString()))
                                    {
                                        dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "所属原料编号:不能为空-序号" + dr["序号"].ToString() + "导入失败  "));
                                    }
                                    if (string.IsNullOrEmpty(dr["所属仓库编号"].ToString()))
                                    {
                                        dtResult.Rows.Add(SetResultRow(dtResult, dr["序号"].ToString(), dr["菜品名称"].ToString(), dr["菜品名称"].ToString() + "所属原料编号:不能为空-序号" + dr["序号"].ToString() + "导入失败  "));
                                    }
                                }


                                string ChannelCodeList = dr["适用渠道"].ToString();
                                if (ChannelCodeList == "PC" || ChannelCodeList == "2")
                                {
                                    ChannelCodeList = "2";
                                }
                                else if (ChannelCodeList == "微信" || ChannelCodeList == "1")
                                {
                                    ChannelCodeList = "1";
                                }
                                else
                                {
                                    ChannelCodeList = "1,2";//全部
                                }
                                sbSql.Append(" exec [dbo].[p_GetbaseCode] @DisCode output;");
                                sbSql.Append(" INSERT INTO TB_Dish( " +
                                    "[BusCode],[StoCode],[CCode]," +
                                    "[CCname],[UCode],[UCname]," +
                                    "[TStatus],[ChannelCodeList],[DisName]," +
                                    "[OtherName],[TypeCode],[QuickCode]," +
                                    "[CusDisCode],[Unit],[Price]," +
                                    "[MenuCode],[MemPrice],[CostPrice]," +
                                    "[RoyMoney],[ExtCode],[FinCode]," +
                                    "[KitCode],[CookerCode],[MakeTime]," +
                                    "[QRCode],[WarCode],[MatCode]," +
                                    "[Descript],[IsCount],[DefCount]," +
                                    "[CountPrice],[IsVarPrice],[IsWeight]," +
                                    "[IsMethod],[IsStock],[IsPoint]," +
                                    "[IsMemPrice],[IsCoupon],[IsKeep]," +
                                    "[IsCombo],DisCode,CTime," +
                                    "UTime,fintypename ) values(" +
                                    "'" + GetCacheToUserBusCode(userid) + "','" + stocode + "','" + userid + "'," +
                                    "'" + CCname + "','" + userid + "','" + CCname + "'," +
                                    "'0','" + ChannelCodeList + "','" + dr["菜品名称"].ToString() + "'," +
                                    "'" + dr["其他名称"].ToString() + "','" + dr["菜品类别编号"].ToString() + "','" + Others.GetChineseSpell(dr["菜品名称"].ToString()) + "'," +
                                    "'" + dr["自定义编号"].ToString() + "','" + dr["单位"].ToString() + "','" + dr["售价"].ToString() + "'," +
                                    " '" + dr["菜谱编号"].ToString() + "'," + dr["会员价"].ToString() + "," + dr["成本价"].ToString() + "," +
                                    "" + dr["提成金额"].ToString() + ", '" + dr["外部码"].ToString() + "', '" + dr["财务类别编号"].ToString() + "'," +
                                    "'" + dr["厨房编号"].ToString() + "','',0,'','" + dr["所属仓库编号"].ToString() + "','" + dr["所属原料编号"].ToString() + "'," +
                                    "'','0',0," +
                                    "0,'" + (dr["是否可变价"].ToString() == "是" ? "1" : "0") + "','" + (dr["是否需称重"].ToString() == "是" ? "1" : "0") + "'," +
                                    "'" + (dr["是否做法必选"].ToString() == "是" ? "1" : "0") + "', '" + (dr["是否烟酒可入库"].ToString() == "是" ? "1" : "0") + "','" + (dr["是否可兑换"].ToString() == "是" ? "1" : "0") + "'," +
                                    "'" + (dr["是否允许会员价"].ToString() == "是" ? "1" : "0") + "','" + (dr["是否支持使用消费券"].ToString() == "是" ? "1" : "0") + "','" + (dr["是否可寄存"].ToString() == "是" ? "1" : "0") + "'," +
                                    "'0',@DisCode,getdate(),getdate(),'" + dr["财务类别"].ToString() + "');");
                            }
                        }
                        if (dtResult.Rows.Count > 0)
                        {
                            string jsonstr = JsonHelper.ToJson("2", "数据校验失败,如果确认导入校验失败的菜品将不会导入", new ArrayList() { dtResult }, new string[] { "data" }, null, null, null, null);
                            Pagcontext.Response.Write(jsonstr);
                        }
                        else
                        {
                            ToJsonStr("{\"code\":\"0\",\"msg\":\"数据校验成功\"}");
                        }
                    }
                    else
                    {
                        ToJsonStr("{\"code\":\"1\",\"msg\":\"数据解析失败,无法导入\"}");
                    }
                }
                else
                {
                    ToJsonStr("{\"code\":\"1\",\"msg\":\"文件保存失败\"}");
                }
            }
            catch (Exception ex)
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        private DataRow SetResultRow(DataTable dt, string no, string disname, string err)
        {
            DataRow newdr = dt.NewRow();
            newdr["no"] = no;
            newdr["disname"] = disname;
            newdr["err"] = err;
            return newdr;
        }

        /// <summary>
        /// 菜品导入
        /// </summary>
        private void UploadDishesByTemplatePost(Dictionary<string, object> dicPar)
        {
            if (this.Pagcontext.Request.Files.Count == 0)
            {
                this.Pagcontext.Response.Write(JsonHelper.ToJson("1", "没有接收到任何文件！"));
                return;
            }

            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "CCname" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                this.Pagcontext.Response.Write(JsonHelper.ToJson("1", "请求参数错误！"));
                return;
            }
            try
            {
                string GUID = dicPar["GUID"].ToString();
                string USER_ID = dicPar["USER_ID"].ToString();
                string stocode = string.Empty;
                //if (string.IsNullOrEmpty(stocode))
                //{
                //    this.Pagcontext.Response.Write(JsonHelper.ToJson("1", "请选择门店！"));
                //    return;
                //}
                string CCname = dicPar["CCname"].ToString();
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
                HttpPostedFile file = this.Pagcontext.Request.Files[0];
                string filename = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                string uploadPath = HttpContext.Current.Server.MapPath("/uploadtemplate/");
                if (file != null)
                {
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    file.SaveAs(uploadPath + filename);
                    //开始导入菜品
                    string importMessage = "";
                    DataTable dtImportDishes = ExcelsHelp.GetExeclToDataTable_template(uploadPath + filename, "TB_Dish", 1, out importMessage);
                    if (dtImportDishes == null || dtImportDishes.Rows.Count == 0)
                    {
                        this.Pagcontext.Response.Write("模板文件上传失败！");
                        return;
                    }
                    string userid = dicPar["userid"].ToString();
                    stocode = dtImportDishes.Rows[0]["门店编号"].ToString();
                    DataTable dtcDep = GetCacheToDepartment(userid);
                    DataRow[] drsDep = dtcDep.Select("stocode='" + stocode + "' and status='1'");
                    DataTable dtDep = new DataTable();//获取部门信息
                    dtDep.Columns.Add("dcode", typeof(string));
                    dtDep.Columns.Add("dname", typeof(string));
                    foreach (DataRow dr in drsDep)
                    {
                        DataRow newdr = dtDep.NewRow();
                        newdr["dcode"] = dr["dcode"].ToString();
                        newdr["dname"] = dr["dname"].ToString();
                        dtDep.Rows.Add(newdr);
                    }

                    //获取该门店的所有菜品
                    int recordCount = 0;
                    int totalPage = 0;
                    DataTable dtDis = bll.GetPagingListInfo(GUID, USER_ID, 5000, 1, "where dis.stocode='" + stocode + "'", string.Empty, string.Empty, out recordCount, out totalPage);
                    DataTable dtKit = new bllTB_Kitchen().GetOutDepCodeToKit(GUID, USER_ID, stocode);//获取厨房
                    DataTable dtDishMenu = new bllTB_DishMenu().GetStoDishMenu(GUID, USER_ID, "where StoCode='" + stocode + "' and tstatus='1'"+where, "");//菜谱
                    DataTable dtDishType = new bllTB_DishType().GetDishTypePPKToPKData(GUID, USER_ID, "where StoCode='" + stocode + "' and tstatus='1'"+where, "");//菜品类别
                    DataTable dtFin = GetCacheToFinType(userid).Copy();
                    DataTable dtDic = GetCacheToDicts(userid).Copy();
                    DataTable dtWareHouse = new bllStockWareHouse().GetStoWareHouseAllList("where stocode='" + stocode + "'"+where);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtDep);
                    ds.Tables[0].TableName = "table";
                    ds.Tables.Add(dtKit);
                    ds.Tables[1].TableName = "table1";
                    ds.Tables.Add(dtDishMenu);
                    ds.Tables[2].TableName = "table2";
                    ds.Tables.Add(dtDishType);
                    ds.Tables[3].TableName = "table3";
                    ds.Tables.Add(dtFin);
                    ds.Tables[4].TableName = "table4";
                    ds.Tables.Add(dtDic);
                    ds.Tables[5].TableName = "table5";
                    ds.Tables.Add(dtWareHouse);
                    ds.Tables[6].TableName = "table6";
                    List<DataRow> xh_list = new List<DataRow>();
                    if (ds != null && ds.Tables.Count == 7)
                    {

                        string errmessage = string.Empty;
                        for (int i = 0; i < dtImportDishes.Rows.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(dtImportDishes.Rows[i][0].ToString()))
                            {
                                errmessage = CheckImportRow(ds, dtImportDishes.Rows[i]);
                                if (!string.IsNullOrWhiteSpace(errmessage))
                                {
                                    xh_list.Add(dtImportDishes.Rows[i]);
                                }
                            }
                        }

                        string DisNames = string.Empty;
                        string QRCode = string.Empty;
                        string ewm = string.Empty;

                        //上传菜品
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.Append(" declare @DisCode varchar(32);");
                        foreach (DataRow dr in dtImportDishes.Rows)
                        {
                            if (xh_list.Contains(dr))
                            {
                                continue;
                            }
                            if (!string.IsNullOrWhiteSpace(dr["序号"].ToString()))
                            {

                                if (!string.IsNullOrEmpty(dr["菜品名称"].ToString()))
                                {
                                    if (DisNames.Contains(dr["菜品名称"].ToString() + ",") || dtDis.Select("disname='" + dr["菜品名称"].ToString() + "'").Length > 0)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        DisNames += dr["菜品名称"].ToString() + ",";
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(dr["菜品二维码"].ToString()))
                                {
                                    if (QRCode.Contains(dr["菜品二维码"].ToString() + ",") || dtDis.Select("QRCode='" + dr["菜品二维码"].ToString() + "'").Length > 0)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(dr["菜品二维码"].ToString()))
                                        {
                                            QRCode += dr["菜品二维码"].ToString() + ",";
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(dr["售价"].ToString()))
                                {
                                    try
                                    {
                                        decimal value = decimal.Parse(dr["售价"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }

                                }
                                else
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(dr["成本价"].ToString()))
                                {
                                    try
                                    {
                                        decimal value = decimal.Parse(dr["成本价"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }

                                }
                                else
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(dr["会员价"].ToString()))
                                {
                                    try
                                    {
                                        decimal value = decimal.Parse(dr["会员价"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }

                                }
                                else
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(dr["提成金额"].ToString()))
                                {
                                    try
                                    {
                                        decimal value = decimal.Parse(dr["提成金额"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }

                                }
                                else
                                {
                                    continue;
                                }
                                if (dr["是否烟酒可入库"].ToString() == "是")
                                {
                                    if (string.IsNullOrEmpty(dr["所属原料编号"].ToString()))
                                    {
                                        continue;
                                    }
                                    if (string.IsNullOrEmpty(dr["所属仓库编号"].ToString()))
                                    {
                                        continue;
                                    }
                                }


                                string ChannelCodeList = dr["适用渠道"].ToString();
                                if (ChannelCodeList == "PC" || ChannelCodeList == "2")
                                {
                                    ChannelCodeList = "2";
                                }
                                else if (ChannelCodeList == "微信" || ChannelCodeList == "1")
                                {
                                    ChannelCodeList = "1";
                                }
                                else
                                {
                                    ChannelCodeList = "1,2";//全部
                                }
                                sbSql.Append(" exec [dbo].[p_GetbaseCode] @DisCode output;");
                                sbSql.Append(" INSERT INTO TB_Dish( " +
                                    "[BusCode],[StoCode],[CCode]," +
                                    "[CCname],[UCode],[UCname]," +
                                    "[TStatus],[ChannelCodeList],[DisName]," +
                                    "[OtherName],[TypeCode],[QuickCode]," +
                                    "[CusDisCode],[Unit],[Price]," +
                                    "[MenuCode],[MemPrice],[CostPrice]," +
                                    "[RoyMoney],[ExtCode],[FinCode]," +
                                    "[KitCode],[CookerCode],[MakeTime]," +
                                    "[QRCode],[WarCode],[MatCode]," +
                                    "[Descript],[IsCount],[DefCount]," +
                                    "[CountPrice],[IsVarPrice],[IsWeight]," +
                                    "[IsMethod],[IsStock],[IsPoint]," +
                                    "[IsMemPrice],[IsCoupon],[IsKeep]," +
                                    "[IsCombo],DisCode,CTime," +
                                    "UTime,fintypename ) values(" +
                                    "'" + GetCacheToUserBusCode(userid) + "','" + stocode + "','" + userid + "'," +
                                    "'" + CCname + "','" + userid + "','" + CCname + "'," +
                                    "'0','" + ChannelCodeList + "','" + dr["菜品名称"].ToString() + "'," +
                                    "'" + dr["其他名称"].ToString() + "','" + dr["菜品类别编号"].ToString() + "','" + Others.GetChineseSpell(dr["菜品名称"].ToString()) + "'," +
                                    "'" + dr["自定义编号"].ToString() + "','" + dr["单位"].ToString() + "','" + dr["售价"].ToString() + "'," +
                                    " '" + dr["菜谱编号"].ToString() + "'," + dr["会员价"].ToString() + "," + dr["成本价"].ToString() + "," +
                                    "" + dr["提成金额"].ToString() + ", '" + dr["外部码"].ToString() + "', '" + dr["财务类别编号"].ToString() + "'," +
                                    "'" + dr["厨房编号"].ToString() + "','',0,'" + dr["菜品二维码"].ToString() + "','" + dr["所属仓库编号"].ToString() + "','" + dr["所属原料编号"].ToString() + "'," +
                                    "'','0',0," +
                                    "0,'" + (dr["是否可变价"].ToString() == "是" ? "1" : "0") + "','" + (dr["是否需称重"].ToString() == "是" ? "1" : "0") + "'," +
                                    "'" + (dr["是否做法必选"].ToString() == "是" ? "1" : "0") + "', '" + (dr["是否烟酒可入库"].ToString() == "是" ? "1" : "0") + "','" + (dr["是否可兑换"].ToString() == "是" ? "1" : "0") + "'," +
                                    "'" + (dr["是否允许会员价"].ToString() == "是" ? "1" : "0") + "','" + (dr["是否支持使用消费券"].ToString() == "是" ? "1" : "0") + "','" + (dr["是否可寄存"].ToString() == "是" ? "1" : "0") + "'," +
                                    "'0',@DisCode,getdate(),getdate(),'" + dr["财务类别"].ToString() + "');");
                            }
                        }
                        string sqlUpload = sbSql.ToString();
                        if (!string.IsNullOrEmpty(sqlUpload))
                        {
                            int rows = bll.ExecuteSql(sqlUpload);
                            if (rows > 0)
                            {
                                ToJsonStr("{\"code\":\"0\",\"msg\":\"菜品上传成功\"}");
                            }
                            else
                            {
                                ToJsonStr("{\"code\":\"1\",\"msg\":\"菜品上传失败\"}");
                            }
                        }
                        else
                        {
                            ToJsonStr("{\"code\":\"1\",\"msg\":\"菜品上传失败\"}");
                        }
                    }
                    else
                    {
                        ToJsonStr("{\"code\":\"1\",\"msg\":\"数据校验失败\"}");
                    }
                }
                else
                {
                    ToJsonStr("{\"code\":\"1\",\"msg\":\"文件保存失败\"}");
                }
            }
            catch (Exception ex)
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"" + ex.Message + "\"}");
            }
        }

        private string CheckImportRow(DataSet ds, DataRow importRow)
        {
            string message = "";

            if (ds.Tables[1].Select("PKCode='" + importRow["厨房编号"].ToString() + "'").Length == 0)
            {
                message = "厨房编号";
            }
            else
            {
                if (ds.Tables[1].Select("PKCode='" + importRow["厨房编号"] + "'")[0]["KitName"].ToString() != importRow["厨房"].ToString())
                {
                    message = "厨房";
                }
            }

            if (ds.Tables[2].Select("PKCode='" + importRow["菜谱编号"].ToString() + "'").Length == 0)
            {
                message = "菜谱编号";
            }
            else
            {
                if (ds.Tables[2].Select("PKCode='" + importRow["菜谱编号"] + "'")[0]["MenuName"].ToString() != importRow["菜谱"].ToString())
                {
                    message = "菜谱";
                }
            }
            if (ds.Tables[3].Select("PKCode='" + importRow["菜品类别编号"].ToString() + "'").Length == 0)
            {
                message = "菜品类别编号";
            }
            else
            {
                if (ds.Tables[3].Select("PKCode='" + importRow["菜品类别编号"] + "'")[0]["TypeName"].ToString() != importRow["菜品类别"].ToString())
                {
                    message = "菜品类别";
                }
            }
            if (ds.Tables[4].Select("fincode='" + importRow["财务类别编号"].ToString() + "'").Length == 0)
            {
                message = "财务类别编号";
            }
            else
            {
                if (ds.Tables[4].Select("fincode='" + importRow["财务类别编号"] + "'")[0]["finname"].ToString() != importRow["财务类别"].ToString())
                {
                    message = "财务类别";
                }
            }
            //if (ds.Tables[5].Select("dicname='" + importRow["单位"].ToString() + "'").Length == 0)
            //{
            //    message = "单位";
            //}
            if (ds.Tables[6].Select("warcode='" + importRow["所属仓库编号"].ToString() + "'").Length == 0)
            {
                message = "所属仓库编号";
            }
            else
            {
                if (ds.Tables[6].Select("warcode='" + importRow["所属仓库编号"] + "'")[0]["warname"].ToString() != importRow["所属仓库名称"].ToString())
                {
                    message = "所属仓库编号";
                }
            }
            return message;
        }

        /// <summary>
        /// 获取财务类别
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetFinType(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "userid" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string userid = dicPar["userid"].ToString();
            DataTable dtFinType = GetCacheToFinType(userid);
            ReturnListJson(dtFinType);
        }

        //根据菜品编号获取菜品信息及可选、标配菜品信息
        private void GetDisheByCodes(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "discodes" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string discodes = dicPar["discodes"].ToString();

            DataSet ds = new bllPaging().GetDataSetInfoBySQL("select discode,disname,price,iscombo from tb_dish where discode in(select col from dbo.fn_StringSplit('" + discodes + "',',')) and stocode='" + stocode + "';select tdfc.DisCode,dbo.fnGetDisheNameByDiscode(tdfc.DisCode,'" + stocode + "') as usedisname,dbo.fngetdispricebystocode('" + stocode + "',tdfc.DisCode) as price,tdfc.DefNum,tdfc.MaxNum from TR_DisForCombo tdfc where tdfc.PDisCode in (select col from dbo.fn_StringSplit('" + discodes + "',',')) and tdfc.stocode='" + stocode + "' and tdfc.IsStand='1';select tdfc.DisCode,dbo.fnGetDisheNameByDiscode(tdfc.DisCode,'" + stocode + "') as usedisname,dbo.fngetdispricebystocode('" + stocode + "',tdfc.DisCode) as price,tdfc.DefNum,tdfc.MaxNum,tci.MaxOptNum,tci.totaloptiNum,tci.TotalOptMoney,tdcg.GroupName from TR_DisForCombo tdfc left join TR_ComboInfo tci on tdfc.StoCode=tci.StoCode and tdfc.PPKCode=tci.PKCode and tdfc.pdiscode=tci.PDisCode left join TB_DisComGroup tdcg on tdcg.StoCode=tci.StoCode and tdcg.pkcode = tci.ComGroupCode where tdfc.PDisCode in (select col from dbo.fn_StringSplit('" + discodes + "',',')) and tdfc.stocode='" + stocode + "' and tdfc.IsStand='2';");
            if (ds != null)
            {
                if (ds.Tables.Count == 3)
                {
                    DataTable dtDis = ds.Tables[0];
                    DataTable dtDisCom = ds.Tables[1];
                    DataTable dtDisPti = ds.Tables[2];
                    string jsonStr = "{'disInfo':[";
                    if (dtDis != null && dtDis.Rows.Count > 0)
                    {
                        for (var i = 0; i < dtDis.Rows.Count; i++)
                        {
                            if (i == dtDis.Rows.Count - 1)
                            {
                                jsonStr += "{'discode':'" + dtDis.Rows[i]["discode"].ToString() + "','disname':'" + dtDis.Rows[i]["disname"].ToString() + "','price':'" + dtDis.Rows[i]["price"].ToString() + "','iscombo':'" + dtDis.Rows[i]["iscombo"].ToString() + "'}]";
                            }
                            else
                            {
                                jsonStr += "{'discode':'" + dtDis.Rows[i]["discode"].ToString() + "','disname':'" + dtDis.Rows[i]["disname"].ToString() + "','price':'" + dtDis.Rows[i]["price"].ToString() + "','iscombo':'" + dtDis.Rows[i]["iscombo"].ToString() + "'},";
                            }
                        }
                    }
                    else
                    {
                        jsonStr += "]";
                    }

                    jsonStr += ",'dishescombo':[";
                    if (dtDisCom != null && dtDisCom.Rows.Count > 0)
                    {
                        for (var i = 0; i < dtDisCom.Rows.Count; i++)
                        {
                            if (i == dtDisCom.Rows.Count - 1)
                            {
                                jsonStr += "{'discode':'" + dtDisCom.Rows[i]["DisCode"].ToString() + "','disname':'" + dtDisCom.Rows[i]["usedisname"].ToString() + "','price':'" + dtDisCom.Rows[i]["price"].ToString() + "','defnum':'" + dtDisCom.Rows[i]["DefNum"].ToString() + "','maxnum':'" + dtDisCom.Rows[i]["MaxNum"].ToString() + "'}]";
                            }
                            else
                            {
                                jsonStr += "{'discode':'" + dtDisCom.Rows[i]["DisCode"].ToString() + "','disname':'" + dtDisCom.Rows[i]["usedisname"].ToString() + "','price':'" + dtDisCom.Rows[i]["price"].ToString() + "','defnum':'" + dtDisCom.Rows[i]["DefNum"].ToString() + "','maxnum':'" + dtDisCom.Rows[i]["MaxNum"].ToString() + "'},";
                            }
                        }
                    }
                    else
                    {
                        jsonStr += "]";
                    }

                    jsonStr += ",'dishesoptional':[";
                    if (dtDisPti != null && dtDisPti.Rows.Count > 0)
                    {
                        for (var i = 0; i < dtDisPti.Rows.Count; i++)
                        {
                            if (i == dtDisPti.Rows.Count - 1)
                            {
                                jsonStr += "{'discode':'" + dtDisPti.Rows[i]["DisCode"].ToString() + "','disname':'" + dtDisPti.Rows[i]["usedisname"].ToString() + "','price':'" + dtDisPti.Rows[i]["price"].ToString() + "','defnum':'" + dtDisPti.Rows[i]["DefNum"].ToString() + "','maxnum':'" + dtDisPti.Rows[i]["MaxNum"].ToString() + "','maxoptnum':'" + dtDisPti.Rows[i]["MaxOptNum"].ToString() + "','totaloptinum':'" + dtDisPti.Rows[i]["totaloptiNum"].ToString() + "','totaloptmoney':'" + dtDisPti.Rows[i]["TotalOptMoney"].ToString() + "','groupname':'" + dtDisPti.Rows[i]["GroupName"].ToString() + "'}]}";
                            }
                            else
                            {
                                jsonStr += "{'discode':'" + dtDisPti.Rows[i]["DisCode"].ToString() + "','disname':'" + dtDisPti.Rows[i]["usedisname"].ToString() + "','price':'" + dtDisPti.Rows[i]["price"].ToString() + "','defnum':'" + dtDisPti.Rows[i]["DefNum"].ToString() + "','maxnum':'" + dtDisPti.Rows[i]["MaxNum"].ToString() + "','maxoptnum':'" + dtDisPti.Rows[i]["MaxOptNum"].ToString() + "','totaloptinum':'" + dtDisPti.Rows[i]["totaloptiNum"].ToString() + "','totaloptmoney':'" + dtDisPti.Rows[i]["TotalOptMoney"].ToString() + "','groupname':'" + dtDisPti.Rows[i]["GroupName"].ToString() + "'},";
                            }
                        }
                    }
                    else
                    {
                        jsonStr += "]}";
                    }

                    ToJsonStr(jsonStr);
                }
                else
                {
                    ToCustomerJson("1", "暂无数据");
                }
            }
            else
            {
                ToCustomerJson("1", "暂无数据");
            }
        }

        //获取门店菜品信息
        private void GetDishesByName(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "disname" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string disname = dicPar["disname"].ToString();

            dt = new bllPaging().GetDataTableInfoBySQL("select discode,disname,price,iscombo from tb_dish where (disname like '%" + disname + "%' or ''='" + disname + "') and stocode='" + stocode + "' and tstatus='1';");

            if (dt != null && dt.Rows.Count > 0)
            {
                ReturnListJson(dt);
            }
            else
            {
                ToCustomerJson("1", "暂无数据");
            }
        }

        private void GetDisheInfo(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "discodes" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string discodes = dicPar["discodes"].ToString();

            dt = new bllPaging().GetDataTableInfoBySQL("select StoCode,discode,dbo.fngettbdishinfo(stocode,discode) as disnames from tb_dish where discode in(select col from dbo.fn_StringSplit('" + discodes + "',','))");

            if (dt != null && dt.Rows.Count > 0)
            {
                ReturnListJson(dt);
            }
            else
            {
                ToCustomerJson("1", "暂无数据");
            }
        }

        private void GetDisheInfoNew(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "discodes" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string discodes = dicPar["discodes"].ToString();
            string[] dis_arr = discodes.Split(';');
            string SqlStr = string.Empty;
            if (dis_arr.Length > 0)
            {
                for (var i = 0; i < dis_arr.Length; i++)
                {
                    string[] d_arr = dis_arr[i].Split(',');
                    if (i == dis_arr.Length - 1)
                    {
                        SqlStr += "select StoCode,discode,dbo.fngettbdishinfo(stocode,discode) as disnames from tb_dish where discode='" + d_arr[0] + "' and stocode='" + d_arr[1] + "';";
                    }
                    else
                    {
                        SqlStr += "select StoCode,discode,dbo.fngettbdishinfo(stocode,discode) as disnames from tb_dish where discode='" + d_arr[0] + "' and stocode='" + d_arr[1] + "' ";
                        SqlStr += " union all ";
                    }
                }
                dt = new bllPaging().GetDataTableInfoBySQL(SqlStr);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                ReturnListJson(dt);
            }
            else
            {
                ToCustomerJson("1", "暂无数据");
            }
        }

        /// <summary>
        /// 菜品出库
        /// </summary>
        /// <param name="dicPar"></param>
        private void  DishOutStock(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "ccname", "tablecode", "ordercode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string ccname = dicPar["ccname"].ToString();
            string tablecode = dicPar["tablecode"].ToString();
            string ordercode = dicPar["ordercode"].ToString();

            DataTable dtDish = new bllTB_Bill().GetOutStockDishByBillCode(GUID, USER_ID, ordercode, stocode);

            if (dtDish != null)
            {
                string jsonStock = string.Empty;
                foreach (DataRow dr in dtDish.Rows)
                {
                    //仓库编号，matcode，disnum
                    string jsonStockTemp = dr["WarCode"].ToString() + "," + dr["MatCode"].ToString() + "," + dr["DisNum"].ToString() + ";";
                    jsonStock += jsonStockTemp;
                }
                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/IStore/stock/WSStockSales.ashx";
                postStr = new StringBuilder();
                postStr.Append("actionname=stocksalestrue&parameters={" +
                                               string.Format("'GUID':'{0}'", GUID) +
                                               string.Format(",'USER_ID': '{0}'", USER_ID) +
                                               string.Format(",'buscode': '{0}'", buscode) +
                                               string.Format(",'stocode': '{0}'", stocode) +
                                               string.Format(",'stockjsons': '{0}'", jsonStock) +
                                               string.Format(",'cname': '{0}'", ccname) +
                                               string.Format(",'ordercode': '{0}'", ordercode) +
                                               string.Format(",'tablecode': '{0}'", tablecode) +
                                               string.Format(",'rancode': '{0}'", Guid.NewGuid().ToString()) +
                                       "}");//键值对
                string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, "出库："+strAdminJson + ";");
                string status = JsonHelper.GetJsonValByKey(strAdminJson, "status");
                if (status == "0")
                {
                    //更新账单的出入库信息
                    new bllTB_Bill().UpdateStockStatus(GUID, USER_ID, ordercode, stocode, "1");
                }
                ToJsonStr(strAdminJson);
            }
            else
            {
                ToErrorJson();
            }
        }

        /// <summary>
        /// 菜品入库
        /// </summary>
        /// <param name="dicPar"></param>
        private void DishInStock(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "ccname", "tablecode", "ordercode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            StringBuilder postStr = new StringBuilder();
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string ccname = dicPar["ccname"].ToString();
            string tablecode = dicPar["tablecode"].ToString();
            string ordercode = dicPar["ordercode"].ToString();

            DataTable dtDish = new bllTB_Bill().GetOutStockDishByBillCode(GUID, USER_ID, ordercode, stocode);
            if (dtDish != null)
            {
                string jsonStock = string.Empty;
                foreach (DataRow dr in dtDish.Rows)
                {
                    //仓库编号，matcode，disnum
                    string jsonStockTemp = dr["WarCode"].ToString() + "," + dr["MatCode"].ToString() + "," + dr["DisNum"].ToString() + ";";
                    jsonStock += jsonStockTemp;
                }
                string ShortMesUrl = Helper.GetAppSettings("ServiceUrl") + "/IStore/stock/WSStockSales.ashx";
                postStr.Append("actionname=stockrecoverytrue&parameters={" +
                                           string.Format("'GUID':'{0}'", GUID) +
                                           string.Format(",'USER_ID': '{0}'", USER_ID) +
                                           string.Format(",'buscode': '{0}'", buscode) +
                                           string.Format(",'stocode': '{0}'", stocode) +
                                           string.Format(",'stockjsons': '{0}'", jsonStock) +
                                           string.Format(",'cname': '{0}'", ccname) +
                                           string.Format(",'ordercode': '{0}'", ordercode) +
                                           string.Format(",'tablecode': '{0}'", tablecode) +
                                           string.Format(",'rancode': '{0}'", Guid.NewGuid().ToString()) +
                                   "}");//键值对
                string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                string status = JsonHelper.GetJsonValByKey(strAdminJson, "status");
                string stockinJson = ShortMesUrl + "?" + postStr.ToString();
                if (status == "0")
                {
                    //更新账单的出入库信息
                    new bllTB_Bill().UpdateStockStatus(GUID, USER_ID, ordercode, stocode, "0");
                }
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, "菜品入库：" + stockinJson);
                ToJsonStr(strAdminJson);
            }
            else
            {
                ToErrorJson();
            }
        }
        #endregion
    }
}