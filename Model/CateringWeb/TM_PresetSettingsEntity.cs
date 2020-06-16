using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("预定设置")]
    [Serializable]
    public class TM_PresetSettingsEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private int _Sort = 0;
		private string _IsPublicPreset = string.Empty;
		private string _PresetDate = string.Empty;
		private string _PresetType = string.Empty;
		private string _PKCode = string.Empty;

		/// <summary>
		///Id
		/// <summary>
		[ModelInfo(Name = "Id",ControlName="txt_Id", NotEmpty = false, Length = 18, NotEmptyECode = "TM_PresetSettings_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_002")]
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TM_PresetSettings_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TM_PresetSettings_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TM_PresetSettings_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TM_PresetSettings_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_014")]
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
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TM_PresetSettings_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///排序号
		/// <summary>
		[ModelInfo(Name = "排序号",ControlName="txt_Sort", NotEmpty = false, Length = 9, NotEmptyECode = "TM_PresetSettings_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_023")]
		public int Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		///公共假期是否可预定
		/// <summary>
		[ModelInfo(Name = "公共假期是否可预定",ControlName="txt_IsPublicPreset", NotEmpty = false, Length = 1, NotEmptyECode = "TM_PresetSettings_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_026")]
		public string IsPublicPreset
		{
			get { return _IsPublicPreset; }
			set { _IsPublicPreset = value; }
		}
		/// <summary>
		///不可预定日期
		/// <summary>
		[ModelInfo(Name = "不可预定日期",ControlName="txt_PresetDate", NotEmpty = false, Length = -1, NotEmptyECode = "TM_PresetSettings_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_029")]
		public string PresetDate
		{
			get { return _PresetDate; }
			set { _PresetDate = value; }
		}
		/// <summary>
		///预定状态
		/// <summary>
		[ModelInfo(Name = "预定状态",ControlName="txt_PresetType", NotEmpty = false, Length = 1, NotEmptyECode = "TM_PresetSettings_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_PresetSettings_032")]
		public string PresetType
		{
			get { return _PresetType; }
			set { _PresetType = value; }
		}
		/// <summary>
		///预定设置编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}        
    }
}