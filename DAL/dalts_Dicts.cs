using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 系统字典信息数据访问类
    /// </summary>
    public partial class dalts_Dicts 
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref ts_DictsEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@dicid", Entity.dicid),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@strcode", Entity.strcode),
				new SqlParameter("@dictype", Entity.dictype),
				new SqlParameter("@lng", Entity.lng),
				new SqlParameter("@pdicid", Entity.pdicid),
				new SqlParameter("@diccode", Entity.diccode),
				new SqlParameter("@dicname", Entity.dicname),
				new SqlParameter("@dicvalue", Entity.dicvalue),
				new SqlParameter("@orderno", Entity.orderno),
				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@cuser", Entity.cuser),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_ts_Dicts_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.dicid = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(ts_DictsEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@dicid", Entity.dicid),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@strcode", Entity.strcode),
				new SqlParameter("@dictype", Entity.dictype),
				new SqlParameter("@lng", Entity.lng),
				new SqlParameter("@pdicid", Entity.pdicid),
				new SqlParameter("@diccode", Entity.diccode),
				new SqlParameter("@dicname", Entity.dicname),
				new SqlParameter("@dicvalue", Entity.dicvalue),
				new SqlParameter("@orderno", Entity.orderno),
				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@cuser", Entity.cuser),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_ts_Dicts_Update", CommandType.StoredProcedure, sqlParameters); 
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="dicid">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_ts_Dicts_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string dicid, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@dicid", dicid),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,64,mescode)
             };
			sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_ts_Dicts_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }
    }
}