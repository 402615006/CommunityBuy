using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    public class MemCardEntityList
    {
        public string status = "0";
        public string mes = string.Empty;
        public List<MemCardEntity> data = new List<MemCardEntity>();
    }

    public class MemCardOpenEntityList
    {
        public string status = "0";
        public string mes = string.Empty;
        public List<MemCardOpenEntity> data = new List<MemCardOpenEntity>();
    }

    [Description("会员卡信息")]
    public class MemCardEntity : BaseModel
    {
        private string _cardid = string.Empty;
        private string _cardCode = string.Empty;
        private string _maincard = string.Empty;
        private string _batchSerialNumber = string.Empty;

        private string _strcode = string.Empty;
        private string _affiliated = string.Empty;
        private string _cracode = string.Empty;
        private string _ctype = string.Empty;
        private string _sendMoney = string.Empty;
        private decimal _creditLines = 0;
        private DateTime _validate = DateTime.Parse("1900-01-01");
        private string _pmemcode = string.Empty;
        private string _memcode = string.Empty;
        private string _password = string.Empty;
        private string _isgive = string.Empty;
        private string _scid = string.Empty;
        private string _couponIds = string.Empty;
        private string _nowritedoc = string.Empty;
        private string _salesmancode = string.Empty;
        private string _salesmanname = string.Empty;
        private string _authorizedPerson = string.Empty;
        private DateTime _authDate = DateTime.Parse("1900-01-01");
        private string _companyid = string.Empty;
        private string _cardlocation = string.Empty;
        private string _status = string.Empty;
        private string _orderno = string.Empty;
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private long _uuser = 0;
        private DateTime _utime = DateTime.Parse("1900-01-01");
        private string _isdelete = string.Empty;
        private string _ousercode = string.Empty;
        private string _ousername = string.Empty;
        private string _ispay = string.Empty;
        private decimal _oncemoney = 0;
        private string _boundstatus = string.Empty;
        private string _returnstatus = string.Empty;
        private string _pcardcode = string.Empty;
        private decimal _pledge = 0;  //押金
        private decimal _balancetotal = 0;//卡余额
        private string _mob = string.Empty;
        private string _cname = string.Empty;
        private string _isneedpwd = "0";
        private string _memName = string.Empty;
        private string _memIDType = string.Empty;
        private string _memIDNO = string.Empty;
        private string _memPhone = string.Empty;

        public string isneedpwd
        {
            get { return _isneedpwd; }
            set { _isneedpwd = value; }
        }

        public string mob
        {
            get { return _mob; }
            set { _mob = value; }
        }

        public string memcname
        {
            get { return _cname; }
            set { _cname = value; }
        }

        public decimal balancetotal
        {
            get { return _balancetotal; }
            set { _balancetotal = value; }
        }

        public decimal pledge
        {
            get { return _pledge; }
            set { _pledge = value; }
        }

        public string pcardcode
        {
            get { return _pcardcode; }
            set { _pcardcode = value; }
        }

        /// <summary>
        /// 退卡状态
        /// </summary>
        private string returnstatus
        {
            get { return _returnstatus; }
            set { _returnstatus = value; }
        }

        /// <summary>
        /// 绑定状态 0.未绑定 1.已绑定
        /// </summary>
        private string boundstatus
        {
            get { return _boundstatus; }
            set { _boundstatus = value; }
        }

        public string ispay
        {
            get { return _ispay; }
            set { _ispay = value; }
        }

        //卡类型
        private string _ctypename = string.Empty;
        public string ctypename
        {
            get { return _ctypename; }
            set { _ctypename = value; }
        }

        //卡等级信息
        private memcardlevelEntity _memcardlevel = new memcardlevelEntity();
        public memcardlevelEntity memcardlevel
        {
            get { return _memcardlevel; }
            set { _memcardlevel = value; }
        }

        //发卡工本费
        private decimal _addcardcost = 0;
        public decimal addcardcost
        {
            get { return _addcardcost; }
            set { _addcardcost = value; }
        }

        /// <summary>
        /// 次卡金额
        /// </summary>
        public decimal oncemoney
        {
            get { return _oncemoney; }
            set { _oncemoney = value; }
        }

        //卡等级优惠方案信息
        private List<couponpresentEntity> _couponpresent = new List<couponpresentEntity>();
        public List<couponpresentEntity> couponpresent
        {
            get { return _couponpresent; }
            set { _couponpresent = value; }
        }

        /// <summary>
        ///会员卡标识
        /// <summary>
        public string cardid
        {
            get { return _cardid; }
            set { _cardid = value; }
        }
        /// <summary>
        ///会员卡卡号
        /// <summary>
        [ModelInfo(Name = "会员卡卡号", ControlName = "txt_cardCode", NotEmpty = false, Length = 32, NotEmptyECode = "MemCard_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_005")]
        public string cardCode
        {
            get { return _cardCode; }
            set { _cardCode = value; }
        }
        /// <summary>
        ///物理卡号
        /// <summary>
        [ModelInfo(Name = "物理卡号", ControlName = "txt_maincard", NotEmpty = false, Length = 32, NotEmptyECode = "MemCard_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_008")]
        public string maincard
        {
            get { return _maincard; }
            set { _maincard = value; }
        }
        /// <summary>
        ///批量流水号
        /// <summary>
        [ModelInfo(Name = "批量流水号", ControlName = "txt_batchSerialNumber", NotEmpty = false, Length = 32, NotEmptyECode = "MemCard_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_011")]
        public string batchSerialNumber
        {
            get { return _batchSerialNumber; }
            set { _batchSerialNumber = value; }
        }

        /// <summary>
        ///门店编号
        /// <summary>
        [ModelInfo(Name = "门店编号", ControlName = "txt_strcode", NotEmpty = false, Length = 16, NotEmptyECode = "MemCard_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_017")]
        public string strcode
        {
            get { return _strcode; }
            set { _strcode = value; }
        }
        /// <summary>
        ///是否附属卡
        /// <summary>
        [ModelInfo(Name = "是否附属卡", ControlName = "txt_affiliated", NotEmpty = false, Length = 1, NotEmptyECode = "MemCard_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_020")]
        public string affiliated
        {
            get { return _affiliated; }
            set { _affiliated = value; }
        }
        /// <summary>
        ///会员卡等级
        /// <summary>
        [ModelInfo(Name = "会员卡等级", ControlName = "txt_cracode", NotEmpty = false, Length = 16, NotEmptyECode = "MemCard_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_023")]
        public string cracode
        {
            get { return _cracode; }
            set { _cracode = value; }
        }
        /// <summary>
        ///会员卡类型
        /// <summary>
        [ModelInfo(Name = "会员卡类型", ControlName = "txt_ctype", NotEmpty = false, Length = 16, NotEmptyECode = "MemCard_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_026")]
        public string ctype
        {
            get { return _ctype; }
            set { _ctype = value; }
        }
        /// <summary>
        ///赠送金额
        /// <summary>
        [ModelInfo(Name = "赠送金额", ControlName = "txt_sendMoney", NotEmpty = false, Length = 9, NotEmptyECode = "MemCard_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_029")]
        public string sendMoney
        {
            get { return _sendMoney; }
            set { _sendMoney = value; }
        }
        /// <summary>
        ///授信额度
        /// <summary>
        [ModelInfo(Name = "授信额度", ControlName = "txt_creditLines", NotEmpty = false, Length = 9, NotEmptyECode = "MemCard_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_032")]
        public decimal creditLines
        {
            get { return _creditLines; }
            set { _creditLines = value; }
        }
        /// <summary>
        ///卡有效期
        /// <summary>
        [ModelInfo(Name = "卡有效期", ControlName = "txt_validate", NotEmpty = false, Length = 8, NotEmptyECode = "MemCard_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_035")]
        public DateTime validate
        {
            get { return _validate; }
            set { _validate = value; }
        }
        /// <summary>
        ///会员编号
        /// </summary>
        [ModelInfo(Name = "会员编号", ControlName = "txt_memcode", NotEmpty = false, Length = 32, NotEmptyECode = "MemCard_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_038")]
        public string memcode
        {
            get { return _memcode; }
            set { _memcode = value; }
        }

        /// <summary>
        ///大客户会员编号
        /// </summary>
        [ModelInfo(Name = "大客户会员编号", ControlName = "txt_bigcustomer", NotEmpty = false, Length = 32, NotEmptyECode = "MemCard_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_038")]
        public string pmemcode
        {
            get { return _pmemcode; }
            set { _pmemcode = value; }
        }
        /// <summary>
        ///卡密码
        /// <summary>
        [ModelInfo(Name = "卡密码", ControlName = "txt_password", NotEmpty = false, Length = 30, NotEmptyECode = "MemCard_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_041")]
        public string password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        ///是否赠送
        /// <summary>
        [ModelInfo(Name = "是否赠送", ControlName = "txt_isgive", NotEmpty = false, Length = 1, NotEmptyECode = "MemCard_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_044")]
        public string isgive
        {
            get { return _isgive; }
            set { _isgive = value; }
        }
        /// <summary>
        ///卡优惠方案
        /// <summary>
        [ModelInfo(Name = "卡优惠方案", ControlName = "txt_scid", NotEmpty = false, Length = 8, NotEmptyECode = "MemCard_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_047")]
        public string scid
        {
            get { return _scid; }
            set { _scid = value; }
        }
        /// <summary>
        ///优惠券信息
        /// <summary>
        [ModelInfo(Name = "优惠券信息", ControlName = "txt_couponIds", NotEmpty = false, Length = 128, NotEmptyECode = "MemCard_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_050")]
        public string couponIds
        {
            get { return _couponIds; }
            set { _couponIds = value; }
        }
        /// <summary>
        ///不填写资料
        /// <summary>
        [ModelInfo(Name = "不填写资料", ControlName = "txt_nowritedoc", NotEmpty = false, Length = 1, NotEmptyECode = "MemCard_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_053")]
        public string nowritedoc
        {
            get { return _nowritedoc; }
            set { _nowritedoc = value; }
        }
        /// <summary>
        ///业务员编号(多个)
        /// <summary>
        [ModelInfo(Name = "业务员编号(多个)", ControlName = "txt_salesmancode", NotEmpty = false, Length = 128, NotEmptyECode = "MemCard_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_056")]
        public string salesmancode
        {
            get { return _salesmancode; }
            set { _salesmancode = value; }
        }
        /// <summary>
        ///业务员姓名(多个)
        /// <summary>
        [ModelInfo(Name = "业务员姓名(多个)", ControlName = "txt_salesmanname", NotEmpty = false, Length = 128, NotEmptyECode = "MemCard_058", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_059")]
        public string salesmanname
        {
            get { return _salesmanname; }
            set { _salesmanname = value; }
        }
        /// <summary>
        ///授权人
        /// <summary>
        [ModelInfo(Name = "授权人", ControlName = "txt_authorizedPerson", NotEmpty = false, Length = 20, NotEmptyECode = "MemCard_061", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_062")]
        public string authorizedPerson
        {
            get { return _authorizedPerson; }
            set { _authorizedPerson = value; }
        }
        /// <summary>
        ///激活时间
        /// <summary>
        [ModelInfo(Name = "激活时间", ControlName = "txt_authDate", NotEmpty = false, Length = 8, NotEmptyECode = "MemCard_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_065")]
        public DateTime authDate
        {
            get { return _authDate; }
            set { _authDate = value; }
        }
        /// <summary>
        ///单位名称
        /// <summary>
        [ModelInfo(Name = "单位名称", ControlName = "txt_companyid", NotEmpty = false, Length = 6, NotEmptyECode = "MemCard_067", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_068")]
        public string companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        /// <summary>
        ///卡地址
        /// <summary>
        [ModelInfo(Name = "卡地址", ControlName = "txt_cardlocation", NotEmpty = false, Length = 512, NotEmptyECode = "MemCard_070", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_071")]
        public string cardlocation
        {
            get { return _cardlocation; }
            set { _cardlocation = value; }
        }
        /// <summary>
        ///状态
        /// <summary>
        [ModelInfo(Name = "状态", ControlName = "txt_status", NotEmpty = false, Length = 1, NotEmptyECode = "MemCard_073", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_074")]
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        ///排序号
        /// <summary>
        [ModelInfo(Name = "排序号", ControlName = "txt_orderno", NotEmpty = false, Length = 4, NotEmptyECode = "MemCard_076", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "MemCard_077")]
        public string orderno
        {
            get { return _orderno; }
            set { _orderno = value; }
        }
        /// <summary>
        ///创建人
        /// <summary>
        public long cuser
        {
            get { return _cuser; }
            set { _cuser = value; }
        }
        /// <summary>
        ///创建时间
        /// <summary>
        public DateTime ctime
        {
            get { return _ctime; }
            set { _ctime = value; }
        }
        /// <summary>
        ///最后更新人标识
        /// <summary>
        public long uuser
        {
            get { return _uuser; }
            set { _uuser = value; }
        }
        /// <summary>
        ///更新时间
        /// <summary>
        public DateTime utime
        {
            get { return _utime; }
            set { _utime = value; }
        }
        /// <summary>
        ///删除标志
        /// <summary>
        public string isdelete
        {
            get { return _isdelete; }
            set { _isdelete = value; }
        }


        //记录卡操作人
        public string userName { get; set; }

        //赠送积分
        public string initintegral { get; set; }

        //旧卡号
        public string oldcardcode { get; set; }


        /// <summary>
        /// 签单门店编号
        /// </summary>
        public string signstorecode { get; set; }
        /// <summary>
        /// 挂账门店编号
        /// </summary>
        public string creditstorecode { get; set; }
        /// <summary>
        /// 是否可签单
        /// </summary>
        public string issign { get; set; }

        /// <summary>
        /// 签单额度
        /// </summary>
        public decimal signcredit { get; set; }
        /// <summary>
        /// 是否可挂账
        /// </summary>
        /// 

        public string iscredit { get; set; }


        /// <summary>
        ///开卡人编号
        /// <summary>
        public string ousercode
        {
            get { return _ousercode; }
            set { _ousercode = value; }
        }
        /// <summary>
        ///开卡人姓名
        /// <summary>
        public string ousername
        {
            get { return _ousername; }
            set { _ousername = value; }
        }
        public string memName
        {
            get { return _memName; }
            set { _memName = value; }
        }
        public string memIDType
        {
            get { return _memIDType; }
            set { _memIDType = value; }
        }
        public string memPhone
        {
            get { return _memPhone; }
            set { _memPhone = value; }
        }
        public string memIDNO
        {
            get { return _memIDNO; }
            set { _memIDNO = value; }
        }
    }


    [Description("会员卡开卡信息")]
    public class MemCardOpenEntity : BaseModel
    {
        //卡等级信息
        private List<memcardlevelEntity> _memcardlevel = new List<memcardlevelEntity>();
        public List<memcardlevelEntity> memcardlevel
        {
            get { return _memcardlevel; }
            set { _memcardlevel = value; }
        }

        //卡等级优惠方案信息
        private couponpresentEntity _couponpresent = new couponpresentEntity();
        public couponpresentEntity couponpresent
        {
            get { return _couponpresent; }
            set { _couponpresent = value; }
        }
    }
}