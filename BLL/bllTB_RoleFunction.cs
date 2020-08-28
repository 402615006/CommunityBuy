using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 门店权限管理业务类
    /// </summary>
    public class bllTB_RoleFunction : bllBase
    {
		DAL.dalTB_RoleFunction dal = new DAL.dalTB_RoleFunction();
        TB_RoleFunctionEntity Entity = new TB_RoleFunctionEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCname, string RoleId, string FunctionId,string CCode)
        {
            bool rel = false;
            try
            {
                Entity = new TB_RoleFunctionEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;

                Entity.RoleId = StringHelper.StringToLong(RoleId);
                Entity.FunctionId = StringHelper.StringToLong(FunctionId);
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
        public void Add(string GUID, string UID, out  string Id, string BusCode, string StoCode, string CCname, string RoleId, string FunctionId,string CCode)
        {
			Id = "0";
            int result = 0;
            bool strReturn = CheckPageInfo("add",  Id, BusCode, StoCode, CCname, RoleId, FunctionId, CCode);
            //数据页面验证
            result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result,"");
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_RoleFunctionEntity UEntity)
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
            string Mescode = "";
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
        public TB_RoleFunctionEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_RoleFunctionEntity();
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

            return new bllPaging().GetPagingInfo("rolefunction", "roleid", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_RoleFunctionEntity SetEntityInfo(DataRow dr)
        {
            TB_RoleFunctionEntity Entity = new TB_RoleFunctionEntity();
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.RoleId = StringHelper.StringToLong(dr["RoleId"].ToString());
			Entity.FunctionId = StringHelper.StringToLong(dr["FunctionId"].ToString());
            return Entity;
        }

        public DataTable GetRoleFunctionInfoList(string GUID, string UID, string roleid)
        {
            return new bllPaging().GetDataTableInfoBySQL("SELECT A.id,A.level ,A.parentid AS pId,A.Cname AS name,(CASE WHEN B.funid IS NULL THEN 0 ELSE 1 END) as ishave,'true' as [open],B.roleid,R.cname as rolename,R.descr as roledescr,A.descr,A.status,B.funid FROM functions  A left join rolefunction B on A.id=B.funid AND B.roleid=" + roleid + " right join roles R on B.roleid=R.roleid  WHERE A.[status]='1' ORDER BY A.[level] ASC,A.parentid ASC,A.orders ASC");
        }

        public DataTable GetAllFunctions()
        { 
            return new bllPaging().GetDataTableInfoBySQL("SELECT * from functions");
        }

    }
}