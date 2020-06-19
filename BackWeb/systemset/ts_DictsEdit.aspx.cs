using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.systemset
{
    public partial class ts_DictsEdit : EditPage
    {
        public string dicid;
        bllts_Dicts bll = new bllts_Dicts();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindddlSource();
                if (Request["id"] != null)
                {
                    dicid = Request["id"].ToString();
                    hidId.Value = dicid;
                    SetPage(hidId.Value);
                    this.PageTitle.Operate = 修改
                }
                else
                {
                    this.PageTitle.Operate ="新增";
                }
            }
        }

        private void BindddlSource()
        {
            int recnum = 0; int pagenums = 0;
            DataTable dtdict = bll.GetPagingListInfo("", "0", 10000, 1, "pdicid=0  and status ='1' ", "", out recnum, out pagenums);
            Helper.BindDropDownListForSearch(ddl_pdicid, dtdict, "dicname", "dicid", 0);


            DataTable dtlng = new blllanguages().GetPagingListInfo("", "0", 10000, 1, "  status ='1' ", "", out recnum, out pagenums);
            Helper.BindDropDownListForSearch(this.ddl_lng, dtlng, "cname", "code", 0);
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where dicid='" + id + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ddl_lng.SelectedValue = dr["lng"].ToString();
                ddl_pdicid.SelectedValue = dr["pdicid"].ToString();
                if (dr["pdicid"].ToString() == "0")
                {
                    this.ddl_pdicid.Enabled = false;
                    this.savepage.Visible = false;
                }
                else
                {
                    this.ddl_pdicid.Enabled = true;
                    this.savepage.Visible = true;
                }

                txt_diccode.Text = dr["diccode"].ToString();
                txt_dicname.Text = dr["dicname"].ToString();
                if (dr["pdicid"].ToString() == "0")
                {

                    txt_diccode.Enabled = false;
                    txt_dicname.Enabled = false;
                }

                txt_orderno.Text = dr["orderno"].ToString();
                txt_remark.Text = dr["remark"].ToString();
                ddl_status.SelectedValue = dr["status"].ToString();
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string buscode = Helper.GetAppSettings("BusCode");
            string strcode = Helper.GetAppSettings("Stocode");
            string dictype = "0";
            string lng = Helper.ReplaceString(this.ddl_lng.SelectedValue);
            string pdicid = Helper.ReplaceString(this.ddl_pdicid.SelectedValue);
            string diccode = Helper.ReplaceString(txt_diccode.Text);
            string dicname = Helper.ReplaceString(txt_dicname.Text);
            string dicvalue = "";
            string orderno = Helper.ReplaceString(txt_orderno.Text);
            string remark = Helper.ReplaceString(txt_remark.Text);
            string status = Helper.ReplaceString(ddl_status.SelectedValue);
            string cuser = LoginedUser.UserInfo.Id.ToString();

            //日志信息
            logentity.module = ErrMessage.GetMessageInfoByCode("admins_Menu").Body;
            logentity.pageurl = "ts_Dictsedit.aspx";
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());

            DataTable dt = new DataTable();

            if (hidId.Value.Length == 0)//添加信息
            {
                logentity.logcontent = "新增系统字典信息信息";
                dt = bll.Add("0", "0", out dicid, buscode, strcode, dictype, lng, pdicid, diccode, dicname, dicvalue, orderno, remark, status, cuser, logentity);
                hidId.Value = dicid;
                this.PageTitle.Operate = 修改
            }
            else//修改信息
            {
                logentity.logcontent = "修改dicid为" + hidId.Value + "的系统字典信息信息";
                logentity.otype = SystemEnum.LogOperateType.Edit;
                dt = bll.Update("0", "0", hidId.Value, buscode, strcode, dictype, lng, pdicid, diccode, dicname, dicvalue, orderno, remark, status, cuser, logentity);
                this.PageTitle.Operate = 修改
            }
            //显示结果
            ShowResult(dt, errormessage);


        }
    }
}