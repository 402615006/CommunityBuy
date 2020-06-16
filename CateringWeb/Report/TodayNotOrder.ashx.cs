using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_StoreTodayReport 的摘要说明
    /// </summary>
    public class TodayNotOrder : ServiceBase
    {

        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    switch (actionname.ToLower())
                    {
                        case "gettodaynotorder"://列表
                            GetTodayNotOrder(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 今日已结订单
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetTodayNotOrder(Dictionary<string, object> dicPar)
        {
            //判断时间
            List<string> pra = new List<string>() { "userid", "stocode", "billtype" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string userid = dicPar["userid"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string billtype = dicPar["billtype"].ToString();
            DateTime sdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 09:00:00");
            DateTime edate = DateTime.Parse(DateTime.Now.AddDays(+1).ToString("yyyy-MM-dd") + " 09:00:00");
            SqlParameter[] ps =
            {
                new SqlParameter("@stocode",stocode),
                new SqlParameter("@billtype",billtype),
                new SqlParameter("@startdate",sdate),
                new SqlParameter("@enddate",edate)
            };

            DataSet ds = new BLL.bllPaging().GetDatasetByProcedure("[dbo].[p_TodayNotOrder]", ps);
            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));//订单类型
            dt_Result.Columns.Add("field6", typeof(string));//点单号
            dt_Result.Columns.Add("field2", typeof(string));//结帐号
            dt_Result.Columns.Add("field2_url", typeof(string));//结帐号跳转
            dt_Result.Columns.Add("field3", typeof(string));//桌台号
            dt_Result.Columns.Add("field4", typeof(string));//时间
            dt_Result.Columns.Add("field5", typeof(string));//金额

            ArrayList arrayList = new ArrayList();
            List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题

            Title_list1.Add(new LayUItableHelper { Field = "field1", Title = "订单类型", Align = "center", TotalRowText = "合计",width=-1 });
            Title_list1.Add(new LayUItableHelper { Field = "field6", Title = "点单号", Align = "center", width = -1 });
            Title_list1.Add(new LayUItableHelper { Field = "field2", Title = "结帐号", Align = "center", HeadTemplet = "field2_url", Style = "cursor: pointer;color: #0000FF", width = -1 });
            Title_list1.Add(new LayUItableHelper { Field = "field3", Title = "桌台号", Align = "center", width = -1 });
            Title_list1.Add(new LayUItableHelper { Field = "field4", Title = "时间", Align = "center", width = -1 });
            Title_list1.Add(new LayUItableHelper { Field = "field5", Title = "金额", Align = "center", TotalRow = true, width = -1 });
            arrayList.Add(Title_list1);//第一行表头

            if (ds != null && ds.Tables.Count == 1)
            {
                DataTable dt_order = ds.Tables[0];
                if (dt_order != null && dt_order.Rows.Count > 0)
                {
                    DataTable dtStore = GetCacheToStore(userid);
                    foreach (DataRow dr in dt_order.Rows)
                    {
                        string drstocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + drstocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + drstocode + "'")[0];
                            if (dr_sto != null)
                            {
                                dr["StoName"] = dr_sto["cname"].ToString();
                            }
                        }
                        DataRow newdr = dt_Result.NewRow();
                        newdr["field1"] = dr["BillTypeName"].ToString();
                        newdr["field2_url"] = "";
                        newdr["field6"] = dr["OrderCode"].ToString();
                        newdr["field2"] = dr["BillCode"].ToString();
                        newdr["field3"] = dr["TableName"].ToString();
                        newdr["field4"] = dr["CTime"].ToString();
                        newdr["field5"] = dr["BillMoney"].ToString();
                        dt_Result.Rows.Add(newdr);
                    }
                }
            }
            string json1 = new LayUItableHelper().GetLayUITableHead(arrayList);
            string json2 = JsonHelper.DataTableToJSON(dt_Result);
            string json = "{";
            json += "\"title\":[" + json1 + "],";
            json += "\"data\":" + json2;
            json += "}";
            Pagcontext.Response.Write(json);

        }

    }
}