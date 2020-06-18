using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("账单优惠券")]
    [Serializable]
    public class TB_BillCouponEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _BillCode = string.Empty;
		private string _CouponCode = string.Empty;
		private decimal _CouponMoney = 0;
		private string _MemberCardCode = string.Empty;
		private decimal _RealPay = 0;
		private decimal _VIMoney = 0;
		private string _Remark = string.Empty;
		private string _UseType = string.Empty;
		private string _ShiftCode = string.Empty;
		private string _CouponName = string.Empty;
        private string _McCode = string.Empty;
        private string _TicType = string.Empty;
        private string _TicWay = string.Empty;
        /// <summary>
        ///Id
        /// <summary>
        public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		public string CCname
		{
			get { return _CCname; }
			set { _CCname = value; }
		}
		/// <summary>
		///记录创建时间
		/// <summary>
		public DateTime CTime
		{
			get { return _CTime; }
			set { _CTime = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///账单编号
		/// <summary>
		public string BillCode
		{
			get { return _BillCode; }
			set { _BillCode = value; }
		}
		/// <summary>
		///优惠券编号
		/// <summary>
		public string CouponCode
		{
			get { return _CouponCode; }
			set { _CouponCode = value; }
		}
		/// <summary>
		///优惠券金额
		/// <summary>
		public decimal CouponMoney
		{
			get { return _CouponMoney; }
			set { _CouponMoney = value; }
		}
		/// <summary>
		///会员卡编号
		/// <summary>
		public string MemberCardCode
		{
			get { return _MemberCardCode; }
			set { _MemberCardCode = value; }
		}
		/// <summary>
		///使用金额
		/// <summary>
		public decimal RealPay
		{
			get { return _RealPay; }
			set { _RealPay = value; }
		}
		/// <summary>
		///虚增金额
		/// <summary>
		public decimal VIMoney
		{
			get { return _VIMoney; }
			set { _VIMoney = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		public string Remark
		{
			get { return _Remark; }
			set { _Remark = value; }
		}
		/// <summary>
		///使用类型1普通使用2会员卡优惠券
		/// <summary>
		public string UseType
		{
			get { return _UseType; }
			set { _UseType = value; }
		}
		/// <summary>
		///班次号
		/// <summary>
		public string ShiftCode
		{
			get { return _ShiftCode; }
			set { _ShiftCode = value; }
		}

        /// <summary>
        ///优惠券名称
        /// <summary>
        public string CouponName
        {
            get { return _CouponName; }
            set { _CouponName = value; }
        }

        /// <summary>
        ///优惠券类型
        /// <summary>
        public string McCode
        {
            get { return _McCode; }
            set { _McCode = value; }
        }

        /// <summary>
        ///优惠券类型
        /// <summary>
        public string TicType
        {
            get { return _TicType; }
            set { _TicType = value; }
        }

        /// <summary>
        ///优惠券类型
        /// <summary>
        public string TicWay
        {
            get { return _TicWay; }
            set { _TicWay = value; }
        }
    }
}