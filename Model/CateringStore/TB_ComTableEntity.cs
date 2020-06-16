using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("并台记录")]
    [Serializable]
    public class TB_ComTableEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _PKCode = string.Empty;
		private string _OpenCodeList = string.Empty;
		private string _Remark = string.Empty;

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_ComTable_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ComTable_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 4, NotEmptyECode = "TB_ComTable_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ComTable_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_ComTable_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ComTable_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_ComTable_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ComTable_014")]
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
		[ModelInfo(Name = "状态",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_ComTable_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ComTable_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///并台编号
		/// <summary>
		[ModelInfo(Name = "并台编号",ControlName="txt_PKCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_ComTable_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ComTable_023")]
		public string PKCode
		{
			get { return _PKCode; }
			set { _PKCode = value; }
		}
		/// <summary>
		///开台编号
		/// <summary>
		[ModelInfo(Name = "开台编号",ControlName="txt_OpenCodeList", NotEmpty = false, Length = 4096, NotEmptyECode = "TB_ComTable_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ComTable_026")]
		public string OpenCodeList
		{
			get { return _OpenCodeList; }
			set { _OpenCodeList = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_Remark", NotEmpty = false, Length = 64, NotEmptyECode = "TB_ComTable_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_ComTable_029")]
		public string Remark
		{
			get { return _Remark; }
			set { _Remark = value; }
		}        
    }
}