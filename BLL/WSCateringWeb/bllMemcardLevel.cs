using CommunityBuy.DAL;
using CommunityBuy.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BLL
{
    public class bllMemcardLevel : bllBase
    {
        dalMemcardLevel dal = new dalMemcardLevel();

        /// <summary>
        /// 根据商户编号获取所有员工,缓存用,仅缓存dicname
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public memcardtypeEntityList GetMemcardLevel(string BusCode)
        {
            //调用逻辑
            memcardtypeEntityList list = new memcardtypeEntityList();
            list.status = "0";
            DataSet ds = dal.GetMemcardLevel(BusCode);
            if (ds != null && ds.Tables.Count == 2)
            {
                DataTable dtMemcardType = ds.Tables[0];
                DataTable dtLevel = ds.Tables[1];
                for (int i = 0; i < dtMemcardType.Rows.Count; i++)
                {
                    memcardtypeEntity MethodType = EntityHelper.GetEntityByDR<memcardtypeEntity>(dtMemcardType.Rows[i], null);
                    DataRow[] rowsMethod = dtLevel.Select("mctcode='" + MethodType.mctcode + "'");

                    if (rowsMethod.Length > 0)
                    {
                        MethodType.levels = EntityHelper.GetEntityListByDR<memcardlevelEntity>(rowsMethod, null);
                    }
                    list.data.Add(MethodType);
                }
                
            }
            return list;
        }

        /// <summary>
        /// 根据条件获取会员卡信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetMemcard(string where)
        {
            return dal.GetMemcard(where);
        }

    }
}
