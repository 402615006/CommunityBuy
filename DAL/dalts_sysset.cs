using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 系统设置数据访问类
    /// </summary>
    public partial class dalts_sysset 
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref ts_syssetEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@setid", Entity.setid),
				new SqlParameter("@stocode", Entity.stocode),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@key", Entity.key),
				new SqlParameter("@val", Entity.val),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@descr", Entity.descr),
                new SqlParameter("@explain",Entity.explain)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_ts_sysset_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.setid = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int NewAdd(ref systemsetEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@setid", Entity.setid),
                new SqlParameter("@stocode", Entity.stocode),
                new SqlParameter("@buscode", Entity.buscode),
                new SqlParameter("@key", Entity.key),
                new SqlParameter("@val", Entity.val),
                new SqlParameter("@status", Entity.status),
                new SqlParameter("@descr", Entity.descr),
                new SqlParameter("@explain",Entity.explain)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_ts_sysset_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.setid = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(ts_syssetEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@setid", Entity.setid),
				new SqlParameter("@stocode", Entity.stocode),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@key", Entity.key),
				new SqlParameter("@val", Entity.val),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@descr", Entity.descr),
                new SqlParameter("@explain",Entity.explain)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_ts_sysset_Update", CommandType.StoredProcedure, sqlParameters); 
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int NewUpdate(systemsetEntity Entity)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@setid", Entity.setid),
                new SqlParameter("@stocode", Entity.stocode),
                new SqlParameter("@buscode", Entity.buscode),
                new SqlParameter("@key", Entity.key),
                new SqlParameter("@val", Entity.val),
                new SqlParameter("@status", Entity.status),
                new SqlParameter("@descr", Entity.descr),
                new SqlParameter("@explain",Entity.explain)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_ts_sysset_Update", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="setid">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_ts_sysset_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string setid, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@setid", setid),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,64,mescode)
             };
			sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_ts_sysset_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }
    }
}