using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class ts_syssetDetail : DetailPage
    {
        public string setid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["setid"] != null)
                {
                    setid = Request["setid"].ToString();
                    SetPage(setid);
                }
            }
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string setid)
        {
            bllts_sysset bll = new bllts_sysset();
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where setid='" + setid + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                //键
                key.InnerHtml = dr["key"].ToString();
                //值
                val.InnerHtml = dr["val"].ToString();
                //有效状态（0无效，1有效）
                status.InnerHtml = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dr["status"].ToString());
                //描述
                descr.InnerHtml = dr["descr"].ToString();
                //创建时间
                //ctime.InnerHtml = dr["ctime"].ToString();


                //
                ctime.InnerHtml = DateTime.Parse(dr["ctime"].ToString()).ToShortDateString(); ;




            }
        }
    }
}