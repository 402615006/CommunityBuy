using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class ts_syssetEdit : EditPage
    {
        public string setid;
        bllts_sysset bll = new bllts_sysset();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    setid = Request["id"].ToString();
                    hidId.Value = setid;
                    SetPage(hidId.Value);
                    this.PageTitle.Operate = 修改
                }
                else
                {
                    this.PageTitle.Operate ="新增";
                }
            }
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where setid='" + id + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                //txt_stocode.Text = dr["stocode"].ToString();
                //txt_buscode.Text = dr["buscode"].ToString();
                txt_key.Text = dr["key"].ToString();
                txt_val.Text = dr["val"].ToString();
                ddl_status.SelectedValue = dr["status"].ToString();
                txt_descr.Value = dr["descr"].ToString();
                txt_explain.Value = dr["explain"].ToString();
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string stocode = Helper.GetAppSettings("Stocode");
            string buscode = Helper.GetAppSettings("BusCode");
            string key = Helper.ReplaceString(txt_key.Text);
            string val = Helper.ReplaceString(txt_val.Text);
            string status = Helper.ReplaceString(this.ddl_status.SelectedValue);
            string descr = Helper.ReplaceString(txt_descr.Value);
            string explain = Helper.ReplaceString(txt_explain.Value);
            //日志信息
            logentity.module = ErrMessage.GetMessageInfoByCode("admins_Menu").Body;
            logentity.pageurl = "ts_syssetedit.aspx";
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());

            DataTable dt = new DataTable();
            if (hidId.Value.Length == 0)//添加信息
            {
                logentity.logcontent = "新增系统设置信息";
                dt = bll.Add("0", "0", out setid, stocode, buscode, key, val, status, descr,explain, logentity);
                hidId.Value = setid;
                this.PageTitle.Operate = 修改
            }
            else//修改信息
            {
                logentity.logcontent = "修改setid为" + hidId.Value + "的系统设置信息";
                logentity.otype = SystemEnum.LogOperateType.Edit;
                dt = bll.Update("0", "0", hidId.Value, stocode, buscode, key, val, status, descr,explain, logentity);
            }
            //显示结果
            ShowResult(dt, errormessage);
        }
    }
}