using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.Model
{
    [Description("后台用户信息")]

    public class AdminsEntity
    {
        private long _userid = 0;
        private string _buscode = Helper.GetAppSettings("BusCode");
        private string _strcode = string.Empty;
        private string _username = string.Empty;
        private string _upwd = string.Empty;
        private string _realname = string.Empty;
        private string _umobile = string.Empty;
        private string _empcode = string.Empty;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        //固定字段
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private long _uuser = 0;
        private DateTime _utime = DateTime.Parse("1900-01-01");
        private string _isdelete = string.Empty;

        private string _scope = string.Empty;
        private string _stocode = string.Empty;

        public string scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        public string stocode
        {
            get { return _stocode; }
            set { _stocode = value; }
        }


        #region //扩展字段 tsg
        private string _GUID = string.Empty;
        /// <summary>
        ///令牌
        /// <summary>
        public string GUID
        {
            get { return _GUID; }
            set { _GUID = value; }
        }

        private string _roleids = string.Empty;
        /// <summary>
        ///用户角色
        /// <summary>
        public string roleids
        {
            get { return _roleids; }
            set { _roleids = value; }
        }

        #endregion

        /// <summary>
        ///用户标识
        /// <summary>
        public long userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        ///商户编号
        /// <summary>
        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }
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
        ///用户名
        /// <summary>
        public string uname
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        ///密码
        /// <summary>
        public string upwd
        {
            get { return _upwd; }
            set { _upwd = value; }
        }
        /// <summary>
        ///姓名
        /// <summary>
        public string realname
        {
            get { return _realname; }
            set { _realname = value; }
        }
        /// <summary>
        ///手机号
        /// <summary>
        public string umobile
        {
            get { return _umobile; }
            set { _umobile = value; }
        }
        /// <summary>
        ///员工编号
        /// <summary>
        public string empcode
        {
            get { return _empcode; }
            set { _empcode = value; }
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