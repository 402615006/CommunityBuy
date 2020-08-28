using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 订单菜品数据访问类
    /// </summary>
    public partial class dalTB_OrderDish
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_OrderDishEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@OrderCode", Entity.OrderCode),
				new SqlParameter("@DisCode", Entity.DisCode),
				new SqlParameter("@DisName", Entity.DisName),
				new SqlParameter("@Price", Entity.Price),
				new SqlParameter("@DisUite", Entity.DisUite),
				new SqlParameter("@DisNum", Entity.DisNum),
				new SqlParameter("@PKCode", Entity.PKCode),
				new SqlParameter("@TotalMoney", Entity.TotalMoney),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_OrderDish_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.Id = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_OrderDishEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@OrderCode", Entity.OrderCode),
				new SqlParameter("@DisCode", Entity.DisCode),
				new SqlParameter("@DisName", Entity.DisName),
				new SqlParameter("@Price", Entity.Price),
				new SqlParameter("@DisUite", Entity.DisUite),
				new SqlParameter("@DisNum", Entity.DisNum),
				new SqlParameter("@PKCode", Entity.PKCode),
				new SqlParameter("@TotalMoney", Entity.TotalMoney),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_OrderDish_Update", CommandType.StoredProcedure, sqlParameters); 
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
            return DBHelper.ExecuteNonQuery("dbo.p_TB_OrderDish_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 退菜
        /// </summary>
        /// <param name="orderdishcode">点菜编号</param>
        /// <returns></returns>
        public int BackOrderDish(string stocode,string orderdishcode)
        {
            SqlParameter[] sqlParameters =
           {
                new SqlParameter("@stocode", stocode),
                new SqlParameter("@orderdishcode", orderdishcode)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_OrderDish_Back", CommandType.StoredProcedure, sqlParameters);
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
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_OrderDish_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }
    }
}