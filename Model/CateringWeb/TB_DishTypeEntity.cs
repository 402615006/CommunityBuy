using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("菜品类别表")]
    [Serializable]
    public class TB_DishTypeEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _PKKCode = string.Empty;
		private string _PKCode = string.Empty;
		private string _TypeName = string.Empty;
		private int _Sort = 0;
		private string _TStatus = string.Empty;
        private string _StoName = string.Empty;

        /// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店名称", ControlName = "txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_DishType_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_008")]
        public string StoName
        {
            get { return _StoName; }
            set { _StoName = value; }
        }

        /// <summary>
        ///Id
        /// <summary>
        [ModelInfo(Name = "Id",ControlName="txt_Id", NotEmpty = false, Length = 18, NotEmptyECode = "TB_DishType_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_002")]
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_DishType_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_DishType_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_DishType_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_DishType_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_014")]
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
		///父类别编号
		/// <summary>
		[ModelInfo(Name = "父类别编号",ControlName="txt_PKKCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_DishType_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_020")]
		public string PKKCode
		{
			get { return _PKKCode; }
			set { _PKKCode = value; }
		}
		/// <summary>
		///类别编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///类别名称
		/// <summary>
		[ModelInfo(Name = "类别名称",ControlName="txt_TypeName", NotEmpty = false, Length = 32, NotEmptyECode = "TB_DishType_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_026")]
		public string TypeName
		{
			get { return _TypeName; }
			set { _TypeName = value; }
		}
		/// <summary>
		///排序号
		/// <summary>
		[ModelInfo(Name = "排序号",ControlName="txt_Sort", NotEmpty = false, Length = 9, NotEmptyECode = "TB_DishType_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_029")]
		public int Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_DishType_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_DishType_032")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}        
    }
}