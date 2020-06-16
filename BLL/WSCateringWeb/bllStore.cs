using CommunityBuy.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 门店信息
    /// </summary>
    public class bllStore : bllBase
    {
        dalStore dal = new dalStore();

        /// <summary>
        /// 根据商户编号获取所有门店,缓存用,仅缓存stocode,cname
        /// </summary>
        /// <param name="BusCode">商户编号</param>
        /// <returns></returns>
        public DataTable GetBusCodeToAllStore(string BusCode)
        {
            return dal.GetBusCodeToAllStore(BusCode);
        }

        /// <summary>
        /// 根据条件获取门店信息，不分页
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public DataTable GetAllStore(string filter)
        {
            return dal.GetAllStore(filter);
        }

    }
}
