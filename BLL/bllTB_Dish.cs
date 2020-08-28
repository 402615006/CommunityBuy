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
        public bool CheckPageInfo(string type, string Images, string StoCode, string TStatus, string DisName, string OtherName, string TypeCode, string QuickCode, string Unit, string Price, string CostPrice, string Descript)
        {
            bool rel = false;
            try
            {
                Entity = new TB_DishEntity();
                Entity.Id = 0;
                Entity.ImageName = Images;
                Entity.StoCode = StoCode;
                Entity.TStatus = TStatus;
                Entity.DisName = DisName;
                Entity.OtherName = OtherName;
                Entity.TypeCode = TypeCode;
                Entity.QuickCode = QuickCode;
                Entity.Unit = Unit;
                Entity.Price = StringHelper.StringToDecimal(Price);
                Entity.CostPrice = StringHelper.StringToDecimal(CostPrice);
                Entity.Descript = Descript;
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
        public void Add(string GUID, string UID, string Images, string StoCode, string TStatus, string DisName, string OtherName, string TypeCode, string QuickCode, string Unit, string Price, string CostPrice, string Descript)
        {
            int result = 0;
            bool strReturn = CheckPageInfo("add", Images, StoCode, TStatus, DisName, OtherName, TypeCode, QuickCode, Unit, Price, CostPrice, Descript);
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
        /// 更新状态
        /// </summary>
        /// <param name="DisCode">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public void UpdateStatus(string GUID, string UID, string ids, string Status)
        {
            int result = dal.UpdateStatus(ids, Status);
            //检测执行结果
            CheckResult(result,"");
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
        /// 获取单条数据实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TB_DishEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
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
        public DataTable GetPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            return new bllPaging().GetPagingInfo(
                "TB_Dish dis " +
                " left join [dbo].[TB_DishType] dist on dis.typecode=dist.PKCode " 
                ,"dis.discode"
                ,"dis.*,dist.PKKCode as pdistypecode," +
                "dist.TypeName"+
                ",(case dis.tstatus when '1' then '有效' else '无效' end) as statusname"+
                ",[dbo].[fnGetDisheImages](dis.DisCode) as images"+
                ",1 as IsMethod" 
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
			//Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.TStatus = dr["TStatus"].ToString();
			//Entity.ChannelCodeList = dr["ChannelCodeList"].ToString();
			Entity.DisCode = dr["DisCode"].ToString();
			Entity.DisName = dr["DisName"].ToString();
			Entity.OtherName = dr["OtherName"].ToString();
			Entity.TypeCode = dr["TypeCode"].ToString();
			Entity.QuickCode = dr["QuickCode"].ToString();
			Entity.Unit = dr["Unit"].ToString();
			Entity.Price = StringHelper.StringToDecimal(dr["Price"].ToString());
			Entity.QRCode = dr["QRCode"].ToString();
			Entity.Descript = dr["Descript"].ToString();
            Entity.CostPrice = StringHelper.StringToDecimal(dr["CostPrice"].ToString());
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