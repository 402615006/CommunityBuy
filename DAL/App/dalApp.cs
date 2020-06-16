using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 账单数据访问类
    /// </summary>
    public partial class dalApp
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();

        /// <summary>
        /// 获取门店，菜品信息
        /// </summary>
        /// <param name="stocode"></param>
        /// <param name="tablecode"></param>
        /// <param name="menucode"></param>
        /// <returns></returns>
        public DataSet GetStoreDishInfo(string stocode,string tablecode,string menucode)
        {
            DataSet dsReturn = new DataSet();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@TableCode",tablecode ),
                new SqlParameter("@StoCode",stocode),
                new SqlParameter("@menucode",menucode)
             };
            dsReturn=DBHelper.ExecuteDataSet("p_App_GetStoreDish", CommandType.StoredProcedure, sqlParameters);
            return dsReturn;
        }

        /// <summary>
        /// 获取会员账单菜品信息
        /// </summary>
        /// <param name="stocode"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataSet GetMemBillDish(string memcode,string page,string pagesize,string status)
        {
            DataSet dsReturn = new DataSet();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@memcode",memcode),
                new SqlParameter("@status",status),
                new SqlParameter("@page",page),
                new SqlParameter("@pagesize",pagesize)
             };
            dsReturn = DBHelper.ExecuteDataSet("p_App_GetBillAndDishByMemCode", CommandType.StoredProcedure, sqlParameters);
            return dsReturn;
        }

        /// <summary>
        /// 获取桌台订单和未完成的账单
        /// </summary>
        /// <param name="stocode">门店编号</param>
        /// <param name="opencode">开台编号</param>
        /// <param name="menucode"></param>
        /// <returns></returns>
        public DataSet GetTableOrderAndBill(string stocode, string opencode)
        {
            DataSet dsReturn = new DataSet();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@opencode",opencode ),
                new SqlParameter("@stocode",stocode)
             };
            dsReturn = DBHelper.ExecuteDataSet("p_App_GetTableOrderAndBill", CommandType.StoredProcedure, sqlParameters);
            return dsReturn;
        }

        /// <summary>
        /// 取消账单
        /// </summary>
        /// <param name="stocde"></param>
        /// <param name="billcode"></param>
        /// <returns></returns>
        public int CancelBill(string stocde, string billcode)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@stocode",stocde ),
                new SqlParameter("@billcode",billcode)
             };
            return DBHelper.ExecuteNonQuery("p_App_TB_Bill_Cancel", CommandType.StoredProcedure, sqlParameters);
        }

    }
}