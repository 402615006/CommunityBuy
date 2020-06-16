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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TR_DishImage_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DishImage_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TR_DishImage_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DishImage_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///菜品编号
		/// <summary>
		[ModelInfo(Name = "菜品编号",ControlName="txt_DisCode", NotEmpty = false, Length = 32, NotEmptyECode = "TR_DishImage_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DishImage_011")]
		public string DisCode
		{
			get { return _DisCode; }
			set { _DisCode = value; }
		}
		/// <summary>
		///图片路径
		/// <summary>
		[ModelInfo(Name = "图片路径",ControlName="txt_ImgUrl", NotEmpty = false, Length = 128, NotEmptyECode = "TR_DishImage_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_DishImage_014")]
		public string ImgUrl
		{
			get { return _ImgUrl; }
			set { _ImgUrl = value; }
		}        
    }
}