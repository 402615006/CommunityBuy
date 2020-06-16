using System;
using System.ComponentModel;
using XJWZCatering.CommonBasic;
namespace CateringYun.Model
{
    [Description("折扣方案特殊折扣率")]
    [Serializable]
    public class TR_DiscountSchemeRateEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private int _Sort = 0;
		private string _SchCode = string.Empty;
		private string _DiscountType = string.Empty;
		private string _DisTypeCode = string.Empty;
		private string _DisCode = string.Empty;
		private string _DisMetCode = string.Empty;
		private decimal _DiscountRate = 0;

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TR_DiscountSchemeRate_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TR_DiscountSchemeRate_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TR_DiscountSchemeRate_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TR_DiscountSchemeRate_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_014")]
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
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TR_DiscountSchemeRate_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///排序号
		/// <summary>
		[ModelInfo(Name = "排序号",ControlName="txt_Sort", NotEmpty = false, Length = 9, NotEmptyECode = "TR_DiscountSchemeRate_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_023")]
		public int Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		///所属折扣方案编号
		/// <summary>
		[ModelInfo(Name = "所属折扣方案编号",ControlName="txt_SchCode", NotEmpty = false, Length = 32, NotEmptyECode = "TR_DiscountSchemeRate_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_026")]
		public string SchCode
		{
			get { return _SchCode; }
			set { _SchCode = value; }
		}
		/// <summary>
		///折扣类型
		/// <summary>
		[ModelInfo(Name = "折扣类型",ControlName="txt_DiscountType", NotEmpty = false, Length = 1, NotEmptyECode = "TR_DiscountSchemeRate_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_029")]
		public string DiscountType
		{
			get { return _DiscountType; }
			set { _DiscountType = value; }
		}
		/// <summary>
		///菜品类别编号
		/// <summary>
		[ModelInfo(Name = "菜品类别编号",ControlName="txt_DisTypeCode", NotEmpty = false, Length = 32, NotEmptyECode = "TR_DiscountSchemeRate_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_032")]
		public string DisTypeCode
		{
			get { return _DisTypeCode; }
			set { _DisTypeCode = value; }
		}
		/// <summary>
		///菜品编号
		/// <summary>
		[ModelInfo(Name = "菜品编号",ControlName="txt_DisCode", NotEmpty = false, Length = 32, NotEmptyECode = "TR_DiscountSchemeRate_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_035")]
		public string DisCode
		{
			get { return _DisCode; }
			set { _DisCode = value; }
		}
		/// <summary>
		///做法编号
		/// <summary>
		[ModelInfo(Name = "做法编号",ControlName="txt_DisMetCode", NotEmpty = false, Length = 32, NotEmptyECode = "TR_DiscountSchemeRate_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_038")]
		public string DisMetCode
		{
			get { return _DisMetCode; }
			set { _DisMetCode = value; }
		}
		/// <summary>
		///折扣率
		/// <summary>
		[ModelInfo(Name = "折扣率",ControlName="txt_DiscountRate", NotEmpty = false, Length = 18, NotEmptyECode = "TR_DiscountSchemeRate_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DiscountSchemeRate_041")]
		public decimal DiscountRate
		{
			get { return _DiscountRate; }
			set { _DiscountRate = value; }
		}        
    }
}