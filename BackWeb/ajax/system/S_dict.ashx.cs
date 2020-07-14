using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.ajax.system
{
    /// <summary>
    /// globalreq 的摘要说明
    /// </summary>
    public class S_dict : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Charset = "utf-8";
            string type = context.Request["type"];
            switch (type)
            {
                case "dict":
                    getDicts(context);
                    break;
                case "dictsigle":
                    getDictinfo(context);
                    break;
            }
        }


        #region  获取所有字典数据  转换为json 进行传递
        /// <summary>
        /// 获取所有字典数据  转换为json 进行传递
        /// </summary>
        /// <param name="context"></param>
        private void getDicts(HttpContext context)
        {
            BLL.bllts_Dicts dictbll = new BLL.bllts_Dicts();
            int intCount = 0;
            int pagenums = 0;
            DataTable dt = dictbll.GetPagingListInfo("", "0", 10000, 1, "", "", out intCount, out pagenums);
            List<ts_DictDto> list = new List<ts_DictDto>();
            if (dt.Rows.Count > 0)
            {

                DataRow[] rows = dt.Select("pdicid =0 "); // 
                for (int i = 0; i < rows.Length; i++)
                {


                    ts_DictDto dto = new ts_DictDto();

                    dto.id = int.Parse(rows[i]["dicid"].ToString());
                    dto.isParent = true;
                    dto.open = false;
                    dto.name = rows[i]["dicname"].ToString();
                    dto.pId = rows[i]["pdicid"].ToString();
                    dto.iconClose = "../img/dict_close.png";
                    dto.iconOpen = "../img/dict_open.png";
                    DataRow[] itemsrows = dt.Select("pdicid =" + dto.id); // 
                    if (itemsrows.Length > 0)
                    {
                        List<ts_DictDto> itemlist = new List<ts_DictDto>();
                        for (int k = 0; k < itemsrows.Length; k++)
                        {
                            ts_DictDto itemdto = new ts_DictDto();

                            itemdto.id = int.Parse(itemsrows[k]["dicid"].ToString());
                            itemdto.isParent = false;
                            itemdto.open = false;
                            itemdto.name = itemsrows[k]["dicname"].ToString();
                            itemdto.pId = itemsrows[k]["pdicid"].ToString();
                            itemdto.icon = "../img/dict_chilren.png";
                            itemlist.Add(itemdto);
                            dto.children = itemlist;
                        }
                    }
                    list.Add(dto);


                }
            }
            JavaScriptSerializer s_serializer = new JavaScriptSerializer(); // 通过JavaScriptSerializer对象的Serialize序列化为["value1","value2",...]的字符串 
            context.Response.Write(s_serializer.Serialize(list)); // 返回客户端json格式数据
        }
        #endregion


        #region   获取字典单个值
        /// <summary>
        /// 获取传递过来id 参数  转换为json 进行传递
        /// </summary>
        /// <param name="context"></param>
        private void getDictinfo(HttpContext context)
        {
            string id = context.Request["id"];
            BLL.bllts_Dicts dictbll = new BLL.bllts_Dicts();
            int intCount = 0;
            int pagenums = 0;
            DataTable dt = dictbll.GetPagingListInfo("", "0", 10000, 1, " dicid =" + id, "", out intCount, out pagenums);

            string json = JsonHelper.DataTableToJSON(dt);
            context.Response.Write(json);
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}