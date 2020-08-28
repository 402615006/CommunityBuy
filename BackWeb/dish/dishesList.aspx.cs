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
        bllTB_Dish bll = new bllTB_Dish();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = "商品列表";
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

            //一级菜品类别
            DataTable dt2 = new bllTB_DishType().GetPagingListInfo("0", "0", int.MaxValue, 1, "[tstatus]='1' and len(isnull(PKKCode,''))=0", "", out recount, out pagenums);
            ddl_selDisheType.DataTextField = "typename";
            ddl_selDisheType.DataValueField = "pkcode";
            ddl_selDisheType.DataSource = dt2;
            ddl_selDisheType.DataBind();
            ddl_selDisheType.Items.Add(itemDefault);
        }

        protected override void BindGridView()
        {
            int recount;
            int pagenums;
            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, "", out recount, out pagenums);
           
            if (dt != null)
            {
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

                            Selected = GetSelectStr(gv_list);
                           
                            string[] arrSel = Selected.Split(',');
                            if (arrSel.Length != 1)
                            {
                                sp_showmes.InnerText = "请选择一项进行操作";
                            }
                            bll.Delete("0", "0", arrSel[0]);
                            sp_showmes.InnerText = bll.oResult.Msg;
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请至少选择一项进行操作";
                        }

                        bll.UpdateStatus("", "0",Selected,"1");
                    if (ShowResult(bll.oResult.Code,bll.oResult.Msg, sp_showmes))
                    {
                        BindGridView();
                    }
                        break;
                    //无效
                    case "invalid":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请至少选择一项进行操作";
                        }

                        bll.UpdateStatus("", "0", Selected, "0");
                        if (ShowResult(bll.oResult.Code, bll.oResult.Msg, sp_showmes))
                        {
                            BindGridView();
                        }
                        break;
                    //导出事件代码
                    case "export":
                        //int recount;
                        //int pagenums;
                        //string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                        //if (HidSortExpression.Value == "")
                        //{
                        //    order = " stocode asc";
                        //}
                        //dt = bll.GetPagingListInfo3("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, string.Empty, ddl_stocode.SelectedValue, 0, out recount, out pagenums);
                        //if (dt != null)
                        //{
                        //        foreach (DataRow dr in dt.Rows)
                        //        {
                        //            if (dr["disid"].ToString().Length > 0)
                        //            {
                        //                dr["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dr["status"].ToString());
                        //            }
                        //            if (dr["disname"].ToString().Length > 0 && dr["iscombo"].ToString() == "0")
                        //            {
                        //                dr["disname"] = dr["disname"].ToString() + "(" + dr["unit"].ToString() + ")";
                        //            }
                        //            dr["iscanmodifyprice"] = GetIsStatus(dr["iscanmodifyprice"].ToString());
                        //            dr["isneedweigh"] = GetIsStatus(dr["isneedweigh"].ToString());
                        //            dr["isneedmethod"] = GetIsStatus(dr["isneedmethod"].ToString());
                        //            dr["iscaninventory"] = GetIsStatus(dr["iscaninventory"].ToString());
                        //            dr["iscancustom"] = GetIsStatus(dr["iscancustom"].ToString());
                        //            dr["isallowmemberprice"] = GetIsStatus(dr["isallowmemberprice"].ToString());
                        //            dr["isattachcalculate"] = GetIsStatus(dr["isattachcalculate"].ToString());
                        //            dr["isclipcoupons"] = GetIsStatus(dr["isclipcoupons"].ToString());
                        //            dr["iscandeposit"] = GetIsStatus(dr["iscandeposit"].ToString());
                        //            dr["isnonoperating"] = GetIsStatus(dr["isnonoperating"].ToString());
                        //        }
                        //}
                        //string fileName = string.Format(ErrMessage.GetMessageInfoByCode("Dishes_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        //string strColumnName = ErrMessage.GetMessageInfoByCode("Dishes_Export").Body;
                        //string ColumnCode = "melname,departname,kitname,disname,quickcode,customcode,distypename,realprice,costprice,finname,unit,realmemberprice,iscanmodifyprice,isneedweigh,isneedmethod,iscaninventory,iscancustom,isallowmemberprice,isattachcalculate,isclipcoupons,iscandeposit,isnonoperating,statusname,ctime";
                        //ExcelsHelp.ExportExcelFileB(dt, fileName, strColumnName.Split(','), ColumnCode.Split(','));
                        break;
                }
            }
        }

        public void GotoSearch()
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" where 1=1");
            //拼接Where条件

            //菜品分类
            string TB_DishTypetwo = this.ddl_sel_dishetypetwo.SelectedValue;
            if (!string.IsNullOrEmpty(TB_DishTypetwo) && TB_DishTypetwo != "-1")
            {
                Where.Append(" and TypeCode ='" + TB_DishTypetwo + "'");
            }
            else
            {
                string TB_DishType =this.ddl_selDisheType.SelectedValue;
                if (!string.IsNullOrEmpty(TB_DishType) && TB_DishType != "-1")
                {
                    Where.Append(" and TypeCode in (SELECT PKCode FROM dbo.TB_DishType WHERE PPKCode='" + TB_DishType + "')");
                }
            }
            string status =this.ddl_status.SelectedValue;
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                Where.Append(" and status='" + status + "'");

            }
            string disname = txt_disname.Value;
            if (!string.IsNullOrEmpty(disname))
            {
                Where.Append(" and (disname like '%" + disname + "%' or quickcode like '%" + disname + "%' or customcode like '%" + disname + "%')");
            }

            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }

        protected void ddl_selDisheType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_selDisheType.SelectedValue != "-1")
            {
                int recount;
                int pagenums;
                System.Web.UI.WebControls.ListItem itemDefault = new System.Web.UI.WebControls.ListItem();
                itemDefault.Text = "全部";
                itemDefault.Value = "-1";
                itemDefault.Selected = true;
                DataTable dt2 = new bllTB_DishType().GetPagingListInfo("0", "0", int.MaxValue, 1, "where [tstatus]='1' and PKKCode='" + ddl_selDisheType.SelectedValue + "'", "typename desc", out recount, out pagenums);
                ddl_sel_dishetypetwo.DataTextField = "typename";
                ddl_sel_dishetypetwo.DataValueField = "pkcode";
                ddl_sel_dishetypetwo.DataSource = dt2;
                ddl_sel_dishetypetwo.DataBind();
                ddl_sel_dishetypetwo.Items.Add(itemDefault);
            }
        }
    }
}