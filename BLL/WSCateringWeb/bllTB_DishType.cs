using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 菜品类别表业务类
    /// </summary>
    public class bllTB_DishType : bllBase
    {
		DAL.dalTB_DishType dal = new DAL.dalTB_DishType();
        TB_DishTypeEntity Entity = new TB_DishTypeEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCname, string PKKCode, string PKCode, string TypeName, string Sort, string TStatus,string CCode)
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
            CheckValue<TB_DishTypeEntity>(EName, EValue, ref errorCode, new TB_DishTypeEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new TB_DishTypeEntity();
				Entity.Id = Helper.StringToLong(Id);
				Entity.BusCode = BusCode;
				Entity.StoCode = StoCode;
				Entity.CCode = CCode;
				Entity.CCname = CCname;
				Entity.PKKCode = PKKCode;
				Entity.PKCode = PKCode;
				Entity.TypeName = TypeName;
				Entity.Sort = Helper.StringToInt(Sort);
				Entity.TStatus = TStatus;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID, string Id, string BusCode, string StoCode, string CCname, string PKKCode, string PKCode, string TypeName, string Sort, string TStatus,string CCode, operatelogEntity entity)
        {
			PKCode = "0";
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add",  Id, BusCode, StoCode, CCname, PKKCode, PKCode, TypeName, Sort, TStatus,CCode);
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
        public DataTable Update(string GUID, string UID,  string Id, string BusCode, string StoCode, string CCname, string PKKCode, string PKCode, string TypeName, string Sort, string TStatus,string CCode, operatelogEntity entity)
        {
			
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update",  Id, BusCode, StoCode, CCname, PKKCode, PKCode, TypeName, Sort, TStatus,CCode);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
			//获取更新前的数据对象
            TB_DishTypeEntity OldEntity = new TB_DishTypeEntity();
            OldEntity = GetEntitySigInfo(" where PKCode='" + PKCode + "' and stocode='"+StoCode+"'");
			//更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<TB_DishTypeEntity>(entity, Entity, OldEntity);
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
        public TB_DishTypeEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_DishTypeEntity();
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
            return new bllPaging().GetPagingInfo("TB_DishType", "PKCode", "BusCode,StoCode,CCode,CCname,CTime,PKKCode,PKCode,PKKCode as pId,PKCode as id,'' as StoName,TypeName,Sort,TStatus,dbo.fn_GetDisTypeParentName(PKKCode) as PPKName", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 菜品类别管理树节点
        /// </summary>
        /// <returns></returns>
        public DataTable GetDisTypeTreeListInfo(string GUID, string UID, string filter, string order)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            return dal.GetDisTypeTreeListInfo(filter, order);
        }

        /// <summary>
        /// 菜品类别获取一级大类
        /// </summary>
        /// <returns></returns>
        public DataTable GetDisTypeOneListInfo(string GUID, string UID, string filter, string order)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            return dal.GetDisTypeOneListInfo(filter, order);
        }

        /// <summary>
        /// 菜品类别获取一级大类
        /// </summary>
        /// <returns></returns>
        public DataTable GetDishTypePPKToPKData(string GUID, string UID, string filter, string order)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            return dal.GetDishTypePPKToPKData(filter, order);
        }

        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_DishTypeEntity SetEntityInfo(DataRow dr)
        {
            TB_DishTypeEntity Entity = new TB_DishTypeEntity();
			Entity.Id = Helper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
            Entity.StoName = dr["StoName"].ToString();
            Entity.PKKCode = dr["PKKCode"].ToString();
			Entity.PKCode = dr["PKCode"].ToString();
			Entity.TypeName = dr["TypeName"].ToString();
			Entity.Sort = Helper.StringToInt(dr["Sort"].ToString());
			Entity.TStatus = dr["TStatus"].ToString();
            return Entity;
        }



    }
}