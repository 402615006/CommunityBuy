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
        public string CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string OrderCodeList, string PKCode, string BillMoney, string PayMoney, string ZeroCutMoney, string ShiftCode, string Remar, string FTime, string OpenDate, string DiscountName, string DiscountMoney, string AUCode, string AUName, string PointMoney, string VirMoney, string BillType, string PayWay, string CStatus)
        {
            string strRetuen = string.Empty;
            //要验证的实体属性
            List<string> EName = new List<string>() { };
            //要验证的实体属性值
            List<string> EValue = new List<string>() { };
            //错误信息
            List<string> errorCode = new List<string>();
            List<string> ControlName = new List<string>();
            //验证数据
            CheckValue<TB_BillEntity>(EName, EValue, ref errorCode, new TB_BillEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new TB_BillEntity();
                Entity.Id = Helper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;

                Entity.TStatus = TStatus;
                Entity.OrderCodeList = OrderCodeList;
                Entity.PKCode = PKCode;
                Entity.BillMoney = Helper.StringToDecimal(BillMoney);
                Entity.PayMoney = Helper.StringToDecimal(PayMoney);
                Entity.ZeroCutMoney = Helper.StringToDecimal(ZeroCutMoney);
                Entity.ShiftCode = ShiftCode;
                Entity.Remar = Remar;
                Entity.FTime = Helper.StringToDateTime(FTime);
                Entity.OpenDate = Helper.StringToDateTime(OpenDate);
                Entity.DiscountName = DiscountName;
                Entity.DiscountMoney = Helper.StringToDecimal(DiscountMoney);
                Entity.AUCode = Helper.StringToDecimal(AUCode);
                Entity.AUName = AUName;
                Entity.PointMoney = Helper.StringToDecimal(PointMoney);
                Entity.VirMoney = Helper.StringToDecimal(VirMoney);
                Entity.BillType = BillType;
                Entity.PayWay = PayWay;
                Entity.CStatus = CStatus;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID, out  string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string OrderCodeList, string PKCode, string BillMoney, string PayMoney, string ZeroCutMoney, string ShiftCode, string Remar, string FTime, string OpenDate, string DiscountName, string DiscountMoney, string AUCode, string AUName, string PointMoney, string VirMoney, string BillType, string PayWay, string CStatus, operatelogEntity entity)
        {
            Id = "0";
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add", Id, BusCode, StoCode, CCode, CCname, TStatus, OrderCodeList, PKCode, BillMoney, PayMoney, ZeroCutMoney, ShiftCode, Remar, FTime, OpenDate, DiscountName, DiscountMoney, AUCode, AUName, PointMoney, VirMoney, BillType, PayWay, CStatus);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
            int result = dal.Add(ref Entity);
            //检测执行结果
            DataRow dr = dtBase.NewRow();
            dr["type"] = result;
            switch (result)
            {
                case 0:
                    dr["mes"] = Entity.PKCode;
                    break;
                case 1:
                    dr["mes"] = "订单状态不正确";
                    break;
                case 2:
                    dr["mes"] = "账单已存在";
                    break;
                case 3:
                    dr["mes"] = "没有可结账订单";
                    break;
            }
            dtBase.Rows.Add(dr);
            dtBase.AcceptChanges();
            return dtBase;
        }

        /// <summary>
        /// 额外增加一个账单
        /// </summary>
        public DataTable AddOrder(string GUID, string UID,string OrderCodeList, string BusCode, string StoCode,string BillCode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.AddOrder(BillCode, OrderCodeList, StoCode);
            //检测执行结果
            DataRow dr = dtBase.NewRow();
            dr["type"] = result;
            dr["mes"] = "";
            dtBase.Rows.Add(dr);
            dtBase.AcceptChanges();
            return dtBase;
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public DataTable Update(string GUID, string UID, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string OrderCodeList, string PKCode, string BillMoney, string PayMoney, string ZeroCutMoney, string ShiftCode, string Remar, string FTime, string OpenDate, string DiscountName, string DiscountMoney, string AUCode, string AUName, string PointMoney, string VirMoney, string BillType, string PayWay, string CStatus, operatelogEntity entity)
        {

            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update", Id, BusCode, StoCode, CCode, CCname, TStatus, OrderCodeList, PKCode, BillMoney, PayMoney, ZeroCutMoney, ShiftCode, Remar, FTime, OpenDate, DiscountName, DiscountMoney, AUCode, AUName, PointMoney, VirMoney, BillType, PayWay, CStatus);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
            //获取更新前的数据对象
            TB_BillEntity OldEntity = new TB_BillEntity();
            OldEntity = GetEntitySigInfo(" where Id='" + Id + "'");
            //更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<TB_BillEntity>(entity, Entity, OldEntity);
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
        public DataTable UpdateStatus(string GUID, string UID, string billCode, string stoCode, string Status)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateStatus(billCode, stoCode, Status);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 更新赠送优惠券信息
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataTable UpdateGiveCoupons(string GUID, string UID, string billCode, string stoCode, string coupons)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateGiveCoupon(billCode, stoCode, coupons);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 更新库存状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataTable UpdateStockStatus(string GUID, string UID, string billCode, string stoCode, string status)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateStockStatus(billCode, stoCode, status);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 更新状态取餐状态
        /// </summary>
        /// <param name="billCode"></param>
        /// <param name="stoCode"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public DataTable UpdateCStatus(string GUID, string UID, string billCode, string stoCode, string CStatus, string ccode, string ccname)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateCStatus(billCode, stoCode, CStatus, ccode, ccname);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public DataTable Delete(string GUID, string UID, string Id, operatelogEntity entity)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string Mescode = string.Empty;
            int result = dal.Delete(Id, ref Mescode);
            //检测执行结果
            if (CheckDeleteResult(result, Mescode))
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
        /// 完成收银
        /// </summary>
        /// <param name="BillCode">账单号</param>
        /// <returns>返回操作结果</returns>
        public DataSet Finish(string GUID, string UID, string BillCode, string StoCode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return null;
            }
            dtBase.Clear();
            string Mescode = string.Empty;
            DataSet ds = dal.Finish(BillCode, StoCode);
            return ds;
        }

        /// <summary>
        /// 获取打印详情的数据
        /// </summary>
        /// <param name="BillCode">账单号</param>
        /// <returns>返回操作结果</returns>
        public DataSet PrintDetail(string GUID, string UID, string BillCode, string StoCode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return null;
            }
            dtBase.Clear();
            string Mescode = string.Empty;
            DataSet ds = dal.PrintDetail(BillCode, StoCode);
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
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return null;
            }
            dtBase.Clear();
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
        /// 获取订单下待出库的菜品
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="BillCode"></param>
        /// <param name="StoCode"></param>
        /// <returns></returns>
        public DataTable GetOutStockDishByBillCode(string GUID, string UID, string BillCode, string StoCode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return null;
            }
            //菜品可以出库，有matcode，点单数量>0,该菜品没有出库过
            string Sql = "select sum(A.DisNum-A.ReturnNum) as DisNum,B.MatCode,B.WarCode  from TB_OrderDish A inner join TB_Dish B on A.discode=B.discode and A.StoCode=B.Stocode where A.StoCode='" + StoCode + "' and len(isnull(B.MatCode,''))>0 and B.IsStock='1' and (DisNum-ReturnNum)>0 and OrderCode in (select PKCode from TB_Order where BillCode='" + BillCode + "' and StoCode='" + StoCode + "') group by B.MatCode,B.WarCode";
            return new bllPaging().GetDataTableInfoBySQL(Sql);
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
            if (!CheckLogin(GUID, UID))//非法登录
            {
                recnums = -1;
                pagenums = -1;
                return dtBase;
            }
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
            Entity.Id = Helper.StringToLong(dr["Id"].ToString());
            Entity.BusCode = dr["BusCode"].ToString();
            Entity.StoCode = dr["StoCode"].ToString();
            Entity.CCode = dr["CCode"].ToString();
            Entity.CCname = dr["CCname"].ToString();

            Entity.TStatus = dr["TStatus"].ToString();
            Entity.OrderCodeList = dr["OrderCodeList"].ToString();
            Entity.PKCode = dr["PKCode"].ToString();
            Entity.BillMoney = Helper.StringToDecimal(dr["BillMoney"].ToString());
            Entity.PayMoney = Helper.StringToDecimal(dr["PayMoney"].ToString());
            Entity.ZeroCutMoney = Helper.StringToDecimal(dr["ZeroCutMoney"].ToString());
            Entity.ShiftCode = dr["ShiftCode"].ToString();
            Entity.Remar = dr["Remar"].ToString();
            Entity.FTime = Helper.StringToDateTime(dr["FTime"].ToString());
            Entity.OpenDate = Helper.StringToDateTime(dr["OpenDate"].ToString());
            Entity.DiscountName = dr["DiscountName"].ToString();
            Entity.DiscountMoney = Helper.StringToDecimal(dr["DiscountMoney"].ToString());
            Entity.AUCode = Helper.StringToDecimal(dr["AUCode"].ToString());
            Entity.AUName = dr["AUName"].ToString();
            Entity.PointMoney = Helper.StringToDecimal(dr["PointMoney"].ToString());
            Entity.VirMoney = Helper.StringToDecimal(dr["VirMoney"].ToString());
            return Entity;
        }

        /// <summary>
        /// 获取获取未出库的账单菜品
        /// </summary>
        /// <param name="filter">指定条件</param>
        /// <returns>返回第一行</returns>
        public DataTable GetUnStockBillDish(string GUID, string UID, string StoCode)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@stocode", StoCode)
             };
            DataTable dtReturn = new bllPaging().GetDataTableInfoByProcedure("p_GetUnStockBillDishTable", sqlParameters);
            return dtReturn;
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
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();

            DataTable dt = dal.BillReturn(BusCode, StoCode, BillCode, CCode, CCname, out mescode);
            if (mescode.Length > 0)
            //检测执行结果
            {
                CheckControlResult(2, mescode);
                return dtBase;
            }
            else
            {
                return dt;
            }
        }
    }
}