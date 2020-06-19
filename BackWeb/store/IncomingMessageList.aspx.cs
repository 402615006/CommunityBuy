using Sam.WebControl;
using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb
{
    public partial class IncomingMessageList : Common.ListPage
    {
        public string memcode;
        //public string provinceids = string.Empty;//所属省
        //public string citys = string.Empty;//所属城市
        //public string areas = string.Empty;//所属区域
        public string url;
        public string url1;
        bllmembers bll = new bllmembers();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindDDL();
                url = "MemCardLists.aspx";
                url1 = "memcardconsumptionLists.aspx";
                this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateDetail").Body;
            }
        }

        private void BindDDL()
        {
            //证件类型
            DataTable dtdict = new bllts_Dicts().GetDictsListByEnum("", "", SystemEnum.DictList.IDType);
            Helper.BindDropDownListForSearch(this.ddl_idtype, dtdict, "dicname", "diccode", 0);
        }
        //查询按钮
        protected void Button1_Click(object sender, EventArgs e)
        {
            string mobileNum = phone_where.Text;//会员信息-电话
            int recount;
            int pagenums;
            string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
            if (HidSortExpression.Value == "")
            {
                order = " ctime desc";
            }

            if (mobileNum != null && mobileNum.Length > 0)
            {
                string sql = HidWhere.Value != "" ? HidWhere.Value + " and mobile='" + mobileNum + "'" : " where mobile='" + mobileNum + "'";
                DataTable dt = bll.GetPagingListInfo("0", "0", 10000, 0, sql, order, out recount, out pagenums);

                if (dt != null)
                {
                    if (dt.Rows.Count == 1)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            //隐藏域
                            Ghostsel_memid.Value = dr["memid"].ToString();//主键
                            Ghostsel_source.Value = dr["source"].ToString();//会员来源
                            Ghostsel_wxaccount.Value = dr["wxaccount"].ToString();//微信账号
                            Ghostsel_bigcustomer.Value = dr["bigcustomer"].ToString();//是否大客户
                            Ghostsel_tel.Value = dr["tel"].ToString();//客户电话
                            Ghostsel_provinceids.Value = dr["provinceid"].ToString();//所属省
                            Ghostsel_citys.Value = dr["cityid"].ToString();//所属城市
                            Ghostsel_areas.Value = dr["areaid"].ToString();//所属区域
                            Ghostsel_PhoneNum.Value = dr["mobile"].ToString();//会员姓名

                            //显示值
                            MemberName.Text = dr["cname"].ToString();//会员名称
                            MemberNumber.Text = dr["memcode"].ToString();//会员卡号
                            Ghostsel_memcardcodes.Value = dr["memcode"].ToString();//会员卡号
                            MemberSex.Text = dr["sex"].ToString();//会员性别
                            MemberPhone.Text = dr["mobile"].ToString();//会员手机号
                            ddl_idtype.SelectedValue = dr["idtype"].ToString();//证件类型
                            txt_IDNO.Text = dr["IDNO"].ToString();//证件号码
                            MemberBirthday.Text = dr["birthday"].ToString();//会员生日
                            txt_email.Text = dr["email"].ToString();//会员邮箱                           
                            hid_photo.Value = dr["photo"].ToString();//照片
                            hid_signature.Value = dr["signature"].ToString();//电子签名
                            txt_address.Text = dr["address"].ToString();//会员地址
                            txt_hobby.Text = dr["hobby"].ToString();//特殊爱好
                            txt_remark.Text = dr["remark"].ToString();//备注
                            Ghostsel_cuser.Value = dr["cuser"].ToString();
                        }
                    }
                }
                url = "MemCardLists.aspx?id=" + mobileNum + "";
                url1 = "memcardconsumptionLists.aspx?id=" + mobileNum + "";
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "select", "<script>select();</script>");//添加所属区域
            }
            else
            {
                Ghostsel_memid.Value = string.Empty;
                Ghostsel_source.Value = string.Empty;
                Ghostsel_wxaccount.Value = string.Empty;
                Ghostsel_bigcustomer.Value = string.Empty;
                Ghostsel_tel.Value = string.Empty;
                Ghostsel_provinceids.Value = string.Empty;
                Ghostsel_citys.Value = string.Empty;
                Ghostsel_areas.Value = string.Empty;
                Ghostsel_PhoneNum.Value = string.Empty;

                MemberName.Text = string.Empty;
                MemberNumber.Text = string.Empty;
                MemberSex.Text = string.Empty;
                MemberPhone.Text = string.Empty;
                ddl_idtype.SelectedValue = "IDNO";
                txt_IDNO.Text = string.Empty;
                MemberBirthday.Text = string.Empty;
                txt_email.Text = string.Empty;
                txt_address.Text = string.Empty;
                txt_hobby.Text = string.Empty;
                txt_remark.Text = string.Empty;
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (Ghostsel_memcardcodes.Value.Length > 0)
            {
                //获取页面信息
                string memid = Ghostsel_memid.Value;
                string memcode = Ghostsel_memcardcodes.Value;//会员卡号
                string source = Helper.ReplaceString(Ghostsel_source.Value);//会员来源
                string wxaccount = Helper.ReplaceString(Ghostsel_wxaccount.Value);//微信账号
                string tel = Helper.ReplaceString(Ghostsel_tel.Value);//客户电话
                string provinceid = Helper.ReplaceString(Ghostsel_provinceids.Value);//所属省
                string cityid = Helper.ReplaceString(Ghostsel_citys.Value);//所属城市
                string areaid = Helper.ReplaceString(Ghostsel_areas.Value);//所属区域
                string cuser = Helper.ReplaceString(Ghostsel_cuser.Value);

                string buscode = Helper.GetAppSettings("BusCode");
                string strcode = Helper.GetAppSettings("StoCode");
                string bigcustomer = Ghostsel_bigcustomer.Value;//是否大客户
                string cname = Helper.ReplaceString(MemberName.Text);//客户姓名
                string birthday = Helper.ReplaceString(MemberBirthday.Text);//客户生日
                string sex = Helper.ReplaceString(MemberSex.Text);//客户性别
                string mobile = Helper.ReplaceString(MemberPhone.Text);//客户手机号
                string email = Helper.ReplaceString(txt_email.Text);//客户邮箱
                string idtype = Helper.ReplaceString(ddl_idtype.SelectedValue);//证件类型
                string IDNO = Helper.ReplaceString(txt_IDNO.Text);//证件号码
                string photo = Helper.ReplaceString(hid_photo.Value);//照片
                string signature = Helper.ReplaceString(hid_signature.Value);//电子签名
                string address = Helper.ReplaceString(txt_address.Text);//会员地址
                string hobby = Helper.ReplaceString(txt_hobby.Text);//特殊爱好
                string remark = Helper.ReplaceString(txt_remark.Text);//备注
                string status = "1";
                string orderno = "";
                string uuser = "0";
                string ousercode = LoginedUser.UserInfo.username;
                string ousername = LoginedUser.UserInfo.cname;
                //日志信息
                memberslogEntity memlogentity = new memberslogEntity();
                memlogentity.operatetype = "修改";
                memlogentity.stocode = strcode;
                memlogentity.memcode = memcode;
                memlogentity.ocode = ousercode;
                memlogentity.oname = ousername;

                DataTable dt = new DataTable();
                
                dt = bll.Update("0", "0", memid, memcode, source, buscode, strcode, wxaccount, bigcustomer, cname, birthday, sex, mobile, email, tel, idtype, IDNO, provinceid, cityid, areaid, photo, signature, address, hobby, remark, status, orderno, cuser, uuser, ousercode, ousername, memlogentity);
                this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateDetail").Body;
                //显示结果
                ShowResult(dt, errormessage);
                phone_where.Text = MemberPhone.Text;
                Button1_Click(null, null);
            }
        }
    }
}