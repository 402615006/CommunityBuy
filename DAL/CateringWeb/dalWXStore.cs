using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 商家门店信息数据访问类
    /// </summary>
    public partial class dalWXStore
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess("CateringDBConnectionString");
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref WXStoreEntity Entity)
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
				new SqlParameter("@descr", Entity.descr),
				new SqlParameter("@stourl", Entity.stourl),
				new SqlParameter("@stocoordx", Entity.stocoordx),
				new SqlParameter("@stocoordy", Entity.stocoordy),
				new SqlParameter("@netlinklasttime", Entity.netlinklasttime),
				new SqlParameter("@calcutime", Entity.calcutime),
				new SqlParameter("@busHour", Entity.busHour),
				new SqlParameter("@recommended", Entity.recommended),
				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@cuser", Entity.cuser),
				new SqlParameter("@uuser", Entity.uuser),
				new SqlParameter("@btime", Entity.btime),
				new SqlParameter("@etime", Entity.etime),
				new SqlParameter("@TerminalNumber", Entity.TerminalNumber),
				new SqlParameter("@ValuesDate", Entity.ValuesDate),
				new SqlParameter("@isfood", Entity.isfood),
				new SqlParameter("@pstocode", Entity.pstocode),
				new SqlParameter("@sqcode", Entity.sqcode),
				new SqlParameter("@storetype", Entity.storetype),
				new SqlParameter("@jprice", Entity.jprice),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_Store_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.stoid = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(WXStoreEntity Entity)
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
				new SqlParameter("@descr", Entity.descr),
				new SqlParameter("@stourl", Entity.stourl),
				new SqlParameter("@stocoordx", Entity.stocoordx),
				new SqlParameter("@stocoordy", Entity.stocoordy),
				new SqlParameter("@netlinklasttime", Entity.netlinklasttime),
				new SqlParameter("@calcutime", Entity.calcutime),
				new SqlParameter("@busHour", Entity.busHour),
				new SqlParameter("@recommended", Entity.recommended),
				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@cuser", Entity.cuser),
				new SqlParameter("@uuser", Entity.uuser),
				new SqlParameter("@btime", Entity.btime),
				new SqlParameter("@etime", Entity.etime),
				new SqlParameter("@TerminalNumber", Entity.TerminalNumber),
				new SqlParameter("@ValuesDate", Entity.ValuesDate),
				new SqlParameter("@isfood", Entity.isfood),
				new SqlParameter("@pstocode", Entity.pstocode),
				new SqlParameter("@sqcode", Entity.sqcode),
				new SqlParameter("@storetype", Entity.storetype),
				new SqlParameter("@jprice", Entity.jprice),
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
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };
			sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_Store_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        public DataTable GetTopProduct(string StoCode)
        {
            string sql = "select top 10 p.DisCode,p.PKName as DisName,p.CostPrice as Price,p.ProImgPath as ImgUrl,0 as num,dis.[IsCount],dis.[DefCount],dis.[CountPrice],dis.[IsVarPrice],dis.[IsWeight],dis.[IsMethod],dis.[IsStock] ,dis.[IsPoint],dis.[IsMemPrice],dis.[IsCoupon],dis.[IsKeep],dis.[IsCombo] from TB_TopProduct p inner join TB_Dish dis on p.discode=dis.discode where p.stocode='" + StoCode+ "' and dis.stocode='" + StoCode + "' and p.TStatus='1' order by p.ProSort desc";
            return DBHelper.ExecuteDataTable(sql);
        }

    }
}