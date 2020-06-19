using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class tl_loginlogDetail : DetailPage
    {
		public string logid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["logid"]!=null)
                {
					logid = Request["logid"].ToString();
					SetPage(logid);
                }
            }
        }

		/// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string logid)
        {
            blltl_loginlog bll = new blltl_loginlog();
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where logid='" + logid + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
				//引用商户表Business的商户编号字段buscode的值
				buscode.InnerHtml = dr["buscode"].ToString();
				//门店编号
				strcode.InnerHtml = dr["strcode"].ToString();
				//引用系统用户表ts_admins的userid字段值
				userid.InnerHtml = dr["userid"].ToString();
				//登录姓名
				cname.InnerHtml = dr["cname"].ToString();
				//登录IP
				ip.InnerHtml = dr["ip"].ToString();
				//日志信息
				logcontent.InnerHtml = dr["logcontent"].ToString();
				//创建时间
				ctime.InnerHtml = dr["ctime"].ToString();

            }
        }
    }
}