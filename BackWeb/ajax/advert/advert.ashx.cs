using CommunityBuy.BackWeb.ajax;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CommunityBuy.BackWeb.ajax.advert
{
    /// <summary>
    /// advert 的摘要说明
    /// </summary>
    public class advert :ServiceBase
    {
        bllActivityAd bll = new bllActivityAd();
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

                    }
                }
            }
        }


        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "type", "page", "pagesize" };
            if (!CheckActionParameters(dicPar, pra))
            {
                ReturnResultJson("0", "参数错误");
            }
            int pageSize = StringHelper.StringToInt(dicPar["pagesize"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            string type = dicPar["type"].ToString();
            string filter = " status=1 ";
            string order = "";
            switch (type)
            { 
                case "1"://首页轮播
                    filter += " and type=1 ";
                    pageSize = int.MaxValue;
                    currentPage = 1;
                break;
                case "2"://首页轮播
                    filter += " and type=2 ";
                    pageSize = int.MaxValue;
                    currentPage = 1;
                    break;
            }

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            DataTable dt = bll.GetPagingListInfo("","",pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

    }
}