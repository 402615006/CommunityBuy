using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb.systemset
{
    public partial class ts_DictsDetail : DetailPage
    {
		public string dicid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["dicid"]!=null)
                {
					dicid = Request["dicid"].ToString();
					SetPage(dicid);
                }
            }
        }

		/// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string dicid)
        {
            bllts_Dicts bll = new bllts_Dicts();
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where dicid='" + dicid + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
				//引用商户表Business的商户编号字段buscode的值
				buscode.InnerHtml = dr["buscode"].ToString();
				//门店编号
				strcode.InnerHtml = dr["strcode"].ToString();
				//类别
				dictype.InnerHtml = dr["dictype"].ToString();
				//语言代码
				lng.InnerHtml = dr["lng"].ToString();
				//父ID
				pdicid.InnerHtml = dr["pdicid"].ToString();
				//字典编号
				diccode.InnerHtml = dr["diccode"].ToString();
				//字典名称
				dicname.InnerHtml = dr["dicname"].ToString();
				//字典值
				dicvalue.InnerHtml = dr["dicvalue"].ToString();
				//排序号
				orderno.InnerHtml = dr["orderno"].ToString();
				//备注
				remark.InnerHtml = dr["remark"].ToString();
				//有效状态（0无效，1有效）
				status.InnerHtml = dr["status"].ToString() == "1" ? "有效" : "无效";
				//引用系统用户表ts_admins的userid字段值


            }
        }
    }
}