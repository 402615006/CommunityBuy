using Sam.WebControl;
using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class membersDetail : EditPage
    {
        public string memcode;
        public string url;
        bllmembers bll = new bllmembers();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindDDL();
                txt_birthday.Text = DateTime.Now.AddYears(-20).ToString("yyyy-MM-dd");
                if (Request["id"] != null)
                {
                    memcode = Request["id"].ToString();
                    hidId.Value = memcode;
                    url = "MemberCardShow.aspx?id=" + memcode;
                    SetPage(hidId.Value);
                    ddl_provinceid_SelectedIndexChanged(null, null);
                    ddl_cityid_SelectedIndexChanged(null, null);
                    this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateDetail").Body;

                    //会员优惠券 
                    DataTable dtcoupon = new bllMemCard().GetCouponInfo(memcode);
                    if (dtcoupon != null)
                    {
                        for (int i = 0; i < dtcoupon.Rows.Count; i++)
                        {
                            string storename = dtcoupon.Rows[i]["storename"].ToString();
                            if (storename.Length == 0)
                            {
                                dtcoupon.Rows[i]["storename"] = "不限";
                            }
                        }
                        gv_list.DataSource = dtcoupon;
                        gv_list.DataBind();
                    }
                }
                else
                {
                    url = "MemberCardShow.aspx";
                    this.PageTitle.Operate ="新增";
                }
                SetControlReadOnly(this.Page);
            }
        }

        private void BindDDL()
        {
            int recnums, pagenums;
            DataTable dtdict = new bllts_Dicts().GetDictsListByEnum("", "", SystemEnum.DictList.IDType);
            Helper.BindDropDownListForSearch(this.ddl_idtype, dtdict, "dicname", "diccode", 1);

            dtdict = new bllts_Dicts().GetDictsListByEnum("", "", SystemEnum.DictList.MemberCome);
            Helper.BindDropDownListForSearch(this.txt_source, dtdict, "dicname", "diccode", 1);

            dtdict = new BLL.bllprovinces().GetPagingListInfo("0", "", 10000, 1, "", " id", out recnums, out pagenums);
            Helper.BindDropDownListForSearch(this.ddl_provinceid, dtdict, "province", "provinceid", 2);
        }

        protected void ddl_provinceid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddl_provinceid.SelectedValue))
            {
                int recnums, pagenums;
                string fiter = " parentid =" + this.ddl_provinceid.SelectedValue;
                DataTable dtdict = new BLL.bllcitys().GetPagingListInfo("0", "", 10000, 1, fiter, " id", out recnums, out pagenums);
                Helper.BindDropDownListForSearch(this.ddl_cityid, dtdict, "city", "cityid", 2);
                ddl_cityid.SelectedValue = hidcity.Value;
            }
        }

        protected void ddl_cityid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddl_cityid.SelectedValue))
            {
                int recnums, pagenums;
                string fiter = " parentid =" + this.ddl_cityid.SelectedValue;
                DataTable dtdict = new BLL.bllareas().GetPagingListInfo("0", "", 10000, 1, fiter, " id", out recnums, out pagenums);
                Helper.BindDropDownListForSearch(this.ddl_areaid, dtdict, "area", "areaid", 2);
                ddl_areaid.SelectedValue = hidarea.Value;
            }
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where memcode='" + id + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                txt_source.SelectedValue = dr["source"].ToString();
                hidstore.Value = dr["strcode"].ToString();
                txt_stocode.Text = dr["stoname"].ToString();
                txt_wxaccount.Text = dr["wxaccount"].ToString();
                hidbigcustomer.Value = dr["bigcustomer"].ToString();
                txt_cname.Text = dr["cname"].ToString();
                if (dr["birthday"].ToString().Length > 0)
                {
                    if (StringHelper.StringToDateTime(dr["birthday"].ToString()).ToShortDateString() != "1900/1/1")
                    {
                        txt_birthday.Text = StringHelper.StringToDateTime(dr["birthday"].ToString()).ToString("yyyy-MM-dd");
                    }
                }
                txt_sex.SelectedValue = dr["sex"].ToString();
                txt_mobile.Text = dr["mobile"].ToString();
                txt_email.Text = dr["email"].ToString();
                txt_tel.Text = dr["tel"].ToString();
                ddl_idtype.SelectedValue = dr["idtype"].ToString();
                txt_IDNO.Text = dr["IDNO"].ToString();
                ddl_provinceid.SelectedValue = dr["provinceid"].ToString();

                hidprovince.Value = dr["provinceid"].ToString();
                hidcity.Value = dr["cityid"].ToString();
                hidarea.Value = dr["areaid"].ToString();

                txt_address.Text = dr["address"].ToString();
                txt_hobby.Text = dr["hobby"].ToString();
                txt_remark.Text = dr["remark"].ToString();
                txt_photo.Src = dr["photo"].ToString();
                txt_signature.Src = dr["signature"].ToString();
            }
        }
    }
}