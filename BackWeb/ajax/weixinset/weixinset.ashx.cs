using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BackWeb.ajax.weixinset
{
    /// <summary>
    /// weixinset 的摘要说明
    /// </summary>
    public class weixinset : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            string type = Helper.ReplaceString(context.Request["way"]);
            switch (type)
            {
                case "existsetdate":
                    {
                        string stocode = Helper.ReplaceString(context.Request["stocode"]);
                        string sql = string.Format("select 1 from wx_setdate where stocode='{0}' and isdelete=0", stocode);
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                    }
                    break;
                case "existbasicset":
                    {
                        string stocode = Helper.ReplaceString(context.Request["stocode"]);
                        string sql = string.Format("select 1 from WX_stoset where stocode='{0}'", stocode);
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                    }
                    break;
                case "existstocode":
                    {
                        string stocode = Helper.ReplaceString(context.Request["stocode"]);
                        string sql = string.Format("select 1 from store where stocode='{0}'", stocode);
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                    }
                    break;
                case "existstonameinsq":
                    {
                        string stoname = Helper.ReplaceString(context.Request["stoname"]);
                        string sqcode = Helper.ReplaceString(context.Request["sqcode"]);
                        string sql = string.Format("select 1 from store a inner join storegx b on (a.stocode=b.stocode) where b.sqcode='{0}' and a.cname='{1}'", sqcode, stoname);
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                    }
                    break;
                case "existquepnumcross":
                    {
                        string stocode = Helper.ReplaceString(context.Request["stocode"]);
                        string maxnum = Helper.ReplaceString(context.Request["maxnum"]);
                        string minnum = Helper.ReplaceString(context.Request["minnum"]);
                        string sql = string.Format("select 1 from WX_setlineUp where (({0}<=minperosn and {1}>=minperosn) or ({0}>minperosn and {0}<=maxperson)) and stocode='{2}' and isdelete=0",
                            minnum, maxnum, stocode);
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                    }
                    break;
                case "existsqname":
                    {
                        string sqname = Helper.ReplaceString(context.Request["sqname"]);
                        string citycode = Helper.ReplaceString(context.Request["citycode"]);
                        string sql = string.Format("select 1 from sqinfo where sqname='{0}' and city='{1}' and isdelete=0", sqname, citycode);
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                    }
                    break;
                case "existsremarkset":
                    {
                        string stocode = Helper.ReplaceString(context.Request["stocode"]);
                        string remark = Helper.ReplaceString(context.Request["remark"]);
                        string sql = string.Format("select 1 from WX_setremark where stocode='{0}' and dicname='{1}' and isdelete=0", stocode, remark);
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                    }
                    break;
                case "remarksetcount":
                    {
                        string stocode = Helper.ReplaceString(context.Request["stocode"]);
                        string sql = string.Format("select count(1) from WX_setremark where stocode='{0}' and status=1 and isdelete=0", stocode);
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            int count = (int)dt.Rows[0][0];
                            context.Response.Write("" + count);
                        }
                        else
                        {
                            context.Response.Write("-1");
                        }
                    }
                    break;
                case "provinces":
                    {
                        string sql = string.Format("select provinceid,province from provinces");
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string code = "";
                            foreach (DataRow dr in dt.Rows)
                            {
                                code += string.Format("{0},{1}|", dr["provinceid"].ToString(), dr["province"].ToString());
                            }
                            if (code.Length > 0)
                            {
                                code = code.Substring(0, code.Length - 1);
                            }
                            context.Response.Write(code);
                        }
                        else
                        {
                            context.Response.Write("");
                        }
                    }
                    break;
                case "citys":
                    {
                        string proviceid = Helper.ReplaceString(context.Request["proviceid"]);
                        string sql = string.Format("select cityid,city from citys where parentid='{0}'", proviceid);
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string code = "";
                            foreach (DataRow dr in dt.Rows)
                            {
                                code += string.Format("{0},{1}|", dr["cityid"].ToString(), dr["city"].ToString());
                            }
                            if (code.Length > 0)
                            {
                                code = code.Substring(0, code.Length - 1);
                            }
                            context.Response.Write(code);
                        }
                        else
                        {
                            context.Response.Write("");
                        }
                    }
                    break;
                case "members":
                    {
                        string sql = string.Format("select nickname,mobile,openid from wx_members_wx");
                        DataTable dt = new BLL.bllBase().getDataTableBySql(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            List<KeyValue> lst = new List<KeyValue>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                lst.Add(new KeyValue() { key = dr["openid"].ToString(), value = string.Format("{0}({1})", dr["nickname"].ToString(), dr["mobile"].ToString()) });
                            }
                            JavaScriptSerializer s_serializer = new JavaScriptSerializer();
                            context.Response.Write(s_serializer.Serialize(lst));
                        }
                        else
                        {
                            context.Response.Write("");
                        }
                    }
                    break;
                case "existsgoodsid":
                    {
                        string goodsid = Helper.ReplaceString(context.Request["goodsid"]);
                        string sql = string.Format("select 1 from mv_goods where goodsid='{0}'", goodsid);
                        DataTable dt = new BLL.bllMVgoods().dal.DBHelper.ExecuteDataTable(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                    }
                    break;
                default:
                    context.Response.Write("");
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class KeyValue
        {
            public string key { get; set; }
            public string value { get; set; }
        }
    }
}