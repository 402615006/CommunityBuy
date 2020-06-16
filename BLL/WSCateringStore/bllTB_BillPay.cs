using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 账单支付业务类
    /// </summary>
    public class bllTB_BillPay : bllBase
    {
		DAL.dalTB_BillPay dal = new DAL.dalTB_BillPay();
        TB_BillPayEntity Entity = new TB_BillPayEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string PKCode, string BillCode, string PayMoney, string PayMethodName, string PayMethodCode, string Remar, string OutOrderCode, string PPKCode)
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
            CheckValue<TB_BillPayEntity>(EName, EValue, ref errorCode, new TB_BillPayEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new TB_BillPayEntity();
				Entity.Id = Helper.StringToLong(Id);
				Entity.BusCode = BusCode;
				Entity.StoCode = StoCode;
				Entity.CCode = CCode;
				Entity.CCname = CCname;
				
				Entity.TStatus = TStatus;
				Entity.PKCode = PKCode;
				Entity.BillCode = BillCode;
				Entity.PayMoney = Helper.StringToDecimal(PayMoney);
				Entity.PayMethodName = PayMethodName;
				Entity.PayMethodCode = PayMethodCode;
				Entity.Remar = Remar;
				Entity.OutOrderCode = OutOrderCode;
				Entity.PPKCode = PPKCode;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID,  string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus,out string PKCode, string BillCode, string PayMoney, string PayMethodName, string PayMethodCode, string Remar, string OutOrderCode, string PPKCode,string DiscountName,string DiscountMoney,string ZeroCutMoney,string AuthCode,string AuthName,string MemberCard, string MemberName,string  MemberBalance,string  MemberLeve,string  MemberDiscount, operatelogEntity entity)
        {
            PKCode = "0";
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add",  Id, BusCode, StoCode, CCode, CCname, TStatus, PKCode, BillCode, PayMoney, PayMethodName, PayMethodCode, Remar, OutOrderCode, PPKCode);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
            int result = dal.Add(ref Entity,DiscountName,DiscountMoney,ZeroCutMoney,AuthCode,AuthName,MemberCard,MemberName,MemberBalance,MemberLeve,MemberDiscount);
            PKCode = Entity.PKCode;
            //检测执行结果
            DataRow dr = dtBase.NewRow();
            dr["type"] = result;
            dr["mes"] = Entity.PKCode;
            dtBase.Rows.Add(dr);
            dtBase.AcceptChanges();
            return dtBase;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public DataTable Update(string GUID, string UID,  string Id, string BusCode, string StoCode, string CCode, string CCname, string TStatus, string PKCode, string BillCode, string PayMoney, string PayMethodName, string PayMethodCode, string Remar, string OutOrderCode, string PPKCode, operatelogEntity entity)
        {
			
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update",  Id, BusCode, StoCode, CCode, CCname, TStatus, PKCode, BillCode, PayMoney, PayMethodName, PayMethodCode, Remar, OutOrderCode, PPKCode);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
			//获取更新前的数据对象
            TB_BillPayEntity OldEntity = new TB_BillPayEntity();
            OldEntity = GetEntitySigInfo(" where Id='" + Id + "'");
			//更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<TB_BillPayEntity>(entity, Entity, OldEntity);
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
        public DataTable UpdateStatus(string GUID, string UID, string StoCode,string PkCode, string Status, string DiscountName, string DiscountMoney, string ZeroCutMoney, string AuthCode, string AuthName)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateStatus(StoCode,PkCode, Status,DiscountName, DiscountMoney, ZeroCutMoney, AuthCode, AuthName);
            //检测执行结果
			CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataTable UpdateStatusByOutOrderCode(string GUID, string UID, string StoCode, string OutOrderCode, string Status)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateStatusByOurOrderCode(StoCode, Status, OutOrderCode);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public DataTable Delete(string GUID, string UID, string PKCode,string StoCode,operatelogEntity entity)
        {
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
			string Mescode = string.Empty;
            int result = dal.Delete(PKCode,StoCode, ref Mescode);
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
        public TB_BillPayEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_BillPayEntity();
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
            return new bllPaging().GetPagingInfoTime("TB_BillPay", "Id", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_BillPayEntity SetEntityInfo(DataRow dr)
        {
            TB_BillPayEntity Entity = new TB_BillPayEntity();
			Entity.Id = Helper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.PKCode = dr["PKCode"].ToString();
			Entity.BillCode = dr["BillCode"].ToString();
			Entity.PayMoney = Helper.StringToDecimal(dr["PayMoney"].ToString());
			Entity.PayMethodName = dr["PayMethodName"].ToString();
			Entity.PayMethodCode = dr["PayMethodCode"].ToString();
			Entity.Remar = dr["Remar"].ToString();
			Entity.OutOrderCode = dr["OutOrderCode"].ToString();
			Entity.PPKCode = dr["PPKCode"].ToString();
            return Entity;
        }

        /// <summary>
        /// 反结算
        /// </summary>
        /// (GUID, USER_ID, BusCode, StoCode, CCode, CCname, TStatus, out PKCode, PayMoney, Remar, PPKCode, logentity
        public DataTable Back(string GUID, string UID, string BusCode, string StoCode, string CCode, string CCname, string TStatus,out string PKCode, string PayMoney,  string Remar,out string OutOrderCode, string PPKCode, operatelogEntity entity,out string PayMethodCode)
        {
            PKCode = "0";
            OutOrderCode = "0";
            PayMethodCode = "0";

            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();

            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add", "0", BusCode, StoCode, CCode, CCname, TStatus, PKCode, "0", PayMoney, "0", "0", Remar, OutOrderCode, PPKCode);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }

            int result =dal.Back(ref Entity);
            PKCode = Entity.PKCode;
            OutOrderCode = Entity.OutOrderCode;
            PayMethodCode = Entity.PayMethodCode;
            //检测执行结果
            DataRow dr = dtBase.NewRow();
            dr["type"] = result;
            dr["mes"] = Entity.PKCode;
            dtBase.Rows.Add(dr);
            dtBase.AcceptChanges();
            return dtBase;
        }

    }
}