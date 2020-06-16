using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("购物车设置")]
    [Serializable]
    public class TB_ShoppingCartEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _PKCode = string.Empty;
		private DateTime _AutoDelTime = DateTime.Parse("1900-01-01");
		private string _IsAutoDelete = string.Empty;
		private int _MaxNum = 0;

		/// <summary>
		///Id主键自增
		/// <summary>
		[ModelInfo(Name = "Id主键自增",ControlName="txt_Id", NotEmpty = false, Length = 18, NotEmptyECode = "TB_ShoppingCart_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ShoppingCart_002")]
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_ShoppingCart_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ShoppingCart_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_ShoppingCart_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ShoppingCart_008")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_ShoppingCart_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ShoppingCart_011")]
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
		[ModelInfo(Name = "状态  0无效   1有效",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_ShoppingCart_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ShoppingCart_017")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///自动删除时间
		/// <summary>
		[ModelInfo(Name = "自动删除时间",ControlName="txt_AutoDelTime", NotEmpty = false, Length = 19, NotEmptyECode = "TB_ShoppingCart_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ShoppingCart_023")]
		public DateTime AutoDelTime
		{
			get { return _AutoDelTime; }
			set { _AutoDelTime = value; }
		}
		/// <summary>
		///是否自动删除
		/// <summary>
		[ModelInfo(Name = "是否自动删除",ControlName="txt_IsAutoDelete", NotEmpty = false, Length = 1, NotEmptyECode = "TB_ShoppingCart_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ShoppingCart_026")]
		public string IsAutoDelete
		{
			get { return _IsAutoDelete; }
			set { _IsAutoDelete = value; }
		}
		/// <summary>
		///商品最大数量
		/// <summary>
		[ModelInfo(Name = "商品最大数量",ControlName="txt_MaxNum", NotEmpty = false, Length = 9, NotEmptyECode = "TB_ShoppingCart_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ShoppingCart_029")]
		public int MaxNum
		{
			get { return _MaxNum; }
			set { _MaxNum = value; }
		}        
    }
}