using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_StoreTodayReport 的摘要说明
    /// </summary>
    public class WS_StoreTodayReport : ServiceBase
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
                        case "storetodayreportlist"://列表
                            StoreTodayReportList(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 今日营业报表
        /// </summary>
        /// <param name="dicPar"></param>
        public void StoreTodayReportList(Dictionary<string, object> dicPar)
        {
            //判断时间
            List<string> pra = new List<string>() { "stocode", "starttime", "endtime" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string stocode = dicPar["stocode"].ToString();
            string starttime = dicPar["starttime"].ToString();
            string endtime = dicPar["endtime"].ToString();
            SqlParameter[] ps =
            {
                new SqlParameter("@stocode",stocode),
                new SqlParameter("@startdate",starttime),
                new SqlParameter("@enddate",endtime)
            };

            DataSet ds = new BLL.bllPaging().GetDatasetByProcedure("[dbo].[p_StoreTodayReportList]", ps);
            var json = string.Empty;
            if (ds != null)
            {

                var dt1 = ds.Tables[0];  //头部数据
                var dt2 = ds.Tables[1];  //消费项目头部数据
                var dt3 = ds.Tables[2];  //优惠券 使用金额，虚增金额
                var dt4 = ds.Tables[3];  //菜品类别详情
                var dt5 = ds.Tables[4];  //会员项目支付方式
                var dt6 = ds.Tables[5];  //会员销售项目

                double ymje = 0, zkje = 0, mlje = 0; //优免金额，折扣金额，抹零金额


                json += "[{";
                json += "\"sdate\":\"" + DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss") + "\",";
                json += "\"edate\":\"" + DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss") + "\",";


                #region 头部信息
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    ymje = Convert.ToDouble(dt1.Rows[0]["ymje"]);

                    if (dt1.Rows[0]["zkje"] is System.DBNull == false)
                        zkje = Convert.ToDouble(dt1.Rows[0]["zkje"]);

                    if (dt1.Rows[0]["mlje"] is System.DBNull == false)
                        mlje = Convert.ToDouble(dt1.Rows[0]["mlje"]);

                    json += "\"headdata\":{";
                    json += "\"xsze\":\"" + dt1.Rows[0][0].ToString() + "\",";
                    json += "\"lqbzl\":\"" + dt1.Rows[0][1].ToString() + "\",";
                    json += "\"cxse\":\"" + dt1.Rows[0][2].ToString() + "\",";
                    json += "\"lks\":\"" + dt1.Rows[0][3].ToString() + "\",";
                    json += "\"zds\":\"" + dt1.Rows[0][4].ToString() + "\",";
                    json += "\"mkdj\":\"" + dt1.Rows[0][5].ToString() + "\",";
                    json += "\"mddj\":\"" + dt1.Rows[0][6].ToString() + "\",";
                    json += "\"zsje\":\"" + dt1.Rows[0]["zsje"].ToString() + "\"";
                    json += "},";
                }

                #endregion

                #region 消费项目
                double skxmhj = 0;   //收款项目合计
                double xsxmhj = 0;  //销售项目合计

                json += "\"xfxmdata\":{";
                json += "\"data\":[";

                //收款支付类型
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    skxmhj = Convert.ToDouble(dt2.Compute("SUM(money)", "")) + Convert.ToDouble(dt3.Compute("SUM(RealPay)", ""));
                    json += "{\"title\":\"\",\"sum\":\"\",\"bigbfb\":\"0\",\"data1\":[";
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        var money = Convert.ToDouble(dt2.Rows[i]["money"]);
                        if (dt2.Rows[i]["PayMethodName"].ToString() == "优免")
                        {
                            skxmhj -= money;
                            continue;
                        }
                        var bfb = money / skxmhj;

                        json += "{";
                        json += "\"name\":\"" + dt2.Rows[i]["PayMethodName"].ToString() + "\",";
                        json += "\"skxm\":\"" + money + "\",";
                        json += "\"xssm\":\"\",";
                        json += "\"bfb\":\"" + bfb.ToString("P") + "\"";
                        json += "},";
                    }

                    //优惠券 使用金额，虚增金额
                    if (dt3.Rows.Count > 0)
                    {
                        //优惠券
                        var money = Convert.ToDouble(dt3.Rows[0]["RealPay"]);
                        var bfb = money / skxmhj;
                        json += "{";
                        json += "\"name\":\"优惠券\",";
                        json += "\"skxm\":\"" + money + "\",";
                        json += "\"xssm\":\"\",";
                        json += "\"bfb\":\"" + bfb.ToString("P") + "\"";
                        json += "},";

                        //虚增金额
                        //var xzmoney = Convert.ToDouble(dt3.Rows[0]["VIMoney"]);
                        //var xzbfb = xzmoney / skxmhj;
                        //json += "{";
                        //json += "\"name\":\"优惠券虚增\",";
                        //json += "\"skxm\":\"" + xzmoney + "\",";
                        //json += "\"xssm\":\"\",";
                        //json += "\"bfb\":\"" + xzbfb.ToString("P") + "\"";
                        //json += "},";

                    }
                    if(ymje==0)
                    {
                        //优免
                        json += "{";
                        json += "\"name\":\"优免\",";
                        json += "\"skxm\":\"\",";
                        json += "\"xssm\":\"\",";
                        json += "\"bfb\":\"0%\"";
                        json += "},";
                    }
                    else
                    {
                        //优免
                        json += "{";
                        json += "\"name\":\"优免\",";
                        json += "\"skxm\":\"\",";
                        json += "\"xssm\":\"" + (ymje*-1) + "\",";
                        json += "\"bfb\":\"0%\"";
                        json += "},";
                    }
                    
                    if(mlje==0)
                    {
                        //抹零
                        json += "{";
                        json += "\"name\":\"抹零\",";
                        json += "\"skxm\":\"\",";
                        json += "\"xssm\":\"\",";
                        json += "\"bfb\":\"0%\"";
                        json += "},";
                    }
                    else
                    {
                        //抹零
                        json += "{";
                        json += "\"name\":\"抹零\",";
                        json += "\"skxm\":\"\",";
                        json += "\"xssm\":\"" + (mlje*-1) + "\",";
                        json += "\"bfb\":\"0%\"";
                        json += "},";
                    }

                    if(zkje==0)
                    {
                        //折扣
                        json += "{";
                        json += "\"name\":\"折扣\",";
                        json += "\"skxm\":\"\",";
                        json += "\"xssm\":\"\",";
                        json += "\"bfb\":\"0%\"";
                        json += "}";
                    }
                    else
                    {
                        //折扣
                        json += "{";
                        json += "\"name\":\"折扣\",";
                        json += "\"skxm\":\"\",";
                        json += "\"xssm\":\"" + (zkje*-1) + "\",";
                        json += "\"bfb\":\"0%\"";
                        json += "}";
                    }

                    json += "]},";

                }

                //消费项目菜品类别
                if (dt4 != null && dt4.Rows.Count > 0)
                {

                    string filter = "";
                    for (int i = 0; i < dt4.Columns.Count; i++)
                    {
                        if (i < dt4.Columns.Count - 1)
                            filter += dt4.Columns[i].ColumnName + " IS NULL AND ";
                        else
                            filter += dt4.Columns[i].ColumnName + " IS NULL";
                    }
                    var rows = dt4.Select(filter);
                    for (int i = 0; i < rows.Length; i++)
                    {
                        dt4.Rows.Remove(rows[i]);
                    }


                    xsxmhj = Convert.ToDouble(dt4.Compute("SUM(price)", "")) - ymje - mlje - zkje;
                    var bigTypeList = dt4.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["BtypeName"].ToString()); //所有大类别
                    for (int i = 0; i < bigTypeList.Count(); i++)
                    {
                        var tempV = bigTypeList.ElementAt(i);
                        var bigType = tempV.Key;

                        var tempSumR = dt4.Select("BtypeName='" + bigType + "'");
                        var Sum = Convert.ToDouble(tempSumR.Sum(x => x.Field<decimal>("price")));
                        var bbfb = Sum / xsxmhj;
                        json += "{\"title\":\"" + bigType + "\",\"sum\":\"" + Sum + "\",\"bigbfb\":\"" + bbfb.ToString("P") + "\",\"data1\":[";

                        for (int h = 0; h < tempSumR.Count(); h++)
                        {
                            var money = Convert.ToDouble(tempSumR[h]["price"]);

                            var bfb = money / xsxmhj;
                            json += "{";
                            json += "\"name\":\"" + tempSumR[h]["TypeName"].ToString() + "\",";
                            json += "\"skxm\":\"\",";
                            json += "\"xssm\":\"" + money + "\",";
                            json += "\"bfb\":\"" + bfb.ToString("P") + "\"";
                            json += "},";
                        }
                        json = json.TrimEnd(',');
                        json += "]},";
                    }
                    json = json.TrimEnd(',');
                }
                else
                {
                    json = json.TrimEnd(',');
                }

                json += "]";

                json += ",\"skxmhj\":\"" + skxmhj + "\",";
                json += "\"xsxmhj\":\"" + xsxmhj + "\"";

                json += "},";
                #endregion

                #region 会员项目
                json += "\"vipdata\":{\"v_data\":[";
                double v_skxmhj = 0;   //收款项目合计
                double v_xsxmhj = 0;  //销售项目合计

                //会员收款方式
                if (dt5 != null && dt5.Rows.Count > 0)
                {
                    v_skxmhj = Convert.ToDouble(dt5.Compute("SUM(payamount)", ""));
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        var money = Convert.ToDouble(dt5.Rows[i]["payamount"]);
                        var bfb = money / v_skxmhj;
                        json += "{";
                        json += "\"name\":\"" + dt5.Rows[i]["PayMethodName"].ToString() + "\",";
                        json += "\"skxm\":\"" + money + "\",";
                        json += "\"xssm\":\"\",";
                        json += "\"bfb\":\"" + bfb.ToString("P") + "\"";
                        json += "},";
                    }
                }

                //会员销售项目
                if (dt6 != null && dt6.Rows.Count > 0)
                {
                    var sum = Convert.ToDouble(dt6.Compute("SUM(payamount)", ""));
                    var cardcost = Convert.ToDouble(dt6.Compute("SUM(cardcost)", "")); //工本费
                    v_xsxmhj = sum + cardcost;

                    //客用会员卡信息  --cardtype 8：员工卡  food:美食城卡  <>8,food 客用会员卡
                    var kyDr = dt6.Select("cardtype<>'8' and cardtype<>'food'");
                    if (kyDr.Count() > 0)
                    {
                        var kymoney = Convert.ToDouble(kyDr.Sum(x => x.Field<decimal>("payamount")));
                        var bfb = kymoney / v_xsxmhj;
                        json += "{";
                        json += "\"name\":\"客用会员卡\",";
                        json += "\"skxm\":\"\",";
                        json += "\"xssm\":\"" + kymoney + "\",";
                        json += "\"bfb\":\"" + bfb.ToString("P") + "\"";
                        json += "},";

                        var kycost = Convert.ToDouble(kyDr.Sum(x => x.Field<decimal>("cardcost")));  //客用会员卡工本费
                        if (kycost > 0)
                        {
                            bfb = kycost / v_xsxmhj;
                            json += "{";
                            json += "\"name\":\"客用会员卡工本费\",";
                            json += "\"skxm\":\"\",";
                            json += "\"xssm\":\"" + kycost + "\",";
                            json += "\"bfb\":\"" + bfb.ToString("P") + "\"";
                            json += "},";
                        }
                    }

                    //员工卡信息
                    var ygDr = dt6.Select("cardtype='8'");
                    if (ygDr.Count() > 0)
                    {
                        var ygmoney = Convert.ToDouble(ygDr.Sum(x => x.Field<decimal>("payamount")));
                        var bfb = ygmoney / v_xsxmhj;
                        json += "{";
                        json += "\"name\":\"员工卡\",";
                        json += "\"skxm\":\"\",";
                        json += "\"xssm\":\"" + ygmoney + "\",";
                        json += "\"bfb\":\"" + bfb.ToString("P") + "\"";
                        json += "},";
                    }

                    //美食卡信息
                    var msDr = dt6.Select("cardtype='food'");
                    if (msDr.Count() > 0)
                    {
                        var msmoney = Convert.ToDouble(msDr.Sum(x => x.Field<decimal>("payamount")));
                        var bfb = msmoney / v_xsxmhj;
                        json += "{";
                        json += "\"name\":\"美食卡\",";
                        json += "\"skxm\":\"\",";
                        json += "\"xssm\":\"" + msmoney + "\",";
                        json += "\"bfb\":\"" + bfb.ToString("P") + "\"";
                        json += "},";

                        var mscost = Convert.ToDouble(msDr.Sum(x => x.Field<decimal>("cardcost")));
                        if (mscost > 0)
                        {
                            bfb = mscost / v_xsxmhj;
                            json += "{";
                            json += "\"name\":\"美食卡工本费-押金\",";
                            json += "\"skxm\":\"\",";
                            json += "\"xssm\":\"" + mscost + "\",";
                            json += "\"bfb\":\"" + bfb.ToString("P") + "\"";
                            json += "},";
                        }
                    }

                }
                json = json.TrimEnd(',');
                json += "],";
                json += "\"v_skxmhj\":\"" + v_skxmhj + "\",";
                json += "\"v_xsxmhj\":\"" + v_xsxmhj + "\"";
                json += "}";
                #endregion

                json += "}]"; //结束json
            }
            ToJsonStr(json);

        }


    }
}