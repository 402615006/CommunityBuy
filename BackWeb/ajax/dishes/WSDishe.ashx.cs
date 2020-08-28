using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommunityBuy.BackWeb.ajax.dishes
{
    /// <summary>
    /// 门店_菜品类别接口类
    /// </summary>
    public class WSDishe : ServiceBase
    {
        bllTB_DishType bll = new bllTB_DishType();
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
                        case "getmethodlist"://获取菜品的规格
                            GetMethodList(dicPar);
                            break;
                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key","page", "pagesize", "ftype","stype","name" };
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            int pageSize = StringHelper.StringToInt(dicPar["pagesize"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            object ftype = "";
            dicPar.TryGetValue("ftype", out ftype);
            object stype = "";
            dicPar.TryGetValue("stype", out stype);
            object name = "";
            dicPar.TryGetValue("name", out name);

            string filter = " dis.tstatus='1'";
            if (ftype!=null)
            {
                filter += " and dis.Typecode in(select pkcode from TB_DishType where pkkcode='" + ftype + "' ) ";
            }
            if (stype!=null)
            {
                filter += " and dis.Typecode='"+ stype + "' ";
            }
            if (name!=null)
            {
                filter += " and dis.DisName like('%" + name + "%') ";
            }
            string order = "";
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = new bllTB_Dish().GetPagingListInfo(GUID,USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        /// <summary>
        /// 获取菜品的做法
        /// </summary>
        /// <param name="dicPar"></param>
        private void GetMethodList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "key", "discode" };
            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            object discode = "";
            dicPar.TryGetValue("discode", out discode);

            string filter = " 1=1";
            if (discode != null)
            {
                filter += " and discode like('%,"+ discode + ",%') ";
            }

            string order = "";
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = new bllPaging().GetPagingInfo("TR_DishesMethods","id","*", int.MaxValue, 1, filter,"",order, out recordCount, out totalPage);
            ReturnListJson(dt, int.MaxValue, recordCount,1, totalPage);
        }

    }
}