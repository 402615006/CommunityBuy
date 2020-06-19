using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class dishesRefer : Common.ListPage
    {
        blldishes bll = new blldishes();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                BindGridView();
            }
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
            if (Request["stocode"] != null)
            {
                string stocode = Request.QueryString["stocode"].ToString();
                if (string.IsNullOrEmpty(HidWhere.Value))
                {
                    if (stocode.Length > 0)
                    {
                        HidWhere.Value = " where stocode in(" + GetWhereStrs(stocode, ",") + ")";
                    }
                }
                else
                {
                    if (stocode.Length > 0)
                    {
                        HidWhere.Value += " and stocode in(" + GetWhereStrs(stocode, ",") + ")";
                    }
                }
            }

            DataTable dt = bll.GetPagingListInfo1("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, string.Empty, string.Empty, 0, out recount, out pagenums);
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
            Where.Append(" where 1=1 ");
            //拼接Where条件
            //Where.Append(GetAuthoritywhere("stocode"));

            string strdisname = Helper.ReplaceString(txt_disname.Value);
            if (strdisname.Length > 0)
            {
                Where.Append(" and disname like '%" + strdisname + "%' ");

            }
            string strcustomcode = Helper.ReplaceString(txt_customcode.Value);
            if (strcustomcode.Length > 0)
            {
                Where.Append(" and customcode like '%" + strcustomcode + "%' ");

            }
            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GotoSearch();
        }
    }
}