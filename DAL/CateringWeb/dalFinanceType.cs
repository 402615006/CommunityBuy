using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 财务类别
    /// </summary>
    public partial class dalFinanceType
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess("CateringDBConnectionString");

        /// <summary>
        /// 根据商户编号获取所有财务类别,缓存用,仅缓存fincode,finname
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetBusCodeToAllFin(string BusCode)
        {
            string where = string.Empty;
            if(!string.IsNullOrEmpty(BusCode))
            {
                where = " where buscode='" + BusCode + "'";
            }
            string sql = "select fincode,finname from FinanceType"+where;
            return DBHelper.ExecuteDataTable(sql,CommandType.Text,null);
        }
    }
}
