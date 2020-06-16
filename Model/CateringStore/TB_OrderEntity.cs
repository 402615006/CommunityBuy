using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("订单")]
    [Serializable]
    public class TB_OrderEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _PKCode = string.Empty;
		private string _OpenCodeList = string.Empty;
		private decimal _OrderMoney = 0;
		private decimal _DisNum = 0;
		private int _DisTypeNum = 0;
		private string _Remar = string.Empty;
		private DateTime _CheckTime = DateTime.Parse("1900-01-01");
		private string _BillCode = string.Empty;
        private string _OrderType = string.Empty;
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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_Order_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 4, NotEmptyECode = "TB_Order_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Order_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Order_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_014")]
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
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Order_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///订单编号
		/// <summary>
		[ModelInfo(Name = "订单编号",ControlName="txt_PKCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Order_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_023")]
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///开台编号
		/// <summary>
		[ModelInfo(Name = "开台编号",ControlName="txt_OpenCodeList", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Order_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_026")]
		public string OpenCodeList
		{
			get { return _OpenCodeList; }
			set { _OpenCodeList = value; }
		}
		/// <summary>
		///订单金额
		/// <summary>
		[ModelInfo(Name = "订单金额",ControlName="txt_OrderMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Order_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_029")]
		public decimal OrderMoney
		{
			get { return _OrderMoney; }
			set { _OrderMoney = value; }
		}
		/// <summary>
		///菜品数量
		/// <summary>
		[ModelInfo(Name = "菜品数量",ControlName="txt_DisNum", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Order_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_032")]
		public decimal DisNum
		{
			get { return _DisNum; }
			set { _DisNum = value; }
		}
		/// <summary>
		///菜品种类
		/// <summary>
		[ModelInfo(Name = "菜品种类",ControlName="txt_DisTypeNum", NotEmpty = false, Length = 9, NotEmptyECode = "TB_Order_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_035")]
		public int DisTypeNum
		{
			get { return _DisTypeNum; }
			set { _DisTypeNum = value; }
		}
		/// <summary>
		///下单备注
		/// <summary>
		[ModelInfo(Name = "下单备注",ControlName="txt_Remar", NotEmpty = false, Length = 64, NotEmptyECode = "TB_Order_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_038")]
		public string Remar
		{
			get { return _Remar; }
			set { _Remar = value; }
		}
		/// <summary>
		///结账时间
		/// <summary>
		[ModelInfo(Name = "结账时间",ControlName="txt_CheckTime", NotEmpty = false, Length = 19, NotEmptyECode = "TB_Order_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_041")]
		public DateTime CheckTime
		{
			get { return _CheckTime; }
			set { _CheckTime = value; }
		}
		/// <summary>
		///账单号
		/// <summary>
		[ModelInfo(Name = "账单号",ControlName="txt_BillCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Order_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_044")]
		public string BillCode
		{
			get { return _BillCode; }
			set { _BillCode = value; }
		}
        /// <summary>
        ///订单类型
        /// <summary>
        [ModelInfo(Name = "订单类型", ControlName = "txt_BillCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Order_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_044")]
        public string OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; }
        }

        /// <summary>
        ///部门编号
        /// <summary>
        [ModelInfo(Name = "订单类型", ControlName = "txt_BillCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Order_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Order_044")]
        public string DepartCode
        {
            get { return _DepartCode; }
            set { _DepartCode = value; }
        }
    }
}