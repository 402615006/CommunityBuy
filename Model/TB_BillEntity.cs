using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("账单")]
    [Serializable]
    public class TB_BillEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _OrderCodeList = string.Empty;
		private string _PKCode = string.Empty;
		private decimal _BillMoney = 0;
		private decimal _PayMoney = 0;
		private decimal _ZeroCutMoney = 0;
        private string _Remar = string.Empty;
		private DateTime _FTime = DateTime.Parse("1900-01-01");
		private DateTime _OpenDate = DateTime.Parse("1900-01-01");
		private decimal _DiscountMoney = 0;
		private decimal _AUCode = 0;
		private decimal _PointMoney = 0;
		private decimal _VirMoney = 0;
        private string _BillType = string.Empty;

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
		///订单编号
		/// <summary>
		public string OrderCodeList
		{
			get { return _OrderCodeList; }
			set { _OrderCodeList = value; }
		}
		/// <summary>
		///账单编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///账单金额
		/// <summary>
		public decimal BillMoney
		{
			get { return _BillMoney; }
			set { _BillMoney = value; }
		}
		/// <summary>
		///支付金额
		/// <summary>
		public decimal PayMoney
		{
			get { return _PayMoney; }
			set { _PayMoney = value; }
		}

		/// <summary>
		///备注
		/// <summary>
		public string Remar
		{
			get { return _Remar; }
			set { _Remar = value; }
		}
		/// <summary>
		///完成时间
		/// <summary>
		public DateTime FTime
		{
			get { return _FTime; }
			set { _FTime = value; }
		}
		/// <summary>
		///账单营业日
		/// <summary>
		public DateTime OpenDate
		{
			get { return _OpenDate; }
			set { _OpenDate = value; }
		}

        /// <summary>
        ///账单类型
        /// <summary>
        public string BillType
        {
            get { return _BillType; }
            set { _BillType = value; }
        }
    }
}