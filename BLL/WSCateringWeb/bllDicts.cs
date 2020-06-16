using CommunityBuy.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommunityBuy.BLL
{
   public class bllDicts : bllBase
    {
        dalDicts dal = new dalDicts();

        /// <summary>
        /// 根据商户编号获取所有员工,缓存用,仅缓存dicname
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetBusCodeToDicts(string BusCode)
        {
            return dal.GetBusCodeToDicts(BusCode);
        }

    }
}
