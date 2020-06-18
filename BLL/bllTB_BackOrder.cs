using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 退单信息业务类
    /// </summary>
    public class bllTB_BackOrder : bllBase
    {
		DAL.dalTB_BackOrder dal = new DAL.dalTB_BackOrder();
        TB_BackOrderEntity Entity = new TB_BackOrderEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCode, string CCname, string AuthCode, string AuthName, string TStatus, string OrderCode, string OrderDisCode, string ReasonCode, string ReasonName, string Remar, string BackNum)
        {
            bool rel = false;
            try
            {
                Entity = new TB_BackOrderEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;
                Entity.AuthCode = AuthCode;
                Entity.AuthName = AuthName;
                Entity.TStatus = TStatus;
                Entity.OrderCode = OrderCode;
                Entity.OrderDisCode = OrderDisCode;
                Entity.ReasonCode = ReasonCode;
                Entity.ReasonName = ReasonName;
                Entity.Remar = Remar;
                Entity.BackNum = StringHelper.StringToDecimal(BackNum);
                rel = true; 
            }
            catch (Exception)
            { 
                
            }
            return rel;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(string GUID, string UID, string Id, string BusCode, string StoCode, string CCode, string CCname, string AuthCode, string AuthName, string TStatus, string OrderCode, string OrderDisCode, string ReasonCode, string ReasonName, string Remar, string BackNum)
        {
			Id = "0";
            //赋值到实体类
            CheckPageInfo("add", Id, BusCode, StoCode, CCode, CCname, AuthCode, AuthName, TStatus, OrderCode, OrderDisCode, ReasonCode, ReasonName, Remar, BackNum);
            int result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result, Entity.Id.ToString());
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_BackOrderEntity OldEntity)
        {
			//更新数据
            int result = dal.Update(OldEntity);
            //检测执行结果
            CheckResult(result,"");
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
        public TB_BackOrderEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_BackOrderEntity();
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
            return new bllPaging().GetPagingInfo("TB_BackOrder", "Id", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_BackOrderEntity SetEntityInfo(DataRow dr)
        {
            TB_BackOrderEntity Entity = new TB_BackOrderEntity();
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.AuthCode = dr["AuthCode"].ToString();
			Entity.AuthName = dr["AuthName"].ToString();
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.OrderCode = dr["OrderCode"].ToString();
			Entity.OrderDisCode = dr["OrderDisCode"].ToString();
			Entity.ReasonCode = dr["ReasonCode"].ToString();
			Entity.ReasonName = dr["ReasonName"].ToString();
			Entity.Remar = dr["Remar"].ToString();
			Entity.BackNum = StringHelper.StringToDecimal(dr["BackNum"].ToString());
            return Entity;
        }
    }
}