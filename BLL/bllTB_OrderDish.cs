using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
using System.Text;

namespace CommunityBuy.BLL
{
	/// <summary>
    /// 订单菜品业务类
    /// </summary>
    public class bllTB_OrderDish : bllBase
    {
		DAL.dalTB_OrderDish dal = new DAL.dalTB_OrderDish();
        TB_OrderDishEntity Entity = new TB_OrderDishEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCode, string CCname, string OrderCode, string FinCode, string DisTypeCode, string DisCode, string DisName, string MemPrice, string Price, string DisUite, string DisNum, string ReturnNum, string IsPackage, string PDisCode, string Remar, string PKCode, string DiscountPrice, string DiscountRemark, string DiscountType, string DisCase, string Favor, string ItemNum, string ItemPrice, string CookName, string CookMoney, string TotalMoney)
        {
            bool rel = false;
            try
            {
                Entity = new TB_OrderDishEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;

                Entity.OrderCode = OrderCode;
                Entity.DisTypeCode = DisTypeCode;
                Entity.DisCode = DisCode;
                Entity.DisName = DisName;
                Entity.MemPrice = StringHelper.StringToDecimal(MemPrice);
                Entity.Price = StringHelper.StringToDecimal(Price);
                Entity.DisUite = DisUite;
                Entity.DisNum = StringHelper.StringToDecimal(DisNum);

                Entity.PDisCode = PDisCode;
                Entity.Remar = Remar;
                Entity.PKCode = PKCode;

                Entity.TotalMoney = StringHelper.StringToDecimal(TotalMoney);
                rel = true;
            }
            catch (System.Exception)
            {

            }
            return rel;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(string GUID, string UID, string Id, string BusCode, string StoCode, string CCode, string CCname, string OrderCode, string FinCode, string DisTypeCode, string DisCode, string DisName, string MemPrice, string Price, string DisUite, string DisNum, string ReturnNum, string IsPackage, string PDisCode, string Remar, string PKCode, string DiscountPrice, string DiscountRemark, string DiscountType, string DisCase, string Favor, string ItemNum, string ItemPrice, string CookName, string CookMoney, string TotalMoney)
        {
            int result = 0;
            bool strReturn = CheckPageInfo("add",  Id, BusCode, StoCode, CCode, CCname, OrderCode, FinCode, DisTypeCode, DisCode, DisName, MemPrice, Price, DisUite, DisNum, ReturnNum, IsPackage, PDisCode, Remar, PKCode, DiscountPrice, DiscountRemark, DiscountType, DisCase, Favor, ItemNum, ItemPrice, CookName, CookMoney, TotalMoney);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
            }
            result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result,"");
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_OrderDishEntity UEntity)
        {
			//更新数据
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result, "");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string GUID, string UID, string Id)
        {
			string Mescode = string.Empty;
            int result = dal.Delete(Id, ref Mescode);
            //检测执行结果
            CheckResult(result, Mescode);
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="filter">指定条件</param>
        /// <returns>返回第一行</returns>
        public DataTable GetPagingSigInfo(string GUID, string UID, string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            return GetPagingListInfo(GUID, UID, 1, 1, filter, string.Empty, out recnums, out pagenums);
        }

		/// <summary>
        /// 获取单条数据实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TB_OrderDishEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_OrderDishEntity();
        }

		/// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <returns></returns>
        public DataTable GetPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            return new bllPaging().GetPagingInfo("TB_OrderDish od inner join tb_order o on od.ordercode=o.pkcode left join TB_Dish dis on od.discode=dis.discode and od.stocode=dis.stocode left join TR_SpecialOfferDish s on s.Discode=dis.DisCode and s.Stocode=dis.Stocode and s.SpeCode in(select PKCode from TB_SpecialOffer where StoCode=s.StoCode and TStatus='1'  " +
                " and charindex(convert(char(1), Datepart(weekday, getdate() + @@DateFirst - 1)), Week) > 0 " +
                " and convert(varchar(10), getdate(), 23) between  convert(varchar(10), StartTime, 23) and convert(varchar(10), EndTime, 23) " +
                " and convert(varchar(8), getdate(), 8) between convert(varchar(8), StartTime, 8) and convert(varchar(8), EndTime, 8)) ", "od.Id", "od.disnum,od.cookmoney,od.totalmoney,od.uptype,od.returnnum,od.DiscountPrice,od.cookname,od.OrderCode,od.DiscountType,od.IsPackage,od.PDisCode,od.PKCode,od.Favor,od.ItemNum,od.ItemPrice,od.remar,od.discase,dis.*,s.DiscountPrice as SecPrice,od.IsMp,od.MpCheckCode as checkcode" +
                ",(case when isnull(s.DiscountPrice,'0')='0' then '0' else '1' end) as IsSecPrice", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_OrderDishEntity SetEntityInfo(DataRow dr)
        {
            TB_OrderDishEntity Entity = new TB_OrderDishEntity();
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.OrderCode = dr["OrderCode"].ToString();
			Entity.DisTypeCode = dr["DisTypeCode"].ToString();
			Entity.DisCode = dr["DisCode"].ToString();
			Entity.DisName = dr["DisName"].ToString();
			Entity.MemPrice = StringHelper.StringToDecimal(dr["MemPrice"].ToString());
			Entity.Price = StringHelper.StringToDecimal(dr["Price"].ToString());
			Entity.DisUite = dr["DisUite"].ToString();
			Entity.DisNum = StringHelper.StringToDecimal(dr["DisNum"].ToString());

			Entity.PDisCode = dr["PDisCode"].ToString();
			Entity.Remar = dr["Remar"].ToString();
			Entity.PKCode = dr["PKCode"].ToString();

			Entity.TotalMoney = StringHelper.StringToDecimal(dr["TotalMoney"].ToString());
            return Entity;
        }

        /// <summary>
        /// 退菜
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public void BackOrderDish(string GUID, string UID,string stocode, string dishordercode)
        {
            string Mescode = string.Empty;
            int result = dal.BackOrderDish(stocode, dishordercode);
            //检测执行结果
            CheckResult(result, "");
        }

        public DataTable GetOrderDishByTable(string GUID, string UID, string stocode,string tablecode, int pageSize, int currentpage,out int recnums,out int pagenums)
        {
            recnums = 0;
            pagenums = 0;
            string filter = " od.OrderCode in(select OrderCode from TB_Order where OpenCodeList in(select PKCode from TB_OpenTable where TableCode='" + tablecode + "')) and od.Stocode='" + stocode + "' ";

            return GetPagingListInfo(GUID, UID, pageSize, currentpage, filter, string.Empty, out recnums, out pagenums);
        }

        /// <summary>
        /// 整表更新订单菜品 
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="StoCode"></param>
        /// <param name="dtDish"></param>
        public void UpdateByTable(string GUID, string UID,string Stocode, DataTable dtDish)
        {
            StringBuilder sbSql = new StringBuilder();
            foreach (DataRow dr in dtDish.Rows)
            {
                string discountType = dr["DiscountType"].ToString();
                string discountmoney= dr["DiscountPrice"].ToString();
                string pkcode = dr["orderdiscode"].ToString();
                string totalmoney = ((StringHelper.StringToDecimal(dr["disnum"].ToString()))* StringHelper.StringToDecimal(dr["DiscountPrice"].ToString())).ToString("f2");
                string tempSql = string.Format("Update TB_OrderDish set DiscountType='{0}',DiscountPrice={1},TotalMoney='{4}' where PKCode='{2}' and Stocode='{3}';", discountType, discountmoney, pkcode, Stocode, totalmoney);
                sbSql.AppendLine(tempSql);
            }
            int rel=new bllPaging().ExecuteNonQueryBySQL(sbSql.ToString());
        }

    }
}