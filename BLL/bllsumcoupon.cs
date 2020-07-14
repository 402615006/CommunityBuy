using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
    /// <summary>
    /// 优惠券活动业务类
    /// </summary>
    public class bllsumcoupon : bllBase
    {
        DAL.dalsumcouponN dal = new DAL.dalsumcouponN();
        sumcouponNEntity Entity = new sumcouponNEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string sumid, string sumcode, string buscode, string stocode, string cname, string ctype, string status, string descr, string audcode, string audcname, string audremark, string audstatus, string ccode, string ccname)
        {
            bool rel = false;
            try
            {
                Entity = new sumcouponNEntity();
                Entity.sumid = StringHelper.StringToLong(sumid);
                Entity.sumcode = sumcode;
                Entity.buscode = buscode;
                Entity.stocode = stocode;
                Entity.cname = cname;
                Entity.ctype = ctype;
                Entity.status = status;
                Entity.descr = descr;
                Entity.audcode = audcode;
                Entity.audcname = audcname;
                Entity.audremark = audremark;
                Entity.audstatus = audstatus;
                Entity.ccode = ccode;
                Entity.ccname = ccname;
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
        public void Add(string GUID, string UID,   string sumid, string sumcode, string buscode, string stocode, string cname, string ctype, string status, string descr, string audcode, string audcname, string audremark, string audstatus, string ccode, string ccname)
        {
            bool strReturn = CheckPageInfo("add",  sumid,  sumcode,  buscode,  stocode,  cname,  ctype,  status,  descr,  audcode,  audcname,  audremark,  audstatus,  ccode,  ccname);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
                return;
            }
            int result = dal.Add(ref Entity);
            sumid = Entity.sumid.ToString();
            //检测执行结果
            CheckResult(result,sumid);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID,sumcouponNEntity UEntity)
        {
            //更新数据
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result, "");
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sumid">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public void UpdateStatus(string GUID, string UID, string ids, string Status)
        {
            int result = dal.UpdateStatus(ids, Status);
            //检测执行结果
            CheckResult(result,"");
        }

        /// <summary>
        /// 作废未使用优惠券
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void UpdateStatusNotSend(string GUID, string UID, string ids)
        {
            int result = dal.UpdateStatusNotSend(ids);
            //检测执行结果
            CheckResult(result,"");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string GUID, string UID, string sumid)
        {
            string Mescode = string.Empty;
            int result = dal.Delete(sumid, ref Mescode);
            //检测执行结果
            CheckResult(result, "");
        }

        /// <summary>
        /// 优惠活动审核
        /// </summary>
        /// <param name="sumid">标识</param>
        /// <returns></returns>
        public void Audit(string GUID, string UID, string sumid, string AudChar, string Audreason, string audcode, string audcname)
        {
            string Mescode = string.Empty;
            int result = dal.Audit(sumid, AudChar, Audreason, audcode, audcname, ref Mescode);
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
        public sumcouponNEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new sumcouponNEntity();
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
            return new bllPaging().GetPagingInfo("N_sumcoupon", "sumid", "*,stoname=dbo.fnGetStoreNameByCodes(stocode,','),busname=dbo.f_GetBusinessSname(buscode)", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private sumcouponNEntity SetEntityInfo(DataRow dr)
        {
            sumcouponNEntity Entity = new sumcouponNEntity();
            Entity.sumid = StringHelper.StringToLong(dr["sumid"].ToString());
            Entity.sumcode = dr["sumcode"].ToString();
            Entity.buscode = dr["buscode"].ToString();
            Entity.stocode = dr["stocode"].ToString();
            Entity.cname = dr["cname"].ToString();
            Entity.ctype = dr["ctype"].ToString();
            Entity.status = dr["status"].ToString();
            Entity.descr = dr["descr"].ToString();
            Entity.audcode = dr["audcode"].ToString();
            Entity.audcname = dr["audcname"].ToString();
            Entity.audremark = dr["audremark"].ToString();
            Entity.audstatus = dr["audstatus"].ToString();

            Entity.ccode = dr["ccode"].ToString();
            Entity.ccname = dr["ccname"].ToString();

            return Entity;
        }
    }
}