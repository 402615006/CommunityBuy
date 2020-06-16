/********************************************************************************
** 描述： 实体初始化公共类
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.CommonBasic
{
    public class EntityHelper
    {

        /// <summary>
        /// 根据数据表生成相应的实体对象列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="srcDT">数据</param>
        /// <param name="relation">数据库表列名与对象属性名对应关系；如果列名与实体对象属性名相同，该参数可为空</param>
        /// <returns>对象列表</returns>
        public static List<T> GetEntityListByDT<T>(DataTable srcDT, Hashtable relation)
        {
            List<T> list = new List<T>();
            T destObj = default(T);

            if (srcDT != null && srcDT.Rows.Count > 0)
            {

                list = new List<T>();
                foreach (DataRow row in srcDT.Rows)
                {
                    destObj = GetEntityListByDT<T>(row, relation);
                    list.Add(destObj);
                }
            }

            return list;
        }

        public static List<T> GetEntityListByDR<T>(DataRow[] drows, Hashtable relation)
        {
            List<T> list = new List<T>();
            T destObj = default(T);

            if (drows != null && drows.Length > 0)
            {
                list = new List<T>();
                foreach (DataRow row in drows)
                {
                    destObj = GetEntityListByDT<T>(row, relation);
                    list.Add(destObj);
                }
            }

            return list;
        }

        public static List<T> GetEntityListByDRs<T>(DataRow drows, Hashtable relation)
        {
            List<T> list = new List<T>();
            T destObj = default(T);

            if (drows != null)
            {
                list = new List<T>();
                destObj = GetEntityListByDT<T>(drows, relation);
                list.Add(destObj);
            }
            return list;
        }

        public static T GetEntityByDR<T>(DataRow drow, Hashtable relation)
        {
            List<T> list = new List<T>();
            T destObj = default(T);
            destObj = GetEntityListByDT<T>(drow, relation);
            return destObj;
        }


        public static List<T> GetEntityListByJson<T>(string EntityJson)
        {
            List<T> list = null;
            try
            {
                JArray jarray = (JArray)JsonConvert.DeserializeObject(EntityJson);
                if (jarray != null && jarray.Count > 0)
                {
                    list = new List<T>();
                    for (int i = 0; i < jarray.Count; i++)
                    {
                        Type type = typeof(T);
                        T destObj = Activator.CreateInstance<T>();
                        PropertyInfo temp = null;
                        foreach (PropertyInfo prop in type.GetProperties())
                        {
                            if (jarray[i][prop.Name] != null)
                            {
                                SetPropertyValue(prop, destObj, jarray[i][prop.Name].ToString());
                            }
                        }

                        list.Add(destObj);
                    }
                }
            }
            catch(Exception ex) 
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
            }
            return list;
        }



        public static T GetEntityListByDT<T>(DataRow row, Hashtable relation)
        {
            Type type = typeof(T);
            T destObj = Activator.CreateInstance<T>();
            PropertyInfo temp = null;
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (row.Table.Columns.Contains(prop.Name) &&
                    row[prop.Name] != DBNull.Value)
                {
                    SetPropertyValue(prop, destObj, row[prop.Name]);
                }
            }

            if (relation != null)
            {
                foreach (string name in relation.Keys)
                {
                    temp = type.GetProperty(relation[name].ToString());
                    if (temp != null &&
                        row[name] != DBNull.Value)
                    {
                        SetPropertyValue(temp, destObj, row[name]);
                    }
                }
            }

            return destObj;
        }
        /// <summary>
        /// 为对象的属性赋值
        /// </summary>
        /// <param name="prop">属性</param>
        /// <param name="destObj">目标对象</param>
        /// <param name="value">源值</param>
        private static void SetPropertyValue(PropertyInfo prop, object destObj, object value)
        {
            try
            {
                object temp = ChangeType(prop.PropertyType, value);
                prop.SetValue(destObj, temp, null);
            }
            catch { }
        }

        /// <summary>
        /// 用于类型数据的赋值
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="value">原值</param>
        /// <returns></returns>
        private static object ChangeType(Type type, object value)
        {
            int temp = 0;
            if ((value == null) && type.IsGenericType)
            {
                return Activator.CreateInstance(type);
            }
            if (value == null)
            {
                return null;
            }
            if (type == value.GetType())
            {
                return value;
            }
            if (type.IsEnum)
            {
                if (value is string)
                {
                    return Enum.Parse(type, value as string);
                }
                return Enum.ToObject(type, value);
            }

            if (type == typeof(bool) && typeof(int).IsInstanceOfType(value))
            {
                temp = int.Parse(value.ToString());
                return temp != 0;
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type type1 = type.GetGenericArguments()[0];
                object obj1 = ChangeType(type1, value);
                return Activator.CreateInstance(type, new object[] { obj1 });
            }
            if ((value is string) && (type == typeof(Guid)))
            {
                return new Guid(value as string);
            }
            if ((value is string) && (type == typeof(Version)))
            {
                return new Version(value as string);
            }


            //tsg 2014-12-03 
            //Byte[] 转换为 Image
            if (type.Name == "Image" && value.GetType().Name == "Byte[]")
            {
                Byte[] Bytes = (Byte[])value;
                MemoryStream ms = new MemoryStream(Bytes, 0, Bytes.Length);
                BinaryFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(ms);
                ms.Close();
                return (Image)obj;
            }
            //Image 转换为 Byte[]
            if (type.Name == "Byte[]" && value.GetType().Name == "Image")
            {
                Image im = (Image)value;
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, (object)im);
                ms.Close();
                return ms.ToArray();
            }



            if (!(value is IConvertible))
            {
                return value;
            }
            return Convert.ChangeType(value, type);
        }




        #region 根据表结构和实体类，构造新增和修改的语句

        public enum eSqlType
        {

            /// <summary>
            /// 新增
            /// </summary>
            [EnumAttribute(Name = "新增")]
            insert,
            /// <summary>
            /// 更新
            /// </summary>
            [EnumAttribute(Name = "更新")]
            update
        }

        /// <summary>
        /// 根据表结构和实体类，构造新增和修改的语句
        /// </summary>
        /// <typeparam name="T">实体类名称</typeparam>
        /// <param name="srcDT">表结构</param>
        /// <param name="objT">实体</param>
        /// <param name="SqlType">返回sql类型（0新增，1修改）</param>
        /// <returns></returns>
        public static string GenerateSqlByDE<T>(DataTable srcDT, T objT, List<string> ExcludeFileds, Dictionary<string, string> dicAttachFieldValue, eSqlType SqlType)
        {
            string strSql = string.Empty;
            string strFields = string.Empty;
            string strValues = string.Empty;
            string strUpdateSql = string.Empty;

            T destObj = default(T);

            if (srcDT != null && srcDT.Columns != null && srcDT.Columns.Count > 0)
            {
                strUpdateSql = GetSqlByDE<T>(srcDT, objT, ref strFields, ref strValues, ExcludeFileds);
            }

            if (!string.IsNullOrEmpty(strFields))
            {
                strFields = strFields.TrimStart(',');
            }
            if (!string.IsNullOrEmpty(strValues))
            {
                strValues = strValues.TrimStart(',');
            }
            if (!string.IsNullOrEmpty(strUpdateSql))
            {
                strUpdateSql = strUpdateSql.TrimStart(',');
            }

            if (dicAttachFieldValue != null && dicAttachFieldValue.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in dicAttachFieldValue)
                {
                    #region
                    if (!string.IsNullOrEmpty(strFields))
                    {
                        strFields += "," + kv.Key;
                    }
                    else
                    {
                        strFields = kv.Key;
                    }
                    #endregion
                    //------------------------------
                    #region
                    if (!string.IsNullOrEmpty(strValues))
                    {
                        strValues += "," + kv.Value.ToString();
                    }
                    else
                    {
                        strValues = kv.Value.ToString();
                    }
                    #endregion
                    //------------------------------
                    #region
                    if (!string.IsNullOrEmpty(strUpdateSql))
                    {
                        strUpdateSql += "," + kv.Key + "=" + kv.Value.ToString();
                    }
                    else
                    {
                        strUpdateSql = kv.Key + "=" + kv.Value.ToString();
                    }
                    #endregion
                    //------------------------------
                }
            }

            if (SqlType == eSqlType.insert)
            {
                strSql = " ( " + strFields + " ) values ( " + strValues + " ) ";
            }
            else
            {
                strSql = strUpdateSql;
            }

            return strSql;
        }

        public static string GetSqlByDE<T>(DataTable srcDT, T destObj, ref string Fields, ref string Values, List<string> ExcludeFileds)
        {
            string strUpdateSql = string.Empty;

            Type type = typeof(T);
            PropertyInfo temp = null;
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (srcDT.Columns.Contains(prop.Name))
                {
                    if (ExcludeFileds.Contains(prop.Name)) continue;

                    Fields += ",[" + prop.Name + "]"; //字段名

                    Type objType = srcDT.Columns[prop.Name].DataType;
                    object objValue = prop.GetValue(destObj, null);

                    #region 根据数据类型构造sql语句
                    string DataTypeName = objType.Name.ToLower();
                    switch (DataTypeName)
                    {
                        case "smalldatetime":
                            #region
                            if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                            {
                                DateTime dtime = Convert.ToDateTime(objValue.ToString());
                                if (dtime != DateTime.MinValue && dtime != DateTime.MaxValue)
                                {
                                    Values += string.Format(",'{0}'", Convert.ToDateTime(objValue).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''"));
                                    strUpdateSql += string.Format(",{0}='{1}'", prop.Name, Convert.ToDateTime(objValue).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''"));
                                }
                                else
                                {
                                    Values += ",'1900-01-01'";
                                    strUpdateSql += string.Format(",{0}='1900-01-01'", prop.Name);
                                }
                            }
                            else
                            {
                                Values += ",'1900-01-01'";
                                strUpdateSql += string.Format(",{0}='1900-01-01'", prop.Name);
                            }
                            #endregion
                            break;
                        case "datetime":
                            #region
                            if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                            {
                                DateTime dtime = Convert.ToDateTime(objValue.ToString());
                                if (dtime != DateTime.MinValue && dtime != DateTime.MaxValue)
                                {
                                    Values += string.Format(",'{0}'", Convert.ToDateTime(objValue).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''"));
                                    strUpdateSql += string.Format(",{0}='{1}'", prop.Name, Convert.ToDateTime(objValue).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''"));
                                }
                                else
                                {
                                    Values += ",'1900-01-01'";
                                    strUpdateSql += string.Format(",{0}='1900-01-01'", prop.Name);
                                }
                            }
                            else
                            {
                                Values += ",'1900-01-01'";
                                strUpdateSql += string.Format(",{0}='1900-01-01'", prop.Name);
                            }
                            #endregion
                            break;
                        case "string":
                            #region
                            if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                            {
                                Values += string.Format(",'{0}'", objValue.ToString().Replace("'", "''"));
                                strUpdateSql += string.Format(",{0}='{1}'", prop.Name, objValue.ToString().Replace("'", "''"));
                            }
                            else
                            {
                                Values += ",''";
                                strUpdateSql += string.Format(",{0}=''", prop.Name);
                            }
                            #endregion
                            break;
                        case "decimal":
                            //有28位小数的高度精度浮点数
                            #region
                            if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                            {
                                Values += string.Format(",{0}", objValue.ToString());
                                strUpdateSql += string.Format(",{0}={1}", prop.Name, objValue.ToString());
                            }
                            else
                            {
                                Values += ",0";
                                strUpdateSql += string.Format(",{0}=0", prop.Name);
                            }
                            #endregion
                            break;
                        case "bool":
                            //true或false
                            #region
                            if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                            {
                                string bValue = "0";
                                if (objValue.ToString().Trim().ToLower() == "true" || objValue.ToString().Trim().ToLower() == "1")
                                {
                                    bValue = "1";
                                }
                                Values += string.Format(",'{0}'", bValue);
                                strUpdateSql += string.Format(",{0}='{1}'", prop.Name, bValue);
                            }
                            else
                            {
                                Values += ",'0'";
                                strUpdateSql += string.Format(",{0}='0'", prop.Name);
                            }
                            #endregion
                            break;
                        case "char":
                            //16位Unicode字符
                            #region
                            if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                            {
                                Values += string.Format(",'{0}'", objValue.ToString().Replace("'", "''"));
                                strUpdateSql += string.Format(",{0}='{1}'", prop.Name, objValue.ToString().Replace("'", "''"));
                            }
                            else
                            {
                                Values += ",''";
                                strUpdateSql += string.Format(",{0}=''", prop.Name);
                            }
                            #endregion
                            break;
                        case "byte":
                        case "sbyte":
                        //8位有符号整型
                        case "short":
                        //16位有符号整型
                        case "int":
                        //32位有符号整型
                        case "int16":
                        //32位有符号整型
                        case "int32":
                        case "int64":
                        //64位有符号整型
                        case "long":
                        //long位有符号整型
                        case "ushort":
                        //16位无符号整型
                        case "uint":
                        //32位无符号整型
                        case "ulong":
                        //64位无符号整型
                        case "single":
                        //(float)单精度浮点类型
                        case "double":
                            //双精度浮点类型
                            #region
                            if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                            {
                                Values += string.Format(",{0}", objValue.ToString());
                                strUpdateSql += string.Format(",{0}={1}", prop.Name, objValue.ToString());
                            }
                            else
                            {
                                Values += ",0";
                                strUpdateSql += string.Format(",{0}=0", prop.Name);
                            }
                            #endregion
                            break;
                        default:
                            /*
                            #region 
                            if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                            {
                                Values += string.Format(",'{0}'", objValue.ToString().Replace("'", "''"));
                                strUpdateSql += string.Format(",{0}='{1}'",prop.Name,objValue.ToString().Replace("'","''"));
                            }
                            else
                            {
                                Values += ",''";
                                strUpdateSql += string.Format(",{0}=''", prop.Name);
                            }
                            #endregion
                            */

                            string st = "";

                            break;
                    }

                    #endregion

                }
            }

            return strUpdateSql;
        }

        /// <summary>
        /// 根据表行数据构造插入或更新语句
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ExcludeFileds"></param>
        /// <param name="dicAttachFieldValue"></param>
        /// <param name="SqlType"></param>
        /// <returns></returns>
        public static string GenerateSqlByDataRow(DataRow dr, List<string> ExcludeFileds, Dictionary<string, string> dicAttachFieldValue, eSqlType SqlType)
        {
            string strSql = string.Empty;

            string strFields = string.Empty;
            string strValues = string.Empty;
            string strUpdateSql = string.Empty;

            if (dr != null && dr.Table.Columns != null && dr.Table.Columns.Count > 0)
            {
                strUpdateSql = GetSqlByDataRow(dr, ref strFields, ref strValues, ExcludeFileds);
            }

            if (!string.IsNullOrEmpty(strFields))
            {
                strFields = strFields.TrimStart(',');
            }
            if (!string.IsNullOrEmpty(strValues))
            {
                strValues = strValues.TrimStart(',');
            }
            if (!string.IsNullOrEmpty(strUpdateSql))
            {
                strUpdateSql = strUpdateSql.TrimStart(',');
            }

            if (dicAttachFieldValue != null && dicAttachFieldValue.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in dicAttachFieldValue)
                {
                    #region
                    if (!string.IsNullOrEmpty(strFields))
                    {
                        strFields += "," + kv.Key;
                    }
                    else
                    {
                        strFields = kv.Key;
                    }
                    #endregion
                    //------------------------------
                    #region
                    if (!string.IsNullOrEmpty(strValues))
                    {
                        strValues += "," + kv.Value.ToString();
                    }
                    else
                    {
                        strValues = kv.Value.ToString();
                    }
                    #endregion
                    //------------------------------
                    #region
                    if (!string.IsNullOrEmpty(strUpdateSql))
                    {
                        strUpdateSql += "," + kv.Key + "=" + kv.Value.ToString();
                    }
                    else
                    {
                        strUpdateSql = kv.Key + "=" + kv.Value.ToString();
                    }
                    #endregion
                    //------------------------------
                }
            }

            if (SqlType == eSqlType.insert)
            {
                strSql = " ( " + strFields + " ) values ( " + strValues + " ) ";
            }
            else
            {
                strSql = strUpdateSql;
            }

            return strSql;
        }

        public static string GetSqlByDataRow(DataRow dr, ref string Fields, ref string Values, List<string> ExcludeFileds)
        {
            string strUpdateSql = string.Empty;
            string strColName=string.Empty;
            if (dr == null || dr.Table.Columns.Count == 0) return string.Empty;

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                strColName=dr.Table.Columns[i].ColumnName;
                if (ExcludeFileds.Contains(strColName) ) continue;

                Fields += ",[" + strColName + "]"; //字段名
                object objValue = dr[strColName];

                switch (dr.Table.Columns[i].DataType.Name.ToLower())
                { 
                    case "smalldatetime":
                        #region
                        if (dr[strColName] != DBNull.Value && !string.IsNullOrEmpty(dr[strColName].ToString()))
                        {
                            DateTime dtime = Convert.ToDateTime(objValue.ToString());
                            if (dtime != DateTime.MinValue && dtime != DateTime.MaxValue)
                            {
                                Values += string.Format(",'{0}'", Convert.ToDateTime(objValue).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''"));
                                strUpdateSql += string.Format(",{0}='{1}'", strColName, Convert.ToDateTime(objValue).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''"));
                            }
                            else
                            {
                                Values += ",'1900-01-01'";
                                strUpdateSql += string.Format(",{0}='1900-01-01'", strColName);
                            }
                        }
                        else
                        {
                            Values += ",'1900-01-01'";
                            strUpdateSql += string.Format(",{0}='1900-01-01'", strColName);
                        }
                        #endregion
                        break;
                    case "datetime":
                        #region
                        if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                        {
                            DateTime dtime = Convert.ToDateTime(objValue.ToString());
                            if (dtime != DateTime.MinValue && dtime != DateTime.MaxValue)
                            {
                                Values += string.Format(",'{0}'", Convert.ToDateTime(objValue).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''"));
                                strUpdateSql += string.Format(",{0}='{1}'", strColName, Convert.ToDateTime(objValue).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''"));
                            }
                            else
                            {
                                Values += ",'1900-01-01'";
                                strUpdateSql += string.Format(",{0}='1900-01-01'", strColName);
                            }
                        }
                        else
                        {
                            Values += ",'1900-01-01'";
                            strUpdateSql += string.Format(",{0}='1900-01-01'", strColName);
                        }
                        #endregion
                        break;
                    case "string":
                        #region
                        if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                        {
                            Values += string.Format(",'{0}'", objValue.ToString().Replace("'", "''"));
                            strUpdateSql += string.Format(",{0}='{1}'", strColName, objValue.ToString().Replace("'", "''"));
                        }
                        else
                        {
                            Values += ",''";
                            strUpdateSql += string.Format(",{0}=''", strColName);
                        }
                        #endregion
                        break;
                    case "decimal":
                        //有28位小数的高度精度浮点数
                        #region
                        if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                        {
                            Values += string.Format(",{0}", objValue.ToString());
                            strUpdateSql += string.Format(",{0}={1}", strColName, objValue.ToString());
                        }
                        else
                        {
                            Values += ",0";
                            strUpdateSql += string.Format(",{0}=0", strColName);
                        }
                        #endregion
                        break;
                    case "bool":
                        //true或false
                        #region
                        if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                        {
                            string bValue = "0";
                            if (objValue.ToString().Trim().ToLower() == "true" || objValue.ToString().Trim().ToLower() == "1")
                            {
                                bValue = "1";
                            }
                            Values += string.Format(",'{0}'", bValue);
                            strUpdateSql += string.Format(",{0}='{1}'", strColName, bValue);
                        }
                        else
                        {
                            Values += ",'0'";
                            strUpdateSql += string.Format(",{0}='0'", strColName);
                        }
                        #endregion
                        break;
                    case "char":
                        //16位Unicode字符
                        #region
                        if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                        {
                            Values += string.Format(",'{0}'", objValue.ToString().Replace("'", "''"));
                            strUpdateSql += string.Format(",{0}='{1}'", strColName, objValue.ToString().Replace("'", "''"));
                        }
                        else
                        {
                            Values += ",''";
                            strUpdateSql += string.Format(",{0}=''", strColName);
                        }
                        #endregion
                        break;
                    case "byte":
                    case "sbyte":
                    //8位有符号整型
                    case "short":
                    //16位有符号整型
                    case "int":
                    //32位有符号整型
                    case "int16":
                    //32位有符号整型
                    case "int32":
                    case "int64":
                    //64位有符号整型
                    case "long":
                    //long位有符号整型
                    case "ushort":
                    //16位无符号整型
                    case "uint":
                    //32位无符号整型
                    case "ulong":
                    //64位无符号整型
                    case "single":
                    //(float)单精度浮点类型
                    case "double":
                        //双精度浮点类型
                        #region
                        if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                        {
                            Values += string.Format(",{0}", objValue.ToString());
                            strUpdateSql += string.Format(",{0}={1}", strColName, objValue.ToString());
                        }
                        else
                        {
                            Values += ",0";
                            strUpdateSql += string.Format(",{0}=0", strColName);
                        }
                        #endregion
                        break;
                    default:
                        /*
                        #region 
                        if (objValue != null && !string.IsNullOrEmpty(objValue.ToString()))
                        {
                            Values += string.Format(",'{0}'", objValue.ToString().Replace("'", "''"));
                            strUpdateSql += string.Format(",{0}='{1}'",prop.Name,objValue.ToString().Replace("'","''"));
                        }
                        else
                        {
                            Values += ",''";
                            strUpdateSql += string.Format(",{0}=''", prop.Name);
                        }
                        #endregion
                        */

                        string st = "";

                        break;
                }
            }


            return strUpdateSql;
        }


        /// <summary>
        /// 根据表结构生成sql字段字符串
        /// </summary>
        /// <param name="srcDT"></param>
        /// <param name="ExcludeFileds"></param>
        /// <param name="dicAttachFieldValue"></param>
        /// <returns></returns>
        public static string GenerateFieldsSql(DataTable srcDT, List<string> ExcludeFileds, Dictionary<string, string> dicAttachFieldValue)
        {
            string strFields = string.Empty;
            StringBuilder sbFields = new StringBuilder();
            if (srcDT == null || srcDT.Columns.Count == 0) return string.Empty;
            for (int i = 0; i < srcDT.Columns.Count; i++)
            {
                if (ExcludeFileds.Contains(srcDT.Columns[i].ColumnName)) continue;

                if (dicAttachFieldValue.ContainsKey(srcDT.Columns[i].ColumnName))
                { 
                    sbFields.Append(","+dicAttachFieldValue[srcDT.Columns[i].ColumnName]+" as "+ srcDT.Columns[i].ColumnName) ;
                }
                else
                {
                    sbFields.Append(","+srcDT.Columns[i].ColumnName);
                }

            }
            if (sbFields.Length > 0)
            {
                strFields = sbFields.ToString();
                strFields = strFields.TrimStart(',');
            }

            return strFields;
        }

        #endregion

        /// <summary>
        /// 根据对象数组获取json
        /// </summary>
        /// <param name="arrObj"></param>
        /// <returns></returns>
        public static string getJsonByObjArr(Dictionary<string, object> dicPar, string key)
        {
            object[] arrObj = ((object[])(dicPar[key]));
            string MethodsMatesJson = "[";// dicPar["MethodsMatesJson"].ToString();//做法使用的原料
            for (int i = 0; i < arrObj.Length; i++)
            {
                Dictionary<string, object> obj = ((Dictionary<string, object>)(arrObj[0]));
                MethodsMatesJson += "{";
                string vals = string.Empty;
                foreach (var str in obj)
                {
                    vals += "\"" + str.Key + "\"" + ":" + "\"" + str.Value + "\"" + ",";
                }
                MethodsMatesJson += vals.TrimEnd(',');
                MethodsMatesJson += "}";
            }
            MethodsMatesJson += "]";

            return MethodsMatesJson;
        }

    }
}
