using System;
using System.ComponentModel;

namespace CommunityBuy.Model
{
    [Description("后台角色信息表")]
    public class rolesEntity
    {
        private int _roleid = 0;
        private string _scope = string.Empty;
        private string _stocode = string.Empty;
        private string _cname = string.Empty;
        private string _status = string.Empty;
        private string _descr = string.Empty;
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private long _uuser = 0;
        private DateTime _utime = DateTime.Parse("1900-01-01");
        private string _buscode = string.Empty;

        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }
        }
        /// <summary>
        ///标识
        /// <summary>
        public int roleid
        {
            get { return _roleid; }
            set { _roleid = value; }
        }
        /// <summary>
        ///角色范围
        /// <summary>
        public string scope
        {
            get { return _scope; }
            set { _scope = value; }
        }
        /// <summary>
        ///权限门店
        /// <summary>
        public string stocode
        {
            get { return _stocode; }
            set { _stocode = value; }
        }
        /// <summary>
        ///角色名称
        /// <summary>
        public string cname
        {
            get { return _cname; }
            set { _cname = value; }
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
        ///角色描述
        /// <summary>
        public string descr
        {
            get { return _descr; }
            set { _descr = value; }
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
        ///时间戳
        /// <summary>
        public DateTime ctime
        {
            get { return _ctime; }
            set { _ctime = value; }
        }
        /// <summary>
        ///修改人
        /// <summary>
        public long uuser
        {
            get { return _uuser; }
            set { _uuser = value; }
        }
        /// <summary>
        ///修改时间
        /// <summary>
        public DateTime utime
        {
            get { return _utime; }
            set { _utime = value; }
        }
    }
}
