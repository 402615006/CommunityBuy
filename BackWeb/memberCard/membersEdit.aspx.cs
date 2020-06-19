using Sam.WebControl;
using System;
using System.Data;
using System.Diagnostics;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb
{
    public partial class membersEdit : EditPage
    {
        public string memcode;
        public string url;
        bllmembers bll = new bllmembers();
        memberslogEntity memlogentity = new memberslogEntity();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindDDL();
                txt_birthday.Text = DateTime.Now.AddYears(-20).ToString("yyyy-MM-dd");

                if (Request["type"] != null)
                {
                    types.Value = "big";
                }

                if (Request["id"] != null)
                {
                    string from = "";
                    if (Request["from"] != null)
                    {
                        from = Request["from"].ToString();
                    }
                    memcode = Request["id"].ToString(); ;
                    hidId.Value = memcode;
                    //cb_bigcustomer.Disabled = true;
                    SetPage(hidId.Value, from);
                    this.PageTitle.Operate = 修改
                }
                else
                {
                    //url = "MemberCardShow.aspx";
                    this.PageTitle.Operate ="新增";
                }
            }
        }

        private void BindDDL()
        {
            DataTable dtdict = new bllts_Dicts().GetDictsListByEnum("", "", SystemEnum.DictList.IDType);
            Helper.BindDropDownListForSearch(this.ddl_idtype, dtdict, "dicname", "diccode", 0);

            dtdict = new bllts_Dicts().GetDictsListByEnum("", "", SystemEnum.DictList.MemberCome);
            Helper.BindDropDownListForSearch(this.txt_source, dtdict, "dicname", "diccode", 0);
            txt_source.SelectedValue = "SH";
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id, string from)
        {
            string filter = " where memcode='" + id + "'";
            if (from.Length > 0)
            {
                filter = " where memcode=(select memcode from dbo.memcard where cardid='" + id + "')";
            }
            DataTable dt = bll.GetPagingSigInfo("0", "0", filter);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                hidId.Value = dr["memcode"].ToString();
                url = "MemberCardShow.aspx?id=" + hidId.Value;
                memcode = hidId.Value;
                txt_source.SelectedValue = dr["source"].ToString();
                //hidstore.Value = dr["strcode"].ToString();
                //txt_stocode.Text = dr["stoname"].ToString();
                txt_wxaccount.Text = dr["wxaccount"].ToString();
                //hidbigcustomer.Value = dr["bigcustomer"].ToString();
                txt_cname.Text = dr["cname"].ToString();
                if (dr["birthday"].ToString().Length > 0)
                {
                    txt_birthday.Text = StringHelper.StringToDateTime(dr["birthday"].ToString()).ToString("yyyy-MM-dd");
                }
                txt_sex.SelectedValue = dr["sex"].ToString();
                txt_mobile.Text = dr["mobile"].ToString();
                txt_email.Text = dr["email"].ToString();
                txt_tel.Text = dr["tel"].ToString();
                ddl_idtype.SelectedValue = dr["idtype"].ToString();
                txt_IDNO.Text = dr["IDNO"].ToString();
                this.sel_provinceid.Value = dr["provinceid"].ToString();
                this.sel_city.Value = dr["cityid"].ToString();
                sel_area.Value = dr["areaid"].ToString();
                hid_provinceid.Value = dr["provinceid"].ToString();
                hidcity.Value = dr["cityid"].ToString();
                hidarea.Value = dr["areaid"].ToString();

                txt_address.Text = dr["address"].ToString();
                txt_hobby.Text = dr["hobby"].ToString();
                txt_remark.Text = dr["remark"].ToString();
                txt_photo.ImageUrl = dr["photo"].ToString();
                hid_photo.Value = dr["photo"].ToString();
                txt_signature.ImageUrl = dr["signature"].ToString();
                hid_signature.Value = dr["signature"].ToString();
                //hidbigcustomer.Value = dr["bigcustomer"].ToString();
                //Script(this.Page, "readbigcustomer();");
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string memid = "";
            string source = Helper.ReplaceString(txt_source.SelectedValue);
            string buscode = Helper.GetAppSettings("BusCode");
            string strcode = LoginedUser.UserInfo.stocode;
            string wxaccount = Helper.ReplaceString(txt_wxaccount.Text);
            string bigcustomer = "0";
            string cname = Helper.ReplaceString(txt_cname.Text);
            string IDNO = Helper.ReplaceString(txt_IDNO.Text);
            string birthday = string.Empty;
            if (IDNO.Length > 0)
            {
                if (IDNO.Length == 15)
                {
                    birthday = IDNO.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                }
                else if (IDNO.Length == 18)
                {
                    birthday = IDNO.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                }
            }
            string sex = Helper.ReplaceString(txt_sex.Text);
            string mobile = Helper.ReplaceString(txt_mobile.Text);
            string email = Helper.ReplaceString(txt_email.Text);
            string tel = Helper.ReplaceString(txt_tel.Text);
            string idtype = Helper.ReplaceString(ddl_idtype.SelectedValue);
            string provinceid = Helper.ReplaceString(Request.Form["sel_provinceid"]);
            string cityid = Helper.ReplaceString(Request.Form["sel_city"]);
            string areaid = Helper.ReplaceString(Request.Form["sel_area"]);
            string photo = Helper.ReplaceString(hid_photo.Value);
            string signature = Helper.ReplaceString(hid_signature.Value);
            string address = Helper.ReplaceString(txt_address.Text);
            string hobby = Helper.ReplaceString(txt_hobby.Text);
            string remark = Helper.ReplaceString(txt_remark.Text);
            string status = "1";
            string orderno = "";
            string cuser = LoginedUser.UserInfo.Id.ToString();
            string uuser = "0";
            string ousercode = LoginedUser.UserInfo.username;
            string ousername = LoginedUser.UserInfo.cname;

            //日志信息
            memlogentity.stocode = strcode;
            memlogentity.memcode = hidId.Value;
            memlogentity.ocode = ousercode;
            memlogentity.oname = ousername;

            DataTable dt = new DataTable();
            if (this.hidId.Value.Length == 0)//添加信息
            {
                memlogentity.operatetype = "新增";
                memlogentity.logcontext = "新增会员信息";
                dt = bll.Add("0", "0", out memid, "", source, buscode, strcode, wxaccount, bigcustomer, cname, birthday, sex, mobile, email, tel, idtype, IDNO, provinceid, cityid, areaid, photo, signature, address, hobby, remark, status, orderno, cuser, uuser, ousercode, ousername, memlogentity);
                hidId.Value = memid;

                this.PageTitle.Operate = 修改
            }
            else//修改信息
            {
                memlogentity.operatetype = "修改";
                dt = bll.Update("0", "0", memid, hidId.Value, source, buscode, strcode, wxaccount, bigcustomer, cname, birthday, sex, mobile, email, tel, idtype, IDNO, provinceid, cityid, areaid, photo, signature, address, hobby, remark, status, orderno, cuser, uuser, ousercode, ousername, memlogentity);
                this.PageTitle.Operate = 修改
            }
            //显示结果
            ShowResult(dt, errormessage);
        }

        Process keybord = null;
        private void btn_keybord_Click(object sender, EventArgs e)
        {
            if (keybord == null)
            {
                //开启键盘
                Process kbpr = System.Diagnostics.Process.Start("osk.exe");
            }
            else
            {
                keybord.Kill();
                keybord = null;
            }
        }
    }
}