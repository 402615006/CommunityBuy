using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 会员卡订单表数据访问类
    /// </summary>
    public partial class dalmemcardorders
    {
        public MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        MSSqlDataAccess LSDBHelper = new MSSqlDataAccess("CateringDBConnectionString");
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref memcardordersEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ID", Entity.ID),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@stocode", Entity.stocode),
				new SqlParameter("@memcode", Entity.memcode),
				new SqlParameter("@cardcode", Entity.cardcode),
				new SqlParameter("@otype", Entity.otype),
				new SqlParameter("@regamount", Entity.regamount),
				new SqlParameter("@freeamount", Entity.freeamount),
				new SqlParameter("@cardcost", Entity.cardcost),
				new SqlParameter("@payamount", Entity.payamount),
				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@ucode", Entity.ucode),
				new SqlParameter("@uname", Entity.uname)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_memcardorders_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.ID = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ID">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_memcardorders_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 0元支付/退卡支付
        /// </summary>
        /// <param name="buscode"></param>
        /// <param name="stocode"></param>
        /// <param name="cardcode"></param>
        /// <param name="cardtype"></param>
        /// <param name="otype"></param>
        /// <param name="refundmoney"></param>
        /// <param name="postmac"></param>
        /// <param name="ordercode"></param>
        /// <param name="shiftid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int RefundMemcard(string buscode, string stocode, string cardcode, string cardtype, string otype, string refundmoney, string postmac, string ordercode, string shiftid, string userid, string terminaltype, ref string payordercode)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@buscode", buscode),
				new SqlParameter("@stocode", stocode),
                new SqlParameter("@cardcode", cardcode),
                new SqlParameter("@cardtype", cardtype),
                new SqlParameter("@otype", otype),
                new SqlParameter("@refundmoney", refundmoney),
                new SqlParameter("@postmac", postmac),
                new SqlParameter("@ordercode", ordercode),
                new SqlParameter("@shiftid", shiftid),
                new SqlParameter("@userid", userid),
                new SqlParameter("@payordercode",SqlDbType.VarChar, 32),
                new SqlParameter("@terminaltype", terminaltype)
             };
            sqlParameters[10].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_RefundMemcard", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 1)
            {
                payordercode = sqlParameters[10].Value.ToString();
            }
            return intReturn;
        }

        /// <summary>
        /// 获取连锁端有效的会员卡类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetMemCardTypeList(string BusCode)
        {
            if(!string.IsNullOrEmpty(BusCode))
            {
                return LSDBHelper.ExecuteDataTable("select * from [dbo].[memcardtype] where [status]='1' and buscode='"+BusCode+"'");
            }
            else
            {
                return LSDBHelper.ExecuteDataTable("select * from [dbo].[memcardtype] where [status]='1'");
            }
        }

    }
}