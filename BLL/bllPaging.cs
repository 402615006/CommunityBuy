using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BLL
{
    public class bllPaging
    {
        public MSSqlDataAccess Obj = new MSSqlDataAccess();
        MSSqlDataAccess LS_Obj = new MSSqlDataAccess("CateringDBConnectionStringR");

        /// <summary>
        /// 分页功能
        /// </summary>
        /// <param name="tableName">表名，可以是多个表，最好用别名</param>
        /// <param name="primarykey">主键，可以为空，但@order为空时该值不能为空</param>
        /// <param name="fields">要取出的字段，可以是多个表的字段，可以为空，为空表示select *  </param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentpage">当前页，表示第页</param>
        /// <param name="filter">条件，可以为空，不用填where</param>
        /// <param name="group">分组依据，可以为空，不用填group by</param>
        /// <param name="order">排序，可以为空，为空默认按主键升序排列，不用填order by</param>
        /// <param name="recnums">记录个数</param>
        /// <param name="pagenums">页数</param>
        /// <returns></returns>
        public DataTable GetPagingInfo(string tableName, string primarykey, string fields, int pageSize, int currentpage, string filter, string group, string order, out int recnums, out int pagenums)
        {
            DataTable Dt = new DataTable("data");
            try
            {
                SqlParameter[] sqlParameters = 
                {
                     new SqlParameter("@tablenames", tableName),
                     new SqlParameter("@primarykey", primarykey),
                     new SqlParameter("@fields", fields),
                     new SqlParameter("@pagesize", pageSize),
                     new SqlParameter("@currentpage", currentpage),
                     new SqlParameter("@filter", filter),
                     new SqlParameter("@group", group),
                     new SqlParameter("@order", order),
                     new SqlParameter("@recnums", 0),
                     new SqlParameter("@pagenums", 0)
                 };
                sqlParameters[8].Direction = ParameterDirection.Output;
                sqlParameters[9].Direction = ParameterDirection.Output;
                //只读服务器
                Obj = new MSSqlDataAccess("DBConnectionString");
                Dt = Obj.ExecuteDataTable("dbo.pPagingLarge", CommandType.StoredProcedure, sqlParameters);
                recnums = StringHelper.StringToInt(sqlParameters[8].Value.ToString());
                pagenums = StringHelper.StringToInt(sqlParameters[9].Value.ToString());
                return Dt;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, fields + "->" + tableName + "->" + filter);
                recnums = 0;
                pagenums = 0;
                return Dt;
            }
        }

        /// <summary>
        /// 分页功能
        /// </summary>
        /// <param name="tableName">表名，可以是多个表，最好用别名</param>
        /// <param name="primarykey">主键，可以为空，但@order为空时该值不能为空</param>
        /// <param name="fields">要取出的字段，可以是多个表的字段，可以为空，为空表示select *  </param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentpage">当前页，表示第页</param>
        /// <param name="filter">条件，可以为空，不用填where</param>
        /// <param name="group">分组依据，可以为空，不用填group by</param>
        /// <param name="order">排序，可以为空，为空默认按主键升序排列，不用填order by</param>
        /// <param name="recnums">记录个数</param>
        /// <param name="pagenums">页数</param>
        /// <returns></returns>
        public DataTable GetPagingInfoTime(string tableName, string primarykey, string fields, int pageSize, int currentpage, string filter, string group, string order, out int recnums, out int pagenums)
        {
            DataTable Dt = new DataTable("data");
            try
            {
                SqlParameter[] sqlParameters =
                {
                     new SqlParameter("@tablenames", tableName),
                     new SqlParameter("@primarykey", primarykey),
                     new SqlParameter("@fields", fields),
                     new SqlParameter("@pagesize", pageSize),
                     new SqlParameter("@currentpage", currentpage),
                     new SqlParameter("@filter", filter),
                     new SqlParameter("@group", group),
                     new SqlParameter("@order", order),
                     new SqlParameter("@recnums", 0),
                     new SqlParameter("@pagenums", 0)
                 };
                sqlParameters[8].Direction = ParameterDirection.Output;
                sqlParameters[9].Direction = ParameterDirection.Output;
                
                Dt = Obj.ExecuteDataTable("dbo.pPagingLarge", CommandType.StoredProcedure, sqlParameters);
                recnums = StringHelper.StringToInt(sqlParameters[8].Value.ToString());
                pagenums = StringHelper.StringToInt(sqlParameters[9].Value.ToString());
                return Dt;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, fields + "->" + tableName + "->" + filter);
                recnums = 0;
                pagenums = 0;
                return Dt;
            }
        }


        /// <summary>
        /// 执行存储过程获取数据表
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public DataTable GetDataTableInfoByProcedure(string ProcedureName, SqlParameter[] sqlParameters)
        {
            DataTable Dt = new DataTable("data");
            try
            {
                Dt = Obj.ExecuteDataTable(ProcedureName, CommandType.StoredProcedure, sqlParameters);
                return Dt;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                return Dt;
            }
        }

        /// <summary>
        /// 执行存储过程获取数据表
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public DataSet GetDatasetByProcedure(string ProcedureName, SqlParameter[] sqlParameters)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = Obj.ExecuteDataSet(ProcedureName, CommandType.StoredProcedure, sqlParameters);
                return ds;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                return ds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public DataTable GetDataTableInfoBySQL(string SQL)
        {
            return Obj.ExecuteDataTable(SQL);
        }

        public DataSet GetDataSetInfoBySQL(string SQL)
        {
            return Obj.ExecuteDataSet(SQL);
        }

        /// <summary>
        /// 执行SQL 返回第一行第一列
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public string ExecuteScalarBySQL(string SQL)
        {
            object obje = Obj.ExecuteScalar(SQL);
            if (obje != null)
            {
                return obje.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 执行SQL命令,并返回受影响的行数
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public int ExecuteNonQueryBySQL(string SQL)
        {
            return Obj.ExecuteNonQuery(SQL);
        }

        /// <summary>
        /// 执行SQL命令,并返回受影响的行数
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public int ExecuteNonQueryNoLogBySQL(string SQL)
        {
            return Obj.ExecuteNonQueryNoLog(SQL,CommandType.Text,null);
        }

        public int UpdateData(string table, string colname, string colvalue, string where)
        {
            int rel = -1;
            try
            {
                SqlParameter[] sqlParameters =
                {
                     new SqlParameter("@tablename", table),
                     new SqlParameter("@colname", colname),
                     new SqlParameter("@colvalue", colvalue),
                     new SqlParameter("@where", where)
                 };
                rel = Obj.ExecuteNonQuery("[dbo].[p_UpdateTableData]", CommandType.StoredProcedure, sqlParameters);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
            return rel;
        }

        /// <summary>
        /// 分页功能
        /// </summary>
        /// <param name="tableName">表名，可以是多个表，最好用别名</param>
        /// <param name="primarykey">主键，可以为空，但@order为空时该值不能为空</param>
        /// <param name="fields">要取出的字段，可以是多个表的字段，可以为空，为空表示select *  </param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentpage">当前页，表示第页</param>
        /// <param name="filter">条件，可以为空，不用填where</param>
        /// <param name="group">分组依据，可以为空，不用填group by</param>
        /// <param name="order">排序，可以为空，为空默认按主键升序排列，不用填order by</param>
        /// <param name="recnums">记录个数</param>
        /// <param name="pagenums">页数</param>
        /// <returns></returns>
        public DataTable GetLSPagingInfo(string tableName, string primarykey, string fields, int pageSize, int currentpage, string filter, string group, string order, out int recnums, out int pagenums)
        {
            DataTable Dt = new DataTable("data");
            try
            {
                SqlParameter[] sqlParameters =
                {
                     new SqlParameter("@tablenames", tableName),
                     new SqlParameter("@primarykey", primarykey),
                     new SqlParameter("@fields", fields),
                     new SqlParameter("@pagesize", pageSize),
                     new SqlParameter("@currentpage", currentpage),
                     new SqlParameter("@filter", filter),
                     new SqlParameter("@group", group),
                     new SqlParameter("@order", order),
                     new SqlParameter("@recnums", 0),
                     new SqlParameter("@pagenums", 0)
                 };
                sqlParameters[8].Direction = ParameterDirection.Output;
                sqlParameters[9].Direction = ParameterDirection.Output;
                //只读服务器
                Dt = LS_Obj.ExecuteDataTable("dbo.pPagingLarge", CommandType.StoredProcedure, sqlParameters);
                recnums = StringHelper.StringToInt(sqlParameters[8].Value.ToString());
                pagenums = StringHelper.StringToInt(sqlParameters[9].Value.ToString());
                return Dt;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, fields + "->" + tableName + "->" + filter);
                recnums = 0;
                pagenums = 0;
                return Dt;
            }
        }
    }
}
