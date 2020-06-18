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
        public bool CheckPageInfo(string type, string Id, string BusCode, string StoCode, string CCname, string TStatus, string FType, string ParentId, string Code, string Cname, string BtnCode, string Orders, string ImgName, string Url, string Level, string Descr,string CCode)
        {
            bool rel = false;
            try
            {
                Entity = new TB_FunctionsEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.BusCode = BusCode;
                Entity.StoCode = StoCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;

                Entity.TStatus = TStatus;
                Entity.FType = StringHelper.StringToInt(FType);
                Entity.ParentId = StringHelper.StringToLong(ParentId);
                Entity.Code = Code;
                Entity.Cname = Cname;
                Entity.BtnCode = BtnCode;
                Entity.Orders = StringHelper.StringToInt(Orders);
                Entity.ImgName = ImgName;
                Entity.Url = Url;
                Entity.Level = StringHelper.StringToInt(Level);
                Entity.Descr = Descr;
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
        public void Add(string GUID, string UID,  string Id, string BusCode, string StoCode, string CCname, string TStatus, string FType, string ParentId, string Code, string Cname, string BtnCode, string Orders, string ImgName, string Url, string Level, string Descr,string CCode)
        {
            int result = 0;
           bool strReturn = CheckPageInfo("add",  Id, BusCode, StoCode, CCname, TStatus, FType, ParentId, Code, Cname, BtnCode, Orders, ImgName, Url, Level, Descr,CCode);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
            }
            result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result, Entity.Id.ToString());
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_FunctionsEntity UEntity)
        {
			
			//更新数据
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result, "");
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public void Delete(string GUID, string UID, string Id)
        {

			string Mescode = string.Empty;
            int result = dal.Delete(Id, ref Mescode);
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
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.StoCode = dr["StoCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.FType = StringHelper.StringToInt(dr["FType"].ToString());
			Entity.ParentId = StringHelper.StringToLong(dr["ParentId"].ToString());
			Entity.Code = dr["Code"].ToString();
			Entity.Cname = dr["Cname"].ToString();
			Entity.BtnCode = dr["BtnCode"].ToString();
			Entity.Orders = StringHelper.StringToInt(dr["Orders"].ToString());
			Entity.ImgName = dr["ImgName"].ToString();
			Entity.Url = dr["Url"].ToString();
			Entity.Level = StringHelper.StringToInt(dr["Level"].ToString());
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