using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 系统功能管理数据访问类
    /// </summary>
    public partial class dalTB_Functions
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_FunctionsEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@FType", Entity.FType),
				new SqlParameter("@ParentId", Entity.ParentId),
				new SqlParameter("@Code", Entity.Code),
				new SqlParameter("@Cname", Entity.Cname),
				new SqlParameter("@BtnCode", Entity.BtnCode),
				new SqlParameter("@Orders", Entity.Orders),
				new SqlParameter("@ImgName", Entity.ImgName),
				new SqlParameter("@Url", Entity.Url),
				new SqlParameter("@Level", Entity.Level),
				new SqlParameter("@Descr", Entity.Descr),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_Functions_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.Id = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_FunctionsEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@FType", Entity.FType),
				new SqlParameter("@ParentId", Entity.ParentId),
				new SqlParameter("@Code", Entity.Code),
				new SqlParameter("@Cname", Entity.Cname),
				new SqlParameter("@BtnCode", Entity.BtnCode),
				new SqlParameter("@Orders", Entity.Orders),
				new SqlParameter("@ImgName", Entity.ImgName),
				new SqlParameter("@Url", Entity.Url),
				new SqlParameter("@Level", Entity.Level),
				new SqlParameter("@Descr", Entity.Descr),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Functions_Update", CommandType.StoredProcedure, sqlParameters); 
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
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Functions_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
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
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_Functions_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 获取二级三级权限（根据一级）
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public DataTable GetFunctionListByParentId(string GUID, string UID, string parentid, string stocode)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@userid", UID),
                 new SqlParameter("@parentid",parentid),
                 new SqlParameter("@stocode",stocode)
             };

            return DBHelper.ExecuteDataTable("dbo.p_getfunctionlistbyparentidandstocode", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 获取商品管理下的功能权限
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public DataTable GetFunctionListByParentIdSP(string GUID, string UID)
        {
            return DBHelper.ExecuteDataTable("  SELECT cname,url,imgname,id FROM dbo.TB_Functions WHERE id in (select FunctionId from TB_RoleFunction where roleid in (SELECT roleid FROM dbo.TB_UserRole WHERE userid = '"+UID+"') and FunctionId in('2002002', '2002003', '2002004', '2002005', '2002008', '2002011', '2002006', '2002007'))");
        }

        /// <summary>
        /// 获取商品管理下的功能权限
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public DataTable GetFunctionListByParentIdSPAdmin(string GUID, string UID)
        {
            return DBHelper.ExecuteDataTable("  SELECT cname,url,imgname,id FROM dbo.TB_Functions WHERE id in ('2002002', '2002003', '2002004', '2002005', '2002008', '2002011', '2002006', '2002007')");
        }

        /// <summary>
        /// 获取当前登录用户的一级权限
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="stocode"></param>
        /// <returns></returns>
        public DataTable GetFirFunction(string GUID, string UID, string stocode)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@userid", UID),
                 new SqlParameter("@stocode",stocode)
             };

            return DBHelper.ExecuteDataTable("dbo.p_getfirfunctionbystocode", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 获取当前登录用户的一级权限
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="stocode"></param>
        /// <returns></returns>
        public DataTable GetUserFuncionList(string GUID, string UID, string stocode)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@userid", UID),
                 new SqlParameter("@stocode",stocode)
             };

            return DBHelper.ExecuteDataTable("dbo.p_getUserFunctionList", CommandType.StoredProcedure, sqlParameters);
        }

    }
}