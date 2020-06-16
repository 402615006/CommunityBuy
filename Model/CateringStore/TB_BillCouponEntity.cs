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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_BillCoupon_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 4, NotEmptyECode = "TB_BillCoupon_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillCoupon_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillCoupon_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_014")]
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
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_BillCoupon_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///账单编号
		/// <summary>
		[ModelInfo(Name = "账单编号",ControlName="txt_BillCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillCoupon_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_023")]
		public string BillCode
		{
			get { return _BillCode; }
			set { _BillCode = value; }
		}
		/// <summary>
		///优惠券编号
		/// <summary>
		[ModelInfo(Name = "优惠券编号",ControlName="txt_CouponCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillCoupon_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_026")]
		public string CouponCode
		{
			get { return _CouponCode; }
			set { _CouponCode = value; }
		}
		/// <summary>
		///优惠券金额
		/// <summary>
		[ModelInfo(Name = "优惠券金额",ControlName="txt_CouponMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_BillCoupon_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_029")]
		public decimal CouponMoney
		{
			get { return _CouponMoney; }
			set { _CouponMoney = value; }
		}
		/// <summary>
		///会员卡编号
		/// <summary>
		[ModelInfo(Name = "会员卡编号",ControlName="txt_MemberCardCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillCoupon_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_032")]
		public string MemberCardCode
		{
			get { return _MemberCardCode; }
			set { _MemberCardCode = value; }
		}
		/// <summary>
		///使用金额
		/// <summary>
		[ModelInfo(Name = "使用金额",ControlName="txt_RealPay", NotEmpty = false, Length = 18, NotEmptyECode = "TB_BillCoupon_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_035")]
		public decimal RealPay
		{
			get { return _RealPay; }
			set { _RealPay = value; }
		}
		/// <summary>
		///虚增金额
		/// <summary>
		[ModelInfo(Name = "虚增金额",ControlName="txt_VIMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_BillCoupon_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_038")]
		public decimal VIMoney
		{
			get { return _VIMoney; }
			set { _VIMoney = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_Remark", NotEmpty = false, Length = 64, NotEmptyECode = "TB_BillCoupon_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_041")]
		public string Remark
		{
			get { return _Remark; }
			set { _Remark = value; }
		}
		/// <summary>
		///使用类型1普通使用2会员卡优惠券
		/// <summary>
		[ModelInfo(Name = "使用类型1普通使用2会员卡优惠券",ControlName="txt_UseType", NotEmpty = false, Length = 1, NotEmptyECode = "TB_BillCoupon_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_044")]
		public string UseType
		{
			get { return _UseType; }
			set { _UseType = value; }
		}
		/// <summary>
		///班次号
		/// <summary>
		[ModelInfo(Name = "班次号",ControlName="txt_ShiftCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillCoupon_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_047")]
		public string ShiftCode
		{
			get { return _ShiftCode; }
			set { _ShiftCode = value; }
		}

        /// <summary>
        ///优惠券名称
        /// <summary>
        [ModelInfo(Name = "优惠券名称", ControlName = "", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillCoupon_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_050")]
        public string CouponName
        {
            get { return _CouponName; }
            set { _CouponName = value; }
        }

        /// <summary>
        ///优惠券类型
        /// <summary>
        [ModelInfo(Name = "优惠券类型", ControlName = "", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillCoupon_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_050")]
        public string McCode
        {
            get { return _McCode; }
            set { _McCode = value; }
        }

        /// <summary>
        ///优惠券类型
        /// <summary>
        [ModelInfo(Name = "优惠券类型", ControlName = "", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillCoupon_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_050")]
        public string TicType
        {
            get { return _TicType; }
            set { _TicType = value; }
        }

        /// <summary>
        ///优惠券类型
        /// <summary>
        [ModelInfo(Name = "优惠券类型", ControlName = "", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillCoupon_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillCoupon_050")]
        public string TicWay
        {
            get { return _TicWay; }
            set { _TicWay = value; }
        }
    }
}