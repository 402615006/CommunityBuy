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
        /// 
        /// </summary>
        /// <param name="PayMoney">支付金额</param>
        /// <param name="MType">抹零类型</param>
        /// <returns>抹零金额</returns>
        public static decimal GetMaLingByMoney(decimal PayMoney, string MType)
        {
            if (PayMoney <= 0)
            {
                return PayMoney;
            }
            decimal NewPayMoney = PayMoney;
            decimal MaL = 0;
            //0-无；1-分向上；2-分向下；3-分四舍五入；4-角向上；5-角向下；6-角四舍五入；
            switch (MType)
            {
                case "1":
                    NewPayMoney = decimal.Parse((Math.Ceiling(PayMoney * 10) / 10).ToString("F2"));
                    break;
                case "2":
                    NewPayMoney = decimal.Parse((Math.Floor(PayMoney * 10) / 10).ToString("F2"));
                    break;
                case "3":
                    NewPayMoney = decimal.Parse((Math.Round(((int)(PayMoney * 100) * 1.0)) / 100).ToString("F1"));
                    break;
                case "4":
                    NewPayMoney = decimal.Parse((Math.Ceiling(((int)(PayMoney * 10) * 1.0) / 10)).ToString("F1"));
                    break;
                case "5":
                    NewPayMoney = decimal.Parse((Math.Floor(((int)(PayMoney * 10) * 1.0) / 10)).ToString("F1"));
                    break;
                case "6":
                    NewPayMoney = decimal.Parse((Math.Round(((int)(PayMoney * 10) * 1.0)) / 10).ToString("F0"));
                    break;
            }
            MaL = PayMoney - NewPayMoney;
            return MaL;
        }

        /// <summary>
        /// 获取已点菜品的折扣信息
        /// </summary>
        /// <param name="dtDish">已点菜品</param>
        /// <param name="dtDiscountSchemeRate">折扣方案信息</param>
        /// <param name="DiscountRate">一般折扣率</param>
        /// <returns>打折后的已点菜品</returns>
        public static void GetDiscountByDishes(ref DataTable dtOrderDish, DataTable dtDiscountSchemeRate, decimal DiscountRate)
        {
            //折扣类型1会员价2折扣模板3次卡4商品券5时价特价6签送
            DataRow[] drs = dtOrderDish.Select("IsPackage<>'2' and DiscountType not in ('3','4','5','6')");
            if (dtOrderDish != null && dtDiscountSchemeRate != null)
            {
                foreach (DataRow dr in drs)
                {
                    //菜品售价
                    decimal Price = Helper.StringToDecimal(dr["Price"].ToString());
                    //菜品类别编号
                    string DisTypeCode = dr["TypeCode"].ToString();
                    //菜品编号
                    string DisCode = dr["DisCode"].ToString();
                    //是否会员价
                    string IsMemPrice = dr["IsMemPrice"].ToString();
                    //菜品折扣类型
                    string DiscountType = dr["DiscountType"].ToString();
                    //菜品折扣价
                    decimal DiscountPrice = Helper.StringToDecimal(dr["DiscountPrice"].ToString());
                    if (dr["DiscountType"].ToString() != "1")//没有进行任何折扣
                    {
                        dr["DiscountType"] = "2";
                        dr["DiscountPrice"] = getDisMoneyByDisCode(DisTypeCode, DisCode, (double)Price, (double)DiscountRate, dtDiscountSchemeRate);
                    }
                }
            }
        }

        /// <summary>
        /// 打开会员价
        /// </summary>
        /// <param name="dtOrderDish"></param>
        /// <param name="bolMemPrice"></param>
        public static void AddMemberPrice(ref DataTable dtOrderDish)
        {
            //过滤时价特价、商品券、次卡菜品
            DataRow[] drs = dtOrderDish.Select("DiscountType not in ('3','4','5','6')");
            if (dtOrderDish != null)
            {
                foreach (DataRow dr in drs)
                {
                    //菜品售价
                    decimal Price = Helper.StringToDecimal(dr["Price"].ToString());
                    //菜品类别编号
                    string DisTypeCode = dr["TypeCode"].ToString();
                    //菜品编号
                    string DisCode = dr["DisCode"].ToString();
                    //是否会员价
                    string IsMemPrice = dr["IsMemPrice"].ToString();
                    //菜品折扣类型
                    string DiscountType = dr["DiscountType"].ToString();
                    //菜品折扣价
                    decimal DiscountPrice = Helper.StringToDecimal(dr["DiscountPrice"].ToString());
                    //会员价
                    decimal MemPrice = Helper.StringToDecimal(dr["MemPrice"].ToString());
                    //菜品是否允许会员价

                    if (IsMemPrice == "1")
                    {
                        dr["DiscountType"] = "1";
                        //DiscountType 5-时价特价，1-会员价，2-折扣模板，3-次卡，4-商品券
                        if (Price >= MemPrice && MemPrice > 0)
                        {
                            dr["DiscountPrice"] = MemPrice;
                        }
                        else
                        {
                            dr["DiscountPrice"] = Price;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取消会员价
        /// </summary>
        /// <param name="dtOrderDish"></param>
        /// <param name="bolMemPrice"></param>
        public static void CancelMemberPrice(ref DataTable dtOrderDish)
        {
            if (dtOrderDish != null)
            {
                //DiscountType 5-时价特价，1-会员价，2-折扣模板，3-次卡，4-商品券
                DataRow[] drs = dtOrderDish.Select("DiscountType='1'");
                foreach (DataRow dr in drs)
                {
                    //菜品售价
                    decimal Price = Helper.StringToDecimal(dr["Price"].ToString());
                    //菜品类别编号
                    string DisTypeCode = dr["DisTypeCode"].ToString();
                    //菜品编号
                    string DisCode = dr["DisCode"].ToString();
                    //是否会员价
                    string IsMemPrice = dr["IsMemPrice"].ToString();
                    //菜品折扣类型
                    string DiscountType = dr["DiscountType"].ToString();
                    //菜品折扣价
                    decimal DiscountPrice = Helper.StringToDecimal(dr["DiscountPrice"].ToString());
                    dr["DiscountType"] = "";
                    dr["DiscountPrice"] =dr["Price"];
                }
                dtOrderDish.AcceptChanges();
            }
        }

        /// <summary>
        /// 重新计算账单金额
        /// </summary>
        /// <param name="dtBill"></param>
        /// <param name="dtDish"></param>
        /// <param name="ZeroType"></param>
        /// <param name=""></param>
        public static void ResetBillMoney(ref DataTable dtBill, DataTable dtDish,DataTable dtCoupon,string ZeroType)
        {
            if (dtBill != null && dtBill.Rows.Count > 0)
            {
                decimal DiscountMoney = SumDiscountMoney(dtDish);
                decimal CouponMoney = SumCouponMoney(dtCoupon);
                decimal ZeroCutMoney = Helper.StringToDecimal(dtBill.Rows[0]["ZeroCutMoney"].ToString());
                decimal BillMoney = Helper.StringToDecimal(dtBill.Rows[0]["BillMoney"].ToString());
                decimal PayMoney = Helper.StringToDecimal(dtBill.Rows[0]["PayMoney"].ToString());
                decimal ToPayMoney = BillMoney - DiscountMoney - CouponMoney;
                ZeroCutMoney = GetMaLingByMoney(ToPayMoney, ZeroType);
                ToPayMoney = ToPayMoney - ZeroCutMoney- PayMoney;
                
                dtBill.Rows[0]["ZeroCutMoney"] = ZeroCutMoney;
                dtBill.Rows[0]["DiscountMoney"] = DiscountMoney;
                dtBill.Rows[0]["ToPayMoney"] = ToPayMoney;
                dtBill.Rows[0]["CouponMoney"] = CouponMoney;
                dtBill.AcceptChanges();
            }
        }

        /// <summary>
        /// 计算折扣金额
        /// </summary>
        /// <param name="dtDish"></param>
        /// <returns></returns>
        public static decimal SumDiscountMoney(DataTable dtDish)
        {
            decimal rel = 0;
            foreach (DataRow dr in dtDish.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["DiscountType"].ToString()))
                {
                    //商品券、时价特价、签送的不算作折扣金额
                    if (dr["DiscountType"].ToString() != "4" && dr["DiscountType"].ToString() != "5" && dr["DiscountType"].ToString() != "6")
                    {
                        rel += (Helper.StringToDecimal(dr["price"].ToString()) - Helper.StringToDecimal(dr["DiscountPrice"].ToString())) * (Helper.StringToDecimal(dr["DisNum"].ToString()));
                    }
                }
            }
            return rel;
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
                     rel += Helper.StringToDecimal(dr["RealPay"].ToString());
                }
            }
            return rel;
        }

        /// <summary>
        /// 获取菜品折扣
        /// </summary>
        /// <param name="DisTypeCode"></param>
        /// <param name="discode"></param>
        /// <param name="disprice"></param>
        /// <param name="DiscountRate">一般折扣率</param>
        /// <param name="dtDiscountSchemeRate"></param>
        /// <returns></returns>
        private static decimal getDisMoneyByDisCode(string DisTypeCode, string discode, double disprice, double DiscountRate, DataTable dtDiscountSchemeRate)
        {
            double dis = 0.0;
            //折扣率是整数，需缩小100倍
            DiscountRate = DiscountRate / 100.0;
            if (dtDiscountSchemeRate != null)
            {
                DataRow[] drs = dtDiscountSchemeRate.Select("DisCode='" + discode + "'");
                if (drs.Length > 0)//走菜品折扣
                {
                    dis = Math.Round((Helper.StringToDouble(drs[0]["DiscountRate"].ToString()) / 100.0 * disprice), 2);
                }
                else
                {
                    drs = dtDiscountSchemeRate.Select("DisTypeCode='" + DisTypeCode + "'");
                    if (drs.Length > 0)//走菜品类别折扣
                    {
                        dis = Math.Round((Helper.StringToDouble(drs[0]["DiscountRate"].ToString()) / 100.0 * disprice), 2);
                    }
                    else
                    {
                        dis = Math.Round((DiscountRate * disprice), 2);
                    }
                }
            }

            return (decimal)dis;
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
                decimal maxmoney = Helper.StringToDecimal(coupon["maxmoney"].ToString());//消费满金额
                int uselimit = Helper.StringToInt(coupon["uselimit"].ToString());//使用上限
                decimal singlemoney = Helper.StringToDecimal(coupon["singlemoney"].ToString());//优惠券金额
                decimal disuselack = Helper.StringToDecimal(coupon["disuselack"].ToString());//补差金额
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

                DataTable dtNewDish = GetActiveDishes(dtOrderDish);

                if (dtNewDish==null || dtNewDish.Rows.Count == 0)
                {
                    Msg = "没有可优惠的菜品信息，请检查！";
                return Msg;
                }
                DataRow[] drDish = null;
                switch (sectype)
                {
                    case "ProCoupon"://商品券
                        //按价格排序（先优惠最高金额的菜品）
                        dtOrderDish.DefaultView.Sort = "Price DESC";
                        DataTable dtNewOrderDish = dtNewDish.DefaultView.ToTable();
                        drDish = dtNewOrderDish.Select("DisCode in(" + discode + ")");
                        int index = 0;
                        if (drDish.Length == 0)
                        {
                            Msg = "没有可优惠的菜品信息，请检查！";
                        return Msg;
                        }
                        else
                        {
                            //取最大金额菜品
                            index = GetMaxMoneyDish(drDish);
                            DataRow drYes = drDish[index];
                            //5-时价特价，1-会员价，2-折扣模板，3-次卡，4-商品券，6-赠送
                            string DiscountType = drYes["DiscountType"].ToString();
                            DiscountPrice = disuselack;
                            OrderDishId = drYes["orderdiscode"].ToString();
                        }
                        RealPay = Helper.StringToDecimal(drDish[index]["Price"].ToString()) - disuselack;
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
                        if (!string.IsNullOrWhiteSpace(bigclass))//财务类别判断
                        {
                            drDish = dtNewDish.Select("FinCode in(" + bigclass + ")");
                            if (drDish.Length == 0)
                            {
                                Msg = "没有可优惠的菜品信息，请检查！";
                            return Msg;
                            }
                        }

                        if (drCoupon.Length > 0)//该同类型券已使用金额
                        {
                            DataTable dt_BCoupon = drCoupon.CopyToDataTable();
                            B_TCouponMoney = GetConponRealMoney(dt_BCoupon, string.Empty);
                            B_CouponMoney = GetConponRealMoney(dt_BCoupon, McCode);
                        }
                        B_TDish_bigclass = GetDishesRealMoney(dtNewDish, string.Empty);//已点菜品金额
                        B_Dish_bigclass = GetDishesRealMoney(dtNewDish, bigclass);//该同类型券已点菜品金额

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
        /// 获取有效的菜品信息（单菜及套餐主记录，可使用消费券）
        /// </summary>
        /// <param name="dtOrderDish"></param>
        /// <returns></returns>
        private static DataTable GetActiveDishes(DataTable dtOrderDish)
        {
            DataTable dtNewDish = null;
            DataRow[] dtRows = dtOrderDish.Select("isCoupon='1' and IsPackage<>'2' and DiscountType not in('3','4','5','6')");
            if (dtRows != null && dtRows.Length > 0)
            {
                dtNewDish = dtRows.CopyToDataTable();
            }
            if (dtNewDish != null && dtNewDish.Rows.Count > 0)
            {
                for (int i = dtNewDish.Rows.Count - 1; i >= 0;i-- )
                {
                    if (Helper.StringToDecimal(dtNewDish.Rows[i]["DisNum"].ToString()) <= 0)
                    {
                        dtNewDish.Rows.RemoveAt(i);
                    }
                }
            }

            return dtNewDish;
        }

        /// <summary>
        /// 获取菜品单价最高的序号
        /// </summary>
        /// <param name="drDish"></param>
        /// <returns></returns>
        private static int GetMaxMoneyDish(DataRow [] drDish)
        {
            decimal maxP = 0;
            int index = 0;
            for (int i = 0; i <drDish.Length; i++)
            {
                decimal n=Helper.StringToDecimal(drDish[i]["Price"].ToString());
                if ( n> maxP)
                {
                    index =i;
                    maxP=n;
                }
            }
            return index;
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
                    decReturn += Helper.StringToDecimal(dr["DiscountPrice"].ToString()) * (Helper.StringToDecimal(dr["DisNum"].ToString()));
                }
            }

            return decReturn;
        }

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
                    decReturn += Helper.StringToDecimal(dr["RealPay"].ToString());
                }
            }

            return decReturn;
        }

        /// <summary>
        /// 使用折扣模板
        /// </summary>
        public static void AddBillDiscount(ref DataTable dtOrderDish, DataTable dtDiscountSchemeRate, DataTable dtBill)
        {
            //一般折扣率
            decimal DiscountRate = 0;
            string DiscountName = string.Empty;
            if (dtDiscountSchemeRate != null && dtDiscountSchemeRate.Rows.Count > 0)
            {
                //获取折扣名称
                DiscountName = dtDiscountSchemeRate.Rows[0]["SchName"].ToString();
                DiscountRate = Helper.StringToDecimal(dtDiscountSchemeRate.Rows[0]["NDiscountRate"].ToString());
                //执行折扣模板
                GetDiscountByDishes(ref dtOrderDish, dtDiscountSchemeRate, DiscountRate);
            }
            
        }

        /// <summary>
        /// 取消折扣模板
        /// </summary>
        /// <param name="dtOrderDish"></param>
        /// <param name="bolMemPrice"></param>
        public static void CancelDiscount(ref DataTable dtOrderDish)
        {
            //过滤时价特价、商品券、次卡菜品、会员价
            DataRow[] drs = dtOrderDish.Select("DiscountType not in ('1','3','4','5','6')");
            if (dtOrderDish != null)
            {
                foreach (DataRow dr in drs)
                {
                    //菜品折扣类型
                    string DiscountType = dr["DiscountType"].ToString();
                    if (DiscountType == "2")//没有进行任何折扣
                    {
                        dr["DiscountType"] = "";
                        dr["DiscountPrice"] = dr["Price"];
                    }
                }
            }
        }

        /// <summary>
        /// 根据账单设置菜品
        /// </summary>
        /// <param name="dtOrderDish"></param>
        public void SetDishDiscountByBill(ref DataTable dtOrderDish,DataTable dtBill)
        {
            if (dtBill != null && dtBill.Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(dtBill.Rows[0]["DiscountName"].ToString()) && Helper.StringToDecimal(dtBill.Rows[0]["DiscountMoney"].ToString()) == 0)
                {
                    if (dtOrderDish != null)
                    {
                        foreach (DataRow dr in dtOrderDish.Rows)
                        {
                            if (dr["DiscountType"].ToString() == "2" || dr["DiscountType"].ToString() == "1")
                            {
                                dr["DiscountPrice"] = dr["Price"];
                                dr["DiscountType"] = "";
                            }
                        }
                    }
                }
            }
        }

    }
}
