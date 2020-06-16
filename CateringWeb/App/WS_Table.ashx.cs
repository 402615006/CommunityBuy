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
    public class WS_Table : IServices.ServiceBase
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
                        case "i_opentable"://开台
                            OpenTable(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 小程序开台
        /// </summary>
        /// <param name="dicPar"></param>
        private void OpenTable(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "buscode", "stocode", "tablecode","memcode","paytype" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string buscode = dicPar["buscode"].ToString();
            string stocode = dicPar["stocode"].ToString();
            string tablecode = dicPar["tablecode"].ToString();
            string memcode= dicPar["memcode"].ToString();
            string paytype = dicPar["paytype"].ToString();
            bllTB_OpenTable bllOpenTable = new bllTB_OpenTable();
            string id = "";

            logentity.pageurl = "TB_OpenTableEdit.html";
            logentity.logcontent = "新增开台信息信息";
            logentity.cuser = Helper.StringToLong("0");
            logentity.otype = SystemEnum.LogOperateType.Add;
            //检查桌台状态
            TB_OpenTableEntity checkEntity= bllOpenTable.GetEntitySigInfo(" TableCode='"+ tablecode + "' and Stocode='"+ stocode + "' and Tstatus<>'4' ");
            if (checkEntity != null&& !string.IsNullOrWhiteSpace(checkEntity.PKCode)&&checkEntity.TStatus!="4")
            {
               
                //检查桌台的订单数量和未完成的账单数量
                DataSet ds= new bllApp().GetTableOrderAndBill(stocode, checkEntity.PKCode);
                if (ds != null && ds.Tables.Count == 2)
                {
                    ArrayList arrTables = new ArrayList();
                    string[] tableNames = { "order", "bill", "ordernum", "billnum" };
                    foreach (DataTable dt in ds.Tables)
                    {
                        arrTables.Add(dt);
                    }
                    int ordernum = ds.Tables[0].Rows.Count;
                    int billnum = ds.Tables[1].Rows.Count;
                    arrTables.Add(ordernum);
                    arrTables.Add(billnum);
                    ReturnListJson("0", checkEntity.PKCode, arrTables, tableNames);
                }
                else
                {
                    ToCustomerJson("0", checkEntity.PKCode);
                }
            }
            else
            {
                if (paytype == "1")
                {
                    paytype = "5";
                }
                else
                {
                    paytype = "1";
                }
                DataTable dt = bllOpenTable.Add("", "", out id, buscode, stocode, memcode, "线上点餐", paytype, "", "", "0", "", tablecode, "", "", logentity);
                ReturnJson(dt);
            }
        }


    }
}