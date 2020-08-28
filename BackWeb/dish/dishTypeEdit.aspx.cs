using System;
using System.Data;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.systemset
{
    public partial class dishTypeEdit : EditPage
    {
        public string dicid;
        bllTB_DishType bll = new bllTB_DishType();
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
            DataTable dtdict = bll.GetPagingListInfo("", "0", 10000, 1, "  isnull(PKKCode,'0')='0' or PKKCode='' ", "", out recnum, out pagenums);
            BindDropDownListInfo(ddl_pdicid, dtdict, "TypeName", "PKCode", 1);
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where PPKCode='" + id + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
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
                txt_dicname.Text = dr["typename"].ToString();
                if (string.IsNullOrWhiteSpace(dr["ppkcode"].ToString()) || dr["ppkcode"].ToString() == "0")
                {
                    txt_dicname.Enabled = false;
                }

                txt_orderno.Text = dr["Sort"].ToString();
                ddl_status.SelectedValue = dr["status"].ToString();
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string pdicid =this.ddl_pdicid.SelectedValue;
            string dicname =txt_dicname.Text;
            string orderno =txt_orderno.Text;
            string status =ddl_status.SelectedValue;
            string cuser = base.LoginedUser.UserID.ToString();
            string diccode = "";
            DataTable dt = new DataTable();

            if (hidId.Value.Length == 0|| hidId.Value=="0")//添加信息
            {
                string dicid = "";
                bll.Add("0", "0",dicid, pdicid, diccode, dicname, orderno, status);
                hidId.Value = bll.oResult.Data;
                this.PageTitle.Operate = "修改";
            }
            else//修改信息
            {
                TB_DishTypeEntity UEntity = bll.GetEntitySigInfo("where pkcode='" + hidId.Value+"'");
                UEntity.PKKCode =pdicid;
                UEntity.PKCode = hidId.Value;
                UEntity.TypeName = dicname;
                UEntity.Sort =StringHelper.StringToInt(orderno);
                UEntity.TStatus = status;
               bll.Update("0", "0", UEntity);
                this.PageTitle.Operate = "修改";
            }
            //显示结果
            ShowResult(bll.oResult.Code,bll.oResult.Msg, errormessage);
        }
    }
}