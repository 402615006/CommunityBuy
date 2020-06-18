using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 购物车设置业务类
    /// </summary>
    public class bllTB_ShoppingCart : bllBase
    {
		DAL.dalTB_ShoppingCart dal = new DAL.dalTB_ShoppingCart();
        TB_ShoppingCartEntity Entity = new TB_ShoppingCartEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string BusCode, string CCname, string TStatus, string PKCode, string AutoDelTime, string IsAutoDelete, string MaxNum, string CCode)
        {
            bool rel = false;
            try
            {
                Entity = new TB_ShoppingCartEntity();
                Entity.Id = 0;
                Entity.BusCode = BusCode;
                Entity.CCode = CCode;
                Entity.CCname = CCname;

                Entity.TStatus = TStatus;
                Entity.PKCode = PKCode;
                Entity.AutoDelTime = StringHelper.StringToDateTime(AutoDelTime);
                Entity.IsAutoDelete = IsAutoDelete;
                Entity.MaxNum = StringHelper.StringToInt(MaxNum);
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
        public void Add(string GUID, string UID,string BusCode, string CCname, string TStatus, string PKCode, string AutoDelTime, string IsAutoDelete, string MaxNum,string CCode)
        {
            PKCode = "0";

            bool strReturn = CheckPageInfo("add", BusCode, CCname, TStatus, PKCode, AutoDelTime, IsAutoDelete, MaxNum, CCode);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
                return;
            }
            int result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result,Entity.PKCode);
   
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID, TB_ShoppingCartEntity UEntity)
        {
			//更新数据
            int result = dal.Update(UEntity);
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
            int result = dal.Delete(PKCode, ref Mescode);
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
        public TB_ShoppingCartEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new TB_ShoppingCartEntity();
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
            return new bllPaging().GetPagingInfo("TB_ShoppingCart", "PKCode", "*,(case TStatus when '0' then '无效' else '有效' end) TStatusName,(case IsAutoDelete when '0' then '已关闭' else '已开启' end) IsAutoDeleteName,CONVERT(varchar(100), AutoDelTime, 24) as strAutoDelTime", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TB_ShoppingCartEntity SetEntityInfo(DataRow dr)
        {
            TB_ShoppingCartEntity Entity = new TB_ShoppingCartEntity();
			Entity.Id = StringHelper.StringToLong(dr["Id"].ToString());
			Entity.BusCode = dr["BusCode"].ToString();
			Entity.CCode = dr["CCode"].ToString();
			Entity.CCname = dr["CCname"].ToString();
			
			Entity.TStatus = dr["TStatus"].ToString();
			Entity.PKCode = dr["PKCode"].ToString();
			Entity.AutoDelTime = StringHelper.StringToDateTime(dr["AutoDelTime"].ToString());
			Entity.IsAutoDelete = dr["IsAutoDelete"].ToString();
			Entity.MaxNum = StringHelper.StringToInt(dr["MaxNum"].ToString());
            return Entity;
        }
    }
}