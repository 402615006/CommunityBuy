using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("优惠券赠送方案")]
    public class couponpresentEntity : BaseModel
    {
        private long _cpid = 0;
        //private string _buscode = string.Empty;
        private string _stocode = string.Empty;
        private string _pcode = string.Empty;
        private string _pname = string.Empty;
        private DateTime _btime = DateTime.Parse("1900-01-01");
        private DateTime _etime = DateTime.Parse("1900-01-01");
        private string _astrcodes = string.Empty;
        private string _pretype = string.Empty;
        private decimal _preamount = 0;
        private decimal _presentNum = 0;
        private decimal _conmin = 0;
        private decimal _conmax = 0;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private int _orderno = 0;
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private long _uuser = 0;
        private DateTime _utime = DateTime.Parse("1900-01-01");
        private string _isdelete = string.Empty;
        private string _freearea = string.Empty;

        private List<couponpresentDetailEntity> _detail = new List<couponpresentDetailEntity>();
        public List<couponpresentDetailEntity> detail
        {
            get { return _detail; }
            set { _detail = value; }
        }
        /// <summary>
        ///标识
        /// <summary>
        public long cpid
        {
            get { return _cpid; }
            set { _cpid = value; }
        }

        /// <summary>
        ///门店编号
        /// <summary>
        [ModelInfo(Name = "门店编号", ControlName = "txt_stocode", NotEmpty = false, Length = 8, NotEmptyECode = "couponpresent_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_008")]
        public string stocode
        {
            get { return _stocode; }
            set { _stocode = value; }
        }
        /// <summary>
        ///方案编号
        /// <summary>
        [ModelInfo(Name = "方案编号", ControlName = "txt_pcode", NotEmpty = false, Length = 16, NotEmptyECode = "couponpresent_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_011")]
        public string pcode
        {
            get { return _pcode; }
            set { _pcode = value; }
        }
        /// <summary>
        ///方案名称
        /// <summary>
        [ModelInfo(Name = "方案名称", ControlName = "txt_pname", NotEmpty = false, Length = 64, NotEmptyECode = "couponpresent_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_014")]
        public string pname
        {
            get { return _pname; }
            set { _pname = value; }
        }
        /// <summary>
        ///方案有效开始日期
        /// <summary>
        [ModelInfo(Name = "方案有效开始日期", ControlName = "txt_btime", NotEmpty = false, Length = 4, NotEmptyECode = "couponpresent_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_017")]
        public DateTime btime
        {
            get { return _btime; }
            set { _btime = value; }
        }
        /// <summary>
        ///方案有效结束日期
        /// <summary>
        [ModelInfo(Name = "方案有效结束日期", ControlName = "txt_etime", NotEmpty = false, Length = 4, NotEmptyECode = "couponpresent_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_020")]
        public DateTime etime
        {
            get { return _etime; }
            set { _etime = value; }
        }
        /// <summary>
        ///活动门店编号
        /// <summary>
        [ModelInfo(Name = "活动门店编号", ControlName = "txt_astrcodes", NotEmpty = false, Length = 8, NotEmptyECode = "couponpresent_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_023")]
        public string astrcodes
        {
            get { return _astrcodes; }
            set { _astrcodes = value; }
        }
        /// <summary>
        ///赠送条件类别
        /// <summary>
        [ModelInfo(Name = "赠送条件类别", ControlName = "txt_pretype", NotEmpty = false, Length = 16, NotEmptyECode = "couponpresent_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_026")]
        public string pretype
        {
            get { return _pretype; }
            set { _pretype = value; }
        }
        /// <summary>
        ///赠送条件金额
        /// <summary>
        [ModelInfo(Name = "赠送条件金额", ControlName = "txt_preamount", NotEmpty = false, Length = 9, NotEmptyECode = "couponpresent_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_029")]
        public decimal preamount
        {
            get { return _preamount; }
            set { _preamount = value; }
        }
        /// <summary>
        ///赠送数量计算子表
        /// <summary>
        [ModelInfo(Name = "赠送数量计算子表", ControlName = "txt_presentNum", NotEmpty = false, Length = 9, NotEmptyECode = "couponpresent_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_032")]
        public decimal presentNum
        {
            get { return _presentNum; }
            set { _presentNum = value; }
        }
        /// <summary>
        ///条件下限
        /// <summary>
        [ModelInfo(Name = "条件下限", ControlName = "txt_conmin", NotEmpty = false, Length = 9, NotEmptyECode = "couponpresent_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_035")]
        public decimal conmin
        {
            get { return _conmin; }
            set { _conmin = value; }
        }
        /// <summary>
        ///条件上限
        /// <summary>
        [ModelInfo(Name = "条件上限", ControlName = "txt_conmax", NotEmpty = false, Length = 9, NotEmptyECode = "couponpresent_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_038")]
        public decimal conmax
        {
            get { return _conmax; }
            set { _conmax = value; }
        }
        /// <summary>
        ///备注
        /// <summary>
        [ModelInfo(Name = "备注", ControlName = "txt_remark", NotEmpty = false, Length = 128, NotEmptyECode = "couponpresent_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_041")]
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        ///状态
        /// <summary>
        [ModelInfo(Name = "状态", ControlName = "txt_status", NotEmpty = false, Length = 1, NotEmptyECode = "couponpresent_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_044")]
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        ///排序号
        /// <summary>
        [ModelInfo(Name = "排序号", ControlName = "txt_orderno", NotEmpty = false, Length = 4, NotEmptyECode = "couponpresent_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "couponpresent_047")]
        public int orderno
        {
            get { return _orderno; }
            set { _orderno = value; }
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

        /// <summary>
        /// 赠送适用范围
        /// </summary>
        public string freearea
        {
            get { return _freearea; }
            set { _freearea = value; }
        }


    }
}