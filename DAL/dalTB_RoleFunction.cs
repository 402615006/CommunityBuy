using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 门店权限管理数据访问类
    /// </summary>
    public partial class dalTB_RoleFunction
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_RoleFunctionEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@RoleId", Entity.RoleId),
				new SqlParameter("@FunctionId", Entity.FunctionId),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_RoleFunction_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.Id = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_RoleFunctionEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@RoleId", Entity.RoleId),
				new SqlParameter("@FunctionId", Entity.FunctionId),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_RoleFunction_Update", CommandType.StoredProcedure, sqlParameters); 
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_RoleFunction_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string Id, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@Id", Id),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };
			sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_RoleFunction_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }
    }
}