using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using System.Collections;
using System.Drawing;
using System;
using System.Web.UI.WebControls;
using Image = System.Drawing.Image;

namespace CommunityBuy.BackWeb.ajax.dishes
{
    /// <summary>
    /// 门店_菜品接口类
    /// </summary>
    public class WSdishes : ServiceBase
    {
        DataTable dt = new DataTable();
        bllTB_Dish bll = new bllTB_Dish();
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
                        case "update"://修改							
                            Update(dicPar);
                            break;
                        case "delete"://删除
                            Delete(dicPar);
                            break;
                        case "updatestatus"://修改状态
                            UpdateStatus(dicPar);
                            break;
                        case "dishesexport":
                            DishesExport(dicPar);
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
            bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
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
                ReturnJsonStr("{\"status\":\"0\",\"mes\":\"操作成功\",\"data\":[{\"filepath\":\"" + html + "\"}]}");
            }
            else
            {
                ReturnJsonStr("{\"status\":\"1\",\"mes\":\"暂无数据\"}");
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
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
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
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
            string unit = dicPar["unit"].ToString();
            string price = dicPar["price"].ToString();
            string costprice = dicPar["costprice"].ToString();
            string qrcode = dicPar["qrcode"].ToString();
            string status = "0";//dicPar["status"].ToString();
            //调用逻辑
            bll.Add(GUID,USER_ID, buscode, stocode,status,"", discode, disname, disothername, distypecode, quickcode, unit, price, costprice,qrcode,"");
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "buscode", "stocode","disname", "disothername", "distypecode", "quickcode", "unit", "price", "costprice", "remark", "status" };
            //检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";

            string stocode = dicPar["stocode"].ToString();
            string discode = dicPar["discode"].ToString();
            string disname = dicPar["disname"].ToString();
            string disothername = dicPar["disothername"].ToString();
            string distypecode = dicPar["distypecode"].ToString();
            string quickcode = dicPar["quickcode"].ToString();

            string unit = dicPar["unit"].ToString();
            string price = dicPar["price"].ToString();

            string costprice = dicPar["costprice"].ToString();

            string remark = dicPar["remark"].ToString();

            string status = dicPar["status"].ToString();


            TB_DishEntity UEntity = bll.GetEntitySigInfo("where discode='"+ discode + "' and stocode='"+ stocode + "'");
            UEntity.DisName = disname;
            UEntity.OtherName = disothername;
            UEntity.TypeCode = distypecode;
            UEntity.Unit = unit;
            UEntity.Price = StringHelper.StringToDecimal(price);
            UEntity.QuickCode = quickcode;
            UEntity.CostPrice = StringHelper.StringToDecimal(costprice);
            UEntity.Descript = remark;
            UEntity.TStatus = status;


            //调用逻辑
            bll.Update(GUID, USER_ID,UEntity);
            ReturnResultJson(bll.oResult.Code,bll.oResult.Msg);
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
            bll.Delete(GUID, USER_ID, discode);
            ReturnResultJson(bll.oResult.Code,bll.oResult.Msg);
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
            string codes = dicPar["discode"].ToString();

            bll.UpdateStatus(GUID, USER_ID,codes, status);

            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

    }
}