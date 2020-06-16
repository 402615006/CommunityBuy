using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CommunityBuy.CommonBasic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using CommunityBuy.BLL;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WSTB_BusinessAnalysisReport 的摘要说明
    /// </summary>
    public class WSTB_BusinessAnalysisReport : ServiceBase
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
                        case "getlist"://列表
                            GetList(dicPar);
                            break;

                        case "getsumlist": //门店合计
                            GetSumListNew(dicPar);
                            break;

                        case "getbackorder": //退单列表
                            BackOrderList(dicPar);
                            break;

                        case "getbackdetails": //退单详情
                            BackOrderDetails(dicPar);
                            break;

                        case "getsalelist": //销售报表
                            GetSaleList(dicPar);
                            break;

                        case "getsaledetail": //销售明细报表
                            GetSaleDetails(dicPar);
                            break;

                        case "getgivelist"://赠送列表
                            GetGiveList(dicPar);
                            break;

                        case "givedetail": //赠送详情
                            GvieDetails(dicPar);
                            break;

                        case "getroylist"://提成人销售明细
                            GetRoyList(dicPar);
                            break;

                        case "getshiftinfo": //开班信息表
                            GetShiftInfo(dicPar);
                            break;

                        case "getsdzk": //手动折扣信息
                            GetZkList(dicPar);
                            break;

                        case "getzkdetails": //根据折扣名称查询手动折扣详情
                            GetZkDetailsByZkName(dicPar);
                            break;

                        case "getzkbyaucode": //根据用户查询手动折扣详情
                            GetZkDetailsByAuCode(dicPar);
                            break;
                        case "getcpxsslhzlist":
                            GetcpxsslhzList(dicPar);//菜品销售数量汇总
                            break;
                        case "getcpxsslhzmxlist":
                            GetcpxsslhzmxList(dicPar);//菜品销售数量汇总明细
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 门店营业报表
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();


            SqlParameter[] ps =
            {
                new SqlParameter("@stocode",stocode),
                new SqlParameter("@startdate",btime),
                new SqlParameter("@enddate",etime)
            };

            DataSet ds = new BLL.bllPaging().GetDatasetByProcedure("[dbo].[p_IncomeDetails]", ps);
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
                        //2019-12-24 19:56分,郭晓玲说去掉优免的
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
                    //优免
                    json += "{";
                    json += "\"name\":\"优免\",";
                    json += "\"skxm\":\"\",";
                    json += "\"xssm\":\"" + (ymje * -1) + "\",";
                    json += "\"bfb\":\"0%\"";
                    json += "},";

                    //抹零
                    json += "{";
                    json += "\"name\":\"抹零\",";
                    json += "\"skxm\":\"\",";
                    json += "\"xssm\":\"" + (mlje * -1) + "\",";
                    json += "\"bfb\":\"0%\"";
                    json += "},";

                    //折扣
                    json += "{";
                    json += "\"name\":\"折扣\",";
                    json += "\"skxm\":\"\",";
                    json += "\"xssm\":\"" + (zkje * -1) + "\",";
                    json += "\"bfb\":\"0%\"";
                    json += "}";

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

        /// <summary>
        /// 门店营业报表 横版
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetSumListNew(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            if (string.IsNullOrEmpty(stocode)) //如果门店为空则查询所有有数据的门店
            {
                if (!string.IsNullOrEmpty(btime) && !string.IsNullOrEmpty(etime))
                {
                    stocode = new BLL.bllPaging().ExecuteScalarBySQL("SELECT STUFF((SELECT DISTINCT ','+stocode FROM TH_OpenShift where Etime>='" + btime + "' and Etime<='" + etime + "' FOR XML PATH('')),1,1,'') stocode");
                }
                else
                {
                    return;
                }
            }
            string stocodes = stocode;
            SqlParameter[] ps =
            {
                new SqlParameter("@stocode",stocode),
                new SqlParameter("@startdate",btime),
                new SqlParameter("@enddate",etime)
            };


            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));//门店编号
            dt_Result.Columns.Add("field2", typeof(string));//门店名称
            dt_Result.Columns.Add("field3", typeof(string));//日期
            dt_Result.Columns.Add("field4", typeof(string));//赠送金额
            dt_Result.Columns.Add("field5", typeof(string));//发票金额
            dt_Result.Columns.Add("h_hj", typeof(string));//合计

            ArrayList arrayList = new ArrayList();
            List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题
            List<LayUItableHelper> Title_list2 = new List<LayUItableHelper>();//第二行标题

            //数据
            DataSet ds = new BLL.bllPaging().GetDatasetByProcedure("[dbo].[p_IncomeDetailsSum]", ps);

            var dt1 = ds.Tables[0];  //头部数据
            var dt2 = ds.Tables[1];  //营业收入头部数据
            var dt3 = ds.Tables[2];  //优惠券 使用金额，虚增金额
            var dt4 = ds.Tables[3];  //菜品类别详情
            var dt5 = ds.Tables[4];  //会员项目支付方式
            var dt6 = ds.Tables[5];  //会员销售项目
            var dt7 = ds.Tables[6];  //有数据的门店(需要循环的门店)

            var dt8 = ds.Tables[7]; //赠送金额
            var dt9 = ds.Tables[8]; //抹零、折扣
            var dt10 = ds.Tables[9]; //优免

            var stodt = new BLL.bllStore().GetAllStore("where 1=1");

            if (ds != null)
            {

                Title_list1.Add(new LayUItableHelper { Fixed = "left", Sort = true, Field = "field1", Title = "门店编号", Align = "center", TotalRowText = "合计", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Fixed = "left", Sort = false, Field = "field2", Title = "门店名称", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field3", Title = "日期", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field4", Title = "赠送金额", Align = "center", Rowspan = 2, TotalRow = true });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field5", Title = "发票金额", Align = "center", Rowspan = 2, TotalRow = true });

                int width = 130;

                bool ky = false, ygk = false, msk = false, gbf = false, msgbf = false;

                #region 表头

                IEnumerable<IGrouping<string, DataRow>> list_ys = null, list_xf = null, list_hysk = null, list_hy = null;

                #region 营业收入表头
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    list_ys = dt2.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["PayMethodName"].ToString()); //第二行标题

                    int colspan = 5;
                    if (dt2.Select("PayMethodName='优免'").Length > 0)
                    {
                        colspan = 4;
                    }
                    Title_list1.Add(new LayUItableHelper { Sort = false, Field = "ys", Title = "营业收入", Align = "center", Colspan = colspan + list_ys.Count(), Rowspan = 1 });
                    for (int i = 0; i < list_ys.Count(); i++)
                    {
                        var temp = list_ys.ElementAt(i);
                        var Val = temp.Key;
                        if (Val == "优免")
                        {
                            continue;
                        }
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "ys_" + Val, Title = Val, Align = "center", TotalRow = true, width = width });
                        dt_Result.Columns.Add("ys_" + Val, typeof(string));
                    }

                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "qs", Title = "签送", Align = "center", TotalRow = true, width = width });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "qhs", Title = "券回收", Align = "center", TotalRow = true, width = width });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "wx", Title = "微信小程序", Align = "center", TotalRow = true, width = width });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "yhq", Title = "优惠券", Align = "center", TotalRow = true, width = width });
                    //Title_list2.Add(new LayUItableHelper { Sort = false, Field = "yhqxz", Title = "优惠券虚增金额", Align = "center", TotalRow = true, width = width });

                    dt_Result.Columns.Add("qs", typeof(string)); //签送
                    dt_Result.Columns.Add("qhs", typeof(string)); //券回收
                    dt_Result.Columns.Add("wx", typeof(string)); //微信小程序
                    dt_Result.Columns.Add("yhq", typeof(string));//优惠券
                    //dt_Result.Columns.Add("yhqxz", typeof(string));//优惠券虚增金额
                }
                #endregion

                #region 消费项目表头
                if (dt4 != null && dt4.Rows.Count > 0)
                {
                    list_xf = dt4.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["typename"].ToString()); //第二行标题
                    Title_list1.Add(new LayUItableHelper { Sort = false, Field = "xf", Title = "消费项目", Align = "center", Colspan = 4 + list_xf.Count(), Rowspan = 1 });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "ym", Title = "优免", Align = "center", TotalRow = true, width = width });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "ml", Title = "抹零", Align = "center", TotalRow = true, width = width });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "zk", Title = "折扣", Align = "center", TotalRow = true, width = width });
                    for (int i = 0; i < list_xf.Count(); i++)
                    {
                        var temp = list_xf.ElementAt(i);
                        var Val = temp.Key;
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "xf_" + Val, Title = Val, Align = "center", TotalRow = true, width = width });
                        dt_Result.Columns.Add("xf_" + Val, typeof(string));
                    }

                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "xfxj", Title = "小计", Align = "center", Style = "cursor: pointer;color: #0000FF", TotalRow = true, width = width });

                    dt_Result.Columns.Add("ym", typeof(string));
                    dt_Result.Columns.Add("ml", typeof(string));
                    dt_Result.Columns.Add("zk", typeof(string));
                    dt_Result.Columns.Add("xfxj", typeof(string));
                }
                #endregion

                #region 会员收款项目表头
                if (dt5 != null && dt5.Rows.Count > 0)
                {
                    list_hysk = dt5.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["PayMethodName"].ToString());
                    if (list_hysk.Count() <= 1)
                    {
                        Title_list1.Add(new LayUItableHelper { Sort = false, Field = "hysktitle", Title = "会员收款项目", Align = "center" });
                    }
                    else
                    {
                        Title_list1.Add(new LayUItableHelper { Sort = false, Field = "hysktitle", Title = "会员收款项目", Align = "center", Colspan = list_hysk.Count(), Rowspan = 1 });
                    }

                    for (int i = 0; i < list_hysk.Count(); i++)
                    {
                        var temp = list_hysk.ElementAt(i);
                        var Val = temp.Key;
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "hysk_" + Val, Title = Val, Align = "center", TotalRow = true, width = width });
                        dt_Result.Columns.Add("hysk_" + Val, typeof(string));
                    }
                }
                #endregion

                #region 会员项目表头
                if (dt6 != null && dt6.Rows.Count > 0)
                {
                    list_hy = dt6.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["cardtype"].ToString());
                    Title_list1.Add(new LayUItableHelper { Sort = false, Field = "hytitle", Title = "会员项目", Align = "center", Colspan = list_hy.Count() + 1, Rowspan = 1 });

                    //客用会员卡
                    DataRow[] memrows = dt6.Select("cardtype<>'8' and cardtype<>'food'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "hy_ky", Title = "客用会员卡", Align = "center", TotalRow = true, width = width });
                        dt_Result.Columns.Add("hy_ky", typeof(string));
                        ky = true;
                    }

                    //员工卡
                    memrows = dt6.Select("cardtype='8'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "hy_ygk", Title = "员工卡", Align = "center", TotalRow = true, width = width });
                        dt_Result.Columns.Add("hy_ygk", typeof(string));
                        ygk = true;
                    }

                    //美食卡
                    memrows = dt6.Select("cardtype='food'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "hy_food", Title = "美食卡", Align = "center", TotalRow = true, width = width });
                        dt_Result.Columns.Add("food", typeof(string));
                        msk = true;
                    }

                    //客用会员卡工本费
                    decimal cost = dt6.Select().Sum(x => x.Field<decimal>("cardcost"));
                    if (cost != 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "hy_gbf", Title = "客用会员卡工本费", Align = "center", TotalRow = true, width = width });
                        dt_Result.Columns.Add("cardcost", typeof(string));
                        gbf = true;
                    }

                    //美食卡工本费-押金
                    cost = dt6.Select("cardtype='food'").Sum(x => x.Field<decimal>("cardcost"));
                    if (cost != 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "hy_foodgbf", Title = "美食卡工本费-押金", Align = "center", TotalRow = true, width = width });
                        dt_Result.Columns.Add("hy_foodgbf", typeof(string));
                        msgbf = true;
                    }

                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "hyxj", Title = "小计", Align = "center", Style = "cursor: pointer;color: #0000FF", TotalRow = true, width = width });
                    dt_Result.Columns.Add("hyxj", typeof(string));
                }
                #endregion

                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "h_hj", Title = "合计", Align = "center", Colspan = 1, Rowspan = 2 });

                arrayList.Add(Title_list1);//第一行表头
                arrayList.Add(Title_list2);//第二行表头
                dt_Result.AcceptChanges();

                #endregion

                #region 数据

                if (dt7 != null && dt7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt7.Rows.Count; i++) //循环门店
                    {
                        DataRow newdr = dt_Result.NewRow();
                        stocode = dt7.Rows[i][0].ToString();
                        newdr["field1"] = stocode;
                        newdr["field2"] = Convert.ToString(stodt.Select("stocode='" + stocode + "'")[0]["cname"]);
                        newdr["field3"] = Convert.ToDateTime(btime).ToString("yyyy.MM.dd") + "-" + Convert.ToDateTime(etime).ToString("yyyy.MM.dd");
                        newdr["field4"] = "0";
                        newdr["field5"] = "0";

                        decimal xfxmxj = 0, hyxmxj = 0;

                        #region 营业收入数据
                        if (list_ys != null && list_ys.Count() > 0)
                        {
                            for (int h = 0; h < list_ys.Count(); h++)
                            {
                                var temp = list_ys.ElementAt(h);
                                var Key = temp.Key;
                                if (Key == "优免")
                                {
                                    continue;
                                }

                                decimal Val = 0;

                                try
                                {
                                    Val = Convert.ToDecimal(dt2.Select("PayMethodName='" + Key + "' and StoCode='" + stocode + "'")[0][0]);
                                    if (stocode != "08" && Key == "优惠券虚增金额")
                                    {
                                        Val = 0;
                                    }
                                }
                                catch (Exception)
                                {
                                    Val = 0;
                                }
                                try
                                {
                                    newdr["ys_" + Key] = Val;
                                }
                                catch (Exception)
                                {

                                }

                            }

                            decimal yhq = 0, yhqxz = 0, qs = 0;

                            try
                            {
                                var temprows = dt3.Select("StoCode='" + stocode + "'");
                                yhq = Convert.ToDecimal(temprows[0]["RealPay"]);
                                yhqxz = Convert.ToDecimal(temprows[0]["VIMoney"]);
                            }
                            catch (Exception)
                            {

                            }

                            try
                            {
                                qs = Convert.ToDecimal(dt8.Select("StoCode='" + stocode + "'")[0][0]);
                            }
                            catch (Exception)
                            {
                                qs = 0;
                            }



                            newdr["qs"] = qs;
                            newdr["qhs"] = "0"; //券回收
                            newdr["wx"] = "0";  //微信小程序
                            newdr["yhq"] = yhq;
                            //if (stocode == "08")
                            //{
                            //    newdr["yhqxz"] = yhqxz;
                            //}
                        }
                        #endregion

                        #region 消费项目数据
                        if (list_xf != null && list_xf.Count() > 0)
                        {

                            try
                            {
                                xfxmxj = dt4.Select("StoCode='" + stocode + "'").Sum(x => x.Field<decimal>("price"));
                            }
                            catch (Exception ex)
                            {
                                xfxmxj = 0;
                            }

                            for (int h = 0; h < list_xf.Count(); h++)
                            {
                                var temp = list_xf.ElementAt(h);
                                var Key = temp.Key;

                                decimal Val = 0;

                                try
                                {
                                    Val = Convert.ToDecimal(dt4.Select("typename='" + Key + "' and StoCode='" + stocode + "'")[0]["price"]);
                                }
                                catch (Exception)
                                {
                                    Val = 0;
                                }
                                newdr["xf_" + Key] = Val;
                            }

                            decimal ym = 0, ml = 0, zk = 0;

                            try
                            {
                                var temp = dt9.Select("StoCode='" + stocode + "'");
                                ml = Convert.ToDecimal(temp[0]["ZeroCutMoney"]);
                                zk = Convert.ToDecimal(temp[0]["DiscountMoney"]);
                            }
                            catch (Exception)
                            {

                            }

                            try
                            {
                                ym = Convert.ToDecimal(dt10.Select("StoCode='" + stocode + "'")[0][0]);
                            }
                            catch (Exception)
                            {

                            }

                            newdr["ym"] = (ym * -1);
                            newdr["zk"] = (zk * -1);
                            newdr["ml"] = (ml * -1);
                            xfxmxj = xfxmxj - Math.Abs(zk) - ml - Math.Abs(ym); //消费项目小计=总销售额-折扣-抹零-优免
                            newdr["xfxj"] = xfxmxj;
                        }
                        #endregion

                        #region 会员收款项目
                        if (list_hysk != null && list_hysk.Count() > 0)
                        {
                            for (int h = 0; h < list_hysk.Count(); h++)
                            {
                                var temp = list_hysk.ElementAt(h);
                                var Key = temp.Key;

                                decimal Val = 0;

                                try
                                {
                                    Val = Convert.ToDecimal(dt5.Select("PayMethodName='" + Key + "' and StoCode='" + stocode + "'")[0][0]);
                                }
                                catch (Exception)
                                {
                                    Val = 0;
                                }
                                newdr["hysk_" + Key] = Val;
                            }
                        }
                        #endregion

                        #region 会员项目
                        if (list_hy != null && list_hy.Count() > 0)
                        {

                            try
                            {
                                hyxmxj = dt6.Select("StoCode='" + stocode + "'").Sum(x => x.Field<decimal>("payamount"));
                            }
                            catch (Exception)
                            {
                                hyxmxj = 0;
                            }

                            if (ky)
                            {
                                try
                                {
                                    //客用会员卡
                                    DataRow[] memrows = dt6.Select("cardtype<>'8' and cardtype<>'food' and StoCode='" + stocode + "'");
                                    newdr["hy_ky"] = memrows.Sum(x => x.Field<decimal>("payamount"));
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            if (ygk)
                            {
                                try
                                {
                                    //员工卡
                                    DataRow[] memrows = dt6.Select("cardtype='8' and StoCode='" + stocode + "'");
                                    newdr["hy_ygk"] = memrows.Sum(x => x.Field<decimal>("payamount"));
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            if (msk)
                            {
                                try
                                {
                                    //美食卡
                                    DataRow[] memrows = dt6.Select("cardtype='food' and StoCode='" + stocode + "'");
                                    newdr["hy_food"] = memrows.Sum(x => x.Field<decimal>("payamount"));
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            if (gbf)
                            {
                                try
                                {
                                    //美食卡工本费
                                    decimal cost = dt6.Select("StoCode='" + stocode + "").Sum(x => x.Field<decimal>("cardcost"));
                                    newdr["hy_foodgbf"] = cost;
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            if (msgbf)
                            {
                                try
                                {
                                    //美食卡工本费-押金
                                    decimal cost = dt6.Select("cardtype='food' and StoCode='" + stocode + "").Sum(x => x.Field<decimal>("cardcost"));
                                    newdr["hy_ky"] = cost;
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            newdr["hyxj"] = hyxmxj;
                        }
                        #endregion

                        newdr["h_hj"] = xfxmxj + hyxmxj;

                        dt_Result.Rows.Add(newdr);
                    }
                }
                #endregion
            }

            string json1 = new LayUItableHelper().GetLayUITableHead(arrayList);
            string json2 = JsonHelper.DataTableToJSON(dt_Result);
            string json = "{";
            json += "\"title\":[" + json1 + "],";
            json += "\"data\":" + json2;
            json += "}";
            Pagcontext.Response.Write(json);
        }

        /// <summary>
        /// 退单报表
        /// </summary>
        /// <param name="dicPar"></param>
        private void BackOrderList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();

            var sql = @"SELECT a.*,(a.Price*a.BackNum) TotalMoney FROM ( SELECT tbb.OrderCode,OrderDisCode,tbo.Price,(case when dis.IsWeight='1' then tbo.ItemNum else tbo.DisNum end) as BackNum,(SELECT PKKCode FROM dbo.TB_DishType WHERE PKCode=(SELECT TypeCode FROM TB_Dish WHERE DisCode=tbo.DisCode AND StoCode=tbb.StoCode) and StoCode='" + stocode + "') btype FROM TH_BackOrder AS tbb inner JOIN  TH_OrderDish AS tbo ON tbb.OrderDisCode=tbo.PKCode inner join tb_dish dis on tbo.discode=dis.discode and tbo.stocode=dis.stocode AND tbb.OrderCode=tbo.OrderCode WHERE tbb.StoCode='" + stocode + "' and (tbb.CTime>='" + btime + "' AND tbb.CTime<='" + etime + "')) AS a;SELECT TypeName,PKCode FROM TB_DishType where StoCode='" + stocode + "'";

            var ds = new BLL.bllPaging().GetDataSetInfoBySQL(sql);
            var json = "[";
            if (ds.Tables.Count == 2)
            {
                var dt = ds.Tables[0];
                var typeDt = ds.Tables[1];
                if (dt != null)
                {
                    var hjnum = dt.Compute("SUM(BackNum)", "");
                    var hjmoney = dt.Compute("SUM(TotalMoney)", "");
                    json += "{\"hjnum\":\"" + hjnum + "\",\"hjmoney\":\"" + hjmoney + "\",\"data\":[";
                    if (dt.Rows.Count > 0)
                    {
                        var type = dt.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["btype"].ToString());
                        if (type.Count() > 0)
                        {
                            for (int i = 0; i < type.Count(); i++)
                            {
                                var tempV = type.ElementAt(i);
                                var bigType = tempV.Key;
                                var newdr = dt.Select("btype='" + bigType + "'");
                                if (newdr.Count() > 0)
                                {
                                    var money = Convert.ToDouble(newdr.Sum(x => x.Field<decimal>("TotalMoney"))); //总价格
                                    var num = Convert.ToDouble(newdr.Sum(x => x.Field<decimal>("BackNum"))); //数量
                                    var bigName = typeDt.Select("PKCode='" + bigType + "'")[0][0];
                                    json += "{";
                                    json += "\"type\":\"" + bigName.ToString() + "\",";
                                    json += "\"typecode\":\"" + bigType + "\",";
                                    json += "\"sumnum\":\"" + Math.Round(Convert.ToDouble(num), 2) + "\",";
                                    json += "\"summoney\":\"" + Math.Round(Convert.ToDouble(money), 2) + "\"";
                                    json += "},";
                                }
                            }
                            json = json.TrimEnd(',');
                        }
                    }
                    json += "]}";
                }

            }
            json += "]";
            ToJsonStr(json);
        }

        /// <summary>
        /// 退单详情
        /// </summary>
        /// <param name="dicPar"></param>
        private void BackOrderDetails(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime", "distype", "reasonname" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            string distype = dicPar["distype"].ToString();
            string reasonname = dicPar["reasonname"].ToString();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.*,(a.Price*a.BackNum) TotalMoney FROM ( SELECT torder.BillCode OrderCode,tbo.DisName,(case when dis.IsWeight='1' then tbo.ItemNum else tbo.DisNum end) as BackNum,tbo.Price,torder.CTime AS stime,tbb.CTime AS etime,ReasonName,tbb.CCname,tbb.CCode,(SELECT TableName FROM TB_Table WHERE PKCode=(SELECT TableCode FROM dbo.TH_OpenTable WHERE PKCode=torder.OpenCodeList) and StoCode='" + stocode + "') tablename,(SELECT PKKCode FROM dbo.TB_DishType WHERE PKCode=(SELECT TypeCode FROM dbo.TB_Dish WHERE DisCode=tbo.DisCode AND StoCode=tbb.StoCode) and StoCode='" + stocode + "') btype,tbb.AuthCode,tbb.AuthName FROM TH_BackOrder AS tbb");
            sql.Append(" inner JOIN TH_OrderDish AS tbo ON tbb.OrderDisCode = tbo.PKCode AND tbb.OrderCode = tbo.OrderCode");
            sql.Append(" inner JOIN TH_Order AS torder ON tbb.OrderCode = torder.PKCode");
            sql.Append(" inner JOIN tb_dish AS dis ON dis.discode=tbo.discode and dis.stocode=tbo.stocode");
            sql.Append(" WHERE tbb.StoCode = '" + stocode + "' and(tbb.CTime >= '" + btime + "' AND tbb.CTime <= '" + etime + "')) AS a where 1=1");
            if (!string.IsNullOrEmpty(reasonname))
            {
                sql.Append(" and ReasonName like '%" + reasonname + "%'");
            }
            if (!string.IsNullOrEmpty(distype))
            {
                sql.Append(" and a.btype = '" + distype + "'");
            }

            var dt = new BLL.bllPaging().GetDataTableInfoBySQL(sql.ToString());
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    string json = JsonHelper.DataTableToJSON(dt);
                    var hjnum = dt.Compute("SUM(BackNum)", "");
                    var hjmoney = dt.Compute("SUM(TotalMoney)", "");
                    var tempJson = "{\"hjnum\":\"" + hjnum + "\",\"hjmoney\":\"" + hjmoney + "\",\"data\":[";

                    json = json.Insert(json.IndexOf('[') + 1, tempJson);
                    json = json.Insert(json.LastIndexOf(']'), "]}");

                    ToJsonStr(json);
                }
                else
                {
                    ToJsonStr("[]");
                }
            }
            else
            {
                ToJsonStr("[]");
            }

        }

        /// <summary>
        /// 赠送报表
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private void GetGiveList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();

            var sql = @"SELECT a.*,(a.DisNum*a.Price) TotalMoney FROM ( SELECT (tbo.DisNum-tbo.returnnum) as DisNum,tbo.Price,(SELECT PKKCode FROM dbo.TB_DishType WHERE PKCode=(SELECT TypeCode FROM TB_Dish WHERE DisCode=tbo.DisCode AND StoCode=tbo.StoCode) and stocode='" + stocode + "') btype FROM TH_OrderDish tbo WHERE StoCode='" + stocode + "' AND (CTime>='" + btime + "' AND CTime<='" + etime + "') and tbo.DiscountType=6) AS a;SELECT TypeName,PKCode FROM TB_DishType where StoCode='" + stocode + "'";

            var ds = new BLL.bllPaging().GetDataSetInfoBySQL(sql);
            var json = "[";
            if (ds != null)
            {
                var dt = ds.Tables[0];
                var dishtypedt = ds.Tables[1];
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {

                        var hjnum = dt.Compute("SUM(DisNum)", "");
                        var hjmoney = dt.Compute("SUM(TotalMoney)", "");
                        json += "{\"hjnum\":\"" + hjnum + "\",\"hjmoney\":\"" + hjmoney + "\",\"data\":[";

                        var btype = dt.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["btype"].ToString());
                        if (btype.Count() > 0)
                        {
                            for (int i = 0; i < btype.Count(); i++)
                            {
                                var tempV = btype.ElementAt(i);
                                var type = tempV.Key;
                                var newdr = dt.Select("btype='" + type + "'");
                                if (newdr.Count() > 0)
                                {
                                    var givenum = Convert.ToDouble(newdr.Sum(x => x.Field<decimal>("DisNum"))); //赠送数量
                                    var givemoney = Convert.ToDouble(newdr.Sum(x => x.Field<decimal>("TotalMoney"))); //赠送金额

                                    json += "{";
                                    json += "\"type\":\"" + type + "\",";
                                    json += "\"typename\":\"" + dishtypedt.Select("PKCode='" + type + "'")[0]["TypeName"].ToString() + "\",";
                                    json += "\"givemoney\":\"" + givemoney + "\",";
                                    json += "\"givenum\":\"" + givenum + "\"";
                                    json += "},";
                                }
                            }
                            json = json.TrimEnd(',');
                        }

                        json += "]}";
                    }
                }
            }
            json += "]";

            ToJsonStr(json);
        }

        /// <summary>
        /// 赠送详情
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private void GvieDetails(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime", "distype" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            string distype = dicPar["distype"].ToString();

            var sql = @"SELECT a.*,CONVERT(decimal(18,2),((case when a.IsWeight='1' then a.ItemNum else a.DisNum end)*a.Price)) TotalMoney,'' AUname  FROM ( SELECT ord.BillCode as OrderCode,(SELECT TableName FROM dbo.TB_Table WHERE PKCode=( SELECT TOP 1 TableCode FROM TH_OpenTable WHERE PKCode IN( SELECT TOP 1 OpenCodeList FROM dbo.TH_Order WHERE PKCode=tbo.OrderCode and stocode='" + stocode + "') and stocode='" + stocode + "') and stocode='" + stocode + "') tablename,tbo.DisName,tbo.DisNum,tbo.Price,dis.IsWeight,tbo.itemnum,(SELECT PKKCode FROM dbo.TB_DishType WHERE PKCode=(SELECT TypeCode FROM TB_Dish WHERE DisCode=tbo.DisCode AND StoCode=tbo.StoCode) and stocode='" + stocode + "') btype,tbo.CCname,DiscountRemark FROM TH_OrderDish tbo inner join TH_Order ord on tbo.OrderCode=ord.PKCode inner join tb_dish dis on tbo.discode=dis.discode and tbo.stocode=dis.stocode WHERE tbo.StoCode = '" + stocode + "' and (tbo.CTime>='" + btime + "' AND tbo.CTime<='" + etime + "') and tbo.DiscountType=6 and (tbo.DisNum-tbo.returnnum)>0) AS a where 1=1";
            if (!string.IsNullOrEmpty(distype))
            {
                sql += " and a.btype = '" + distype + "'";
            }

            var dt = new BLL.bllPaging().GetDataTableInfoBySQL(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable admins = new bllEmployee().GetAllAdmin();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["DiscountRemark"].ToString()))
                        {
                            DataRow[] zsadmin = admins.Select("userid=" + dr["DiscountRemark"].ToString() + "");
                            if (zsadmin != null && zsadmin.Length > 0)
                            {
                                dr["AUname"] = zsadmin[0]["realname"].ToString();
                                dr["DiscountRemark"] = "";
                            }
                        }
                    }
                    string json = JsonHelper.DataTableToJSON(dt);

                    var hjnum = dt.Compute("SUM(DisNum)", "");
                    var hjmoney = dt.Compute("SUM(TotalMoney)", "");
                    var tempJson = "{\"hjnum\":\"" + hjnum + "\",\"hjmoney\":\"" + hjmoney + "\",\"data\":[";

                    json = json.Insert(json.IndexOf('[') + 1, tempJson);
                    json = json.Insert(json.LastIndexOf(']'), "]}");

                    ToJsonStr(json);
                }
            }

        }

        /// <summary>
        /// 点单员销售报表
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetSaleList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();

            var sql = @" SELECT (case when tbod.CCname='小程序' then '小程序' else tbod.CCode end) as CCode,tbod.CCname,isnull(SUM(tbod.Price*(tbod.disnum-tbod.returnnum)),0) Price,isnull(SUM(tbod.Price*(tbod.disnum-tbod.returnnum)),0) TotalPay,isnull(SUM(tbd.RoyMoney),0) RoyMoney FROM  TH_Order AS tbo
 inner JOIN TH_OrderDish AS tbod ON tbo.PKCode=tbod.OrderCode AND tbod.StoCode=tbo.StoCode
 inner JOIN TB_Dish AS tbd ON tbod.DisCode=tbd.DisCode AND tbd.StoCode=tbo.StoCode
  WHERE tbo.TStatus=3  AND tbo.StoCode='" + stocode + "' AND (tbo.CTime>='" + btime + "' AND tbo.CTime<='" + etime + "')  AND tbod.IsPackage<>'2'  AND tbod.DiscountType<>'6' GROUP BY tbod.CCode,tbod.CCname ORDER BY tbod.CCode";

            var dt = new BLL.bllPaging().GetDataTableInfoBySQL(sql);
            var json = "[";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    var dismoney = dt.Compute("SUM(Price)", "");
                    var paymoney = dt.Compute("SUM(TotalPay)", "");
                    var tcmoney = dt.Compute("SUM(RoyMoney)", "");
                    json += "{\"dismoney\":\"" + dismoney + "\",\"paymoney\":\"" + paymoney + "\",\"tcmoney\":\"" + tcmoney + "\",\"data\":[";

                    var Code = dt.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["CCode"].ToString());
                    if (Code.Count() > 0)
                    {
                        for (int i = 0; i < Code.Count(); i++)
                        {
                            var tempV = Code.ElementAt(i);
                            var CCode = tempV.Key;
                            var newdr = dt.Select("CCode='" + CCode + "'");
                            if (newdr.Count() > 0)
                            {
                                var Price = Convert.ToDouble(newdr.Sum(x => x.Field<decimal>("Price"))); //菜品金额
                                var PayMoney = Convert.ToDouble(newdr.Sum(x => x.Field<decimal>("TotalPay"))); //实收金额
                                var RoyMoney = Convert.ToDouble(newdr.Sum(x => x.Field<decimal>("RoyMoney"))); //提成金额

                                json += "{";
                                json += "\"CCode\":\"" + CCode + "\",";
                                json += "\"CCname\":\"" + dt.Select("CCode='" + CCode + "'")[0]["CCname"].ToString() + "\",";
                                json += "\"PayMoney\":\"" + PayMoney + "\",";
                                json += "\"Price\":\"" + Price + "\",";
                                json += "\"tcPrice\":\"" + RoyMoney + "\"";
                                json += "},";
                            }
                        }
                        json = json.TrimEnd(',');
                    }
                    json += "]}";

                }
            }
            json += "]";

            ToJsonStr(json);
        }

        /// <summary>
        /// 点单员销售报表明细
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetSaleDetails(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "ccode", "cwtype", "discode", "btime", "etime" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            string ccode = dicPar["ccode"].ToString(); //点单员用户
            string cwtype = dicPar["cwtype"].ToString(); //财务类别
            string discode = dicPar["discode"].ToString(); //菜品编号

            var sql = @"SELECT tbod.DisCode,tbod.CCode,tbod.CCname,tbd.FinTypeName,tbod.DisName,(tbod.DisNum-tbod.returnnum) as DisNum,tbod.Price TotalMoney,tbod.Price as TotalPay,RoyMoney FROM  TH_Order AS tbo
 inner JOIN TH_OrderDish AS tbod ON tbo.PKCode=tbod.OrderCode AND tbod.StoCode=tbo.StoCode
 inner JOIN TB_Dish AS tbd ON tbod.DisCode=tbd.DisCode AND tbd.StoCode=tbo.StoCode
  WHERE tbo.TStatus=3  AND tbod.DiscountType<>'6' AND tbo.StoCode='" + stocode + @"' AND (tbo.CTime>='" + btime + @"' AND tbo.CTime<='" + etime + @"')
   AND tbod.IsPackage <> '2'";

            if (!string.IsNullOrEmpty(ccode))
            {
                sql += " and tbod.CCname='" + ccode + "'";
            }

            if (!string.IsNullOrEmpty(cwtype))
            {
                sql += " and tbod.FinCode='" + cwtype + "'";
            }

            if (!string.IsNullOrEmpty(discode))
            {
                sql += " and tbod.discode='" + discode + "'";
            }

            var dt = new BLL.bllPaging().GetDataTableInfoBySQL(sql);

            var json = "[";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    var disnum = dt.Compute("SUM(DisNum)", "");
                    var tcmoney = dt.Compute("SUM(RoyMoney)", "");
                    var tccount = dt.Compute("SUM(RoyMoney)", "");

                    json += "{\"disnum\":\"" + disnum + "\",\"tcmoney\":\"" + tcmoney + "\",\"tccount\":\"" + tccount + "\",\"data\":[";
                    var Code = dt.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["CCode"].ToString());
                    if (Code.Count() > 0)
                    {
                        for (int i = 0; i < Code.Count(); i++)
                        {
                            var tempV = Code.ElementAt(i);
                            var CCode = tempV.Key;
                            var CCname = dt.Select("CCode='" + CCode + "'")[0]["CCname"].ToString();
                            var NumCount = 0; //点餐菜品小计
                            var newdr = dt.Select("CCode='" + CCode + "'");
                            json += "{\"CCode\":\"" + CCode + "\",\"CCname\":\"" + CCname + "\",\"data\":[";
                            if (newdr.Count() > 0)
                            {
                                NumCount = Convert.ToInt32(newdr.Sum(x => x.Field<decimal>("DisNum"))); //小计

                                var disDr = newdr.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["DisCode"].ToString());
                                if (disDr.Count() > 0)
                                {
                                    for (int x = 0; x < disDr.Count(); x++)
                                    {
                                        var _discode = disDr.ElementAt(x).ToList()[0].ItemArray[0];
                                        json += "{";
                                        json += "\"cwType\":\"" + disDr.ElementAt(x).ToList()[0].ItemArray[3] + "\",";
                                        json += "\"disName\":\"" + disDr.ElementAt(x).ToList()[0].ItemArray[4] + "\",";
                                        json += "\"disNum\":\"" + ToDataTable(newdr).Select("DisCode='" + _discode + "'").Sum(h => h.Field<decimal>("DisNum")) + "\",";
                                        json += "\"TotalMoney\":\"" + ToDataTable(newdr).Select("DisCode='" + _discode + "'").Sum(h => h.Field<decimal>("DisNum")) * Helper.StringToDecimal(disDr.ElementAt(x).ToList()[0].ItemArray[6].ToString()) + "\",";
                                        json += "\"PayMoney\":\"" + ToDataTable(newdr).Select("DisCode='" + _discode + "'").Sum(h => h.Field<decimal>("DisNum")) * Helper.StringToDecimal(disDr.ElementAt(x).ToList()[0].ItemArray[7].ToString()) + "\",";
                                        json += "\"tcMoney\":\"" + disDr.ElementAt(x).ToList()[0].ItemArray[8] + "\",";
                                        json += "\"tcCount\":\"" + disDr.ElementAt(x).ToList()[0].ItemArray[8] + "\"";
                                        json += "},";
                                    }
                                    json = json.TrimEnd(',');
                                }

                            }
                            json += "],\"NumCount\":\"" + NumCount + "\",\"tcmoneysum\":\"0\",\"tccountsum\":\"0\"},";
                        }
                        json = json.TrimEnd(',');
                    }
                    json += "]}";
                }
            }
            json += "]";

            ToJsonStr(json);
        }

        /// <summary>
        /// 提成人销售报表
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private void GetRoyList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();

            var sql = @" SELECT tbot.CusEmpCode,tbot.CusEmpName,ISNULL(SUM(tbod.TotalMoney),0) TotalMoney,ISNULL(SUM(tbod.TotalPay),0) TotalPay FROM TB_OpenTable AS tbot
 INNER JOIN TB_Order AS tbo ON tbot.PKCode=tbo.OpenCodeList and tbo.stocode=tbot.stocode
 INNER JOIN TB_OrderDish AS tbod ON tbo.PKCode=tbod.OrderCode and tbod.stocode=tbo.stocode
 INNER JOIN dbo.TB_Dish AS tbd ON tbod.DisCode=tbd.DisCode and tbd.stocode=tbod.stocode
 WHERE tbo.TStatus=3 AND tbot.CusEmpCode <>'' and tbo.StoCode = '" + stocode + "' and tbot.stocode='" + stocode + "' and (tbot.CTime>='" + btime + "' AND tbot.CTime<='" + etime + "') GROUP BY tbot.CusEmpCode,tbot.CusEmpName";
            var dt = new BLL.bllPaging().GetDataTableInfoBySQL(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    string json = JsonHelper.DataTableToJSON(dt);
                    var hjdmoney = dt.Compute("SUM(TotalMoney)", "");
                    var hjpmoney = dt.Compute("SUM(TotalPay)", "");
                    var tempJson = "{\"hjdmoney\":\"" + hjdmoney + "\",\"hjpmoney\":\"" + hjpmoney + "\",\"data\":[";
                    json = json.Insert(json.IndexOf('[') + 1, tempJson);
                    json = json.Insert(json.LastIndexOf(']'), "]}");
                    ToJsonStr(json);
                }
                else
                {
                    ToJsonStr("[]");
                }
            }
            else
            {
                ToJsonStr("[]");
            }
        }

        //开班信息表
        private void GetShiftInfo(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime", "userid" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            string userid = dicPar["userid"].ToString();
            var sql = @"SELECT StoCode,'' as StoName,Id,CCname,CTime,Etime,CASE TStatus WHEN 1 THEN '收银中' WHEN 2 THEN '已结班' END AS TStatus FROM dbo.TB_OpenShift Where  StoCode = '" + stocode + "' and (CTime>='" + btime + "' AND CTime<='" + etime + "') UNION ALL SELECT StoCode,'' as StoName,Id,CCname,CTime,Etime,CASE TStatus WHEN 1 THEN '收银中' WHEN 2 THEN '已结班' END AS TStatus FROM dbo.Th_OpenShift Where  StoCode = '" + stocode + "' and (CTime>='" + btime + "' AND CTime<='" + etime + "')";
            var dt = new BLL.bllPaging().GetDataTableInfoBySQL(sql);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable dtStore = GetCacheToStore(userid);
                    if (dtStore != null && dtStore.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string _stocode = dr["StoCode"].ToString();
                            if (dtStore.Select("stocode='" + _stocode + "'").Length > 0)
                            {
                                DataRow dr_sto = dtStore.Select("stocode='" + _stocode + "'")[0];
                                dr["StoName"] = dr_sto["cname"].ToString();
                            }
                        }
                    }

                    ToJsonStr(JsonHelper.DataTableToJSON(dt));
                }
                else
                {
                    ToJsonStr("[]");
                }
            }
            else
            {
                ToJsonStr("[]");
            }
        }

        //手动折扣模板统计
        private void GetZkList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            string userid = dicPar["userid"].ToString();
            var sql = @"SELECT b.StoCode,b.DiscountName,COUNT(1) AS Num,'' as StoName FROM dbo.TH_Bill b inner join [NewCatering].[dbo].[TB_DiscountScheme] ds on b.DiscountName=ds.SchName and b.stocode=ds.stocode WHERE len(b.AUCode)>1  AND b.DiscountName<>'' and b.aucode<>'0' AND b.StoCode = '" + stocode + "' and (b.CTime>='" + btime + "' AND b.CTime<='" + etime + "') and len(ds.InsideCode)=0 GROUP BY b.DiscountName,b.StoCode";
            var dt = new BLL.bllPaging().GetDataTableInfoBySQL(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable dtStore = GetCacheToStore(userid);
                    foreach (DataRow dr in dt.Rows)
                    {
                        string _stocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + _stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + _stocode + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }
                    }
                    ToJsonStr(JsonHelper.DataTableToJSON(dt));
                }
                else
                {
                    ToJsonStr("[]");
                }
            }
            else
            {
                ToJsonStr("[]");
            }
        }

        /// <summary>
        /// 手动折扣模板详情
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetZkDetailsByZkName(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime", "zkname" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            string zkname = dicPar["zkname"].ToString();
            string userid = dicPar["userid"].ToString();
            var sql = @"SELECT StoCode,AUName,AUCode,COUNT(1) AS Num,'' as StoName FROM TH_Bill WHERE  len(AUCode)>1 and aucode<>'0' AND DiscountName='" + zkname + "' AND StoCode = '" + stocode + "' and (CTime>='" + btime + "' AND CTime<='" + etime + "') GROUP BY AUName,AUCode,StoCode";
            var dt = new BLL.bllPaging().GetDataTableInfoBySQL(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable dtStore = GetCacheToStore(userid);
                    foreach (DataRow dr in dt.Rows)
                    {
                        string _stocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + _stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + _stocode + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }
                    }
                    ToJsonStr(JsonHelper.DataTableToJSON(dt));
                }
                else
                {
                    ToJsonStr("[]");
                }
            }
            else
            {
                ToJsonStr("[]");
            }
        }

        /// <summary>
        /// 手动折扣模板详情
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetZkDetailsByAuCode(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "stocode", "btime", "etime", "aucode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string btime = dicPar["btime"].ToString();
            string etime = dicPar["etime"].ToString();
            string aucode = dicPar["aucode"].ToString();
            string userid = dicPar["userid"].ToString();
            var sql = @"SELECT b.StoCode,b.DiscountName,COUNT(1) AS Num,'' as StoName FROM dbo.TH_Bill b inner join [NewCatering].[dbo].[TB_DiscountScheme] ds on b.DiscountName=ds.SchName and b.stocode=ds.stocode WHERE len(b.AUCode)>1 and b.aucode<>'0' and  b.AUCode='" + aucode + "' AND b.DiscountName<>'' AND b.StoCode = '" + stocode + "' and (b.CTime>='" + btime + "' AND b.CTime<='" + etime + "') and len(ds.InsideCode)=0 GROUP BY b.DiscountName,b.StoCode";
            var dt = new BLL.bllPaging().GetDataTableInfoBySQL(sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable dtStore = GetCacheToStore(userid);
                    foreach (DataRow dr in dt.Rows)
                    {
                        string _stocode = dr["StoCode"].ToString();
                        if (dtStore.Select("stocode='" + _stocode + "'").Length > 0)
                        {
                            DataRow dr_sto = dtStore.Select("stocode='" + _stocode + "'")[0];
                            dr["StoName"] = dr_sto["cname"].ToString();
                        }
                    }
                    ToJsonStr(JsonHelper.DataTableToJSON(dt));
                }
                else
                {
                    ToJsonStr("[]");
                }
            }
            else
            {
                ToJsonStr("[]");
            }
        }

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构  
            foreach (DataRow row in rows)
                tmp.Rows.Add(row.ItemArray);  // 将DataRow添加到DataTable中  
            return tmp;
        }

        private void GetcpxsslhzList(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "StartTime", "EndTime", "StoCode", "DisName", "FinCode", "WarCode", "userid" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string StartTime = dicPar["StartTime"].ToString() + "-01";
            string EndTime = dicPar["EndTime"].ToString() + "-01";
            EndTime = DateTime.Parse(EndTime).AddMonths(1).ToString("yyyy-MM-dd");
            if ((DateTime.Parse(EndTime).Year - DateTime.Parse(StartTime).Year) > 1)
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"不能进行跨年查询\"}");
                return;
            }
            else if ((DateTime.Parse(EndTime).Year - DateTime.Parse(StartTime).Year) == 1 && DateTime.Parse(EndTime).Month != 1)
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"不能进行跨年查询\"}");
                return;
            }
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string StoName = string.Empty;
            if (!string.IsNullOrEmpty(StoCode))
            {
                DataTable dtStore = GetCacheToStore(userid);
                if (dtStore != null)
                {
                    DataRow[] stodr = dtStore.Select("stocode='" + StoCode + "'");
                    if (stodr.Length > 0)
                    {
                        StoName = stodr[0]["cname"].ToString();
                    }
                }
            }
            string DisName = dicPar["DisName"].ToString();
            string FinCode = dicPar["FinCode"].ToString();
            string WarCode = dicPar["WarCode"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }

            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));//月份


            string where = " where b.ctime>='" + StartTime + " 09:00:00' and b.ctime<='" + EndTime + " 09:00:00' and od.IsPackage in('0','2') and od.DiscountType<>'6' and o.TStatus='3'";
            if (!string.IsNullOrEmpty(StoCode))
            {
                where += " and od.stocode='" + StoCode + "'";
            }
            if (!string.IsNullOrEmpty(DisName))
            {
                where += " and od.disname like '%" + DisName + "%'";
            }
            if (!string.IsNullOrEmpty(FinCode))
            {
                where += " and d.fincode='" + FinCode + "'";
            }
            if (!string.IsNullOrEmpty(WarCode))
            {
                where += " and d.warcode='" + WarCode + "'";
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                where += " and od.buscode='" + BusCode + "'";
            }
            string sql = "SELECT od.PDisCode,od.CTime, isnull((case od.DiscountType when '6' then 0 else isnull(od.PackOnePrice,0)+isnull(od.cookMoney,0) end), 0) as PackOnePrice,(case when len(isnull(d.warcode,''))>0 then d.warcode else '无' end ) WarCode, Year(od.CTIME)[year] FROM [NewCatering].[dbo].[TH_OrderDish] od inner join tb_dish d on od.discode = d.discode and od.stocode = d.stocode inner join [dbo].[TH_Order] o on od.OrderCode=o.PKCode  inner join [TH_Bill] b on o.BillCode=b.PKCode " + where + "";

            sql += ";select od.pkcode from [dbo].[TH_OrderDish] od inner join[dbo].[TH_Order] o on od.OrderCode=o.PKCode inner join[dbo].[TH_Bill] b on o.BillCode=b.PKCode inner join[dbo].[TB_Dish] dis on dis.discode=od.discode and dis.stocode=od.stocode where od.IsPackage='1' and od.DiscountType='6' ";
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and b.CTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and b.CTime<='" + EndTime + "'";
            }
            if (!string.IsNullOrEmpty(StoCode))
            {
                sql += " and b.stocode='" + StoCode + "'";
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                sql += " and od.BusCode='" + BusCode + "'";
            }

            DataSet ds = new BLL.bllPaging().GetDataSetInfoBySQL(sql);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtWareHouse = new bllStockWareHouse().GetStoWareHouseAllList(" where stocode='" + StoCode + "'");
                ArrayList arrayList = new ArrayList();
                List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field1", Title = "月份", Align = "center" });
                int index = 0;
                string warcodes = ",";
                foreach (DataRow dr in dt.Select())
                {
                    string warcode = dr["WarCode"].ToString();
                    if (warcodes.Contains("," + warcode + ","))
                    {
                        continue;
                    }
                    warcodes += warcode + ",";
                    if (warcode != "无")
                    {
                        //获取仓库名称
                        DataRow[] drs = dtWareHouse.Select("warcode='" + warcode + "'");
                        if (drs.Length > 0)
                        {
                            Title_list1.Add(new LayUItableHelper { Sort = false, Field = drs[0]["warname"].ToString(), Title = drs[0]["warname"].ToString(), Align = "center", HeadTemplet = drs[0]["warname"].ToString() + "_url", Style = "cursor: pointer;color: #0000FF", TotalRow = true });
                            dt_Result.Columns.Add(drs[0]["warname"].ToString(), typeof(string));
                            dt_Result.Columns.Add(drs[0]["warname"].ToString() + "_url", typeof(string));
                        }
                        else
                        {
                            if (index == 0)
                            {
                                dt_Result.Columns.Add("(空)", typeof(string));
                                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "(空)", Title = "(空)", Align = "center", HeadTemplet = "(空)_url", Style = "cursor: pointer;color: #0000FF", TotalRow = true });
                                dt_Result.Columns.Add("(空)_url", typeof(string));
                            }
                            ++index;
                        }
                    }
                    else
                    {
                        if (index == 0)
                        {
                            dt_Result.Columns.Add("(空)", typeof(string));
                            Title_list1.Add(new LayUItableHelper { Sort = false, Field = "(空)", Title = "(空)", Align = "center", HeadTemplet = "(空)_url", Style = "cursor: pointer;color: #0000FF", TotalRow = true });
                            dt_Result.Columns.Add("(空)_url", typeof(string));
                        }
                        ++index;
                    }
                }
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field27", Title = "合计", Align = "center", TotalRow = true });
                dt_Result.Columns.Add("field27", typeof(string));
                arrayList.Add(Title_list1);//第一行表头
                warcodes = warcodes.Trim(',');
                int count = (DateTime.Parse(EndTime).Year - DateTime.Parse(StartTime).Year) * 12 + (DateTime.Parse(EndTime).Month - DateTime.Parse(StartTime).Month);
                int s_month = DateTime.Parse(StartTime).Month;
                int e_month = DateTime.Parse(EndTime).AddMonths(-1).Month;
                int year = DateTime.Parse(StartTime).Year;
                for (int i = s_month; i <= e_month; i++)
                {
                    DataRow new_dr = dt_Result.NewRow();
                    new_dr["field1"] = i + "月";
                    decimal _sum = 0;
                    foreach (string code in warcodes.Split(','))
                    {
                        int _m = i;
                        int _y = year;
                        if(i+1>12)
                        {
                            _m = 1;
                            _y = year + 1;
                        }
                        else
                        {
                            _m = i + 1;
                        }
                        DataRow[] drs = dt.Select("warcode='" + code + "' and CTime>='" + year+"-"+i+"-01 09:00:00' and CTime<='" +_y + "-" + _m + "-01 09:00:00'");
                        if (drs.Length > 0)
                        {
                            DataRow[] war_drs = dtWareHouse.Select("warcode='" + code + "'");
                            if (war_drs.Length > 0)
                            {
                                decimal pop = drs.Sum(x => x.Field<decimal>("PackOnePrice"));
                                //排除赠送的

                                foreach (DataRow zdr in drs)
                                {
                                    if (!string.IsNullOrEmpty(zdr["PDisCode"].ToString()))
                                    {
                                        string pdiscode = zdr["PDisCode"].ToString();
                                        DataRow[] pdrs = dt1.Select("pkcode='" + pdiscode + "'");
                                        if (pdrs.Length > 0)
                                        {
                                            pop = pop - decimal.Parse(zdr["PackOnePrice"].ToString());//套餐赠送的不计算金额
                                        }
                                    }
                                }
                                _sum += pop;
                                if (pop <= 0)
                                {
                                    new_dr[war_drs[0]["warname"].ToString()] = "-";
                                    new_dr[war_drs[0]["warname"].ToString() + "_url"] = "";
                                }
                                else
                                {
                                    new_dr[war_drs[0]["warname"].ToString()] = string.Format("{0:N}", pop.ToString());
                                    string url = @"./cpxsslmx.html?year=" + _y + "&month=" + i + "&StoCode=" + StoCode + "&StoName=" + StoName + "&DisName=" + DisName + "&FinCode=" + FinCode + "'&WarCode=" + drs[0]["WarCode"].ToString();
                                    new_dr[war_drs[0]["warname"].ToString() + "_url"] = url;
                                }
                            }
                            else
                            {
                                string m = new_dr["(空)"].ToString();
                                decimal pop = drs.Sum(x => x.Field<decimal>("PackOnePrice"));
                                //排除赠送的

                                foreach (DataRow zdr in drs)
                                {
                                    if (!string.IsNullOrEmpty(zdr["PDisCode"].ToString()))
                                    {
                                        string pdiscode = zdr["PDisCode"].ToString();
                                        DataRow[] pdrs = dt1.Select("pkcode='" + pdiscode + "'");
                                        if (pdrs.Length > 0)
                                        {
                                            pop = pop - decimal.Parse(zdr["PackOnePrice"].ToString());//套餐赠送的不计算金额
                                        }
                                    }
                                }
                                _sum += pop;
                                if (!string.IsNullOrEmpty(m))
                                {
                                    if (m == "-")
                                    {
                                        m = "0.00";
                                    }
                                    pop = pop + decimal.Parse(m);
                                    if (pop <= 0)
                                    {
                                        new_dr["(空)"] = string.Format("{0:N}", "-");
                                        new_dr["(空)_url"] = "";
                                    }
                                    else
                                    {
                                        new_dr["(空)"] = string.Format("{0:N}", pop.ToString());
                                        string url = @"./cpxsslmx.html?year=" + _y + "&month=" + i + "&StoCode=" + StoCode + "&StoName=" + StoName + "&DisName=" + DisName + "&FinCode=" + FinCode + "'&WarCode=" + drs[0]["WarCode"].ToString();
                                        new_dr["(空)_url"] = url;
                                    }
                                }
                                else
                                {
                                    if (pop <= 0)
                                    {
                                        new_dr["(空)"] = string.Format("{0:N}", "-");
                                        new_dr["(空)_url"] = "";
                                    }
                                    else
                                    {
                                        new_dr["(空)"] = string.Format("{0:N}", pop.ToString());
                                        string url = @"./cpxsslmx.html?year=" + _y + "&month=" + i + "&StoCode=" + StoCode + "&StoName=" + StoName + "&DisName=" + DisName + "&FinCode=" + FinCode + "'&WarCode=" + drs[0]["WarCode"].ToString();
                                        new_dr["(空)_url"] = url;
                                    }
                                }
                            }
                        }
                        else
                        {
                            DataRow[] war_drs = dtWareHouse.Select("warcode='" + code + "'");
                            if (war_drs.Length > 0)
                            {
                                string m = new_dr[war_drs[0]["warname"].ToString()].ToString();
                                if (!string.IsNullOrEmpty(m))
                                {

                                }
                                else
                                {
                                    new_dr[war_drs[0]["warname"].ToString()] = "-";
                                    string url = @"";
                                    new_dr[war_drs[0]["warname"].ToString() + "_url"] = url;
                                }
                            }
                            else
                            {
                                string m = new_dr["(空)"].ToString();
                                if (!string.IsNullOrEmpty(m))
                                {

                                }
                                else
                                {
                                    new_dr["(空)"] = "-";
                                    string url = @"";
                                    new_dr["(空)_url"] = url;
                                }
                            }
                        }
                    }
                    new_dr["field27"] = string.Format("{0:N}", _sum);
                    dt_Result.Rows.Add(new_dr);
                }

                string json1 = new LayUItableHelper().GetLayUITableHead(arrayList);
                string json2 = JsonHelper.DataTableToJSON(dt_Result);
                string json = "{";
                json += "\"title\":[" + json1 + "],";
                json += "\"data\":" + json2;
                json += "}";
                Pagcontext.Response.Write(json);
            }
            else
            {
                string nulljson = "{";
                nulljson += "\"title\":[[{\"field\": \"err\",\"title\":\"\",\"align\":\"center\"}]],";
                nulljson += "\"data\":[{\"err\":\"无数据\"}]";
                nulljson += "}";
                Pagcontext.Response.Write(nulljson);
                return;
            }
        }

        /// <summary>
        /// 菜品销售数量汇总明细
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetcpxsslhzmxList(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "StartTime", "EndTime", "StoCode", "DisName", "FinCode", "WarCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string StartTime = dicPar["StartTime"].ToString() + "-01";
            string EndTime = dicPar["EndTime"].ToString() + "-01";
            EndTime = DateTime.Parse(EndTime).AddMonths(1).ToString("yyyy-MM-dd");
            if ((DateTime.Parse(EndTime).Year - DateTime.Parse(StartTime).Year) > 1)
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"不能进行跨年查询\"}");
                return;
            }
            else if ((DateTime.Parse(EndTime).Year - DateTime.Parse(StartTime).Year) == 1 && DateTime.Parse(EndTime).Month != 1)
            {
                ToJsonStr("{\"code\":\"1\",\"msg\":\"不能进行跨年查询\"}");
                return;
            }
            string StoCode = dicPar["StoCode"].ToString();
            string DisName = dicPar["DisName"].ToString();
            string FinCode = dicPar["FinCode"].ToString();
            string WarCode = dicPar["WarCode"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }

            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));
            dt_Result.Columns.Add("field2", typeof(string));
            dt_Result.Columns.Add("field3", typeof(string));
            dt_Result.Columns.Add("field4", typeof(string));
            dt_Result.Columns.Add("field5", typeof(string));
            dt_Result.Columns.Add("field6", typeof(string));
            dt_Result.Columns.Add("field7", typeof(string));
            dt_Result.Columns.Add("field8", typeof(string));



            string where = " where b.ctime>='" + StartTime + " 09:00:00' and b.ctime<='" + EndTime + " 09:00:00' and od.IsPackage in('0','2') and o.TStatus='3'";
            if (!string.IsNullOrEmpty(StoCode))
            {
                where += " and od.stocode='" + StoCode + "'";
            }
            if (!string.IsNullOrEmpty(DisName))
            {
                where += " and od.disname like '%" + DisName + "%'";
            }
            if (!string.IsNullOrEmpty(FinCode))
            {
                where += " and d.fincode='" + FinCode + "'";
            }
            if (!string.IsNullOrEmpty(WarCode) && WarCode != "无")
            {
                where += " and d.warcode='" + WarCode + "'";
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                where += " and od.buscode='" + BusCode + "'";
            }
            string sql = "SELECT d.fintypename,isnull((case od.DiscountType when '6' then 0 else  isnull(od.PackOnePrice,0)+isnull(od.cookMoney,0) end), 0) as PackOnePrice, isnull(d.WarCode,'无') as WarCode, od.disname,od.discode,od.price,(od.DisNum - od.ReturnNum) as disnum,(case od.ispackage when '0' then '单品' when '1' then '套餐' else '套内单品' end) as dispackagetype,od.PDisCode FROM[NewCatering].[dbo].[TH_OrderDish] od inner join tb_dish d on od.discode = d.discode and od.stocode = d.stocode inner join [dbo].[TH_Order] o on od.OrderCode=o.PKCode  inner join [TH_Bill] b on o.BillCode=b.PKCode  " + where + " order by od.discode,d.warcode;";

            sql += "select od.pkcode from [dbo].[TH_OrderDish] od inner join[dbo].[TH_Order] o on od.OrderCode=o.PKCode inner join[dbo].[TH_Bill] b on o.BillCode=b.PKCode inner join[dbo].[TB_Dish] dis on dis.discode=od.discode and dis.stocode=od.stocode where od.IsPackage='1' and od.DiscountType='6' ";
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and b.CTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and b.CTime<='" + EndTime + "'";
            }
            if (!string.IsNullOrEmpty(StoCode))
            {
                sql += " and b.stocode='" + StoCode + "'";
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                sql += " and od.BusCode='" + BusCode + "'";
            }

            DataSet ds = new BLL.bllPaging().GetDataSetInfoBySQL(sql);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtWareHouse = new bllStockWareHouse().GetStoWareHouseAllList(" where stocode='" + StoCode + "'");
                ArrayList arrayList = new ArrayList();
                List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field1", Title = "档口", Align = "center" });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field2", Title = "财务类别", Align = "center" });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field3", Title = "编号", Align = "center" });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field4", Title = "菜品名称", Align = "center" });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field5", Title = "数量", Align = "center" });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field6", Title = "单价", Align = "center" });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field7", Title = "金额", Align = "center" });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field8", Title = "说明", Align = "center" });
                arrayList.Add(Title_list1);//第一行表头
                string warcodes = ",";
                foreach (DataRow dr in dt.Select())
                {
                    if (!warcodes.Contains("," + dr["WarCode"] + ","))
                    {
                        warcodes += dr["WarCode"].ToString() + ",";
                    }
                }

                decimal x_sum = 0;//小计
                decimal z_sum = 0;//总计
                warcodes = warcodes.Trim(',');
                foreach (string warcode in warcodes.Split(','))
                {
                    x_sum = 0;
                    DataRow[] drs = dt.Select("WarCode='" + warcode + "'");
                    if (drs.Length > 0)
                    {
                        string disnames = ",";
                        foreach (DataRow dr in drs)
                        {
                            string disname = dr["disname"].ToString();
                            string discode = dr["discode"].ToString();
                            string dispackagetype = dr["dispackagetype"].ToString();
                            if(disnames.Contains(","+disname+ dispackagetype + ","))
                            {
                                continue;
                            }
                            DataRow[] drss = dt.Select("WarCode='" + warcode + "' and discode='" + discode + "' and disname='" + disname + "' and dispackagetype='" + dispackagetype + "'");
                            decimal pop = drss.Sum(x => x.Field<decimal>("PackOnePrice"));
                            decimal disnum = drss.Sum(x => x.Field<decimal>("disnum"));
                            //排除赠送的
                            foreach (DataRow zdr in drss)
                            {
                                if (!string.IsNullOrEmpty(zdr["PDisCode"].ToString()))
                                {
                                    string pdiscode = zdr["PDisCode"].ToString();
                                    DataRow[] pdrs = dt1.Select("pkcode='" + pdiscode + "'");
                                    if (pdrs.Length > 0)
                                    {
                                        pop = pop - decimal.Parse(zdr["PackOnePrice"].ToString());//套餐赠送的不计算金额
                                    }
                                }
                            }

                            DataRow new_dr = dt_Result.NewRow();
                            string warname = string.Empty;
                            if (dr["WarCode"].ToString() != "无")
                            {
                                DataRow[] wardrs = dtWareHouse.Select("warcode='" + dr["WarCode"].ToString() + "'");
                                if (wardrs.Length > 0)
                                {
                                    warname = wardrs[0]["warname"].ToString();
                                    if (WarCode == "无")
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    warname = "无";
                                }
                            }
                            else
                            {
                                warname = "无";
                            }
                            new_dr["field1"] = warname;
                            new_dr["field2"] = dr["fintypename"].ToString();
                            new_dr["field3"] = dr["discode"].ToString();
                            new_dr["field4"] = dr["disname"].ToString();
                            new_dr["field5"] = disnum.ToString();
                            new_dr["field6"] = dr["price"].ToString();
                            new_dr["field7"] = pop.ToString();
                            x_sum += pop;
                            new_dr["field8"] = dr["dispackagetype"].ToString();
                            dt_Result.Rows.Add(new_dr);
                            disnames += disname + dispackagetype + ",";
                        }
                        if (WarCode != "无")
                        {
                            DataRow _new_dr1 = dt_Result.NewRow();
                            _new_dr1["field1"] = "小计:";
                            _new_dr1["field2"] = "";
                            _new_dr1["field3"] = "";
                            _new_dr1["field4"] = "";
                            _new_dr1["field5"] = "";
                            _new_dr1["field6"] = "";
                            _new_dr1["field7"] = x_sum;
                            _new_dr1["field8"] = "";
                            dt_Result.Rows.Add(_new_dr1);
                        }
                        z_sum += x_sum;
                    }
                }
                if (WarCode == "无")
                {
                    DataRow new_dr2 = dt_Result.NewRow();
                    new_dr2["field1"] = "小计:";
                    new_dr2["field2"] = "";
                    new_dr2["field3"] = "";
                    new_dr2["field4"] = "";
                    new_dr2["field5"] = "";
                    new_dr2["field6"] = "";
                    new_dr2["field7"] = z_sum;
                    new_dr2["field8"] = "";
                    dt_Result.Rows.Add(new_dr2);
                }
                DataRow new_dr1 = dt_Result.NewRow();
                new_dr1["field1"] = "合计:";
                new_dr1["field2"] = "";
                new_dr1["field3"] = "";
                new_dr1["field4"] = "";
                new_dr1["field5"] = "";
                new_dr1["field6"] = "";
                new_dr1["field7"] = z_sum;
                new_dr1["field8"] = "";
                dt_Result.Rows.Add(new_dr1);

                string json1 = new LayUItableHelper().GetLayUITableHead(arrayList);
                string json2 = JsonHelper.DataTableToJSON(dt_Result);
                string json = "{";
                json += "\"title\":[" + json1 + "],";
                json += "\"data\":" + json2;
                json += "}";
                Pagcontext.Response.Write(json);
            }
            else
            {
                string nulljson = "{";
                nulljson += "\"title\":[[{\"field\": \"err\",\"title\":\"\",\"align\":\"center\"}]],";
                nulljson += "\"data\":[{\"err\":\"无数据\"}]";
                nulljson += "}";
                Pagcontext.Response.Write(nulljson);
                return;
            }
        }

    }
}