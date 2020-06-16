using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("")]
    [Serializable]
    public class TB_UserDiscountSchemeEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _DisCountCode = string.Empty;
		private string _UserCode = string.Empty;

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
		[ModelInfo(Name = "",ControlName="txt_BusCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_UserDiscountScheme_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserDiscountScheme_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_StoCode", NotEmpty = false, Length = 4, NotEmptyECode = "TB_UserDiscountScheme_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserDiscountScheme_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_CCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_UserDiscountScheme_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserDiscountScheme_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_UserDiscountScheme_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserDiscountScheme_014")]
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
		[ModelInfo(Name = "",ControlName="txt_DisCountCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_UserDiscountScheme_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserDiscountScheme_020")]
		public string DisCountCode
		{
			get { return _DisCountCode; }
			set { _DisCountCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_UserCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_UserDiscountScheme_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserDiscountScheme_023")]
		public string UserCode
		{
			get { return _UserCode; }
			set { _UserCode = value; }
		}        
    }
}