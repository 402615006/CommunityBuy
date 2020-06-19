using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.CommonBasic;
using CommunityBuy.BackWeb.Common;

namespace CommunityBuy.BackWeb
{
    public partial class dishesedit : EditPage
    {
        public string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GetSotre();
                if (Request["id"] != null)
                {
                    id = Request["id"].ToString();
                    //hidId.Value = id;
                    //SetPage(hidId.Value);
                    this.PageTitle.Operate = 修改
                }
                else
                {
                    this.PageTitle.Operate ="新增";
                }
                if (Request["mid"] != null)
                {
                    //HidMid.Value = Request["mid"].ToString();
                }
            }
        }
    }
}