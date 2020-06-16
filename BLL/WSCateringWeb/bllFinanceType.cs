using CommunityBuy.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 财务类别
    /// </summary>
    public class bllFinanceType : bllBase
    {
        dalFinanceType dal = new dalFinanceType();

        /// <summary>
        /// 根据商户编号获取所有财务类别,缓存用,仅缓存fincode,finname
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetBusCodeToAllFin(string BusCode)
        {
            return dal.GetBusCodeToAllFin(BusCode);
        }
    }
}
