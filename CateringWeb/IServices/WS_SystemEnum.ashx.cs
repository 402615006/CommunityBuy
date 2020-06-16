using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_SystemEnum 系统枚举 的摘要说明
    /// </summary>
    public class WS_SystemEnum : ServiceBase
    {

        DataTable dt = new DataTable();
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    switch (actionname.ToLower())
                    {
                        case "getenumlist"://枚举列表
                            GetEnumListByCode(dicPar);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取枚举列表信息
        /// </summary>
        /// <param name="dicPar">枚举Code</param>
        private void GetEnumListByCode(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "enumcode", "lng" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string enumcode = dicPar["enumcode"].ToString();
            string lng = dicPar["lng"].ToString();
            DataTable dt = new DataTable();
            switch (enumcode)
            {
                case "LogOperateType"://日志操作类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.LogOperateType));
                    break;
                case "Status"://状态
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.Status));
                    break;
                case "MarriageTypes"://婚姻类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.MarriageTypes));
                    break;
                case "Sex"://性别
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.Sex));
                    break;
                case "Nation"://民族
                    dt = Helper.GetDataTableEnumInfoByEnumType2(typeof(SystemEnum.Nation));
                    break;
                case "BackupCycle"://备份周期
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.BackupCycle));
                    break;
                case "SendType"://优惠券赠送方案 赠送类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.SendType));
                    break;
                case "CardPayMethod"://会员卡付款类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.CardPayMethod));
                    break;
                case "PayWay"://会员卡充值途径
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.PayWay));
                    break;
                case "MemCardStatus"://会员卡状态
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.MemCardStatus));
                    break;
                case "AffiliatedType"://附属卡类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.AffiliatedType));
                    break;
                case "DisposedStatus"://处理状态
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DisposedStatus));
                    break;
                case "LostStatus"://招领状态
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.LostStatus));
                    break;
                case "Top"://置顶状态
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.Top));
                    break;
                case "LostType"://失物招领类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.LostType));
                    break;
                case "ActionType"://活动类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.ActionType));
                    break;
                case "InventoryCycle"://盘点周期
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.InventoryCycle));
                    break;
                case "AuditStatus"://审核状态
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.AuditStatus));
                    break;
                case "PriceWay"://加价方式
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.PriceWay));
                    break;
                case "CombiningScheme"://组合方案
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.CombiningScheme));
                    break;
                case "DiscountPackage"://折扣方案
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DiscountPackage));
                    break;
                case "Week"://星期
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.Week));
                    break;
                case "FinancialSubject"://财务科目类别
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.FinancialSubject));
                    break;
                case "TablesDetailType"://桌台明细类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.TablesDetailType));
                    break;
                case "TableStatus"://桌台管理使用状态
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.TableStatus));
                    break;
                case "SurchargeCalculationMethod"://附加费计算方法
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.SurchargeCalculationMethod));
                    break;
                case "NoteType"://备注类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.NoteType));
                    break;
                case "SingleResponsibility"://退单责任归属
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.SingleResponsibility));
                    break;
                case "BookingSource"://预定方式
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.BookingSource));
                    break;
                case "InOutType"://出入库类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.InOutType));
                    break;
                case "AccScope"://权限范围
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.AccScope));
                    break;
                case "CouponFirstType"://优惠券一级类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.CouponFirstType));
                    break;
                case "CouponIniType"://优惠券发起类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.CouponIniType));
                    break;
                case "DisheOperareType"://点菜操作类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DisheOperareType));
                    break;
                case "Hour"://小时
                    dt = Helper.GetDataTableEnumInfoByEnumType2(typeof(SystemEnum.Hour));
                    break;
                case "Minutes"://分钟
                    dt = Helper.GetDataTableEnumInfoByEnumType2(typeof(SystemEnum.Minutes));
                    break;
                case "Frontmodify"://修改前台
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.Frontmodify));
                    break;
                case "Autocalc"://自动计算
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.Autocalc));
                    break;
                case "Payfunction"://支付方式修改 反结算 开钱箱
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.Payfunction));
                    break;
                case "eOutofstype": //沽清类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.eOutofstype));
                    break;
                case "DefaultOrderMethod": //点餐方式
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DefaultOrderMethod));
                    break;
                case "SaveAndTakeWincOptionType"://存取酒操作类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.SaveAndTakeWincOptionType));
                    break;
                case "OverdueRecoveryDetailOptionType"://存取酒操作类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.OverdueRecoveryDetailOptionType));
                    break;
                case "SaveWinceType"://存取酒类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.SaveWinceType));
                    break;
                case "SysTerminalType"://终端类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.SysTerminalType));
                    break;
                case "RolesType"://角色类型
                    dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.RolesType));
                    break;
                case "eTableStatus":
                     dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.eTableStatus));
                    break;
                case "DisChannel":
                     dt = Helper.GetDataTableEnumInfoByEnumType(typeof(SystemEnum.DisChannel));
                    break;
            }
            //调用逻辑
            ReturnListJson(dt);
        }
    }
}