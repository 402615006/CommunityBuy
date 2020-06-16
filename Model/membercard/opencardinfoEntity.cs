using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("发卡临时信息表")]
    //[Serializable]
    public class opencardinfoEntity
    {
		private long _oid = 0;
		private string _ordercode = string.Empty;
		private string _buscode = string.Empty;
		private string _stocode = string.Empty;
		private string _memcode = string.Empty;
		private string _cardcode = string.Empty;
		private decimal _regamount = 0;
		private decimal _freeamount = 0;
		private decimal _cardcost = 0;
		private decimal _payamount = 0;
		private DateTime _validate = DateTime.Parse("1900-01-01");
		private string _password = string.Empty;
		private string _nowritedoc = string.Empty;
		private string _cname = string.Empty;
		private string _mobile = string.Empty;
		private string _idtype = string.Empty;
		private string _IDNO = string.Empty;
		private string _ucode = string.Empty;
        private string _uname = string.Empty;
        private string _sex = string.Empty;
        private string _bak1 = string.Empty;
        private string _bak2 = string.Empty;
        private string _bak3 = string.Empty;
		private DateTime _ctime = DateTime.Parse("1900-01-01");

		/// <summary>
		///
		/// <summary>
		public long oid
		{
			get { return _oid; }
			set { _oid = value; }
		}
		/// <summary>
		///发卡编号
		/// <summary>
		[ModelInfo(Name = "发卡编号",ControlName="txt_ordercode", NotEmpty = false, Length = 32, NotEmptyECode = "opencardinfo_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_005")]
		public string ordercode
		{
			get { return _ordercode; }
			set { _ordercode = value; }
		}
		/// <summary>
		///商户编号
		/// <summary>
		[ModelInfo(Name = "商户编号",ControlName="txt_buscode", NotEmpty = false, Length = 16, NotEmptyECode = "opencardinfo_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_008")]
		public string buscode
		{
			get { return _buscode; }
			set { _buscode = value; }
		}
		/// <summary>
		///所属门店
		/// <summary>
		[ModelInfo(Name = "所属门店",ControlName="txt_stocode", NotEmpty = false, Length = 8, NotEmptyECode = "opencardinfo_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_011")]
		public string stocode
		{
			get { return _stocode; }
			set { _stocode = value; }
		}
		/// <summary>
		///会员编号
		/// <summary>
		[ModelInfo(Name = "会员编号",ControlName="txt_memcode", NotEmpty = false, Length = 32, NotEmptyECode = "opencardinfo_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_014")]
		public string memcode
		{
			get { return _memcode; }
			set { _memcode = value; }
		}
		/// <summary>
		///会员卡号
		/// <summary>
		[ModelInfo(Name = "会员卡号",ControlName="txt_cardcode", NotEmpty = false, Length = 32, NotEmptyECode = "opencardinfo_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_017")]
		public string cardcode
		{
			get { return _cardcode; }
			set { _cardcode = value; }
		}
		/// <summary>
		///充值金额
		/// <summary>
		[ModelInfo(Name = "充值金额",ControlName="txt_regamount", NotEmpty = false, Length = 18, NotEmptyECode = "opencardinfo_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_020")]
		public decimal regamount
		{
			get { return _regamount; }
			set { _regamount = value; }
		}
		/// <summary>
		///赠送金额
		/// <summary>
		[ModelInfo(Name = "赠送金额",ControlName="txt_freeamount", NotEmpty = false, Length = 18, NotEmptyECode = "opencardinfo_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_023")]
		public decimal freeamount
		{
			get { return _freeamount; }
			set { _freeamount = value; }
		}
		/// <summary>
		///工本费
		/// <summary>
		[ModelInfo(Name = "工本费",ControlName="txt_cardcost", NotEmpty = false, Length = 18, NotEmptyECode = "opencardinfo_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_026")]
		public decimal cardcost
		{
			get { return _cardcost; }
			set { _cardcost = value; }
		}
		/// <summary>
		///付款总金额
		/// <summary>
		[ModelInfo(Name = "付款总金额",ControlName="txt_payamount", NotEmpty = false, Length = 18, NotEmptyECode = "opencardinfo_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_029")]
		public decimal payamount
		{
			get { return _payamount; }
			set { _payamount = value; }
		}
		/// <summary>
		///卡有效期
		/// <summary>
		[ModelInfo(Name = "卡有效期",ControlName="txt_validate", NotEmpty = false, Length = 19, NotEmptyECode = "opencardinfo_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_032")]
		public DateTime validate
		{
			get { return _validate; }
			set { _validate = value; }
		}
		/// <summary>
		///密码
		/// <summary>
		[ModelInfo(Name = "密码",ControlName="txt_password", NotEmpty = false, Length = 32, NotEmptyECode = "opencardinfo_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_035")]
		public string password
		{
			get { return _password; }
			set { _password = value; }
		}
		/// <summary>
		///不填写资料
		/// <summary>
		[ModelInfo(Name = "不填写资料",ControlName="txt_nowritedoc", NotEmpty = false, Length = 1, NotEmptyECode = "opencardinfo_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_038")]
		public string nowritedoc
		{
			get { return _nowritedoc; }
			set { _nowritedoc = value; }
		}
		/// <summary>
		///姓名
		/// <summary>
		[ModelInfo(Name = "姓名",ControlName="txt_cname", NotEmpty = false, Length = 32, NotEmptyECode = "opencardinfo_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_041")]
		public string cname
		{
			get { return _cname; }
			set { _cname = value; }
		}
		/// <summary>
		///手机号码
		/// <summary>
		[ModelInfo(Name = "手机号码",ControlName="txt_mobile", NotEmpty = false, Length = 12, NotEmptyECode = "opencardinfo_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_044")]
		public string mobile
		{
			get { return _mobile; }
			set { _mobile = value; }
		}
		/// <summary>
		///证件类型
		/// <summary>
		[ModelInfo(Name = "证件类型",ControlName="txt_idtype", NotEmpty = false, Length = 2, NotEmptyECode = "opencardinfo_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_047")]
		public string idtype
		{
			get { return _idtype; }
			set { _idtype = value; }
		}
		/// <summary>
		///证件号码
		/// <summary>
		[ModelInfo(Name = "证件号码",ControlName="txt_IDNO", NotEmpty = false, Length = 20, NotEmptyECode = "opencardinfo_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_050")]
		public string IDNO
		{
			get { return _IDNO; }
			set { _IDNO = value; }
		}
		/// <summary>
		///操作人编号
		/// <summary>
		[ModelInfo(Name = "操作人编号",ControlName="txt_ucode", NotEmpty = false, Length = 32, NotEmptyECode = "opencardinfo_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_053")]
		public string ucode
		{
			get { return _ucode; }
			set { _ucode = value; }
		}
		/// <summary>
		///操作人姓名
		/// <summary>
		[ModelInfo(Name = "操作人姓名",ControlName="txt_uname", NotEmpty = false, Length = 64, NotEmptyECode = "opencardinfo_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "opencardinfo_056")]
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
        /// 性别
        /// </summary>
        public string sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        /// <summary>
        /// 备用字段1 64位
        /// </summary>
        public string bak1
        {
            get { return _bak1; }
            set { _bak1 = value; }
        }

        /// <summary>
        /// 备用字段2 64位
        /// </summary>
        public string bak2
        {
            get { return _bak2; }
            set { _bak2 = value; }
        }

        /// <summary>
        /// 备用字段3 64位
        /// </summary>
        public string bak3
        {
            get { return _bak3; }
            set { _bak3 = value; }
        }
    }
}