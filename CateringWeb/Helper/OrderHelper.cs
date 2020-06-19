using CommunityBuy.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy
{
    /// <summary>
    /// 支付页面帮助类
    /// </summary>
    public class OrderHelper
    {
        /// <summary>
        /// 重新计算账单金额
        /// </summary>
        /// <param name="dtBill"></param>
        /// <param name="dtDish"></param>
        /// <param name="ZeroType"></param>
        /// <param name=""></param>
        public static void ResetBillMoney(ref DataTable dtBill, DataTable dtDish,DataTable dtCoupon)
        {
            if (dtBill != null && dtBill.Rows.Count > 0)
            {
                decimal DiscountMoney = 0;
                decimal CouponMoney = SumCouponMoney(dtCoupon);
                decimal BillMoney = StringHelper.StringToDecimal(dtBill.Rows[0]["BillMoney"].ToString());
                decimal PayMoney = StringHelper.StringToDecimal(dtBill.Rows[0]["PayMoney"].ToString());
                decimal ToPayMoney = BillMoney  - CouponMoney;
                ToPayMoney = ToPayMoney - PayMoney;
                
                dtBill.Rows[0]["DiscountMoney"] = DiscountMoney;
                dtBill.Rows[0]["ToPayMoney"] = ToPayMoney;
                dtBill.Rows[0]["CouponMoney"] = CouponMoney;
                dtBill.AcceptChanges();
            }
        }

        /// <summary>
        /// 计算优惠券优惠金额
        /// </summary>
        /// <param name="dtDish"></param>
        /// <returns></returns>
        public static decimal SumCouponMoney(DataTable dtCoupon)
        {
            decimal rel = 0;
            if (dtCoupon != null)
            {
                foreach (DataRow dr in dtCoupon.Rows)
                {
                     rel += StringHelper.StringToDecimal(dr["RealPay"].ToString());
                }
            }
            return rel;
        }

        /// <summary>
        /// 使用优惠券逻辑
        /// </summary>
        /// <param name="dtCoupon"></param>
        /// <returns></returns>
        public static string UseCoupon(string BillCode, dynamic coupon, decimal NowPayMoney, ref DataTable dtOrderDish, ref TB_BillCouponEntity CouponEntity,DataTable dtBillCoupon)
        {
            string Msg = "";
                string CouponCode = coupon["checkcode"].ToString();
                string CouponName = coupon["couname"].ToString();
                string sectype = coupon["sectype"].ToString();//券类型（ProCoupon-商品券，FilmCoupon-影城券，DIC00000045-代金券）
                string discode = coupon["discodes"].ToString();//抵用菜品编号
                discode = "'" + discode.TrimStart(',').TrimEnd(',').Replace(",", "','") + "'";
                string bigclass = coupon["fincodes"].ToString();//财务类别
                if (!string.IsNullOrWhiteSpace(bigclass))
                {
                    bigclass = "'" + bigclass.TrimStart(',').TrimEnd(',').Replace(",", "','") + "'";
                }
                decimal maxmoney = StringHelper.StringToDecimal(coupon["maxmoney"].ToString());//消费满金额
                int uselimit = StringHelper.StringToInt(coupon["uselimit"].ToString());//使用上限
                decimal singlemoney = StringHelper.StringToDecimal(coupon["singlemoney"].ToString());//优惠券金额
                decimal disuselack = StringHelper.StringToDecimal(coupon["disuselack"].ToString());//补差金额
                string Remark = CouponCode;
                string McCode = coupon["mccode"].ToString();

                decimal RealPay = 0;
                decimal VIMoney = 0;
                decimal DiscountPrice = 0;
                string UseType = "";
                string OrderDishId = "";
                //获取已点菜品信息
                DataRow[] drCoupon = new DataRow[] { };
                //使用上限判断
                if (dtBillCoupon != null && dtBillCoupon.Rows.Count > 0)
                {
                    drCoupon = dtBillCoupon.Select("BillCode='" + BillCode + "' and McCode='" + McCode + "'");
                    if (drCoupon.Length >= uselimit)
                    {
                        Msg = "优惠券超过使用上限“" + uselimit.ToString() + "”张，请检查！";
                    return Msg;
                    }
                }

                //筛选掉退菜和套内菜品
                switch (sectype)
                {
                    case "ProCoupon"://商品券
                        //按价格排序（先优惠最高金额的菜品）
                        dtOrderDish.DefaultView.Sort = "Price DESC";
                        DataTable dtNewOrderDish = dtOrderDish.DefaultView.ToTable();
                        DataRow[] drDish = dtNewOrderDish.Select("DisCode in(" + discode + ")");
                        int index = 0;
                        if (drDish.Length == 0)
                        {
                            Msg = "没有可优惠的商品信息，请检查！";
                            return Msg;
                        }

                        RealPay = StringHelper.StringToDecimal(drDish[index]["Price"].ToString()) - disuselack;
                        VIMoney = 0;
                        UseType = "4";
                        break;
                    default://满减券，代金券
                        RealPay = singlemoney;
                        UseType = "2";
                        OrderDishId = "";
                        decimal B_TCouponMoney = 0;//券总金额
                        decimal B_CouponMoney = 0;//该类型券金额
                        decimal B_TDish_bigclass = 0;
                        decimal B_Dish_bigclass = 0;

                        decimal diff = 0;

                        if (drCoupon.Length > 0)//该同类型券已使用金额
                        {
                            DataTable dt_BCoupon = drCoupon.CopyToDataTable();
                            B_TCouponMoney = GetConponRealMoney(dt_BCoupon, string.Empty);
                            B_CouponMoney = GetConponRealMoney(dt_BCoupon, McCode);
                        }
                        B_TDish_bigclass = GetDishesRealMoney(dtOrderDish, McCode);//已点菜品金额

                        //获取订单菜品财务类别的合计金额
                        if (bigclass.Length > 0)
                        {
                            diff = B_Dish_bigclass - B_CouponMoney;
                        }
                        else
                        {
                            diff = B_TDish_bigclass - B_TCouponMoney;
                        }
                        if (maxmoney > 0)//消费满金额
                        {
                            if (diff < maxmoney || NowPayMoney < maxmoney)//满额不够
                            {
                                Msg = "优惠券满" + maxmoney.ToString() + "元不满足，请检查！";
                            return Msg;
                            }
                        }

                        if (diff < NowPayMoney)//差额小于剩余金额
                        {
                            NowPayMoney = diff;
                        }
                        
                        if (NowPayMoney < singlemoney)//剩余金额<优惠券金额
                        {
                            RealPay = NowPayMoney;
                            VIMoney = singlemoney - NowPayMoney;
                        }
                        break;
                }
                //设置菜品的折扣
                if (OrderDishId.Length > 0)
                {
                    DataRow[] arrdr = dtOrderDish.Select("orderdiscode='" + OrderDishId + "'");
                    if (arrdr.Length > 0)
                    {
                        arrdr[0]["DiscountType"] = UseType;
                        arrdr[0]["DiscountPrice"] = DiscountPrice;
                        arrdr[0]["DiscountRemark"] = CouponCode;
                    }
                }

            CouponEntity.CouponCode = CouponCode;
            CouponEntity.CouponMoney = singlemoney;
            CouponEntity.MemberCardCode = "";
            CouponEntity.RealPay = RealPay;
            CouponEntity.VIMoney = VIMoney;
            CouponEntity.Remark = OrderDishId;
            CouponEntity.UseType = UseType;
            CouponEntity.CouponName = CouponName;
            CouponEntity.McCode = McCode;
            return Msg;
        }

        /// <summary>
        /// 获取优惠券的实际使用金额
        /// </summary>
        /// <param name="dtCoupon"></param>
        /// <param name="McCode"></param>
        /// <returns></returns>
        private static decimal GetConponRealMoney(DataTable dtCoupon, string McCode)
        {
            decimal decReturn = 0;
            DataRow[] drCoupon;
            if (McCode.Length > 0)
            {
                drCoupon = dtCoupon.Select("McCode='" + McCode + "'");
            }
            else
            {
                drCoupon = dtCoupon.Select();
            }
            if (drCoupon != null && drCoupon.Length > 0)
            {
                foreach (DataRow dr in drCoupon)
                {
                    decReturn += StringHelper.StringToDecimal(dr["RealPay"].ToString());
                }
            }

            return decReturn;
        }


        /// <summary>
        /// 获取菜品金额
        /// </summary>
        /// <param name="dtOrderDish"></param>
        /// <param name="FinCode">财务类别</param>
        /// <returns></returns>
        private static decimal GetDishesRealMoney(DataTable dtOrderDish, string FinCode)
        {
            decimal decReturn = 0;
            DataRow[] drDish;
            if (FinCode.Length > 0)
            {
                drDish = dtOrderDish.Select("FinCode in(" + FinCode + ")");
            }
            else
            {
                drDish = dtOrderDish.Select();
            }
            if (drDish != null && drDish.Length > 0)
            {
                foreach (DataRow dr in drDish)
                {
                    decReturn += StringHelper.StringToDecimal(dr["DiscountPrice"].ToString()) * (StringHelper.StringToDecimal(dr["DisNum"].ToString()));
                }
            }

            return decReturn;
        }
    }
}
