using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("会员卡订单表")]
    [Serializable]
    public class memcardordersEntity
    {
		private long _ID = 0;
		private string _buscode = string.Empty;
		private string _stocode = string.Empty;
		private string _memcode = string.Empty;
		private string _cardcode = string.Empty;
		private string _otype = string.Empty;
		private decimal _regamount = 0;
		private decimal _freeamount = 0;
		private decimal _cardcost = 0;
		private decimal _payamount = 0;
		private string _remark = string.Empty;
		private string _status = string.Empty;
		private string _ucode = string.Empty;
		private string _uname = string.Empty;
		private DateTime _ctime = DateTime.Parse("1900-01-01");
		private string _ordercode = string.Empty;
		private string _paystatus = string.Empty;

        private string _oldcardcodes = string.Empty;
        private string _oldbalance = string.Empty;
        private decimal _balance = 0;
        private DateTime _ptime = DateTime.Parse("1900-01-01");
        private long _shiftid = 0;//班次



        private string _pushemcode = string.Empty;//提成人员工编号
        private string _pushname = string.Empty;//提成人姓名
        private string _amanagerid = string.Empty;//客户经理id
        private string _amanagername = string.Empty;//客户经理姓名
        private string _stotel = string.Empty;//门店电话

        private string _cardtype = string.Empty;//卡类别(8是员工卡，其他是贵宾卡)
        private string _cardname = string.Empty;//卡名称

        private string _memcname = string.Empty;//会员姓名
        private string _mob = string.Empty;//会员手机号
        private string _pcname = string.Empty;//赠送礼包名称
        private string _bak1 = string.Empty;//扩展信息1
        private string _bak2 = string.Empty;//扩展信息2
        private string _bak3 = string.Empty;//扩展信息3

        /// <summary>
        ///门店电话
        /// <summary>
        public string stotel
        {
            get { return _stotel; }
            set { _stotel = value; }
        }

        /// <summary>
        ///提成人id
        /// <summary>
        public string pushemcode
        {
            get { return _pushemcode; }
            set { _pushemcode = value; }
        }


        /// <summary>
        ///提成人姓名
        /// <summary>
        public string pushname
        {
            get { return _pushname; }
            set { _pushname = value; }
        }

        /// <summary>
        ///客户经理id
        /// <summary>
        public string amanagerid
        {
            get { return _amanagerid; }
            set { _amanagerid = value; }
        }

        /// <summary>
        ///客户经理姓名
        /// <summary>
        public string amanagername
        {
            get { return _amanagername; }
            set { _amanagername = value; }
        }


		/// <summary>
		///ID
		/// <summary>
		[ModelInfo(Name = "ID",ControlName="txt_ID", NotEmpty = false, Length = 18, NotEmptyECode = "memcardorders_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_002")]
		public long ID
		{
			get { return _ID; }
			set { _ID = value; }
		}
		/// <summary>
		///商户编号
		/// <summary>
		[ModelInfo(Name = "商户编号",ControlName="txt_buscode", NotEmpty = false, Length = 16, NotEmptyECode = "memcardorders_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_005")]
		public string buscode
		{
			get { return _buscode; }
			set { _buscode = value; }
		}
		/// <summary>
		///所属门店
		/// <summary>
		[ModelInfo(Name = "所属门店",ControlName="txt_stocode", NotEmpty = false, Length = 8, NotEmptyECode = "memcardorders_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_008")]
		public string stocode
		{
			get { return _stocode; }
			set { _stocode = value; }
		}
		/// <summary>
		///会员编号
		/// <summary>
		[ModelInfo(Name = "会员编号",ControlName="txt_memcode", NotEmpty = false, Length = 32, NotEmptyECode = "memcardorders_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_011")]
		public string memcode
		{
			get { return _memcode; }
			set { _memcode = value; }
		}
		/// <summary>
		///会员卡号
		/// <summary>
		[ModelInfo(Name = "会员卡号",ControlName="txt_cardcode", NotEmpty = false, Length = 32, NotEmptyECode = "memcardorders_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_014")]
		public string cardcode
		{
			get { return _cardcode; }
			set { _cardcode = value; }
		}
		/// <summary>
        ///订单类型(1-发卡,3-补卡,4-换卡,8-退卡,9-充值)
		/// <summary>
		[ModelInfo(Name = "订单类型",ControlName="txt_otype", NotEmpty = false, Length = 1, NotEmptyECode = "memcardorders_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_017")]
		public string otype
		{
			get { return _otype; }
			set { _otype = value; }
		}
		/// <summary>
		///充值金额
		/// <summary>
		[ModelInfo(Name = "充值金额",ControlName="txt_regamount", NotEmpty = false, Length = 18, NotEmptyECode = "memcardorders_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_020")]
		public decimal regamount
		{
			get { return _regamount; }
			set { _regamount = value; }
		}
		/// <summary>
		///赠送金额
		/// <summary>
		[ModelInfo(Name = "赠送金额",ControlName="txt_freeamount", NotEmpty = false, Length = 18, NotEmptyECode = "memcardorders_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_023")]
		public decimal freeamount
		{
			get { return _freeamount; }
			set { _freeamount = value; }
		}
		/// <summary>
		///工本费
		/// <summary>
		[ModelInfo(Name = "工本费",ControlName="txt_cardcost", NotEmpty = false, Length = 18, NotEmptyECode = "memcardorders_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_026")]
		public decimal cardcost
		{
			get { return _cardcost; }
			set { _cardcost = value; }
		}
		/// <summary>
		///付款总金额
		/// <summary>
		[ModelInfo(Name = "付款总金额",ControlName="txt_payamount", NotEmpty = false, Length = 18, NotEmptyECode = "memcardorders_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_029")]
		public decimal payamount
		{
			get { return _payamount; }
			set { _payamount = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_remark", NotEmpty = false, Length = 128, NotEmptyECode = "memcardorders_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_032")]
		public string remark
		{
			get { return _remark; }
			set { _remark = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		[ModelInfo(Name = "状态",ControlName="txt_status", NotEmpty = false, Length = 1, NotEmptyECode = "memcardorders_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_035")]
		public string status
		{
			get { return _status; }
			set { _status = value; }
		}
		/// <summary>
		///操作人编号
		/// <summary>
		[ModelInfo(Name = "操作人编号",ControlName="txt_ucode", NotEmpty = false, Length = 32, NotEmptyECode = "memcardorders_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_038")]
		public string ucode
		{
			get { return _ucode; }
			set { _ucode = value; }
		}
		/// <summary>
		///操作人姓名
		/// <summary>
		[ModelInfo(Name = "操作人姓名",ControlName="txt_uname", NotEmpty = false, Length = 64, NotEmptyECode = "memcardorders_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_041")]
		public string uname
		{
			get { return _uname; }
			set { _uname = value; }
		}
		/// <summary>
		///操作时间
		/// <summary>
		public DateTime ctime
		{
			get { return _ctime; }
			set { _ctime = value; }
		}
		/// <summary>
		///发卡编号
		/// <summary>
		[ModelInfo(Name = "发卡编号",ControlName="txt_ordercode", NotEmpty = false, Length = 32, NotEmptyECode = "memcardorders_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_047")]
		public string ordercode
		{
			get { return _ordercode; }
			set { _ordercode = value; }
		}
		/// <summary>
		///订金支付状态(0未支付，1部分支付，2已全部支付)
		/// <summary>
		[ModelInfo(Name = "订金支付状态(0未支付，1部分支付，2已全部支付)",ControlName="txt_paystatus", NotEmpty = false, Length = 1, NotEmptyECode = "memcardorders_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardorders_050")]
		public string paystatus
		{
			get { return _paystatus; }
			set { _paystatus = value; }
        }

        /// <summary>
        /// 老卡号（多个以逗号分隔）
        /// </summary>
        public string oldcardcodes
        {
            get { return _oldcardcodes; }
            set { _oldcardcodes = value; }
        }

        /// <summary>
        /// 老卡号余额（多个以逗号分隔）
        /// </summary>
        public string oldbalance
        {
            get { return _oldbalance; }
            set { _oldbalance = value; }
        }

        /// <summary>
        /// 现卡余额
        /// </summary>
        public decimal balance
        {
            get { return _balance; }
            set { _balance = value; }
        }


        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime ptime
        {
            get { return _ptime; }
            set { _ptime = value; }
        }



        /// <summary>
        ///班次
        /// <summary>
        public long shiftid
        {
            get { return _shiftid; }
            set { _shiftid = value; }
        }



        /// <summary>
        ///卡类别(8是员工卡，其他是贵宾卡)
        /// <summary>
        public string cardtype
        {
            get { return _cardtype; }
            set { _cardtype = value; }
        }


        /// <summary>
        ///卡名称
        /// <summary>
        public string cardname
        {
            get { return _cardname; }
            set { _cardname = value; }
        }




        /// <summary>
        ///会员姓名
        /// <summary>
        public string memcname
        {
            get { return _memcname; }
            set { _memcname = value; }
        }

        /// <summary>
        ///会员手机号
        /// <summary>
        public string mob
        {
            get { return _mob; }
            set { _mob = value; }
        }

        /// <summary>
        ///赠送礼包名称
        /// <summary>
        public string pcname
        {
            get { return _pcname; }
            set { _pcname = value; }
        }

        /// <summary>
        ///扩展信息1
        /// <summary>
        public string bak1
        {
            get { return _bak1; }
            set { _bak1 = value; }
        }

        /// <summary>
        ///扩展信息2
        /// <summary>
        public string bak2
        {
            get { return _bak2; }
            set { _bak2 = value; }
        }


        /// <summary>
        ///扩展信息3
        /// <summary>
        public string bak3
        {
            get { return _bak3; }
            set { _bak3 = value; }
        }



        #region 扩展属性
        /// <summary>
        ///订单类型(1-发卡,3-补卡,4-换卡,8-退卡,9-充值)
        /// <summary>
        public string otypetext
        {
            get 
            {
                string _otypetext = "会员卡";
                switch (_otype)
                {
                    case "0":
                        _otypetext = "未激活";
                        break;
                    case "1":
                        _otypetext = "正常";
                        break;
                    case "2":
                        _otypetext = "挂失";
                        break;
                    case "3":
                        _otypetext = "补卡";
                        break;
                    case "4":
                        _otypetext = "换卡";
                        break;
                    case "5":
                        _otypetext = "合并";
                        break;
                    case "6":
                        _otypetext = "解冻";
                        break;
                    case "7":
                        _otypetext = "冻结";
                        break;
                    case "8":
                        _otypetext = "过户";
                        break;
                    case "9":
                        _otypetext = "退卡";
                        break;
                    case "10":
                        _otypetext = "发卡";
                        break;
                    case "11":
                        _otypetext = "充值";
                        break;
                }
                return _otypetext;
            }
        }
        #endregion


    }
}