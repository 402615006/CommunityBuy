using System;
using System.Data;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb
{
    public partial class StoreEdit : EditPage
    {
        public string stoid;
        string buscode = Helper.GetAppSettings("BusCode");
        bllStore bll = new bllStore();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindProvince();
                BindStoreType();
                if (Request["id"] != null)
                {
                    txt_stocode.Enabled = false;
                    stoid = Request["id"].ToString();
                    hidId.Value = stoid;
                    SetPage(hidId.Value);

                    this.PageTitle.Operate = "修改";
                }
                else
                {
                    txt_stocode.Enabled = true;
                    this.PageTitle.Operate ="新增";
                }
            }
        }


        public void BindStoreType()
        {
            DataTable dtdict = new bllts_Dicts().GetPagingListInfoByParentCode("","",int.MaxValue,1,"IndustryInvolved","",out int recnums,out int pagenums);
            BindDropDownListInfo(this.ddl_storetype, dtdict, "dicname", "diccode",  0);
        }

        private void bindProvince()
        {
            DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from [dbo].[provinces]");
            BindDropDownListInfo(this.ddl_provinceid, dtdict, "province", "provinceid", 0);
            ddl_provinceid_SelectedIndexChanged(null, null);
        }

        protected void ddl_provinceid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fiter = " parentid =" + this.ddl_provinceid.SelectedValue;
            DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from [dbo].[citys] where " + fiter);
            BindDropDownListInfo(this.ddl_cityid, dtdict, "city", "cityid", 0);
            ddl_cityid_SelectedIndexChanged(null, null);
        }

        protected void ddl_cityid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fiter = " parentid =" + this.ddl_cityid.SelectedValue;
            DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from [dbo].[areas] where " + fiter);
            BindDropDownListInfo(this.ddl_areaid, dtdict, "area", "areaid", 0);
            
        }

        protected void ddl_areaid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fiter = " areaid =" + this.ddl_areaid.SelectedValue;
            DataTable dtdict = new bllPaging().GetDataTableInfoBySQL("select * from BusinessCenter where " + fiter);
            BindDropDownListInfo(this.ddl_sq, dtdict, "name", "id", 0);
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id) 
        {
            DataTable dt = bll.GetPagingSigInfo("0", "0"," where stoid=" + id);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                txt_stocode.Text = dr["stocode"].ToString();
                txt_cname.Text = dr["cname"].ToString();
                txt_sname.Text = dr["sname"].ToString();
                txt_bcode.Text = dr["bcode"].ToString();
                this.ddl_provinceid.SelectedValue = dr["provinceid"].ToString();
                ddl_provinceid_SelectedIndexChanged(null, null);
                this.ddl_cityid.SelectedValue = dr["cityid"].ToString();
                ddl_cityid_SelectedIndexChanged(null, null);
                this.ddl_areaid.SelectedValue = dr["areaid"].ToString();
                txt_address.Text = dr["address"].ToString();
                txt_stoprincipal.Text = dr["stoprincipal"].ToString();
                txt_stoprincipaltel.Text = dr["stoprincipaltel"].ToString();
                txt_tel.Text = dr["tel"].ToString();

                txt_descr.Text = dr["descr"].ToString();
                txt_stourl.Text = dr["stourl"].ToString();
                txt_stocoordx.Text = dr["stocoordx"].ToString();
                txt_stocoordy.Text = dr["stocoordy"].ToString();
                txt_jprice.Text = dr["stocoordy"].ToString();
                ddl_status.SelectedValue = dr["status"].ToString();
                logo.Src = dr["logo"].ToString();
                hid_logo.Value = dr["logo"].ToString();
                txt_btime.Text = dr["btime"].ToString();
                txt_etime.Text = dr["etime"].ToString();
  
                DataTable dtgx = new bllPaging().GetDataTableInfoBySQL("select top 1 jprice,firtype,ptype,sqcode,stopath from storegx where stocode='" + txt_stocode.Text + "';");
                if (dtgx != null && dtgx.Rows.Count > 0)
                {
                    txt_jprice.Text = dtgx.Rows[0]["jprice"].ToString();
                    ddl_storetype.SelectedValue = dtgx.Rows[0]["firtype"].ToString();
                    ddl_sq.SelectedValue = dtgx.Rows[0]["sqcode"].ToString();
                    hidstopath.Value = dtgx.Rows[0]["stopath"].ToString();
                }
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string stocode =txt_stocode.Text;
            string cname =txt_cname.Text;
            string sname =txt_sname.Text;
            string bcode =txt_bcode.Text;
            string indcode = "";
            string provinceid =this.ddl_provinceid.SelectedValue;
            string cityid =this.ddl_cityid.SelectedValue;
            string areaid =this.ddl_areaid.SelectedValue;
            string address =txt_address.Text;
            string sqcode= this.ddl_sq.SelectedValue;
            string stoprincipal =txt_stoprincipal.Text;
            string stoprincipaltel =txt_stoprincipaltel.Text;
            string tel =txt_tel.Text;
            string logo =this.hid_logo.Value;
            string descr =txt_descr.Text;
            string stourl =txt_stourl.Text;
            string stocoordx =txt_stocoordx.Text;
            string stocoordy =txt_stocoordy.Text;
            string remark = "";
            string status =ddl_status.SelectedValue;
            string cuser = "1";
            string stopath = hidstopath.Value;
            string services = "";
            string btime = txt_btime.Text;
            string etime = txt_etime.Text;
            string recommended =txt_recommended.Text;
            string jprice = txt_jprice.Text;
            if (hidId.Value.Length == 0|| hidId.Value=="0")//添加信息
            {
                bll.Add("0", "0", stoid, stocode, cname, sname, bcode, indcode, provinceid, cityid, areaid, address, stoprincipal, stoprincipaltel, tel, logo, "", stopath, services, descr, stourl, stocoordx, stocoordy,
                    "",remark, status,cuser,btime, etime,sqcode,jprice);
                hidId.Value = bll.oResult.Data;
                this.PageTitle.Operate = "修改";
            }
            else//修改信息
            {
                StoreEntity UEntity = bll.GetEntitySigInfo(" where stoid=" + hidId.Value);
                UEntity.cname = cname;
                UEntity.sname = sname;
                UEntity.bcode = bcode;
                UEntity.indcode = indcode;
                UEntity.provinceid = StringHelper.StringToInt(provinceid);
                UEntity.cityid =StringHelper.StringToInt(cityid);
                UEntity.areaid = StringHelper.StringToInt(areaid);
                UEntity.stoprincipal = stoprincipal;
                UEntity.stoprincipaltel = stoprincipaltel;
                UEntity.tel = tel;
                UEntity.logo = logo;
                UEntity.stopath = stopath;
                //UEntity.services = services;
                UEntity.descr = descr;
                UEntity.stourl = stourl;
                UEntity.stocoordx = stocoordx;
                UEntity.stocoordy = stocoordy;
                UEntity.recommended = recommended;
                UEntity.remark = remark;
                UEntity.status = status;
                UEntity.btime = btime;
                UEntity.etime = etime;
                UEntity.sqcode = StringHelper.StringToInt(sqcode);
                bll.Update("0", "0", UEntity);
            }
            //显示结果
            ShowResult(bll.oResult.Code,bll.oResult.Msg, errormessage);
            SetPage(hidId.Value);
        }
    }
}