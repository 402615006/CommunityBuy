using System;
using System.Data;
using System.Text;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BackWeb.UserControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class ChangePhoneNumRefer : Common.ListPage
    {
        bllStockMaterial bll = new bllStockMaterial();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bllBase bl = new bllBase();
            string GUID = LoginedUser.UserInfo.LoginGuid.ToString();
            string UID = LoginedUser.UserInfo.Id.ToString();
            string nowPhoneNumber = string.Empty;
            string VCode = CTextBox1.Text;////验证码
            string oldPhoneNums = oldPhoneNum.Text;//老手机号

            DataTable checkPhoneNumber = bll.getDataTableBySql("SELECT * FROM dbo.members WHERE  mobile='" + oldPhoneNums + "'");
            if (checkPhoneNumber != null && checkPhoneNumber.Rows.Count == 1)
            {
                string vercode = string.Empty;
                bool bls = LoginUniqueness.MobileMesSendByTemp1(nowPhoneNumber, "更改手机号码", ref vercode);//给新手机号发送验证码
                DataTable checkVCode = bl.CheckMobileVerificationCode(GUID, UID, nowPhoneNumber, VCode);//验证手机验证码
                if (checkVCode != null && checkVCode.Rows.Count > 0)
                {
                    foreach (DataRow dr in checkVCode.Rows)
                    {
                        if (dr["type"].ToString() == "0")
                        {
                            bll.getDataTableBySql("update dbo.members set mobile='" + nowPhoneNumber + "'  WHERE  mobile='" + oldPhoneNums + "'");
                            ClientScript.RegisterStartupScript(ClientScript.GetType(), "Success", "<script>Success();</script>");
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(ClientScript.GetType(), "Fail", "<script>Fail();</script>");
                        }
                    }
                }
            }
        }
    }
}