using System.Data;
using System.Data.SqlClient;
using System.Text;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.DAL
{
    public class dalroles
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();

        /// <summary>
        /// 添加角色权限信息
        /// </summary>
        /// <param name="Entity">角色实体</param>
        /// <param name="FunList">功能数组</param>
        /// <returns></returns>
        public int AddRITMAS(rolesEntity Entity, string[] FunList)
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(" BEGIN TRAN tan1");
            Builder.Append(" declare @rolid int");
            int rel = CheckRoleName(Entity.roleid.ToString(), Entity.cname,Entity.buscode);
            if (rel <= 0)
            {
                if (Entity.roleid == 0)
                {
                    Builder.AppendFormat(" INSERT INTO dbo.roles(scope,stocode,cname,[status],descr,cuser,ctime,buscode)VALUES({0},'{1}','{2}','{3}','{4}','{5}',getdate(),'{6}');", Entity.scope, Entity.stocode, Entity.cname, Entity.status, Entity.descr, Entity.cuser,Entity.buscode);
                    Builder.Append(" set @rolid=SCOPE_IDENTITY();");
                }
                else
                {
                    Builder.Append(" UPDATE roles SET scope=" + Entity.scope + ",utime=getdate(),uuser=" + Entity.uuser);
                    Builder.Append(" ,stocode='" + Entity.stocode + "'");
                    if (Entity.cname.Length > 0)
                    {
                        Builder.Append(" ,cname='" + Entity.cname + "'");
                    }
                    if (Entity.status.Length > 0)
                    {
                        Builder.Append(" ,[status]='" + Entity.status + "'");
                    }
                    if (Entity.descr.Length > 0)
                    {
                        Builder.Append(" ,descr='" + Entity.descr + "'");
                    }
                    Builder.AppendFormat(" WHERE roleid={0};", Entity.roleid);
                    Builder.Append(" set @rolid=" + Entity.roleid + ";");
                }
                Builder.AppendFormat(" DELETE FROM rolefunction WHERE roleid={0}", Entity.roleid);

                try
                {
                    if (FunList != null && FunList.Length > 0)
                    {
                        foreach (string item in FunList)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                Builder.Append(" INSERT INTO rolefunction(roleid,funid)");
                                Builder.AppendFormat(" VALUES(@rolid,{0});", item);
                            }
                        }
                    }
                }
                catch
                { }
                Builder.Append(" if(@@error=0) begin commit tran tan1 end else begin rollback tran tran1 end");
                return DBHelper.ExecuteNonQuery(Builder.ToString());
            }
            else
            {
                return 3;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string ID, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@id", ID),
                 //new SqlParameter("@mescode",SqlDbType.NVarChar ,64,mescode)
             };
            //sqlParameters[1].Direction = ParameterDirection.Output;
            int intReturn = DBHelper.ExecuteNonQuery("dbo.p_roles_Delete", CommandType.StoredProcedure, sqlParameters);
            //mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        //删除角色
        public void DeleteRoles(string selected, ref string mes)
        {
            SqlParameter[] sqlParameters = 
            {
                new SqlParameter("@ids",selected),
                new SqlParameter("@mes",mes)
            };

            sqlParameters[1].Direction = ParameterDirection.Output;
            DBHelper.ExecuteNonQuery("p_deleteroles", CommandType.StoredProcedure, sqlParameters);
            mes = sqlParameters[1].Value.ToString();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string id, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@id", id),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_ROLMAS_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }
        /// <summary>
        /// 检测名称是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public int CheckRoleName(string id, string rolename,string buscode)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@id", id),
				new SqlParameter("@cname", rolename),
                new SqlParameter("@buscode", buscode)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_checkrolename", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public int Update(StoreEntity Entity, string storetype, string jprice, string paytype, string sqcode, string jcaddress, string isjc, string jctype, string xftime, string sumcode, string mccode, string idtype)
        public int Update(rolesEntity Entity,string funlist)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@roleid", Entity.roleid),
                new SqlParameter("@scope", Entity.buscode),
                new SqlParameter("@stocode", Entity.stocode),
                new SqlParameter("@cname", Entity.cname),
                new SqlParameter("@descr", Entity.descr),
                new SqlParameter("@status", Entity.status),
                new SqlParameter("@funs", funlist)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_roles_Update", CommandType.StoredProcedure, sqlParameters);
        }
    }
}
