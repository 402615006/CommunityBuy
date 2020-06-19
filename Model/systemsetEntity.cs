using CommunityBuy.CommonBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CommunityBuy.Model
{
    [Description("系统设置")]

    public class systemsetEntity
    {
        private int _setid = 0;
        private string _stocode = string.Empty;

        private string _key = string.Empty;
        private string _val = string.Empty;
        private int _status = 0;
        private string _buscode = string.Empty;
        private string _descr = string.Empty;
        private string _explain = string.Empty;
        private DateTime _ctime = DateTime.Parse("1900-01-01");

        /// <summary>
        /// 说明
        /// </summary>
        public string explain
        {
            get { return _explain; }
            set { _explain = value; }
        }

        /// <summary>
        ///设置标识
        /// <summary>
        public int setid
        {
            get { return _setid; }
            set { _setid = value; }
        }
        /// <summary>
        ///引用门店表Store的门店编号字段stocode的值
        /// <summary>
        public string stocode
        {
            get { return _stocode; }
            set { _stocode = value; }
        }
        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }
        }

        /// <summary>
        ///键
        /// <summary>
        public string key
        {
            get { return _key; }
            set { _key = value; }
        }
        /// <summary>
        ///值
        /// <summary>
        public string val
        {
            get { return _val; }
            set { _val = value; }
        }
        /// <summary>
        ///有效状态（0无效，1有效）
        /// <summary>
        public int status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        ///描述
        /// <summary>
        public string descr
        {
            get { return _descr; }
            set { _descr = value; }
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
