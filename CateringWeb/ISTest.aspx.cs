using CommunityBuy.Adapter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.CommonBasic;

namespace CommunityBuy
{
    public partial class ISTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //BLL.bllPaging obj = new BLL.bllPaging();
            //int i = 0;
            //int j = 0;
            //DataTable dt = obj.GetLSPagingInfo("areas", "id", "*", 1, 10, "", "", "", out i, out j);
            //j = 1;
            //string code = "";
            //string msg = "";
            //DataTable dt2 = new DataTable();
            //dt2.Columns.Add("price", typeof(decimal));
            //dt2.Columns.Add("age", typeof(int));
            //dt2.Columns.Add("dtime", typeof(DateTime));
            //dt2.Columns.Add("cname", typeof(string));
            //DataRow drc = dt2.NewRow();
            //drc["price"] = 12.25;
            //drc["age"] = 45;
            //drc["dtime"] = DateTime.Now;
            //drc["cname"] = "程国栋";
            //dt2.Rows.Add(drc);
            //ArrayList arrData=new ArrayList();
            //arrData.Add(dt2);
            //string[] arrTBName=new string[1]{"dish"};
            //string j = JsonHelper.ToJson(code, msg, arrData,arrTBName);

            //DataTable dtresult = JsonHelper.JsonToDataTable(j);
            //Type tt = dtresult.Columns["price"].DataType;
            ////CommunityBuy.CommonBasic;
            //ComputerHelper com = new ComputerHelper();

            //string ComputerNo = com.ComputerNo;

            ////PrintAdapter obj = new PrintAdapter();
            ////数据：BusCode(商户编号),StoCode(门店编号),StoName(门店名称),OrderCode(订单编号),TableCode(桌台编号),TableName(桌台名称),CCname(操作人),CTime(操作时间),CusNum(客人数量),KitCode(厨房编号),TypeCode(菜品类别编号),DisCode(菜品编号),DisName(菜品名称),DisNum(菜品数量)
            //DataTable dt = new DataTable();
            //dt.Columns.Add("BusCode", typeof(string));
            //dt.Columns.Add("StoCode", typeof(string));
            //dt.Columns.Add("StoName", typeof(string));
            //dt.Columns.Add("OrderCode", typeof(string));
            //dt.Columns.Add("TableCode", typeof(string));
            //dt.Columns.Add("TableName", typeof(string));
            //dt.Columns.Add("CCode", typeof(string));
            //dt.Columns.Add("CCname", typeof(string));
            //dt.Columns.Add("CTime", typeof(string));
            //dt.Columns.Add("CusNum", typeof(string));
            //dt.Columns.Add("KitCode", typeof(string));
            //dt.Columns.Add("TypeCode", typeof(string));
            //dt.Columns.Add("DisCode", typeof(string));
            //dt.Columns.Add("DisName", typeof(string));
            //dt.Columns.Add("DisNum", typeof(string));
            //dt.Columns.Add("PersonCount", typeof(string));
            //dt.Columns.Add("ReasonName", typeof(string));
            //dt.Columns.Add("OldTableName", typeof(string));
            //dt.AcceptChanges();

            //DataRow dr = dt.NewRow();
            //dr["BusCode"] = "88888888";
            //dr["StoCode"] = "03";
            //dr["StoName"] = "东街.胡桃里";
            //dr["OrderCode"] = "03_20190929001";
            //dr["TableCode"] = "B10311";
            //dr["TableName"] = "A01";
            //dr["CCode"] = "LiChao";
            //dr["CCname"] = "李超";
            //dr["CTime"] = "2019-09-29 12:00:00";
            //dr["CusNum"] = "8";
            //dr["KitCode"] = "B10273";
            //dr["TypeCode"] = "B10465";
            //dr["DisCode"] = "B10292";
            //dr["DisName"] = "百威啤酒（1打)";
            //dr["DisNum"] = "2";
            //dr["ReasonName"] = "客人不要了";
            //dr["OldTableName"] = "A02";
            //dt.Rows.Add(dr);

            //DataRow dr2 = dt.NewRow();
            //dr2["BusCode"] = "88888888";
            //dr2["StoCode"] = "03";
            //dr2["StoName"] = "东街.胡桃里";
            //dr2["OrderCode"] = "03_20190929021";
            //dr2["TableCode"] = "B10080";
            //dr2["TableName"] = "胡桃里A02";
            //dr2["CCode"] = "LiChao";
            //dr2["CCname"] = "李超";
            //dr2["CTime"] = "2019-09-29 13:00:00";
            //dr2["CusNum"] = "8";
            //dr2["KitCode"] = "B10346";
            //dr2["TypeCode"] = "B10465";
            //dr2["DisCode"] = "B10218";
            //dr2["DisName"] = "二十一";
            //dr2["DisNum"] = "4"; 
            //dr2["ReasonName"] = "客人不要了";
            //dr2["OldTableName"] = "A05";
            //dt.Rows.Add(dr2);

            //obj.InsertPushFoodNote(dt);
            //obj.InsertReturnDishNote(dt);
            //obj.InsertTurntableNote(dt);
            //DataTable dtOrderDish = new DataTable();
            //dtOrderDish.Columns.Add("DisCode", typeof(string));
            //DataRow dr = dtOrderDish.NewRow();
            //dr["DisCode"] = "B1003";
            //dtOrderDish.Rows.Add(dr);

            //DataRow dr2 = dtOrderDish.NewRow();
            //dr2["DisCode"] = "B1002";
            //dtOrderDish.Rows.Add(dr2);

            //DataRow dr3 = dtOrderDish.NewRow();
            //dr3["DisCode"] = "B1002";
            //dtOrderDish.Rows.Add(dr3);

            //dtOrderDish.DefaultView.Sort = "DisCode ASC";
            //dtOrderDish = dtOrderDish.DefaultView.ToTable();


            //dtOrderDish.AcceptChanges();

            //DataRow [] drs = dtOrderDish.Select("DisCode like 'B103%'");
            //int count = drs.Length;

            //string a = (Math.Ceiling(0.11 * 10) / 10).ToString("F2");
            //a = (Math.Round(((int)(0.01 * 100) * 1.0)) / 100).ToString("F1");
            //a = (Math.Round(((int)(0.05 * 100) * 1.0)) / 100).ToString("F1");
            //a = (Math.Round(((int)(0.15 * 10) * 1.0)) / 10).ToString("F0");
            //a = (Math.Round(((int)(0.55 * 10) * 1.0)) / 10).ToString("F0");
            //a = (Math.Ceiling(0.10 * 10) / 10).ToString("F2");
            //a = (Math.Floor(0.16 * 10) / 10).ToString("F2");
            //a = (Math.Floor(0.11 * 10) / 10).ToString("F2");
            //a = (Math.Ceiling(((int)(0.01 * 10) * 1.0) / 10)).ToString("F2");
            //a = (Math.Ceiling(((int)(0.11 * 10) * 1.0) / 10)).ToString("F2");
            //a = (Math.Ceiling(((int)(1.01 * 10) * 1.0) / 10)).ToString("F2");
        }
    }
}