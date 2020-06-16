using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("商圈")]
    [Serializable]
    public class WXsqinfoEntity
    {
		private long _id = 0;
		private string _sqcode = string.Empty;
		private string _sqname = string.Empty;
		private string _city = string.Empty;
		private string _jwcodes = string.Empty;
		private DateTime _ctime = DateTime.Parse("1900-01-01");
		private long _cuser = 0;
		private string _status = string.Empty;
		private DateTime _utime = DateTime.Parse("1900-01-01");
		private long _uuser = 0;
		private string _isdelete = string.Empty;

		/// <summary>
		///
		/// <summary>
		public long id
		{
			get { return _id; }
			set { _id = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_sqcode", NotEmpty = false, Length = 32, NotEmptyECode = "sqinfo_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sqinfo_005")]
		public string sqcode
		{
			get { return _sqcode; }
			set { _sqcode = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_sqname", NotEmpty = false, Length = 128, NotEmptyECode = "sqinfo_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sqinfo_008")]
		public string sqname
		{
			get { return _sqname; }
			set { _sqname = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_city", NotEmpty = false, Length = 32, NotEmptyECode = "sqinfo_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sqinfo_011")]
		public string city
		{
			get { return _city; }
			set { _city = value; }
		}
		/// <summary>
		///经纬度坐标集合，多个用分号分隔
		/// <summary>
		[ModelInfo(Name = "经纬度坐标集合，多个用分号分隔",ControlName="txt_jwcodes", NotEmpty = false, Length = 256, NotEmptyECode = "sqinfo_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sqinfo_014")]
		public string jwcodes
		{
			get { return _jwcodes; }
			set { _jwcodes = value; }
		}
		/// <summary>
		///
		/// <summary>
		public DateTime ctime
		{
			get { return _ctime; }
			set { _ctime = value; }
		}
		/// <summary>
		///
		/// <summary>
		public long cuser
		{
			get { return _cuser; }
			set { _cuser = value; }
		}
		/// <summary>
		///
		/// <summary>
		[ModelInfo(Name = "",ControlName="txt_status", NotEmpty = false, Length = 1, NotEmptyECode = "sqinfo_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sqinfo_023")]
		public string status
		{
			get { return _status; }
			set { _status = value; }
		}
		/// <summary>
		///
		/// <summary>
		public DateTime utime
		{
			get { return _utime; }
			set { _utime = value; }
		}
		/// <summary>
		///
		/// <summary>
		public long uuser
		{
			get { return _uuser; }
			set { _uuser = value; }
		}
		/// <summary>
		///
		/// <summary>
		public string isdelete
		{
			get { return _isdelete; }
			set { _isdelete = value; }
		}        
    }
}