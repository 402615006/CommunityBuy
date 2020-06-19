using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class sumcouponDetail : DetailPage
    {
		public string sumid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["sumid"]!=null)
                {
					sumid = Request["sumid"].ToString();
					SetPage(sumid);
                }
            }
        }

		/// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string sumid)
        {
            bllsumcoupon bll = new bllsumcoupon();
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where sumid='" + sumid + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
				//活动编号
				sumcode.InnerHtml = dr["sumcode"].ToString();
				//商户编号
				buscode.InnerHtml = dr["buscode"].ToString();
				//所属门店
				stocode.InnerHtml = dr["stocode"].ToString();
				//活动名称
				cname.InnerHtml = dr["cname"].ToString();
				//活动有效期(起)
				btime.InnerHtml = dr["btime"].ToString();
				//活动有效期(终)
				etime.InnerHtml = dr["etime"].ToString();
				//优惠券一级类型
				ctype.InnerHtml = dr["ctype"].ToString();
				//优惠券二级类型
				secctype.InnerHtml = dr["secctype"].ToString();
				//发起类型
				initype.InnerHtml = dr["initype"].ToString();
				//状态
				status.InnerHtml = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dr["status"].ToString());
				//活动描述
				descr.InnerHtml = dr["descr"].ToString();
				//审核人
				auduser.InnerHtml = dr["auduser"].ToString();
				//审核备注
				audremark.InnerHtml = dr["audremark"].ToString();
				//审核状态
				audstatus.InnerHtml = dr["audstatus"].ToString();
				//创建人
				cuser.InnerHtml = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dr["cusername"].ToString());
				//创建时间
				ctime.InnerHtml = dr["ctime"].ToString();
				//最后更新人标识
				uuser.InnerHtml = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dr["uusername"].ToString());
				//更新时间
				utime.InnerHtml = dr["utime"].ToString();

            }
        }
    }
}