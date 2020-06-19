using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.manage
{
    public partial class rolefunctionlist : ListPage
    {
        bllroles bll = new bllroles();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                if (LoginedUser.UserInfo != null)
                {
                    BindGridView();
                }
            }
        }
        protected override void BindGridView()
        {
            int recnums = 0;
            int pagenums = 0;
            if (HidSortExpression.Value == "")
            {
                HidSortExpression.Value = "ctime";
                HidOrder.Value = "desc";
            }
            //���ݲ�ѯ������ȡ�����б�
            string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
            DataTable dt = bll.GetPagingListInfo("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recnums, out pagenums);

            if (dt != null)
            {
                dt.Columns.Add("rolestatus", typeof(string));
                dt.Columns.Add("scopename", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["rolestatus"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                    dt.Rows[i]["scopename"] = Helper.GetEnumNameByValue(typeof(SystemEnum.AccScope), dt.Rows[i]["scope"].ToString());
                }
                gv_list.DataSource = dt;
                gv_list.DataBind();
                anp_top.RecordCount = recnums;
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
                    //�����¼�����
                    case "search":
                        this.anp_top.CurrentPageIndex = 1;
                        GotoSearch();
                        break;
                    //ˢ���¼�����
                    case "refresh":
                        BindGridView();
                        break;
                    case "delete":
                        {
                            Selected = GetSelectStr(gv_list);
                            if (Selected.Length == 0)
                            {
                                sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_005").Body;
                            }
                            else
                            {
                                string mes = string.Empty;
                                bll.DeleteRoles(Selected, ref mes);
                                switch (mes)
                                {
                                    case "0":
                                        sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_001").Body;
                                        break;
                                    case "2":
                                        sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_060").Body;
                                        break;
                                    default:
                                        sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_058").Body;
                                        break;
                                }
                                anp_top.CurrentPageIndex = 1;
                            }
                        }
                        break;
                    //��Ч
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
                    //��Ч
                    case "invalid":
                        Selected = GetSelectStr(gv_list);
                        if (Selected.Length == 0)
                        {
                            sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_005").Body;
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
                    //�����¼�����
                    case "export":
                        int recount;
                        int pagenums;
                        string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                        dt = bll.GetPagingListInfo("", "0", 50000, 1, HidWhere.Value, order, out recount, out pagenums);
                        if (dt != null)
                        {
                            dt.Columns.Add("rolestatus", typeof(string));
                            dt.Columns.Add("scopename", typeof(string));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["rolestatus"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                                dt.Rows[i]["scopename"] = Helper.GetEnumNameByValue(typeof(SystemEnum.AccScope), dt.Rows[i]["scope"].ToString());
                            }
                        }
                        string fileName = string.Format(ErrMessage.GetMessageInfoByCode("roles_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        string strColumnName = ErrMessage.GetMessageInfoByCode("roles_Export").Body;
                        string ColumnCode = "cname,rolestatus,descr,ctime";
                        ExcelsHelp.ExportExcelFileB(dt, fileName, strColumnName.Split(','), ColumnCode.Split(','));
                        break;
                }
            }
        }

        /// <summary>
        /// ������ťƴ��Where����
        /// </summary>
        public void GotoSearch()
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" where 1=1 ");
            //ƴ��Where����
            string Name = Helper.ReplaceString(username.Value);
            if (Name.Length > 0)
            {
                Where.Append(" and cname like'%" + Name + "%'");
            }
            string status = sel_status.SelectedValue;
            if (status.Length > 0)
            {
                Where.Append(" and status='" + status + "'");
            }
            HidWhere.Value = Where.ToString();
            BindGridView();
        }
    }
}

