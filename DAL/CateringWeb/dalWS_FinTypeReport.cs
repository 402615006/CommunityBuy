using System.Data;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.DAL
{
    public class dalWS_FinTypeReport
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        public DataSet ShiftReport(string filters, string orders, string apporderwhere, string appshiftcode, string appordersdatetime, string apporderedatetime)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" CREATE TABLE #dt_shifthistory(ShiftCode VARCHAR(32),buscode VARCHAR(16),stocode VARCHAR(8),depcode VARCHAR(32),depname nvarchar(128),stoname nvarchar(128),ccname nvarchar(128),ccode varchar(16),ctime datetime,etime datetime,comcode nvarchar(32),PayWay varchar(32) );");//班次临时表
            sql.Append(" CREATE TABLE #dt_bill(BillPKCode VARCHAR(32),billtype varchar(5),ShiftCode varchar(64),OrderCodeList varchar(max),PayWay varchar(32));");//账单信息单号临时表
            sql.Append(" CREATE TABLE #dt_Order (OrderPKCode VARCHAR(32),BillCode varchar(32));");//消费单信息临时表
            sql.Append(" insert into #dt_shifthistory SELECT PKCode,buscode,stocode,DepartCode,'' as depname,'' as stoname,ccname+'('+ccode+')',ccode,ctime,etime,commac,'1' FROM TH_OpenShift " + filters + " " + orders + ";");
            sql.Append(" insert into #dt_bill select PKCode,BillType,s.ShiftCode,OrderCodeList,b.PayWay from TH_Bill b inner join  #dt_shifthistory s on b.ShiftCode=s.ShiftCode  where b.TStatus<>'4' and b.PayWay<>'2';");

            #region 线上订单
            sql.Append(" if EXISTS(select 1 from TH_Bill b" + apporderwhere + ")");
            sql.Append(" begin");
            sql.Append(" DECLARE @stocode varchar(32);DECLARE @buscode varchar(32); select top 1 @stocode=stocode,@buscode=buscode from TH_Bill b" + apporderwhere + ";");
            //添加一个虚拟班次
            sql.Append(" insert into #dt_shifthistory(ShiftCode,buscode,stocode,depcode,depname,stoname,ccname,ccode,ctime,etime,comcode,PayWay) values('" + appshiftcode + "',@buscode,@stocode,'','','','小程序','app','" + appordersdatetime + "','" + apporderedatetime + "','小程序','2')");
            sql.Append(" insert into #dt_bill select PKCode,BillType,'" + appshiftcode + "',OrderCodeList,'2' from TH_Bill b " + apporderwhere + ";");//账单
            sql.Append(" end");
            #endregion

            sql.Append(" SELECT * FROM #dt_shifthistory order by ctime desc;");//获取班次信息0
            //账单信息
            sql.Append(" select b.* from TH_Bill b inner join #dt_shifthistory s on b.ShiftCode=s.ShiftCode where TStatus<>'4' and b.PayWay<>'2'");//线下
            sql.Append(" union all select [Id],[BusCode],[StoCode],[CCode],[CCname],[CTime],[TStatus],[OrderCodeList],[PKCode],[BillMoney],[PayMoney],[ZeroCutMoney],'" + appshiftcode + "' as ShiftCode,[Remar],[FTime],[OpenDate],[DiscountName],[DiscountMoney],[AUCode],[AUName],[PointMoney],[VirMoney],[BillType],[IsStocked],[PayWay],[CStatus],[TackNo],[GiveCoupons],[PayMethod],[DepartCode] from TH_Bill b " + apporderwhere + ";");//线上
            //支付信息
            sql.Append(" select '1' as PayWay,bp.BillCode,sum(isnull(bp.PayMoney,0)) as PayMoney,bp.PayMethodName,bp.PayMethodCode,b.ShiftCode,b.billtype from TH_BillPay bp inner join #dt_bill b on bp.BillCode=b.BillPKCode   where  bp.TStatus='1' and b.PayWay='1' group by bp.BillCode,bp.PayMethodName,bp.PayMethodCode,b.ShiftCode,b.billtype");//线下
            sql.Append(" union all select '2' as PayWay,bp.BillCode,sum(isnull(bp.PayMoney,0)) as PayMoney,bp.PayMethodName,bp.PayMethodCode,'" + appshiftcode + "' as ShiftCode, b.billtype from TH_BillPay bp inner join #dt_bill b on bp.BillCode=b.BillPKCode   where  bp.TStatus='1' and b.PayWay='2' group by bp.BillCode,bp.PayMethodName,bp.PayMethodCode,b.billtype;");//线上

            sql.Append(" insert into #dt_Order select o.PKCode,o.BillCode from TH_Order o inner join #dt_bill b on o.BillCode=b.BillPKCode where o.TStatus='3';");

            sql.Append(" select o.* from TH_Order o inner join #dt_bill b on o.BillCode=b.BillPKCode where o.TStatus='3';");//消费订单信息3

            //消费订单明细
            sql.Append("select (od.cookMoney+od.Price)*((case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum) as TotalMoney,d.FinTypeName,od.[FinCode],b.ShiftCode as ShiftCode,od.DiscountType,'1' PayWay  from TH_OrderDish od inner join TB_Dish d on d.DisCode=od.DisCode and d.StoCode=od.StoCode inner join #dt_Order o on od.OrderCode=o.OrderPKCode inner join #dt_bill b on o.BillCode=b.BillPKCode where od.IsPackage in('0','1') and b.PayWay='1'");//线下                                                                                                                                                                                                                                                                                                                                               //sql.Append(" select stocode,ordercode,(case when otype='9' then -abs(isnull(a.regamount,0)) else isnull(a.regamount,0) end) as regamount" +                                                                                                                                                                                                                                                                                                         //    ",a.cardtype,[dbo].[fn_GetShiftCodeToOrderCode](a.ordercode) as ShiftCode,[dbo].[fn_GetBillCodeToOrderCode](a.ordercode) as billcode from TH_memcardorders a where a.ordercode in(select BillPKCode from @dt_bill) and ISNULL(a.paystatus,'0')='2';");//会员卡业务5
            sql.Append(" union all select (od.cookMoney+od.Price)*((case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum) as TotalMoney,d.FinTypeName,od.[FinCode],'" + appshiftcode + "' as ShiftCode,od.DiscountType,'2' PayWay  from TH_OrderDish od inner join TB_Dish d on d.DisCode=od.DisCode and d.StoCode=od.StoCode inner join #dt_Order o on od.OrderCode=o.OrderPKCode inner join #dt_bill b on o.BillCode=b.BillPKCode where od.IsPackage in('0','1') and b.PayWay='2'");//线上

            sql.Append(" select stocode,paystatus,ordercode,(case when otype='9' then -abs(isnull(a.regamount,0)) else isnull(a.regamount,0) end) as regamount" +
    ",(case when otype='9' then -abs(isnull(a.regamount,0)) else isnull(a.regamount,0) end) as consummoney" +
    ",(case when otype='9' then -abs(isnull(a.cardcost,0)) else isnull(a.cardcost,0) end) as cardcost" +
    ",a.cardtype,b.ShiftCode as ShiftCode,b.BillPKCode as billcode from TH_memcardorders a inner join #dt_bill b on a.ordercode=b.OrderCodeList where ISNULL(a.paystatus,'0')='2' and b.PayWay='1'");//会员卡业务5
            sql.Append(" union all ");
            sql.Append(" select stocode,paystatus,ordercode,(case when otype='9' then -abs(isnull(a.regamount,0)) else isnull(a.regamount,0) end) as regamount" +
   ",(case when otype='9' then -abs(isnull(a.regamount,0)) else isnull(a.regamount,0) end) as consummoney" +
   ",(case when otype='9' then -abs(isnull(a.cardcost,0)) else isnull(a.cardcost,0) end) as cardcost" +
   ",a.cardtype,'" + appshiftcode + "' as ShiftCode,b.BillPKCode as billcode from TH_memcardorders a inner join #dt_bill b on a.ordercode=b.OrderCodeList where ISNULL(a.paystatus,'0')='2' and b.PayWay='2'");//会员卡业务5

            sql.Append("select c.*,'1' as PayWay from TH_BillCoupon c inner join #dt_bill b on c.BillCode=b.BillPKCode where TStatus='1' and b.PayWay='1' UNION ALL select c.id,c.buscode,c.stocode,c.ccode,c.ccname,c.ctime,c.tstatus,c.billcode,c.couponcode,c.couponmoney,c.membercardcode,c.realpay,c.vimoney,c.remark,c.usetype,'"+ appshiftcode + "'shiftcode,c.couponname,c.mccode,c.tictype,c.ticway,'2' as PayWay from TH_BillCoupon c inner join #dt_bill b on c.BillCode=b.BillPKCode where TStatus='1' and b.PayWay='2';");//账单使用的优惠券信息6
            //获取批量验券
            sql.Append(" select * from [dbo].[TH_CouponCheckLog] ccl inner join  #dt_shifthistory s on ccl.ShiftId=s.ShiftCode;");//8券回收

            //9班次开票
            sql.Append(" select b.ShiftCode,sum(isnull(v.inmoney,0)) as InMoney from [dbo].[TB_BillInvoice] v inner join TH_Bill b on v.billcode=b.pkcode inner join #dt_shifthistory s on b.ShiftCode=s.ShiftCode where b.TStatus<>'4' and b.PayWay<>'2' group by b.ShiftCode");//线下
            sql.Append(" union all select '" + appshiftcode + "' as ShiftCode,sum(isnull(v.inmoney,0)) as InMoney from [dbo].[TB_BillInvoice] v inner join TH_Bill b on v.billcode=b.pkcode " + apporderwhere + ";");//线上
            return DBHelper.ExecuteDataSet(sql.ToString());
        }

        public DataSet ShiftBillReport(string StartTime, string EndTime, string StoCode, string DepCode, string CCode, string ShiftCode, string IsInvmoney, string IsPresdishe, string PayType, string FinCode, string MemType, string BillCode, string BillPayCode, string BillType, string PayWay)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" create table #dt_shifthistory (ShiftCode VARCHAR(32),buscode VARCHAR(16),stocode VARCHAR(8),depcode VARCHAR(32),depname nvarchar(128),stoname nvarchar(128),ccname nvarchar(128),ctime datetime,etime datetime,comcode nvarchar(32) );");//班次临时表
            sql.Append(" create table #dt_bill(BillPKCode VARCHAR(32),OrderCode nvarchar(max),shiftcode varchar(32));create table #dt_MemOrder(OrderPKCode VARCHAR(32));");
            //根据条件添加关联查询
            sql.Append(" insert into #dt_bill select DISTINCT bill.PKCode,OrderCodeList,shiftcode from TH_Bill bill");
            if (!string.IsNullOrEmpty(DepCode) || !string.IsNullOrEmpty(CCode))//部门、收银员需要关联班次表
            {
                sql.Append(" inner join TH_OpenShift os on bill.shiftcode=os.PKCode");
                if (!string.IsNullOrEmpty(DepCode))
                {
                    sql.Append(" and os.DepartCode='" + DepCode + "'");
                }
                else if (!string.IsNullOrEmpty(CCode))
                {
                    sql.Append(" and os.CCode='" + CCode + "'");
                }
            }
            if (!string.IsNullOrEmpty(IsInvmoney))//是否开票
            {

            }
            if (!string.IsNullOrEmpty(IsPresdishe))//是否赠送，需要关联订单明细表中
            {
                sql.Append(" inner join TH_Order torder on torder.billcode=bill.PKCode inner join TH_OrderDish orderdish on torder.PKCode=orderdish.OrderCode and orderdish.DiscountType='6' ");
                if (!string.IsNullOrEmpty(FinCode))//财务类别与是否赠送关联的表相同
                {
                    sql.Append(" and orderdish.fincode='" + FinCode + "'");
                }
            }
            else if (!string.IsNullOrEmpty(FinCode))
            {
                sql.Append(" inner join TH_Order torder on torder.billcode=bill.PKCode inner join TH_OrderDish orderdish on torder.PKCode=orderdish.OrderCode and orderdish.FinCode='" + FinCode + "'");
            }
            if (!string.IsNullOrEmpty(PayType))//支付方式
            {
                sql.Append(" inner join TH_BillPay billpay on billpay.billcode=bill.PKCode and billpay.PayMethodCode='" + PayType + "' and billpay.TStatus='1'");
                if (!string.IsNullOrEmpty(BillPayCode))
                {
                    sql.Append(" and billpay.PKCode='" + BillPayCode + "'");
                }
            }
            else if (!string.IsNullOrEmpty(BillPayCode))
            {
                sql.Append(" inner join TH_BillPay billpay on billpay.billcode=bill.PKCode and billpay.PKCode='" + BillPayCode + "' and billpay.TStatus='1'");
            }
            if (!string.IsNullOrEmpty(MemType) || BillType == "2")
            {
                sql.Append(" inner join TH_memcardorders memcardorders on memcardorders.ordercode=bill.OrderCodeList");
                if (MemType == "8")//员工卡
                {
                    sql.Append(" and memcardorders.cardtype='8'");
                }
                else if (MemType == "food")//美食卡
                {
                    sql.Append(" and memcardorders.cardtype='food'");
                }
                else if (MemType == "1")
                {
                    sql.Append(" and memcardorders.cardtype<>'8' and memcardorders.cardtype<>'food'");
                }
                else
                {
                }
            }
            sql.Append(" where bill.FTime between '" + StartTime + "' and '" + EndTime + "' and bill.TStatus<>'4' and bill.stocode='" + StoCode + "'");
            if (!string.IsNullOrEmpty(PayWay))
            {
                sql.Append(" and bill.PayWay='" + PayWay + "'");
            }
            if (!string.IsNullOrEmpty(ShiftCode))
            {
                sql.Append(" and bill.shiftcode='" + ShiftCode + "'");
            }
            if (BillType == "2")
            {
                sql.Append(" and bill.billtype='2'");
            }
            else
            {
                if (!string.IsNullOrEmpty(BillType))
                {
                    sql.Append(" and bill.billtype='" + BillType + "'");
                }
            }
            if (!string.IsNullOrEmpty(BillCode))
            {
                sql.Append(" and bill.PKCode='" + BillCode + "'");
            }
            sql.Append(";");
            sql.Append(" create table #dt_Order(OrderPKCode VARCHAR(32));");//消费单信息临时表
            sql.Append(" insert into #dt_shifthistory SELECT PKCode,buscode,stocode,DepartCode,'' as depname,'' as stoname,ccname+'('+ccode+')',ctime,etime,commac FROM TH_OpenShift where PKCode in(select shiftcode from #dt_bill);");
            sql.Append(" select *,[dbo].[fn_GetBillCtimeToMealNameHistory](PKCode,StoCode) as mealname,[dbo].[fn_GetBillTableNameHistory](PKCode,StoCode) TableName,[dbo].[fn_GetBillCouponMoneyHistory](PKCode,StoCode) as CouponMoney,[dbo].[fn_GetBillVIMoneyHistory](PKCode,StoCode) as VIMoney,[dbo].[fn_GetBillCusNumHistory](PKCode,StoCode) as CusNum  from TH_Bill where PKCode in(select BillPKCode from #dt_bill);");//账单信息0
            sql.Append(" select * from #dt_shifthistory order by ctime desc;");//班次信息1
            sql.Append(" select bp.BillCode,sum(isnull(bp.PayMoney,0)) as PayMoney,bp.PayMethodName,bp.PayMethodCode from TH_BillPay bp inner join  #dt_bill b on bp.BillCode=b.BillPKCode  where bp.TStatus='1' group by bp.BillCode,bp.PayMethodName,bp.PayMethodCode;");//支付信息,支付正常的2
            sql.Append(" insert into #dt_Order select PKCode from TH_Order where TStatus='3' and BillCode in (select PKCode from TH_Bill where OrderCodeList in (select b.OrderCodeList from TH_Bill b inner join #dt_bill bi on b.PKCode=bi.BillPKCode)); ");
            sql.Append(" insert into #dt_MemOrder select ordercode from TH_memcardorders where ordercode in(select ordercodelist from TH_Bill where PKCode in(select BillPKCode from #dt_bill));");
            sql.Append(" select * from TH_Order ord where ord.TStatus='3' and ord.BillCode in (select PKCode from TH_Bill where OrderCodeList in (select OrderCodeList from TH_Bill b inner join #dt_bill bi on b.PKCode=bi.BillPKCode)); ");//消费订单信息3
            sql.Append("select sum(b.TotalMoney) TotalMoney,b.FinTypeName,b.FinCode,b.billcode from ( select (od.cookMoney+od.Price)*((case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum) as TotalMoney,d.FinTypeName,od.[FinCode],[dbo].[fn_GetOrderCodeToBillCodeHistory](od.ordercode) as billcode  from TH_OrderDish od inner join TB_Dish d on d.DisCode=od.DisCode and d.StoCode=od.StoCode  where OrderCode in(select OrderPKCode from #dt_Order) and od.IsPackage in('0','1')) b group by b.FinTypeName,b.FinCode,b.billcode;");//消费订单明细4
            sql.Append(" select stocode,ordercode,(case when otype='9' then -abs(isnull(a.regamount,0)) else isnull(a.regamount,0) end) as regamount" +
                ",(case when otype='9' then -abs(isnull(a.regamount,0)) else isnull(a.regamount,0) end) as consummoney" +
                ",(case when otype='9' then -abs(isnull(a.cardcost,0)) else isnull(a.cardcost,0) end) as cardcost" +
                ",a.cardtype,[dbo].[fn_GetBillCodeToOrderCodeHistory](a.ordercode) as billcode from TH_memcardorders a where a.ordercode in(select OrderPKCode from #dt_MemOrder) and ISNULL(a.paystatus,'0')='2';");//会员卡业务5
            sql.Append("select * from TH_BillCoupon where BillCode in (select PKCode from TH_Bill where OrderCodeList in (select b.OrderCodeList from TH_Bill b inner join #dt_bill bi on b.PKCode=bi.BillPKCode)) and TStatus='1';");//账单使用的优惠券信息6
            sql.Append(" select sum((od.cookMoney+od.Price)*((case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum)) as TotalMoney,[dbo].[fn_GetOrderCodeToBillCodeHistory](od.OrderCode) as BillCode  from TH_OrderDish od inner join TB_Dish d on d.DisCode=od.DisCode and d.StoCode=od.StoCode where OrderCode in(select OrderPKCode from #dt_Order) and od.DiscountType='6' group by od.OrderCode;");//赠送订单明细7
            //获取该班次的批量验券的信息
            sql.Append("select sum(l.SingleMoney) as SingleMoney,l.[UCname],l.[ShiftId],MAX(l.CTime) as CTime from TH_CouponCheckLog l inner join #dt_shifthistory s on l.ShiftId=s.ShiftCode GROUP BY l.UCname,l.ShiftId");
            return DBHelper.ExecuteDataSet(sql.ToString());

        }

        public DataSet StoreFinTypeReport(string StartTime, string EndTime, string userid, string StoCode, string DisCode, string QuickCode, string FinType,string BusCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select dd.DisTypeName, isnull(dd.FinTypeName,'无') as FinTypeName, dd.DisCode, dd.DisName, dd.IsPackage, dd.Uite, sum(dd.Price) as Price, sum(dd.DisNum) as DisNum, sum(dd.DiscountSum) as DiscountSum, sum(TotalPrice) as TotalPrice,dd.data from(");
            sql.Append(" select a.*,(a.Price * a.DisNum) as TotalPrice,'' as data from(");
            sql.Append(" select");
            sql.Append(" [dbo].[fn_GetDisTypeCodeToParentName](od.DisTypeCode,od.StoCode) DisTypeName,");
            sql.Append(" d.FinTypeName,");
            sql.Append(" od.DisCode,");
            sql.Append(" od.DisName,");
            sql.Append(" od.IsPackage,");
            sql.Append(" (case od.IsPackage when '0' then od.DisUite else '套' end) as Uite,");
            sql.Append(" (case od.DiscountType when '6' then 0 else (case od.IsPackage when '2' then 0 else od.Price+od.CookMoney end) end) as Price,");
            sql.Append(" (case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum as DisNum,");
            sql.Append(" (case od.DiscountType when '6' then (case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum else 0 end) DiscountSum");
            sql.Append(" from TH_OrderDish od inner join TB_Dish d on od.discode = d.DisCode and od.stocode=d.stocode inner join th_order o on od.OrderCode=o.PKCode and od.stocode=o.stocode  inner join [TH_Bill] b on o.BillCode=b.PKCode ");
            sql.Append(" where 1 = 1");
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql.Append(" and b.CTime>='" + StartTime + "'");
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql.Append(" and b.CTime<='" + EndTime + "'");
            }
            if (!string.IsNullOrEmpty(StoCode))
            {
                sql.Append(" and od.stocode='" + StoCode + "'");
            }
            if(!string.IsNullOrEmpty(BusCode))
            {
                sql.Append(" and od.buscode='" + BusCode + "'");
            }
            if (!string.IsNullOrEmpty(DisCode))
            {
                string[] discodes = DisCode.Split(',');
                sql.Append(" and od.discode in(");
                string codes = string.Empty;
                foreach (string code in discodes)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        codes += "'" + code + "',";
                    }
                }
                codes = codes.TrimEnd(',');
                sql.Append(codes + ")");
            }
            if (!string.IsNullOrEmpty(QuickCode))
            {
                sql.Append(" and d.QuickCode='" + QuickCode + "'");
            }
            if (!string.IsNullOrEmpty(FinType))
            {
                string[] fintypes = FinType.Split(',');
                sql.Append(" and d.FinCode in(");
                string codes = string.Empty;
                foreach (string code in fintypes)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        codes += "'" + code + "',";
                    }
                }
                codes = codes.TrimEnd(',');
                sql.Append(codes + ")");
            }
            sql.Append(" and o.tstatus='3'");
            sql.Append(") a where DisNum > 0 ) as dd group by dd.DisTypeName,dd.FinTypeName,dd.DisCode,dd.DisName,dd.IsPackage,dd.Uite,dd.data  order by DisTypeName,FinTypeName desc");
            return DBHelper.ExecuteDataSet(sql.ToString());
        }

        public DataSet StoreDisTypeReport(string StartTime, string EndTime, string userid, string StoCode, string DisCode, string QuickCode, string TypeCode, string eTypeCode,string BusCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select dd.DisTypeName, isnull(dd.DisTypeName1,'无') as DisTypeName1, dd.DisCode, dd.DisName, dd.IsPackage, dd.Uite, sum(dd.Price) as Price, sum(dd.DisNum) as DisNum, sum(dd.DiscountSum) as DiscountSum,sum(TotalPrice) as TotalPrice from(");
            sql.Append(" select a.*,(a.Price * a.DisNum) as TotalPrice,'' as data from(");
            sql.Append(" select");
            sql.Append(" [dbo].[fn_GetDisTypeCodeToParentName](od.DisTypeCode,od.stocode) DisTypeName,");
            sql.Append(" [dbo].[fn_GetDisTypeToTypeName](od.DisTypeCode,od.stocode) as DisTypeName1,");
            sql.Append(" od.DisCode,");
            sql.Append(" od.DisName,");
            sql.Append(" od.IsPackage,");
            sql.Append(" (case od.IsPackage when '0' then od.DisUite else '套' end) as Uite,");
            sql.Append(" (case od.DiscountType when '6' then 0 else (case od.IsPackage when '2' then 0 else od.Price+od.CookMoney end) end) as Price,");
            sql.Append(" (case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum as DisNum,");
            sql.Append(" (case od.DiscountType when '6' then (case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum else 0 end) DiscountSum");
            sql.Append(" from TH_OrderDish od inner join TB_Dish d on od.discode = d.DisCode and od.stocode=d.stocode inner join th_order o on od.OrderCode=o.PKCode and od.stocode=o.stocode inner join [TH_Bill] b on o.BillCode=b.PKCode ");
            sql.Append(" where 1 = 1");
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql.Append(" and b.CTime>='" + StartTime + "'");
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql.Append(" and b.CTime<='" + EndTime + "'");
            }
            if (!string.IsNullOrEmpty(StoCode))
            {
                sql.Append(" and od.stocode='" + StoCode + "'");
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                sql.Append(" and od.BusCode='" + BusCode + "'");
            }
            if (!string.IsNullOrEmpty(DisCode))
            {
                string[] discodes = DisCode.Split(',');
                sql.Append(" and od.discode in(");
                string codes = string.Empty;
                foreach (string code in discodes)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        codes += "'" + code + "',";
                    }
                }
                codes = codes.TrimEnd(',');
                sql.Append(codes + ")");
            }
            if (!string.IsNullOrEmpty(QuickCode))
            {
                sql.Append(" and d.QuickCode='" + QuickCode + "'");
            }
            if (!string.IsNullOrEmpty(TypeCode))
            {
                string[] fintypes = TypeCode.Split(',');
                sql.Append(" and od.DisTypeCode in(select pkcode from TB_DishType where PKKCode in(");
                string codes = string.Empty;
                foreach (string code in fintypes)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        codes += "'" + code + "',";
                    }
                }
                codes = codes.TrimEnd(',');
                sql.Append(codes + "))");
            }
            if (!string.IsNullOrEmpty(eTypeCode))
            {
                string[] fintypes = eTypeCode.Split(',');
                sql.Append(" and od.DisTypeCode in(");
                string codes = string.Empty;
                foreach (string code in fintypes)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        codes += "'" + code + "',";
                    }
                }
                codes = codes.TrimEnd(',');
                sql.Append(codes + ")");
            }
            sql.Append(" and o.tstatus='3'");
            sql.Append(") a where DisNum > 0  ) as dd group by dd.DisTypeName,dd.DisTypeName1,dd.DisCode,dd.DisName,dd.IsPackage,dd.Uite order by DisTypeName,DisTypeName1 desc");
            return DBHelper.ExecuteDataSet(sql.ToString());
        }

        public DataSet OrdertcdpReport(string StartTime, string EndTime, string userid, string StoCode, string ShiftCode, string Type, string DisCode, string PDisCode,string BusCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select b.PKCode,od.DisCode,od.DisName,sum((case when dis.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum) DisNum,(case when od.DiscountType='6' then 0 else sum(od.PackOnePrice) end) TotalMoney,od.pdiscode");
            sql.Append(" from [dbo].[TH_OrderDish] od");
            sql.Append(" inner join [dbo].[TH_Order] o on od.OrderCode=o.PKCode and o.stocode=od.stocode");
            sql.Append(" inner join [dbo].[TH_Bill] b on o.BillCode=b.PKCode");
            sql.Append(" inner join[dbo].[TB_Dish] dis on dis.discode=od.discode and dis.stocode=od.stocode");
            sql.Append(" where 1=1 ");
            if (Type == "1")//套内单品
            {
                sql.Append(" and od.IsPackage='2'");
                if (!string.IsNullOrEmpty(DisCode))
                {
                    sql.Append(" and od.DisCode='" + DisCode + "'");
                }
                if (!string.IsNullOrEmpty(PDisCode))
                {
                    sql.Append(" and od.PDisCode in (select PKCode from TH_OrderDish where stocode='" + StoCode + "' and DisCode='" + PDisCode + "' and ispackage='1')");
                }
            }
            else if (Type == "0")//普通单品
            {
                sql.Append(" and od.IsPackage='0'");
                sql.Append(" and od.discode='" + DisCode + "'");
            }
            else if (Type == "2")//套内单品
            {
                sql.Append(" and od.IsPackage='2'");
                if (!string.IsNullOrEmpty(DisCode))
                {
                    sql.Append(" and od.DisCode='" + DisCode + "'");
                }
                if (!string.IsNullOrEmpty(PDisCode))
                {
                    sql.Append(" and od.PDisCode in (select PKCode from TH_OrderDish where stocode='" + StoCode + "' and DisCode='" + PDisCode + "' and ispackage='1')");
                }
            }
            else
            {
                sql.Append(" and od.IsPackage='0'");
                sql.Append(" and od.discode='" + DisCode + "'");
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql.Append(" and b.CTime>='" + StartTime + "'");
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql.Append(" and b.CTime<='" + EndTime + "'");
            }
            if (!string.IsNullOrEmpty(StoCode))
            {
                sql.Append(" and b.stocode='" + StoCode + "'");
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                sql.Append(" and od.BusCode='" + BusCode + "'");
            }
            if (!string.IsNullOrEmpty(ShiftCode))
            {
                sql.Append(" and b.ShiftCode='" + ShiftCode + "'");
            }
            sql.Append(" group by od.DisName, b.PKCode, od.DisName, od.DisCode,od.DiscountType,od.pdiscode order by b.PKCode desc;");
            sql.Append("select od.pkcode from [dbo].[TH_OrderDish] od inner join[dbo].[TH_Order] o on od.OrderCode=o.PKCode inner join[dbo].[TH_Bill] b on o.BillCode=b.PKCode inner join[dbo].[TB_Dish] dis on dis.discode=od.discode and dis.stocode=od.stocode where od.IsPackage='1' and od.DiscountType='6' ");
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql.Append(" and b.CTime>='" + StartTime + "'");
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql.Append(" and b.CTime<='" + EndTime + "'");
            }
            if (!string.IsNullOrEmpty(StoCode))
            {
                sql.Append(" and b.stocode='" + StoCode + "'");
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                sql.Append(" and od.BusCode='" + BusCode + "'");
            }
            return DBHelper.ExecuteDataSet(sql.ToString());
        }

        /// <summary>
        /// 获取账单明细详情(历史)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="StoCode"></param>
        /// <param name="BillCode"></param>
        /// <returns></returns>
        public DataSet OrderBillDetailReport(string userid, string StoCode, string BillCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select t.PKCode, t.CTime, CONVERT(varchar(10), t.CTime,111) YTime,[dbo].[fn_GetBillCtimeToMealNameHistory] (t.PKCode, t.StoCode) MealName,'',[dbo].[fn_GetBillCusNumHistory] (t.PKCode, t.StoCode) as CusNum,(case t.TStatus when '0' then '未完成' when '1' then '全部结账' when '2' then '部分结账' when '3' then '全部反结' when '4' then '已取消' end) StatusName,t.FTime,t.ShiftCode,t.CCname,(case isnull(v.inmoney,0) when '0.00' then '未开' else '已开' end) as Fp,('消费单号:'+OrderCodeList) as remark,isnull(t.ZeroCutMoney,0) ZeroCutMoney,isnull(t.DiscountMoney,0) DiscountMoney,isnull(t.PointMoney,0) PointMoney,isnull(t.VirMoney,0) VirMoney,t.DiscountName from TH_Bill t left join [dbo].[TB_BillInvoice] v on t.PKCode=v.billcode where t.PKCode='" + BillCode + "' and t.StoCode = '" + StoCode + "';");
            sql.Append(" SELECT  t.TableName+'('+t.PKCode+')' as TableName,tt.TypeName,t.PKCode as TableCode FROM TB_Table t inner join TB_TableType tt on t.TableTypeCode=tt.PKCode WHERE t.PKCode in (SELECT DISTINCT TableCode FROM[dbo].[TH_OpenTable] WHERE PKCode IN(SELECT (select top 1 col from [dbo].[fn_StringSplit]([OpenCodeList],',')) FROM [dbo].[TH_Order] where BillCode ='" + BillCode + "')) and t.StoCode='" + StoCode + "' and tt.StoCode= '" + StoCode + "' order by t.PKCode desc;");
            sql.Append(" SELECT distinct od.pkcode,(case od.DiscountType when '6' then od.DisName+'(赠送)' else od.DisName end) as DisName,od.DisUite,(case when dis.IsWeight='1' then od.ItemNum else od.DisNum end) as DisNum,od.returnnum,(case od.DiscountType when '6' then 0 else od.Price end) as Price,od.CookMoney,od.TotalMoney,(case od.DiscountType when '6' then 0 else od.DiscountPrice end) as DiscountPrice,(case od.TotalPay when '6' then 0 else od.TotalPay end) as TotalPay,od.CCname,od.CTime,ot.TableCode,od.IsPackage FROM [dbo].[TH_Order] o");
            sql.Append(" left join[dbo].[TH_OpenTable] ot on o.OpenCodeList=ot.PKCode");
            sql.Append(" inner join[dbo].[TH_OrderDish] od on o.PKCode=od.OrderCode");
            sql.Append(" inner join[dbo].[TB_Dish] dis on dis.discode=od.discode and dis.stocode=od.stocode");
            sql.Append(" where o.BillCode='" + BillCode + "' and o.StoCode='" + StoCode + "' and od.StoCode='" + StoCode + "' order by IsPackage;");
            sql.Append(" SELECT od.FinCode,sum(((case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum)*(od.cookMoney+od.Price)) as TotalMoney,d.FinTypeName FROM[dbo].[TH_Order] o");
            // sql.Append(" left join[dbo].[TH_OpenTable] ot on o.OpenCodeList=ot.PKCode");
            sql.Append(" inner join[dbo].[TH_OrderDish] od on o.PKCode=od.OrderCode");
            sql.Append(" inner join [dbo].[TB_Dish] d on od.DisCode=d.DisCode");
            sql.Append(" where od.DiscountType<>'6' and BillCode ='" + BillCode + "' and o.StoCode='" + StoCode + "' and od.StoCode= '" + StoCode + "' and od.IsPackage<>'2' and d.stocode='" + StoCode + "' group by od.FinCode,d.FinTypeName; ");
            sql.Append(" select PayMethodName,PayMethodCode,sum(PayMoney) PayMoney,MemberCard,MemberName,MemberDiscount,Remar from[dbo].[TH_BillPay] where TStatus = '1' and BillCode ='" + BillCode + "' and stocode = '" + StoCode + "' group by PayMethodName,PayMethodCode,MemberCard,MemberName,MemberDiscount,Remar;");
            sql.Append(" select CouponCode,UseType,RealPay,(case UseType when '1' then '商品券' when '2' then '代金券' end) CouponType from [dbo].[TH_BillCoupon] where BillCode ='" + BillCode + "' and StoCode = '" + StoCode + "' and tstatus<>'2';");
            sql.Append(" select [memcode],[cardcode],[payamount] from [dbo].[TH_memcardorders] where ordercode in(select OrderCodeList from TH_Bill where PKCode='" + BillCode + "' and billtype='2') and stocode='" + StoCode + "' and [status]='1';");
            return DBHelper.ExecuteDataSet(sql.ToString());
        }


        /// <summary>
        /// 获取账单明细详情（实时）
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="StoCode"></param>
        /// <param name="BillCode"></param>
        /// <returns></returns>
        public DataSet OrderTBBillDetailReport(string userid, string StoCode, string BillCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select t.PKCode, t.CTime, CONVERT(varchar(10), t.CTime,111) YTime,[dbo].[fn_GetBillCtimeToMealNameHistory] (t.PKCode, t.StoCode) MealName,'',[dbo].[fn_GetBillCusNumHistory] (t.PKCode, t.StoCode) as CusNum,(case t.TStatus when '0' then '未完成' when '1' then '全部结账' when '2' then '部分结账' when '3' then '全部反结' when '4' then '已取消' end) StatusName,t.FTime,t.ShiftCode,t.CCname,(case isnull(v.inmoney,0) when '0.00' then '未开' else '已开' end) as Fp,('消费单号:'+OrderCodeList) as remark,isnull(t.ZeroCutMoney,0) ZeroCutMoney,isnull(t.DiscountMoney,0) DiscountMoney,isnull(t.PointMoney,0) PointMoney,isnull(t.VirMoney,0) VirMoney,t.DiscountName from TB_Bill t left join [dbo].[TB_BillInvoice] v on t.PKCode=v.billcode where t.PKCode='" + BillCode + "' and t.StoCode = '" + StoCode + "';");
            sql.Append(" SELECT  t.TableName+'('+t.PKCode+')' as TableName,tt.TypeName,t.PKCode as TableCode FROM TB_Table t inner join TB_TableType tt on t.TableTypeCode=tt.PKCode WHERE t.PKCode in (SELECT DISTINCT TableCode FROM[dbo].[TB_OpenTable] WHERE PKCode IN(SELECT (select top 1 col from [dbo].[fn_StringSplit]([OpenCodeList],',')) FROM [dbo].[TB_Order] where BillCode ='" + BillCode + "')) and t.StoCode='" + StoCode + "' and tt.StoCode= '" + StoCode + "' order by t.PKCode desc;");
            sql.Append(" SELECT distinct od.pkcode,(case od.DiscountType when '6' then od.DisName+'(赠送)' else od.DisName end) as DisName,od.DisUite,(case when dis.IsWeight='1' then od.ItemNum else od.DisNum end) as DisNum,od.returnnum,(case od.DiscountType when '6' then 0 else od.Price end) as Price,od.CookMoney,od.TotalMoney,(case od.DiscountType when '6' then 0 else od.DiscountPrice end) as DiscountPrice,(case od.TotalPay when '6' then 0 else od.TotalPay end) as TotalPay,od.CCname,od.CTime,ot.TableCode,od.IsPackage FROM [dbo].[TB_Order] o");
            sql.Append(" left join[dbo].[TB_OpenTable] ot on o.OpenCodeList=ot.PKCode");
            sql.Append(" inner join[dbo].[TB_OrderDish] od on o.PKCode=od.OrderCode");
            sql.Append(" inner join[dbo].[TB_Dish] dis on dis.discode=od.discode and dis.stocode=od.stocode");
            sql.Append(" where o.BillCode='" + BillCode + "' and o.StoCode='" + StoCode + "' and od.StoCode='" + StoCode + "' order by IsPackage;");
            sql.Append(" SELECT od.FinCode,sum(((case when d.IsWeight='1' then od.ItemNum else od.DisNum end)-od.returnnum)*od.Price) as TotalMoney,d.FinTypeName FROM[dbo].[TB_Order] o");
            //sql.Append(" left join[dbo].[TB_OpenTable] ot on o.OpenCodeList=ot.PKCode");
            sql.Append(" inner join[dbo].[TB_OrderDish] od on o.PKCode=od.OrderCode");
            sql.Append(" inner join [dbo].[TB_Dish] d on od.DisCode=d.DisCode");
            sql.Append(" where od.DiscountType<>'6' and BillCode ='" + BillCode + "' and o.StoCode='" + StoCode + "' and od.StoCode= '" + StoCode + "' and od.IsPackage<>'2' and d.stocode='" + StoCode + "' group by od.FinCode,d.FinTypeName; ");
            sql.Append(" select PayMethodName,PayMethodCode,sum(PayMoney) PayMoney,MemberCard,MemberName,MemberDiscount from [dbo].[TB_BillPay] where TStatus = '1' and BillCode ='" + BillCode + "' and stocode = '" + StoCode + "' group by PayMethodName,PayMethodCode,MemberCard,MemberName,MemberDiscount;");
            sql.Append(" select CouponCode,UseType,RealPay,(case UseType when '1' then '商品券' when '2' then '代金券' end) CouponType from [dbo].[TB_BillCoupon] where BillCode ='" + BillCode + "' and StoCode = '" + StoCode + "' and tstatus<>'2';");
            sql.Append(" select [memcode],[cardcode],[payamount] from [dbo].[memcardorders] where ordercode in(select OrderCodeList from TB_Bill where PKCode='" + BillCode + "' and billtype='2') and stocode='" + StoCode + "' and [status]='1';");
            return DBHelper.ExecuteDataSet(sql.ToString());
        }

        public DataSet OrdertcdpzjReport(string StartTime, string EndTime, string userid, string StoCode, string Type, string DisCode,string BusCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select od.DisCode,od.DisName,sum((case when dis.IsWeight='1' then od.ItemNum else od.DisNum end)-returnnum) DisNum,isnull(sum((case when od.DiscountType='6' then 0 else od.PackOnePrice end)),0) TotalMoney,od.pdiscode");
            sql.Append(" from[dbo].[TH_OrderDish] od");
            sql.Append(" inner join[dbo].[TH_Order] o on od.OrderCode=o.PKCode");
            sql.Append(" inner join[dbo].[TH_Bill] b on o.BillCode=b.PKCode");
            sql.Append(" inner join[dbo].[TB_Dish] dis on dis.discode=od.discode and dis.stocode=od.stocode");
            sql.Append(" where 1=1 ");
            if (Type == "1")//套内单品
            {
                sql.Append(" and od.IsPackage='2'");
                if (!string.IsNullOrEmpty(DisCode))
                {
                    sql.Append(" and od.PDisCode in (select PKCode from TH_OrderDish where stocode='" + StoCode + "' and DisCode='" + DisCode + "' and ispackage='1')");
                }
            }
            else if (Type == "2")
            {
                sql.Append(" and od.IsPackage='2'");
                if (!string.IsNullOrEmpty(DisCode))
                {
                    sql.Append(" and od.DisCode='" + DisCode + "'");
                }
            }
            else if (Type == "0")//普通单品
            {
                sql.Append(" and od.IsPackage='0'");
                if (!string.IsNullOrEmpty(DisCode))
                {
                    sql.Append(" and od.DisCode='" + DisCode + "'");
                }
            }
            else
            {
                sql.Append(" and od.IsPackage<>'1'");
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql.Append(" and b.CTime>='" + StartTime + "'");
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql.Append(" and b.CTime<='" + EndTime + "'");
            }
            if (!string.IsNullOrEmpty(StoCode))
            {
                sql.Append(" and b.stocode='" + StoCode + "'");
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                sql.Append(" and od.BusCode='" + BusCode + "'");
            }
            sql.Append(" group by od.DisName, od.DisName, od.DisCode,od.pdiscode order by od.DisCode desc;");

            sql.Append("select od.pkcode from [dbo].[TH_OrderDish] od inner join[dbo].[TH_Order] o on od.OrderCode=o.PKCode inner join[dbo].[TH_Bill] b on o.BillCode=b.PKCode inner join[dbo].[TB_Dish] dis on dis.discode=od.discode and dis.stocode=od.stocode where od.IsPackage='1' and od.DiscountType='6' ");
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql.Append(" and b.CTime>='" + StartTime + "'");
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql.Append(" and b.CTime<='" + EndTime + "'");
            }
            if (!string.IsNullOrEmpty(StoCode))
            {
                sql.Append(" and b.stocode='" + StoCode + "'");
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                sql.Append(" and od.BusCode='" + BusCode + "'");
            }
            return DBHelper.ExecuteDataSet(sql.ToString());
        }

        /// <summary>
        /// 获取门店班次批量消券的记录
        /// </summary>
        /// <param name="StoCode"></param>
        /// <param name="ShiftCode"></param>
        /// <returns></returns>
        public DataTable GetCouponCheckLog(string StoCode, string ShiftCode,string BusCode)
        {
            string sql = "select * from [TH_CouponCheckLog] where stocode='" + StoCode + "' and ShiftId='" + ShiftCode + "' and buscode='"+BusCode+"'";
            return DBHelper.ExecuteDataTable(sql);
        }

    }

}
