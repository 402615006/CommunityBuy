using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 系统设置业务类
    /// </summary>
    public class bllts_sysset : bllBase
    {
		DAL.dalts_sysset dal = new DAL.dalts_sysset();
        ts_syssetEntity Entity = new ts_syssetEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string setid, string stocode, string buscode, string key, string val, string status, string descr, string explain)
        {
            bool rel = false;
            try
            {
                Entity = new ts_syssetEntity();
                Entity.setid = StringHelper.StringToInt(setid);
                Entity.stocode = stocode;

                Entity.key = key;
                Entity.val = val;
                Entity.status = StringHelper.StringToInt(status);
                Entity.descr = descr;
                Entity.explain = explain;
                rel = true;
            }
            catch (System.Exception)
            {

                throw;
            }
            return rel;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(string GUID, string UID,  string setid, string stocode, string buscode, string key, string val, string status, string descr, string explain)
        {
            bool strReturn = CheckPageInfo("add", setid, stocode, buscode, key, val, status, descr, explain);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
            }
            int result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result, Entity.setid.ToString());
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, ts_syssetEntity UEntity)
        {
			//更新数据
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result, "");
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="setid">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public void UpdateStatus(string GUID, string UID, string ids, string Status)
        {
            int result = dal.UpdateStatus(ids, Status);
            //检测执行结果
			CheckResult(result,"");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string GUID, string UID, string setid)
        {
			string Mescode = string.Empty;
            int result = dal.Delete(setid, ref Mescode);
            //检测执行结果
            CheckResult(result, "");
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
        public ts_syssetEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new ts_syssetEntity();
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

            return new bllPaging().GetPagingInfo("ts_sysset", "setid", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ts_syssetEntity SetEntityInfo(DataRow dr)
        {
            ts_syssetEntity Entity = new ts_syssetEntity();
			Entity.setid = StringHelper.StringToInt(dr["setid"].ToString());
			Entity.stocode = dr["stocode"].ToString();
            //Entity.buscode = dr["buscode"].ToString();
			Entity.key = dr["key"].ToString();
			Entity.val = dr["val"].ToString();
			Entity.status = StringHelper.StringToInt(dr["status"].ToString());
			Entity.descr = dr["descr"].ToString();
			
            return Entity;
        }
    }
}