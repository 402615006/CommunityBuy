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
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                BindBusInfo();
                this.ddl_status.SelectedValue = "1";
                GotoSearch();
            }
        }

        public void BindBusInfo()
        {
            DataTable dt = new bllPaging().GetDataTableInfoBySQL("select buscode,cname from Business where status='1';");
            Helper.BindDropDownListForSearch(ddl_businfo, dt, "cname", "buscode", 2);
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
            DataTable dt = bll.GetPagingListInfoByBack("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recount, out pagenums);

            if (dt != null)
            {
                dt.Columns.Add("statusname", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                }
                gv_list.DataSource = dt;
                gv_list.DataBind();
                anp_top.RecordCount = recount;
            }
            DataTable dtdict = new BLL.bllprovinces().GetPagingListInfo("0", "", 10000, 1, "", " id", out recount, out pagenums);
            Helper.BindDropDownListForSearch(this.ddl_provinceid, dtdict, "province", "provinceid", 2);
            ddl_provinceid_SelectedIndexChanged(null, null);
        }

        protected void ddl_provinceid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddl_provinceid.SelectedValue))
            {
                int recnums, pagenums;
                string fiter = " parentid =" + this.ddl_provinceid.SelectedValue;
                DataTable dtdict = new BLL.bllcitys().GetPagingListInfo("0", "", 10000, 1, fiter, " id", out recnums, out pagenums);
                Helper.BindDropDownListForSearch(this.ddl_cityid, dtdict, "city", "cityid", 2);
                ddl_cityid_SelectedIndexChanged(null, null);
            }
        }

        protected void ddl_cityid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddl_cityid.SelectedValue))
            {
                int recnums, pagenums;
                string fiter = " parentid =" + this.ddl_cityid.SelectedValue;
                DataTable dtdict = new BLL.bllareas().GetPagingListInfo("0", "", 10000, 1, fiter, " id", out recnums, out pagenums);
                Helper.BindDropDownListForSearch(this.ddl_areaid, dtdict, "area", "areaid", 2);
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
                            //日志信息
                            logentity.module = ErrMessage.GetMessageInfoByCode("Store_Menu").Body;
                            logentity.pageurl = "StoreEdit.aspx";
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
                        dt = bll.GetPagingListInfo("", "0", 50000, 1, HidWhere.Value, order, out recount, out pagenums);
                        if (dt != null)
                        {
                            dt.Columns.Add("statusname", typeof(string));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                            }
                        }
                        string fileName = string.Format(ErrMessage.GetMessageInfoByCode("Store_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        string strColumnName = ErrMessage.GetMessageInfoByCode("store_Export").Body;
                        string ColumnCode = "stocode,cname,sname,bcode,areaname,stoprincipal,stoprincipaltel,tel,statusname";
                        ExcelsHelp.ExportExcelFileB(dt, fileName, strColumnName.Split(','), ColumnCode.Split(','));
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

            string strstocode = Helper.ReplaceString(txt_stocode.Value);
            if (strstocode.Length > 0)
            {
                Where.Append(" and stocode like '%" + strstocode + "%' ");

            }
            string strcname = Helper.ReplaceString(txt_cname.Value);
            if (strcname.Length > 0)
            {
                Where.Append(" and cname like '%" + strcname + "%' ");

            }
            string strprovinceid = Helper.ReplaceString(this.ddl_provinceid.SelectedValue);
            if (strprovinceid.Length > 0)
            {
                Where.Append(" and provinceid= '" + strprovinceid + "'");

            }
            string strcityid = Helper.ReplaceString(this.ddl_cityid.SelectedValue);
            if (strcityid.Length > 0)
            {
                Where.Append(" and cityid= '" + strcityid + "'");

            }
            string strareaid = Helper.ReplaceString(this.ddl_areaid.SelectedValue);
            if (strareaid.Length > 0)
            {
                Where.Append(" and areaid= '" + strareaid + "'");

            }
            string strstoprincipal = Helper.ReplaceString(txt_stoprincipal.Value);
            if (strstoprincipal.Length > 0)
            {
                Where.Append(" and stoprincipal like '%" + strstoprincipal + "%' ");

            }
            string strstatus = Helper.ReplaceString(ddl_status.SelectedValue);
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