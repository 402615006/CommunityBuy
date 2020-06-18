using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.CommonBasic
{
    /// <summary>
    /// 数据库操作抽象类
    /// </summary>
    public abstract class BaseDataAccess
    {

        #region Property & method
        /// <summary>
        /// 数据库连接串
        /// </summary>
        public abstract string ConnectionString
        {
            get;
            set;
        }        
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public abstract void Open();
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// Web事务
        /// </summary>
        public abstract void BeginTransaction();
        /// <summary>
        /// 提交事务
        /// </summary>
        public abstract void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        public abstract void RollBack();

        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 执行SQL命令,并返回受影响的行数
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行SQL命令,并返回受影响的行数
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, commandType, null);
        }



        /// <summary>
        /// 用事务执行SQL命令,并返回数据集
        /// </summary>
        /// <param name="sql">sql语句（不包含事务语句）</param>
        /// <returns></returns>
        public int ExecuteSqlByTran(string sql)
        {
            int intResult = 0;
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine(" BEGIN TRAN tan1");//开始事务
            Builder.AppendLine(sql);//开始事务
            Builder.AppendLine(" if(@@error=0) begin commit tran tan1;" + sql + " end else begin rollback tran tran1 end");//结束事务
            try
            {
                return ExecuteNonQuery(Builder.ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog,"BaseDataAccess.ExecuteDataSetByTran() " + ex.Message);
            }

            return intResult;
        }

        /// <summary>
        /// 执行SQL命令,并返回受影响的行数
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <param name="Parameters">命令参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public abstract int ExecuteNonQuery(string commandText, CommandType commandType, SqlParameter[] Parameters);
        #endregion

        #region ExecuteDataReader
        /// <summary>
        /// 执行SQL命令,并返回数据的只读流
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <returns>返回数据的只读流</returns>
        public DbDataReader ExecuteDataReader(string commandText)
        {
            return ExecuteDataReader(commandText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行SQL命令,并返回数据的只读流
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <returns>返回数据的只读流</returns>
        public DbDataReader ExecuteDataReader(string commandText, CommandType commandType)
        {
            return ExecuteDataReader(commandText, commandType, null);
        }
        /// <summary>
        /// 执行SQL命令,并返回数据的只读流
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <param name="Parameters">命令参数集合</param>
        /// <returns>返回数据的只读流</returns>
        public abstract DbDataReader ExecuteDataReader(string commandText, CommandType commandType, SqlParameter[] Parameters);
        #endregion

        #region ExecuteDataTable
        /// <summary>
        /// 执行SQL命令,并返回数据表
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <returns>返回数据表</returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行SQL命令,并返回数据表
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <returns>返回数据表</returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return ExecuteDataTable(commandText, commandType, null);
        }
        /// <summary>
        /// 执行SQL命令,并返回数据表
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <param name="Parameters">命令参数集合</param>
        /// <returns>返回数据表</returns>
        public abstract DataTable ExecuteDataTable(string commandText, CommandType commandType, SqlParameter[] Parameters);

        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// 执行SQL命令,并返回数据集
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <returns>返回数据集</returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行SQL命令,并返回数据集
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <returns>返回数据集</returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return ExecuteDataSet(commandText, commandType, null);
        }
        /// <summary>
        /// 执行SQL命令,并返回数据集
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <param name="Parameters">命令参数集合</param>
        /// <returns>返回数据集</returns>
        public abstract DataSet ExecuteDataSet(string commandText, CommandType commandType, SqlParameter[] Parameters);

        /// <summary>
        /// 用事务执行SQL命令,并返回数据集
        /// </summary>
        /// <param name="sql">sql语句（不包含事务语句）</param>
        /// <returns></returns>
        public DataSet ExecuteDataSetByTran(string sql)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine(" BEGIN TRAN tan1");//开始事务
            Builder.AppendLine(sql);//开始事务
            Builder.AppendLine(" if(@@error=0) begin commit tran tan1;" + sql + " end else begin rollback tran tran1 end");//结束事务
            try
            {
                return ExecuteDataSet(Builder.ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog,"dalchoorderdishes.Update() " + ex.Message);
            }

            return null;
        }


        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行SQL命令,并返回结果的第一行第一列
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <returns>返回结果的第一行第一列</returns>
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, CommandType.Text, null);
        }
        /// <summary>
        /// 执行SQL命令,并返回结果的第一行第一列
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <returns>返回结果的第一行第一列</returns>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return ExecuteScalar(commandText, commandType, null);
        }
        /// <summary>
        /// 执行SQL命令,并返回结果的第一行第一列
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">SQL命令类型</param>
        /// <param name="parameters">命令参数集合</param>
        /// <returns>返回结果的第一行第一列</returns>
        public abstract object ExecuteScalar(string commandText, CommandType commandType, SqlParameter[] parameters);
        #endregion

        #region 扩展tsg
        /// <summary>
        /// 分页获取菜品数据（带附加数据，开台用到此函数）
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <param name="pagenums"></param>
        /// <returns></returns>
        public DataSet GetDishesDataSetPagingListInfo(int pageSize, int currentpage, string filter,string order, out int recnums, out int pagenums)
        {
            DataSet dsAttachData = new DataSet();

            DataTable dtDishes = GetPagingInfo("dishes", "discode", "*,cusername=dbo.fnGetUserName(cuser),uusername=dbo.fnGetUserName(uuser)"+
                                                ",melname=dbo.fnGetDisheMealName2(discode,stocode),distypename=dbo.fnGetDisheTypeName2(distypecode,stocode)" +
                                                ",finname=dbo.fnGetFinanceTypeName2(fincode,stocode)", pageSize, currentpage, filter, "", order, out recnums, out pagenums);

            if (dtDishes != null && dtDishes.Rows.Count > 0)
            {
                dsAttachData = new DataSet();
                dsAttachData.Tables.Add(dtDishes);

                #region 附加数据
                string strRelationCodes = string.Empty;//关联父编号
                for (int j = 0; j < dtDishes.Rows.Count; j++)
                {
                    strRelationCodes += "," + "'" + dtDishes.Rows[j]["discode"].ToString() + "'";
                }
                strRelationCodes = strRelationCodes.TrimStart(',');

                string buscode = string.Empty;//商户编号
                string stocode = string.Empty;//门店编号
                buscode = dtDishes.Rows[0]["buscode"].ToString();
                if (string.IsNullOrEmpty(stocode) || stocode.Trim() == "")
                {
                    stocode = dtDishes.Rows[0]["stocode"].ToString();
                }

                StringBuilder sbSql = new StringBuilder();
                sbSql.Append(string.Format(" select *,methodname=dbo.fnGetmethodsName(methodcode) from DishesMethods where buscode='{1}' and stocode='{2}' and discode in ({0});  ", strRelationCodes, buscode, stocode));//菜品做法加价
                sbSql.Append(string.Format(" select *,matclassname=dbo.fnGetStockMateClassNameByMatcode(matcode) from DishesMate where buscode='{1}' and stocode='{2}' and discode in ({0});", strRelationCodes, buscode, stocode));//菜品所用配料
                sbSql.Append(string.Format(" select m.*,matclassname=dbo.fnGetStockMateClassNameByMatcode(m.matcode),dm.discode from StockMateUnits m "+
                                            " INNER JOIN (select matcode,discode from DishesMate  where discode in ({0})) dm ON m.matcode=dm.matcode;", strRelationCodes, buscode, stocode));//菜品所用配料单位信息
                sbSql.Append(string.Format(" select dm.*,m.isdefault,m.melname from DishesMeal dm INNER JOIN Meal m ON dm.buscode=m.buscode and dm.melcode=m.melcode "+
                                            " where dm.buscode='{1}' and dm.stocode='{2}' and dm.discode in ({0})  ;", strRelationCodes, buscode, stocode));//菜品所属菜谱

                sbSql.Append(string.Format(" select a.discomid,b.* from dishescombo a inner join dbo.dishes b on a.buscode=b.buscode and a.stocode=b.stocode and a.discode=b.discode " +
                                            " where b.buscode='{1}' and b.stocode='{2}' and b.discode in ({0});  ", strRelationCodes, buscode, stocode));//套餐标配菜品
                sbSql.Append(string.Format(" select d.*,b.distypecode from dishesoptional d " +
                                           " inner join dbo.dishes b on d.buscode=b.buscode and d.stocode=b.stocode and d.usediscode=b.discode  " +
                                           " where d.buscode='{1}' and d.stocode='{2}' and d.discode in ({0});  ", strRelationCodes, buscode, stocode));//套餐可选餐品

                string strSqlBP = string.Format(" SELECT d.*,pd.packagetype FROM dbo.dishes d INNER JOIN " +  //套餐菜品(标配菜品和可选菜品)
                                            " ( " +
                                                " select usediscode AS discode,'c' AS packagetype  from dishescombo where buscode='{1}' and stocode='{2}' and discode in ({0}) " +
                                                " union " +
                                                " select usediscode AS discode,'o' AS packagetype from dishesoptional where buscode='{1}' and stocode='{2}' and discode in ({0}) " +
                                            " ) pd ON d.discode=pd.discode " +
                                            " where d.buscode='{1}' and d.stocode='{2}' and d.discode in ({0});  ", strRelationCodes, buscode, stocode)
                                            ;

                sbSql.Append(string.Format(" select * from DisheType ;  ", strRelationCodes, buscode, stocode));//菜品类别
                sbSql.Append(strSqlBP);
                

                DataSet dsAttachData2 = ExecuteDataSet(sbSql.ToString());
                if (dsAttachData2 != null && dsAttachData2.Tables.Count > 0)
                {
                    for (int k = 0; k < dsAttachData2.Tables.Count; k++)
                    {
                        try
                        {
                            dsAttachData.Tables.Add(dsAttachData2.Tables[k].Copy());
                        }
                        catch (Exception err)
                        {
                            ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog,"blldishes.GetDataSetPagingListInfo：" + err.Message);
                        }
                    }
                }
                #endregion
            }

            return dsAttachData;
        }


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
                Dt = ExecuteDataTable("dbo.pPagingLarge", CommandType.StoredProcedure, sqlParameters);
                recnums = StringHelper.StringToInt(sqlParameters[8].Value.ToString());
                pagenums = StringHelper.StringToInt(sqlParameters[9].Value.ToString());
                return Dt;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                recnums = 0;
                pagenums = 0;
                return Dt;
            }
        }


        #endregion

    }
}
