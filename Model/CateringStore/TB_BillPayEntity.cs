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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_BillPay_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 4, NotEmptyECode = "TB_BillPay_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillPay_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillPay_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_014")]
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
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_BillPay_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///支付编号
		/// <summary>
		[ModelInfo(Name = "支付编号",ControlName="txt_PKCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillPay_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_023")]
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///账单编号
		/// <summary>
		[ModelInfo(Name = "账单编号",ControlName="txt_BillCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillPay_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_026")]
		public string BillCode
		{
			get { return _BillCode; }
			set { _BillCode = value; }
		}
		/// <summary>
		///支付金额
		/// <summary>
		[ModelInfo(Name = "支付金额",ControlName="txt_PayMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_BillPay_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_029")]
		public decimal PayMoney
		{
			get { return _PayMoney; }
			set { _PayMoney = value; }
		}
		/// <summary>
		///支付方式名称
		/// <summary>
		[ModelInfo(Name = "支付方式名称",ControlName="txt_PayMethodName", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillPay_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_032")]
		public string PayMethodName
		{
			get { return _PayMethodName; }
			set { _PayMethodName = value; }
		}
		/// <summary>
		///支付方式编号
		/// <summary>
		[ModelInfo(Name = "支付方式编号",ControlName="txt_PayMethodCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillPay_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_035")]
		public string PayMethodCode
		{
			get { return _PayMethodCode; }
			set { _PayMethodCode = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_Remar", NotEmpty = false, Length = 64, NotEmptyECode = "TB_BillPay_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_038")]
		public string Remar
		{
			get { return _Remar; }
			set { _Remar = value; }
		}
		/// <summary>
		///交易流水号
		/// <summary>
		[ModelInfo(Name = "交易流水号",ControlName="txt_OutOrderCode", NotEmpty = false, Length = 128, NotEmptyECode = "TB_BillPay_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_041")]
		public string OutOrderCode
		{
			get { return _OutOrderCode; }
			set { _OutOrderCode = value; }
		}
		/// <summary>
		///原支付标号
		/// <summary>
		[ModelInfo(Name = "原支付标号",ControlName="txt_PPKCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillPay_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillPay_044")]
		public string PPKCode
		{
			get { return _PPKCode; }
			set { _PPKCode = value; }
		}        
    }
}