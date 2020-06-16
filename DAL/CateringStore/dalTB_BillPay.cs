using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 账单支付数据访问类
    /// </summary>
    public partial class dalTB_BillPay
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_BillPayEntity Entity,string DiscountName,string DiscountMoney,string ZeroCutMoney,string AuthCode,string AuthName,string MemberCard,string MemberName,string MemberBalance,string MemberLeve,string MemberDiscount)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
                new SqlParameter("@PKCode", SqlDbType.VarChar,32){ Value=Entity.PKCode},
                new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@BillCode", Entity.BillCode),
				new SqlParameter("@PayMoney", Entity.PayMoney),
				new SqlParameter("@PayMethodName", Entity.PayMethodName),
				new SqlParameter("@PayMethodCode", Entity.PayMethodCode),
				new SqlParameter("@Remar", Entity.Remar),
				new SqlParameter("@OutOrderCode", Entity.OutOrderCode),
				new SqlParameter("@PPKCode", Entity.PPKCode),
                new SqlParameter("@DiscountMoney",DiscountMoney),
                new SqlParameter("@DiscountName", DiscountName),
                new SqlParameter("@zerocutmoney", ZeroCutMoney),
                new SqlParameter("@AuthCode", AuthCode),
                new SqlParameter("@AuthName", AuthName),
                new SqlParameter("@MemberCard", MemberCard),
                new SqlParameter("@MemberName", MemberName),
                new SqlParameter("@MemberBalance", MemberBalance),
                new SqlParameter("@MemberLeve", MemberLeve),
                new SqlParameter("@MemberDiscount", MemberDiscount)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_BillPay_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.PKCode = sqlParameters[0].Value.ToString();
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_BillPayEntity Entity)
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
				new SqlParameter("@PayMoney", Entity.PayMoney),
				new SqlParameter("@PayMethodName", Entity.PayMethodName),
				new SqlParameter("@PayMethodCode", Entity.PayMethodCode),
				new SqlParameter("@Remar", Entity.Remar),
				new SqlParameter("@OutOrderCode", Entity.OutOrderCode),
				new SqlParameter("@PPKCode", Entity.PPKCode),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_BillPay_Update", CommandType.StoredProcedure, sqlParameters); 
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string StoCode,string PKCode, string Status, string DiscountName, string DiscountMoney, string ZeroCutMoney, string AuthCode, string AuthName)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@PKCode", PKCode),
                new SqlParameter("@stocode", StoCode),
                new SqlParameter("@status", Status),
                new SqlParameter("@DiscountMoney",DiscountMoney),
                new SqlParameter("@DiscountName", DiscountName),
                new SqlParameter("@zerocutmoney", ZeroCutMoney),
                new SqlParameter("@AuthCode", AuthCode),
                new SqlParameter("@AuthName", AuthName)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_BillPay_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatusByOurOrderCode(string StoCode, string Status, string OutOrderCode)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@stocode", StoCode),
                new SqlParameter("@status", Status),
                new SqlParameter("@OutOrderCode",OutOrderCode)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_BillPay_UpdateStatusByOutOrderCode", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string pkcode,string stocode ,ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@PKCode", pkcode),
                 new SqlParameter("@StoCode", stocode)
             };
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_BillPay_Delete", CommandType.StoredProcedure, sqlParameters);
            return intReturn;
        }

        /// <summary>
        /// 反结算
        /// </summary>
        /// <returns></returns>
        public int Back(ref TB_BillPayEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@PKCode", SqlDbType.VarChar,32,Entity.PKCode),
                new SqlParameter("@OutOrderCode", SqlDbType.VarChar,32,Entity.OutOrderCode),
                new SqlParameter("@PayMethodCode", SqlDbType.VarChar,32,Entity.PayMethodCode),
                new SqlParameter("@BusCode", Entity.BusCode),
                new SqlParameter("@StoCode", Entity.StoCode),
                new SqlParameter("@CCode", Entity.CCode),
                new SqlParameter("@CCname", Entity.CCname),
                new SqlParameter("@TStatus", Entity.TStatus),
                new SqlParameter("@PayMoney", Entity.PayMoney),
                new SqlParameter("@Remar", Entity.Remar),
                new SqlParameter("@PPKCode",  Entity.PPKCode),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[1].Direction = ParameterDirection.Output;
            sqlParameters[2].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_BillPay_Back", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.PKCode = sqlParameters[0].Value.ToString();
                Entity.OutOrderCode= sqlParameters[1].Value.ToString();
                Entity.PayMethodCode= sqlParameters[2].Value.ToString();
            }
            return intReturn;
        }
    }
}