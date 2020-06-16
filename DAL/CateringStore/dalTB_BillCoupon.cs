using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 账单优惠券数据访问类
    /// </summary>
    public partial class dalTB_BillCoupon
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(ref TB_BillCouponEntity Entity, ref string mescode)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@BillCode", Entity.BillCode),
				new SqlParameter("@CouponCode", Entity.CouponCode),
				new SqlParameter("@CouponMoney", Entity.CouponMoney),
				new SqlParameter("@MemberCardCode", Entity.MemberCardCode),
				new SqlParameter("@RealPay", Entity.RealPay),
				new SqlParameter("@VIMoney", Entity.VIMoney),
				new SqlParameter("@Remark", Entity.Remark),
				new SqlParameter("@UseType", Entity.UseType),
				new SqlParameter("@ShiftCode", Entity.ShiftCode),
				new SqlParameter("@CouponName", Entity.CouponName),
                new SqlParameter("@McCode", Entity.McCode),
                new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode),
                new SqlParameter("@TicType",Entity.TicType),
                new SqlParameter("@TicWay",Entity.TicWay)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[17].Direction = ParameterDirection.Output;
            DataTable dt = DBHelper.ExecuteDataTable("dbo.p_TB_BillCoupon_Add", CommandType.StoredProcedure, sqlParameters);
            if (dt!=null&&dt.Rows.Count>0)
            {
                Entity.Id = int.Parse(sqlParameters[0].Value.ToString());
            }
            return dt;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_BillCouponEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@BillCode", Entity.BillCode),
				new SqlParameter("@CouponCode", Entity.CouponCode),
				new SqlParameter("@CouponMoney", Entity.CouponMoney),
				new SqlParameter("@MemberCardCode", Entity.MemberCardCode),
				new SqlParameter("@RealPay", Entity.RealPay),
				new SqlParameter("@VIMoney", Entity.VIMoney),
				new SqlParameter("@Remark", Entity.Remark),
				new SqlParameter("@UseType", Entity.UseType),
				new SqlParameter("@ShiftCode", Entity.ShiftCode),
				new SqlParameter("@CouponName", Entity.CouponName),
                new SqlParameter("@McCode", Entity.McCode),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_BillCoupon_Update", CommandType.StoredProcedure, sqlParameters); 
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataSet UpdateStatus(string StoCode, string BillCode, string id, string Status, string isget, string OrderDishId, decimal DiscountPrice)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@id", id),
				new SqlParameter("@StoCode", StoCode),
				new SqlParameter("@BillCode", BillCode),
				new SqlParameter("@status", Status),
				new SqlParameter("@isget", isget),
				new SqlParameter("@OrderDishId", OrderDishId),
				new SqlParameter("@DiscountPrice", DiscountPrice)
             };
            return DBHelper.ExecuteDataSet("dbo.p_TB_BillCoupon_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 取消使用优惠券
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataSet CancelCoupon(string StoCode, string BillCode, string CouponCode, string isget)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@couponcode", CouponCode),
                new SqlParameter("@StoCode", StoCode),
                new SqlParameter("@BillCode", BillCode),
                new SqlParameter("@isget", isget)
             };
            return DBHelper.ExecuteDataSet("dbo.p_TB_BillCoupon_Cancel", CommandType.StoredProcedure, sqlParameters);
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
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_BillCoupon_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }
    }
}