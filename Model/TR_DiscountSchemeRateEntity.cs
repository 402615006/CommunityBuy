using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("折扣方案特殊折扣率")]
    [Serializable]
    public class TR_DiscountSchemeRateEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private int _Sort = 0;
		private string _SchCode = string.Empty;
		private string _DiscountType = string.Empty;
		private string _DisTypeCode = string.Empty;
		private string _DisCode = string.Empty;
		private decimal _DiscountRate = 0;

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
		///所属折扣方案编号
		/// <summary>
		public string SchCode
		{
			get { return _SchCode; }
			set { _SchCode = value; }
		}
		/// <summary>
		///折扣类型
		/// <summary>
		public string DiscountType
		{
			get { return _DiscountType; }
			set { _DiscountType = value; }
		}
		/// <summary>
		///菜品类别编号
		/// <summary>
		public string DisTypeCode
		{
			get { return _DisTypeCode; }
			set { _DisTypeCode = value; }
		}
		/// <summary>
		///菜品编号
		/// <summary>
		public string DisCode
		{
			get { return _DisCode; }
			set { _DisCode = value; }
		}
		/// <summary>
		///折扣率
		/// <summary>
		public decimal DiscountRate
		{
			get { return _DiscountRate; }
			set { _DiscountRate = value; }
		}        
    }
}