using System;
using System.Data;
using System.Web.UI.WebControls;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.dish
{
    public partial class dishMethodEdit : EditPage
    {
        public string mid;
        bllPaging bll = new bllPaging();
        protected void Page_Load(object sender, EventArgs e)
        {

                if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    mid = Request["id"].ToString();
                    hidId.Value = mid;
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
            DataTable dt =bll.GetDataTableInfoBySQL("select * from [dbo].[TR_DishesMethods] where id="+id);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ddltype.SelectedValue = dr["MType"].ToString();
                txt_typename.Text = dr["TypeName"].ToString();
                txt_name.Text = dr["Name"].ToString();
                txt_money.Text = dr["Money"].ToString();
            }
        }

        //保存数据
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            //获取页面信息
            string mtype = ddltype.SelectedValue;
            string name = txt_name.Text;
            string typename = txt_typename.Text;
            string money = txt_money.Text;
            if (string.IsNullOrWhiteSpace(money))
            {
                money = "0";
            }
            string relmsg ="操作失败";
            int recnum = 0;
            if (hidId.Value.Length == 0|| hidId.Value=="0")//添加信息
            {
                //string dicid = "";
                string sql = "insert into [dbo].[TR_DishesMethods]([StoCode] ,[DisCode] ,[MType],[Money] ,[TypeName],[Name]) values('',''," + mtype + "," + money + ",'" + typename + "','" + name + "')";
                recnum =bll.ExecuteNonQueryBySQL(sql);
                //hidId.Value = bll.oResult.Data;
                this.PageTitle.Operate = "添加";
            }
            else//修改信息
            {
                recnum= bll.ExecuteNonQueryBySQL("update [dbo].[TR_DishesMethods] set [MType]=" + mtype + ",[Money] =" + money + ",[TypeName]='" + typename + "',[Name]='" + name + "' where id="+ hidId.Value);
                //hidId.Value = bll.oResult.Data;
                this.PageTitle.Operate = "修改";
            }
            //显示结果
            if (recnum >= 0)
            {
                relmsg = "操作成功";
            }
            ShowResult(recnum.ToString(), relmsg, errormessage);
        }


    }
}