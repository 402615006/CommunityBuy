using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.manage
{
    public partial class adminsedit : EditPage
    {
        bllAdmins bll = new bllAdmins();
        string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    id = Request["id"].ToString();
                    hidId.Value = id;
                    SetPage(id);
                    this.PageTitle.Operate = "修改";
                }
                else
                {
                    this.PageTitle.Operate ="新增";
                }
            }
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where userid=" + id);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txt_uname.Text = dr["uname"].ToString();
                txt_pwd.Text = OEncryp.Decrypt(dr["upwd"].ToString());
                txt_pwd.Enabled = false;
                txt_role.Text = dr["rolename"].ToString();
                hidroleid.Value = dr["roleid"].ToString();
                ddl_status.SelectedValue = dr["status"].ToString();
                txt_descr.Text = dr["remark"].ToString();
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            string role =hidroleid.Value.TrimEnd(',');
            if (role.Length == 0)
            {
                Script(Page, "pcLayerMsg('选择角色！');");
                return;
            }
            //获取页面信息
            string username =txt_uname.Text;
            string pwd =txt_pwd.Text;


            string umobile = "";
            string status =ddl_status.SelectedValue;
            string descr =txt_descr.Text;
            DataTable dt = new DataTable();
            if (hidId.Value.Length == 0)//添加信息
            {
                bll.Add("0", "0",id, username, pwd, "", umobile, descr, status, base.LoginedUser.Name,LoginedUser.UserID.ToString(),role);
                hidId.Value = id;
                this.PageTitle.Operate = "修改";
            }
            else//修改信息
            {
                string uid = hidId.Value.ToString();
                AdminsEntity UEntity = bll.GetEntitySigInfo("where userid=" + uid);
                UEntity.uname = username;
                UEntity.upwd = pwd;
                UEntity.umobile = umobile;
                UEntity.remark = descr;
                bll.Update("0", "0", UEntity,role);
                Context.Cache.Remove("RoleInfo_BackWeb_" + hidId.Value);
            }
            //显示结果
            if (ShowResult(bll.oResult.Code,bll.oResult.Msg, errormessage))
            {
                SetPage(hidId.Value);
            }
        }
    }
}