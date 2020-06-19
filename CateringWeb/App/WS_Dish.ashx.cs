using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using System.Web.UI;

namespace CommunityBuy
{
    /// <summary>
    /// 菜品信息接口类
    /// </summary>
    public class WS_Dish :IServices.ServiceBase
    {
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
                        case "i_getdishtaglist"://获取菜品标签列表
                            GetDisTagList(dicPar);
                            break;
                        case "i_getdishlist"://获取菜品列表
                            GetDisList(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取菜品标签列表
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetDisTagList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string filter = "stocode='" + stocode + "' and TStatus='1'";
            int recnum;
            int pagenum;
            DataTable dt=new bllTB_DishType().GetPagingListInfo("", "", 99999, 1, filter, "", out recnum, out pagenum);
            ReturnListJson(dt,int.MaxValue,recnum,1,recnum);
        }


        /// <summary>
        /// 获取菜品列表
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetDisList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode","tag","tablecode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string guid = "";
            string uid = "";

            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string type = dicPar["type"].ToString();
            int page = 0, pagesize = 0,recnum=0, pagenums = 0;

            string filter = "";
            string order = "";

            DataTable dt = new bllTB_Dish().GetPagingListInfo(guid, uid, pagesize, page, filter, order,out recnum,out pagenums);
            ReturnListJson(dt, null, null, null, null);
        }
    }
}