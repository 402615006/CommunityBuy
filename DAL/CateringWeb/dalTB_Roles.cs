using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 门店角色信息数据访问类
    /// </summary>
    public partial class dalTB_Roles
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;

        /// <summary>
        /// 检测名称是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public int CheckRoleName(string id, string rolename)
        {
            int Result = 0;
            DataTable dtCheck = DBHelper.ExecuteDataTable(string.Format("select * from TB_Roles where RoleName='{0}' and id<>{1} ", rolename, id));
            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                Result = 1;
            }
            dtCheck = null;
            return Result;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_RolesEntity Entity,string[] FunList)
        {
            intReturn = 0;
            StringBuilder Builder = new StringBuilder();
            DataTable dtStructure = DBHelper.ExecuteDataTable("select top 0 * from TB_Roles ");
            string InsertSql = string.Empty;
            Builder.Append(" BEGIN TRAN tan1");
            Builder.Append(" declare @roleid int;");
            int rel = CheckRoleName(Entity.Id.ToString(), Entity.RoleName);
            if (rel <= 0)
            {
                List<string> lstExcludeFilds = new List<string>();//构造sql语句排除的字段
                Dictionary<string, string> dicAttachFilds = new Dictionary<string, string>();//构造语句（格式是：<字段名，字段值>）

                Builder.AppendLine(" DECLARE @buscode varchar(16); DECLARE @stocode varchar(16); ");
                Builder.AppendLine(" SET @buscode='" + Entity.BusCode + "';");
                Builder.AppendLine(" SET @stocode='" + Entity.StoCode + "';");

                Builder.AppendLine(" DECLARE @ID VARCHAR(20); SET @ID='';");

                if (Entity.Id == 0)
                {//添加
                    #region 添加
                    lstExcludeFilds.Clear();
                    lstExcludeFilds.Add("Id");//methodsmid字段不在构造插入语句内
                    InsertSql = EntityHelper.GenerateSqlByDE<TB_RolesEntity>(dtStructure, Entity, lstExcludeFilds, dicAttachFilds, EntityHelper.eSqlType.insert);//构造sql语句
                    Builder.AppendFormat(" INSERT INTO TB_Roles " + InsertSql);
                    Builder.Append(" set @roleid=SCOPE_IDENTITY();");
                    Builder.AppendLine(" SET @ID=CONVERT(VARCHAR(20),@roleid); ");
                    Builder.AppendLine(" exec dbo.p_uploaddata_isSync  @buscode,@stocode,'TB_Roles','id',@ID,'add'; ");
                    #endregion
                }
                else
                {//修改
                    #region 修改

                    lstExcludeFilds.Clear();
                    lstExcludeFilds.Add("Id");//methodsmid字段不在构造插入语句内
                    InsertSql = EntityHelper.GenerateSqlByDE<TB_RolesEntity>(dtStructure, Entity, lstExcludeFilds, dicAttachFilds, EntityHelper.eSqlType.update);//构造sql语句
                    Builder.AppendFormat(" update TB_Roles set " + InsertSql + " where id=" + Entity.Id.ToString());
                    Builder.Append(" set @roleid=" + Entity.Id.ToString() + ";");
                    Builder.AppendLine(" SET @ID=CONVERT(VARCHAR(20),@roleid); ");
                    Builder.AppendLine(" exec dbo.p_uploaddata_isSync  @buscode,@stocode,'TB_Roles','id',@ID,'update'; ");

                    #endregion
                }

                #region 权限
                Builder.Append(" delete TB_RoleFunction where roleid=@roleid;");
                if (FunList != null && FunList.Length > 0)
                {
                    for (int i = 0; i < FunList.Length; i++)
                    {
                        Builder.Append(string.Format(" insert into TB_RoleFunction (buscode,stocode,ccode,ccname,ctime,roleid,functionid) values ('{0}','{1}','{2}','{3}','{4}',@roleid,'{5}') ;", Entity.BusCode, Entity.StoCode,Entity.CCode,Entity.CCname,Entity.CTime,FunList[i].ToString()));
                        Builder.AppendLine(" SET @ID=CONVERT(VARCHAR(20),SCOPE_IDENTITY()); ");
                        Builder.AppendLine(" exec dbo.p_uploaddata_isSync  @buscode,@stocode,'TB_RoleFunctio','id',@ID,'add'; ");
                    }
                }
                #endregion

                Builder.Append(" if(@@error=0) begin commit tran tan1 end else begin rollback tran tran1 end");
                return DBHelper.ExecuteNonQuery(Builder.ToString());
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_RolesEntity Entity,string[] FunList)
        {
            return Add(ref Entity, FunList);
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Roles_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
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
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_Roles_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 获取PKCode
        /// </summary>
        /// <returns></returns>
        public string GetPKCode()
        {
            string pkCode = string.Empty;
            SqlParameter[] sqlParameters =
             {
                 new SqlParameter("@basecode",SqlDbType.VarChar ,32,pkCode)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("[dbo].[p_GetbaseCode]", CommandType.StoredProcedure, sqlParameters);
            pkCode = sqlParameters[0].Value.ToString();
            return pkCode;
        }

    }
}