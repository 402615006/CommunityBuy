using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 优惠券活动数据访问类
    /// </summary>
    public partial class dalsumcouponN
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref sumcouponNEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@sumid", Entity.sumid),
				new SqlParameter("@sumcode", Entity.sumcode),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@stocode", Entity.stocode),
				new SqlParameter("@cname", Entity.cname),
				new SqlParameter("@ctype", Entity.ctype),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@descr", Entity.descr),
				new SqlParameter("@audcode", Entity.audcode),
				new SqlParameter("@audcname", Entity.audcname),
				new SqlParameter("@audremark", Entity.audremark),
				new SqlParameter("@audstatus", Entity.audstatus),
				new SqlParameter("@ccode", Entity.ccode),
				new SqlParameter("@ccname", Entity.ccname)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_sumcouponN_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.sumid = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(sumcouponNEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@sumid", Entity.sumid),
				new SqlParameter("@sumcode", Entity.sumcode),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@stocode", Entity.stocode),
				new SqlParameter("@cname", Entity.cname),
				new SqlParameter("@ctype", Entity.ctype),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@descr", Entity.descr),
				new SqlParameter("@audcode", Entity.audcode),
				new SqlParameter("@audcname", Entity.audcname),
				new SqlParameter("@audremark", Entity.audremark),
				new SqlParameter("@audstatus", Entity.audstatus)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_sumcouponN_Update", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sumid">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_sumcouponN_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 作废未使用优惠券
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int UpdateStatusNotSend(string ids)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_sumcouponN_UpdateStatusNotSend", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 优惠活动审核
        /// </summary>
        /// <param name="sumid">活动ID</param>
        /// <returns></returns>
        public int Audit(string sumid, string AudChar, string Audreason, string audcode, string audcname, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@sumid", sumid),
				new SqlParameter("@AudChar", AudChar),
				new SqlParameter("@Audreason", Audreason),
                new SqlParameter("@audcode", audcode),
                new SqlParameter("@audcname", audcname),
				new SqlParameter("@mescode",SqlDbType.NVarChar,64,mescode)
             };
            sqlParameters[5].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_sumcouponN_Audit", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[5].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string sumid, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@sumid", sumid),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,64,mescode)
             };
            sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_sumcouponN_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }
    }
}