using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 订单业务类
    /// </summary>
    public class bllTB_Order : bllBase
    {
		DAL.dalTB_Order dal = new DAL.dalTB_Order();
        TB_OrderEntity Entity = new TB_OrderEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string PKCode, string OpenCodeList, string OrderMoney, string DisNum, string DisTypeNum, string Remar, string CheckTime, string BillCode,string OrderType,string DepartCode)
        {
            bool rel = false;
            try
            {
                Entity = new TB_OrderEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;

                Entity.TStatus = TStatus;
                Entity.PKCode = PKCode;
                Entity.OpenCodeList = OpenCodeList;
                Entity.OrderMoney = StringHelper.StringToDecimal(OrderMoney);
                Entity.DisNum = StringHelper.StringToDecimal(DisNum);
                Entity.DisTypeNum = StringHelper.StringToInt(DisTypeNum);
                Entity.Remar = Remar;
                Entity.CheckTime = StringHelper.StringToDateTime(CheckTime);
                Entity.BillCode = BillCode;
                Entity.OrderType = OrderType;
                Entity.DepartCode = DepartCode;
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
        public void Add(string OrderDishJson,string GUID, string UID,string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus,out string PKCode, string OpenCodeList, string OrderMoney, string DisNum, string DisTypeNum, string Remar, string CheckTime, string BillCode,string OrderType,string DepartCode)
        {
            PKCode = "0";
            int result = 0;
            bool strReturn = CheckPageInfo ("add",  Id, BusCode, StoCode, CCode, CCname, TStatus, PKCode, OpenCodeList, OrderMoney, DisNum, DisTypeNum, Remar, CheckTime, BillCode,OrderType,DepartCode);
            //数据页面验证
             result = dal.Add(ref Entity, OrderDishJson);
            //检测执行结果
            CheckResult(result, Entity.PKCode);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID,TB_OrderEntity UEntity)
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
        public void Delete(string GUID, string UID, string pkcode,string stocode)
        {
			string Mescode = string.Empty;
            int result = dal.Delete(pkcode,stocode, ref Mescode);
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
        public TB_OrderEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_OrderEntity();
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
            return new bllPaging().GetPagingInfo("TB_Order", "Id", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 分页方法获取未结账的账单
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <returns></returns>
        public DataTable GetUnFinishPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            return new bllPaging().GetPagingInfo("TB_Order o", "OpenCodeList", " OpenCodeList,[dbo].[fn_GetTableNameByOpenCode](OpenCodeList,StoCode) as tableName,min(CTime) as ctime,sum(OrderMoney) as ordermoney, STUFF((select ',' + PKCode from  TB_Order where OpenCodeList = o.OpenCodeList and BillCode=o.BillCode and StoCode=o.Stocode and TStatus<>'0'  for xml path('')),1,1,'') as ordercodelist, (select top 1 pkcode from tb_bill where pkcode=o.billcode and stocode=o.stocode and TStatus<>'5') as BillCode,(select top 1 CCode from tb_bill where pkcode=o.billcode and stocode=o.stocode and TStatus<>'5') as BCCode", pageSize, currentpage, filter, " OpenCodeList,StoCode,BillCode", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 分页方法-带订单菜品信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <returns></returns>
        public DataTable GetOrderDisPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            string tableName = "TB_Order o " +
                " left join TB_OrderDish d on d.OrderCode=o.PKCode and d.Stocode=o.Stocode " +
                " left join TB_Dish dis on dis.DisCode=d.DisCode and dis.StoCode=d.StoCode"+
                " left join TB_OpenTable ot on ot.PKCode=o.OpenCodeList and ot.Stocode=o.Stocode " +
                " left join TB_Table t on t.PKCode=ot.TableCode and ot.Stocode=t.Stocode";

            string columns = "o.PKCode as code" +
                ",o.Ctime as ctime" +
                ",o.OrderMoney as money" +
                ",o.Disnum as orderdisnum" +
                ",o.DisTypeNum as distypenum" +
                 ",o.TStatus as status" +
                 ",o.OrderType as ordertype" +
                 ",t.TableName as tablename" +
                 ",ot.CusNum as cusnum" +
                 ",ot.CTime as opentime" +
                 ",ot.CCname as openman" +
                ",dis.Discode as discode" +
                ",d.disname as disname" +
                ",d.price as price" +
                ",(d.disnum-d.returnnum) as disnum" +
                ",d.uptype as uptype" +
                ",d.totalmoney as totalmoney" +
                ",d.returnnum as returnnum" +
                ",d.DiscountType as DiscountType" +
                ",d.DiscountPrice as DiscountPrice" +
                ",d.MemPrice as MemPrice" +
                ",d.CookMoney as CookMoney" +
                ",d.FinCode as FinCode" +
                ",d.DisTypeCode as DisTypeCode" +
                ",d.CookName as CookName" +
                ",d.OrderCode as OrderCode" +
                ",dis.kitcode as kitcode" +
                ",dis.TypeCode as typecode" +
                ",dis.IsMemPrice as IsMemPrice" +
                ",dis.IsCoupon as IsCoupon" +
                ",d.ispackage as ispackage" +
                ",d.pdiscode as pdiscode" +
                ",d.PKCode as PKCode" +
                ",(d.ItemNum-d.returnnum) as ItemNum" +
                ",d.ItemPrice as ItemPrice" +
                ",d.IsMp as IsMp" +
                ",d.MpCheckCode as MpCheckCode" +
                ",d.cookmoney as extraMoney";

            return new bllPaging().GetPagingInfoTime(tableName, "o.id", columns, pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_OrderEntity SetEntityInfo(DataRow dr)
        {
            TB_OrderEntity Entity = new TB_OrderEntity();
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.PKCode = dr["PKCode"].ToString();
			Entity.OpenCodeList = dr["OpenCodeList"].ToString();
			Entity.OrderMoney = StringHelper.StringToDecimal(dr["OrderMoney"].ToString());
			Entity.DisNum = StringHelper.StringToDecimal(dr["DisNum"].ToString());
			Entity.DisTypeNum = StringHelper.StringToInt(dr["DisTypeNum"].ToString());
			Entity.Remar = dr["Remar"].ToString();
			Entity.CheckTime = StringHelper.StringToDateTime(dr["CheckTime"].ToString());
			Entity.BillCode = dr["BillCode"].ToString();
            return Entity;
        }
    }
}