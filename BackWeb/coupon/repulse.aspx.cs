using System;
using System.Data;
using CommunityBuy.BLL;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class repulse : ListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    hidId.Value = Request["id"].ToString();
                }
            }
        }

        public void btnrepulse_Click(object sender, EventArgs e)
        {
            string reason =txt_reason.Text);
            if (reason.Length == 0)
            {
                errormessage.InnerText = ErrMessage.GetMessageInfoByCode("sumcoupon_201").Body;
                return;
            }
            if (reason.Length > 64)
            {
                errormessage.InnerText = ErrMessage.GetMessageInfoByCode("sumcoupon_202").Body;
                return;
            }

            //发放门店
            string ffstocode = base.LoginedUser.stocode;
            //发放人编号
            string ffcuser = base.LoginedUser.Name;
            //发放人id
            string ffcusercode = base.LoginedUser.empcode;

            //日志信息
            logentity.module = ErrMessage.GetMessageInfoByCode("sumcoupon_Menu").Body;
            logentity.pageurl = "coupon/repulse.aspx";
            logentity.otype = SystemEnum.LogOperateType.Audit;
            logentity.cuser = base.LoginedUser.Id;
            logentity.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("sumcoupon_963").Body, base.LoginedUser.Id.ToString(), hidId.Value);
            DataTable dt = new bllsumcoupon().Audit("0", "0", base.LoginedUser.Id.ToString(), hidId.Value, "2", reason, ffstocode, ffcuser, ffcusercode, logentity);
            //显示结果
            if (ShowResult(dt, errormessage))
            {
                Script(this.Page, "if(parent.location.href!=null){parent.location.href=parent.location.href;}");
            }
        }


    }
}