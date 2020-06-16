using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("折扣方案")]
    [Serializable]
    public class TB_DiscountSchemeEntity
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
		private string _InsideCode = string.Empty;
		private decimal _DiscountRate = 0;
		private string _MenuCode = string.Empty;
		private string _LevelCode = string.Empty;
		private string _SchName = string.Empty;


        private List<TR_DiscountSchemeRateEntity> _DSRateList = new List<TR_DiscountSchemeRateEntity>();
        /// <summary>
        /// 特殊折扣
        /// </summary>
        public List<TR_DiscountSchemeRateEntity> DSRateList
        {
            get { return _DSRateList; }
            set { _DSRateList = value; }
        }


        /// <summary>
        ///Id
        /// <summary>
        [ModelInfo(Name = "Id",ControlName="txt_Id", NotEmpty = false, Length = 18, NotEmptyECode = "TB_DiscountScheme_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_002")]
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_DiscountScheme_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_DiscountScheme_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_DiscountScheme_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_DiscountScheme_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_014")]
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
		[ModelInfo(Name = "记录修改人编码",ControlName="txt_UCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_DiscountScheme_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_020")]
		public string UCode
		{
			get { return _UCode; }
			set { _UCode = value; }
		}
		/// <summary>
		///记录修改人姓名
		/// <summary>
		[ModelInfo(Name = "记录修改人姓名",ControlName="txt_UCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_DiscountScheme_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_023")]
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
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_DiscountScheme_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_029")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///排序号
		/// <summary>
		[ModelInfo(Name = "排序号",ControlName="txt_Sort", NotEmpty = false, Length = 9, NotEmptyECode = "TB_DiscountScheme_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_032")]
		public int Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		///方案编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///内部编号
		/// <summary>
		[ModelInfo(Name = "内部编号",ControlName="txt_InsideCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_DiscountScheme_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_038")]
		public string InsideCode
		{
			get { return _InsideCode; }
			set { _InsideCode = value; }
		}
		/// <summary>
		///一般折扣率
		/// <summary>
		[ModelInfo(Name = "一般折扣率",ControlName="txt_DiscountRate", NotEmpty = false, Length = 18, NotEmptyECode = "TB_DiscountScheme_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_041")]
		public decimal DiscountRate
		{
			get { return _DiscountRate; }
			set { _DiscountRate = value; }
		}
		/// <summary>
		///菜谱
		/// <summary>
		[ModelInfo(Name = "菜谱",ControlName="txt_MenuCode", NotEmpty = false, Length = -1, NotEmptyECode = "TB_DiscountScheme_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_044")]
		public string MenuCode
		{
			get { return _MenuCode; }
			set { _MenuCode = value; }
		}
		/// <summary>
		///适用卡等级
		/// <summary>
		[ModelInfo(Name = "适用卡等级",ControlName="txt_LevelCode", NotEmpty = false, Length = -1, NotEmptyECode = "TB_DiscountScheme_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_047")]
		public string LevelCode
		{
			get { return _LevelCode; }
			set { _LevelCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_SchName", NotEmpty = false, Length = 64, NotEmptyECode = "TB_DiscountScheme_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DiscountScheme_050")]
		public string SchName
		{
			get { return _SchName; }
			set { _SchName = value; }
		}        
    }
}