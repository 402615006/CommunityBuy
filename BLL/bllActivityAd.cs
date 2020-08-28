using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 业务类
    /// </summary>
    public class bllActivityAd : bllBase
    {
		DAL.dalActivityAd dal = new DAL.dalActivityAd();
        ActivityAdEntity Entity = new ActivityAdEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public bool CheckPageInfo(string type, string Id, string Title, string status, string Sort, string Description, string Type, string images, string Url)
        {
            bool rel = false;
            try
            {

                Entity = new ActivityAdEntity();
                Entity.Id = StringHelper.StringToInt(Id);
                Entity.Title = Title;
                Entity.status = StringHelper.StringToInt(status);
                Entity.Sort = StringHelper.StringToInt(Sort);
                Entity.Description = Description;
                Entity.Type = StringHelper.StringToInt(Type);
                Entity.images = images;
                Entity.Url = Url;
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
        public void Add(string GUID, string UID,  string Id, string Title, string status, string Sort, string Description, string Type, string images, string Url)
        {
            string spanids = string.Empty;
            bool strReturn = CheckPageInfo("add",  Id, Title, status, Sort, Description, Type, images, Url);
            //数据页面验证

            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
                return;
            }
            int result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result, Entity.Id.ToString());
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(string GUID, string UID,  string Id, string Title, string status, string Sort, string Description, string Type, string images, string Url)
        {
            bool strReturn = CheckPageInfo("update",  Id, Title, status, Sort, Description, Type, images, Url);
            //数据页面验证
            if (!strReturn)
            {
                CheckResult(-2, "");
                return;
            }
            //获取更新前的数据对象
            ActivityAdEntity OldEntity = new ActivityAdEntity();
            OldEntity = GetEntitySigInfo(" where Id='" + Id + "'");
            OldEntity.Title = Title;
            OldEntity.status = StringHelper.StringToInt(status);
            OldEntity.Sort = StringHelper.StringToInt(Sort);
            OldEntity.Description = Description;
            OldEntity.Type = StringHelper.StringToInt(Type);
            OldEntity.images = images;
            OldEntity.Url = Url;
            //更新数据
            int result = dal.Update(OldEntity);
            //检测执行结果
            CheckResult(result, "");
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public DataTable Update(string GUID, string UID, string userid, string uname, string upwd, string realname, string umobile, string empcode, string remark, string status, string cname, string ccode, string role, string scope, string stocode, string sigmonthmoney, string sigstocode,string buscode,string utype, operatelogEntity entity)
        public void UpdateByEntity(string GUID, string UID, ActivityAdEntity UEntity)
        {

            //更新数据
            int result = dal.Update(UEntity);
            //检测执行结果
            CheckResult(result, "");
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">标识</param>
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
        public ActivityAdEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new ActivityAdEntity();
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
            return new bllPaging().GetPagingInfo("ActivityAd", "Id", "*,(case when type=1 then '首页轮播' end) as typename", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private ActivityAdEntity SetEntityInfo(DataRow dr)
        {
            ActivityAdEntity Entity = new ActivityAdEntity();
			Entity.Id = StringHelper.StringToInt(dr["Id"].ToString());
			Entity.Title = dr["Title"].ToString();
			Entity.status = StringHelper.StringToInt(dr["status"].ToString());
			Entity.Sort = StringHelper.StringToInt(dr["Sort"].ToString());
			Entity.Description = dr["Description"].ToString();
			Entity.Type = StringHelper.StringToInt(dr["Type"].ToString());
			Entity.images = dr["images"].ToString();
			Entity.Url = dr["Url"].ToString();
            return Entity;
        }
    }
}