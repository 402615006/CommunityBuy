using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.Model
{
    public class memcardtypeEntityList
    {
        public string status = "0";
        public string mes = string.Empty;
        public List<memcardtypeEntity> data = new List<memcardtypeEntity>();
    }

    [Description("会员卡类型")]
    public class memcardtypeEntity
    {
        private long _mctid = 0;
        private string _buscode = string.Empty;
        private string _mctcode = string.Empty;
        private string _strcode = string.Empty;
        private string _cname = string.Empty;
        private string _ispay = string.Empty;
        private string _isnum = string.Empty;
        private string _isquit = string.Empty;
        private string _remark = string.Empty;
        private string _notstocode = string.Empty;
        private decimal _addcardcost = 0;
        private decimal _fillcardcost = 0;
        private decimal _incardcost = 0;
        private string _status = string.Empty;
        private int _orderno = 0;
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private long _uuser = 0;
        private DateTime _utime = DateTime.Parse("1900-01-01");
        private string _isdelete = string.Empty;
        private decimal _pledge = 0;
        private List<memcardlevelEntity> _levels = new List<memcardlevelEntity>();
        public List<memcardlevelEntity> levels
        {
            get { return _levels; }
            set { _levels = value; }
        } 
        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }
        }

        /// <summary>
        ///
        /// <summary>
        public long mctid
        {
            get { return _mctid; }
            set { _mctid = value; }
        }
        /// <summary>
        ///类型编码
        /// <summary>
        public string mctcode
        {
            get { return _mctcode; }
            set { _mctcode = value; }
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
        /// 不适用门店
        /// </summary>
        public string notstocode
        {
            get { return _notstocode; }
            set { _notstocode = value; }
        }

        /// <summary>
        ///类型名称
        /// <summary>
        public string cname
        {
            get { return _cname; }
            set { _cname = value; }
        }
        /// <summary>
        ///是否可充值
        /// <summary>
        public string ispay
        {
            get { return _ispay; }
            set { _ispay = value; }
        }

        /// <summary>
        ///是否计次卡
        /// <summary>
        public string isnum
        {
            get { return _isnum; }
            set { _isnum = value; }
        }

        /// <summary>
        ///是否可退卡
        /// <summary>
        public string isquit
        {
            get { return _isquit; }
            set { _isquit = value; }
        }

        /// <summary>
        ///备注
        /// <summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        ///发卡费用
        /// <summary>
        public decimal addcardcost
        {
            get { return _addcardcost; }
            set { _addcardcost = value; }
        }
        /// <summary>
        ///补卡费用
        /// <summary>
        public decimal fillcardcost
        {
            get { return _fillcardcost; }
            set { _fillcardcost = value; }
        }
        /// <summary>
        /// 换卡费用
        /// </summary>
        public decimal incardcost
        {
            get { return _incardcost; }
            set { _incardcost = value; }
        }
        /// <summary>
        ///状态
        /// </summary>
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

        /// <summary>
        /// 押金
        /// </summary>
        public decimal pledge
        {
            get { return _pledge; }
            set { _pledge = value; }
        }
    }
}
