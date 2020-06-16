using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("退单信息")]
    [Serializable]
    public class TB_BackOrderEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _AuthCode = string.Empty;
		private string _AuthName = string.Empty;
		private string _TStatus = string.Empty;
		private string _OrderCode = string.Empty;
		private string _OrderDisCode = string.Empty;
		private string _ReasonCode = string.Empty;
		private string _ReasonName = string.Empty;
		private string _Remar = string.Empty;
		private decimal _BackNum = 0;

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_BackOrder_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 4, NotEmptyECode = "TB_BackOrder_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BackOrder_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BackOrder_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_014")]
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
		///授权人编号
		/// <summary>
		[ModelInfo(Name = "授权人编号",ControlName="txt_AuthCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BackOrder_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_020")]
		public string AuthCode
		{
			get { return _AuthCode; }
			set { _AuthCode = value; }
		}
		/// <summary>
		///授权人姓名
		/// <summary>
		[ModelInfo(Name = "授权人姓名",ControlName="txt_AuthName", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BackOrder_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_023")]
		public string AuthName
		{
			get { return _AuthName; }
			set { _AuthName = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_BackOrder_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_026")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///订单编号
		/// <summary>
		[ModelInfo(Name = "订单编号",ControlName="txt_OrderCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BackOrder_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_029")]
		public string OrderCode
		{
			get { return _OrderCode; }
			set { _OrderCode = value; }
		}
		/// <summary>
		///点单菜品编号
		/// <summary>
		[ModelInfo(Name = "点单菜品编号",ControlName="txt_OrderDisCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BackOrder_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_032")]
		public string OrderDisCode
		{
			get { return _OrderDisCode; }
			set { _OrderDisCode = value; }
		}
		/// <summary>
		///退单原因编号
		/// <summary>
		[ModelInfo(Name = "退单原因编号",ControlName="txt_ReasonCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BackOrder_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_035")]
		public string ReasonCode
		{
			get { return _ReasonCode; }
			set { _ReasonCode = value; }
		}
		/// <summary>
		///退单原因
		/// <summary>
		[ModelInfo(Name = "退单原因",ControlName="txt_ReasonName", NotEmpty = false, Length = 64, NotEmptyECode = "TB_BackOrder_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_038")]
		public string ReasonName
		{
			get { return _ReasonName; }
			set { _ReasonName = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_Remar", NotEmpty = false, Length = 64, NotEmptyECode = "TB_BackOrder_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_041")]
		public string Remar
		{
			get { return _Remar; }
			set { _Remar = value; }
		}
		/// <summary>
		///退菜数量
		/// <summary>
		[ModelInfo(Name = "退菜数量",ControlName="txt_BackNum", NotEmpty = false, Length = 18, NotEmptyECode = "TB_BackOrder_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackOrder_044")]
		public decimal BackNum
		{
			get { return _BackNum; }
			set { _BackNum = value; }
		}        
    }
}