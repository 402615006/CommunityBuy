using System.Collections.Generic;
using System.Data;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 门店角色信息业务类
    /// </summary>
    public class bllTB_Roles : bllBase
    {
        DAL.dalTB_Roles dal = new DAL.dalTB_Roles();
        TB_RolesEntity Entity = new TB_RolesEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCname, string TStatus, string UCname, string SCope, string RoleName, string RoleParent, string RoleDescr, string RoleDisCount, string RoleEnable, string MaxDiffPrice, string MaxPrefePrice, string IsSig, string Sigcredit, string RoleType, string TerminalType, string CCode, string UCode)
        {
            bool rel = false;
            try
            {
                Entity = new TB_RolesEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;
                Entity.TStatus = TStatus;
                Entity.RoleName = RoleName;
                Entity.RoleParent = StringHelper.StringToInt(RoleParent);
                Entity.RoleDescr = RoleDescr;
                Entity.RoleEnable = RoleEnable;
                Entity.RoleType = RoleType;
                rel = true;
            }
            catch (System.Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(string GUID, string UID,  string Id, string BusCode, string StoCode, string CCname, string TStatus, string UCname, string SCope, string RoleName, string RoleParent, string RoleDescr, string RoleDisCount, string RoleEnable, string MaxDiffPrice, string MaxPrefePrice, string IsSig, string Sigcredit, string RoleType, string TerminalType, string CCode, string UCode, string FunctionIds)
        {
            int result = 0;
           string spanids = string.Empty;
            bool strReturn = CheckPageInfo("add", Id, BusCode, StoCode, CCname, TStatus, UCname, SCope, RoleName, RoleParent, RoleDescr, RoleDisCount, RoleEnable, MaxDiffPrice, MaxPrefePrice, IsSig, Sigcredit, RoleType, TerminalType, CCode, UCode);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
                return;
            }
            string[] FunList = null;
            if (!string.IsNullOrEmpty(FunctionIds))
            {
                FunList = FunctionIds.Split(',');
            }
            result = dal.Add(ref Entity, FunList);
            //检测执行结果
            CheckResult(result,Entity.Id.ToString());
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_RolesEntity UEntity,string FunctionIds)
        {
            //获取更新前的数据对象
            string[] FunList = null;
            if (!string.IsNullOrEmpty(FunctionIds))
            {
                FunList = FunctionIds.Split(',');
            }
            //更新数据
            int result = dal.Update(UEntity, FunList);
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
        public TB_RolesEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_RolesEntity();
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
            return new bllPaging().GetPagingInfo("roles", "roleid", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }


        /// <summary>
        /// 获取按钮权限
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public DataTable GetbtnRoleData(string GUID, string UID)
        {
            return new bllPaging().GetDataTableInfoBySQL("SELECT distinct A.Cname AS btnname,a.BtnCode as code FROM dbo.TB_Functions  A left join TB_RoleFunction B on A.id=B.functionid WHERE A.[tstatus]='1' and A.Ftype='2' and A.Level='4' AND b.RoleId in (select tb_user.RoleId from TB_UserRole tb_user left join TB_Roles tb_role on tb_user.RoleId=tb_role.Id where tb_user.UserId='" + UID + "')");
        }

        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_RolesEntity SetEntityInfo(DataRow dr)
        {
            TB_RolesEntity Entity = new TB_RolesEntity();
            Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
            Entity.BusCode = dr["BusCode"].ToString();
            Entity.StoCode = dr["StoCode"].ToString();
            Entity.CCode = dr["CCode"].ToString();
            Entity.CCname = dr["CCname"].ToString();

            Entity.TStatus = dr["TStatus"].ToString();
            Entity.RoleName = dr["RoleName"].ToString();
            Entity.RoleParent = StringHelper.StringToInt(dr["RoleParent"].ToString());
            Entity.RoleDescr = dr["RoleDescr"].ToString();
            Entity.RoleEnable = dr["RoleEnable"].ToString();
            Entity.RoleType = dr["RoleType"].ToString();
            return Entity;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="dicid">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public void UpdateStatus(string GUID, string UID, string ids, string Status)
        {
            int result = dal.UpdateStatus(ids, Status);
            //检测执行结果
            CheckResult(result, "");
        }
    }
}