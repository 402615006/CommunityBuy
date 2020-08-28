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
            StringBuilder sql = new StringBuilder();
            sql.Append(" declare @ID int;");
            sql.Append(" declare @returnval int;");
            sql.Append(" set @returnval=0;");
            sql.Append("declare @DisCode varchar(32);");
            sql.Append("declare @DisMethodCode varchar(32);");
            sql.Append("exec [dbo].[p_GetbaseCode] @DisCode output;");
            sql.Append(" if exists(select 1 from TB_Dish where StoCode='" + Entity.StoCode + "' and DisName='" + Entity.DisName + "') set @returnval=1;");
            sql.Append("BEGIN TRANSACTION tan1");
            string InsertSql = " values('{0}',getdate(),'1',@DisCode,'{1}','{2}','{3}','{4}','{5}',{6},{7},'{8}','{9}')";
            InsertSql = string.Format(InsertSql, Entity.StoCode, Entity.DisName, Entity.OtherName, Entity.TypeCode, Entity.QuickCode, Entity.Unit, Entity.Price, Entity.CostPrice, Entity.QRCode, Entity.Descript);
            sql.AppendFormat(" INSERT INTO TB_Dish " + InsertSql + " ;");
            #region 菜品图片
            if (!string.IsNullOrEmpty(Entity.ImageName))
            {
                string[] ins = Entity.ImageName.Split(',');
                foreach (string name in ins)
                {
                    sql.AppendFormat(" INSERT INTO TR_DishImage(DisCode,ImgUrl) values(@DisCode,'/uploads/UpDishImages/" + name + "');");
                }
            }
            #endregion
            sql.AppendLine(" if(@@error=0) begin set @returnval=0; commit TRANSACTION tan1; end else begin rollback TRANSACTION tran1;set @returnval=1; end");
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
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_DishEntity Entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" declare @ID int;");
            sql.Append(" declare @returnval int;");
            sql.Append(" set @returnval=0;");
            sql.Append(" declare @DisCode varchar(32);");
            sql.Append(" set @DisCode='" + Entity.DisCode + "';");
            sql.Append("declare @DisMethodCode varchar(32)");
            sql.Append(" if not exists(select 1 from TB_Dish where StoCode='" + Entity.StoCode + "' and DisCode='" + Entity.DisCode + "')  set @returnval=2;  ");
            sql.Append(" if exists(select 1 from TB_Dish where StoCode='" + Entity.StoCode + "' and DisCode<>'" + Entity.DisCode + "'  and ((QRCode='" + Entity.QRCode + "' and '" + Entity.QRCode + "'<>'') or DisName='"+Entity.DisName+"'))  set @returnval=7;  ");
            sql.Append(" if @returnval=0 begin");

            string InsertSql = "[StoCode] ='{0}',[DisName] ='{1}',[OtherName] ='{2}',[TypeCode] ='{3}',[QuickCode] ='{4}',[Unit] ='{5}',[Price] ={6},[CostPrice] ={7} ,[QRCode] ='{8}' ,[Descript] ='{9}'";
            InsertSql = string.Format(InsertSql, Entity.StoCode, Entity.DisName, Entity.OtherName, Entity.TypeCode, Entity.QuickCode, Entity.Unit, Entity.Price, Entity.CostPrice, Entity.QRCode, Entity.Descript);
            sql.AppendFormat(" UPDATE TB_Dish SET " + InsertSql + " where DisCode='" + Entity.DisCode + "' and StoCode='"+Entity.StoCode+"' ;");
            #region 菜品图片
            if (!string.IsNullOrEmpty(Entity.ImageName))
            {
                sql.AppendLine(" delete TR_DishImage where DisCode=@DisCode;");
                string[] ins = Entity.ImageName.TrimEnd(',').Split(',');
                foreach (string name in ins)
                {
                    sql.AppendFormat(" INSERT INTO TR_DishImage(DisCode,ImgUrl) values(@DisCode,'"+name+"');");
                }
            }
            #endregion
            sql.AppendFormat(" set @returnval=1;");
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