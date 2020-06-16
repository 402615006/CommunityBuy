using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("账单")]
    [Serializable]
    public class TB_BillEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _OrderCodeList = string.Empty;
		private string _PKCode = string.Empty;
		private decimal _BillMoney = 0;
		private decimal _PayMoney = 0;
		private decimal _ZeroCutMoney = 0;
		private string _ShiftCode = string.Empty;
        private string _Remar = string.Empty;
		private DateTime _FTime = DateTime.Parse("1900-01-01");
		private DateTime _OpenDate = DateTime.Parse("1900-01-01");
		private string _DiscountName = string.Empty;
		private decimal _DiscountMoney = 0;
		private decimal _AUCode = 0;
		private string _AUName = string.Empty;
		private decimal _PointMoney = 0;
		private decimal _VirMoney = 0;
        private string _BillType = string.Empty;
        private string _PayWay = string.Empty;
        private string _CStatus = string.Empty;
        private string _DepartCode = string.Empty;
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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_Bill_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 4, NotEmptyECode = "TB_Bill_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Bill_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Bill_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_014")]
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
		///状态
		/// <summary>
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Bill_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///订单编号
		/// <summary>
		[ModelInfo(Name = "订单编号",ControlName="txt_OrderCodeList", NotEmpty = false, Length = 4096, NotEmptyECode = "TB_Bill_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_023")]
		public string OrderCodeList
		{
			get { return _OrderCodeList; }
			set { _OrderCodeList = value; }
		}
		/// <summary>
		///账单编号
		/// <summary>
		[ModelInfo(Name = "账单编号",ControlName="txt_PKCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Bill_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_026")]
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///账单金额
		/// <summary>
		[ModelInfo(Name = "账单金额",ControlName="txt_BillMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_029")]
		public decimal BillMoney
		{
			get { return _BillMoney; }
			set { _BillMoney = value; }
		}
		/// <summary>
		///支付金额
		/// <summary>
		[ModelInfo(Name = "支付金额",ControlName="txt_PayMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_032")]
		public decimal PayMoney
		{
			get { return _PayMoney; }
			set { _PayMoney = value; }
		}
		/// <summary>
		///抹零金额
		/// <summary>
		[ModelInfo(Name = "抹零金额",ControlName="txt_ZeroCutMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_035")]
		public decimal ZeroCutMoney
		{
			get { return _ZeroCutMoney; }
			set { _ZeroCutMoney = value; }
		}
		/// <summary>
		///班次号
		/// <summary>
		[ModelInfo(Name = "班次号",ControlName="txt_ShiftCode", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_038")]
		public string ShiftCode
		{
			get { return _ShiftCode; }
			set { _ShiftCode = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_Remar", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Bill_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_041")]
		public string Remar
		{
			get { return _Remar; }
			set { _Remar = value; }
		}
		/// <summary>
		///完成时间
		/// <summary>
		[ModelInfo(Name = "完成时间",ControlName="txt_FTime", NotEmpty = false, Length = 19, NotEmptyECode = "TB_Bill_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_044")]
		public DateTime FTime
		{
			get { return _FTime; }
			set { _FTime = value; }
		}
		/// <summary>
		///账单营业日
		/// <summary>
		[ModelInfo(Name = "账单营业日",ControlName="txt_OpenDate", NotEmpty = false, Length = 19, NotEmptyECode = "TB_Bill_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_047")]
		public DateTime OpenDate
		{
			get { return _OpenDate; }
			set { _OpenDate = value; }
		}
		/// <summary>
		///折扣名称
		/// <summary>
		[ModelInfo(Name = "折扣名称",ControlName="txt_DiscountName", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Bill_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_050")]
		public string DiscountName
		{
			get { return _DiscountName; }
			set { _DiscountName = value; }
		}
		/// <summary>
		///折扣金额
		/// <summary>
		[ModelInfo(Name = "折扣金额",ControlName="txt_DiscountMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_053")]
		public decimal DiscountMoney
		{
			get { return _DiscountMoney; }
			set { _DiscountMoney = value; }
		}
		/// <summary>
		///授权用户编号
		/// <summary>
		[ModelInfo(Name = "授权用户编号",ControlName="txt_AUCode", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_056")]
		public decimal AUCode
		{
			get { return _AUCode; }
			set { _AUCode = value; }
		}
		/// <summary>
		///授权用户名称
		/// <summary>
		[ModelInfo(Name = "授权用户名称",ControlName="txt_AUName", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Bill_058", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_059")]
		public string AUName
		{
			get { return _AUName; }
			set { _AUName = value; }
		}
		/// <summary>
		///积分抵扣金额
		/// <summary>
		[ModelInfo(Name = "积分抵扣金额",ControlName="txt_PointMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_061", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_062")]
		public decimal PointMoney
		{
			get { return _PointMoney; }
			set { _PointMoney = value; }
		}
		/// <summary>
		///虚拟币抵扣金额
		/// <summary>
		[ModelInfo(Name = "虚拟币抵扣金额",ControlName="txt_VirMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_065")]
		public decimal VirMoney
		{
			get { return _VirMoney; }
			set { _VirMoney = value; }
		}

        /// <summary>
        ///账单类型
        /// <summary>
        [ModelInfo(Name = "账单类型", ControlName = "txt_VirMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_065")]
        public string BillType
        {
            get { return _BillType; }
            set { _BillType = value; }
        }
        /// <summary>
        ///账单类型
        /// <summary>
        [ModelInfo(Name = "结账渠道", ControlName = "txt_VirMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_065")]
        public string PayWay
        {
            get { return _PayWay; }
            set { _PayWay = value; }
        }

        /// <summary>
        ///账单类型
        /// <summary>
        [ModelInfo(Name = "制作状态", ControlName = "txt_VirMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_065")]
        public string CStatus
        {
            get { return _CStatus; }
            set { _CStatus = value; }
        }

        /// <summary>
        ///部门类型
        /// <summary>
        [ModelInfo(Name = "制作状态", ControlName = "txt_VirMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Bill_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Bill_065")]
        public string DepartCode
        {
            get { return _DepartCode; }
            set { _DepartCode = value; }
        }
    }
}