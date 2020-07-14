using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.manage
{
    /// <summary>
    /// 功能设置
    /// </summary>
    public partial class rolefunctionedit : EditPage
    {
        public string id;
        public string rolName;
        public string rolMemo;
        public string rolStatus;

        bllTB_RoleFunction _bll = new bllTB_RoleFunction();
        TB_RolesEntity _Entity = new TB_RolesEntity();
        bllroles bllroles = new bllroles();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    id = Request["id"].ToString();
                    if (Request["copying"] != null)
                    {
                        this.PageTitle.Operate ="新增";
                    }
                    else
                    {
                        hidId.Value = id;
                        if (Request["type"] != "edit")
                        {
                            this.PageTitle.Operate = "详情";
                            Script(this.Page, "hidebutton()");
                        }
                        else
                        {
                            this.PageTitle.Operate = "修改";
                        }
                    }
                    SetPage(id);
                }
                else
                {
                    this.PageTitle.Operate ="新增";
                }
                MenuList.InnerHtml = GetMenuTable(StringHelper.StringToInt(id));
            }
        }


        public void SetPage(string RId)
        {
            DataTable dt = _bll.GetRoleFunctionInfoList("","",RId);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                if (Request["copying"] != null)
                {
                    rol_name.Text = dr["rolename"].ToString() + "Copy";
                }
                else
                {
                    rol_name.Text = dr["rolename"].ToString();
                }
                rol_descr.Text = dr["roledescr"].ToString();
                ddl_status.SelectedValue = dr["roledescr"].ToString();
            }
        }

        public string GetMenuTable(int Rid)
        {
            DataTable dt = _bll.GetRoleFunctionInfoList("0", "0", Rid.ToString());
            //查询出所有的功能，并显示。
            StringBuilder html = new StringBuilder(256);
            DataRow[] drOne = dt.Select("Pid=0", "id asc");//一级菜单
            DataRow[] drThree = null;
            DataRow[] drFour = null;
            html.Append("<table class=\"list\" cellpadding=\"0\" cellspacing=\"1\" width=\"100%\">");
            html.Append("<tr class=\"list_line\">");

            html.Append("<td style=\"width:5%;\">");
            string choose = "";
            if (Request["id"] != null)
            {
                choose = "checked=\"checked\"";
            }
            html.AppendFormat("<input id=\"chkAll\" type=\"checkbox\" " + choose + " onclick=\"CheckAll(this);\"/>");
            html.Append("</td>");

            html.Append("<td style=\"width:25%;\">");
            html.Append("业务功能");
            html.Append("</td>");

            html.Append("<td style=\"width:70%;\">");
            html.Append("功能点");
            html.Append("</td>");

            html.Append("</tr>");
            foreach (DataRow dr1 in drOne)
            {
                html.Append("<tr>");
                html.Append("<td>");
                html.Append("&nbsp;");
                html.Append("</td>");

                html.Append("<td>");
                html.AppendFormat("<input id=\"m{0}\" onclick=\"setMenuuMain(this);\" type=\"checkbox\" value='{0}' {1}/>", dr1["id"], Check(StringHelper.StringToInt(dr1["funid"].ToString())));
                html.AppendFormat("╋{0}", dr1["name"]);
                html.Append("</td>");

                html.Append("<td>");
                html.Append("&nbsp");
                html.Append("</td>");

                html.Append("</tr>");

                DataRow[] drTwo = dt.Select("level=2 and Pid=" + dr1["id"] + "", "id asc");//二级菜单

                foreach (DataRow dr2 in drTwo)
                {
                    html.Append("<tr>");
                    html.Append("<td>");
                    html.Append("&nbsp;");
                    html.Append("</td>");

                    html.Append("<td>");
                    html.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"m{0}_{1}\" onclick=\"setMenuuMain(this);\" type=\"checkbox\" value='{1}' {2}/>", dr1["id"], dr2["id"], Check(StringHelper.StringToInt(dr2["funid"].ToString())));
                    html.AppendFormat("┝{0}", dr2["name"]);
                    html.Append("</td>");

                    html.Append("<td>");
                    html.Append("&nbsp;");
                    html.Append("</td>");

                    html.Append("</tr>");

                    drThree = dt.Select("level=3 and Pid=" + dr2["id"] + "", "id asc");//三级页面
                    foreach (DataRow dr3 in drThree)
                    {
                        html.Append("<tr>");
                        html.Append("<td>");
                        html.Append("&nbsp;");
                        html.Append("</td>");

                        html.Append("<td>");
                        html.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"m{0}_{1}_{2}\" onclick=\"setMenuuMain(this);\" type=\"checkbox\" value='{2}' {3}/>", dr1["id"], dr2["id"], dr3["id"], Check(StringHelper.StringToInt(dr3["funid"].ToString())));
                        html.AppendFormat("┝{0}", dr3["name"]);
                        html.Append("</td>");
                        html.Append("<td>");
                        //四级菜单
                        drFour = dt.Select("level=4 and Pid=" + dr3["id"] + "", "id asc");//四级按钮（toolbar）添，修，删
                        foreach (DataRow dr4 in drFour)
                        {
                            html.AppendFormat("<input id=\"m{0}_{1}_{2}_{3}\" onclick=\"setMenuuMain(this);\" type=\"checkbox\" value='{3}' {4}/>{5}&nbsp;&nbsp;&nbsp;", dr1["id"], dr2["id"], dr3["id"], dr4["id"], Check(StringHelper.StringToInt(dr4["funid"].ToString())), dr4["name"]);
                        }
                    }
                    if (drThree.Length == 0)
                    {
                        html.Append("&nbsp");
                    }
                    html.Append("</td>");
                    html.Append("</tr>");
                }
            }
            html.Append("</table>");
            return html.ToString();
        }

        public string Check(int isok)
        {
            if (isok > 0)
            {
                return string.Format("checked=\"checked\"");
            }
            else
            {
                return "";
            }
        }

        private string[] GetArray(string rolMas)
        {
            string mas = rolMas.TrimEnd(',');

            string[] array = null;
            if (mas.Length > 0)
            {
                array = mas.Split(',');
            }
            return array;
        }

        protected void Save_btn_Click(object sender, EventArgs e)
        {
            rolesEntity ROLMAEntity = new rolesEntity();
            ROLMAEntity.cname = rol_name.Text;
            ROLMAEntity.descr = rol_descr.Text;
            ROLMAEntity.status = ddl_status.SelectedValue;
            if (hidId.Value.Length == 0)
            {
                bllroles.AddRITMAS(ROLMAEntity, GetArray(this.HidfunIdStr.Value));
                errormessage.InnerHtml = bllroles.oResult.Msg;
            }
            else
            {
                ROLMAEntity.roleid = StringHelper.StringToInt(hidId.Value);
                bllroles.Update("","",ROLMAEntity, this.HidfunIdStr.Value);
                if (bllroles.oResult.Code == "1")
                {
                    MenuList.InnerHtml = GetMenuTable(StringHelper.StringToInt(hidId.Value));
                }
                errormessage.InnerHtml = bllroles.oResult.Msg;
                this.PageTitle.Operate = "修改";

            }
        }
    }
}