using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("菜品信息")]
    [Serializable]
    public class TB_DishEntity
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
		private string _ChannelCodeList = string.Empty;
		private string _DisCode = string.Empty;
		private string _DisName = string.Empty;
		private string _OtherName = string.Empty;
		private string _TypeCode = string.Empty;
		private string _QuickCode = string.Empty;
		private string _CusDisCode = string.Empty;
		private string _Unit = string.Empty;
		private decimal _Price = 0;
		private string _MenuCode = string.Empty;
		private decimal _MemPrice = 0;
		private decimal _CostPrice = 0;
		private decimal _RoyMoney = 0;
		private string _ExtCode = string.Empty;
		private string _FinCode = string.Empty;
		private string _KitCode = string.Empty;
		private string _CookerCode = string.Empty;
		private int _MakeTime = 0;
		private string _QRCode = string.Empty;
		private string _WarCode = string.Empty;
		private string _MatCode = string.Empty;
		private string _Descript = string.Empty;
		private string _IsCount = string.Empty;
		private int _DefCount = 0;
		private decimal _CountPrice = 0;
		private string _IsVarPrice = string.Empty;
		private string _IsWeight = string.Empty;
		private string _IsMethod = string.Empty;
		private string _IsStock = string.Empty;
		private string _IsPoint = string.Empty;
		private string _IsMemPrice = string.Empty;
		private string _IsCoupon = string.Empty;
		private string _IsKeep = string.Empty;
		private string _IsCombo = string.Empty;
        private string _FinTypeName = string.Empty;

        #region 附加属性
        private string _ImageName = string.Empty;
        public string ImageName
        {
            get { return _ImageName; }
            set { _ImageName = value; }
        }

        public string FinTypeName
        {
            get { return _FinTypeName; }
            set { _FinTypeName = value; }
        }


        private List<TR_DishImageEntity> _DishImage = new List<TR_DishImageEntity>();
        /// <summary>
        /// 菜品图片集合
        /// </summary>
        public List<TR_DishImageEntity> DishImage
        {
            get { return _DishImage; }
            set { _DishImage = value; }
        }

        private List<TR_ComboInfoEntity> _ComboInfo = new List<TR_ComboInfoEntity>();
        /// <summary>
        /// 套餐组合信息集合
        /// </summary>
        public List<TR_ComboInfoEntity> ComboInfo
        {
            get { return _ComboInfo; }
            set { _ComboInfo = value; }
        }
        #endregion

        /// <summary>
        ///Id
        /// <summary>
        [ModelInfo(Name = "Id",ControlName="txt_Id", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Dish_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_002")]
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Dish_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_Dish_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_014")]
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
		[ModelInfo(Name = "记录修改人编码",ControlName="txt_UCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_020")]
		public string UCode
		{
			get { return _UCode; }
			set { _UCode = value; }
		}
		/// <summary>
		///记录修改人姓名
		/// <summary>
		[ModelInfo(Name = "记录修改人姓名",ControlName="txt_UCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_023")]
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
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_029")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///适用渠道
		/// <summary>
		[ModelInfo(Name = "适用渠道",ControlName="txt_ChannelCodeList", NotEmpty = false, Length = 128, NotEmptyECode = "TB_Dish_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_032")]
		public string ChannelCodeList
		{
			get { return _ChannelCodeList; }
			set { _ChannelCodeList = value; }
		}
		/// <summary>
		///菜品编号
		/// <summary>
		public string DisCode
		{
			get { return _DisCode; }
			set { _DisCode = value; }
		}
		/// <summary>
		///菜品名称
		/// <summary>
		[ModelInfo(Name = "菜品名称",ControlName="txt_DisName", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Dish_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_038")]
		public string DisName
		{
			get { return _DisName; }
			set { _DisName = value; }
		}
		/// <summary>
		///其他名称
		/// <summary>
		[ModelInfo(Name = "其他名称",ControlName="txt_OtherName", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Dish_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_041")]
		public string OtherName
		{
			get { return _OtherName; }
			set { _OtherName = value; }
		}
		/// <summary>
		///菜品类别
		/// <summary>
		[ModelInfo(Name = "菜品类别",ControlName="txt_TypeCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_044")]
		public string TypeCode
		{
			get { return _TypeCode; }
			set { _TypeCode = value; }
		}
		/// <summary>
		///速查码
		/// <summary>
		[ModelInfo(Name = "速查码",ControlName="txt_QuickCode", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Dish_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_047")]
		public string QuickCode
		{
			get { return _QuickCode; }
			set { _QuickCode = value; }
		}
		/// <summary>
		///自定义菜品编号
		/// <summary>
		[ModelInfo(Name = "自定义菜品编号",ControlName="txt_CusDisCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Dish_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_050")]
		public string CusDisCode
		{
			get { return _CusDisCode; }
			set { _CusDisCode = value; }
		}
		/// <summary>
		///单位
		/// <summary>
		[ModelInfo(Name = "单位",ControlName="txt_Unit", NotEmpty = false, Length = 4, NotEmptyECode = "TB_Dish_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_053")]
		public string Unit
		{
			get { return _Unit; }
			set { _Unit = value; }
		}
		/// <summary>
		///售价
		/// <summary>
		[ModelInfo(Name = "售价",ControlName="txt_Price", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Dish_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_056")]
		public decimal Price
		{
			get { return _Price; }
			set { _Price = value; }
		}
		/// <summary>
		///菜谱
		/// <summary>
		[ModelInfo(Name = "菜谱",ControlName="txt_MenuCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_058", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_059")]
		public string MenuCode
		{
			get { return _MenuCode; }
			set { _MenuCode = value; }
		}
		/// <summary>
		///会员价
		/// <summary>
		[ModelInfo(Name = "会员价",ControlName="txt_MemPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Dish_061", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_062")]
		public decimal MemPrice
		{
			get { return _MemPrice; }
			set { _MemPrice = value; }
		}
		/// <summary>
		///成本价
		/// <summary>
		[ModelInfo(Name = "成本价",ControlName="txt_CostPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Dish_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_065")]
		public decimal CostPrice
		{
			get { return _CostPrice; }
			set { _CostPrice = value; }
		}
		/// <summary>
		///提成金额
		/// <summary>
		[ModelInfo(Name = "提成金额",ControlName="txt_RoyMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Dish_067", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_068")]
		public decimal RoyMoney
		{
			get { return _RoyMoney; }
			set { _RoyMoney = value; }
		}
		/// <summary>
		///外部码
		/// <summary>
		[ModelInfo(Name = "外部码",ControlName="txt_ExtCode", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Dish_070", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_071")]
		public string ExtCode
		{
			get { return _ExtCode; }
			set { _ExtCode = value; }
		}
		/// <summary>
		///财务类别
		/// <summary>
		[ModelInfo(Name = "财务类别",ControlName="txt_FinCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_073", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_074")]
		public string FinCode
		{
			get { return _FinCode; }
			set { _FinCode = value; }
		}
		/// <summary>
		///制作厨房
		/// <summary>
		[ModelInfo(Name = "制作厨房",ControlName="txt_KitCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_076", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_077")]
		public string KitCode
		{
			get { return _KitCode; }
			set { _KitCode = value; }
		}
		/// <summary>
		///制作厨师
		/// <summary>
		[ModelInfo(Name = "制作厨师",ControlName="txt_CookerCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_079", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_080")]
		public string CookerCode
		{
			get { return _CookerCode; }
			set { _CookerCode = value; }
		}
		/// <summary>
		///制作时长
		/// <summary>
		[ModelInfo(Name = "制作时长",ControlName="txt_MakeTime", NotEmpty = false, Length = 3, NotEmptyECode = "TB_Dish_082", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_083")]
		public int MakeTime
		{
			get { return _MakeTime; }
			set { _MakeTime = value; }
		}
		/// <summary>
		///菜品二维码
		/// <summary>
		[ModelInfo(Name = "菜品二维码",ControlName="txt_QRCode", NotEmpty = false, Length = 128, NotEmptyECode = "TB_Dish_085", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_086")]
		public string QRCode
		{
			get { return _QRCode; }
			set { _QRCode = value; }
		}
		/// <summary>
		///所属仓库
		/// <summary>
		[ModelInfo(Name = "所属仓库",ControlName="txt_WarCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_088", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_089")]
		public string WarCode
		{
			get { return _WarCode; }
			set { _WarCode = value; }
		}
		/// <summary>
		///所属原料
		/// <summary>
		[ModelInfo(Name = "所属原料",ControlName="txt_MatCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Dish_091", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_092")]
		public string MatCode
		{
			get { return _MatCode; }
			set { _MatCode = value; }
		}
		/// <summary>
		///描述
		/// <summary>
		[ModelInfo(Name = "描述",ControlName="txt_Descript", NotEmpty = false, Length = 128, NotEmptyECode = "TB_Dish_094", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_095")]
		public string Descript
		{
			get { return _Descript; }
			set { _Descript = value; }
		}
		/// <summary>
		///是否使用条只方案
		/// <summary>
		[ModelInfo(Name = "是否使用条只方案",ControlName="txt_IsCount", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_097", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_098")]
		public string IsCount
		{
			get { return _IsCount; }
			set { _IsCount = value; }
		}
		/// <summary>
		///默认条只
		/// <summary>
		[ModelInfo(Name = "默认条只",ControlName="txt_DefCount", NotEmpty = false, Length = 9, NotEmptyECode = "TB_Dish_100", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_101")]
		public int DefCount
		{
			get { return _DefCount; }
			set { _DefCount = value; }
		}
		/// <summary>
		///条只单价
		/// <summary>
		[ModelInfo(Name = "条只单价",ControlName="txt_CountPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Dish_103", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_104")]
		public decimal CountPrice
		{
			get { return _CountPrice; }
			set { _CountPrice = value; }
		}
		/// <summary>
		///是否可变价
		/// <summary>
		[ModelInfo(Name = "是否可变价",ControlName="txt_IsVarPrice", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_106", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_107")]
		public string IsVarPrice
		{
			get { return _IsVarPrice; }
			set { _IsVarPrice = value; }
		}
		/// <summary>
		///是否需称重
		/// <summary>
		[ModelInfo(Name = "是否需称重",ControlName="txt_IsWeight", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_109", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_110")]
		public string IsWeight
		{
			get { return _IsWeight; }
			set { _IsWeight = value; }
		}
		/// <summary>
		///是否做法必选
		/// <summary>
		[ModelInfo(Name = "是否做法必选",ControlName="txt_IsMethod", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_112", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_113")]
		public string IsMethod
		{
			get { return _IsMethod; }
			set { _IsMethod = value; }
		}
		/// <summary>
		///是否烟酒可入库
		/// <summary>
		[ModelInfo(Name = "是否烟酒可入库",ControlName="txt_IsStock", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_115", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_116")]
		public string IsStock
		{
			get { return _IsStock; }
			set { _IsStock = value; }
		}
		/// <summary>
		///是否可兑换
		/// <summary>
		[ModelInfo(Name = "是否可兑换",ControlName="txt_IsPoint", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_118", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_119")]
		public string IsPoint
		{
			get { return _IsPoint; }
			set { _IsPoint = value; }
		}
		/// <summary>
		///是否允许会员价
		/// <summary>
		[ModelInfo(Name = "是否允许会员价",ControlName="txt_IsMemPrice", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_121", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_122")]
		public string IsMemPrice
		{
			get { return _IsMemPrice; }
			set { _IsMemPrice = value; }
		}
		/// <summary>
		///是否支持使用消费券
		/// <summary>
		[ModelInfo(Name = "是否支持使用消费券",ControlName="txt_IsCoupon", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_124", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_125")]
		public string IsCoupon
		{
			get { return _IsCoupon; }
			set { _IsCoupon = value; }
		}
		/// <summary>
		///是否可寄存
		/// <summary>
		[ModelInfo(Name = "是否可寄存",ControlName="txt_IsKeep", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_127", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_128")]
		public string IsKeep
		{
			get { return _IsKeep; }
			set { _IsKeep = value; }
		}
		/// <summary>
		///是否套餐
		/// <summary>
		[ModelInfo(Name = "是否套餐",ControlName="txt_IsCombo", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Dish_130", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Dish_131")]
		public string IsCombo
		{
			get { return _IsCombo; }
			set { _IsCombo = value; }
		}        
    }
}