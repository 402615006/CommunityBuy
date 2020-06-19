using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

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
                txt_empcode.Text = dr["empcode"].ToString() + " " + dr["empcodename"].ToString();
                hidempcode.Value = dr["empcode"].ToString();
                ddl_status.SelectedValue = dr["status"].ToString();
                txt_descr.Text = dr["remark"].ToString();

                hidstore.Value = dr["stocode"].ToString();
                ddl_scope.SelectedValue = dr["scope"].ToString();
                txt_stocode.Text = dr["storename"].ToString();
                lblstoname.Text = dr["empstoname"].ToString();
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            string role = Helper.ReplaceString(hidroleid.Value).TrimEnd(',');
            if (role.Length == 0)
            {
                Script(Page, "pcLayerMsg('选择角色！');");
                return;
            }
            //获取页面信息
            string username = Helper.ReplaceString(txt_uname.Text);
            string pwd = Helper.ReplaceString(txt_pwd.Text);
            string cname = Helper.ReplaceString(txt_empcode.Text);
            if (cname.Split(' ').Length >= 2)
            {
                cname = cname.Split(' ')[1];
            }

            string umobile = "";
            string empcode = Helper.ReplaceString(hidempcode.Value);
            string status = Helper.ReplaceString(ddl_status.SelectedValue);
            string descr = Helper.ReplaceString(txt_descr.Text);

            string scope = ddl_scope.SelectedValue;
            string stocode = hidstore.Value;
            if (scope == "2")
            {
                stocode = "";
            }

            logentity.module = ErrMessage.GetMessageInfoByCode("admins_Menu").Body;
            logentity.pageurl = "adminsedit.aspx";
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());
            DataTable dt = new DataTable();
            if (hidId.Value.Length == 0)//添加信息
            {
                dt = bll.Add("0", "0", out id, username, pwd, cname, umobile, empcode, descr, status, LoginedUser.UserInfo.Id.ToString(), "0", role, scope, stocode, string.Empty, string.Empty, logentity);
                hidId.Value = id;
                this.PageTitle.Operate = "修改";
            }
            else//修改信息
            {
                logentity.otype = SystemEnum.LogOperateType.Edit;
                dt = bll.Update("0", "0", hidId.Value, username, pwd, cname, umobile, empcode, descr, status, LoginedUser.UserInfo.Id.ToString(), LoginedUser.UserInfo.Id.ToString(), role, scope, stocode, string.Empty, string.Empty, logentity);
                WebCache.Remove("RoleInfo_BackWeb_" + hidId.Value);
            }
            //显示结果
            if (ShowResult(dt, errormessage))
            {
                SetPage(hidId.Value);
            }
        }
    }
}