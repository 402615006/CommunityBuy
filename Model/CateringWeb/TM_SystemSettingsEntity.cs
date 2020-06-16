using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("系统设置")]
    [Serializable]
    public class TM_SystemSettingsEntity
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
		private string _AStatus = string.Empty;
		private string _KeyName = string.Empty;
		private string _DataValue = string.Empty;

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TM_SystemSettings_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TM_SystemSettings_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TM_SystemSettings_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TM_SystemSettings_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_014")]
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
		[ModelInfo(Name = "记录修改人编码",ControlName="txt_UCode", NotEmpty = false, Length = 32, NotEmptyECode = "TM_SystemSettings_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_020")]
		public string UCode
		{
			get { return _UCode; }
			set { _UCode = value; }
		}
		/// <summary>
		///记录修改人姓名
		/// <summary>
		[ModelInfo(Name = "记录修改人姓名",ControlName="txt_UCname", NotEmpty = false, Length = 32, NotEmptyECode = "TM_SystemSettings_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_023")]
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
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TM_SystemSettings_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_029")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///审核状态
		/// <summary>
		[ModelInfo(Name = "审核状态",ControlName="txt_AStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TM_SystemSettings_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_032")]
		public string AStatus
		{
			get { return _AStatus; }
			set { _AStatus = value; }
		}
		/// <summary>
		///是否开启排队
		/// <summary>
		[ModelInfo(Name = "是否开启排队",ControlName="txt_IsLineUp", NotEmpty = false, Length = 1, NotEmptyECode = "TM_SystemSettings_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_035")]
		public string KeyName
        {
			get { return _KeyName; }
			set { _KeyName = value; }
		}
		/// <summary>
		///抹零方式
		/// <summary>
		[ModelInfo(Name = "抹零方式",ControlName="txt_SmallChangeType", NotEmpty = false, Length = 1, NotEmptyECode = "TM_SystemSettings_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_SystemSettings_038")]
		public string DataValue
        {
			get { return _DataValue; }
			set { _DataValue = value; }
		}        
    }
}