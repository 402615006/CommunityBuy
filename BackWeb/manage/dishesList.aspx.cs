using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.BackWeb.UserControls;

namespace CommunityBuy.BackWeb
{
    public partial class dishesList : Common.ListPage
    {
        blldishes bll = new blldishes();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                BindBusInfo();
                GotoSearch();
                BindGridView();
            }
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        public void BindBusInfo()
        {
            System.Web.UI.WebControls.ListItem itemDefault = new System.Web.UI.WebControls.ListItem();
            itemDefault.Text = "--请选择--";
            itemDefault.Value = "-1";
            itemDefault.Selected = true;
            int recount;
            int pagenums;
            //部门下拉框
            string strWhere = "where status='1'";
            if (LoginedUser.UserInfo.rolstocode.Length > 0)
            {
                strWhere += " and stocode in('" + LoginedUser.UserInfo.rolstocode.Replace(",", "','") + "')";
            }
            DataTable dtStore = new bllStore().GetPagingListInfo("0", "0", int.MaxValue, 1, strWhere, "cname desc", out recount, out pagenums);
            ddl_stocode.DataTextField = "cname";
            ddl_stocode.DataValueField = "stocode";
            ddl_stocode.DataSource = dtStore;
            ddl_stocode.DataBind();
            if (dtStore != null && dtStore.Rows.Count > 0)
            {
                ddl_stocode.SelectedIndex = 0;
            }

            //菜谱下拉框
            DataTable dt1 = new bllmeal().GetPagingListInfo("0", "0", int.MaxValue, 1, "where [status]='1' and stocode='" + ddl_stocode.SelectedValue + "'", "melname desc", out recount, out pagenums);
            ddl_Menu_list.DataTextField = "melname";
            ddl_Menu_list.DataValueField = "melcode";
            ddl_Menu_list.DataSource = dt1;
            ddl_Menu_list.DataBind();
            ddl_Menu_list.Items.Add(itemDefault);

            //一级菜品类别
            DataTable dt2 = new bllDisheType().GetPagingListInfo("0", "0", int.MaxValue, 1, "[status]='1' and len(isnull(pdistypecode,''))=0 and stocode='"+ ddl_stocode.SelectedValue + "'", "distypename desc", out recount, out pagenums);
            ddl_selDisheType.DataTextField = "distypename";
            ddl_selDisheType.DataValueField = "distypecode";
            ddl_selDisheType.DataSource = dt2;
            ddl_selDisheType.DataBind();
            ddl_selDisheType.Items.Add(itemDefault);

            //绑定财务类别
            DataTable dt3 = new bllFinanceType().GetPagingListInfo("0", "0", int.MaxValue, 1, "[status]='1'", "finname desc", out recount, out pagenums);
            ddl_selfincode.DataTextField = "finname";
            ddl_selfincode.DataValueField = "fincode";
            ddl_selfincode.DataSource = dt3;
            ddl_selfincode.DataBind();
            ddl_selfincode.Items.Add(itemDefault);
        }

        protected override void BindGridView()
        {
            int recount;
            int pagenums;
            string order = " ctime desc";
            DataTable dt = bll.GetPagingListInfo3("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, string.Empty, ddl_stocode.SelectedValue, 0, out recount, out pagenums);
           
            if (dt != null)
            {
                DataTable dt1 = new DataTable();
                string discodes = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    discodes += dr["discode"].ToString() + ",";
                }
                discodes = discodes.Trim(',');
                string sql1 = "select discode,Convert(decimal(18, 6), isnull(SUM(isnull(useamount, 0)), 0)) as cb from(select A.buscode, A.stocode, A.discode, A.matcode, useamount = convert(decimal(18, 6), A.mlnum / isnull(sto.conversionratio, 1)) * C.acost, sto.matunitcode from DishesMate A inner join (select matcode, conversionratio, matunitcode from catering_stock.dbo.StockMateUnits where stocode = '12' and iscommonunit = '1') as sto on A.matcode = sto.matcode left join(select acost, stocode, matcode, row_number() over (partition by matcode order by djdate desc) as rownum from catering_stock.dbo.StockInOutDetail where stocode = '" + ddl_stocode.SelectedValue + "') as C  on A.stocode = C.stocode and A.matcode = C.matcode and C.rownum = 1 where A.discode in (select col from dbo.fn_StringSplit('" + discodes + "', ',')) and A.stocode = '" + ddl_stocode.SelectedValue + "')as X group by X.discode";
                DataSet ds = new blldishes().getDataSetBySql(sql1);
                dt1 = ds.Tables[0];//成本金额

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["disid"].ToString().Length > 0)
                    {
                        dr["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dr["status"].ToString());
                    }
                    if (dr["disname"].ToString().Length > 0 && dr["iscombo"].ToString() == "0")
                    {
                        dr["disname"] = dr["disname"].ToString() + "(" + dr["unit"].ToString() + ")";
                    }
                    try
                    {
                        DataRow[] drs = dt1.Select("discode='" + dr["discode"].ToString() + "'");
                        if(drs.Length>0)
                        {
                            string mlv = (((double.Parse(dr["realprice"].ToString()) - double.Parse(drs[0]["cb"].ToString())) / double.Parse(dr["realprice"].ToString())) * 100).ToString("f2") + "%";
                            dr["mlvprice"] = (((double.Parse(dr["realprice"].ToString()) - double.Parse(drs[0]["cb"].ToString())) / double.Parse(dr["realprice"].ToString())) * 100).ToString("f2") + "%";
                        }
                        else
                        {
                            dr["mlvprice"] = "100.00%";
                        }
                    }
                    catch (Exception)
                    {
                    }
                    dr["iscanmodifyprice"] = GetIsStatus(dr["iscanmodifyprice"].ToString());
                    dr["isneedweigh"] = GetIsStatus(dr["isneedweigh"].ToString());
                    dr["isneedmethod"] = GetIsStatus(dr["isneedmethod"].ToString());
                    dr["iscaninventory"] = GetIsStatus(dr["iscaninventory"].ToString());
                    dr["iscancustom"] = GetIsStatus(dr["iscancustom"].ToString());
                    dr["isallowmemberprice"] = GetIsStatus(dr["isallowmemberprice"].ToString());
                    dr["isattachcalculate"] = GetIsStatus(dr["isattachcalculate"].ToString());
                    dr["isclipcoupons"] = GetIsStatus(dr["isclipcoupons"].ToString());
                    dr["iscandeposit"] = GetIsStatus(dr["iscandeposit"].ToString());
                    dr["isnonoperating"] = GetIsStatus(dr["isnonoperating"].ToString());
                }
                gv_list.DataSource = dt;
                gv_list.DataBind();
                anp_top.RecordCount = recount;
            }
        }


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
                        GotoSearch();
                        break;
                    //刷新事件代码
                    case "refresh":
                        Response.Redirect(Request.Url.ToString());
                        break;
                    case "delete":
                        {
                            //日志信息
                            logentity.module = ErrMessage.GetMessageInfoByCode("Dishes_Menu").Body;
                            logentity.pageurl = "dishesList.aspx";
                            logentity.otype = SystemEnum.LogOperateType.Delete;
                            logentity.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());
                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_005").Body;
                            }
                            string[] arrSel = Selected.Split(',');
                            for (int i = 0; i < arrSel.Length; i++)
                            {
                                logentity.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("Store_961").Body, LoginedUser.UserInfo.cname, arrSel[i]);
                                bll.Delete("0", "0", arrSel[i], logentity);
                            }
                            sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_001").Body;
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_005").Body;
                        }
                        else
                        {
                            dt = bll.UpdateStatus("", "0", Selected, SystemEnum.Status.Valid.ToString("D"));
                            if (ShowResult(dt, sp_showmes))
                            {
                                BindGridView();
                            }
                        }
                        break;
                    //无效
                    case "invalid":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请选择要操作记录！";
                        }
                        else
                        {
                            dt = bll.UpdateStatus("", "0", Selected, SystemEnum.Status.Invalid.ToString("D"));
                            if (ShowResult(dt, sp_showmes))
                            {
                                BindGridView();
                            }
                        }
                        break;
                    //导出事件代码
                    case "export":
                        int recount;
                        int pagenums;
                        string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                        if (HidSortExpression.Value == "")
                        {
                            order = " stocode asc";
                        }
                        dt = bll.GetPagingListInfo3("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, string.Empty, ddl_stocode.SelectedValue, 0, out recount, out pagenums);
                        if (dt != null)
                        {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["disid"].ToString().Length > 0)
                                    {
                                        dr["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dr["status"].ToString());
                                    }
                                    if (dr["disname"].ToString().Length > 0 && dr["iscombo"].ToString() == "0")
                                    {
                                        dr["disname"] = dr["disname"].ToString() + "(" + dr["unit"].ToString() + ")";
                                    }
                                    dr["iscanmodifyprice"] = GetIsStatus(dr["iscanmodifyprice"].ToString());
                                    dr["isneedweigh"] = GetIsStatus(dr["isneedweigh"].ToString());
                                    dr["isneedmethod"] = GetIsStatus(dr["isneedmethod"].ToString());
                                    dr["iscaninventory"] = GetIsStatus(dr["iscaninventory"].ToString());
                                    dr["iscancustom"] = GetIsStatus(dr["iscancustom"].ToString());
                                    dr["isallowmemberprice"] = GetIsStatus(dr["isallowmemberprice"].ToString());
                                    dr["isattachcalculate"] = GetIsStatus(dr["isattachcalculate"].ToString());
                                    dr["isclipcoupons"] = GetIsStatus(dr["isclipcoupons"].ToString());
                                    dr["iscandeposit"] = GetIsStatus(dr["iscandeposit"].ToString());
                                    dr["isnonoperating"] = GetIsStatus(dr["isnonoperating"].ToString());
                                }
                        }
                        string fileName = string.Format(ErrMessage.GetMessageInfoByCode("Dishes_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        string strColumnName = ErrMessage.GetMessageInfoByCode("Dishes_Export").Body;
                        string ColumnCode = "melname,departname,kitname,disname,quickcode,customcode,distypename,realprice,costprice,finname,unit,realmemberprice,iscanmodifyprice,isneedweigh,isneedmethod,iscaninventory,iscancustom,isallowmemberprice,isattachcalculate,isclipcoupons,iscandeposit,isnonoperating,statusname,ctime";
                        ExcelsHelp.ExportExcelFileB(dt, fileName, strColumnName.Split(','), ColumnCode.Split(','));
                        break;
                    case "cardexport":
                        GotoSearchMates();
                        Selected = discodes.Value;
                        if (Selected.Length == 0)
                        {
                            dt = bll.LSgetDishesMates(HidWhere1.Value + " and t1.stocode='" + ddl_stocode.SelectedValue + "' and t.stocode='" + ddl_stocode.SelectedValue + "'");
                        }
                        else
                        {
                            string[] arrSel = Selected.Split(',');
                            string lsids = string.Empty;
                            HidWhere1.Value = HidWhere1.Value + " and t.discode in(";
                            for (int i = 0; i < arrSel.Length; i++)
                            {
                                lsids += "'" + arrSel[i] + "',";
                            }
                            lsids = lsids.TrimEnd(',');
                            HidWhere1.Value = HidWhere1.Value +lsids +")";
                            dt = bll.LSgetDishesMates(HidWhere1.Value+" and t1.stocode='"+ddl_stocode.SelectedValue+ "' and t.stocode='" + ddl_stocode.SelectedValue + "'");
                        }

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            fileName = string.Format(ErrMessage.GetMessageInfoByCode("DishesMate_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                            strColumnName = ErrMessage.GetMessageInfoByCode("DishesMate_Export").Body;
                            ColumnCode = "disname,discode,matname,matcode,jlnum,mlnum,mattype,remark,matunitname";
                            ExcelsHelp.ExportExcelFileB(dt, fileName, strColumnName.Split(','), ColumnCode.Split(','));
                        }
                        else
                        {
                            sp_showmes.InnerText = "没有找到该菜品成本卡信息！";
                        }
                        break;
                    case "downtemp":
                        DataSet dsData = new DataSet();
                        DataTable dt1 = new DataTable();
                        dt1.TableName = "dishes";
                        dsData.Tables.Add(dt1);
                        DataSet ds2 = bll.LSgetDishesMates1(ddl_stocode.SelectedValue);
                        DataTable dt2 = new DataTable();
                        dt2.TableName = "Mate";
                        dt2 = ds2.Tables[0].Copy();
                        dsData.Tables.Add(dt2);
                        DataTable dt3 = new DataTable();
                        dt3.TableName = "dishess";
                        dt3 = ds2.Tables[1].Copy();
                        dsData.Tables.Add(dt3);
                        string tempFile = HttpContext.Current.Request.PhysicalApplicationPath + "\\Template\\成本卡模板.xlsx";
                        AsponseHelper asponse = new AsponseHelper();
                        fileName = "成本卡导入模板";
                        asponse.ExportOPenfileByFilePath(fileName, tempFile, dsData, new DataTable());
                        break;
                }
            }
        }

        public void GotoSearch()
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" where 1=1 and iscombo='0' ");
            //拼接Where条件
            //部门
            string strstocode = Helper.ReplaceString(ddl_stocode.SelectedValue);
            if (strstocode != "-1")
            {
                Where.Append(" and stocode = '" + strstocode + "' ");
            }
            //回收站
            if (ddl_searchrecyble.SelectedValue=="1")
            {
                Where.Append(" and isdelete='1'");
            }
            else
            {
                Where.Append(" and isdelete='0'");
            }
            //菜谱
            string strcname = Helper.ReplaceString(ddl_Menu_list.SelectedValue);
            if (strcname != "-1")
            {
                Where.Append(" and (melcode ='" + strcname + "' or discode in(SELECT discode FROM dbo.DishesMeal WHERE melcode='" + strcname + "')) and stocode='" + strstocode + "'");
            }
            //菜品分类
            string DisheTypetwo = Helper.ReplaceString(this.ddl_sel_dishetypetwo.SelectedValue);
            if (!string.IsNullOrEmpty(DisheTypetwo) && DisheTypetwo != "-1")
            {
                Where.Append(" and distypecode ='" + DisheTypetwo + "'");
            }
            else
            {
                string DisheType = Helper.ReplaceString(this.ddl_selDisheType.SelectedValue);
                if (!string.IsNullOrEmpty(DisheType) && DisheType != "-1")
                {
                    Where.Append(" and distypecode in (SELECT distypecode FROM dbo.DisheType WHERE pdistypecode='" + DisheType + "')");
                }
            }
            //财务类别
            string selfincode = Helper.ReplaceString(this.ddl_selfincode.SelectedValue);
            if (!string.IsNullOrEmpty(selfincode) && selfincode != "-1")
            {
                Where.Append(" and fincode='" + selfincode + "'");

            }
            string status = Helper.ReplaceString(this.ddl_status.SelectedValue);
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                Where.Append(" and status='" + status + "'");

            }
            string disname = Helper.ReplaceString(txt_disname.Value);
            if (!string.IsNullOrEmpty(disname))
            {
                Where.Append(" and (disname like '%" + disname + "%' or quickcode like '%" + disname + "%' or customcode like '%" + disname + "%')");
            }
            if (ddl_cbk.SelectedValue != "-1")
            {
                if (ddl_cbk.SelectedValue == "1")
                {
                    Where.Append(" and (select count(0) from [dbo].[DishesMate] t where t.discode=t1.discode and t.stocode=t1.stocode)>0");
                }
                else
                {
                    Where.Append(" and (select count(0) from [dbo].[DishesMate] t where t.discode=t1.discode and t.stocode=t1.stocode)=0");
                }
            }
            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }

        public void GotoSearchMates()
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" where 1=1 and t.iscombo='0' ");
            //拼接Where条件
            //部门
            string strstocode = Helper.ReplaceString(ddl_stocode.SelectedValue);
            if (strstocode != "-1")
            {
                Where.Append(" and t.stocode = '" + strstocode + "' ");
            }
            //回收站
            if (ddl_searchrecyble.SelectedValue == "1")
            {
                Where.Append(" and t.isdelete='1'");
            }
            else
            {
                Where.Append(" and t.isdelete='0'");
            }
            //菜谱
            string strcname = Helper.ReplaceString(ddl_Menu_list.SelectedValue);
            if (strcname != "-1")
            {
                Where.Append(" and (t.melcode ='" + strcname + "' or t.discode in(SELECT discode FROM dbo.DishesMeal WHERE melcode='" + strcname + "')) and t.stocode='" + strstocode + "'");
            }
            //菜品分类
            string DisheTypetwo = Helper.ReplaceString(this.ddl_sel_dishetypetwo.SelectedValue);
            if (!string.IsNullOrEmpty(DisheTypetwo) && DisheTypetwo != "-1")
            {
                Where.Append(" and t.distypecode ='" + DisheTypetwo + "'");
            }
            else
            {
                string DisheType = Helper.ReplaceString(this.ddl_selDisheType.SelectedValue);
                if (!string.IsNullOrEmpty(DisheType) && DisheType != "-1")
                {
                    Where.Append(" and t.distypecode in (SELECT distypecode FROM dbo.DisheType WHERE pdistypecode='" + DisheType + "')");
                }
            }
            //财务类别
            string selfincode = Helper.ReplaceString(this.ddl_selfincode.SelectedValue);
            if (!string.IsNullOrEmpty(selfincode) && selfincode != "-1")
            {
                Where.Append(" and t.fincode='" + selfincode + "'");

            }
            string status = Helper.ReplaceString(this.ddl_status.SelectedValue);
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                Where.Append(" and t.status='" + status + "'");

            }
            string disname = Helper.ReplaceString(txt_disname.Value);
            if (!string.IsNullOrEmpty(disname))
            {
                Where.Append(" and (t.disname like '%" + disname + "%' or t.quickcode like '%" + disname + "%' or t.customcode like '%" + disname + "%')");
            }
            HidWhere1.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }

        protected void ddl_stocode_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.ListItem itemDefault = new System.Web.UI.WebControls.ListItem();
            itemDefault.Text = "全部";
            itemDefault.Value = "-1";
            itemDefault.Selected = true;
            int recount;
            int pagenums;
            //菜谱下拉框
            DataTable dt1 = new bllmeal().GetPagingListInfo("0", "0", int.MaxValue, 1, "where [status]='1' and stocode='" + ddl_stocode.SelectedValue + "'", "melname desc", out recount, out pagenums);
            ddl_Menu_list.DataTextField = "melname";
            ddl_Menu_list.DataValueField = "melcode";
            ddl_Menu_list.DataSource = dt1;
            ddl_Menu_list.DataBind();
            ddl_Menu_list.Items.Add(itemDefault);

            //一级菜品类别
            DataTable dt2 = new bllDisheType().GetPagingListInfo("0", "0", int.MaxValue, 1, "where [status]='1' and len(isnull(pdistypecode,''))=0 and stocode='" + ddl_stocode.SelectedValue + "'", "distypename desc", out recount, out pagenums);
            ddl_selDisheType.DataTextField = "distypename";
            ddl_selDisheType.DataValueField = "distypecode";
            ddl_selDisheType.DataSource = dt2;
            ddl_selDisheType.DataBind();
            ddl_selDisheType.Items.Add(itemDefault);
        }

        protected void ddl_selDisheType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int recount;
            int pagenums;
            System.Web.UI.WebControls.ListItem itemDefault = new System.Web.UI.WebControls.ListItem();
            itemDefault.Text = "全部";
            itemDefault.Value = "-1";
            itemDefault.Selected = true;
            DataTable dt2 = new bllDisheType().GetPagingListInfo("0", "0", int.MaxValue, 1, "where [status]='1' and pdistypecode='" + ddl_selDisheType.SelectedValue + "' and stocode='" + ddl_stocode.SelectedValue + "'", "distypename desc", out recount, out pagenums);
            ddl_sel_dishetypetwo.DataTextField = "distypename";
            ddl_sel_dishetypetwo.DataValueField = "distypecode";
            ddl_sel_dishetypetwo.DataSource = dt2;
            ddl_sel_dishetypetwo.DataBind();
            ddl_sel_dishetypetwo.Items.Add(itemDefault);
        }

        private string GetIsStatus(string status)
        {
            string result = "否";
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "0")
                {
                    result = "否";
                }
                else
                {
                    result = "是";
                }
            }
            return result;
        }
    }
}