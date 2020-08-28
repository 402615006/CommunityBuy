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
        public bool CheckPageInfo(string type, string Id, string PKKCode, string PKCode, string TypeName, string Sort, string TStatus)
        {
            bool rel = false;
            try
            {
                Entity = new TB_DishTypeEntity();
                Entity.Id = StringHelper.StringToLong(Id);
                Entity.PKKCode = PKKCode;
                Entity.PKCode = PKCode;
                Entity.TypeName = TypeName;
                Entity.Sort = StringHelper.StringToInt(Sort);
                Entity.TStatus = TStatus;
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
        public void Add(string GUID, string UID, string Id,  string PKKCode, string PKCode, string TypeName, string Sort, string TStatus)
        {
            int result = 0;
           bool strReturn = CheckPageInfo("add",  Id,PKKCode, PKCode, TypeName, Sort, TStatus);
            //数据页面验证
            if (!strReturn)
            {
                 CheckResult(-2, "");
                return;
            }
            result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result,Entity.PKCode);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_DishTypeEntity UEntity)
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
        public void Delete(string GUID, string UID, string PKCode)
        {

			string Mescode = string.Empty;
            int result = dal.Delete(PKCode);
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

            return new bllPaging().GetPagingInfo("TB_DishType", "PKCode", "*,dbo.fn_GetDisTypeParentName(PKKCode) as PPKName", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        /// <summary>
        /// 菜品类别管理树节点
        /// </summary>
        /// <returns></returns>
        public DataTable GetDisTypeTreeListInfo(string GUID, string UID, string filter, string order)
        {

            return dal.GetDisTypeTreeListInfo(filter, order);
        }

        /// <summary>
        /// 菜品类别获取一级大类
        /// </summary>
        /// <returns></returns>
        public DataTable GetDisTypeOneListInfo(string GUID, string UID, string filter, string order)
        {

            return dal.GetDisTypeOneListInfo(filter, order);
        }

        /// <summary>
        /// 菜品类别获取一级大类
        /// </summary>
        /// <returns></returns>
        public DataTable GetDishTypePPKToPKData(string GUID, string UID, string filter, string order)
        {

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
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
            Entity.PKKCode = dr["PKKCode"].ToString();
			Entity.PKCode = dr["PKCode"].ToString();
			Entity.TypeName = dr["TypeName"].ToString();
			Entity.Sort = StringHelper.StringToInt(dr["Sort"].ToString());
			Entity.TStatus = dr["TStatus"].ToString();
            return Entity;
        }



    }
}