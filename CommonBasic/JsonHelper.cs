using LitJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml;

namespace CommunityBuy.CommonBasic
{
    /// <summary>
    /// 描述：应用程序缓存
    /// 作者：CGD
    /// 日期：2019-04-28 Add
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>  
        /// 将JSON解析成DataSet（只限标准的JSON数据）  
        /// 例如：Json＝{t1:[{name:'数据name',type:'数据type'}]} 或 Json＝{t1:[{name:'数据name',type:'数据type'}],t2:[{id:'数据id',gx:'数据gx',val:'数据val'}]}         /// </summary>  
        /// <param name="json">Json字符串</param>  
        /// <returns>DataSet</returns>  
        //public static DataSet JsonToDataSet(string json, out string code, out string mes, out string NP)  
        public static DataSet JsonToDataSet(string json, out string code, out string mes)
        {
            code = string.Empty;
            mes = string.Empty;
            var ds = new DataSet();
            try
            {
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                var datajson = (Dictionary<string, object>)obj;
                foreach (var item in datajson)
                {
                    bool Flag = false;
                    switch (item.Key.ToLower())
                    {
                        case "code":
                            code = item.Value.ToString();
                            Flag = true;
                            break;
                        case "status":
                            code = item.Value.ToString();
                            Flag = true;
                            break;
                        case "mes":
                            mes = item.Value.ToString();
                            Flag = true;
                            break;
                        case "msg":
                            mes = item.Value.ToString();
                            Flag = true;
                            break;
                        case "pagesize":
                            Flag = true;
                            break;
                        case "recordcount":
                            Flag = true;
                            break;
                        case "currentpage":
                            Flag = true;
                            break;
                        case "totalpage":
                            Flag = true;
                            break;
                    }
                    if (Flag)
                    {
                        continue;
                    }
                    var dt = new DataTable(item.Key);
                    var rows = (object[])item.Value;
                    foreach (object row in rows)
                    {
                        var val = (Dictionary<string, object>)row;
                        DataRow dr = dt.NewRow();
                        foreach (var sss in val)
                        {
                            if (!dt.Columns.Contains(sss.Key))
                            {
                                dt.Columns.Add(sss.Key, typeof(string));
                                dr[sss.Key] = sss.Value;
                            }
                            else
                                dr[sss.Key] = sss.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                    ds.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                return null;
            }
            return ds;
        }

        public static DataSet JsonToDataSet(string json, out string code, out string mes, out int pageSize, out int recordCount, out int currentPage, out int totalPage)
        {
            code = string.Empty;
            mes = string.Empty;
            pageSize = 0;
            recordCount = 0;
            currentPage = 0;
            totalPage = 0;
            try
            {
                var ds = new DataSet();
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                var datajson = (Dictionary<string, object>)obj;
                foreach (var item in datajson)
                {
                    bool Flag = false;
                    switch (item.Key.ToLower())
                    {
                        case "code":
                        case "status":
                            code = item.Value.ToString();
                            Flag = true;
                            break;
                        case "mes":
                            mes = item.Value.ToString();
                            Flag = true;
                            break;
                        case "limit":
                        case "pagesize":
                            pageSize = StringHelper.StringToInt(item.Value.ToString());
                            Flag = true;
                            break;
                        case "count":
                        case "recordcount":
                            recordCount = StringHelper.StringToInt(item.Value.ToString());
                            Flag = true;
                            break;
                        case "curpage":
                        case "currentpage":
                            currentPage = StringHelper.StringToInt(item.Value.ToString());
                            Flag = true;
                            break;
                        case "totpage":
                        case "totalpage":
                            totalPage = StringHelper.StringToInt(item.Value.ToString());
                            Flag = true;
                            break;
                        case "isnextpage":
                            Flag = true;
                            break;
                    }
                    if (Flag)
                    {
                        continue;
                    }
                    var dt = new DataTable(item.Key);
                    var rows = (object[])item.Value;
                    foreach (object row in rows)
                    {
                        var val = (Dictionary<string, object>)row;
                        DataRow dr = dt.NewRow();
                        foreach (var sss in val)
                        {
                            if (!dt.Columns.Contains(sss.Key))
                            {
                                dt.Columns.Add(sss.Key, typeof(string));
                            }
                            dr[sss.Key] = sss.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                    ds.Tables.Add(dt);
                }
                return ds;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 根据jason获取操作结果信息
        /// </summary>
        /// <param name="json"></param>
        /// <param name="code"></param>
        /// <param name="mes"></param>
        public static void JsonToMessage(string json, out string code, out string mes)
        {
            code = string.Empty;
            mes = string.Empty;
            try
            {
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                var datajson = (Dictionary<string, object>)obj;
                foreach (var item in datajson)
                {
                    bool Flag = false;
                    switch (item.Key.ToLower())
                    {
                        case "code":
                        case "status":
                            code = item.Value.ToString();
                            Flag = true;
                            break;
                        case "msg":
                        case "mes":
                            mes = item.Value.ToString();
                            Flag = true;
                            break;
                    }
                    if (Flag)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="code"></param>
        public static void JsonToStatus(string json, out string code)
        {
            code = string.Empty;
            try
            {
                var ds = new DataSet();
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                var datajson = (Dictionary<string, object>)obj;
                foreach (var item in datajson)
                {
                    bool Flag = false;
                    switch (item.Key.ToLower())
                    {
                        case "code":
                        case "status":
                            code = item.Value.ToString();
                            Flag = true;
                            break;
                    }
                    if (Flag)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
        }

        /// <summary>
        /// 转Json字符串 slw
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="mes">提示信息</param>
        /// <returns>返回：Json字符串</returns>
        public static string ToNewJson(string status, string mes)
        {
            return ToNewJson(status, mes, null, null, null, null, null, null);
        }

        /// <summary>
        /// 转Json字符串 slw
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="mes">提示信息</param>
        /// <param name="arrData">DataTable（DataRow）List对象</param>
        /// <param name="arrTBName">对应的Table名称</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="totalPage">总页数</param>
        /// <returns>返回：Json字符串</returns>
        public static string ToNewJson(string code, string mes, ArrayList arrData, string[] arrTBName, int? pageSize, long? recordCount, int? currentPage, int? totalPage)
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("{\"code\":\"" + code + "\",\"msg\":\"" + mes + "\"");
            if (arrData != null && arrTBName != null && arrData.Count == arrTBName.Length)
            {
                string T;
                for (int i = 0; i < arrData.Count; i++)
                {
                    if (arrData[i] == null) continue;
                    T = arrData[i].GetType().Name.ToLower();
                    switch (T)
                    {
                        case "datarow":
                            sbJson.Append(DataRowToString((DataRow)arrData[i], arrTBName[i]));
                            break;
                        case "datatable":
                            sbJson.Append(DataTableToString((DataTable)arrData[i], arrTBName[i]));
                            break;
                    }
                }
            }
            if (pageSize != null)
            {
                sbJson.Append(",\"limit\":" + pageSize.ToString());
            }
            if (recordCount != null)
            {
                sbJson.Append(",\"count\":" + recordCount.ToString());
            }
            if (currentPage != null)
            {
                sbJson.Append(",\"curpage\":" + currentPage.ToString());
            }
            if (totalPage != null)
            {
                sbJson.Append(",\"totpage\":" + totalPage.ToString());
            }
            sbJson.Append("}");
            return sbJson.ToString();
        }


        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                if (!dataTable.Columns.Contains(current))
                                {
                                    if (dictionary[current] != null)
                                    {
                                        if (dictionary[current].GetType().Name.ToString() == "Int32")
                                        {
                                            dataTable.Columns.Add(current, typeof(decimal));
                                        }
                                        else
                                        {
                                            dataTable.Columns.Add(current, dictionary[current].GetType());
                                        }

                                    }
                                    else
                                    {
                                        dataTable.Columns.Add(current, typeof(string));
                                    }
                                }
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            if (dictionary[current] == null)
                            {
                                switch (dataTable.Columns[current].DataType.Name.ToLower())
                                {
                                    case "decimal":
                                        dataRow[current] = 0;
                                        break;
                                    case "int32":
                                        dataRow[current] = 0;
                                        break;
                                    case "int64":
                                        dataRow[current] = 0;
                                        break;
                                    default:
                                        dataRow[current] = "";
                                        break;
                                }

                            }
                            else
                            {
                                dataRow[current] = dictionary[current];
                            }

                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, json);
            }
            result = dataTable;
            return result;
        }

        /// <summary>
        /// 转Json字符串
        /// </summary>
        /// <param name="code">状态</param>
        /// <param name="mes">提示信息</param>
        /// <returns>返回：Json字符串</returns>
        public static string ToJson(string code, string mes)
        {
            return ToJson(code, mes, null, null, null, null, null, null);
        }

        /// <summary>
        /// 转Json字符串
        /// </summary>
        /// <param name="code">状态</param>
        /// <param name="mes">提示信息</param>
        /// <param name="arrData">DataTable（DataRow）List对象</param>
        /// <param name="arrTBName">对应的Table名称</param>
        /// <returns>返回：Json字符串</returns>
        public static string ToJson(string code, string mes, ArrayList arrData, string[] arrTBName)
        {
            return ToJson(code, mes, arrData, arrTBName, null, null, null, null);
        }

        /// <summary>
        /// 转Json字符串
        /// </summary>
        /// <param name="code">状态</param>
        /// <param name="mes">提示信息</param>
        /// <param name="arrData">DataTable（DataRow）List对象</param>
        /// <param name="arrTBName">对应的Table名称</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="totalPage">总页数</param>
        /// <returns>返回：Json字符串</returns>
        public static string ToJson(string code, string mes, ArrayList arrData, string[] arrTBName, int? pageSize, long? recordCount, int? currentPage, int? totalPage)
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("{\"code\":\"" + code + "\",\"msg\":\"" + mes + "\"");
            if (arrData != null && arrTBName != null && arrData.Count == arrTBName.Length)
            {
                string T;
                for (int i = 0; i < arrData.Count; i++)
                {
                    if (arrData[i] == null) continue;
                    T = arrData[i].GetType().Name.ToLower();
                    switch (T)
                    {
                        case "datarow":
                            sbJson.Append(DataRowToString((DataRow)arrData[i], arrTBName[i]));
                            break;
                        case "datatable":
                            sbJson.Append(DataTableToString((DataTable)arrData[i], arrTBName[i]));
                            break;
                        case "int32":
                            sbJson.Append(",\"" + arrTBName[i] + "\":" + arrData[i]);
                            break;
                        default:
                            sbJson.Append(",\"" + arrTBName[i] + "\":\"" + arrData[i] + "\"");
                            break;
                    }
                }
            }
            if (pageSize != null)
            {
                sbJson.Append(",\"limit\":" + pageSize.ToString());
            }
            if (recordCount != null)
            {
                sbJson.Append(",\"count\":" + recordCount.ToString());
            }
            if (currentPage != null)
            {
                sbJson.Append(",\"curpage\":" + currentPage.ToString());
            }
            if (totalPage != null)
            {
                sbJson.Append(",\"totpage\":" + totalPage.ToString());
            }
            if (totalPage != null && currentPage != null)
            {
                if (currentPage >= totalPage)
                {
                    sbJson.Append(",\"isnextpage\":0");//不可分页
                }
                else
                {
                    sbJson.Append(",\"isnextpage\":1");//可分页
                }
            }
            sbJson.Append("}");
            return sbJson.ToString();
        }

        /// <summary>
        /// 待转换的DT
        /// </summary>
        /// <param name="dt">dt</param>
        /// <param name="TBName">表名称</param>
        /// <returns>Json字符串</returns>
        private static string DataTableToString(DataTable dt, string TBName)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            StringBuilder sbJson = new StringBuilder();
            if (dt != null)
            {
                if (TBName.Length > 0)
                {
                    sbJson.Append(",\"" + TBName + "\":");
                }
                else
                {
                    sbJson.Append(",\"data\":");
                }
                foreach (DataRow dataRow in dt.Rows)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    //实例化一个参数集合
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        if (dataColumn.DataType.Name.ToLower() == "datetime")
                        {
                            dictionary.Add(dataColumn.ColumnName + "str", StringHelper.StringToDateTime(dataRow[dataColumn.ColumnName].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                            if (dataRow[dataColumn.ColumnName] == null || dataRow[dataColumn.ColumnName].ToString().Length == 0)
                            {
                                dictionary.Add(dataColumn.ColumnName, StringHelper.StringToDateTime("1900-01-01"));
                            }
                            else
                            {
                                dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName]);
                            }
                        }
                        else
                        {
                            dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName]);
                        }
                    }
                    arrayList.Add(dictionary); //ArrayList集合中添加键值
                }
                sbJson.Append(javaScriptSerializer.Serialize(arrayList));  //返回一个json字符串
            }
            return sbJson.ToString();
        }

        /// <summary>
        /// 待转换的DR
        /// </summary>
        /// <param name="dr">dr</param>
        /// <param name="TBName">表名称</param>
        /// <returns>Json字符串</returns>
        private static string DataRowToString(DataRow dr, string TBName)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            StringBuilder sbJson = new StringBuilder();
            if (dr != null)
            {
                if (TBName.Length > 0)
                {
                    sbJson.Append(",\"" + TBName + "\":");
                }
                else
                {
                    sbJson.Append(",\"data\":");
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //实例化一个参数集合
                foreach (DataColumn dataColumn in dr.Table.Columns)
                {
                    switch (dataColumn.DataType.Name.ToLower())
                    {
                        case "datetime":
                            dictionary.Add(dataColumn.ColumnName + "str", dr[dataColumn.ColumnName].ToString());
                            if (dr[dataColumn.ColumnName] == null || dr[dataColumn.ColumnName].ToString().Length == 0)
                            {
                                dictionary.Add(dataColumn.ColumnName, StringHelper.StringToDateTime("1900-01-01"));
                            }
                            else
                            {
                                dictionary.Add(dataColumn.ColumnName, dr[dataColumn.ColumnName]);
                            }
                            break;
                        default:
                            dictionary.Add(dataColumn.ColumnName, dr[dataColumn.ColumnName]);
                            break;
                    }
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
                string Result = javaScriptSerializer.Serialize(arrayList);
                if (Result.Length >= 2)
                {
                    Result = Result.TrimStart('[').TrimEnd(']');
                }
                sbJson.Append(Result);  //返回一个json字符串
            }
            return sbJson.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJsonValByKey(string json, string key)
        {
            string Val = string.Empty;
            try
            {
                var ds = new DataSet();
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                if (obj != null)
                {
                    var datajson = (Dictionary<string, object>)obj;
                    foreach (var item in datajson)
                    {
                        if (item.Key.ToLower() == key.ToLower())
                        {
                            Val = item.Value.ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
            return Val;
        }

        /// <summary>
        /// 数据表转JSON
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>JSON字符串</returns>
        public static string DataTableToJSON(DataTable dt)
        {
            string jsonStr = ObjectToJSON(DataTableToList(dt));

            return jsonStr;
        }

        /// <summary>
        /// DataTable转List对象
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list
            = new List<Dictionary<string, object>>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                    }
                    list.Add(dic);
                }
            }
            return list;
        }

        /// <summary>
        /// Json对象转字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJSON(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                string str = jss.Serialize(obj);
                str = Regex.Replace(str, @"\\/Date\((\d+)\)\\/", match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                });
                if (str == "[]")
                {
                    str = string.Empty;
                }
                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }

        public static string ToErrorJson(string code, string mes)
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("{\"code\":\"" + code + "\",\"msg\":\"" + mes + "\"}");
            return sbJson.ToString();
        }

        /// <summary>
        /// 解析JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T AnalysisJson<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        /// <summary>
        /// 对象转Jason
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ObjectToJson<T>(T t)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(t.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, t);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            string strReturn = Encoding.UTF8.GetString(dataBytes);
            return strReturn;
        }

        /// <summary>
        /// 获取排序拼接字符
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonToOrderByString(string json)
        {
            if (json == null || json.Length <= 2)
            {
                return string.Empty;
            }
            StringBuilder strReturn = new StringBuilder();
            strReturn.Append("order by ");
            //json = "{'orders':[{'col':'cname','sort':'asc'},{'col':'sex','sort':'desc'}]}";
            DataTable dt = null;
            try
            {
                dt = JsonToDataTableWhere(json);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
            finally
            {
                //拼接SQL
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("col") && dt.Columns.Contains("sort"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            strReturn.Append(dr["col"].ToString() + " " + dr["sort"].ToString() + ",");
                        }
                    }
                }
            }
            if (strReturn.ToString().Length <= 9)
            {
                return string.Empty;
            }
            return strReturn.ToString().TrimEnd(',');
        }

        public static DataTable JsonToDataTable(string strJson)
        {
            string status = string.Empty;
            string mes = string.Empty;
            //string data = strJson;
            string data = string.Empty;
            JsonToMessageData(strJson, out status, out mes, out data);
            if (string.IsNullOrWhiteSpace(data) && string.IsNullOrWhiteSpace(status) && string.IsNullOrWhiteSpace(mes))
            {
                data = strJson;
            }
            else
            {
                data = "[" + data.TrimStart('[').TrimEnd(']') + "]";
            }
            return ToDataTable(data);

            //DataTable dt = null;
            //try
            //{
            //    JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);
            //    JArray ja = (JArray)jo["data"];
            //    dt = ToDataTable(ja.ToString());
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
            //return dt;
        }

        public static DataTable JsonToDataTableWhere(string strJson)
        {
            // 取出表名    
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = new DataTable();
            if (!strJson.Contains("["))
            {
                return null;
            }
            // 去除表名    
            strJson = strJson.Substring(strJson.IndexOf("["));
            strJson = strJson.Substring(0, strJson.LastIndexOf("]") + 1);

            DataTable dt = new DataTable();  //实例化
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(strJson);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        //if (dictionary.Keys.Count<string>() == 0)
                        if (dictionary.Keys.Count == 0)
                        {
                            return dt;
                        }
                        if (dt.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dt.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dt.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dt.Rows.Add(dataRow);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
            return dt;
        }

        /// <summary>
        /// 获取Where条件拼接字符
        /// </summary>
        /// <param name="json"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string JsonToFilterByString(string json, out DataTable dt)
        {
            dt = null;
            if (json == null || json.Length <= 2)
            {
                return string.Empty;
            }
            StringBuilder strReturn = new StringBuilder();
            strReturn.Append("where 1=1 ");
            //json = "{'filters':[{'col':'cname','filter':'猴子','exp':'=','cus':''},{'col':'sex','filter':'1','exp':'%'}]}";

            try
            {
                dt = JsonToDataTableWhere(json);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, json);
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
            finally
            {
                //拼接SQL
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("col") && dt.Columns.Contains("filter") && dt.Columns.Contains("exp"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string where = "";
                            switch (dr["exp"].ToString())
                            {
                                case "=":
                                case ">=":
                                case "<=":
                                case ">":
                                case "<>":
                                case "<":
                                    where = " and " + dr["col"].ToString() + " " + dr["exp"].ToString() + " '" + StringHelper.ReplaceString(dr["filter"].ToString()) + "'";
                                    break;
                                case "%":
                                    where = " and " + dr["col"].ToString() + " like '%" + StringHelper.ReplaceString(dr["filter"].ToString()) + "'";
                                    break;
                                case "%%":
                                    where = " and " + dr["col"].ToString() + " like '%" + StringHelper.ReplaceString(dr["filter"].ToString()) + "%'";
                                    break;
                            }
                            strReturn.Append(where);
                        }
                    }
                }
            }
            return strReturn.ToString();
        }

        /// <summary>
        /// 获取Where条件拼接字符(过滤 cus)
        /// </summary>
        /// <param name="json"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string JsonToFilterByString1(string json, out DataTable dt)
        {
            dt = null;
            if (json == null || json.Length <= 2)
            {
                return string.Empty;
            }
            StringBuilder strReturn = new StringBuilder();
            strReturn.Append("where 1=1 ");
            //json = "{'filters':[{'col':'cname','filter':'猴子','exp':'=','cus':''},{'col':'sex','filter':'1','exp':'%'}]}";

            try
            {
                dt = JsonToDataTableWhere(json);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, json);
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
            finally
            {
                //拼接SQL
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("col") && dt.Columns.Contains("filter") && dt.Columns.Contains("exp"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string where = "";
                            switch (dr["exp"].ToString())
                            {
                                case "=":
                                case ">=":
                                case "<=":
                                case ">":
                                case "<>":
                                case "<":
                                    if (string.IsNullOrEmpty(dr["cus"].ToString()))
                                    {
                                        where = " and " + dr["col"].ToString() + " " + dr["exp"].ToString() + " '" + StringHelper.ReplaceString(dr["filter"].ToString()) + "'";
                                    }
                                    break;
                                case "%":
                                    if (string.IsNullOrEmpty(dr["cus"].ToString()))
                                    {
                                        where = " and " + dr["col"].ToString() + " like '%" + StringHelper.ReplaceString(dr["filter"].ToString()) + "'";
                                    }
                                    break;
                                case "%%":
                                    if (string.IsNullOrEmpty(dr["cus"].ToString()))
                                    {
                                        where = " and " + dr["col"].ToString() + " like '%" + StringHelper.ReplaceString(dr["filter"].ToString()) + "%'";
                                    }
                                    break;
                            }
                            strReturn.Append(where);
                        }
                    }
                }
            }
            return strReturn.ToString();
        }

        /// <summary>
        /// 免登陆验证用
        /// </summary>
        /// <param name="json"></param>
        /// <param name="status"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public static DataSet NewJsonToDataSet(string json, out string status, out string mes)
        {
            status = string.Empty;
            mes = string.Empty;
            var ds = new DataSet();
            try
            {
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                var datajson = (Dictionary<string, object>)obj;
                foreach (var item in datajson)
                {
                    bool Flag = false;
                    switch (item.Key.ToLower())
                    {
                        case "code":
                        case "status":
                            status = item.Value.ToString();
                            Flag = true;
                            break;
                        case "msg":
                            mes = item.Value.ToString();
                            Flag = true;
                            break;
                        case "pagesize":
                            Flag = true;
                            break;
                        case "recordcount":
                            Flag = true;
                            break;
                        case "currentpage":
                            Flag = true;
                            break;
                        case "totalpage":
                            Flag = true;
                            break;
                        case "isnextpage":
                            Flag = true;
                            break;
                    }
                    if (Flag)
                    {
                        continue;
                    }
                    var dt = new DataTable(item.Key);
                    var rows = (object[])item.Value;
                    foreach (object row in rows)
                    {
                        var val = (Dictionary<string, object>)row;
                        DataRow dr = dt.NewRow();
                        foreach (var sss in val)
                        {
                            if (!dt.Columns.Contains(sss.Key))
                            {
                                dt.Columns.Add(sss.Key, typeof(string));
                                dr[sss.Key] = sss.Value;
                            }
                            else
                                dr[sss.Key] = sss.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                    ds.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return ds;
        }

        /// <summary>
        /// 序列化json  返回数据data
        /// </summary>
        /// <param name="json"></param>
        /// <param name="status"></param>
        /// <param name="mes"></param>
        /// <param name="data"></param>
        public static void JsonToMessageData(string json, out string status, out string mes, out string data)
        {
            status = string.Empty;
            mes = string.Empty;
            data = "";
            try
            {
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                object dataobj = obj;
                if (obj.GetType().IsArray)
                {
                    dataobj = ((Array)obj).GetValue(0);
                }
                var datajson = (Dictionary<string, object>)dataobj;
                foreach (var item in datajson)
                {
                    bool Flag = false;
                    switch (item.Key.ToLower())
                    {
                        case "status":
                            status = item.Value.ToString();
                            Flag = true;
                            break;
                        case "mes":
                            mes = item.Value.ToString();
                            Flag = true;
                            break;
                        case "data":
                            data = JsonMapper.ToJson(item.Value).TrimStart('[').TrimEnd(']');
                            Flag = true;
                            break;
                    }
                    if (Flag)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, json);
                //ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
            }
        }

        /// <summary>
        /// 获取连锁云分页数据后需要本地重组后分页再json化使用
        /// </summary>
        /// <param name="json"></param>
        /// <param name="status"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public static DataSet NewLinJsonToDataSet(string json, out string status, out string mes, out string limit, out string count, out string curpage, out string totpage)
        {
            status = string.Empty;
            mes = string.Empty;
            limit = string.Empty;
            count = string.Empty;
            curpage = string.Empty;
            totpage = string.Empty;
            var ds = new DataSet();
            try
            {
                var jss = new JavaScriptSerializer();
                object obj = jss.DeserializeObject(json);
                var datajson = (Dictionary<string, object>)obj;
                foreach (var item in datajson)
                {
                    bool Flag = false;
                    switch (item.Key.ToLower())
                    {
                        case "code":
                        case "status":
                            status = item.Value.ToString();
                            Flag = true;
                            break;
                        case "msg":
                            mes = item.Value.ToString();
                            Flag = true;
                            break;
                        case "limit":
                        case "pagesize":
                            limit = item.Value.ToString();
                            Flag = true;
                            break;
                        case "count":
                        case "recordcount":
                            count = item.Value.ToString();
                            Flag = true;
                            break;
                        case "curpage":
                        case "currentpage":
                            curpage = item.Value.ToString();
                            Flag = true;
                            break;
                        case "totpage":
                        case "totalpage":
                            totpage = item.Value.ToString();
                            Flag = true;
                            break;
                        case "isnextpage":
                            Flag = true;
                            break;
                    }
                    if (Flag)
                    {
                        continue;
                    }
                    var dt = new DataTable(item.Key);
                    var rows = (object[])item.Value;
                    foreach (object row in rows)
                    {
                        var val = (Dictionary<string, object>)row;
                        DataRow dr = dt.NewRow();
                        foreach (var sss in val)
                        {
                            if (!dt.Columns.Contains(sss.Key))
                            {
                                dt.Columns.Add(sss.Key, typeof(string));
                                dr[sss.Key] = sss.Value;
                            }
                            else
                                dr[sss.Key] = sss.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                    ds.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return ds;
        }

        /// <summary>  
        /// json字符串转换为Xml对象  
        /// </summary>  
        /// <param name="sJson"></param>  
        /// <returns></returns>  
        public static XmlDocument JsonToXml(string sJson)
        {
            var serializer = new JavaScriptSerializer();
            var dic = (Dictionary<string, object>)serializer.DeserializeObject(sJson);
            var doc = new XmlDocument();
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");
            doc.InsertBefore(xmlDec, doc.DocumentElement);
            XmlElement root = doc.CreateElement("root");
            doc.AppendChild(root);
            foreach (var item in dic)
            {
                XmlElement element = doc.CreateElement(item.Key);
                KeyValue2Xml(element, item);
                root.AppendChild(element);
            }
            return doc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="source"></param>
        private static void KeyValue2Xml(XmlElement node, KeyValuePair<string, object> source)
        {
            object kValue = source.Value;
            if (kValue.GetType() == typeof(Dictionary<string, object>))
            {
                var dictionary = kValue as Dictionary<string, object>;
                if (dictionary != null)
                    foreach (var item in dictionary)
                    {
                        if (node.OwnerDocument != null)
                        {
                            XmlElement element = node.OwnerDocument.CreateElement(item.Key);
                            KeyValue2Xml(element, item);
                            node.AppendChild(element);
                        }
                    }
            }
            else if (kValue.GetType() == typeof(object[]))
            {
                var o = kValue as object[];
                if (o != null)
                    foreach (object t in o)
                    {
                        if (node.OwnerDocument != null)
                        {
                            XmlElement xitem = node.OwnerDocument.CreateElement("Item");
                            var item = new KeyValuePair<string, object>("Item", t);
                            KeyValue2Xml(xitem, item);
                            node.AppendChild(xitem);
                        }
                    }
            }
            else
            {
                if (node.OwnerDocument != null)
                {
                    XmlText text = node.OwnerDocument.CreateTextNode(kValue.ToString());
                    node.AppendChild(text);
                }
            }
        }

    }
}
