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
		///角色ID
		/// <summary>
		public long RoleId
		{
			get { return _RoleId; }
			set { _RoleId = value; }
		}
		/// <summary>
		///用户ID
		/// <summary>
		public long UserId
		{
			get { return _UserId; }
			set { _UserId = value; }
		}
		/// <summary>
		///姓名
		/// <summary>
		public string RealName
		{
			get { return _RealName; }
			set { _RealName = value; }
		}
		/// <summary>
		///员工编号
		/// <summary>
		public string EmpCode
		{
			get { return _EmpCode; }
			set { _EmpCode = value; }
		}

        /// <summary>
        ///折扣方案ID
        /// <summary>
        public string RoleDisCount
        {
            get { return _RoleDisCount; }
            set { _RoleDisCount = value; }
        }

    }
}