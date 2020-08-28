using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    public class membersEntityList
    {
        public string status = "0";
        public string mes = string.Empty;
        public List<membersEntity> data = new List<membersEntity>();
    }

    [Description("会员信息表")]
    public class membersEntity : BaseModel
    {
        private string _memid = string.Empty;
        private string _memcode = string.Empty;

        private string _wxaccount = string.Empty;
        private string _mobile = string.Empty;
        private string _remark = string.Empty;
        private string _status = string.Empty;
        private string _loginpwd = string.Empty;
        private string _paypwd = string.Empty;

        /// <summary>
        ///会员标识
        /// <summary>
        public string memid
        {
            get { return _memid; }
            set { _memid = value; }
        }
        /// <summary>
        ///会员编号
        /// <summary>
        public string memcode
        {
            get { return _memcode; }
            set { _memcode = value; }
        }
        /// <summary>
        ///微信账户
        /// <summary>
        public string wxaccount
        {
            get { return _wxaccount; }
            set { _wxaccount = value; }
        }


        /// <summary>
        ///手机号码
        /// <summary>
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }
        /// <summary>
        ///备注
        /// <summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        ///状态
        /// <summary>
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        ///登录密码
        /// <summary>
        public string loginpwd
        {
            get { return _loginpwd; }
            set { _loginpwd = value; }
        }

        /// <summary>
        ///支付密码
        /// <summary>
        public string paypwd
        {
            get { return _paypwd; }
            set { _paypwd = value; }
        }

    }
}