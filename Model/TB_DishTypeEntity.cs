using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("菜品类别表")]
    [Serializable]
    public class TB_DishTypeEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _PKKCode = string.Empty;
		private string _PKCode = string.Empty;
		private string _TypeName = string.Empty;
		private int _Sort = 0;
		private string _TStatus = string.Empty;
        private string _StoName = string.Empty;

        /// <summary>
		///门店编号
		/// <summary>
        public string StoName
        {
            get { return _StoName; }
            set { _StoName = value; }
        }

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
		///记录创建时间
		/// <summary>
		public DateTime CTime
		{
			get { return _CTime; }
			set { _CTime = value; }
		}
		/// <summary>
		///父类别编号
		/// <summary>
		public string PKKCode
		{
			get { return _PKKCode; }
			set { _PKKCode = value; }
		}
		/// <summary>
		///类别编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///类别名称
		/// <summary>
		public string TypeName
		{
			get { return _TypeName; }
			set { _TypeName = value; }
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
		///状态
		/// <summary>
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}        
    }
}