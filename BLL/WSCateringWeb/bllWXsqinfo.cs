using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.DAL;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
    /// <summary>
    /// 业务类
    /// </summary>
    public class bllWXsqinfo : bllBase
    {
        dalWXsqinfo dal = new dalWXsqinfo();
        WXsqinfoEntity Entity = new WXsqinfoEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string id, string sqcode, string sqname, string city, string jwcodes, string cuser, string status, string uuser)
        {
            string strRetuen = string.Empty;
            //要验证的实体属性
            List<string> EName = new List<string>() { };
            //要验证的实体属性值
            List<string> EValue = new List<string>() { };
            //错误信息
            List<string> errorCode = new List<string>();
            List<string> ControlName = new List<string>();
            //验证数据
            CheckValue<WXsqinfoEntity>(EName, EValue, ref errorCode, new WXsqinfoEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new WXsqinfoEntity();
                Entity.id = Helper.StringToLong(id);
                Entity.sqcode = sqcode;
                Entity.sqname = sqname;
                Entity.city = city;
                Entity.jwcodes = jwcodes;

                Entity.cuser = Helper.StringToLong(cuser);
                Entity.status = status;

                Entity.uuser = Helper.StringToLong(uuser);

            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID, out string id, string sqcode, string sqname, string city, string jwcodes, string cuser, string status, string uuser, operatelogEntity entity)
        {
            id = "0";
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add", id, sqcode, sqname, city, jwcodes, cuser, status, uuser);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
            int result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public DataTable Update(string GUID, string UID, string id, string sqcode, string sqname, string city, string jwcodes, string cuser, string status, string uuser, operatelogEntity entity)
        {

            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update", id, sqcode, sqname, city, jwcodes, cuser, status, uuser);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
            //获取更新前的数据对象
            WXsqinfoEntity OldEntity = new WXsqinfoEntity();
            OldEntity = GetEntitySigInfo(" where id='" + id + "'");
            //更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<WXsqinfoEntity>(entity, Entity, OldEntity);
                }
            }
            return dtBase;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataTable UpdateStatus(string GUID, string UID, string ids, string Status)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateStatus(ids, Status);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public DataTable Delete(string GUID, string UID, string id, operatelogEntity entity)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string Mescode = string.Empty;
            int result = dal.Delete(id, ref Mescode);
            //检测执行结果
            if (CheckDeleteResult(result, Mescode))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add(entity);
                }

            }
            return dtBase;
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="filter">指定条件</param>
        /// <returns>返回第一行</returns>
        public DataTable GetPagingSigInfo(string GUID, string UID, string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            return GetPagingListInfo(GUID, UID, 1, 1, filter, string.Empty, out recnums, out pagenums);
        }

        /// <summary>
        /// 获取单条数据实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public WXsqinfoEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new WXsqinfoEntity();
        }

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <returns></returns>
        public DataTable GetPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                recnums = -1;
                pagenums = -1;
                return dtBase;
            }
            return new bllPaging().GetLSPagingInfo("sqinfo", "id", "*,cusername=dbo.fnGetUserName(cuser),uusername=dbo.fnGetUserName(uuser)", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private WXsqinfoEntity SetEntityInfo(DataRow dr)
        {
            WXsqinfoEntity Entity = new WXsqinfoEntity();
            Entity.id = Helper.StringToLong(dr["id"].ToString());
            Entity.sqcode = dr["sqcode"].ToString();
            Entity.sqname = dr["sqname"].ToString();
            Entity.city = dr["city"].ToString();
            Entity.jwcodes = dr["jwcodes"].ToString();

            Entity.cuser = Helper.StringToLong(dr["cuser"].ToString());
            Entity.status = dr["status"].ToString();

            Entity.uuser = Helper.StringToLong(dr["uuser"].ToString());

            return Entity;
        }

        /// <summary>
        /// 获取商圈
        /// </summary>
        /// <returns></returns>
        public DataTable GetGPSList()
        {
            return dal.GetGPSList();
        }

        /// <summary>
        /// 获取商圈
        /// </summary>
        /// <returns></returns>
        public DataTable GetNewGPSList(string typecode)
        {
            return dal.GetNewGPSList(typecode);
        }

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <returns></returns>
        public DataTable GetSQStoList(int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            //return new bllPaging().GetPagingInfo("Store s inner join storegx gx on s.stocode=gx.stocode", "s.stoid", "gx.jprice,s.stocode,s.cname,'0' as isyuding,'0' as ispaidui,", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
            if (!string.IsNullOrEmpty(filter))
            {
                filter += " and isnull(gx.sqcode,'')<>'' and dic.pdicid in (select dicid from ts_Dicts where diccode='HYTypeFir')";
            }
            else
            {
                filter = " where isnull(gx.sqcode,'')<>'' and dic.pdicid in (select dicid from ts_Dicts where diccode='HYTypeFir')";
            }
            return new bllPaging().GetLSPagingInfo("Store s inner join storegx gx on s.stocode=gx.stocode left join ts_Dicts dic on gx.firtype=dic.diccode", "s.stoid", "'' as isdc,gx.jprice,s.stocode,s.cname,dic.dicname,'0' as isyuding,'0' as ispaidui,s.logo,s.stocoordx,s.stocoordy", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        public DataTable GetNewSQStoList(int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums, string x, string y)
        {
            //return new bllPaging().GetPagingInfo("Store s inner join storegx gx on s.stocode=gx.stocode", "s.stoid", "gx.jprice,s.stocode,s.cname,'0' as isyuding,'0' as ispaidui,", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
            if (filter.Contains("gx.sqcode"))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and isnull(gx.sqcode,'')<>'' and dic.pdicid in (select dicid from ts_Dicts where diccode='HYTypeFir')";
                }
                else
                {
                    filter = " where isnull(gx.sqcode,'')<>'' and dic.pdicid in (select dicid from ts_Dicts where diccode='HYTypeFir')";
                }
            }
            else
            {
                filter += " and dic.pdicid in (select dicid from ts_Dicts where diccode='HYTypeFir') and s.stocode not in('LZKJ','24','WXXL')";
            }
            return new bllPaging().GetLSPagingInfo("Store s inner join storegx gx on s.stocode=gx.stocode left join ts_Dicts dic on gx.firtype=dic.diccode", "s.stoid", "'' as isdc,gx.jprice,s.stocode,s.cname,dic.dicname,'0' as isyuding,'0' as ispaidui,s.logo,s.stocoordx,s.stocoordy", pageSize, currentpage, filter, "", "dbo.fnGetDistance(" + x + "," + y + ",s.stocoordx,s.stocoordy)", out recnums, out pagenums);
        }

        /// <summary>
        /// 获取首页轮播图
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="modelcode"></param>
        /// <returns></returns>
        public DataTable GetUnImage(string GUID, string UID, string modelcode)
        {
            return dal.GetUnImage(modelcode);
        }

        /// <summary>
        /// 门店预定信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPresetSettings()
        {
            return dal.GetPresetSettings();
        }

        /// <summary>
        /// 门店是否启用排队
        /// </summary>
        /// <returns></returns>
        public DataTable GetQueuingSettings()
        {
            return dal.GetQueuingSettings();
        }

        /// <summary>
        /// 获取门店的排队人数段
        /// </summary>
        /// <param name="StoCode"></param>
        /// <returns></returns>
        public DataTable GetQueuing(string StoCode)
        {
            return dal.GetQueuing(StoCode);
        }

        /// <summary>
        /// 获取指定门店的预定时间段
        /// </summary>
        /// <param name="StoCode"></param>
        /// <returns></returns>
        public DataTable GetReservationRemark(string StoCode)
        {
            return dal.GetReservationRemark(StoCode);
        }

        /// <summary>
        /// 获取排队信息
        /// </summary>
        /// <param name="StoCode"></param>
        /// <returns></returns>
        public DataTable GetQueue(string StoCode, string WxId)
        {
            return dal.GetQueue(StoCode, WxId);
        }

    }
}