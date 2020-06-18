
﻿using CommunityBuy.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Text;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 折扣方案数据访问类
    /// </summary>
    public partial class dalTB_DiscountScheme
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_DiscountSchemeEntity Entity)
        {
            DataSet dtStructure = DBHelper.ExecuteDataSet("select top 0 * from TB_DiscountScheme");
            StringBuilder sql = new StringBuilder();
            sql.Append(" declare @returnval int;");
            sql.Append(" set @returnval=0;");
            sql.Append("declare @PKCode varchar(32)");
            sql.Append("exec [dbo].[p_GetbaseCode] @PKCode output");
            sql.Append(" if exists(select 1 from TB_DiscountScheme where BusCode='" + Entity.BusCode + "' and StoCode='" + Entity.StoCode + "' and SchName='" + Entity.SchName + "') begin set @returnval=1; end ");
            if(!string.IsNullOrEmpty(Entity.InsideCode))
            {
                sql.Append(" else if exists(select 1 from TB_DiscountScheme where BusCode='" + Entity.BusCode + "' and StoCode='" + Entity.StoCode + "' and InsideCode='" + Entity.InsideCode + "') begin set @returnval=1; end ");
            }
            sql.Append(" else begin BEGIN TRAN tan1");

            Entity.CTime = System.DateTime.Now;
            Entity.UTime = System.DateTime.Now;
            List<string> lstExcludeFilds = new List<string>();
            Dictionary<string, string> dicAttachFilds = new Dictionary<string, string>();
            lstExcludeFilds.Clear();
            lstExcludeFilds.Add("Id");
            lstExcludeFilds.Add("PKCode");
            lstExcludeFilds.Add("CTime");
            lstExcludeFilds.Add("UTime");
            dicAttachFilds.Clear();
            dicAttachFilds.Add("PKCode", "@PKCode");
            dicAttachFilds.Add("CTime", "getdate()");
            dicAttachFilds.Add("UTime", "getdate()");

            string InsertSql = EntityHelper.GenerateSqlByDE<TB_DiscountSchemeEntity>(dtStructure.Tables[0], Entity, lstExcludeFilds, dicAttachFilds, EntityHelper.eSqlType.insert);
            sql.AppendFormat(" INSERT INTO TB_DiscountScheme " + InsertSql + " ;");
            sql.AppendLine(" exec dbo.p_uploaddata_isSync '" + Entity.BusCode + "','" + Entity.StoCode + "','TB_DiscountScheme','PKCode',@PKCode,'add'");
            #region 特殊折扣、
            foreach(TR_DiscountSchemeRateEntity rate in Entity.DSRateList)
            {
                sql.Append("insert into [dbo].[TR_DiscountSchemeRate]([BusCode],[StoCode],[CCode],[CCname],[CTime],[TStatus],[Sort],[SchCode],[DiscountType],[DisTypeCode],[DisCode],[DisMetCode],[DiscountRate]) values('" + Entity.BusCode + "','" + Entity.StoCode + "','" + Entity.CCode
                    + "','" + Entity.CCname + "','" + Entity.CTime + "','" + rate.TStatus + "','" + rate.Sort + "',@PKCode,'" + rate.DiscountType + "','" + rate.DisTypeCode + "','" + rate.DisCode + "','" + rate.DiscountRate + "');");
            }
            sql.AppendLine(" if(@@error=0) begin set @returnval=0; commit tran tan1; end else begin rollback tran tran1;set @returnval=1; end");
            sql.AppendLine(" end");
            sql.AppendLine(" select @returnval;");
            DataTable dt = DBHelper.ExecuteDataTable(sql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return StringHelper.StringToInt(dt.Rows[0][0].ToString());
            }
            else
            {
                return 2;
            }
            #endregion

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_DiscountSchemeEntity Entity)
        {
            DataSet dtStructure = DBHelper.ExecuteDataSet("select top 0 * from TB_DiscountScheme");
            StringBuilder sql = new StringBuilder();
            sql.Append(" declare @returnval int;");
            sql.Append(" set @returnval=0;");
            sql.Append("declare @PKCode varchar(32)");
            sql.Append(" set @PKCode='" + Entity.PKCode + "';");
            sql.Append(" if not exists(select 1 from TB_DiscountScheme where BusCode='" + Entity.BusCode + "' and StoCode='" + Entity.StoCode + "' and PKCode=@PKCode) begin set @returnval=2; end ");
            sql.Append(" else if exists(select 1 from TB_DiscountScheme where BusCode='" + Entity.BusCode + "' and StoCode='" + Entity.StoCode + "' and PKCode<>@PKCode and SchName='" + Entity.SchName + "') begin set @returnval=1; end ");
            if (!string.IsNullOrEmpty(Entity.InsideCode))
            {
                sql.Append(" else if exists(select 1 from TB_DiscountScheme where BusCode='" + Entity.BusCode + "' and StoCode='" + Entity.StoCode + "' and InsideCode='" + Entity.InsideCode + "' and PKCode<>@PKCode) begin set @returnval=1; end ");
            }
            sql.Append(" else begin BEGIN TRAN tan1");

            Entity.CTime = System.DateTime.Now;
            Entity.UTime = System.DateTime.Now;
            List<string> lstExcludeFilds = new List<string>();
            Dictionary<string, string> dicAttachFilds = new Dictionary<string, string>();
            lstExcludeFilds.Clear();
            lstExcludeFilds.Add("Id");
            lstExcludeFilds.Add("PKCode");
            lstExcludeFilds.Add("CTime");
            lstExcludeFilds.Add("UTime");
            dicAttachFilds.Clear();
            dicAttachFilds.Add("CTime", "getdate()");
            dicAttachFilds.Add("UTime", "getdate()");

            string InsertSql = EntityHelper.GenerateSqlByDE<TB_DiscountSchemeEntity>(dtStructure.Tables[0], Entity, lstExcludeFilds, dicAttachFilds, EntityHelper.eSqlType.update);
            sql.AppendFormat(" update TB_DiscountScheme set " + InsertSql + " where BusCode='"+ Entity.BusCode + "' and PKCode=@PKCode;");
            sql.AppendLine(" exec dbo.p_uploaddata_isSync '" + Entity.BusCode + "','" + Entity.StoCode + "','TB_DiscountScheme','PKCode',@PKCode,'update'");
            #region 特殊折扣、
            sql.Append("delete [dbo].[TR_DiscountSchemeRate] where buscode='" + Entity.BusCode + "' and SchCode=@PKCode");
            foreach (TR_DiscountSchemeRateEntity rate in Entity.DSRateList)
            {
                sql.Append(" insert into [dbo].[TR_DiscountSchemeRate]([BusCode],[StoCode],[CCode],[CCname],[CTime],[TStatus],[Sort],[SchCode],[DiscountType],[DisTypeCode],[DisCode],[DisMetCode],[DiscountRate]) values('" + Entity.BusCode + "','" + Entity.StoCode + "','" + Entity.CCode
                    + "','" + Entity.CCname + "',getdate(),'" + Entity.TStatus + "','" + Entity.Sort + "',@PKCode,'" + rate.DiscountType + "','" + rate.DisTypeCode + "','" + rate.DisCode + "','" + rate.DiscountRate + "');");
            }
            sql.AppendLine(" if(@@error=0) begin set @returnval=0; commit tran tan1; end else begin rollback tran tran1;set @returnval=1; end");
            sql.AppendLine(" end");
            sql.AppendLine(" select @returnval;");
            DataTable dt = DBHelper.ExecuteDataTable(sql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return StringHelper.StringToInt(dt.Rows[0][0].ToString());
            }
            else
            {
                return 2;
            }
            #endregion
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="PKCode">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_DiscountScheme_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string PKCode, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@PKCode", PKCode),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };
			sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_DiscountScheme_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        public DataTable GetDiscountSchRate(string PKCode,string StoCode)
        {
            string sql = "select r.*,(case r.TStatus when '0' then '无效' else '有效' end) TStatusName,(case r.DiscountType when '1' then '菜品类别' when '2' then '菜品' else '做法' end) DiscountTypeName,dis.DisName,[dbo].[fn_GetStoDisTypeParentName](dt.PKKCode,r.StoCode)+'-'+dt.TypeName as DisTypeCodeName,dm.MetName as DisMetCode from TR_DiscountSchemeRate r left join (select * from TB_Dish where stocode='" + StoCode+ "') dis on r.DisCode=dis.DisCode left join (select * from TB_DishType where stocode='"+StoCode+ "') dt on r.DisTypeCode=dt.PKCode left join (select * from TB_DishMethods where stocode='"+StoCode+"') dm on dm.PKCode=r.DisMetCode where r.SchCode='" + PKCode + "' and r.StoCode='"+StoCode+"'";
            return DBHelper.ExecuteDataTable(sql);
        }

    }
}