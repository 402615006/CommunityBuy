using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Cache;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// getuser 的摘要说明
    /// </summary>
    public class store : ServiceBase
    {
        bllStore bll = new bllStore();

        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    switch (actionname)
                    {
                        case "getstore"://获取用户气泡提示
                            GetStore(dicPar);
                            break;
                    }
                }
            }
        }

        private void GetStore(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "page", "limit","x","y"};
            //检测方法需要的参数
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            int pageSize = StringHelper.StringToInt(dicPar["limit"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            double lat= StringHelper.StringToDouble(dicPar["x"].ToString());
            double lng = StringHelper.StringToDouble(dicPar["y"].ToString());
            //string filter = dicPar["filter"].ToString();
            //filter = CombinationFilter(new List<string>() { "buscode","stocode","pdistypecode","distypecode","dispath","distypename","metcode","fincode","maxdiscount","busSort","status","cuser","uuser" }, dicPar, typeof(string), filter);
            //string order = dicPar["order"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            DataTable dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, "", "", out recordCount, out totalPage);
            dt.Columns.Add("distance");
            foreach (DataRow dr in dt.Rows)
            {
                double stolat = StringHelper.StringToDouble(dr["stocoordx"].ToString());
                double stolng = StringHelper.StringToDouble(dr["stocoordy"].ToString());
                double distance = Helper.GetDistance(stolat, stolng, lat, lng);
                dr["distance"] = distance.ToString("f0");
            }
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}