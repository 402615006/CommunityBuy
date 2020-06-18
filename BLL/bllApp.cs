using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
using System.Text;

namespace CommunityBuy.BLL
{
	/// <summary>
    /// 账单业务类
    /// </summary>
    public class bllApp : bllBase
    {
		DAL.dalApp dal = new DAL.dalApp();
        TB_BillEntity Entity = new TB_BillEntity();

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

            return new bllPaging().GetPagingInfo("TB_Bill", "Id",
                "*,ctime as checktime,CCName as creater," +
                "dbo.fn_GetBillTableName(PKCode, Stocode) as TableName," +
                "dbo.fn_GetBillCouponMoney(PKCode, Stocode) as CouponMoney," +
                "(BillMoney - PayMoney - ZeroCutMoney - dbo.fn_GetBillCouponMoney(PKCode, Stocode)) as ToPayMoney," +
                "dbo.fn_GetBillOrderTime(PKCode, Stocode) as ordertime", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 获取门店和菜品信息
        /// </summary>
        /// <param name="stocode"></param>
        /// <param name="tablecode"></param>
        /// <param name="menucode"></param>
        /// <returns></returns>
        public DataSet GetStoreDishInfo(string stocode,string tablecode,string menucode)
        {
            //获取门店的信息

            DataSet ds = dal.GetStoreDishInfo(stocode, tablecode, menucode);
            return ds;
        }

        /// <summary>
        /// 获取桌台的账单和订单信息
        /// </summary>
        /// <param name="stocode">门店编号</param>
        /// <param name="tablecode">桌台编号</param>
        /// <returns></returns>
        public DataSet GetTableOrderAndBill(string stocode,string opencode)
        {
            StringBuilder postStr = new StringBuilder();
            //获取参数信息 "GUID", "USER_ID", "pageSize", "currentPage", "filter", "order"
            DataSet ds = dal.GetTableOrderAndBill(stocode, opencode);
            return ds;
        }

        public string UpdateBillAndDish(DataTable dtBill, DataTable dtDish)
        {
            string rel = "";

            string billcode= dtBill.Rows[0]["PKCode"].ToString();
            string stocode= dtBill.Rows[0]["stocode"].ToString();
            string zerocutmoney= dtBill.Rows[0]["zerocutmoney"].ToString();
            string discountname = "";
            string paymethod= dtBill.Rows[0]["PayMethod"].ToString();
            //dtBill.Rows[0]["discountname"].ToString();
            string discountmoney = dtBill.Rows[0]["discountmoney"].ToString();
            string paymoney = dtBill.Rows[0]["paymoney"].ToString();

            string billSql =string.Format("update tb_bill set  PayMethod='{6}',zerocutmoney={0},discountname='{1}',discountmoney={2},paymoney={3} where PKCode='{4}' and StoCode='{5}' ;",
                zerocutmoney, discountname, discountmoney, paymoney, billcode, stocode, paymethod
                );

            StringBuilder sbDisSql = new StringBuilder() ;
            sbDisSql.AppendLine(billSql);
            foreach (DataRow dr in dtDish.Rows)
            {
                string discounttype = dr["discounttype"].ToString();
                discountmoney = dr["discountprice"].ToString();
                string pkcode = dr["orderdiscode"].ToString();
                string discountremark= dr["discountremark"].ToString();
                string sqlTemp = string.Format("update tb_orderdish set discountprice={0},discounttype='{1}',discountremark='{4}'  where PKCode='{2}' and Stocode='{3}';",
                    discountmoney, discounttype, pkcode, stocode, discountremark
                    );
                sbDisSql.AppendLine(sqlTemp);
            }
            sbDisSql.AppendLine(string.Format("execute [dbo].[p_TB_Bill_UpStatuByMoney] '{0}','{1}';", stocode, billcode));

            string sql = sbDisSql.ToString();

            int rn=new bllPaging().ExecuteNonQueryBySQL(sql);
            if (rn <0)
            {
                rel = "error";
            }
            return rel;
        }


        /// <summary>
        /// 获取我的账单和账单下的菜品
        /// </summary>
        /// <param name="stocode">门店编号</param>
        /// <param name="memcode">会员卡号</param>
        /// <returns></returns>
        public DataSet GetMemBillAndDish(string memcode,string page,string pagesize,string status)
        {
            //获取门店的信息
            DataSet ds = dal.GetMemBillDish( memcode,page,pagesize,status);
            return ds;
        }

        /// <summary>
        /// 取消账单
        /// </summary>
        /// <param name="stocode"></param>
        /// <param name="billcode"></param>
        /// <returns></returns>
        public int CancelBill(string stocode, string billcode)
        {
            int rel = dal.CancelBill(stocode, billcode);
            return rel;
        }

    }
}