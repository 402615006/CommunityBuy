using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("商家门店信息")]
    [Serializable]
    public class WXStoreEntity
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
		private string _descr = string.Empty;
		private string _stourl = string.Empty;
		private string _stocoordx = string.Empty;
		private string _stocoordy = string.Empty;
		private DateTime _netlinklasttime = DateTime.Parse("1900-01-01");
		private DateTime _calcutime = DateTime.Parse("1900-01-01");
		private string _busHour = string.Empty;
		private string _recommended = string.Empty;
		private string _remark = string.Empty;
		private string _status = string.Empty;
		private long _cuser = 0;
		private DateTime _ctime = DateTime.Parse("1900-01-01");
		private long _uuser = 0;
		private DateTime _utime = DateTime.Parse("1900-01-01");
		private string _isdelete = string.Empty;
		private string _btime = string.Empty;
		private string _etime = string.Empty;
		private int _TerminalNumber = 0;
		private DateTime _ValuesDate = DateTime.Parse("1900-01-01");
		private string _isfood = string.Empty;
		private string _pstocode = string.Empty;
		private string _sqcode = string.Empty;
		private string _storetype = string.Empty;
		private decimal _jprice = 0;

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
		[ModelInfo(Name = "引用分公司表company的公司编号字段comcode的值",ControlName="txt_comcode", NotEmpty = false, Length = 8, NotEmptyECode = "Store_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_005")]
		public string comcode
		{
			get { return _comcode; }
			set { _comcode = value; }
		}
		/// <summary>
		///引用商户表Business的商户编号字段buscode的值
		/// <summary>
		[ModelInfo(Name = "引用商户表Business的商户编号字段buscode的值",ControlName="txt_buscode", NotEmpty = false, Length = 16, NotEmptyECode = "Store_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_008")]
		public string buscode
		{
			get { return _buscode; }
			set { _buscode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_stocode", NotEmpty = false, Length = 8, NotEmptyECode = "Store_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_011")]
		public string stocode
		{
			get { return _stocode; }
			set { _stocode = value; }
		}
		/// <summary>
		///门店名称
		/// <summary>
		[ModelInfo(Name = "门店名称",ControlName="txt_cname", NotEmpty = false, Length = 64, NotEmptyECode = "Store_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_014")]
		public string cname
		{
			get { return _cname; }
			set { _cname = value; }
		}
		/// <summary>
		///门店简称
		/// <summary>
		[ModelInfo(Name = "门店简称",ControlName="txt_sname", NotEmpty = false, Length = 16, NotEmptyECode = "Store_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_017")]
		public string sname
		{
			get { return _sname; }
			set { _sname = value; }
		}
		/// <summary>
		///门店简码
		/// <summary>
		[ModelInfo(Name = "门店简码",ControlName="txt_bcode", NotEmpty = false, Length = 16, NotEmptyECode = "Store_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_020")]
		public string bcode
		{
			get { return _bcode; }
			set { _bcode = value; }
		}
		/// <summary>
		///所属行业
		/// <summary>
		[ModelInfo(Name = "所属行业",ControlName="txt_indcode", NotEmpty = false, Length = 16, NotEmptyECode = "Store_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_023")]
		public string indcode
		{
			get { return _indcode; }
			set { _indcode = value; }
		}
		/// <summary>
		///所在省
		/// <summary>
		[ModelInfo(Name = "所在省",ControlName="txt_provinceid", NotEmpty = false, Length = 9, NotEmptyECode = "Store_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_026")]
		public int provinceid
		{
			get { return _provinceid; }
			set { _provinceid = value; }
		}
		/// <summary>
		///所在城市
		/// <summary>
		[ModelInfo(Name = "所在城市",ControlName="txt_cityid", NotEmpty = false, Length = 9, NotEmptyECode = "Store_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_029")]
		public int cityid
		{
			get { return _cityid; }
			set { _cityid = value; }
		}
		/// <summary>
		///所在区
		/// <summary>
		[ModelInfo(Name = "所在区",ControlName="txt_areaid", NotEmpty = false, Length = 9, NotEmptyECode = "Store_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_032")]
		public int areaid
		{
			get { return _areaid; }
			set { _areaid = value; }
		}
		/// <summary>
		///门店地址
		/// <summary>
		[ModelInfo(Name = "门店地址",ControlName="txt_address", NotEmpty = false, Length = 128, NotEmptyECode = "Store_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_035")]
		public string address
		{
			get { return _address; }
			set { _address = value; }
		}
		/// <summary>
		///负责人
		/// <summary>
		[ModelInfo(Name = "负责人",ControlName="txt_stoprincipal", NotEmpty = false, Length = 32, NotEmptyECode = "Store_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_038")]
		public string stoprincipal
		{
			get { return _stoprincipal; }
			set { _stoprincipal = value; }
		}
		/// <summary>
		///负责人联系电话
		/// <summary>
		[ModelInfo(Name = "负责人联系电话",ControlName="txt_stoprincipaltel", NotEmpty = false, Length = 32, NotEmptyECode = "Store_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_041")]
		public string stoprincipaltel
		{
			get { return _stoprincipaltel; }
			set { _stoprincipaltel = value; }
		}
		/// <summary>
		///门店电话
		/// <summary>
		[ModelInfo(Name = "门店电话",ControlName="txt_tel", NotEmpty = false, Length = 32, NotEmptyECode = "Store_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_044")]
		public string tel
		{
			get { return _tel; }
			set { _tel = value; }
		}
		/// <summary>
		///门店邮箱
		/// <summary>
		[ModelInfo(Name = "门店邮箱",ControlName="txt_stoemail", NotEmpty = false, Length = 64, NotEmptyECode = "Store_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_047")]
		public string stoemail
		{
			get { return _stoemail; }
			set { _stoemail = value; }
		}
		/// <summary>
		///门店Logo
		/// <summary>
		[ModelInfo(Name = "门店Logo",ControlName="txt_logo", NotEmpty = false, Length = 64, NotEmptyECode = "Store_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_050")]
		public string logo
		{
			get { return _logo; }
			set { _logo = value; }
		}
		/// <summary>
		///门店背景图
		/// <summary>
		[ModelInfo(Name = "门店背景图",ControlName="txt_backgroundimg", NotEmpty = false, Length = 64, NotEmptyECode = "Store_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_053")]
		public string backgroundimg
		{
			get { return _backgroundimg; }
			set { _backgroundimg = value; }
		}
		/// <summary>
		///门店描述
		/// <summary>
		[ModelInfo(Name = "门店描述",ControlName="txt_descr", NotEmpty = false, Length = -1, NotEmptyECode = "Store_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_056")]
		public string descr
		{
			get { return _descr; }
			set { _descr = value; }
		}
		/// <summary>
		///门店网址
		/// <summary>
		[ModelInfo(Name = "门店网址",ControlName="txt_stourl", NotEmpty = false, Length = 128, NotEmptyECode = "Store_058", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_059")]
		public string stourl
		{
			get { return _stourl; }
			set { _stourl = value; }
		}
		/// <summary>
		///X坐标
		/// <summary>
		[ModelInfo(Name = "X坐标",ControlName="txt_stocoordx", NotEmpty = false, Length = 128, NotEmptyECode = "Store_061", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_062")]
		public string stocoordx
		{
			get { return _stocoordx; }
			set { _stocoordx = value; }
		}
		/// <summary>
		///Y坐标
		/// <summary>
		[ModelInfo(Name = "Y坐标",ControlName="txt_stocoordy", NotEmpty = false, Length = 128, NotEmptyECode = "Store_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_065")]
		public string stocoordy
		{
			get { return _stocoordy; }
			set { _stocoordy = value; }
		}
		/// <summary>
		///最后联网时间
		/// <summary>
		[ModelInfo(Name = "最后联网时间",ControlName="txt_netlinklasttime", NotEmpty = false, Length = 19, NotEmptyECode = "Store_067", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_068")]
		public DateTime netlinklasttime
		{
			get { return _netlinklasttime; }
			set { _netlinklasttime = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_calcutime", NotEmpty = false, Length = 19, NotEmptyECode = "Store_070", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_071")]
		public DateTime calcutime
		{
			get { return _calcutime; }
			set { _calcutime = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_busHour", NotEmpty = false, Length = 32, NotEmptyECode = "Store_073", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_074")]
		public string busHour
		{
			get { return _busHour; }
			set { _busHour = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_recommended", NotEmpty = false, Length = 128, NotEmptyECode = "Store_076", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_077")]
		public string recommended
		{
			get { return _recommended; }
			set { _recommended = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_remark", NotEmpty = false, Length = 100, NotEmptyECode = "Store_079", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_080")]
		public string remark
		{
			get { return _remark; }
			set { _remark = value; }
		}
		/// <summary>
		///有效状态（0无效，1有效）
		/// <summary>
		[ModelInfo(Name = "有效状态（0无效，1有效）",ControlName="txt_status", NotEmpty = false, Length = 1, NotEmptyECode = "Store_082", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_083")]
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
		///引用系统用户表ts_admins的userid字段值
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
		///删除标志（0未删除，1已删除，默认值为0）
		/// <summary>
		public string isdelete
		{
			get { return _isdelete; }
			set { _isdelete = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_btime", NotEmpty = false, Length = 19, NotEmptyECode = "Store_100", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_101")]
		public string btime
		{
			get { return _btime; }
			set { _btime = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_etime", NotEmpty = false, Length = 19, NotEmptyECode = "Store_103", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_104")]
		public string etime
		{
			get { return _etime; }
			set { _etime = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_TerminalNumber", NotEmpty = false, Length = 9, NotEmptyECode = "Store_106", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_107")]
		public int TerminalNumber
		{
			get { return _TerminalNumber; }
			set { _TerminalNumber = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_ValuesDate", NotEmpty = false, Length = 19, NotEmptyECode = "Store_109", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_110")]
		public DateTime ValuesDate
		{
			get { return _ValuesDate; }
			set { _ValuesDate = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_isfood", NotEmpty = false, Length = 1, NotEmptyECode = "Store_112", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_113")]
		public string isfood
		{
			get { return _isfood; }
			set { _isfood = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_pstocode", NotEmpty = false, Length = 8, NotEmptyECode = "Store_115", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_116")]
		public string pstocode
		{
			get { return _pstocode; }
			set { _pstocode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_sqcode", NotEmpty = false, Length = 32, NotEmptyECode = "Store_118", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_119")]
		public string sqcode
		{
			get { return _sqcode; }
			set { _sqcode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_storetype", NotEmpty = false, Length = 32, NotEmptyECode = "Store_121", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_122")]
		public string storetype
		{
			get { return _storetype; }
			set { _storetype = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_jprice", NotEmpty = false, Length = 18, NotEmptyECode = "Store_124", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Store_125")]
		public decimal jprice
		{
			get { return _jprice; }
			set { _jprice = value; }
		}        
    }
}