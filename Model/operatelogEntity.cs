﻿using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.Model
{
    [Description("后台用户操作日志")]

    public class operatelogEntity
    {
        private int _id = 0;
        private string _module = string.Empty;
        private string _pageurl = string.Empty;
        private SystemEnum.LogOperateType _otype = SystemEnum.LogOperateType.Add;
        private string _ip = string.Empty;
        private string _logcontent = string.Empty;
        private long _cuser = 0;
        private string _buscode = string.Empty;
        private string _strcode = string.Empty;

        /// <summary>
        ///商户
        /// <summary>
        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }
        }
        /// <summary>
        ///门店
        /// <summary>
        public string strcode
        {
            get { return _strcode; }
            set { _strcode = value; }
        }
        /// <summary>
        ///标识
        /// <summary>
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        ///操作模块
        /// <summary>
        public string module
        {
            get { return _module; }
            set { _module = value; }
        }
        /// <summary>
        ///页面地址
        /// <summary>
        public string pageurl
        {
            get { return _pageurl; }
            set { _pageurl = value; }
        }
        /// <summary>
        ///操作类型
        /// <summary>
        public SystemEnum.LogOperateType otype
        {
            get { return _otype; }
            set { _otype = value; }
        }
        /// <summary>
        ///日志信息
        /// <summary>
        public string logcontent
        {
            get { return _logcontent; }
            set { _logcontent = value; }
        }
        /// <summary>
        ///IP
        /// <summary>
        public string ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        /// <summary>
        ///创建人
        /// <summary>
        public long cuser
        {
            get { return _cuser; }
            set { _cuser = value; }
        }
    }
}