using System;
using System.Data;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.WebControl
{
    [ToolboxData("<{0}:CDropDownList runat=server></{0}:CDropDownList>")]
    sealed public class CDropDownList : DropDownList
    {
        [Description("下拉框类型")]
        public enum eType
        {
            [CAttribute(Name = "普通下拉框")]
            Normal,
            [Description("状态")]
            Status,
            [Description("性别")]
            Sex,
            [Description("民族")]
            Nation,
            [Description("星期")]
            Week,
            [Description("月份")]
            Months,
            [Description("备注类型")]
            NoteType,
            [Description("退单责任归属")]
            SingleResponsibility,
            [Description("权限范围")]
            AccScope,
            [Description("操作类型")]
            LogOperateType,
            [Description("优惠券一级类型")]
            CouponFirstType,
            [Description("优惠券发起类型")]
            CouponIniType,
            [Description("小时")]
            Hour,
            [Description("分钟")]
            Minutes,

        }

        private eType _SelType;
        [Browsable(true), Category("自定义属性"), Description("下拉框类型")]
        public eType SelType
        {
            get { return _SelType; }
            set { _SelType = value; }
        }

        private bool _IsSearch;
        [Browsable(true), Category("自定义属性"), Description("是否查询页面，默认否。")]
        public bool IsSearch
        {
            get { return _IsSearch; }
            set { _IsSearch = value; }
        }

        private bool _IsNotNull;
        [Browsable(true), Category("自定义属性"), Description("添加页面是否必填，默认是。")]
        public bool IsNotNull
        {
            get { return _IsNotNull; }
            set { _IsNotNull = value; }
        }

        public CDropDownList()
        {
            _SelType = eType.Normal;
            _IsNotNull = true;
            this.Init += new EventHandler(CDropDownList_Init);
        }

        /// <summary>
        /// 下拉框初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDropDownList_Init(object sender, EventArgs e)
        {
            ListItem[] items;
            string[] nation;
            switch (SelType)
            {
                case eType.Status:
                    this.Items.Add(new ListItem("有效", "1"));
                    this.Items.Add(new ListItem("无效", "0"));
                    break;

                case eType.Sex:
                    this.Items.Add(new ListItem("男", "1"));
                    this.Items.Add(new ListItem("女", "0"));
                    break;
                case eType.Nation:
                    this.Items.Add(new ListItem("汉", "1"));
                    this.Items.Add(new ListItem("其他", "0"));
                    break;
                //case eType.Months:
                //    this.Items.AddRange(Helper.GetEnumInfoByEnumType(typeof(SystemEnum.Months)));
                //    break;
                //case eType.Week:
                //    this.Items.AddRange(Helper.GetEnumInfoByEnumType(typeof(SystemEnum.Week)));
                //    break;
              
                //case eType.NoteType:
                //    this.Items.AddRange(Helper.GetEnumInfoByEnumType(typeof(SystemEnum.NoteType)));
                //    break;
                //case eType.SingleResponsibility:
                //    this.Items.AddRange(Helper.GetEnumInfoByEnumType(typeof(SystemEnum.SingleResponsibility)));
                //    break;
             
                //case eType.AccScope:
                //    this.Items.AddRange(Helper.GetEnumInfoByEnumType(typeof(SystemEnum.AccScope)));
                //    break;
             
                //case eType.LogOperateType:
                //    this.Items.AddRange(Helper.GetEnumInfoByEnumType(typeof(SystemEnum.LogOperateType)));
                //    break;
                //case eType.CouponFirstType:
                //    this.Items.AddRange(Helper.GetEnumInfoByEnumType(typeof(SystemEnum.CouponFirstType)));
                //    break;
            
                //case eType.CouponIniType:
                //    this.Items.AddRange(Helper.GetEnumInfoByEnumType(typeof(SystemEnum.CouponIniType)));
                //    break;
            
                //case eType.Hour:
                //    items = Helper.GetEnumInfoByEnumType(typeof(SystemEnum.Hour));
                //    if (items != null)
                //    {
                //        nation = items[0].ToString().Split(',');
                //        foreach (string str in nation)
                //        {
                //            this.Items.Add(new ListItem(str, str));
                //        }
                //    }
                //    this.SelectedIndex = 0;
                //    break;
                //case eType.Minutes:
                //    items = Helper.GetEnumInfoByEnumType(typeof(SystemEnum.Minutes));
                //    if (items != null)
                //    {
                //        nation = items[0].ToString().Split(',');
                //        foreach (string str in nation)
                //        {
                //            this.Items.Add(new ListItem(str, str));
                //        }
                //    }
                //    this.SelectedIndex = 0;
                //    break;
            }
            if (IsSearch)
            {
                this.Items.Insert(0, new ListItem("--全部--", ""));
            }
            if (!IsNotNull)
            {
                this.Items.Insert(0, new ListItem("--无--", ""));
            }
        }
    }
}
