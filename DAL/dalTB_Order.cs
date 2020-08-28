using System;
using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 订单数据访问类
    /// </summary>
    public partial class dalTB_Order
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
		int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref TB_OrderEntity Entity,string DishListJson)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
                new SqlParameter("@PKCode",SqlDbType.VarChar,32){ Value=Entity.PKCode},
                new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@OrderMoney", Entity.OrderMoney),
				new SqlParameter("@Remar", Entity.Remar),
                new SqlParameter("@DisNum", Entity.DisNum),
                new SqlParameter("@OrderType", Entity.OrderType),
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_Order_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn ==1)
            {
                Entity.PKCode = sqlParameters[0].Value.ToString();
                int disRel = -1;
                try
                {
                    //解析json拼接SQL
                    DataTable dtDish = JsonHelper.ToDataTable(DishListJson);
                    string dishSql = " declare @odiscode varchar(32);";
                    dishSql += " declare @podiscode varchar(32);";
                    dishSql += " set @podiscode='';";
                    string checkmoneySql=" declare @allmoney decimal(18,2);";
                    checkmoneySql += " set @allmoney=0; ";
                    if (dtDish != null && dtDish.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDish.Rows)
                        {
                            string disPrice = dr["price"].ToString();
                            string totalmoney = "0";
                            totalmoney = disPrice + "*" + dr["disnum"].ToString() + "+" + dr["cookmoney"].ToString();
                            string tempSql = " exec[dbo].[p_GetOrderCode] @odiscode output;";
                            tempSql += " insert into TB_OrderDish([StoCode],[OrderCode],[DisCode],[DisName],[Price],[DisNum],[PKCode],[CookName],[CookMoney],[TotalMoney]) ";
                            tempSql += " values('"+Entity.StoCode+ "','" + Entity.PKCode + "','" + dr["discode"] + "','" + dr["disname"] + "',"+dr["price"]+ "," + dr["disnum"] + ",@odiscode,'"+dr["cookname"]+"',"+dr["cookmoney"]+","+ totalmoney + ");";
                            dishSql += tempSql;
                        }
                        disRel = DBHelper.ExecuteNonQuery(dishSql, CommandType.Text, new SqlParameter[] { });
                    }
                }
                catch (Exception ex){}
                if (disRel != 0)
                {
                    //删除订单信息，返回失败
                    string meg = "";
                    Delete(Entity.PKCode, Entity.StoCode, ref meg);
                    intReturn = -1;
                }
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(TB_OrderEntity Entity)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", Entity.Id),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@PKCode", Entity.PKCode),
				new SqlParameter("@OrderMoney", Entity.OrderMoney),
				new SqlParameter("@Remar", Entity.Remar),
				new SqlParameter("@CheckTime", Entity.CheckTime),
                new SqlParameter("@OrderType", Entity.OrderType),
                new SqlParameter("@PayMoney", Entity.PayMoney),
                new SqlParameter("@CouponMoney", Entity.CouponMoney),
                new SqlParameter("@WxBillCode", Entity.WxBillCode),
                new SqlParameter("@CouponCode", Entity.CouponCode),
                new SqlParameter("@FTime", Entity.FTime),
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Order_Update", CommandType.StoredProcedure, sqlParameters); 
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status,string stocode)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
                new SqlParameter("@stocode", stocode),
                new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_TB_Order_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string pkcode,string stocode, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@pkcode", pkcode),
                 new SqlParameter("@stocode", stocode),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };
			sqlParameters[2].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_Order_Delete", CommandType.StoredProcedure, sqlParameters);
            if (intReturn >= 0)
            {
                mescode = sqlParameters[2].Value.ToString();
            }
            
            return intReturn;
        }
    }
}