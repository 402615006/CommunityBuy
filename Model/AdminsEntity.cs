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
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private string _isdelete = string.Empty;


        //string cuser, string ccode
        private string _cname = string.Empty;
        public string cname
        {
            get { return _cname; }
            set { _cname = value; }
        }
        private string _ccode = string.Empty;
        public string ccode
        {
            get { return _ccode; }
            set { _ccode = value; }
        }

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
        ///删除标志
        /// <summary>
        public string isdelete
        {
            get { return _isdelete; }
            set { _isdelete = value; }
        }


    }
}