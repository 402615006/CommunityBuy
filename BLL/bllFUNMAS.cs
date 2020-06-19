using CommunityBuy.CommonBasic;
using System.Data;
using System.Data.SqlClient;

namespace CommunityBuy.BLL
{
    public class bllFUNMAS
    {
        MSSqlDataAccess Obj = new MSSqlDataAccess();
        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="pageSize">页面记录数</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="filter">条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="recnums">总记录数</param>
        /// <returns>返回数据表</returns>
        public DataTable GetPagingInfo(int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            return new bllPaging().GetPagingInfo("functions", "id", "*", pageSize, currentpage, filter, string.Empty, order, out recnums, out pagenums);
        }


        public DataTable GetParentID(int parentID)
        {
            SqlParameter[] pars = { 
                                  new SqlParameter("@parentID",parentID)
                                  };
            DataTable dt = Obj.ExecuteDataTable("pro_GetParentID", CommandType.StoredProcedure, pars);
            return dt;
        }
    }
}
