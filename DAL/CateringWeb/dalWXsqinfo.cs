using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 数据访问类
    /// </summary>
    public partial class dalWXsqinfo
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        MSSqlDataAccess MovieDBHelper = new MSSqlDataAccess("DBConnectionStringMovie");
        MSSqlDataAccess LSDBHelper = new MSSqlDataAccess("CateringDBConnectionString");
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref WXsqinfoEntity Entity)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@id", Entity.id),
                new SqlParameter("@sqcode", Entity.sqcode),
                new SqlParameter("@sqname", Entity.sqname),
                new SqlParameter("@city", Entity.city),
                new SqlParameter("@jwcodes", Entity.jwcodes),
                new SqlParameter("@cuser", Entity.cuser),
                new SqlParameter("@status", Entity.status),
                new SqlParameter("@uuser", Entity.uuser),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = LSDBHelper.ExecuteNonQuery("dbo.p_sqinfo_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.id = int.Parse(sqlParameters[0].Value.ToString());
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(WXsqinfoEntity Entity)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@id", Entity.id),
                new SqlParameter("@sqcode", Entity.sqcode),
                new SqlParameter("@sqname", Entity.sqname),
                new SqlParameter("@city", Entity.city),
                new SqlParameter("@jwcodes", Entity.jwcodes),
                new SqlParameter("@cuser", Entity.cuser),
                new SqlParameter("@status", Entity.status),
                new SqlParameter("@uuser", Entity.uuser),
             };
            return LSDBHelper.ExecuteNonQuery("dbo.p_sqinfo_Update", CommandType.StoredProcedure, sqlParameters);
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
            return LSDBHelper.ExecuteNonQuery("dbo.p_sqinfo_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string id, ref string mescode)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@id", id),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };
            sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = LSDBHelper.ExecuteNonQuery("dbo.p_sqinfo_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 获取商圈信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetGPSList()
        {
            string sql = "select sqcode,sqname,jwcodes,'' as jcode,'' as wcode from sqinfo where [status]='1'";
            return LSDBHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 获取商圈信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetNewGPSList(string typecode)
        {
            string sql = "select sqcode,sqname,jwcodes,'' as jcode,'' as wcode from sqinfo where [status]='1' and sqcode in( select gx.sqcode from Store s inner join storegx gx on s.stocode=gx.stocode left join ts_Dicts dic on gx.firtype=dic.diccode where gx.firtype='"+typecode+"'  and isnull(gx.sqcode,'')<>'' and dic.pdicid in (select dicid from ts_Dicts where diccode='HYTypeFir'))";
            return LSDBHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 分模块获取首页轮播图
        /// </summary>
        /// <param name="modelcode"></param>
        /// <returns></returns>
        public DataTable GetUnImage(string modelcode)
        {
            string sql = "select smallimg,isinbuy as isButton,jumptype as hreftype  from mv_latestactivitys where CHARINDEX('" + modelcode + "',isinposition,0)>0 and isinhome='1' and status='1' and bdate<=getdate() and edate>=getdate() order by [index] desc";
            return MovieDBHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 门店预定设置
        /// </summary>
        /// <returns></returns>
        public DataTable GetPresetSettings()
        {
            string sql = "select * from TM_PresetSettings where TStatus='1' and PresetType='1'";
            return DBHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 门店是否启用排队
        /// </summary>
        /// <returns></returns>
        public DataTable GetQueuingSettings()
        {
            string sql = "select * from TM_SystemSettings where KeyName='IsLineUp' and TStatus='1'";
            return DBHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 获取指定门店的排队人数段
        /// </summary>
        /// <param name="StoCode"></param>
        /// <returns></returns>
        public DataTable GetQueuing(string StoCode)
        {
            string sql = "select Convert(varchar(10),MinNumber)+'-'+Convert(varchar(10),MaxNumber)+'人',MinNumber,MaxNumber from TB_Queuing where stocode='" + StoCode + "' and TStatus='1'  order by MinNumber";
            return DBHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 获取指定门店的预定备注
        /// </summary>
        /// <param name="StoCode"></param>
        /// <returns></returns>
        public DataTable GetReservationRemark(string StoCode)
        {
            string sql = "select Remark from TB_CommonRemarks where stocode='" + StoCode + "' and RType='1'";
            return DBHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 获取用户的排队信息
        /// </summary>
        /// <param name="StoCode"></param>
        /// <param name="WxId"></param>
        /// <returns></returns>
        public DataTable GetQueue(string StoCode, string WxId)
        {
            string sql = "select BusCode,StoCode,PKCode,isnull(Wtime,0) as Wtime,dbo.fn_GetQueueNumber(PKCode,BusCode,StoCode,CusNum) as tablenumber,isnull(dbo.[fn_GetQueueCusNum](BusCode,StoCode,CusNum),'') as tabletype,CTime,dbo.[fn_GetQueueDQPKCode](PKCode,BusCode,StoCode,CusNum,CTime) as dqpkcode from TB_Queue where stocode='" + StoCode + "' and WxId='"+WxId+"' and TStatus='1'";
            return DBHelper.ExecuteDataTable(sql);
        }

    }
}