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

                txt_mobile.Text = entity.mobile;
                //txt_email.Text = entity
                txt_tel.Text = entity.mobile;

                ddl_provinceid_SelectedIndexChanged(null, null);

                ddl_cityid_SelectedIndexChanged(null, null);

                txt_remark.Text = entity.remark;

            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string wxaccount =txt_wxaccount.Text;


            string mobile = txt_mobile.Text;

            string remark = txt_remark.Text;
            string status = "1"; 


            if (this.hidId.Value.Length!= 0)//添加信息
            {
                membersEntity UEntity = bll.GetEntitySigInfo(" where memid='" + hidId.Value + "'");
                if (UEntity.memcode.Length > 0)
                {
                    UEntity.wxaccount = wxaccount;

                    UEntity.mobile = mobile;

                    UEntity.remark = remark;
                    UEntity.status = status;

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