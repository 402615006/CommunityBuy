using System;
using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.DAL;
using CommunityBuy.Model;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 后台用户信息业务类
    /// </summary>
    public class bllAdmins : bllBase
    {
        dalAdmins dal = new dalAdmins();
        AdminsEntity Entity = new AdminsEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        //public string CheckPageInfo(string type, string userid, string uname, string upwd, string realname, string umobile, string empcode, string remark, string status, string cname, string ccode, string scope, string stocode, string sigmonthmoney, string sigstocode,string buscode, string utype, out string spansid)
            public bool CheckPageInfo(string type, string userid, string uname, string upwd, string realname, string umobile, string remark, string status, string cname, string ccode)
        {
            bool rel = false;
            try
            {
                Entity = new AdminsEntity();
                Entity.userid = StringHelper.StringToInt(userid);
                Entity.uname = uname;
                Entity.upwd = OEncryp.Encrypt(upwd);
                Entity.realname = realname;
                Entity.umobile = umobile;

                Entity.remark = remark;
                Entity.status = status;

                Entity.cname = cname;
                Entity.ccode = ccode;
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
        //public DataTable Add(string GUID, string UID, out string userid, string uname, string upwd, string realname, string umobile, string empcode, string remark, string status, string cname, string ccode, string role, string scope, string stocode, string sigmonthmoney, string sigstocode,string buscode, string utype, operatelogEntity entity)
             public void Add(string GUID, string UID, string userid, string uname, string upwd, string realname, string umobile,  string remark, string status, string cname, string ccode, string role)
        {

            //string strReturn = CheckPageInfo("add", userid, uname, upwd, realname, umobile, empcode, remark, status, cname, ccode, scope, stocode, sigmonthmoney, sigstocode,buscode,  utype, out spanids);
            bool strReturn = CheckPageInfo("add", userid, uname, upwd, realname, umobile, remark, status, cname, ccode);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
                return;
            }
            int result = dal.Add(ref Entity, role);
            userid = Entity.userid.ToString();
            //检测执行结果
            CheckResult(result, userid);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public DataTable Update(string GUID, string UID, string userid, string uname, string upwd, string realname, string umobile, string empcode, string remark, string status, string cname, string ccode, string role, string scope, string stocode, string sigmonthmoney, string sigstocode,string buscode,string utype, operatelogEntity entity)
             public void Update(string GUID, string UID,AdminsEntity UEntity,string role)
        {

            //更新数据
            int result = dal.Update(UEntity, role);
            //检测执行结果
            CheckResult(result, "");
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public void UpdateStatus(string GUID, string UID, string ids, string Status)
        {
            int result = dal.UpdateStatus(ids, Status);
            //检测执行结果
            CheckResult(result, "");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string GUID, string UID, string ID)
        {

            string Mescode = string.Empty;
            int result = dal.Delete(ID, ref Mescode);
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
        public AdminsEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new AdminsEntity();
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

            return new bllPaging().GetPagingInfo("Admins", "userid", "*,rolename=dbo.f_GetRoleName(userid),roleid=dbo.f_GetRoleID(userid)", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
           
        }



        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private AdminsEntity SetEntityInfo(DataRow dr)
        {
            AdminsEntity Entity = new AdminsEntity();
            Entity.userid =StringHelper.StringToLong(dr["userid"].ToString());
            Entity.strcode = dr["strcode"].ToString();
            Entity.uname = dr["uname"].ToString();
            Entity.upwd = dr["upwd"].ToString();
            Entity.realname = dr["realname"].ToString();
            Entity.umobile = dr["umobile"].ToString();

            Entity.remark = dr["remark"].ToString();
            Entity.status = dr["status"].ToString();

            return Entity;
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public void ResetPwd(string GUID, string UID, string id, string Pwd)
        {
            int result = dal.ResetPwd(id, Pwd);
            //检测执行结果
            CheckResult(result,"");
        }

        /// <summary>
        /// 获取登录后用户菜单信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetPermissionInfo(string GUID, string UID, string userid)
        {
            return new bllPaging().GetDataTableInfoBySQL("select id,parentid,[level],code,cname,imgname,url,orders from functions where status='1' and [level]<=3 and id in(select funid from rolefunction where roleid in(select roleid from userrole where userid='" + userid + "')) order by parentid,[level],orders");
        }
       
        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="USER_ID"></param>
        /// <param name="userid"></param>
        /// <param name="pagecode"></param>
        /// <returns></returns>
        public DataTable GetUserRole(string GUID, string UID, string userid, string pagecode)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM dbo.sto_functions WHERE ftype=1 and status='1' and id in(select funid from sto_rolefunction where  roleid in(SELECT roleid FROM dbo.sto_userrole WHERE userid='" + userid + "')) order by orders asc";
            dt = new bllPaging().GetDataTableInfoBySQL(sql);
            DataTable butdt = dt.Clone();
            if (dt != null && dt.Rows.Count > 0)
            {
                //获取一级菜单
                DataRow[] drs = dt.Select(" code='" + pagecode + "'");
                if (drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        DataRow dr = butdt.NewRow();
                        dr.ItemArray = drs[i].ItemArray;
                        butdt.Rows.Add(dr);
                    }
                }
            }
            return butdt;
        }


        public DataTable GetFunctionsButtonByPageCode(string GUID, string UID, string PageCode)
        {
            return new bllPaging().GetDataTableInfoBySQL("select code,cname,btnname,orders,imgname,url from functions where [status]='1' and code='" + PageCode + "' and [level]=4 and id in(select funid from rolefunction where roleid in (select roleid from userrole where userid='" + UID + "'))");
        }

    }
}