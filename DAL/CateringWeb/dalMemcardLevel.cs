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
    /// 会员卡等级
    /// </summary>
    public partial class dalMemcardLevel
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess("CateringDBConnectionString");

        /// <summary>
        /// 获取会员卡等级信息,缓存用
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataSet GetMemcardLevel(string BusCode)
        {
            string where = "";
            if(!string.IsNullOrEmpty(BusCode))
            {
                where = " and buscode='" + BusCode + "'";
            }
            DataSet ds = new DataSet();
            ds = DBHelper.ExecuteDataSet("SELECT * FROM dbo.memcardtype WHERE [status]='1'"+where+";SELECT * FROM memcardlevel  WHERE status='1' AND mctcode IN (SELECT mctcode FROM dbo.memcardtype WHERE [status]='1'"+where+")"+where);
            return ds;
        }

        /// <summary>
        /// 按条件获取会员卡基本信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetMemcard(string where)
        {
            string sql = "SELECT *,mt.cname as typename FROM[dbo].[MemCard] m inner join[dbo].[memcardtype] mt on m.ctype = mt.mctcode " + where;
            return DBHelper.ExecuteDataTable(sql);
        }

    }
}
