using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("退单原因管理")]
    [Serializable]
    public class TB_BackReasonEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _UCode = string.Empty;
		private string _UCname = string.Empty;
		private DateTime _UTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private int _Sort = 0;
		private string _PKCode = string.Empty;
		private string _Reason = string.Empty;
		private string _Ascription = string.Empty;
		private string _Remark = string.Empty;

		/// <summary>
		///Id
		/// <summary>
		[ModelInfo(Name = "Id",ControlName="txt_Id", NotEmpty = false, Length = 18, NotEmptyECode = "TB_BackReason_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_002")]
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_BackReason_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_BackReason_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BackReason_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BackReason_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_014")]
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
		///记录修改人编码
		/// <summary>
		[ModelInfo(Name = "记录修改人编码",ControlName="txt_UCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BackReason_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_020")]
		public string UCode
		{
			get { return _UCode; }
			set { _UCode = value; }
		}
		/// <summary>
		///记录修改人姓名
		/// <summary>
		[ModelInfo(Name = "记录修改人姓名",ControlName="txt_UCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BackReason_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_023")]
		public string UCname
		{
			get { return _UCname; }
			set { _UCname = value; }
		}
		/// <summary>
		///记录修改时间
		/// <summary>
		public DateTime UTime
		{
			get { return _UTime; }
			set { _UTime = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_BackReason_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_029")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///排序号
		/// <summary>
		[ModelInfo(Name = "排序号",ControlName="txt_Sort", NotEmpty = false, Length = 9, NotEmptyECode = "TB_BackReason_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_032")]
		public int Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		///原因编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///退单原因
		/// <summary>
		[ModelInfo(Name = "退单原因",ControlName="txt_Reason", NotEmpty = false, Length = 32, NotEmptyECode = "TB_BackReason_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_038")]
		public string Reason
		{
			get { return _Reason; }
			set { _Reason = value; }
		}
		/// <summary>
		///责任归属
		/// <summary>
		[ModelInfo(Name = "责任归属",ControlName="txt_Ascription", NotEmpty = false, Length = 8, NotEmptyECode = "TB_BackReason_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_041")]
		public string Ascription
		{
			get { return _Ascription; }
			set { _Ascription = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_Remark", NotEmpty = false, Length = 128, NotEmptyECode = "TB_BackReason_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_BackReason_044")]
		public string Remark
		{
			get { return _Remark; }
			set { _Remark = value; }
		}        
    }
}