using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 账单优惠券业务类
    /// </summary>
    public class bllTB_BillCoupon : bllBase
    {
		DAL.dalTB_BillCoupon dal = new DAL.dalTB_BillCoupon();
        TB_BillCouponEntity Entity = new TB_BillCouponEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string BillCode, string CouponCode, string CouponMoney, string MemberCardCode, string RealPay, string VIMoney, string Remark, string UseType, string ShiftCode, string CouponName,string McCode,string TicType,string TicWay)
        {
            bool rel = false;

            try
            {
                Entity = new TB_BillCouponEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;
                Entity.TStatus = TStatus;
                Entity.BillCode = BillCode;
                Entity.CouponCode = CouponCode;
                Entity.CouponMoney = StringHelper.StringToDecimal(CouponMoney);
                Entity.MemberCardCode = MemberCardCode;
                Entity.RealPay = StringHelper.StringToDecimal(RealPay);
                Entity.VIMoney = StringHelper.StringToDecimal(VIMoney);
                Entity.Remark = Remark;
                Entity.UseType = UseType;
                Entity.ShiftCode = ShiftCode;
                Entity.CouponName = CouponName;
                Entity.McCode = McCode;
                Entity.TicType = TicType;
                Entity.TicWay = TicWay;
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
        public DataTable Add(string GUID, string UID, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string BillCode, string CouponCode, string CouponMoney, string MemberCardCode, string RealPay, string VIMoney, string Remark, string UseType, string ShiftCode, string CouponName,string McCode,string TicType,string TicWay)
        {
            int reuslt = 0;
            string Mescode = "";
            bool strReturn = CheckPageInfo("add", Id, BusCode, StoCode, CCode, CCname, TStatus, BillCode, CouponCode, CouponMoney, MemberCardCode, RealPay, VIMoney, Remark, UseType, ShiftCode, CouponName,McCode,TicType,TicWay);
            if (!strReturn)
            {
                CheckResult(-2, "");
                return null;
            }
            //数据页面验证
            DataTable dt = dal.Add(ref Entity, ref Mescode);
            return dt;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_BillCouponEntity UEntity)
        {
			//更新数据
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result,"");
        }


        /// <summary>
        /// 取消优惠券
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataSet Cancel(string GUID, string UID, string StoCode, string BillCode,string CouponCode, string isget)
        {
            DataSet result = dal.CancelCoupon(StoCode, BillCode,CouponCode , isget);
            return result;
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
        public TB_BillCouponEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_BillCouponEntity();
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
            return new bllPaging().GetPagingInfo("TB_BillCoupon", "Id", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_BillCouponEntity SetEntityInfo(DataRow dr)
        {
            TB_BillCouponEntity Entity = new TB_BillCouponEntity();
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.BillCode = dr["BillCode"].ToString();
			Entity.CouponCode = dr["CouponCode"].ToString();
			Entity.CouponMoney = StringHelper.StringToDecimal(dr["CouponMoney"].ToString());
			Entity.MemberCardCode = dr["MemberCardCode"].ToString();
			Entity.RealPay = StringHelper.StringToDecimal(dr["RealPay"].ToString());
			Entity.VIMoney = StringHelper.StringToDecimal(dr["VIMoney"].ToString());
			Entity.Remark = dr["Remark"].ToString();
			Entity.UseType = dr["UseType"].ToString();
			Entity.ShiftCode = dr["ShiftCode"].ToString();
            Entity.CouponName = dr["couponname"].ToString();
            Entity.TicType = dr["tictype"].ToString();
            Entity.TicWay = dr["ticway"].ToString();
            Entity.McCode= dr["McCode"].ToString();
            return Entity;
        }

        /// <summary>
        /// 根据账单号获取账单优惠券信息
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="StoCode"></param>
        /// <param name="BillCode"></param>
        /// <returns></returns>
        public DataTable GetCouponInfoByBillCode(string GUID, string UID, string StoCode, string BillCode,int page,int pagesize,out int recnums,out int pagenums)
        {
            recnums = 0;
            pagenums = 0;
            string tablename = "TB_BillCoupon";
            string columns = "*";
            string filters =string.Format("TStatus='1' and StoCode='{0}' and BillCode='{1}'", StoCode, BillCode);
            return new bllPaging().GetPagingInfo(tablename, "id", columns, pagesize, page, filters, "", "", out recnums, out pagenums);
        }
    }
}