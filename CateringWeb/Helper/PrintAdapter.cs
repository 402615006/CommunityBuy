using CommunityBuy.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy
{
    [Description("单据类型")]
    public enum PrintType
    {
        /// <summary>
        /// 制作单
        /// </summary>
        [EnumAttribute(Name = "制作单")]
        ProNote = 1,
        /// <summary>
        /// 划菜单
        /// </summary>
        [EnumAttribute(Name = "划菜单")]
        CrossFoodNote = 2,
        /// <summary>
        /// 退菜单
        /// </summary>
        [EnumAttribute(Name = "退菜单")]
        ReturnDishNote = 3,
        /// <summary>
        /// 转台单
        /// </summary>
        [EnumAttribute(Name = "转台单")]
        TurntableNote = 4,
        /// <summary>
        /// 催菜单
        /// </summary>
        [EnumAttribute(Name = "催菜单")]
        PushFoodNote = 5,
        /// <summary>
        /// 存酒贴瓶单
        /// </summary>
        [EnumAttribute(Name = "存酒贴瓶单")]
        SaveStickNote = 6
    }


    /// <summary>
    /// 打印帮助类
    /// 作者：程国栋
    /// 日期:2019-09-30
    /// </summary>
    public class PrintAdapter
    {
        /// <summary>
        /// 本地打印
        /// </summary>
        /// <param name="PrintName">打印机名称</param>
        /// <param name="TempFileName">模板完全路径</param>
        /// <param name="ChineseName">打印机显示文件名</param>
        /// <param name="ds">循环模板数据</param>
        /// <param name="dtData">单字段数据</param>
        /// <param name="PrintNum">打印数量</param>
        /// <returns></returns>
        public bool LocalPrintByData(string PrintName, string TempFileName, string ChineseName, DataSet ds, DataTable dtData, int PrintNum = 1)
        {
            bool Flag = false;

            AsponseHelper ap = new AsponseHelper();
            ap.IsPrint = true;
            if (PrintName.Length > 0)
            {
                ap.PrintName = PrintName;
            }
            Flag = ap.PrintOPenfileByFilePath(ChineseName, TempFileName, ds, dtData, PrintNum);
            return Flag;
        }


        /// <summary>
        /// 插入菜品制作单到打印队列
        /// </summary>
        /// <param name="dtData">数据：BusCode(商户编号),StoCode(门店编号),StoName(门店名称),OrderCode(订单编号),TableCode(桌台编号),TableName(桌台名称),CCname(操作人),CCode(操作人编号),CTime(操作时间),CusNum(客人数量),KitCode(厨房编号),TypeCode(菜品类别编号),DisCode(菜品编号),DisName(菜品名称),DisNum(菜品数量)
        /// </param>
        /// <param name="PrintNum">打印数量</param>
        /// <returns></returns>
        public bool InsertProNote(DataTable dtData)
        {
            bool Flag = false;
            Flag = PackPrintConten(PrintType.ProNote, dtData);
            return Flag;
        }

        /// <summary>
        /// 插入退菜单到打印队列
        /// </summary>
        /// <param name="dtData">数据：BusCode,StoCode,StoName,OrderCode,TableCode,TableName,CCname,CTime,ReasonName(退单原因),KitCode(厨房编号),TypeCode(菜品类别编号),DisCode(菜品编号),DisName,DisNum
        /// </param>
        /// <param name="PrintNum">打印数量</param>
        /// <returns></returns>
        public bool InsertReturnDishNote(DataTable dtData)
        {
            bool Flag = false;
            Flag = PackPrintContenOther(PrintType.ReturnDishNote, dtData);
            return Flag;
        }

        /// <summary>
        /// 插入转台单到打印队列
        /// </summary>
        /// <param name="dtData">数据：BusCode,StoCode,StoName,OrderCode,TableCode,TableName,CCname,CTime,CusNum,OldTableCode(原桌台编号),OldTableName(原桌台名称)
        /// </param>
        /// <param name="PrintNum">打印数量</param>
        /// <returns></returns>
        public bool InsertTurntableNote(DataTable dtData)
        {
            bool Flag = false;
            Flag = PackPrintContenOther(PrintType.TurntableNote, dtData);
            return Flag;
        }

        /// <summary>
        /// 插入催菜单到打印队列
        /// </summary>
        /// <param name="dtData">数据：BusCode,StoCode,StoName,OrderCode,TableCode,TableName,CCname,CTime,KitCode(厨房编号),TypeCode(菜品类别编号),DisCode(菜品编号),DisName,DisNum        
        /// </param>
        /// <param name="PrintNum">打印数量</param>
        /// <returns></returns>
        public bool InsertPushFoodNote(DataTable dtData, int PrintNum = 1)
        {
            bool Flag = false;
            Flag = PackPrintContenOther(PrintType.PushFoodNote, dtData);
            return Flag;
        }

        /// <summary>
        /// 插入存酒贴瓶单到打印队列
        /// </summary>
        /// <param name="dtData">数据：buscode,stocode,stoname,storecardid(存酒序号),savetime,endtime,customermobile,discode,disname,ccode,cname        
        /// </param>
        /// <param name="PrintNum">打印数量</param>
        /// <returns></returns>
        public bool InsertPoswinNote(DataTable dtData, int PrintNum = 1)
        {
            bool Flag = false;
            Flag = PackPrintContenOther(PrintType.SaveStickNote, dtData);
            return Flag;
        }

        //添加打印数据TB_PrintLog
        string strInsert = "INSERT INTO dbo.TB_PrintLog(BusCode,StoCode,CCode,CCname,CTime,TStatus,BillType,BillCode,KitCode,PrintCode,PrintContent,FailtNum,Remark,FailReason)VALUES('{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}');";

        /// <summary>
        /// 组装其他单据(退菜单、催菜单、转台单、贴瓶卡)
        /// </summary>
        /// <param name="PType"></param>
        /// <param name="dtData"></param>
        /// <returns></returns>
        private bool PackPrintContenOther(PrintType PType, DataTable dtData)
        {
            bool Flag = false;
            if (dtData != null && dtData.Rows.Count > 0)
            {
                if (dtData.Columns["BusCode"] == null || dtData.Columns["StoCode"] == null || dtData.Columns["CCode"] == null || dtData.Columns["CCname"] == null || dtData.Columns["OrderCode"] == null || dtData.Columns["KitCode"] == null)
                {
                    return Flag;
                }
                string PrintContent = string.Empty;
                string buscode = dtData.Rows[0]["BusCode"].ToString();
                string stocode = dtData.Rows[0]["StoCode"].ToString();
                DataTable print = GetCacheToPrintSet(buscode, stocode);

                DataTable dtReturn = new DataTable();
                StringBuilder sbReturn = new StringBuilder();
                foreach (DataRow dr in dtData.Rows)
                {
                    switch (PType)
                    {
                        //退菜单,催菜单
                        case PrintType.ReturnDishNote:
                        case PrintType.PushFoodNote:
                            dtReturn = GetPrintList(print, PType, dr["DisCode"].ToString(), dr["TypeCode"].ToString());
                            break;
                        //转台单，贴瓶卡
                        case PrintType.TurntableNote:
                        case PrintType.SaveStickNote:
                            dtReturn = GetOtherPrintList(print, PType);
                            break;
                    }
                    if (dtReturn != null && dtReturn.Rows.Count > 0)//生成打印SQL
                    {
                        foreach (DataRow dr2 in dtReturn.Rows)
                        {
                            switch (PType)
                            {
                                //退菜单
                                case PrintType.ReturnDishNote:
                                    PrintContent = GetReturn_PrintContent(dr);
                                    break;
                                //催菜单
                                case PrintType.PushFoodNote:
                                    PrintContent = GetExpedite_PrintContent(dr);
                                    break;
                                //转台单
                                case PrintType.TurntableNote:
                                    PrintContent = GetTurntable_PrintContent(dr);
                                    break;
                                //贴瓶卡
                                case PrintType.SaveStickNote:
                                    PrintContent = GetPoswin_PrintContent(dr);
                                    break;
                            }
                            sbReturn.Append(string.Format(strInsert, dr["BusCode"].ToString(), dr["StoCode"].ToString(), dr["CCode"].ToString(), dr["CCname"].ToString(), "getdate()", "0", PType.ToString("D"), dr["OrderCode"].ToString(), dr["KitCode"].ToString(), dr2["PrintCode"].ToString(), PrintContent, "0", "", ""));
                        }
                    }
                }
                //调用接口插入待打印数据
                Flag = InsertDataToInterface(sbReturn.ToString());
            }
            return Flag;
        }

        /// <summary>
        /// 组装制作单划菜端打印数据
        /// </summary>
        /// <returns></returns>
        private bool PackPrintConten(PrintType PType, DataTable dtData)
        {


            bool Flag = false;
            if (dtData != null && dtData.Rows.Count > 0)
            {
                if (dtData.Columns["BusCode"] == null || dtData.Columns["StoCode"] == null || dtData.Columns["CCode"] == null || dtData.Columns["CCname"] == null || dtData.Columns["OrderCode"] == null || dtData.Columns["KitCode"] == null)
                {
                    return Flag;
                }
                string PrintContent = string.Empty;
                string buscode = dtData.Rows[0]["BusCode"].ToString();
                string stocode = dtData.Rows[0]["StoCode"].ToString();
                DataTable print = GetCacheToPrintSet(buscode, stocode);

                DataTable dtReturn = new DataTable();
                StringBuilder sbReturn = new StringBuilder();
                foreach (DataRow dr in dtData.Rows)
                {
                    switch (PType)
                    {
                        //制作单
                        case PrintType.ProNote:
                            dtReturn = GetPrintList(print, PType, dr["DisCode"].ToString(), dr["TypeCode"].ToString());
                            break;
                    }
                    if (dtReturn != null && dtReturn.Rows.Count > 0)//生成打印SQL
                    {
                        foreach (DataRow dr2 in dtReturn.Rows)
                        {
                            if (dr2["PrintCode"] == null)
                            {
                                continue;
                            }
                            //打印行数信息
                            string ProContent = dr2["ProContent"].ToString().Trim().TrimStart(',').TrimEnd(',');
                            if (ProContent.Length > 0)//有配置多行数据
                            {
                                string[] arrCon = ProContent.Split(',');
                                if (arrCon.Length > 1)
                                {
                                    foreach (string str in arrCon)
                                    {
                                        switch (PType)
                                        {
                                            //制作单
                                            case PrintType.ProNote:
                                                dr["Stoname"] = str;
                                                PrintContent = GetPro_PrintContent(dr);
                                                break;
                                        }
                                        sbReturn.Append(string.Format(strInsert, dr["BusCode"].ToString(), dr["StoCode"].ToString(), dr["CCode"].ToString(), dr["CCname"].ToString(), "getdate()", "0", PType.ToString("D"), dr["OrderCode"].ToString(), dr["KitCode"].ToString(), dr2["PrintCode"].ToString(), PrintContent, "0", "", ""));
                                    }
                                }
                                else
                                {
                                    PrintContent = GetPro_PrintContent(dr);
                                    sbReturn.Append(string.Format(strInsert, dr["BusCode"].ToString(), dr["StoCode"].ToString(), dr["CCode"].ToString(), dr["CCname"].ToString(), "getdate()", "0", PType.ToString("D"), dr["OrderCode"].ToString(), dr["KitCode"].ToString(), dr2["PrintCode"].ToString(), PrintContent, "0", "", ""));
                                }
                            }
                            else
                            {
                                PrintContent = GetPro_PrintContent(dr);
                                sbReturn.Append(string.Format(strInsert, dr["BusCode"].ToString(), dr["StoCode"].ToString(), dr["CCode"].ToString(), dr["CCname"].ToString(), "getdate()", "0", PType.ToString("D"), dr["OrderCode"].ToString(), dr["KitCode"].ToString(), dr2["PrintCode"].ToString(), PrintContent, "0", "", ""));
                            }
                        }
                    }
                    else//未找到打印设置
                    {
                        PrintContent = GetPro_PrintContent(dr);
                        sbReturn.Append(string.Format(strInsert, dr["BusCode"].ToString(), dr["StoCode"].ToString(), dr["CCode"].ToString(), dr["CCname"].ToString(), "getdate()", "0", PType.ToString("D"), dr["OrderCode"].ToString(), "", "", PrintContent, "0", "未找到打印设置项", ""));
                    }
                }
                if (PType == PrintType.ProNote)//根据制作单生成划菜单
                {
                    DataTable dtCross = GetCrossPrintList(print, dtData);

                    DataTable dtCross_List = dtCross.DefaultView.ToTable(true, "PrintCode", "CrossContent");//分组打印机数据
                    if (dtCross_List != null && dtCross_List.Rows.Count > 0)
                    {
                        if (dtCross_List.Columns["PrintCode"] == null)
                        {
                            return Flag;
                        }
                        foreach (DataRow drList in dtCross_List.Rows)
                        {
                            if (drList["PrintCode"].ToString().Length == 0)//没有配置打印机不管
                            {
                                continue;
                            }
                            string ProContent = drList["CrossContent"].ToString().Trim().TrimStart(',').TrimEnd(',');//打印行数

                            //根据打印机查找所有的菜品信息
                            DataRow[] drSelect = dtCross.Select("PrintCode='" + drList["PrintCode"].ToString() + "'");
                            if (drSelect.Length > 0)
                            {
                                if (ProContent.Length > 0)//有多行配置
                                {
                                    string[] arrCon = ProContent.Split(',');
                                    if (arrCon.Length > 1)
                                    {
                                        foreach (string str in arrCon)
                                        {
                                            PrintContent = GetCross_PrintContent(drSelect, str);
                                            sbReturn.Append(string.Format(strInsert, drSelect[0]["BusCode"].ToString(), drSelect[0]["StoCode"].ToString(), drSelect[0]["CCode"].ToString(), drSelect[0]["CCname"].ToString(), "getdate()", "0", PrintType.CrossFoodNote.ToString("D"), drSelect[0]["OrderCode"].ToString(), drSelect[0]["KitCode"].ToString(), drList["PrintCode"].ToString(), PrintContent, "0", "", ""));
                                        }
                                    }
                                    else
                                    {
                                        PrintContent = GetCross_PrintContent(drSelect, drSelect[0]["StoName"].ToString());
                                        sbReturn.Append(string.Format(strInsert, drSelect[0]["BusCode"].ToString(), drSelect[0]["StoCode"].ToString(), drSelect[0]["CCode"].ToString(), drSelect[0]["CCname"].ToString(), "getdate()", "0", PrintType.CrossFoodNote.ToString("D"), drSelect[0]["OrderCode"].ToString(), drSelect[0]["KitCode"].ToString(), drList["PrintCode"].ToString(), PrintContent, "0", "", ""));
                                    }
                                }
                                else
                                {
                                    PrintContent = GetCross_PrintContent(drSelect, drSelect[0]["StoName"].ToString());
                                    sbReturn.Append(string.Format(strInsert, drSelect[0]["BusCode"].ToString(), drSelect[0]["StoCode"].ToString(), drSelect[0]["CCode"].ToString(), drSelect[0]["CCname"].ToString(), "getdate()", "0", PrintType.CrossFoodNote.ToString("D"), drSelect[0]["OrderCode"].ToString(), drSelect[0]["KitCode"].ToString(), drList["PrintCode"].ToString(), PrintContent, "0", "", ""));
                                }
                            }
                        }
                    }
                }
                //调用接口插入待打印数据
                Flag = InsertDataToInterface(sbReturn.ToString());
            }
            return Flag;
        }

        /// <summary>
        /// 调用接口插入待打印数据
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        private bool InsertDataToInterface(string SQL)
        {
            bool Flag = false;
            int result=new bllPaging().ExecuteNonQueryBySQL(SQL);
            if(result==0)
            {
                Flag = true;
            }

            return Flag;
        }

        #region 获取打印Json

        /// <summary>
        /// 获取退菜单打印Json
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetReturn_PrintContent(DataRow dr)
        {
            string strReturn = string.Empty;

            //数据：BusCode,StoCode,StoName,OrderCode,TableCode,TableName,CCname,CTime,ReasonName(退单原因),KitCode(厨房编号),TypeCode(菜品类别编号),DisCode(菜品编号),DisName,DisNum
            if (dr != null)
            {
                if (dr["StoName"] != null && dr["TableName"] != null && dr["OrderCode"] != null && dr["CCname"] != null && dr["CTime"] != null && dr["DisName"] != null && dr["DisNum"] != null && dr["ReasonName"] != null)
                {
                    strReturn = "\"data\":[{\"StoName\":\"" + dr["StoName"].ToString() + "\",\"TableName\":\"" + dr["TableName"].ToString() + "\",\"OrderCode\":\"" + dr["OrderCode"].ToString() + "\",\"CCname\":\"" + dr["CCname"].ToString() + "\",\"CTime\":\"" + dr["CTime"].ToString() + "\",\"DisName\":\"" + dr["DisName"].ToString() + "\",\"DisNum\":\"" + dr["DisNum"].ToString() + "\",\"ReasonName\":\"" + dr["ReasonName"].ToString() + "\"}]";
                }
            }
            return strReturn;
        }

        /// <summary>
        /// 获取催菜单打印Json
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetExpedite_PrintContent(DataRow dr)
        {
            string strReturn = string.Empty;
            //数据：BusCode,StoCode,StoName,OrderCode,TableCode,TableName,CCname,CTime,KitCode(厨房编号),TypeCode(菜品类别编号),DisCode(菜品编号),DisName,DisNum              if (dr != null)
            {
                if (dr["StoName"] != null && dr["TableName"] != null && dr["OrderCode"] != null && dr["CCname"] != null && dr["CTime"] != null && dr["DisName"] != null && dr["DisNum"] != null)
                {
                    strReturn = "\"data\":[{\"StoName\":\"" + dr["StoName"].ToString() + "\",\"TableName\":\"" + dr["TableName"].ToString() + "\",\"OrderCode\":\"" + dr["OrderCode"].ToString() + "\",\"CCname\":\"" + dr["CCname"].ToString() + "\",\"CTime\":\"" + dr["CTime"].ToString() + "\",\"DisName\":\"" + dr["DisName"].ToString() + "\",\"DisNum\":\"" + dr["DisNum"].ToString() + "\"}]";
                }
            }
            return strReturn;
        }

        /// <summary>
        /// 获取转台单打印Json
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetTurntable_PrintContent(DataRow dr)
        {
            string strReturn = string.Empty;
            //数据：数据：BusCode,StoCode,StoName,OrderCode,TableCode,TableName,CCname,CTime,CusNum,OldTableCode(原桌台编号),OldTableName(原桌台名称)
            if (dr != null)
            {
                if (dr["StoName"] != null && dr["TableName"] != null && dr["OrderCode"] != null && dr["CCname"] != null && dr["OldTableName"] != null && dr["CusNum"] != null)
                {
                    strReturn = "\"data\":[{\"StoName\":\"" + dr["StoName"].ToString() + "\",\"TableName\":\"" + dr["TableName"].ToString() + "\",\"OrderCode\":\"" + dr["OrderCode"].ToString() + "\",\"CCname\":\"" + dr["CCname"].ToString() + "\",\"OldTableName\":\"" + dr["OldTableName"].ToString() + "\",\"CusNum\":\"" + dr["CusNum"].ToString() + "\"}]";
                }
            }
            return strReturn;
        }

        /// <summary>
        /// 获取贴瓶单打印Json
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetPoswin_PrintContent(DataRow dr)
        {
            string strReturn = string.Empty;
            //数据：buscode,stocode,stoname,storecardid(存酒序号),savetime,endtime,customermobile,discode,disname,ccode,cname
            if (dr != null)
            {
                if (dr["savetime"] != null && dr["endtime"] != null && dr["disname"] != null && dr["cname"] != null && dr["storecardid"] != null && dr["customermobile"] != null)
                {
                    strReturn = "\"data\":[{\"endtime\":\"" + dr["endtime"].ToString() + "\",\"disname\":\"" + dr["disname"].ToString() + "\",\"cname\":\"" + dr["cname"].ToString() + "\",\"savetime\":\"" + dr["savetime"].ToString() + "\",\"postno\":\"" + dr["storecardid"].ToString() + "\",\"customermobile\":\"" + dr["customermobile"].ToString() + "\"}]";
                }
            }
            return strReturn;
        }


        /// <summary>
        /// 获取制作单打印Json
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetPro_PrintContent(DataRow dr)
        {
            string strReturn = string.Empty;

            //数据：BusCode(商户编号),StoCode(门店编号),StoName(门店名称),OrderCode(订单编号),TableCode(桌台编号),TableName(桌台名称),CCname(操作人),CTime(操作时间),CusNum(客人数量),KitCode(厨房编号),TypeCode(菜品类别编号),DisCode(菜品编号),DisName(菜品名称),DisNum(菜品数量)
            if (dr != null)
            {
                if (dr["StoName"] != null && dr["TableName"] != null && dr["OrderCode"] != null && dr["CCname"] != null && dr["CTime"] != null && dr["DisName"] != null && dr["DisNum"] != null)
                {
                    strReturn = "\"data\":[{\"StoName\":\"" + dr["StoName"].ToString() + "\",\"TableName\":\"" + dr["TableName"].ToString() + "\",\"OrderCode\":\"" + dr["OrderCode"].ToString() + "\",\"CCname\":\"" + dr["CCname"].ToString() + "\",\"CTime\":\"" + dr["CTime"].ToString() + "\",\"DisName\":\"" + dr["DisName"].ToString() + "\",\"DisNum\":\"" + dr["DisNum"].ToString() + "\"}]";
                }
            }

            return strReturn;
        }

        /// <summary>
        /// 获取划菜单打印Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetCross_PrintContent(DataRow[] drSelect, string CrossContent)
        {
            string strReturn = string.Empty;

            //数据：BusCode(商户编号),StoCode(门店编号),StoName(门店名称),OrderCode(订单编号),TableCode(桌台编号),TableName(桌台名称),CCname(操作人),CTime(操作时间),CusNum(客人数量),KitCode(厨房编号),TypeCode(菜品类别编号),DisCode(菜品编号),DisName(菜品名称),DisNum(菜品数量)
            if (drSelect != null && drSelect.Length > 0)
            {
                if (drSelect[0]["StoName"] != null && drSelect[0]["TableName"] != null && drSelect[0]["OrderCode"] != null && drSelect[0]["CCname"] != null && drSelect[0]["CTime"] != null && drSelect[0]["DisName"] != null && drSelect[0]["DisNum"] != null && drSelect[0]["CusNum"] != null)
                {
                    strReturn = "\"data\":[";
                    foreach (DataRow dr in drSelect)
                    {
                        strReturn += "{\"StoName\":\"" + CrossContent + "\",\"TableName\":\"" + dr["TableName"].ToString() + "\",\"OrderCode\":\"" + dr["OrderCode"].ToString() + "\",\"CCname\":\"" + dr["CCname"].ToString() + "\",\"CTime\":\"" + dr["CTime"].ToString() + "\",\"DisName\":\"" + dr["DisName"].ToString() + "\",\"DisNum\":\"" + dr["DisNum"].ToString() + "\",\"CusNum\":\"" + dr["CusNum"].ToString() + "\"},";
                    }
                    strReturn = strReturn.TrimEnd(',') + "]";
                }
            }

            return strReturn;
        }
        #endregion

        /// <summary>
        /// 获取转台单、贴瓶卡打印机列表
        /// </summary>
        /// <param name="print"></param>
        /// <returns></returns>
        private DataTable GetOtherPrintList(DataTable print, PrintType PType)
        {
            DataTable dtReturn = new DataTable("Print");
            dtReturn.Columns.Add("PrintCode", typeof(string));
            dtReturn.Columns.Add("TerminalName", typeof(string));
            dtReturn.Columns.Add("TerminalIP", typeof(string));
            dtReturn.Columns.Add("SpareIP", typeof(string));
            dtReturn.AcceptChanges();

            DataRow[] drsPrint = print.Select("BillTypes='" + PType.ToString("D") + "'");
            if (drsPrint.Length > 0)
            {
                foreach (DataRow dr in drsPrint)
                {
                    DataRow drR = dtReturn.NewRow();
                    drR["PrintCode"] = dr["PrintCode"].ToString();
                    drR["TerminalName"] = dr["TerminalName"].ToString();
                    drR["TerminalIP"] = dr["TerminalIP"].ToString();
                    drR["SpareIP"] = dr["SpareIP"].ToString();
                    dtReturn.Rows.Add(drR);
                }
            }
            return dtReturn;
        }


        /// <summary>
        /// 获取制作单，退菜单、催菜单打印机列表
        /// </summary>
        /// <param name="print"></param>
        /// <param name="PType"></param>
        /// <param name="discode"></param>
        /// <param name="DisTypeCode"></param>
        /// <returns></returns>
        private DataTable GetPrintList(DataTable print, PrintType PType, string discode, string DisTypeCode)
        {
            DataTable dtReturn = new DataTable("Print");
            dtReturn.Columns.Add("PrintCode", typeof(string));
            dtReturn.Columns.Add("ProContent", typeof(string));
            dtReturn.Columns.Add("CrossContent", typeof(string));
            dtReturn.Columns.Add("TerminalName", typeof(string));
            dtReturn.Columns.Add("TerminalIP", typeof(string));
            dtReturn.Columns.Add("SpareIP", typeof(string));
            dtReturn.AcceptChanges();

            string where = "BillTypes='" + PrintType.ProNote.ToString("D") + "'";
            string where2 = "BillTypes='" + PrintType.ProNote.ToString("D") + "'";
            //查找打印机
            switch (PType)
            {
                //制作单，退菜单、催菜单
                case PrintType.ReturnDishNote:
                case PrintType.PushFoodNote:
                case PrintType.ProNote:
                    where += " and ProDisCodeList like '%\"DisCode\":\"" + discode + "\"%'";
                    where2 += " and DTCodeList like '%\"PKCode\":\"" + DisTypeCode + "\"%'";
                    break;
            }
            //1.按菜品搜索
            DataRow[] drsPrint = print.Select(where);
            if (drsPrint.Length == 0)
            {
                //1.按菜品类别搜索
                drsPrint = print.Select(where2);
            }

            if (drsPrint.Length > 0)
            {
                foreach (DataRow dr in drsPrint)
                {
                    DataRow drR = dtReturn.NewRow();
                    drR["PrintCode"] = dr["PrintCode"].ToString();
                    drR["ProContent"] = dr["ProContent"].ToString();
                    drR["CrossContent"] = dr["CrossContent"].ToString();
                    drR["TerminalName"] = dr["TerminalName"].ToString();
                    drR["TerminalIP"] = dr["TerminalIP"].ToString();
                    drR["SpareIP"] = dr["SpareIP"].ToString();
                    dtReturn.Rows.Add(drR);
                }
            }
            return dtReturn;
        }

        /// <summary>
        /// 获取划菜单打印机列表
        /// </summary>
        /// <param name="print"></param>
        /// <param name="dtDish"></param>
        /// <returns></returns>
        private DataTable GetCrossPrintList(DataTable print, DataTable dtDish)
        {
            DataTable dtReturn = new DataTable("dtPrint");

            if (dtDish != null && dtDish.Rows.Count > 0)
            {
                dtReturn = dtDish.Copy();
                dtReturn.Columns.Add("PrintCode", typeof(string));
                dtReturn.Columns.Add("CrossContent", typeof(string));
                dtReturn.Columns.Add("TerminalName", typeof(string));
                dtReturn.Columns.Add("TerminalIP", typeof(string));
                dtReturn.Columns.Add("SpareIP", typeof(string));
                dtReturn.AcceptChanges();

                for (int i = 0; i < dtReturn.Rows.Count; i++)
                {
                    DataRow dr = dtReturn.Rows[i];
                    //1.按菜品搜索
                    DataRow[] drsPrint = print.Select("BillTypes='" + PrintType.CrossFoodNote.ToString("D") + "' and CrossDisCodeList like '%\"DisCode\":\"" + dr["DisCode"].ToString() + "\"%'");
                    if (drsPrint.Length == 0)
                    {
                        //1.按菜品类别搜索
                        drsPrint = print.Select("BillTypes='" + PrintType.CrossFoodNote.ToString("D") + "' and DTCodeList like '%\"hcdistype\"%' and DTCodeList like '%\"PKCode\":\"" + dr["TypeCode"].ToString() + "\"%'");
                    }

                    if (drsPrint.Length > 0)
                    {
                        dr["PrintCode"] = drsPrint[0]["PrintCode"].ToString();
                        dr["CrossContent"] = drsPrint[0]["CrossContent"].ToString();
                        dr["TerminalName"] = drsPrint[0]["TerminalName"].ToString();
                        dr["TerminalIP"] = drsPrint[0]["TerminalIP"].ToString();
                        dr["SpareIP"] = drsPrint[0]["SpareIP"].ToString();
                        dtReturn.AcceptChanges();
                    }
                }
            }
            return dtReturn;
        }

        #region 获取缓存打印设置信息
        /// <summary>
        /// 根据用户编号从缓存中取部门信息
        /// </summary>
        /// <param name="userid">用户id</param>
        private DataTable GetCacheToPrintSet(string buscode, string stocode)
        {
            DataTable print = null;
            object objprint = AppDomain.CurrentDomain.GetData(buscode + stocode + "_printset");
            if (objprint != null)
            {
                print = (DataTable)AppDomain.CurrentDomain.GetData(buscode + stocode + "_printset");
            }
            else
            {
                string SQL = "select tps.*,tp.TerminalCode,tp.TerminalName,tp.SpareIP,tp.TerminalIP from TB_PrintSet tps left join TB_Printer tp on tps.PrintCode = tp.TerminalCode where tps.StoCode='" + stocode + "' and tp.TStatus='1' ";
                print = new bllPaging().GetDataTableInfoBySQL(SQL);
                AppDomain.CurrentDomain.SetData(buscode + stocode + "_printset", print);
            }
            return print;
        }
        #endregion
    }
}
