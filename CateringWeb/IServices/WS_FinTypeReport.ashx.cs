using CommunityBuy.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_FinTypeReport 的摘要说明
    /// </summary>
    public class WS_FinTypeReport : ServiceBase
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
                        case "shiftreport"://班次统计报表
                            ShiftReport(dicPar);
                            break;
                        case "shiftbillreport"://账单明细报表
                            ShiftBillReport(dicPar);
                            break;
                        case "storefintypereport"://品项销售-财务类别
                            StoreFinTypeReport(dicPar);
                            break;
                        case "storedistypereport"://品项销售-菜品类别
                            StoreDisTypeReport(dicPar);
                            break;
                        case "couponchecklog":
                            CouponCheckLog(dicPar);
                            break;
                    }
                }
            }
        }

        private void ShiftReport(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "filters", "orders", "stocode", "PayWay" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string appordersdatetime = dicPar["appordersdatetime"].ToString();
            string apporderedatetime = dicPar["apporderedatetime"].ToString();
            string PayWay = dicPar["PayWay"].ToString();
            if ((DateTime.Parse(apporderedatetime) - DateTime.Parse(appordersdatetime)).TotalDays > 7)
            {
                string nulljson = "{";
                nulljson += "\"title\":[[{\"field\": \"err\",\"title\":\"\",\"align\":\"center\"}]],";
                nulljson += "\"data\":[{\"err\":\"日期范围不能超过7天\"}]";
                nulljson += "}";
                Pagcontext.Response.Write(nulljson);
                return;
            }
            string apporderwhere = " where b.stocode='" + stocode + "' and b.ctime>='" + appordersdatetime + "' and b.ctime<='" + apporderedatetime + "' and b.PayWay='2' and b.tstatus in ('1','5')";
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);

            DataTable dtFilter = new DataTable();
            if (filter.Length > 0 && filter != "[]")
            {
                filter = JsonHelper.JsonToFilterByString(filter, out dtFilter);
                if (!filter.Contains("StoCode") && !filter.Contains("stocode"))
                {
                    filter += GetAuthoritywhere("stocode", userid);
                }
                if (dtFilter != null)
                {
                    DataRow[] drArr = dtFilter.Select("cus<>''");
                    foreach (DataRow dr in drArr)
                    {
                        string col = dr["col"].ToString();
                        switch (col)
                        {
                            case "":
                                filter += "";
                                break;
                        }
                    }
                }
            }
            else
            {
                filter = "where 1=1" + GetAuthoritywhere("stocode", userid);
            }
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            filter = GetBusCodeWhere(dicPar, filter, "buscode");
            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            if (string.IsNullOrEmpty(order))
            {
                order = " order by ctime desc";
            }
            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));//班次号
            dt_Result.Columns.Add("field1_url", typeof(string));//班次号
            dt_Result.Columns.Add("field2", typeof(string));//门店
            dt_Result.Columns.Add("field3", typeof(string));//部门
            dt_Result.Columns.Add("field4", typeof(string));//收银员
            dt_Result.Columns.Add("field5", typeof(string));//开班时间
            dt_Result.Columns.Add("field6", typeof(string));//接班时间
            dt_Result.Columns.Add("field7", typeof(string));//收银机Mac
            dt_Result.Columns.Add("field8", typeof(string));//开票金额
            dt_Result.Columns.Add("field10", typeof(string));//账单数
            dt_Result.Columns.Add("field11", typeof(string));//班次金额
            dt_Result.Columns.Add("field11_url", typeof(string));//班次金额跳转链接
            dt_Result.Columns.Add("field12", typeof(string));//折扣金额
            dt_Result.Columns.Add("field13", typeof(string));//抹零金额
            dt_Result.Columns.Add("field14", typeof(string));//营收项小计
            dt_Result.Columns.Add("field15", typeof(string));//销售项小计
            dt_Result.Columns.Add("field16", typeof(string));//会员收款小计
            dt_Result.Columns.Add("field17", typeof(string));//会员卡小计
            dt_Result.Columns.Add("field22", typeof(string));//账单是否持平 0为持平 1为不平
            dt_Result.Columns.Add("field9", typeof(string));//赠送金额
            dt_Result.Columns.Add("field9_url", typeof(string));//赠送金额跳转路径
            dt_Result.Columns.Add("field18", typeof(string));//贵宾卡
            dt_Result.Columns.Add("field18_url", typeof(string));//贵宾卡跳转路径
            dt_Result.Columns.Add("field19", typeof(string));//工本费
            dt_Result.Columns.Add("field19_url", typeof(string));//工本费跳转路径
            dt_Result.Columns.Add("field20", typeof(string));//员工卡
            dt_Result.Columns.Add("field20_url", typeof(string));//员工卡跳转路径
            dt_Result.Columns.Add("field21", typeof(string));//美食卡
            dt_Result.Columns.Add("field21_url", typeof(string));//美食卡跳转路径

            DataSet ds;
            //if(stocode!="08")
            //{
            ds = bll.ShiftReport(filter, order, apporderwhere, stocode + "_" + DateTime.Parse(appordersdatetime).ToString("yyyyMMdd") + "_" + DateTime.Parse(apporderedatetime).ToString("yyyyMMdd"), appordersdatetime, apporderedatetime);
            //}
            //else
            //{
            //    ds = bll.FanHuaShiftReport(filter, order);
            //}
            string StoName = string.Empty;
            ArrayList arrayList = new ArrayList();
            List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题
            List<LayUItableHelper> Title_list2 = new List<LayUItableHelper>();//第二行标题
            if (ds != null && ds.Tables.Count == 9)
            {
                #region 组织表头，注意顺序
                DataTable dt_shifthistory = ds.Tables[0];
                DataTable dt_bill = ds.Tables[1];
                DataTable dt_BillPay = ds.Tables[2];
                DataTable dt_MemcarPay = ds.Tables[5];

                DataTable group_table = ds.Tables[6];//优惠券使用
                DataTable dt_OrderDis = ds.Tables[4];//订单菜品信息表
                DataTable dt_CouponBath = ds.Tables[7];//批量验券
                DataTable zs_table = dt_OrderDis.Clone();
                DataRow[] _zsdrs = dt_OrderDis.Select("DiscountType='6'");


                foreach (DataRow dr in _zsdrs)
                {
                    zs_table.ImportRow(dr);
                }
                if (dt_shifthistory != null)
                {
                    DataTable dtStore = GetCacheToStore(userid);
                    if (dtStore != null && dtStore.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_shifthistory.Rows)
                        {
                            string drstocode = dr["stocode"].ToString();
                            if (dtStore.Select("stocode='" + drstocode + "'").Length > 0)
                            {
                                DataRow dr_sto = dtStore.Select("stocode='" + drstocode + "'")[0];
                                if (dr_sto != null)
                                {
                                    dr["stoname"] = dr_sto["cname"].ToString();
                                    StoName = dr_sto["cname"].ToString();
                                }
                            }
                        }
                    }
                    DataTable dtSetDepartment = GetCacheToDepartment(userid);
                    if (dtStore != null && dtStore.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_shifthistory.Rows)
                        {
                            string depcode = dr["depcode"].ToString();
                            if (dtSetDepartment.Select("dcode='" + depcode + "'").Length > 0)
                            {
                                DataRow dr_sto = dtSetDepartment.Select("dcode='" + depcode + "'")[0];
                                if (dr_sto != null)
                                {
                                    dr["depname"] = dr_sto["dname"].ToString();
                                }
                            }
                        }
                    }
                    dt_shifthistory.AcceptChanges();
                }

                //固定title
                Title_list1.Add(new LayUItableHelper { Fixed = "left", Sort = true, Field = "field1", Title = "班次号", Align = "center", HeadTemplet = "field1_url", TotalRowText = "合计", Rowspan = 2, Style = "cursor: pointer;color: #0000FF" });
                Title_list1.Add(new LayUItableHelper { Fixed = "left", Sort = false, Field = "field4", Title = "收银员", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field2", Title = "门店", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field3", Title = "部门", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field5", Title = "开班时间", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field6", Title = "结班时间", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field7", Title = "收银机Mac", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field8", Title = "开票金额", Align = "center", Rowspan = 2, TotalRow = true });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field9", Title = "赠送金额", Align = "center", Rowspan = 2, TotalRow = true, HeadTemplet = "field9_url", Style = "cursor: pointer;color: #0000FF" });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field10", Title = "账单数", Align = "center", Rowspan = 2 });

                //营业收入项目动态title
                string paymethodcodes = ",";
                int width = 130;
                bool isygk = false;//是否存在员工卡充值账单
                bool ismsk = false;//是否存在美食卡充值账单
                bool isgbk = false;//是否存在贵宾卡充值账单
                bool iscost = false;//工本费
                string MemBillCode = string.Empty;
                if (dt_BillPay != null && dt_BillPay.Rows.Count > 0)
                {
                    int colspan = 0;
                    //组织第二行标题-支付方式
                    foreach (DataRow dr in dt_BillPay.Rows)
                    {
                        int memorder = 0;
                        if (dr["billtype"].ToString() == "2")
                        {
                            MemBillCode += "'" + dr["BillCode"].ToString() + "',";//记录一下该笔账单来自于会员卡,方便后面会员卡统计
                            memorder = 1;
                        }
                        if (memorder == 1)
                        {
                            continue;//该笔支付属于会员卡项的，直接跳过
                        }
                        string field = dr["PayMethodCode"].ToString();//后期组织数据时，datatable的列名需要对应
                        if (!paymethodcodes.Contains("," + field + ","))
                        {
                            dt_Result.Columns.Add("ys" + field, typeof(string));//动态表头
                            Title_list2.Add(new LayUItableHelper { Sort = false, Field = "ys" + field, Title = dr["PayMethodName"].ToString(), Align = "center", TotalRow = true, HeadTemplet = "ys" + field + "_url", Style = "cursor: pointer;color: #0000FF", width = width });
                            dt_Result.Columns.Add("ys" + field + "_url", typeof(string));//点击后跳转路径
                            ++colspan;
                            paymethodcodes += field + ",";
                        }
                    }

                    if (stocode == "08")
                    {
                        if (zs_table != null && zs_table.Rows.Count > 0)//签送
                        {
                            ++colspan;
                            dt_Result.Columns.Add("qs", typeof(string));//动态表头
                            Title_list2.Add(new LayUItableHelper { Sort = false, Field = "qs", Title = "签送", Align = "center", TotalRow = true, HeadTemplet = "qs_url", Style = "cursor: pointer;color: #0000FF", width = width });
                            dt_Result.Columns.Add("qs_url", typeof(string));//点击后跳转路径
                        }
                    }
                    if (group_table != null && group_table.Rows.Count > 0)
                    {
                        ++colspan;
                        dt_Result.Columns.Add("group", typeof(string));//动态表头
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "group", Title = "优惠券", Align = "center", TotalRow = true, width = width });
                        //decimal _sumgroup = 0;//虚增金额
                        //DataRow[] _drs = group_table.Select();
                        //if (_drs != null && _drs.Length > 0)
                        //{
                        //    _sumgroup += _drs.Sum(x => x.Field<decimal>("VIMoney"));
                        //}
                        //if (_sumgroup != 0)
                        //{
                        //    ++colspan;
                        //    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "VIMoney", Title = "虚增金额", Align = "center", TotalRow = true, width = width });
                        //    dt_Result.Columns.Add("VIMoney", typeof(string));//动态表头
                        //}
                    }


                    decimal _sumfield12 = 0;//折扣金额
                    decimal _sumfield13 = 0;//抹零金额
                    if (dt_bill != null && dt_bill.Rows.Count > 0)
                    {
                        DataRow[] _drs = dt_bill.Select();
                        if (_drs != null && _drs.Length > 0)
                        {
                            _sumfield12 += _drs.Sum(x => x.Field<decimal>("DiscountMoney"));
                            _sumfield13 += _drs.Sum(x => x.Field<decimal>("ZeroCutMoney"));
                        }
                    }

                    if (_sumfield12 != 0)
                    {
                        ++colspan;
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field12", Title = "折扣金额", Align = "center", TotalRow = true, width = width });
                    }
                    if (_sumfield13 != 0)
                    {
                        ++colspan;
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field13", Title = "抹零金额", Align = "center", TotalRow = true, width = width });
                    }
                    Title_list1.Add(new LayUItableHelper { Sort = false, Field = "sr", Title = "营业收入", Align = "center", Colspan = colspan + 1, Rowspan = 1 });
                }
                if (dt_CouponBath != null && dt_CouponBath.Rows.Count > 0)//如果存在券回收的记录，则加一个券回收的收款项
                {
                    dt_Result.Columns.Add("field23", typeof(string));//动态表头
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field23", Title = "券回收", Align = "center", TotalRow = true, HeadTemplet = "field23_url", Style = "cursor: pointer;color: #0000FF", width = width });
                    dt_Result.Columns.Add("field23_url", typeof(string));//点击后跳转路径
                    LayUItableHelper title1 = Title_list1.SingleOrDefault(a => a.Title == "营业收入");
                    title1.Colspan += 1;
                }
                if ((dt_BillPay != null && dt_BillPay.Rows.Count > 0) || (dt_CouponBath != null && dt_CouponBath.Rows.Count > 0))
                {
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field14", Title = "小计", Align = "center", TotalRow = true, width = width });
                }
                //消费项目动态title
                string typecodes = ",";
                DataTable dt_OrderDis1;
                if (stocode == "08")
                {
                    dt_OrderDis1 = ds.Tables[4];
                }
                else
                {
                    dt_OrderDis1 = ds.Tables[4].Clone();
                    DataRow[] drs = ds.Tables[4].Select("DiscountType<>'6'");//不包含赠送的
                    foreach (DataRow dr in drs)
                    {
                        dt_OrderDis1.ImportRow(dr);
                    }
                }

                if (dt_OrderDis != null && dt_OrderDis.Rows.Count > 0)
                {
                    int colspan = 0;
                    //组织第二行标题-支付方式
                    foreach (DataRow dr in dt_OrderDis.Rows)
                    {
                        string field = dr["FinCode"].ToString();//后期组织数据时，datatable的列名需要对应
                        if(string.IsNullOrEmpty(field))
                        {
                            field = "(空)";
                        }
                        if (!typecodes.Contains("," + field + ","))
                        {
                            dt_Result.Columns.Add("fn" + field, typeof(string));//动态表头
                            Title_list2.Add(new LayUItableHelper { Sort = false, Field = "fn" + field, Title = dr["FinTypeName"].ToString(), Align = "center", TotalRow = true, HeadTemplet = "fn" + field + "_url", Style = "cursor: pointer;color: #0000FF", width = width });
                            dt_Result.Columns.Add("fn" + field + "_url", typeof(string));//点击后跳转路径
                            ++colspan;
                            typecodes += field + ",";
                        }
                        else
                        {
                            if(!typecodes.Contains("(空)"))
                            {
                                dt_Result.Columns.Add("fn(空)", typeof(string));//动态表头
                                Title_list2.Add(new LayUItableHelper { Sort = false, Field = "fn(空)", Title = "(空)", Align = "center", TotalRow = true, HeadTemplet = "fn(空)_url", Style = "cursor: pointer;color: #0000FF", width = width });
                                dt_Result.Columns.Add("fn(空)_url", typeof(string));//点击后跳转路径
                                ++colspan;
                                typecodes += "(空),";
                            }
                        }
                    }
                    Title_list1.Add(new LayUItableHelper { Sort = false, Field = "xf", Title = "消费项目", Align = "center", Colspan = colspan + 1, Rowspan = 1 });
                }
                //批量消券的做特殊处理，如果存在消券记录，但是没有影院票房财务类别的，强制加一个
                if (dt_CouponBath != null && dt_CouponBath.Rows.Count > 0)
                {
                    if (dt_OrderDis != null && dt_OrderDis.Rows.Count > 0)
                    {
                        DataRow[] Coupons = dt_OrderDis.Select("FinTypeName='影院票房'");//判断上面的是否已添加了影院票房的销售项目，如果没有则添加一个
                        if (Coupons == null || Coupons.Length <= 0)
                        {
                            dt_Result.Columns.Add("fnP49976", typeof(string));//动态表头
                            Title_list2.Add(new LayUItableHelper { Sort = false, Field = "fnP49976", Title = "影院票房", Align = "center", TotalRow = true, HeadTemplet = "fnP49976_url", Style = "cursor: pointer;color: #0000FF", width = width });
                            dt_Result.Columns.Add("fnP49976_url", typeof(string));//点击后跳转路径
                            typecodes += "P49976,";
                            LayUItableHelper title1 = Title_list1.SingleOrDefault(a => a.Title == "消费项目");
                            title1.Colspan += 1;
                        }
                    }
                    else
                    {
                        dt_Result.Columns.Add("fnP49976", typeof(string));//动态表头
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "fnP49976", Title = "影院票房", Align = "center", TotalRow = true, HeadTemplet = "fnP49976_url", Style = "cursor: pointer;color: #0000FF", width = width });
                        dt_Result.Columns.Add("fnP49976_url", typeof(string));//点击后跳转路径
                        typecodes += "P49976,";
                        Title_list1.Add(new LayUItableHelper { Sort = false, Field = "xf", Title = "消费项目", Align = "center", Colspan = 1, Rowspan = 1 });
                    }
                }

                if ((dt_OrderDis != null && dt_OrderDis.Rows.Count > 0) || (dt_CouponBath != null && dt_CouponBath.Rows.Count > 0))
                {
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field15", Title = "小计", Align = "center", TotalRow = true, width = width });
                }

                //会员卡项目动态title
                string mempaymethodcodes = ",";
                if (dt_BillPay != null && dt_BillPay.Rows.Count > 0 && !string.IsNullOrEmpty(MemBillCode))
                {
                    int colspan = 0;
                    //组织第二行标题-支付方式
                    DataRow[] memrows = dt_BillPay.Select("BillCode in(" + MemBillCode + ") and billtype='2'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        foreach (DataRow dr in memrows)
                        {
                            string field = dr["PayMethodCode"].ToString();//后期组织数据时，datatable的列名需要对应
                            if (!mempaymethodcodes.Contains("," + field + ","))
                            {
                                dt_Result.Columns.Add("hyk" + field, typeof(string));//动态表头
                                Title_list2.Add(new LayUItableHelper { Sort = false, Field = "hyk" + field, Title = dr["PayMethodName"].ToString(), Align = "center", TotalRow = true, HeadTemplet = "hyk" + field + "_url", Style = "cursor: pointer;color: #0000FF", width = width });
                                dt_Result.Columns.Add("hyk" + field + "_url", typeof(string));//点击后跳转路径
                                ++colspan;
                                mempaymethodcodes += field + ",";
                            }
                        }
                        Title_list1.Add(new LayUItableHelper { Sort = false, Field = "hyksk", Title = "会员卡收款项目", Align = "center", Colspan = colspan + 1, Rowspan = 1 });
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field16", Title = "小计", Align = "center", TotalRow = true, width = width });
                    }
                }

                //会员卡动态title
                if (dt_MemcarPay != null && dt_MemcarPay.Rows.Count > 0)
                {
                    int colspan = 0;

                    DataRow[] memrows = dt_MemcarPay.Select("cardtype<>'8' and cardtype<>'food'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field18", Title = "贵宾卡", Align = "center", TotalRow = true, HeadTemplet = "field18_url", Style = "cursor: pointer;color: #0000FF", width = width });
                        colspan += 1;
                        isgbk = true;
                    }

                    decimal cost = dt_MemcarPay.Select().Sum(x => x.Field<decimal>("cardcost"));
                    if (cost != 0)
                    {
                        colspan += 1;
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field19", Title = "工本费", Align = "center", TotalRow = true, HeadTemplet = "field19_url", Style = "cursor: pointer;color: #0000FF", width = width });
                        iscost = true;
                    }

                    memrows = dt_MemcarPay.Select("cardtype='8'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field20", Title = "员工卡", Align = "center", TotalRow = true, HeadTemplet = "field20_url", Style = "cursor: pointer;color: #0000FF", width = width });
                        colspan += 1;
                        isygk = true;
                    }

                    memrows = dt_MemcarPay.Select("cardtype='food'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field21", Title = "美食卡", Align = "center", TotalRow = true, HeadTemplet = "field21_url", Style = "cursor: pointer;color: #0000FF", width = width });
                        colspan += 1;
                        ismsk = true;
                    }

                    Title_list1.Add(new LayUItableHelper { Sort = false, Field = "hyksk", Title = "会员项目", Align = "center", Colspan = colspan + 1, Rowspan = 1 });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field17", Title = "小计", Align = "center", TotalRow = true, width = 150 });
                }
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field11", Title = "班次金额", Align = "center", Rowspan = 2, Style = "padding:0 !important;", htmltemplet = "#T_field11", width = width, TotalRow = true });//最后一列为班次金额

                arrayList.Add(Title_list1);//第一行表头
                arrayList.Add(Title_list2);//第二行表头
                dt_Result.AcceptChanges();
                #endregion

                #region 组织数据
                DataTable TB_BillInvoice = new DataTable();
                TB_BillInvoice = ds.Tables[8];
                DataRow[] sdrs = dt_shifthistory.Select();
                if (PayWay == "1")
                {
                    sdrs = dt_shifthistory.Select("ccname<>'小程序'");
                }
                foreach (DataRow dr in sdrs)//循环班次组装数据
                {
                    decimal _field14 = 0;//营业项目小计
                    decimal _field15 = 0;//销售项目小计
                    decimal _field16 = 0;//会员收款项目小计
                    decimal _field17 = 0;//会员项目小计
                    decimal _field11 = 0;//班次金额

                    DataRow newdr = dt_Result.NewRow();
                    newdr["field1"] = dr["ShiftCode"].ToString();
                    newdr["field1_url"] = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString().Trim() + "&EndTime=" + dr["etime"].ToString().Trim() + "&StoCode=" + dr["stocode"].ToString().Trim() + "&ShiftCode=" + dr["ShiftCode"].ToString().Trim() + "&DepCode=" + dr["depcode"].ToString().Trim() + "&StoName=" + dr["stoname"].ToString() + "&ecode=" + dr["ccode"].ToString() + "&PayWay=" + dr["PayWay"].ToString();//模拟
                    newdr["field2"] = dr["stoname"].ToString();
                    newdr["field3"] = dr["depname"].ToString();
                    newdr["field4"] = dr["ccname"].ToString();
                    newdr["field5"] = dr["ctime"].ToString();
                    newdr["field6"] = dr["etime"].ToString();
                    newdr["field7"] = dr["comcode"].ToString();

                    //处理开票金额
                    if(TB_BillInvoice!=null)
                    {
                        if(TB_BillInvoice.Select("ShiftCode='"+ dr["ShiftCode"].ToString().Trim() + "'").Length>0)
                        {
                            double inmoney = Helper.StringToDouble(TB_BillInvoice.Select("ShiftCode='" + dr["ShiftCode"].ToString().Trim() + "'")[0]["InMoney"].ToString());
                            if(inmoney>0)
                            {
                                newdr["field8"] = inmoney;
                            }
                            else
                            {
                                newdr["field8"] = "-";
                            }
                        }
                        else
                        {
                            newdr["field8"] = "-";
                        }
                    }
                    else
                    {
                        newdr["field8"] = "-";
                    }


                    //处理赠送金额
                    if (zs_table != null && zs_table.Rows.Count > 0)
                    {
                        decimal zssummoney = 0;
                        DataRow[] zsdrs = zs_table.Select("ShiftCode='" + dr["ShiftCode"] + "'");//找到当前班次的账单数
                        if (zsdrs != null && zsdrs.Length > 0)
                        {
                            zssummoney = zsdrs.Sum(x => x.Field<decimal>("TotalMoney"));
                        }
                        if (stocode == "08")
                        {
                            newdr["qs"] = zssummoney == 0 ? "-" : string.Format("{0:N}", zssummoney);
                            if (newdr["qs"].ToString() != "-")
                            {
                                newdr["qs_url"] = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&PayType=99&BillType=1&DepCode=" + dr["depcode"].ToString() + "&StoName=" + dr["stoname"].ToString() + "&PayWay=1";
                            }
                            else
                            {
                                newdr["qs_url"] = string.Empty;
                            }
                            _field14 += zssummoney;
                        }
                        else
                        {
                            newdr["field9"] = zssummoney == 0 ? "-" : string.Format("{0:N}", zssummoney);
                            if (newdr["field9"].ToString() != "-")
                            {
                                newdr["field9_url"] = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&zsje=1&StoName=" + dr["stoname"].ToString() + "&PayWay=1";
                            }
                            else
                            {
                                newdr["field9_url"] = string.Empty;
                            }
                        }
                    }

                    //处理账单数

                    if (dt_bill != null && dt_bill.Rows.Count > 0)
                    {
                        int billcount = dt_bill.Select("ShiftCode='" + dr["ShiftCode"] + "'").Length;//找到当前班次的账单数
                        newdr["field10"] = billcount == 0 ? '-' : billcount;
                    }
                    else
                    {
                        newdr["field10"] = "-";
                    }
                    //营业收入项目
                    if (dt_BillPay != null && dt_BillPay.Rows.Count > 0)
                    {
                        string[] pays = paymethodcodes.Split(',');
                        if (pays != null && pays.Length > 0)
                        {
                            foreach (string p in pays)
                            {
                                if (!string.IsNullOrEmpty(p))
                                {
                                    DataRow[] _drs = dt_BillPay.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "' and billtype<>'2' and PayMethodCode='" + p + "'");
                                    if (_drs != null && _drs.Length > 0)
                                    {
                                        decimal sumpaymoney = _drs.Sum(x => x.Field<decimal>("PayMoney"));
                                        _field14 += sumpaymoney;
                                        newdr["ys" + p] = sumpaymoney == 0 ? "-" : string.Format("{0:N}", sumpaymoney);
                                        if (newdr["ys" + p].ToString() != "-")
                                        {
                                            newdr["ys" + p + "_url"] = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&PayType=" + p + "&BillType=1&DepCode=" + dr["depcode"].ToString() + "&StoName=" + dr["stoname"].ToString() + "&PayWay=" + _drs[0]["PayWay"].ToString();
                                        }
                                        else
                                        {
                                            newdr["ys" + p + "_url"] = string.Empty;
                                        }
                                    }
                                }
                            }
                        }
                        string[] payfields = paymethodcodes.Split(',');
                        foreach (string p in payfields)
                        {
                            if (!string.IsNullOrEmpty(p))
                            {
                                string value = newdr["ys" + p].ToString();
                                if (string.IsNullOrEmpty(value))
                                {
                                    newdr["ys" + p] = "-";
                                    newdr["ys" + p + "_url"] = string.Empty;
                                }
                            }
                        }
                    }
                    if (group_table != null && group_table.Rows.Count > 0)//优惠券使用金额
                    {
                        DataRow[] _drs;
                        if (dr["PayWay"].ToString() == "1")
                        {
                            _drs = group_table.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "'");
                        }
                        else
                        {
                            _drs = group_table.Select("PayWay='2'");
                        }
                        decimal group_summoney = 0;
                        if (_drs != null && _drs.Length > 0)
                        {
                            //group_summoney = _drs.Sum(x => x.Field<decimal>("CouponMoney"));
                            group_summoney = _drs.Sum(x => x.Field<decimal>("RealPay"));
                            _field14 += group_summoney;
                            newdr["group"] = group_summoney == 0 ? "-" : string.Format("{0:N}", group_summoney);
                        }
                        else
                        {
                            newdr["group"] = "-";
                        }
                        //decimal _sumgroup = 0;//虚增金额
                        //if (_drs != null && _drs.Length > 0)
                        //{
                        //    _sumgroup += _drs.Sum(x => x.Field<decimal>("VIMoney"));
                        //    _field14 += _sumgroup;
                        //}
                        //if (_sumgroup != 0)
                        //{
                        //    newdr["VIMoney"] = _sumgroup == 0 ? "-" : string.Format("{0:N}", _sumgroup);
                        //}
                    }

                    //统计抹零和折扣金额
                    decimal _field12 = 0;//折扣金额
                    decimal _field13 = 0;//抹零金额
                    if (dt_bill != null && dt_bill.Rows.Count > 0)
                    {
                        DataRow[] _drs = dt_bill.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "'");//找到该班次的所有账单
                        if (_drs != null && _drs.Length > 0)
                        {
                            foreach (DataRow dr_bill in _drs)
                            {
                                _field12 += Helper.StringToDecimal(dr_bill["DiscountMoney"].ToString());
                                _field13 += Helper.StringToDecimal(dr_bill["ZeroCutMoney"].ToString());
                            }
                        }
                    }
                    if (_field12 != 0)
                    {
                        newdr["field12"] = _field12 == 0 ? "-" : string.Format("{0:N}", _field12);
                    }
                    if (_field13 != 0)
                    {
                        newdr["field13"] = _field13 == 0 ? "-" : string.Format("{0:N}", _field13);
                    }
                    _field14 += _field12;
                    _field14 += _field13;

                    //统计批量消券
                    if (dt_CouponBath != null && dt_CouponBath.Rows.Count > 0)
                    {
                        DataRow[] _drs = dt_CouponBath.Select("ShiftId='" + dr["ShiftCode"].ToString() + "'");
                        decimal group_summoney = 0;
                        if (_drs != null && _drs.Length > 0)
                        {
                            group_summoney = _drs.Sum(x => x.Field<decimal>("SingleMoney"));
                            _field14 += group_summoney;
                            newdr["field23"] = group_summoney == 0 ? "-" : string.Format("{0:N}", group_summoney);
                            newdr["field23_url"] = group_summoney == 0 ? "" : "./plxqjl.html?StoCode=" + stocode + "&StoName=" + StoName + "&ShiftCode=" + dr["ShiftCode"].ToString();//跳转到消券记录列表
                        }
                        else
                        {
                            newdr["field23"] = "-";
                            newdr["field23_url"] = string.Empty;
                        }
                    }

                    if (_field14 == 0)//营业收入小计
                    {
                        newdr["field14"] = "-";
                    }
                    else
                    {
                        newdr["field14"] = string.Format("{0:N}", _field14);
                    }

                    bool isnull = false;
                    //销售类别数据组织
                    if (dt_OrderDis1 != null && dt_OrderDis1.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(typecodes))
                        {
                            string[] fcodes = typecodes.Split(',');
                            foreach (string code in fcodes)
                            {
                                if (!string.IsNullOrEmpty(code) && code!= "(空)")
                                {
                                    decimal summoney = 0;
                                    DataRow[] drfindata = dt_OrderDis1.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "' and FinCode='" + code + "'");
                                    if (drfindata != null && drfindata.Length > 0)
                                    {
                                        summoney = drfindata.Sum(x => x.Field<decimal>("TotalMoney"));
                                        _field15 += summoney;
                                    }
                                    newdr["fn" + code] = summoney == 0 ? "-" : string.Format("{0:N}", summoney);//动态绑定字段值
                                    if (newdr["fn" + code].ToString() != "-")
                                    {
                                        newdr["fn" + code + "_url"] = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&DepCode=" + dr["depcode"].ToString() + "&FinCode=" + code + "&StoName=" + dr["stoname"].ToString() + "&BillType=1&PayWay=" + drfindata[0]["PayWay"].ToString();
                                    }
                                    else
                                    {
                                        newdr["fn" + code + "_url"] = string.Empty;
                                    }
                                }
                                else
                                {
                                    if(!isnull)
                                    {
                                        decimal summoney = 0;
                                        DataRow[] drfindata = dt_OrderDis1.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "' and FinCode=''");
                                        if (drfindata != null && drfindata.Length > 0)
                                        {
                                            summoney = drfindata.Sum(x => x.Field<decimal>("TotalMoney"));
                                            _field15 += summoney;
                                        }
                                        newdr["fn(空)"] = summoney == 0 ? "-" : string.Format("{0:N}", summoney);//动态绑定字段值
                                        if (newdr["fn(空)"].ToString() != "-")
                                        {
                                            newdr["fn(空)_url"] = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&DepCode=" + dr["depcode"].ToString() + "&FinCode=&StoName=" + dr["stoname"].ToString() + "&BillType=1&PayWay=" + drfindata[0]["PayWay"].ToString();
                                        }
                                        else
                                        {
                                            newdr["fn(空)_url"] = string.Empty;
                                        }
                                        isnull = true;
                                    }
                                   
                                }
                            }
                        }
                        newdr["field15"] = _field15 == 0 ? "-" : string.Format("{0:N}", _field15);//销售项小计

                        string[] typecodesArray = typecodes.Split(',');//该班次的销售类别没数据，则将所有该班次的销售类别金额设置为-
                        foreach (string p in typecodesArray)
                        {
                            if (!string.IsNullOrEmpty(p))
                            {
                                string value = newdr["fn" + p].ToString();
                                if (string.IsNullOrEmpty(value))
                                {
                                    newdr["fn" + p] = "-";
                                    newdr["fn" + p + "_url"] = string.Empty;
                                }
                            }
                        }
                    }

                    //统计批量消券
                    if (dt_CouponBath != null && dt_CouponBath.Rows.Count > 0)
                    {
                        DataRow[] _drs = dt_CouponBath.Select("ShiftId='" + dr["ShiftCode"].ToString() + "'");//获取当前班次的批量验券记录
                        decimal group_summoney = 0;
                        if (_drs != null && _drs.Length > 0)
                        {
                            group_summoney = _drs.Sum(x => x.Field<decimal>("SingleMoney"));
                            _field15 += group_summoney;
                            DataRow[] drfindata = dt_OrderDis1.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "' and FinCode='P49976'");//判断是否已经存在影院票房的销售项
                            if (drfindata == null || drfindata.Length <= 0)
                            {
                                newdr["fnP49976"] = group_summoney == 0 ? "-" : string.Format("{0:N}", group_summoney);
                                newdr["fnP49976_url"] = group_summoney == 0 ? string.Empty : "./plxqjl.html?StoCode=" + stocode + "&StoName=" + StoName + "&ShiftCode=" + dr["ShiftCode"].ToString() + "";//跳转路径
                            }
                            else
                            {
                                //已经存在了影院票房的销售项，则在原始影院票房基础上加上该班次的批量验券总金额
                                string fin1_value = newdr["fnP49976"].ToString();
                                if (fin1_value == "-")
                                {
                                    newdr["fnP49976"] = group_summoney;
                                    newdr["fnP49976_url"] = group_summoney == 0 ? string.Empty : "";//跳转路径
                                }
                                else
                                {
                                    decimal v = Helper.StringToDecimal(fin1_value);
                                    v = v + group_summoney;
                                    newdr["fnP49976"] = string.Format("{0:N}", v);
                                }
                            }
                        }
                        else
                        {
                            newdr["fnP49976"] = "-";
                            newdr["fnP49976_url"] = string.Empty;
                        }
                    }

                    //会员卡收款项
                    if (dt_BillPay != null && dt_BillPay.Rows.Count > 0 && !string.IsNullOrEmpty(MemBillCode))
                    {
                        if (!string.IsNullOrEmpty(mempaymethodcodes))
                        {
                            string[] codes = mempaymethodcodes.Split(',');
                            if (codes != null && codes.Length > 0)
                            {
                                foreach (string code in codes)
                                {
                                    if (!string.IsNullOrEmpty(code))
                                    {
                                        if (dt_MemcarPay.Select("paystatus='2'").Length > 0)
                                        {
                                            decimal summoney = 0;
                                            DataRow[] memdrs = dt_BillPay.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "' and BillCode in(" + MemBillCode + ") and billtype='2' and PayMethodCode='" + code + "'");//找到该班次会员卡的支付信息
                                            if (memdrs != null && memdrs.Length > 0)
                                            {
                                                summoney = memdrs.Sum(x => x.Field<decimal>("PayMoney"));
                                                _field16 += summoney;
                                            }
                                            newdr["hyk" + code] = summoney == 0 ? "-" : string.Format("{0:N}", summoney);//动态绑定字段值
                                            if (newdr["hyk" + code].ToString() != "-")
                                            {
                                                newdr["hyk" + code + "_url"] = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&PayType=" + code + "&BillType=2&DepCode=" + dr["depcode"].ToString() + "&StoName=" + dr["stoname"].ToString();
                                            }
                                            else
                                            {
                                                newdr["hyk" + code + "_url"] = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            newdr["hyk" + code] = "-";//动态绑定字段值
                                            newdr["hyk" + code + "_url"] = string.Empty;
                                        }
                                    }
                                }
                            }
                        }
                        string[] mempaymethodcodesArray = mempaymethodcodes.Split(',');
                        foreach (string p in mempaymethodcodesArray)
                        {
                            if (!string.IsNullOrEmpty(p))
                            {
                                string value = newdr["hyk" + p].ToString();
                                if (string.IsNullOrEmpty(value))
                                {
                                    newdr["hyk" + p] = "-";
                                    newdr["hyk" + p + "_url"] = string.Empty;
                                }
                            }
                        }
                    }

                    if (_field16 == 0)//营业收入小计
                    {
                        newdr["field16"] = "-";
                    }
                    else
                    {
                        newdr["field16"] = string.Format("{0:N}", _field16);
                    }

                    //会员项
                    if (dt_MemcarPay != null && dt_MemcarPay.Rows.Count > 0)
                    {
                        decimal cardcost = 0;
                        cardcost = dt_MemcarPay.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "'").Sum(x => x.Field<decimal>("cardcost"));
                        //贵宾卡
                        if (isgbk)//贵宾卡
                        {
                            DataRow[] _gbk = dt_MemcarPay.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "' and cardtype<>'8' and cardtype<>'food'");//贵宾卡项
                            if (_gbk != null && _gbk.Length > 0)
                            {
                                decimal regamount = _gbk.Sum(x => x.Field<decimal>("regamount"));
                                _field17 += regamount;
                                newdr["field18"] = regamount == 0 ? "-" : string.Format("{0:N}", regamount);
                                string field18_url = string.Empty;
                                if (regamount != 0)
                                {
                                    field18_url = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&MemType=1&DepCode=" + dr["depcode"].ToString() + "&StoName=" + dr["stoname"].ToString() + "&BillType=2";
                                }
                                newdr["field18_url"] = field18_url;
                            }
                            else
                            {
                                newdr["field18"] = "-";
                                newdr["field18_url"] = "";
                            }
                        }
                        //工本费
                        if (iscost)
                        {
                            _field17 += cardcost;
                            newdr["field19"] = cardcost == 0 ? "-" : string.Format("{0:N}", cardcost);
                            newdr["field19_url"] = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&MemType=1&DepCode=" + dr["depcode"].ToString() + "&StoName=" + dr["stoname"].ToString() + "&BillType=2";
                        }
                        //员工卡
                        if (isygk)
                        {
                            DataRow[] _ygk = dt_MemcarPay.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "' and cardtype='8'");//贵宾卡项
                            if (_ygk != null && _ygk.Length > 0)
                            {
                                decimal regamount = _ygk.Sum(x => x.Field<decimal>("regamount"));
                                _field17 += regamount;
                                newdr["field20"] = regamount == 0 ? "-" : string.Format("{0:N}", regamount);
                                string field20_url = string.Empty;
                                if (regamount != 0)
                                {
                                    field20_url = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&MemType=8&DepCode=" + dr["depcode"].ToString() + "&StoName=" + dr["stoname"].ToString() + "&BillType=2";
                                }
                                newdr["field20_url"] = field20_url;
                            }
                            else
                            {
                                newdr["field20"] = "-";
                                newdr["field20_url"] = string.Empty;
                            }
                        }
                        //美食卡
                        if (ismsk)
                        {
                            DataRow[] _ygk = dt_MemcarPay.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "' and cardtype='food'");//贵宾卡项
                            if (_ygk != null && _ygk.Length > 0)
                            {
                                decimal regamount = _ygk.Sum(x => x.Field<decimal>("regamount"));
                                _field17 += regamount;
                                newdr["field21"] = regamount == 0 ? "-" : string.Format("{0:N}", regamount);
                                string field21_url = string.Empty;
                                if (regamount != 0)
                                {
                                    field21_url = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&MemType=food&DepCode=" + dr["depcode"].ToString() + "&StoName=" + dr["stoname"].ToString() + "&BillType=2";
                                }
                                newdr["field21_url"] = field21_url;
                            }
                            else
                            {
                                newdr["field21"] = "-";
                                newdr["field" +
                                    "21_url"] = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        if (isgbk)//贵宾卡
                        {
                            newdr["field18"] = "-";
                            newdr["field18_url"] = string.Empty;
                        }
                        if (iscost)
                        {
                            newdr["field19"] = "-";
                            newdr["field19_url"] = string.Empty;
                        }
                        if (isygk)
                        {
                            newdr["field20"] = "-";
                            newdr["field20_url"] = string.Empty;
                        }
                        if (ismsk)
                        {
                            newdr["field21"] = "-";
                            newdr["field21_url"] = string.Empty;
                        }
                    }

                    if (_field17 == 0)
                    {
                        newdr["field17"] = "-";
                    }
                    else
                    {
                        newdr["field17"] = string.Format("{0:N}", _field17);
                    }

                    _field11 = (_field14 + _field16);//营业业务金额+会员业务金额
                    newdr["field11"] = _field11 <= 0 ? "-" : string.Format("{0:N}", _field11);
                    if (_field11 == 0)
                    {
                        newdr["field11_url"] = "";
                    }
                    else
                    {
                        newdr["field11_url"] = @"./zdmxbb.html?StartTime=" + dr["ctime"].ToString() + "&EndTime=" + dr["etime"].ToString() + "&StoCode=" + dr["stocode"].ToString() + "&ShiftCode=" + dr["ShiftCode"].ToString() + "&CCode=" + dr["ccode"].ToString() + "&DepCode=" + dr["depcode"].ToString() + "&StoName=" + dr["stoname"].ToString() + "&BillType=1";
                    }
                    //判断账单是否平
                    bool isp = true;
                    if (_field14 != _field15)
                    {
                        isp = false;
                    }
                    if (_field16 != _field17)
                    {
                        isp = false;
                    }
                    if (isp)
                    {
                        newdr["field22"] = "0";
                    }
                    else
                    {
                        newdr["field22"] = "1";//账单不平
                    }
                    dt_Result.Rows.Add(newdr);
                }
                #endregion
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
            string json1 = new LayUItableHelper().GetLayUITableHead(arrayList);
            string json2 = JsonHelper.DataTableToJSON(dt_Result);
            if (string.IsNullOrEmpty(json2))
            {
                string nulljson = "{";
                nulljson += "\"title\":[[{\"field\": \"err\",\"title\":\"\",\"align\":\"center\"}]],";
                nulljson += "\"data\":[{\"err\":\"无数据\"}]";
                nulljson += "}";
                Pagcontext.Response.Write(nulljson);
                return;
            }
            string json = "{";
            json += "\"title\":[" + json1 + "],";
            json += "\"data\":" + json2;
            json += "}";
            Pagcontext.Response.Write(json);
        }

        private void ShiftBillReport(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "StartTime", "EndTime", "userid", "StoCode", "DepCode", "CCode", "ShiftCode", "IsInvmoney", "IsPresdishe", "PayType", "FinCode", "MemType", "BillCode", "BillPayCode", "BillType", "StoName", "PayWay" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string StartTime = dicPar["StartTime"].ToString();
            string EndTime = dicPar["EndTime"].ToString();
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string DepCode = dicPar["DepCode"].ToString();
            string CCode = dicPar["CCode"].ToString();
            string ShiftCode = dicPar["ShiftCode"].ToString();
            string IsInvmoney = dicPar["IsInvmoney"].ToString();
            string IsPresdishe = dicPar["IsPresdishe"].ToString();
            string PayType = dicPar["PayType"].ToString();
            string FinCode = dicPar["FinCode"].ToString();
            string MemType = dicPar["MemType"].ToString();
            string BillCode = dicPar["BillCode"].ToString();
            string BillPayCode = dicPar["BillPayCode"].ToString();
            string BillType = dicPar["BillType"].ToString();
            string StoName = dicPar["StoName"].ToString();
            string PayWay = dicPar["PayWay"].ToString();
            if (PayWay == "2")
            {
                CCode = string.Empty;
                ShiftCode = string.Empty;
                IsInvmoney = string.Empty;
                IsPresdishe = string.Empty;
                DepCode = string.Empty;
            }


            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));//部门

            dt_Result.Columns.Add("field2", typeof(string));//账单号
            dt_Result.Columns.Add("field2_url", typeof(string));//账单号跳转
            dt_Result.Columns.Add("field3", typeof(string));//餐别
            dt_Result.Columns.Add("field4", typeof(string));//桌台
            dt_Result.Columns.Add("field5", typeof(string));//人数
            dt_Result.Columns.Add("field6", typeof(string));//结账时间
            dt_Result.Columns.Add("field7", typeof(string));//收银机Mac
            dt_Result.Columns.Add("field8", typeof(string));//收银员
            //dt_Result.Columns.Add("field9", typeof(string));//赠送金额
            dt_Result.Columns.Add("field10", typeof(string));//开票金额
            dt_Result.Columns.Add("field21", typeof(string));//账单金额
            dt_Result.Columns.Add("field22", typeof(string));//账单是否持平 0为持平 1为不平
            dt_Result.Columns.Add("field24", typeof(string));//以下都为小计
            dt_Result.Columns.Add("field25", typeof(string));
            dt_Result.Columns.Add("field27", typeof(string));
            dt_Result.Columns.Add("field17", typeof(string));//会员卡项
            dt_Result.Columns.Add("field18", typeof(string));
            dt_Result.Columns.Add("field19", typeof(string));
            dt_Result.Columns.Add("field20", typeof(string));
            DataSet ds = bll.ShiftBillReport(StartTime, EndTime, StoCode, DepCode, CCode, ShiftCode, IsInvmoney, IsPresdishe, PayType, FinCode, MemType, BillCode, BillPayCode, BillType, PayWay);
            ArrayList arrayList = new ArrayList();
            List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题
            List<LayUItableHelper> Title_list2 = new List<LayUItableHelper>();//第二行标题
            if (ds != null && ds.Tables.Count == 9)
            {
                DataTable dt_bill = ds.Tables[0];//账单表
                DataTable dt_Shift = ds.Tables[1];//班次
                DataTable dt_OrderDis = ds.Tables[4];

                if (dt_OrderDis != null && dt_OrderDis.Rows.Count > 0)
                {
                    DataTable dtFinType = GetCacheToFinType(userid);
                    if (dtFinType != null && dtFinType.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_OrderDis.Rows)
                        {
                            string fincode = dr["FinCode"].ToString();
                            if (dtFinType.Select("fincode='" + fincode + "'").Length > 0)
                            {
                                DataRow dr_sto = dtFinType.Select("fincode='" + fincode + "'")[0];
                                dr["FinTypeName"] = dr_sto["finname"].ToString();
                            }
                        }
                    }
                    dt_OrderDis.AcceptChanges();
                }

                #region 组织表头，注意顺序
                //固定title
                Title_list1.Add(new LayUItableHelper { Fixed = "left", type = "numbers", Sort = true, Field = "field0", width = 150, Title = "序号", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Fixed = "left", Sort = false, Field = "field2", Title = "账单号", Align = "center", TotalRowText = "合计", HeadTemplet = "field2_url", Rowspan = 2, Style = "cursor: pointer;color: #0000FF" });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field1", Title = "部门", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field3", Title = "餐别", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field4", Title = "桌台", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field5", Title = "人数", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field6", Title = "结账时间", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field7", Title = "收银机Mac", Align = "center", Rowspan = 2 });
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field8", Title = "收银员", Align = "center", Rowspan = 2 });

                //开票金额

                int colspan = 0;
                DataTable zs_table = ds.Tables[7];//赠送
                if (zs_table != null && zs_table.Rows.Count > 0)
                {
                    if (StoCode == "08")
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field9", Title = "签送", Align = "center", TotalRow = true });
                        dt_Result.Columns.Add("field9", typeof(string));//优惠券金额
                        colspan += 1;
                    }
                    else
                    {
                        Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field9", Title = "赠送金额", Align = "center", Rowspan = 2, TotalRow = true });
                        dt_Result.Columns.Add("field9", typeof(string));//赠送金额
                    }

                }


                //优惠券金额
                decimal CouponMoneyTile = dt_bill.Select().Sum(x => x.Field<decimal>("CouponMoney"));
                if (CouponMoneyTile != 0)
                {
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field11", Title = "优惠券", Align = "center", TotalRow = true });
                    dt_Result.Columns.Add("field11", typeof(string));//优惠券金额
                    colspan += 1;
                }

                //优惠券虚增金额
                //decimal VIMoneyTile = dt_bill.Select().Sum(x => x.Field<decimal>("VIMoney"));
                //if (VIMoneyTile != 0)
                //{
                //    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field12", Title = "优惠券虚增金额", Align = "center", TotalRow = true });
                //    dt_Result.Columns.Add("field12", typeof(string));//优惠券虚增金额
                //    colspan += 1;
                //}

                //折扣金额
                decimal DiscountMoneyTile = dt_bill.Select().Sum(x => x.Field<decimal>("DiscountMoney"));
                if (DiscountMoneyTile != 0)
                {
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field13", Title = "折扣", Align = "center", TotalRow = true });
                    dt_Result.Columns.Add("field13", typeof(string));//折扣金额
                    colspan += 1;
                }

                //抹零金额
                decimal ZeroCutMoneyTile = 0;
                foreach (DataRow dr in dt_bill.Rows)
                {
                    ZeroCutMoneyTile += Math.Abs(Helper.StringToDecimal(dr["ZeroCutMoney"].ToString()));
                }
                if (ZeroCutMoneyTile != 0)
                {
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field14", Title = "抹零", Align = "center", TotalRow = true });
                    dt_Result.Columns.Add("field14", typeof(string));//抹零金额
                    colspan += 1;
                }

                //积分抵扣金额
                decimal PointMoneyTile = dt_bill.Select().Sum(x => x.Field<decimal>("PointMoney"));
                if (PointMoneyTile != 0)
                {
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field15", Title = "积分抵扣", Align = "center", TotalRow = true });
                    dt_Result.Columns.Add("field15", typeof(string));//积分抵扣金额
                    colspan += 1;
                }

                //积分抵扣虚增金额
                decimal VirMoneyTile = dt_bill.Select().Sum(x => x.Field<decimal>("VirMoney"));
                if (VirMoneyTile != 0)
                {
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field16", Title = "积分抵扣虚增金额", Align = "center", TotalRow = true });
                    dt_Result.Columns.Add("field16", typeof(string));//积分抵扣虚增金额
                    colspan += 1;
                }

                //券回收
                DataTable dt8 = ds.Tables[8];
                if (FinCode == "P49976" && dt8 != null && dt8.Rows.Count > 0)
                {
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field30", Title = "券回收", Align = "center", TotalRow = true });
                    dt_Result.Columns.Add("field30", typeof(string));//券回收
                    colspan += 1;
                }

                //收入项目动态title
                string paymethodcodes = string.Empty;
                DataTable dt_BillPay = ds.Tables[2];
                DataTable dt_MemcarPay = ds.Tables[5];
                bool isygk = false;//是否存在员工卡充值账单
                bool ismsk = false;//是否存在美食卡充值账单
                bool isgbk = false;//是否存在贵宾卡充值账单
                bool iscost = false;//工本费
                string MemBillCode = string.Empty;

                if (dt_BillPay != null)
                {
                    //组织第二行标题-支付方式
                    foreach (DataRow dr in dt_BillPay.Rows)
                    {
                        string field = Others.GetChineseSpell(dr["PayMethodName"].ToString() + dr["PayMethodCode"].ToString());//后期组织数据时，datatable的列名需要对应
                        if (!paymethodcodes.Contains(field))
                        {
                            dt_Result.Columns.Add("sy" + field, typeof(string));//动态表头
                            Title_list2.Add(new LayUItableHelper { Sort = false, Field = "sy" + field, Title = dr["PayMethodName"].ToString(), Align = "center", TotalRow = true });
                            ++colspan;
                            paymethodcodes += field + ",";
                        }
                    }
                }



                if (!string.IsNullOrEmpty(paymethodcodes))
                {
                    Title_list1.Add(new LayUItableHelper { Sort = false, Field = "sr", Title = "收款项目", Align = "center", Colspan = (colspan + 1), Rowspan = 1 });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field24", Title = "小计", Align = "center", TotalRow = true });
                }

                //消费项目动态title
                colspan = 0;
                string typecodes = string.Empty;
                if (dt_OrderDis != null && dt_OrderDis.Rows.Count > 0)
                {
                    //组织第二行标题-支付方式
                    foreach (DataRow dr in dt_OrderDis.Rows)
                    {
                        string field = Others.GetChineseSpell(dr["FinTypeName"].ToString() + dr["FinCode"].ToString());//后期组织数据时，datatable的列名需要对应
                        if(string.IsNullOrEmpty(field))
                        {
                            field = "(空)";
                        }
                        if (!typecodes.Contains(field))
                        {
                            dt_Result.Columns.Add("xf" + field, typeof(string));//动态表头
                            Title_list2.Add(new LayUItableHelper { Sort = false, Field = "xf" + field, Title = dr["FinTypeName"].ToString(), Align = "center", TotalRow = true });
                            ++colspan;
                            typecodes += field + ",";
                        }
                    }
                    Title_list1.Add(new LayUItableHelper { Sort = false, Field = "xf", Title = "消费项目", Align = "center", Colspan = (colspan + 1), Rowspan = 1 });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field25", Title = "小计", Align = "center", TotalRow = true });
                }
                typecodes = typecodes.TrimEnd(',');
                if (!typecodes.Contains("YYPFP49976"))
                {
                    if (FinCode == "P49976" && dt8 != null && dt8.Rows.Count > 0)
                    {
                        dt_Result.Columns.Add("xfYYPFP49976", typeof(string));//动态表头
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "xfYYPFP49976", Title = "影院票房", Align = "center", TotalRow = true });
                        colspan += 1;
                    }
                }


                ////会员卡项目动态title
                //colspan = 0;
                //string mempaymethodcodes = string.Empty;
                //if (dt_BillPay != null && dt_BillPay.Rows.Count > 0 && !string.IsNullOrEmpty(MemBillCode))
                //{
                //    //组织第二行标题-支付方式
                //    DataRow[] memrows = dt_BillPay.Select("BillCode in(" + MemBillCode + ")");
                //    if (memrows != null && memrows.Length > 0)
                //    {
                //        foreach (DataRow dr in memrows)
                //        {
                //            string field = Others.GetChineseSpell(dr["PayMethodName"].ToString() + dr["PayMethodCode"].ToString());//后期组织数据时，datatable的列名需要对应
                //            if (!mempaymethodcodes.Contains(field))
                //            {
                //                dt_Result.Columns.Add("sy" + field, typeof(string));//动态表头
                //                Title_list2.Add(new LayUItableHelper { Sort = false, Field = "sy" + field, Title = dr["PayMethodName"].ToString(), Align = "center", TotalRow = true });
                //                ++colspan;
                //                mempaymethodcodes += field + ",";
                //            }
                //        }
                //        Title_list1.Add(new LayUItableHelper { Sort = false, Field = "hyksk", Title = "会员卡收款项目", Align = "center", Colspan = (colspan + 1), Rowspan = 1 });
                //        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field26", Title = "小计", Align = "center", TotalRow = true });
                //    }
                //}

                //会员卡动态title
                colspan = 0;
                if (dt_MemcarPay != null && dt_MemcarPay.Rows.Count > 0)
                {
                    DataRow[] memrows = dt_MemcarPay.Select("cardtype<>'8' and cardtype<>'food'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field17", Title = "贵宾卡", Align = "center", TotalRow = true });
                        colspan += 1;
                        isgbk = true;
                    }

                    decimal cost = dt_MemcarPay.Select().Sum(x => x.Field<decimal>("cardcost"));
                    if (cost != 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field18", Title = "工本费", Align = "center", TotalRow = true });
                        colspan += 1;
                        iscost = true;
                    }

                    memrows = dt_MemcarPay.Select("cardtype='8'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field19", Title = "员工卡", Align = "center", TotalRow = true });
                        colspan += 1;
                        isygk = true;
                    }

                    memrows = dt_MemcarPay.Select("cardtype='food'");
                    if (memrows != null && memrows.Length > 0)
                    {
                        Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field20", Title = "美食卡", Align = "center", TotalRow = true });
                        colspan += 1;
                        ismsk = true;
                    }
                    Title_list1.Add(new LayUItableHelper { Sort = false, Field = "hyksk", Title = "会员项目", Align = "center", Colspan = (colspan + 1), Rowspan = 1 });
                    Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field27", Title = "小计", Align = "center", TotalRow = true });
                }
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field21", Title = "账单金额", Align = "center", Rowspan = 2, htmltemplet = "#T_field21", TotalRow = true });//最后一列为班次金额

                arrayList.Add(Title_list1);//第一行表头
                arrayList.Add(Title_list2);//第二行表头
                dt_Result.AcceptChanges();
                #endregion

                #region 组织数据
                //判断是否从影院票房进来的，影院票房的要统计批量消券
                if (FinCode == "P49976" && dt8 != null && dt8.Rows.Count > 0)
                {
                    #region
                    string ccnams = string.Empty;
                    foreach (DataRow dr in dt8.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["UCname"].ToString()))
                        {
                            if (!ccnams.Contains(dr["UCname"].ToString()))
                            {
                                ccnams += dr["UCname"].ToString() + ",";
                            }
                        }
                    }
                    ccnams = ccnams.TrimEnd(',');

                    foreach (DataRow _dr in dt_Shift.Rows)
                    {
                        foreach (string name in ccnams.Split(','))
                        {
                            DataRow[] drs = dt8.Select("UCname='" + name + "' and ShiftId='" + _dr["ShiftCode"].ToString() + "'");
                            if (drs != null && drs.Length > 0)
                            {
                                DataRow newdr = dt_Result.NewRow();
                                newdr["field1"] = "";
                                newdr["field2_url"] = "./plxqjl.html?StoCode=" + StoCode + "&StoName=" + StoName + "&ShiftCode=" + drs[0]["ShiftId"].ToString();
                                newdr["field2"] = drs[0]["ShiftId"].ToString();
                                newdr["field3"] = string.Empty;
                                newdr["field4"] = string.Empty;
                                newdr["field5"] = "0";
                                newdr["field6"] = drs[0]["CTime"].ToString();
                                newdr["field7"] = string.Empty;
                                newdr["field8"] = name;
                                newdr["field30"] = drs.Sum(x => x.Field<decimal>("SingleMoney"));

                                #region 收款项
                                //处理开票金额

                                //处理赠送金额
                                if (zs_table != null && zs_table.Rows.Count > 0)
                                {
                                    newdr["field9"] = "-";
                                }

                                //优惠券金额
                                if (CouponMoneyTile != 0)
                                {
                                    newdr["field11"] = "-";
                                }

                                ////优惠券虚增金额
                                //if (VIMoneyTile != 0)
                                //{
                                //    newdr["field12"] = "-";
                                //}

                                //折扣金额
                                if (DiscountMoneyTile != 0)
                                {
                                    newdr["field13"] = "-";
                                }

                                //抹零金额
                                if (ZeroCutMoneyTile != 0)
                                {
                                    newdr["field14"] = "-";
                                }

                                //积分抵扣金额
                                if (PointMoneyTile != 0)
                                {
                                    newdr["field15"] = "-";
                                }

                                //积分抵扣虚增金额
                                if (VirMoneyTile != 0)
                                {
                                    newdr["field16"] = "-";
                                }
                                string[] payfields = paymethodcodes.Split(',');
                                foreach (string p in payfields)
                                {
                                    if (!string.IsNullOrEmpty(p))
                                    {
                                        string value = newdr["sy" + p].ToString();
                                        if (string.IsNullOrEmpty(value))
                                        {
                                            newdr["sy" + p] = "-";
                                        }
                                    }
                                }
                                #endregion

                                #region 销售项
                                newdr["xfYYPFP49976"] = drs.Sum(x => x.Field<decimal>("SingleMoney"));
                                string[] typecodesArray = typecodes.Split(',');//该班次的销售类别没数据，则将所有该班次的销售类别金额设置为-
                                foreach (string p in typecodesArray)
                                {
                                    if (!string.IsNullOrEmpty(p))
                                    {
                                        newdr["xf" + p] = "-";
                                    }
                                }
                                #endregion

                                #region 会员项
                                if (isgbk)//贵宾卡
                                {
                                    newdr["field17"] = "-";
                                }
                                if (iscost)
                                {
                                    newdr["field18"] = "-";
                                }
                                if (isygk)
                                {
                                    newdr["field19"] = "-";
                                }
                                if (ismsk)
                                {
                                    newdr["field20"] = "-";
                                }
                                #endregion

                                newdr["field24"] = drs.Sum(x => x.Field<decimal>("SingleMoney"));
                                newdr["field25"] = drs.Sum(x => x.Field<decimal>("SingleMoney"));
                                newdr["field27"] = drs.Sum(x => x.Field<decimal>("SingleMoney"));
                                newdr["field21"] = drs.Sum(x => x.Field<decimal>("SingleMoney"));
                                newdr["field22"] = "0";
                                dt_Result.Rows.Add(newdr);
                            }
                        }
                    }
                    #endregion

                }
                foreach (DataRow dr in dt_bill.Rows)//循环账单组装数据
                {
                    string depname = string.Empty;
                    string comcode = string.Empty;
                    if (dt_Shift != null && dt_Shift.Rows.Count > 0)
                    {
                        DataRow[] shift = dt_Shift.Select("ShiftCode='" + dr["ShiftCode"].ToString() + "'");
                        if (shift != null && shift.Length > 0)
                        {
                            depname = shift[0]["depname"].ToString();
                            comcode = shift[0]["comcode"].ToString();
                        }
                    }

                    DataRow newdr = dt_Result.NewRow();
                    newdr["field1"] = depname;
                    newdr["field2_url"] = @"./BillingDetails.html?StoCode=" + StoCode + "&StoName=" + StoName + "&BillCode=" + dr["PKCode"].ToString();
                    newdr["field2"] = dr["PKCode"].ToString();
                    newdr["field3"] = dr["mealname"].ToString();
                    newdr["field4"] = dr["TableName"].ToString();
                    newdr["field5"] = dr["CusNum"].ToString();
                    newdr["field6"] = dr["FTime"].ToString();
                    newdr["field7"] = comcode;
                    newdr["field8"] = dr["CCname"].ToString();

                    decimal _field14 = 0;//营业项目小计
                    decimal _field15 = 0;//销售项目小计
                    decimal _field16 = 0;//会员收款项目小计
                    decimal _field17 = 0;//会员项目小计
                    decimal _field11 = 0;//账单金额

                    //处理开票金额

                    //处理赠送金额
                    decimal zssummoney = 0;
                    if (zs_table != null && zs_table.Rows.Count > 0)
                    {
                        DataRow[] zsdrs = zs_table.Select("BillCode='" + dr["PKCode"] + "'");//找到当前班次的账单数
                        if (zsdrs != null && zsdrs.Length > 0)
                        {
                            zssummoney = zsdrs.Sum(x => x.Field<decimal>("TotalMoney"));
                        }
                        newdr["field9"] = zssummoney == 0 ? "-" : string.Format("{0:N}", zssummoney);
                        // _field14 += zssummoney;
                    }

                    //优惠券金额
                    if (CouponMoneyTile != 0)
                    {
                        decimal CouponMoney = dt_bill.Select("PKCode='" + dr["PKCode"].ToString() + "'").Sum(x => x.Field<decimal>("CouponMoney"));
                        newdr["field11"] = CouponMoney == 0 ? "-" : string.Format("{0:N}", CouponMoney);
                        _field14 += CouponMoney;
                    }

                    //优惠券虚增金额
                    //if (VIMoneyTile != 0)
                    //{
                    //    decimal VIMoney = dt_bill.Select("PKCode='" + dr["PKCode"].ToString() + "'").Sum(x => x.Field<decimal>("VIMoney"));
                    //    newdr["field12"] = VIMoney == 0 ? "-" : string.Format("{0:N}", VIMoney);
                    //    if(StoCode=="08")
                    //    {
                    //        _field14 += VIMoney;
                    //    }

                    //}

                    //折扣金额
                    if (DiscountMoneyTile != 0)
                    {
                        decimal DiscountMoney = dt_bill.Select("PKCode='" + dr["PKCode"].ToString() + "'").Sum(x => x.Field<decimal>("DiscountMoney"));
                        newdr["field13"] = DiscountMoney == 0 ? "-" : string.Format("{0:N}", DiscountMoney);
                        _field14 += DiscountMoney;
                    }

                    //抹零金额
                    if (ZeroCutMoneyTile != 0)
                    {
                        decimal ZeroCutMoney = dt_bill.Select("PKCode='" + dr["PKCode"].ToString() + "'").Sum(x => x.Field<decimal>("ZeroCutMoney"));
                        newdr["field14"] = ZeroCutMoney == 0 ? "-" : string.Format("{0:N}", ZeroCutMoney);
                        _field14 += ZeroCutMoney;
                    }

                    //积分抵扣金额
                    if (PointMoneyTile != 0)
                    {
                        decimal PointMoney = dt_bill.Select("PKCode='" + dr["PKCode"].ToString() + "'").Sum(x => x.Field<decimal>("PointMoney"));
                        newdr["field15"] = PointMoney == 0 ? "-" : string.Format("{0:N}", PointMoney);
                        _field14 += PointMoney;
                    }

                    //积分抵扣虚增金额
                    if (VirMoneyTile != 0)
                    {
                        decimal VirMoney = dt_bill.Select("PKCode='" + dr["PKCode"].ToString() + "'").Sum(x => x.Field<decimal>("VirMoney"));
                        newdr["field16"] = VirMoney == 0 ? "-" : string.Format("{0:N}", VirMoney);
                        _field14 += VirMoney;
                    }


                    //收入项目
                    if (dt_BillPay != null && dt_BillPay.Rows.Count > 0)
                    {
                        DataRow[] _drs = dt_BillPay.Select("BillCode='" + dr["PKCode"].ToString() + "'");
                        if (_drs != null && _drs.Length > 0)
                        {
                            foreach (DataRow dr_billpay in _drs)
                            {
                                string field = Others.GetChineseSpell(dr_billpay["PayMethodName"].ToString() + dr_billpay["PayMethodCode"].ToString());
                                _field14 += Helper.StringToDecimal(dr_billpay["PayMoney"].ToString());
                                newdr["sy" + field] = Helper.StringToDecimal(dr_billpay["PayMoney"].ToString()) == 0 ? "-" : string.Format("{0:N}", Helper.StringToDecimal(dr_billpay["PayMoney"].ToString()));
                            }
                        }
                        string[] payfields = paymethodcodes.Split(',');
                        foreach (string p in payfields)
                        {
                            if (!string.IsNullOrEmpty(p))
                            {
                                string value = newdr["sy" + p].ToString();
                                if (string.IsNullOrEmpty(value))
                                {
                                    newdr["sy" + p] = "-";
                                }
                            }
                        }
                    }

                    //销售类别数据组织
                    if (dt_OrderDis != null && dt_BillPay.Rows.Count > 0)
                    {
                        string _fincodes = string.Empty;
                        DataRow[] findrs = dt_OrderDis.Select("billcode='" + dr["PKCode"].ToString() + "'");
                        if (findrs != null && findrs.Length > 0)
                        {
                            foreach (DataRow drorder in findrs)//循环订单明细,对财务类型销售金额进行统计
                            {
                                if (!_fincodes.Contains(drorder["FinCode"].ToString() + ","))//重复的不再统计
                                {
                                    decimal _summoney = 0;
                                    string field = Others.GetChineseSpell(drorder["FinTypeName"].ToString() + drorder["FinCode"].ToString());//组装title对应字段名
                                    if (string.IsNullOrEmpty(field))
                                    {
                                        field = "(空)";
                                    }
                                    DataRow[] _findrs = dt_OrderDis.Select("billcode='" + dr["PKCode"].ToString() + "' and FinCode='" + drorder["FinCode"].ToString() + "'");//获取指定班次指定财务类别订单明细
                                    foreach (DataRow _fin in _findrs)
                                    {
                                        _summoney += Helper.StringToDecimal(_fin["TotalMoney"].ToString());//统计该财务类别的金额
                                        _field15 += Helper.StringToDecimal(_fin["TotalMoney"].ToString());//累加小计
                                    }
                                    if(!string.IsNullOrEmpty(field))
                                    {
                                        newdr["xf" + field] = _summoney == 0 ? "-" : string.Format("{0:N}", _summoney);//动态绑定字段值
                                        _fincodes += drorder["FinCode"].ToString() + ",";//记录本类别以统计，避免重复统计
                                    }
                                    else
                                    {
                                        newdr["xf"] = _summoney == 0 ? "-" : string.Format("{0:N}", _summoney);//动态绑定字段值
                                        _fincodes +="" + ",";//记录本类别以统计，避免重复统计
                                    }
                                }
                            }
                            string[] typecodesArray = typecodes.Split(',');//该班次的销售类别没数据，则将所有该班次的销售类别金额设置为-
                            foreach (string p in typecodesArray)
                            {
                                if (!string.IsNullOrEmpty(p))
                                {
                                    string value = newdr["xf" + p].ToString();
                                    if (string.IsNullOrEmpty(value))
                                    {
                                        newdr["xf" + p] = "-";
                                    }
                                }
                            }
                        }
                    }

                    ////会员卡收款项
                    //if (dt_BillPay != null && dt_BillPay.Rows.Count > 0 && !string.IsNullOrEmpty(MemBillCode) && MemBillCode.Contains(dr["PKCode"].ToString() + ","))
                    //{
                    //    DataRow[] _drs = dt_BillPay.Select("BillCode='" + dr["PKCode"].ToString() + "'");//找到该班次会员卡的支付信息
                    //    if (_drs != null && _drs.Length > 0)
                    //    {
                    //        foreach (DataRow dr_billpay in _drs)
                    //        {
                    //            string field = Others.GetChineseSpell(dr_billpay["PayMethodName"].ToString() + dr_billpay["PayMethodCode"].ToString());
                    //            _field16 += Helper.StringToDecimal(dr_billpay["PayMoney"].ToString());
                    //            newdr["hy" + field] = Helper.StringToDecimal(dr_billpay["PayMoney"].ToString()) == 0 ? "-" : string.Format("{0:N}", Helper.StringToDecimal(dr_billpay["PayMoney"].ToString()));
                    //        }
                    //    }
                    //    string[] mempaymethodcodesArray = mempaymethodcodes.Split(',');
                    //    foreach (string p in mempaymethodcodesArray)
                    //    {
                    //        if (!string.IsNullOrEmpty(p))
                    //        {
                    //            string value = newdr["hy" + p].ToString();
                    //            if (string.IsNullOrEmpty(value))
                    //            {
                    //                newdr["hy" + p] = "-";
                    //            }
                    //        }
                    //    }
                    //}

                    //会员项
                    if (dt_MemcarPay != null && dt_MemcarPay.Rows.Count > 0)
                    {
                        decimal cardcost = 0;
                        cardcost = dt_MemcarPay.Select("BillCode='" + dr["PKCode"].ToString() + "'").Sum(x => x.Field<decimal>("cardcost"));
                        //贵宾卡
                        if (isgbk)//贵宾卡
                        {
                            DataRow[] _gbk = dt_MemcarPay.Select("BillCode='" + dr["PKCode"].ToString() + "' and cardtype<>'8' and cardtype<>'food'");//贵宾卡项
                            if (_gbk != null && _gbk.Length > 0)
                            {
                                decimal regamount = _gbk.Sum(x => x.Field<decimal>("regamount"));
                                _field17 += regamount;
                                newdr["field17"] = regamount == 0 ? "-" : string.Format("{0:N}", regamount);
                            }
                            else
                            {
                                newdr["field17"] = "-";
                            }
                        }
                        //工本费
                        if (iscost)
                        {
                            _field17 += cardcost;
                            newdr["field18"] = cardcost == 0 ? "-" : string.Format("{0:N}", cardcost);
                        }
                        //员工卡
                        if (isygk)
                        {
                            DataRow[] _ygk = dt_MemcarPay.Select("BillCode='" + dr["PKCode"].ToString() + "' and cardtype='8'");//贵宾卡项
                            if (_ygk != null && _ygk.Length > 0)
                            {
                                decimal regamount = _ygk.Sum(x => x.Field<decimal>("regamount"));
                                _field17 += regamount;
                                newdr["field19"] = regamount == 0 ? "-" : string.Format("{0:N}", regamount);
                            }
                            else
                            {
                                newdr["field19"] = "-";
                            }
                        }
                        //美食卡
                        if (ismsk)
                        {
                            DataRow[] _ygk = dt_MemcarPay.Select("BillCode='" + dr["PKCode"].ToString() + "' and cardtype='food'");//贵宾卡项
                            if (_ygk != null && _ygk.Length > 0)
                            {
                                decimal regamount = _ygk.Sum(x => x.Field<decimal>("regamount"));
                                _field17 += regamount;
                                newdr["field20"] = regamount == 0 ? "-" : string.Format("{0:N}", regamount);
                            }
                            else
                            {
                                newdr["field20"] = "-";
                            }
                        }
                    }
                    else
                    {
                        if (isgbk)//贵宾卡
                        {
                            newdr["field17"] = "-";
                        }
                        if (iscost)
                        {
                            newdr["field18"] = "-";
                        }
                        if (isygk)
                        {
                            newdr["field19"] = "-";
                        }
                        if (ismsk)
                        {
                            newdr["field20"] = "-";
                        }
                    }
                    _field15 = (_field15 - zssummoney);//收款/销售的不计算赠送的
                    newdr["field24"] = _field14 == 0 ? "-" : string.Format("{0:N}", _field14);
                    newdr["field25"] = _field15 == 0 ? "-" : string.Format("{0:N}", _field15);
                    newdr["field27"] = _field17 == 0 ? "-" : string.Format("{0:N}", _field17);
                    _field11 = _field14;//班次金额合计
                    newdr["field21"] = _field11 == 0 ? "-" : string.Format("{0:N}", _field11);
                    bool isp = true;
                    if (_field14 != (_field15 + _field17))
                    {
                        isp = false;
                    }
                    if (isp)
                    {
                        newdr["field22"] = "0";
                    }
                    else
                    {
                        newdr["field22"] = "1";//账单不平
                    }
                    dt_Result.Rows.Add(newdr);
                }
                #endregion
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
            string json1 = new LayUItableHelper().GetLayUITableHead(arrayList);
            string json2 = JsonHelper.DataTableToJSON(dt_Result);
            string json = "{";
            json += "\"title\":[" + json1 + "],";
            json += "\"data\":" + json2;
            json += "}";
            Pagcontext.Response.Write(json);
        }

        private void StoreFinTypeReport(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "StartTime", "EndTime", "StoCode", "DisCode", "QuickCode", "FinType", "StoName" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string StartTime = dicPar["StartTime"].ToString();
            string EndTime = dicPar["EndTime"].ToString();
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string DisCode = dicPar["DisCode"].ToString();
            if (!string.IsNullOrEmpty(DisCode) && DisCode == "{}")
            {
                DisCode = string.Empty;
            }
            string QuickCode = dicPar["QuickCode"].ToString();
            string FinType = dicPar["FinType"].ToString();
            string StoName = dicPar["StoName"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }

            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));//一级类别
            dt_Result.Columns.Add("field2", typeof(string));//财务类别
            dt_Result.Columns.Add("field3", typeof(string));//编码
            dt_Result.Columns.Add("field4", typeof(string));//菜品名称
            dt_Result.Columns.Add("field5", typeof(string));//单位
            dt_Result.Columns.Add("field6", typeof(string));//单价
            dt_Result.Columns.Add("field7", typeof(string));//纯销售数量
            dt_Result.Columns.Add("field7_url", typeof(string));//纯销售数量跳转链接
            dt_Result.Columns.Add("field8", typeof(string));//纯销售金额
            DataSet ds = bll.StoreFinTypeReport(StartTime, EndTime, userid, StoCode, DisCode, QuickCode, FinType, BusCode);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                ArrayList arrayList = new ArrayList();
                List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题
                List<LayUItableHelper> Title_list2 = new List<LayUItableHelper>();//第二行标题
                List<LayUItableHelper> Title_list3 = new List<LayUItableHelper>();//第三行标题
                #region 组织表头，注意顺序
                Title_list1.Add(new LayUItableHelper { Sort = false, Field = "field0", Title = StoName + "商品销售报表(财务类别)", Align = "center", Colspan = 8 });
                Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field0", Title = "查询日期" + DateTime.Parse(StartTime).ToString("yyyy.MM.dd") + "-" + DateTime.Parse(EndTime).ToString("yyyy.MM.dd"), Align = "center", Colspan = 8 });
                //固定title
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field1", Title = "一级类别", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field2", Title = "财务类别", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field3", Title = "编码", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field4", Title = "菜品名称", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field5", Title = "单位", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field6", Title = "单价", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field7", Title = "销售数量", Align = "center", HeadTemplet = "field7_url", Style = "cursor: pointer;color: #0000FF" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field8", Title = "销售金额", Align = "center" });
                arrayList.Add(Title_list1);//第一行表头
                arrayList.Add(Title_list2);//第二行表头
                arrayList.Add(Title_list3);//第三行表头
                #endregion

                string type = string.Empty;
                decimal TotalNum = 0;
                decimal TotalMoney = 0;
                decimal n = 0;
                decimal m = 0;
                DataRow _adddr;
                if (dt != null && dt.Rows.Count > 0)
                {
                    string tzcode = string.Empty;
                    List<string> ispacklist = new List<string>();//已经统计过的套内单品的类型标识
                    string discode = string.Empty;
                    foreach (DataRow dr in dt.Select(""))
                    {
                            if (discode.Contains(dr["discode"].ToString()))
                            {
                                continue;
                            }
                            else
                            {
                                discode += dr["discode"].ToString() + ",";
                            }
                            if (string.IsNullOrEmpty(type))
                            {
                                type = dr["DisTypeName"].ToString() + "," + dr["FinTypeName"].ToString();
                            }
                            else
                            {
                                if (dr["DisTypeName"].ToString() + "," + dr["FinTypeName"].ToString() != type)
                                {
                                    n = 0;
                                    m = 0;
                                    _adddr = CreateDataRow(dt_Result, type, dt, out n, out m);
                                    if (_adddr != null)
                                    {
                                        dt_Result.Rows.Add(_adddr);
                                    }
                                    TotalNum += n;
                                    TotalMoney += m;
                                }
                            }
                            DataRow newdr = dt_Result.NewRow();
                            newdr["field1"] = dr["DisTypeName"].ToString();
                            newdr["field2"] = dr["FinTypeName"].ToString();
                            newdr["field3"] = dr["DisCode"].ToString();
                            newdr["field4"] = dr["DisName"].ToString();
                            newdr["field5"] = dr["Uite"].ToString();
                            newdr["field6"] = dr["Price"].ToString() == "0.00" ? "-" : dr["Price"].ToString();
                            newdr["field7"] = Helper.StringToDecimal(dr["DisNum"].ToString());
                            newdr["field7_url"] = @"./zdtcdptjbbzj.html?StoCode=" + StoCode + "&type=" + dr["IsPackage"].ToString() + "&StartTime=" + StartTime + "&EndTime=" + EndTime + "&StoName=" + StoName + "&DisCode=" + dr["DisCode"].ToString();
                            newdr["field8"] = dr["TotalPrice"].ToString() == "0.0000" ? "-" : dr["TotalPrice"].ToString();
                            dt_Result.Rows.Add(newdr);
                            type = dr["DisTypeName"].ToString() + "," + dr["FinTypeName"].ToString();
                    }
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
                //最后一行小计
                n = 0;
                m = 0;
                _adddr = CreateDataRow(dt_Result, type, dt, out n, out m);
                if (_adddr != null)
                {
                    dt_Result.Rows.Add(_adddr);
                }
                TotalNum += n;
                TotalMoney += m;


                DataRow hjdr = dt_Result.NewRow();
                hjdr["field1"] = "总计:";
                hjdr["field2"] = string.Empty;
                hjdr["field3"] = string.Empty;
                hjdr["field4"] = string.Empty;
                hjdr["field5"] = string.Empty;
                hjdr["field6"] = string.Empty;
                hjdr["field7"] = TotalNum;
                hjdr["field8"] = TotalMoney == 0 ? "-" : TotalMoney.ToString();
                dt_Result.Rows.Add(hjdr);

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

        private DataRow CreateDataRow(DataTable dt_Result, string type, DataTable data, out decimal num, out decimal money)
        {
            string[] names = type.Split(',');
            num = 0;
            money = 0;
            DataRow[] dataRows = data.Select("DisTypeName='" + names[0] + "' and FinTypeName='" + names[1] + "'");
            if (dataRows != null && dataRows.Length > 0)
            {
                decimal total1 = dataRows.Sum(x => x.Field<decimal>("DisNum"));
                num = total1;
                decimal total2 = dataRows.Sum(x => x.Field<decimal>("TotalPrice"));
                money = total2;
                DataRow xjdr = dt_Result.NewRow();
                xjdr["field1"] = "小计:";
                xjdr["field2"] = string.Empty;
                xjdr["field3"] = string.Empty;
                xjdr["field4"] = string.Empty;
                xjdr["field5"] = string.Empty;
                xjdr["field6"] = string.Empty;
                xjdr["field7"] = total1;
                xjdr["field8"] = total2 == 0 ? "-" : total2.ToString();
                return xjdr;
            }
            return null;
        }

        private DataRow CreatedistypeDataRow(DataTable dt_Result, string type, DataTable data, out decimal num, out decimal money)
        {
            string[] names = type.Split(',');
            num = 0;
            money = 0;
            DataRow[] dataRows = data.Select("DisTypeName='" + names[0] + "' and DisTypeName1='" + names[1] + "'");
            if (dataRows != null && dataRows.Length > 0)
            {
                decimal total1 = dataRows.Sum(x => x.Field<decimal>("DisNum"));
                num = total1;
                decimal total2 = dataRows.Sum(x => x.Field<decimal>("TotalPrice"));
                money = total2;
                DataRow xjdr = dt_Result.NewRow();
                xjdr["field1"] = "小计:";
                xjdr["field2"] = string.Empty;
                xjdr["field3"] = string.Empty;
                xjdr["field4"] = string.Empty;
                xjdr["field5"] = string.Empty;
                xjdr["field6"] = string.Empty;
                xjdr["field7"] = total1;
                xjdr["field8"] = total2 == 0 ? "-" : total2.ToString();
                return xjdr;
            }
            return null;
        }

        private void StoreDisTypeReport(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "StartTime", "EndTime", "StoCode", "DisCode", "QuickCode", "TypeCode", "StoName", "eTypeCode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string StartTime = dicPar["StartTime"].ToString();
            string EndTime = dicPar["EndTime"].ToString();
            string userid = dicPar["userid"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string DisCode = dicPar["DisCode"].ToString();
            string QuickCode = dicPar["QuickCode"].ToString();
            string TypeCode = dicPar["TypeCode"].ToString();
            string StoName = dicPar["StoName"].ToString();
            string eTypeCode = dicPar["eTypeCode"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));//一级类别
            dt_Result.Columns.Add("field2", typeof(string));//财务类别
            dt_Result.Columns.Add("field3", typeof(string));//编码
            dt_Result.Columns.Add("field4", typeof(string));//菜品名称
            dt_Result.Columns.Add("field5", typeof(string));//单位
            dt_Result.Columns.Add("field6", typeof(string));//单价
            dt_Result.Columns.Add("field7", typeof(string));//纯销售数量
            dt_Result.Columns.Add("field7_url", typeof(string));//纯销售数量
            dt_Result.Columns.Add("field8", typeof(string));//纯销售金额
            DataSet ds = bll.StoreDisTypeReport(StartTime, EndTime, userid, StoCode, DisCode, QuickCode, TypeCode, eTypeCode, BusCode);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                ArrayList arrayList = new ArrayList();
                List<LayUItableHelper> Title_list1 = new List<LayUItableHelper>();//第一行标题
                List<LayUItableHelper> Title_list2 = new List<LayUItableHelper>();//第二行标题
                List<LayUItableHelper> Title_list3 = new List<LayUItableHelper>();//第三行标题
                #region 组织表头，注意顺序
                Title_list1.Add(new LayUItableHelper
                {
                    Sort = false,
                    Field = "field0",
                    Title = StoName + "商品销售报表(菜品类别)",
                    Align = "center",
                    Colspan = 8
                });
                Title_list2.Add(new LayUItableHelper { Sort = false, Field = "field0", Title = "查询日期" + DateTime.Parse(StartTime).ToString("yyyy.MM.dd") + "-" + DateTime.Parse(EndTime).ToString("yyyy.MM.dd"), Align = "center", Colspan = 8 });
                //固定title
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field1", Title = "一级类别", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field2", Title = "二级类别", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field3", Title = "编码", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field4", Title = "菜品名称", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field5", Title = "单位", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field6", Title = "单价", Align = "center" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field7", Title = "销售数量", Align = "center", HeadTemplet = "field7_url", Style = "cursor: pointer;color: #0000FF" });
                Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field8", Title = "销售金额", Align = "center" });
                arrayList.Add(Title_list1);
                arrayList.Add(Title_list2);
                arrayList.Add(Title_list3);
                #endregion

                string type = string.Empty;//标识小计
                string _type = string.Empty;//标识合计
                decimal TotalNum = 0;
                decimal TotalMoney = 0;
                decimal n = 0;
                decimal m = 0;
                DataRow _adddr;
                DataRow[] dataRows;
                if (dt != null && dt.Rows.Count > 0)
                {
                    string tzcode = string.Empty;
                    string discode = string.Empty;
                    foreach (DataRow dr in dt.Select(""))
                    {
                            if (discode.Contains(dr["discode"].ToString()+"-"+dr["IsPackage"].ToString()))
                            {
                                continue;
                            }
                            else
                            {
                                discode += dr["discode"].ToString() + "-" + dr["IsPackage"].ToString() + ",";
                            }
                            if (string.IsNullOrEmpty(type))
                            {
                                type = dr["DisTypeName"].ToString() + "," + dr["DisTypeName1"].ToString();
                                _type = dr["DisTypeName"].ToString();
                            }
                            else
                            {
                                if (dr["DisTypeName"].ToString() + "," + dr["DisTypeName1"].ToString() != type)
                                {
                                    n = 0;
                                    m = 0;
                                    _adddr = CreatedistypeDataRow(dt_Result, type, dt, out n, out m);
                                    if (_adddr != null)
                                    {
                                        dt_Result.Rows.Add(_adddr);
                                    }
                                    TotalNum += n;
                                    TotalMoney += m;
                                }
                            }
                            DataRow newdr = dt_Result.NewRow();
                            newdr["field1"] = dr["DisTypeName"].ToString();
                            newdr["field2"] = dr["DisTypeName1"].ToString();
                            newdr["field3"] = dr["DisCode"].ToString();
                            newdr["field4"] = dr["DisName"].ToString();
                            newdr["field5"] = dr["Uite"].ToString();
                            newdr["field6"] = dr["Price"].ToString() == "0.00" ? "-" : dr["Price"].ToString();
                            newdr["field7"] = Helper.StringToDecimal(dr["DisNum"].ToString());
                            newdr["field7_url"] = @"./zdtcdptjbbzj.html?StoCode=" + StoCode + "&type=" + dr["IsPackage"].ToString() + "&StartTime=" + StartTime + "&EndTime=" + EndTime + "&StoName=" + StoName + "&DisCode=" + dr["DisCode"].ToString();
                            newdr["field8"] = dr["TotalPrice"].ToString() == "0.0000" ? "-" : dr["TotalPrice"].ToString();
                            dt_Result.Rows.Add(newdr);
                            type = dr["DisTypeName"].ToString() + "," + dr["DisTypeName1"].ToString();
                            _type = dr["DisTypeName"].ToString();
                    }
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
                //小计
                n = 0;
                m = 0;
                _adddr = CreatedistypeDataRow(dt_Result, type, dt, out n, out m);
                if (_adddr != null)
                {
                    dt_Result.Rows.Add(_adddr);
                }
                TotalNum += n;
                TotalMoney += m;

                //总计
                DataRow hjdr = dt_Result.NewRow();
                hjdr["field1"] = "总计:";
                hjdr["field2"] = string.Empty;
                hjdr["field3"] = string.Empty;
                hjdr["field4"] = string.Empty;
                hjdr["field5"] = string.Empty;
                hjdr["field6"] = string.Empty;
                hjdr["field7"] = TotalNum;
                hjdr["field8"] = TotalMoney == 0 ? "-" : TotalMoney.ToString();
                dt_Result.Rows.Add(hjdr);


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

        private void CouponCheckLog(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            List<string> pra = new List<string>() { "stocode", "shiftid", "stoname" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string stocode = dicPar["stocode"].ToString();
            string shiftid = dicPar["shiftid"].ToString();
            string stoname = dicPar["stoname"].ToString();
            string BusCode = string.Empty;
            if (dicPar.ContainsKey("BusCode"))
            {
                BusCode = dicPar["BusCode"].ToString();
            }
            DataTable dt_log = new DataTable();
            dt_log = bll.GetCouponCheckLog(stocode, shiftid, BusCode);

            DataTable dt_Result = new DataTable("data");
            //非动态数据体
            dt_Result.Columns.Add("field1", typeof(string));//班次号
            dt_Result.Columns.Add("field2", typeof(string));//券码
            dt_Result.Columns.Add("field3", typeof(string));//名称
            dt_Result.Columns.Add("field4", typeof(string));//金额
            dt_Result.Columns.Add("field5", typeof(string));//时间
            dt_Result.Columns.Add("field6", typeof(string));//消券人

            ArrayList arrayList = new ArrayList();
            List<LayUItableHelper> Title_list3 = new List<LayUItableHelper>();//第三行标题
            Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field1", Title = "班次号", TotalRowText = "合计", Align = "center" });
            Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field2", Title = "券码", Align = "center" });
            Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field3", Title = "名称", Align = "center" });
            Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field4", Title = "金额", Align = "center", TotalRow = true });
            Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field5", Title = "时间", Align = "center" });
            Title_list3.Add(new LayUItableHelper { Sort = false, Field = "field6", Title = "消券人", Align = "center" });
            arrayList.Add(Title_list3);//第二行表头

            if (dt_log != null && dt_log.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_log.Rows)
                {
                    DataRow newrow = dt_Result.NewRow();
                    newrow["field1"] = shiftid;
                    newrow["field2"] = dr["CheckCode"];
                    newrow["field3"] = dr["CouName"];
                    newrow["field4"] = dr["SingleMoney"];
                    newrow["field5"] = dr["CTime"];
                    newrow["field6"] = dr["UCname"];
                    dt_Result.Rows.Add(newrow);
                }
            }
            dt_Result.AcceptChanges();
            string json1 = new LayUItableHelper().GetLayUITableHead(arrayList);
            string json2 = JsonHelper.DataTableToJSON(dt_Result);
            string json = "{";
            json += "\"title\":" + json1 + ",";
            json += "\"data\":" + json2;
            json += "}";
            Pagcontext.Response.Write(json);
        }

    }
}