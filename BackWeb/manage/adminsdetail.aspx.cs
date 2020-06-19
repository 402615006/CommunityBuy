using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;

namespace CommunityBuy.BackWeb.manage
{
    public partial class adminsdetail : DetailPage
    {
        public string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {
                id = Request["id"].ToString();
                SetPage(id);
            }
        }
        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            bllAdmins bll = new bllAdmins();
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where userid=" + id);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                uname.InnerHtml = dr["uname"].ToString();
                pwd.InnerHtml = dr["upwd"].ToString();
                realname.InnerHtml = dr["realname"].ToString();
                rolname.InnerHtml = dr["rolename"].ToString();
                empcode.InnerHtml = dr["empcode"].ToString();
                umobile.InnerHtml = dr["umobile"].ToString();
                rolname.InnerHtml = dr["rolename"].ToString();
                descr.InnerHtml = dr["remark"].ToString();
            }
        }
    }
}