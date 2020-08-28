using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace CommunityBuy.BackWeb.ajax.dishes
{
    /// <summary>
    /// 门店_菜品类别接口类
    /// </summary>
    public class WSOrder : ServiceBase
    {
        bllTB_Order bll = new bllTB_Order();
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
                        case "addorder"://下单
                            AddOrder(dicPar);
                            break;
                        case "getorder"://获取订单
                            GetOrder(dicPar);
                            break;
                        case "cancelorder"://取消订单
                            CancelOrder(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dicPar"></param>
        public void AddOrder(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "stocode", "remark", "ordermoney", "memcode", "openid","nickname", "dishlist"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string stocode = dicPar["stocode"].ToString();
            string remark = dicPar["remark"].ToString();
            string ordermoney = dicPar["ordermoney"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string openid= dicPar["openid"].ToString();
            string dishlist = new JavaScriptSerializer().Serialize(dicPar["dishlist"]);
            string ordercode = "";
            string tstatus = "1";//1已下单0已取消2已完成
            int ordertype = 1;
            bll.Add(dishlist, "", "", "", stocode, memcode, openid, tstatus, out ordercode, ordermoney, remark, ordertype);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Data);

        }

        /// <summary>
        /// 获取订餐和菜品
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetOrder(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "opencode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string opencode = dicPar["opencode"].ToString();
            string filter = " o.stocode='" + stocode + "' and o.opencodelist='" + opencode + "'";
            string order = "";
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = new bllTB_Order().GetOrderDisPagingListInfo("", "", 999999, 1, filter, order, out recordCount, out totalPage);

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("code");
            dtResult.Columns.Add("ctime");
            dtResult.Columns.Add("money");
            dtResult.Columns.Add("disnum");
            dtResult.Columns.Add("distypenum");
            dtResult.Columns.Add("ordertype");
            dtResult.Columns.Add("dishList");
            dtResult.Columns.Add("tablename");
            dtResult.Columns.Add("cusnum");
            dtResult.Columns.Add("opentime");
            dtResult.Columns.Add("openman");
            dtResult.Columns.Add("status");
            if (dt != null)
            {

                DataTable dtOrder = dt.DefaultView.ToTable(true, "code");
                foreach (DataRow dr in dtOrder.Rows)
                {
                    DataTable dtDish = new DataTable();
                    dtDish.Columns.Add("discode");
                    dtDish.Columns.Add("disname");
                    dtDish.Columns.Add("price");
                    dtDish.Columns.Add("extraMoney");
                    dtDish.Columns.Add("disnum");
                    dtDish.Columns.Add("totalMoney");
                    dtDish.Columns.Add("uptype");
                    dtDish.Columns.Add("returnnum");
                    dtDish.Columns.Add("DiscountType");
                    dtDish.Columns.Add("DiscountPrice");
                    dtDish.Columns.Add("MemPrice");
                    dtDish.Columns.Add("CookMoney");
                    dtDish.Columns.Add("FinCode");
                    dtDish.Columns.Add("DisTypeCode");
                    dtDish.Columns.Add("CookName");
                    dtDish.Columns.Add("OrderCode");
                    dtDish.Columns.Add("kitcode");
                    dtDish.Columns.Add("typecode");
                    dtDish.Columns.Add("ispackage");
                    dtDish.Columns.Add("pdiscode");
                    dtDish.Columns.Add("IsMemPrice");
                    dtDish.Columns.Add("orderdiscode");

                    foreach (DataRow drTemp in dt.Select("code='" + dr["code"] + "'"))
                    {
                        DataRow drAdd = dtDish.NewRow();
                        drAdd["discode"] = drTemp["discode"];
                        drAdd["disname"] = drTemp["disname"];
                        drAdd["price"] = drTemp["price"];
                        drAdd["extraMoney"] = drTemp["extraMoney"];
                        drAdd["disnum"] = drTemp["disnum"];
                        drAdd["totalMoney"] = drTemp["totalmoney"];
                        drAdd["uptype"] = drTemp["uptype"];
                        drAdd["returnnum"] = drTemp["returnnum"];
                        drAdd["DiscountType"] = drTemp["DiscountType"];
                        drAdd["DiscountPrice"] = drTemp["DiscountPrice"];
                        drAdd["MemPrice"] = drTemp["MemPrice"];
                        drAdd["CookMoney"] = drTemp["CookMoney"];
                        drAdd["FinCode"] = drTemp["FinCode"];
                        drAdd["DisTypeCode"] = drTemp["DisTypeCode"];
                        drAdd["CookName"] = drTemp["CookName"];
                        drAdd["OrderCode"] = drTemp["OrderCode"];
                        drAdd["kitcode"] = drTemp["kitcode"];
                        drAdd["typecode"] = drTemp["typecode"];
                        drAdd["ispackage"] = drTemp["ispackage"];
                        drAdd["pdiscode"] = drTemp["pdiscode"];
                        drAdd["IsMemPrice"] = drTemp["IsMemPrice"];
                        drAdd["orderdiscode"] = drTemp["PkCode"];
                        dtDish.Rows.Add(drAdd);
                    }
                    string jsonDish = JsonHelper.DataTableToJSON(dtDish);

                    DataRow drResult = dtResult.NewRow();

                    DataRow drOrder = dt.Select("code='" + dr["code"] + "'")[0];
                    drResult["code"] = drOrder["code"];
                    drResult["ctime"] =StringHelper.StringToDateTime(drOrder["ctime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    drResult["money"] = drOrder["money"];
                    drResult["disnum"] = drOrder["disnum"];
                    drResult["distypenum"] = drOrder["distypenum"];
                    drResult["ordertype"] = drOrder["ordertype"];
                    drResult["tablename"] = drOrder["tablename"];
                    drResult["cusnum"] = drOrder["cusnum"];
                    drResult["opentime"] = drOrder["opentime"];
                    drResult["openman"] = drOrder["openman"];
                    drResult["status"] = drOrder["status"];
                    drResult["dishList"] = jsonDish;
                    dtResult.Rows.Add(drResult);
                }
            }
            ReturnListJson(dtResult, 1, 1, 1, 1);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="dicPar"></param>
        public void CancelOrder(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "ordercode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string ordercode = dicPar["ordercode"].ToString();

            //获取门店的付款方式
            bll.UpdateStatus("", "", ordercode, "0", stocode);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Data);
        }

    }
}