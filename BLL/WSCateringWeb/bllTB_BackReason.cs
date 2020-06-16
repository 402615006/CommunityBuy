using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 退单原因管理业务类
    /// </summary>
    public class bllTB_BackReason : bllBase
    {
		DAL.dalTB_BackReason dal = new DAL.dalTB_BackReason();
        TB_BackReasonEntity Entity = new TB_BackReasonEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string BusCode, string StoCode, string CCname, string UCname, string TStatus, string Sort, string PKCode, string Reason, string Ascription, string Remark,string CCode,string UCode)
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
            CheckValue<TB_BackReasonEntity>(EName, EValue, ref errorCode, new TB_BackReasonEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new TB_BackReasonEntity();
				Entity.Id = 0;
				Entity.BusCode = BusCode;
				Entity.StoCode = StoCode;
				Entity.CCode = CCode;
				Entity.CCname = CCname;
				
				Entity.UCode = UCode;
				Entity.UCname = UCname;
				
				Entity.TStatus = TStatus;
				Entity.Sort = Helper.StringToInt(Sort);
				Entity.PKCode = PKCode;
				Entity.Reason = Reason;
				Entity.Ascription = Ascription;
				Entity.Remark = Remark;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID,string BusCode, string StoCode, string CCname, string UCname, string TStatus, string Sort, string PKCode, string Reason, string Ascription, string Remark,string CCode,string UCode, operatelogEntity entity)
        {
			PKCode = "0";
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add",BusCode, StoCode, CCname, UCname, TStatus, Sort, PKCode, Reason, Ascription, Remark,CCode,UCode);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
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
        public DataTable Update(string GUID, string UID,string BusCode, string StoCode, string CCname, string UCname, string TStatus, string Sort, string PKCode, string Reason, string Ascription, string Remark, string CCode, string UCode, operatelogEntity entity)
        {
			
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update",BusCode, StoCode, CCname, UCname, TStatus, Sort, PKCode, Reason, Ascription, Remark,CCode,UCode);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
			//获取更新前的数据对象
            TB_BackReasonEntity OldEntity = new TB_BackReasonEntity();
            OldEntity = GetEntitySigInfo(" where PKCode='" + PKCode + "'");
			//更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<TB_BackReasonEntity>(entity, Entity, OldEntity);
                }
            }
            return dtBase;
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="PKCode">标识</param>
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
        public DataTable Delete(string GUID, string UID, string PKCode, operatelogEntity entity)
        {
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
			string Mescode = string.Empty;
            int result = dal.Delete(PKCode, ref Mescode);
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
        public TB_BackReasonEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_BackReasonEntity();
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
            return new bllPaging().GetPagingInfo("TB_BackReason", "PKCode", "*,'' as StoName,(case TStatus when '0' then '无效' else '有效' end) TStatusName,(case Ascription when '0' then '楼面' else '后厨' end) AscriptionName", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_BackReasonEntity SetEntityInfo(DataRow dr)
        {
            TB_BackReasonEntity Entity = new TB_BackReasonEntity();
			Entity.Id = Helper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.UCode = dr["UCode"].ToString();
			Entity.UCname = dr["UCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.Sort = Helper.StringToInt(dr["Sort"].ToString());
			Entity.PKCode = dr["PKCode"].ToString();
			Entity.Reason = dr["Reason"].ToString();
			Entity.Ascription = dr["Ascription"].ToString();
			Entity.Remark = dr["Remark"].ToString();
            return Entity;
        }
    }
}