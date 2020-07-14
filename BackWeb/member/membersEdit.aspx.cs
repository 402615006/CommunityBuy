using CommunityBuy.WebControl;
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
                    SetPage(hidId.Value, from);
                    this.PageTitle.Operate = "修改";
                }
                else
                {
                    this.PageTitle.Operate ="新增";
                }
            }
        }

        private void BindDDL()
        {
            int recnums, pagenums;
            DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from [dbo].[provinces]");
            BindDropDownListInfo(this.ddl_provinceid, dtdict, "province", "provinceid", 2);

            DataTable idType = new bllts_Dicts().GetPagingListInfoByParentCode("","",int.MaxValue,1, "IdType", "",out recnums,out pagenums);
            BindDropDownListInfo(this.ddl_idtype, idType, "dicname", "diccode", 2);
        }

        protected void ddl_provinceid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddl_provinceid.SelectedValue))
            {
                int recnums, pagenums;
                string fiter = " parentid =" + this.ddl_provinceid.SelectedValue;
                DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from [dbo].[citys] where " + fiter);
                BindDropDownListInfo(this.ddl_cityid, dtdict, "city", "cityid", 2);
            }
        }

        protected void ddl_cityid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddl_cityid.SelectedValue))
            {
                int recnums, pagenums;
                string fiter = " parentid =" + this.ddl_cityid.SelectedValue;
                DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from [dbo].[areas] where " + fiter);
                BindDropDownListInfo(this.ddl_areaid, dtdict, "area", "areaid", 2);
            }
        }


        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id, string from)
        {
            string filter = " where memcode='" + id + "'";

            membersEntity entity = bll.GetEntitySigInfo(filter);
            if (entity != null )
            {
                hidId.Value = entity.memid;
                url = "MemberCardShow.aspx?id=" + hidId.Value;
                memcode = hidId.Value;
                txt_wxaccount.Text = entity.wxaccount;
                txt_cname.Text = entity.cname;
                txt_birthday.Text = entity.birthday.ToString("yyyy-MM-dd");
                txt_sex.SelectedValue = entity.sex;
                txt_mobile.Text = entity.mobile;
                //txt_email.Text = entity
                txt_tel.Text = entity.mobile;
                ddl_idtype.SelectedValue = entity.idtype;
                txt_IDNO.Text = entity.IDNO;
                this.ddl_provinceid.SelectedValue = entity.provinceid.ToString();
                ddl_provinceid_SelectedIndexChanged(null, null);
                this.ddl_cityid.SelectedValue = entity.cityid.ToString();
                ddl_cityid_SelectedIndexChanged(null, null);
                ddl_areaid.SelectedValue = entity.areaid.ToString();
                txt_address.Text = entity.address;
                txt_remark.Text = entity.remark;
                txt_photo.ImageUrl = entity.photo;
                hid_photo.Value = entity.photo;
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string memid = "";
            string wxaccount =txt_wxaccount.Text;
            string cname = txt_cname.Text;
            string IDNO = txt_IDNO.Text;
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
            string sex = txt_sex.Text;
            string mobile = txt_mobile.Text;
            string idtype = ddl_idtype.SelectedValue;
            string provinceid = ddl_provinceid.SelectedValue;
            string cityid = ddl_cityid.SelectedValue;
            string areaid = ddl_areaid.SelectedValue;
            string photo = hid_photo.Value;
            string address = txt_address.Text;
            string remark = txt_remark.Text;
            string status = "1"; 
            string cuser = base.LoginedUser.UserID.ToString();


            if (this.hidId.Value.Length == 0)//添加信息
            {
                bll.Add("0", "0", memid, "", "", cname, birthday, sex, mobile, idtype, IDNO, provinceid, cityid, areaid, photo, address, remark, status, cuser);
                hidId.Value = bll.oResult.Data;

                this.PageTitle.Operate = "修改";
            }
            else//修改信息
            {
                membersEntity UEntity = bll.GetEntitySigInfo(" where memid='" + hidId.Value + "'");
                if (UEntity.memcode.Length > 0)
                {
                    UEntity.wxaccount = wxaccount;
                    UEntity.cname = cname;
                    UEntity.birthday = StringHelper.StringToDateTime(birthday);
                    UEntity.sex = sex;
                    UEntity.mobile = mobile;
                    UEntity.idtype = idtype;
                    UEntity.IDNO = IDNO;
                    UEntity.provinceid = StringHelper.StringToInt(provinceid);
                    UEntity.cityid = StringHelper.StringToInt(cityid);
                    UEntity.areaid = StringHelper.StringToInt(areaid);
                    UEntity.photo = photo;
                    UEntity.address = address;
                    UEntity.remark = remark;
                    UEntity.status = status;
                    UEntity.cuser = StringHelper.StringToLong(cuser);
                    bll.Update("0", "0", UEntity);
                }
                this.PageTitle.Operate = "修改";
            }
            //显示结果
            ShowResult(bll.oResult.Code,bll.oResult.Msg, errormessage);
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