using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.Model
{
    [Description("系统功能管理")]
    [Serializable]
    public class TB_FunctionsEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private int _FType = 0;
		private long _ParentId = 0;
		private string _Code = string.Empty;
		private string _Cname = string.Empty;
		private string _BtnCode = string.Empty;
		private int _Orders = 0;
		private string _ImgName = string.Empty;
		private string _Url = string.Empty;
		private int _Level = 0;
		private string _Descr = string.Empty;

		/// <summary>
		///Id主键自增
		/// <summary>
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		public string CCname
		{
			get { return _CCname; }
			set { _CCname = value; }
		}
		/// <summary>
		///记录创建时间
		/// <summary>
		public DateTime CTime
		{
			get { return _CTime; }
			set { _CTime = value; }
		}
		/// <summary>
		///状态  0无效   1有效
		/// <summary>
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///类型
		/// <summary>
		public int FType
		{
			get { return _FType; }
			set { _FType = value; }
		}
		/// <summary>
		///父ID
		/// <summary>
		public long ParentId
		{
			get { return _ParentId; }
			set { _ParentId = value; }
		}
		/// <summary>
		///功能编号
		/// <summary>
		public string Code
		{
			get { return _Code; }
			set { _Code = value; }
		}
		/// <summary>
		///功能名称
		/// <summary>
		public string Cname
		{
			get { return _Cname; }
			set { _Cname = value; }
		}
		/// <summary>
		///按钮编号
		/// <summary>
		public string BtnCode
		{
			get { return _BtnCode; }
			set { _BtnCode = value; }
		}
		/// <summary>
		///排序号
		/// <summary>
		public int Orders
		{
			get { return _Orders; }
			set { _Orders = value; }
		}
		/// <summary>
		///图片名称
		/// <summary>
		public string ImgName
		{
			get { return _ImgName; }
			set { _ImgName = value; }
		}
		/// <summary>
		///图片路径
		/// <summary>
		public string Url
		{
			get { return _Url; }
			set { _Url = value; }
		}
		/// <summary>
		///
		/// <summary>
		public int Level
		{
			get { return _Level; }
			set { _Level = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		public string Descr
		{
			get { return _Descr; }
			set { _Descr = value; }
		}        
    }
}