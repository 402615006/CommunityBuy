using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb
{
    public partial class StoreDetail : DetailPage
    {
        public string stoid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["stoid"] != null)
                {
                    stoid = Request["stoid"].ToString();
                    SetPage(stoid);
                }
            }
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string stoid)
        {
            bllStore bll = new bllStore();
            DataTable dt = bll.GetPagingSigInfoByBack("0", "0", " where stoid='" + stoid + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                //引用分公司表company的公司编号字段comcode的值
                comcode.InnerHtml = dr["comcode"].ToString();
                //引用商户表Business的商户编号字段buscode的值
                //buscode.InnerHtml = dr["buscode"].ToString();
                //门店编号
                stocode.InnerHtml = dr["stocode"].ToString();
                //门店名称
                cname.InnerHtml = dr["cname"].ToString();
                //门店简称
                sname.InnerHtml = dr["sname"].ToString();
                //门店简码
                bcode.InnerHtml = dr["bcode"].ToString();
                //所属行业
                //indcode.InnerHtml = dr["indcode"].ToString();
                //所在省
                provinceid.InnerHtml = dr["provinceid"].ToString();
                //所在城市
                //cityid.InnerHtml = dr["cityid"].ToString();
                //所在区
                //areaid.InnerHtml = dr["areaid"].ToString();
                //门店地址
                address.InnerHtml = dr["address"].ToString();
                //负责人
                stoprincipal.InnerHtml = dr["stoprincipal"].ToString();
                //负责人联系电话
                stoprincipaltel.InnerHtml = dr["stoprincipaltel"].ToString();
                //门店电话
                tel.InnerHtml = dr["tel"].ToString();
                //门店邮箱
                stoemail.InnerHtml = dr["stoemail"].ToString();
                //门店Logo
                logo.Src = dr["logo"].ToString();
                //门店背景图
                //backgroundimg.Src = dr["backgroundimg"].ToString();
                //门店描述
                descr.InnerHtml = dr["descr"].ToString();
                //门店网址
                stourl.InnerHtml = dr["stourl"].ToString();
                //X坐标
                stocoordx.InnerHtml = dr["stocoordx"].ToString();
                //Y坐标
                stocoordy.InnerHtml = dr["stocoordy"].ToString();
                hidLvData.Value = dr["services"].ToString();
                //最后联网时间
                //netlinklasttime.InnerHtml = dr["netlinklasttime"].ToString();
                calcutime.InnerHtml = DateTime.Parse(dr["calcutime"].ToString()).ToShortDateString();
                buscode.InnerHtml = dr["busname"].ToString();
                status.InnerHtml = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dr["status"].ToString());
                DataTable dtgx = new bllPaging().GetDataTableInfoBySQL("select jprice,[dbo].fnGetDistsName(firtype) as stname,ptype,stopath from storegx where stocode='" + dr["stocode"].ToString() + "'");
                if (dtgx != null && dtgx.Rows.Count > 0)
                {
                    storetype.InnerHtml = dtgx.Rows[0]["stname"].ToString();
                    string paytypes = dtgx.Rows[0]["ptype"].ToString();
                    jprice.InnerHtml = dtgx.Rows[0]["jprice"].ToString();
                    hidstopath.Value = dtgx.Rows[0]["stopath"].ToString();
                    if (paytypes == "0")
                    {
                        paytype.InnerHtml = "先支付";
                    }
                    else
                    {
                        paytype.InnerHtml = "后支付";
                    }
                }
            }
        }
    }
}