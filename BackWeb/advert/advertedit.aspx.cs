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
    public partial class advertedit : EditPage
    {
        public string id;
        bllActivityAd bll = new bllActivityAd();
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

        private void SetPage(string id)
        {
            ActivityAdEntity disEntity=bll.GetEntitySigInfo("id="+id);
            if (disEntity.Id>0)
            {
                ddl_sel_dishetypetwo.SelectedValue = disEntity.Type.ToString();
                txt_title.Value = disEntity.Title;
                txt_sort.Value = disEntity.Sort.ToString();
                txt_url.Value = disEntity.Url;
                txtDes.Value = disEntity.Description;
                hidimages.Value = disEntity.images;
                string imageHtml = "";
                if (disEntity.images.Length > 0)
                {
                    foreach (string img in disEntity.images.Split(','))
                    {
                        if (!string.IsNullOrWhiteSpace(img))
                        { 
                             imageHtml += "<img  imgindex=\"\"  width=\"200\" height=\"200\" style=\"float: left; margin-left:10px;\" src=\"/UploadFiles" + img + "\" onclick=\"deleteimage(this, '"+ img + "')\" />";
                        }
                    }
                    HidImagesHtml.Value = imageHtml;
                }
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

        }

        /// <summary>
        /// 获取菜品下拉框
        /// </summary>
        public void BindStore()
        {
            //一级菜品类别
            ListItem itemDefault = new ListItem();
            itemDefault.Text = "--请选择--";
            itemDefault.Value = "-1";
            itemDefault.Selected = true;

            ListItem item1 = new ListItem();
            item1.Text = "首页轮播";
            item1.Value = "1";

            ListItem item2 = new ListItem();
            item2.Text = "首页通知";
            item2.Value = "2";

            ddl_sel_dishetypetwo.Items.Add(item1);
            ddl_sel_dishetypetwo.Items.Add(item2);
            ddl_sel_dishetypetwo.Items.Add(itemDefault);
        }

        /// <summary>
        /// 保存菜品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_btn_Click(object sender, EventArgs e)
        {
            id = hidid.Value;
            string type = ddl_sel_dishetypetwo.SelectedValue;
            string title = txt_title.Value;
            string sort = txt_sort.Value;
            string url = txt_url.Value;
            string images = hidimages.Value.TrimStart(',').TrimEnd(',');
            string Descript = txtDes.Value;
            if (hidid.Value.Length == 0 || hidid.Value == "0")//添加信息
            {
                bll.Add("", "",id,title, "1",sort,Descript,type,images, url);
                hidid.Value = bll.oResult.Data;
                if (bll.oResult.Code == "1")
                { 
                //添加图片
                
                }
                this.PageTitle.Operate = "修改";
            }
            else//修改信息
            {
                bll.Update("", "", id, title, "1", sort, Descript, type, images, url);
            }
            //显示结果
            ShowResult(bll.oResult.Code, bll.oResult.Msg, errormessage);

        }
    }
}