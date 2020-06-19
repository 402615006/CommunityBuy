using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using System.Collections;
using System.Drawing;
using System;
namespace CommunityBuy.BackWeb.ajax.dishes
{
    /// <summary>
    /// 门店_菜品接口类
    /// </summary>
    public class WSdishes : ServiceBase
    {
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
                        case "getlistbymeal":
                            GetListByMeal(dicPar);
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
                        //case "getselectinfo"://修改状态
                        //GetSelectInfo(dicPar);
                        //break;
                        case "getimgstring"://获取照片（二进制字符串返回）
                            getimgstring(dicPar);
                            break;
                        case "getremoteimgstring"://获取服务器图片
                            getremoteimgstring(dicPar);
                            break;
                        case "deletepackage": //删除套餐
                            DeletePackage(dicPar);
                            break;
                        case "getprice":
                            GetPrice(dicPar);
                            break;
                        case "updateprice":
                            UpdateDishesPrice(dicPar); //修改菜价
                            break;
                        case "dishesexport":
                            DishesExport(dicPar);
                            break;
                        case "detail1"://详细
                            Detail1(dicPar);
                            break;

                    }
                }
            }
        }

        //菜品导出
        private void DishesExport(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "currentPage", "pageSize", "filter", "order", "stocode", "type" };
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
            string filter = dicPar["filter"].ToString();
            string order = dicPar["order"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string type = dicPar["type"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, string.Empty, stocode, 0, out recordCount, out totalPage);
            if (dt != null && dt.Rows.Count > 0)
            {
                //dt.Columns.Add("RowNumber", typeof(string));
                dt.Columns.Add("iscanmodifypricename", typeof(string));
                dt.Columns.Add("isneedweighname", typeof(string));
                dt.Columns.Add("isneedmethodname", typeof(string));
                dt.Columns.Add("iscaninventoryname", typeof(string));
                dt.Columns.Add("iscancustomname", typeof(string));
                dt.Columns.Add("isallowmemberpricename", typeof(string));
                dt.Columns.Add("isattachcalculatename", typeof(string));
                dt.Columns.Add("isclipcouponsname", typeof(string));
                dt.Columns.Add("iscandepositname", typeof(string));
                dt.Columns.Add("isnonoperatingname", typeof(string));
                dt.Columns.Add("statusname", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["disname"].ToString().Length > 0 && dt.Rows[i]["iscombo"].ToString() == "0")
                    {
                        //dt.Rows[i]["disname"] = dt.Rows[i]["disname"].ToString() + "(" + dt.Rows[i]["unit"].ToString() + ")";
                        //去掉名称后单位
                        dt.Rows[i]["disname"] = dt.Rows[i]["disname"].ToString();
                    }
                    //dt.Rows[i]["RowNumber"] = (i + 1).ToString();
                    dt.Rows[i]["iscanmodifypricename"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["iscanmodifyprice"].ToString());
                    dt.Rows[i]["isneedweighname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["isneedweigh"].ToString());
                    dt.Rows[i]["isneedmethodname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["isneedmethod"].ToString());
                    dt.Rows[i]["iscaninventoryname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["iscaninventory"].ToString());
                    dt.Rows[i]["iscancustomname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["iscancustom"].ToString());
                    dt.Rows[i]["isallowmemberpricename"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["isallowmemberprice"].ToString());
                    dt.Rows[i]["isattachcalculatename"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["isattachcalculate"].ToString());
                    dt.Rows[i]["isclipcouponsname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["isclipcoupons"].ToString());
                    dt.Rows[i]["iscandepositname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["iscandeposit"].ToString());
                    dt.Rows[i]["isnonoperatingname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.eBool), dt.Rows[i]["isnonoperating"].ToString());

                    if (dt.Rows[i]["disid"].ToString().Length > 0)
                    {
                        dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                    }
                }

                AsponseHelper asponse = new AsponseHelper();
                string tempFile = HttpContext.Current.Request.PhysicalApplicationPath + "\\templateData\\";
                if (type == "0")
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
                if (type == "0")
                {
                    html = asponse.GetExportOPenfileByFilePath("菜品套餐", tempFile, ds, dt);
                }
                else
                {
                    html = asponse.GetExportOPenfileByFilePath("菜品信息", tempFile, ds, dt);
                }
                ToJsonStr("{\"status\":\"0\",\"mes\":\"操作成功\",\"data\":[{\"filepath\":\"" + html + "\"}]}");
            }
            else
            {
                ToJsonStr("{\"status\":\"1\",\"mes\":\"暂无数据\"}");
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            dishesEntityList objdishesEntityList = new dishesEntityList();

            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "pageSize", "currentPage", "filter", "order", "stocode" };
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
            string filter = dicPar["filter"].ToString();
            string order = dicPar["order"].ToString();
            string stocode = dicPar["stocode"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, string.Empty, stocode, 0, out recordCount, out totalPage);
            if (dt != null)
            {
                dt.Columns.Add("statusname", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["disname"].ToString().Length > 0 && dt.Rows[i]["iscombo"].ToString() == "0")
                    {
                        dt.Rows[i]["disname"] = dt.Rows[i]["disname"].ToString() + "(" + dt.Rows[i]["unit"].ToString() + ")";
                    }
                    if (dt.Rows[i]["disid"].ToString().Length > 0)
                    {
                        dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                    }
                }
            }
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void GetListByMeal(Dictionary<string, object> dicPar)
        {
            dishesEntityList objdishesEntityList = new dishesEntityList();

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
            string filter = dicPar["filter"].ToString();
            string order = dicPar["order"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfoByMeal(GUID, USER_ID, pageSize, currentPage, filter, order, string.Empty, string.Empty, 0, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "discode", "disname", "disothername", "distypecode", "quickcode", "customcode", "unit", "price", "memberprice", "ismultiprice", "costprice", "iscostbyingredient", "pushmoney", "matclscode", "matcode", "extcode", "fincode", "dcode", "kitcode", "ecode", "maketime", "qrcode", "dispicture", "remark", "isentity", "entitydefcount", "entityprice", "iscanmodifyprice", "isneedweigh", "isneedmethod", "iscaninventory", "iscancustom", "iscandeposit", "isallowmemberprice", "isattachcalculate", "isclipcoupons", "iscombo", "iscombooptional", "isnonoperating", "status", "busSort", "warcode", "cuser", "uuser",
                "isdelete",
                "dishesMethodsJson", // 菜品做法加价
                "dishesMatesJson", // 菜品所用配料
                "dishesMealsJson", // 菜品所属菜谱
                "dishescombosJson", // 套餐标配菜品
                "dishesoptionalsJson" // 套餐可选餐品
            };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string buscode = Helper.GetAppSettings("BusCode");
            string stocode = dicPar["stocode"].ToString();
            string discode = dicPar["discode"].ToString();
            string disname = dicPar["disname"].ToString();
            string disothername = dicPar["disothername"].ToString();
            string distypecode = dicPar["distypecode"].ToString();
            string quickcode = dicPar["quickcode"].ToString();
            string customcode = dicPar["customcode"].ToString();
            string unit = dicPar["unit"].ToString();
            string price = dicPar["price"].ToString();
            string memberprice = dicPar["memberprice"].ToString();
            string ismultiprice = dicPar["ismultiprice"].ToString();
            string costprice = dicPar["costprice"].ToString();
            string iscostbyingredient = dicPar["iscostbyingredient"].ToString();
            string pushmoney = dicPar["pushmoney"].ToString();
            string matclscode = dicPar["matclscode"].ToString();
            string matcode = dicPar["matcode"].ToString();
            string extcode = dicPar["extcode"].ToString();
            string fincode = dicPar["fincode"].ToString();
            string dcode = dicPar["dcode"].ToString();
            string kitcode = dicPar["kitcode"].ToString();
            string ecode = dicPar["ecode"].ToString();
            string maketime = dicPar["maketime"].ToString();
            string qrcode = dicPar["qrcode"].ToString();
            string dispicture = dicPar["dispicture"].ToString();
            string remark = dicPar["remark"].ToString();
            string isentity = dicPar["isentity"].ToString();
            string entitydefcount = dicPar["entitydefcount"].ToString();
            string entityprice = dicPar["entityprice"].ToString();
            string iscanmodifyprice = dicPar["iscanmodifyprice"].ToString();
            string isneedweigh = dicPar["isneedweigh"].ToString();
            string isneedmethod = dicPar["isneedmethod"].ToString();
            string iscaninventory = dicPar["iscaninventory"].ToString();
            string iscancustom = dicPar["iscancustom"].ToString();
            string iscandeposit = dicPar["iscandeposit"].ToString();
            string isallowmemberprice = dicPar["isallowmemberprice"].ToString();
            string isattachcalculate = dicPar["isattachcalculate"].ToString();
            string isclipcoupons = dicPar["isclipcoupons"].ToString();
            string iscombo = dicPar["iscombo"].ToString();
            string iscombooptional = dicPar["iscombooptional"].ToString();
            string isnonoperating = dicPar["isnonoperating"].ToString();
            string status = "0";//dicPar["status"].ToString();
            string busSort = dicPar["busSort"].ToString();
            string warcode = dicPar["warcode"].ToString();
            string cuser = LoginedUser.UserInfo.Id.ToString();
            string uuser = LoginedUser.UserInfo.Id.ToString();
            string isdelete = dicPar["isdelete"].ToString();
            string melcode = dicPar["melcode"] == null ? "" : dicPar["melcode"].ToString();

            string dishesMethodsJson = dicPar["dishesMethodsJson"].ToString();// 菜品做法加价
            string dishesMatesJson = dicPar["dishesMatesJson"].ToString(); // 菜品所用配料
            string dishesMealsJson = dicPar["dishesMealsJson"].ToString();// 菜品所属菜谱
            string dishescombosJson = dicPar["dishescombosJson"].ToString(); // 套餐标配菜品
            string dishesoptionalsJson = dicPar["dishesoptionalsJson"].ToString(); // 套餐可选餐品

            //调用逻辑
            logentity.pageurl = "dishesEdit.html";
            logentity.logcontent = "新增门店_菜品信息";

            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.LSAdd(GUID, USER_ID, buscode, stocode, discode, disname, disothername, distypecode, quickcode, customcode, unit, price, memberprice, ismultiprice, costprice, iscostbyingredient, pushmoney, matclscode, matcode, extcode, fincode, dcode, kitcode, ecode, maketime, qrcode, dispicture, remark, isentity, entitydefcount, entityprice, iscanmodifyprice, isneedweigh, isneedmethod, iscaninventory, iscancustom, iscandeposit, isallowmemberprice, isattachcalculate, isclipcoupons, iscombo, iscombooptional, isnonoperating, status, busSort, warcode, cuser, uuser, isdelete
                , melcode, dishesMethodsJson, dishesMatesJson, dishesMealsJson, dishescombosJson, dishesoptionalsJson, entity);
            ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode", "discode", "disname", "disothername", "distypecode", "quickcode", "customcode", "unit", "price", "memberprice", "ismultiprice", "costprice", "iscostbyingredient", "pushmoney", "matclscode", "matcode", "extcode", "fincode", "dcode", "kitcode", "ecode", "maketime", "qrcode", "dispicture", "remark", "isentity", "entitydefcount", "entityprice", "iscanmodifyprice", "isneedweigh", "isneedmethod", "iscaninventory", "iscancustom", "iscandeposit", "isallowmemberprice", "isattachcalculate", "isclipcoupons", "iscombo", "iscombooptional", "isnonoperating", "status", "busSort", "warcode", "cuser", "uuser",
                "dishesMethodsJson", // 菜品做法加价
                "dishesMatesJson", // 菜品所用配料
                "dishesMealsJson", // 菜品所属菜谱
                "dishescombosJson", // 套餐标配菜品
                "dishesoptionalsJson" // 套餐可选餐品
            };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string buscode = Helper.GetAppSettings("BusCode");
            string stocode = dicPar["stocode"].ToString();
            string discode = dicPar["discode"].ToString();
            string disname = dicPar["disname"].ToString();
            string disothername = dicPar["disothername"].ToString();
            string distypecode = dicPar["distypecode"].ToString();
            string quickcode = dicPar["quickcode"].ToString();
            string customcode = dicPar["customcode"].ToString();
            string unit = dicPar["unit"].ToString();
            string price = dicPar["price"].ToString();
            string memberprice = dicPar["memberprice"].ToString();
            string ismultiprice = dicPar["ismultiprice"].ToString();
            string costprice = dicPar["costprice"].ToString();
            string iscostbyingredient = dicPar["iscostbyingredient"].ToString();
            string pushmoney = dicPar["pushmoney"].ToString();
            string matclscode = dicPar["matclscode"].ToString();
            string matcode = dicPar["matcode"].ToString();
            string extcode = dicPar["extcode"].ToString();
            string fincode = dicPar["fincode"].ToString();
            string dcode = dicPar["dcode"].ToString();
            string kitcode = dicPar["kitcode"].ToString();
            string ecode = dicPar["ecode"].ToString();
            string maketime = dicPar["maketime"].ToString();
            string qrcode = dicPar["qrcode"].ToString();
            string dispicture = dicPar["dispicture"].ToString();
            string remark = dicPar["remark"].ToString();
            string isentity = dicPar["isentity"].ToString();
            string entitydefcount = dicPar["entitydefcount"].ToString();
            string entityprice = dicPar["entityprice"].ToString();
            string iscanmodifyprice = dicPar["iscanmodifyprice"].ToString();
            string isneedweigh = dicPar["isneedweigh"].ToString();
            string isneedmethod = dicPar["isneedmethod"].ToString();
            string iscaninventory = dicPar["iscaninventory"].ToString();
            string iscancustom = dicPar["iscancustom"].ToString();
            string iscandeposit = dicPar["iscandeposit"].ToString();
            string isallowmemberprice = dicPar["isallowmemberprice"].ToString();
            string isattachcalculate = dicPar["isattachcalculate"].ToString();
            string isclipcoupons = dicPar["isclipcoupons"].ToString();
            string iscombo = dicPar["iscombo"].ToString();
            string iscombooptional = dicPar["iscombooptional"].ToString();
            string isnonoperating = dicPar["isnonoperating"].ToString();
            string status = dicPar["status"].ToString();
            string busSort = dicPar["busSort"].ToString();
            string warcode = dicPar["warcode"].ToString();
            string cuser = LoginedUser.UserInfo.Id.ToString();
            string uuser = LoginedUser.UserInfo.Id.ToString();

            string dishesMethodsJson = dicPar["dishesMethodsJson"].ToString();// 菜品做法加价
            string dishesMatesJson = dicPar["dishesMatesJson"].ToString(); // 菜品所用配料
            string dishesMealsJson = dicPar["dishesMealsJson"].ToString();// 菜品所属菜谱
            string dishescombosJson = dicPar["dishescombosJson"].ToString(); // 套餐标配菜品
            string dishesoptionalsJson = dicPar["dishesoptionalsJson"].ToString(); // 套餐可选餐品
            string melcode = dicPar["melcode"] == null ? "" : dicPar["melcode"].ToString();

            //调用逻辑
            logentity.pageurl = "dishesEdit.html";
            logentity.logcontent = "修改id为:" + discode + "的门店_菜品信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Edit;
            dt = bll.LSUpdate(GUID, USER_ID, buscode, stocode, discode, disname, disothername, distypecode, quickcode, customcode, unit, price, memberprice, ismultiprice, costprice, iscostbyingredient, pushmoney, matclscode, matcode, extcode, fincode, dcode, kitcode, ecode, maketime, qrcode, dispicture, remark, isentity, entitydefcount, entityprice, iscanmodifyprice, isneedweigh, isneedmethod, iscaninventory, iscancustom, iscandeposit, isallowmemberprice, isattachcalculate, isclipcoupons, iscombo, iscombooptional, isnonoperating, status, busSort, warcode, cuser, uuser, melcode, dishesMethodsJson, dishesMatesJson, dishesMealsJson, dishescombosJson, dishesoptionalsJson, logentity);
            ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "discode" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string discode = dicPar["discode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            DataSet ds = bll.GetDataSetPagingListInfoByInterfaceNew(GUID, USER_ID, 1, 1, " discode='" + discode + "' and stocode='"+stocode+"'", "", out recordCount, out totalPage);
            if (ds != null && ds.Tables.Count > 0)
            {
                dishesEntityList list = new dishesEntityList();
                list.status = "0";
                DataTable dtdishes = ds.Tables[0];//菜品信息
                discode = dtdishes.Rows[0]["discode"].ToString();
                DataTable dtDishesMate = ds.Tables[1];//菜品所用配料
                DataTable dtStockMateUnits = ds.Tables[2];//菜品所用配料单位信息
                if (dtdishes.Rows.Count > 0)
                {
                    string nowdiscode = dtdishes.Rows[0]["discode"].ToString();
                    //菜品信息
                    dishesEntity dishes = EntityHelper.GetEntityByDR<dishesEntity>(dtdishes.Rows[0], null);

                    //菜品所用配料
                    DataRow[] rowsDishesMate = dtDishesMate.Select("discode='" + nowdiscode + "'");
                    if (rowsDishesMate.Length > 0)
                    {
                        for (int j = 0; j < rowsDishesMate.Length; j++)
                        {
                            string matcode = rowsDishesMate[j]["matcode"].ToString();
                            //原料对象
                            DishesMateEntity DMate = EntityHelper.GetEntityByDR<DishesMateEntity>(rowsDishesMate[j], null);
                            //对象单位信息
                            DataRow[] rowsStockMateUnits = dtStockMateUnits.Select("matcode='" + matcode + "' and stocode='"+ stocode + "' and isminunit='1'");
                            if(rowsStockMateUnits.Length==0)
                            {
                                rowsStockMateUnits = dtStockMateUnits.Select("matcode='" + matcode + "' and stocode='12' and isminunit='1'");
                            }
                            DMate.StockMateUnits = EntityHelper.GetEntityListByDR<StockMateUnitsEntity>(rowsStockMateUnits, null);
                            List<StockMateUnitsEntity> MateList = new List<StockMateUnitsEntity>();
                            foreach (StockMateUnitsEntity me in DMate.StockMateUnits)
                            {
                                if (MateList.FindIndex(A => A.matcode == matcode && A.matunitcode == me.matunitcode) < 0)
                                {
                                    MateList.Add(me);
                                }
                            }
                            DMate.StockMateUnits = MateList;
                            dishes.DishesMates.Add(DMate);
                        }
                    }

                    list.data.Add(dishes);
                }
                ReturnJsonByT<dishesEntityList>(list);
            }
            else
            {
                ToErrorJson();
            }
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "discode" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string discode = dicPar["discode"].ToString();
            //调用逻辑
            logentity.pageurl = "dishesList.html";
            logentity.logcontent = "删除id为:" + discode + "的门店_菜品信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.Delete(GUID, USER_ID, discode, entity);
            ReturnListJson(dt);
        }

        private void DeletePackage(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "discode" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string discode = dicPar["discode"].ToString();
            //调用逻辑
            logentity.pageurl = "dishesList.html";
            logentity.logcontent = "删除id为:" + discode + "的门店_套餐可选商品及套餐标配";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            dt = bll.DeletePackage(GUID, USER_ID, discode, entity);
            ReturnListJson(dt);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "discode", "status" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string status = dicPar["status"].ToString();

            string discode = dicPar["discode"].ToString().Trim(',');
            logentity.pageurl = "dishesList.html";
            logentity.logcontent = "修改状态id为:" + discode + "的门店_菜品信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            DataTable dt = bll.UpdateStatus(GUID, USER_ID, discode, status);

            ReturnListJson(dt);
        }

        private void GetSelectInfo(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "discode", "status" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string status = dicPar["status"].ToString();

            string discode = dicPar["discode"].ToString().Trim(',');
            logentity.pageurl = "dishesList.html";
            logentity.logcontent = "修改状态id为:" + discode + "的门店_菜品信息";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            DataSet ds = bll.GetSelectInfo(GUID, USER_ID, discode, status);
            if (ds != null && ds.Tables.Count > 0)
            {
                ArrayList list = new ArrayList();
                string[] Names = new string[] { "a", "", "", "" };
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    list.Add(dt);
                }
                ReturnListJson("0", string.Empty, list, Names, null, null, null, null);
            }
            else
            {
                ReturnListJson("-1", string.Empty, null, null, null, null, null, null);
            }

        }

        /// <summary>
        /// 获取照片（二进制字符串返回）
        /// </summary>
        /// <param name="dicPar"></param>
        /// <returns></returns>
        private void getimgstring(Dictionary<string, object> dicPar)
        {
            string strImg = string.Empty;
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "discode" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string discode = dicPar["discode"].ToString();

            Image headImg = null;
            blldishes objblldishes = new blldishes();
            string filter = string.Format(" discode = '{0}'", discode);
            DataTable dtData = null;
            try
            {
                dtData = objblldishes.GetPagingSigInfo(GUID, USER_ID, filter, string.Empty, string.Empty, 0);
                if (dtData != null && dtData.Rows.Count > 0 && dtData.Rows[0]["dispicture"] != DBNull.Value && !string.IsNullOrEmpty(dtData.Rows[0]["dispicture"].ToString()))
                {
                    headImg = Image.FromFile(dtData.Rows[0]["dispicture"].ToString());
                    if (headImg != null)
                    {
                        strImg = Helper.ImageToString(headImg);
                    }
                    headImg = null;
                }
            }
            catch { }
            Pagcontext.Response.Write(strImg);
        }

        /// <summary>
        /// 获取远程照片（二进制字符串返回）
        /// </summary>
        /// <param name="dicPar"></param>
        /// <returns></returns>
        private void getremoteimgstring(Dictionary<string, object> dicPar)
        {
            string strImg = string.Empty;
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "path", "filename" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string path = dicPar["path"].ToString();
            string filename = dicPar["filename"].ToString();

            string strFullPath = path;
            if (!string.IsNullOrEmpty(strFullPath) && strFullPath.Trim() != "")
            {
                strFullPath += @"\" + filename;
            }
            else
            {
                strFullPath = filename;
            }

            Image headImg = null;
            blldishes objblldishes = new blldishes();
            try
            {
                string RealPath = HttpContext.Current.Server.MapPath("../" + strFullPath);
                headImg = Image.FromFile(RealPath);
                if (headImg != null)
                {
                    strImg = Helper.ImageToString(headImg);
                }
                headImg = null;
            }
            catch { }
            Pagcontext.Response.Write(strImg);
        }

        public void GetPrice(Dictionary<string, object> dicPar)
        {
            string GUID = "0";
            string USER_ID = "0";
            string discode = dicPar["discode"].ToString();
            string depcode = string.Empty;//部门编号
            //if (dicPar["depcode"] != null)
            //{
            //    depcode = dicPar["depcode"].ToString();
            //}

            DataSet ds = bll.GetPrice(GUID, USER_ID, discode, depcode);
            if (ds != null && ds.Tables.Count == 2)
            {
                DataTable dtdishes = ds.Tables[0];
                DataTable dtDishesMeal = ds.Tables[1];
                dishesEntityList list = new dishesEntityList();
                for (int i = 0; i < dtdishes.Rows.Count; i++)
                {
                    dishesEntity dishesentity = EntityHelper.GetEntityByDR<dishesEntity>(dtdishes.Rows[i], null); ;
                    DataRow[] DRDishesMeal = null;
                    //多菜谱
                    if (dtdishes.Rows[i]["ismultiprice"].ToString() == "1")
                    {
                        DRDishesMeal = dtDishesMeal.Select("discode='" + dtdishes.Rows[i]["discode"].ToString() + "'");
                        if (DRDishesMeal.Length > 0)
                        {
                            dishesentity.DishesMeals = EntityHelper.GetEntityListByDR<DishesMealEntity>(DRDishesMeal, null);
                        }
                    }
                    list.data.Add(dishesentity);
                }
                ReturnJsonByT(list);
            }
            else
            {
                ToErrorJson();
            }
        }

        /// <summary>
        /// 修改菜品价格
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateDishesPrice(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() {
                "GUID",
                "USER_ID",
                "dishesPriceUpdateLogJson",
                "dispriceJson"
            };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            string GUID = "0";
            string USER_ID = "0";
            string dishesPriceUpdateLogJson = dicPar["dishesPriceUpdateLogJson"].ToString();
            string dispriceJson = dicPar["dispriceJson"].ToString();

            //调用逻辑
            logentity.pageurl = "EditDishesPrice.html";
            logentity.logcontent = "菜价修改";
            logentity.cuser = StringHelper.StringToLong(USER_ID);
            logentity.otype = SystemEnum.LogOperateType.Add;
            dt = bll.UpdatePrice(GUID, USER_ID, dishesPriceUpdateLogJson, dispriceJson, entity);
            ReturnListJson(dt);
        }

        private void Detail1(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "discode" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            string discode = dicPar["discode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            DataSet ds = bll.GetDataSetPagingListInfoByInterface11(GUID, USER_ID, 1, 1, " discode='" + discode + "' and stocode='" + stocode + "'", "", out recordCount, out totalPage);
            if (ds != null && ds.Tables.Count > 0)
            {
                dishesEntityList list = new dishesEntityList();
                list.status = "0";
                DataTable dtdishes = ds.Tables[0];//菜品信息
                discode = dtdishes.Rows[0]["discode"].ToString();
                DataTable dtDishesMate = ds.Tables[1];//菜品所用配料
                DataTable dtStockMateUnits = ds.Tables[2];//菜品所用配料单位信息
                if (dtdishes.Rows.Count > 0)
                {
                    string nowdiscode = dtdishes.Rows[0]["discode"].ToString();
                    //菜品信息
                    dishesEntity dishes = EntityHelper.GetEntityByDR<dishesEntity>(dtdishes.Rows[0], null);

                    //菜品所用配料
                    DataRow[] rowsDishesMate = dtDishesMate.Select("discode='" + nowdiscode + "'");
                    if (rowsDishesMate.Length > 0)
                    {
                        for (int j = 0; j < rowsDishesMate.Length; j++)
                        {
                            string matcode = rowsDishesMate[j]["matcode"].ToString();
                            //原料对象
                            DishesMateEntity DMate = EntityHelper.GetEntityByDR<DishesMateEntity>(rowsDishesMate[j], null);
                            //对象单位信息
                            DataRow[] rowsStockMateUnits = dtStockMateUnits.Select("matcode='" + matcode + "' and stocode='" + stocode + "' and isminunit='1'  and matunitcode='" + rowsDishesMate[j]["unitcode"].ToString() + "'");
                            if (rowsStockMateUnits.Length == 0)
                            {
                                rowsStockMateUnits = dtStockMateUnits.Select("matcode='" + matcode + "' and stocode='12' and isminunit='1'  and matunitcode='" + rowsDishesMate[j]["unitcode"].ToString() + "'");
                            }
                            DMate.StockMateUnits = EntityHelper.GetEntityListByDR<StockMateUnitsEntity>(rowsStockMateUnits, null);
                            List<StockMateUnitsEntity> MateList = new List<StockMateUnitsEntity>();
                            foreach (StockMateUnitsEntity me in DMate.StockMateUnits)
                            {
                                if (MateList.FindIndex(A => A.matcode == matcode && A.matunitcode == me.matunitcode) < 0)
                                {
                                    MateList.Add(me);
                                }
                            }
                            DMate.StockMateUnits = MateList;
                            dishes.DishesMates.Add(DMate);
                        }
                    }
                    list.data.Add(dishes);
                }
                ReturnJsonByT<dishesEntityList>(list);
            }
            else
            {
                ToErrorJson();
            }
        }


    }
}