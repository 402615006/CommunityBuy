using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    public class couponpresentDetailEntityList
    {
        public string status = "0";
        public string mes = string.Empty;
        public List<couponpresentDetailEntity> data = new List<couponpresentDetailEntity>();
    }

    [Description("优惠券赠送方案赠送明细")]
    public class couponpresentDetailEntity
    {
		private long _cpdid = 0;
		private string _buscode = string.Empty;
		private string _stocode = string.Empty;
		private string _pcode = string.Empty;
		private string _mccode = string.Empty;
		private string _sumcode = string.Empty;
		private long _num = 0;
		private string _status = string.Empty;
		private long _cuser = 0;
		private DateTime _ctime = DateTime.Parse("1900-01-01");
		private long _uuser = 0;
		private DateTime _utime = DateTime.Parse("1900-01-01");
		private string _isdelete = string.Empty;


        private string _couname = string.Empty;
        /// <summary>
        ///优惠券名称
        /// <summary>
        public string couname
        {
            get { return _couname; }
            set { _couname = value; }
        }

		/// <summary>
		///标识
		/// <summary>
		public long cpdid
		{
			get { return _cpdid; }
			set { _cpdid = value; }
		}
		/// <summary>
		///商户编号
		/// <summary>
		[ModelInfo(Name = "商户编号",ControlName="txt_buscode", NotEmpty = false, Length = 16, NotEmptyECode = "couponpresentDetail_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresentDetail_005")]
		public string buscode
		{
			get { return _buscode; }
			set { _buscode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_stocode", NotEmpty = false, Length = 8, NotEmptyECode = "couponpresentDetail_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresentDetail_008")]
		public string stocode
		{
			get { return _stocode; }
			set { _stocode = value; }
		}
		/// <summary>
		///方案编号
		/// <summary>
		[ModelInfo(Name = "方案编号",ControlName="txt_pcode", NotEmpty = false, Length = 16, NotEmptyECode = "couponpresentDetail_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresentDetail_011")]
		public string pcode
		{
			get { return _pcode; }
			set { _pcode = value; }
		}
		/// <summary>
		///优惠券编号
		/// <summary>
		[ModelInfo(Name = "优惠券编号",ControlName="txt_mccode", NotEmpty = false, Length = 16, NotEmptyECode = "couponpresentDetail_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresentDetail_014")]
		public string mccode
		{
			get { return _mccode; }
			set { _mccode = value; }
		}
		/// <summary>
		///活动编号
		/// <summary>
		[ModelInfo(Name = "活动编号",ControlName="txt_sumcode", NotEmpty = false, Length = 16, NotEmptyECode = "couponpresentDetail_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresentDetail_017")]
		public string sumcode
		{
			get { return _sumcode; }
			set { _sumcode = value; }
		}
		/// <summary>
		///数量
		/// <summary>
		[ModelInfo(Name = "数量",ControlName="txt_num", NotEmpty = false, Length = 8, NotEmptyECode = "couponpresentDetail_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresentDetail_020")]
		public long num
		{
			get { return _num; }
			set { _num = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		[ModelInfo(Name = "状态",ControlName="txt_status", NotEmpty = false, Length = 1, NotEmptyECode = "couponpresentDetail_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresentDetail_023")]
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
    }
}