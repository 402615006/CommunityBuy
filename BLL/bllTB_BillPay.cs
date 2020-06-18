using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 账单支付业务类
    /// </summary>
    public class bllTB_BillPay : bllBase
    {
		DAL.dalTB_BillPay dal = new DAL.dalTB_BillPay();
        TB_BillPayEntity Entity = new TB_BillPayEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string PKCode, string BillCode, string PayMoney, string PayMethodName, string PayMethodCode, string Remar, string OutOrderCode, string PPKCode)
        {
            bool rel = false;
            try
            {
                Entity = new TB_BillPayEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;

                Entity.TStatus = TStatus;
                Entity.PKCode = PKCode;
                Entity.BillCode = BillCode;
                Entity.PayMoney = StringHelper.StringToDecimal(PayMoney);
                Entity.PayMethodName = PayMethodName;
                Entity.PayMethodCode = PayMethodCode;
                Entity.Remar = Remar;
                Entity.OutOrderCode = OutOrderCode;
                Entity.PPKCode = PPKCode;
                rel = true;
            }
            catch (System.Exception)
            {
                throw;
            }
            return rel;

        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(string GUID, string UID,  string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string PKCode, string BillCode, string PayMoney, string PayMethodName, string PayMethodCode, string Remar, string OutOrderCode, string PPKCode,string DiscountName,string DiscountMoney,string ZeroCutMoney,string AuthCode,string AuthName,string MemberCard, string MemberName,string  MemberBalance,string  MemberLeve,string  MemberDiscount)
        {
            int result = 0;
            bool strReturn = CheckPageInfo("add",  Id, BusCode, StoCode, CCode, CCname, TStatus, PKCode, BillCode, PayMoney, PayMethodName, PayMethodCode, Remar, OutOrderCode, PPKCode);
            if (!strReturn)
            {
                result = -2;
                CheckResult(result, "");
            }
            result = dal.Add(ref Entity,DiscountName,DiscountMoney,ZeroCutMoney,AuthCode,AuthName,MemberCard,MemberName,MemberBalance,MemberLeve,MemberDiscount);
            CheckResult(result, Entity.PKCode);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_BillPayEntity UEntity )
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
        public void Delete(string GUID, string UID, string PKCode,string StoCode)
        {
			string Mescode = string.Empty;
            int result = dal.Delete(PKCode,StoCode, ref Mescode);
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
        public TB_BillPayEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_BillPayEntity();
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

            return new bllPaging().GetPagingInfoTime("TB_BillPay", "Id", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_BillPayEntity SetEntityInfo(DataRow dr)
        {
            TB_BillPayEntity Entity = new TB_BillPayEntity();
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.PKCode = dr["PKCode"].ToString();
			Entity.BillCode = dr["BillCode"].ToString();
			Entity.PayMoney = StringHelper.StringToDecimal(dr["PayMoney"].ToString());
			Entity.PayMethodName = dr["PayMethodName"].ToString();
			Entity.PayMethodCode = dr["PayMethodCode"].ToString();
			Entity.Remar = dr["Remar"].ToString();
			Entity.OutOrderCode = dr["OutOrderCode"].ToString();
			Entity.PPKCode = dr["PPKCode"].ToString();
            return Entity;
        }

        /// <summary>
        /// 反结算
        /// </summary>
        /// (GUID, USER_ID, BusCode, StoCode, CCode, CCname, TStatus, out PKCode, PayMoney, Remar, PPKCode, logentity
        public void Back(string GUID, string UID, string BusCode, string StoCode, string CCode, string CCname, string TStatus,out string PKCode, string PayMoney,  string Remar,out string OutOrderCode, string PPKCode,out string PayMethodCode)
        {
            int result = 0;
            PKCode = "0";
            OutOrderCode = "0";
            PayMethodCode = "0";

            string spanids = string.Empty;
            bool strReturn = CheckPageInfo("add", "0", BusCode, StoCode, CCode, CCname, TStatus, PKCode, "0", PayMoney, "0", "0", Remar, OutOrderCode, PPKCode);
            if (!strReturn)
            {
                result = -2;
                CheckResult(result, "");
                return;
            }

            result =dal.Back(ref Entity);
            PKCode = Entity.PKCode;
            OutOrderCode = Entity.OutOrderCode;
            PayMethodCode = Entity.PayMethodCode;
        }

    }
}