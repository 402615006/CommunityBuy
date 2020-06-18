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
    /// 门店信息
    /// </summary>
    public partial class dalStore
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess("CateringDBConnectionString");

        /// <summary>
        /// 根据商户编号获取所有门店,缓存用,仅缓存stocode,cname
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetBusCodeToAllStore(string BusCode)
        {
            //return DBHelper.ExecuteDataTable("select s.*,sg.stopath,sg.ptype from Store s left join storegx sg on s.stocode=sg.stocode where s.stocode not in (select stocode from store where isfood='1' and pstocode='') and s.buscode='" + BusCode+"'",CommandType.Text,null);
            string where = string.Empty;
            if (!string.IsNullOrEmpty(BusCode))
            {
                where = " where s.buscode='" + BusCode + "'";
            }
            return DBHelper.ExecuteDataTable("select s.*,sg.stopath,sg.ptype from Store s left join storegx sg on s.stocode=sg.stocode"+where, CommandType.Text, null);
        }

        /// <summary>
        /// 根据商户编号获取所有门店,缓存用,仅缓存stocode,cname
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetAllStore(string filter)
        {
            return DBHelper.ExecuteDataTable("select stocode,cname,btime,etime,tel,status from Store " + filter + "", CommandType.Text, null);
        }

    }
}
