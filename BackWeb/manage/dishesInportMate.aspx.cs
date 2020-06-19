using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.BackWeb.Common;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb
{
    public partial class dishesInportMate : Common.ListPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int recount;
                int pagenums;
                //部门下拉框
                string strWhere = "where status='1'";
                if (LoginedUser.UserInfo.rolstocode.Length > 0)
                {
                    strWhere += " and stocode in('" + LoginedUser.UserInfo.rolstocode.Replace(",", "','") + "')";
                }
                DataTable dtStore = new bllStore().GetPagingListInfo("0", "0", int.MaxValue, 1, strWhere, "cname desc", out recount, out pagenums);
                ddl_stocode.DataTextField = "cname";
                ddl_stocode.DataValueField = "stocode";
                ddl_stocode.DataSource = dtStore;
                ddl_stocode.DataBind();
                ddl_stocode.SelectedIndex = 0;
            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            try
            {
                if (ddl_stocode.SelectedValue == "-1")
                {
                    Script(Page, "layer.alert('请选择门店!');");
                    return;
                }
                List<DishesMateEntity> matelist = new List<DishesMateEntity>();
                List<DishesMateEntity> mateerrlist = new List<DishesMateEntity>();
                string sql1 = " select *,matunitname=dbo.f_Get_DictsName(matunitcode) from catering_stock.dbo.StockMateUnits where buscode='" + Helper.GetAppSettings("BusCode") + "' and (stocode='" + ddl_stocode.SelectedValue + "' or stocode='12');";//菜品所用配料单位信息
                DataTable UnitDataTable = new blldishes().getDataTableBySql(sql1);
                sql1 = " select * from dishes where buscode='" + Helper.GetAppSettings("BusCode") + "' and stocode='" + ddl_stocode.SelectedValue + "';";//菜品所用配料单位信息
                DataTable DishesTable = new blldishes().getDataTableBySql(sql1);
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    string ext = System.IO.Path.GetExtension(file.FileName);
                    string sql = "begin ";
                    if (ext == ".xlsx" || ext == ".xls")
                    {
                        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/uploads/dishesmates/")))
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/uploads/dishesmates/"));
                        }
                        string filen = Guid.NewGuid().ToString() + ext;
                        string path = HttpContext.Current.Server.MapPath("~/uploads/dishesmates/" + filen);
                        file.SaveAs(path);

                        //任务
                        DataTable dt = ExcelsHelp.XlSToDataTable(Server.MapPath("~/uploads/dishesmates/" + filen), "配料", 0); //Excel读取出的datatable
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            //解析任务
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                try
                                {
                                    if (dt.Rows[j][0].ToString() == "" || dt.Rows[j][0].ToString() == null)
                                    {
                                        break;
                                    }
                                    DishesMateEntity obj = new DishesMateEntity();
                                    obj.buscode = Helper.GetAppSettings("BusCode");
                                    obj.stocode = ddl_stocode.SelectedValue;
                                    if (DishesTable.Select("discode='" + dt.Rows[j][1].ToString().Trim() + "'").Length == 0)
                                    {
                                        this.sp_showmes.InnerHtml = "菜品" + dt.Rows[j][0].ToString() + "不属于该门店,请核实，文档第" + j + "行";
                                        return;
                                    }
                                    obj.discode = dt.Rows[j][1].ToString();
                                    obj.matcode = dt.Rows[j][3].ToString();
                                    try
                                    {
                                        obj.useamount = decimal.Parse(dt.Rows[j][4].ToString());
                                        obj.jlnum = decimal.Parse(dt.Rows[j][4].ToString());
                                        obj.jlv = decimal.Parse(dt.Rows[j][5].ToString().Trim('%'))/100;
                                    }
                                    catch (Exception)
                                    {
                                        this.sp_showmes.InnerHtml = "请确认净料用料和毛料用量的列格式为字符格式，并且毛料用量文档第" + j + "行";
                                        return;
                                    }

                                    obj.mattype = dt.Rows[j][6].ToString();
                                    obj.remark = dt.Rows[j][7].ToString();
                                    obj.uuser = LoginedUser.UserInfo.Id;
                                    obj.isdelete = "0";
                                    obj.unitcode = dt.Rows[j][8].ToString();
                                    obj.mlnum = obj.jlnum / obj.jlv;
                                    //判断单位在不在有效范围内
                                    DataRow[] drs = UnitDataTable.Select("matunitname='" + obj.unitcode.Trim() + "' and matcode='" + obj.matcode.Trim() + "'");
                                    if (drs == null || drs.Length == 0)
                                    {
                                        //单位不对
                                        //记录到错误记录中
                                        obj.name = "[菜名:" + dt.Rows[j][0].ToString() + "-原料名:" + dt.Rows[j][2].ToString() + " 单位：" + obj.unitcode + "]";
                                        mateerrlist.Add(obj);
                                        matelist.Add(obj);
                                    }
                                    else
                                    {
                                        //判断是否为该物料最小单位
                                        if (drs[0]["isminunit"].ToString() != "1")
                                        {
                                            //取出最小单位进行换算
                                            DataRow[] min = UnitDataTable.Select("matcode='" + obj.matcode.Trim() + "' and isminunit='1'");
                                            obj.useamount = obj.useamount * decimal.Parse(drs[0][6].ToString());
                                            obj.jlnum = obj.jlnum * decimal.Parse(drs[0][6].ToString());
                                            obj.mlnum = obj.mlnum * decimal.Parse(drs[0][6].ToString());
                                            obj.unitcode = min[0]["matunitcode"].ToString();
                                        }
                                        else
                                        {
                                            obj.unitcode = drs[0]["matunitcode"].ToString();
                                        }
                                        matelist.Add(obj);
                                    }
                                }
                                catch (Exception err)
                                {
                                    this.sp_showmes.InnerHtml = err.Message + "，文档第" + j + "行";
                                    return;
                                }

                            }
                            StringBuilder Builder = new StringBuilder();
                            if (mateerrlist.Count == 0)
                            {
                                Builder.AppendFormat("begin declare @returnval int; set @returnval=0; BEGIN TRAN tan1 DECLARE @ID VARCHAR(20);");
                                foreach (DishesMateEntity mate in matelist)
                                {
                                    Builder.AppendFormat("delete DishesMate where stocode='" + mate.stocode + "' and discode='" + mate.discode + "';");
                                }
                                foreach (DishesMateEntity mate in matelist)
                                {
                                    Builder.AppendFormat("INSERT INTO DishesMate ([buscode],[stocode],[discode],[matcode],[unitcode],[useamount],[cuser],[ctime],[uuser],[utime],[isdelete],[jlnum],[jlv],[mlnum],[mattype],[remark])");
                                    Builder.AppendFormat(" values('" + mate.buscode + "','" + mate.stocode + "','" + mate.discode + "','" + mate.matcode + "','" + mate.unitcode + "'," + mate.useamount + ",'" + LoginedUser.UserInfo.Id + "',getDate(),'" + LoginedUser.UserInfo.Id + "',null,'0'," + mate.jlnum + "," + mate.jlv + "," + mate.mlnum + ",'" + mate.mattype + "','" + mate.remark + "');");
                                    Builder.AppendLine(" SET @ID=CONVERT(VARCHAR(20),SCOPE_IDENTITY()); ");
                                    Builder.AppendLine(" exec dbo.p_uploaddata_isSync '" + mate.buscode + "','" + mate.stocode + "','DishesMate','dishmateid',@ID,'add';");

                                }
                                Builder.AppendLine(" if(@@error=0) begin commit tran tan1; end else begin rollback tran tran1;set @returnval=1; end");
                                Builder.AppendLine(" end");
                                Builder.AppendLine(" select @returnval;");
                                if (new blldishes().ExecuteDataTable(Builder.ToString()) == 0)
                                {
                                    this.sp_showmes.InnerHtml = "导入成功";
                                }
                                else
                                {
                                    //导入失败
                                    this.sp_showmes.InnerHtml = "导入失败";
                                }
                            }
                            else
                            {
                                string name = string.Empty;
                                foreach (DishesMateEntity m in mateerrlist)
                                {
                                    name += m.name + "</br>";
                                }
                                errmate.InnerHtml = name;
                                Script(Page, "showerr()");
                                JavaScriptSerializer jss = new JavaScriptSerializer();
                                hiddate.Value = jss.Serialize(matelist);
                            }
                        }
                        else
                        {
                            //上传模板文件无数据
                            Script(Page, "layer.alert('文档数据不能为空');");
                            return;
                        }
                    }
                    else
                    {
                        Script(Page, "layer.alert('文档格式不支持,请上传xls或xlsx格式文件!');");
                        return;
                    }
                }
            }
            catch (Exception err)
            {
                this.sp_showmes.InnerHtml = "导入失败" + err.Message;
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<DishesMateEntity> matelist = new List<DishesMateEntity>();
                matelist = new JavaScriptSerializer().Deserialize<List<DishesMateEntity>>(hiddate.Value);
                StringBuilder Builder = new StringBuilder();
                Builder.AppendFormat("begin declare @returnval int; set @returnval=0; BEGIN TRAN tan1 DECLARE @ID VARCHAR(20);");
                foreach (DishesMateEntity mate in matelist)
                {
                    Builder.AppendFormat("delete DishesMate where stocode='" + mate.stocode + "' and discode='" + mate.discode + "';");
                }
                foreach (DishesMateEntity mate in matelist)
                {
                    Builder.AppendFormat("INSERT INTO DishesMate ([buscode],[stocode],[discode],[matcode],[unitcode],[useamount],[cuser],[ctime],[uuser],[utime],[isdelete],[jlnum],[jlv],[mlnum],[mattype],[remark])");
                    Builder.AppendFormat(" values('" + mate.buscode + "','" + mate.stocode + "','" + mate.discode + "','" + mate.matcode + "','" + mate.unitcode + "'," + mate.useamount + ",'" + LoginedUser.UserInfo.Id + "',getDate(),'" + LoginedUser.UserInfo.Id + "',null,'0'," + mate.jlnum + "," + mate.jlv + "," + mate.mlnum + ",'" + mate.mattype + "','" + mate.remark + "');");
                    Builder.AppendLine(" SET @ID=CONVERT(VARCHAR(20),SCOPE_IDENTITY()); ");
                    Builder.AppendLine(" exec dbo.p_uploaddata_isSync '" + mate.buscode + "','" + mate.stocode + "','DishesMate','dishmateid',@ID,'add';");
                }
                Builder.AppendLine(" if(@@error=0) begin commit tran tan1; end else begin rollback tran tran1;set @returnval=1; end");
                Builder.AppendLine(" end");
                Builder.AppendLine(" select @returnval;");
                if (new blldishes().ExecuteDataTable(Builder.ToString()) == 0)
                {
                    this.sp_showmes.InnerHtml = "导入成功";
                }
                else
                {
                    //导入失败
                    this.sp_showmes.InnerHtml = "导入失败";
                }
            }
            catch (Exception err)
            {
                //导入失败
                this.sp_showmes.InnerHtml = "导入失败" + err.Message;
            }

        }
    }
}