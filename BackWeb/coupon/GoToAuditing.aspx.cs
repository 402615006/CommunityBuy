using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class GoToAuditing : EditPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)
             {
                 hidId.Value = Request["id"].ToString();
             }
        }
        /// <summary>
        /// 通过拒绝事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_save_Click(object sender, EventArgs e)
        {
            string audstatus = "1";//1-通过，2-拒绝
            string formpage = string.Empty;
           
            if (Request["formpage"] != null)
            {
                formpage = Request["formpage"].ToString();
            }
            string type = hidtype.Value;
            if (type != "1")
            {
                audstatus = "2";
            }
            string audremark = Helper.ReplaceString(txt_remark.Value);
            DataTable dt = new DataTable();
            switch (formpage.ToLower())
            {
                case "couponpresent"://赠送方案审核
                    dt = new bllmarketingN().AuditStatus("", "0", hidId.Value, audstatus, LoginedUser.UserInfo.empcode.ToString(),LoginedUser.UserInfo.cname, audremark);
                    break;
            }

            if (ShowResult(dt, errormessage))//操作成功关闭当前窗口
            {
                Script(this.Page, "closeparent();");
            }
        }
    }
}