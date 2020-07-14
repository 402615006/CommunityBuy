
using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("商家门店信息")]

    public class StoreEntity
    {
        private long _stoid = 0;
        private string _comcode = string.Empty;
        private string _buscode = string.Empty;
        private string _stocode = string.Empty;
        private string _cname = string.Empty;
        private string _sname = string.Empty;
        private string _bcode = string.Empty;
        private string _indcode = string.Empty;
        private int _provinceid = 0;
        private int _cityid = 0;
        private int _areaid = 0;
        private string _address = string.Empty;
        private string _stoprincipal = string.Empty;
        private string _stoprincipaltel = string.Empty;
        private string _tel = string.Empty;
        private string _stoemail = string.Empty;
        private string _logo = string.Empty;
        private string _backgroundimg = string.Empty;
        private string _stopath = string.Empty;
        private string _descr = string.Empty;
        private string _stourl = string.Empty;
        private string _stocoordx = string.Empty;
        private string _stocoordy = string.Empty;
        private string _recommended = string.Empty;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private string _isdelete = string.Empty;
        private string _btime = string.Empty;
        private string _etime = string.Empty;
        private string _services = string.Empty;
        private int _sqcode = 0;
        private decimal _jprice = 0;

        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }
        }
        /// <summary>
        /// 营业开始时间
        /// </summary>
        public string btime
        {
            get { return _btime; }
            set { _btime = value; }
        }

        /// <summary>
        /// 营业结束时间
        /// </summary>
        public string etime
        {
            get { return _etime; }
            set { _etime = value; }
        }

        /// <summary>
        ///门店ID
        /// <summary>
        public long stoid
        {
            get { return _stoid; }
            set { _stoid = value; }
        }
        /// <summary>
        ///引用分公司表company的公司编号字段comcode的值
        /// <summary>
        public string comcode
        {
            get { return _comcode; }
            set { _comcode = value; }
        }

        /// <summary>
        ///门店编号
        /// <summary>
        public string stocode
        {
            get { return _stocode; }
            set { _stocode = value; }
        }
        /// <summary>
        ///门店名称
        /// <summary>
        public string cname
        {
            get { return _cname; }
            set { _cname = value; }
        }
        /// <summary>
        ///门店简称
        /// <summary>
        public string sname
        {
            get { return _sname; }
            set { _sname = value; }
        }
        /// <summary>
        ///门店简码
        /// <summary>
        public string bcode
        {
            get { return _bcode; }
            set { _bcode = value; }
        }
        /// <summary>
        ///所属行业
        /// <summary>
        public string indcode
        {
            get { return _indcode; }
            set { _indcode = value; }
        }
        /// <summary>
        ///所在省
        /// <summary>
        public int provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        /// <summary>
        ///所在城市
        /// <summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        /// <summary>
        ///所在区
        /// <summary>
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        /// <summary>
        ///门店地址
        /// <summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        ///负责人
        /// <summary>
        public string stoprincipal
        {
            get { return _stoprincipal; }
            set { _stoprincipal = value; }
        }
        /// <summary>
        ///负责人联系电话
        /// <summary>
        public string stoprincipaltel
        {
            get { return _stoprincipaltel; }
            set { _stoprincipaltel = value; }
        }
        /// <summary>
        ///门店电话
        /// <summary>
        public string tel
        {
            get { return _tel; }
            set { _tel = value; }
        }
        /// <summary>
        ///门店邮箱
        /// <summary>
        public string stoemail
        {
            get { return _stoemail; }
            set { _stoemail = value; }
        }
        /// <summary>
        ///门店Logo
        /// <summary>
        public string logo
        {
            get { return _logo; }
            set { _logo = value; }
        }
        /// <summary>
        ///门店背景图
        /// <summary>
        public string backgroundimg
        {
            get { return _backgroundimg; }
            set { _backgroundimg = value; }
        }
        /// <summary>
        ///轮播图
        /// <summary>
        public string stopath
        {
            get { return _stopath; }
            set { _stopath = value; }
        }

        public string services
        {
            get { return _services; }
            set { _services = value; }
        }
        /// <summary>
        ///门店描述
        /// <summary>
        public string descr
        {
            get { return _descr; }
            set { _descr = value; }
        }
        /// <summary>
        ///门店网址
        /// <summary>
        public string stourl
        {
            get { return _stourl; }
            set { _stourl = value; }
        }
        /// <summary>
        ///X坐标
        /// <summary>
        public string stocoordx
        {
            get { return _stocoordx; }
            set { _stocoordx = value; }
        }
        /// <summary>
        ///Y坐标
        /// <summary>
        public string stocoordy
        {
            get { return _stocoordy; }
            set { _stocoordy = value; }
        }

        /// <summary>
        ///推荐
        /// <summary>
        public string recommended
        {
            get { return _recommended; }
            set { _recommended = value; }
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
        ///删除标志（0未删除，1已删除，默认值为0）
        /// <summary>
        public string isdelete
        {
            get { return _isdelete; }
            set { _isdelete = value; }
        }

        /// <summary>
        ///商圈id
        /// <summary>
        public int sqcode
        {
            get { return _sqcode; }
            set { _sqcode = value; }
        }

        /// <summary>
        ///人均消费
        /// <summary>
        public decimal jprice
        {
            get { return _jprice; }
            set { _jprice = value; }
        }
    }
}