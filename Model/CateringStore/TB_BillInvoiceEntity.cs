using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("")]
    [Serializable]
    public class TB_BillInvoiceEntity
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
		private decimal _InMoney = 0;
		private string _CardMoney = string.Empty;
		private decimal _OtherMoney = 0;

		/// <summary>
		///
		/// <summary>
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BillInvoice_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_StoCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillInvoice_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillInvoice_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillInvoice_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_014")]
		public string CCname
		{
			get { return _CCname; }
			set { _CCname = value; }
		}
		/// <summary>
		///
		/// <summary>
		public DateTime CTime
		{
			get { return _CTime; }
			set { _CTime = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_BillInvoice_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_PKCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillInvoice_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_023")]
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_BillCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BillInvoice_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_026")]
		public string BillCode
		{
			get { return _BillCode; }
			set { _BillCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_InMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_BillInvoice_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_029")]
		public decimal InMoney
		{
			get { return _InMoney; }
			set { _InMoney = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_CardMoney", NotEmpty = false, Length = -1, NotEmptyECode = "TB_BillInvoice_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_032")]
		public string CardMoney
		{
			get { return _CardMoney; }
			set { _CardMoney = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_OtherMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_BillInvoice_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BillInvoice_035")]
		public decimal OtherMoney
		{
			get { return _OtherMoney; }
			set { _OtherMoney = value; }
		}        
    }
}