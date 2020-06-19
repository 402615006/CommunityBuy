using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.BackWeb.ajax.dishes
{
    /// <summary>
    /// 系统字典信息接口类
    /// </summary>
    public class WSsto_ts_Dicts : ServiceBase
    {
        bllts_Dicts bll = new bllts_Dicts();
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
                        case "updatestatus"://修改状态
                            UpdateStatus(dicPar);
                            break;
                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "pageSize", "currentPage", "filter", "order" };
            //检测方法需要的参数


            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            int pageSize = StringHelper.StringToInt(dicPar["pageSize"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["currentPage"].ToString());
            string filter = dicPar["filter"].ToString();
            //filter = CombinationFilter(new List<string>() { "dicid","buscode","strcode","dictype","lng","pdicid","diccode","dicname","dicvalue","orderno","remark","status","cuser" }, dicPar, typeof(string), filter);
            string order = dicPar["order"].ToString();
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void Add(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "dicid", "buscode", "strcode", "dictype", "lng", "pdicid", "diccode", "dicname", "dicvalue", "orderno", "remark", "status", "cuser", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string dicid = dicPar["dicid"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string strcode = dicPar["strcode"].ToString();
            string dictype = dicPar["dictype"].ToString();
            string lng = dicPar["lng"].ToString();
            string pdicid = dicPar["pdicid"].ToString();
            string diccode = dicPar["diccode"].ToString();
            string dicname = dicPar["dicname"].ToString();
            string dicvalue = dicPar["dicvalue"].ToString();
            string orderno = dicPar["orderno"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string cuser = dicPar["cuser"].ToString();
            //调用逻辑
            bll.Add(GUID, USER_ID, dicid, buscode, strcode, dictype, lng, pdicid, diccode, dicname, dicvalue, orderno, remark, status, cuser);
            //table添加一个id，防止多次提交
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "dicid", "buscode", "strcode", "dictype", "lng", "pdicid", "diccode", "dicname", "dicvalue", "orderno", "remark", "status", "cuser", };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string dicid = dicPar["dicid"].ToString();
            string buscode = dicPar["buscode"].ToString();
            string strcode = dicPar["strcode"].ToString();
            string dictype = dicPar["dictype"].ToString();
            string lng = dicPar["lng"].ToString();
            string pdicid = dicPar["pdicid"].ToString();
            string diccode = dicPar["diccode"].ToString();
            string dicname = dicPar["dicname"].ToString();
            string dicvalue = dicPar["dicvalue"].ToString();
            string remark = dicPar["remark"].ToString();
            string status = dicPar["status"].ToString();
            string cuser = dicPar["cuser"].ToString();

            //调用逻辑
            ts_DictsEntity UEntity = bll.GetEntitySigInfo(" where id="+ dicid);
            UEntity.dicname = dicname;
            UEntity.dicvalue = dicvalue;

            bll.Update(GUID, USER_ID, UEntity);
            //table添加一个id，防止多次提交
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "dicid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string dicid = dicPar["dicid"].ToString();
            //调用逻辑			
            dt = bll.GetPagingSigInfo(GUID, USER_ID, "where dicid=" + dicid);
            ReturnListJson(dt,1,1,1,1);
        }

        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "dicid" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string dicid = dicPar["dicid"].ToString();
            //调用逻辑
            bll.Delete(GUID, USER_ID, dicid);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "dicid", "status" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string status = dicPar["status"].ToString();

            string dicid = dicPar["dicid"].ToString().Trim(',');
            bll.UpdateStatus(GUID, USER_ID, dicid, status);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }
    }
}