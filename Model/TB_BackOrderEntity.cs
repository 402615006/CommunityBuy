﻿using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("退单信息")]
    [Serializable]
    public class TB_BackOrderEntity
    {
		private long _Id = 0;
		private string _BusCode = string.Empty;
		private string _StoCode = string.Empty;
		private string _CCode = string.Empty;
		private string _CCname = string.Empty;
		private DateTime _CTime = DateTime.Parse("1900-01-01");
		private string _AuthCode = string.Empty;
		private string _AuthName = string.Empty;
		private string _TStatus = string.Empty;
		private string _OrderCode = string.Empty;
		private string _OrderDisCode = string.Empty;
		private string _ReasonCode = string.Empty;
		private string _ReasonName = string.Empty;
		private string _Remar = string.Empty;
		private decimal _BackNum = 0;

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
		///授权人编号
		/// <summary>
		public string AuthCode
		{
			get { return _AuthCode; }
			set { _AuthCode = value; }
		}
		/// <summary>
		///授权人姓名
		/// <summary>
		public string AuthName
		{
			get { return _AuthName; }
			set { _AuthName = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		public string TStatus
		{
			get { return _TStatus; }
			set { _TStatus = value; }
		}
		/// <summary>
		///订单编号
		/// <summary>
		public string OrderCode
		{
			get { return _OrderCode; }
			set { _OrderCode = value; }
		}
		/// <summary>
		///点单菜品编号
		/// <summary>
		public string OrderDisCode
		{
			get { return _OrderDisCode; }
			set { _OrderDisCode = value; }
		}
		/// <summary>
		///退单原因编号
		/// <summary>
		public string ReasonCode
		{
			get { return _ReasonCode; }
			set { _ReasonCode = value; }
		}
		/// <summary>
		///退单原因
		/// <summary>
		public string ReasonName
		{
			get { return _ReasonName; }
			set { _ReasonName = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		public string Remar
		{
			get { return _Remar; }
			set { _Remar = value; }
		}
		/// <summary>
		///退菜数量
		/// <summary>
		public decimal BackNum
		{
			get { return _BackNum; }
			set { _BackNum = value; }
		}        
    }
}