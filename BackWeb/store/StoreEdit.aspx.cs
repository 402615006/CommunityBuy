using System;
using System.Data;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class StoreEdit : EditPage
    {
        public string stoid;
        string buscode = Helper.GetAppSettings("BusCode");
        bllStore bll = new bllStore();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBusInfo();
                BindCompanyInfo();
                bindddlSource();
                BindStoreType();
                BindSqInfo();
                BindPayType();
                BindPstocode("");
                if (Request["id"] != null)
                {
                    txt_stocode.Enabled = false;
                    stoid = Request["id"].ToString();
                    hidId.Value = stoid;
                    SetPage(hidId.Value);

                    this.PageTitle.Operate = 修改
                }
                else
                {
                    txt_stocode.Enabled = true;
                    this.PageTitle.Operate ="新增";
                }
            }
        }

        /// <summary>
        /// 绑定公司信息
        /// </summary>
        private void BindCompanyInfo()
        {
            DataTable dt = new bllcompany().GetListInfo("", "0", buscode);
            Helper.BindDropDownListForSearch(ddl_comcode, dt, "comname", "comcode", 2);
        }

        public void BindStoreType()
        {
            DataTable dt = new bllPaging().GetDataTableInfoBySQL("select diccode,dicname from ts_Dicts where pdicid in (select dicid from ts_Dicts where diccode='HYTypeFir');");
            Helper.BindDropDownListForSearch(ddl_storetype, dt, "dicname", "diccode", 0);
        }

        public void BindSqInfo()
        {
            DataTable dt = new bllPaging().GetDataTableInfoBySQL(string.Format("select sqcode,sqname from sqinfo where status='1' and isdelete='0' and city='{0}'", this.ddl_cityid.SelectedValue));
            Helper.BindDropDownListForSearch(ddl_sq, dt, "sqname", "sqcode", 0);
        }

        public void BindPayType()
        {
            this.ddl_paytype.Items.Add(new ListItem("先支付", "0"));
            this.ddl_paytype.Items.Add(new ListItem("后支付", "1"));
        }

        public void BindBusInfo()
        {
            DataTable dt = new bllPaging().GetDataTableInfoBySQL("select buscode,cname from Business where status='1';");
            Helper.BindDropDownListForSearch(ddl_businfo, dt, "cname", "buscode", 2);
        }

        /// <summary>
        /// 绑定父级门店code
        /// </summary>
        private void BindPstocode(string pstocode)
        {
            //int recnums, pagenums;
            //DataTable dtstore = new bllStore().GetPagingListInfo("0", "", 10000, 1, " isfood='1' and len(isnull(pstocode,''))=0 ", "", out recnums, out pagenums);
            //Helper.BindDropDownListForSearch(this.ddl_pstocode, dtstore, "cname", "stocode", 1);
            //if (pstocode != "")
            //{
            //    ddl_pstocode.SelectedValue = pstocode;
            //}
        }

        private void bindddlSource()
        {
            int recnums, pagenums;
            DataTable dtdict = new BLL.bllprovinces().GetPagingListInfo("0", "", 10000, 1, "", " id", out recnums, out pagenums);
            Helper.BindDropDownListForSearch(this.ddl_provinceid, dtdict, "province", "provinceid", 0);
            //hard code  选中新疆自治区
            ddl_provinceid.SelectedValue = "650000";
            ddl_provinceid_SelectedIndexChanged(null, null);
        }

        protected void ddl_provinceid_SelectedIndexChanged(object sender, EventArgs e)
        {
            int recnums, pagenums;
            string fiter = " parentid =" + this.ddl_provinceid.SelectedValue;
            DataTable dtdict = new BLL.bllcitys().GetPagingListInfo("0", "", 10000, 1, fiter, " id", out recnums, out pagenums);
            Helper.BindDropDownListForSearch(this.ddl_cityid, dtdict, "city", "cityid", 0);
            ddl_cityid_SelectedIndexChanged(null, null);
        }

        protected void ddl_cityid_SelectedIndexChanged(object sender, EventArgs e)
        {
            int recnums, pagenums;
            string fiter = " parentid =" + this.ddl_cityid.SelectedValue;
            DataTable dtdict = new BLL.bllareas().GetPagingListInfo("0", "", 10000, 1, fiter, " id", out recnums, out pagenums);
            Helper.BindDropDownListForSearch(this.ddl_areaid, dtdict, "area", "areaid", 0);
            //
            BindSqInfo();
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            DataTable dt = bll.GetPagingSigInfoByBack("0", "0", " where stoid='" + id + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ddl_comcode.Text = dr["comcode"].ToString();
                txt_stocode.Text = dr["stocode"].ToString();
                txt_cname.Text = dr["cname"].ToString();
                txt_sname.Text = dr["sname"].ToString();
                txt_bcode.Text = dr["bcode"].ToString();
                this.ddl_provinceid.SelectedValue = dr["provinceid"].ToString();
                ddl_provinceid_SelectedIndexChanged(null, null);
                this.ddl_cityid.SelectedValue = dr["cityid"].ToString();
                ddl_cityid_SelectedIndexChanged(null, null);
                this.ddl_areaid.SelectedValue = dr["areaid"].ToString();
                txt_address.Text = dr["address"].ToString();
                txt_stoprincipal.Text = dr["stoprincipal"].ToString();
                txt_stoprincipaltel.Text = dr["stoprincipaltel"].ToString();
                txt_tel.Text = dr["tel"].ToString();
                txt_stoemail.Text = dr["stoemail"].ToString();
                hidisfood.Value = dr["isfood"].ToString();
                //txt_logo.Text = dr["logo"].ToString();
                //txt_backgroundimg.Text = dr["backgroundimg"].ToString();
                txt_descr.Text = dr["descr"].ToString();
                txt_stourl.Text = dr["stourl"].ToString();
                txt_stocoordx.Text = dr["stocoordx"].ToString();
                txt_stocoordy.Text = dr["stocoordy"].ToString();
                txt_calcutime.Text = dr["calcutime"].ToString();
                ddl_status.SelectedValue = dr["status"].ToString();
                //txt_isdelete.Text = dr["isdelete"].ToString();
                logo.Src = dr["logo"].ToString();
                //backgroundimg.Src = dr["backgroundimg"].ToString();
                //hid_backgroundimg.Value = dr["backgroundimg"].ToString();
                hid_logo.Value = dr["logo"].ToString();
                txt_btime.Text = dr["btime"].ToString();
                txt_etime.Text = dr["etime"].ToString();
                this.ddl_businfo.SelectedValue = dr["buscode"].ToString();
                hidLvData.Value = dr["services"].ToString();
                BindPstocode(dr["pstocode"].ToString());

                DataTable dtgx = new bllPaging().GetDataTableInfoBySQL("select top 1 jprice,firtype,ptype,sqcode,stopath from storegx where stocode='" + txt_stocode.Text + "';");
                if (dtgx != null && dtgx.Rows.Count > 0)
                {
                    txt_jprice.Text = dtgx.Rows[0]["jprice"].ToString();
                    ddl_storetype.SelectedValue = dtgx.Rows[0]["firtype"].ToString();
                    ddl_paytype.SelectedValue = dtgx.Rows[0]["ptype"].ToString();
                    ddl_sq.SelectedValue = dtgx.Rows[0]["sqcode"].ToString();
                    hidstopath.Value = dtgx.Rows[0]["stopath"].ToString();
                }
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string comcode = ddl_comcode.SelectedValue;
            string buscode = ddl_businfo.SelectedValue;
            string stocode = Helper.ReplaceString(txt_stocode.Text);
            string cname = Helper.ReplaceString(txt_cname.Text);
            string sname = Helper.ReplaceString(txt_sname.Text);
            string bcode = Helper.ReplaceString(txt_bcode.Text);
            string indcode = "";
            string provinceid = Helper.ReplaceString(this.ddl_provinceid.SelectedValue);
            string cityid = Helper.ReplaceString(this.ddl_cityid.SelectedValue);
            string areaid = Helper.ReplaceString(this.ddl_areaid.SelectedValue);
            string address = Helper.ReplaceString(txt_address.Text);
            string stoprincipal = Helper.ReplaceString(txt_stoprincipal.Text);
            string stoprincipaltel = Helper.ReplaceString(txt_stoprincipaltel.Text);
            string tel = Helper.ReplaceString(txt_tel.Text);
            string stoemail = Helper.ReplaceString(txt_stoemail.Text);
            string logo = Helper.ReplaceString(this.hid_logo.Value);
            //string backgroundimg = Helper.ReplaceString(hid_backgroundimg.Value);
            string descr = Helper.ReplaceString(txt_descr.Text);
            string stourl = Helper.ReplaceString(txt_stourl.Text);
            string stocoordx = Helper.ReplaceString(txt_stocoordx.Text);
            string stocoordy = Helper.ReplaceString(txt_stocoordy.Text);
            string netlinklasttime = "";
            string calcutime = Helper.ReplaceString(txt_calcutime.Text);
            string remark = "";
            string status = Helper.ReplaceString(ddl_status.SelectedValue);
            string cuser = LoginedUser.UserInfo.Id.ToString();
            string uuser = "0";
            string terminalnumber = "0";
            string valuesdate = "2050-12-31";
            string storetype = ddl_storetype.SelectedValue;
            string paytype = ddl_paytype.SelectedValue;
            string jprice = Helper.ReplaceString(txt_jprice.Text);
            string sqcode = ddl_sq.SelectedValue;
            string stopath = hidstopath.Value;
            string services = hidLvData.Value;

            //日志信息
            logentity.module = ErrMessage.GetMessageInfoByCode("admins_Menu").Body;
            logentity.pageurl = "Storeedit.aspx";
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());


            string busHour = "";//Helper.ReplaceString(txt_busHour.Text);
            string btime = txt_btime.Text;
            string etime = txt_etime.Text;
            string isfood = hidisfood.Value;
            string recommended = Helper.ReplaceString(txt_recommended.Text);
            string pstocode = ddl_pstocode.SelectedValue.ToString();
            DataTable dt = new DataTable();
            if (hidId.Value.Length == 0)//添加信息
            {
                logentity.logcontent = "新增商家门店信息信息";
                dt = bll.Add("0", "0", out stoid, comcode, buscode, stocode, cname, sname, bcode, indcode, provinceid, cityid, areaid, address, stoprincipal, stoprincipaltel, tel, stoemail, logo, "", stopath, services, descr, stourl, stocoordx, stocoordy, netlinklasttime, calcutime, busHour, recommended, remark, status, cuser, uuser, btime, etime, terminalnumber, valuesdate, isfood, pstocode, storetype, jprice, paytype, sqcode, logentity);
                hidId.Value = stoid;
                this.PageTitle.Operate = 修改
            }
            else//修改信息
            {
                logentity.logcontent = "修改stoid为" + hidId.Value + "的商家门店信息信息";
                logentity.otype = SystemEnum.LogOperateType.Edit;
                dt = bll.UpdateByBack("0", "0", hidId.Value, comcode, buscode, stocode, cname, sname, bcode, indcode, provinceid, cityid, areaid, address, stoprincipal, stoprincipaltel, tel, stoemail, logo, "", stopath, services, descr, stourl, stocoordx, stocoordy, netlinklasttime, calcutime, busHour, recommended, remark, status, cuser, uuser, btime, etime, terminalnumber, valuesdate, isfood, pstocode, storetype, jprice, paytype, sqcode, logentity);
            }
            //显示结果
            ShowResult(dt, errormessage);
            SetPage(hidId.Value);
        }
    }
}