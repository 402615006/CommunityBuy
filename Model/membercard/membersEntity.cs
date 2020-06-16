
using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    public class membersEntityList
    {
        public string status = "0";
        public string mes = string.Empty;
        public List<membersEntity> data = new List<membersEntity>();
    }

    [Description("会员信息表")]
    public class membersEntity : BaseModel
    {
        private string _memid = string.Empty;
        private string _memcode = string.Empty;
        private string _source = string.Empty;

        private string _strcode = string.Empty;
        private string _wxaccount = string.Empty;
        private string _bigcustomer = string.Empty;
        private string _cname = string.Empty;
        private DateTime _birthday = DateTime.Parse("1900-01-01");
        private string _sex = string.Empty;
        private string _mobile = string.Empty;
        private string _email = string.Empty;
        private string _tel = string.Empty;
        private string _idtype = string.Empty;
        private string _IDNO = string.Empty;
        private int _provinceid = 0;
        private int _cityid = 0;
        private int _areaid = 0;
        private string _photo = string.Empty;
        private string _signature = string.Empty;
        private string _address = string.Empty;
        private string _hobby = string.Empty;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private int _orderno = 0;
        private long _cuser = 0;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private long _uuser = 0;
        private DateTime _utime = DateTime.Parse("1900-01-01");
        private string _isdelete = string.Empty;
        private string _ousercode = string.Empty;
        private string _ousername = string.Empty;

        /// <summary>
        ///会员标识
        /// <summary>
        [ModelInfo(Name = "会员标识", ControlName = "txt_memid", NotEmpty = false, Length = 8, NotEmptyECode = "members_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_002")]
        public string memid
        {
            get { return _memid; }
            set { _memid = value; }
        }
        /// <summary>
        ///会员编号
        /// <summary>
        public string memcode
        {
            get { return _memcode; }
            set { _memcode = value; }
        }
        /// <summary>
        ///会员来源
        /// <summary>
        [ModelInfo(Name = "会员来源", ControlName = "txt_source", NotEmpty = false, Length = 16, NotEmptyECode = "members_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_008")]
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <summary>
        ///门店编号
        /// <summary>
        [ModelInfo(Name = "门店编号", ControlName = "txt_strcode", NotEmpty = false, Length = 8, NotEmptyECode = "members_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_014")]
        public string strcode
        {
            get { return _strcode; }
            set { _strcode = value; }
        }
        /// <summary>
        ///微信账户
        /// <summary>
        [ModelInfo(Name = "微信账户", ControlName = "txt_wxaccount", NotEmpty = false, Length = 20, NotEmptyECode = "members_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_017")]
        public string wxaccount
        {
            get { return _wxaccount; }
            set { _wxaccount = value; }
        }
        /// <summary>
        ///是否大客户
        /// <summary>
        [ModelInfo(Name = "是否大客户", ControlName = "txt_bigcustomer", NotEmpty = false, Length = 1, NotEmptyECode = "members_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_020")]
        public string bigcustomer
        {
            get { return _bigcustomer; }
            set { _bigcustomer = value; }
        }
        /// <summary>
        ///姓名
        /// <summary>
        [ModelInfo(Name = "姓名", ControlName = "txt_cname", NotEmpty = false, Length = 32, NotEmptyECode = "members_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_023")]
        public string cname
        {
            get { return _cname; }
            set { _cname = value; }
        }
        /// <summary>
        ///生日
        /// <summary>
        [ModelInfo(Name = "生日", ControlName = "txt_birthday", NotEmpty = false, Length = 4, NotEmptyECode = "members_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_026")]
        public DateTime birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }
        /// <summary>
        ///性别
        /// <summary>
        [ModelInfo(Name = "性别", ControlName = "txt_sex", NotEmpty = false, Length = 1, NotEmptyECode = "members_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_029")]
        public string sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        /// <summary>
        ///手机号码
        /// <summary>
        [ModelInfo(Name = "手机号码", ControlName = "txt_mobile", NotEmpty = false, Length = 12, NotEmptyECode = "members_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_032")]
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }
        /// <summary>
        ///邮箱地址
        /// <summary>
        [ModelInfo(Name = "邮箱地址", ControlName = "txt_email", NotEmpty = false, Length = 128, NotEmptyECode = "members_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_035")]
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        ///电话
        /// <summary>
        [ModelInfo(Name = "电话", ControlName = "txt_tel", NotEmpty = false, Length = 64, NotEmptyECode = "members_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_038")]
        public string tel
        {
            get { return _tel; }
            set { _tel = value; }
        }
        /// <summary>
        ///证件类型
        /// <summary>
        [ModelInfo(Name = "证件类型", ControlName = "txt_idtype", NotEmpty = false, Length = 2, NotEmptyECode = "members_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_041")]
        public string idtype
        {
            get { return _idtype; }
            set { _idtype = value; }
        }
        /// <summary>
        ///证件号码
        /// <summary>
        [ModelInfo(Name = "证件号码", ControlName = "txt_IDNO", NotEmpty = false, Length = 20, NotEmptyECode = "members_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_044")]
        public string IDNO
        {
            get { return _IDNO; }
            set { _IDNO = value; }
        }
        /// <summary>
        ///所属省
        /// <summary>
        [ModelInfo(Name = "所属省", ControlName = "txt_provinceid", NotEmpty = false, Length = 4, NotEmptyECode = "members_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_047")]
        public int provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        /// <summary>
        ///所属城市
        /// <summary>
        [ModelInfo(Name = "所属城市", ControlName = "txt_cityid", NotEmpty = false, Length = 4, NotEmptyECode = "members_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_050")]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        /// <summary>
        ///所属区域
        /// <summary>
        [ModelInfo(Name = "所属区域", ControlName = "txt_areaid", NotEmpty = false, Length = 4, NotEmptyECode = "members_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_053")]
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        /// <summary>
        ///照片
        /// <summary>
        [ModelInfo(Name = "照片", ControlName = "txt_photo", NotEmpty = false, Length = 50, NotEmptyECode = "members_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_056")]
        public string photo
        {
            get { return _photo; }
            set { _photo = value; }
        }
        /// <summary>
        ///电子签名
        /// <summary>
        [ModelInfo(Name = "电子签名", ControlName = "txt_signature", NotEmpty = false, Length = 50, NotEmptyECode = "members_058", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_059")]
        public string signature
        {
            get { return _signature; }
            set { _signature = value; }
        }
        /// <summary>
        ///会员地址
        /// <summary>
        [ModelInfo(Name = "会员地址", ControlName = "txt_address", NotEmpty = false, Length = 128, NotEmptyECode = "members_061", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_062")]
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        ///特殊爱好
        /// <summary>
        [ModelInfo(Name = "特殊爱好", ControlName = "txt_hobby", NotEmpty = false, Length = 128, NotEmptyECode = "members_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_065")]
        public string hobby
        {
            get { return _hobby; }
            set { _hobby = value; }
        }
        /// <summary>
        ///备注
        /// <summary>
        [ModelInfo(Name = "备注", ControlName = "txt_remark", NotEmpty = false, Length = 128, NotEmptyECode = "members_067", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_068")]
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        ///状态
        /// <summary>
        [ModelInfo(Name = "状态", ControlName = "txt_status", NotEmpty = false, Length = 1, NotEmptyECode = "members_070", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_071")]
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        ///排序号
        /// <summary>
        [ModelInfo(Name = "排序号", ControlName = "txt_orderno", NotEmpty = false, Length = 4, NotEmptyECode = "members_073", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "members_074")]
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
        ///  
        /// </summary>
        public string ousercode
        {
            get { return _ousercode; }
            set { _ousercode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ousername
        {
            get { return _ousername; }
            set { _ousername = value; }
        } 
    }
}