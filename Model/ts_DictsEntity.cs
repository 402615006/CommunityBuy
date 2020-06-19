using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("系统字典信息")]
    
    public class ts_DictsEntity
    {
        private long _dicid = 0;
        private string _buscode = Helper.GetAppSettings("BusCode");
        private string _strcode = string.Empty;
        private string _dictype = string.Empty;
        private string _lng = string.Empty;
        private long _pdicid = 0;
        private string _diccode = string.Empty;
        private string _dicname = string.Empty;
        private string _dicvalue = string.Empty;
        private int _orderno = 0;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private string _isdelete = string.Empty;

        /// <summary>
        ///标识
        /// <summary>
        public long dicid
        {
            get { return _dicid; }
            set { _dicid = value; }
        }
        /// <summary>
        ///商户编号
        /// <summary>
        public string buscode
        {
            get { return _buscode; }

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
        ///类别
        /// <summary>
        public string dictype
        {
            get { return _dictype; }
            set { _dictype = value; }
        }
        /// <summary>
        ///语言代码
        /// <summary>
        public string lng
        {
            get { return _lng; }
            set { _lng = value; }
        }
        /// <summary>
        ///父ID
        /// <summary>
        public long pdicid
        {
            get { return _pdicid; }
            set { _pdicid = value; }
        }
        /// <summary>
        /// 字典编号
        /// </summary>
        public string diccode
        {
            get { return _diccode; }
            set { _diccode = value; }
        }
        /// <summary>
        ///字典名称
        /// <summary>
        public string dicname
        {
            get { return _dicname; }
            set { _dicname = value; }
        }
        /// <summary>
        ///字典值
        /// <summary>
        public string dicvalue
        {
            get { return _dicvalue; }
            set { _dicvalue = value; }
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
        ///备注
        /// <summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        ///有效状态（0无效，1有效）
        /// <summary>
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        ///引用系统用户表ts_admins的userid字段值
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
        ///是否删除（0未删除，1已删除，默认值为0）
        /// <summary>
        public string isdelete
        {
            get { return _isdelete; }
            set { _isdelete = value; }
        }
    }

    public class ts_DictDto
    {
        public int id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool open { get; set; }
        public bool isParent { get; set; }
        public string iconClose { get; set; }
        public string iconOpen { get; set; }
        public string icon { get; set; }
        public List<ts_DictDto> children { get; set; }
    }
}