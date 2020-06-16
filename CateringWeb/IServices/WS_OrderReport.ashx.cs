using CommunityBuy.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_FinTypeReport 的摘要说明
    /// </summary>
    public class WS_OrderReport : ServiceBase
    {
        bllWS_FinTypeReport bll = new bllWS_FinTypeReport();
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
                        case "orderbilleetailreport"://结账账单详情(历史)
                            OrderBillDetailReport(dicPar);
                            break;
                        case "ordertbbilleetailreport"://结账账单详情(实时)
                            OrderTBBillDetailReport(dicPar);
                            break;
                        case "ordertcdpreport"://账单套餐单品统计
                            OrdertcdpReport(dicPar);
                            break;
                        case "ordertcdpzjreport"://品项销售穿透的单品套餐统计
                            OrdertcdpzjReport(dicPar);
                            break;
                        case "getstossbb":
                            GetStoSsBb(dicPar);//门店实时报表
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 历史
        /// </summary>
        /// <param name="dicPar"></param>
        private void OrderBillDetailReport(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "userid", "StoCode", "BillCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string BillCode = dicPar["BillCode"].ToString();

            DataSet ds = bll.OrderBillDetailReport(userid, StoCode, BillCode);
            StringBuilder html = new StringBuilder();
            if (ds != null && ds.Tables.Count == 7)
            {
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    string errjson = "{";
                    errjson += "\"data\":\"账单加载失败\"";
                    errjson += "}";
                    Pagcontext.Response.Write(errjson);
                    return;
                }
                DataRow TR_Bill = ds.Tables[0].Rows[0];
                html.Append("<table id='orderdetailinfotable' border='1' cellpadding='0' cellspacing='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>账单号</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["PKCode"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>开始时间</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["CTime"].ToString() + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>业务日期</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["YTime"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>餐别</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["MealName"].ToString() + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>客户名称</td>");
                html.Append("<td style='padding-left: 5px;'></td>");
                html.Append("<td style='padding-left: 5px;'>人数</td>");
                html.Append("<td style='padding-left: 5px;'> " + TR_Bill["CusNum"].ToString() + " </td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>状态</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["StatusName"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>结算时间</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["FTime"].ToString() + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>班次号</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["ShiftCode"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>收银员</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["CCname"].ToString() + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>开发票</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["Fp"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>消费备注</td>");
                if (!string.IsNullOrEmpty(TR_Bill["remark"].ToString()))
                {
                    //if (TR_Bill["remark"].ToString().IndexOf(',') > 0)
                    //{
                    //    html.Append("<td style='padding-left: 5px;'>" + TR_Bill["remark"].ToString().Split(',')[0] + ",..." + "</td>");
                    //}
                    //else
                    //{
                    html.Append("<td style='padding-left: 5px;max-width: 500px;overflow-wrap: break-word;'>" + TR_Bill["remark"].ToString() + "</td>");
                    //}
                }
                else
                {
                    html.Append("<td style='padding-left: 5px;'></td>");
                }
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='4' align='left' valign='top'>");
                html.Append("<table width='97%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td>&nbsp;</td>");
                html.Append("<td> 桌台 </td>");
                html.Append("<td> 品项名称 </td>");
                html.Append("<td> 单位 </td>");
                html.Append("<td> 数量 </td>");
                html.Append("<td> 价格 </td>");
                html.Append("<td> 加价 </td>");
                html.Append("<td> 金额 </td>");
                html.Append("<td> 折扣金额 </td >");
                html.Append("<td> 实收金额 </td >");
                html.Append("<td> 点菜员 </td>");
                html.Append("<td> 下单时间 </td>");
                html.Append("</tr>");

                DataTable OpenTable = ds.Tables[1];
                DataTable OrderDishes = ds.Tables[2];
                if (OpenTable != null && OpenTable.Rows.Count > 0 && OrderDishes != null && OrderDishes.Rows.Count > 0)
                {
                    decimal hj_sum_disnum = 0;
                    decimal hj_sum_price = 0;
                    decimal hj_sum_cookmoney = 0;
                    decimal hj_sum_disnum_money = 0;
                    decimal hj_sum_zk_money = 0;
                    decimal hj_sum_ss_money = 0;
                    foreach (DataRow dr in OpenTable.Rows)
                    {

                        html.Append("<tr>");
                        html.Append("<td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color:#808080'>&nbsp;</td>");
                        html.Append("<td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '11' >" + (dr["TypeName"].ToString() + "-" + dr["TableName"].ToString()) + "</td>");
                        html.Append("</tr>");
                        DataRow[] ddrs = OrderDishes.Select("TableCode='" + dr["TableCode"].ToString() + "'");
                        decimal xj_summoney = 0;

                        decimal sum_disnum = 0;
                        decimal sum_price = 0;
                        decimal sum_cookmoney = 0;
                        decimal sum_disnum_money = 0;
                        decimal sum_zk_money = 0;
                        decimal sum_ss_money = 0;
                        foreach (DataRow ddr in OrderDishes.Rows)
                        {
                            html.Append("<tr>");
                            html.Append("<td>&nbsp;</td>");
                            html.Append("<td>&nbsp;</td>");
                            string disname = string.Empty;
                            if (ddr["IsPackage"].ToString() == "2")
                            {
                                disname = "(套)" + ddr["DisName"].ToString();
                                sum_price += 0;

                                sum_zk_money += 0;
                                sum_ss_money += 0;
                            }
                            else
                            {
                                disname = ddr["DisName"].ToString();
                                sum_price += Helper.StringToDecimal(ddr["Price"].ToString());

                                sum_zk_money += (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * (Helper.StringToDecimal(ddr["DisNum"].ToString()) - Helper.StringToDecimal(ddr["returnnum"].ToString()));
                                sum_ss_money += Helper.StringToDecimal(ddr["TotalMoney"].ToString());
                            }
                            html.Append("<td> " + disname + " </td>");
                            string uite = string.Empty;
                            if (ddr["IsPackage"].ToString() == "1")
                            {
                                uite = "套";
                            }
                            else
                            {
                                uite = ddr["DisUite"].ToString();
                            }
                            sum_disnum += Helper.StringToDecimal(ddr["DisNum"].ToString());

                            sum_cookmoney += Helper.StringToDecimal(ddr["CookMoney"].ToString());
                            sum_disnum_money += ((Helper.StringToDecimal(ddr["DisNum"].ToString()) - Helper.StringToDecimal(ddr["returnnum"].ToString())) * Helper.StringToDecimal(ddr["Price"].ToString()));

                            html.Append("<td> " + uite + " </td>");
                            html.Append("<td> " + ddr["DisNum"].ToString() + " </td>");
                            if (ddr["IsPackage"].ToString() == "2")
                            {
                                html.Append("<td> 0.00 </td>");
                                html.Append("<td> " + ddr["CookMoney"].ToString() + " </td>");
                                html.Append("<td> 0.00 </td>");
                                html.Append("<td> 0.00 </td>");
                                html.Append("<td> 0.00 </td>");
                            }
                            else
                            {
                                html.Append("<td> " + ddr["Price"].ToString() + " </td>");
                                html.Append("<td> " + ddr["CookMoney"].ToString() + " </td>");
                                html.Append("<td> " + string.Format("{0:N}", (Helper.StringToDecimal(ddr["DisNum"].ToString()) * Helper.StringToDecimal(ddr["Price"].ToString()))) + " </td>");
                                html.Append("<td> " + string.Format("{0:N}", (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * Helper.StringToDecimal(ddr["DisNum"].ToString())) + " </td>");
                                html.Append("<td> " + Helper.StringToDecimal(ddr["TotalMoney"].ToString()) + " </td>");
                            }


                            html.Append("<td> " + ddr["CCname"].ToString() + " </td>");
                            html.Append("<td> " + ddr["CTime"].ToString() + " </td>");
                            html.Append("</tr>");
                            if (Helper.StringToDecimal(ddr["returnnum"].ToString()) > 0)
                            {
                                html.Append("<tr>");
                                html.Append("<td>&nbsp;</td>");
                                html.Append("<td>&nbsp;</td>");
                                html.Append("<td> " + disname + " </td>");
                                html.Append("<td> " + uite + " </td>");
                                html.Append("<td> -" + ddr["returnnum"].ToString() + " </td>");
                                html.Append("<td> -" + ddr["Price"].ToString() + " </td>");
                                html.Append("<td> -" + ddr["CookMoney"].ToString() + " </td>");
                                html.Append("<td> -" + string.Format("{0:N}", (Helper.StringToDecimal(ddr["returnnum"].ToString()) * Helper.StringToDecimal(ddr["Price"].ToString()))) + " </td>");
                                html.Append("<td> -" + string.Format("{0:N}", (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * Helper.StringToDecimal(ddr["returnnum"].ToString())) + " </td>");
                                html.Append("<td> 0.00 </td>");
                                html.Append("<td> " + ddr["CCname"].ToString() + " </td>");
                                html.Append("<td> " + ddr["CTime"].ToString() + " </td>");
                                html.Append("</tr>");
                            }
                        }
                        //小计
                        html.Append("<tr>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td> 小计: </td>");
                        html.Append("<td></td>");
                        html.Append("<td> " + sum_disnum + " </td>");
                        html.Append("<td> " + sum_price + " </td>");
                        html.Append("<td> " + sum_cookmoney + " </td>");
                        html.Append("<td> " + sum_disnum_money + " </td>");
                        html.Append("<td> " + sum_zk_money + " </td>");
                        html.Append("<td> " + sum_ss_money + " </td>");
                        html.Append("<td></td>");
                        html.Append("<td></td>");
                        html.Append("</tr>");
                        html.Append("<tr><td></td><td></td><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan='10' ></td></tr>");
                        hj_sum_disnum += sum_disnum;
                        hj_sum_price += sum_price;
                        hj_sum_cookmoney += sum_cookmoney;
                        hj_sum_disnum_money += sum_disnum_money;
                        hj_sum_zk_money += sum_zk_money;
                        hj_sum_ss_money += sum_ss_money;
                    }
                    //合计
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 合计: </td>");
                    html.Append("<td> " + (OpenTable.Rows.Count + "桌") + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + hj_sum_disnum + " </td>");
                    html.Append("<td> " + hj_sum_price + " </td>");
                    html.Append("<td> " + hj_sum_cookmoney + " </td>");
                    html.Append("<td> " + hj_sum_disnum_money + " </td>");
                    html.Append("<td> " + hj_sum_zk_money + " </td>");
                    html.Append("<td> " + hj_sum_ss_money + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td></td>");
                    html.Append("</tr>");
                    html.Append("<tr><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '14' ></td></tr>");
                }
                else if ((OpenTable == null || OpenTable.Rows.Count <= 0) && OrderDishes != null && OrderDishes.Rows.Count > 0)
                {
                    decimal hj_sum_disnum = 0;
                    decimal hj_sum_price = 0;
                    decimal hj_sum_cookmoney = 0;
                    decimal hj_sum_disnum_money = 0;
                    decimal hj_sum_zk_money = 0;
                    decimal hj_sum_ss_money = 0;
                    html.Append("<tr>");
                    html.Append("<td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color:#808080'>&nbsp;</td>");
                    html.Append("<td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '11' ></td>");
                    html.Append("</tr>");
                    decimal xj_summoney = 0;
                    decimal sum_disnum = 0;
                    decimal sum_price = 0;
                    decimal sum_cookmoney = 0;
                    decimal sum_disnum_money = 0;
                    decimal sum_zk_money = 0;
                    decimal sum_ss_money = 0;
                    foreach (DataRow ddr in OrderDishes.Rows)
                    {
                        html.Append("<tr>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>&nbsp;</td>");
                        string disname = string.Empty;
                        if (ddr["IsPackage"].ToString() == "2")
                        {
                            disname = "(套)" + ddr["DisName"].ToString();
                        }
                        else
                        {
                            disname = ddr["DisName"].ToString();
                        }
                        html.Append("<td> " + disname + " </td>");
                        string uite = string.Empty;
                        if (ddr["IsPackage"].ToString() == "1")
                        {
                            uite = "套";
                        }
                        else
                        {
                            uite = ddr["DisUite"].ToString();
                        }
                        sum_disnum += Helper.StringToDecimal(ddr["DisNum"].ToString());
                        sum_price += Helper.StringToDecimal(ddr["Price"].ToString());
                        sum_cookmoney += Helper.StringToDecimal(ddr["CookMoney"].ToString());
                        sum_disnum_money += (Helper.StringToDecimal(ddr["DisNum"].ToString()) * Helper.StringToDecimal(ddr["Price"].ToString()));
                        sum_zk_money += (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * Helper.StringToDecimal(ddr["DisNum"].ToString());
                        sum_ss_money += Helper.StringToDecimal(ddr["TotalMoney"].ToString());



                        html.Append("<td> " + uite + " </td>");
                        html.Append("<td> " + ddr["DisNum"].ToString() + " </td>");
                        html.Append("<td> " + ddr["Price"].ToString() + " </td>");
                        html.Append("<td> " + ddr["CookMoney"].ToString() + " </td>");
                        html.Append("<td> " + string.Format("{0:N}", (Helper.StringToDecimal(ddr["DisNum"].ToString()) * Helper.StringToDecimal(ddr["Price"].ToString()))) + " </td>");
                        html.Append("<td> " + string.Format("{0:N}", (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * Helper.StringToDecimal(ddr["DisNum"].ToString())) + " </td>");
                        html.Append("<td> " + Helper.StringToDecimal(ddr["TotalMoney"].ToString()) + " </td>");
                        html.Append("<td> " + ddr["CCname"].ToString() + " </td>");
                        html.Append("<td> " + ddr["CTime"].ToString() + " </td>");
                        html.Append("</tr>");
                    }
                    //小计
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 小计: </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + sum_disnum + " </td>");
                    html.Append("<td> " + sum_price + " </td>");
                    html.Append("<td> " + sum_cookmoney + " </td>");
                    html.Append("<td> " + sum_disnum_money + " </td>");
                    html.Append("<td> " + sum_zk_money + " </td>");
                    html.Append("<td> " + sum_ss_money + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td></td>");
                    html.Append("</tr>");
                    html.Append("<tr><td></td><td></td><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan='10' ></td></tr>");
                    hj_sum_disnum += sum_disnum;
                    hj_sum_price += sum_price;
                    hj_sum_cookmoney += sum_cookmoney;
                    hj_sum_disnum_money += sum_disnum_money;
                    hj_sum_zk_money += sum_zk_money;
                    hj_sum_ss_money += sum_ss_money;

                    //合计
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 合计: </td>");
                    html.Append("<td>  </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + hj_sum_disnum + " </td>");
                    html.Append("<td> " + hj_sum_price + " </td>");
                    html.Append("<td> " + hj_sum_cookmoney + " </td>");
                    html.Append("<td> " + hj_sum_disnum_money + " </td>");
                    html.Append("<td> " + hj_sum_zk_money + " </td>");
                    html.Append("<td> " + hj_sum_ss_money + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td></td>");
                    html.Append("</tr>");
                    html.Append("<tr><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '14' ></td></tr>");
                }
                html.Append("<tr>");
                html.Append("<td colspan='15' valign='top'>");
                html.Append("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0' style='width:100%;margin:0px;'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td width ='50px'>&nbsp;</td>");
                html.Append("<td style = 'border-bottom-style:solid; border-bottom-width: 1px; border-bottom-color: #808080'> 科目名称 </td>");
                html.Append("<td style='border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #808080'> 消费项目 </td>");
                html.Append("<td style='border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #808080'> 结算项目 </td>");
                html.Append("<td style='border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #808080'> 备注 </td>");
                html.Append("<td style='border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #808080'> 操作员 </td>");
                html.Append("</tr>");
                //菜品类目
                DataTable OrderDisheFin = ds.Tables[3];
                if (OrderDisheFin != null && OrderDisheFin.Rows.Count > 0)
                {
                    foreach (DataRow dr in OrderDisheFin.Rows)
                    {
                        html.Append("<tr>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td> " + dr["FinTypeName"].ToString() + " </td>");
                        html.Append("<td> " + dr["TotalMoney"].ToString() + " </td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("</tr>");
                    }
                }
                DataTable memOrder = ds.Tables[6];//会员卡订单的一次只能充值一张卡
                if (memOrder != null && memOrder.Rows.Count > 0)
                {
                    foreach (DataRow dr in memOrder.Rows)//其实只有一条记录
                    {
                        DataTable mem = new bllMemcardLevel().GetMemcard("where 1 = 1 and m.cardcode='" + dr["cardcode"].ToString() + "'");
                        if (mem != null && mem.Rows.Count > 0)
                        {
                            html.Append("<tr>");
                            html.Append("<td>&nbsp;</td>");
                            html.Append("<td> " + mem.Rows[0]["typename"].ToString() + " </td>");
                            html.Append("<td> " + dr["payamount"].ToString() + " </td>");
                            html.Append("<td>&nbsp;</td>");
                            html.Append("<td>卡号" + mem.Rows[0]["cardcode"].ToString() + "</td>");
                            html.Append("<td>&nbsp;</td>");
                            html.Append("</tr>");
                        }

                    }
                }


                decimal dishessummoney = OrderDisheFin.Select().Sum(x => x.Field<decimal>("TotalMoney"));
                decimal paysummoney = 0;
                //收款项
                //优惠券信息
                DataTable Coupon = ds.Tables[5];
                if (Coupon != null && Coupon.Rows.Count > 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 优惠券 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>" + Coupon.Select().Sum(x => x.Field<decimal>("RealPay")) + "</td>");
                    html.Append("<td> 普通券:商品券:");
                    foreach (DataRow dr in Coupon.Rows)
                    {
                        html.Append(dr["CouponCode"].ToString() + "</br>");
                    }
                    html.Append("</td>");
                    html.Append("<td>" + TR_Bill["CCname"].ToString() + "</td>");
                    html.Append("</tr>");
                    paysummoney += Coupon.Select().Sum(x => x.Field<decimal>("RealPay"));
                }
                //折扣
                if (Helper.StringToDecimal(TR_Bill["DiscountMoney"].ToString()) != 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 折扣 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>" + TR_Bill["DiscountMoney"].ToString() + "</td>");
                    html.Append("<td>折扣方案名称:" + TR_Bill["DiscountName"].ToString() + "</td>");
                    html.Append("<td>" + TR_Bill["CCname"].ToString() + "</td>");
                    html.Append("</tr>");
                    paysummoney += Helper.StringToDecimal(TR_Bill["DiscountMoney"].ToString());
                }
                //抹零
                if (Helper.StringToDecimal(TR_Bill["ZeroCutMoney"].ToString()) != 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 抹零 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>" + TR_Bill["ZeroCutMoney"].ToString() + "</td>");
                    html.Append("<td></td>");
                    html.Append("<td>" + TR_Bill["CCname"].ToString() + "</td>");
                    html.Append("</tr>");
                    paysummoney += Helper.StringToDecimal(TR_Bill["ZeroCutMoney"].ToString());
                }
                //积分抵扣
                if (Helper.StringToDecimal(TR_Bill["PointMoney"].ToString()) != 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 积分抵扣 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>" + TR_Bill["PointMoney"].ToString() + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + TR_Bill["CCname"].ToString() + " </td>");
                    html.Append("</tr>");
                    paysummoney += Helper.StringToDecimal(TR_Bill["PointMoney"].ToString());
                }
                //虚拟币抵扣
                if (Helper.StringToDecimal(TR_Bill["VirMoney"].ToString()) != 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 虚拟币抵扣 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> " + TR_Bill["VirMoney"].ToString() + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + TR_Bill["CCname"].ToString() + " </td>");
                    html.Append("</tr>");
                    paysummoney += Helper.StringToDecimal(TR_Bill["VirMoney"].ToString());
                }
                //支付项
                DataTable Pay = ds.Tables[4];
                if (Pay != null && Pay.Rows.Count > 0)
                {
                    //判断是否存在会员卡支付
                    DataRow[] mempay = Pay.Select("PayMethodCode='7'");
                    if (mempay != null && mempay.Length > 0)
                    {
                        string where = " where 1=1 and m.cardcode in(";
                        foreach (DataRow mdr in mempay)
                        {
                            if (!string.IsNullOrEmpty(mdr["MemberCard"].ToString()))
                            {
                                string[] d = mdr["MemberCard"].ToString().Split(' ');
                                where += "'" + d[0] + "',";
                            }
                        }
                        where = where.TrimEnd(',') + ")";
                        //获取会员卡信息
                        DataTable mem = new bllMemcardLevel().GetMemcard(where);
                        foreach (DataRow dr in mempay)
                        {
                            if (StoCode != "08")
                            {
                                DataRow[] mdr = mem.Select("cardcode='" + dr["MemberCard"].ToString() + "' or memcode='" + dr["MemberCard"].ToString() + "'");
                                string value = string.Empty;
                                string cardtypename = string.Empty;
                                if (mdr != null && mdr.Length > 0)
                                {
                                    cardtypename = mdr[0]["typename"].ToString();
                                    value = " 卡号:" + mdr[0]["cardcode"].ToString() + " 名称:" + dr["MemberName"].ToString() + " 折扣模板:" + (dr["MemberDiscount"].ToString() == "" ? "无" : dr["MemberDiscount"].ToString());
                                }
                                if (cardtypename != "储值卡" && cardtypename != "员工卡" && cardtypename != "美食卡")
                                {
                                    cardtypename = "贵宾卡";
                                }
                                html.Append("<tr>");
                                html.Append("<td>&nbsp;</td>");
                                html.Append("<td> " + cardtypename + " </td>");
                                html.Append("<td> &nbsp; </td>");
                                html.Append("<td> " + dr["PayMoney"].ToString() + " </td>");
                                html.Append("<td> " + value + "</td>");
                                html.Append("<td> " + TR_Bill["CCname"].ToString() + " </td>");
                                html.Append("</tr>");
                            }
                            else
                            {
                                
                                string value = string.Empty;
                                string cardtypename = string.Empty;
                                if(dr["MemberName"]!=null)
                                {
                                    cardtypename = dr["MemberName"].ToString();
                                }
                                if(string.IsNullOrEmpty(cardtypename))
                                {
                                    cardtypename = "会员卡";
                                }
                                value = dr["Remar"].ToString();
                                html.Append("<tr>");
                                html.Append("<td>&nbsp;</td>");
                                html.Append("<td> "+ cardtypename + " </td>");
                                html.Append("<td> &nbsp; </td>");
                                html.Append("<td> " + dr["PayMoney"].ToString() + " </td>");
                                html.Append("<td> " + value + "</td>");
                                html.Append("<td> " + TR_Bill["CCname"].ToString() + " </td>");
                                html.Append("</tr>");
                            }
                        }
                    }
                    DataRow[] paydr = Pay.Select("PayMethodCode<>'7'");
                    foreach (DataRow dr in paydr)
                    {
                        html.Append("<tr>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td> " + dr["PayMethodName"].ToString() + " </td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>" + dr["PayMoney"].ToString() + " </td>");
                        html.Append("<td></td>");
                        html.Append("<td> " + TR_Bill["CCname"].ToString() + " </td>");
                        html.Append("</tr>");
                    }
                    paysummoney += Pay.Select().Sum(x => x.Field<decimal>("PayMoney"));
                }
                html.Append("<tr><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '14' ></td></tr>");
                html.Append("<tr>");
                html.Append("<td>合计:</td>");
                html.Append("<td></td>");
                html.Append("<td>" + dishessummoney + "</td>");
                html.Append("<td>" + paysummoney + "</td>");
                html.Append("<td></td>");
                html.Append("<td></td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
            }
            string json = "{";
            json += "\"data\":\"" + html.ToString() + "\"";
            json += "}";
            Pagcontext.Response.Write(json);
        }

        /// <summary>
        /// 实时
        /// </summary>
        /// <param name="dicPar"></param>
        private void OrderTBBillDetailReport(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "userid", "StoCode", "BillCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string BillCode = dicPar["BillCode"].ToString();

            DataSet ds = bll.OrderTBBillDetailReport(userid, StoCode, BillCode);
            StringBuilder html = new StringBuilder();
            if (ds != null && ds.Tables.Count == 7)
            {
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    string errjson = "{";
                    errjson += "\"data\":\"账单加载失败\"";
                    errjson += "}";
                    Pagcontext.Response.Write(errjson);
                    return;
                }
                DataRow TR_Bill = ds.Tables[0].Rows[0];
                html.Append("<table id='orderdetailinfotable' border='1' cellpadding='0' cellspacing='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>账单号</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["PKCode"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>开始时间</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["CTime"].ToString() + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>业务日期</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["YTime"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>餐别</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["MealName"].ToString() + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>客户名称</td>");
                html.Append("<td style='padding-left: 5px;'></td>");
                html.Append("<td style='padding-left: 5px;'>人数</td>");
                html.Append("<td style='padding-left: 5px;'> " + TR_Bill["CusNum"].ToString() + " </td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>状态</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["StatusName"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>结算时间</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["FTime"].ToString() + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>班次号</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["ShiftCode"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>收银员</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["CCname"].ToString() + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='padding-left: 5px;'>开发票</td>");
                html.Append("<td style='padding-left: 5px;'>" + TR_Bill["Fp"].ToString() + "</td>");
                html.Append("<td style='padding-left: 5px;'>消费备注</td>");
                if (!string.IsNullOrEmpty(TR_Bill["remark"].ToString()))
                {
                    if (TR_Bill["remark"].ToString().IndexOf(',') > 0)
                    {
                        html.Append("<td style='padding-left: 5px;'>" + TR_Bill["remark"].ToString().Split(',')[0] + ",..." + "</td>");
                    }
                    else
                    {
                        html.Append("<td style='padding-left: 5px;'>" + TR_Bill["remark"].ToString() + "</td>");
                    }
                }
                else
                {
                    html.Append("<td style='padding-left: 5px;'></td>");
                }
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='4' align='left' valign='top'>");
                html.Append("<table width='97%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td>&nbsp;</td>");
                html.Append("<td> 桌台 </td>");
                html.Append("<td> 品项名称 </td>");
                html.Append("<td> 单位 </td>");
                html.Append("<td> 数量 </td>");
                html.Append("<td> 价格 </td>");
                html.Append("<td> 加价 </td>");
                html.Append("<td> 金额 </td>");
                html.Append("<td> 折扣金额 </td >");
                html.Append("<td> 实收金额 </td >");
                html.Append("<td> 点菜员 </td>");
                html.Append("<td> 下单时间 </td>");
                html.Append("</tr>");

                DataTable OpenTable = ds.Tables[1];
                DataTable OrderDishes = ds.Tables[2];
                if (OpenTable != null && OpenTable.Rows.Count > 0 && OrderDishes != null && OrderDishes.Rows.Count > 0)
                {
                    decimal hj_sum_disnum = 0;
                    decimal hj_sum_price = 0;
                    decimal hj_sum_cookmoney = 0;
                    decimal hj_sum_disnum_money = 0;
                    decimal hj_sum_zk_money = 0;
                    decimal hj_sum_ss_money = 0;
                    foreach (DataRow dr in OpenTable.Rows)
                    {

                        html.Append("<tr>");
                        html.Append("<td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color:#808080'>&nbsp;</td>");
                        html.Append("<td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '11' >" + (dr["TypeName"].ToString() + "-" + dr["TableName"].ToString()) + "</td>");
                        html.Append("</tr>");
                        DataRow[] ddrs = OrderDishes.Select("TableCode='" + dr["TableCode"].ToString() + "'");
                        decimal xj_summoney = 0;

                        decimal sum_disnum = 0;
                        decimal sum_price = 0;
                        decimal sum_cookmoney = 0;
                        decimal sum_disnum_money = 0;
                        decimal sum_zk_money = 0;
                        decimal sum_ss_money = 0;
                        foreach (DataRow ddr in OrderDishes.Rows)
                        {
                            html.Append("<tr>");
                            html.Append("<td>&nbsp;</td>");
                            html.Append("<td>&nbsp;</td>");
                            string disname = string.Empty;
                            if (ddr["IsPackage"].ToString() == "2")
                            {
                                disname = "(套)" + ddr["DisName"].ToString();
                            }
                            else
                            {
                                disname = ddr["DisName"].ToString();
                            }
                            html.Append("<td> " + disname + " </td>");
                            string uite = string.Empty;
                            if (ddr["IsPackage"].ToString() == "1")
                            {
                                uite = "套";
                            }
                            else
                            {
                                uite = ddr["DisUite"].ToString();
                            }
                            sum_disnum += Helper.StringToDecimal(ddr["DisNum"].ToString());
                            sum_price += Helper.StringToDecimal(ddr["Price"].ToString());
                            sum_cookmoney += Helper.StringToDecimal(ddr["CookMoney"].ToString());
                            sum_disnum_money += ((Helper.StringToDecimal(ddr["DisNum"].ToString()) - Helper.StringToDecimal(ddr["returnnum"].ToString())) * Helper.StringToDecimal(ddr["Price"].ToString()));
                            sum_zk_money += (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * (Helper.StringToDecimal(ddr["DisNum"].ToString()) - Helper.StringToDecimal(ddr["returnnum"].ToString()));
                            sum_ss_money += Helper.StringToDecimal(ddr["TotalMoney"].ToString());



                            html.Append("<td> " + uite + " </td>");
                            html.Append("<td> " + ddr["DisNum"].ToString() + " </td>");
                            html.Append("<td> " + ddr["Price"].ToString() + " </td>");
                            html.Append("<td> " + ddr["CookMoney"].ToString() + " </td>");
                            html.Append("<td> " + string.Format("{0:N}", (Helper.StringToDecimal(ddr["DisNum"].ToString()) * Helper.StringToDecimal(ddr["Price"].ToString()))) + " </td>");
                            html.Append("<td> " + string.Format("{0:N}", (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * Helper.StringToDecimal(ddr["DisNum"].ToString())) + " </td>");
                            html.Append("<td> " + Helper.StringToDecimal(ddr["TotalMoney"].ToString()) + " </td>");
                            html.Append("<td> " + ddr["CCname"].ToString() + " </td>");
                            html.Append("<td> " + ddr["CTime"].ToString() + " </td>");
                            html.Append("</tr>");
                            if (Helper.StringToDecimal(ddr["returnnum"].ToString()) > 0)
                            {
                                html.Append("<tr>");
                                html.Append("<td>&nbsp;</td>");
                                html.Append("<td>&nbsp;</td>");
                                html.Append("<td> " + disname + " </td>");
                                html.Append("<td> " + uite + " </td>");
                                html.Append("<td> -" + ddr["returnnum"].ToString() + " </td>");
                                html.Append("<td> -" + ddr["Price"].ToString() + " </td>");
                                html.Append("<td> -" + ddr["CookMoney"].ToString() + " </td>");
                                html.Append("<td> -" + string.Format("{0:N}", (Helper.StringToDecimal(ddr["returnnum"].ToString()) * Helper.StringToDecimal(ddr["Price"].ToString()))) + " </td>");
                                html.Append("<td> -" + string.Format("{0:N}", (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * Helper.StringToDecimal(ddr["returnnum"].ToString())) + " </td>");
                                html.Append("<td> 0.00 </td>");
                                html.Append("<td> " + ddr["CCname"].ToString() + " </td>");
                                html.Append("<td> " + ddr["CTime"].ToString() + " </td>");
                                html.Append("</tr>");
                            }
                        }
                        //小计
                        html.Append("<tr>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td> 小计: </td>");
                        html.Append("<td></td>");
                        html.Append("<td> " + sum_disnum + " </td>");
                        html.Append("<td> " + sum_price + " </td>");
                        html.Append("<td> " + sum_cookmoney + " </td>");
                        html.Append("<td> " + sum_disnum_money + " </td>");
                        html.Append("<td> " + sum_zk_money + " </td>");
                        html.Append("<td> " + sum_ss_money + " </td>");
                        html.Append("<td></td>");
                        html.Append("<td></td>");
                        html.Append("</tr>");
                        html.Append("<tr><td></td><td></td><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan='10' ></td></tr>");
                        hj_sum_disnum += sum_disnum;
                        hj_sum_price += sum_price;
                        hj_sum_cookmoney += sum_cookmoney;
                        hj_sum_disnum_money += sum_disnum_money;
                        hj_sum_zk_money += sum_zk_money;
                        hj_sum_ss_money += sum_ss_money;
                    }
                    //合计
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 合计: </td>");
                    html.Append("<td> " + (OpenTable.Rows.Count + "桌") + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + hj_sum_disnum + " </td>");
                    html.Append("<td> " + hj_sum_price + " </td>");
                    html.Append("<td> " + hj_sum_cookmoney + " </td>");
                    html.Append("<td> " + hj_sum_disnum_money + " </td>");
                    html.Append("<td> " + hj_sum_zk_money + " </td>");
                    html.Append("<td> " + hj_sum_ss_money + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td></td>");
                    html.Append("</tr>");
                    html.Append("<tr><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '14' ></td></tr>");
                }
                else if ((OpenTable == null || OpenTable.Rows.Count <= 0) && OrderDishes != null && OrderDishes.Rows.Count > 0)
                {
                    decimal hj_sum_disnum = 0;
                    decimal hj_sum_price = 0;
                    decimal hj_sum_cookmoney = 0;
                    decimal hj_sum_disnum_money = 0;
                    decimal hj_sum_zk_money = 0;
                    decimal hj_sum_ss_money = 0;
                    html.Append("<tr>");
                    html.Append("<td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color:#808080'>&nbsp;</td>");
                    html.Append("<td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '11' ></td>");
                    html.Append("</tr>");
                    decimal xj_summoney = 0;
                    decimal sum_disnum = 0;
                    decimal sum_price = 0;
                    decimal sum_cookmoney = 0;
                    decimal sum_disnum_money = 0;
                    decimal sum_zk_money = 0;
                    decimal sum_ss_money = 0;
                    foreach (DataRow ddr in OrderDishes.Rows)
                    {
                        html.Append("<tr>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>&nbsp;</td>");
                        string disname = string.Empty;
                        if (ddr["IsPackage"].ToString() == "2")
                        {
                            disname = "(套)" + ddr["DisName"].ToString();
                        }
                        else
                        {
                            disname = ddr["DisName"].ToString();
                        }
                        html.Append("<td> " + disname + " </td>");
                        string uite = string.Empty;
                        if (ddr["IsPackage"].ToString() == "1")
                        {
                            uite = "套";
                        }
                        else
                        {
                            uite = ddr["DisUite"].ToString();
                        }
                        sum_disnum += Helper.StringToDecimal(ddr["DisNum"].ToString());
                        sum_price += Helper.StringToDecimal(ddr["Price"].ToString());
                        sum_cookmoney += Helper.StringToDecimal(ddr["CookMoney"].ToString());
                        sum_disnum_money += (Helper.StringToDecimal(ddr["DisNum"].ToString()) * Helper.StringToDecimal(ddr["Price"].ToString()));
                        sum_zk_money += (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * Helper.StringToDecimal(ddr["DisNum"].ToString());
                        sum_ss_money += Helper.StringToDecimal(ddr["TotalMoney"].ToString());



                        html.Append("<td> " + uite + " </td>");
                        html.Append("<td> " + ddr["DisNum"].ToString() + " </td>");
                        html.Append("<td> " + ddr["Price"].ToString() + " </td>");
                        html.Append("<td> " + ddr["CookMoney"].ToString() + " </td>");
                        html.Append("<td> " + string.Format("{0:N}", (Helper.StringToDecimal(ddr["DisNum"].ToString()) * Helper.StringToDecimal(ddr["Price"].ToString()))) + " </td>");
                        html.Append("<td> " + string.Format("{0:N}", (Helper.StringToDecimal(ddr["Price"].ToString()) - Helper.StringToDecimal(ddr["DiscountPrice"].ToString())) * Helper.StringToDecimal(ddr["DisNum"].ToString())) + " </td>");
                        html.Append("<td> " + Helper.StringToDecimal(ddr["TotalMoney"].ToString()) + " </td>");
                        html.Append("<td> " + ddr["CCname"].ToString() + " </td>");
                        html.Append("<td> " + ddr["CTime"].ToString() + " </td>");
                        html.Append("</tr>");
                    }
                    //小计
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 小计: </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + sum_disnum + " </td>");
                    html.Append("<td> " + sum_price + " </td>");
                    html.Append("<td> " + sum_cookmoney + " </td>");
                    html.Append("<td> " + sum_disnum_money + " </td>");
                    html.Append("<td> " + sum_zk_money + " </td>");
                    html.Append("<td> " + sum_ss_money + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td></td>");
                    html.Append("</tr>");
                    html.Append("<tr><td></td><td></td><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan='10' ></td></tr>");
                    hj_sum_disnum += sum_disnum;
                    hj_sum_price += sum_price;
                    hj_sum_cookmoney += sum_cookmoney;
                    hj_sum_disnum_money += sum_disnum_money;
                    hj_sum_zk_money += sum_zk_money;
                    hj_sum_ss_money += sum_ss_money;

                    //合计
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 合计: </td>");
                    html.Append("<td>  </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + hj_sum_disnum + " </td>");
                    html.Append("<td> " + hj_sum_price + " </td>");
                    html.Append("<td> " + hj_sum_cookmoney + " </td>");
                    html.Append("<td> " + hj_sum_disnum_money + " </td>");
                    html.Append("<td> " + hj_sum_zk_money + " </td>");
                    html.Append("<td> " + hj_sum_ss_money + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td></td>");
                    html.Append("</tr>");
                    html.Append("<tr><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '14' ></td></tr>");
                }
                html.Append("<tr>");
                html.Append("<td colspan='15' valign='top'>");
                html.Append("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0' style='width:100%;margin:0px;'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td width ='50px'>&nbsp;</td>");
                html.Append("<td style = 'border-bottom-style:solid; border-bottom-width: 1px; border-bottom-color: #808080'> 科目名称 </td>");
                html.Append("<td style='border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #808080'> 消费项目 </td>");
                html.Append("<td style='border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #808080'> 结算项目 </td>");
                html.Append("<td style='border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #808080'> 备注 </td>");
                html.Append("<td style='border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #808080'> 操作员 </td>");
                html.Append("</tr>");
                //菜品类目
                DataTable OrderDisheFin = ds.Tables[3];
                if (OrderDisheFin != null && OrderDisheFin.Rows.Count > 0)
                {
                    foreach (DataRow dr in OrderDisheFin.Rows)
                    {
                        html.Append("<tr>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td> " + dr["FinTypeName"].ToString() + " </td>");
                        html.Append("<td> " + dr["TotalMoney"].ToString() + " </td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("</tr>");
                    }
                }
                DataTable memOrder = ds.Tables[6];//会员卡订单的一次只能充值一张卡
                if (memOrder != null && memOrder.Rows.Count > 0)
                {
                    foreach (DataRow dr in memOrder.Rows)//其实只有一条记录
                    {
                        DataTable mem = new bllMemcardLevel().GetMemcard("where 1 = 1 and m.cardcode='" + dr["cardcode"].ToString() + "'");
                        if (mem != null && mem.Rows.Count > 0)
                        {
                            html.Append("<tr>");
                            html.Append("<td>&nbsp;</td>");
                            html.Append("<td> " + mem.Rows[0]["typename"].ToString() + " </td>");
                            html.Append("<td> " + dr["payamount"].ToString() + " </td>");
                            html.Append("<td>&nbsp;</td>");
                            html.Append("<td>卡号" + mem.Rows[0]["cardcode"].ToString() + "</td>");
                            html.Append("<td>&nbsp;</td>");
                            html.Append("</tr>");
                        }

                    }
                }


                decimal dishessummoney = OrderDisheFin.Select().Sum(x => x.Field<decimal>("TotalMoney"));
                decimal paysummoney = 0;
                //收款项
                //优惠券信息
                DataTable Coupon = ds.Tables[5];
                if (Coupon != null && Coupon.Rows.Count > 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 优惠券 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>" + Coupon.Select().Sum(x => x.Field<decimal>("RealPay")) + "</td>");
                    html.Append("<td> 普通券:商品券:");
                    foreach (DataRow dr in Coupon.Rows)
                    {
                        html.Append(dr["CouponCode"].ToString() + "</br>");
                    }
                    html.Append("</td>");
                    html.Append("<td>" + TR_Bill["CCname"].ToString() + "</td>");
                    html.Append("</tr>");
                    paysummoney += Coupon.Select().Sum(x => x.Field<decimal>("RealPay"));
                }
                //折扣
                if (Helper.StringToDecimal(TR_Bill["DiscountMoney"].ToString()) != 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 折扣 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>" + TR_Bill["DiscountMoney"].ToString() + "</td>");
                    html.Append("<td>折扣方案名称:" + TR_Bill["DiscountName"].ToString() + "</td>");
                    html.Append("<td>" + TR_Bill["CCname"].ToString() + "</td>");
                    html.Append("</tr>");
                    paysummoney += Helper.StringToDecimal(TR_Bill["DiscountMoney"].ToString());
                }
                //抹零
                if (Helper.StringToDecimal(TR_Bill["ZeroCutMoney"].ToString()) != 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 抹零 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>" + TR_Bill["ZeroCutMoney"].ToString() + "</td>");
                    html.Append("<td></td>");
                    html.Append("<td>" + TR_Bill["CCname"].ToString() + "</td>");
                    html.Append("</tr>");
                    paysummoney += Helper.StringToDecimal(TR_Bill["ZeroCutMoney"].ToString());
                }
                //积分抵扣
                if (Helper.StringToDecimal(TR_Bill["PointMoney"].ToString()) != 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 积分抵扣 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td>" + TR_Bill["PointMoney"].ToString() + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + TR_Bill["CCname"].ToString() + " </td>");
                    html.Append("</tr>");
                    paysummoney += Helper.StringToDecimal(TR_Bill["PointMoney"].ToString());
                }
                //虚拟币抵扣
                if (Helper.StringToDecimal(TR_Bill["VirMoney"].ToString()) != 0)
                {
                    html.Append("<tr>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> 虚拟币抵扣 </td>");
                    html.Append("<td>&nbsp;</td>");
                    html.Append("<td> " + TR_Bill["VirMoney"].ToString() + " </td>");
                    html.Append("<td></td>");
                    html.Append("<td> " + TR_Bill["CCname"].ToString() + " </td>");
                    html.Append("</tr>");
                    paysummoney += Helper.StringToDecimal(TR_Bill["VirMoney"].ToString());
                }
                //支付项
                DataTable Pay = ds.Tables[4];
                if (Pay != null && Pay.Rows.Count > 0)
                {
                    //判断是否存在会员卡支付
                    DataRow[] mempay = Pay.Select("PayMethodCode='7'");
                    if (mempay != null && mempay.Length > 0)
                    {
                        string where = " where 1=1 and m.cardcode in(";
                        foreach (DataRow mdr in mempay)
                        {
                            if (!string.IsNullOrEmpty(mdr["MemberCard"].ToString()))
                            {
                                string[] d = mdr["MemberCard"].ToString().Split(' ');
                                where += "'" + d[0] + "',";
                            }
                        }
                        where = where.TrimEnd(',') + ")";
                        //获取会员卡信息
                        DataTable mem = new bllMemcardLevel().GetMemcard(where);
                        foreach (DataRow dr in mempay)
                        {
                            DataRow[] mdr = mem.Select("cardcode='" + dr["MemberCard"].ToString() + "' or memcode='" + dr["MemberCard"].ToString() + "'");
                            string value = string.Empty;
                            string cardtypename = string.Empty;
                            if (mdr != null && mdr.Length > 0)
                            {
                                cardtypename = mdr[0]["typename"].ToString();
                                value = " 卡号:" + mdr[0]["cardcode"].ToString() + " 名称:" + dr["MemberName"].ToString() + " 折扣模板:" + (dr["MemberDiscount"].ToString() == "" ? "无" : dr["MemberDiscount"].ToString());
                            }
                            if (cardtypename != "储值卡" && cardtypename != "员工卡" && cardtypename != "美食卡")
                            {
                                cardtypename = "贵宾卡";
                            }
                            html.Append("<tr>");
                            html.Append("<td>&nbsp;</td>");
                            html.Append("<td> " + cardtypename + " </td>");
                            html.Append("<td> &nbsp; </td>");
                            html.Append("<td> " + dr["PayMoney"].ToString() + " </td>");
                            html.Append("<td> " + value + "</td>");
                            html.Append("<td> " + TR_Bill["CCname"].ToString() + " </td>");
                            html.Append("</tr>");
                        }
                    }
                    DataRow[] paydr = Pay.Select("PayMethodCode<>'7'");
                    foreach (DataRow dr in paydr)
                    {
                        html.Append("<tr>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td> " + dr["PayMethodName"].ToString() + " </td>");
                        html.Append("<td>&nbsp;</td>");
                        html.Append("<td>" + dr["PayMoney"].ToString() + " </td>");
                        html.Append("<td></td>");
                        html.Append("<td> " + TR_Bill["CCname"].ToString() + " </td>");
                        html.Append("</tr>");
                    }
                    paysummoney += Pay.Select().Sum(x => x.Field<decimal>("PayMoney"));
                }
                html.Append("<tr><td style='border-bottom-style:solid;border-bottom-width:1px;border-bottom-color: #808080' colspan = '14' ></td></tr>");
                html.Append("<tr>");
                html.Append("<td>合计:</td>");
                html.Append("<td></td>");
                html.Append("<td>" + dishessummoney + "</td>");
                html.Append("<td>" + paysummoney + "</td>");
                html.Append("<td></td>");
                html.Append("<td></td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
            }
            string json = "{";
            json += "\"data\":\"" + html.ToString() + "\"";
            json += "}";
            Pagcontext.Response.Write(json);
        }



        private void OrdertcdpReport(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "StartTime", "EndTime", "userid", "StoCode", "ShiftCode", "Type", "StoName", "DisCode", "PDisCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string StartTime = dicPar["StartTime"].ToString();
            string EndTime = dicPar["EndTime"].ToString();
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string ShiftCode = dicPar["ShiftCode"].ToString();
            string StoName = dicPar["StoName"].ToString();
            string Type = dicPar["Type"].ToString();
            string DisCode = dicPar["DisCode"].ToString();
            string PDisCode = dicPar["PDisCode"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }

            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));//
            dt_Result.Columns.Add("field2", typeof(string));//
            dt_Result.Columns.Add("field2_url", typeof(string));
            dt_Result.Columns.Add("field3", typeof(string));//
            dt_Result.Columns.Add("field4", typeof(string));//
            dt_Result.Columns.Add("field5", typeof(string));//
            dt_Result.Columns.Add("field6", typeof(string));//

            DataSet ds = bll.OrdertcdpReport(StartTime, EndTime, userid, StoCode, ShiftCode, Type, DisCode, PDisCode, BusCode);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            ArrayList arrayList = new ArrayList();
            List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题
            List<LayUItableHelper> Title_list2 = new List<LayUItableHelper>();//第二行标题
            List<LayUItableHelper> Title_list3 = new List<LayUItableHelper>();//第三行标题
            if (dt != null)
            {
                #region 组织表头，注意顺序
                string title = string.Empty;
                if (!string.IsNullOrEmpty(DisCode))
                {
                    if (Type == "1")
                    {
                        title = "套内单品统计";
                    }
                    else
                    {
                        title = "单品统计";
                    }
                }
                else
                {
                    title = StoName + "账单套餐单品统计报表";
                }
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field0", Title = title, Align = "center", Colspan = 6 });
                Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field0", Title = "查询日期" + DateTime.Parse(StartTime).ToString("yyyy.MM.dd") + "-" + DateTime.Parse(EndTime).ToString("yyyy.MM.dd"), Align = "center", Colspan = 6 });
                //固定title
                Title_list3.Add(new LayUItableHelper { Sort = false, type = "numbers", Field = "field1", Title = "序号", Align = "center", TotalRowText = "合计" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field2", Title = "账单号", Align = "center", HeadTemplet = "field2_url", Style = "cursor: pointer;color: #0000FF" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field3", Title = "菜品编号", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field4", Title = "菜品名称", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field5", Title = "数量", Align = "center", TotalRow = true });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field6", Title = "总金额", Align = "center", TotalRow = true });
                arrayList.Add(Title_list1);//第一行表头
                arrayList.Add(Title_list2);//第二行表头
                arrayList.Add(Title_list3);//第三行表头
                #endregion

                if (title == "套内单品统计")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string discode = dr["DisCode"].ToString();
                        string disname = dr["DisName"].ToString();
                        if(dr["DisNum"].ToString() == "0.00")
                        {
                            continue;
                        }
                        DataRow newdr = dt_Result.NewRow();
                        newdr["field2"] = dr["PKCode"].ToString();
                        newdr["field2_url"] = @"./BillingDetails.html?StoCode=" + StoCode + "&StoName=" + StoName + "&BillCode=" + dr["PKCode"].ToString();
                        newdr["field3"] = dr["DisCode"].ToString();
                        newdr["field4"] = dr["DisName"].ToString();
                        newdr["field5"] = dr["DisNum"].ToString() == "0.00" ? "-" : dr["DisNum"].ToString();
                        if (!string.IsNullOrEmpty(dr["pdiscode"].ToString()))
                        {
                            string code = dr["pdiscode"].ToString();
                            DataRow[] drs = dt1.Select("pkcode='" + dr["pdiscode"].ToString() + "'");
                            if (drs.Length > 0)
                            {
                                dr["TotalMoney"] = "0.0000";
                            }
                        }
                        newdr["field6"] = dr["TotalMoney"].ToString() == "0.0000" ? "-" : dr["TotalMoney"].ToString();
                        dt_Result.Rows.Add(newdr);
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Helper.StringToDecimal(dr["DisNum"].ToString()) <= 0)
                        {
                            continue;
                        }
                        DataRow newdr = dt_Result.NewRow();
                        newdr["field2"] = dr["PKCode"].ToString();
                        newdr["field2_url"] = @"./BillingDetails.html?StoCode=" + StoCode + "&StoName=" + StoName + "&BillCode=" + dr["PKCode"].ToString();
                        newdr["field3"] = dr["DisCode"].ToString();
                        newdr["field4"] = dr["DisName"].ToString();
                        newdr["field5"] = dr["DisNum"].ToString() == "0.00" ? "-" : dr["DisNum"].ToString();
                        newdr["field6"] = dr["TotalMoney"].ToString() == "0.0000" ? "-" : dr["TotalMoney"].ToString();
                        dt_Result.Rows.Add(newdr);
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

        private void OrdertcdpzjReport(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "StartTime", "EndTime", "userid", "StoCode", "Type", "StoName", "DisCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string StartTime = dicPar["StartTime"].ToString();
            string EndTime = dicPar["EndTime"].ToString();
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string StoName = dicPar["StoName"].ToString();
            string Type = dicPar["Type"].ToString();
            string DisCode = dicPar["DisCode"].ToString();
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
            dt_Result.Columns.Add("field3_url", typeof(string));
            dt_Result.Columns.Add("field4", typeof(string));

            DataSet ds = bll.OrdertcdpzjReport(StartTime, EndTime, userid, StoCode, Type, DisCode, BusCode);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            ArrayList arrayList = new ArrayList();
            List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题
            List<LayUItableHelper> Title_list2 = new List<LayUItableHelper>();//第二行标题
            List<LayUItableHelper> Title_list3 = new List<LayUItableHelper>();//第三行标题
            if (dt != null)
            {
                #region 组织表头，注意顺序
                string title = string.Empty;
                if (Type == "1")
                {
                    title = "套内单品汇总";
                }
                else
                {
                    title = "单品汇总";
                }
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field0", Title = title, Align = "center", Colspan = 6 });
                Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field0", Title = "查询日期" + DateTime.Parse(StartTime).ToString("yyyy.MM.dd") + "-" + DateTime.Parse(EndTime).ToString("yyyy.MM.dd"), Align = "center", Colspan = 6 });
                //固定title
                Title_list3.Add(new LayUItableHelper { Sort = false, type = "numbers", Field = "field0", Title = "序号", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field1", Title = "菜品编号", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field2", Title = "菜品名称", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field3", Title = "数量", Align = "center", HeadTemplet = "field3_url", Style = "cursor: pointer;color: #0000FF" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field4", Title = "总金额", Align = "center" });
                arrayList.Add(Title_list1);//第一行表头
                arrayList.Add(Title_list2);//第二行表头
                arrayList.Add(Title_list3);//第三行表头
                #endregion

                if (title == "套内单品汇总")
                {
                    string disnames = ",";
                    foreach (DataRow dr in dt.Rows)
                    {

                        if (disnames.Contains("," + dr["DisName"].ToString() + ","))
                        {
                            continue;
                        }
                        string discode = dr["DisCode"].ToString();
                        string disname = dr["DisName"].ToString();
                        DataRow[] dataRows = dt.Select("DisCode='" + discode + "' and DisName='" + disname + "'");
                        decimal disnum = dataRows.Sum(x => x.Field<decimal>("DisNum"));
                        if(disnum<=0)
                        {
                            continue;
                        }
                        decimal totalmoney = dataRows.Sum(x => x.Field<decimal>("TotalMoney"));
                        DataRow newdr = dt_Result.NewRow();
                        newdr["field1"] = discode;
                        newdr["field2"] = disname;
                        newdr["field3"] = disnum.ToString() == "0.00" ? "-" : disnum.ToString();
                        if (Type == "1")
                        {
                            newdr["field3_url"] = @"./zdtcdptjbb.html?StoCode=" + StoCode + "&StoName=" + StoName + "&StartTime=" + StartTime + "&EndTime=" + EndTime + "&type=" + Type + "&DisCode=" + dr["DisCode"].ToString() + "&PDisCode=" + DisCode;
                        }
                        else
                        {
                            newdr["field3_url"] = @"./zdtcdptjbb.html?StoCode=" + StoCode + "&StoName=" + StoName + "&StartTime=" + StartTime + "&EndTime=" + EndTime + "&type=" + Type + "&DisCode=" + dr["DisCode"].ToString() + "&PDisCode=";
                        }
                        if (dataRows.Length > 0)//判断是否有赠送的，进行逻辑去除
                        {
                            foreach (DataRow zdr in dataRows)
                            {
                                if (!string.IsNullOrEmpty(zdr["pdiscode"].ToString()))
                                {
                                    string code = zdr["pdiscode"].ToString();
                                    DataRow[] drs = dt1.Select("pkcode='" + zdr["pdiscode"].ToString() + "'");
                                    if (drs.Length > 0)
                                    {
                                        totalmoney = totalmoney - decimal.Parse(zdr["TotalMoney"].ToString());//套餐赠送的不计算金额
                                    }
                                }
                            }

                        }
                        newdr["field4"] = totalmoney.ToString() == "0.0000" ? "-" : totalmoney.ToString();
                        dt_Result.Rows.Add(newdr);
                        disnames += disname + ",";
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow newdr = dt_Result.NewRow();
                        newdr["field1"] = dr["DisCode"].ToString();
                        newdr["field2"] = dr["DisName"].ToString();
                        if(dr["DisNum"].ToString()=="0.00")
                        {
                            continue;
                        }
                        newdr["field3"] = dr["DisNum"].ToString() == "0.00" ? "-" : dr["DisNum"].ToString();
                        if (Type == "1")
                        {
                            newdr["field3_url"] = @"./zdtcdptjbb.html?StoCode=" + StoCode + "&StoName=" + StoName + "&StartTime=" + StartTime + "&EndTime=" + EndTime + "&type=" + Type + "&DisCode=" + dr["DisCode"].ToString() + "&PDisCode=" + DisCode;
                        }
                        else
                        {
                            newdr["field3_url"] = @"./zdtcdptjbb.html?StoCode=" + StoCode + "&StoName=" + StoName + "&StartTime=" + StartTime + "&EndTime=" + EndTime + "&type=" + Type + "&DisCode=" + dr["DisCode"].ToString() + "&PDisCode=";
                        }
                        newdr["field4"] = dr["TotalMoney"].ToString() == "0.0000" ? "-" : dr["TotalMoney"].ToString();
                        dt_Result.Rows.Add(newdr);
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
            else
            {
                string nulljson = "{";
                nulljson += "\"title\":[[{\"field\": \"err\",\"title\":\"\",\"align\":\"center\"}]],";
                nulljson += "\"data\":[{\"err\":\"无数据\"]";
                nulljson += "}";
                Pagcontext.Response.Write(nulljson);
                return;
            }
        }

        private void GetStoSsBb(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "userid", "StoCode", "StartTime", "EndTime", "StoName" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string StartTime = dicPar["StartTime"].ToString();
            string EndTime = dicPar["EndTime"].ToString();
            string StoName = dicPar["StoName"].ToString();
            SqlParameter[] ps =
           {
                new SqlParameter("@stocode",StoCode),
                new SqlParameter("@startdate",StartTime),
                new SqlParameter("@enddate",EndTime)
            };

            DataSet ds = new BLL.bllPaging().GetDatasetByProcedure("[dbo].[p_StoreTodaySSReportList]", ps);
            //获取数据
            if (ds != null && ds.Tables.Count == 10)
            {
                //第一张汇总表
                DataTable dt1 = ds.Tables[0];
                StringBuilder html = new StringBuilder();
                html.Append("<table id = 'reportList' class='List_tab' style ='min-width: 920px; border-collapse:separate;border: 1px solid #bdbdbd;' cellspacing ='0' cellpadding ='0'> ");
                html.Append("<tbody><tr class='list_tab_tit' style ='font-weight: bold; '><td colspan = '4' class='td_boldwidth' align ='center' style ='border-top: 1px solid #bdbdbd;' > " + StoName + "实时报表</td></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold; '><td colspan = '2' style ='font-weight: bold;' align ='center' > 销售总额：</td><td colspan = '2' > " + dt1.Rows[0]["summoney"] + " </td></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold; '><td colspan = '2' style='font-weight: bold;' align ='center' > 礼券不找零：</td><td colspan = '2' > " + dt1.Rows[0]["lqbzl"] + " </td></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold; '><td colspan = '2' style ='font-weight: bold;' align ='center' > 纯销售额：</td><td colspan = '2' > " + dt1.Rows[0]["cxse"] + " </td></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold; '><td colspan = '2' style ='font-weight: bold;' align ='center' > 来客数：</td><td colspan = '2' > " + dt1.Rows[0]["lks"] + " </td ></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold; '><td colspan = '2' style ='font-weight: bold;' align ='center' > 账单数：</td><td colspan ='2' > " + dt1.Rows[0]["zds"] + " </td ></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold; '><td colspan = '2' style ='font-weight: bold;' align ='center' > 每客单价：</td><td colspan ='2' > " + dt1.Rows[0]["mkdj"] + " </td></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold; '><td colspan = '2' style ='font-weight: bold;' align ='center' > 每单单价：</td><td colspan ='2' > " + dt1.Rows[0]["mddj"] + " </td></tr >");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold; '><td colspan = '2' style ='font-weight: bold;' align ='center' > 每日翻台率：</td><td colspan = '2' > " + dt1.Rows[0]["mrftl"] + " </td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 920px; height: 25px; '><tbody><tr><td style='height: 25px; border: 0px; '>&nbsp;</td></tr></tbody></table>");

                //第二张表,收款情况
                DataTable dt2 = new DataTable();
                dt2 = ds.Tables[1];
                string payccnames = string.Empty;
                string paynames = string.Empty;
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt2.Rows)
                    {
                        if (!paynames.Contains(dr["PayMethodName"].ToString()))
                        {
                            paynames += dr["PayMethodName"].ToString() + ",";
                        }
                        if (!payccnames.Contains(dr["CCname"].ToString()))
                        {
                            payccnames += dr["CCname"].ToString() + ",";
                        }
                    }
                }
                payccnames = payccnames.TrimEnd(',');
                paynames = paynames.TrimEnd(',');
                html.Append("<table class='List_tab' style='width: 920px; border-collapse:separate;border: 1px solid #bdbdbd;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody><tr class='list_tab_tit' style='font-weight: bold;'><td colspan = '" + (payccnames.Split(',').Count() + 2) + "' class='td_boldwidth' align='left' style='border-top: 1px solid #bdbdbd;text-align:left;padding-left:20px;'>收款情况</td></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                html.Append("<td class='td_boldwidth' align='center'>付款方式</td>");
                foreach (string ccname in payccnames.Split(','))
                {
                    if (!string.IsNullOrEmpty(ccname))
                    {
                        html.Append("<td class='td_boldwidth' align='center'>" + ccname + "</td>");
                    }
                }
                html.Append("<td class='td_boldwidth' align='center'>合计</td></tr>");
                Dictionary<string, double> hj = new Dictionary<string, double>();
                foreach (string pyname in paynames.Split(','))
                {
                    if (!string.IsNullOrEmpty(pyname))
                    {
                        if (pyname == "优免")
                        {
                            continue;
                        }
                        html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                        html.Append("<td class='td_boldwidth' align='center'>" + pyname + "</td>");
                        double summoney = 0;
                        foreach (string ccname in payccnames.Split(','))
                        {
                            if (!string.IsNullOrEmpty(ccname))
                            {
                                DataRow[] drs = dt2.Select("PayMethodName='" + pyname + "' and CCname='" + ccname + "'");
                                if (drs != null && drs.Length > 0)
                                {
                                    summoney += Helper.StringToDouble(drs[0]["money"].ToString());
                                    if (!hj.ContainsKey(ccname))
                                    {
                                        hj.Add(ccname, Helper.StringToDouble(drs[0]["money"].ToString()));
                                    }
                                    else
                                    {
                                        hj[ccname] = hj[ccname] + Helper.StringToDouble(drs[0]["money"].ToString());
                                    }
                                    html.Append("<td class='td_boldwidth' align='center'>" + drs[0]["money"].ToString() + "</td>");
                                }
                                else
                                {
                                    html.Append("<td class='td_boldwidth' align='center'>0.00</td>");
                                }
                            }
                        }
                        html.Append("<td class='td_boldwidth' align='center'>" + summoney + "</td></tr>");
                    }
                }
                //优惠券
                DataTable cdt = ds.Tables[2];
                if (cdt != null && cdt.Rows.Count > 0)
                {
                    html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                    html.Append("<td class='td_boldwidth' align='center'>优惠券</td>");
                    double summoney = 0;
                    foreach (string ccname in payccnames.Split(','))
                    {
                        if (!string.IsNullOrEmpty(ccname))
                        {
                            DataRow[] drs = cdt.Select("CCname='" + ccname + "'");
                            if (drs != null && drs.Length > 0)
                            {
                                summoney += Helper.StringToDouble(drs[0]["RealPay"].ToString());
                                if (!hj.ContainsKey(ccname))
                                {
                                    hj.Add(ccname, Helper.StringToDouble(drs[0]["RealPay"].ToString()));
                                }
                                else
                                {
                                    hj[ccname] = hj[ccname] + Helper.StringToDouble(drs[0]["RealPay"].ToString());
                                }
                                html.Append("<td class='td_boldwidth' align='center'>" + drs[0]["RealPay"].ToString() + "</td>");
                            }
                            else
                            {
                                html.Append("<td class='td_boldwidth' align='center'>0.00</td>");
                            }
                        }
                    }
                    html.Append("<td class='td_boldwidth' align='center'>" + summoney + "</td></tr>");
                }

                //优惠券虚增
                if (StoCode == "08")
                {
                    if (cdt != null && cdt.Rows.Count > 0)
                    {
                        html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                        html.Append("<td class='td_boldwidth' align='center'>优惠券虚增</td>");
                        double summoney = 0;
                        foreach (string ccname in payccnames.Split(','))
                        {
                            if (!string.IsNullOrEmpty(ccname))
                            {
                                DataRow[] drs = cdt.Select("CCname='" + ccname + "'");
                                if (drs != null && drs.Length > 0)
                                {
                                    summoney += Helper.StringToDouble(drs[0]["VIMoney"].ToString());
                                    if (!hj.ContainsKey(ccname))
                                    {
                                        hj.Add(ccname, Helper.StringToDouble(drs[0]["VIMoney"].ToString()));
                                    }
                                    else
                                    {
                                        hj[ccname] = hj[ccname] + Helper.StringToDouble(drs[0]["VIMoney"].ToString());
                                    }
                                    html.Append("<td class='td_boldwidth' align='center'>" + drs[0]["VIMoney"].ToString() + "</td>");
                                }
                                else
                                {
                                    html.Append("<td class='td_boldwidth' align='center'>0.00</td>");
                                }
                            }
                        }
                        html.Append("<td class='td_boldwidth' align='center'>" + summoney + "</td></tr>");
                    }
                }

                //优免
                #region 优免
                html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                html.Append("<td class='td_boldwidth' align='center'>优免</td>");
                foreach (string tn in payccnames.Split(','))
                {
                    if (!string.IsNullOrEmpty(tn))
                    {
                        html.Append("<td class='td_boldwidth' align='center'>0</td>");
                    }
                }
                html.Append("<td class='td_boldwidth' align='center'>0</td></tr>");
                #endregion

                DataTable dt4 = ds.Tables[4];//折扣抹零
                #region 折扣
                html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                html.Append("<td class='td_boldwidth' align='center'>折扣</td>");
                double zk_summoney = 0;
                foreach (string ccname in payccnames.Split(','))
                {
                    if (!string.IsNullOrEmpty(ccname))
                    {
                        DataRow[] drs = dt4.Select("CCname='" + ccname + "'");
                        if (drs != null && drs.Length > 0)
                        {
                            zk_summoney += Helper.StringToDouble(drs[0]["DiscountMoney"].ToString());
                            if (!hj.ContainsKey(ccname))
                            {
                                hj.Add(ccname, Helper.StringToDouble(drs[0]["DiscountMoney"].ToString()));
                            }
                            else
                            {
                                hj[ccname] = hj[ccname] + Helper.StringToDouble(drs[0]["DiscountMoney"].ToString());
                            }
                            html.Append("<td class='td_boldwidth' align='center'>" + drs[0]["DiscountMoney"].ToString() + "</td>");
                        }
                        else
                        {
                            html.Append("<td class='td_boldwidth' align='center'>0</td>");
                        }
                    }
                }
                html.Append("<td class='td_boldwidth' align='center'>" + zk_summoney + "</td></tr>");
                #endregion

                #region 抹零
                html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                html.Append("<td class='td_boldwidth' align='center'>抹零</td>");
                double ml_summoney = 0;
                foreach (string ccname in payccnames.Split(','))
                {
                    if (!string.IsNullOrEmpty(ccname))
                    {
                        DataRow[] drs = dt4.Select("CCname='" + ccname + "'");
                        if (drs != null && drs.Length > 0)
                        {
                            ml_summoney += Helper.StringToDouble(drs[0]["ZeroCutMoney"].ToString());
                            if (!hj.ContainsKey(ccname))
                            {
                                hj.Add(ccname, Helper.StringToDouble(drs[0]["ZeroCutMoney"].ToString()));
                            }
                            else
                            {
                                hj[ccname] = hj[ccname] + Helper.StringToDouble(drs[0]["ZeroCutMoney"].ToString());
                            }
                            html.Append("<td class='td_boldwidth' align='center'>" + drs[0]["ZeroCutMoney"].ToString() + "</td>");
                        }
                        else
                        {
                            html.Append("<td class='td_boldwidth' align='center'>0</td>");
                        }
                    }
                }
                html.Append("<td class='td_boldwidth' align='center'>" + ml_summoney + "</td></tr>");
                #endregion


                html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                html.Append("<td class='td_boldwidth' align='center'>合计</td>");
                double zj = 0;
                foreach (string key in payccnames.Split(','))
                {
                    if (hj.ContainsKey(key))
                    {
                        zj += hj[key];
                        html.Append("<td class='td_boldwidth' align='center'>" + hj[key] + "</td>");
                    }
                    else
                    {
                        html.Append("<td class='td_boldwidth' align='center'>0</td>");
                    }
                }
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    html.Append("<td class='td_boldwidth' align='center'>" + zj + "</td>");
                }

                html.Append("</tr></tbody></table>");

                html.Append("<table style='width: 920px; height: 25px; '><tbody><tr><td style='height: 25px; border: 0px; '>&nbsp;</td></tr></tbody></table>");

                //第三张表，部门销售情况
                DataTable dt3 = ds.Tables[7];//桌台类型
                DataTable dt7 = ds.Tables[3];//订单开台记录
                DataTable dt8 = ds.Tables[8];//桌台信息表
                DataTable dt5 = ds.Tables[5];//订单菜品
                DataTable dt9 = ds.Tables[9];//菜谱，快餐不按桌台统计，按菜谱统计
                string tabletype = string.Empty;
                if (dt3 != null && dt3.Rows.Count > 0 && dt7 != null && dt7.Rows.Count > 0 && dt5 != null && dt5.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt3.Rows)
                    {
                        if (!tabletype.Contains(dr["TypeName"].ToString()))
                        {
                            tabletype += dr["TypeName"].ToString() + ",";
                        }
                    }
                }
                else
                {
                    if (dt5 != null && dt5.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt9.Rows)
                        {
                            if (!tabletype.Contains(dr["MenuName"].ToString()))
                            {
                                tabletype += dr["MenuName"].ToString() + ",";
                            }
                        }
                    }
                }
                tabletype = tabletype.TrimEnd(',');
                html.Append("<table class='List_tab' style='width: 920px; border-collapse:separate;border: 1px solid #bdbdbd;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody><tr class='list_tab_tit' style='font-weight: bold;'><td colspan = '" + (tabletype.Split(',').Count() + 2) + "' class='td_boldwidth' align='left' style='border-top: 1px solid #bdbdbd;text-align:left;padding-left:20px;'>部门销售</td></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                html.Append("<td class='td_boldwidth' align='center'>财务类别</td>");
                foreach (string tn in tabletype.Split(','))
                {
                    if (!string.IsNullOrEmpty(tn))
                    {
                        html.Append("<td class='td_boldwidth' align='center'>" + tn + "</td>");
                    }
                }
                html.Append("<td class='td_boldwidth' align='center'>合计</td></tr>");


                #region 财务类别

                if (dt5 != null && dt5.Rows.Count > 0)
                {
                    //一级类别获取
                    string ptypenames = string.Empty;
                    foreach (DataRow dr in dt5.Rows)
                    {
                        if (!ptypenames.Contains(dr["btypename"].ToString()))
                        {
                            ptypenames += dr["btypename"].ToString() + ",";
                        }
                    }
                    ptypenames = ptypenames.TrimEnd(',');

                    Dictionary<string, double> type_hj = new Dictionary<string, double>();
                    foreach (string ptypename in ptypenames.Split(','))
                    {
                        Dictionary<string, double> xj = new Dictionary<string, double>();
                        if (!string.IsNullOrEmpty(ptypename))
                        {
                            html.Append("<tr class='list_tab_tit' style='font-weight: bold;'><td colspan = '" + (tabletype.Split(',').Count() + 2) + "' class='td_boldwidth' align='left' style='border-top: 1px solid #bdbdbd;text-align:left;'>" + ptypename + "</td></tr>");
                            //获取二级类别
                            string typenames = string.Empty;
                            DataRow[] drr = dt5.Select("btypename='" + ptypename + "'");
                            if (drr != null && drr.Length > 0)
                            {
                                foreach (DataRow tdr in drr)
                                {
                                    if (!typenames.Contains(tdr["typename"].ToString()))
                                    {
                                        typenames += tdr["typename"].ToString() + ",";
                                    }
                                }
                                typenames = typenames.TrimEnd(',');

                                //开始统计
                                foreach (string typename in typenames.Split(','))//菜品二级类别
                                {
                                    html.Append("<tr class='list_tab_tit' style='font-weight: bold;'><td class='td_boldwidth' align='left' style='border-top: 1px solid #bdbdbd;text-align:left;padding-left:30px;'>" + typename + "</td>");
                                    double type_sum_money = 0;
                                    if (dt7 != null && dt7.Rows.Count > 0 && dt5 != null && dt5.Rows.Count > 0)
                                    {
                                        foreach (string tabletypename in tabletype.Split(','))//统计各个桌台类型
                                        {
                                            if (!string.IsNullOrEmpty(tabletypename))
                                            {
                                                DataRow[] opcodes = dt7.Select("TypeName='" + tabletypename + "'");//找到该桌台类型的所有的开台号
                                                string opencodes = "opencodelist in(";
                                                if (opcodes != null && opcodes.Length > 0)
                                                {
                                                    foreach (DataRow odr in opcodes)
                                                    {
                                                        opencodes += "'" + odr["opentablecode"].ToString() + "',";
                                                    }
                                                    opencodes = opencodes.TrimEnd(',') + ") and btypename='" + ptypename + "' and typename='" + typename + "'";
                                                    DataRow[] mdr = dt5.Select(opencodes);
                                                    double _sum_money = Helper.StringToDouble(mdr.Sum(x => x.Field<decimal>("price")).ToString());
                                                    type_sum_money += _sum_money;
                                                    if (!xj.ContainsKey(tabletypename))
                                                    {
                                                        xj.Add(tabletypename, _sum_money);
                                                    }
                                                    else
                                                    {
                                                        xj[tabletypename] = xj[tabletypename] + _sum_money;
                                                    }
                                                    if (!type_hj.ContainsKey(tabletypename))
                                                    {
                                                        type_hj.Add(tabletypename, _sum_money);
                                                    }
                                                    else
                                                    {
                                                        type_hj[tabletypename] = type_hj[tabletypename] + _sum_money;
                                                    }
                                                    html.Append("<td class='td_boldwidth' align='center'>" + _sum_money + "</td>");
                                                }
                                                else
                                                {
                                                    if (!xj.ContainsKey(tabletypename))
                                                    {
                                                        xj.Add(tabletypename, 0);
                                                    }
                                                    else
                                                    {
                                                        xj[tabletypename] = xj[tabletypename] + 0;
                                                    }
                                                    if (!type_hj.ContainsKey(tabletypename))
                                                    {
                                                        type_hj.Add(tabletypename, 0);
                                                    }
                                                    else
                                                    {
                                                        type_hj[tabletypename] = type_hj[tabletypename] + 0;
                                                    }
                                                    html.Append("<td class='td_boldwidth' align='center'>0</td>");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //根据菜谱来
                                        if (dt5 != null && dt5.Rows.Count > 0)
                                        {
                                            foreach (string tabletypename in tabletype.Split(','))//统计各个菜谱
                                            {
                                                if (!string.IsNullOrEmpty(tabletypename))
                                                {
                                                    DataRow[] mdr = dt5.Select("MenuName='" + tabletypename + "' and btypename='" + ptypename + "' and typename='" + typename + "'");
                                                    double _sum_money = Helper.StringToDouble(mdr.Sum(x => x.Field<decimal>("price")).ToString());
                                                    type_sum_money += _sum_money;
                                                    if (!xj.ContainsKey(tabletypename))
                                                    {
                                                        xj.Add(tabletypename, _sum_money);
                                                    }
                                                    else
                                                    {
                                                        xj[tabletypename] = xj[tabletypename] + _sum_money;
                                                    }
                                                    if (!type_hj.ContainsKey(tabletypename))
                                                    {
                                                        type_hj.Add(tabletypename, _sum_money);
                                                    }
                                                    else
                                                    {
                                                        type_hj[tabletypename] = type_hj[tabletypename] + _sum_money;
                                                    }
                                                    html.Append("<td class='td_boldwidth' align='center'>" + _sum_money + "</td>");
                                                }
                                            }
                                        }
                                    }
                                    html.Append("<td class='td_boldwidth' align='center'>" + type_sum_money + "</td></tr>");
                                }

                            }
                        }
                        //小计
                        html.Append("<tr class='list_tab_tit' style='font-weight: bold;'><td class='td_boldwidth' align='left' style='border-top: 1px solid #bdbdbd;text-align:left;padding-left:20px;'>小计</td>");
                        double xj_hj = 0;
                        foreach (string key in tabletype.Split(','))
                        {
                            if (xj.ContainsKey(key))
                            {
                                xj_hj += xj[key];
                                html.Append("<td class='td_boldwidth' align='center'>" + xj[key] + "</td>");
                            }
                            else
                            {
                                html.Append("<td class='td_boldwidth' align='center'>0</td>");
                            }
                        }
                        html.Append("<td class='td_boldwidth' align='center'>" + xj_hj + "</td>");
                        html.Append("</tr>");
                    }
                    //合计
                    html.Append("<tr class='list_tab_tit' style='font-weight: bold;'><td class='td_boldwidth' align='left' style='border-top: 1px solid #bdbdbd;text-align:left;padding-left:20px;'>合计</td>");
                    double hj_hj = 0;
                    foreach (string tabletypename in tabletype.Split(','))//统计各个菜谱
                    {
                        if (type_hj.ContainsKey(tabletypename))
                        {
                            hj_hj += type_hj[tabletypename];
                            html.Append("<td class='td_boldwidth' align='center'>" + type_hj[tabletypename] + "</td>");
                        }
                        else
                        {
                            html.Append("<td class='td_boldwidth' align='center'>0</td>");
                        }
                    }
                    html.Append("<td class='td_boldwidth' align='center'>" + hj_hj + "</td>");
                    html.Append("</tr>");
                }
                html.Append("</tbody></table>");
                #endregion

                html.Append("<table style='width: 920px; height: 25px; '><tbody><tr><td style='height: 25px; border: 0px; '>&nbsp;</td></tr></tbody></table>");

                //会员卡项
                DataTable dt6 = new DataTable();
                dt6 = ds.Tables[6];
                string mempayccnames = string.Empty;
                string mempaynames = string.Empty;
                if (dt6 != null && dt6.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt6.Rows)
                    {
                        if (!mempaynames.Contains(dr["PayMethodName"].ToString()))
                        {
                            mempaynames += dr["PayMethodName"].ToString() + ",";
                        }
                        if (!mempayccnames.Contains(dr["CCname"].ToString()))
                        {
                            mempayccnames += dr["CCname"].ToString() + ",";
                        }
                    }
                }
                mempayccnames = mempayccnames.TrimEnd(',');
                mempaynames = mempaynames.TrimEnd(',');
                html.Append("<table class='List_tab' style='width: 920px; border-collapse:separate;border: 1px solid #bdbdbd;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody><tr class='list_tab_tit' style='font-weight: bold;'><td colspan = '" + (mempayccnames.Split(',').Count() + 2) + "' class='td_boldwidth' align='left' style='border-top: 1px solid #bdbdbd;text-align:left;padding-left:20px;'>会员卡项</td></tr>");
                html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                html.Append("<td class='td_boldwidth' align='center'>付款方式</td>");
                foreach (string ccname in mempayccnames.Split(','))
                {
                    if (!string.IsNullOrEmpty(ccname))
                    {
                        html.Append("<td class='td_boldwidth' align='center'>" + ccname + "</td>");
                    }
                }
                html.Append("<td class='td_boldwidth' align='center'>合计</td></tr>");
                Dictionary<string, double> memhj = new Dictionary<string, double>();
                foreach (string pyname in mempaynames.Split(','))
                {
                    if (!string.IsNullOrEmpty(pyname))
                    {
                        DataRow[] isrows = dt6.Select("PayMethodName='" + pyname + "'");
                        decimal money = isrows.Sum(x => x.Field<decimal>("payamount"));
                        if (money == 0)
                        {
                            continue;
                        }
                        html.Append("<tr class='list_tab_tit' style='font-weight: bold;'>");
                        html.Append("<td class='td_boldwidth' align='center'>" + pyname + "</td>");
                        double summoney = 0;
                        foreach (string ccname in mempayccnames.Split(','))
                        {
                            if (!string.IsNullOrEmpty(ccname))
                            {
                                DataRow[] drs = dt6.Select("PayMethodName='" + pyname + "' and CCname='" + ccname + "'");
                                if (drs != null && drs.Length > 0)
                                {
                                    summoney += Helper.StringToDouble(drs[0]["payamount"].ToString());
                                    if (!memhj.ContainsKey(ccname))
                                    {
                                        memhj.Add(ccname, Helper.StringToDouble(drs[0]["payamount"].ToString()));
                                    }
                                    else
                                    {
                                        memhj[ccname] = memhj[ccname] + Helper.StringToDouble(drs[0]["payamount"].ToString());
                                    }
                                    html.Append("<td class='td_boldwidth' align='center'>" + drs[0]["payamount"].ToString() + "</td>");
                                }
                                else
                                {
                                    html.Append("<td class='td_boldwidth' align='center'>0</td>");
                                }
                            }
                        }
                        html.Append("<td class='td_boldwidth' align='center'>" + summoney + "</td></tr>");
                    }
                }
                html.Append("<tr class='list_tab_tit' style='font-weight: bold;'><td class='td_boldwidth' align='left' style='border-top: 1px solid #bdbdbd;text-align:left;padding-left:20px;'>合计</td>");
                double mem_hj_hj = 0;
                foreach (string key in mempayccnames.Split(','))
                {
                    if (memhj.ContainsKey(key))
                    {
                        mem_hj_hj += memhj[key];
                        html.Append("<td class='td_boldwidth' align='center'>" + memhj[key] + "</td>");
                    }
                    else
                    {
                        html.Append("<td class='td_boldwidth' align='center'>0</td>");
                    }
                }
                if (dt6 != null && dt6.Rows.Count > 0)
                {
                    html.Append("<td class='td_boldwidth' align='center'>" + mem_hj_hj + "</td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");

                string json = "{";
                json += "\"data\":\"" + html.ToString() + "\"";
                json += "}";
                Pagcontext.Response.Write(json);
            }

        }

    }
}