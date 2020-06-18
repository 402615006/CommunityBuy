using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("账单支付")]
    [Serializable]
    public class TB_BillPayEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _PKCode = string.Empty;
		private string _BillCode = string.Empty;
		private decimal _PayMoney = 0;
		private string _PayMethodName = string.Empty;
		private string _PayMethodCode = string.Empty;
		private string _Remar = string.Empty;
		private string _OutOrderCode = string.Empty;
		private string _PPKCode = string.Empty;

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
		///支付编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
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
		///支付金额
		/// <summary>
		public decimal PayMoney
		{
			get { return _PayMoney; }
			set { _PayMoney = value; }
		}
		/// <summary>
		///支付方式名称
		/// <summary>
		public string PayMethodName
		{
			get { return _PayMethodName; }
			set { _PayMethodName = value; }
		}
		/// <summary>
		///支付方式编号
		/// <summary>
		public string PayMethodCode
		{
			get { return _PayMethodCode; }
			set { _PayMethodCode = value; }
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
		///交易流水号
		/// <summary>
		public string OutOrderCode
		{
			get { return _OutOrderCode; }
			set { _OutOrderCode = value; }
		}
		/// <summary>
		///原支付标号
		/// <summary>
		public string PPKCode
		{
			get { return _PPKCode; }
			set { _PPKCode = value; }
		}        
    }
}