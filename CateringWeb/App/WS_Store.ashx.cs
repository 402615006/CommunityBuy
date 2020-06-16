using CommunityBuy.BLL;
using CommunityBuy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.App
{
    /// <summary>
    /// WS_Store 的摘要说明
    /// </summary>
    public class WS_Store : IServices.ServiceBase
    {
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
                    logentity.module = "门店信息表";
                    switch (actionname.ToLower())
                    {
                        case "i_getstore"://获取连锁端门店信息,不分页
                            GetStoreInfo(dicPar);
                            break;
                        case "i_getdepart"://获取连锁端门店信息,不分页
                            GetDepartList(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取门店菜品
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetStoreInfo(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "tablecode","menucode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string tablecode = dicPar["tablecode"].ToString();
            string menucode = dicPar["menucode"].ToString();

            DataSet ds = new bllApp().GetStoreDishInfo(stocode, tablecode, menucode);
            if (ds != null && ds.Tables.Count >= 3)
            {
                ArrayList arrTables = new ArrayList();
                string[] tableNames = { "table", "dish", "dishtype" };
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dr["image"] = Helper.GetAppSettings("StoDisImgUrl") + dr["image"].ToString();
                }
                foreach (DataTable dt in ds.Tables)
                {
                    arrTables.Add(dt);
                }
                ReturnListJson("0", "获取成功", arrTables, tableNames);
            }
            else
            {
                ToCustomerJson("1", "数据获取失败");
            }
        }

        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetDepartList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "page", "pagesize" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string page = dicPar["page"].ToString();
            string pagesize = dicPar["pagesize"].ToString();

            DataTable dtDepart= new bllApp().GetDepartmenByStore(stocode,page,pagesize);
            ReturnListJson(dtDepart);
        }

    }
}