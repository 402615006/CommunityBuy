using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 折扣方案业务类
    /// </summary>
    public class bllTB_DiscountScheme : bllBase
    {
		DAL.dalTB_DiscountScheme dal = new DAL.dalTB_DiscountScheme();
        TB_DiscountSchemeEntity Entity = new TB_DiscountSchemeEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string PKCode, string BusCode, string StoCode, string CCname, string UCname, string TStatus, string Sort, string InsideCode, string DiscountRate, string MenuCode, string LevelCode, string SchName,string CCode,string UCode)
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
            CheckValue<TB_DiscountSchemeEntity>(EName, EValue, ref errorCode, new TB_DiscountSchemeEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new TB_DiscountSchemeEntity();
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
				Entity.InsideCode = InsideCode;
				Entity.DiscountRate = Helper.StringToDecimal(DiscountRate);
				Entity.MenuCode = MenuCode;
				Entity.LevelCode = LevelCode;
				Entity.SchName = SchName;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID, out string PKCode, string BusCode, string StoCode, string CCname, string UCname, string TStatus, string Sort, string InsideCode, string DiscountRate, string MenuCode, string LevelCode, string SchName, string CCode, string UCode,string discountschemerateJson, operatelogEntity entity)
        {
			PKCode = "0";
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add", PKCode,BusCode, StoCode, CCname, UCname, TStatus, Sort, InsideCode, DiscountRate, MenuCode, LevelCode, SchName,  CCode,  UCode);
            if (!string.IsNullOrEmpty(discountschemerateJson))//做法加价
            {
                Entity.DSRateList = EntityHelper.GetEntityListByJson<TR_DiscountSchemeRateEntity>(discountschemerateJson);
            }
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
        public DataTable Update(string GUID, string UID,string BusCode, string StoCode, string CCname, string UCname, string TStatus, string Sort, string PKCode, string InsideCode, string DiscountRate, string MenuCode, string LevelCode, string SchName, string CCode, string UCode,string discountschemerateJson, operatelogEntity entity)
        {
			
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update", PKCode, BusCode, StoCode, CCname, UCname, TStatus, Sort,  InsideCode, DiscountRate, MenuCode, LevelCode, SchName, CCode, UCode);
            if (!string.IsNullOrEmpty(discountschemerateJson))//做法加价
            {
                Entity.DSRateList = EntityHelper.GetEntityListByJson<TR_DiscountSchemeRateEntity>(discountschemerateJson);
            }
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
			//获取更新前的数据对象
            TB_DiscountSchemeEntity OldEntity = new TB_DiscountSchemeEntity();
            OldEntity = GetEntitySigInfo(" where PKCode='" + PKCode + "'");
			//更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<TB_DiscountSchemeEntity>(entity, Entity, OldEntity);
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
        public TB_DiscountSchemeEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_DiscountSchemeEntity();
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
            return new bllPaging().GetPagingInfo("TB_DiscountScheme", "PKCode", "*,'' as StoName,'' as LevelCodeName,[dbo].[fn_GetStoDisMenuName](MenuCode,StoCode) as MenuCodeName,(case TStatus when '0' then '无效' else '有效' end) TStatusName", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_DiscountSchemeEntity SetEntityInfo(DataRow dr)
        {
            TB_DiscountSchemeEntity Entity = new TB_DiscountSchemeEntity();
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
			Entity.InsideCode = dr["InsideCode"].ToString();
			Entity.DiscountRate = Helper.StringToDecimal(dr["DiscountRate"].ToString());
			Entity.MenuCode = dr["MenuCode"].ToString();
			Entity.LevelCode = dr["LevelCode"].ToString();
			Entity.SchName = dr["SchName"].ToString();
            return Entity;
        }

        /// <summary>
        /// 获取门店下的折扣方案
        /// </summary>
        /// <param name="GUID">登陆令牌</param>
        /// <param name="UID">用户id</param>
        /// <param name="stocode">门店编号</param>
        /// <returns></returns>
        public DataTable GetTableInfo(string GUID, string UID,string stocode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            return new bllPaging().GetDataTableInfoBySQL("select PKCode,SchName from TB_DiscountScheme where stocode='" + stocode + "'");
        }

        /// <summary>
        /// 获取指定折扣方案下的特殊折扣信息
        /// </summary>
        /// <param name="PKCode"></param>
        /// <returns></returns>
        public DataTable GetDiscountSchRate(string PKCode,string StoCode)
        {
            return dal.GetDiscountSchRate(PKCode, StoCode);
        }

        /// <summary>
        /// 根据模板编号获取模板详细
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="stocode"></param>
        /// <param name="PKCode"></param>
        /// <returns></returns>
        public DataTable GetDiscountSchemeRateByCode(string GUID, string UID, string stocode,string PKCode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            return new bllPaging().GetDataTableInfoBySQL("select A.DiscountRate as NDiscountRate,A.SchName,B.* from TB_DiscountScheme A left join TR_DiscountSchemeRate B on A.StoCode=B.StoCode and A.PKCode=B.SchCode where A.StoCode='" + stocode + "' and A.TStatus='1' and A.PKCode='" + PKCode + "'");
        }

        /// <summary>
        /// 根据会员编号获取模板详细
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="stocode"></param>
        /// <param name="LevelCode"></param>
        /// <returns></returns>
        public DataTable GetDiscountSchemeRateByLevelCode(string GUID, string UID, string stocode, string LevelCode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            return new bllPaging().GetDataTableInfoBySQL("select A.DiscountRate as NDiscountRate,A.SchName,B.* from TB_DiscountScheme A left join TR_DiscountSchemeRate B on A.StoCode=B.StoCode and A.PKCode=B.SchCode where A.StoCode='" + stocode + "' and A.TStatus='1' and A.PKCode=(select top 1 PKCode from TB_DiscountScheme where StoCode='" + stocode + "' and TStatus='1' and CHARINDEX(',"+ LevelCode + ",',','+LevelCode+',')>0)");
        }

        /// <summary>
        /// 获取用户折扣方案列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserDiscountScheme(string BusCode)
        {
            string where = "";
            if (!string.IsNullOrEmpty(BusCode))
            {
                where += " where uds.buscode='" + BusCode + "'";
            }
            return new bllPaging().GetDataTableInfoBySQL("  select uds.DisCountCode,ds.SchName,uds.usercode from [dbo].[TB_UserDiscountScheme] uds inner join [dbo].[TB_DiscountScheme] ds on uds.DisCountCode=ds.pkcode"+where);
        }

    }
}