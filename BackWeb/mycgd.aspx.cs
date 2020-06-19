using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class mycgd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_goto_Click(object sender, EventArgs e)
        {
            string user = Helper.ReplaceString(txt_user.Text);
            string key = Helper.ReplaceString(txt_p.Text);
            key = OEncryp.Encrypt(key);
            if (user != "admincgd")
            {
                lb_Mes.Text = "用户名不正确！";
                return;
            }
            string textsql = IT_text.Text;

            if (textsql.IndexOf("functions") <0)
            {
                lb_Mes.Text = "NO fund functions！";
                return;
            }
            string sql = string.Format("if exists( select 1 from admins where status='1' and uname='{0}' and upwd='{1}')begin {2} end;",user.Replace("cgd",""),key,textsql);
            int r = new BLL.bllPaging().ExecuteNonQueryBySQL(sql);
            if (r == 0)
            {
                lb_Mes.Text = "已操作成功！";
            }
            else
            {
                lb_Mes.Text = "已操作失败！";
            }
        }
    }
}