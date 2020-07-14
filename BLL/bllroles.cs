using System.Data;
using System.Text;
using CommunityBuy.CommonBasic;
using CommunityBuy.DAL;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
    public class bllroles : bllBase
    {
        dalroles _dal = new dalroles();
        DataTable dt = new DataTable();
        public bllroles()
        {
            dt = new DataTable("error");
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("mes", typeof(string));
            dt.Columns.Add("spanids", typeof(string));
            dt.AcceptChanges();
        }
        public int AddRITMAS(rolesEntity Entity, string[] FunList)
        {
            return _dal.AddRITMAS(Entity, FunList);
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="filter">指定条件</param>
        /// <returns>返回第一行</returns>
        public DataTable GetPagingInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingInfo(10, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetPagingListInfo(string GUID, string UserID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {

            return new bllPaging().GetPagingInfo("roles", "roleid", "*,storename=dbo.fnGetMuStoreName(stocode),[dbo].[fnGetBusinessNameByCode](buscode) as BusName", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string ID)
        {
            string Mescode = string.Empty;
            int result = _dal.Delete(ID, ref Mescode);
            //检测执行结果
            CheckResult(result, Mescode);
        }

        public void DeleteRoles(string selected, ref string mes)
        {
            _dal.DeleteRoles(selected, ref mes);
        }

        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="pageSize">页面记录数</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="filter">条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="recnums">总记录数</param>
        /// <returns>返回数据表</returns>
        public DataTable GetPagingInfo(int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            return new bllPaging().GetPagingInfo("roles", "roleid", "*,storename=dbo.fnGetMuStoreName(stocode)", pageSize, currentpage, filter, string.Empty, order, out recnums, out pagenums);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public void UpdateStatus(string GUID, string UID, string id, string Status)
        {

            dt.Clear();
            DataRow dr = dt.NewRow();
            int result = _dal.UpdateStatus(id, Status);
            CheckResult(result, "");
        }

        public DataSet GetDetail(string roleid)
        {
            DataSet ds = new bllPaging().GetDataSetInfoBySQL("select *,storename=dbo.fnGetMuStoreName(stocode),[dbo].[fnGetBusinessNameByCode](buscode) as BusName from roles where roleid='" + roleid + "';select B.id,parentid as pId,cname as name,'true'as 'open',ishave=(case when funid>0 then 1 else 0 end) from functions B left join (select funid from rolefunction where roleid='" + roleid + "') A on A.funid=B.id where B.status='1';");
            return ds;
        }

        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private rolesEntity SetEntityInfo(DataRow dr)
        {
            rolesEntity Entity = new rolesEntity();
            Entity.buscode = dr["buscode"].ToString();
            Entity.cname = dr["cname"].ToString();
            Entity.ctime = StringHelper.StringToDateTime(dr["ctime"].ToString());
            Entity.cuser = StringHelper.StringToInt(dr["cuser"].ToString());
            Entity.descr = dr["descr"].ToString();
            Entity.roleid = StringHelper.StringToInt(dr["roleid"].ToString());
            Entity.scope = dr["scope"].ToString();
            Entity.status = dr["status"].ToString();
            Entity.stocode = dr["stocode"].ToString();
            Entity.utime = StringHelper.StringToDateTime(dr["utime"].ToString());
            Entity.uuser = StringHelper.StringToInt(dr["uuser"].ToString());
            return Entity;
        }

        /// <summary>
        /// 获取单条数据实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public rolesEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new rolesEntity();
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, rolesEntity UEntity,string FunList)
        {
            int result = _dal.Update(UEntity,FunList);
            //检测执行结果
            CheckResult(result, "");
        }

    }
}
