using CommunityBuy.BLL;
using CommunityBuy.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.IServices
{
    /// <summary>
    /// WS_TB_RoleFunction 角色功能 的摘要说明
    /// </summary>
    public class WS_TB_RoleFunction : ServiceBase
    {
        bllTB_RoleFunction bll = new bllTB_RoleFunction();
        DataTable dt = new DataTable();
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    logentity.module = "角色权限详细表";
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        case "add"://添加							
                            Add(dicPar);
                            break;
                        case "detail"://详细
                            Detail(dicPar);
                            break;
                        case "update"://修改							
                            Update(dicPar);
                            break;
                        case "delete"://删除
                            Delete(dicPar);
                            break;
                        case "getrolefunctioninfo"://获取角色权限关联
                            GetRoleFunctionInfo(dicPar);
                            break;
                        case "getroleusers"://获取角色中的所有人员
                            getroleusers(dicPar);
                            break;
                        case "getroleusersbystocode":
                            getroleusersByStocode(dicPar);
                            break;
                        case "getrolefunctioninfobystocode":
                            GetRoleFunctionInfoByStocode(dicPar);
                            break;
                    }
                }
            }
        }

        //获取权限角色关联
        private void GetRoleFunctionInfo(Dictionary<string, object> dicPar)
        {
            //List<string> pra = new List<string>() { "GUID", "userid", "roleid" };
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //string GUID = dicPar["GUID"].ToString();
            //string userid = dicPar["userid"].ToString();
            //string roleid = dicPar["roleid"].ToString();
            //dt = bll.GetRoleFunctionInfoList(GUID, userid, roleid);
            //ReturnListJson(dt);
        }

        /// <summary>
        /// 获取指定角色的功能
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetRoleFunctionInfoByStocode(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "GUID", "userid", "roleid" };
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string roleid = dicPar["roleid"].ToString();
            dt = bll.GetRoleFunctionInfoList(GUID, userid, roleid);
            ReturnListJson(dt);
        }


        //获取角色中的所有人员
        private void getroleusers(Dictionary<string, object> dicPar)
        {
            //List<string> pra = new List<string>() { "GUID", "userid", "roleid" };
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //string GUID = dicPar["GUID"].ToString();
            //string userid = dicPar["userid"].ToString();
            //string roleid = dicPar["roleid"].ToString();
            //dt = bll.GetRoleUsers(GUID, userid, roleid);
            //ReturnListJson(dt);
        }

        //获取角色中的所有人员
        private void getroleusersByStocode(Dictionary<string, object> dicPar)
        {
            //List<string> pra = new List<string>() { "GUID", "userid", "roleid", "stocode" };
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //string GUID = dicPar["GUID"].ToString();
            //string userid = dicPar["userid"].ToString();
            //string roleid = dicPar["roleid"].ToString();
            //string stocode = dicPar["stocode"].ToString();
            //dt = bll.GetRoleUsers(GUID, userid, roleid, stocode);
            //ReturnListJson(dt);
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "pageSize", "currentPage", "filter", "order" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            int pageSize = StringHelper.StringToInt(dicPar["pageSize"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["currentPage"].ToString());
            string filter = dicPar["filter"].ToString();
            //filter = CombinationFilter(new List<string>() { "id","roleid","funid" }, dicPar, typeof(string), filter);
            string order = dicPar["order"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            filter = GetBusCodeWhere(dicPar, filter, "buscode");
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, userid, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            //List<string> pra = new List<string>() { "GUID", "userid", "id", "roleid", "funid", };
            ////检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            ////获取参数信息
            //string GUID = dicPar["GUID"].ToString();
            //string userid = dicPar["userid"].ToString();
            //string id = dicPar["id"].ToString();
            //string roleid = dicPar["roleid"].ToString();
            //string funid = dicPar["funid"].ToString();
            ////调用逻辑
            //logentity.pageurl = "sto_rolefunctionEdit.html";
            //logentity.logcontent = "新增角色权限详细表信息";
            //logentity.cuser = StringHelper.StringToLong(userid);
            //logentity.otype = SystemEnum.LogOperateType.Add;
            //dt = bll.Add(GUID, userid, out  id, roleid, funid, entity);
            ////table添加一个id，防止多次提交
            //dt.Columns.Add("id", typeof(int));
            //dt.Rows[0]["id"] = id;
            //ReturnListJson(dt);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            ////要检测的参数信息
            //List<string> pra = new List<string>() { "GUID", "userid", "id", "roleid", "funid", };
            ////检测方法需要的参数
            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            ////获取参数信息
            //string GUID = dicPar["GUID"].ToString();
            //string userid = dicPar["userid"].ToString();
            //string id = dicPar["id"].ToString();
            //string roleid = dicPar["roleid"].ToString();
            //string funid = dicPar["funid"].ToString();

            ////调用逻辑
            //logentity.pageurl = "sto_rolefunctionEdit.html";
            //logentity.logcontent = "修改id为:" + id + "的角色权限详细表信息";
            //logentity.cuser = StringHelper.StringToLong(userid);
            //logentity.otype = SystemEnum.LogOperateType.Edit;
            //dt = bll.Update(GUID, userid, id, roleid, funid, logentity);
            ////table添加一个id，防止多次提交
            //dt.Columns.Add("id", typeof(int));
            //dt.Rows[0]["id"] = id;
            //ReturnListJson(dt);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string id = dicPar["id"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, userid, "where id=" + id);
            ReturnListJson(dt);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string id = dicPar["id"].ToString();
            //调用逻辑
            logentity.pageurl = "sto_rolefunctionList.html";
            logentity.logcontent = "删除id为:" + id + "的角色权限详细表信息";
            logentity.cuser = StringHelper.StringToLong(userid);
            logentity.otype = SystemEnum.LogOperateType.Delete;
            logentity.buscode = GetCacheToUserBusCode(logentity.cuser.ToString());
            dt = bll.Delete(GUID, userid, id, logentity);
            ReturnListJson(dt);
        }
    }
}