using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.Model
{
    [Description("门店后台用户信息")]
    public class sto_adminsEntity
    {
        private long _userid = 0;
        private string _buscode = Helper.GetAppSettings("BusCode");
        private string _stocode = string.Empty;
        private string _strcode = string.Empty;
        private string _username = string.Empty;
        private string _upwd = string.Empty;
        private string _realname = string.Empty;
        private string _umobile = string.Empty;
        private string _empcode = string.Empty;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private string _rolecode = string.Empty;
        //固定字段
        private string _cuser = string.Empty;
        private DateTime _ctime = DateTime.Parse("1900-01-01");
        private string _uuser = string.Empty;
        private DateTime _utime = DateTime.Parse("1900-01-01");

        private string _Chopenshift = string.Empty;
        #region //扩展字段 tsg


        private int _AccBillPrintNum = 1;
        /// <summary>
        /// 结账单打印张数
        /// </summary>
        public int AccBillPrintNum
        {
            get
            {
                return _AccBillPrintNum > 0 ? _AccBillPrintNum : 1;
            }
            set { _AccBillPrintNum = value > 0 ? value : 1; }
        }


        private decimal _maxdiffPrice = 0;
        public string Chopenshift {
            get { return _Chopenshift; }
            set { _Chopenshift = value; }
        }
        /// <summary>
        ///可换菜最大差额
        /// <summary>
        public decimal maxdiffPrice
        {
            get { return _maxdiffPrice; }
            set { _maxdiffPrice = value; }
        }

        private decimal _prefeprice = 0;
        /// <summary>
        ///最大优免金额
        /// <summary>
        public decimal prefeprice
        {
            get { return _prefeprice; }
            set { _prefeprice = value; }
        }

        private string _GUID = string.Empty;
        /// <summary>
        ///令牌
        /// <summary>
        public string GUID
        {
            get { return _GUID; }
            set { _GUID = value; }
        }

        private string _roleids = string.Empty;
       

        private string _roletypes = string.Empty;
       

        private string _rolenames = string.Empty;

      

        private string _cname = string.Empty;//员工姓名
        /// <summary>
        ///员工姓名
        /// <summary>
        public string cname
        {
            get
            {
                string strCName = _cname;
                if (string.IsNullOrEmpty(strCName))
                {
                    strCName = _realname;
                }
                if (string.IsNullOrEmpty(strCName))
                {
                    strCName = _username;
                }
                return strCName;
            }
            set
            {
                _cname = value;

            }
        }

        private long _shiftid = 0;//开班编号
        public long shiftid
        {
            get { return _shiftid; }
            set { _shiftid = value; }
        }
        private string _Mac = string.Empty;//客户端Mac地址
        /// <summary>
        /// 客户端Mac地址
        /// </summary>
        public string Mac
        {
            get { return _Mac; }
            set { _Mac = value; }
        }

        private string _ClientIP = string.Empty;//客户端IP地址
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIP
        {
            get { return _ClientIP; }
            set { _ClientIP = value; }
        }

        private string _ClientName = string.Empty;//客户端客户端机器名称
        /// <summary>
        /// 客户端机器名称
        /// </summary>
        public string ClientName
        {
            get { return _ClientName; }
            set { _ClientName = value; }
        }


        private string _TerminalID = string.Empty;//终端ID
        /// <summary>
        /// 终端ID(客户端机mac对应的记录id）
        /// </summary>
        public string TerminalID
        {
            get { return _TerminalID; }
            set { _TerminalID = value; }
        }


        private DateTime _LoginTime = System.DateTime.Now;//登录时间
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime
        {
            get { return _LoginTime; }
            set { _LoginTime = value; }
        }

        private string _strname = string.Empty;
        /// <summary>
        /// 门店名称
        /// </summary>
        public string strname
        {
            get { return _strname; }
            set { _strname = value; }
        }

        private string _strtel = string.Empty;
        /// <summary>
        /// 门店电话
        /// </summary>
        public string strtel
        {
            get { return _strtel; }
            set { _strtel = value; }
        }

       

        private int _HavePaddle = 0;
        /// <summary>
        /// 是否有划菜端(0没有，1有)
        /// </summary>
        public int HavePaddle
        {
            get { return _HavePaddle; }
            set { _HavePaddle = value; }
        }


        private string _reason = string.Empty;
        /// <summary>
        /// 原因（授权操作需要）
        /// </summary>
        public string reason
        {
            get { return _reason; }
            set { _reason = value; }
        }


        private string _reasontype = string.Empty;
        /// <summary>
        /// 原因类别（授权操作需要）
        /// </summary>
        public string reasontype
        {
            get { return _reasontype; }
            set { _reasontype = value; }
        }

        private string _IsTest = "0";
        /// <summary>
        /// 当前系统是否作为测试使用(打印结账单带测试字样)
        /// </summary>
        public string IsTest
        {
            get { return _IsTest; }
            set { _IsTest = value; }
        }


        private string _KeCunCode = string.Empty;
        /// <summary>
        /// 客存库编号
        /// </summary>
        public string KeCunCode
        {
            get { return _KeCunCode; }
            set { _KeCunCode = value; }
        }



        private string _HuiJiuCode = string.Empty;
        /// <summary>
        /// 回酒库编号
        /// </summary>
        public string HuiJiuCode
        {
            get { return _HuiJiuCode; }
            set { _HuiJiuCode = value; }
        }


        private string _XiaoShouCode = string.Empty;
        /// <summary>
        /// 销售库编号
        /// </summary>
        public string XiaoShouCode
        {
            get { return _XiaoShouCode; }
            set { _XiaoShouCode = value; }
        }





        private CommunityBuy.CommonBasic.SystemEnum.SysTerminalType _TerminalType = SystemEnum.SysTerminalType.中餐端;
        /// <summary>
        /// 终端类型（1中餐端、2快餐端、3美食端、4刷卡端、5超市端）
        /// </summary>
        public CommunityBuy.CommonBasic.SystemEnum.SysTerminalType TerminalType
        {
            get { return _TerminalType; }
            set { _TerminalType = value; }
        }

        /// <summary>
        /// 终端类型值（1中餐端、2快餐端、3美食端、4刷卡端、5超市端）
        /// </summary>
        public string TerminalTypeValue
        {
            get { return ((int)_TerminalType).ToString(); }
        }

        /// <summary>
        /// 终端类型文本（1中餐端、2快餐端、3美食端、4刷卡端、5超市端）
        /// </summary>
        public string TerminalTypeText
        {
            get { return _TerminalType.ToString(); }
        }

        private string _IsFood = "否";
        /// <summary>
        /// 是否美食城
        /// </summary>
        public string IsFood
        {
            get { return _IsFood; }
            set { _IsFood = value; }
        }

        private string _PrintName = string.Empty;
        /// <summary>
        /// 本地默认打印机名称
        /// </summary>
        public string PrintName
        {
            get { return _PrintName; }
            set { _PrintName = value; }
        }


        private string _IsKitchen = "1";
        /// <summary>
        /// 是否出品打印
        /// </summary>
        public string IsKitchen
        {
            get { return _IsKitchen; }
            set { _IsKitchen = value; }
        }

        private string _IsPrintTableBakcReport = "1";
        /// <summary>
        /// 是否打印桌台退菜单
        /// </summary>
        public string IsPrintTableBakcReport
        {
            get { return _IsPrintTableBakcReport; }
            set { _IsPrintTableBakcReport = value; }
        }

        private bool _IsOrderBillPrint = true;
        /// <summary>
        /// 是否下单小票打印
        /// </summary>
        public bool IsOrderBillPrint
        {
            get { return _IsOrderBillPrint; }
            set { _IsOrderBillPrint = value; }
        }


        private bool _IsCustBillPrint = true;
        /// <summary>
        /// 是否顾客小票打印
        /// </summary>
        public bool IsCustBillPrint
        {
            get { return _IsCustBillPrint; }
            set { _IsCustBillPrint = value; }
        }



        private bool _IsAccBillPrint = true;
        /// <summary>
        /// 是否结账单打印
        /// </summary>
        public bool IsAccBillPrint
        {
            get { return _IsAccBillPrint; }
            set { _IsAccBillPrint = value; }
        }


        private bool _IsPrintBalance = true;
        /// <summary>
        /// 是否打印余额小票
        /// </summary>
        public bool IsPrintBalance
        {
            get { return _IsPrintBalance; }
            set { _IsPrintBalance = value; }
        }

        


        private string _DepartCode = string.Empty;
        /// <summary>
        /// 部门编号
        /// </summary>
        public string DepartCode
        {
            get { return _DepartCode; }
            set { _DepartCode = value; }
        }


        private string _DepartName = string.Empty;
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName
        {
            get { return _DepartName; }
            set { _DepartName = value; }
        }



        private string _PayOnlineServiceUrl = string.Empty;
        /// <summary>
        /// 在线支付访问地址
        /// </summary>
        public string PayOnlineServiceUrl
        {
            get { return _PayOnlineServiceUrl; }
            set { _PayOnlineServiceUrl = value; }
        }

        #endregion

        #region 基本属性
        public string stocode
        {
            get { return _stocode; }
            set { _stocode = value; }
        }
        public string rolecode
        {
            get { return _rolecode; }
            set { _rolecode = value; }
        }
        /// <summary>
        ///用户标识
        /// <summary>
        public long userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        ///商户编号
        /// <summary>
        [ModelInfo(Name = "商户编号", ControlName = "txt_buscode", NotEmpty = true, Length = 16, NotEmptyECode = "admins_001", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "admins_002")]
        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }

        }
        /// <summary>
        ///门店编号
        /// <summary>
        [ModelInfo(Name = "门店编号", ControlName = "txt_strcode", NotEmpty = true, Length = 16, NotEmptyECode = "admins_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "admins_005")]
        public string strcode
        {
            get { return _strcode; }
            set
            {
                _strcode = value;
                _stocode = value;
            }
        }
        /// <summary>
        ///用户名
        /// <summary>
        [ModelInfo(Name = "用户名", ControlName = "txt_uname", NotEmpty = true, Length = 16, NotEmptyECode = "admins_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "admins_008")]
        public string uname
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        ///密码
        /// <summary>
        [ModelInfo(Name = "密码", ControlName = "txt_upwd", NotEmpty = true, Length = 32, NotEmptyECode = "admins_010", RType = RegularExpressions.RegExpType.Password, RTypeECode = "admins_011")]
        public string upwd
        {
            get { return _upwd; }
            set { _upwd = value; }
        }
        /// <summary>
        ///姓名
        /// <summary>
        [ModelInfo(Name = "姓名", ControlName = "txt_realname", NotEmpty = false, Length = 32, NotEmptyECode = "admins_013", RType = RegularExpressions.RegExpType.Normal)]
        public string realname
        {
            get
            {
                return _realname;
            }
            set
            {
                _realname = value;

            }
        }
        /// <summary>
        ///手机号
        /// <summary>
        [ModelInfo(Name = "手机号", ControlName = "txt_umobile", NotEmpty = false, Length = 32, NotEmptyECode = "admins_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "admins_017")]
        public string umobile
        {
            get { return _umobile; }
            set { _umobile = value; }
        }
        /// <summary>
        ///员工编号
        /// <summary>
        [ModelInfo(Name = "员工编号", ControlName = "txt_empcode", NotEmpty = false, Length = 32, NotEmptyECode = "admins_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "admins_020")]
        public string empcode
        {
            get { return _empcode; }
            set { _empcode = value; }
        }
        /// <summary>
        ///备注
        /// <summary>
        [ModelInfo(Name = "备注", ControlName = "txt_remark", NotEmpty = false, Length = 128, NotEmptyECode = "admins_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "admins_023")]
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        ///状态
        /// <summary>
        [ModelInfo(Name = "状态", ControlName = "ddl_status", NotEmpty = true, Length = 1, NotEmptyECode = "admins_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "admins_026")]
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        ///创建人
        /// <summary>
        public string cuser
        {
            get { return _cuser; }
            set { _cuser = value; }
        }
        /// <summary>
        ///创建时间
        /// <summary>
        public DateTime ctime
        {
            get { return _ctime; }
            set { _ctime = value; }
        }
        /// <summary>
        ///修改人
        /// <summary>
        public string uuser
        {
            get { return _uuser; }
            set { _uuser = value; }
        }
        /// <summary>
        ///修改时间
        /// <summary>
        public DateTime utime
        {
            get { return _utime; }
            set { _utime = value; }
        }

        #endregion

        #region method
        /// <summary>
        /// 判断模块是否在权限内
        /// </summary>
        /// <param name="code"></param>
        /// <param name="btnname"></param>
        /// <returns></returns>
        public bool InFunctions(string code)
        {
            bool blnResult = false;
            if (_Functions != null && _Functions.Count > 0)
            {
                for (int i = 0; i < _Functions.Count; i++)
                {
                    if (_Functions[i].code.ToLower().Trim() == code.Trim().ToLower())
                    {
                        blnResult = true;
                        break;
                    }
                }
            }
            return blnResult;
        }


        #endregion

        #region 按键权限属性
        public List<rol_Button> Buttonlist = new List<rol_Button>();

        private DataTable _dtFunctions = new DataTable();
        /// <summary>
        /// 权限按钮
        /// </summary>
        public DataTable dtFunctions
        {
            get { return _dtFunctions; }
            set { _dtFunctions = value; }
        }
        #endregion

        private List<sto_functionsEntity> _Functions = new List<sto_functionsEntity>();//模块权限集
        /// <summary>
        /// 模块权限集
        /// </summary>
        public List<sto_functionsEntity> Functions
        {
            get { return _Functions; }
            set { _Functions = value; }
        }

    }


    public class rol_Button
    {
        private string fromName;

        public string FromName
        {
            get { return fromName; }
            set { fromName = value; }
        }



        private string buttonName;

        public string ButtonName
        {
            get { return buttonName; }
            set { buttonName = value; }
        }
    }
}
