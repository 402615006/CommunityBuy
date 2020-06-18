using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 系统设置业务类
    /// </summary>
    public class bllTM_SystemSettings : bllBase
    {
		DAL.dalTM_SystemSettings dal = new DAL.dalTM_SystemSettings();
        TM_SystemSettingsEntity Entity = new TM_SystemSettingsEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCname, string UCname, string TStatus, string AStatus, string IsLineUp, string SmallChangeType,string CCode,string UCode)
        {
            bool rel = false;
            try
            {
                Entity = new TM_SystemSettingsEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;

                Entity.TStatus = TStatus;
                Entity.KeyName = IsLineUp;
                Entity.DataValue = SmallChangeType;
                rel = true;
            }
            catch (System.Exception)
            {

            }
            return rel;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(string GUID, string UID,   string Id, string BusCode, string StoCode, string CCname, string UCname, string TStatus, string AStatus, string KeyName, string DataValue, string CCode, string UCode)
        {
            bool strReturn = CheckPageInfo("add",  Id, BusCode, StoCode, CCname, UCname, TStatus, AStatus, KeyName, DataValue, CCode,UCode);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
                return;
            }
            int result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result, Entity.Id.ToString());
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TM_SystemSettingsEntity UEntity)
        {
			
			//更新数据
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result, "");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string GUID, string UID, string Id)
        {

			string Mescode = string.Empty;
            int result = dal.Delete(Id, ref Mescode);
            //检测执行结果
            CheckResult(result, Mescode);
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
        public TM_SystemSettingsEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TM_SystemSettingsEntity();
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
            return new bllPaging().GetPagingInfo("TM_SystemSettings", "Id", "*,'' as StoName", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TM_SystemSettingsEntity SetEntityInfo(DataRow dr)
        {
            TM_SystemSettingsEntity Entity = new TM_SystemSettingsEntity();
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
            Entity.KeyName = dr["KeyName"].ToString();
            Entity.DataValue = dr["DataValue"].ToString();
            return Entity;
        }
    }
}