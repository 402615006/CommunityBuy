using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("菜品信息")]
    [Serializable]
    public class TB_DishEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _ChannelCodeList = string.Empty;
		private string _DisCode = string.Empty;
		private string _DisName = string.Empty;
		private string _OtherName = string.Empty;
		private string _TypeCode = string.Empty;
		private string _QuickCode = string.Empty;
		private string _CusDisCode = string.Empty;
		private string _Unit = string.Empty;
		private decimal _Price = 0;

		private decimal _MemPrice = 0;
		private decimal _CostPrice = 0;
		private decimal _RoyMoney = 0;

		private string _QRCode = string.Empty;

		private string _Descript = string.Empty;

		private string _IsPoint = string.Empty;
		private string _IsMemPrice = string.Empty;
		private string _IsCoupon = string.Empty;


        #region 附加属性
        private string _ImageName = string.Empty;
        public string ImageName
        {
            get { return _ImageName; }
            set { _ImageName = value; }
        }

        private List<TR_DishImageEntity> _DishImage = new List<TR_DishImageEntity>();
        /// <summary>
        /// 菜品图片集合
        /// </summary>
        public List<TR_DishImageEntity> DishImage
        {
            get { return _DishImage; }
            set { _DishImage = value; }
        }
        #endregion

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
		///状态
		/// <summary>
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///适用渠道
		/// <summary>
		public string ChannelCodeList
		{
			get { return _ChannelCodeList; }
			set { _ChannelCodeList = value; }
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
		///菜品名称
		/// <summary>
		public string DisName
		{
			get { return _DisName; }
			set { _DisName = value; }
		}
		/// <summary>
		///其他名称
		/// <summary>
		public string OtherName
		{
			get { return _OtherName; }
			set { _OtherName = value; }
		}
		/// <summary>
		///菜品类别
		/// <summary>
		public string TypeCode
		{
			get { return _TypeCode; }
			set { _TypeCode = value; }
		}
		/// <summary>
		///速查码
		/// <summary>
		public string QuickCode
		{
			get { return _QuickCode; }
			set { _QuickCode = value; }
		}

		/// <summary>
		///单位
		/// <summary>
		public string Unit
		{
			get { return _Unit; }
			set { _Unit = value; }
		}
		/// <summary>
		///售价
		/// <summary>
		public decimal Price
		{
			get { return _Price; }
			set { _Price = value; }
		}

		/// <summary>
		///会员价
		/// <summary>
		public decimal MemPrice
		{
			get { return _MemPrice; }
			set { _MemPrice = value; }
		}
		/// <summary>
		///成本价
		/// <summary>
		public decimal CostPrice
		{
			get { return _CostPrice; }
			set { _CostPrice = value; }
		}
		/// <summary>
		///提成金额
		/// <summary>
		public decimal RoyMoney
		{
			get { return _RoyMoney; }
			set { _RoyMoney = value; }
		}
		/// <summary>
		///菜品二维码
		/// <summary>
		public string QRCode
		{
			get { return _QRCode; }
			set { _QRCode = value; }
		}
		/// <summary>
		///描述
		/// <summary>
		public string Descript
		{
			get { return _Descript; }
			set { _Descript = value; }
		}
		/// <summary>
		///是否可兑换
		/// <summary>
		public string IsPoint
		{
			get { return _IsPoint; }
			set { _IsPoint = value; }
		}
		/// <summary>
		///是否允许会员价
		/// <summary>
		public string IsMemPrice
		{
			get { return _IsMemPrice; }
			set { _IsMemPrice = value; }
		}
		/// <summary>
		///是否支持使用消费券
		/// <summary>
		public string IsCoupon
		{
			get { return _IsCoupon; }
			set { _IsCoupon = value; }
		}    
    }
}