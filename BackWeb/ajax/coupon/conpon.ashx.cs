using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.ajax.coupon
{
    /// <summary>
    /// 影院优惠券数据接口
    /// </summary>
    public class conpon : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = Helper.ReplaceString(context.Request["way"]);
            string json = "";
            bllMVsumcoupon bll = new bllMVsumcoupon();
            bllMVmaincoupon bllmain = new bllMVmaincoupon();
            bllmaincouponrule bllrule = new bllmaincouponrule();
            operatelogEntity logentity = new operatelogEntity();
            DataTable dt = null;
            string GUID = "";
            string UID = "";
            string cuser = "";
            string uuser = "";
            int pageSize = 0;
            int currentPage = 0;
            string filter = "";
            string order = "";
            int recnums = 0;
            int pagenums = 0;
            string selected = "";
            string status = "";

            switch (type)
            {
                case "maincouponlist":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    filter = Helper.ReplaceString(context.Request["filter"]);
                    filter = filter.Replace("‘", "'");
                    dt = bllmain.GetListInfo(GUID, UID, filter);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "maincouponpagelist":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    pageSize = int.Parse(Helper.ReplaceString(context.Request["pagesize"]));
                    currentPage = int.Parse(Helper.ReplaceString(context.Request["currentpage"]));
                    filter = Helper.ReplaceString(context.Request["filter"]);
                    order = Helper.ReplaceString(context.Request["order"]);
                    filter = filter.Replace("‘", "'");
                    dt = bllmain.GetPagingListInfo(GUID, UID, pageSize, currentPage, filter, order, out recnums, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" }, pageSize, recnums, currentPage, pagenums);

                    break;
                case "maincouponadd":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    string sumidM = Helper.ReplaceString(context.Request["sumid"]);
                    string mcidM = Helper.ReplaceString(context.Request["mcid"]);
                    string buscodeM = Helper.ReplaceString(context.Request["buscode"]);
                    string stocodeM = Helper.ReplaceString(context.Request["stocode"]);
                    string sumcodeM = Helper.ReplaceString(context.Request["sumcode"]);
                    string mccodeM = Helper.ReplaceString(context.Request["mccode"]);
                    string bigclassM = Helper.ReplaceString(context.Request["bigclass"]);
                    string firtypeM = Helper.ReplaceString(context.Request["firtype"]);
                    string sectypeM = Helper.ReplaceString(context.Request["sectype"]);
                    string counameM = Helper.ReplaceString(context.Request["couname"]);
                    string btimeM = Helper.ReplaceString(context.Request["btime"]);
                    string etimeM = Helper.ReplaceString(context.Request["etime"]);
                    string batchnoM = Helper.ReplaceString(context.Request["batchno"]);
                    string prefixM = Helper.ReplaceString(context.Request["prefix"]);
                    string numberM = Helper.ReplaceString(context.Request["number"]);
                    string singlemoneyM = Helper.ReplaceString(context.Request["singlemoney"]);
                    string maxmoneyM = Helper.ReplaceString(context.Request["maxmoney"]);
                    string couponimgM = Helper.ReplaceString(context.Request["couponimg"]);
                    string descrM = Helper.ReplaceString(context.Request["descr"]);
                    string costpriceM = Helper.ReplaceString(context.Request["costprice"]);
                    string uselimitM = Helper.ReplaceString(context.Request["uselimit"]);
                    string discodeM = Helper.ReplaceString(context.Request["discode"]);
                    string matcodeM = Helper.ReplaceString(context.Request["matcode"]);
                    string yjcodeM = Helper.ReplaceString(context.Request["yjcode"]);
                    status = Helper.ReplaceString(context.Request["status"]);
                    string ordernoM = Helper.ReplaceString(context.Request["orderno"]);
                    cuser = Helper.ReplaceString(context.Request["cuser"]);
                    uuser = Helper.ReplaceString(context.Request["uuser"]);
                    string isfreeM = Helper.ReplaceString(context.Request["isfree"]);
                    string istodayM = Helper.ReplaceString(context.Request["istoday"]);
                    string isCinemaM = Helper.ReplaceString(context.Request["isCinema"]);
                    string disusecountM = Helper.ReplaceString(context.Request["disusecount"]);
                    operatelogEntity entityM = new operatelogEntity();
                    entityM.module = ErrMessage.GetMessageInfoByCode("coupons_Menu").Body;
                    entityM.pageurl = "maincouponedit.aspx";
                    entityM.otype = SystemEnum.LogOperateType.Add;
                    entityM.cuser = StringHelper.StringToLong(cuser);
                    entityM.logcontent = "新增优惠活动优惠券信息";
                    dt = bllmain.Add(GUID, UID, out mcidM, buscodeM, stocodeM, sumcodeM, mccodeM, bigclassM, firtypeM, sectypeM, counameM, btimeM, etimeM, batchnoM, prefixM, numberM, singlemoneyM, maxmoneyM, couponimgM, descrM, costpriceM, uselimitM, discodeM, matcodeM, yjcodeM, status, ordernoM, cuser, uuser, isfreeM, istodayM, isCinemaM, disusecountM, entityM);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", mcidM, new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "maincouponupdate":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    cuser = Helper.ReplaceString(context.Request["cuser"]);
                    string sumidMM = Helper.ReplaceString(context.Request["sumid"]);
                    string mcidMM = Helper.ReplaceString(context.Request["mcid"]);
                    string buscodeMM = Helper.ReplaceString(context.Request["buscode"]);
                    string stocodeMM = Helper.ReplaceString(context.Request["stocode"]);
                    string sumcodeMM = Helper.ReplaceString(context.Request["sumcode"]);
                    string mccodeMM = Helper.ReplaceString(context.Request["mccode"]);
                    string bigclassMM = Helper.ReplaceString(context.Request["bigclass"]);
                    string firtypeMM = Helper.ReplaceString(context.Request["firtype"]);
                    string sectypeMM = Helper.ReplaceString(context.Request["sectype"]);
                    string counameMM = Helper.ReplaceString(context.Request["couname"]);
                    string btimeMM = Helper.ReplaceString(context.Request["btime"]);
                    string etimeMM = Helper.ReplaceString(context.Request["etime"]);
                    string batchnoMM = Helper.ReplaceString(context.Request["batchno"]);
                    string prefixMM = Helper.ReplaceString(context.Request["prefix"]);
                    string numberMM = Helper.ReplaceString(context.Request["number"]);
                    string singlemoneyMM = Helper.ReplaceString(context.Request["singlemoney"]);
                    string maxmoneyMM = Helper.ReplaceString(context.Request["maxmoney"]);
                    string couponimgMM = Helper.ReplaceString(context.Request["couponimg"]);
                    string descrMM = Helper.ReplaceString(context.Request["descr"]);
                    string costpriceMM = Helper.ReplaceString(context.Request["costprice"]);
                    string uselimitMM = Helper.ReplaceString(context.Request["uselimit"]);
                    string discodeMM = Helper.ReplaceString(context.Request["discode"]);
                    string matcodeMM = Helper.ReplaceString(context.Request["matcode"]);
                    string yjcodeMM = Helper.ReplaceString(context.Request["yjcode"]);
                    status = Helper.ReplaceString(context.Request["status"]);
                    string ordernoMM = Helper.ReplaceString(context.Request["orderno"]);
                    cuser = Helper.ReplaceString(context.Request["cuser"]);
                    uuser = Helper.ReplaceString(context.Request["uuser"]);
                    string isfreeMM = Helper.ReplaceString(context.Request["isfree"]);
                    string istodayMM = Helper.ReplaceString(context.Request["istoday"]);
                    string isCinemaMM = Helper.ReplaceString(context.Request["isCinema"]);
                    string disusecountMM = Helper.ReplaceString(context.Request["disusecount"]);
                    operatelogEntity entityMM = new operatelogEntity();
                    entityMM.module = ErrMessage.GetMessageInfoByCode("coupons_Menu").Body;
                    entityMM.pageurl = "maincouponedit.aspx";
                    entityMM.otype = SystemEnum.LogOperateType.Add;
                    entityMM.cuser = StringHelper.StringToLong(cuser);
                    entityMM.logcontent = "新增优惠活动优惠券信息";
                    dt = bllmain.Update(GUID, UID, mcidMM, buscodeMM, stocodeMM, sumcodeMM, mccodeMM, bigclassMM, firtypeMM, sectypeMM, counameMM, btimeMM, etimeMM, batchnoMM, prefixMM, numberMM, singlemoneyMM, maxmoneyMM, couponimgMM, descrMM, costpriceMM, uselimitMM, discodeMM, matcodeMM, yjcodeMM, status, ordernoMM, cuser, uuser, isfreeMM, istodayMM, isCinemaMM, disusecountMM, entityMM);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });

                    break;
                case "maincouponpagelistrule":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    pageSize = int.Parse(Helper.ReplaceString(context.Request["pagesize"]));
                    currentPage = int.Parse(Helper.ReplaceString(context.Request["currentpage"]));
                    filter = Helper.ReplaceString(context.Request["filter"]);
                    order = Helper.ReplaceString(context.Request["order"]);
                    filter = filter.Replace("‘", "'");
                    dt = bllrule.GetPagingListInfo(GUID, UID, pageSize, currentPage, filter, order, out recnums, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" }, pageSize, recnums, currentPage, pagenums);

                    break;
                case "maincoupondelete":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    selected = Helper.ReplaceString(context.Request["selected"]);
                    cuser = Helper.ReplaceString(context.Request["cuser"]);
                    operatelogEntity logentity2 = new operatelogEntity();
                    logentity2.module = ErrMessage.GetMessageInfoByCode("sumcoupon_Menu").Body;
                    logentity2.pageurl = "maincouponEdit.aspx";
                    logentity2.otype = SystemEnum.LogOperateType.Delete;
                    logentity2.cuser = StringHelper.StringToLong(cuser);
                    logentity2.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("sumcoupon_961").Body, cuser, selected);

                    dt = bllmain.Delete(GUID, UID, selected, logentity2);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "maincouponupdatestatus":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    selected = Helper.ReplaceString(context.Request["selected"]);
                    status = Helper.ReplaceString(context.Request["status"]);
                    dt = bllmain.UpdateStatus(GUID, UID, selected, status);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                ///////////////////////////////////////////////

                case "maincouponruleadd":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    string id = Helper.ReplaceString(context.Request["id"]);
                    string couponcode = Helper.ReplaceString(context.Request["couponcode"]);
                    string cinemaids = Helper.ReplaceString(context.Request["cinemaids"]);
                    string distype = Helper.ReplaceString(context.Request["distype"]);
                    string gettimes = Helper.ReplaceString(context.Request["gettimes"]);
                    string iswithothers = Helper.ReplaceString(context.Request["iswithothers"]);
                    string usetype = Helper.ReplaceString(context.Request["usetype"]);
                    string ismemdiscount = Helper.ReplaceString(context.Request["ismemdiscount"]);
                    string limits = Helper.ReplaceString(context.Request["limits"]);
                    string direction = Helper.ReplaceString(context.Request["direction"]);
                    string attention = Helper.ReplaceString(context.Request["attention"]);
                    string bdate = Helper.ReplaceString(context.Request["bdate"]);
                    string edate = Helper.ReplaceString(context.Request["edate"]);
                    string cuserR = Helper.ReplaceString(context.Request["cuser"]);
                    operatelogEntity entityR = new operatelogEntity();
                    entityR.module = ErrMessage.GetMessageInfoByCode("coupons_Menu").Body;
                    entityR.pageurl = "maincouponruleedit.aspx";
                    entityR.otype = SystemEnum.LogOperateType.Add;
                    entityR.cuser = StringHelper.StringToLong(cuserR);
                    entityR.logcontent = "新增优惠活动优惠券规则信息";
                    dt = bllrule.Add(GUID, UID, out id, couponcode, cinemaids, distype, gettimes, iswithothers, usetype, ismemdiscount, limits, direction, attention, bdate, edate, entityR);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", id, new ArrayList() { dt }, new string[] { "data" });
                    break;

                case "maincouponruleupdate":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    string id1 = Helper.ReplaceString(context.Request["id"]);
                    string couponcode1 = Helper.ReplaceString(context.Request["couponcode"]);
                    string cinemaids1 = Helper.ReplaceString(context.Request["cinemaids"]);
                    string distype1 = Helper.ReplaceString(context.Request["distype"]);
                    string gettimes1 = Helper.ReplaceString(context.Request["gettimes"]);
                    string iswithothers1 = Helper.ReplaceString(context.Request["iswithothers"]);
                    string usetype1 = Helper.ReplaceString(context.Request["usetype"]);
                    string ismemdiscount1 = Helper.ReplaceString(context.Request["ismemdiscount"]);
                    string limits1 = Helper.ReplaceString(context.Request["limits"]);
                    string direction1 = Helper.ReplaceString(context.Request["direction"]);
                    string attention1 = Helper.ReplaceString(context.Request["attention"]);
                    string bdate1 = Helper.ReplaceString(context.Request["bdate"]);
                    string edate1 = Helper.ReplaceString(context.Request["edate"]);
                    string cuserRR = Helper.ReplaceString(context.Request["cuser"]);
                    operatelogEntity entityRR = new operatelogEntity();
                    entityRR.module = ErrMessage.GetMessageInfoByCode("coupons_Menu").Body;
                    entityRR.pageurl = "maincouponruleedit.aspx";
                    entityRR.otype = SystemEnum.LogOperateType.Add;
                    entityRR.cuser = StringHelper.StringToLong(cuserRR);
                    entityRR.logcontent = "新增优惠活动优惠券规则信息";
                    dt = bllrule.Add(GUID, UID, out id1, couponcode1, cinemaids1, distype1, gettimes1, iswithothers1, usetype1, ismemdiscount1, limits1, direction1, attention1, bdate1, edate1, entityRR);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;

                case "maincouponruledelete":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    selected = Helper.ReplaceString(context.Request["selected"]);
                    operatelogEntity logentity3 = new operatelogEntity();
                    logentity3.module = ErrMessage.GetMessageInfoByCode("maincoupon_Menu").Body;
                    logentity3.pageurl = "maincouponEdit.aspx";
                    logentity3.otype = SystemEnum.LogOperateType.Delete;
                    //logentity3.cuser = StringHelper.StringToLong(LoginedUser.UserInfo.Id.ToString());
                    logentity3.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("sumcoupon_961").Body, "", selected);

                    dt = bllrule.Delete(GUID, UID, selected, logentity3);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                //////////////////////////////////////////////
                case "sumcouponlist":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    pageSize = int.Parse(Helper.ReplaceString(context.Request["pagesize"]));
                    currentPage = int.Parse(Helper.ReplaceString(context.Request["currentpage"]));
                    filter = Helper.ReplaceString(context.Request["filter"]);
                    filter = filter.Replace("‘", "'");
                    order = Helper.ReplaceString(context.Request["order"]);

                    dt = bll.GetPagingListInfo(GUID, UID, pageSize, currentPage, filter, order, out recnums, out pagenums);
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" }, pageSize, recnums, currentPage, pagenums);

                    break;
                case "sumcouponlistdelete":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    selected = Helper.ReplaceString(context.Request["selected"]);
                    cuser = Helper.ReplaceString(context.Request["cuser"]);
                    logentity.module = ErrMessage.GetMessageInfoByCode("sumcoupon_Menu").Body;
                    logentity.pageurl = "sumcouponEdit.aspx";
                    logentity.otype = SystemEnum.LogOperateType.Delete;
                    logentity.cuser = StringHelper.StringToLong(cuser);
                    logentity.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("sumcoupon_961").Body, cuser, selected);

                    dt = bll.Delete(GUID, UID, selected, logentity);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "sumcouponupdatestatus":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    selected = Helper.ReplaceString(context.Request["selected"]);
                    status = Helper.ReplaceString(context.Request["status"]);
                    dt = bll.UpdateStatus(GUID, UID, selected, status);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "sumcouponupdatestatusnotsend":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    selected = Helper.ReplaceString(context.Request["selected"]);
                    dt = bll.UpdateStatusNotSend(GUID, UID, selected);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "sumcouponadd":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    string sumid = Helper.ReplaceString(context.Request["sumid"]);
                    string sumcode = Helper.ReplaceString(context.Request["sumcode"]);
                    string buscode = Helper.ReplaceString(context.Request["buscode"]);
                    string stocode = Helper.ReplaceString(context.Request["stocode"]);
                    string cname = Helper.ReplaceString(context.Request["cname"]);
                    string btime = Helper.ReplaceString(context.Request["btime"]);
                    string etime = Helper.ReplaceString(context.Request["etime"]);
                    string ctype = Helper.ReplaceString(context.Request["ctype"]);
                    string secctype = Helper.ReplaceString(context.Request["secctype"]);
                    string initype = Helper.ReplaceString(context.Request["initype"]);
                    status = Helper.ReplaceString(context.Request["status"]);
                    string descr = Helper.ReplaceString(context.Request["descr"]);
                    string auduser = Helper.ReplaceString(context.Request["auduser"]);
                    string audremark = Helper.ReplaceString(context.Request["audremark"]);
                    string audstatus = Helper.ReplaceString(context.Request["audstatus"]);
                    cuser = Helper.ReplaceString(context.Request["cuser"]);
                    uuser = Helper.ReplaceString(context.Request["uuser"]);
                    operatelogEntity entity = new operatelogEntity();
                    entity.module = ErrMessage.GetMessageInfoByCode("coupons_Menu").Body;
                    entity.pageurl = "sumcouponedit.aspx";
                    entity.otype = SystemEnum.LogOperateType.Add;
                    entity.cuser = StringHelper.StringToLong(cuser);
                    entity.logcontent = "新增优惠券活动信息";
                    dt = bll.Add(GUID, UID, out sumid, sumcode, buscode, stocode, cname, btime, etime, ctype, secctype, initype, status, descr, auduser, audremark, audstatus, cuser, uuser, entity);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", sumid, new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "sumcouponupdate":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    string sumid1 = Helper.ReplaceString(context.Request["sumid"]);
                    string sumcode1 = Helper.ReplaceString(context.Request["sumcode"]);
                    string buscode1 = Helper.ReplaceString(context.Request["buscode"]);
                    string stocode1 = Helper.ReplaceString(context.Request["stocode"]);
                    string cname1 = Helper.ReplaceString(context.Request["cname"]);
                    string btime1 = Helper.ReplaceString(context.Request["btime"]);
                    string etime1 = Helper.ReplaceString(context.Request["etime"]);
                    string ctype1 = Helper.ReplaceString(context.Request["ctype"]);
                    string secctype1 = Helper.ReplaceString(context.Request["secctype"]);
                    string initype1 = Helper.ReplaceString(context.Request["initype"]);
                    status = Helper.ReplaceString(context.Request["status"]);
                    string descr1 = Helper.ReplaceString(context.Request["descr"]);
                    string auduser1 = Helper.ReplaceString(context.Request["auduser"]);
                    string audremark1 = Helper.ReplaceString(context.Request["audremark"]);
                    string audstatus1 = Helper.ReplaceString(context.Request["audstatus"]);
                    cuser = Helper.ReplaceString(context.Request["cuser"]);
                    uuser = Helper.ReplaceString(context.Request["uuser"]);

                    operatelogEntity entity1 = new operatelogEntity();
                    entity1.module = ErrMessage.GetMessageInfoByCode("coupons_Menu").Body;
                    entity1.pageurl = "sumcouponedit.aspx";
                    entity1.otype = SystemEnum.LogOperateType.Edit;
                    entity1.cuser = StringHelper.StringToLong(uuser);
                    entity1.logcontent = "修改sumid为" + sumid1 + "的优惠券活动信息";
                    dt = bll.Update(GUID, UID, sumid1, sumcode1, buscode1, stocode1, cname1, btime1, etime1, ctype1, secctype1, initype1, status, descr1, auduser1, audremark1, audstatus1, cuser, uuser, entity1);
                    dt.TableName = "error";
                    json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    break;
                case "getmccodebymcid":
                    string mcid = Helper.ReplaceString(context.Request["mcid"]);
                    string strSql = string.Format("select mccode from maincoupon where mcid={0}", mcid);
                    dt = bll.getDataTableBySql(strSql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        json = JsonHelper.ToJson("0", dt.Rows[0][0].ToString());
                    }
                    else
                    {
                        json = JsonHelper.ToJson("1", "");
                    }
                    break;
                case "maincouponsiginfo":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    filter = Helper.ReplaceString(context.Request["filter"]);
                    filter = filter.Replace("‘", "'");
                    recnums = 0;
                    pagenums = 0;
                    dt = bllmain.GetPagingListInfo(GUID, UID, 1, 1, filter, string.Empty, out recnums, out pagenums);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt.TableName = "error";
                        json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    }
                    else
                    {
                        json = JsonHelper.ToJson("1", "");
                    }
                    break;
                case "maincouponlistinfo":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    filter = Helper.ReplaceString(context.Request["filter"]);
                    filter = filter.Replace("‘", "'");
                    recnums = 0;
                    pagenums = 0;
                    dt = bllmain.GetListInfo(GUID, UID, filter);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt.TableName = "error";
                        json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    }
                    else
                    {
                        json = JsonHelper.ToJson("1", "");
                    }
                    break;
                case "maincouponexportinfo":
                    string sumid2 = Helper.ReplaceString(context.Request["sumid"]);
                    string mcid1 = Helper.ReplaceString(context.Request["mcid"]);
                    dt = new bllPaging().GetDataTableInfoBySQL(@"
select RIGHT('00000000'+CAST(c.serialno as varchar(10)),5)+'('+mc.batchno+')' as sortno,sc.cname,mc.couname,convert(varchar(10),mc.btime,102)+'-'+convert(varchar(10),mc.etime,102) as vaildate,c.checkcode 
from coupon c left join sumcoupon sc on c.sumcode= sc.sumcode left join maincoupon mc on c.mccode=mc.mccode
where sc.sumid=" + sumid2 + " and mc.mcid=" + mcid1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt.TableName = "error";
                        json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    }
                    else
                    {
                        json = JsonHelper.ToJson("1", "");
                    }
                    break;
                case "sumcouponrepulse":
                    GUID = Helper.ReplaceString(context.Request["guid"]);
                    UID = Helper.ReplaceString(context.Request["uid"]);
                    string userid = Helper.ReplaceString(context.Request["userid"]);
                    string sumid3 = Helper.ReplaceString(context.Request["sumid"]);
                    string reason = Helper.ReplaceString(context.Request["reason"]);
                    stocode = Helper.ReplaceString(context.Request["stocode"]);
                    status = Helper.ReplaceString(context.Request["status"]);
                    string ffcuser = Helper.ReplaceString(context.Request["ffcuser"]);
                    string ffcusercode = Helper.ReplaceString(context.Request["ffcusercode"]);
                    cuser = Helper.ReplaceString(context.Request["cuser"]);
                    logentity.module = ErrMessage.GetMessageInfoByCode("sumcoupon_Menu").Body;
                    logentity.pageurl = "coupon/repulse.aspx(coupon/pass.aspx)";
                    logentity.otype = SystemEnum.LogOperateType.Audit;
                    logentity.cuser = StringHelper.StringToLong(cuser);
                    logentity.logcontent = string.Format(ErrMessage.GetMessageInfoByCode("sumcoupon_963").Body + "   status=" + status, userid, sumid3);
                    dt = new bllsumcoupon().AuditMV(GUID, UID, userid, sumid3, status, reason, stocode, ffcuser, ffcusercode, logentity);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt.TableName = "error";
                        json = JsonHelper.ToJson("0", "", new ArrayList() { dt }, new string[] { "data" });
                    }
                    else
                    {
                        json = JsonHelper.ToJson("1", "");
                    }
                    break;
                default:
                    break;
            }
            json = Regex.Replace(json, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt1 = new DateTime(1970, 1, 1);
                dt1 = dt1.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt1 = dt1.ToLocalTime();
                return dt1.ToString("yyyy-MM-dd HH:mm:ss");
            });
            json = Regex.Replace(json, @"\\/Date(-2209017600000)\\/", match =>
            {
                return "";
            });
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}