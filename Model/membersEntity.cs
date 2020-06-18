
using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    public class membersEntityList
    {
        public string status = "0";
        public string mes = string.Empty;
        public List<membersEntity> data = new List<membersEntity>();
    }

    [Description("会员信息表")]
    public class membersEntity : BaseModel
    {
        private string _memid = string.Empty;
        private string _memcode = string.Empty;
        private string _source = string.Empty;

        private string _strcode = string.Empty;
        private string _wxaccount = string.Empty;
        private string _bigcustomer = string.Empty;
        private string _cname = string.Empty;
        private DateTime _birthday = DateTime.Parse("1900-01-01");
        private string _sex = string.Empty;
        private string _mobile = string.Empty;
        private string _email = string.Empty;
        private string _tel = string.Empty;
        private string _idtype = string.Empty;
        private string _IDNO = string.Empty;
        private int _provinceid = 0;
        private int _cityid = 0;
        private int _areaid = 0;
        private string _photo = string.Empty;
        private string _signature = string.Empty;
        private string _address = string.Empty;
        private string _hobby = string.Empty;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private int _orderno = 0;
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private long _uuser = 0;
        private DateTime _utime = DateTime.Parse("1900-01-01");
        private string _isdelete = string.Empty;
        private string _ousercode = string.Empty;
        private string _ousername = string.Empty;

        /// <summary>
        ///会员标识
        /// <summary>
        public string memid
        {
            get { return _memid; }
            set { _memid = value; }
        }
        /// <summary>
        ///会员编号
        /// <summary>
        public string memcode
        {
            get { return _memcode; }
            set { _memcode = value; }
        }
        /// <summary>
        ///微信账户
        /// <summary>
        public string wxaccount
        {
            get { return _wxaccount; }
            set { _wxaccount = value; }
        }

        /// <summary>
        ///姓名
        /// <summary>
        public string cname
        {
            get { return _cname; }
            set { _cname = value; }
        }
        /// <summary>
        ///生日
        /// <summary>
        public DateTime birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }
        /// <summary>
        ///性别
        /// <summary>
        public string sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        /// <summary>
        ///手机号码
        /// <summary>
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        /// <summary>
        ///证件类型
        /// <summary>
        public string idtype
        {
            get { return _idtype; }
            set { _idtype = value; }
        }
        /// <summary>
        ///证件号码
        /// <summary>
        public string IDNO
        {
            get { return _IDNO; }
            set { _IDNO = value; }
        }
        /// <summary>
        ///所属省
        /// <summary>
        public int provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        /// <summary>
        ///所属城市
        /// <summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        /// <summary>
        ///所属区域
        /// <summary>
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        /// <summary>
        ///照片
        /// <summary>
        public string photo
        {
            get { return _photo; }
            set { _photo = value; }
        }

        /// <summary>
        ///会员地址
        /// <summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
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
        ///状态
        /// <summary>
        public string status
        {
            get { return _status; }
            set { _status = value; }
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

    }
}