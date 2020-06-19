using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 菜品类别表数据访问类
    /// </summary>
    public partial class dalTB_DishType
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_DishTypeEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@BusCode", Entity.BusCode),
                new SqlParameter("@StoCode", Entity.StoCode),
                new SqlParameter("@PKKCode", Entity.PKKCode),
                new SqlParameter("@PKCode", Entity.PKCode),
                new SqlParameter("@TypeName", Entity.TypeName),
                new SqlParameter("@Sort", Entity.Sort),
                new SqlParameter("@TStatus", Entity.TStatus)
             };
            sqlParameters[5].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_DishType_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.PKCode = sqlParameters[5].Value.ToString();
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_DishTypeEntity Entity)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@BusCode", Entity.BusCode),
                new SqlParameter("@StoCode", Entity.StoCode),
                new SqlParameter("@PKKCode", Entity.PKKCode),
                new SqlParameter("@PKCode", Entity.PKCode),
                new SqlParameter("@TypeName", Entity.TypeName),
                new SqlParameter("@Sort", Entity.Sort),
                new SqlParameter("@TStatus", Entity.TStatus)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_DishType_Update", CommandType.StoredProcedure, sqlParameters);
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
            return DBHelper.ExecuteNonQuery("dbo.p_TB_DishType_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
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
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_DishType_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 菜品类别管理树节点
        /// </summary>
        /// <returns></returns>
        public DataTable GetDisTypeTreeListInfo(string filter, string order)
        {
            string sql = "select PKKCode as pId,PKCode as id,TypeName,(case TStatus when '0' then '(无效)' else '(有效)' end) as TStatusName,StoCode,Sort,'' as StoName,dbo.fn_GetDisTypeParentName(PKKCode) as PPKName,TStatus from TB_DishType ";
            if(!string.IsNullOrEmpty(filter))
            {
                sql += filter;
            }
            if(!string.IsNullOrEmpty(order))
            {
                sql += order;
            }
            return DBHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 菜品类别获取一级大类
        /// </summary>
        /// <returns></returns>
        public DataTable GetDisTypeOneListInfo(string filter, string order)
        {
            string sql = "select PKCode,TypeName from TB_DishType ";
            if (!string.IsNullOrEmpty(filter))
            {
                sql += filter+ " and PKKCode=''";
            }
            else
            {
                sql += " where PKKCode=''";
            }
            if (!string.IsNullOrEmpty(order))
            {
                sql += order;
            }
            return DBHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 根据一级类别获取旗下二级类别
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataTable GetDishTypePPKToPKData(string filter,string order)
        {
            string sql = "select PKCode,TypeName from TB_DishType ";
            if (!string.IsNullOrEmpty(filter))
            {
                sql += filter;
            }
            if (!string.IsNullOrEmpty(order))
            {
                sql += order;
            }
            return DBHelper.ExecuteDataTable(sql);
        }

    }
}