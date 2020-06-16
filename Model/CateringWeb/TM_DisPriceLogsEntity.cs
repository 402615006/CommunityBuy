using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("菜价修改记录表")]
    [Serializable]
    public class TM_DisPriceLogsEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _UpdateReason = string.Empty;
		private string _DisCode = string.Empty;
		private string _MenuCode = string.Empty;
		private decimal _OldPrice = 0;
		private decimal _NewPrice = 0;
		private decimal _OldMemPrice = 0;
		private decimal _NewMemPrice = 0;
		private string _TStatus = string.Empty;
		private string _AStatus = string.Empty;

        #region 附加属性
        private string _dispricelogsJson = string.Empty;
        public string dispricelogsJson
        {
            get { return _dispricelogsJson; }
            set { _dispricelogsJson = value; }
        }
        #endregion

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TM_DisPriceLogs_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TM_DisPriceLogs_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TM_DisPriceLogs_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TM_DisPriceLogs_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_014")]
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
		///修改原因
		/// <summary>
		[ModelInfo(Name = "修改原因",ControlName="txt_UpdateReason", NotEmpty = false, Length = 128, NotEmptyECode = "TM_DisPriceLogs_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_020")]
		public string UpdateReason
		{
			get { return _UpdateReason; }
			set { _UpdateReason = value; }
		}
		/// <summary>
		///菜品编号
		/// <summary>
		[ModelInfo(Name = "菜品编号",ControlName="txt_DisCode", NotEmpty = false, Length = 32, NotEmptyECode = "TM_DisPriceLogs_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_023")]
		public string DisCode
		{
			get { return _DisCode; }
			set { _DisCode = value; }
		}
		/// <summary>
		///菜谱
		/// <summary>
		[ModelInfo(Name = "菜谱",ControlName="txt_MenuCode", NotEmpty = false, Length = 32, NotEmptyECode = "TM_DisPriceLogs_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_026")]
		public string MenuCode
		{
			get { return _MenuCode; }
			set { _MenuCode = value; }
		}
		/// <summary>
		///原价
		/// <summary>
		[ModelInfo(Name = "原价",ControlName="txt_OldPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TM_DisPriceLogs_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_029")]
		public decimal OldPrice
		{
			get { return _OldPrice; }
			set { _OldPrice = value; }
		}
		/// <summary>
		///现价
		/// <summary>
		[ModelInfo(Name = "现价",ControlName="txt_NewPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TM_DisPriceLogs_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_032")]
		public decimal NewPrice
		{
			get { return _NewPrice; }
			set { _NewPrice = value; }
		}
		/// <summary>
		///会员原价
		/// <summary>
		[ModelInfo(Name = "会员原价",ControlName="txt_OldMemPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TM_DisPriceLogs_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_035")]
		public decimal OldMemPrice
		{
			get { return _OldMemPrice; }
			set { _OldMemPrice = value; }
		}
		/// <summary>
		///会员现价
		/// <summary>
		[ModelInfo(Name = "会员现价",ControlName="txt_NewMemPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TM_DisPriceLogs_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_038")]
		public decimal NewMemPrice
		{
			get { return _NewMemPrice; }
			set { _NewMemPrice = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TM_DisPriceLogs_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_041")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///审核状态
		/// <summary>
		[ModelInfo(Name = "审核状态",ControlName="txt_AStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TM_DisPriceLogs_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TM_DisPriceLogs_044")]
		public string AStatus
		{
			get { return _AStatus; }
			set { _AStatus = value; }
		}        
    }
}