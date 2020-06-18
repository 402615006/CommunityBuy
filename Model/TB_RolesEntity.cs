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
		private int _SCope = 0;
		private string _RoleName = string.Empty;
		private int _RoleParent = 0;
		private string _RoleDescr = string.Empty;
		private string _RoleEnable = string.Empty;
		private string _RoleType = string.Empty;

        #region 附加属性
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
		///状态  0无效   1有效
		/// <summary>
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///角色名称
		/// <summary>
		public string RoleName
		{
			get { return _RoleName; }
			set { _RoleName = value; }
		}
		/// <summary>
		///所属角色
		/// <summary>
		public int RoleParent
		{
			get { return _RoleParent; }
			set { _RoleParent = value; }
		}
		/// <summary>
		///角色描述
		/// <summary>
		public string RoleDescr
		{
			get { return _RoleDescr; }
			set { _RoleDescr = value; }
		}

		/// <summary>
		///启用
		/// <summary>
		public string RoleEnable
		{
			get { return _RoleEnable; }
			set { _RoleEnable = value; }
		}

		/// <summary>
		///角色类别编号
		/// <summary>
		public string RoleType
		{
			get { return _RoleType; }
			set { _RoleType = value; }
		}
      
    }
}