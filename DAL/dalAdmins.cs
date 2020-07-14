using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using System.Text;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 后台用户信息数据访问类
    /// </summary>
    public partial class dalAdmins
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref AdminsEntity Entity, string role)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("IF NOT EXISTS(Select 1 from[dbo].[admins] Where uname = '"+Entity.uname+"')");
            sql.Append(" BEGIN");
            sql.Append(" BEGIN TRAN tran1"); 
            sql.Append(" declare @userid bigint;");
            sql.Append(" set @userid = 0;");
            sql.Append(" declare @result bigint;");
            sql.Append(" set @result = 0;");
            sql.Append(" declare @empid varchar(8);");
            sql.Append(" set @empid = '';");
            sql.Append(" exec[dbo].[p_GetEmpID] @empid output;");
            sql.Append(" declare @stocode varchar(8);");
            sql.Append(" set @stocode = '';");
            sql.Append(" declare @realname varchar(64);");
            sql.Append(" set @realname = '';");
            sql.Append(" declare @mescode varchar(64);");
            sql.Append(" set @mescode = '';");
            sql.Append(" declare @PY varchar(64);select top 1 @realname=cname,@PY=dbo.fn_GetPy(cname) from Employee where ecode=@empcode;");
            sql.Append(" INSERT INTO[admins]([buscode],[strcode],[uname],[upwd],[realname],[umobile],[empcode],[remark],[status],[cuser],[ctime],[uuser],[utime], isdelete, scope, stocode, empid,msigmoney,PY)");
            sql.Append(" VALUES('" + Entity.buscode + "', '', '" + Entity.uname + "', '" + Entity.upwd + "',@realname, '" + Entity.umobile + "', @empcode, '" + Entity.remark + "', '" + Entity.status + "', getdate(), null, NULL, null, @PY);");
            sql.Append(" SET @userid = SCOPE_IDENTITY();");
            sql.Append(" insert into userrole([userid], roleid, ctime) select @userid, col, getdate() from[dbo].[fn_StringSplit]('"+ role + "', ',') where len(col)> 0;");
            sql.Append(" IF(@@ERROR = 0)");
            sql.Append(" BEGIN");
            sql.Append(" COMMIT TRAN tran1;");
            sql.Append(" set @result=0;");
            sql.Append(" END");
            sql.Append(" ELSE");
            sql.Append(" BEGIN");
            sql.Append(" ROLLBACK TRAN tran1;");
            sql.Append(" set @result=-1;");
            sql.Append(" END");
            sql.Append(" END");
            sql.Append(" ELSE");
            sql.Append(" BEGIN");
            sql.Append(" set @result= 1;");
            sql.Append(" select @result;");
            sql.Append("END");
            intReturn = 0;
            intReturn = DBHelper.ExecuteNonQuery(sql.ToString());
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(AdminsEntity Entity, string role)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" BEGIN");
            sql.Append(" BEGIN TRAN tran1");
            sql.Append(" declare @userid bigint;");
            sql.Append(" set @userid = "+Entity.userid+";");
            sql.Append(" declare @result bigint;");
            sql.Append(" set @result = 0;");
            sql.Append(" declare @stocode varchar(8);");
            sql.Append(" set @stocode = '';");
            sql.Append(" declare @realname varchar(64);");
            sql.Append(" set @realname = '';");
            sql.Append(" declare @mescode varchar(64);");
            sql.Append(" set @mescode = '';");
            sql.Append(" declare @PY varchar(64);select top 1 @realname=cname,@PY=dbo.fn_GetPy(cname) from Employee where ecode=@empcode;");
            sql.Append(" UPDATE [dbo].[admins] SET[uname] =  '" + Entity.uname + "',[realname]=@realname,[umobile]='" + Entity.umobile + "',[remark]= '" + Entity.remark + "',[status]='" + Entity.status + "',[utime]=GETDATE(),PY=@PY WHERE userid=" + Entity.userid+";");
            sql.Append(" DELETE FROM  userrole where userid=" + Entity.userid + "; ");
            sql.Append(" insert into userrole([userid], roleid, ctime) select @userid, col, getdate() from[dbo].[fn_StringSplit]('" + role + "', ',') where len(col)> 0;");
            sql.Append(" IF(@@ERROR = 0)");
            sql.Append(" BEGIN");
            sql.Append(" COMMIT TRAN tran1;");
            sql.Append(" set @result=0;");
            sql.Append(" END");
            sql.Append(" ELSE");
            sql.Append(" BEGIN");
            sql.Append(" ROLLBACK TRAN tran1;");
            sql.Append(" set @result=-1;");
            sql.Append(" END");
            sql.Append(" END");
            sql.Append(" select @result;");
            intReturn = 0;
            intReturn = DBHelper.ExecuteNonQuery(sql.ToString());
            return intReturn;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@ids", ids),
                new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_Admins_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int ResetPwd(string id, string Pwd)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@ids", id),
                new SqlParameter("@Pwd", Pwd)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_Admins_ResetPwd", CommandType.StoredProcedure, sqlParameters);
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
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,64,mescode)
             };
            sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_Admins_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        public int GetAdminsCheck(string id, string mobile, string username)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@id", id),
                 new SqlParameter("@mobile", mobile),
                 new SqlParameter("@username", username)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_Admins_check", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldpwd"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        public int EditPwd(string id, string oldpwd, string NewPwd)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@oldpwd", oldpwd),
                new SqlParameter("@newpwd", NewPwd)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_Admins_EditPwd", CommandType.StoredProcedure, sqlParameters);
        }


    }
}