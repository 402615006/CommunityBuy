using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("优惠券活动")]
    public class sumcouponNEntity
    {
		private long _sumid = 0;
		private string _sumcode = string.Empty;
        private string _buscode = Helper.GetAppSettings("BusCode");
        private string _stocode = "";
		private string _cname = string.Empty;
		private string _ctype = string.Empty;
		private string _status = string.Empty;
		private string _descr = string.Empty;
        private string _audcode = string.Empty;
        private string _audcname = string.Empty;
		private string _audremark = string.Empty;
		private string _audstatus = string.Empty;
        private string _ccode = string.Empty;
        private string _ccname = string.Empty;
		private DateTime _ctime = DateTime.Parse("1900-01-01");

		/// <summary>
		///活动ID
		/// <summary>
		public long sumid
		{
			get { return _sumid; }
			set { _sumid = value; }
		}
		/// <summary>
		///活动编号
		/// <summary>
		public string sumcode
		{
			get { return _sumcode; }
			set { _sumcode = value; }
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
		///门店编号(报表显示门店)
		/// <summary>
        public string stocode
		{
            get { return _stocode; }
            set { _stocode = value; }
		}

		/// <summary>
		///活动名称
		/// <summary>
		public string cname
		{
			get { return _cname; }
			set { _cname = value; }
		}
		/// <summary>
		///优惠券一级类型
		/// <summary>
		public string ctype
		{
			get { return _ctype; }
			set { _ctype = value; }
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
		///活动描述
		/// <summary>
		public string descr
		{
			get { return _descr; }
			set { _descr = value; }
		}
		/// <summary>
		///审核人
		/// <summary>
        public string audcode
		{
			get { return _audcode; }
			set { _audcode = value; }
		}
		/// <summary>
		///审核人姓名
		/// <summary>
        public string audcname
		{
            get { return _audcname; }
            set { _audcname = value; }
		}
		/// <summary>
		///审核备注
		/// <summary>
		public string audremark
		{
			get { return _audremark; }
			set { _audremark = value; }
		}
		/// <summary>
		///审核状态
		/// <summary>
		public string audstatus
		{
			get { return _audstatus; }
			set { _audstatus = value; }
		}
		/// <summary>
		///创建人
		/// <summary>
		public string ccode
		{
			get { return _ccode; }
			set { _ccode = value; }
		}
		/// <summary>
		///创建人姓名
		/// <summary>
        public string ccname
        {
            get { return _ccname; }
            set { _ccname = value; }
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