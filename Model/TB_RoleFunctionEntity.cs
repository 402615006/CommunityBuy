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
		///角色ID
		/// <summary>
		public long RoleId
		{
			get { return _RoleId; }
			set { _RoleId = value; }
		}
		/// <summary>
		///功能ID
		/// <summary>
		public long FunctionId
		{
			get { return _FunctionId; }
			set { _FunctionId = value; }
		}        
    }
}