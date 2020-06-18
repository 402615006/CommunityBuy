using System.Data;
using System.Data.SqlClient;
using System.Text;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 用户角色关系表数据访问类
    /// </summary>
    public partial class dalTB_UserRole
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_UserRoleEntity Entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" declare @returnval int;");
            sql.Append(" set @returnval=0;");
            sql.Append("declare @BusCode varchar(16);");
            sql.Append("declare @StoCode varchar(8);");
            sql.Append("declare @RoleId varchar(1024);");
            sql.Append("declare @UserId bigint;");
            sql.Append("declare @RealName nvarchar(32);");
            sql.Append("declare @EmpCode varchar(16);");

            sql.Append(" set @BusCode='" + Entity.BusCode + "';");
            sql.Append(" set @StoCode='" + Entity.StoCode + "';");
            sql.Append(" set @RoleId='" + Entity.StrRoleId + "';");
            sql.Append(" set @UserId=" + Entity.UserId + ";");
            sql.Append(" set @RealName='" + Entity.RealName + "';");
            sql.Append(" set @EmpCode='" + Entity.EmpCode + "';");

            sql.Append(" IF NOT EXISTS(Select 1 from [dbo].TB_UserRole Where userid=@UserId)");
            sql.Append(" BEGIN  ");
            sql.Append(" BEGIN TRANSACTION tran1");
            sql.Append(" exec dbo.p_uploaddata_isSync  @BusCode,@StoCode,'TB_UserRole','userid',@UserId,'add';");
            sql.Append(" insert into TB_UserRole ([buscode],[stocode],userid,roleid,realname,empcode,CTime)SELECT @BusCode,@StoCode,@UserId,col,@RealName,@EmpCode,getdate() FROM  [dbo].[fn_StringSplit](@RoleId,',');");
            if(!string.IsNullOrEmpty(Entity.RoleDisCount))
            {
                foreach (string code in Entity.RoleDisCount.Split(','))
                {
                    if(!string.IsNullOrEmpty(code))
                    {
                        sql.Append("insert into TB_UserDiscountScheme(BusCode,StoCode,CCode,CCname,CTime,DisCountCode,UserCode) values(@BusCode,@StoCode,'','',getdate(),'" + code + "',@UserId);");
                    }
                }
            }
            sql.Append(" IF(@@ERROR=0)  BEGIN COMMIT TRAN tran1;set @returnval=0;END ELSE BEGIN ROLLBACK TRAN tran1; set @returnval=1; END  ");
            sql.Append(" END");
            sql.Append(" else ");
            sql.Append(" BEGIN  ");
            sql.Append(" BEGIN TRANSACTION tran1");
            sql.Append(" exec dbo.p_uploaddata_isSync  @BusCode,@StoCode,'TB_UserRole','userid',@UserId,'update';");
            sql.Append(" DELETE FROM  TB_UserRole where userid=@UserId; delete TB_UserSigScheme where usercode=@UserId;");
            sql.Append(" insert into TB_UserRole ([buscode],[stocode],userid,roleid,realname,empcode,CTime)SELECT @BusCode,@StoCode,@UserId,col,@RealName,@EmpCode,getdate() FROM  [dbo].[fn_StringSplit](@RoleId,',');");
            sql.Append("delete TB_UserDiscountScheme where usercode=@UserId;");
            if (!string.IsNullOrEmpty(Entity.RoleDisCount))
            {
                foreach (string code in Entity.RoleDisCount.Split(','))
                {
                    sql.Append("insert into TB_UserDiscountScheme(BusCode,StoCode,CCode,CCname,CTime,DisCountCode,UserCode) values(@BusCode,@StoCode,'','',getdate(),'" + code + "',@UserId);");
                }
            }
            sql.Append(" IF(@@ERROR=0)  BEGIN COMMIT TRAN tran1;set @returnval=0;END ELSE BEGIN ROLLBACK TRAN tran1; set @returnval=1; END  ");
            sql.Append(" END ");
            sql.Append(" select @returnval;");
            DataTable dt = DBHelper.ExecuteDataTable(sql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {

                return StringHelper.StringToInt(dt.Rows[0][0].ToString());
            }
            else
            {
                return 2;
            }
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_UserRoleEntity Entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" declare @returnval int;");
            sql.Append(" set @returnval=0;");
            sql.Append("declare @BusCode varchar(16);");
            sql.Append("declare @StoCode varchar(8);");
            sql.Append("declare @RoleId varchar(1024);");
            sql.Append("declare @UserId bigint;");
            sql.Append("declare @RealName nvarchar(32);");
            sql.Append("declare @EmpCode varchar(16);");

            sql.Append(" set @BusCode='" + Entity.BusCode + "';");
            sql.Append(" set @StoCode='" + Entity.StoCode + "';");
            sql.Append(" set @RoleId='" + Entity.RoleId + "';");
            sql.Append(" set @UserId=" + Entity.UserId + ";");
            sql.Append(" set @RealName='" + Entity.RealName + "';");
            sql.Append(" set @EmpCode='" + Entity.EmpCode + "';");

            sql.Append(" IF NOT EXISTS(Select 1 from [dbo].TB_UserRole Where userid=@UserId)");
            sql.Append(" BEGIN  ");
            sql.Append(" BEGIN TRANSACTION tran1");
            sql.Append(" exec dbo.p_uploaddata_isSync  @BusCode,@StoCode,'TB_UserRole','userid',@UserId,'add';");
            sql.Append(" insert into TB_UserRole ([buscode],[stocode],userid,roleid,realname,empcode,CTime)SELECT @BusCode,@StoCode,@UserId,col,@RealName,@EmpCode,getdate() FROM  [dbo].[fn_StringSplit](@RoleId,',');");

            sql.Append(" IF(@@ERROR=0)  BEGIN COMMIT TRAN tran1;set @returnval=0;END ELSE BEGIN ROLLBACK TRAN tran1; set @returnval=1; END  ");
            sql.Append(" END");
            sql.Append(" else ");
            sql.Append(" BEGIN  ");
            sql.Append(" BEGIN TRANSACTION tran1");
            sql.Append(" exec dbo.p_uploaddata_isSync  @BusCode,@StoCode,'TB_UserRole','userid',@UserId,'update';");
            sql.Append(" DELETE FROM  TB_UserRole where userid=@UserId; delete TB_UserSigScheme where buscode=@buscode and stocode=@stocode and usercode=@UserId;");
            sql.Append(" insert into TB_UserRole ([buscode],[stocode],userid,roleid,realname,empcode,CTime)SELECT @BusCode,@StoCode,@UserId,col,@RealName,@EmpCode,getdate() FROM  [dbo].[fn_StringSplit](@RoleId,',');");

            sql.Append(" IF(@@ERROR=0)  BEGIN COMMIT TRAN tran1;set @returnval=0;END ELSE BEGIN ROLLBACK TRAN tran1; set @returnval=1; END  ");
            sql.Append(" END ");
            sql.Append(" select @returnval;");
            DataTable dt = DBHelper.ExecuteDataTable(sql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return StringHelper.StringToInt(dt.Rows[0][0].ToString());
            }
            else
            {
                return 2;
            }
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
            return DBHelper.ExecuteNonQuery("dbo.p_TB_UserRole_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
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
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_UserRole_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 登陆获取用户角色
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="TerminalType"></param>
        /// <returns></returns>
        public DataTable GetUserToRole(string UserId)
        {
            string sql = "SELECT  DISTINCT A.UserId,(select CONVERT(varchar(16),t2.Roleid)+',' from TB_UserRole t2 where CONVERT(varchar(16),t2.Roleid) = CONVERT(varchar(16),t2.Roleid) For XML PATH('')) roleids FROM TB_UserRole A INNER JOIN dbo.TB_Roles B ON A.roleid=B.id WHERE A.userid=" + UserId ;
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 登陆获取用户角色
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="TerminalType"></param>
        /// <returns></returns>
        public DataTable GetUserStoreRoleList(string UserId)
        {
            string sql =string.Format(" SELECT r.*,ur.StoCode as RoleStore from TB_Roles r left join TB_UserRole ur on r.id=ur.RoleId where ur.UserId={0} ", UserId);
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 绑定用户角色时使用
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataTable GetRoleListUser(string UserId, string filter)
        {
            string sql = "select RoleDescr,RoleName,r.Id,r.MaxDiffPrice,r.MaxPrefePrice,r.StoCode,r.RoleDisCount,'' as RoleDisCountName,isnull(ur.UserId,-1),r.StoCode,'' as StoName from [dbo].[TB_Roles] r inner join [dbo].[TB_UserRole] ur on ur.RoleId=r.id and ur.UserId=" + UserId + filter;
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 获取用户角色名称
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataTable GetUserRoleNameList(string BusCode)
        {
            string sql = "select ur.UserId,r.RoleName from [dbo].[TB_UserRole] ur inner join [dbo].[TB_Roles] r on ur.RoleId=r.Id ";
            if(!string.IsNullOrEmpty(BusCode))
            {
                sql += " where ur.buscode='" + BusCode + "'";
            }
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 获取用户签送方案
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataTable GetUserSigList(string filter)
        {
            string sql = "select uss.SigCode,uss.DayMoney,ss.SignatureName,0 as TotalMoney from TB_UserSigScheme uss inner join TM_SignatureScheme ss on uss.SigCode=ss.PKCode "+filter;
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataTable GetRoleListUser(string UserId)
        {
            string sql = "select RoleDescr,RoleName,r.Id,r.MaxDiffPrice,r.MaxPrefePrice,r.StoCode,r.RoleDisCount,'' as RoleDisCountName,r.StoCode,'' as StoName from [dbo].[TB_Roles] r inner join [dbo].[TB_UserRole] ur on ur.RoleId=r.id and ur.UserId=" + UserId;
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 获取指定门店下的指定角色的用户信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataTable GetRoleUserNameList(string stocode,string roletype)
        {
            string sql = "select ur.UserId,ur.RealName,r.RoleName,'' as ucname from [dbo].[TB_UserRole] ur inner join [dbo].[TB_Roles] r on ur.RoleId=r.Id where charindex('"+stocode+",',ur.stocode+',',0)>0 and r.RoleType='"+roletype+"'";
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

    }
}