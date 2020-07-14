using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.report.membercard
{
    public partial class memcardreport : ListPage
    {
        repMemCard bll = new repMemCard();
        string bdate = string.Empty;
        string edate = string.Empty;
        string ctype = string.Empty;
        string cracode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateReport").Body;
            if (!IsPostBack)
            {
                SearchToolBarAddJSClick("LinkSearch", "return dataloading();");
                txt_btime.Value = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 07:00:00");
                txt_etime.Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 07:00:00");
            }
        }

        private bool CheckWhere()
        {
            bool Flag = true;

            bdate =txt_btime.Value);
            edate =txt_etime.Value);
            if (bdate.Length == 0 || edate.Length == 0)
            {
                Script(Page, "pcLayerMsg('" + ErrMessage.GetMessageInfoByCode("StoreReports_date").Body + "');");
                Flag = false;
            }

            DateTime btime = StringHelper.StringToDateTime(bdate);
            DateTime etime = StringHelper.StringToDateTime(edate);
            if ((etime - btime).TotalDays > 366)
            {
                Script(Page, "pcLayerMsg('" + ErrMessage.GetMessageInfoByCode("StoreReports_dateerror").Body + "');");
                Flag = false;
            }

            return Flag;
        }

        //获取权限门店
        public string GetTrueStocodes(string hidstocodes)
        {
            string rolestocode = string.Empty;
            string truestocode = string.Empty;
            List<string> list = new List<string>();
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            if (base.LoginedUser.Scope_ID == "2")
            {
                rolestocode = base.LoginedUser.rolstocode.TrimEnd(',');
                if (hidstocodes.Length > 0)
                {
                    string[] s = hidstocodes.Split(',');
                    for (var i = 0; i < s.Length; i++)
                    {
                        list.Add(s[i]);
                    }

                    string[] s1 = rolestocode.Split(',');
                    for (var i = 0; i < s1.Length; i++)
                    {
                        list1.Add(s1[i]);
                    }

                    for (var i = 0; i < list.Count; i++)
                    {
                        if (list1.Contains(list[i]))
                        {
                            list2.Add(list[i]);
                        }
                    }

                    if (list2.Count > 0)
                    {
                        for (var i = 0; i < list2.Count; i++)
                        {
                            truestocode = truestocode + list2[i] + ",";
                        }
                        truestocode = truestocode.TrimEnd(',');
                        return truestocode;
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return rolestocode;
                }
            }
            else if (base.LoginedUser.rolstocode.Length > 0)
            {
                rolestocode = base.LoginedUser.rolstocode.TrimEnd(',');
                if (hidstocodes.Length > 0)
                {
                    string[] s = hidstocodes.Split(',');
                    for (var i = 0; i < s.Length; i++)
                    {
                        list.Add(s[i]);
                    }

                    string[] s1 = rolestocode.Split(',');
                    for (var i = 0; i < s1.Length; i++)
                    {
                        list1.Add(s1[i]);
                    }

                    for (var i = 0; i < list.Count; i++)
                    {
                        if (list1.Contains(list[i]))
                        {
                            list2.Add(list[i]);
                        }
                    }

                    if (list2.Count > 0)
                    {
                        for (var i = 0; i < list2.Count; i++)
                        {
                            truestocode = truestocode + list2[i] + ",";
                        }
                        truestocode = truestocode.TrimEnd(',');
                        return truestocode;
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return rolestocode;
                }
            }
            else
            {
                return hidstocodes.TrimEnd(',');
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected override void BindGridView()
        {
            if (CheckWhere())
            {
                ctype =hidctype.Value);
                cracode =hidcracode.Value);
                DataTable dt = bll.GetMemCardReport(bdate, edate, ctype, cracode, GetTrueStocodes(hidstocode.Value));
                if (dt != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<table id='reportList' freezeColumnNum='2' freezeRowNum='2' class=\"List_tab\" style=\"width: 1200px; border-collapse: collapse;\" cellspacing=\"0\" cellpadding=\"0\" >");
                    sb.AppendLine("<tr class='list_tab_tit' style='font-weight: bold;height:35px;'><td rowspan='2'>会员卡类型</td><td rowspan='2'>会员卡等级</td><td rowspan='2'>发卡数量</td><td rowspan='2'>发卡工本费</td><td rowspan='2'>补卡用卡数量</td><td rowspan='2'>补卡工本费</td><td rowspan='2'>过户用卡数量</td><td rowspan='2'>过户卡工本费</td><td rowspan='2'>换卡数量</td><td rowspan='2'>换卡工本费</td><td rowspan='2'>会员卡报损数量</td><td colspan='2'>小计</td></tr>");
                    sb.AppendLine("<tr class='list_tab_tit' style='font-weight: bold;height:35px;'><td>数量</td><td>金额</td></tr>");
                    int num = 0;//数量
                    decimal money = 0;//金额
                    string ctypename = string.Empty;//会员卡类型
                    string cracodename = string.Empty;//会员卡等级
                    //int csnum = 0;//初始化数量
                    int fknum = 0;//发卡数量
                    decimal fkmoney = 0;//发卡工本金额
                    int bknum = 0;//补卡用卡数量
                    decimal bkmoney = 0;//补卡工本费
                    int ghnum = 0;//过户用卡数量
                    decimal ghmoney = 0;//过户工本费
                    int hknum = 0;//换卡数量
                    decimal hkmoney = 0;//换卡工本费
                    int bsnum = 0;//报损数量
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        num = 0;
                        money = 0;
                        ctypename = dt.Rows[i]["ctypename"].ToString();
                        cracodename = dt.Rows[i]["cracode"].ToString();
                        //csnum = StringHelper.StringToInt(dt.Rows[i]["cscardnum"].ToString());
                        fknum = StringHelper.StringToInt(dt.Rows[i]["fkcardnum"].ToString());
                        fkmoney = StringHelper.StringToDecimal(dt.Rows[i]["fkgbcardmoney"].ToString());
                        bknum = StringHelper.StringToInt(dt.Rows[i]["bkcardnum"].ToString());
                        bkmoney = StringHelper.StringToDecimal(dt.Rows[i]["bkcardmoney"].ToString());
                        ghnum = StringHelper.StringToInt(dt.Rows[i]["ghcardnum"].ToString());
                        ghmoney = StringHelper.StringToDecimal(dt.Rows[i]["ghcardmoney"].ToString());
                        hknum = StringHelper.StringToInt(dt.Rows[i]["hkcardnum"].ToString());
                        hkmoney = StringHelper.StringToDecimal(dt.Rows[i]["hkcardmoney"].ToString());
                        bsnum = StringHelper.StringToInt(dt.Rows[i]["bscardnum"].ToString());
                        //num = csnum + fknum + bknum + ghnum + hknum + bsnum;
                        num = fknum + bknum + ghnum + hknum + bsnum;
                        money = fkmoney + bkmoney + ghmoney + hkmoney;
                        sb.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{12}</td></tr>", ctypename, cracodename, fknum.ToString(), fkmoney.ToString(), bknum.ToString(), bkmoney.ToString(), ghnum.ToString(), ghmoney.ToString(), hknum.ToString(), hkmoney.ToString(), bsnum.ToString(), num.ToString(), money.ToString()));
                    }

                    //csnum = 0;//初始化数量
                    fknum = 0;//发卡数量
                    fkmoney = 0;//发卡工本金额
                    bknum = 0;//补卡用卡数量
                    bkmoney = 0;//补卡工本费
                    ghnum = 0;//过户用卡数量
                    ghmoney = 0;//过户工本费
                    hknum = 0;//换卡数量
                    hkmoney = 0;
                    bsnum = 0;//报损数量
                    sb.AppendLine("<tr><td colspan='2'>合计</td>");
                    //csnum = StringHelper.StringToInt(dt.Compute("sum(cscardnum)", "true").ToString());
                    fknum = StringHelper.StringToInt(dt.Compute("sum(fkcardnum)", "true").ToString());
                    fkmoney = StringHelper.StringToDecimal(dt.Compute("sum(fkgbcardmoney)", "true").ToString());
                    bknum = StringHelper.StringToInt(dt.Compute("sum(bkcardnum)", "true").ToString());
                    bkmoney = StringHelper.StringToDecimal(dt.Compute("sum(bkcardmoney)", "true").ToString());
                    ghnum = StringHelper.StringToInt(dt.Compute("sum(ghcardnum)", "true").ToString());
                    ghmoney = StringHelper.StringToDecimal(dt.Compute("sum(ghcardmoney)", "true").ToString());
                    hknum = StringHelper.StringToInt(dt.Compute("sum(hkcardnum)", "true").ToString());
                    hkmoney = StringHelper.StringToDecimal(dt.Compute("sum(hkcardmoney)", "true").ToString());
                    bsnum = StringHelper.StringToInt(dt.Compute("sum(bscardnum)", "true").ToString());
                    //num = csnum + fknum + bknum + ghnum + hknum + bsnum;
                    num = fknum + bknum + ghnum + hknum + bsnum;
                    money = fkmoney + bkmoney + ghmoney;
                    sb.AppendLine(string.Format("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td></tr>", fknum.ToString(), fkmoney.ToString(), bknum.ToString(), bkmoney.ToString(), ghnum.ToString(), ghmoney.ToString(), hknum.ToString(), hkmoney.ToString(), bsnum.ToString(), num.ToString(), money.ToString()));
                    sb.AppendLine("</table>");
                    lstData.InnerHtml = sb.ToString();
                    Script(this.Page, "tbfreeze();closeloading();");
                }
            }
            Script(this.Page, "closeloading();");
        }

        /// <summary>
        /// ToolBar所有按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToolBar1_Click(object sender, ToolBarEventArgs e)
        {
            DataTable dt = new DataTable();
            if (e != null)
            {
                string Selected = string.Empty;
                switch (((ToolBarEventArgs)(e)).btntype.ToLower())
                {
                    //搜索事件代码
                    case "search":
                        BindGridView();
                        break;
                    //刷新事件代码
                    case "refresh":
                        BindGridView();
                        break;
                    //导出事件代码
                    case "export":
                        string sdate = txt_btime.Value + "至" + txt_etime.Value;
                        string stocode = string.Empty;
                        if (txt_stoname.Value.Length > 0)
                        {
                            stocode = txt_stoname.Value;
                        }
                        else
                        {
                            stocode = "全部";
                        }

                        string cardtype = "全部";
                        if (hidctypename.Value.Length > 0)
                        {
                            cardtype = hidctypename.Value;
                        }

                        string cardlevel = "全部";
                        if (hidlevelname.Value.Length > 0)
                        {
                            cardlevel = hidlevelname.Value;
                        }

                        //增加表头及条件
                        string html = string.Format("<table class='List_tab' style='weight:100%;'><tr><td style='font-size:15px;font-weight:bold;text-align:center;height:30px;line-height:30px;' colspan='13'> 会员卡报表</td></tr><tr><td style='font-size:15px;font-weight:bold;text-align:left;height:30px;line-height:30px;' colspan='13'>开始时间：{0}</td></tr><tr><td style='font-size:15px;font-weight:bold;text-align:left;height:30px;line-height:30px;' colspan='13'>门店：{1}</td></tr><tr><td style='font-size:15px;font-weight:bold;text-align:left;height:30px;line-height:30px;' colspan='13'>会员卡类型：{2}</td></tr><tr><td style='font-size:15px;font-weight:bold;text-align:left;height:30px;line-height:30px;' colspan='13'>会员卡等级：{3}</td></tr><tr><td colspan='13'></td></tr></table>", sdate, stocode, cardtype, cardlevel) + lstData.InnerHtml;

                        ExportHtml(html);
                        break;
                }
            }
        }

        public void ExportHtml(string html)
        {
            //string html = lstData.InnerHtml;
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition",
                "attachment; filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            Response.Write("<head>");
            Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            Response.Write("<!--[if gte mso 9]><xml>");
            Response.Write("<x:ExcelWorkbook>");
            Response.Write("<x:ExcelWorksheets>");
            Response.Write("<x:ExcelWorksheet>");
            Response.Write("<x:Name>Sheet1</x:Name>");
            Response.Write("<x:WorksheetOptions>");
            Response.Write("<x:Print>");
            Response.Write("<x:ValidPrinterInfo/>");
            Response.Write("</x:Print>");
            Response.Write("</x:WorksheetOptions>");
            Response.Write("</x:ExcelWorksheet>");
            Response.Write("</x:ExcelWorksheets>");
            Response.Write("</x:ExcelWorkbook>");
            Response.Write("</xml>");
            Response.Write("<![endif]--> ");
            Response.Write(html);//HTML
            Response.Flush();
            Response.End();
        }
    }
}