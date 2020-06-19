using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb
{
    public partial class DBBakSetlist : EditPage
    {
        public string dbsid;
        bllDBBakSet bll = new bllDBBakSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AllBing();//数据绑定
                SetPage();
                this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateDetail").Body;
            }
        }
        private void AllBing()
        {
            //备份周期
            DataTable dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.BackupCycle));
            Helper.BindDropDownListForSearch(txt_bakcycles, dt, "enumname", "enumcode", 0);

            //绑定周
            DataTable dts = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.WeekDBBack));
            Helper.BindDropDownListForSearch(txt_week, dts, "enumname", "enumcode", 0);
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage()
        {
            //DataTable dt = bll.GetPagingSigInfo("0", "0", " where dbsid='" + id + "'");
            DataTable dt = bll.GetPagingSigInfo("0", "0", "");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                hidId.Value = dr["dbsid"].ToString();
                bakcycles.Value = dr["bakcycle"].ToString();//备份周期
                if (dr["bakcycle"].ToString() == "perhour")
                {
                    string str = dr["btime"].ToString();
                    str = str.Substring(0, str.IndexOf(':'));
                    if (str.Substring(0, 1) == "0")
                    {
                        str = str.Substring(1, 1);
                    }
                    btime.Value = str;
                }
                else
                {
                    btime.Value = dr["btime"].ToString();//开始执行时间
                }
                txt_isautos.Value = dr["isauto"].ToString();
                txt_durday.Text = dr["durday"].ToString();
            }
            ClientScript.RegisterStartupScript(ClientScript.GetType(), "sel_cycle", "<script>sel_cycle();</script>");
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息

            string buscode = Helper.GetAppSettings("BusCode");
            string stocode = Helper.GetAppSettings("Stocode");
            string bakpath = "null";//保存路径
            string bakcycle = Helper.ReplaceString(txt_bakcycles.SelectedValue);//备份周期
            string btime = string.Empty;//开始执行时间
            switch (bakcycle)
            {
                case "0"://周
                    bakcycle = Helper.ReplaceString(txt_week.SelectedValue);
                    btime = Helper.ReplaceString(txt_btime.Text);
                    break;
                case "1"://天
                    bakcycle = "everyday";
                    btime = Helper.ReplaceString(txt_btime.Text);
                    break;
                case "2"://小时
                    bakcycle = "perhour";
                    btime = Helper.ReplaceString(txt_hour.Text) + ":00";
                    break;
            }
            string asdf = btime;
            string isauto = Helper.ReplaceString(txt_isautos.Value);//请确保硬盘空间足够大
            string durday = Helper.ReplaceString(txt_durday.Text);//保留备份数据时长(天)
            string cuser = LoginedUser.UserInfo.Id.ToString();
            
            //日志信息
            logentity.module = ErrMessage.GetMessageInfoByCode("admins_Menu").Body;
            logentity.pageurl = "DBBakSetedit.aspx";
            logentity.otype = SystemEnum.LogOperateType.Add;
            logentity.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());

            DataTable dt = new DataTable();
            if (hidId.Value.Length == 0)//添加信息
            {
                logentity.logcontent = "新增数据备份设置信息";
                dt = bll.Add("0", "0", out dbsid, buscode, stocode, bakpath, bakcycle, btime, isauto, durday, cuser, logentity);
                hidId.Value = dbsid;
                this.PageTitle.Operate = 修改
            }
            else//修改信息
            {
                logentity.logcontent = "修改dbsid为" + hidId.Value + "的数据备份设置信息";
                logentity.otype = SystemEnum.LogOperateType.Edit;
                dt = bll.Update("0", "0", hidId.Value, buscode, stocode, bakpath, bakcycle, btime, isauto, durday, cuser, logentity);
                this.PageTitle.Operate = 修改
            }
            //显示结果
            ShowResult(dt, errormessage);
            SetPage();
        }
    }
}