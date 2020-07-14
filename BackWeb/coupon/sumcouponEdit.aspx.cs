using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class sumcouponEdit : EditPage
    {
        public string sumid;
        bllsumcoupon bll = new bllsumcoupon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    sumid = Request["id"].ToString();
                    hidId.Value = sumid;
                    SetPage(hidId.Value);
                    this.PageTitle.Operate = "修改";
                }
                else
                {
                    //this.txt_btime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    //this.txt_etime.Text = this.txt_btime.Text;
                    this.PageTitle.Operate ="新增";
                }
            }
        }

        //// <summary>
        ///// 绑定门店信息
        ///// </summary>
        //private void BindStoreInfo()
        //{
        //    string buscode = Helper.GetAppSettings("BusCode");
        //    DataTable dt = new bllStore().GetListInfo("", "0", buscode, "");
        //    Helper.BindDropDownListForSearch(ddl_stocode, dt, "cname", "stocode", 2);
        //}

        ///// <summary>
        /////  绑定字典信息
        ///// </summary>
        //private void BindDictInfo()
        //{
        //    DataTable dt = new bllts_Dicts().GetDictsListByEnum("", "0", SystemEnum.DictList.CouponSecType);
        //    Helper.BindDropDownListForSearch(ddl_secctype, dt, "dicname", "diccode", 0);
        //}

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where sumid='" + id + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                txt_sumcode.Text = dr["sumcode"].ToString();
                Hidsumcode.Value = dr["sumcode"].ToString();
                //ddl_stocode.SelectedValue = dr["stocode"].ToString();
                txt_cname.Text = dr["cname"].ToString();
                //txt_btime.Text = StringHelper.StringToDateTime(dr["btime"].ToString()).ToString("yyyy-MM-dd");
                //txt_etime.Text = StringHelper.StringToDateTime(dr["etime"].ToString()).ToString("yyyy-MM-dd");
                ddl_ctype.SelectedValue = dr["ctype"].ToString();
                Hidctype.Value = dr["ctype"].ToString();
                //ddl_secctype.SelectedValue = dr["secctype"].ToString();
                ddl_initype.SelectedValue = dr["initype"].ToString();
                //ddl_status.SelectedValue = dr["status"].ToString();
                txt_descr.Value = dr["descr"].ToString();
                //Script(Page, "gotocouponpage();");
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string sumcode =txt_sumcode.Text);
            string stocode = base.LoginedUser.stocode;
            if (string.IsNullOrEmpty(stocode))
            {
                stocode = Helper.GetAppSettings("Stocode");
            }
            else
            {
                string[] stocode_arr = stocode.Split(',');
                if (stocode_arr.Length > 0)
                {
                    stocode = stocode_arr[0];
                }
            }
            string buscode = Helper.GetAppSettings("BusCode");
            hidStocode.Value = stocode;
            string cname =txt_cname.Text);
            string btime ="1900-01-01");
            string etime ="1900-01-01");
            string ctype =ddl_ctype.SelectedValue);
            string secctype = "";
            string initype =ddl_initype.SelectedValue);
            string status = "1";
            string descr =txt_descr.Value);
            string cuser = base.LoginedUser.Id.ToString();
            string uuser = base.LoginedUser.Id.ToString();
            //日志信息
            logentity.module = ErrMessage.GetMessageInfoByCode("coupons_Menu").Body;
            logentity.pageurl = "sumcouponedit.aspx";
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.cuser = StringHelper.StringToLong(base.LoginedUser.Id.ToString());

            DataTable dt = new DataTable();
            if (hidId.Value.Length == 0)//添加信息
            {
                logentity.logcontent = "新增优惠券活动信息";
                dt = bll.Add("0", "0", out sumid, sumcode, buscode, stocode, cname, btime, etime, ctype, secctype, initype, status, descr, "0", "", "0", cuser, uuser, logentity);
                hidId.Value = sumid;

                this.PageTitle.Operate = 修改;
            }
            else//修改信息
            {
                logentity.logcontent = "修改sumid为" + hidId.Value + "的优惠券活动信息";
                logentity.otype = SystemEnum.LogOperateType.Edit;
                dt = bll.Update("0", "0", hidId.Value, sumcode, buscode, stocode, cname, btime, etime, ctype, secctype, initype, status, descr, "0", "", "0", cuser, uuser, logentity);
                this.PageTitle.Operate = 修改;
            }
            string type;
            string[] spanids;
            string[] mes;
            Helper.GetDataTableToResult(dt, out type, out mes, out spanids);
            if (type == "0")
            {
                Hidctype.Value = ctype;
                Hidsectype.Value = secctype;
                SetPage(hidId.Value);
                Script(Page, "gotocouponpage();");
            }
            //显示结果
            ShowResult(dt, errormessage);
        }
    }
}