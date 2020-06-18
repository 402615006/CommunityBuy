using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("购物车设置")]
    [Serializable]
    public class TB_ShoppingCartEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _PKCode = string.Empty;
		private DateTime _AutoDelTime = DateTime.Parse("1900-01-01");
		private string _IsAutoDelete = string.Empty;
		private int _MaxNum = 0;

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
		///编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///自动删除时间
		/// <summary>
		public DateTime AutoDelTime
		{
			get { return _AutoDelTime; }
			set { _AutoDelTime = value; }
		}
		/// <summary>
		///是否自动删除
		/// <summary>
		public string IsAutoDelete
		{
			get { return _IsAutoDelete; }
			set { _IsAutoDelete = value; }
		}
		/// <summary>
		///商品最大数量
		/// <summary>
		public int MaxNum
		{
			get { return _MaxNum; }
			set { _MaxNum = value; }
		}        
    }
}