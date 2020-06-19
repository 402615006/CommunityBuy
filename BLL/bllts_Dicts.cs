using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
    /// <summary>
    /// 系统字典信息业务类
    /// </summary>
    public class bllts_Dicts : bllBase
    {
        DAL.dalts_Dicts dal = new DAL.dalts_Dicts();
        ts_DictsEntity Entity = new ts_DictsEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string dicid, string buscode, string strcode, string dictype, string lng, string pdicid, string diccode, string dicname, string dicvalue, string orderno, string remark, string status, string cuser)
        {
            bool rel = false;
            try
            {
                Entity = new ts_DictsEntity();
                Entity.dicid = StringHelper.StringToLong(dicid);

                Entity.strcode = strcode;
                Entity.dictype = dictype;
                Entity.lng = lng;
                Entity.pdicid = StringHelper.StringToLong(pdicid);
                Entity.diccode = diccode;
                Entity.dicname = dicname;
                Entity.dicvalue = dicvalue;
                Entity.orderno = StringHelper.StringToInt(orderno);
                Entity.remark = remark;
                Entity.status = status;
                Entity.cuser = StringHelper.StringToLong(cuser);
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
        public void Add(string GUID, string UID, string dicid, string buscode, string strcode, string dictype, string lng, string pdicid, string diccode, string dicname, string dicvalue, string orderno, string remark, string status, string cuser)
        {
            bool strReturn = CheckPageInfo("add", dicid, buscode, strcode, dictype, lng, pdicid, diccode, dicname, dicvalue, orderno, remark, status, cuser);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
                return;
            }
            int result = dal.Add(ref Entity);
            dicid = Entity.dicid.ToString();
            //检测执行结果
            CheckResult(result, dicid);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, ts_DictsEntity UEntity)
        {
            //更新数据
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result, "");
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
            CheckResult(result,"");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string GUID, string UID, string dicid)
        {
            string Mescode = string.Empty;
            int result = dal.Delete(dicid, ref Mescode);
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
        public ts_DictsEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new ts_DictsEntity();
        }

        ///// <summary>
        ///// 分页方法
        ///// </summary>
        ///// <param name="pageSize"></param>
        ///// <param name="currentpage"></param>
        ///// <param name="filter"></param>
        ///// <param name="order"></param>
        ///// <param name="recnums"></param>
        ///// <returns></returns>
        //public DataTable GetPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        //{
        //    if (!CheckLogin(GUID, UID))//非法登录
        //    {
        //        recnums = -1;
        //        pagenums = -1;
        //        return dtBase;
        //    }
        //    return new bllPaging().GetPagingInfo("ts_Dicts", "dicid", "*,cusername=dbo.fnGetUserName(cuser)", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        //}

        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ts_DictsEntity SetEntityInfo(DataRow dr)
        {
            ts_DictsEntity Entity = new ts_DictsEntity();
            Entity.dicid = StringHelper.StringToLong(dr["dicid"].ToString());

            Entity.strcode = dr["strcode"].ToString();
            Entity.dictype = dr["dictype"].ToString();
            Entity.lng = dr["lng"].ToString();
            Entity.pdicid = StringHelper.StringToLong(dr["pdicid"].ToString());
            Entity.diccode = dr["diccode"].ToString();
            Entity.dicname = dr["dicname"].ToString();
            Entity.dicvalue = dr["dicvalue"].ToString();
            Entity.orderno = StringHelper.StringToInt(dr["orderno"].ToString());
            Entity.remark = dr["remark"].ToString();
            Entity.status = dr["status"].ToString();
            Entity.cuser = StringHelper.StringToLong(dr["cuser"].ToString());


            return Entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GUID">默认0</param>
        /// <param name="UID">默认0</param>
        /// <param name="pageSize">100000  全部数据 值设置最大</param>
        /// <param name="currentpage">1</param>
        /// <param name="filter">默认空</param>
        /// <param name="order">排序</param>
        /// <param name="recnums">返回记录数</param>
        /// <param name="pagenums">返回总页数</param>
        /// <returns></returns>
        public DataTable GetPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            return new bllPaging().GetPagingInfo("ts_Dicts", "dicid", "*,pname=dbo.fnGetPnameBypid(pdicid),uname=dbo.fnGetUserName(cuser)", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }
    }
}