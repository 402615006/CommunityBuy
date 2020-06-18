using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
using System.Data.SqlClient;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 账单业务类
    /// </summary>
    public class bllTB_Bill : bllBase
    {
        DAL.dalTB_Bill dal = new DAL.dalTB_Bill();
        TB_BillEntity Entity = new TB_BillEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string OrderCodeList, string PKCode, string BillMoney, string PayMoney, string ZeroCutMoney, string ShiftCode, string Remar, string FTime, string OpenDate, string DiscountName, string DiscountMoney, string AUCode, string AUName, string PointMoney, string VirMoney, string BillType, string PayWay, string CStatus)
        {
            bool rel = false;
            try
            {
                Entity = new TB_BillEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;

                Entity.TStatus = TStatus;
                Entity.OrderCodeList = OrderCodeList;
                Entity.PKCode = PKCode;
                Entity.BillMoney = StringHelper.StringToDecimal(BillMoney);
                Entity.PayMoney = StringHelper.StringToDecimal(PayMoney);
                Entity.ZeroCutMoney = StringHelper.StringToDecimal(ZeroCutMoney);
                Entity.ShiftCode = ShiftCode;
                Entity.Remar = Remar;
                Entity.FTime = StringHelper.StringToDateTime(FTime);
                Entity.OpenDate = StringHelper.StringToDateTime(OpenDate);
                Entity.DiscountName = DiscountName;
                Entity.DiscountMoney = StringHelper.StringToDecimal(DiscountMoney);
                Entity.AUCode = StringHelper.StringToDecimal(AUCode);
                Entity.AUName = AUName;
                Entity.PointMoney = StringHelper.StringToDecimal(PointMoney);
                Entity.VirMoney = StringHelper.StringToDecimal(VirMoney);
                Entity.BillType = BillType;
                Entity.PayWay = PayWay;
                Entity.CStatus = CStatus;
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
        public void Add(string GUID, string UID, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string OrderCodeList, string PKCode, string BillMoney, string PayMoney, string ZeroCutMoney, string ShiftCode, string Remar, string FTime, string OpenDate, string DiscountName, string DiscountMoney, string AUCode, string AUName, string PointMoney, string VirMoney, string BillType, string PayWay, string CStatus)
        {
            int result = 0;
            bool validatePar = CheckPageInfo("add", Id, BusCode, StoCode, CCode, CCname, TStatus, OrderCodeList, PKCode, BillMoney, PayMoney, ZeroCutMoney, ShiftCode, Remar, FTime, OpenDate, DiscountName, DiscountMoney, AUCode, AUName, PointMoney, VirMoney, BillType, PayWay, CStatus);
            //数据页面验证
            if (!validatePar)
            {
                result = -2;
            }
            else
            { 
                result = dal.Add(ref Entity);
            }
            //检测执行结果
            CheckResult(result,Entity.PKCode);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_BillEntity UEntity)
        {
            //更新数据
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result,"");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string GUID, string UID, string pkcode)
        {
            string msg = "";
            int result = dal.Delete(pkcode,ref msg);
            //检测执行结果
            CheckResult(result, msg);
        }

        /// <summary>
        /// 完成收银
        /// </summary>
        /// <param name="BillCode">账单号</param>
        /// <returns>返回操作结果</returns>
        public DataSet Finish(string GUID, string UID, string BillCode, string StoCode)
        {
            DataSet ds = dal.Finish(BillCode, StoCode);
            return ds;
        }

        /// <summary>
        /// 收银反结
        /// </summary>
        /// <param name="BillCode">账单号</param>
        /// <returns>返回操作结果</returns>
        public DataSet UnFinish(string GUID, string UID, string BillCode, string StoCode, out int rv)
        {
            rv = 1;
            string Mescode = string.Empty;
            DataSet ds = dal.UnFinish(BillCode, StoCode, out rv);
            return ds;
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
        /// 获取账单详情
        /// </summary>
        /// <param name="filter">指定条件</param>
        /// <returns>返回第一行</returns>
        public DataSet GetDetail(string GUID, string UID, string BillCode, string StoCode, string UserCode)
        {
            return dal.GetDetail(BillCode, StoCode, UserCode);
        }

        /// <summary>
        /// 获取会员卡账单详情
        /// </summary>
        /// <param name="filter">指定条件</param>
        /// <returns>返回第一行</returns>
        public DataSet GetMemDetail(string GUID, string UID, string BillCode, string StoCode)
        {
            return dal.GetMemDetail(BillCode, StoCode);
        }

        /// <summary>
        /// 获取单条数据实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TB_BillEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_BillEntity();
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
            return new bllPaging().GetPagingInfoTime("TB_Bill b", "Id",
                "*,ftime as checktime,CCName as creater," +
                "dbo.fn_GetBillTableName(PKCode, Stocode) as TableName," +
                "dbo.fn_GetBillCouponMoney(PKCode, Stocode) as CouponMoney," +
                "(dbo.fn_GetBillCouponMoney(PKCode, Stocode)+paymoney) as totalpay," +
                "(BillMoney - PayMoney-DiscountMoney - ZeroCutMoney - dbo.fn_GetBillCouponMoney(PKCode, Stocode)) as ToPayMoney," +
                 "(select sum((dbo.fn_GetBillCouponMoney(PKCode, Stocode)+paymoney)) from TB_Bill " + filter + ") as allmoney," +
                "dbo.fn_GetBillOrderTime(PKCode, Stocode) as ordertime", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_BillEntity SetEntityInfo(DataRow dr)
        {
            TB_BillEntity Entity = new TB_BillEntity();
            Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
            Entity.BusCode = dr["BusCode"].ToString();
            Entity.StoCode = dr["StoCode"].ToString();
            Entity.CCode = dr["CCode"].ToString();
            Entity.CCname = dr["CCname"].ToString();

            Entity.TStatus = dr["TStatus"].ToString();
            Entity.OrderCodeList = dr["OrderCodeList"].ToString();
            Entity.PKCode = dr["PKCode"].ToString();
            Entity.BillMoney = StringHelper.StringToDecimal(dr["BillMoney"].ToString());
            Entity.PayMoney = StringHelper.StringToDecimal(dr["PayMoney"].ToString());
            Entity.ZeroCutMoney = StringHelper.StringToDecimal(dr["ZeroCutMoney"].ToString());
            Entity.ShiftCode = dr["ShiftCode"].ToString();
            Entity.Remar = dr["Remar"].ToString();
            Entity.FTime = StringHelper.StringToDateTime(dr["FTime"].ToString());
            Entity.OpenDate = StringHelper.StringToDateTime(dr["OpenDate"].ToString());
            Entity.DiscountName = dr["DiscountName"].ToString();
            Entity.DiscountMoney = StringHelper.StringToDecimal(dr["DiscountMoney"].ToString());
            Entity.AUCode = StringHelper.StringToDecimal(dr["AUCode"].ToString());
            Entity.AUName = dr["AUName"].ToString();
            Entity.PointMoney = StringHelper.StringToDecimal(dr["PointMoney"].ToString());
            Entity.VirMoney = StringHelper.StringToDecimal(dr["VirMoney"].ToString());
            return Entity;
        }

        /// <summary>
        /// 获取获取未出库的账单菜品
        /// </summary>
        /// <param name="filter">指定条件</param>
        /// <returns>返回第一行</returns>
        public DataTable ClearUnuseBillByTable(string GUID, string UID, string OpenCode, string StoCode)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@stocode", StoCode),
                new SqlParameter("@opencode", OpenCode)
             };
            DataTable dtReturn = new bllPaging().GetDataTableInfoByProcedure("p_ClearTableUnuseBill", sqlParameters);
            return dtReturn;
        }

        /// <summary>
        /// 账单退款操作
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="BusCode"></param>
        /// <param name="StoCode"></param>
        /// <param name="BillCode"></param>
        /// <param name="CCode"></param>
        /// <param name="CCname"></param>
        /// <param name="mescode"></param>
        /// <returns></returns>
        public DataTable BillReturn(string GUID, string UID, string BusCode, string StoCode, string BillCode, string CCode, string CCname)
        {
            string mescode = "";
            DataTable dt = dal.BillReturn(BusCode, StoCode, BillCode, CCode, CCname, out mescode);
            return dt;
        }
    }
}