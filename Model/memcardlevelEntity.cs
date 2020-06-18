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
        public long levelId
        {
            get { return _levelId; }
            set { _levelId = value; }
        }

        /// <summary>
        ///门店编号
        /// <summary>
        public string strcode
        {
            get { return _strcode; }
            set { _strcode = value; }
        }
        /// <summary>
        ///会员卡类型
        /// <summary>
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
        public string levelname
        {
            get { return _levelname; }
            set { _levelname = value; }
        }
        /// <summary>
        ///充值最小额度
        /// <summary>
        public int minpay
        {
            get { return _minpay; }
            set { _minpay = value; }
        }
        /// <summary>
        ///充值最大额度
        /// <summary>
        public int maxpay
        {
            get { return _maxpay; }
            set { _maxpay = value; }
        }
        /// <summary>
        ///初始积分
        /// <summary>
        public decimal initintegral
        {
            get { return _initintegral; }
            set { _initintegral = value; }
        }
        /// <summary>
        ///累计积分开启
        /// <summary>
        public string sumintegralon
        {
            get { return _sumintegralon; }
            set { _sumintegralon = value; }
        }
        /// <summary>
        ///累计积分
        /// <summary>
        public decimal sumintegral
        {
            get { return _sumintegral; }
            set { _sumintegral = value; }
        }
        /// <summary>
        ///累计充值金额开启
        /// <summary>
        public string sumrechargemoneyon
        {
            get { return _sumrechargemoneyon; }
            set { _sumrechargemoneyon = value; }
        }
        /// <summary>
        ///累计充值金额
        /// <summary>
        public decimal sumrechargemoney
        {
            get { return _sumrechargemoney; }
            set { _sumrechargemoney = value; }
        }
        /// <summary>
        ///累计消费次数开启
        /// <summary>
        public string sumconsumptionnumon
        {
            get { return _sumconsumptionnumon; }
            set { _sumconsumptionnumon = value; }
        }
        /// <summary>
        ///累计消费次数
        /// <summary>
        public decimal sumconsumptionnum
        {
            get { return _sumconsumptionnum; }
            set { _sumconsumptionnum = value; }
        }
        /// <summary>
        ///累计消费金额开启
        /// <summary>
        public string sumconsumptionmoneyon
        {
            get { return _sumconsumptionmoneyon; }
            set { _sumconsumptionmoneyon = value; }
        }
        /// <summary>
        ///累计消费金额
        /// <summary>
        public decimal sumconsumptionmoney
        {
            get { return _sumconsumptionmoney; }
            set { _sumconsumptionmoney = value; }
        }
        /// <summary>
        ///积分倍数
        /// <summary>
        public decimal scoremult
        {
            get { return _scoremult; }
            set { _scoremult = value; }
        }
        /// <summary>
        ///享受折扣
        /// <summary>
        public decimal privilegepre
        {
            get { return _privilegepre; }
            set { _privilegepre = value; }
        }
        /// <summary>
        ///有价物品
        /// <summary>
        public string mattypecode
        {
            get { return _mattypecode; }
            set { _mattypecode = value; }
        }
        /// <summary>
        ///有价物品(原料类型编号)
        /// <summary>
        public string matcode
        {
            get { return _matcode; }
            set { _matcode = value; }
        }
        /// <summary>
        ///(原料类型编号)
        /// <summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        ///状态
        /// <summary>
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        ///排序号
        /// <summary>
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
