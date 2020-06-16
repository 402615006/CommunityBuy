using System.Collections.Generic;
using System.Data;

using CommunityBuy.Model;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BLL
{
	/// <summary>
    /// 系统功能管理业务类
    /// </summary>
    public class bllTB_Functions : bllBase
    {
		DAL.dalTB_Functions dal = new DAL.dalTB_Functions();
        TB_FunctionsEntity Entity = new TB_FunctionsEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCname, string TStatus, string FType, string ParentId, string Code, string Cname, string BtnCode, string Orders, string ImgName, string Url, string Level, string Descr,string CCode)
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
            CheckValue<TB_FunctionsEntity>(EName, EValue, ref errorCode, new TB_FunctionsEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new TB_FunctionsEntity();
				Entity.Id = Helper.StringToLong(Id);
				Entity.BusCode = BusCode;
				Entity.StoCode = StoCode;
				Entity.CCode = CCode;
				Entity.CCname = CCname;
				
				Entity.TStatus = TStatus;
				Entity.FType = Helper.StringToInt(FType);
				Entity.ParentId = Helper.StringToLong(ParentId);
				Entity.Code = Code;
				Entity.Cname = Cname;
				Entity.BtnCode = BtnCode;
				Entity.Orders = Helper.StringToInt(Orders);
				Entity.ImgName = ImgName;
				Entity.Url = Url;
				Entity.Level = Helper.StringToInt(Level);
				Entity.Descr = Descr;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID, out  string Id, string BusCode, string StoCode, string CCname, string TStatus, string FType, string ParentId, string Code, string Cname, string BtnCode, string Orders, string ImgName, string Url, string Level, string Descr,string CCode, operatelogEntity entity)
        {
			Id = "0";
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string strReturn = CheckPageInfo("add",  Id, BusCode, StoCode, CCname, TStatus, FType, ParentId, Code, Cname, BtnCode, Orders, ImgName, Url, Level, Descr,CCode);
            //数据页面验证
            if (!CheckControl(strReturn))
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
        public DataTable Update(string GUID, string UID,  string Id, string BusCode, string StoCode, string CCname, string TStatus, string FType, string ParentId, string Code, string Cname, string BtnCode, string Orders, string ImgName, string Url, string Level, string Descr,string CCode, operatelogEntity entity)
        {
			
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update",  Id, BusCode, StoCode, CCname, TStatus, FType, ParentId, Code, Cname, BtnCode, Orders, ImgName, Url, Level, Descr,CCode);
            //数据页面验证
            if (!CheckControl(strReturn))
            {
                return dtBase;
            }
			//获取更新前的数据对象
            TB_FunctionsEntity OldEntity = new TB_FunctionsEntity();
            OldEntity = GetEntitySigInfo(" where Id='" + Id + "'");
			//更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<TB_FunctionsEntity>(entity, Entity, OldEntity);
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
        public DataTable Delete(string GUID, string UID, string Id, operatelogEntity entity)
        {
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
			string Mescode = string.Empty;
            int result = dal.Delete(Id, ref Mescode);
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
        public TB_FunctionsEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_FunctionsEntity();
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
            return new bllPaging().GetPagingInfo("TB_Functions", "Id", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_FunctionsEntity SetEntityInfo(DataRow dr)
        {
            TB_FunctionsEntity Entity = new TB_FunctionsEntity();
			Entity.Id = Helper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.FType = Helper.StringToInt(dr["FType"].ToString());
			Entity.ParentId = Helper.StringToLong(dr["ParentId"].ToString());
			Entity.Code = dr["Code"].ToString();
			Entity.Cname = dr["Cname"].ToString();
			Entity.BtnCode = dr["BtnCode"].ToString();
			Entity.Orders = Helper.StringToInt(dr["Orders"].ToString());
			Entity.ImgName = dr["ImgName"].ToString();
			Entity.Url = dr["Url"].ToString();
			Entity.Level = Helper.StringToInt(dr["Level"].ToString());
			Entity.Descr = dr["Descr"].ToString();
            return Entity;
        }

        /// <summary>
        /// 获取二级三级权限（根据一级）
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public DataTable GetFunctionListByParentId(string GUID, string UID, string parentid, string stocode)
        {
            return dal.GetFunctionListByParentId(GUID, UID, parentid, stocode);
        }

        /// <summary>
        /// 获取商品管理下的功能权限
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public DataTable GetFunctionListByParentIdSP(string GUID, string UID)
        {
            if (UID == "1")//管理员
            {
                return dal.GetFunctionListByParentIdSPAdmin(GUID, UID);
            }
            else
            {
                return dal.GetFunctionListByParentIdSP(GUID, UID);
            }
                
        }

        public DataTable GetFunctionsButtonByPageCode(string GUID, string UID, string PageCode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            if(UID=="1")//管理员
            {
                return new bllPaging().GetDataTableInfoBySQL("select code,cname,btncode as btnname,orders,imgname,url from TB_Functions where [tstatus]='1' and code='" + PageCode + "' and [level]=4");
            }
            else
            {

            }
            return new bllPaging().GetDataTableInfoBySQL("select code,cname,btncode as btnname,orders,imgname,url from TB_Functions where [tstatus]='1' and code='" + PageCode + "' and [level]=4 and id in(select functionid from TB_RoleFunction where roleid in (select roleid from TB_UserRole where userid='" + UID + "'))");
        }


        /// <summary>
        /// 获取当前登录用户的一级权限
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="stocode"></param>
        /// <returns></returns>
        public DataTable GetFirFunction(string GUID, string UID, string stocode)
        {
            return dal.GetFirFunction(GUID, UID, stocode);
        }

        /// <summary>
        /// 获取当前登录用户所有权限
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID">用户id</param>
        /// <param name="stocode">门店编号</param>
        /// <returns></returns>
        public DataTable GetUserFunctionList(string GUID, string UID, string stocode)
        {
            return dal.GetUserFuncionList(GUID, UID, stocode);
        }

    }
}