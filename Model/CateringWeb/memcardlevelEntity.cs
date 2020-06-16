using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.Model
{
    [Description("会员卡等级")]
    public class memcardlevelEntity
    {
        private long _levelId = 0;

        private string _strcode = string.Empty;
        private string _mctcode = string.Empty;
        private string _buscode = string.Empty;
        private string _levelcode = string.Empty;
        private string _levelname = string.Empty;
        private int _minpay = 0;
        private int _maxpay = 0;
        private decimal _initintegral = 0;
        private string _sumintegralon = string.Empty;
        private decimal _sumintegral = 0;
        private string _sumrechargemoneyon = string.Empty;
        private decimal _sumrechargemoney = 0;
        private string _sumconsumptionnumon = string.Empty;
        private decimal _sumconsumptionnum = 0;
        private string _sumconsumptionmoneyon = string.Empty;
        private decimal _sumconsumptionmoney = 0;
        private decimal _scoremult = 0;
        private decimal _privilegepre = 0;
        private decimal _oncemoney = 0;
        private string _mattypecode = string.Empty;
        private string _matcode = string.Empty;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private int _orderno = 0;
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private long _uuser = 0;
        private DateTime _utime = DateTime.Parse("1900-01-01");
        private string _isdelete = string.Empty;

        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }
        }

        /// <summary>
        /// 计次金额
        /// </summary>
        public decimal oncemoney
        {
            get { return _oncemoney; }
            set { _oncemoney = value; }
        }

        /// <summary>
        ///
        /// <summary>
        [ModelInfo(Name = "", ControlName = "txt_levelId", NotEmpty = false, Length = 8, NotEmptyECode = "memcardlevel_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_002")]
        public long levelId
        {
            get { return _levelId; }
            set { _levelId = value; }
        }

        /// <summary>
        ///门店编号
        /// <summary>
        [ModelInfo(Name = "门店编号", ControlName = "txt_strcode", NotEmpty = false, Length = 8, NotEmptyECode = "memcardlevel_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_008")]
        public string strcode
        {
            get { return _strcode; }
            set { _strcode = value; }
        }
        /// <summary>
        ///会员卡类型
        /// <summary>
        [ModelInfo(Name = "会员卡类型", ControlName = "txt_mctcode", NotEmpty = false, Length = 2, NotEmptyECode = "memcardlevel_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_011")]
        public string mctcode
        {
            get { return _mctcode; }
            set { _mctcode = value; }
        }
        /// <summary>
        ///等级编号
        /// <summary>
        public string levelcode
        {
            get { return _levelcode; }
            set { _levelcode = value; }
        }
        /// <summary>
        ///等级名称
        /// <summary>
        [ModelInfo(Name = "等级名称", ControlName = "txt_levelname", NotEmpty = false, Length = 16, NotEmptyECode = "memcardlevel_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_017")]
        public string levelname
        {
            get { return _levelname; }
            set { _levelname = value; }
        }
        /// <summary>
        ///充值最小额度
        /// <summary>
        [ModelInfo(Name = "充值最小额度", ControlName = "txt_minpay", NotEmpty = false, Length = 9, NotEmptyECode = "memcardlevel_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_020")]
        public int minpay
        {
            get { return _minpay; }
            set { _minpay = value; }
        }
        /// <summary>
        ///充值最大额度
        /// <summary>
        [ModelInfo(Name = "充值最大额度", ControlName = "txt_maxpay", NotEmpty = false, Length = 9, NotEmptyECode = "memcardlevel_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_023")]
        public int maxpay
        {
            get { return _maxpay; }
            set { _maxpay = value; }
        }
        /// <summary>
        ///初始积分
        /// <summary>
        [ModelInfo(Name = "初始积分", ControlName = "txt_initintegral", NotEmpty = false, Length = 4, NotEmptyECode = "memcardlevel_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_026")]
        public decimal initintegral
        {
            get { return _initintegral; }
            set { _initintegral = value; }
        }
        /// <summary>
        ///累计积分开启
        /// <summary>
        [ModelInfo(Name = "累计积分开启", ControlName = "sumintegralon", NotEmpty = false, Length = 1, NotEmptyECode = "memcardlevel_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_029")]
        public string sumintegralon
        {
            get { return _sumintegralon; }
            set { _sumintegralon = value; }
        }
        /// <summary>
        ///累计积分
        /// <summary>
        [ModelInfo(Name = "累计积分", ControlName = "txt_sumintegral", NotEmpty = false, Length = 4, NotEmptyECode = "memcardlevel_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_032")]
        public decimal sumintegral
        {
            get { return _sumintegral; }
            set { _sumintegral = value; }
        }
        /// <summary>
        ///累计充值金额开启
        /// <summary>
        [ModelInfo(Name = "累计充值金额开启", ControlName = "sumrechargemoneyon", NotEmpty = false, Length = 1, NotEmptyECode = "memcardlevel_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_035")]
        public string sumrechargemoneyon
        {
            get { return _sumrechargemoneyon; }
            set { _sumrechargemoneyon = value; }
        }
        /// <summary>
        ///累计充值金额
        /// <summary>
        [ModelInfo(Name = "累计充值金额", ControlName = "txt_sumrechargemoney", NotEmpty = false, Length = 4, NotEmptyECode = "memcardlevel_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_038")]
        public decimal sumrechargemoney
        {
            get { return _sumrechargemoney; }
            set { _sumrechargemoney = value; }
        }
        /// <summary>
        ///累计消费次数开启
        /// <summary>
        [ModelInfo(Name = "累计消费次数开启", ControlName = "sumconsumptionnumon", NotEmpty = false, Length = 1, NotEmptyECode = "memcardlevel_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_041")]
        public string sumconsumptionnumon
        {
            get { return _sumconsumptionnumon; }
            set { _sumconsumptionnumon = value; }
        }
        /// <summary>
        ///累计消费次数
        /// <summary>
        [ModelInfo(Name = "累计消费次数", ControlName = "txt_sumconsumptionnum", NotEmpty = false, Length = 4, NotEmptyECode = "memcardlevel_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_044")]
        public decimal sumconsumptionnum
        {
            get { return _sumconsumptionnum; }
            set { _sumconsumptionnum = value; }
        }
        /// <summary>
        ///累计消费金额开启
        /// <summary>
        [ModelInfo(Name = "累计消费金额开启", ControlName = "sumconsumptionmoneyon", NotEmpty = false, Length = 1, NotEmptyECode = "memcardlevel_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_047")]
        public string sumconsumptionmoneyon
        {
            get { return _sumconsumptionmoneyon; }
            set { _sumconsumptionmoneyon = value; }
        }
        /// <summary>
        ///累计消费金额
        /// <summary>
        [ModelInfo(Name = "累计消费金额", ControlName = "txt_sumconsumptionmoney", NotEmpty = false, Length = 4, NotEmptyECode = "memcardlevel_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_050")]
        public decimal sumconsumptionmoney
        {
            get { return _sumconsumptionmoney; }
            set { _sumconsumptionmoney = value; }
        }
        /// <summary>
        ///积分倍数
        /// <summary>
        [ModelInfo(Name = "积分倍数", ControlName = "txt_scoremult", NotEmpty = false, Length = 4, NotEmptyECode = "memcardlevel_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_053")]
        public decimal scoremult
        {
            get { return _scoremult; }
            set { _scoremult = value; }
        }
        /// <summary>
        ///享受折扣
        /// <summary>
        [ModelInfo(Name = "享受折扣", ControlName = "txt_privilegepre", NotEmpty = false, Length = 4, NotEmptyECode = "memcardlevel_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_056")]
        public decimal privilegepre
        {
            get { return _privilegepre; }
            set { _privilegepre = value; }
        }
        /// <summary>
        ///有价物品
        /// <summary>
        [ModelInfo(Name = "有价物品", ControlName = "ddl_mattypecode", NotEmpty = false, Length = 10, NotEmptyECode = "memcardlevel_058", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_059")]
        public string mattypecode
        {
            get { return _mattypecode; }
            set { _mattypecode = value; }
        }
        /// <summary>
        ///有价物品(原料类型编号)
        /// <summary>
        [ModelInfo(Name = "有价物品(原料类型编号)", ControlName = "ddl_matcode", NotEmpty = false, Length = 10, NotEmptyECode = "memcardlevel_061", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_062")]
        public string matcode
        {
            get { return _matcode; }
            set { _matcode = value; }
        }
        /// <summary>
        ///(原料类型编号)
        /// <summary>
        [ModelInfo(Name = "(原料类型编号)", ControlName = "txt_remark", NotEmpty = false, Length = 128, NotEmptyECode = "memcardlevel_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_065")]
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        ///状态
        /// <summary>
        [ModelInfo(Name = "状态", ControlName = "ddl_status", NotEmpty = false, Length = 1, NotEmptyECode = "memcardlevel_067", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_068")]
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        ///排序号
        /// <summary>
        [ModelInfo(Name = "排序号", ControlName = "txt_orderno", NotEmpty = false, Length = 4, NotEmptyECode = "memcardlevel_070", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "memcardlevel_071")]
        public int orderno
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
    }
}
