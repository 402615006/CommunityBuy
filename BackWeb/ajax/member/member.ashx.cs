using System;
using System.Collections.Generic;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;

namespace CommunityBuy.BackWeb.ajax
{
    /// <summary>
    /// getuser 的摘要说明
    /// </summary>
    public class member : ServiceBase
    {
            WxHelper wxHelper = new WxHelper();
        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                //ErrorLog.WriteLogMessage("111", "UserCenter.ashx?actionname=" + actionname + "&parameters=" + JsonHelper.ObjectToJSON(dicPar));
                if (dicPar != null)
                {
                    switch (actionname)
                    {
                        case "getopenid"://获取用户气泡提示
                            GetOpenId(dicPar);
                            break;
                        case "getmember"://获取用户openid sessionkey
                            GetMember(dicPar);
                            break;
                        case "mpdecrypt"://微信加密字符串解密
                            MpDecrypt(dicPar);
                            break;
                        case "getusertips"://获取首页 弹出用户提醒
                            GetUserTips(dicPar);
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// 获取用户消息
        /// </summary>
        /// <param name="dicPar"></param>
        //public void GetUserMsg(Dictionary<string, object> dicPar)
        //{
        //    List<string> pra = new List<string>() { "GUID", "status", "memcode" };
        //    //检测方法需要的参数
        //    if (!CheckActionParameters(dicPar, pra))
        //    {
        //        return;
        //    }
        //    var memcode = dicPar["memcode"].ToString();
        //    var sql = "SELECT COUNT(ID) FROM dbo.mv_filmorder WHERE memcode='" + memcode + "' and isdelete='0';";
        //    var waitTable = SQL.XJWZSQLTool.ExecuteDataTable(sql);

        //    //获取用户优惠券数量
        //    string couponno = memberCls.GetCouponNumber(memcode).ToString();

        //    var waitPay = 0;
        //    var waitUse = 0;
        //    var ds = SQL.XJWZSQLTool.ExecuteDataset("select distinct ordernoex from mv_orders where memcode='" + memcode + "' and status='0';select distinct ordernoex from mv_orders where ordernoex in(select distinct mo.ordernoex from mv_orders mo left join mv_Schedules ms on mo.scheduleId=ms.scheduleId left join mv_films mf on ms.filmCode=mf.filmcode where mo.memcode='" + memcode + "' and mo.ispickup='0' and mo.status='1' and DATEADD(MINUTE,mf.duration,ms.showDateTime)>getdate() and isgoods='0' and checkcode<>'' union all select distinct mo.ordernoex from mv_orders mo left join mv_goods mg on mo.gid=mg.goodsid where mo.memcode='" + memcode + "' and mo.ispickup='0' and mo.status='1' and isgoods='1' and mg.gtype='1');");
        //    if (ds.Tables.Count == 2)
        //    {
        //        DataTable dtWaitPay = ds.Tables[0];
        //        DataTable dtWaitUse = ds.Tables[1];

        //        if (dtWaitPay != null && dtWaitPay.Rows.Count > 0)
        //        {
        //            waitPay = dtWaitPay.Rows.Count;
        //        }

        //        if (dtWaitUse != null && dtWaitUse.Rows.Count > 0)
        //        {
        //            waitUse = dtWaitUse.Rows.Count;
        //        }
        //    }

        //    string waitmovie = "0";
        //    if (waitTable != null && waitTable.Rows.Count > 0)
        //    {
        //        waitmovie = waitTable.Rows[0][0].ToString();
        //    }
        //    var strJson = "{\"status\":\"0\",\"mes\":\"获取数据成功\",\"data\":[{";
        //    strJson += "\"waitPay\":\"" + waitPay + "\",";
        //    strJson += "\"waitUse\":\"" + waitUse + "\",";
        //    strJson += "\"coupon\":\"" + couponno + "\",";
        //    strJson += "\"movie\":\"" + waitmovie + "\"";
        //    strJson += "}]}";
        //    ToJsonStr(strJson);

        //}


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetOpenId(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "code" };
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            var code = dicPar["code"].ToString();
            var result = wxHelper.GetOpenIdAndSessionKeyString(code);
            ReturnJsonStr(result);
        }

        /// <summary>
        /// 微信小程序获取信息
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetMember(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "openid","mobile" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            var openid = dicPar["openid"].ToString();
            var mobile = dicPar["mobile"].ToString();
            if (string.IsNullOrEmpty(mobile))
            {
                ReturnResultJson("2", "手机号为空，请在微信中绑定手机号");
                return;
            }

            if (openid == "undefined")
            {
                ReturnResultJson("2", "网络繁忙，请稍后再试");
                return;
            }
            bllmembers bllmembers = new bllmembers();
            membersEntity member=bllmembers.GetEntitySigInfo(" where wxaccount='"+ openid + "'"); 
            if (!string.IsNullOrWhiteSpace(member.memid))//已注册过会员
            {
                ReturnJsonStr(JsonHelper.ObjectToJSON(member));
            }
            else//未注册过会员
            {
                bllmembers.Add("", "", "", "", openid, mobile, "", "1");
                member = bllmembers.GetEntitySigInfo(" where wxaccount='" + openid + "'");
                ReturnJsonStr(JsonHelper.ObjectToJSON(member));
            }
        }

        /// <summary>
        /// 微信加密字符串解密
        /// </summary>
        /// <param name="dicPar"></param>
        public void MpDecrypt(Dictionary<string, object> dicPar)
        {
            List<string> pra = new List<string>() { "encryptedData", "iv", "sessionKey" };

            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            try
            {
                var encryptedData = dicPar["encryptedData"].ToString();
                var iv = dicPar["iv"].ToString();
                var sessionKey = dicPar["sessionKey"].ToString();
                var result = wxHelper.Decrypt(encryptedData.Replace(" ", "+"), iv, sessionKey);
                if (result != "fail")
                {
                    ReturnJsonStr(result);
                }
                else
                {
                    ReturnResultJson("-1", "网络错误,请稍后重试！");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }

        }

        /// <summary>
        /// 首页 弹出用户提醒
        /// </summary>
        /// <param name="dicPar"></param>
        public void GetUserTips(Dictionary<string, object> dicPar)
        {
            //List<string> pra = new List<string>() { "GUID", "USER_ID", "memcode" };

            //if (!CheckActionParameters(dicPar, pra))
            //{
            //    return;
            //}
            //var memcode = dicPar["memcode"].ToString();

            //BLL.bllMVfilmorder bll = new BLL.bllMVfilmorder();
            //DataTable dt = bll.GetUserTips(memcode);

            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    string id = dt.Rows[0]["id"].ToString();
            //    SQL.XJWZSQLTool.ExecuteNonQuery("update mv_usertips set status='1' where id='" + id + "';");
            //    string jsonStr = "{\"status\":\"0\",\"mes\":\"获取数据成功\",\"data\":[";
            //    jsonStr += "{\"id\":\"" + dt.Rows[0]["id"].ToString() + "\",\"actid\":\"" + dt.Rows[0]["actid"].ToString() + "\",\"acttype\":\"" + dt.Rows[0]["acttype"].ToString() + "\",\"activityname\":\"" + dt.Rows[0]["activityname"].ToString() + "\",\"thumbnailpath\":\"" + Helper.GetAppSettings("imgurl") + dt.Rows[0]["thumbnailpath"].ToString() + "\",\"type\":\"" + dt.Rows[0]["type"].ToString() + "\",\"couponname\":\"" + dt.Rows[0]["couponname"].ToString() + "\"}]}";
            //    ToJsonStr(jsonStr);
            //}
            //else
            //{
            //    ReturnResultJson("1", "暂无数据");
            //}
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