using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("餐收端配置管理")]
    [Serializable]
    public class TB_SetEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _StoreHouseCode = string.Empty;
		private string _WineHouseCode = string.Empty;
		private string _SalesHouseCode = string.Empty;
		private string _PayUrl = string.Empty;
		private string _Back1 = string.Empty;
		private string _Back2 = string.Empty;
		private string _Back3 = string.Empty;
		private string _Back4 = string.Empty;

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Set_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_Set_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Set_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Set_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_014")]
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
		///所属客存库编号
		/// <summary>
		[ModelInfo(Name = "所属客存库编号",ControlName="txt_StoreHouseCode", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Set_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_020")]
		public string StoreHouseCode
		{
			get { return _StoreHouseCode; }
			set { _StoreHouseCode = value; }
		}
		/// <summary>
		///所属回酒库编号
		/// <summary>
		[ModelInfo(Name = "所属回酒库编号",ControlName="txt_WineHouseCode", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Set_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_023")]
		public string WineHouseCode
		{
			get { return _WineHouseCode; }
			set { _WineHouseCode = value; }
		}
		/// <summary>
		///客存销售库编号
		/// <summary>
		[ModelInfo(Name = "客存销售库编号",ControlName="txt_SalesHouseCode", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Set_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_026")]
		public string SalesHouseCode
		{
			get { return _SalesHouseCode; }
			set { _SalesHouseCode = value; }
		}
		/// <summary>
		///支付宝微信访问地址
		/// <summary>
		[ModelInfo(Name = "支付宝微信访问地址",ControlName="txt_PayUrl", NotEmpty = false, Length = 128, NotEmptyECode = "TB_Set_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_029")]
		public string PayUrl
		{
			get { return _PayUrl; }
			set { _PayUrl = value; }
		}
		/// <summary>
		///备用1
		/// <summary>
		[ModelInfo(Name = "备用1",ControlName="txt_Back1", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Set_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_032")]
		public string Back1
		{
			get { return _Back1; }
			set { _Back1 = value; }
		}
		/// <summary>
		///备用2
		/// <summary>
		[ModelInfo(Name = "备用2",ControlName="txt_Back2", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Set_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_035")]
		public string Back2
		{
			get { return _Back2; }
			set { _Back2 = value; }
		}
		/// <summary>
		///备用3
		/// <summary>
		[ModelInfo(Name = "备用3",ControlName="txt_Back3", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Set_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_038")]
		public string Back3
		{
			get { return _Back3; }
			set { _Back3 = value; }
		}
		/// <summary>
		///备用4
		/// <summary>
		[ModelInfo(Name = "备用4",ControlName="txt_Back4", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Set_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Set_041")]
		public string Back4
		{
			get { return _Back4; }
			set { _Back4 = value; }
		}        
    }
}