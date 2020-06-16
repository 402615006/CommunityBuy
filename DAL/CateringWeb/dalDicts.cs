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
    /// 系统字典
    /// </summary>
    public partial class dalDicts
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess("CateringDBConnectionString");

        /// <summary>
        /// 根据商户编号获取所有员工,缓存用,仅缓存dicname
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetBusCodeToDicts(string BusCode)
        {
            string where = string.Empty;
            if(!string.IsNullOrEmpty(BusCode))
            {
                where = " and buscode='" + BusCode + "'";
            }
            string sql = "select dicname from ts_Dicts  where status = 1 and pdicid = 28"+where+";";
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

    }
}
