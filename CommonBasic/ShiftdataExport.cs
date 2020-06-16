using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using XJWZCatering.CommonBasic;

namespace XJWZCatering.CommonBasic
{
    public class ShiftdataExport
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        private string _buscode = string.Empty;
        private string _stocode = string.Empty;
        private bool _isMaterialAutoOutStock = false;//是否自动出库原料

        /// <summary>
        /// 是否自动出库原料
        /// </summary>
        public bool isMaterialAutoOutStock
        {
            get { return _isMaterialAutoOutStock; }
            set { _isMaterialAutoOutStock = value; }
        }

        #region 从历史表导出班次业务数据
        public string ExportShiftDataForHistory(string strBuscode, string strStocode, string strShiftID)
        {
            string strExportResult = string.Empty;
            if (strShiftID.Trim() == "")
            {
                strExportResult = "请输入班次号";
                return strExportResult;
            }
            if (strStocode.Trim() == "")
            {
                strExportResult = "请输入门店编号";
                return strExportResult;
            }

            try
            {
                //生成连锁端数据
                StringBuilder sbUploadTables = new StringBuilder();

                string strHistory = string.Empty;
                //MSSqlDataAccess DBHelper = new MSSqlDataAccess();

                sbUploadTables.AppendFormat(" select *,ishistory='1' from chopenshifthistory where shiftid={0} and strcode='{1}' ;  ", strShiftID, strStocode);
                DataTable dtShift = DBHelper.ExecuteDataTable(sbUploadTables.ToString());
                if (dtShift != null && dtShift.Rows.Count > 0)
                {
                    if (dtShift.Rows[0]["ishistory"].ToString().Trim() == "1")
                    {
                        strHistory = "history";
                    }
                }
                else
                {
                    strExportResult = "不存在该班次，请确认。";
                    return strExportResult;
                }

                sbUploadTables.Clear();
                //临时数据
                sbUploadTables.AppendFormat(" DECLARE @chopenshift TABLE (shiftid BIGINT,buscode varchar(16),strcode varchar(16)); ");
                sbUploadTables.AppendFormat(" INSERT INTO @chopenshift SELECT shiftid,buscode,strcode FROM chopenshift" + strHistory + " WHERE shiftid={0} ; ", strShiftID);
                sbUploadTables.AppendFormat(" DECLARE @choorderdetailBreak TABLE(detailcodes varchar(5120),orderno varchar(32), shiftid BIGINT,expendtype char(1),pstatus char(1)); ");
                sbUploadTables.AppendFormat(" INSERT INTO @choorderdetailBreak SELECT detailcodes ,orderno,shiftid ,expendtype,pstatus FROM choorderdetailBreak" + strHistory + " ");
                sbUploadTables.AppendFormat(" WHERE shiftid IN (SELECT shiftid FROM @chopenshift ) AND [pstatus] IN ('1','2','4','5') ; ");
                sbUploadTables.AppendFormat(" DECLARE @choorderdetail TABLE(detailid BIGINT,orderid BIGINT); ");
                sbUploadTables.AppendFormat(" INSERT @choorderdetail SELECT a.detailid,a.orderid  ");
                sbUploadTables.AppendFormat(" FROM choorderdetail" + strHistory + "  a INNER JOIN @choorderdetailBreak b ON CHARINDEX(','+a.detailcode+',',','+b.detailcodes+',')>0 WHERE b.pstatus<>'5' ;  ");
                sbUploadTables.AppendFormat(" DECLARE @choorderdishes TABLE(orderdishesid BIGINT,porderdishesid BIGINT); ");
                sbUploadTables.AppendFormat(" INSERT INTO @choorderdishes SELECT orderdishesid,porderdishesid FROM choorderdishes" + strHistory + "  WHERE detailid IN (SELECT detailid FROM @choorderdetail ); ");
                sbUploadTables.AppendFormat(" DECLARE @busDestine TABLE(ID BIGINT,desCode varchar(32)); ");
                sbUploadTables.AppendFormat(" INSERT INTO @busDestine SELECT ID,desCode FROM busDestine" + strHistory + "  WHERE desCode IN (SELECT detailcodes AS desCode FROM @choorderdetailBreak WHERE pstatus<>'5' ); ");
                //获取表数据
                sbUploadTables.AppendFormat("select a.*,'cholossdetail' as tbname FROM cholossdetail" + strHistory + "  a INNER JOIN choloss" + strHistory + "  b ON a.losscode=b.losscode WHERE b.detailid IN (SELECT detailid FROM @choorderdetail );");
                sbUploadTables.AppendFormat("select b.*,'choloss' as tbname from choloss" + strHistory + "  b WHERE b.detailid IN (SELECT detailid FROM @choorderdetail );");
                sbUploadTables.AppendFormat("select *,'choorderdetailsurcharge' as tbname  FROM choorderdetailsurcharge" + strHistory + "  WHERE detailid IN (SELECT detailid FROM @choorderdetail );");
                sbUploadTables.AppendFormat("select *,'chobackdetails' as tbname  FROM chobackdetails" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'choorderdishesdetails' as tbname FROM choorderdishesdetails" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'cdmethodprice' as tbname FROM cdmethodprice" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'chopackage' as tbname  FROM chopackage" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'choorderdishes' as tbname  FROM choorderdishes" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'choorder' as tbname  FROM choorder" + strHistory + "  WHERE orderid IN (SELECT orderid FROM @choorderdetail GROUP BY orderid );");
                sbUploadTables.AppendFormat("select *,'choorderdetail' as tbname  FROM choorderdetail" + strHistory + "  WHERE detailid IN (SELECT detailid FROM @choorderdetail);");
                sbUploadTables.AppendFormat("select *,'paymentDetails' as tbname  FROM paymentDetails" + strHistory + "  WHERE busId IN (SELECT ID AS busId FROM @busDestine );");
                sbUploadTables.AppendFormat("select *,'busDestineTable' as tbname  FROM busDestineTable" + strHistory + "  WHERE desCode IN (SELECT desCode FROM @busDestine );");
                sbUploadTables.AppendFormat("select *,'busDestine' as tbname FROM busDestine" + strHistory + "  WHERE Id IN (SELECT ID AS busId FROM @busDestine );");
                sbUploadTables.AppendFormat("select *,'memcardorders' as tbname FROM memcardorders" + strHistory + "  WHERE ordercode IN (SELECT detailcodes AS ordercode FROM @choorderdetailBreak WHERE pstatus<>'5');");
                sbUploadTables.AppendFormat("select *,'choorderdetailcoupon' as tbname  FROM choorderdetailcoupon" + strHistory + "  WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak );");
                sbUploadTables.AppendFormat("select *,'chopayincome' as tbname  FROM chopayincome" + strHistory + "  WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak );");
                sbUploadTables.AppendFormat("select *,'chopay' as tbname  FROM chopay" + strHistory + "  WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak );");
                sbUploadTables.AppendFormat("select a.*,'chopayMisposDetail' as tbname FROM chopayMisposDetail" + strHistory + "  a inner JOIN @chopenshift b on a.shiftid=b.shiftid;");
                sbUploadTables.AppendFormat("select *,'choorderdetailBreak' as tbname  FROM choorderdetailBreak" + strHistory + "  WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak );");
                sbUploadTables.AppendFormat("select a.*,'chopenshiftmoney' as tbname FROM chopenshiftmoney" + strHistory + "  a inner JOIN @chopenshift b on a.shiftid=b.shiftid;");
                sbUploadTables.AppendFormat("select a.*,'chopenshift' as tbname FROM chopenshift" + strHistory + "  a inner JOIN @chopenshift b on a.shiftid=b.shiftid;");
                //批量验券
                sbUploadTables.AppendFormat("select *,'couponchecklog' as tbname FROM couponchecklog where shiftid in(select shiftid from @chopenshift) ;");
                //
                sbUploadTables.AppendFormat("select *,'bookmoneylog' as tbname FROM bookmoneylog where shiftid in(select shiftid from @chopenshift) ;");

                #region 表关键字典字典
                Dictionary<string, string> dicTableUniqueColumnName = new Dictionary<string, string>();
                dicTableUniqueColumnName.Add("cholossdetailhistory", "cldid,stocode");
                dicTableUniqueColumnName.Add("cholosshistory", "outofid,stocode");
                dicTableUniqueColumnName.Add("choorderdetailsurchargehistory", "surchargeid,stocode");
                dicTableUniqueColumnName.Add("chobackdetailshistory", "backdetailsid,strcode");
                dicTableUniqueColumnName.Add("choorderdishesdetailshistory", "orderdetails,strcode");
                dicTableUniqueColumnName.Add("cdmethodpricehistory", "cdmp,stocode");
                dicTableUniqueColumnName.Add("chopackagehistory", "packageid,stocode");
                dicTableUniqueColumnName.Add("choorderdisheshistory", "orderdishesid,stocode");
                dicTableUniqueColumnName.Add("choorderhistory", "orderid,strcode");
                dicTableUniqueColumnName.Add("choorderdetailhistory", "detailid,stocode");
                dicTableUniqueColumnName.Add("paymentDetailshistory", "payId,strcode");
                dicTableUniqueColumnName.Add("busDestineTablehistory", "dtid,stocode");
                dicTableUniqueColumnName.Add("busDestinehistory", "ID,stocode");
                dicTableUniqueColumnName.Add("memcardordershistory", "ID,stocode");
                dicTableUniqueColumnName.Add("choorderdetailcouponhistory", "paydcid,strcode");
                dicTableUniqueColumnName.Add("chopayincomehistory", "incomid,strcode");
                dicTableUniqueColumnName.Add("chopayhistory", "payid,strcode");
                dicTableUniqueColumnName.Add("chopayMisposDetailhistory", "mispid,stocode");
                dicTableUniqueColumnName.Add("choorderdetailBreakhistory", "detailbreakid,stocode");
                dicTableUniqueColumnName.Add("chopenshiftmoneyhistory", "shiftmid,strcode");
                dicTableUniqueColumnName.Add("chopenshifthistory", "shiftid,strcode");
                #endregion

                #region 先删除本门店该班次的所有数据
                StringBuilder sbDeleteShiftDataSql = new StringBuilder();
                //删除损赔
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete cholossdetailhistory where stocode=''{0}'' and losscode in ( select losscode from cholosshistory where stocode=''{0}'' " +
                    " and detailid in (select d.detailid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ) ; ", strStocode, strShiftID));
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete cholosshistory where stocode=''{0}'' " +
                    " and detailid in (select d.detailid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //附加费
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdetailsurchargehistory where stocode=''{0}'' " +
                    " and detailid in (select d.detailid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //退菜信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chobackdetailshistory where strcode=''{0}'' " +
                    " and orderdishesid in (select d.orderdishesid from choorderdisheshistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and d.orderno=b.orderno " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //点单明细
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdishesdetailshistory where strcode=''{0}'' " +
                    " and orderdishesid in (select d.orderdishesid from choorderdisheshistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and d.orderno=b.orderno " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //做法加价
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete cdmethodpricehistory where stocode=''{0}'' " +
                    " and orderdishesid in (select d.orderdishesid from choorderdisheshistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and d.orderno=b.orderno " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //套餐


                //点单单信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderhistory where strcode=''{0}'' and orderid in " +
                    " ( select d.orderid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   )  ; ", strStocode, strShiftID));

                //点单单明细信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdetailhistory where stocode=''{0}'' and detailid in " +
                    " ( select d.detailid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   )  ; ", strStocode, strShiftID));

                //点单信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdisheshistory where stocode=''{0}'' " +
                    " and orderno in (select b.orderno from choorderdetailBreakhistory b where b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));


                //预订信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete busDestineTablehistory where stocode=''{0}'' " +
                    " and desCode in (select b.desCode from busDestinehistory b where b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                sbDeleteShiftDataSql.AppendLine(string.Format(" delete busDestinehistory where stocode=''{0}'' and shiftid={1} ;", strStocode, strShiftID));

                //会员卡业务信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete memcardordershistory where stocode=''{0}'' and shiftid={1} ;", strStocode, strShiftID));

                //优惠券信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdetailcouponhistory where orderno in(select orderno from choorderdetailBreakhistory where shiftid={1}  and  stocode=''{0}'');  ", strStocode, strShiftID));

                //支付的营业内收入信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chopayincomehistory where strcode=''{0}'' and shiftid={1} ;", strStocode, strShiftID));

                //支付信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chopayhistory where strcode=''{0}'' and shiftid={1} ;", strStocode, strShiftID));

                //mispos支付记录
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chopayMisposDetailhistory where stocode=''{0}'' and shiftid={1} ; ", strStocode, strShiftID));

                //账单信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdetailBreakhistory where stocode=''{0}'' and shiftid={1} ; ", strStocode, strShiftID));

                //批量验券信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete couponchecklog where strcode=''{0}'' and shiftid={1} ; ", strStocode, strShiftID));

                //预收款信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete bookmoneylog where stocode=''{0}'' and shiftid={1} ; ", strStocode, strShiftID));

                //班次信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chopenshifthistory where strcode=''{0}'' and shiftid={1} ; ", strStocode, strShiftID));

                #endregion

                if (CreateBaseFileToHistory(strBuscode, strStocode, strShiftID, sbUploadTables.ToString(), dicTableUniqueColumnName, sbDeleteShiftDataSql.ToString()))
                {
                    strExportResult = "生成成功";
                }
                else
                {
                    strExportResult = "生成失败";
                }


            }
            catch (Exception er)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, er.Message);
            }

            return strExportResult;
        }

        //导入备份文件夹（uploadsqlbak)
        public string ExportShiftDataForHistoryBak(string strBuscode, string strStocode, string strShiftID)
        {
            string strExportResult = string.Empty;

            try
            {
                //生成连锁端数据
                StringBuilder sbUploadTables = new StringBuilder();

                string strHistory = string.Empty;

                sbUploadTables.AppendFormat(" select *,ishistory='1' from chopenshifthistory where shiftid={0};", strShiftID);
                DataTable dtShift = DBHelper.ExecuteDataTable(sbUploadTables.ToString());
                if (dtShift != null && dtShift.Rows.Count > 0)
                {
                    if (dtShift.Rows[0]["ishistory"].ToString().Trim() == "1")
                    {
                        strHistory = "history";
                    }
                }
                else
                {
                    strExportResult = "不存在该班次，请确认。";
                    return strExportResult;
                }

                sbUploadTables.Clear();
                //临时数据
                sbUploadTables.AppendFormat(" DECLARE @chopenshift TABLE (shiftid BIGINT,buscode varchar(16),strcode varchar(16)); ");
                sbUploadTables.AppendFormat(" INSERT INTO @chopenshift SELECT shiftid,buscode,strcode FROM chopenshift" + strHistory + " WHERE shiftid={0} ; ", strShiftID);
                sbUploadTables.AppendFormat(" DECLARE @choorderdetailBreak TABLE(detailcodes varchar(5120),orderno varchar(32), shiftid BIGINT,expendtype char(1),pstatus char(1)); ");
                sbUploadTables.AppendFormat(" INSERT INTO @choorderdetailBreak SELECT detailcodes ,orderno,shiftid ,expendtype,pstatus FROM choorderdetailBreak" + strHistory + " ");
                sbUploadTables.AppendFormat(" WHERE shiftid IN (SELECT shiftid FROM @chopenshift ) AND [pstatus] IN ('1','2','4','5') ; ");
                sbUploadTables.AppendFormat(" DECLARE @choorderdetail TABLE(detailid BIGINT,orderid BIGINT); ");
                sbUploadTables.AppendFormat(" INSERT @choorderdetail SELECT a.detailid,a.orderid  ");
                sbUploadTables.AppendFormat(" FROM choorderdetail" + strHistory + "  a INNER JOIN @choorderdetailBreak b ON CHARINDEX(','+a.detailcode+',',','+b.detailcodes+',')>0 WHERE b.pstatus<>'5' ;  ");
                sbUploadTables.AppendFormat(" DECLARE @choorderdishes TABLE(orderdishesid BIGINT,porderdishesid BIGINT); ");
                sbUploadTables.AppendFormat(" INSERT INTO @choorderdishes SELECT orderdishesid,porderdishesid FROM choorderdishes" + strHistory + "  WHERE detailid IN (SELECT detailid FROM @choorderdetail ); ");
                sbUploadTables.AppendFormat(" DECLARE @busDestine TABLE(ID BIGINT,desCode varchar(32)); ");
                sbUploadTables.AppendFormat(" INSERT INTO @busDestine SELECT ID,desCode FROM busDestine" + strHistory + "  WHERE desCode IN (SELECT detailcodes AS desCode FROM @choorderdetailBreak WHERE pstatus<>'5' ); ");
                //获取表数据
                sbUploadTables.AppendFormat("select a.*,'cholossdetail' as tbname FROM cholossdetail" + strHistory + "  a INNER JOIN choloss" + strHistory + "  b ON a.losscode=b.losscode WHERE b.detailid IN (SELECT detailid FROM @choorderdetail );");
                sbUploadTables.AppendFormat("select b.*,'choloss' as tbname from choloss" + strHistory + "  b WHERE b.detailid IN (SELECT detailid FROM @choorderdetail );");
                sbUploadTables.AppendFormat("select *,'choorderdetailsurcharge' as tbname  FROM choorderdetailsurcharge" + strHistory + "  WHERE detailid IN (SELECT detailid FROM @choorderdetail );");
                sbUploadTables.AppendFormat("select *,'chobackdetails' as tbname  FROM chobackdetails" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'choorderdishesdetails' as tbname FROM choorderdishesdetails" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'cdmethodprice' as tbname FROM cdmethodprice" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'chopackage' as tbname  FROM chopackage" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'choorderdishes' as tbname  FROM choorderdishes" + strHistory + "  WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );");
                sbUploadTables.AppendFormat("select *,'choorder' as tbname  FROM choorder" + strHistory + "  WHERE orderid IN (SELECT orderid FROM @choorderdetail GROUP BY orderid );");
                sbUploadTables.AppendFormat("select *,'choorderdetail' as tbname  FROM choorderdetail" + strHistory + "  WHERE detailid IN (SELECT detailid FROM @choorderdetail);");
                sbUploadTables.AppendFormat("select *,'paymentDetails' as tbname  FROM paymentDetails" + strHistory + "  WHERE busId IN (SELECT ID AS busId FROM @busDestine );");
                sbUploadTables.AppendFormat("select *,'busDestineTable' as tbname  FROM busDestineTable" + strHistory + "  WHERE desCode IN (SELECT desCode FROM @busDestine );");
                sbUploadTables.AppendFormat("select *,'busDestine' as tbname FROM busDestine" + strHistory + "  WHERE Id IN (SELECT ID AS busId FROM @busDestine );");
                sbUploadTables.AppendFormat("select *,'memcardorders' as tbname FROM memcardorders" + strHistory + "  WHERE ordercode IN (SELECT detailcodes AS ordercode FROM @choorderdetailBreak WHERE pstatus<>'5');");
                sbUploadTables.AppendFormat("select *,'choorderdetailcoupon' as tbname  FROM choorderdetailcoupon" + strHistory + "  WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak );");
                sbUploadTables.AppendFormat("select *,'chopayincome' as tbname  FROM chopayincome" + strHistory + "  WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak );");
                sbUploadTables.AppendFormat("select *,'chopay' as tbname  FROM chopay" + strHistory + "  WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak );");
                sbUploadTables.AppendFormat("select a.*,'chopayMisposDetail' as tbname FROM chopayMisposDetail" + strHistory + "  a inner JOIN @chopenshift b on a.shiftid=b.shiftid;");
                sbUploadTables.AppendFormat("select *,'choorderdetailBreak' as tbname  FROM choorderdetailBreak" + strHistory + "  WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak );");
                sbUploadTables.AppendFormat("select a.*,'chopenshiftmoney' as tbname FROM chopenshiftmoney" + strHistory + "  a inner JOIN @chopenshift b on a.shiftid=b.shiftid;");
                sbUploadTables.AppendFormat("select a.*,'chopenshift' as tbname FROM chopenshift" + strHistory + "  a inner JOIN @chopenshift b on a.shiftid=b.shiftid;");
                //批量验券
                sbUploadTables.AppendFormat("select *,'couponchecklog' as tbname FROM couponchecklog where shiftid in(select shiftid from @chopenshift) ;");

                #region 表关键字典字典
                Dictionary<string, string> dicTableUniqueColumnName = new Dictionary<string, string>();
                dicTableUniqueColumnName.Add("cholossdetailhistory", "cldid,stocode");
                dicTableUniqueColumnName.Add("cholosshistory", "outofid,stocode");
                dicTableUniqueColumnName.Add("choorderdetailsurchargehistory", "surchargeid,stocode");
                dicTableUniqueColumnName.Add("chobackdetailshistory", "backdetailsid,strcode");
                dicTableUniqueColumnName.Add("choorderdishesdetailshistory", "orderdetails,strcode");
                dicTableUniqueColumnName.Add("cdmethodpricehistory", "cdmp,stocode");
                dicTableUniqueColumnName.Add("chopackagehistory", "packageid,stocode");
                dicTableUniqueColumnName.Add("choorderdisheshistory", "orderdishesid,stocode");
                dicTableUniqueColumnName.Add("choorderhistory", "orderid,strcode");
                dicTableUniqueColumnName.Add("choorderdetailhistory", "detailid,stocode");
                dicTableUniqueColumnName.Add("paymentDetailshistory", "payId,strcode");
                dicTableUniqueColumnName.Add("busDestineTablehistory", "dtid,stocode");
                dicTableUniqueColumnName.Add("busDestinehistory", "ID,stocode");
                dicTableUniqueColumnName.Add("memcardordershistory", "ID,stocode");
                dicTableUniqueColumnName.Add("choorderdetailcouponhistory", "paydcid,strcode");
                dicTableUniqueColumnName.Add("chopayincomehistory", "incomid,strcode");
                dicTableUniqueColumnName.Add("chopayhistory", "payid,strcode");
                dicTableUniqueColumnName.Add("chopayMisposDetailhistory", "mispid,stocode");
                dicTableUniqueColumnName.Add("choorderdetailBreakhistory", "detailbreakid,stocode");
                dicTableUniqueColumnName.Add("chopenshiftmoneyhistory", "shiftmid,strcode");
                dicTableUniqueColumnName.Add("chopenshifthistory", "shiftid,strcode");
                dicTableUniqueColumnName.Add("couponchecklog", "ccid,strcode");
                #endregion

                #region 先删除本门店该班次的所有数据
                StringBuilder sbDeleteShiftDataSql = new StringBuilder();
                //删除损赔
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete cholossdetailhistory where stocode=''{0}'' and losscode in ( select losscode from cholosshistory where stocode=''{0}'' " +
                    " and detailid in (select d.detailid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ) ; ", strStocode, strShiftID));
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete cholosshistory where stocode=''{0}'' " +
                    " and detailid in (select d.detailid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //附加费
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdetailsurchargehistory where stocode=''{0}'' " +
                    " and detailid in (select d.detailid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //退菜信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chobackdetailshistory where strcode=''{0}'' " +
                    " and orderdishesid in (select d.orderdishesid from choorderdisheshistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and d.orderno=b.orderno " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //点单明细
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdishesdetailshistory where strcode=''{0}'' " +
                    " and orderdishesid in (select d.orderdishesid from choorderdisheshistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and d.orderno=b.orderno " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //做法加价
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete cdmethodpricehistory where stocode=''{0}'' " +
                    " and orderdishesid in (select d.orderdishesid from choorderdisheshistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and d.orderno=b.orderno " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));
                //套餐

                //点单单信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderhistory where strcode=''{0}'' and orderid in " +
                    " ( select d.orderid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   )  ; ", strStocode, strShiftID));

                //点单单明细信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdetailhistory where stocode=''{0}'' and detailid in " +
                    " ( select d.detailid from choorderdetailhistory d inner join choorderdetailBreakhistory b on d.stocode=b.stocode and  CHARINDEX('',''+d.detailcode+'','','',''+b.detailcodes+'','')>0 " +
                    " where d.stocode=''{0}'' and b.stocode=''{0}'' and b.shiftid={1}   )  ; ", strStocode, strShiftID));

                //点单信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdisheshistory where stocode=''{0}'' " +
                    " and orderno in (select b.orderno from choorderdetailBreakhistory b where b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                //预订信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete busDestineTablehistory where stocode=''{0}'' " +
                    " and desCode in (select b.desCode from busDestinehistory b where b.stocode=''{0}'' and b.shiftid={1}   ) ; ", strStocode, strShiftID));

                sbDeleteShiftDataSql.AppendLine(string.Format(" delete busDestinehistory where stocode=''{0}'' and shiftid={1} ;", strStocode, strShiftID));

                //会员卡业务信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete memcardordershistory where stocode=''{0}'' and shiftid={1} ;", strStocode, strShiftID));

                //优惠券信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdetailcouponhistory where orderno in(select orderno from choorderdetailBreakhistory where shiftid={1}  and  stocode=''{0}'');  ", strStocode, strShiftID));

                //支付的营业内收入信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chopayincomehistory where strcode=''{0}'' and shiftid={1} ;", strStocode, strShiftID));

                //支付信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chopayhistory where strcode=''{0}'' and shiftid={1} ;", strStocode, strShiftID));

                //mispos支付记录
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chopayMisposDetailhistory where stocode=''{0}'' and shiftid={1} ; ", strStocode, strShiftID));

                //账单信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete choorderdetailBreakhistory where stocode=''{0}'' and shiftid={1} ; ", strStocode, strShiftID));

                //批量验券信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete couponchecklog where strcode=''{0}'' and shiftid={1} ; ", strStocode, strShiftID));

                //班次信息
                sbDeleteShiftDataSql.AppendLine(string.Format(" delete chopenshifthistory where strcode=''{0}'' and shiftid={1} ; ", strStocode, strShiftID));

                #endregion

                if (CreateBaseFileToHistoryBak(strBuscode, strStocode, strShiftID, sbUploadTables.ToString(), dicTableUniqueColumnName, sbDeleteShiftDataSql.ToString()))
                {
                    strExportResult = "生成成功";
                }
                else
                {
                    strExportResult = "生成失败";
                }
            }
            catch (Exception er)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, er.Message);
            }

            return strExportResult;
        }

        /// <summary>
        /// 生成上传文件(上传班次数据专用)
        /// </summary>
        /// <param name="stocode">门店编号</param>
        /// <param name="Shiftid">班次号</param>
        /// <param name="TablesSql">获取表sql语句</param>
        /// <param name="dicTableUniqueColumnName">表的唯一值字段名</param>
        /// <returns></returns>
        private bool CreateBaseFileToHistory(string buscode, string stocode, string Shiftid, string TablesSql, Dictionary<string, string> dicTableUniqueColumnName, string DeleteShiftDataSql)
        {
            bool isCreate = true;
            DataSet ds = null;

            try
            {
                #region 先删除文件
                //获取文件路径（根目录）
                string path = "d:\\";
                DataTable dt = DBHelper.ExecuteDataTable("select top 1 datapath from StoreUploaddatapath where stocode='" + stocode + "';");
                if (dt != null && dt.Rows.Count > 0)
                {
                    path = dt.Rows[0]["datapath"].ToString();
                }
                string strFilename = path + "busdata_" + buscode + "_" + stocode + "_" + Shiftid + ".sql";
                //object objFilename = DBHelper.ExecuteScalar(string.Format("SELECT datapath from StoreUploaddatapath WHERE stocode='{0}';", stocode));
                //if (objFilename != null && objFilename.ToString().Trim() != "")
                //{
                //    strFilename = objFilename.ToString().Trim() + "busdata_" + buscode + "_" + stocode + "_" + Shiftid + ".sql;";
                //}
                if (File.Exists(strFilename))
                {
                    File.Delete(strFilename);
                }
                #endregion

                StringBuilder BuilderTotal = new StringBuilder();
                SqlParameter[] sqlParameters = 
                {
                    new SqlParameter("@stocode", stocode)
                 };
                //获取要生成数据的表
                ds = DBHelper.ExecuteDataSet(TablesSql);


                if (ds != null && ds.Tables.Count > 0)
                {
                    int count = ds.Tables.Count;
                    if (count > 0)
                    {
                        StringBuilder Builder = new StringBuilder();
                        if (!string.IsNullOrEmpty(DeleteShiftDataSql) && DeleteShiftDataSql.Trim() != "")
                        {
                            //先删除该班次的所有业务数据
                            Builder.AppendLine(DeleteShiftDataSql);
                            Builder.AppendLine(";");
                        }

                        int InsertCount = 0;

                        foreach (DataTable dtNames in ds.Tables)
                        {
                            #region 循环读取表生成数据
                            if (dtNames != null && dtNames.Rows.Count > 0)
                            {
                                //获取表名称
                                string tbname = dtNames.Rows[0]["tbname"].ToString(); // 表名
                                if (!tbname.ToLower().Contains("history")&&tbname.ToLower().Trim() != "couponchecklog")
                                {
                                    tbname += "history";
                                }

                                bool blnHaveShiftidColumn = false;
                                if (dtNames.Columns.Contains("shiftid") && tbname.ToLower().Trim() != "choorderdetailhistory" && tbname.ToLower().Trim() != "choorderhistory" && tbname.ToLower().Trim() != "busDestinehistory")
                                { //先删除该班次数据
                                    blnHaveShiftidColumn = true;
                                    string strDelFilter = string.Empty;
                                    if (dtNames.Columns.Contains("stocode"))
                                    {
                                        strDelFilter = string.Format(" and stocode=''{0}'' ", stocode);
                                    }
                                    else if (dtNames.Columns.Contains("strcode"))
                                    {
                                        strDelFilter = string.Format(" and  strcode=''{0}'' ", stocode);
                                    }

                                    Builder.AppendLine(string.Format("delete " + tbname + " where shiftid={0} " + strDelFilter + " ;", Shiftid));
                                }

                                Builder.AppendLine(BuilderSqlToHistory(tbname, dtNames, "0", stocode, dicTableUniqueColumnName, blnHaveShiftidColumn).ToString());

                                if (Builder.Length >= 100000)
                                {
                                    BuilderTotal.AppendLine("DECLARE @buscode VARCHAR(16);DECLARE @stocode VARCHAR(16);");
                                    BuilderTotal.AppendLine("DECLARE @SqlDataPath VARCHAR(128);declare @uploadfilename VARCHAR(256);");
                                    BuilderTotal.AppendLine("DECLARE @Result int;DECLARE @FSO_Token int;");

                                    BuilderTotal.AppendFormat("select top 1 @stocode=isnull(strcode,''),@buscode=isnull(buscode,'') from sto_admins where strcode='{0}' ;", stocode);
                                    BuilderTotal.AppendLine("if(@buscode<>'' and @stocode<>'') ");
                                    BuilderTotal.AppendLine(" begin ");
                                    //获取文件存放路径
                                    BuilderTotal.AppendLine("SELECT  @SqlDataPath=datapath from StoreUploaddatapath WHERE buscode=@buscode AND stocode=@stocode;");
                                    BuilderTotal.AppendLine("IF(@SqlDataPath IS NULL OR @SqlDataPath='') ");
                                    BuilderTotal.AppendLine(" begin ");
                                    BuilderTotal.AppendLine(" SET @SqlDataPath='d:\'; ");
                                    BuilderTotal.AppendLine(" end ");
                                    //生成文件名称
                                    BuilderTotal.AppendLine("SET @uploadfilename=@SqlDataPath +'busdata_'+@buscode+'_'+@stocode+'_" + Shiftid + ".sql';");



                                    //生成文件
                                    BuilderTotal.AppendLine("EXEC dbo.p_WriteTextfile @uploadfilename,'" + Builder.ToString() + "';");
                                    BuilderTotal.AppendLine(" end ");
                                    bool blnExecResult = DBHelper.ExecuteNonQuery3(BuilderTotal.ToString(), CommandType.Text);
                                    InsertCount++;
                                    if (blnExecResult)
                                    {
                                        Builder.Clear();
                                        BuilderTotal.Clear();
                                    }
                                    else
                                    {
                                        BuilderTotal.Clear();
                                        return false;
                                    }
                                }
                            }
                            #endregion
                        }

                        //生成基础数据文件
                        if (Builder.Length > 0)
                        {
                            BuilderTotal.AppendLine("DECLARE @buscode VARCHAR(16);DECLARE @stocode VARCHAR(16);");
                            BuilderTotal.AppendLine("DECLARE @SqlDataPath VARCHAR(128);declare @uploadfilename VARCHAR(256);");
                            BuilderTotal.AppendLine("DECLARE @Result int;DECLARE @FSO_Token int;");

                            BuilderTotal.AppendFormat("select top 1 @stocode=isnull(strcode,''),@buscode=isnull(buscode,'') from sto_admins where strcode='{0}' ;", stocode);
                            BuilderTotal.AppendLine("if(@buscode<>'' and @stocode<>'') ");
                            BuilderTotal.AppendLine(" begin ");
                            //获取文件存放路径
                            BuilderTotal.AppendLine("SELECT  @SqlDataPath=datapath from StoreUploaddatapath WHERE buscode=@buscode AND stocode=@stocode;");
                            BuilderTotal.AppendLine("IF(@SqlDataPath IS NULL OR @SqlDataPath='') ");
                            BuilderTotal.AppendLine(" begin ");
                            BuilderTotal.AppendLine(" SET @SqlDataPath='d:\'; ");
                            BuilderTotal.AppendLine(" end ");
                            //生成文件名称
                            BuilderTotal.AppendLine("SET @uploadfilename=@SqlDataPath +'busdata_'+@buscode+'_'+@stocode+'_" + Shiftid + ".sql';");
                            //生成文件
                            BuilderTotal.AppendLine("EXEC dbo.p_WriteTextfile @uploadfilename,'" + Builder.ToString() + "';");
                            BuilderTotal.AppendLine(" end ");
                            //ErrorLog.WriteErrorMessage(BuilderTotal.ToString());
                            bool blnExecResult = DBHelper.ExecuteNonQuery3(BuilderTotal.ToString(), CommandType.Text);
                            if (blnExecResult)
                            {
                                isCreate = true;
                            }
                        }
                        else
                        {
                            if (InsertCount == 0)
                            {
                                isCreate = false;
                            }
                            else
                            {
                                isCreate = true;
                            }
                        }
                    }
                    else
                    {
                        isCreate = false;
                    }
                }
                else
                {
                    isCreate = false;
                }

            }
            catch (Exception ex)
            {

            }

            return isCreate;
        }

        /// <summary>
        /// 生成上传文件(上传班次数据专用)(放在bak文件夹)
        /// </summary>
        /// <param name="stocode">门店编号</param>
        /// <param name="Shiftid">班次号</param>
        /// <param name="TablesSql">获取表sql语句</param>
        /// <param name="dicTableUniqueColumnName">表的唯一值字段名</param>
        /// <returns></returns>
        private bool CreateBaseFileToHistoryBak(string buscode, string stocode, string Shiftid, string TablesSql, Dictionary<string, string> dicTableUniqueColumnName, string DeleteShiftDataSql)
        {
            bool isCreate = true;
            DataSet ds = null;

            try
            {
                #region 先删除文件
                //获取文件路径（根目录）
                string path = "d:\\";
                string pathMove = string.Empty;
                DataTable dt = DBHelper.ExecuteDataTable("select top 1 datapath from StoreUploaddatapath where stocode='" + stocode + "';");
                if (dt != null && dt.Rows.Count > 0)
                {
                    path = dt.Rows[0]["datapath"].ToString();
                    path = path.Substring(0, path.Length - 1) + "_bak\\";
                    pathMove = dt.Rows[0]["datapath"].ToString().Substring(0, dt.Rows[0]["datapath"].ToString().Length - 1) + "_mov\\";
                }

                if (!Directory.Exists(path))//若文件夹不存在则新建文件夹
                {
                    Directory.CreateDirectory(path);
                }

                if (!Directory.Exists(pathMove))//若文件夹不存在则新建文件夹
                {
                    Directory.CreateDirectory(pathMove);
                }

                string strFilename = path + "busdata_" + buscode + "_" + stocode + "_" + Shiftid + ".sql";
                string strmoveFilename = pathMove + "busdata_" + buscode + "_" + stocode + "_" + Shiftid + ".sql";

                if (File.Exists(strFilename))
                {
                    File.Delete(strFilename);
                }

                if (File.Exists(strmoveFilename))
                {
                    File.Delete(strmoveFilename);
                }
                #endregion

                StringBuilder BuilderTotal = new StringBuilder();
                SqlParameter[] sqlParameters = 
                {
                    new SqlParameter("@stocode", stocode)
                 };
                //获取要生成数据的表
                ds = DBHelper.ExecuteDataSet(TablesSql);

                if (ds != null && ds.Tables.Count > 0)
                {
                    int count = ds.Tables.Count;
                    if (count > 0)
                    {
                        StringBuilder Builder = new StringBuilder();
                        if (!string.IsNullOrEmpty(DeleteShiftDataSql) && DeleteShiftDataSql.Trim() != "")
                        {
                            //先删除该班次的所有业务数据
                            Builder.AppendLine(DeleteShiftDataSql);
                            Builder.AppendLine(";");
                        }

                        int InsertCount = 0;

                        foreach (DataTable dtNames in ds.Tables)
                        {
                            #region 循环读取表生成数据
                            if (dtNames != null && dtNames.Rows.Count > 0)
                            {
                                //获取表名称
                                string tbname = dtNames.Rows[0]["tbname"].ToString(); // 表名
                                if (!tbname.ToLower().Contains("history") && tbname.ToLower().Trim() != "couponchecklog")
                                {
                                    tbname += "history";
                                }

                                bool blnHaveShiftidColumn = false;
                                if (dtNames.Columns.Contains("shiftid") && tbname.ToLower().Trim() != "choorderdetailhistory" && tbname.ToLower().Trim() != "choorderhistory" && tbname.ToLower().Trim() != "busDestinehistory" )
                                { //先删除该班次数据
                                    blnHaveShiftidColumn = true;
                                    string strDelFilter = string.Empty;
                                    if (dtNames.Columns.Contains("stocode"))
                                    {
                                        strDelFilter = string.Format(" and stocode=''{0}'' ", stocode);
                                    }
                                    else if (dtNames.Columns.Contains("strcode"))
                                    {
                                        strDelFilter = string.Format(" and  strcode=''{0}'' ", stocode);
                                    }

                                    Builder.AppendLine(string.Format("delete " + tbname + " where shiftid={0} " + strDelFilter + " ;", Shiftid));
                                }

                                Builder.AppendLine(BuilderSqlToHistory(tbname, dtNames, "0", stocode, dicTableUniqueColumnName, blnHaveShiftidColumn).ToString());

                                if (Builder.Length >= 100000)
                                {
                                    //生成文件bak文件夹
                                    BuilderTotal.AppendLine("EXEC dbo.p_WriteTextfile '" + strFilename + "','" + Builder.ToString() + "';");
                                    //生成到move文件夹
                                    BuilderTotal.AppendLine("EXEC dbo.p_WriteTextfile '" + strmoveFilename + "','" + Builder.ToString() + "';");
                                    bool blnExecResult = DBHelper.ExecuteNonQuery3(BuilderTotal.ToString(), CommandType.Text);
                                    InsertCount++;
                                    if (blnExecResult)
                                    {
                                        Builder.Clear();
                                        BuilderTotal.Clear();
                                    }
                                    else
                                    {
                                        BuilderTotal.Clear();
                                        return false;
                                    }
                                }
                            }
                            #endregion
                        }

                        //生成数据文件
                        if (Builder.Length > 0)
                        {
                            //生成到bak文件夹
                            BuilderTotal.AppendLine("EXEC dbo.p_WriteTextfile '" + strFilename + "','" + Builder.ToString() + "';");
                            //生成到move文件夹
                            BuilderTotal.AppendLine("EXEC dbo.p_WriteTextfile '" + strmoveFilename + "','" + Builder.ToString() + "';");
                            bool blnExecResult = DBHelper.ExecuteNonQuery3(BuilderTotal.ToString(), CommandType.Text);
                            if (blnExecResult)
                            {
                                isCreate = true;
                            }
                        }
                        else
                        {
                            if (InsertCount == 0)
                            {
                                isCreate = false;
                            }
                            else
                            {
                                isCreate = true;
                            }
                        }
                    }
                    else
                    {
                        isCreate = false;
                    }
                }
                else
                {
                    isCreate = false;
                }

            }
            catch (Exception ex)
            {

            }

            return isCreate;
        }

        /// <summary>
        /// 获取表 insert 语句(插入历史表专用)
        /// </summary>
        /// <param name="tbname">要插入数据的表名称</param>
        /// <param name="dt1">表结果集</param>
        /// <param name="identityvalue">是否自动增长列</param>
        /// <param name="stocode">门店编号</param>
        /// <param name="dicTableUniqueColumnName">表唯一值字段名</param>
        /// <returns></returns>
        private StringBuilder BuilderSqlToHistory(string tbname, DataTable dt1, string identityvalue, string stocode, Dictionary<string, string> dicTableUniqueColumnName, bool blnHaveShiftidColumn)
        {
            StringBuilder Builder = new StringBuilder();
            StringBuilder insertSql = new StringBuilder();
            int index = 0;
            //sql表头名称
            string columnnames = String.Empty;
            //表 值
            string valuenames = String.Empty;
            //表 值类型
            List<string> list = new List<string>();


            //获取表头名称
            foreach (DataColumn dc in dt1.Columns)
            {
                //表是自动增长列表
                if (identityvalue == "1")
                {
                    index++;
                    if (index > 1)
                    {
                        if (dc.ColumnName != "tbname")
                        {
                            columnnames += "[" + dc.ColumnName + "],";
                            list.Add(dc.DataType.ToString());
                        }
                    }
                }
                else
                {
                    if (dc.ColumnName != "tbname")
                    {
                        columnnames += "[" + dc.ColumnName + "],";
                        list.Add(dc.DataType.ToString());
                    }
                }
            }

            insertSql.Append(" insert into dbo." + tbname + "(" + columnnames.TrimEnd(',') + ") values ");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                valuenames = "";
                if (identityvalue == "1")//表是自动增长列表
                {
                    for (int j = 1; j < dt1.Columns.Count - 1; j++)
                    {
                        switch (list[j - 1].ToLower())
                        {
                            case "system.string":
                            case "system.smalldatetime":
                            case "system.datetime":
                                valuenames += "''" + replacestocode(stocode, dt1.Columns[j].ColumnName.ToLower(), dt1.Rows[i][j].ToString(), tbname) + "'',";
                                break;
                            default:
                                if (String.IsNullOrEmpty(dt1.Rows[i][j].ToString()))
                                {
                                    valuenames += "null,";
                                }
                                else
                                {
                                    valuenames += replacestocode(stocode, dt1.Columns[j].ColumnName.ToLower(), dt1.Rows[i][j].ToString().Trim().Replace('\0', ' ').Replace(" ", ""), tbname) + ",";
                                }
                                break;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < dt1.Columns.Count - 1; j++)
                    {
                        switch (list[j].ToLower())
                        {
                            case "system.string":
                            case "system.smalldatetime":
                            case "system.datetime":
                                valuenames += "''" + replacestocode(stocode, dt1.Columns[j].ColumnName.ToLower(), dt1.Rows[i][j].ToString().Trim(), tbname) + "'',";
                                break;
                            default:
                                if (String.IsNullOrEmpty(dt1.Rows[i][j].ToString()))
                                {
                                    valuenames += "null,";
                                }
                                else
                                {
                                    valuenames += replacestocode(stocode, dt1.Columns[j].ColumnName.ToLower(), dt1.Rows[i][j].ToString().Trim().Replace('\0', ' ').Replace(" ", ""), tbname) + ",";
                                }
                                break;
                        }
                    }
                }

                #region //生成前先清除连锁端表数据
                if (!blnHaveShiftidColumn && dicTableUniqueColumnName != null && dicTableUniqueColumnName.Count > 0)
                {
                    string DeleteFilter = string.Empty;
                    string FilterFilds = dicTableUniqueColumnName[tbname];
                    if (!string.IsNullOrEmpty(FilterFilds) && FilterFilds.Trim() != "")
                    {
                        string[] arrrFilds = FilterFilds.Split(',');
                        for (int f = 0; f < arrrFilds.Length; f++)
                        {
                            if (arrrFilds[f].Trim() != "")
                            {
                                if (string.IsNullOrEmpty(DeleteFilter))
                                {
                                    DeleteFilter = " " + arrrFilds[f].Trim() + "=''" + dt1.Rows[i][arrrFilds[f].Trim()].ToString().Trim().Replace('\0', ' ').Replace(" ", "") + "'' ";
                                }
                                else
                                {
                                    DeleteFilter += " and " + arrrFilds[f].Trim() + "=''" + dt1.Rows[i][arrrFilds[f].Trim()].ToString().Trim().Replace('\0', ' ').Replace(" ", "") + "'' ";
                                }
                            }
                        }
                        Builder.AppendLine(" delete from dbo." + tbname + " where " + DeleteFilter + " ;");
                    }
                }
                #endregion

                if (i == dt1.Rows.Count - 1)
                {
                    insertSql.Append("(" + valuenames.TrimEnd(',') + ");");
                }
                else
                {
                    //单次最多插入900条数据
                    if (i>0 && i % 900 == 0)
                    {
                        insertSql.Append("(" + valuenames.TrimEnd(',') + ");");
                        insertSql.Append(" insert into dbo." + tbname + "(" + columnnames.TrimEnd(',') + ") values");
                    }
                    else
                    {
                        insertSql.Append("(" + valuenames.TrimEnd(',') + "),");
                    }
                }
            }
            return Builder.Append(insertSql.ToString());
        }


        private string replacestocode(string stocode, string colname, string val, string tbname)
        {
            string strreturn = val;

            if (colname == "strcode" || colname == "stocode")
            {
                strreturn = stocode;
            }
            return strreturn;
        }


        #endregion


        #region 提取需要出库的原料进行成本计算

        /// <summary>
        /// 将消费单数据导入到历史表
        /// </summary>
        /// <param name="buscode"></param>
        /// <param name="stocode"></param>
        /// <param name="shiftid"></param>
        /// <param name="orderno"></param>
        /// <param name="detailcodes"></param>
        /// <returns></returns>
        public bool OrderDataImportToHistory(string buscode, string stocode, long shiftid, string orderno, string detailcodes)
        {
            bool blnResult = false;
            StringBuilder sbSql = new StringBuilder();
            #region 构造导入语句
            sbSql.AppendLine(" BEGIN TRAN tan_ImportToHistory ");


            sbSql.AppendLine(" DECLARE @InsertSql NVARCHAR(MAX); ");
            sbSql.AppendLine(" set @InsertSql=''; ");
            sbSql.AppendLine(" declare @filter VARCHAR(400); ");
            sbSql.AppendLine(" declare @IDS VARCHAR(400); ");
            sbSql.AppendLine(" SET @IDS=''; ");
            sbSql.AppendLine(" DECLARE @orderid BIGINT; ");

            sbSql.AppendLine(" DECLARE @buscode VARCHAR(16);");
            sbSql.AppendLine(" DECLARE @stocode VARCHAR(16)");
            sbSql.AppendLine(" DECLARE @shiftid BIGINT");
            sbSql.AppendLine(" DECLARE @orderno VARCHAR(32)");
            sbSql.AppendLine(" DECLARE @detailcode VARCHAR(32)");


            sbSql.AppendLine(string.Format(" set @buscode='{0}' ;", buscode));
            sbSql.AppendLine(string.Format(" set @stocode='{0}' ;", stocode));
            sbSql.AppendLine(string.Format(" set @shiftid={0} ;", shiftid.ToString()));
            sbSql.AppendLine(string.Format(" set @orderno='{0}' ;", orderno));
            sbSql.AppendLine(string.Format(" set @detailcode='{0}' ;", detailcodes));


            sbSql.AppendLine(" DECLARE @chopenshift TABLE (shiftid BIGINT,buscode varchar(16),strcode varchar(16));  ");
            sbSql.AppendLine(" INSERT INTO @chopenshift SELECT shiftid,buscode,strcode FROM chopenshift WHERE [status]='1' AND shiftid=ISNULL(@shiftid,0) ; ");

            sbSql.AppendLine(" DECLARE @choorderdetailBreak TABLE(detailcodes varchar(5120),orderno varchar(32), shiftid BIGINT,expendtype char(1),pstatus char(1) ); ");
            sbSql.AppendLine(" IF ISNULL(@orderno,'')='' ");
            sbSql.AppendLine(" begin ");
            sbSql.AppendLine(" INSERT INTO @choorderdetailBreak SELECT detailcodes ,orderno,shiftid ,expendtype,pstatus FROM choorderdetailBreak  ");
            sbSql.AppendLine(" WHERE shiftid=@shiftid AND [pstatus] IN ('1','2','4','5') ; ");
            sbSql.AppendLine(" END ");
            sbSql.AppendLine(" ELSE ");
            sbSql.AppendLine(" BEGIN ");
            sbSql.AppendLine(" INSERT INTO @choorderdetailBreak SELECT detailcodes ,orderno,shiftid ,expendtype,pstatus FROM choorderdetailBreak  ");
            sbSql.AppendLine(" WHERE orderno=@orderno AND [pstatus] IN ('1','2','4','5') ; ");
            sbSql.AppendLine(" END ");



            sbSql.AppendLine(" DECLARE @choorderdetail TABLE(detailid BIGINT,orderid BIGINT); ");
            sbSql.AppendLine(" IF ISNULL(@detailcode,'')='' ");
            sbSql.AppendLine(" begin ");
            sbSql.AppendLine(" INSERT @choorderdetail SELECT a.detailid,a.orderid  ");
            sbSql.AppendLine("  FROM choorderdetail a INNER JOIN @choorderdetailBreak b ON CHARINDEX(','+a.detailcode+',',','+b.detailcodes+',')>0 WHERE b.pstatus<>'5' ;  ");
            sbSql.AppendLine(" END ");
            sbSql.AppendLine(" ELSE ");
            sbSql.AppendLine(" BEGIN ");
            sbSql.AppendLine("     INSERT @choorderdetail SELECT a.detailid,a.orderid  ");
            sbSql.AppendLine(string.Format("      FROM choorderdetail a WHERE CHARINDEX(','+a.detailcode+',',','+@detailcode+',')>0 ;  "));
            sbSql.AppendLine(" END ");


            sbSql.AppendLine(" IF EXISTS(SELECT TOP 1 * FROM @choorderdetail) ");
            sbSql.AppendLine(" begin ");

            sbSql.AppendLine("  DECLARE  @batuid VARCHAR(64); ");
            sbSql.AppendLine("  SET @batuid= NEWID(); ");
            sbSql.AppendLine("  DECLARE @ctime DATETIME; ");
            sbSql.AppendLine("  SET @ctime=GETDATE(); ");

            /*
            sbSql.AppendLine("  DECLARE @tb_UsedMates tsg_TableType ; ");
            sbSql.AppendLine(" INSERT INTO @tb_UsedMates ");
            sbSql.AppendLine("  SELECT '' as dmuid, @batuid AS batuid, b.buscode,b.stocode,b.detailid,b.detailcode,b.orderdishesid,b.ctime as constime ");
            sbSql.AppendLine("  ,b.distypecode,b.dtypecode,dbo.f_GetDishTypeName(b.dtypecode) AS dtypename,b.discode,b.disname,b.disothername ");
            sbSql.AppendLine("  ,(b.refundaddnum+b.disnum) AS disnum,b.oneprice AS disprice,b.comprice,b.warcode ");
            sbSql.AppendLine("  ,d.mattypecode,dbo.fnGetStockMateTypeName(d.mattypecode) AS mattypename,a.matcode,d.matname,d.matsimplename ");
            sbSql.AppendLine("  ,0 AS methodnum  ");
            sbSql.AppendLine("  ,a.useamount AS matnum ");
            sbSql.AppendLine("  ,a.unitcode ");
            sbSql.AppendLine("  ,[dbo].[fnGetUnitNameByCode](a.unitcode) AS smunitname ");
            sbSql.AppendLine("  ,d.smsalprice AS matprice ");
            sbSql.AppendLine("  ,a.useamount * d.smsalprice AS matamount ");
            sbSql.AppendLine("  ,e.warcode AS matwarcode ");
            sbSql.AppendLine("  ,b.methodmoney ");
            sbSql.AppendLine("  ,'0' AS relatype ");
            sbSql.AppendLine("  ,b.isneedweigh ");
            sbSql.AppendLine("  ,b.discode as relacode ");
            sbSql.AppendLine("  ,b.disname AS relaname ");
            sbSql.AppendLine("  ,a.useamount AS relanum  ");
            sbSql.AppendLine("  ,ISNULL(c.endtime,'1900-01-01') AS endtime ");
            sbSql.AppendLine("  ,CONVERT(varchar(100),ISNULL(c.endtime,'1900-01-01'), 23)  AS checkouttime ");
            sbSql.AppendLine("  ,b.dtypecode AS typecode ");
            sbSql.AppendLine("  ,dbo.f_GetDishTypeName(b.dtypecode) AS typename ");
            sbSql.AppendLine("  FROM DishesMate a INNER JOIN dbo.choorderdishes b  ");
            sbSql.AppendLine("  ON a.buscode=b.buscode AND a.stocode=b.stocode AND a.discode=b.discode AND ISNULL(b.porderdishesid,0)=0   and isnull(b.iscaninventory,'')<>'1' ");
            sbSql.AppendLine("  INNER JOIN choorderdetail c ON b.detailid=c.detailid ");
            sbSql.AppendLine("  INNER JOIN @choorderdetail t ON c.detailid=t.detailid ");
            sbSql.AppendLine("  LEFT OUTER JOIN StockMaterial d ");
            sbSql.AppendLine("  ON a.buscode=d.buscode AND a.stocode=d.stocode AND a.matcode=d.matcode ");
            sbSql.AppendLine("  LEFT OUTER JOIN Stock e  ");
            sbSql.AppendLine("   ON a.buscode=e.buscode AND a.stocode=e.stocode AND a.matcode=e.matcode ");
            sbSql.AppendLine("   union ");
            sbSql.AppendLine("  SELECT '' as dmuid, @batuid AS batuid,b.buscode,b.stocode,b.detailid,b.detailcode,b.orderdishesid,b.ctime as constime ");
            sbSql.AppendLine("   ,b.distypecode,b.dtypecode,dbo.f_GetDishTypeName(b.dtypecode) AS dtypename,b.discode,b.disname,b.disothername ");
            sbSql.AppendLine("  ,(b.refundaddnum+b.disnum) AS disnum,b.oneprice AS disprice,b.comprice,b.warcode ");
            sbSql.AppendLine("  ,d.mattypecode,dbo.fnGetStockMateTypeName(d.mattypecode) AS mattypename, a.matcode,d.matname,d.matsimplename ");
            sbSql.AppendLine("  ,c.addamount*(b.refundaddnum+b.disnum) AS methodnum  ");
            sbSql.AppendLine("  ,a.useamount AS matnum ");
            sbSql.AppendLine("  ,a.unitcode ");
            sbSql.AppendLine("  ,[dbo].[fnGetUnitNameByCode](a.unitcode) AS smunitname ");
            sbSql.AppendLine("  ,d.smsalprice AS matprice ");
            sbSql.AppendLine("  ,c.addamount*(b.refundaddnum+b.disnum)*a.useamount * d.smsalprice AS matamount ");
            sbSql.AppendLine("  ,e.warcode AS matwarcode ");
            sbSql.AppendLine("  ,b.methodmoney ");
            sbSql.AppendLine("  ,'1' AS relatype ");
            sbSql.AppendLine("  ,b.isneedweigh ");
            sbSql.AppendLine("  ,c.methodcode as relacode ");
            sbSql.AppendLine("  ,c.methodname AS relaname ");
            sbSql.AppendLine("  ,a.useamount AS relanum  ");
            sbSql.AppendLine("  ,ISNULL(f.endtime,'1900-01-01') AS endtime ");
            sbSql.AppendLine("  ,CONVERT(varchar(100),ISNULL(f.endtime,'1900-01-01'), 23)  AS checkouttime ");
            sbSql.AppendLine("  ,dbo.f_GetMethodTypeCodeByMethodcode(a.methodcode) AS typecode ");
            sbSql.AppendLine("  ,dbo.f_GetMethodTypeNameByMethodcode(a.methodcode) AS typename ");
            sbSql.AppendLine("   FROM MethodsMate a   ");
            sbSql.AppendLine("   INNER JOIN cdmethodprice c ");
            sbSql.AppendLine("    ON a.buscode=c.buscode AND a.stocode=c.stocode AND a.methodcode=c.methodcode  ");
            sbSql.AppendLine("  INNER JOIN dbo.choorderdishes b  ");
            sbSql.AppendLine("   ON c.buscode=b.buscode AND c.stocode=b.stocode AND c.orderdishesid=b.orderdishesid and c.discode=b.discode  ");
            sbSql.AppendLine("   INNER JOIN choorderdetail f ON b.detailid=f.detailid ");
            sbSql.AppendLine("   INNER JOIN @choorderdetail t ON f.detailid=t.detailid ");
            sbSql.AppendLine("   LEFT OUTER JOIN StockMaterial d ");
            sbSql.AppendLine("    ON a.buscode=d.buscode AND a.stocode=d.stocode AND a.matcode=d.matcode ");
            sbSql.AppendLine("   LEFT OUTER JOIN Stock e  ");
            sbSql.AppendLine("   ON a.buscode=e.buscode AND a.stocode=e.stocode AND a.matcode=e.matcode ");
            sbSql.AppendLine("     ; ");

            sbSql.AppendLine("   DECLARE @DisheMaterialMain table ( ");
            sbSql.AppendLine("    dmuid                varchar(64)          null, ");
            sbSql.AppendLine("    batuid               varchar(64)          null, ");
            sbSql.AppendLine("   buscode              varchar(16)          null, ");
            sbSql.AppendLine("    stocode              varchar(8)           null, ");
            sbSql.AppendLine("    orderdishesids       varchar(max)         null, ");
            sbSql.AppendLine("    checkouttime         datetime             null, ");
            sbSql.AppendLine("    relatype             char(1)              null, ");
            sbSql.AppendLine("   relacode             varchar(16)          null, ");
            sbSql.AppendLine("   relaname             nvarchar(32)         null, ");
            sbSql.AppendLine("  relanum              decimal(18,3)        null, ");
            sbSql.AppendLine("  relawarcode          varchar(16)          null, ");
            sbSql.AppendLine("  matamount			decimal(18,3)        null, ");
            sbSql.AppendLine(" ctime                datetime             NULL, ");
            sbSql.AppendLine(" disprice				decimal(18,2)        null, ");
            sbSql.AppendLine("  typecode             varchar(16)          null, ");
            sbSql.AppendLine("  typename             nvarchar(16)         null ");
            sbSql.AppendLine("  ) ");
            sbSql.AppendLine("   ; ");

            sbSql.AppendLine("  INSERT INTO @DisheMaterialMain ");
            sbSql.AppendLine("  SELECT NEWID() AS dmuid, @batuid AS batuid,buscode,stocode ");
            sbSql.AppendLine("  ,dbo.f_GetODIDs(buscode,stocode,relacode,relaname,relawarcode,relatype,@tb_UsedMates) AS  orderdishesids ");
            sbSql.AppendLine("  ,checkouttime,relatype ");
            sbSql.AppendLine("  ,relacode,relaname ");
            sbSql.AppendLine("  ,sum(relanum) AS relanum ");
            sbSql.AppendLine("  ,relawarcode ");
            sbSql.AppendLine("  ,sum(matamount) AS matamount ");
            sbSql.AppendLine("  ,@ctime AS ctime ");
            sbSql.AppendLine("   ,MAX(disprice) AS disprice ");
            sbSql.AppendLine("   ,typecode,typename ");
            sbSql.AppendLine("   FROM ");
            sbSql.AppendLine("    ( ");
            sbSql.AppendLine("    SELECT buscode,stocode ");
            sbSql.AppendLine("   ,checkouttime,relatype ");
            sbSql.AppendLine("   ,relacode,relaname ");
            sbSql.AppendLine("   ,MAX(disnum) AS relanum ");
            sbSql.AppendLine("   ,warcode AS relawarcode ");
            sbSql.AppendLine("  ,@ctime AS ctime ");
            sbSql.AppendLine("   ,MAX(disprice) AS disprice ");
            sbSql.AppendLine("   ,sum(matamount) AS matamount  ");
            sbSql.AppendLine("   ,typecode,typename ");
            sbSql.AppendLine("     FROM @tb_UsedMates ");
            sbSql.AppendLine("     WHERE relatype='0' ");
            sbSql.AppendLine("       GROUP BY buscode,stocode,orderdishesid,relacode,relaname,warcode,checkouttime,relatype,typecode,typename	 ");
            sbSql.AppendLine("    ) d GROUP BY buscode,stocode,relacode,relaname,relawarcode,checkouttime,relatype,typecode,typename ");
            sbSql.AppendLine("    UNION ");
            sbSql.AppendLine("   SELECT NEWID() AS dmuid, @batuid AS batuid,buscode,stocode ");
            sbSql.AppendLine("   ,dbo.f_GetODIDs(buscode,stocode,relacode,relaname,relawarcode,relatype,@tb_UsedMates) AS  orderdishesids ");
            sbSql.AppendLine("   ,checkouttime,relatype ");
            sbSql.AppendLine("    ,relacode,relaname ");
            sbSql.AppendLine("   ,sum(relanum) AS relanum ");
            sbSql.AppendLine("  ,relawarcode ");
            sbSql.AppendLine("  ,sum(matamount) AS matamount ");
            sbSql.AppendLine("  ,@ctime AS ctime ");
            sbSql.AppendLine("   ,SUM(methodmoney) / sum(matamount) AS disprice ");
            sbSql.AppendLine("  ,typecode,typename ");
            sbSql.AppendLine("   FROM  ");
            sbSql.AppendLine("   ( ");
            sbSql.AppendLine("  SELECT  buscode,stocode ");
            sbSql.AppendLine("  ,checkouttime,relatype ");
            sbSql.AppendLine("  ,relacode,relaname ");
            sbSql.AppendLine("  ,max(methodnum) AS relanum ");
            sbSql.AppendLine("  ,warcode AS relawarcode ");
            sbSql.AppendLine("  ,@ctime AS ctime ");
            sbSql.AppendLine(" ,max(matamount) AS disprice ");
            sbSql.AppendLine("  ,max(matamount) AS matamount ");
            sbSql.AppendLine("  ,typecode,typename ");
            sbSql.AppendLine("  ,MAX(methodmoney) AS methodmoney ");
            sbSql.AppendLine("  FROM @tb_UsedMates ");
            sbSql.AppendLine(" WHERE relatype='1' ");
            sbSql.AppendLine("   GROUP BY buscode,stocode,orderdishesid,relacode,relaname,warcode,checkouttime,relatype,typecode,typename ");
            sbSql.AppendLine("  ) m GROUP BY buscode,stocode,relacode,relaname,relawarcode,checkouttime,relatype,typecode,typename ");
            sbSql.AppendLine("  ; ");

            sbSql.AppendLine("   INSERT INTO Stock_DisheMaterialMain(dmuid,batuid,buscode,stocode,orderdishesids,checkouttime,relatype,relacode,relaname,relanum,relawarcode,matamount,ctime,disprice,typecode,typename) ");
            sbSql.AppendLine("  SELECT * FROM @DisheMaterialMain; ");

            sbSql.AppendLine("  INSERT INTO Stock_DisheMaterial(dmuid ,batuid,checkouttime,buscode ,stocode ,relanum,relatype,isneedweigh,mattypecode,mattypename ,matcode,.matname,matnum ,smunitname,matprice ,mctime,warcode,unitcode) ");
            sbSql.AppendLine("   select ");
            sbSql.AppendLine("   b.dmuid , ");
            sbSql.AppendLine("   a.batuid, ");
            sbSql.AppendLine("   b.checkouttime, ");
            sbSql.AppendLine("   a.buscode , ");
            sbSql.AppendLine("   a.stocode , ");
            sbSql.AppendLine("   a.relanum, ");
            sbSql.AppendLine("   a.relatype, ");
            sbSql.AppendLine("   a.isneedweigh, ");
            sbSql.AppendLine("   a.mattypecode, ");
            sbSql.AppendLine("   a.mattypename , ");
            sbSql.AppendLine("   a.matcode, ");
            sbSql.AppendLine("   a.matname, ");
            sbSql.AppendLine("   a.matnum , ");
            sbSql.AppendLine("   a.smunitname, ");
            sbSql.AppendLine("  a.matprice , ");
            sbSql.AppendLine("  @ctime AS mctime, ");
            sbSql.AppendLine("  b.relawarcode AS warcode ");
            sbSql.AppendLine("   ,a.unitcode ");
            sbSql.AppendLine("   FROM @tb_UsedMates a LEFT OUTER JOIN @DisheMaterialMain b ");
            sbSql.AppendLine("   ON a.buscode=b.buscode AND a.stocode=b.stocode  AND a.relacode=b.relacode ");
            sbSql.AppendLine("   AND a.relaname=b.relaname AND a.warcode=b.relawarcode AND a.checkouttime=b.checkouttime ");
            sbSql.AppendLine("   AND a.relatype=b.relatype ");
            sbSql.AppendLine("         ; ");
            sbSql.AppendLine(" END ");
            sbSql.AppendLine("  ; ");
            sbSql.AppendLine(" update chopenshift set isextractmeta='1' WHERE shiftid IN (SELECT shiftid FROM @chopenshift); "); //更新已提取计算出库原料状态值
            */

            sbSql.AppendLine(" INSERT INTO cholossdetailhistory SELECT DISTINCT a.* FROM cholossdetail a INNER JOIN choloss b ON a.losscode=b.losscode WHERE b.detailid IN (SELECT detailid FROM @choorderdetail ); ");
            sbSql.AppendLine(" DELETE cholossdetail WHERE  losscode IN ( SELECT losscode from choloss WHERE detailid IN (SELECT detailid FROM @choorderdetail )); ");

            sbSql.AppendLine(" INSERT INTO cholosshistory SELECT * FROM choloss WHERE detailid IN (SELECT detailid FROM @choorderdetail ); ");
            sbSql.AppendLine(" DELETE choloss WHERE detailid IN (SELECT detailid FROM @choorderdetail ); ");

            sbSql.AppendLine(" INSERT INTO choorderdetailsurchargehistory SELECT * FROM choorderdetailsurcharge WHERE detailid IN (SELECT detailid FROM @choorderdetail ); ");
            sbSql.AppendLine(" DELETE choorderdetailsurcharge WHERE detailid IN (SELECT detailid FROM @choorderdetail ); ");


            sbSql.AppendLine(" DECLARE @choorderdishes TABLE(orderdishesid BIGINT,porderdishesid BIGINT); ");
            sbSql.AppendLine(" INSERT INTO @choorderdishes SELECT orderdishesid,porderdishesid FROM choorderdishes WHERE detailid IN (SELECT detailid FROM @choorderdetail ); ");

            sbSql.AppendLine(" INSERT INTO chobackdetailshistory SELECT * FROM chobackdetails WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes ); ");
            sbSql.AppendLine(" DELETE chobackdetails WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes ); ");


            sbSql.AppendLine(" INSERT INTO choorderdishesdetailshistory SELECT * FROM choorderdishesdetails WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );  ");
            sbSql.AppendLine(" DELETE choorderdishesdetails WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );  ");

            sbSql.AppendLine(" INSERT INTO cdmethodpricehistory SELECT * FROM cdmethodprice WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );  ");
            sbSql.AppendLine(" DELETE cdmethodprice WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );  ");


            sbSql.AppendLine(" INSERT INTO chopackagehistory SELECT * FROM chopackage WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );  ");
            sbSql.AppendLine(" DELETE chopackage WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );  ");

            sbSql.AppendLine(" INSERT INTO choorderdisheshistory SELECT * FROM choorderdishes WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );  ");
            sbSql.AppendLine(" DELETE choorderdishes WHERE orderdishesid IN (SELECT orderdishesid FROM @choorderdishes );  ");

            sbSql.AppendLine(" INSERT INTO choorderhistory SELECT * FROM choorder WHERE orderid IN (SELECT orderid FROM @choorderdetail GROUP BY orderid ); ");
            sbSql.AppendLine(" DELETE choorder WHERE orderid IN (SELECT orderid FROM @choorderdetail GROUP BY orderid ); ");

            sbSql.AppendLine(" INSERT INTO choorderdetailhistory SELECT * FROM choorderdetail WHERE detailid IN (SELECT detailid FROM @choorderdetail); ");
            sbSql.AppendLine(" DELETE choorderdetail WHERE detailid IN (SELECT detailid FROM @choorderdetail); ");


            sbSql.AppendLine(" DECLARE @busDestine TABLE(ID BIGINT,desCode varchar(32)); ");
            sbSql.AppendLine(" INSERT INTO @busDestine SELECT ID,desCode FROM busDestine WHERE desCode IN (SELECT detailcodes AS desCode FROM @choorderdetailBreak WHERE pstatus<>'5' ); ");

            sbSql.AppendLine(" INSERT INTO paymentDetailshistory SELECT * FROM paymentDetails WHERE busId IN (SELECT ID AS busId FROM @busDestine ); ");
            sbSql.AppendLine(" DELETE paymentDetails WHERE busId IN (SELECT ID AS busId FROM @busDestine ); ");

            sbSql.AppendLine(" INSERT INTO busDestineTablehistory SELECT * FROM busDestineTable WHERE desCode IN (SELECT desCode FROM @busDestine ); ");
            sbSql.AppendLine(" delete busDestineTable WHERE desCode IN (SELECT desCode FROM @busDestine ); ");

            sbSql.AppendLine(" INSERT INTO busDestinehistory SELECT * FROM  busDestine WHERE Id IN (SELECT ID AS busId FROM @busDestine ); ");
            sbSql.AppendLine(" DELETE busDestine WHERE Id IN (SELECT ID AS busId FROM @busDestine ); ");

            sbSql.AppendLine(" INSERT INTO memcardordershistory SELECT * FROM memcardorders WHERE ordercode IN (SELECT detailcodes AS ordercode FROM @choorderdetailBreak WHERE pstatus<>'5'); ");
            sbSql.AppendLine(" DELETE memcardorders WHERE ordercode IN (SELECT detailcodes AS ordercode FROM @choorderdetailBreak WHERE pstatus<>'5'); ");

            sbSql.AppendLine(" INSERT INTO choorderdetailcouponhistory SELECT * FROM choorderdetailcoupon WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak ); ");
            sbSql.AppendLine(" DELETE choorderdetailcoupon WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak ); ");

            sbSql.AppendLine(" INSERT INTO chopayincomehistory SELECT * FROM chopayincome WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak ); ");
            sbSql.AppendLine(" DELETE chopayincome WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak ); ");

            sbSql.AppendLine(" INSERT INTO chopayhistory SELECT * FROM chopay WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak ); ");
            sbSql.AppendLine(" DELETE chopay WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak ); ");


            sbSql.AppendLine(" INSERT INTO chopayMisposDetailhistory SELECT a.* FROM chopayMisposDetail a inner JOIN @chopenshift b on a.shiftid=b.shiftid; ");
            sbSql.AppendLine(" DELETE chopayMisposDetail WHERE shiftid IN (SELECT shiftid FROM @chopenshift); ");

            sbSql.AppendLine(" INSERT INTO choorderdetailBreakhistory SELECT * FROM choorderdetailBreak WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak ); ");
            sbSql.AppendLine(" DELETE choorderdetailBreak WHERE orderno IN (SELECT orderno FROM @choorderdetailBreak ); ");

            sbSql.AppendLine(" INSERT INTO chopenshiftmoneyhistory SELECT a.* FROM chopenshiftmoney a inner JOIN @chopenshift b on a.shiftid=b.shiftid; ");
            sbSql.AppendLine(" DELETE chopenshiftmoney WHERE shiftid IN (SELECT shiftid FROM @chopenshift);	 ");

            sbSql.AppendLine(" INSERT INTO chopenshifthistory SELECT a.* FROM chopenshift a inner JOIN @chopenshift b on a.shiftid=b.shiftid; ");
            sbSql.AppendLine(" DELETE chopenshift WHERE shiftid IN (SELECT shiftid FROM @chopenshift); ");

            sbSql.AppendLine(" if(@@error=0) begin commit tran tan_ImportToHistory; end else begin rollback tran tan_ImportToHistory end ");

            #endregion

            blnResult = DBHelper.ExecuteNonQuery4(sbSql.ToString(), CommandType.Text, true);
            return blnResult;
        }

        /// <summary>
        /// 从历史表提取菜品使用的原料信息到菜品消耗原料表，提供原料出库成本计算
        /// </summary>
        /// <returns></returns>
        public void ExtractDisheMaterialFormHistory(string buscode, string stocode)
        {
            try
            {
                DataTable dtShiftInfo = DBHelper.ExecuteDataTable(string.Format("select top 50 * from chopenshifthistory where buscode='{0}' and strcode='{1}' and isnull(isextractmeta,'0')<>'1' order by shiftid ", buscode, stocode));
                if (dtShiftInfo != null && dtShiftInfo.Rows.Count > 0)
                {
                    string shiftid = string.Empty;
                    StringBuilder sbSql = new StringBuilder();
                    for (int i = 0; i < dtShiftInfo.Rows.Count; i++)
                    {
                        sbSql.Clear();
                        shiftid = dtShiftInfo.Rows[i]["shiftid"].ToString();
                        #region 构造导入语句
                        sbSql.AppendLine(" BEGIN TRAN tan_ExtractToDisheMaterial ");

                        sbSql.AppendLine(" DECLARE @InsertSql NVARCHAR(MAX); ");
                        sbSql.AppendLine(" set @InsertSql=''; ");
                        sbSql.AppendLine(" declare @filter VARCHAR(400); ");
                        sbSql.AppendLine(" declare @IDS VARCHAR(400); ");
                        sbSql.AppendLine(" SET @IDS=''; ");
                        sbSql.AppendLine(" DECLARE @orderid BIGINT; ");

                        sbSql.AppendLine(" DECLARE @buscode VARCHAR(16);");
                        sbSql.AppendLine(" DECLARE @stocode VARCHAR(16)");
                        sbSql.AppendLine(" DECLARE @shiftid BIGINT");
                        sbSql.AppendLine(" DECLARE @orderno VARCHAR(32)");
                        sbSql.AppendLine(" DECLARE @detailcode VARCHAR(32)");


                        sbSql.AppendLine(string.Format(" set @buscode='{0}' ;", buscode));
                        sbSql.AppendLine(string.Format(" set @stocode='{0}' ;", stocode));
                        sbSql.AppendLine(string.Format(" set @shiftid={0} ;", shiftid.ToString()));
                        sbSql.AppendLine(string.Format(" set @orderno='{0}' ;", ""));
                        sbSql.AppendLine(string.Format(" set @detailcode='{0}' ;", ""));

                        sbSql.AppendLine(" DECLARE @chopenshift TABLE (shiftid BIGINT,buscode varchar(16),strcode varchar(16));  ");
                        sbSql.AppendLine(" INSERT INTO @chopenshift SELECT shiftid,buscode,strcode FROM chopenshifthistory WHERE [status]='1' AND shiftid=ISNULL(@shiftid,0) ; ");

                        sbSql.AppendLine(" DECLARE @choorderdetailBreak TABLE(detailcodes varchar(5120),orderno varchar(32), shiftid BIGINT,expendtype char(1),pstatus char(1) ); ");
                        sbSql.AppendLine(" IF ISNULL(@orderno,'')='' ");
                        sbSql.AppendLine(" begin ");
                        sbSql.AppendLine(" INSERT INTO @choorderdetailBreak SELECT detailcodes ,orderno,shiftid ,expendtype,pstatus FROM choorderdetailBreakhistory  ");
                        sbSql.AppendLine(" WHERE shiftid=@shiftid AND [pstatus] IN ('1','2','4','5') ; ");
                        sbSql.AppendLine(" END ");
                        sbSql.AppendLine(" ELSE ");
                        sbSql.AppendLine(" BEGIN ");
                        sbSql.AppendLine(" INSERT INTO @choorderdetailBreak SELECT detailcodes ,orderno,shiftid ,expendtype,pstatus FROM choorderdetailBreakhistory  ");
                        sbSql.AppendLine(" WHERE orderno=@orderno AND [pstatus] IN ('1','2','4','5') ; ");
                        sbSql.AppendLine(" END ");

                        sbSql.AppendLine(" DECLARE @choorderdetail TABLE(detailid BIGINT,orderid BIGINT); ");
                        sbSql.AppendLine(" IF ISNULL(@detailcode,'')='' ");
                        sbSql.AppendLine(" begin ");
                        sbSql.AppendLine(" INSERT @choorderdetail SELECT a.detailid,a.orderid  ");
                        sbSql.AppendLine("  FROM choorderdetailhistory a INNER JOIN @choorderdetailBreak b ON CHARINDEX(','+a.detailcode+',',','+b.detailcodes+',')>0 WHERE b.pstatus<>'5' ;  ");
                        sbSql.AppendLine(" END ");
                        sbSql.AppendLine(" ELSE ");
                        sbSql.AppendLine(" BEGIN ");
                        sbSql.AppendLine("     INSERT @choorderdetail SELECT a.detailid,a.orderid  ");
                        sbSql.AppendLine(string.Format("  FROM choorderdetailhistory a WHERE CHARINDEX(','+a.detailcode+',',','+@detailcode+',')>0 ;  "));
                        sbSql.AppendLine(" END ");


                        sbSql.AppendLine(" IF EXISTS(SELECT TOP 1 * FROM @choorderdetail) ");
                        sbSql.AppendLine(" begin ");

                        sbSql.AppendLine("  DECLARE  @batuid VARCHAR(64); ");
                        sbSql.AppendLine("  SET @batuid= '" + System.Guid.NewGuid().ToString() + "'; ");
                        sbSql.AppendLine("  DECLARE @ctime DATETIME; ");
                        sbSql.AppendLine("  SET @ctime=GETDATE(); ");
                        sbSql.AppendLine("  DECLARE @tb_UsedMates tsg_TableType ; ");

                        sbSql.AppendLine(" INSERT INTO @tb_UsedMates ");
                        sbSql.AppendLine("  SELECT '' as dmuid, @batuid AS batuid, b.buscode,b.stocode,b.detailid,b.detailcode,b.orderdishesid,b.ctime as constime ");
                        sbSql.AppendLine("  ,b.distypecode,b.dtypecode,dbo.f_GetDishTypeName(b.dtypecode) AS dtypename,b.discode,b.disname,b.disothername ");
                        sbSql.AppendLine("  ,(b.refundaddnum+b.disnum) AS disnum,b.oneprice AS disprice,b.comprice,b.warcode ");
                        sbSql.AppendLine("  ,d.mattypecode,dbo.fnGetStockMateTypeName(d.mattypecode) AS mattypename,a.matcode,d.matname,d.matsimplename ");
                        sbSql.AppendLine("  ,0 AS methodnum  ");
                        sbSql.AppendLine("  ,a.useamount AS matnum ");
                        sbSql.AppendLine("  ,a.unitcode ");
                        sbSql.AppendLine("  ,[dbo].[fnGetUnitNameByCode](a.unitcode) AS smunitname ");
                        sbSql.AppendLine("  ,d.smsalprice AS matprice ");
                        sbSql.AppendLine("  ,a.useamount * d.smsalprice AS matamount ");
                        sbSql.AppendLine("  ,e.warcode AS matwarcode ");
                        sbSql.AppendLine("  ,b.methodmoney ");
                        sbSql.AppendLine("  ,'0' AS relatype ");
                        sbSql.AppendLine("  ,b.isneedweigh ");
                        sbSql.AppendLine("  ,b.discode as relacode ");
                        sbSql.AppendLine("  ,b.disname AS relaname ");
                        sbSql.AppendLine("  ,a.useamount AS relanum  ");
                        sbSql.AppendLine("  ,ISNULL(c.endtime,'1900-01-01') AS endtime ");
                        sbSql.AppendLine("  ,CONVERT(varchar(100),ISNULL(c.endtime,'1900-01-01'), 23)  AS checkouttime ");
                        sbSql.AppendLine("  ,b.dtypecode AS typecode ");
                        sbSql.AppendLine("  ,dbo.f_GetDishTypeName(b.dtypecode) AS typename ");
                        sbSql.AppendLine("  FROM DishesMate a INNER JOIN dbo.choorderdisheshistory b  ");
                        sbSql.AppendLine("  ON a.buscode=b.buscode AND a.stocode=b.stocode AND a.discode=b.discode AND ISNULL(b.porderdishesid,0)=0 and isnull(b.iscaninventory,'')<>'1' "); //烟酒可入库除外
                        sbSql.AppendLine("  INNER JOIN choorderdetailhistory c ON b.detailid=c.detailid ");
                        sbSql.AppendLine("  INNER JOIN @choorderdetail t ON c.detailid=t.detailid ");
                        sbSql.AppendLine("  LEFT OUTER JOIN StockMaterial d ");
                        sbSql.AppendLine("  ON  a.matcode=d.matcode "); //a.buscode=d.buscode AND a.stocode=d.stocode AND
                        sbSql.AppendLine("  LEFT OUTER JOIN Stock e  ");
                        sbSql.AppendLine("   ON a.buscode=e.buscode AND a.stocode=e.stocode AND a.matcode=e.matcode ");
                        sbSql.AppendLine("   union ");
                        sbSql.AppendLine("  SELECT '' as dmuid, @batuid AS batuid,b.buscode,b.stocode,b.detailid,b.detailcode,b.orderdishesid,b.ctime as constime ");
                        sbSql.AppendLine("   ,b.distypecode,b.dtypecode,dbo.f_GetDishTypeName(b.dtypecode) AS dtypename,b.discode,b.disname,b.disothername ");
                        sbSql.AppendLine("  ,(b.refundaddnum+b.disnum) AS disnum,b.oneprice AS disprice,b.comprice,b.warcode ");
                        sbSql.AppendLine("  ,d.mattypecode,dbo.fnGetStockMateTypeName(d.mattypecode) AS mattypename, a.matcode,d.matname,d.matsimplename ");
                        sbSql.AppendLine("  ,c.addamount*(b.refundaddnum+b.disnum) AS methodnum  ");
                        sbSql.AppendLine("  ,a.useamount AS matnum ");
                        sbSql.AppendLine("  ,a.unitcode ");
                        sbSql.AppendLine("  ,[dbo].[fnGetUnitNameByCode](a.unitcode) AS smunitname ");
                        sbSql.AppendLine("  ,d.smsalprice AS matprice ");
                        sbSql.AppendLine("  ,c.addamount*(b.refundaddnum+b.disnum)*a.useamount * d.smsalprice AS matamount ");
                        sbSql.AppendLine("  ,e.warcode AS matwarcode ");
                        sbSql.AppendLine("  ,b.methodmoney ");
                        sbSql.AppendLine("  ,'1' AS relatype ");
                        sbSql.AppendLine("  ,b.isneedweigh ");
                        sbSql.AppendLine("  ,c.methodcode as relacode ");
                        sbSql.AppendLine("  ,c.methodname AS relaname ");
                        sbSql.AppendLine("  ,a.useamount AS relanum  ");
                        sbSql.AppendLine("  ,ISNULL(f.endtime,'1900-01-01') AS endtime ");
                        sbSql.AppendLine("  ,CONVERT(varchar(100),ISNULL(f.endtime,'1900-01-01'), 23)  AS checkouttime ");
                        sbSql.AppendLine("  ,dbo.f_GetMethodTypeCodeByMethodcode(a.methodcode) AS typecode ");
                        sbSql.AppendLine("  ,dbo.f_GetMethodTypeNameByMethodcode(a.methodcode) AS typename ");
                        sbSql.AppendLine("   FROM MethodsMate a   ");
                        sbSql.AppendLine("   INNER JOIN cdmethodpricehistory c ");
                        sbSql.AppendLine("    ON a.buscode=c.buscode AND a.stocode=c.stocode AND a.methodcode=c.methodcode  ");
                        sbSql.AppendLine("   INNER JOIN dbo.choorderdisheshistory b  ");
                        sbSql.AppendLine("   ON c.buscode=b.buscode AND c.stocode=b.stocode AND c.orderdishesid=b.orderdishesid and c.discode=b.discode  ");
                        sbSql.AppendLine("   INNER JOIN choorderdetailhistory f ON b.detailid=f.detailid ");
                        sbSql.AppendLine("   INNER JOIN @choorderdetail t ON f.detailid=t.detailid ");
                        sbSql.AppendLine("   LEFT OUTER JOIN StockMaterial d ");
                        sbSql.AppendLine("    ON  a.matcode=d.matcode "); //a.buscode=d.buscode AND a.stocode=d.stocode AND
                        sbSql.AppendLine("   LEFT OUTER JOIN Stock e  ");
                        sbSql.AppendLine("   ON a.buscode=e.buscode AND a.stocode=e.stocode AND a.matcode=e.matcode ");
                        sbSql.AppendLine("     ; ");

                        sbSql.AppendLine("   DECLARE @DisheMaterialMain table ( ");
                        sbSql.AppendLine("    dmuid                varchar(64)          null, ");
                        sbSql.AppendLine("    batuid               varchar(64)          null, ");
                        sbSql.AppendLine("   buscode              varchar(16)          null, ");
                        sbSql.AppendLine("    stocode              varchar(8)           null, ");
                        sbSql.AppendLine("    orderdishesids       varchar(max)         null, ");
                        sbSql.AppendLine("    checkouttime         datetime             null, ");
                        sbSql.AppendLine("    relatype             char(1)              null, ");
                        sbSql.AppendLine("   relacode             varchar(16)          null, ");
                        sbSql.AppendLine("   relaname             nvarchar(32)         null, ");
                        sbSql.AppendLine("  relanum              decimal(18,3)        null, ");
                        sbSql.AppendLine("  relawarcode          varchar(16)          null, ");
                        sbSql.AppendLine("  matamount			decimal(18,3)        null, ");
                        sbSql.AppendLine(" ctime                datetime             NULL, ");
                        sbSql.AppendLine(" disprice				decimal(18,2)        null, ");
                        sbSql.AppendLine(" matnum				decimal(18,3)        null, ");
                        sbSql.AppendLine("  typecode             varchar(16)          null, ");
                        sbSql.AppendLine("  typename             nvarchar(16)         null ");
                        sbSql.AppendLine("  ) ");
                        sbSql.AppendLine("   ; ");

                        sbSql.AppendLine("  INSERT INTO @DisheMaterialMain ");
                        sbSql.AppendLine("  SELECT NEWID() AS dmuid, @batuid AS batuid,buscode,stocode ");
                        sbSql.AppendLine("  ,dbo.f_GetODIDs(buscode,stocode,relacode,relaname,relawarcode,relatype,@tb_UsedMates) AS  orderdishesids ");
                        sbSql.AppendLine("  ,checkouttime,relatype ");
                        sbSql.AppendLine("  ,relacode,relaname ");
                        sbSql.AppendLine("  ,sum(relanum) AS relanum ");
                        sbSql.AppendLine("  ,relawarcode ");
                        sbSql.AppendLine("  ,sum(matamount) AS matamount ");
                        sbSql.AppendLine("  ,@ctime AS ctime ");
                        sbSql.AppendLine("   ,MAX(disprice) AS disprice ");
                        sbSql.AppendLine("   ,0 AS matnum ");
                        sbSql.AppendLine("   ,typecode,typename ");
                        sbSql.AppendLine("   FROM ");
                        sbSql.AppendLine("    ( ");
                        sbSql.AppendLine("    SELECT buscode,stocode ");
                        sbSql.AppendLine("   ,checkouttime,relatype ");
                        sbSql.AppendLine("   ,relacode,relaname ");
                        sbSql.AppendLine("   ,MAX(disnum) AS relanum ");
                        sbSql.AppendLine("   ,warcode AS relawarcode ");
                        sbSql.AppendLine("   ,@ctime AS ctime ");
                        sbSql.AppendLine("   ,MAX(disprice) AS disprice ");
                        sbSql.AppendLine("   ,sum(matamount) AS matamount  ");
                        sbSql.AppendLine("   ,sum(matnum) AS matnum ");
                        sbSql.AppendLine("   ,typecode,typename ");
                        sbSql.AppendLine("   FROM @tb_UsedMates ");
                        sbSql.AppendLine("   WHERE relatype='0' ");
                        sbSql.AppendLine("   GROUP BY buscode,stocode,orderdishesid,relacode,relaname,warcode,checkouttime,relatype,typecode,typename	 ");
                        sbSql.AppendLine("    ) d GROUP BY buscode,stocode,relacode,relaname,relawarcode,checkouttime,relatype,typecode,typename ");
                        sbSql.AppendLine("    UNION ");
                        sbSql.AppendLine("   SELECT NEWID() AS dmuid, @batuid AS batuid,buscode,stocode ");
                        sbSql.AppendLine("   ,dbo.f_GetODIDs(buscode,stocode,relacode,relaname,relawarcode,relatype,@tb_UsedMates) AS  orderdishesids ");
                        sbSql.AppendLine("   ,checkouttime,relatype ");
                        sbSql.AppendLine("    ,relacode,relaname ");
                        sbSql.AppendLine("   ,sum(relanum) AS relanum ");
                        sbSql.AppendLine("  ,relawarcode ");
                        sbSql.AppendLine("  ,sum(matamount) AS matamount ");
                        sbSql.AppendLine("  ,@ctime AS ctime ");
                        sbSql.AppendLine("   ,SUM(methodmoney) / sum(matamount) AS disprice ");
                        sbSql.AppendLine("   ,sum(matnum) AS matnum ");
                        sbSql.AppendLine("  ,typecode,typename ");
                        sbSql.AppendLine("   FROM  ");
                        sbSql.AppendLine("   ( ");
                        sbSql.AppendLine("  SELECT  buscode,stocode ");
                        sbSql.AppendLine("  ,checkouttime,relatype ");
                        sbSql.AppendLine("  ,relacode,relaname ");
                        sbSql.AppendLine("  ,max(methodnum) AS relanum ");
                        sbSql.AppendLine("  ,warcode AS relawarcode ");
                        sbSql.AppendLine("  ,@ctime AS ctime ");
                        sbSql.AppendLine("  ,max(matamount) AS disprice ");
                        sbSql.AppendLine("  ,sum(matamount) AS matamount ");
                        sbSql.AppendLine("  ,sum(matnum) AS matnum ");
                        sbSql.AppendLine("  ,typecode,typename ");
                        sbSql.AppendLine("  ,MAX(methodmoney) AS methodmoney ");
                        sbSql.AppendLine("  FROM @tb_UsedMates ");
                        sbSql.AppendLine("  WHERE relatype='1' ");
                        sbSql.AppendLine("  GROUP BY buscode,stocode,orderdishesid,relacode,relaname,warcode,checkouttime,relatype,typecode,typename ");
                        sbSql.AppendLine("  ) m GROUP BY buscode,stocode,relacode,relaname,relawarcode,checkouttime,relatype,typecode,typename ");
                        sbSql.AppendLine("  ; ");

                        sbSql.AppendLine("   INSERT INTO Stock_DisheMaterialMain(dmuid,batuid,buscode,stocode,orderdishesids,checkouttime,relatype,relacode,relaname,relanum,relawarcode,matamount,ctime,disprice,matnum,typecode,typename) ");
                        sbSql.AppendLine("  SELECT * FROM @DisheMaterialMain  ; ");//where orderdishesids not in (select orderdishesids from Stock_DisheMaterialMain )

                        sbSql.AppendLine("  INSERT INTO Stock_DisheMaterial(dmuid ,batuid,checkouttime,buscode ,stocode ,relanum,relatype,isneedweigh,mattypecode,mattypename ,matcode,.matname,matnum ,smunitname,matprice ,mctime,warcode,unitcode) ");
                        sbSql.AppendLine("   select ");
                        sbSql.AppendLine("   b.dmuid , ");
                        sbSql.AppendLine("   @batuid as batuid, ");
                        sbSql.AppendLine("   b.checkouttime, ");
                        sbSql.AppendLine("   a.buscode , ");
                        sbSql.AppendLine("   a.stocode , ");
                        sbSql.AppendLine("   a.relanum, ");
                        sbSql.AppendLine("   a.relatype, ");
                        sbSql.AppendLine("   a.isneedweigh, ");
                        sbSql.AppendLine("   a.mattypecode, ");
                        sbSql.AppendLine("   a.mattypename , ");
                        sbSql.AppendLine("   a.matcode, ");
                        sbSql.AppendLine("   a.matname, ");
                        sbSql.AppendLine("   a.matnum , ");
                        sbSql.AppendLine("   a.smunitname, ");
                        sbSql.AppendLine("  a.matprice , ");
                        sbSql.AppendLine("  @ctime AS mctime, ");
                        sbSql.AppendLine("  b.relawarcode AS warcode ");
                        sbSql.AppendLine("   ,a.unitcode ");
                        sbSql.AppendLine("   FROM @tb_UsedMates a LEFT OUTER JOIN @DisheMaterialMain b ");
                        sbSql.AppendLine("   ON a.buscode=b.buscode AND a.stocode=b.stocode  AND a.relacode=b.relacode ");
                        sbSql.AppendLine("   AND a.relaname=b.relaname AND a.warcode=b.relawarcode AND a.checkouttime=b.checkouttime ");
                        sbSql.AppendLine("   AND a.relatype=b.relatype ");
                        sbSql.AppendLine("         ; ");
                        sbSql.AppendLine(" END ");

                        sbSql.AppendLine(" update chopenshifthistory set isextractmeta='1' WHERE shiftid IN (SELECT shiftid FROM @chopenshift); ");

                        sbSql.AppendLine(" if(@@error=0) begin commit tran tan_ExtractToDisheMaterial; end else begin rollback tran tan_ExtractToDisheMaterial end ");

                        #endregion
                        DBHelper.ExecuteNonQuery4(sbSql.ToString(), CommandType.Text, true);


                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
            }
        }

        /// <summary>
        /// 出库原料消费信息()
        /// </summary>
        /// <param name="sPar"></param>
        public string ConsumMaterialOut(string sGUID, string sUserID, string sUserCode, string sUserName)
        {
            string strReturn = string.Empty;

            WebServiceHelper objWebServiceHelper = new WebServiceHelper();
            string strSubmitResult = string.Empty;

            StringBuilder sbSql = new StringBuilder();//更新本地库语句
            StringBuilder sbSqlIDS1 = new StringBuilder();//更新本地库语句的ID组
            StringBuilder sbSqlIDS2 = new StringBuilder();//更新本地库语句的ID组
            StringBuilder postStr = new StringBuilder();//提交webservice语句
            #region 提交远程服务
            int intSucess = 0; //成功组数
            List<string> lstMessage = new List<string>();
            StringBuilder sbMessage = new StringBuilder();
            string status = "";
            string mes = "";

            string GUID = sGUID;// string.Empty;
            string USER_ID = sUserID;// string.Empty;
            string buscode = string.Empty;//商户编号
            string stocode = string.Empty;//门店编号
            string warcode = string.Empty;//仓库编号
            string outdate = string.Empty;//出库日期
            string outcode = string.Empty;//消耗单号
            string outecode = sUserCode;// string.Empty;//操作人员工编号
            string username = sUserName;//操作人姓名
            StringBuilder sbMatinfos = new StringBuilder();//出库原料信息
            string remark = string.Empty;
            if (string.IsNullOrEmpty(sUserID) || sUserID.Trim() == "" || sUserID.Trim() == "0")
            {
                remark = "自动消耗出库";
            }
            else
            {
                remark = "手动消耗出库";
            }
            //获取URL
            string ShortMesUrl = Helper.GetAppSettings("MemcardServiceUrl");// +"/IStore/stock/StockOut.ashx"; 
            if (string.IsNullOrEmpty(ShortMesUrl) || ShortMesUrl.Trim() == "")
            {
                ShortMesUrl = getDBConfigValue("LSServer");//客存库
            }
            if (!string.IsNullOrEmpty(ShortMesUrl) && ShortMesUrl.Trim() != "")
            {
                if (ShortMesUrl.Substring(ShortMesUrl.Length - 1, 1) == "/")
                {
                    ShortMesUrl += "IStore/stock/StockOut.ashx";
                }
                else
                {
                    ShortMesUrl += "/IStore/stock/StockOut.ashx";
                }
            }


            DataTable dtMats = DBHelper.ExecuteDataTable(" select * from Stock_DisheMaterial where isnull(outstauts,'0')='0' order by buscode,stocode,checkouttime,warcode,dmid ");
            if (dtMats != null && dtMats.Rows.Count > 0)
            {
                Dictionary<string, List<bStock_DisheMaterialEntity>> dicMaterials = new Dictionary<string, List<bStock_DisheMaterialEntity>>();
                for (int i = 0; i < dtMats.Rows.Count; i++)
                {
                    if (dtMats.Rows[i]["warcode"] != DBNull.Value && dtMats.Rows[i]["warcode"].ToString().Trim() != "")
                    {
                        string strKey = dtMats.Rows[i]["stocode"].ToString() + "_"
                            + Helper.StringToDateTime(dtMats.Rows[i]["checkouttime"].ToString()).ToString("yyyyMMdd")
                            + "_" + dtMats.Rows[i]["warcode"].ToString();
                        if (!dicMaterials.ContainsKey(strKey))
                        {
                            dicMaterials.Add(strKey, new List<bStock_DisheMaterialEntity>());
                        }
                        dicMaterials[strKey].Add(EntityHelper.GetEntityByDR<bStock_DisheMaterialEntity>(dtMats.Rows[i], null));
                    }
                    Thread.Sleep(5);
                }

                if (dicMaterials.Count == 0) return string.Empty;//没有需要出库的

                foreach (KeyValuePair<string, List<bStock_DisheMaterialEntity>> kv in dicMaterials)
                {
                    sbMatinfos.Clear();//原料信息
                    sbSql.Clear();//更新本地库语句
                    sbSqlIDS1.Clear();//更新本地库语句的ID组
                    sbSqlIDS2.Clear();
                    postStr.Clear();

                    List<bStock_DisheMaterialEntity> lstMats = kv.Value;
                    buscode = lstMats[0].buscode;//商户编号
                    stocode = lstMats[0].stocode;//门店编号
                    warcode = lstMats[0].warcode;//仓库编号
                    outdate = lstMats[0].checkouttime.ToString("yyyy-MM-dd");//出库日期

                    string rancode = kv.Key + "_" + lstMats[lstMats.Count - 1].dmid.ToString(); //出库随机码

                    string t = System.DateTime.Now.ToString("yyyyMMddHHmmssfff", DateTimeFormatInfo.InvariantInfo);
                    outcode = t.Substring(1); //消耗单号
                    for (int j = 0; j < lstMats.Count; j++)
                    {
                        bStock_DisheMaterialEntity objMat = lstMats[j];
                        #region 原料信息
                        if (j > 0)
                        {
                            sbMatinfos.Append(";");
                            sbSqlIDS1.Append(",");
                            sbSqlIDS2.Append(",");
                        }
                        sbMatinfos.AppendFormat("{0},{1},{2}", objMat.matcode, objMat.unitcode, objMat.matnum);
                        sbSqlIDS1.Append("'" + objMat.dmuid + "'");
                        sbSqlIDS2.Append(objMat.dmid.ToString());
                        #endregion
                    }

                    #region 更新本地库信息
                    sbSql.AppendFormat(" update Stock_DisheMaterialMain set consorderno='{0}' where dmuid in ({1}) ; ", outcode, sbSqlIDS1.ToString());
                    sbSql.AppendFormat(" update Stock_DisheMaterial set outstauts='1',usercode='{0}',username='{1}',outtime=getdate(),outstoreno='{2}' where dmid in ({3}) ; ", outecode, username, outcode, sbSqlIDS2.ToString());
                    #endregion

                    #region 提交远程webservice参数

                    postStr.Append("actionname=consumeexport&parameters={" +
                                                string.Format("'GUID':'{0}'", GUID) +  //令牌
                                                string.Format(",'USER_ID': '{0}'", USER_ID) + //用户id
                                                string.Format(",'buscode': '{0}'", buscode) + //商户编号
                                                string.Format(",'stocode': '{0}'", stocode) + //门店编号
                                                string.Format(",'warcode': '{0}'", warcode) + //仓库编号
                                                string.Format(",'outcode': '{0}'", outcode) + //消耗单号
                                                string.Format(",'outdate': '{0}'", outdate) + //出库日期
                                                string.Format(",'outecode': '{0}'", outecode) + //操作人员工编号
                                                string.Format(",'matinfos': '{0}'", sbMatinfos.ToString()) + //出库原料信息
                                                string.Format(",'remark': '{0}'", remark) + //备注
                                                string.Format(",'rancode': '{0}'", rancode) + //随机号
                                        "}");//键值对

                    string strAdminJson = Helper.HttpWebRequestByURL(ShortMesUrl, postStr.ToString());
                    if (!string.IsNullOrEmpty(strAdminJson) && strAdminJson.Trim() != "")
                    {
                        DataSet ds = JsonHelper.JsonToDataSet(strAdminJson, out status, out mes);
                        if (status.Trim() == "0")
                        {//提交成功
                            intSucess++;
                            DBHelper.ExecuteNonQuery(sbSql.ToString()); //更新本地库的出库状态等信息
                        }
                        else
                        {//提交失败 
                            if (!lstMessage.Contains(mes))
                            {
                                lstMessage.Add(mes);
                            }
                        }
                    }
                    else
                    { //提交失败 
                        if (!lstMessage.Contains("消耗原料可出库失败！"))
                        {
                            lstMessage.Add(mes);
                        }
                    }

                    #endregion

                    Thread.Sleep(10);
                }

                #region 弹消息
                if (lstMessage.Count > 0)
                {
                    for (int i = 0; i < lstMessage.Count; i++)
                    {
                        if (i > 0)
                        {
                            sbMessage.Append("；");
                        }
                        sbMessage.Append(lstMessage[i]);
                    }
                }

                if (intSucess > 0)
                {
                    if (lstMessage.Count > 0)
                    {
                        strReturn = "原料可出库部分成功，部分失败：" + sbMessage.ToString();
                    }
                    else
                    {
                        strReturn = "原料可出库已成功。";
                    }
                }
                else
                {
                    strReturn = "原料可出库失败：" + sbMessage.ToString();
                }
                #endregion
            }
            else
            {
                strReturn = "没有消耗原料可出库！";
            }

            #endregion

            return strReturn;
        }

        #region 提取原料线程
        Thread thExtractDisheMaterial = null;//
        private void LoopExtractDisheMaterialFormHistory()
        {
            string strUserID = "0";
            string strUserCode = "admin";
            string strUserName = "admin";
            try
            {
                DataTable dtAdmin = DBHelper.ExecuteDataTable("select top 1 * from sto_admins where uname like '%admin%' ");
                if (dtAdmin != null && dtAdmin.Rows.Count > 0)
                {
                    if (dtAdmin.Rows[0]["empcode"] != DBNull.Value && dtAdmin.Rows[0]["empcode"].ToString().Trim() != "")
                    {
                        strUserCode = dtAdmin.Rows[0]["empcode"].ToString();
                    }
                    if (dtAdmin.Rows[0]["uname"] != DBNull.Value && dtAdmin.Rows[0]["uname"].ToString().Trim() != "")
                    {
                        strUserName = dtAdmin.Rows[0]["uname"].ToString();
                    }
                }
                dtAdmin = null;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
            }


            while (true)
            {
                ExtractDisheMaterialFormHistory(_buscode, _stocode); //从历史点单菜品中提取原料(在交班时候已经提取了，所以在此处注释)
                if (_isMaterialAutoOutStock)
                {
                    Thread.Sleep(10000);
                    ConsumMaterialOut(System.Guid.NewGuid().ToString(), strUserID, strUserCode, strUserName); //出库原料
                }
                Thread.Sleep(1800000); //半小时执行一次
            }
        }

        /// <summary>
        /// 开启提取菜品原料信息线程
        /// </summary>
        /// <param name="buscode"></param>
        /// <param name="stocode"></param>
        public void StartExtractDisheMaterialThread(string buscode, string stocode, bool MaterialAutoOutStock)
        {
            _buscode = buscode;
            _stocode = stocode;
            _isMaterialAutoOutStock = MaterialAutoOutStock;

            if (thExtractDisheMaterial == null)
            {
                thExtractDisheMaterial = new Thread(LoopExtractDisheMaterialFormHistory);
                thExtractDisheMaterial.IsBackground = true;
                thExtractDisheMaterial.Start();
            }
            else
            {
                if (thExtractDisheMaterial.ThreadState == ThreadState.Stopped)
                {
                    thExtractDisheMaterial.Start();
                }
            }
        }

        /// <summary>
        /// 停止提取菜品原料信息线程
        /// </summary>
        public void StopExtractDisheMaterialThread()
        {
            if (thExtractDisheMaterial != null)
            {
                thExtractDisheMaterial.Abort();
            }
        }

        #endregion

        #endregion


        /// <summary>
        /// 获取配置的商户编号和门店编号
        /// </summary>
        /// <param name="objAdminsEntity"></param>
        /// <returns></returns>
        private string getDBConfigValue(string key)
        {
            string strReturn = string.Empty;
            if (key.Length == 0)
            {
                return strReturn;
            }
            System.Xml.XmlDocument xmlDoc = null;
            string xmlFileName = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\DBConfig.xml";
            try
            {
                if (!System.IO.File.Exists(xmlFileName))
                {
                    xmlFileName = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\xml\\DBConfig.xml";
                }

                if (System.IO.File.Exists(xmlFileName))
                {
                    xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.Load(xmlFileName);

                    #region //是否有划菜端
                    XmlNode ServerPaddleNode = xmlDoc.SelectSingleNode("//root//" + key);
                    if (ServerPaddleNode != null)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(ServerPaddleNode.InnerText) && ServerPaddleNode.InnerText.Trim() != "")
                            {
                                strReturn = pwHelper.Decode(ServerPaddleNode.InnerText);
                            }
                        }
                        catch
                        {
                            strReturn = ServerPaddleNode.InnerText;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                xmlDoc = null;
            }
            return strReturn;
        }

    }

    public class bStock_DisheMaterialEntity
    {
        #region 基本字段
        private long _dmid = 0;
        private string _dmuid = string.Empty;
        private string _batuid = string.Empty;
        private DateTime _checkouttime = DateTime.Parse("1900-01-01");
        private string _buscode = string.Empty;
        private string _stocode = string.Empty;
        private decimal _relanum = 0;
        private string _relatype = string.Empty;
        private string _isneedweigh = string.Empty;
        private string _mattypecode = string.Empty;
        private string _mattypename = string.Empty;
        private string _matcode = string.Empty;
        private string _matname = string.Empty;
        private decimal _matnum = 0;
        private string _smunitname = string.Empty;
        private decimal _matprice = 0;
        private string _outstauts = string.Empty;
        private string _usercode = string.Empty;
        private string _username = string.Empty;
        private DateTime _outtime = DateTime.Parse("1900-01-01");
        private string _mremark = string.Empty;
        private DateTime _mctime = DateTime.Parse("1900-01-01");
        private string _outstoreno = string.Empty;//出库编号
        private string _warcode = string.Empty;//仓库编号
        private string _unitcode = string.Empty;//单位编号
        #endregion

        #region 基本属性
        /// <summary>
        ///标识
        /// <summary>
        public long dmid
        {
            get { return _dmid; }
            set { _dmid = value; }
        }
        /// <summary>
        ///主唯一标识
        /// <summary>
        [ModelInfo(Name = "主唯一标识", ControlName = "txt_dmuid", NotEmpty = false, Length = 64, NotEmptyECode = "Stock_DisheMaterial_004", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_005")]
        public string dmuid
        {
            get { return _dmuid; }
            set { _dmuid = value; }
        }
        /// <summary>
        ///批次号
        /// <summary>
        [ModelInfo(Name = "批次号", ControlName = "txt_batuid", NotEmpty = false, Length = 64, NotEmptyECode = "Stock_DisheMaterial_007", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_008")]
        public string batuid
        {
            get { return _batuid; }
            set { _batuid = value; }
        }
        /// <summary>
        ///结账日期
        /// <summary>
        [ModelInfo(Name = "结账日期", ControlName = "txt_checkouttime", NotEmpty = false, Length = 19, NotEmptyECode = "Stock_DisheMaterial_010", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_011")]
        public DateTime checkouttime
        {
            get { return _checkouttime; }
            set { _checkouttime = value; }
        }
        /// <summary>
        ///所属商户编号
        /// <summary>
        [ModelInfo(Name = "所属商户编号", ControlName = "txt_buscode", NotEmpty = false, Length = 16, NotEmptyECode = "Stock_DisheMaterial_013", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_014")]
        public string buscode
        {
            get { return _buscode; }
            set { _buscode = value; }
        }
        /// <summary>
        ///所属门店编号
        /// <summary>
        [ModelInfo(Name = "所属门店编号", ControlName = "txt_stocode", NotEmpty = false, Length = 8, NotEmptyECode = "Stock_DisheMaterial_016", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_017")]
        public string stocode
        {
            get { return _stocode; }
            set { _stocode = value; }
        }
        /// <summary>
        ///菜品或做法数量
        /// <summary>
        [ModelInfo(Name = "菜品或做法数量", ControlName = "txt_relanum", NotEmpty = false, Length = 18, NotEmptyECode = "Stock_DisheMaterial_019", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_020")]
        public decimal relanum
        {
            get { return _relanum; }
            set { _relanum = value; }
        }
        /// <summary>
        ///关联类别
        /// <summary>
        [ModelInfo(Name = "关联类别", ControlName = "txt_relatype", NotEmpty = false, Length = 1, NotEmptyECode = "Stock_DisheMaterial_022", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_023")]
        public string relatype
        {
            get { return _relatype; }
            set { _relatype = value; }
        }
        /// <summary>
        ///菜品是否需称重
        /// <summary>
        [ModelInfo(Name = "菜品是否需称重", ControlName = "txt_isneedweigh", NotEmpty = false, Length = 1, NotEmptyECode = "Stock_DisheMaterial_025", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_026")]
        public string isneedweigh
        {
            get { return _isneedweigh; }
            set { _isneedweigh = value; }
        }
        /// <summary>
        ///原料类别编号
        /// <summary>
        [ModelInfo(Name = "原料类别编号", ControlName = "txt_mattypecode", NotEmpty = false, Length = 16, NotEmptyECode = "Stock_DisheMaterial_028", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_029")]
        public string mattypecode
        {
            get { return _mattypecode; }
            set { _mattypecode = value; }
        }
        /// <summary>
        ///原料类别名称
        /// <summary>
        [ModelInfo(Name = "原料类别名称", ControlName = "txt_mattypename", NotEmpty = false, Length = 32, NotEmptyECode = "Stock_DisheMaterial_031", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_032")]
        public string mattypename
        {
            get { return _mattypename; }
            set { _mattypename = value; }
        }
        /// <summary>
        ///原料编号
        /// <summary>
        [ModelInfo(Name = "原料编号", ControlName = "txt_matcode", NotEmpty = false, Length = 16, NotEmptyECode = "Stock_DisheMaterial_034", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_035")]
        public string matcode
        {
            get { return _matcode; }
            set { _matcode = value; }
        }
        /// <summary>
        ///原料名称
        /// <summary>
        [ModelInfo(Name = "原料名称", ControlName = "txt_matname", NotEmpty = false, Length = 32, NotEmptyECode = "Stock_DisheMaterial_037", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_038")]
        public string matname
        {
            get { return _matname; }
            set { _matname = value; }
        }
        /// <summary>
        ///原料使用数量
        /// <summary>
        [ModelInfo(Name = "原料使用数量", ControlName = "txt_matnum", NotEmpty = false, Length = 18, NotEmptyECode = "Stock_DisheMaterial_040", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_041")]
        public decimal matnum
        {
            get { return _matnum; }
            set { _matnum = value; }
        }
        /// <summary>
        ///原料计量单位
        /// <summary>
        [ModelInfo(Name = "原料计量单位", ControlName = "txt_smunitname", NotEmpty = false, Length = 16, NotEmptyECode = "Stock_DisheMaterial_043", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_044")]
        public string smunitname
        {
            get { return _smunitname; }
            set { _smunitname = value; }
        }
        /// <summary>
        ///原料单价
        /// <summary>
        [ModelInfo(Name = "原料单价", ControlName = "txt_matprice", NotEmpty = false, Length = 18, NotEmptyECode = "Stock_DisheMaterial_046", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_047")]
        public decimal matprice
        {
            get { return _matprice; }
            set { _matprice = value; }
        }
        /// <summary>
        ///消耗库存出库状态
        /// <summary>
        [ModelInfo(Name = "消耗库存出库状态", ControlName = "txt_outstauts", NotEmpty = false, Length = 1, NotEmptyECode = "Stock_DisheMaterial_049", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_050")]
        public string outstauts
        {
            get { return _outstauts; }
            set { _outstauts = value; }
        }
        /// <summary>
        ///消耗库存出库操作人编号
        /// <summary>
        [ModelInfo(Name = "消耗库存出库操作人编号", ControlName = "txt_usercode", NotEmpty = false, Length = 16, NotEmptyECode = "Stock_DisheMaterial_052", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_053")]
        public string usercode
        {
            get { return _usercode; }
            set { _usercode = value; }
        }
        /// <summary>
        ///消耗库存出库操作人姓名
        /// <summary>
        [ModelInfo(Name = "消耗库存出库操作人姓名", ControlName = "txt_username", NotEmpty = false, Length = 32, NotEmptyECode = "Stock_DisheMaterial_055", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_056")]
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        ///消耗库存出库操作时间
        /// <summary>
        [ModelInfo(Name = "消耗库存出库操作时间", ControlName = "txt_outtime", NotEmpty = false, Length = 19, NotEmptyECode = "Stock_DisheMaterial_058", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_059")]
        public DateTime outtime
        {
            get { return _outtime; }
            set { _outtime = value; }
        }
        /// <summary>
        ///备注
        /// <summary>
        [ModelInfo(Name = "备注", ControlName = "txt_mremark", NotEmpty = false, Length = 128, NotEmptyECode = "Stock_DisheMaterial_061", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_062")]
        public string mremark
        {
            get { return _mremark; }
            set { _mremark = value; }
        }
        /// <summary>
        ///创建日期
        /// <summary>
        [ModelInfo(Name = "创建日期", ControlName = "txt_mctime", NotEmpty = false, Length = 19, NotEmptyECode = "Stock_DisheMaterial_064", RType = RegularExpressions.RegExpType.Normal, RTypeECode = "Stock_DisheMaterial_065")]
        public DateTime mctime
        {
            get { return _mctime; }
            set { _mctime = value; }
        }


        /// <summary>
        /// 出库编号
        /// </summary>
        public string outstoreno
        {
            get { return _outstoreno; }
            set { _outstoreno = value; }
        }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public string warcode
        {
            get { return _warcode; }
            set { _warcode = value; }
        }

        /// <summary>
        /// 单位编号
        /// </summary>
        public string unitcode
        {
            get { return _unitcode; }
            set { _unitcode = value; }
        }

        #endregion

        #region 扩展属性
        private decimal _matamount = 0;
        /// <summary>
        /// 原料总成本金额
        /// </summary>
        public decimal matamount
        {
            get { return _matamount; }
            set { _matamount = value; }
        }

        #endregion

    }


}
