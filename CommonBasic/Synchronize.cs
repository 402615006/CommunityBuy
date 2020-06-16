using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityBuy.CommonBasic
{
    public class Synchronize
    {
        /// <summary>
        /// 解析sql并插入
        /// </summary>
        /// <param name="notanalysisSql">未解析的sql</param>
        /// <param name="connectionString">解析出来的sql需要插入的连接</param>
        public static int AnalysisSql(string notanalysisSql, string connectionString)
        {
            MSSqlDataAccess DBHelper = new MSSqlDataAccess(connectionString);
            string status = string.Empty; //状态
            string mes = string.Empty; //消息
            string data = string.Empty;
            var result = 0;
            //接口返回成功
            if (notanalysisSql != null && notanalysisSql.Length > 0)
            {
                notanalysisSql = notanalysisSql.Replace("\"", "");
                notanalysisSql = notanalysisSql.TrimStart('{').TrimEnd('}');
                string[] stringSeparators = new string[] { "," };
                string[] result_arr = notanalysisSql.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 0; i < result_arr.Length; i++)
                {
                    string[] result_array = result_arr[i].Split(':');
                    if (result_array.Length == 2)
                    {
                        switch (result_array[0])
                        {
                            case "status":
                                status = result_array[1];
                                break;
                            case "mes":
                                mes = result_array[1];
                                break;
                            case "data":
                                if (status == "0")
                                {
                                    //sql json串
                                    string resultArr = result_array[1];
                                    resultArr = resultArr.TrimStart('[').TrimEnd(']');
                                    string[] res_Arr = resultArr.Split(';');
                                    if (res_Arr.Length > 0)
                                    {
                                        StringBuilder sbStr = new StringBuilder();
                                        string DecryptStr = String.Empty;//解密字符串
                                        for (int j = 0; j < res_Arr.Length; j++)
                                        {
                                            string[] res_Array = res_Arr[j].Split('*');
                                            switch (res_Array[0].ToLower())
                                            {
                                                case "stockmaterial":
                                                case "stockmateunits":
                                                    DecryptStr = OEncryp.DecryptLCL(res_Array[1].TrimEnd(']'));
                                                    sbStr.AppendLine(DecryptStr);
                                                    break;
                                                default:
                                                    //解密后累加
                                                    DecryptStr = OEncryp.DecryptLCL(res_Array[1]);
                                                    sbStr.AppendLine(DecryptStr);
                                                    break;
                                            }
                                        }
                                        //有未提交的提交
                                        if (sbStr.ToString().Length > 0)
                                        {
                                            var tempStr = "";
                                            tempStr = " BEGIN TRAN tan1 declare @userid bigint set @userid=0; " + sbStr.ToString() + " if(@@error=0) begin commit tran tan1 end else begin rollback tran tran1 end";
                                            //ErrorLog.WriteUploadErrorMessage(tempStr);
                                            result = DBHelper.ExecuteNonQuery(tempStr);
                                            DBHelper.ExecuteNonQuery("update sto_admins set status='0' where empcode  in(select ecode from Employee where [status]='0') ");
                                            sbStr.Clear();
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        result = -1;
                        status = "-1";
                        mes = "解析失败";
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="notanalysisSql"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int AnalysisSqlmodify(string notanalysisSql, string connectionString)
        {
            MSSqlDataAccess DBHelper = new MSSqlDataAccess(connectionString);
            string status = string.Empty; //状态
            string mes = string.Empty; //消息
            string data = string.Empty;
            var result = 0;
            //接口返回成功
            if (notanalysisSql != null && notanalysisSql.Length > 0)
            {
                notanalysisSql = notanalysisSql.Replace("\"", "");
                notanalysisSql = notanalysisSql.TrimStart('{').TrimEnd('}');
                string[] stringSeparators = new string[] { "," };
                string[] result_arr = notanalysisSql.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 0; i < result_arr.Length; i++)
                {
                    string[] result_array = result_arr[i].Split(':');
                    if (result_array.Length == 2)
                    {
                        switch (result_array[0])
                        {
                            case "status":
                                status = result_array[1];
                                break;
                            case "mes":
                                mes = result_array[1];
                                break;
                            case "data":
                                if (status == "0")
                                {
                                    string resultArr = result_array[1];
                                    resultArr = resultArr.TrimStart('[').TrimEnd(']');
                                    if (!string.IsNullOrEmpty(resultArr))
                                    {
                                        string[] res_Arr = resultArr.Split(';');
                                        if (res_Arr.Length > 0)
                                        {
                                            StringBuilder sbStr = new StringBuilder();
                                            string DecryptStr = String.Empty;//解密字符串
                                            for (int j = 0; j < res_Arr.Length; j++)
                                            {
                                                string[] res_Array = res_Arr[j].Split('*');
                                                switch (res_Array[0].ToLower())
                                                {
                                                    case "stockmaterial":
                                                    case "stockmateunits":
                                                        DecryptStr = OEncryp.DecryptLCL(res_Array[1].TrimEnd(']'));
                                                        sbStr.AppendLine(DecryptStr);
                                                        break;
                                                    default:
                                                        //解密后累加
                                                        DecryptStr = OEncryp.DecryptLCL(res_Array[1]);
                                                        sbStr.AppendLine(DecryptStr);
                                                        break;
                                                }
                                            }
                                            //有未提交的提交
                                            if (sbStr.ToString().Length > 0)
                                            {
                                                var tempStr = "";
                                                tempStr = " BEGIN TRAN tan1 " + sbStr.ToString() + " if(@@error=0) begin commit tran tan1 end else begin rollback tran tran1 end";
                                                result = DBHelper.ExecuteNonQuery(tempStr);
                                                sbStr.Clear();
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        result = -1;
                        status = "-1";
                        mes = "解析失败";
                        break;
                    }
                }
            }
            return result;
        }

    }
}
