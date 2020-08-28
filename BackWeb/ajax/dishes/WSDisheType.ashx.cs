using System.Collections.Generic;
using System.Data;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace CommunityBuy.BackWeb.ajax.dishes
{
    /// <summary>
    /// 门店_菜品类别接口类
    /// </summary>
    public class WSDisheType : ServiceBase
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
                            GetTree(dicPar);
                            break;
                        case "getapplist"://列表
                            GetList(dicPar);
                            break;
                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "pageSize", "currentPage", "filter", "order" };
            //检测方法需要的参数


            //获取参数信息
            string GUID = "0";
            string USER_ID = "0";
            int pageSize = StringHelper.StringToInt(dicPar["pageSize"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["currentPage"].ToString());
            string filter = dicPar["filter"].ToString();
            //filter = CombinationFilter(new List<string>() { "buscode","stocode","pdistypecode","distypecode","dispath","distypename","metcode","fincode","maxdiscount","busSort","status","cuser","uuser" }, dicPar, typeof(string), filter);
            string order = "sort asc";
            switch (filter)
            {
                case "order":
                    filter = "pkkcode='B14'";
                    break;
            }
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        private void GetTree(Dictionary<string, object> dicPar)
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
            //filter = CombinationFilter(new List<string>() { "buscode","stocode","pdistypecode","distypecode","dispath","distypename","metcode","fincode","maxdiscount","busSort","status","cuser","uuser" }, dicPar, typeof(string), filter);
            string order = "sort asc";
            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            List<ts_DictDto> list = new List<ts_DictDto>();
            if (dt.Rows.Count > 0)
            {

                DataRow[] rows = dt.Select("pkkcode='0'"); // 
                for (int i = 0; i < rows.Length; i++)
                {
                    ts_DictDto dto = new ts_DictDto();
                    dto.id = rows[i]["pkcode"].ToString();
                    dto.isParent = true;
                    dto.open = false;
                    dto.name = rows[i]["typename"].ToString();
                    dto.pId = rows[i]["pkkcode"].ToString();
                    dto.iconClose = "../img/dict_close.png";
                    dto.iconOpen = "../img/dict_open.png";
                    dto.sort = rows[i]["sort"].ToString();
                    DataRow[] itemsrows = dt.Select("pkkcode ='" + dto.id+"'"); // 
                    if (itemsrows.Length > 0)
                    {
                        List<ts_DictDto> itemlist = new List<ts_DictDto>();
                        for (int k = 0; k < itemsrows.Length; k++)
                        {
                            ts_DictDto itemdto = new ts_DictDto();
                            itemdto.id = itemsrows[k]["pkcode"].ToString();
                            itemdto.isParent = false;
                            itemdto.open = false;
                            itemdto.name = itemsrows[k]["typename"].ToString();
                            itemdto.pId = itemsrows[k]["pkkcode"].ToString();
                            itemdto.icon = "../img/dict_chilren.png";
                            dto.sort = itemsrows[k]["sort"].ToString();
                            itemlist.Add(itemdto);
                            dto.children = itemlist;
                        }
                    }
                    list.Add(dto);
                }
            }
            JavaScriptSerializer s_serializer = new JavaScriptSerializer(); // 通过JavaScriptSerializer对象的Serialize序列化为["value1","value2",...]的字符串 
            ReturnJsonStr(s_serializer.Serialize(list));
        }

    }
}