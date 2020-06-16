using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("用户角色关系表")]
    [Serializable]
    public class TB_UserRoleEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private long _RoleId = 0;
		private long _UserId = 0;
		private string _RealName = string.Empty;
		private string _EmpCode = string.Empty;
        private string _RoleDisCount = string.Empty;

        #region 附加属性
        private string _StrRoleId = string.Empty;
        /// <summary>
        /// 绑定用户的角色id,多个使用，分隔
        /// </summary>
        public string StrRoleId
        {
            get { return _StrRoleId; }
            set { _StrRoleId = value; }
        }

        /// <summary>
        /// 用户签送方案
        /// </summary>
        private List<TB_UserSigSchemeEntity> _UserSig = new List<TB_UserSigSchemeEntity>();
        public List<TB_UserSigSchemeEntity> UserSig
        {
            get
            {
                return _UserSig;
            }
            set
            {
                _UserSig = value;
            }
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
		[ModelInfo(Name = "商户标识",ControlName="txt_BusCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_UserRole_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserRole_005")]
		public string BusCode
		{
			get { return _BusCode; }
			set { _BusCode = value; }
		}
		/// <summary>
		///门店编号
		/// <summary>
		[ModelInfo(Name = "门店编号",ControlName="txt_StoCode", NotEmpty = false, Length = 8, NotEmptyECode = "TB_UserRole_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserRole_008")]
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
		///角色ID
		/// <summary>
		[ModelInfo(Name = "角色ID",ControlName="txt_RoleId", NotEmpty = false, Length = 18, NotEmptyECode = "TB_UserRole_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserRole_014")]
		public long RoleId
		{
			get { return _RoleId; }
			set { _RoleId = value; }
		}
		/// <summary>
		///用户ID
		/// <summary>
		[ModelInfo(Name = "用户ID",ControlName="txt_UserId", NotEmpty = false, Length = 18, NotEmptyECode = "TB_UserRole_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserRole_017")]
		public long UserId
		{
			get { return _UserId; }
			set { _UserId = value; }
		}
		/// <summary>
		///姓名
		/// <summary>
		[ModelInfo(Name = "姓名",ControlName="txt_RealName", NotEmpty = false, Length = 32, NotEmptyECode = "TB_UserRole_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserRole_020")]
		public string RealName
		{
			get { return _RealName; }
			set { _RealName = value; }
		}
		/// <summary>
		///员工编号
		/// <summary>
		[ModelInfo(Name = "员工编号",ControlName="txt_EmpCode", NotEmpty = false, Length = 16, NotEmptyECode = "TB_UserRole_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_UserRole_023")]
		public string EmpCode
		{
			get { return _EmpCode; }
			set { _EmpCode = value; }
		}

        /// <summary>
        ///折扣方案ID
        /// <summary>
        [ModelInfo(Name = "折扣方案ID", ControlName = "txt_RoleDisCount", NotEmpty = false, Length = 256, NotEmptyECode = "TB_Roles_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "TB_Roles_044")]
        public string RoleDisCount
        {
            get { return _RoleDisCount; }
            set { _RoleDisCount = value; }
        }

    }
}