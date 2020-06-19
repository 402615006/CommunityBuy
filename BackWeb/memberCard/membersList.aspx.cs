﻿using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class membersList : Common.ListPage
    {
        bllmembers bll = new bllmembers();
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
            string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
            if (HidSortExpression.Value == "")
            {
                order = " ctime desc";
            }
            if (string.IsNullOrWhiteSpace(HidWhere.Value.Trim()))
            {
                HidWhere.Value = " where 1=2 ";
            }
            DataTable dt = bll.GetPagingInfoByList("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, order, out recount, out pagenums);

            if (dt != null)
            {
                dt.Columns.Add("statusname", typeof(string));
                dt.Columns.Add("sexname", typeof(string));
                dt.Columns.Add("bigcustomername", typeof(string));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                    dt.Rows[i]["sexname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Sex), dt.Rows[i]["sex"].ToString());
                    dt.Rows[i]["bigcustomername"] = dt.Rows[i]["bigcustomer"].ToString() == "0" ? "否" : "是";

                }
                gv_list.DataSource = dt;
                gv_list.DataBind();
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
                    case "delete":
                        {
                            //日志信息
                            logentity.module = ErrMessage.GetMessageInfoByCode("members_Menu").Body;
                            logentity.pageurl = "membersEdit.aspx";
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
                                logentity.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("members_961").Body, LoginedUser.UserInfo.cname, Selected);
                                bll.Delete("0", "0", arrSel[i], logentity);
                            }
                            sp_showmes.InnerText = ErrMessage.GetMessageInfoByCode("Err_001").Body;
                            anp_top.CurrentPageIndex = 1;
                        }
                        break;
                    //有效
                    case "active":
                        Selected = GetSelectStr(gv_list, true);
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
                        Selected = GetSelectStr(gv_list, true);
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
                    case "memeditphone":
                         Selected = GetSelectStr(gv_list, true);
                         if (Selected.Length == 0)
                         {
                             sp_showmes.InnerText = "请选择要操作记录！";
                         }
                         else
                         {
                             var dto = bll.GetEntitySigInfo(string.Format("memcode ={0}", Selected));
                             Script(this.Page, "ShowOpenpage('修改手机号', '/memberCard/membersPhoneEdit.aspx?id=" + dto.memcode + "', '100%', '100%', true, true);");
                         }
                        break;
                    //导出事件代码
                    case "export":
                        int recount;
                        int pagenums;
                        string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
                        dt = bll.GetPagingInfoByList("", "0", 50000, 1, HidWhere.Value, order, out recount, out pagenums);
                        dt.Columns.Add("statusname", typeof(string));
                        dt.Columns.Add("sexname", typeof(string));

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                            dt.Rows[i]["sexname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Sex), dt.Rows[i]["sex"].ToString());

                        }
                        string fileName = string.Format(ErrMessage.GetMessageInfoByCode("members_TName").Body + "{0}.xls", DateTime.Now.ToString("_yyyyMMddHHmmss"));
                        string strColumnName = ErrMessage.GetMessageInfoByCode("members_Export").Body;
                        string ColumnCode = "memcode,cname,sexname,mobile,idtypeName,IDNO,areaname,totalcard,statusname";
                        ExcelsHelp.ExportExcelFileB(dt, System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8), strColumnName.Split(','), ColumnCode.Split(','));
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
            Where.AppendFormat(" where  1=1 ");
            //拼接Where条件
            string memcode = Helper.ReplaceString(txt_memcode.Value);
            if (memcode.Length > 0)
            {
                Where.Append(" and memcode =  '" + memcode + "'");
            }
            string strcname = Helper.ReplaceString(txt_cname.Value);
            if (strcname.Length > 0)
            {
                Where.Append(" and cname = '" + strcname + "'");

            }
            string strmobile = Helper.ReplaceString(txt_mobile.Value);
            if (strmobile.Length > 0)
            {
                Where.Append(" and mobile = '" + strmobile + "'");

            }
            string strIDNO = Helper.ReplaceString(txt_IDNO.Value);
            if (strIDNO.Length > 0)
            {
                Where.Append(" and IDNO = '" + strIDNO + "'");
            }
            if (Where.ToString().Trim() == "where  1=1")
            {
                HidWhere.Value = " where 1=2";
            }
            else
            {
                HidWhere.Value = Where.ToString();
            }
            anp_top.CurrentPageIndex = 1;
        }
    }
}