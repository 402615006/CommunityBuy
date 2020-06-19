using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.manage
{
    public partial class selectDishes : Common.ListPage
    {
        string stocode = "";
        BLL.blldishes bll = new BLL.blldishes();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageTitle.Operate = ErrMessage.GetMessageInfoByCode("PageOperateList").Body;
            if (!IsPostBack)
            {
                if (Request["stocode"] != null)
                {
                    stocode = Request["stocode"].ToString();
                    hidstocode.Value = stocode;
                }
                initComb();
                BindGridView();
            }
        }

        private void initComb()
        {
            seliscombo.Items.Clear();
            seliscombo.Items.Add(new ListItem() { Value = "", Text = "全部" });
            seliscombo.Items.Add(new ListItem() { Value = "0", Text = "否" });
            seliscombo.Items.Add(new ListItem() { Value = "1", Text = "是" });

            string strSql = string.Format("select [distypename],[distypecode] from [DisheType] where stocode='{0}' and ([pdistypecode] is null or [pdistypecode]='')", hidstocode.Value);
            DataTable dtdict = new BLL.bllBase().getDataTableBySql(strSql);
            Helper.BindDropDownListForSearch(this.seldistype, dtdict, "distypename", "distypecode", 0);
            seldistype.Items.Insert(0, new ListItem() { Text = "全部", Value = "" });
            seldistype1.Items.Clear();
            seldistype1.Items.Insert(0,new ListItem(){Text="全部",Value=""});
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected override void BindGridView()
        {
            int recount;
            int pagenums;
            hidisfirst.Value = (StringHelper.StringToInt(hidisfirst.Value) + 1).ToString();
            string order = string.Format("{0} {1}", HidSortExpression.Value, HidOrder.Value);
            if (Request["stocode"] != null)
            {
                string stocode = hidstocode.Value;
                if (string.IsNullOrEmpty(HidWhere.Value))
                {
                    if (stocode.Length > 0)
                    {
                        HidWhere.Value = " where stocode in(" + GetWhereStrs(stocode, ",") + ")";
                    }
                }
                else
                {
                    if (stocode.Length > 0)
                    {
                        HidWhere.Value += " and stocode in(" + GetWhereStrs(stocode, ",") + ")";
                    }
                }
            }
            DataTable dt = new blldishes().GetPagingListInfo1("0", "0", anp_top.PageSize, anp_top.CurrentPageIndex, HidWhere.Value, " order by ctime desc ", string.Empty, string.Empty, 0, out recount, out pagenums);
            if (dt != null)
            {
                if (dt != null)
                {
                    dt.Columns.Add("statusname", typeof(string));
                    dt.Columns.Add("iscomboname", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["statusname"] = Helper.GetEnumNameByValue(typeof(SystemEnum.Status), dt.Rows[i]["status"].ToString());
                        dt.Rows[i]["iscomboname"] = dt.Rows[i]["iscombo"].ToString() == "0" ? "否" : "是";
                    }
                    gv_list.DataSource = dt;
                    gv_list.DataBind();
                    anp_top.RecordCount = recount;
                    return;
                }
            }
        }

        /// <summary>
        /// 搜索按钮拼接Where条件
        /// </summary>
        public void GotoSearch()
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" where 1=1 ");
            //拼接Where条件
            //Where.Append(GetAuthoritywhere("stocode"));

            string strdisname = Helper.ReplaceString(txt_disname.Value);
            if (strdisname.Length > 0)
            {
                Where.Append(" and disname like '%" + strdisname + "%' ");

            }
            string strcustomcode = Helper.ReplaceString(txt_customcode.Value);
            if (strcustomcode.Length > 0)
            {
                Where.Append(" and customcode like '%" + strcustomcode + "%' ");

            }
            string iscombo = hidiscombo.Value;
            if (iscombo.Length > 0)
            {
                Where.Append(" and iscombo = '" + iscombo + "' ");
            }
            string pdistype = seldistype.SelectedValue;
            if (pdistype.Length > 0)
            {
                Where.Append(" and distypecode in (select distypecode from DisheType where pdistypecode='" + pdistype + "') ");
            }
            string pdistype1 = seldistype1.SelectedValue;
            if (pdistype1.Length > 0)
            {
                Where.Append(" and distypecode = '" + pdistype1 + "' ");
            }
            HidWhere.Value = Where.ToString();
            anp_top.CurrentPageIndex = 1;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GotoSearch();
        }

        protected void seldistype_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pdistypecode = seldistype.SelectedValue;
            if (pdistypecode.Length > 0)
            {
                string strSql = string.Format("select [distypename],[distypecode] from [DisheType] where stocode='{0}' and  [pdistypecode]='{1}'", hidstocode.Value, pdistypecode);
                DataTable dtdict = new BLL.bllBase().getDataTableBySql(strSql);
                Helper.BindDropDownListForSearch(this.seldistype1, dtdict, "distypename", "distypecode", 0);
                seldistype1.Items.Insert(0, new ListItem() { Text = "全部", Value = "" });
            }
            else
            {
                this.seldistype1.Items.Clear();
                seldistype1.Items.Insert(0, new ListItem() { Text = "全部", Value = "" });
            }
        }
    }
}