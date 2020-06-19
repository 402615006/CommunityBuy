using System;
using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 账单数据访问类
    /// </summary>
    public partial class dalTB_Bill
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_BillEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
                new SqlParameter("@PKCode",SqlDbType.NVarChar,32){ Value=Entity.PKCode },
                new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@OrderCodeList", Entity.OrderCodeList),
				new SqlParameter("@Remar", Entity.Remar),
                new SqlParameter("@BillType", Entity.BillType),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_Bill_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.PKCode = sqlParameters[0].Value.ToString();
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_BillEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@OrderCodeList", Entity.OrderCodeList),
				new SqlParameter("@PKCode", Entity.PKCode),
				new SqlParameter("@BillMoney", Entity.BillMoney),
				new SqlParameter("@PayMoney", Entity.PayMoney),
				new SqlParameter("@Remar", Entity.Remar),
				new SqlParameter("@FTime", Entity.FTime),
				new SqlParameter("@OpenDate", Entity.OpenDate),
                new SqlParameter("@BillType", Entity.BillType),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Bill_Update", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string billCode, string stoCode, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@billcode", billCode),
                new SqlParameter("@stocode", stoCode),
                new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Bill_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 更新账单消费赠送
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateGiveCoupon(string billCode, string stoCode, string coupons)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@billcode", billCode),
                new SqlParameter("@stocode", stoCode),
                new SqlParameter("@coupons", coupons)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Bill_UpdateGiveCoupon", CommandType.StoredProcedure, sqlParameters);
        }
        /// <summary>
        /// 给订单添加账单
        /// </summary>
        /// <param name="billCode">账单编号</param>
        /// <param name="orderCodeList">订单编号</param>
        /// <param name="StoCode">门店编号</param>
        /// <returns></returns>
        public int AddOrder(string billCode, string orderCodeList, string StoCode)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@billcode", billCode),
                new SqlParameter("@stocode", StoCode),
                new SqlParameter("@OrderCodeList", orderCodeList)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Bill_AddOrder", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 更新库存状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStockStatus(string billCode, string stoCode, string status)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@billcode", billCode),
                new SqlParameter("@stocode", stoCode),
                new SqlParameter("@status", status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Bill_Update_StockStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 更新状态取餐状态
        /// </summary>
        /// <param name="billCode"></param>
        /// <param name="stoCode"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public int UpdateCStatus(string billCode, string stoCode, string CStatus, string ccode, string cname)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@billcode", billCode),
                new SqlParameter("@stocode", stoCode),
                new SqlParameter("@cstatus", CStatus),
                new SqlParameter("@ccode",ccode),
                new SqlParameter("@cname",cname)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Bill_Update_CStatus", CommandType.StoredProcedure, sqlParameters);
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
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_Bill_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }


        /// <summary>
        /// 获取账单详情
        /// </summary>
        /// <param name="BillCode">账单编号</param>
        /// <param name="StoCode">门店编号</param>
        /// <param name="UserCode">用户编号</param>
        /// <returns></returns>
        public DataSet GetDetail(string BillCode, string StoCode, string UserCode)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@BillCode",BillCode),
                 new SqlParameter("@StoCode", StoCode),
                 new SqlParameter("@UserCode", UserCode)
             };
            return DBHelper.ExecuteDataSet("dbo.p_TB_Bill_Detail", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 获取会员卡账单详情
        /// </summary>
        /// <param name="BillCode">账单编号</param>
        /// <param name="StoCode">门店编号</param>
        /// <param name="UserCode">用户编号</param>
        /// <returns></returns>
        public DataSet GetMemDetail(string BillCode, string StoCode)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@BillCode",BillCode),
                 new SqlParameter("@StoCode", StoCode)
             };
            return DBHelper.ExecuteDataSet("dbo.p_TB_MemOrder_Detail", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 完成结账
        /// </summary>
        /// <param name="BillCode">账单编号</param>
        /// <param name="StoCode">门店编号</param>
        /// <returns></returns>
        public DataSet Finish(string BillCode, string StoCode)
        {
            int rel = 1;
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@BillCode",BillCode),
                 new SqlParameter("@StoCode", StoCode)
             };
            return DBHelper.ExecuteDataSet("dbo.p_TB_Bill_Finish", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 打印详情
        /// </summary>
        /// <param name="BillCode">账单编号</param>
        /// <param name="StoCode">门店编号</param>
        /// <returns></returns>
        public DataSet PrintDetail(string BillCode, string StoCode)
        {
            int rel = 1;
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@BillCode",BillCode),
                 new SqlParameter("@StoCode", StoCode)
             };
            return DBHelper.ExecuteDataSet("dbo.p_TB_Bill_PrintDetail", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 账单反结
        /// </summary>
        /// <param name="BillCode">账单编号</param>
        /// <param name="StoCode">门店编号</param>
        /// <returns></returns>
        public DataSet UnFinish(string BillCode, string StoCode, out int rv)
        {
            rv = 0;
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@BillCode",BillCode),
                 new SqlParameter("@StoCode", StoCode),
                 new SqlParameter("@outnum",rv)
             };
            sqlParameters[2].Direction = ParameterDirection.Output;
            DataSet ds = DBHelper.ExecuteDataSet("dbo.p_TB_Bill_UnFinish", CommandType.StoredProcedure, sqlParameters);
            rv = StringHelper.StringToInt(sqlParameters[2].Value.ToString());
            return ds;
        }

        /// <summary>
        /// 账单退款操作
        /// </summary>
        /// <param name="BusCode"></param>
        /// <param name="StoCode"></param>
        /// <param name="BillCode"></param>
        /// <param name="CCode"></param>
        /// <param name="CCname"></param>
        /// <param name="mescode"></param>
        /// <returns></returns>
        public DataTable BillReturn(string BusCode, string StoCode, string BillCode, string CCode, string CCname, out string mescode)
        {
            mescode = "";
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@BusCode",BusCode),
                 new SqlParameter("@StoCode",StoCode),
                 new SqlParameter("@BillCode",BillCode),
                 new SqlParameter("@CCode", CCode),
                 new SqlParameter("@CCname",CCname),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,64,mescode)
             };
            sqlParameters[5].Direction = ParameterDirection.Output;
            DataTable dt = DBHelper.ExecuteDataTable("dbo.p_TB_Bill_Return", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[5].Value.ToString();
            if (mescode.Length > 0)
            {
                return null;
            }
            else
            {
                return dt;
            }
        }
    }
}