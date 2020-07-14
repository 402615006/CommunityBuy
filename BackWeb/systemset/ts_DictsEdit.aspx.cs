using System;
using System.Data;
using System.Web.UI.WebControls;
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
                    this.PageTitle.Operate = "修改";
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
            BindDropDownListInfo(ddl_pdicid, dtdict, "dicname", "dicid", 0);
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
            string lng =this.ddl_lng.SelectedValue;
            string pdicid =this.ddl_pdicid.SelectedValue;
            string diccode =txt_diccode.Text;
            string dicname =txt_dicname.Text;
            string dicvalue = "";
            string orderno =txt_orderno.Text;
            string remark =txt_remark.Text;
            string status =ddl_status.SelectedValue;
            string cuser = base.LoginedUser.UserID.ToString();

            DataTable dt = new DataTable();

            if (hidId.Value.Length == 0)//添加信息
            {
                string dicid = "";
                bll.Add("0", "0", dicid,buscode, strcode, dictype, lng, pdicid, diccode, dicname, dicvalue, orderno, remark, status, cuser);
                hidId.Value = bll.oResult.Data;
                this.PageTitle.Operate = "修改";
            }
            else//修改信息
            {
                ts_DictsEntity UEntity = bll.GetEntitySigInfo("where id=" + hidId.Value);
                UEntity.strcode = strcode;
                UEntity.dictype = dictype;
                UEntity.lng = lng;
                UEntity.pdicid =StringHelper.StringToLong(pdicid);
                UEntity.diccode = diccode;
                UEntity.dicname = dicname;
                UEntity.dicvalue = dicvalue;
                UEntity.orderno =StringHelper.StringToInt(orderno);
                UEntity.remark = remark;
                UEntity.status = status;
                UEntity.cuser = StringHelper.StringToInt(cuser);
               bll.Update("0", "0", UEntity);
                this.PageTitle.Operate = "修改";
            }
            //显示结果
            ShowResult(bll.oResult.Code,bll.oResult.Msg, errormessage);
        }
    }
}