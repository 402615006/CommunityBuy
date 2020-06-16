using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("套餐组别信息")]
    [Serializable]
    public class TR_ComboInfoEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _PDisCode = string.Empty;
		private string _ComGroupCode = string.Empty;
		private string _CombinationType = string.Empty;
		private int _MaxOptNum = 0;
		private int _TotalOptiNum = 0;
		private decimal _TotalOptMoney = 0;
		private string _PKCode = string.Empty;

		/// <summary>
		///Id
		/// <summary>
		[ModelInfo(Name = "Id",ControlName="txt_Id", NotEmpty = false, Length = 18, NotEmptyECode = "TR_ComboInfo_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_ComboInfo_002")]
		public long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///商户标识
		/// <summary>
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TR_ComboInfo_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_ComboInfo_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TR_ComboInfo_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_ComboInfo_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///套餐编号
		/// <summary>
		[ModelInfo(Name = "套餐编号",ControlName="txt_PDisCode", NotEmpty = false, Length = 32, NotEmptyECode = "TR_ComboInfo_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_ComboInfo_011")]
		public string PDisCode
		{
			get { return _PDisCode; }
			set { _PDisCode = value; }
		}
		/// <summary>
		///套餐组别编号
		/// <summary>
		[ModelInfo(Name = "套餐组别编号",ControlName="txt_ComGroupCode", NotEmpty = false, Length = 32, NotEmptyECode = "TR_ComboInfo_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_ComboInfo_014")]
		public string ComGroupCode
		{
			get { return _ComGroupCode; }
			set { _ComGroupCode = value; }
		}
		/// <summary>
		///组合方案
		/// <summary>
		[ModelInfo(Name = "组合方案",ControlName="txt_CombinationType", NotEmpty = false, Length = 1, NotEmptyECode = "TR_ComboInfo_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_ComboInfo_017")]
		public string CombinationType
		{
			get { return _CombinationType; }
			set { _CombinationType = value; }
		}
		/// <summary>
		///最大可选种数
		/// <summary>
		[ModelInfo(Name = "最大可选种数",ControlName="txt_MaxOptNum", NotEmpty = false, Length = 9, NotEmptyECode = "TR_ComboInfo_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_ComboInfo_020")]
		public int MaxOptNum
		{
			get { return _MaxOptNum; }
			set { _MaxOptNum = value; }
		}
		/// <summary>
		///合计可选总数量
		/// <summary>
		[ModelInfo(Name = "合计可选总数量",ControlName="txt_TotalOptiNum", NotEmpty = false, Length = 9, NotEmptyECode = "TR_ComboInfo_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_ComboInfo_023")]
		public int TotalOptiNum
		{
			get { return _TotalOptiNum; }
			set { _TotalOptiNum = value; }
		}
		/// <summary>
		///可选总金额
		/// <summary>
		[ModelInfo(Name = "可选总金额",ControlName="txt_TotalOptMoney", NotEmpty = false, Length = 18, NotEmptyECode = "TR_ComboInfo_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TR_ComboInfo_026")]
		public decimal TotalOptMoney
		{
			get { return _TotalOptMoney; }
			set { _TotalOptMoney = value; }
		}
		/// <summary>
		///套餐组别信息编号
		/// <summary>
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}        
    }
}