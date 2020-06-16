using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("用户签送方案关联表")]
    [Serializable]
    public class TB_UserSigSchemeEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _SigCode = string.Empty;
		private decimal _DayMoney = 0;
		private string _UserCode = string.Empty;

		/// <summary>
		///Id主键自增
		/// <summary>
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_UserSigScheme_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserSigScheme_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_UserSigScheme_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserSigScheme_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_UserSigScheme_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserSigScheme_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_UserSigScheme_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserSigScheme_014")]
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
		///签送方案编号
		/// <summary>
		[ModelInfo(Name = "签送方案编号",ControlName="txt_SigCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_UserSigScheme_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserSigScheme_020")]
		public string SigCode
		{
			get { return _SigCode; }
			set { _SigCode = value; }
		}
		/// <summary>
		///日额度
		/// <summary>
		[ModelInfo(Name = "日额度",ControlName="txt_DayMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_UserSigScheme_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserSigScheme_023")]
		public decimal DayMoney
		{
			get { return _DayMoney; }
			set { _DayMoney = value; }
		}
		/// <summary>
		///用户id
		/// <summary>
		[ModelInfo(Name = "用户id",ControlName="txt_UserCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_UserSigScheme_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserSigScheme_026")]
		public string UserCode
		{
			get { return _UserCode; }
			set { _UserCode = value; }
		}        
    }
}