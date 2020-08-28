using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("订单")]
    [Serializable]
    public class TB_OrderEntity
    {
		private long _Id = 0;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _PKCode = string.Empty;
		private decimal _OrderMoney = 0;
		private string _Remar = string.Empty;
		private DateTime _CheckTime = DateTime.Parse("1900-01-01");
		private decimal _DisNum = 0;
		private int _OrderType = 1;
		private decimal _PayMoney = 0;
		private decimal _CouponMoney = 0;
		private string _WxBillCode = string.Empty;
		private string _CouponCode = string.Empty;
		private DateTime _FTime = DateTime.Parse("1900-01-01");

		/// <summary>
		///Id
		/// <summary>
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
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
		///订单编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///订单金额
		/// <summary>
		public decimal OrderMoney
		{
			get { return _OrderMoney; }
			set { _OrderMoney = value; }
		}

		/// <summary>
		///下单备注
		/// <summary>
		public string Remar
		{
			get { return _Remar; }
			set { _Remar = value; }
		}
		/// <summary>
		///结账时间
		/// <summary>
		public DateTime CheckTime
		{
			get { return _CheckTime; }
			set { _CheckTime = value; }
		}

        public decimal DisNum { get => _DisNum; set => _DisNum = value; }
        public int OrderType { get => _OrderType; set => _OrderType = value; }
        public decimal PayMoney { get => _PayMoney; set => _PayMoney = value; }
        public decimal CouponMoney { get => _CouponMoney; set => _CouponMoney = value; }
        public string WxBillCode { get => _WxBillCode; set => _WxBillCode = value; }
        public string CouponCode { get => _CouponCode; set => _CouponCode = value; }
        public DateTime FTime { get => _FTime; set => _FTime = value; }
    }
}