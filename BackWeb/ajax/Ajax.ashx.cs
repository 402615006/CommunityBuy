using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// Ajax 的摘要说明
    /// </summary>
    public class Ajax : IHttpHandler
    {
        bllPaging bll = new bllPaging();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string actionname = Helper.ReplaceString(context.Request["actionname"]);
            switch (actionname.ToLower())
            {
                case "addmaterialscost":
                    AddMCost(context);
                    break;
                case "addmaterialsincome":
                    AddIncome(context);
                    break;
                case "showcounttips":
                    showCountTips(context);
                    break;

            }
        }

        /// <summary>
        /// 添加调整成本
        /// </summary>
        /// <param name="c"></param>
        public void AddMCost(HttpContext c)
        {
            var para = c.Request["parameters"];

            JavaScriptSerializer jss = new JavaScriptSerializer();
            object obj = jss.DeserializeObject(para);
            Dictionary<string, object> dicPar = (Dictionary<string, object>)obj;

            var stocode = dicPar["stocode"];
            var warcode = dicPar["warcode"];
            var month = dicPar["month"];
            var param = dicPar["p1"];
            var edittype = dicPar["edittype"].ToString();
            try
            {
                var sql = "select Id from MaterialsCost where stocode='" + stocode + "' and warcode='" + warcode + "' and month='" + month + "'";

                var Id = bll.ExecuteScalarBySQL(sql);
                if (!string.IsNullOrEmpty(Id)) //如果存在则更新
                {
                    if (edittype == "m")
                        sql = "update MaterialsCost set money='" + param + "' where Id='" + Id + "'";
                    else
                        sql = "update MaterialsCost set remark='" + param + "' where Id='" + Id + "'";
                }
                else
                {
                    if (edittype == "m")
                        sql = "insert into MaterialsCost(stocode,warcode,money,month) values ('" + stocode + "','" + warcode + "','" + param + "','" + month + "')";
                    else
                        sql = "insert into MaterialsCost(stocode,warcode,remark,month) values ('" + stocode + "','" + warcode + "','" + param + "','" + month + "')";
                }
                bll.ExecuteScalarBySQL(sql);
                c.Response.Clear();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Response.End();
            }
        }

        /// <summary>
        /// 添加调整成本
        /// </summary>
        /// <param name="c"></param>
        public void AddIncome(HttpContext c)
        {
            var para = c.Request["parameters"];

            JavaScriptSerializer jss = new JavaScriptSerializer();
            object obj = jss.DeserializeObject(para);
            Dictionary<string, object> dicPar = (Dictionary<string, object>)obj;

            var stocode = dicPar["stocode"];
            var warcode = dicPar["warcode"];
            var month = dicPar["month"];
            var param = dicPar["p1"];
            var edittype = dicPar["edittype"].ToString();
            try
            {
                var sql = "select Id from MaterialsIncome where stocode='" + stocode + "' and warcode='" + warcode + "' and month='" + month + "'";

                var Id = bll.ExecuteScalarBySQL(sql);
                if (!string.IsNullOrEmpty(Id)) //如果存在则更新
                {
                    sql = "update MaterialsIncome set money='" + param + "' where Id='" + Id + "'";
                }
                else
                {
                    sql = "insert into MaterialsIncome(stocode,warcode,money,month) values ('" + stocode + "','" + warcode + "','" + param + "','" + month + "')";
                }
                bll.ExecuteScalarBySQL(sql);
                c.Response.Clear();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Response.End();
            }
        }

        public void showCountTips(HttpContext c)
        {
            var para = c.Request["parameters"];

            JavaScriptSerializer jss = new JavaScriptSerializer();
            object obj = jss.DeserializeObject(para);
            Dictionary<string, object> dicPar = (Dictionary<string, object>)obj;

            var year = Convert.ToInt32(dicPar["year"]);
            var stocode = dicPar["stocode"];

            bllPaging bll = new bllPaging();
            var btime = new DateTime(year, 1, 1);
            var etime = btime.AddYears(1);

            var strWhere = string.Empty;
            var tsql = string.Empty;
            if (!string.IsNullOrEmpty(stocode.ToString()))
            {
                strWhere = " and stocode='" + stocode + "'";
                tsql = "SELECT [dbo].[fnGetStoYearSumCostMoney]('" + stocode + "','" + btime.AddHours(9) + "');";
            }
            else
            {
                var tempSql = "SELECT STUFF((SELECT ','+stocode FROM R_DataSource GROUP BY stocode  FOR XML PATH('')),1,1,'')";
                var stolist = bll.ExecuteScalarBySQL(tempSql);
                if (!string.IsNullOrEmpty(stolist))
                {
                    tsql = string.Empty;
                    var stolists = stolist.Split(',');
                    for (int i = 0; i < stolists.Length; i++)
                    {
                        tsql += "SELECT [dbo].[fnGetStoYearSumCostMoney]('" + stolists[i] + "','" + btime.AddHours(9) + "')";
                        if (i != stolists.Length - 1) //如果不是最后一行
                            tsql += " UNION ALL ";
                    }
                    tsql += ";";
                }
            }

            var sql = "SELECT SUM(disaccount),dtype FROM dbo.R_DataSource where dday>='" + btime + "' and dday<'" + etime + "' " + strWhere + " GROUP BY  dtype;"; //总收入
            sql += "SELECT ISNULL(SUM(bdtarget),0) FROM AnnualTarget WHERE years=" + year + "" + strWhere + ";"; //收入目标
            sql += tsql; //总成本

            decimal yzsr = 0, ysrmb = 0, wcl = 0, zcb = 0, zlr = 0, fycb = 0, scfy = 0, lrmb = 0, lrmbwcl = 0;
            try
            {

                var ds = bll.GetDataSetInfoBySQL(sql);
                var sumdt = ds.Tables[0];

                //年度总收入
                var t1 = sumdt.Select(" dtype=1");
                if (t1.Count() > 0)
                {
                    yzsr = Convert.ToDecimal(t1[0][0]);
                }

                //总收入目标
                var t2 = ds.Tables[1];
                if (t2 != null)
                {
                    if (t2.Rows.Count > 0)
                    {
                        ysrmb = Convert.ToDecimal(t2.Rows[0][0]);
                    }
                }

                //收入目标完成率
                if (ysrmb > 0)
                {
                    wcl = Math.Round(yzsr / ysrmb, 4);
                }

                //总成本
                var t3 = ds.Tables[2];
                if (t3 != null)
                {
                    if (t3.Rows.Count > 0)
                    {
                        for (int x = 0; x < t3.Rows.Count; x++)
                        {
                            zcb += Convert.ToDecimal(t3.Rows[x][0]);
                        }
                    }
                }

                //费用成本
                var t4 = sumdt.Select(" dtype=4");
                if (t4.Count() > 0)
                {
                    fycb = Convert.ToDecimal(t4[0][0]);
                }

                //生产费用
                var t5 = sumdt.Select(" dtype=5");
                if (t5.Count() > 0)
                {
                    scfy = Convert.ToDecimal(t5[0][0]);
                }

                zlr = yzsr - (zcb + fycb + scfy);  //总利润=总收入-总成本-费用成本-生产费用

                StringBuilder shtml = new StringBuilder();

                shtml.Append("<table>");
                shtml.Append("<tr><td>总收入:<b>" + yzsr.toStringN() + "</b></td><td>收入目标:<b>" + ysrmb.toStringN() + "</b></td></tr>");
                shtml.Append("<tr><td>收入目标完成率:<b>" + wcl.ToString("P") + "</b></td><td>总利润:<b>" + zlr.toStringN() + "</b></td></tr>");
                shtml.Append("</table>");

                c.Response.Clear();
                c.Response.Write(shtml.ToString());
            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Response.End();
            }

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