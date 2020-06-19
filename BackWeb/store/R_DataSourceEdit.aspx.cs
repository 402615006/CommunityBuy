using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class R_DataSourceEdit : EditPage
    {
        bllR_DataSource bll = new bllR_DataSource();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = "重新计算";
            if (!IsPostBack)
            {
                BindStoInfo();
            }
        }

        /// <summary>
        /// 绑定门店信息
        /// </summary>
        public void BindStoInfo()
        {
            DataTable dt = new bllPaging().GetDataTableInfoBySQL("select stocode,cname from store where stocode not in (select stocode from store where isfood='1' and pstocode='') and status=1 " + GetAuthoritywhere("stocode") + ";");
            Helper.BindDropDownListForSearch(ddl_stocode, dt, "cname", "stocode", 2);
        }

        //执行计算
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            string buscode = Helper.GetAppSettings("BusCode");
            string stocode = ddl_stocode.SelectedValue;
            //执行日期
            string startdate = Helper.ReplaceString(txt_startdate.Value);
            string enddate = Helper.ReplaceString(txt_enddate.Value);
            string cusercode = LoginedUser.UserInfo.username;
            DataTable dt = bll.CalByStocode(buscode, stocode, startdate, enddate, cusercode);
            //显示结果
            ShowResult(dt, errormessage);
            Script(this.Page, "closeloading();");
        }
    }
}