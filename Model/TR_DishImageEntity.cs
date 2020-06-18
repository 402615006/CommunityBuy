using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("菜品图片")]
    [Serializable]
    public class TR_DishImageEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _DisCode = string.Empty;
		private string _ImgUrl = string.Empty;

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
		///菜品编号
		/// <summary>
		public string DisCode
		{
			get { return _DisCode; }
			set { _DisCode = value; }
		}
		/// <summary>
		///图片路径
		/// <summary>
		public string ImgUrl
		{
			get { return _ImgUrl; }
			set { _ImgUrl = value; }
		}        
    }
}