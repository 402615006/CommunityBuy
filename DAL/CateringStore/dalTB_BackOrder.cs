using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 退单信息数据访问类
    /// </summary>
    public partial class dalTB_BackOrder
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_BackOrderEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@AuthCode", Entity.AuthCode),
				new SqlParameter("@AuthName", Entity.AuthName),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@OrderCode", Entity.OrderCode),
				new SqlParameter("@OrderDisCode", Entity.OrderDisCode),
				new SqlParameter("@ReasonCode", Entity.ReasonCode),
				new SqlParameter("@ReasonName", Entity.ReasonName),
				new SqlParameter("@Remar", Entity.Remar),
				new SqlParameter("@BackNum", Entity.BackNum),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_BackOrder_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.Id = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_BackOrderEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@AuthCode", Entity.AuthCode),
				new SqlParameter("@AuthName", Entity.AuthName),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@OrderCode", Entity.OrderCode),
				new SqlParameter("@OrderDisCode", Entity.OrderDisCode),
				new SqlParameter("@ReasonCode", Entity.ReasonCode),
				new SqlParameter("@ReasonName", Entity.ReasonName),
				new SqlParameter("@Remar", Entity.Remar),
				new SqlParameter("@BackNum", Entity.BackNum),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_BackOrder_Update", CommandType.StoredProcedure, sqlParameters); 
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
            return DBHelper.ExecuteNonQuery("dbo.p_TB_BackOrder_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
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
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_BackOrder_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }
    }
}