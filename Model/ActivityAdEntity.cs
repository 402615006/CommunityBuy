using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("")]
    [Serializable]
    public class ActivityAdEntity
    {
		private int _Id = 0;
		private string _Title = string.Empty;
		private int _status = 0;
		private int _Sort = 0;
		private string _Description = string.Empty;
		private int _Type = 0;
		private string _images = string.Empty;
		private string _Url = string.Empty;

		/// <summary>
		///Id
		/// <summary>
		public int Id
		{
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		///活动名称
		/// <summary>
		public string Title
		{
			get { return _Title; }
			set { _Title = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		public int status
		{
			get { return _status; }
			set { _status = value; }
		}
		/// <summary>
		///排序
		/// <summary>
		public int Sort
		{
			get { return _Sort; }
			set { _Sort = value; }
		}
		/// <summary>
		///简介
		/// <summary>
		public string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}
		/// <summary>
		///类型
		/// <summary>
		public int Type
		{
			get { return _Type; }
			set { _Type = value; }
		}
		/// <summary>
		///
		/// <summary>
		public string images
		{
			get { return _images; }
			set { _images = value; }
		}
		/// <summary>
		///
		/// <summary>
		public string Url
		{
			get { return _Url; }
			set { _Url = value; }
		}        
    }
}