using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.CommonBasic;
using CommunityBuy.BackWeb.Common;
using System.Data;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using System.Web.DynamicData;

namespace CommunityBuy.BackWeb
{
    public partial class dishesedit : EditPage
    {
        public string id;
        bllTB_Dish bll = new bllTB_Dish();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDishType();
                BindStore();
                if (Request["id"] != null)
                {
                    id = Request["id"].ToString();
                    hidid.Value = id;
                    SetPage(hidid.Value);
                    this.PageTitle.Operate = "编辑";
                }
                else
                {
                    this.PageTitle.Operate = "添加";
                }
            }
        }

        private void SetPage(string discode)
        {
            TB_DishEntity disEntity=bll.GetEntitySigInfo("DisCode='"+ discode + "'");
            if (disEntity != null && !string.IsNullOrWhiteSpace(disEntity.DisCode))
            {
                ddlStore.SelectedValue = disEntity.StoCode;
                txt_disname.Value = disEntity.DisName;
                txt_disothername.Value = disEntity.OtherName;
                txt_quickcode.Value = disEntity.QuickCode;
                txt_selunit.Value = disEntity.Unit;
                txt_price.Value = disEntity.Price.ToString("f2");
                txt_costprice.Value = disEntity.CostPrice.ToString("f2");
                txt_remark.Value = disEntity.Descript;
                TB_DishTypeEntity typeEntity = new bllTB_DishType().GetEntitySigInfo(" PKCode='" + disEntity.TypeCode+ "' ");
                if (typeEntity != null && !string.IsNullOrWhiteSpace(typeEntity.PKCode))
                {
                    ddl_selDisheType.SelectedValue = typeEntity.PKKCode;
                    ddl_selDisheType_SelectedIndexChanged(null, null);
                    ddl_sel_dishetypetwo.SelectedValue = disEntity.TypeCode;
                }
            }
            DataTable dtImage = new bllPaging().GetPagingInfo("TR_DishImage", "id", "*", int.MaxValue, 1, "discode='" + discode + "'", "", "", out int recnums,out int pagenums);
            if (dtImage != null)
            {
                string imageHtml = "";
                foreach (DataRow dr in dtImage.Rows)
                {
                    imageHtml += "<img  imgindex=\"\"  width=\"200\" height=\"200\" style=\"float: left; margin - left:6px; \" src=\"/UploadFiles" + dr["ImgUrl"] + "\" onclick=\"deleteimage(this, '')\" />";
                }
                HidImagesHtml.Value = imageHtml;
            }
        }

        /// <summary>
        /// 获取菜品下拉框
        /// </summary>
        public void BindDishType()
        {
            System.Web.UI.WebControls.ListItem itemDefault = new System.Web.UI.WebControls.ListItem();
            itemDefault.Text = "--请选择--";
            itemDefault.Value = "-1";
            itemDefault.Selected = true;
            int recount;
            int pagenums;

            //一级菜品类别
            DataTable dt2 = new bllTB_DishType().GetPagingListInfo("0", "0", int.MaxValue, 1, "[tstatus]='1' and pkkcode='0' ", "", out recount, out pagenums);
            ddl_selDisheType.DataTextField = "typename";
            ddl_selDisheType.DataValueField = "pkcode";
            ddl_selDisheType.DataSource = dt2;
            ddl_selDisheType.DataBind();
            ddl_selDisheType.Items.Add(itemDefault);
        }

        /// <summary>
        /// 获取菜品下拉框
        /// </summary>
        public void BindStore()
        {
            //一级菜品类别
            DataTable dtStore = new bllStore().GetPagingListInfo("0", "0", int.MaxValue, 1, "", "", out int recount, out int pagenums);
            BindDropDownListInfo(ddlStore, dtStore, "sname", "stocode", 1);
        }

        /// <summary>
        /// 一级菜品类别获取二级菜品类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_selDisheType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_selDisheType.SelectedValue != "-1")
            {
                int recount;
                int pagenums;
                System.Web.UI.WebControls.ListItem itemDefault = new System.Web.UI.WebControls.ListItem();
                itemDefault.Text = "全部";
                itemDefault.Value = "-1";
                itemDefault.Selected = true;
                DataTable dt2 = new bllTB_DishType().GetPagingListInfo("0", "0", int.MaxValue, 1, "where [tstatus]='1' and PKKCode='" + ddl_selDisheType.SelectedValue + "'", "typename desc", out recount, out pagenums);
                ddl_sel_dishetypetwo.DataTextField = "typename";
                ddl_sel_dishetypetwo.DataValueField = "pkcode";
                ddl_sel_dishetypetwo.DataSource = dt2;
                ddl_sel_dishetypetwo.DataBind();
                ddl_sel_dishetypetwo.Items.Add(itemDefault);
            }
        }

        /// <summary>
        /// 保存菜品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            string StoCode = ddlStore.SelectedValue;
            string ChannelCodeList = "";
            string disname = txt_disname.Value;
            string OtherName = txt_disothername.Value;
            string typecode = ddl_sel_dishetypetwo.SelectedValue;
            string QuickCode = txt_quickcode.Value;
            string UnitName = txt_selunit.Value;
            string price = txt_price.Value;
            string costprice = txt_costprice.Value;
            string Descript = txt_remark.Value;
            string imagename = hidimages.Value;
            if (hidid.Value.Length == 0 || hidid.Value == "0")//添加信息
            {
                bll.Add("", "", imagename, StoCode, "1", disname, OtherName, typecode, QuickCode, UnitName, price, costprice, Descript);
                hidid.Value = bll.oResult.Data;
                if (bll.oResult.Code == "1")
                { 
                //添加图片
                
                }
                this.PageTitle.Operate = "修改";
            }
            else//修改信息
            {
                TB_DishEntity UEntity = bll.GetEntitySigInfo(" discode='" + hidid.Value+"'");
                UEntity.StoCode = StoCode;
                UEntity.ChannelCodeList = ChannelCodeList;
                UEntity.DisName = disname;
                UEntity.OtherName = OtherName;
                UEntity.TypeCode = typecode;
                UEntity.QuickCode = QuickCode;
                UEntity.Unit = UnitName;
                UEntity.ImageName = imagename;
                UEntity.Price =StringHelper.StringToDecimal(price);
                UEntity.CostPrice= StringHelper.StringToDecimal(costprice);
                bll.Update("0", "0", UEntity);
            }
            //显示结果
            ShowResult(bll.oResult.Code, bll.oResult.Msg, errormessage);

        }
    }
}