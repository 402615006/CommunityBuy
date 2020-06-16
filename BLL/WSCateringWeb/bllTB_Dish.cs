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
        public string CheckPageInfo(string type,string BusCode, string StoCode, string CCname, string UCname, string TStatus, string ChannelCodeList, string DisCode, string DisName, string OtherName, string TypeCode, string QuickCode, string CusDisCode, string Unit, string Price, string MenuCode, string MemPrice, string CostPrice, string RoyMoney, string ExtCode, string FinCode, string KitCode, string CookerCode, string MakeTime, string QRCode, string WarCode, string MatCode, string Descript, string IsCount, string DefCount, string CountPrice, string IsVarPrice, string IsWeight, string IsMethod, string IsStock, string IsPoint, string IsMemPrice, string IsCoupon, string IsKeep, string IsCombo,string CCode,string UCode,string FinTypeName)
        {
			string strRetuen = string.Empty;
            //要验证的实体属性
            List<string> EName = new List<string>() {  };
            //要验证的实体属性值
            List<string> EValue = new List<string>() {  };
            //错误信息
            List<string> errorCode = new List<string>();
            List<string> ControlName = new List<string>();
            //验证数据
            CheckValue<TB_DishEntity>(EName, EValue, ref errorCode, new TB_DishEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new TB_DishEntity();
				Entity.Id = 0;
				Entity.BusCode = BusCode;
				Entity.StoCode = StoCode;
				Entity.CCode = CCode;
				Entity.CCname = CCname;
				
				Entity.UCode = UCode;
				Entity.UCname = UCname;

                Entity.TStatus = TStatus;
				Entity.ChannelCodeList = ChannelCodeList;
				Entity.DisCode = DisCode;
				Entity.DisName = DisName;
				Entity.OtherName = OtherName;
				Entity.TypeCode = TypeCode;
                Entity.FinTypeName = FinTypeName;
                Entity.QuickCode = QuickCode;
				Entity.CusDisCode = CusDisCode;
				Entity.Unit = Unit;
				Entity.Price = Helper.StringToDecimal(Price);
				Entity.MenuCode = MenuCode;
				Entity.MemPrice = Helper.StringToDecimal(MemPrice);
				Entity.CostPrice = Helper.StringToDecimal(CostPrice);
				Entity.RoyMoney = Helper.StringToDecimal(RoyMoney);
				Entity.ExtCode = ExtCode;
				Entity.FinCode = FinCode;
				Entity.KitCode = KitCode;
				Entity.CookerCode = CookerCode;
				Entity.MakeTime = Helper.StringToInt(MakeTime);
				Entity.QRCode = QRCode;
				Entity.WarCode = WarCode;
				Entity.MatCode = MatCode;
				Entity.Descript = Descript;
				Entity.IsCount = IsCount;
				Entity.DefCount = Helper.StringToInt(DefCount);
				Entity.CountPrice = Helper.StringToDecimal(CountPrice);
				Entity.IsVarPrice = IsVarPrice;
				Entity.IsWeight = IsWeight;
				Entity.IsMethod = IsMethod;
				Entity.IsStock = IsStock;
				Entity.IsPoint = IsPoint;
				Entity.IsMemPrice = IsMemPrice;
				Entity.IsCoupon = IsCoupon;
				Entity.IsKeep = IsKeep;
				Entity.IsCombo = IsCombo;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID,string BusCode, string StoCode, string CCname, string UCname, string TStatus, string ChannelCodeList, string DisCode, string DisName, string OtherName, string TypeCode, string QuickCode, string CusDisCode, string Unit, string Price, string MenuCode, string MemPrice, string CostPrice, string RoyMoney, string ExtCode, string FinCode, string KitCode, string CookerCode, string MakeTime, string QRCode, string WarCode, string MatCode, string Descript, string IsCount, string DefCount, string CountPrice, string IsVarPrice, string IsWeight, string IsMethod, string IsStock, string IsPoint, string IsMemPrice, string IsCoupon, string IsKeep, string IsCombo, string CCode, string UCode,string ImageName, string dishesMethodsJson, string dishescombosJson,string dishescomboinfoJson,string FinTypeName, operatelogEntity entity)
        {
			DisCode = "0";
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add", BusCode, StoCode, CCname, UCname, TStatus, ChannelCodeList, DisCode, DisName, OtherName, TypeCode, QuickCode, CusDisCode, Unit, Price, MenuCode, MemPrice, CostPrice, RoyMoney, ExtCode, FinCode, KitCode, CookerCode, MakeTime, QRCode, WarCode, MatCode, Descript, IsCount, DefCount, CountPrice, IsVarPrice, IsWeight, IsMethod, IsStock, IsPoint, IsMemPrice, IsCoupon, IsKeep, IsCombo,CCode,UCode, FinTypeName);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
            Entity.ImageName = ImageName;
            if (!string.IsNullOrEmpty(dishescomboinfoJson))//套餐组合信息
            {
                Entity.ComboInfo = EntityHelper.GetEntityListByJson<TR_ComboInfoEntity>(dishescomboinfoJson);
            }
            int result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public DataTable Update(string GUID, string UID, string BusCode, string StoCode, string CCname, string UCname, string TStatus, string ChannelCodeList, string DisCode, string DisName, string OtherName, string TypeCode, string QuickCode, string CusDisCode, string Unit, string Price, string MenuCode, string MemPrice, string CostPrice, string RoyMoney, string ExtCode, string FinCode, string KitCode, string CookerCode, string MakeTime, string QRCode, string WarCode, string MatCode, string Descript, string IsCount, string DefCount, string CountPrice, string IsVarPrice, string IsWeight, string IsMethod, string IsStock, string IsPoint, string IsMemPrice, string IsCoupon, string IsKeep, string IsCombo,string CCode,string UCode, string ImageName, string dishesMethodsJson, string dishescombosJson,string dishescomboinfoJson,string FinTypeName, operatelogEntity entity)
        {
			
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update", BusCode, StoCode, CCname, UCname, TStatus, ChannelCodeList, DisCode, DisName, OtherName, TypeCode, QuickCode, CusDisCode, Unit, Price, MenuCode, MemPrice, CostPrice, RoyMoney, ExtCode, FinCode, KitCode, CookerCode, MakeTime, QRCode, WarCode, MatCode, Descript, IsCount, DefCount, CountPrice, IsVarPrice, IsWeight, IsMethod, IsStock, IsPoint, IsMemPrice, IsCoupon, IsKeep, IsCombo,CCode,UCode, FinTypeName);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
            if (!string.IsNullOrEmpty(dishescomboinfoJson))//套餐组合信息
            {
                Entity.ComboInfo = EntityHelper.GetEntityListByJson<TR_ComboInfoEntity>(dishescomboinfoJson);
            }
            Entity.ImageName = ImageName;
            //获取更新前的数据对象
            TB_DishEntity OldEntity = new TB_DishEntity();
            OldEntity = GetEntitySigInfo(" where DisCode='" + DisCode + "' and StoCode='"+StoCode+"'");
			//更新数据
            int result = dal.Update(Entity);
            if (result == 7)
            {
                DataRow dr = dtBase.NewRow();
                dr["type"] = "1";
                dr["mes"] = "已存在相同二维码的菜品,操作失败";
                dtBase.Rows.Add(dr);
                dtBase.AcceptChanges();
            }
            else
            {
                //检测执行结果
                if (CheckResult(result))
                {
                    //写日志
                    if (entity != null)
                    {
                        blllog.Add<TB_DishEntity>(entity, Entity, OldEntity);
                    }
                }
            }
          
            return dtBase;
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="DisCode">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataTable UpdateStatus(string GUID, string UID, string ids, string Status)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateStatus(ids, Status);
            //检测执行结果
			CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public DataTable Delete(string GUID, string UID, string DisCode, operatelogEntity entity)
        {
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
			string Mescode = string.Empty;
            int result = dal.Delete(DisCode, ref Mescode);
            //检测执行结果
            if (CheckDeleteResult(result,Mescode))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add(entity);
                }

            }
            return dtBase;
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
        /// 门店后台装用
        /// </summary>
        /// <param name="filter">指定条件</param>
        /// <returns>返回第一行</returns>
        public DataTable GetWebPagingSigInfo(string GUID, string UID, string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            return GetWebPagingListInfo(GUID, UID, 1, 1, filter, string.Empty, string.Empty, out recnums, out pagenums);
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
            if (!CheckLogin(GUID, UID))//非法登录
            {
                recnums = -1;
                pagenums = -1;
                return dtBase;
            }
            return new bllPaging().GetPagingInfo(
                "TB_Dish dis " +
                " left join [dbo].[TB_DishType] dist on dis.typecode=dist.PKCode and dist.stocode=dis.stocode " +
                " left join [dbo].[TB_DishMenu] dism on dis.MenuCode=dism.PKCode  and dism.Stocode=dis.stocode " +
                " left join [dbo].[TB_Kitchen] kit on dis.KitCode=kit.PKCode and kit.Stocode=dis.stocode " +
                " left join TR_SpecialOfferDish s on s.Discode=dis.DisCode and s.Stocode=dis.Stocode and s.SpeCode in(select PKCode from TB_SpecialOffer where StoCode=s.StoCode and TStatus='1'  " +
                " and charindex(convert(char(1), Datepart(weekday, getdate() + @@DateFirst - 1)), Week) > 0 " +
                " and convert(varchar(10), getdate(), 23) between  convert(varchar(10), StartTime, 23) and convert(varchar(10), EndTime, 23) " +
                " and convert(varchar(8), getdate(), 8) between convert(varchar(8), StartTime, 8) and convert(varchar(8), EndTime, 8)) " +
                " left join (select isnull(sum(disnum - returnnum), 0) as orderednum, DisCode,StoCode from TB_OrderDish where OrderCode in (select PKCode from TB_Order where OpenCodeList = '"+ opencode + "' and TStatus <> '2' )  group by discode, stocode)od on od.DisCode = dis.DisCode and od.StoCode=dis.StoCode"
                ,
                "dis.DisCode",
                "dbo.fn_GetDishSellout(dis.DisCode,dis.StoCode) as  selloutnum," +
                "dist.PKKCode as pdistypecode," +
                "od.orderednum," +
                "dis.*,(case dis.TStatus when '0' then '无效' else '有效' end) as TStatusName,'' as ChannelCodeListName," +
                "dist.TypeName,dism.MenuName,kit.KitName,(case dis.IsCount when '0' then '否' else '是' end) IsCountName,(case dis.IsVarPrice when '0' then '否' else '是' end) IsVarPriceName," +
                "(case dis.IsWeight when '0' then '否' else '是' end) IsWeightName,(case dis.IsMethod when '0' then '否' else '是' end) IsMethodName,(case dis.IsStock when '0' then '否' else '是' end) IsStockName," +
                "(case dis.IsPoint when '0' then '否' else '是' end) IsPointName,(case dis.IsMemPrice when '0' then '否' else '是' end) IsMemPriceName,(case dis.IsCoupon when '0' then '否' else '是' end) IsCouponName," +
                "(case dis.IsKeep when '0' then '否' else '是' end) IsKeepName,(case dis.IsCombo when '0' then '否' else '是' end) IsComboName,'' as StoName,'' as CookerCodeName,'' as MatCodeName,'' as WarCodeName," +
                "s.DiscountPrice as SecPrice" +
                ",(case when isnull(s.DiscountPrice,'0')='0' then '0' else '1' end) as IsSecPrice"
                , pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 门店后台专用
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="opencode"></param>
        /// <param name="recnums"></param>
        /// <param name="pagenums"></param>
        /// <returns></returns>
        public DataTable GetWebPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, string opencode, out int recnums, out int pagenums)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                recnums = -1;
                pagenums = -1;
                return dtBase;
            }
            return new bllPaging().GetPagingInfo(
                "TB_Dish dis " +
                " left join [dbo].[TB_DishType] dist on dis.typecode=dist.PKCode and dis.stocode=dist.stocode " +
                " left join [dbo].[TB_DishMenu] dism on dis.MenuCode=dism.PKCode  and dism.Stocode=dis.stocode " +
                " left join [dbo].[TB_Kitchen] kit on dis.KitCode=kit.PKCode and kit.Stocode=dis.stocode " +
                " left join TR_SpecialOfferDish s on s.Discode=dis.DisCode and s.Stocode=dis.Stocode and s.SpeCode in(select PKCode from TB_SpecialOffer where StoCode=s.StoCode and TStatus='1'  " +
                " and charindex(convert(char(1), Datepart(weekday, getdate() + @@DateFirst - 1)), Week) > 0 " +
                " and convert(varchar(10), getdate(), 23) between  convert(varchar(10), StartTime, 23) and convert(varchar(10), EndTime, 23) " +
                " and convert(varchar(8), getdate(), 8) between convert(varchar(8), StartTime, 8) and convert(varchar(8), EndTime, 8)) "
                ,
                "dis.DisCode",
                "dbo.fn_GetDishSellout(dis.DisCode,dis.StoCode) as  selloutnum," +
                "dbo.fn_GetOrderedDisNum(dis.DisCode,dis.StoCode,'" + opencode + "') as  orderednum," +
                "dis.*,(case dis.TStatus when '0' then '无效' else '有效' end) as TStatusName,'' as ChannelCodeListName," +
                "dist.TypeName,dism.MenuName,kit.KitName,(case dis.IsCount when '0' then '否' else '是' end) IsCountName,(case dis.IsVarPrice when '0' then '否' else '是' end) IsVarPriceName," +
                "(case dis.IsWeight when '0' then '否' else '是' end) IsWeightName,(case dis.IsMethod when '0' then '否' else '是' end) IsMethodName,(case dis.IsStock when '0' then '否' else '是' end) IsStockName," +
                "(case dis.IsPoint when '0' then '否' else '是' end) IsPointName,(case dis.IsMemPrice when '0' then '否' else '是' end) IsMemPriceName,(case dis.IsCoupon when '0' then '否' else '是' end) IsCouponName," +
                "(case dis.IsKeep when '0' then '否' else '是' end) IsKeepName,(case dis.IsCombo when '0' then '否' else '是' end) IsComboName,'' as StoName,'' as CookerCodeName,'' as MatCodeName,'' as WarCodeName," +
                "s.DiscountPrice as SecPrice" +
                ",(case when isnull(s.DiscountPrice,'0')='0' then '0' else '1' end) as IsSecPrice"
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
			Entity.Id = Helper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.UCode = dr["UCode"].ToString();
			Entity.UCname = dr["UCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.ChannelCodeList = dr["ChannelCodeList"].ToString();
			Entity.DisCode = dr["DisCode"].ToString();
			Entity.DisName = dr["DisName"].ToString();
			Entity.OtherName = dr["OtherName"].ToString();
			Entity.TypeCode = dr["TypeCode"].ToString();
			Entity.QuickCode = dr["QuickCode"].ToString();
			Entity.CusDisCode = dr["CusDisCode"].ToString();
			Entity.Unit = dr["Unit"].ToString();
			Entity.Price = Helper.StringToDecimal(dr["Price"].ToString());
			Entity.MenuCode = dr["MenuCode"].ToString();
			Entity.MemPrice = Helper.StringToDecimal(dr["MemPrice"].ToString());
			Entity.CostPrice = Helper.StringToDecimal(dr["CostPrice"].ToString());
			Entity.RoyMoney = Helper.StringToDecimal(dr["RoyMoney"].ToString());
			Entity.ExtCode = dr["ExtCode"].ToString();
			Entity.FinCode = dr["FinCode"].ToString();
			Entity.KitCode = dr["KitCode"].ToString();
			Entity.CookerCode = dr["CookerCode"].ToString();
			Entity.MakeTime = Helper.StringToInt(dr["MakeTime"].ToString());
			Entity.QRCode = dr["QRCode"].ToString();
			Entity.WarCode = dr["WarCode"].ToString();
			Entity.MatCode = dr["MatCode"].ToString();
			Entity.Descript = dr["Descript"].ToString();
			Entity.IsCount = dr["IsCount"].ToString();
			Entity.DefCount = Helper.StringToInt(dr["DefCount"].ToString());
			Entity.CountPrice = Helper.StringToDecimal(dr["CountPrice"].ToString());
			Entity.IsVarPrice = dr["IsVarPrice"].ToString();
			Entity.IsWeight = dr["IsWeight"].ToString();
			Entity.IsMethod = dr["IsMethod"].ToString();
			Entity.IsStock = dr["IsStock"].ToString();
			Entity.IsPoint = dr["IsPoint"].ToString();
			Entity.IsMemPrice = dr["IsMemPrice"].ToString();
			Entity.IsCoupon = dr["IsCoupon"].ToString();
			Entity.IsKeep = dr["IsKeep"].ToString();
			Entity.IsCombo = dr["IsCombo"].ToString();
            return Entity;
        }

        /// <summary>
        /// 获取指定菜品的做法加价信息
        /// </summary>
        /// <param name="disCode"></param>
        /// <returns></returns>
        public DataTable GetDisMethods(string busCode,string stoCode,string disCode)
        {
            return dal.GetDisMethods(busCode, stoCode, disCode);
        }

        /// <summary>
        /// 获取指定套餐的套餐内菜品信息
        /// </summary>
        /// <param name="disCode"></param>
        /// <returns></returns>
        public DataTable GetDisForCombo(string busCode, string stoCode, string disCode)
        {
            return dal.GetDisForCombo(busCode, stoCode, disCode);
        }

        /// <summary>
        /// 获取指定套餐的套餐组别信息
        /// </summary>
        /// <param name="disCode"></param>
        /// <returns></returns>
        public DataTable GetDisForComboInfo(string busCode, string stoCode, string disCode)
        {
            return dal.GetDisForComboInfo(busCode, stoCode, disCode);
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

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <returns></returns>
        public DataTable GetComboTypeToDish(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                recnums = -1;
                pagenums = -1;
                return dtBase;
            }
            return new bllPaging().GetPagingInfo("TB_Dish dis inner join TB_DishMenu dt on dis.MenuCode=dt.pkcode and dt.StoCode=dis.StoCode", "dis.DisCode", "dis.DisCode,dis.MenuCode,dis.DisName+'('+dis.Unit+')' as DisName,'￥'+CONVERT(varchar(12),dis.Price) as Price,dis.Price as YPrice,[dbo].[fn_GetStoDisMenuName](dis.MenuCode,dis.StoCode) as MenuName,dis.MemPrice,dt.MenuName as TypeName", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteSql(string sql)
        {
            int intResult = 0;
            bllPaging objbllPaging = new bllPaging();
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine(sql);//开始事务
            try
            {
                intResult = objbllPaging.ExecuteNonQueryBySQL(Builder.ToString());
                intResult = 1;
            }
            catch (Exception ex)
            {
                intResult = 0;
            }
            finally
            {
                objbllPaging = null;
            }
            return intResult;
        }

    }
}