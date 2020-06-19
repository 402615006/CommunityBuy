using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class R_DataSourceList : Common.ListPage
    {
        bllR_DataSource bll = new bllR_DataSource();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                BindStoInfo();
                BindGridView();
            }
        }

        /// <summary>
        /// 绑定门店信息
        /// </summary>
        public void BindStoInfo()
        {
            DataTable dt = new bllPaging().GetDataTableInfoBySQL("select stocode,cname from store where stocode not in (select stocode from store where isfood='1' and pstocode='') and status=1 " + GetAuthoritywhere("stocode") + ";");
            Helper.BindDropDownListForSearch(ddl_stocode, dt, "cname", "stocode", 2);
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected override void BindGridView()
        {
            int recount;
            int pagenums;
            string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
            if (HidSortExpression.Value == "")
            {
                order = " dday desc";
            }
            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recount, out pagenums);

            if (dt != null)
            {
                decimal sumdisnum = 0, sumdisaccount = 0;
                dt.Columns.Add("dtypename", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["dtypename"] = Helper.GetEnumNameByValue(typeof(SystemEnum.DataType), dt.Rows[i]["dtype"].ToString());
                    sumdisnum += StringHelper.StringToDecimal(dt.Rows[i]["disnum"].ToString());
                    sumdisaccount += StringHelper.StringToDecimal(dt.Rows[i]["disaccount"].ToString());
                }

                DataRow dr = dt.NewRow();
                dr["disname"] = "合计:";
                dr["disnum"] = sumdisnum;
                dr["disaccount"] = sumdisaccount;
                dt.Rows.Add(dr);
                gv_list.DataSource = dt;
                gv_list.DataBind();

                gv_list.Items[gv_list.Items.Count - 1].Cells[7].ColumnSpan = 8;
                for (int i = 0; i < 7; i++)
                {
                    gv_list.Items[gv_list.Items.Count - 1].Cells[i].Visible = false;
                }
                gv_list.Items[gv_list.Items.Count - 1].Font.Bold = true;
                anp_top.RecordCount = recount;
            }
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
                        GotoSearch();
                        break;
                    //刷新事件代码
                    case "refresh":
                        BindGridView();
                        break;
                    //重新计算
                    case "recount":
                        Script(this.Page, "ShowOpenpage('重新计算数据', '/store/R_DataSourceEdit.aspx', '600px', '400px', false, true);");
                        break;

                }
            }
        }

        /// <summary>
        /// 搜索按钮拼接Where条件
        /// </summary>
        public void GotoSearch()
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" where 1=1 ");
            //拼接Where条件
            Where.Append(GetAuthoritywhere("stocode"));
            string dtype = ddl_dtype.SelectedValue;
            if (!string.IsNullOrEmpty(dtype))
            {
                Where.Append(" and dtype='" + dtype + "'");
            }
            string datacode = Helper.ReplaceString(HidProject.Value);
            if (!string.IsNullOrEmpty(datacode))
            {
                Where.Append(" and datacode='" + datacode + "'");
            }
            //日期查询条件
            string startdate = Helper.ReplaceString(txt_startdate.Value);
            string enddate = Helper.ReplaceString(txt_enddate.Value);
            if (startdate.Length > 0)
            {
                Where.Append(" and dday>='" + startdate + "'");
            }
            if (enddate.Length > 0)
            {
                enddate = StringHelper.StringToDateTime(txt_enddate.Value).AddDays(1).ToString();
                Where.Append(" and dday<'" + enddate + "'");
            }
            //门店
            string stocode = ddl_stocode.SelectedValue;
            if (!string.IsNullOrEmpty(stocode))
            {
                Where.Append(" and stocode='" + stocode + "'");
            }
            //菜品
            string dishname = Helper.ReplaceString(txt_dishes.Value);
            if (!string.IsNullOrEmpty(dishname))
            {
                Where.Append(" and dbo.[fnGetDisheNameByDiscode](discode,stocode) like '%" + dishname + "%'");
            }
            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }
    }
}