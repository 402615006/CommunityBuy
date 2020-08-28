using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.dish
{
    /// <summary>
    /// 功能设置
    /// </summary>
    public partial class dishmethodlist : EditPage
    {
        bllPaging bll=new bllPaging();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string discode = Request["id"];
                if (discode == null)
                {
                    discode = "0";
                }
                else
                {
                    hidId.Value = discode;
                }
                string html=GetMenuTable(discode);
                MenuList.InnerHtml = html;
            }
        }

        public string GetMenuTable(string discode)
        {
            DataTable dt = bll.GetDataTableInfoBySQL("select * from TR_DishesMethods");

            DataTable dtRoleFunctions = bll.GetDataTableInfoBySQL("select * from TR_DishesMethods where discode like(',"+discode+",')");
            //查询出所有的功能，并显示。
            StringBuilder html = new StringBuilder(256);
            List<string> listTypes = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string typename = dr["typename"].ToString();
                if (!listTypes.Contains(typename))
                {
                    listTypes.Add(typename);
                }
            }
            html.Append("<table class=\"list\" cellpadding=\"0\" cellspacing=\"1\" width=\"100%\">");
            html.Append("<tr class=\"list_line\">");

            html.Append("<td style=\"width:5%;\">");
            string choose = "";
            if (Request["id"] != null)
            {
                choose = "checked=\"checked\"";
            }
            //html.AppendFormat("<input id=\"chkAll\" type=\"checkbox\" " + choose + " onclick=\"CheckAll(this);\"/>");
            html.Append("</td>");

            html.Append("<td style=\"width:25%;\">");
            html.Append("分类");
            html.Append("</td>");

            html.Append("<td style=\"width:70%;\">");
            html.Append("规格");
            html.Append("</td>");

            html.Append("</tr>");
            foreach (string typename in listTypes)
            {
                html.Append("<tr>");
                html.Append("<td>");
                html.Append("&nbsp;");
                html.Append("</td>");

                html.Append("<td>");
                //html.AppendFormat("<input id=\"m{0}\" onclick=\"setMenuuMain(this);\" type=\"checkbox\" value='{0}' {1}/>","0", Check(dr1["id"].ToString(), dtRoleFunctions));
                html.AppendFormat("╋{0}", typename);
                html.Append("</td>");

                html.Append("<td>");
                html.Append("&nbsp");
                html.Append("</td>");

                html.Append("</tr>");

                DataRow[] drTwo = dt.Select("typename='" +typename + "'", "");//二级菜单

                foreach (DataRow dr2 in drTwo)
                {
                    html.Append("<tr>");
                    html.Append("<td>");
                    html.Append("&nbsp;");
                    html.Append("</td>");

                    html.Append("<td>");
                    html.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;<input id=\"m{0}_{1}\" onclick=\"setMenuuMain(this);\" type=\"checkbox\" value='{1}' {2}/>", "0", dr2["id"], Check(dr2["id"].ToString(), dtRoleFunctions));
                    html.AppendFormat("┝{0}", dr2["name"]);
                    html.Append("</td>");

                    html.Append("<td>");
                    html.Append("&nbsp;");
                    html.Append("</td>");
                    html.Append("</tr>");
                    html.Append("</td>");
                    html.Append("</tr>");
                }
            }
            html.Append("</table>");
            return html.ToString();
        }

        public string Check(string funid,DataTable dtRolefunction)
        {
            DataRow[] drs = dtRolefunction.Select("id="+ funid);
            if (drs != null&& drs .Length> 0)
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
            string discode=hidId.Value;
            if (!string.IsNullOrWhiteSpace(discode))
            {
                string ids = this.HidfunIdStr.Value;
                ids = ids.TrimStart(',');
                ids = ids.TrimEnd(',');

                string sql = "update TR_DishesMethods set discode=replace(discode,'," + discode + ",','');";
                sql+= "update TR_DishesMethods set discode=discode+'," + discode + ",' where id in(" + ids + ");";
                int recnums = bll.ExecuteNonQueryBySQL(sql);
                if (recnums >= 0)
                {
                    errormessage.InnerHtml = "操作成功";
                }
                else
                {
                    errormessage.InnerHtml = "操作失败";
                }
            }
            else
            {
                errormessage.InnerHtml = "你选择菜品";
            }

           
        }
    }
}