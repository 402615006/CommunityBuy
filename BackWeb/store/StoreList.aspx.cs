using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class StoreList : Common.ListPage
    {
        bllStore bll = new bllStore();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = "列表";
            if (!IsPostBack)
            {
                BindBusInfo();
                this.ddl_status.SelectedValue = "1";
                GotoSearch();
            }
        }

        public void BindBusInfo()
        {
            //DataTable dt = new bllPaging().GetDataTableInfoBySQL("select buscode,cname from Business where status='1';");

            //Helper.BindDropDownListForSearch(ddl_businfo, dt, "cname", "buscode", 2);
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
                order = " stocode asc";
            }
            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recount, out pagenums);

            if (dt != null)
            {
                dt.Columns.Add("statusname", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["statusname"] = dt.Rows[i]["status"].ToString() == "1" ? "有效" : "无效";
                }
                gv_list.DataSource = dt;
                gv_list.DataBind();
                anp_top.RecordCount = recount;
            }
            DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from [dbo].[provinces]");
            BindDropDownListInfo(this.ddl_provinceid, dtdict, "province", "provinceid",1);
            ddl_provinceid_SelectedIndexChanged(null, null);
        }

        protected void ddl_provinceid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddl_provinceid.SelectedValue))
            {
                string fiter = " parentid =" + this.ddl_provinceid.SelectedValue;
                DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from [dbo].[provinces] where"+ fiter);
                BindDropDownListInfo(this.ddl_cityid, dtdict, "city", "cityid", 2);
                ddl_cityid_SelectedIndexChanged(null, null);
            }
        }

        protected void ddl_cityid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddl_cityid.SelectedValue))
            {
                string fiter = " parentid =" + this.ddl_cityid.SelectedValue;
                DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from [dbo].[provinces] where" + fiter);
                BindDropDownListInfo(this.ddl_areaid, dtdict, "area", "areaid", 2);
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
                    case "delete":
                        {
                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = "请至少选择一项";
                            }
                            string[] arrSel = Selected.Split(',');
                            for (int i = 0; i < arrSel.Length; i++)
                            {
                                bll.Delete("0", "0", arrSel[i]);
                            }
                            sp_showmes.InnerText = bll.oResult.Msg;
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = "请至少选择一项";
                        }
                        else
                        {
                            bll.UpdateStatus("", "0", Selected,"1");


                            if (ShowResult(bll.oResult.Code,bll.oResult.Msg, sp_showmes))
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
                            bll.UpdateStatus("", "0", Selected,"0");
                            if (ShowResult(bll.oResult.Code, bll.oResult.Msg, sp_showmes))
                            {
                                BindGridView();
                            }
                        }
                        break;
                    //导出事件代码
                    case "export":
                        //int recount;
                        //int pagenums;
                        //string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                        //dt = bll.GetPagingListInfo("", "0", 50000, 1, HidWhere.Value, order, out recount, out pagenums);
                        //if (dt != null)
                        //{
                        //    dt.Columns.Add("statusname", typeof(string));
                        //    for (int i = 0; i < dt.Rows.Count; i++)
                        //    {
                        //        dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                        //    }
                        //}
                        //string fileName = string.Format(ErrMessage.GetMessageInfoByCode("Store_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        //string strColumnName = ErrMessage.GetMessageInfoByCode("store_Export").Body;
                        //string ColumnCode = "stocode,cname,sname,bcode,areaname,stoprincipal,stoprincipaltel,tel,statusname";
                        //ExcelsHelp.ExportExcelFileB(dt, fileName, strColumnName.Split(','), ColumnCode.Split(','));
                        //break;
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

            string strstocode =txt_stocode.Value;
            if (strstocode.Length > 0)
            {
                Where.Append(" and stocode like '%" + strstocode + "%' ");

            }
            string strcname =txt_cname.Value;
            if (strcname.Length > 0)
            {
                Where.Append(" and cname like '%" + strcname + "%' ");

            }
            string strprovinceid =this.ddl_provinceid.SelectedValue;
            if (strprovinceid.Length > 0)
            {
                Where.Append(" and provinceid= '" + strprovinceid + "'");

            }
            string strcityid =this.ddl_cityid.SelectedValue;
            if (strcityid.Length > 0)
            {
                Where.Append(" and cityid= '" + strcityid + "'");

            }
            string strareaid =this.ddl_areaid.SelectedValue;
            if (strareaid.Length > 0)
            {
                Where.Append(" and areaid= '" + strareaid + "'");

            }
            string strstoprincipal =txt_stoprincipal.Value;
            if (strstoprincipal.Length > 0)
            {
                Where.Append(" and stoprincipal like '%" + strstoprincipal + "%' ");

            }
            string strstatus =ddl_status.SelectedValue;
            if (strstatus.Length > 0)
            {
                Where.Append(" and status= '" + strstatus + "'");

            }

            string buscode = ddl_businfo.SelectedValue;
            if (buscode.Length > 0)
            {
                Where.Append(" and buscode='" + buscode + "'");
            }
            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }
    }
}