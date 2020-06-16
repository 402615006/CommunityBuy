using System;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    [Description("后台功能信息表")]
    public class sto_functionsEntity
    {
		private int _id = 0;
		private int _ftype = 0;
		private int _parentid = 0;
		private string _code = string.Empty;
		private string _cname = string.Empty;
		private string _btnname = string.Empty;
		private int _orders = 0;
		private string _imgname = string.Empty;
		private string _url = string.Empty;
		private string _status = string.Empty;
		private int _level = 0;
		private string _descr = string.Empty;

		/// <summary>
		///标识
		/// <summary>
		public int id
		{
			get { return _id; }
			set { _id = value; }
		}
		/// <summary>
		///类型
		/// <summary>
		[ModelInfo(Name = "类型",ControlName="txt_ftype", NotEmpty = false, Length = 1, NotEmptyECode = "sto_functions_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_005")]
		public int ftype
		{
			get { return _ftype; }
			set { _ftype = value; }
		}
		/// <summary>
		///父ID
		/// <summary>
		[ModelInfo(Name = "父ID",ControlName="txt_parentid", NotEmpty = false, Length = 4, NotEmptyECode = "sto_functions_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_008")]
		public int parentid
		{
			get { return _parentid; }
			set { _parentid = value; }
		}
		/// <summary>
		///功能编号
		/// <summary>
		[ModelInfo(Name = "功能编号",ControlName="txt_code", NotEmpty = false, Length = 64, NotEmptyECode = "sto_functions_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_011")]
		public string code
		{
			get { return _code; }
			set { _code = value; }
		}
		/// <summary>
		///功能名称
		/// <summary>
		[ModelInfo(Name = "功能名称",ControlName="txt_cname", NotEmpty = false, Length = 64, NotEmptyECode = "sto_functions_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_014")]
		public string cname
		{
			get { return _cname; }
			set { _cname = value; }
		}
		/// <summary>
		///按钮编号
		/// <summary>
		[ModelInfo(Name = "按钮编号",ControlName="txt_btnname", NotEmpty = false, Length = 32, NotEmptyECode = "sto_functions_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_017")]
		public string btnname
		{
			get { return _btnname; }
			set { _btnname = value; }
		}
		/// <summary>
		///排序号
		/// <summary>
		[ModelInfo(Name = "排序号",ControlName="txt_orders", NotEmpty = false, Length = 4, NotEmptyECode = "sto_functions_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_020")]
		public int orders
		{
			get { return _orders; }
			set { _orders = value; }
		}
		/// <summary>
		///图片名称
		/// <summary>
		[ModelInfo(Name = "图片名称",ControlName="txt_imgname", NotEmpty = false, Length = 64, NotEmptyECode = "sto_functions_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_023")]
		public string imgname
		{
			get { return _imgname; }
			set { _imgname = value; }
		}
		/// <summary>
		///图片路径
		/// <summary>
		[ModelInfo(Name = "图片路径",ControlName="txt_url", NotEmpty = false, Length = 128, NotEmptyECode = "sto_functions_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_026")]
		public string url
		{
			get { return _url; }
			set { _url = value; }
		}
		/// <summary>
		///状态
		/// <summary>
		[ModelInfo(Name = "状态",ControlName="txt_status", NotEmpty = false, Length = 1, NotEmptyECode = "sto_functions_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_029")]
		public string status
		{
			get { return _status; }
			set { _status = value; }
		}
		/// <summary>
		///层级
		/// <summary>
		[ModelInfo(Name = "层级",ControlName="txt_level", NotEmpty = false, Length = 1, NotEmptyECode = "sto_functions_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_032")]
		public int level
		{
			get { return _level; }
			set { _level = value; }
		}
		/// <summary>
		///备注
		/// <summary>
		[ModelInfo(Name = "备注",ControlName="txt_descr", NotEmpty = false, Length = 64, NotEmptyECode = "sto_functions_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "sto_functions_035")]
		public string descr
		{
			get { return _descr; }
			set { _descr = value; }
		}        
    }
}