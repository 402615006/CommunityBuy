using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("退单原因管理")]
    [Serializable]
    public class TB_BackReasonEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _UCode = string.Empty;
		private string _UCname = string.Empty;
		private DateTime _UTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private int _Sort = 0;
		private string _PKCode = string.Empty;
		private string _Reason = string.Empty;
		private string _Ascription = string.Empty;
		private string _Remark = string.Empty;

		/// <summary>
		///Id
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
		///记录修改人编码
		/// <summary>
		public string UCode
		{
			get { return _UCode; }
			set { _UCode = value; }
		}
		/// <summary>
		///记录修改人姓名
		/// <summary>
		public string UCname
		{
			get { return _UCname; }
			set { _UCname = value; }
		}
		/// <summary>
		///记录修改时间
		/// <summary>
		public DateTime UTime
		{
			get { return _UTime; }
			set { _UTime = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///排序号
		/// <summary>
		public int Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		///原因编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///退单原因
		/// <summary>
		public string Reason
		{
			get { return _Reason; }
			set { _Reason = value; }
		}
		/// <summary>
		///责任归属
		/// <summary>
		public string Ascription
		{
			get { return _Ascription; }
			set { _Ascription = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		public string Remark
		{
			get { return _Remark; }
			set { _Remark = value; }
		}        
    }
}