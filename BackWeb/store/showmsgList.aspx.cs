using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb.store
{
    public partial class showmsgList : Common.ListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = "列表";
            if (!IsPostBack)
            {
                //GotoSearch();
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected override void BindGridView()
        {
            string sql = "select top 5  [mobile],[smscontent]=substring(smscontent,1,charindex('|',smscontent,1)-1),[sendtime] from SendSmsLogs " + HidWhere.Value + " order by id desc ";
            DataTable dt = new bllPaging().GetDataTableInfoBySQL(sql);
            if (dt != null)
            {
                gv_list.DataSource = dt;
                gv_list.DataBind();
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
                        BindGridView();
                        break;
                    //刷新事件代码
                    case "refresh":
                        BindGridView();
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
            Where.Append("where charindex('code',smscontent,1)<=3 and charindex('code',smscontent,1)>=1 ");
            //拼接Where条件
            if (!string.IsNullOrEmpty(txt_cmobile.Text.Trim()))
            {
                string cmobile = txt_cmobile.Text.Trim().Replace(" ", string.Empty);

                Where.Append(" and mobile ='" + cmobile + "'");
            }
            HidWhere.Value = Where.ToString();
        }

        protected void readCard_Click(object sender, EventArgs e)
        {
            GotoSearch();
        }
    }
}