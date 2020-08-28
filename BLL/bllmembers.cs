using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
    /// <summary>
    /// 会员信息表业务类
    /// </summary>
    public class bllmembers : bllBase
    {
        DAL.dalmembers dal = new DAL.dalmembers();
        membersEntity Entity = new membersEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string memid, string memcode, string wxaccount, string mobile,  string remark, string status)
        {
            bool rel = false;

            try
            {
                Entity = new membersEntity();
                Entity.memid = memid;
                Entity.memcode = memcode;
                Entity.wxaccount = wxaccount;
                Entity.mobile = mobile;
                Entity.remark = remark;
                Entity.status = status;
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
        public void Add(string GUID, string UID,  string memid, string memcode, string wxaccount,string mobile,  string remark, string status)
        {

            //memcode = new bllTrans().GetCustomSerialnumber(SystemEnum.SystemCustomCode.members.ToString());
            bool strReturn = CheckPageInfo("add",  memid,  memcode,  wxaccount, mobile,  remark,  status);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
                return;
            }
            string MesCode = string.Empty;
            int result = dal.Add(ref Entity, ref MesCode);
            memid = Entity.memid.ToString();
            CheckResult(result, memid);
            switch (result)
            {
                case 2:
                    this.oResult.Msg = "用户已经注册过了";
                    break;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, membersEntity UEntity)
        {
            string MesCode = string.Empty;
            int result = dal.Update(UEntity, ref MesCode);
            CheckResult(result, MesCode);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="memcode">标识</param>
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
        public void Delete(string GUID, string UID, string memcode)
        {
            string Mescode = string.Empty;
            int result = dal.Delete(memcode, ref Mescode);
            //检测执行结果
            CheckResult(result, Mescode);
        }

        /// <summary>
        /// 获取我的推广会员信息
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataTable GetMyMemberInfo(string GUID, string UID, string memcode)
        {
            return new bllPaging().GetDataTableInfoBySQL("select nickname,ctime,headimgurl,[dbo].[fnGetMemEarn](memcode,'1,2') as memearn,dbo.fnGetMemEarnLastM(memcode,'1,2') as memearnM from WX_members_wx where memcode='" + memcode + "'");
        }

        /// <summary>
        /// 获取我的推广佣金
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataTable MYEarn(string GUID, string UID, string memcode)
        {

            return new bllPaging().GetDataTableInfoBySQL("select dbo.fnGetMemEarn(memcode,'1,2') as earn,dbo.fnGetMemEarnToday(memcode,'1') as todaygood,dbo.fnGetMemEarnToday(memcode,'2') as todaymem,dbo.fnGetMemEarnLastM(memcode,'1,2') as earnLM,dbo.fnGetMemEarnM(memcode,'1,2') as earnM,'每月20号结算上月预估收入，建议21号再进行提现。' as note from WX_members_wx where memcode='" + memcode + "'");
        }

        /// <summary>
        /// 获取我的推广佣金明细
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataTable MYEarnDetail(string GUID, string UID, string memcode, string date)
        {
            string filter = "";
            if (!string.IsNullOrWhiteSpace(date))
            {
                filter = " and convert(varchar(7),ctime,120)='" + date + "'";
            }
            return new bllPaging().GetDataTableInfoBySQL("select ctype,income,balance,traremark,remark,ctime from TM_MembersCommission where memcode='" + memcode + "' " + filter + " order by ctime desc");
        }

        /// <summary>
        /// 获取我的提现页面详情
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataSet MYCashOut(string GUID, string UID, string memcode)
        {
            return new bllPaging().GetDataSetInfoBySQL("select dbo.fnGetMemEarnValid('" + memcode + "') as amount,'结算说明2' as jsnote,(select mobile from wx_members_wx where memcode='" + memcode + "') as wxaccount;select bmoney,emoney,ctype,val from TB_ExtChargeSet");
        }

        /// <summary>
        /// 获取我的提现页面详情
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataTable MYCashOutList(string GUID, string UID, string memcode)
        {
            return new bllPaging().GetDataTableInfoBySQL("select '佣金提现' as cname,ctime,extmoney,chamoney,remark,( case when estatus='0' then '待处理' when estatus='1' then '已到账' when estatus='2' then '失败' else '' end) as status from TM_MembersComExtRec where memcode='" + memcode + "' order by ctime desc");
        }

        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public int ApplyCashOut(string GUID, string UID, string memcode, float money, float fee, ref string Mescode)
        {
            int result = dal.ApplyCashOut(memcode, money, fee, ref Mescode);
            return result;
        }

        /// <summary>
        /// 获取我的推广佣金
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataTable MYEarnReport(string GUID, string UID, string memcode, string type)
        {

            if (type == "0")
            {
                return new bllPaging().GetDataTableInfoBySQL("select dbo.fnGetMemEarn('" + memcode + "','1,2,3,4') as earn,dbo.fnGetMemEarnToday('" + memcode + "','1') as goodpt,dbo.fnGetMemEarnToday('" + memcode + "','2') as mempt,dbo.fnGetMemEarnToday('" + memcode + "','3') as goodzs,dbo.fnGetMemEarnToday('" + memcode + "','4') as memzs");
            }
            else if (type == "1")
            {
                return new bllPaging().GetDataTableInfoBySQL("select dbo.fnGetMemEarnLastDay('" + memcode + "','1,2,3,4') as earn,dbo.fnGetMemEarnLastDay('" + memcode + "','1') as goodpt,dbo.fnGetMemEarnLastDay('" + memcode + "','2') as mempt,dbo.fnGetMemEarnLastDay('" + memcode + "','3') as goodzs,dbo.fnGetMemEarnLastDay('" + memcode + "','4') as memzs");
            }
            else if (type == "7")
            {
                return new bllPaging().GetDataTableInfoBySQL("select dbo.fnGetMemEarn7day('" + memcode + "','1,2,3,4') as earn,dbo.fnGetMemEarn7day('" + memcode + "','1') as goodpt,dbo.fnGetMemEarn7day('" + memcode + "','2') as mempt,dbo.fnGetMemEarn7day('" + memcode + "','3') as goodzs,dbo.fnGetMemEarn7day('" + memcode + "','4') as memzs");
            }
            else if (type == "30")
            {
                return new bllPaging().GetDataTableInfoBySQL("select dbo.fnGetMemEarn('" + memcode + "','1,2,3,4') as earn,dbo.fnGetMemEarnToday('" + memcode + "','1') as goodpt,dbo.fnGetMemEarnToday('" + memcode + "','2') as mempt,dbo.fnGetMemEarnToday('" + memcode + "','3') as goodzs,dbo.fnGetMemEarnToday('" + memcode + "','4') as memzs");
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取我的推广会员
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataSet MyMembers(string GUID, string UID)
        {
            return new bllPaging().GetDataSetInfoBySQL("select memcode,(select nickname from wx_members_wx where memcode=TR_Members.memcode) as nickname,ctime,(select headimgurl from wx_members_wx where memcode=TR_Members.memcode) as headurl,dbo.fnGetMemPromoteNum(memcode) as tgnum from TR_Members where pmemcode=(select memcode from WX_members_wx where openid='" + UID + "');select memcode,(select nickname from wx_members_wx where memcode=TR_Members.memcode) as nickname,ctime,(select headimgurl from wx_members_wx where memcode=TR_Members.memcode) as headurl,dbo.fnGetMemPromoteNum(memcode) as tgnum from TR_Members where pmemcode in (select memcode from TR_Members where pmemcode=(select memcode from WX_members_wx where openid='" + UID + "'));select pmemcode from TR_Members where memcode=(select memcode from WX_members_wx where openid='" + UID + "')");
        }

        /// <summary>
        /// 获取我的推广会员搜索
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataTable MyMemberSearch(string GUID, string UID, string nickname, string type)
        {
            string filter = " where 1=1";
            if (!string.IsNullOrWhiteSpace(nickname))
            {
                filter = " and memcode in (select memcode from wx_members_wx where nickname like '%" + nickname + "%')";
            }
            if (type == "1")
            {
                filter += " and pmemcode=(select memcode from WX_members_wx where openid='" + UID + "')";
            }
            else if (type == "0")
            {
                filter += " and pmemcode in (select memcode from TR_Members where pmemcode=(select memcode from WX_members_wx where openid='" + UID + "'))";
            }
            else
            {
                filter += " and 1=2";
            }
            return new bllPaging().GetDataTableInfoBySQL("select memcode,(select nickname from wx_members_wx where memcode=TR_Members.memcode) as nickname,ctime,(select headimgurl from wx_members_wx where memcode=TR_Members.memcode) as headurl,isnull((select sex from wx_members_wx where memcode=TR_Members.memcode),'') as sex,dbo.fnGetMemPromoteNum(memcode) as tgnum from TR_Members  " + filter);
        }

        /// <summary>
        /// 邀请码
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="memcode"></param>
        /// <returns></returns>
        public DataTable GetInviteCode(string GUID, string UID)
        {
            return new bllPaging().GetDataTableInfoBySQL("select invitecode from wx_members_wx where openid='" + UID + "'");
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
        public membersEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new membersEntity();
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
            //stoname 门店名称 areaname 所属区域, idtypeName 身份证类型, totalcard 卡数量 
            return new bllPaging().GetPagingInfo("members", "memcode", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }


        /// <summary>
        /// 会员List使用
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <param name="pagenums"></param>
        /// <returns></returns>
        public DataTable GetPagingInfoByList(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            //stoname 门店名称 areaname 所属区域, idtypeName 身份证类型, totalcard 卡数量 
            string query = " *,cusername=dbo.fnGetUserName(cuser),uusername=dbo.fnGetUserName(uuser) ";
            query += " ,stoname =dbo.fnGetStoreName(strcode) ";
            query += " ,areaname =dbo.fnGetAreaFullName(provinceid,cityid,areaid)";
            query += ",idtypeName=dbo.f_Get_DictsName(IDType)";
            return new bllPaging().GetPagingInfo("members", "memcode", query, pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

  
        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private membersEntity SetEntityInfo(DataRow dr)
        {
            membersEntity Entity = new membersEntity();
            Entity.memid = dr["memid"].ToString();
            Entity.memcode = dr["memcode"].ToString();
            Entity.wxaccount = dr["wxaccount"].ToString();
            Entity.mobile = dr["mobile"].ToString();
            Entity.remark = dr["remark"].ToString();
            Entity.status = dr["status"].ToString();
            return Entity;
        }

    }
}