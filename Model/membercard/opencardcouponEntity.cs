using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("发卡临时优惠券表")]
    public class opencardcouponEntity
    {
		private long _cid = 0;
		private string _ordercode = string.Empty;
		private string _buscode = string.Empty;
		private string _stocode = string.Empty;
		private string _memcode = string.Empty;
		private string _cardcode = string.Empty;
		private string _pcode = string.Empty;
		private string _sumcode = string.Empty;
		private string _mccode = string.Empty;
		private long _num = 0;
		private DateTime _ctime = DateTime.Parse("1900-01-01");

		/// <summary>
		///
		/// <summary>
		public long cid
		{
			get { return _cid; }
			set { _cid = value; }
		}
		/// <summary>
		///发卡编号
		/// <summary>
		[ModelInfo(Name = "发卡编号",ControlName="txt_ordercode", NotEmpty = false, Length = 32, NotEmptyECode = "opencardcoupon_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardcoupon_005")]
		public string ordercode
		{
			get { return _ordercode; }
			set { _ordercode = value; }
		}
		/// <summary>
		///商户编号
		/// <summary>
		[ModelInfo(Name = "商户编号",ControlName="txt_buscode", NotEmpty = false, Length = 16, NotEmptyECode = "opencardcoupon_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardcoupon_008")]
		public string buscode
		{
			get { return _buscode; }
			set { _buscode = value; }
		}
		/// <summary>
		///所属门店
		/// <summary>
		[ModelInfo(Name = "所属门店",ControlName="txt_stocode", NotEmpty = false, Length = 8, NotEmptyECode = "opencardcoupon_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardcoupon_011")]
		public string stocode
		{
			get { return _stocode; }
			set { _stocode = value; }
		}
		/// <summary>
		///会员编号
		/// <summary>
		[ModelInfo(Name = "会员编号",ControlName="txt_memcode", NotEmpty = false, Length = 32, NotEmptyECode = "opencardcoupon_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardcoupon_014")]
		public string memcode
		{
			get { return _memcode; }
			set { _memcode = value; }
		}
		/// <summary>
		///会员卡号
		/// <summary>
		[ModelInfo(Name = "会员卡号",ControlName="txt_cardcode", NotEmpty = false, Length = 32, NotEmptyECode = "opencardcoupon_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardcoupon_017")]
		public string cardcode
		{
			get { return _cardcode; }
			set { _cardcode = value; }
		}
		/// <summary>
		///方案编号
		/// <summary>
		[ModelInfo(Name = "方案编号",ControlName="txt_pcode", NotEmpty = false, Length = 16, NotEmptyECode = "opencardcoupon_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardcoupon_020")]
		public string pcode
		{
			get { return _pcode; }
			set { _pcode = value; }
		}
		/// <summary>
		///活动编号
		/// <summary>
		[ModelInfo(Name = "活动编号",ControlName="txt_sumcode", NotEmpty = false, Length = 32, NotEmptyECode = "opencardcoupon_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardcoupon_023")]
		public string sumcode
		{
			get { return _sumcode; }
			set { _sumcode = value; }
		}
		/// <summary>
		///优惠券编号
		/// <summary>
		[ModelInfo(Name = "优惠券编号",ControlName="txt_mccode", NotEmpty = false, Length = 32, NotEmptyECode = "opencardcoupon_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardcoupon_026")]
		public string mccode
		{
			get { return _mccode; }
			set { _mccode = value; }
		}
		/// <summary>
		///数量
		/// <summary>
		[ModelInfo(Name = "数量",ControlName="txt_num", NotEmpty = false, Length = 9, NotEmptyECode = "opencardcoupon_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardcoupon_029")]
        public long num
		{
			get { return _num; }
			set { _num = value; }
		}
		/// <summary>
		///操作时间
		/// <summary>
		public DateTime ctime
		{
			get { return _ctime; }
			set { _ctime = value; }
		}        
    }
}