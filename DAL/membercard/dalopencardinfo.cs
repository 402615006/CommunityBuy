using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 发卡临时信息表数据访问类
    /// </summary>
    public partial class dalopencardinfo
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref opencardinfoEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@oid", Entity.oid),
				new SqlParameter("@ordercode", Entity.ordercode),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@stocode", Entity.stocode),
				new SqlParameter("@memcode", Entity.memcode),
				new SqlParameter("@cardcode", Entity.cardcode),
				new SqlParameter("@regamount", Entity.regamount),
				new SqlParameter("@freeamount", Entity.freeamount),
				new SqlParameter("@cardcost", Entity.cardcost),
				new SqlParameter("@payamount", Entity.payamount),
				new SqlParameter("@validate", Entity.validate),
				new SqlParameter("@password", Entity.password),
				new SqlParameter("@nowritedoc", Entity.nowritedoc),
				new SqlParameter("@cname", Entity.cname),
				new SqlParameter("@mobile", Entity.mobile),
				new SqlParameter("@idtype", Entity.idtype),
				new SqlParameter("@IDNO", Entity.IDNO),
				new SqlParameter("@ucode", Entity.ucode),
				new SqlParameter("@uname", Entity.uname),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_opencardinfo_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.oid = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(opencardinfoEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@oid", Entity.oid),
				new SqlParameter("@ordercode", Entity.ordercode),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@stocode", Entity.stocode),
				new SqlParameter("@memcode", Entity.memcode),
				new SqlParameter("@cardcode", Entity.cardcode),
				new SqlParameter("@regamount", Entity.regamount),
				new SqlParameter("@freeamount", Entity.freeamount),
				new SqlParameter("@cardcost", Entity.cardcost),
				new SqlParameter("@payamount", Entity.payamount),
				new SqlParameter("@validate", Entity.validate),
				new SqlParameter("@password", Entity.password),
				new SqlParameter("@nowritedoc", Entity.nowritedoc),
				new SqlParameter("@cname", Entity.cname),
				new SqlParameter("@mobile", Entity.mobile),
				new SqlParameter("@idtype", Entity.idtype),
				new SqlParameter("@IDNO", Entity.IDNO),
				new SqlParameter("@ucode", Entity.ucode),
				new SqlParameter("@uname", Entity.uname),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_opencardinfo_Update", CommandType.StoredProcedure, sqlParameters); 
        }
    }
}