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
                this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
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
                            //日志信息
                            logentity.module = ErrMessage.GetMessageInfoByCode("ts_Dicts_Menu").Body;
                            logentity.pageurl = "ts_DictsEdit.aspx";
                            logentity.otype = SystemEnum.LogOperateType.Delete;
                            logentity.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());

                            if (hidtreid.Value.Length == 0)
                            {
                                Script(Page, "pcLayerMsg('" + ErrMessage.GetMessageInfoByCode("Err_005").Body + "');");
                            }
                            else
                            {
                                logentity.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("ts_Dicts_961").Body, LoginedUser.UserInfo.cname, Selected);
                                dt = bll.Delete("0", "0", hidtreid.Value, logentity);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    Script(Page, "pcLayerMsg('" + dt.Rows[0]["mes"].ToString() + "');");
                                }
                                else
                                {
                                    Script(Page, "pcLayerMsg('" + ErrMessage.GetMessageInfoByCode("Err_004").Body + "');");
                                }
                            }

                        }
                        break;
                    //修改
                    case "edit":
                        if (hidtreid.Value.Length == 0)
                        {
                            Script(Page, "pcLayerMsg('" + ErrMessage.GetMessageInfoByCode("Tsdicts_choose").Body + "');");
                        }
                        else
                        {
                            Response.Redirect("ts_DictsEdit.aspx?id=" + hidtreid.Value);
                        }
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