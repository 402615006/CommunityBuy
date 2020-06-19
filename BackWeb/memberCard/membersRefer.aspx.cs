using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class membersRefer : Common.ListPage
    {
        bllmembers bll = new bllmembers();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                if (Request.QueryString["isbig"] != null && Request.QueryString["isbig"] == "1")
                {
                    HidWhere.Value = "where  memcode in (select memcode from memcard where affiliated='1' and isnull(pcardcode,'')='') ";
                }
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
            Where.Append(" where  1=1 ");
            if (Request.QueryString["isbig"] != null && Request.QueryString["isbig"] == "1")
            {
                Where.Append("  and members.memcode in (select memcode from memcard where affiliated='1' and isnull(pcardcode,'')='') ");
            }
            //拼接Where条件
            string cname = Helper.ReplaceString(txt_cname.Value);
            string memcode = Helper.ReplaceString(txt_memcode.Value);
            if (cname.Length > 0)
            {
                Where.Append(" and  members.cname like '%" + cname + "%' ");
            }

            if (memcode.Length > 0)
            {
                Where.Append(" and  members.memcode like '%" + memcode + "%' ");
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