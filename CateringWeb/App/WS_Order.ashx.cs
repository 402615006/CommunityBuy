using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy
{
    /// <summary>
    /// 菜品信息接口类
    /// </summary>
    public class WS_Order :IServices.ServiceBase
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
                    switch (actionname.ToLower())
                    {
                        case "i_addorder"://下单
                            AddOrder(dicPar);
                            break;
                        case "i_getorder"://获取订单
                            GetOrder(dicPar);
                            break;
                        //case "i_cancelorder"://取消订单
                        //    CancelOrder(dicPar);
                        //    break;
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
            List<string> pra = new List<string>() { "key","buscode","stocode", "disnum", "distypenum", "remark", "ordermoney", "memcode", "dishcode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string opencode = dicPar["opencode"].ToString();
            string disnum = dicPar["disnum"].ToString();
            string distypenum = dicPar["distypenum"].ToString();
            string remark = dicPar["remark"].ToString();
            string ordermoney = dicPar["ordermoney"].ToString();
            string memcode = dicPar["memcode"].ToString();
            string discode = dicPar["dishlist"].ToString();
            string ordercode = "";
            string ordertype = "";
            string departcode = "";

            new bllTB_Order().Add(discode, "", "", "", buscode, stocode, memcode, "小程序", "1", out ordercode, opencode, ordermoney, disnum, distypenum, remark, "", "", ordertype, departcode);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        /// <summary>
        /// 获取订餐和菜品
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetOrder(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "memcode"};

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string opencode = dicPar["opencode"].ToString();
            string filter = " o.stocode='"+ stocode + "' and o.opencodelist='"+ opencode + "'";
            string order = "";
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = new bllTB_Order().GetOrderDisPagingListInfo("", "", 999999, 1, filter, order, out recordCount, out totalPage);

            ReturnListJson(dt,null,null,null,null);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="dicPar"></param>
        public void CancelOrder(Dictionary<string, object> dicPar)
        {
            string stocode = dicPar["stocode"].ToString();
            string ordercode = dicPar["ordercode"].ToString();

        }


    }
}