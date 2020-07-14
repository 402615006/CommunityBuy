using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
    /// <summary>
    /// 商家门店信息业务类
    /// </summary>
    public class bllStore : bllBase
    {
        DAL.dalStore dal = new DAL.dalStore();
        StoreEntity Entity = new StoreEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string stoid, string stocode, string cname, string sname, string bcode, string indcode, string provinceid, string cityid, string areaid, string address, string stoprincipal, string stoprincipaltel, string tel,string logo, string backgroundimg, string stopath, string services, string descr, string stourl, string stocoordx, string stocoordy,string recommended, string remark, string status, string cuser,string btime, string etime,string sqid,string jprice)
        {
            bool rel = false;
            try
            {
                Entity = new StoreEntity();
                Entity.stoid = StringHelper.StringToLong(stoid);
                Entity.stocode = stocode;
                Entity.cname = cname;
                Entity.sname = sname;
                Entity.bcode = bcode;
                Entity.indcode = indcode;
                Entity.provinceid = StringHelper.StringToInt(provinceid);
                Entity.cityid = StringHelper.StringToInt(cityid);
                Entity.areaid = StringHelper.StringToInt(areaid);
                Entity.address = address;
                Entity.stoprincipal = stoprincipal;
                Entity.stoprincipaltel = stoprincipaltel;
                Entity.tel = tel;
                Entity.logo = logo;
                Entity.backgroundimg = backgroundimg;
                Entity.stopath = stopath;
                Entity.services = services;
                Entity.descr = descr;
                Entity.stourl = stourl;
                Entity.stocoordx = stocoordx;
                Entity.stocoordy = stocoordy;
                Entity.recommended = recommended;
                Entity.remark = remark;
                Entity.status = status;
                Entity.cuser = StringHelper.StringToLong(cuser);
                Entity.btime = btime;
                Entity.etime = etime;
                Entity.sqcode = StringHelper.StringToInt(sqid);
                Entity.jprice = StringHelper.StringToDecimal(jprice);
                rel = true;
            }
            catch (Exception)
            {
            }
            return rel;
        }

        public void Add(string GUID, string UID, string stoid, string stocode, string cname, string sname, string bcode, string indcode, string provinceid, string cityid, string areaid, string address, string stoprincipal, string stoprincipaltel, string tel, string logo, string backgroundimg, string stopath, string services, string descr, string stourl, string stocoordx, string stocoordy, string recommended, string remark, string status, string cuser, string btime, string etime,string sqid,string jprice )
        {
            string spanids = string.Empty;
            bool strReturn = CheckPageInfo("add", stoid, stocode, cname, sname, bcode, indcode, provinceid, cityid, areaid, address, stoprincipal, stoprincipaltel, tel, logo, backgroundimg, stopath, services, descr, stourl, stocoordx, stocoordy, recommended, remark, status, cuser, btime, etime, sqid, jprice);
            //数据页面验证
            int result = dal.Add(ref Entity);
            stoid = Entity.stoid.ToString();
            //检测执行结果
            CheckResult(result, stoid);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
         public void Update(string GUID, string UID, StoreEntity UEntity)
        {
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result, "");
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="stoid">标识</param>
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
        public void Delete(string GUID, string UID, string stoid)
        {

            string Mescode = string.Empty;
            int result = dal.Delete(stoid, ref Mescode);
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
        public StoreEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new StoreEntity();
        }

        public DataTable GetPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            return new bllPaging().GetPagingInfo("Store", "stoid", "*,areaname =dbo.fnGetAreaFullName(provinceid,cityid,areaid)", pageSize, currentpage, filter, "", order, out recnums, out pagenums); //,
        }


        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private StoreEntity SetEntityInfo(DataRow dr)
        {
            StoreEntity Entity = new StoreEntity();
            Entity.stoid = StringHelper.StringToLong(dr["stoid"].ToString());
            Entity.comcode = dr["comcode"].ToString();
            Entity.stocode = dr["stocode"].ToString();
            Entity.cname = dr["cname"].ToString();
            Entity.sname = dr["sname"].ToString();
            Entity.bcode = dr["bcode"].ToString();
            Entity.indcode = dr["indcode"].ToString();
            Entity.provinceid = StringHelper.StringToInt(dr["provinceid"].ToString());
            Entity.cityid = StringHelper.StringToInt(dr["cityid"].ToString());
            Entity.areaid = StringHelper.StringToInt(dr["areaid"].ToString());
            Entity.address = dr["address"].ToString();
            Entity.stoprincipal = dr["stoprincipal"].ToString();
            Entity.stoprincipaltel = dr["stoprincipaltel"].ToString();
            Entity.tel = dr["tel"].ToString();
            Entity.stoemail = dr["stoemail"].ToString();
            Entity.logo = dr["logo"].ToString();
            Entity.backgroundimg = dr["backgroundimg"].ToString();
            //Entity.services = dr["services"].ToString();
            Entity.descr = dr["descr"].ToString();
            Entity.stourl = dr["stourl"].ToString();
            Entity.stocoordx = dr["stocoordx"].ToString();
            Entity.stocoordy = dr["stocoordy"].ToString();
            Entity.recommended = dr["recommended"].ToString();
            Entity.remark = dr["remark"].ToString();
            Entity.status = dr["status"].ToString();
            Entity.cuser = StringHelper.StringToLong(dr["cuser"].ToString());
            return Entity;
        }

        /// <summary>
        /// 获取门店信息
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="buscode">商户编号</param>
        /// <param name="comcode">分公司编号</param>
        /// <returns></returns>
        public DataTable GetListInfo(string GUID, string UID, string buscode)
        {
            string where = " where 1=1";
            if (buscode != null && buscode.Length > 0)
            {
                where += " and buscode='" + buscode + "'";
            }
            return new bllPaging().GetDataTableInfoBySQL("select * from Store " + where);
        }
    }
}