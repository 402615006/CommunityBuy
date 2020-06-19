using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class selectStroe : CommunityBuy.BackWeb.Common.ListPage
    {
        bllStore bll = new bllStore();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["type"] != null)
                {
                    HidType.Value = Request["type"].ToString();
                }
                //if (Request["buscode"] != null)
                //{
                //    if(!string.IsNullOrEmpty(Request["buscode"].ToString()))
                //    {
                //        hidbuscode.Value = Request["buscode"].ToString();
                //    }
                //}
                BindBusInfo();
                GotoSearch();
            }
        }

        public void BindBusInfo()
        {
            //string _buscode = string.Empty;
            //if (!string.IsNullOrEmpty(hidbuscode.Value))
            //{
            //    _buscode += " and buscode='" + hidbuscode.Value + "'";
            //}
            DataTable dt = new bllPaging().GetDataTableInfoBySQL("select buscode,cname from Business where status='1'");
            //DataTable dt = new bllPaging().GetDataTableInfoBySQL("select buscode,cname from Business where status='1'"+_buscode);
            //Helper.BindDropDownListForSearch(ddl_businfo, dt, "cname", "buscode", 2);
            //if (!string.IsNullOrEmpty(_buscode))
            //{
            //    ddl_businfo.SelectedValue = hidbuscode.Value;
            //    ddl_businfo.Enabled = false;
            //}
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected override void BindGridView()
        {
            int recount;
            int pagenums;
            hidisfirst.Value = (StringHelper.StringToInt(hidisfirst.Value) + 1).ToString();
            string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
            if (HidSortExpression.Value == "")
            {
                order = " ctime desc";
            }

            //if (string.IsNullOrEmpty(HidWhere.Value))
            //{
            //    HidWhere.Value = " where stocode not in (select stocode from store where isfood='1' and pstocode='')";
            //}
            //else
            //{
            //    HidWhere.Value += " and stocode not in (select stocode from store where isfood='1' and pstocode='')";
            //}
            if (HidType.Value == "cinemas" && string.IsNullOrWhiteSpace(HidWhere.Value))
            {
                HidWhere.Value = " where stocode in ('07','10','15','HMHXYY')";
            }
            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recount, out pagenums);
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
        }

        /// <summary>
        /// 搜索按钮拼接Where条件
        /// </summary>
        public void GotoSearch()
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" where isdelete=0 ");
            if (Request["isrole"] == null)
            {
                Where.Append(GetAuthoritywhere("stocode"));
            }
            if (HidType.Value == "cinemas")
            {
                Where.Append(" and stocode in ('07','10','15','HMHXYY')");
            }
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

            //string buscode = ddl_businfo.SelectedValue;
            //if (buscode.Length > 0)
            //{
            //    Where.Append(" and buscode='" + buscode + "'");
            //}
            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GotoSearch();
        }
    }
}