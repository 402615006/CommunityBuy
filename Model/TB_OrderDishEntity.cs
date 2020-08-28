using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("订单菜品")]
    [Serializable]
    public class TB_OrderDishEntity
    {
		private long _Id = 0;
		private string _StoCode = string.Empty;
		private string _OrderCode = string.Empty;
		private string _DisCode = string.Empty;
		private string _DisName = string.Empty;
		private decimal _Price = 0;
		private string _DisUite = string.Empty;
		private decimal _DisNum = 0;
		private string _PKCode = string.Empty;
		private decimal _TotalMoney = 0;
		private string _CookName = string.Empty;
		private decimal _CookMoney = 0;
		/// <summary>
		///Id
		/// <summary>
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
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
		///订单编号
		/// <summary>
		public string OrderCode
		{
			get { return _OrderCode; }
			set { _OrderCode = value; }
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
		///菜品单价
		/// <summary>
		public decimal Price
		{
			get { return _Price; }
			set { _Price = value; }
		}
		/// <summary>
		///菜品单位
		/// <summary>
		public string DisUite
		{
			get { return _DisUite; }
			set { _DisUite = value; }
		}
		/// <summary>
		///菜品数量
		/// <summary>
		public decimal DisNum
		{
			get { return _DisNum; }
			set { _DisNum = value; }
		}

		/// <summary>
		///菜品订单号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///总价
		/// <summary>
		public decimal TotalMoney
		{
			get { return _TotalMoney; }
			set { _TotalMoney = value; }
		}

        public string CookName { get => _CookName; set => _CookName = value; }
        public decimal CookMoney { get => _CookMoney; set => _CookMoney = value; }
    }
}