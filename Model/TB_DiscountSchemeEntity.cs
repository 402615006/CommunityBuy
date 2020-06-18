using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("折扣方案")]
    [Serializable]
    public class TB_DiscountSchemeEntity
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
		private string _InsideCode = string.Empty;
		private decimal _DiscountRate = 0;
		private string _MenuCode = string.Empty;
		private string _LevelCode = string.Empty;
		private string _SchName = string.Empty;


        private List<TR_DiscountSchemeRateEntity> _DSRateList = new List<TR_DiscountSchemeRateEntity>();
        /// <summary>
        /// 特殊折扣
        /// </summary>
        public List<TR_DiscountSchemeRateEntity> DSRateList
        {
            get { return _DSRateList; }
            set { _DSRateList = value; }
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
		///方案编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///内部编号
		/// <summary>
		public string InsideCode
		{
			get { return _InsideCode; }
			set { _InsideCode = value; }
		}
		/// <summary>
		///一般折扣率
		/// <summary>
		public decimal DiscountRate
		{
			get { return _DiscountRate; }
			set { _DiscountRate = value; }
		}
		/// <summary>
		///菜谱
		/// <summary>
		public string MenuCode
		{
			get { return _MenuCode; }
			set { _MenuCode = value; }
		}
		/// <summary>
		///适用卡等级
		/// <summary>
		public string LevelCode
		{
			get { return _LevelCode; }
			set { _LevelCode = value; }
		}
		/// <summary>
		///
		/// <summary>
		public string SchName
		{
			get { return _SchName; }
			set { _SchName = value; }
		}        
    }
}