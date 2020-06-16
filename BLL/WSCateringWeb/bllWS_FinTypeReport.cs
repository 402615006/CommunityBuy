using CommunityBuy.BLL;
using CommunityBuy.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommunityBuy.BLL
{
   public class bllWS_FinTypeReport: bllBase
    {
        dalWS_FinTypeReport dal = new dalWS_FinTypeReport();
        public DataSet ShiftReport(string filters, string orders, string apporderwhere, string appshiftcode, string appordersdatetime, string apporderedatetime)
        {
            return dal.ShiftReport(filters, orders, apporderwhere, appshiftcode, appordersdatetime, apporderedatetime);
        }

        //public DataSet FanHuaShiftReport(string filters, string orders)
        //{
        //    return dal.FanHuaShiftReport(filters, orders);
        //}

        public DataSet ShiftBillReport(string StartTime, string EndTime, string StoCode, string DepCode, string CCode, string ShiftCode, string IsInvmoney, string IsPresdishe, string PayType, string FinCode, string MemType, string BillCode, string BillPayCode,string BillType,string PayWay)
        {
            return dal.ShiftBillReport(StartTime, EndTime, StoCode, DepCode, CCode, ShiftCode, IsInvmoney, IsPresdishe, PayType, FinCode, MemType, BillCode, BillPayCode, BillType,PayWay);
        }

        public DataSet StoreFinTypeReport(string StartTime, string EndTime, string userid, string StoCode, string DisCode, string QuickCode, string FinType,string BusCode)
        {
            return dal.StoreFinTypeReport(StartTime, EndTime, userid, StoCode, DisCode, QuickCode, FinType,BusCode);
        }

        public DataSet StoreDisTypeReport(string StartTime, string EndTime, string userid, string StoCode, string DisCode, string QuickCode, string TypeCode, string eTypeCode,string BusCode)
        {
            return dal.StoreDisTypeReport(StartTime, EndTime, userid, StoCode, DisCode, QuickCode, TypeCode, eTypeCode,BusCode);
        }

        
        public DataSet OrdertcdpReport(string StartTime, string EndTime, string userid, string StoCode, string ShiftCode,string Type,String DisCode,string PDisCode,string BusCode)
        {
            return dal.OrdertcdpReport(StartTime, EndTime, userid, StoCode, ShiftCode,Type,DisCode,PDisCode,BusCode);
        }

        /// <summary>
        /// 获取账单明细详情（历史）
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="StoCode"></param>
        /// <param name="ShiftCode"></param>
        /// <returns></returns>
        public DataSet OrderBillDetailReport(string userid, string StoCode, string ShiftCode)
        {
            return dal.OrderBillDetailReport(userid, StoCode, ShiftCode);
        }

        /// <summary>
        /// 获取账单明细详情（实时）
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="StoCode"></param>
        /// <param name="ShiftCode"></param>
        /// <returns></returns>
        public DataSet OrderTBBillDetailReport(string userid, string StoCode, string ShiftCode)
        {
            return dal.OrderTBBillDetailReport(userid, StoCode, ShiftCode);
        }

        public DataSet OrdertcdpzjReport(string StartTime, string EndTime, string userid, string StoCode, string Type,string DisCode,string BusCode)
        {
            return dal.OrdertcdpzjReport(StartTime, EndTime, userid, StoCode, Type,DisCode,BusCode);
        }

        /// <summary>
        /// 获取门店班次批量消券的记录
        /// </summary>
        /// <param name="StoCode"></param>
        /// <param name="ShiftCode"></param>
        /// <returns></returns>
        public DataTable GetCouponCheckLog(string StoCode,string ShiftCode,string BusCode)
        {
            return dal.GetCouponCheckLog(StoCode, ShiftCode,BusCode);
        }

    }
}
