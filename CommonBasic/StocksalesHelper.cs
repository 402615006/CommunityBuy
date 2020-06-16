using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.CommonBasic
{

    public class StockJsonObject
    {
        public string status { get; set; }
        public string mes { get; set; }
    }


    public class StocksalesHelper
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        Thread thRefreshTable = null;// new Thread(RefreshTable()); 
        string _stocode = string.Empty;


        public void StartStockThread(string stocode)
        {
            _stocode = stocode;
            if (thRefreshTable == null)
            {
                thRefreshTable = new Thread(LoopExecStocksales);
                thRefreshTable.IsBackground = true;
                thRefreshTable.Start();
            }
            else
            {
                if (thRefreshTable.ThreadState == ThreadState.Stopped)
                {
                    thRefreshTable.Start();
                }
            }
        }

        public void StopStockThread()
        {
            if (thRefreshTable != null)
            {
                thRefreshTable.Abort();
            }
        }



        private void LoopExecStocksales()
        {
            while (true)
            {
                ExecStocksales(_stocode);
                Thread.Sleep(100);
            }
        }


        private void ExecStocksales(string stocode)
        {
            string actionname = string.Empty;
            string rancode = string.Empty;
            string buscode = string.Empty;
            string orderno = string.Empty;

            int actype = 0;
            int astatus = 0;
            string strOrderdishesids = string.Empty;
            string sql = string.Empty;

            try
            {
                //把2018-02-26日之前的删掉（盘点过了，就不需要出入库处理了）
                //string strYMD = System.DateTime.Now.ToString("yyyy-MM-dd");
                //string[] arrDays = strYMD.Split('-');
                //int intDay = Convert.ToInt32(arrDays[2]);
                DBHelper.ExecuteNonQuery(string.Format("delete StoIVAction where stocode='{0}' and ctime<'2018-02-26 00:09:00' ;  ", stocode));

                DataTable dtStoIVAction = DBHelper.ExecuteDataTable(string.Format("select top 100 * from StoIVAction where stocode='{0}' and ctime<=DATEADD(mi,-5,GETDATE()) order by said ;  ", stocode));
                if (dtStoIVAction != null && dtStoIVAction.Rows.Count > 0)
                {
                    for (int i = 0; i < dtStoIVAction.Rows.Count; i++)
                    {
                        sql = string.Empty;

                        #region 通过web服务接口获取数据
                        string strMessage = string.Empty;
                        //获取URL
                        string ShortMesUrl = dtStoIVAction.Rows[i]["allurl"].ToString();
                        if (dtStoIVAction.Rows[i]["actype"].ToString().Trim() == "0")
                        {
                            //出库
                            actionname = "stocksalestrue";//出库
                        }
                        else
                        {
                            //入库
                            actionname = "stockrecoverytrue"; //入库
                        }

                        rancode = dtStoIVAction.Rows[i]["rancode"].ToString().Trim();
                        buscode = dtStoIVAction.Rows[i]["buscode"].ToString().Trim();
                        orderno = dtStoIVAction.Rows[i]["orderno"].ToString().Trim();
                        actype = Helper.StringToInt(dtStoIVAction.Rows[i]["actype"].ToString().Trim());
                        astatus = Helper.StringToInt(dtStoIVAction.Rows[i]["astatus"].ToString().Trim());
                        strOrderdishesids = dtStoIVAction.Rows[i]["remark"].ToString().Trim();

                        StringBuilder postStr = new StringBuilder();
                        postStr.Append(dtStoIVAction.Rows[i]["stockjsons"].ToString().Trim());//物品json

                        if (ShortMesUrl.Contains("//"))
                        {
                            ShortMesUrl = ShortMesUrl.Replace("//", "/");
                            ShortMesUrl = ShortMesUrl.Replace("http:/", "http://");
                        }
                        string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());//调service


                        if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
                        {
                            #region 有返回信息
                            StockJsonObject objJson = JsonConvert.DeserializeObject<StockJsonObject>(strAdminJson);
                            if (objJson.status.Trim() == "0")
                            {//成功
                                if (actionname == "stockrecovery")
                                {//反结算
                                    sql = string.Format(" if EXISTS(select top 1 * from choorderdishes where orderdishesid in ({0}) ) begin ", strOrderdishesids.ToString());
                                    sql += string.Format(" update choorderdishes set storeupdated='0' where orderdishesid in ({0}) ", strOrderdishesids.ToString());
                                    sql += " end else begin ";
                                    sql += string.Format(" update choorderdisheshistory set storeupdated='0' where orderdishesid in ({0}) ", strOrderdishesids.ToString());
                                    sql += " end ";
                                }
                                else
                                {//结算
                                    sql = string.Format(" if EXISTS(select top 1 * from choorderdishes where orderdishesid in ({0}) ) begin ", strOrderdishesids.ToString());
                                    sql += string.Format(" update choorderdishes set storeupdated='1' where orderdishesid in ({0}) ", strOrderdishesids.ToString());
                                    sql += " end else begin ";
                                    sql += string.Format(" update choorderdisheshistory set storeupdated='1' where orderdishesid in ({0}) ", strOrderdishesids.ToString());
                                    sql += " end ";

                                }
                                if (ExecuteDataSetByTran2(sql) > 0)
                                { //执行成功
                                    #region 删除执行成功的信息
                                    string strMes = string.Empty;
                                    Delete2(buscode, stocode, orderno, actype, astatus, ref strMes); //
                                    #endregion
                                }
                            }
                            else
                            {
                                strMessage = objJson.mes.Trim();
                            }
                            #endregion
                        }
                        else
                        {
                            strMessage = "调用远程货品出入库接口失败。";
                        }

                        if (!string.IsNullOrEmpty(strMessage))
                        {
                            //出入库失败，存入临时表，待总控程序继续执行
                            break;//有一个失败则跳出（必须顺序执行）
                        }
                        else
                        {
                            //执行成功，删除相应的存储信息

                        }

                        #endregion

                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
            }

        }

        private int Delete2(string buscode, string stocode, string orderno, int actype, int astatus, ref string mescode)
        {
            int intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@buscode", buscode),
                 new SqlParameter("@stocode", stocode),
                 new SqlParameter("@actype", actype),
                 new SqlParameter("@orderno", orderno),
                 new SqlParameter("@astatus", astatus),
                 new SqlParameter("@mescode", mescode)
             };
            sqlParameters[5].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_StoIVAction_Delete2", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[5].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 用事务执行SQL命令,并返回受影响的行数
        /// </summary>
        /// <param name="sql">sql语句（不包含事务语句）</param>
        /// <returns>返回影响的行数</returns>
        private int ExecuteDataSetByTran2(string sql)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine(" BEGIN TRAN tan1");//开始事务
            Builder.AppendLine(sql);//开始事务
            Builder.AppendLine(" if(@@error=0) begin commit tran tan1; end else begin rollback tran tran1 end");//结束事务
            DataSet ds = null;
            try
            {
                int intResult = DBHelper.ExecuteNonQuery(Builder.ToString());
                return intResult;
            }
            catch (Exception ex)
            { }
            finally
            {
            }
            return 0;
        }

    }
}
