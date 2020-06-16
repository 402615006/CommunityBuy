using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 用户角色关系表业务类
    /// </summary>
    public class bllTB_UserRole : bllBase
    {
		DAL.dalTB_UserRole dal = new DAL.dalTB_UserRole();
        TB_UserRoleEntity Entity = new TB_UserRoleEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string Id, string BusCode, string StoCode, string StrRoleId, string UserId, string RealName, string EmpCode,string RoleDisCount)
        {
			string strRetuen = string.Empty;
            //要验证的实体属性
            List<string> EName = new List<string>() {  };
            //要验证的实体属性值
            List<string> EValue = new List<string>() {  };
            //错误信息
            List<string> errorCode = new List<string>();
            List<string> ControlName = new List<string>();
            //验证数据
            CheckValue<TB_UserRoleEntity>(EName, EValue, ref errorCode, new TB_UserRoleEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new TB_UserRoleEntity();
				Entity.Id = Helper.StringToLong(Id);
				Entity.BusCode = BusCode;
				Entity.StoCode = StoCode;
                Entity.RoleDisCount = RoleDisCount;
                Entity.StrRoleId = Helper.ReplaceString(StrRoleId);
				Entity.UserId = Helper.StringToLong(UserId);
				Entity.RealName = RealName;
				Entity.EmpCode = EmpCode;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID, out  string Id, string BusCode, string StoCode, string StrRoleId, string UserId, string RealName, string EmpCode,string sigJson,string RoleDisCount, operatelogEntity entity)
        {
			Id = "0";
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add",  Id, BusCode, StoCode, StrRoleId, UserId, RealName, EmpCode, RoleDisCount);
            if (!string.IsNullOrEmpty(sigJson))//套餐内菜品
            {
                Entity.UserSig = EntityHelper.GetEntityListByJson<TB_UserSigSchemeEntity>(sigJson);
            }
            //数据页面验证
            if (!CheckControl(strReturn))
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
        public DataTable Update(string GUID, string UID,  string Id, string BusCode, string StoCode, string RoleId, string UserId, string RealName, string EmpCode,string RoleDisCount, operatelogEntity entity)
        {
			
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update",  Id, BusCode, StoCode, RoleId, UserId, RealName, EmpCode, RoleDisCount);
            //数据页面验证
            if (!CheckControl(strReturn))
            {
                return dtBase;
            }
			//获取更新前的数据对象
            TB_UserRoleEntity OldEntity = new TB_UserRoleEntity();
            OldEntity = GetEntitySigInfo(" where Id='" + Id + "'");
			//更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<TB_UserRoleEntity>(entity, Entity, OldEntity);
                }
            }
            return dtBase;
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
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
        public DataTable Delete(string GUID, string UID, string Id, operatelogEntity entity)
        {
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
			string Mescode = string.Empty;
            int result = dal.Delete(Id, ref Mescode);
            //检测执行结果
            if (CheckDeleteResult(result,Mescode))
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
        public TB_UserRoleEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_UserRoleEntity();
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
            return new bllPaging().GetPagingInfo("TB_UserRole", "Id", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_UserRoleEntity SetEntityInfo(DataRow dr)
        {
            TB_UserRoleEntity Entity = new TB_UserRoleEntity();
			Entity.Id = Helper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			
			Entity.RoleId = Helper.StringToLong(dr["RoleId"].ToString());
			Entity.UserId = Helper.StringToLong(dr["UserId"].ToString());
			Entity.RealName = dr["RealName"].ToString();
			Entity.EmpCode = dr["EmpCode"].ToString();
            return Entity;
        }

        /// <summary>
        /// 登陆获取用户角色
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="TerminalType"></param>
        /// <returns></returns>
        public DataTable GetUserToRole(string UserId)
        {
            return dal.GetUserToRole(UserId);
        }

        public DataTable GetUserStoreRole(string userid)
        {
            return dal.GetUserStoreRoleList(userid);
        }




        /// <summary>
        /// 用户绑定角色用
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <param name="filter">门店条件</param>
        /// <returns></returns>
        public DataTable GetRoleListUser(string UserId,string filter)
        {
            return dal.GetRoleListUser(UserId, filter);
        }

        /// <summary>
        /// 获取用户角色名称
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataTable GetUserRoleNameList(string BusCode)
        {
            return dal.GetUserRoleNameList(BusCode);
        }

        /// <summary>
        /// 用户绑定角色用
        /// </summary>
        /// <param name="UserCode">用户id</param>
        /// <param name="BusCode">商户编号</param>
        /// <returns></returns>
        public DataTable GetUserSigList(string filter)
        {
            return dal.GetUserSigList(filter);
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <returns></returns>
        public DataTable GetRoleListUser(string UserId)
        {
            return dal.GetRoleListUser(UserId);
        }

        public DataTable GetRoleUserNameList(string stocode,string roletype)
        {
            return dal.GetRoleUserNameList(stocode, roletype);
        }

    }
}