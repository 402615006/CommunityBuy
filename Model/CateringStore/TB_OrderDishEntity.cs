using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("订单菜品")]
    [Serializable]
    public class TB_OrderDishEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _OrderCode = string.Empty;
		private string _FinCode = string.Empty;
		private string _DisTypeCode = string.Empty;
		private string _DisCode = string.Empty;
		private string _DisName = string.Empty;
		private decimal _MemPrice = 0;
		private decimal _Price = 0;
		private string _DisUite = string.Empty;
		private decimal _DisNum = 0;
		private decimal _ReturnNum = 0;
		private string _IsPackage = string.Empty;
		private string _PDisCode = string.Empty;
		private string _Remar = string.Empty;
		private string _PKCode = string.Empty;
		private decimal _DiscountPrice = 0;
		private string _DiscountRemark = string.Empty;
		private string _DiscountType = string.Empty;
		private string _DisCase = string.Empty;
		private string _Favor = string.Empty;
		private decimal _ItemNum = 0;
		private decimal _ItemPrice = 0;
		private string _CookName = string.Empty;
		private decimal _CookMoney = 0;
		private decimal _TotalMoney = 0;

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_OrderDish_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 4, NotEmptyECode = "TB_OrderDish_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_OrderDish_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_OrderDish_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_014")]
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
		///订单编号
		/// <summary>
		[ModelInfo(Name = "订单编号",ControlName="txt_OrderCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_OrderDish_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_020")]
		public string OrderCode
		{
			get { return _OrderCode; }
			set { _OrderCode = value; }
		}
		/// <summary>
		///财务类别编号
		/// <summary>
		[ModelInfo(Name = "财务类别编号",ControlName="txt_FinCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_OrderDish_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_023")]
		public string FinCode
		{
			get { return _FinCode; }
			set { _FinCode = value; }
		}
		/// <summary>
		///菜品类别编号
		/// <summary>
		[ModelInfo(Name = "菜品类别编号",ControlName="txt_DisTypeCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_OrderDish_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_026")]
		public string DisTypeCode
		{
			get { return _DisTypeCode; }
			set { _DisTypeCode = value; }
		}
		/// <summary>
		///菜品编号
		/// <summary>
		[ModelInfo(Name = "菜品编号",ControlName="txt_DisCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_OrderDish_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_029")]
		public string DisCode
		{
			get { return _DisCode; }
			set { _DisCode = value; }
		}
		/// <summary>
		///菜品名称
		/// <summary>
		[ModelInfo(Name = "菜品名称",ControlName="txt_DisName", NotEmpty = false, Length = 64, NotEmptyECode = "TB_OrderDish_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_032")]
		public string DisName
		{
			get { return _DisName; }
			set { _DisName = value; }
		}
		/// <summary>
		///菜品会员价
		/// <summary>
		[ModelInfo(Name = "菜品会员价",ControlName="txt_MemPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TB_OrderDish_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_035")]
		public decimal MemPrice
		{
			get { return _MemPrice; }
			set { _MemPrice = value; }
		}
		/// <summary>
		///菜品单价
		/// <summary>
		[ModelInfo(Name = "菜品单价",ControlName="txt_Price", NotEmpty = false, Length = 18, NotEmptyECode = "TB_OrderDish_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_038")]
		public decimal Price
		{
			get { return _Price; }
			set { _Price = value; }
		}
		/// <summary>
		///菜品单位
		/// <summary>
		[ModelInfo(Name = "菜品单位",ControlName="txt_DisUite", NotEmpty = false, Length = 4, NotEmptyECode = "TB_OrderDish_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_041")]
		public string DisUite
		{
			get { return _DisUite; }
			set { _DisUite = value; }
		}
		/// <summary>
		///菜品数量
		/// <summary>
		[ModelInfo(Name = "菜品数量",ControlName="txt_DisNum", NotEmpty = false, Length = 18, NotEmptyECode = "TB_OrderDish_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_044")]
		public decimal DisNum
		{
			get { return _DisNum; }
			set { _DisNum = value; }
		}
		/// <summary>
		///退菜数量
		/// <summary>
		[ModelInfo(Name = "退菜数量",ControlName="txt_ReturnNum", NotEmpty = false, Length = 18, NotEmptyECode = "TB_OrderDish_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_047")]
		public decimal ReturnNum
		{
			get { return _ReturnNum; }
			set { _ReturnNum = value; }
		}
		/// <summary>
		///套餐类型0普通菜品1套餐2套餐内菜品
		/// <summary>
		[ModelInfo(Name = "套餐类型0普通菜品1套餐2套餐内菜品",ControlName="txt_IsPackage", NotEmpty = false, Length = 1, NotEmptyECode = "TB_OrderDish_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_050")]
		public string IsPackage
		{
			get { return _IsPackage; }
			set { _IsPackage = value; }
		}
		/// <summary>
		///父菜品编号
		/// <summary>
		[ModelInfo(Name = "父菜品编号",ControlName="txt_PDisCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_OrderDish_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_053")]
		public string PDisCode
		{
			get { return _PDisCode; }
			set { _PDisCode = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_Remar", NotEmpty = false, Length = 64, NotEmptyECode = "TB_OrderDish_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_056")]
		public string Remar
		{
			get { return _Remar; }
			set { _Remar = value; }
		}
		/// <summary>
		///菜品订单号
		/// <summary>
		[ModelInfo(Name = "菜品订单号",ControlName="txt_PKCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_OrderDish_058", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_059")]
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///折扣价格
		/// <summary>
		[ModelInfo(Name = "折扣价格",ControlName="txt_DiscountPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TB_OrderDish_061", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_062")]
		public decimal DiscountPrice
		{
			get { return _DiscountPrice; }
			set { _DiscountPrice = value; }
		}
		/// <summary>
		///折扣备注
		/// <summary>
		[ModelInfo(Name = "折扣备注",ControlName="txt_DiscountRemark", NotEmpty = false, Length = 32, NotEmptyECode = "TB_OrderDish_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_065")]
		public string DiscountRemark
		{
			get { return _DiscountRemark; }
			set { _DiscountRemark = value; }
		}
		/// <summary>
		///折扣类型
		/// <summary>
		[ModelInfo(Name = "折扣类型",ControlName="txt_DiscountType", NotEmpty = false, Length = 1, NotEmptyECode = "TB_OrderDish_067", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_068")]
		public string DiscountType
		{
			get { return _DiscountType; }
			set { _DiscountType = value; }
		}
		/// <summary>
		///菜品方案1普通2称重3条只
		/// <summary>
		[ModelInfo(Name = "菜品方案1普通2称重3条只",ControlName="txt_DisCase", NotEmpty = false, Length = 1, NotEmptyECode = "TB_OrderDish_070", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_071")]
		public string DisCase
		{
			get { return _DisCase; }
			set { _DisCase = value; }
		}
		/// <summary>
		///口味信息
		/// <summary>
		[ModelInfo(Name = "口味信息",ControlName="txt_Favor", NotEmpty = false, Length = 64, NotEmptyECode = "TB_OrderDish_073", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_074")]
		public string Favor
		{
			get { return _Favor; }
			set { _Favor = value; }
		}
		/// <summary>
		///条只数量
		/// <summary>
		[ModelInfo(Name = "条只数量",ControlName="txt_ItemNum", NotEmpty = false, Length = 18, NotEmptyECode = "TB_OrderDish_076", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_077")]
		public decimal ItemNum
		{
			get { return _ItemNum; }
			set { _ItemNum = value; }
		}
		/// <summary>
		///条只单价
		/// <summary>
		[ModelInfo(Name = "条只单价",ControlName="txt_ItemPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TB_OrderDish_079", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_080")]
		public decimal ItemPrice
		{
			get { return _ItemPrice; }
			set { _ItemPrice = value; }
		}
		/// <summary>
		///做法名称
		/// <summary>
		[ModelInfo(Name = "做法名称",ControlName="txt_CookName", NotEmpty = false, Length = 64, NotEmptyECode = "TB_OrderDish_082", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_083")]
		public string CookName
		{
			get { return _CookName; }
			set { _CookName = value; }
		}
		/// <summary>
		///做法加价
		/// <summary>
		[ModelInfo(Name = "做法加价",ControlName="txt_CookMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_OrderDish_085", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_086")]
		public decimal CookMoney
		{
			get { return _CookMoney; }
			set { _CookMoney = value; }
		}
		/// <summary>
		///总价
		/// <summary>
		[ModelInfo(Name = "总价",ControlName="txt_TotalMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_OrderDish_088", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_OrderDish_089")]
		public decimal TotalMoney
		{
			get { return _TotalMoney; }
			set { _TotalMoney = value; }
		}        
    }
}