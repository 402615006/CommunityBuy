using System;
using System.Data;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
using NPOI.SS.Formula.Functions;

namespace CommunityBuy.BackWeb
{
    public partial class ts_syssetEdit : EditPage
    {
        public string setid;
        bllts_sysset bll = new bllts_sysset();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    setid = Request["id"].ToString();
                    hidId.Value = setid;
                    SetPage(hidId.Value);
                    this.PageTitle.Operate = "修改";
                }
                else
                {
                    this.PageTitle.Operate ="新增";
                }
            }
        }

        /// <summary>
        /// 设置页面信息
        /// </summary>
        /// <param name="id">ID</param>
        private void SetPage(string id)
        {
            DataTable dt = bll.GetPagingSigInfo("0", "0", " where setid='" + id + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                //txt_stocode.Text = dr["stocode"].ToString();
                //txt_buscode.Text = dr["buscode"].ToString();
                txt_key.Text = dr["key"].ToString();
                txt_val.Text = dr["val"].ToString();
                ddl_status.SelectedValue = dr["status"].ToString();
                txt_descr.Value = dr["descr"].ToString();
                txt_explain.Value = dr["explain"].ToString();
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string stocode = Helper.GetAppSettings("Stocode");
            string buscode = Helper.GetAppSettings("BusCode");
            string key =txt_key.Text;
            string val =txt_val.Text;
            string status =this.ddl_status.SelectedValue;
            string descr =txt_descr.Value;
            string explain =txt_explain.Value;

            if (hidId.Value.Length == 0)//添加信息
            {
                bll.Add("0", "0", "", stocode, buscode, key, val, status, descr,explain);
                hidId.Value = bll.oResult.Data;
                this.PageTitle.Operate = "修改";
            }
            else//修改信息
            {
                ts_syssetEntity UEntity = bll.GetEntitySigInfo(" where setid=" + hidId.Value);
                UEntity.buscode = buscode;
                UEntity.stocode = stocode;
                UEntity.key = key;
                UEntity.val = val;
                UEntity.status =StringHelper.StringToInt(status);
                UEntity.descr = descr;
                UEntity.explain = explain;
                bll.Update("0", "0", UEntity);
            }
            //显示结果
            ShowResult(bll.oResult.Code,bll.oResult.Msg, errormessage);
        }
    }
}