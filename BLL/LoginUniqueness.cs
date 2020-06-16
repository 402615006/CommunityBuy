using System;
using System.Collections;
using System.Data;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 单点登录操作
    /// </summary>
    public class LoginUniqueness : bllBase
    {
        /// <summary>
        /// 判断用户登录是否合法性
        /// </summary>
        /// <param name="GUID">唯一标识</param>
        /// <param name="UserID">用户ID</param>
        /// <returns>返回：""空字符合法，否则非法</returns>
        public static DataTable LoginedCheckFromPage(string GUID, string UserID)
        {
            DataTable dt = new DataTable("error");
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("mes", typeof(string));
            dt.Columns.Add("spanids", typeof(string));
            dt.AcceptChanges();
            if (UserID != "0")
            {
                //验证用户合法性
                if (!LoginedCheck(GUID, UserID))
                {
                    DataRow LoginVerify = dt.NewRow();
                    LoginVerify["type"] = "-1";
                    LoginVerify["mes"] = "用户已在其他位置登录，请重新登录！";
                    dt.Rows.Add(LoginVerify);
                }
            }
            return dt;
        }

        /// <summary>
        /// 登录身份验证
        /// </summary>
        /// <param name="GUID">登录GUID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns>返回是否合法</returns>
        public static bool LoginedCheck(string GUID, string UserID)
        {
            bool Flag = true;
            Hashtable hOnline = MemCached.GetCache<Hashtable>("LoginOnline");
            if (hOnline != null)
            {
                object Val = hOnline[UserID];
                if (Val != null && Val.ToString() == GUID)
                {
                    Flag = true;
                }
            }
            return Flag;
        }

        /// <summary>
        /// 获取在线人数
        /// </summary>
        /// <returns></returns>
        public static int GetOnlinePerson()
        {
            int Flag = 0;
            Hashtable hOnline = MemCached.GetCache<Hashtable>("LoginOnline");
            if (hOnline != null)
            {
                Flag = hOnline.Count;
            }
            return Flag;
        }

        /// <summary>
        /// 登录系统
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns>返回：唯一GUID</returns>
        public static string LoginedSetKey(string UserID)
        {
            //以guid作为用户的唯一标识
            string guid = Guid.NewGuid().ToString();
            //
            Hashtable hOnline = MemCached.GetCache<Hashtable>("LoginOnline");
            //Hashtable hOnline = (Hashtable)WebCache.GetCache("LoginOnline");
            if (hOnline == null)
            {
                hOnline = new Hashtable();
            }

            hOnline[UserID] = guid;
            //WebCache.Insert("LoginOnline", hOnline,0);
            //MemCached.AddOrReplaceCache<Hashtable>("LoginOnline", hOnline, DateTime.Now.AddYears(1));
            return guid;
        }

        #region 网易短信
        /// <summary>
        /// 发送手机短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        public static bool MobileMesSendByTemp1(string mobile, string descr, ref string vercode)
        {
            bool Flag = false;
            string SendKey = "mob_" + mobile;
            Random rd = new Random();
            vercode = rd.Next(100001, 999999).ToString();
            Flag = NetEaseNoteInterface.SendTemplateByNetEase("temp1", new string[1] { mobile }, new string[2] { descr, vercode });
            if (Flag)
            {
                WebCache.Insert(SendKey, vercode, 5);
                //MemCached.AddOrReplaceCache<string>(SendKey, vercode, DateTime.Now.AddMinutes(5));
            }
            return Flag;
        }

        /// <summary>
        /// 发送手机短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        public static string MobileMesSendByTemp1_1(string mobile, string descr, ref string vercode)
        {
            string returnVal = string.Empty;
            string SendKey = "mob_" + mobile;
            Random rd = new Random();
            vercode = rd.Next(100001, 999999).ToString();
            returnVal = NetEaseNoteInterface.SendTemplateByNetEase2("temp1", new string[1] { mobile }, new string[2] { descr, vercode });
            if (returnVal != "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
                //MemCached.AddOrReplaceCache<string>(SendKey, vercode, DateTime.Now.AddMinutes(5));
            }
            return returnVal;
        }

        /// <summary>
        /// 发送存酒过期短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        public static string MobileMesSendByTemp5(string mobile, string savetime, string stoname, string descr, string vercode, string stotel)
        {
            string returnVal = string.Empty;
            string SendKey = "mob_" + mobile;
            //您好,您在%s寄存的:%s剩余7天,请尽快前来饮用,如有疑问请拨打电话:%s,谢谢!

            //returnVal = NetEaseNoteInterface.SendTemplateByNetEase2("temp5", new string[1] { mobile }, new string[2] { stoname, descr });
            returnVal = NetEaseNoteInterface.SendTemplateByNetEase2("temp5", new string[1] { mobile }, new string[3] { stoname, descr, stotel });
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }

        /// <summary>
        /// 预订提醒
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        /// //"username", "resdatetime", "metname", "stoname", "tel", 
        public static string MobileMesSendByTemp6(string mobile, string username, string resdatetime, string metname, string stoname, string tel)
        {
            Random rd = new Random();
            string vercode = rd.Next(100001, 999999).ToString();
            string returnVal = string.Empty;
            string SendKey = "mob_" + mobile;
            returnVal = NetEaseNoteInterface.SendTemplateByNetEase2("temp6", new string[1] { mobile }, new string[5] { username, resdatetime, metname, stoname, tel });
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }


        /// <summary>
        /// 发送酒吧预订短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        /// //"mobile","username", "resdatetime", "tmname", "endtime", "address", "tel","stoname"
        public static string MobileMesSendByTemp7(string mobile, string username, string resdatetime, string tmname, string endtime, string address, string tel, string stoname)
        {
            Random rd = new Random();
            string vercode = rd.Next(100001, 999999).ToString();
            string returnVal = string.Empty;
            string SendKey = "mob_" + mobile;
            returnVal = NetEaseNoteInterface.SendTemplateByNetEase2("temp7", new string[1] { mobile }, new string[6] { username, resdatetime, tmname, endtime, address, tel });
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }
        /// <summary>
        /// 存酒短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="descr"></param>
        /// <param name="vercode"></param>
        /// <returns></returns>
        public static bool MobileMesSendByTemp3(string mobile, string savetime, string stoname, string descr, string endtime, ref string vercode)
        {
            bool Flag = false;
            string SendKey = "mob_" + mobile;
            if (vercode.Length == 0)
            {
                Random rd = new Random();
                vercode = rd.Next(100001, 999999).ToString();
            }
            Flag = NetEaseNoteInterface.SendTemplateByNetEase("temp3", new string[1] { mobile }, new string[5] { savetime, stoname, descr, endtime, vercode });
            if (Flag)
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return Flag;
        }

        /// <summary>
        /// 存酒短信
        /// </summary>
        /// <param name="mobile">接收手机号</param>
        /// <param name="savetime">存酒时间</param>
        /// <param name="stoname">存酒门店</param>
        /// <param name="descr">内容</param>
        /// <param name="endtime">存酒截止时间</param>
        /// <param name="vercode">验证码</param>
        /// <returns>短信发送平台返回的code</returns>
        public static string MobileMesSendByTemp3_1(string mobile, string savetime, string stoname, string descr, string endtime, ref string vercode)
        {
            string returnVal = string.Empty;
            string SendKey = "mob_" + mobile;
            if (vercode.Length == 0)
            {
                Random rd = new Random();
                vercode = rd.Next(100001, 999999).ToString();
            }
            returnVal = NetEaseNoteInterface.SendTemplateByNetEase2("temp3", new string[1] { mobile }, new string[5] { savetime, stoname, descr, endtime, vercode });
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }

        /// <summary>
        /// 取酒短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="descr"></param>
        /// <param name="vercode"></param>
        /// <returns></returns>
        public static bool MobileMesSendByTemp4(string mobile, string nowdate, string stoname, string descr, string otelphone, string vercode)
        {
            bool Flag = false;
            string SendKey = "mob_" + mobile;
            Flag = NetEaseNoteInterface.SendTemplateByNetEase("temp4", new string[1] { mobile }, new string[5] { nowdate, stoname, descr, otelphone, vercode });
            if (Flag)
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return Flag;
        }

        /// <summary>
        /// 取酒短信
        /// </summary>
        /// <param name="mobile">新手机号</param>
        /// <param name="nowdate">取酒时间</param>
        /// <param name="stoname">取酒门店</param>
        /// <param name="descr">取酒内容</param>
        /// <param name="otelphone">老手机号</param>
        /// <param name="vercode">验证码</param>
        /// <returns>短信发送平台返回的code</returns>
        public static string MobileMesSendByTemp4_1(string mobile, string nowdate, string stoname, string descr, string otelphone, string vercode)
        {
            string returnVal = string.Empty;
            string SendKey = "mob_" + mobile;
            returnVal = NetEaseNoteInterface.SendTemplateByNetEase2("temp4", new string[1] { mobile }, new string[5] { nowdate, stoname, descr, otelphone, vercode });
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="vercode">验证码</param>
        /// <returns>返回 0.成功 1.过期 2.验证码错误</returns>
        public static int MobileMesCheck(string mobile, string vercode)
        {
            string SendKey = "mob_" + mobile;
            //string objMes = MemCached.GetCache<string>(SendKey);
            object objMes = WebCache.GetCache(SendKey);
            if (objMes == null)
            {
                return 1;//过期
            }
            if (objMes.ToString() != vercode)
            {
                return 2; //验证码错误
            }
            return 0;
        }
        #endregion

        #region 阿里短信
        /// <summary>
        /// 发送手机短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        public static bool AliMobileMesSendByTemp(string mobile, string descr, ref string vercode, string buscode, string stocode)
        {
            bool Flag = false;
            string SendKey = mobile;
            Random rd = new Random();
            vercode = rd.Next(100001, 999999).ToString();
            Flag = AliYunSendMessage.SendTemplateByNetEaseBool("Ali_temp1", new string[1] { mobile }, new string[2] { descr, vercode }, buscode, stocode);
            if (Flag)
            {
                WebCache.Insert(SendKey, vercode, 5);
                //MemCached.AddOrReplaceCache<string>(SendKey, vercode, DateTime.Now.AddMinutes(5));
            }
            return Flag;
        }

        /// <summary>
        /// 发送手机短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        public static string AliMobileMesSendByTemp1_1(string mobile, string descr, ref string vercode, string buscode, string stocode)
        {
            string returnVal = string.Empty;
            string SendKey = mobile;
            Random rd = new Random();
            vercode = rd.Next(100001, 999999).ToString();
            returnVal = AliYunSendMessage.SendTemplateByNetEaseCode("Ali_temp1", new string[1] { mobile }, new string[2] { descr, vercode }, buscode, stocode);
            if (returnVal != "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
                //MemCached.AddOrReplaceCache<string>(SendKey, vercode, DateTime.Now.AddMinutes(5));
            }
            return returnVal;
        }

        /// <summary>
        /// 发送存酒过期短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        public static string AliMobileMesSendByTemp5(string mobile, string savetime, string stoname, string descr, string vercode, string buscode, string stocode, string strTel)
        {
            string returnVal = string.Empty;
            string SendKey = mobile;
            returnVal = AliYunSendMessage.SendTemplateByNetEaseCode("Ali_temp5", new string[1] { mobile }, new string[3] { stoname, descr, strTel }, buscode, stocode);
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }

        /// <summary>
        /// 餐厅预定
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        /// //"username", "resdatetime", "metname", "stoname", "tel", 
        public static string AliMobileMesSendByTemp6(string mobile, string username, string resdatetime, string metname, string stoname, string tel, string buscode, string stocode)
        {
            Random rd = new Random();
            string vercode = rd.Next(100001, 999999).ToString();
            string returnVal = string.Empty;
            string SendKey = mobile;
            returnVal = AliYunSendMessage.SendTemplateByNetEaseCode("Ali_temp15", new string[1] { mobile }, new string[5] { username, resdatetime, stoname, metname, tel }, buscode, stocode);
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }


        /// <summary>
        /// 发送酒吧预订短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="vercode">返回验证码</param>
        /// <returns></returns>
        /// //"mobile","username", "resdatetime", "tmname", "endtime", "address", "tel","stoname"
        public static string AliMobileMesSendByTemp7(string mobile, string username, string resdatetime, string tmname, string endtime, string address, string tel, string stoname, string buscode, string stocode)
        {
            Random rd = new Random();
            string vercode = rd.Next(100001, 999999).ToString();
            string returnVal = string.Empty;
            string SendKey = mobile;
            returnVal = AliYunSendMessage.SendTemplateByNetEaseCode("Ali_temp6", new string[1] { mobile }, new string[6] { username, resdatetime, tmname, endtime, address, tel }, buscode, stocode);
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }


        /// <summary>
        /// 存酒短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="descr"></param>
        /// <param name="vercode"></param>
        /// <returns></returns>
        public static bool AliMobileMesSendByTemp3(string mobile, string savetime, string stoname, string descr, string endtime, ref string vercode, string buscode, string stocode)
        {
            bool Flag = false;
            string SendKey = mobile;
            if (vercode.Length == 0)
            {
                Random rd = new Random();
                vercode = rd.Next(100001, 999999).ToString();
            }
            Flag = AliYunSendMessage.SendTemplateByNetEaseBool("Ali_temp3", new string[1] { mobile }, new string[5] { savetime, stoname, descr, endtime, vercode }, buscode, stocode);
            if (Flag)
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return Flag;
        }

        /// <summary>
        /// 存酒短信
        /// </summary>
        /// <param name="mobile">接收手机号</param>
        /// <param name="savetime">存酒时间</param>
        /// <param name="stoname">存酒门店</param>
        /// <param name="descr">内容</param>
        /// <param name="endtime">存酒截止时间</param>
        /// <param name="vercode">验证码</param>
        /// <returns>短信发送平台返回的code</returns>
        public static string AliMobileMesSendByTemp3_1(string mobile, string savetime, string stoname, string descr, string endtime, ref string vercode, string buscode, string stocode)
        {
            string returnVal = string.Empty;
            string SendKey = mobile;
            if (vercode.Length == 0)
            {
                Random rd = new Random();
                vercode = rd.Next(100001, 999999).ToString();
            }
            returnVal = AliYunSendMessage.SendTemplateByNetEaseCode("Ali_temp3", new string[1] { mobile }, new string[5] { savetime, stoname, descr, endtime, vercode }, buscode, stocode);
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }

        /// <summary>
        /// 取酒短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="descr"></param>
        /// <param name="vercode"></param>
        /// <returns></returns>
        public static bool AliMobileMesSendByTemp4(string mobile, string nowdate, string stoname, string descr, string otelphone, string vercode, string buscode, string stocode)
        {
            bool Flag = false;
            string SendKey = mobile;
            Flag = AliYunSendMessage.SendTemplateByNetEaseBool("Ali_temp4", new string[1] { mobile }, new string[5] { nowdate, stoname, descr, otelphone, vercode }, buscode, stocode);
            if (Flag)
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return Flag;
        }

        /// <summary>
        /// 取酒短信
        /// </summary>
        /// <param name="mobile">新手机号</param>
        /// <param name="nowdate">取酒时间</param>
        /// <param name="stoname">取酒门店</param>
        /// <param name="descr">取酒内容</param>
        /// <param name="otelphone">老手机号</param>
        /// <param name="vercode">验证码</param>
        /// <returns>短信发送平台返回的code</returns>
        public static string AliMobileMesSendByTemp4_1(string mobile, string nowdate, string stoname, string descr, string otelphone, string vercode, string buscode, string stocode)
        {
            string returnVal = string.Empty;
            string SendKey = mobile;
            returnVal = AliYunSendMessage.SendTemplateByNetEaseCode("Ali_temp4", new string[1] { mobile }, new string[5] { nowdate, stoname, descr, otelphone, vercode }, buscode, stocode);
            if (returnVal == "200")
            {
                WebCache.Insert(SendKey, vercode, 5);
            }
            return returnVal;
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="vercode">验证码</param>
        /// <returns>返回 0.成功 1.过期 2.验证码错误</returns>
        public static int AliMobileMesCheck(string mobile, string vercode)
        {
            string SendKey = mobile;
            //string objMes = MemCached.GetCache<string>(SendKey);
            object objMes = WebCache.GetCache(SendKey);
            if (objMes == null)
            {
                return 1;//过期
            }
            if (objMes.ToString() != vercode)
            {
                return 2; //验证码错误
            }
            return 0;
        }
        #endregion

        /// <summary>
        /// 登出系统，清除系统缓存
        /// </summary>
        /// <param name="UserID">用户ID</param>
        public static bool LogoutSystem(string UserID)
        {
            Hashtable hOnline = MemCached.GetCache<Hashtable>("LoginOnline");
            if (hOnline != null)
            {
                hOnline.Remove(UserID);
                MemCached.AddOrReplaceCache<Hashtable>("LoginOnline", hOnline, DateTime.Now.AddYears(1));
                return true;
            }
            return false;
        }
    }
}