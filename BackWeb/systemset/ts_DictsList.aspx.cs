using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb.systemset
{
    public partial class ts_DictsList : Common.ListPage
    {
        bllts_Dicts bll = new bllts_Dicts();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                VisibleToolBar("LinkRefresh", false);
                VisibleToolBar("LinkSearch", false);
                this.PageTitle.Operate = "列表";
                BindGridView();
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected override void BindGridView()
        {
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

                            if (hidtreid.Value.Length == 0)
                            {
                                Script(Page, "pcLayerMsg('选择一项进行操作');");
                            }
                            else
                            {
                                bll.Delete("0", "0", hidtreid.Value);
                                sp_showmes.InnerText = bll.oResult.Msg;
                            }

                        }
                        break;
                    //修改
                    case "edit":

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

        }
    }
}