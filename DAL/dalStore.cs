using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 商家门店信息数据访问类
    /// </summary>
    public partial class dalStore
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        //public int Add(ref StoreEntity Entity, string storetype, string jprice, string paytype, string sqcode, string jcaddress, string isjc, string jctype, string xftime, string sumcode, string mccode, string idtype)
        public int Add(ref StoreEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@stoid", Entity.stoid),
                new SqlParameter("@comcode", Entity.comcode),
                new SqlParameter("@buscode", Entity.buscode),
                new SqlParameter("@stocode", Entity.stocode),
                new SqlParameter("@cname", Entity.cname),
                new SqlParameter("@sname", Entity.sname),
                new SqlParameter("@bcode", Entity.bcode),
                new SqlParameter("@indcode", Entity.indcode),
                new SqlParameter("@provinceid", Entity.provinceid),
                new SqlParameter("@cityid", Entity.cityid),
                new SqlParameter("@areaid", Entity.areaid),
                new SqlParameter("@address", Entity.address),
                new SqlParameter("@stoprincipal", Entity.stoprincipal),
                new SqlParameter("@stoprincipaltel", Entity.stoprincipaltel),
                new SqlParameter("@tel", Entity.tel),
                new SqlParameter("@stoemail", Entity.stoemail),
                new SqlParameter("@logo", Entity.logo),
                new SqlParameter("@backgroundimg", Entity.backgroundimg),
                new SqlParameter("@stopath", Entity.stopath),
                new SqlParameter("@services", Entity.services),
                new SqlParameter("@descr", Entity.descr),
                new SqlParameter("@stourl", Entity.stourl),
                new SqlParameter("@stocoordx", Entity.stocoordx),
                new SqlParameter("@stocoordy", Entity.stocoordy),
                new SqlParameter("@recommended", Entity.recommended),
                new SqlParameter("@remark", Entity.remark),
                new SqlParameter("@status", Entity.status),
                new SqlParameter("@btime", Entity.btime),
                new SqlParameter("@etime", Entity.etime),
                new SqlParameter("@jprice", Entity.jprice),
                new SqlParameter("@sqcode", Entity.sqcode)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_Store_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 1)
            {
                Entity.stoid = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public int Update(StoreEntity Entity, string storetype, string jprice, string paytype, string sqcode, string jcaddress, string isjc, string jctype, string xftime, string sumcode, string mccode, string idtype)
        public int Update(StoreEntity Entity)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@stoid", Entity.stoid),
                new SqlParameter("@comcode", Entity.comcode),
                new SqlParameter("@buscode", Entity.buscode),
                new SqlParameter("@stocode", Entity.stocode),
                new SqlParameter("@cname", Entity.cname),
                new SqlParameter("@sname", Entity.sname),
                new SqlParameter("@bcode", Entity.bcode),
                new SqlParameter("@indcode", Entity.indcode),
                new SqlParameter("@provinceid", Entity.provinceid),
                new SqlParameter("@cityid", Entity.cityid),
                new SqlParameter("@areaid", Entity.areaid),
                new SqlParameter("@address", Entity.address),
                new SqlParameter("@stoprincipal", Entity.stoprincipal),
                new SqlParameter("@stoprincipaltel", Entity.stoprincipaltel),
                new SqlParameter("@tel", Entity.tel),
                new SqlParameter("@stoemail", Entity.stoemail),
                new SqlParameter("@logo", Entity.logo),
                new SqlParameter("@backgroundimg", Entity.backgroundimg),
                new SqlParameter("@stopath", Entity.stopath),
                new SqlParameter("@services", Entity.services),
                new SqlParameter("@descr", Entity.descr),
                new SqlParameter("@stourl", Entity.stourl),
                new SqlParameter("@stocoordx", Entity.stocoordx),
                new SqlParameter("@stocoordy", Entity.stocoordy),
                new SqlParameter("@recommended", Entity.recommended),
                new SqlParameter("@remark", Entity.remark),
                new SqlParameter("@status", Entity.status),
                new SqlParameter("@btime", Entity.btime),
                new SqlParameter("@etime", Entity.etime),
                new SqlParameter("@sqcode", Entity.sqcode)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_Store_Update", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="stoid">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@ids", ids),
                new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_Store_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string stoid, ref string mescode)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@stoid", stoid),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,64,mescode)
             };
            sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_Store_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 找店
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="citycode"></param>
        /// <param name="shopcircle"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public DataTable FindStore(string pagesize, string currentpage, string citycode, string shopcircle, string keywords, ref string sumcount)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@citycode",citycode),
                new SqlParameter("@shopcircle",shopcircle),
                new SqlParameter("@keywords",keywords),
                new SqlParameter("@pagesize",pagesize),
                new SqlParameter("@currentpage",currentpage),
                new SqlParameter("@sumcount",SqlDbType.VarChar ,64,sumcount)
            };
            sqlParameters[5].Direction = ParameterDirection.Output;

            DataTable dt = DBHelper.ExecuteDataTable("p_FindStore", CommandType.StoredProcedure, sqlParameters);
            sumcount = sqlParameters[5].Value.ToString();
            return dt;
        }

        /// <summary>
        /// 门店详情
        /// </summary>
        /// <param name="stocode"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public DataSet GetStoDetail(string stocode, string openid)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@stocode",stocode),
                new SqlParameter("@openid",openid)
            };

            return DBHelper.ExecuteDataSet("p_GetStoreDetail", CommandType.StoredProcedure, sqlParameters);
        }

        //根据员工编号获取门店信息集合
        public DataTable GetStoInfoByEmpcode(string empcode, string searchval)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@empcode",empcode),
                new SqlParameter("@searchval",searchval)
            };

            return DBHelper.ExecuteDataTable("p_getstoinfobyempcode", CommandType.StoredProcedure, sqlParameters);
        }

        //根据userid获取门店信息集合
        public DataTable GetStoInfoByEmpcodeTrue(string userid, string searchval)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@userid",userid),
                new SqlParameter("@searchval",searchval)
            };

            return DBHelper.ExecuteDataTable("p_getstoinfobyempcode_true", CommandType.StoredProcedure, sqlParameters);
        }
    }
}