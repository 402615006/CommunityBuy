using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public partial class daldiscountpackage
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess("CateringDBConnectionString");

        /// <summary>
        /// 根据门店获取折扣方案
        /// </summary>
        /// <param name="BusCode">商户编号</param>
        /// <param name="StoCode">门店编号</param>
        /// <returns></returns>
        public DataTable GetStoDisCountPackage(string BusCode, string StoCode)
        {
            string sql = "select *,'' as CardName from discountpackage where stocode='"+ Helper.GetAppSettings("StoCode") + "' and buscode='" + BusCode + "' and status='1'";
            return DBHelper.ExecuteDataTable(sql);
        }
    }
}
