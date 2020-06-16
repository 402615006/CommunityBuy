using System;
using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 数据访问类
    /// </summary>
    public partial class dalTB_BillInvoice
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_BillInvoiceEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@PKCode",SqlDbType.NVarChar,32,Entity.PKCode),
				new SqlParameter("@BillCode", Entity.BillCode),
				new SqlParameter("@InMoney", Entity.InMoney),
				new SqlParameter("@CardMoney", Entity.CardMoney),
				new SqlParameter("@OtherMoney", Entity.OtherMoney),
             };
            sqlParameters[5].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_BillInvoice_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.PKCode = sqlParameters[5].Value.ToString();
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_BillInvoiceEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@PKCode", Entity.PKCode),
				new SqlParameter("@BillCode", Entity.BillCode),
				new SqlParameter("@InMoney", Entity.InMoney),
				new SqlParameter("@CardMoney", Entity.CardMoney),
				new SqlParameter("@OtherMoney", Entity.OtherMoney),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_BillInvoice_Update", CommandType.StoredProcedure, sqlParameters); 
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
            return DBHelper.ExecuteNonQuery("dbo.p_TB_BillInvoice_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
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
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_BillInvoice_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 获取账单的发票信息
        /// </summary>
        /// <param name="stocode"></param>
        /// <param name="billCode"></param>
        /// <returns></returns>
        public DataSet GetBillInvoiceDetail( string stocode, string billCode, string billtype)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@StoCode", stocode),
                 new SqlParameter("@BillCode",billCode),
                 new SqlParameter("@BillType",billtype)
             };
            DataSet ds = DBHelper.ExecuteDataSet("dbo.p_TB_BillInvoice_Detail", CommandType.StoredProcedure, sqlParameters);
            return ds;
        }
    }
}