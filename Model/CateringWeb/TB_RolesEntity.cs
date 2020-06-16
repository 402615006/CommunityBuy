using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.Model
{
    [Description("门店角色信息")]
    [Serializable]
    public class TB_RolesEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _TStatus = string.Empty;
		private string _UCode = string.Empty;
		private string _UCname = string.Empty;
		private DateTime _UTime = DateTime.Parse("1900-01-01");
		private int _SCope = 0;
		private string _RoleName = string.Empty;
		private int _RoleParent = 0;
		private string _RoleDescr = string.Empty;
		private string _RoleDisCount = string.Empty;
		private string _RoleEnable = string.Empty;
		private decimal _MaxDiffPrice = 0;
		private decimal _MaxPrefePrice = 0;
		private string _IsSig = string.Empty;
		private decimal _Sigcredit = 0;
		private string _RoleType = string.Empty;
		private string _TerminalType = string.Empty;

        #region 附加属性
        private string _RoleDisCountName = string.Empty;
        /// <summary>
        /// 折扣方案名称，多个使用，分隔
        /// </summary>
        public string RoleDisCountName
        {
            get { return _RoleDisCountName; }
            set { _RoleDisCountName = value; }
        }

        private string _StoName = string.Empty;
        /// <summary>
        /// 门店名称
        /// </summary>
        public string StoName
        {
            get { return _StoName; }
            set { _StoName = value; }
        }
        #endregion

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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Roles_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_Roles_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_008")]
		public string StoCode
		{
			get { return _StoCode; }
			set { _StoCode = value; }
		}
		/// <summary>
		///记录创建人编码
		/// <summary>
		[ModelInfo(Name = "记录创建人编码",ControlName="txt_CCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Roles_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_011")]
		public string CCode
		{
			get { return _CCode; }
			set { _CCode = value; }
		}
		/// <summary>
		///记录创建人姓名
		/// <summary>
		[ModelInfo(Name = "记录创建人姓名",ControlName="txt_CCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Roles_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_014")]
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
		///状态  0无效   1有效
		/// <summary>
		[ModelInfo(Name = "状态  0无效   1有效",ControlName="txt_TStatus", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Roles_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_020")]
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///记录修改人编号
		/// <summary>
		[ModelInfo(Name = "记录修改人编号",ControlName="txt_UCode", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Roles_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_023")]
		public string UCode
		{
			get { return _UCode; }
			set { _UCode = value; }
		}
		/// <summary>
		///记录修改人姓名
		/// <summary>
		[ModelInfo(Name = "记录修改人姓名",ControlName="txt_UCname", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Roles_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_026")]
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
		///角色范围
		/// <summary>
		[ModelInfo(Name = "角色范围",ControlName="txt_SCope", NotEmpty = false, Length = 3, NotEmptyECode = "TB_Roles_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_032")]
		public int SCope
		{
			get { return _SCope; }
			set { _SCope = value; }
		}
		/// <summary>
		///角色名称
		/// <summary>
		[ModelInfo(Name = "角色名称",ControlName="txt_RoleName", NotEmpty = false, Length = 16, NotEmptyECode = "TB_Roles_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_035")]
		public string RoleName
		{
			get { return _RoleName; }
			set { _RoleName = value; }
		}
		/// <summary>
		///所属角色
		/// <summary>
		[ModelInfo(Name = "所属角色",ControlName="txt_RoleParent", NotEmpty = false, Length = 9, NotEmptyECode = "TB_Roles_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_038")]
		public int RoleParent
		{
			get { return _RoleParent; }
			set { _RoleParent = value; }
		}
		/// <summary>
		///角色描述
		/// <summary>
		[ModelInfo(Name = "角色描述",ControlName="txt_RoleDescr", NotEmpty = false, Length = 128, NotEmptyECode = "TB_Roles_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_041")]
		public string RoleDescr
		{
			get { return _RoleDescr; }
			set { _RoleDescr = value; }
		}
		/// <summary>
		///折扣方案ID
		/// <summary>
		[ModelInfo(Name = "折扣方案ID",ControlName="txt_RoleDisCount", NotEmpty = false, Length = 256, NotEmptyECode = "TB_Roles_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_044")]
		public string RoleDisCount
		{
			get { return _RoleDisCount; }
			set { _RoleDisCount = value; }
		}
		/// <summary>
		///启用
		/// <summary>
		[ModelInfo(Name = "启用",ControlName="txt_RoleEnable", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Roles_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_047")]
		public string RoleEnable
		{
			get { return _RoleEnable; }
			set { _RoleEnable = value; }
		}
		/// <summary>
		///可换菜最大差额
		/// <summary>
		[ModelInfo(Name = "可换菜最大差额",ControlName="txt_MaxDiffPrice", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Roles_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_050")]
		public decimal MaxDiffPrice
		{
			get { return _MaxDiffPrice; }
			set { _MaxDiffPrice = value; }
		}
		/// <summary>
		///优免最大金额
		/// <summary>
		[ModelInfo(Name = "优免最大金额",ControlName="txt_MaxPrefePrice", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Roles_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_053")]
		public decimal MaxPrefePrice
		{
			get { return _MaxPrefePrice; }
			set { _MaxPrefePrice = value; }
		}
		/// <summary>
		///是否允许签单
		/// <summary>
		[ModelInfo(Name = "是否允许签单",ControlName="txt_IsSig", NotEmpty = false, Length = 1, NotEmptyECode = "TB_Roles_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_056")]
		public string IsSig
		{
			get { return _IsSig; }
			set { _IsSig = value; }
		}
		/// <summary>
		///签单金额
		/// <summary>
		[ModelInfo(Name = "签单金额",ControlName="txt_Sigcredit", NotEmpty = false, Length = 18, NotEmptyECode = "TB_Roles_058", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_059")]
		public decimal Sigcredit
		{
			get { return _Sigcredit; }
			set { _Sigcredit = value; }
		}
		/// <summary>
		///角色类别编号
		/// <summary>
		[ModelInfo(Name = "角色类别编号",ControlName="txt_RoleType", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Roles_061", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_062")]
		public string RoleType
		{
			get { return _RoleType; }
			set { _RoleType = value; }
		}
		/// <summary>
		///终端类别
		/// <summary>
		[ModelInfo(Name = "终端类别",ControlName="txt_TerminalType", NotEmpty = false, Length = 32, NotEmptyECode = "TB_Roles_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_065")]
		public string TerminalType
		{
			get { return _TerminalType; }
			set { _TerminalType = value; }
		}        
    }
}