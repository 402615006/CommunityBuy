using CommunityBuy.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 员工信息
    /// </summary>
    public class bllEmployee : bllBase
    {
        dalEmployee dal = new dalEmployee();

        /// <summary>
        /// 根据商户编号获取所有员工,缓存用,仅缓存stocode,dcode,ecode,cname
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetBusCodeToAllEmp(string BusCode)
        {
            return dal.GetBusCodeToAllEmp(BusCode);
        }

        /// <summary>
        /// 获取所有用户全部信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllEmp()
        {
            return dal.GetAllEmp();
        }

        /// <summary>
        /// 获取提成人
        /// </summary>
        /// <returns></returns>
        public DataTable GetCustomerManager(int page,int pagesize,string filter,string order,out int recnums,out int pagenums)
        {
            return dal.GetPageEmp("admins", "id", "realname,empcode,PY", pagesize, page, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 门店后台获取指定门店的系统用户-分页方法
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <returns></returns>
        public DataTable GetEmpStoList(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            return dal.GetPageEmp("Admins t inner join [dbo].[Employee] e on e.ecode=t.empcode", "t.userid", "t.*,(case t.Status when '0' then '无效' else '有效' end) StatusName,e.mob,rolename=dbo.f_GetRoleName(t.userid),roleid=dbo.f_GetRoleID(t.userid),empcodename=dbo.fnGetEmployeeCname(t.empcode),empmob=[dbo].[fnGetEmployeeMobile](t.empcode),storename=dbo.fnGetMuStoreName1(t.userid,t.scope,t.stocode),empstoname=dbo.fnGetEmployeeStoreName(t.empcode),e.strcode as empstocode,dbo.[f_GetSigStoCode](t.userid) as sigstocodes,rolecode=dbo.GetRoleStocodeByUserName(t.userid),'' as RoleDisCountName,'' as RoleDisCount,sigstorename=dbo.[f_GetSigStoName](t.userid)", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        public DataTable GetFinList(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            return dal.GetPageEmp("FinanceType ", "lsid", "*,(case [Status] when '0' then '无效' else '有效' end) StatusName", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="BusCode"></param>
        /// <returns></returns>
        public DataTable GetAllAdmin()
        {
            return dal.GetAllAdmin();
        }

    }
}
