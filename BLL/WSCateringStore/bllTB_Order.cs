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
        public string CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string PKCode, string OpenCodeList, string OrderMoney, string DisNum, string DisTypeNum, string Remar, string CheckTime, string BillCode,string OrderType,string DepartCode)
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
            CheckValue<TB_OrderEntity>(EName, EValue, ref errorCode, new TB_OrderEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new TB_OrderEntity();
				Entity.Id = Helper.StringToLong(Id);
				Entity.BusCode = BusCode;
				Entity.StoCode = StoCode;
				Entity.CCode = CCode;
				Entity.CCname = CCname;
				
				Entity.TStatus = TStatus;
				Entity.PKCode = PKCode;
				Entity.OpenCodeList = OpenCodeList;
				Entity.OrderMoney = Helper.StringToDecimal(OrderMoney);
				Entity.DisNum = Helper.StringToDecimal(DisNum);
				Entity.DisTypeNum = Helper.StringToInt(DisTypeNum);
				Entity.Remar = Remar;
				Entity.CheckTime = Helper.StringToDateTime(CheckTime);
				Entity.BillCode = BillCode;
                Entity.OrderType = OrderType;
                Entity.DepartCode = DepartCode;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string OrderDishJson,string GUID, string UID,string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus,out string PKCode, string OpenCodeList, string OrderMoney, string DisNum, string DisTypeNum, string Remar, string CheckTime, string BillCode,string OrderType,string DepartCode, operatelogEntity entity)
        {
            PKCode = "0";
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo ("add",  Id, BusCode, StoCode, CCode, CCname, TStatus, PKCode, OpenCodeList, OrderMoney, DisNum, DisTypeNum, Remar, CheckTime, BillCode,OrderType,DepartCode);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
            int result = dal.Add(ref Entity, OrderDishJson);
            //检测执行结果
            DataRow dr = dtBase.NewRow();
            dr["type"] = result;
            switch (result)
            {
                case 0:
                    PKCode = Entity.PKCode;
                    dr["mes"] = PKCode;
                    break;
                case 1:
                    dr["mes"] = "开台失败";
                    break;
                case 2:
                    dr["mes"] = "当前是非营业时间，无法点餐";
                    break;
                case 3:
                    dr["mes"] = "请联系服务员开台，继续点餐";
                    break;
                case 4:
                    dr["mes"] = "该桌台有未完成的账单，无法点餐";
                    break;
                case 5:
                    dr["mes"] = "菜品"+Entity.Remar+"已估清,无法下单";
                    break;
                case 6:
                    dr["mes"] = "订单金额有变化,请重新下单";
                    break;
            }
            dtBase.Rows.Add(dr);
            dtBase.AcceptChanges();
            return dtBase;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public DataTable Update(string GUID, string UID,  string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string PKCode, string OpenCodeList, string OrderMoney, string DisNum, string DisTypeNum, string Remar, string CheckTime, string BillCode,string OrderType,string DepartCode, operatelogEntity entity)
        {
			
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update",  Id, BusCode, StoCode, CCode, CCname, TStatus, PKCode, OpenCodeList, OrderMoney, DisNum, DisTypeNum, Remar, CheckTime, BillCode, OrderType,DepartCode);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
			//获取更新前的数据对象
            TB_OrderEntity OldEntity = new TB_OrderEntity();
            OldEntity = GetEntitySigInfo(" where Id= '" + Id + "'");
			//更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<TB_OrderEntity>(entity, Entity, OldEntity);
                }
            }
            return dtBase;
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataTable UpdateStatus(string GUID, string UID, string ids, string Status,string stocode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateStatus(ids, Status, stocode);
            //检测执行结果
			CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public DataTable Delete(string GUID, string UID, string pkcode,string stocode, operatelogEntity entity)
        {
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
			string Mescode = string.Empty;
            int result = dal.Delete(pkcode,stocode, ref Mescode);
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
			if (!CheckLogin(GUID, UID))//非法登录
            {
                recnums = -1;
                pagenums = -1;
                return dtBase;
            }
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
            if (!CheckLogin(GUID, UID))//非法登录
            {
                recnums = -1;
                pagenums = -1;
                return dtBase;
            }
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
            if (!CheckLogin(GUID, UID))//非法登录
            {
                recnums = -1;
                pagenums = -1;
                return dtBase;
            }
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
			Entity.Id = Helper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.PKCode = dr["PKCode"].ToString();
			Entity.OpenCodeList = dr["OpenCodeList"].ToString();
			Entity.OrderMoney = Helper.StringToDecimal(dr["OrderMoney"].ToString());
			Entity.DisNum = Helper.StringToDecimal(dr["DisNum"].ToString());
			Entity.DisTypeNum = Helper.StringToInt(dr["DisTypeNum"].ToString());
			Entity.Remar = dr["Remar"].ToString();
			Entity.CheckTime = Helper.StringToDateTime(dr["CheckTime"].ToString());
			Entity.BillCode = dr["BillCode"].ToString();
            return Entity;
        }
    }
}