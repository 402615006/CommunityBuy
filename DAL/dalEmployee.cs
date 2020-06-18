using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 员工信息
    /// </summary>
    public partial class dalEmployee
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess("CateringDBConnectionString");

        /// <summary>
        /// 根据商户编号获取所有员工,缓存用,仅缓存stocode,dcode,ecode,cname
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetBusCodeToAllEmp(string BusCode)
        {
            string where = string.Empty;
            if(!string.IsNullOrEmpty(BusCode))
            {
                where = " where buscode='" + BusCode + "'";
            }
            string sql = "select strcode as stocode,dcode,ecode,cname from [dbo].[Employee] "+where;
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 获取提成人
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetCusManager(string filter,string order)
        {
            string sql = "select realname,empcode,PY from admins where "+filter+" order by "+order;
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 获取所有用户全部信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllEmp()
        {
            string sql = "select t.*,e.mob,rolename=dbo.f_GetRoleName(t.userid),roleid=dbo.f_GetRoleID(t.userid),empcodename=dbo.fnGetEmployeeCname(t.empcode),empmob=[dbo].[fnGetEmployeeMobile] (t.empcode),storename=dbo.fnGetMuStoreName1(t.userid, t.scope, t.stocode),empstoname=dbo.fnGetEmployeeStoreName(t.empcode),e.strcode as empstocode,rolecode=dbo.GetRoleStocodeByUserName(t.userid) from  Admins t inner join[dbo].[Employee] e on e.ecode=t.empcode";
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        public DataTable GetPageEmp(string tableName, string primarykey, string fields, int pageSize, int currentpage, string filter, string group, string order, out int recnums, out int pagenums)
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
                Dt = DBHelper.ExecuteDataTable("dbo.pPagingLarge", CommandType.StoredProcedure, sqlParameters);
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

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetAllAdmin()
        {
            string sql = "select * from admins";
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

    }
}
