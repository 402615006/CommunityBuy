using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 购物车设置数据访问类
    /// </summary>
    public partial class dalTB_ShoppingCart
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_ShoppingCartEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@PKCode", Entity.PKCode),
				new SqlParameter("@AutoDelTime", Entity.AutoDelTime),
				new SqlParameter("@IsAutoDelete", Entity.IsAutoDelete),
				new SqlParameter("@MaxNum", Entity.MaxNum),
             };
            sqlParameters[4].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_ShoppingCart_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.PKCode = sqlParameters[4].Value.ToString();
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_ShoppingCartEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@PKCode", Entity.PKCode),
				new SqlParameter("@AutoDelTime", Entity.AutoDelTime),
				new SqlParameter("@IsAutoDelete", Entity.IsAutoDelete),
				new SqlParameter("@MaxNum", Entity.MaxNum),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_ShoppingCart_Update", CommandType.StoredProcedure, sqlParameters); 
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="PKCode">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_ShoppingCart_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string PKCode, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@PKCode", PKCode),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };
			sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_ShoppingCart_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }
    }
}