using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.App
{
    /// <summary>
    /// 菜品信息接口类
    /// </summary>
    public class WS_Dish :IServices.ServiceBase
    {
        bllTB_Dish bll = new bllTB_Dish();
        DataTable dt = new DataTable();
        operatelogEntity logentity = new operatelogEntity();
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    logentity.module = "菜品信息";
                    switch (actionname.ToLower())
                    {
                        case "i_getdispacage"://列表
                            GetDisPackage(dicPar);
                            break;
                        case "i_getcookmethod"://列表
                            GetCookMethod(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取菜皮套餐
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetDisPackage(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "discode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string discode = dicPar["discode"].ToString();
            string filter = " dc.PDisCode='" + discode + "' and dis.stocode='"+ stocode + "' and dis.TStatus='1'";
            int recnum;
            int pagenum;

            DataTable dtGroup = new DataTable();
            dtGroup.Columns.Add("GroupName");
            dtGroup.Columns.Add("MaxOptNum");
            dtGroup.Columns.Add("TotalOptNum");
            dtGroup.Columns.Add("TotalOptMoney");
            dtGroup.Columns.Add("CombinationType");
            dtGroup.Columns.Add("dish");

            foreach (DataRow dr in dt.DefaultView.ToTable(true, "GroupName").Rows)
            {
                dtGroup.Rows.Add(dr["GroupName"], "", "", "", "");
            }

            DataTable dtDish = new DataTable();
            dtDish.Columns.Add("DisCode");
            dtDish.Columns.Add("DisName");
            dtDish.Columns.Add("Price");
            dtDish.Columns.Add("IsMust");
            dtDish.Columns.Add("DefaultNum");
            dtDish.Columns.Add("TypeCode");

            foreach (DataRow dr in dtGroup.Rows)
            {
                dtDish.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["GroupName"].ToString()== dr["GroupName"].ToString())
                    {
                        dr["MaxOptNum"] = dt.Rows[i]["MaxOptNum"];
                        dr["TotalOptNum"] = dt.Rows[i]["TotalOptNum"];
                        dr["TotalOptMoney"] = dt.Rows[i]["TotalOptMoney"];
                        dr["CombinationType"] = dt.Rows[i]["CombinationType"];
                        dtDish.Rows.Add(dt.Rows[i]["DisCode"], dt.Rows[i]["DisName"], dt.Rows[i]["Price"], dt.Rows[i]["IsMust"], dt.Rows[i]["DefaultNum"], dt.Rows[i]["TypeCode"]);
                    }
                }
                string disJson = JsonHelper.DataTableToJSON(dtDish);
                dr["dish"] = disJson;
            }
            ReturnListJson(dtGroup);
        }

        /// <summary>
        /// 获取菜品做法
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetCookMethod(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "discode"};
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string discode = dicPar["discode"].ToString();
            string filter = " tr.DisCode='" + discode + "' and tb.stocode='" + stocode + "' and tstatus='1' ";
            int recnum;
            int pagenum;
            string[] names = {"method","flavor"};
            ArrayList arrTables = new ArrayList();
            ReturnListJson("0","获取成功",arrTables,names);
        }
    }
}