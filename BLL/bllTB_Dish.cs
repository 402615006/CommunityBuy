using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
using System.Text;
using System;

namespace CommunityBuy.BLL
{
	/// <summary>
    /// 菜品信息业务类
    /// </summary>
    public class bllTB_Dish : bllBase
    {
		DAL.dalTB_Dish dal = new DAL.dalTB_Dish();
        TB_DishEntity Entity = new TB_DishEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type,string BusCode, string StoCode, string CCname, string UCname, string TStatus, string ChannelCodeList, string DisCode, string DisName, string OtherName, string TypeCode, string QuickCode, string CusDisCode, string Unit, string Price, string MenuCode, string MemPrice, string CostPrice, string RoyMoney, string ExtCode, string FinCode, string KitCode, string CookerCode, string MakeTime, string QRCode, string WarCode, string MatCode, string Descript, string IsCount, string DefCount, string CountPrice, string IsVarPrice, string IsWeight, string IsMethod, string IsStock, string IsPoint, string IsMemPrice, string IsCoupon, string IsKeep, string IsCombo,string CCode,string UCode,string FinTypeName)
        {
            bool rel = false;
            try
            {
                Entity = new TB_DishEntity();
                Entity.Id = 0;
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.TStatus = TStatus;
                Entity.ChannelCodeList = ChannelCodeList;
                Entity.DisCode = DisCode;
                Entity.DisName = DisName;
                Entity.OtherName = OtherName;
                Entity.TypeCode = TypeCode;
                Entity.QuickCode = QuickCode;
                Entity.Unit = Unit;
                Entity.Price = StringHelper.StringToDecimal(Price);
                Entity.MemPrice = StringHelper.StringToDecimal(MemPrice);
                Entity.CostPrice = StringHelper.StringToDecimal(CostPrice);
                Entity.RoyMoney = StringHelper.StringToDecimal(RoyMoney);
                Entity.QRCode = QRCode;
                Entity.Descript = Descript;
                Entity.IsPoint = IsPoint;
                Entity.IsMemPrice = IsMemPrice;
                Entity.IsCoupon = IsCoupon;
                rel = true;
            }
            catch (Exception)
            {
            }
            return rel;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(string GUID, string UID,string BusCode, string StoCode, string CCname, string UCname, string TStatus, string ChannelCodeList, string DisCode, string DisName, string OtherName, string TypeCode, string QuickCode, string CusDisCode, string Unit, string Price, string MenuCode, string MemPrice, string CostPrice, string RoyMoney, string ExtCode, string FinCode, string KitCode, string CookerCode, string MakeTime, string QRCode, string WarCode, string MatCode, string Descript, string IsCount, string DefCount, string CountPrice, string IsVarPrice, string IsWeight, string IsMethod, string IsStock, string IsPoint, string IsMemPrice, string IsCoupon, string IsKeep, string IsCombo, string CCode, string UCode,string ImageName, string dishesMethodsJson, string dishescombosJson,string dishescomboinfoJson,string FinTypeName)
        {
			DisCode = "0";
            int result = 0;
            bool strReturn = CheckPageInfo("add", BusCode, StoCode, CCname, UCname, TStatus, ChannelCodeList, DisCode, DisName, OtherName, TypeCode, QuickCode, CusDisCode, Unit, Price, MenuCode, MemPrice, CostPrice, RoyMoney, ExtCode, FinCode, KitCode, CookerCode, MakeTime, QRCode, WarCode, MatCode, Descript, IsCount, DefCount, CountPrice, IsVarPrice, IsWeight, IsMethod, IsStock, IsPoint, IsMemPrice, IsCoupon, IsKeep, IsCombo,CCode,UCode, FinTypeName);
            //数据页面验证
            result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result, Entity.DisCode);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_DishEntity UEntity)
        {
		
			//更新数据
            int result = dal.Update(UEntity);
            CheckResult(result, "");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string GUID, string UID, string DisCode)
        {
			string Mescode = string.Empty;
            int result = dal.Delete(DisCode, ref Mescode);
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
            return GetPagingListInfo(GUID, UID, 1, 1, filter, string.Empty, string.Empty,out recnums, out pagenums);
        }

        /// <summary>
        /// 获取单条数据实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TB_DishEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty,string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_DishEntity();
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
        public DataTable GetPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order,string opencode, out int recnums, out int pagenums)
        {
            return new bllPaging().GetPagingInfo(
                "TB_Dish dis " +
                " left join [dbo].[TB_DishType] dist on dis.typecode=dist.PKCode and dist.stocode=dis.stocode " +
                " left join [dbo].[TB_DishMenu] dism on dis.MenuCode=dism.PKCode  and dism.Stocode=dis.stocode "
                ,
                "dis.DisCode",
                "dist.PKKCode as pdistypecode," +
                "dist.TypeName,dism.MenuName" 
                , pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }
        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_DishEntity SetEntityInfo(DataRow dr)
        {
            TB_DishEntity Entity = new TB_DishEntity();
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();

			Entity.TStatus = dr["TStatus"].ToString();
			Entity.ChannelCodeList = dr["ChannelCodeList"].ToString();
			Entity.DisCode = dr["DisCode"].ToString();
			Entity.DisName = dr["DisName"].ToString();
			Entity.OtherName = dr["OtherName"].ToString();
			Entity.TypeCode = dr["TypeCode"].ToString();
			Entity.QuickCode = dr["QuickCode"].ToString();

			Entity.Unit = dr["Unit"].ToString();
			Entity.Price = StringHelper.StringToDecimal(dr["Price"].ToString());

			Entity.MemPrice = StringHelper.StringToDecimal(dr["MemPrice"].ToString());
			Entity.CostPrice = StringHelper.StringToDecimal(dr["CostPrice"].ToString());
			Entity.RoyMoney = StringHelper.StringToDecimal(dr["RoyMoney"].ToString());
			
			Entity.QRCode = dr["QRCode"].ToString();
			
			Entity.Descript = dr["Descript"].ToString();
			
			Entity.IsPoint = dr["IsPoint"].ToString();
			Entity.IsMemPrice = dr["IsMemPrice"].ToString();
			Entity.IsCoupon = dr["IsCoupon"].ToString();
			
            return Entity;
        }


        /// <summary>
        /// 获取菜品图片信息
        /// </summary>
        /// <param name="disCode"></param>
        /// <returns></returns>
        public DataTable GetDisImages(string busCode, string stoCode, string disCode)
        {
            return dal.GetDisImages(busCode, stoCode, disCode);
        }

    }
}