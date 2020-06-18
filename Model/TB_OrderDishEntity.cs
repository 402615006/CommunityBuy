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
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _OrderCode = string.Empty;
		private string _FinCode = string.Empty;
		private string _DisTypeCode = string.Empty;
		private string _DisCode = string.Empty;
		private string _DisName = string.Empty;
		private decimal _MemPrice = 0;
		private decimal _Price = 0;
		private string _DisUite = string.Empty;
		private decimal _DisNum = 0;
		private decimal _ReturnNum = 0;
		private string _IsPackage = string.Empty;
		private string _PDisCode = string.Empty;
		private string _Remar = string.Empty;
		private string _PKCode = string.Empty;
		private decimal _DiscountPrice = 0;
		private string _DiscountRemark = string.Empty;
		private string _DiscountType = string.Empty;
		private string _DisCase = string.Empty;
		private string _Favor = string.Empty;
		private decimal _ItemNum = 0;
		private decimal _ItemPrice = 0;
		private string _CookName = string.Empty;
		private decimal _CookMoney = 0;
		private decimal _TotalMoney = 0;

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
		///订单编号
		/// <summary>
		public string OrderCode
		{
			get { return _OrderCode; }
			set { _OrderCode = value; }
		}
		/// <summary>
		///财务类别编号
		/// <summary>
		public string FinCode
		{
			get { return _FinCode; }
			set { _FinCode = value; }
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
		///菜品名称
		/// <summary>
		public string DisName
		{
			get { return _DisName; }
			set { _DisName = value; }
		}
		/// <summary>
		///菜品会员价
		/// <summary>
		public decimal MemPrice
		{
			get { return _MemPrice; }
			set { _MemPrice = value; }
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
		///退菜数量
		/// <summary>
		public decimal ReturnNum
		{
			get { return _ReturnNum; }
			set { _ReturnNum = value; }
		}
		/// <summary>
		///套餐类型0普通菜品1套餐2套餐内菜品
		/// <summary>
		public string IsPackage
		{
			get { return _IsPackage; }
			set { _IsPackage = value; }
		}
		/// <summary>
		///父菜品编号
		/// <summary>
		public string PDisCode
		{
			get { return _PDisCode; }
			set { _PDisCode = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		public string Remar
		{
			get { return _Remar; }
			set { _Remar = value; }
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
		///折扣价格
		/// <summary>
		public decimal DiscountPrice
		{
			get { return _DiscountPrice; }
			set { _DiscountPrice = value; }
		}
		/// <summary>
		///折扣备注
		/// <summary>
		public string DiscountRemark
		{
			get { return _DiscountRemark; }
			set { _DiscountRemark = value; }
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
		///菜品方案1普通2称重3条只
		/// <summary>
		public string DisCase
		{
			get { return _DisCase; }
			set { _DisCase = value; }
		}
		/// <summary>
		///口味信息
		/// <summary>
		public string Favor
		{
			get { return _Favor; }
			set { _Favor = value; }
		}
		/// <summary>
		///条只数量
		/// <summary>
		public decimal ItemNum
		{
			get { return _ItemNum; }
			set { _ItemNum = value; }
		}
		/// <summary>
		///条只单价
		/// <summary>
		public decimal ItemPrice
		{
			get { return _ItemPrice; }
			set { _ItemPrice = value; }
		}
		/// <summary>
		///做法名称
		/// <summary>
		public string CookName
		{
			get { return _CookName; }
			set { _CookName = value; }
		}
		/// <summary>
		///做法加价
		/// <summary>
		public decimal CookMoney
		{
			get { return _CookMoney; }
			set { _CookMoney = value; }
		}
		/// <summary>
		///总价
		/// <summary>
		public decimal TotalMoney
		{
			get { return _TotalMoney; }
			set { _TotalMoney = value; }
		}        
    }
}