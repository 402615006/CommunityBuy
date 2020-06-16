using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 后台用户操作日志数据访问类
    /// </summary>
    public partial class daloperatelog
    {
        CommunityBuy.CommonBasic.MSSqlDataAccess DBHelper = new CommunityBuy.CommonBasic.MSSqlDataAccess();
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(operatelogEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@opeid", Entity.id),
                new SqlParameter("@buscode", Entity.buscode),
                new SqlParameter("@strcode", Entity.strcode),
                new SqlParameter("@module", Entity.module),
                new SqlParameter("@pageurl", Entity.pageurl),
                new SqlParameter("@otype", Entity.otype),
                new SqlParameter("@logcontent", Entity.logcontent),
                new SqlParameter("@ip", Entity.ip),
                new SqlParameter("@opeuserid", Entity.cuser)
             };
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_tl_operatelog_Add", CommandType.StoredProcedure, sqlParameters);
            return intReturn;
        }
    }
}