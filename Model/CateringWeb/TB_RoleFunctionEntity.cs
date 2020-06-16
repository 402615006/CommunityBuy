using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("门店权限管理")]
    [Serializable]
    public class TB_RoleFunctionEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private long _RoleId = 0;
		private long _FunctionId = 0;

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_RoleFunction_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_RoleFunction_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_RoleFunction_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_RoleFunction_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_RoleFunction_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_RoleFunction_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_RoleFunction_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_RoleFunction_014")]
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
		///角色ID
		/// <summary>
		[ModelInfo(Name = "角色ID",ControlName="txt_RoleId", NotEmpty = false, Length = 18, NotEmptyECode = "TB_RoleFunction_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_RoleFunction_020")]
		public long RoleId
		{
			get { return _RoleId; }
			set { _RoleId = value; }
		}
		/// <summary>
		///功能ID
		/// <summary>
		[ModelInfo(Name = "功能ID",ControlName="txt_FunctionId", NotEmpty = false, Length = 18, NotEmptyECode = "TB_RoleFunction_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_RoleFunction_023")]
		public long FunctionId
		{
			get { return _FunctionId; }
			set { _FunctionId = value; }
		}        
    }
}