using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 菜品信息数据访问类
    /// </summary>
    public partial class dalTB_Dish
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_DishEntity Entity)
        {
            DataSet dtStructure = DBHelper.ExecuteDataSet("select top 0 * from TB_Dish;select top 0 * from TR_DishesMethods;select top 0 * from TR_DisForCombo;select top 0 * from TR_DishImage;select top 0 * from TR_ComboInfo;");
            StringBuilder sql = new StringBuilder();
            sql.Append(" declare @ID int;");
            sql.Append(" declare @returnval int;");
            sql.Append(" set @returnval=0;");
            sql.Append("declare @DisCode varchar(32)");
            sql.Append("declare @DisMethodCode varchar(32)");
            sql.Append("exec [dbo].[p_GetbaseCode] @DisCode output");
            sql.Append(" if exists(select 1 from TB_Dish where BusCode='" + Entity.BusCode + "' and StoCode='" + Entity.StoCode + "' and DisName='" + Entity.DisName + "' and Unit='" + Entity.Unit + "' and StoCode='" + Entity.StoCode + "') begin set @returnval=1; end ");
            sql.Append(" else if exists(select 1 from TB_Dish where BusCode='" + Entity.BusCode + "' and StoCode='" + Entity.StoCode + "' and QRCode='" + Entity.QRCode + "' and '" + Entity.QRCode + "'<>'') begin set @returnval=7; end ");
            sql.Append(" else begin BEGIN TRANSACTION tan1");

            Entity.CTime = System.DateTime.Now;
            Entity.UTime = System.DateTime.Now;
            List<string> lstExcludeFilds = new List<string>();
            Dictionary<string, string> dicAttachFilds = new Dictionary<string, string>();
            lstExcludeFilds.Clear();
            lstExcludeFilds.Add("Id");
            lstExcludeFilds.Add("DisCode");
            lstExcludeFilds.Add("CTime");
            lstExcludeFilds.Add("UTime");
            dicAttachFilds.Clear();
            dicAttachFilds.Add("DisCode", "@DisCode");
            dicAttachFilds.Add("CTime", "getdate()");
            dicAttachFilds.Add("UTime", "getdate()");
           
            string InsertSql = EntityHelper.GenerateSqlByDE<TB_DishEntity>(dtStructure.Tables[0], Entity, lstExcludeFilds, dicAttachFilds, EntityHelper.eSqlType.insert);
            sql.AppendFormat(" INSERT INTO TB_Dish " + InsertSql + " ;");
            sql.AppendLine(" exec dbo.p_uploaddata_isSync '" + Entity.BusCode + "','" + Entity.StoCode + "','TB_Dish','DisCode',@DisCode,'add'");


            #region 菜品图片
            if (!string.IsNullOrEmpty(Entity.ImageName))
            {
                string[] ins = Entity.ImageName.Split(',');
                foreach (string name in ins)
                {
                    sql.AppendFormat(" INSERT INTO TR_DishImage(BusCode,StoCode,DisCode,ImgUrl) values(@BusCode,@StoCode,@DisCode,'/uploads/UpDishImages/" + name + "');");
                    sql.AppendLine(" exec dbo.p_uploaddata_isSync '" + Entity.BusCode + "','" + Entity.StoCode + "','TR_DishImage','DisCode',@DisCode,'add'");
                }
            }
            #endregion

            sql.AppendLine(" if(@@error=0) begin set @returnval=0; commit TRANSACTION tan1; end else begin rollback TRANSACTION tran1;set @returnval=1; end");
            sql.AppendLine(" end");
            sql.AppendLine(" select @returnval;");
            DataTable dt = DBHelper.ExecuteDataTable(sql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return Helper.StringToInt(dt.Rows[0][0].ToString());
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_DishEntity Entity)
        {
            DataSet dtStructure = DBHelper.ExecuteDataSet("select top 0 * from TB_Dish;select top 0 * from TR_DishesMethods;select top 0 * from TR_DisForCombo;select top 0 * from TR_DishImage;select top 0 * from TR_ComboInfo;");
            StringBuilder sql = new StringBuilder();
            sql.Append(" declare @ID int;");
            sql.Append(" declare @returnval int;");
            sql.Append(" set @returnval=0;");
            sql.Append(" declare @DisCode varchar(32);");
            sql.Append("declare @DisMethodCode varchar(32)");
            sql.Append(" if not exists(select 1 from TB_Dish where BusCode='" + Entity.BusCode + "' and StoCode='" + Entity.StoCode + "' and DisCode='" + Entity.DisCode + "') begin set @returnval=2; end ");
            sql.Append(" else if exists(select 1 from TB_Dish where BusCode='" + Entity.BusCode + "' and StoCode='" + Entity.StoCode + "' and DisCode<>'" + Entity.DisCode + "' and IsCombo='"+Entity.IsCombo+"' and ((QRCode='" + Entity.QRCode + "' and '" + Entity.QRCode + "'<>'') or DisName='"+Entity.DisName+"')) begin set @returnval=7; end ");
            sql.Append(" else begin BEGIN TRANSACTION tan1");
            Entity.CTime = System.DateTime.Now;
            Entity.UTime = System.DateTime.Now;
            List<string> lstExcludeFilds = new List<string>();
            Dictionary<string, string> dicAttachFilds = new Dictionary<string, string>();
            lstExcludeFilds.Clear();
            lstExcludeFilds.Add("Id");
            lstExcludeFilds.Add("DisCode");
            lstExcludeFilds.Add("CTime");
            lstExcludeFilds.Add("UTime");
            dicAttachFilds.Clear();
            dicAttachFilds.Add("CTime", "getdate()");
            dicAttachFilds.Add("UTime", "getdate()");
            string InsertSql = EntityHelper.GenerateSqlByDE<TB_DishEntity>(dtStructure.Tables[0], Entity, lstExcludeFilds, dicAttachFilds, EntityHelper.eSqlType.update);
            sql.AppendFormat(" UPDATE TB_Dish SET " + InsertSql + " where BusCode='" + Entity.BusCode + "' and DisCode='" + Entity.DisCode + "' and StoCode='"+Entity.StoCode+"' ;");
            sql.Append(" set @DisCode='" + Entity.DisCode + "';");
            sql.AppendLine(" DECLARE @BusCode varchar(16); DECLARE @StoCode varchar(16); ");
            sql.AppendLine(" SET @BusCode='" + Entity.BusCode + "';");
            sql.AppendLine(" SET @StoCode='" + Entity.StoCode + "';");
            sql.AppendLine(" exec dbo.p_uploaddata_isSync  @BusCode,@StoCode,'TB_Dish','DisCode',@DisCode,'update';	 ");

            #region 菜品图片
            if (!string.IsNullOrEmpty(Entity.ImageName))
            {
                sql.AppendLine(" delete TR_DishImage where BusCode=@BusCode and StoCode=@StoCode and DisCode=@DisCode;");
                string[] ins = Entity.ImageName.Split(',');
                foreach (string name in ins)
                {
                    sql.AppendFormat(" INSERT INTO TR_DishImage(BusCode,StoCode,DisCode,ImgUrl) values(@BusCode,@StoCode,@DisCode,'"+name+"');");
                    sql.AppendLine(" exec dbo.p_uploaddata_isSync '" + Entity.BusCode + "','" + Entity.StoCode + "','TR_DishImage','DisCode',@StoCode,'add'");
                }
            }
            #endregion
            sql.AppendLine(" if(@@error=0) begin commit TRANSACTION tan1; end else begin rollback TRANSACTION tran1;set @returnval=1; end");
            sql.AppendLine(" end");
            sql.AppendLine(" select @returnval;");
            DataTable dt = DBHelper.ExecuteDataTable(sql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return Helper.StringToInt(dt.Rows[0][0].ToString());
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="DisCode">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@ids", ids),
                new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Dish_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string DisCode, ref string mescode)
        {
            SqlParameter[] sqlParameters =
            {
                 new SqlParameter("@DisCode", DisCode),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };
            sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_Dish_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 获取指定菜品的做法加价信息
        /// </summary>
        /// <param name="disCode"></param>
        /// <returns></returns>
        public DataTable GetDisMethods(string busCode, string stoCode, string disCode)
        {
            string sql = "select dm.*,dms.MetName from TR_DishesMethods dm inner join [dbo].[TB_DishMethods] dms on dm.pkcode=dms.pkcode and dm.stocode=dms.stocode where dm.DisCode='" + disCode + "' and dm.BusCode='" + busCode + "' and dm.stoCode='" + stoCode + "'";
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        ///  获取指定套餐的套餐内菜品信息
        /// </summary>
        /// <param name="disCode"></param>
        /// <returns></returns>
        public DataTable GetDisForCombo(string busCode, string stoCode, string disCode)
        {
            string sql = "select fc.*,cg.GroupName,[dbo].[fn_GetDisTypeToTypeName](dis.TypeCode,dis.stocode) as TypeName,dis.DisName,dis.Price from TR_DisForCombo fc inner join (select * from TB_Dish d where discode in(SELECT [DisCode] FROM [dbo].[TR_DisForCombo] c where c.PDisCode='"+ disCode + "' and stocode='"+stoCode+ "')) dis on fc.DisCode=dis.DisCode and dis.StoCode='" + stoCode + "' left join TR_ComboInfo ci on fc.PPKCode=ci.PKCode and ci.StoCode=fc.StoCode left join TB_DisComGroup cg on ci.ComGroupCode=cg.PKCode and cg.StoCode=ci.StoCode where fc.PDisCode='"+ disCode + "' and fc.BusCode='"+busCode+"' and fc.StoCode='"+stoCode+"'";
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        ///  获取指定套餐的套餐组别信息
        /// </summary>
        /// <param name="disCode"></param>
        /// <returns></returns>
        public DataTable GetDisForComboInfo(string busCode, string stoCode, string disCode)
        {
            string sql = "select ci.*,cg.GroupName from TR_ComboInfo ci left join TB_DisComGroup cg on ci.ComGroupCode=cg.PKCode  and cg.StoCode=ci.StoCode where ci.PDisCode='" + disCode + "' and ci.BusCode='" + busCode + "' and ci.StoCode='" + stoCode + "'";
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        ///  获取菜品的图片信息
        /// </summary>
        /// <param name="disCode"></param>
        /// <returns></returns>
        public DataTable GetDisImages(string busCode, string stoCode, string disCode)
        {
            string sql = "select BusCode,StoCode,DisCode,ImgUrl as ImageName,'' as ImageUrl from TR_DishImage where DisCode='" + disCode + "' and BusCode='" + busCode + "' and StoCode='" + stoCode + "'";
            return DBHelper.ExecuteDataTable(sql, CommandType.Text, null);
        }

    }
}