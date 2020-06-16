using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.Model
{
    [Description("系统功能管理")]
    [Serializable]
    public class TB_FunctionsEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private int _FType = 0;
		private long _ParentId = 0;
		private string _Code = string.Empty;
		private string _Cname = string.Empty;
		private string _BtnCode = string.Empty;
		private int _Orders = 0;
		private string _ImgName = string.Empty;
		private string _Url = string.Empty;
		private int _Level = 0;
		private string _Descr = string.Empty;

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Functions_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_Functions_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Functions_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Functions_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_014")]
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
		///状态  0无效   1有效
		/// <summary>
		[ModelInfo(Name = "状态  0无效   1有效",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Functions_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///类型
		/// <summary>
		[ModelInfo(Name = "类型",ControlName="txt_FType", NotEmpty = false, Length = 3, NotEmptyECode = "TB_Functions_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_023")]
		public int FType
		{
			get { return _FType; }
			set { _FType = value; }
		}
		/// <summary>
		///父ID
		/// <summary>
		[ModelInfo(Name = "父ID",ControlName="txt_ParentId", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Functions_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_026")]
		public long ParentId
		{
			get { return _ParentId; }
			set { _ParentId = value; }
		}
		/// <summary>
		///功能编号
		/// <summary>
		[ModelInfo(Name = "功能编号",ControlName="txt_Code", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Functions_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_029")]
		public string Code
		{
			get { return _Code; }
			set { _Code = value; }
		}
		/// <summary>
		///功能名称
		/// <summary>
		[ModelInfo(Name = "功能名称",ControlName="txt_Cname", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Functions_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_032")]
		public string Cname
		{
			get { return _Cname; }
			set { _Cname = value; }
		}
		/// <summary>
		///按钮编号
		/// <summary>
		[ModelInfo(Name = "按钮编号",ControlName="txt_BtnCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Functions_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_035")]
		public string BtnCode
		{
			get { return _BtnCode; }
			set { _BtnCode = value; }
		}
		/// <summary>
		///排序号
		/// <summary>
		[ModelInfo(Name = "排序号",ControlName="txt_Orders", NotEmpty = false, Length = 9, NotEmptyECode = "TB_Functions_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_038")]
		public int Orders
		{
			get { return _Orders; }
			set { _Orders = value; }
		}
		/// <summary>
		///图片名称
		/// <summary>
		[ModelInfo(Name = "图片名称",ControlName="txt_ImgName", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Functions_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_041")]
		public string ImgName
		{
			get { return _ImgName; }
			set { _ImgName = value; }
		}
		/// <summary>
		///图片路径
		/// <summary>
		[ModelInfo(Name = "图片路径",ControlName="txt_Url", NotEmpty = false, Length = 128, NotEmptyECode = "TB_Functions_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_044")]
		public string Url
		{
			get { return _Url; }
			set { _Url = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_Level", NotEmpty = false, Length = 3, NotEmptyECode = "TB_Functions_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_047")]
		public int Level
		{
			get { return _Level; }
			set { _Level = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_Descr", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Functions_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Functions_050")]
		public string Descr
		{
			get { return _Descr; }
			set { _Descr = value; }
		}        
    }
}