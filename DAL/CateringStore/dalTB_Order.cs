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
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@OpenCodeList", Entity.OpenCodeList),
				new SqlParameter("@OrderMoney", Entity.OrderMoney),
				new SqlParameter("@DisNum", Entity.DisNum),
				new SqlParameter("@DisTypeNum", Entity.DisTypeNum),
				new SqlParameter("@Remar", Entity.Remar),
				new SqlParameter("@CheckTime", Entity.CheckTime),
				new SqlParameter("@BillCode", Entity.BillCode),
                new SqlParameter("@OrderType", Entity.OrderType),
                new SqlParameter("@DepartCode", Entity.DepartCode)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_TB_Order_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.PKCode = sqlParameters[0].Value.ToString();
                int disRel = -1;
                try
                {
                    //解析json拼接SQL
                    DataTable dtDish = JsonHelper.ToDataTable(DishListJson);
                    decimal DishTotalMoney = 0;
                    if (!dtDish.Columns.Contains("checkcode"))
                    {
                        dtDish.Columns.Add("checkcode");
                    }
                    string dishSql = " declare @odiscode varchar(32);";
                    dishSql += " declare @podiscode varchar(32);";
                    dishSql += " set @podiscode='';";
                    string checkmoneySql=" declare @allmoney decimal(18,2);";
                    checkmoneySql += " set @allmoney=0; ";
                    if (dtDish != null && dtDish.Rows.Count > 0)
                    {
                        string dislist = "";
                        foreach (DataRow dr in dtDish.Rows)
                        {
                            dislist += "'" + dr["discode"].ToString() + "',";
                            string disPrice = "[Price]";
                            string discountPrice = "[Price]";
                            string discountType = "";
                            string IsMp = "0";
                            //时价特价
                            if (dr["IsSecPrice"].ToString() == "1")
                            {
                                disPrice = dr["SecPrice"].ToString();
                                discountPrice = dr["SecPrice"].ToString();
                                discountType = "5";
                            }

                            if (!string.IsNullOrWhiteSpace(dr["checkcode"].ToString()))
                            {
                                IsMp = "1";
                            }


                            string totalmoney = "0";
                            if (Helper.StringToDecimal(dr["itemnum"].ToString()) > 0)
                            {
                                totalmoney = dr["itemnum"].ToString() + "*" + dr["itemprice"].ToString() + "+" + dr["cookmoney"].ToString();
                            }
                            else
                            {
                                totalmoney = disPrice + "*" + dr["disnum"].ToString() + "+" + dr["cookmoney"].ToString();
                            }
                            //TestDemo
                            //DishTotalMoney += totalmoney;
                            //

                            string tempSql = " exec[dbo].[p_GetOrderCode] '" + Entity.StoCode + "',@odiscode output;";
                            if (dr["ispackage"].ToString() == "1")
                            {
                                tempSql += " set @podiscode=@odiscode;";
                            }
                            else if (dr["ispackage"].ToString() != "2")
                            {
                                tempSql += " set @podiscode='';";
                            }
                            if (dr["ispackage"].ToString() != "2")
                            {
                                checkmoneySql += string.Format(" select @allmoney=@allmoney+" + totalmoney + " from TB_Dish where DisCode='{0}' and StoCode='{1}';", dr["discode"], Entity.StoCode);
                            }

                            tempSql += string.Format(" insert into TB_OrderDish( [BusCode],[StoCode],[CCode] ,[CCname] ,[CTime],[OrderCode],[FinCode],[DisTypeCode],[DisCode],[DisName],[MemPrice],[Price],[DisUite],[DisNum],[ReturnNum],[IsPackage],[PDisCode],[Remar],[PKCode],[DiscountPrice],[DiscountRemark],[DiscountType],[DisCase], [Favor],[ItemNum],[ItemPrice],[CookName],[CookMoney],[TotalMoney],[UpType],[IsMp],[MpCheckCode])");
                            tempSql += string.Format(" select [BusCode],[StoCode],'{1}','{2}',getdate(),'{3}',[FinCode],[TypeCode] ,[DisCode] ,[DisName] ,[MemPrice] ,{16} ,[Unit] ,{4} ,0 ,'{5}',{6},'{7}',@odiscode,{17} ,'','{18}','{8}','{9}',{10},{11},'{12}',{13},{15},'{14}','{20}','{21}' from TB_Dish where DisCode='{0}' and StoCode='{19}';"
                           , dr["discode"], Entity.CCode, Entity.CCname, Entity.PKCode, dr["disnum"], dr["ispackage"], "@podiscode", dr["remark"], dr["discase"], dr["favor"], string.IsNullOrWhiteSpace(dr["itemnum"].ToString())?0:dr["itemnum"], string.IsNullOrWhiteSpace(dr["itemprice"].ToString())?0:dr["itemprice"], dr["cookname"], dr["cookmoney"], dr["UpType"], totalmoney, disPrice, discountPrice, discountType, Entity.StoCode, IsMp, dr["checkcode"]);
                            dishSql += tempSql;

                        }
                        checkmoneySql += "select @allmoney;";

                        try
                        {
                            DishTotalMoney = Helper.StringToDecimal(DBHelper.ExecuteScalar(checkmoneySql).ToString());
                        }
                        catch (Exception)
                        {
                        }
                        if (DishTotalMoney != Entity.OrderMoney)
                        {
                            Entity.Remar = "";
                            //删除订单信息
                            string mescode = "";
                            Delete(Entity.PKCode, Entity.StoCode, ref mescode);
                            intReturn = 6;
                            return intReturn;
                        }

                        string checkoutsql = "select disname,discode,[dbo].[fn_GetDishSellout](discode,stocode) as sellout,[dbo].[fn_GetOrderedDisNum](discode,stocode,'') as ordernum from TB_Dish where discode in(" + dislist.TrimEnd(',') + ") and stocode='" + Entity.StoCode + "'";
                        string disnames = "";
                        DataTable dtOutDish = DBHelper.ExecuteDataTable(checkoutsql);
                        if (dtOutDish != null && dtOutDish.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtOutDish.Rows)
                            {
                                if (Helper.StringToDecimal(dr["sellout"].ToString()) != -1)
                                {
                                    decimal ordernum = Helper.SumDataTableColumn(dtDish, new string[] { "disnum" }, "sum(disnum)", "discode='" + dr["discode"].ToString() + "'");
                                    decimal selloutnum = Helper.StringToDecimal(dr["sellout"].ToString());
                                    decimal orderednum = Helper.StringToDecimal(dr["ordernum"].ToString());
                                    if ((ordernum + orderednum) >= selloutnum)
                                    {
                                        disnames += dr["disname"].ToString() + ",";
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(disnames))
                        {
                            Entity.Remar = disnames;
                            //删除订单信息
                            string mescode = "";
                            Delete(Entity.PKCode, Entity.StoCode, ref mescode);
                            intReturn = 5;
                            return intReturn;
                        }
                        disRel = DBHelper.ExecuteNonQuery(dishSql, CommandType.Text, new SqlParameter[] { });
                    }
                    else
                    {
                        //零元结账
                        disRel = 0;
                    }
                    

                }
                catch (Exception ex)
                {

                }

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
				new SqlParameter("@BusCode", Entity.BusCode),
				new SqlParameter("@StoCode", Entity.StoCode),
				new SqlParameter("@CCode", Entity.CCode),
				new SqlParameter("@CCname", Entity.CCname),
				new SqlParameter("@TStatus", Entity.TStatus),
				new SqlParameter("@PKCode", Entity.PKCode),
				new SqlParameter("@OpenCodeList", Entity.OpenCodeList),
				new SqlParameter("@OrderMoney", Entity.OrderMoney),
				new SqlParameter("@DisNum", Entity.DisNum),
				new SqlParameter("@DisTypeNum", Entity.DisTypeNum),
				new SqlParameter("@Remar", Entity.Remar),
				new SqlParameter("@CheckTime", Entity.CheckTime),
				new SqlParameter("@BillCode", Entity.BillCode),
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
            mescode = sqlParameters[2].Value.ToString();
            return intReturn;
        }
    }
}