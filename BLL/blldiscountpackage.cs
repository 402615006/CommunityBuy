using CommunityBuy.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 连锁端折扣方案
    /// </summary>
    public class blldiscountpackage : bllBase
    {
        daldiscountpackage dal = new daldiscountpackage();

        /// <summary>
        /// 根据门店获取折扣方案
        /// </summary>
        /// <param name="BusCode">商户编号</param>
        /// <param name="StoCode">门店编号</param>
        /// <returns></returns>
        public DataTable GetStoDisCountPackage(string BusCode, string StoCode)
        {
            return dal.GetStoDisCountPackage(BusCode, StoCode);
        }

    }
}
