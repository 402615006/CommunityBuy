using CommunityBuy.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.IServices
{
    /// <summary>
    /// WSSystemCommon 公共的一些接口 的摘要说明
    /// </summary>
    public class WS_SystemCommon : ServiceBase
    {
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
                        case "getpermission"://获取页面权限按钮信息
                            GetPermission(dicPar);
                            break;
                    }
                }
            }
        }
        private void GetPermission(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "userid", "pagecode" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string userid = dicPar["userid"].ToString();
            string pagecode = dicPar["pagecode"].ToString();

            //调用逻辑
            DataTable dt = new bllTB_Functions().GetFunctionsButtonByPageCode(GUID, userid, pagecode);
            ReturnListJson(dt,null,null,null,null);
        }
    }
}