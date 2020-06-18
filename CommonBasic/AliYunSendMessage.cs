using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using CommunityBuy.CommonBasic;
using System.Windows.Forms;

namespace CommunityBuy.CommonBasic
{
    /// <summary>
    /// 阿里云短信发送类
    /// </summary>
    public class AliYunSendMessage
    {

        public static String product = "Dysmsapi";
        //产品域名,开发者无需替换
        public static String domain =Helper.GetAppSettings("Ali_ShortMesUrl");//dysmsapi.aliyuncs.com
        // TODO 此处需要替换成开发者自己的AK(在阿里云访问控制台寻找)
        public static String accessKeyId = Helper.GetAppSettings("Ali_AppKey");//"LTAIRQGtZiXzFU4Z";
        public static String accessKeySecret = Helper.GetAppSettings("Ali_AppSecret");// "MOSi0rGbyxaucFzl6esh4JuOnsJw7Y";

        /// <summary>
        /// 验证类型
        /// </summary>
        public enum SendType
        {
            //发送模板短信
            [EnumAttribute(Name = "发送模板短信")]
            SendTemplate = 1,
            //查询模板短信发送状态
            [EnumAttribute(Name = "查询模板短信发送状态")]
            Querystatus = 2,
            //发送短信验证码
            [EnumAttribute(Name = "发送短信验证码")]
            Sendcode = 3,
            //校验验证码
            [EnumAttribute(Name = "校验验证码")]
            Verifycode = 4
        }


        /// <summary>
        /// 发送模板短信
        /// </summary>
        /// <param name="templateid">模板key</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="Mes">信息</param>
        /// <returns></returns>
        public static bool SendTemplateByNetEaseBool(string templateid, string[] Mobile, string[] Mes, string buscode, string stocode)
        {
            return SendShortMessageByNetEase(SendType.SendTemplate, templateid, Mobile, Mes, buscode, stocode);
        }

        /// <summary>
        /// 发送模板短信
        /// </summary>
        /// <param name="templateid">模板key</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="Mes">信息</param>
        /// <returns></returns>
        public static string SendTemplateByNetEaseCode(string templateid, string[] Mobile, string[] Mes, string buscode, string stocode)
        {
            return SendShortMessageByNetEaseCode(SendType.SendTemplate, templateid, Mobile, Mes, buscode, stocode);
        }


        #region 发送并记录
        /// <summary>
        /// 阿里云短信接口 返回true或false  成功返回true
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="templateid">模板code</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="Mes">模板参数</param>
        /// <param name="buscode"></param>
        /// <param name="stocode">门店编号</param>
        /// <returns></returns>
        private static bool SendShortMessageByNetEase(SendType type, string templateid, string[] Mobile, string[] Mes, string buscode, string stocode)
        {
            StringBuilder sbsql = null;
            bool Flag = false;

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SendSmsResponse response = null;
            string sendtime = DateTime.Now.ToString();
            //必填:短信签名-可在短信控制台中找到
            request.SignName = "Icard会员中心";
            string postStr = "";
            string mobs = "";
            string tempid = string.Empty;

            string retID = string.Empty;
            try
            {
                foreach (string str in Mobile)
                {
                    mobs += str + ",";
                }
                mobs = mobs.TrimEnd(',');
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = mobs;
                //必填:短信模板-可在短信控制台中找到
                tempid =Helper.GetAppSettings(templateid);
                request.TemplateCode = "SMS_" + tempid;
                switch (type)
                {
                    //发送模板短信
                    case SendType.SendTemplate:
                    case SendType.Sendcode:
                        //根据模板编号获取参数
                        postStr = GetTemplateParamJson(templateid, Mes);
                        break;
                    //查询模板短信发送状态
                    case SendType.Querystatus:

                        break;
                    //校验验证码
                    case SendType.Verifycode:

                        break;
                }
                request.TemplateParam = postStr;
                //记录短信日志
                sbsql = new StringBuilder();

                sbsql.Append("declare @id bigint;insert into SendSmsLogs([buscode],[stocode],[mobile],[smscontent],[status],[failurereason],[sendtime]) ");
                sbsql.Append(" values('" + buscode + "','" + stocode + "','" + mobs + "','" + postStr + "|调用模板" + tempid + "','0','',getdate());set @id=SCOPE_IDENTITY();select @id;");
                object resultobj = new MSSqlDataAccess().ExecuteScalar(sbsql.ToString());
                if (resultobj != null)
                {
                    retID = resultobj.ToString();
                }

                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);
                if (response.Code == "OK")
                {
                    Flag = true;
                    sbsql.Clear();
                    sbsql.Append("update SendSmsLogs set [status]='1',failurereason='成功'  where ID='" + retID + "'");
                    new MSSqlDataAccess().ExecuteNonQuery(sbsql.ToString());
                }
                else
                {
                    sbsql.Clear();
                    sbsql.Append("update SendSmsLogs set failurereason='" + GetResultInfo2(response.Code) + "'  where ID='" + retID + "'");
                    new MSSqlDataAccess().ExecuteNonQuery(sbsql.ToString());
                }
            }
            catch (Aliyun.Acs.Core.Exceptions.ServerException e)
            {
                sbsql.Clear();
                sbsql.Append("update SendSmsLogs set failurereason='" + e.Message + "'  where ID='" + retID + "'");
                new MSSqlDataAccess().ExecuteNonQuery(sbsql.ToString());
            }
            catch (ClientException e)
            {
                sbsql.Clear();
                sbsql.Append("update SendSmsLogs set failurereason='" + e.Message + "'  where ID='" + retID + "'");
                new MSSqlDataAccess().ExecuteNonQuery(sbsql.ToString());
            }
            catch (Exception ex)
            {
                sbsql.Clear();
                sbsql.Append("update SendSmsLogs set failurereason='" + ex.Message + "'  where ID='" + retID + "'");
                new MSSqlDataAccess().ExecuteNonQuery(sbsql.ToString());
            }
            return Flag;
        }


        /// <summary>
        /// 阿里云短信接口，返回状态码  成功返回200
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="templateid">模板code</param>
        /// <param name="Mobile">手机号码</param>
        /// <param name="Mes">模板参数</param>
        /// <param name="buscode"></param>
        /// <param name="stocode">门店编号</param>
        /// <returns></returns>
        private static string SendShortMessageByNetEaseCode(SendType type, string templateid, string[] Mobile, string[] Mes, string buscode, string stocode)
        {
            string resultCode = string.Empty;

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SendSmsResponse response = null;

            //必填:短信签名-可在短信控制台中找到
            request.SignName = "Icard会员中心";
            string postStr = "";
            string mobs = "";
            string tempid = string.Empty;
            string sendtime = DateTime.Now.ToString();

                foreach (string str in Mobile)
                {
                    mobs += str + ",";
                }
                mobs = mobs.TrimEnd(',');
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = mobs;
                //必填:短信模板-可在短信控制台中找到
                tempid =Helper.GetAppSettings(templateid);
                request.TemplateCode = "SMS_" + tempid;
                switch (type)
                {
                    //发送模板短信
                    case SendType.SendTemplate:
                    case SendType.Sendcode:
                        //根据模板编号获取参数
                        postStr = GetTemplateParamJson(templateid, Mes);
                        break;
                    //查询模板短信发送状态
                    case SendType.Querystatus:

                        break;
                    //校验验证码
                    case SendType.Verifycode:

                        break;
                }
                request.TemplateParam = postStr;
            return resultCode;
        }
        #endregion


        /// <summary>
        /// 返回指定模板的请求参数，如后续有新的短信模板，则只需在此处添加模板对应的请求参数
        /// </summary>
        /// <param name="TemplateCode"></param>
        /// <returns></returns>
        private static string GetTemplateParamJson(string TemplateCode, string[] Mes)
        {
            string postStr = string.Empty;
            switch (TemplateCode)
            {

                case "Ali_Login"://
                    break;
                case "Ali_temp2"://
                    postStr = "{\"vercode\":\"" + Mes[1] + "\"}";
                    break;
                case "Ali_temp16"://业务提醒
                    postStr = "{\"stoname\":\"" + Mes[0] + "\",\"name\":\"" + Mes[1] + "\",\"num\":\"" + Mes[2] + "\",\"no\":\"" + Mes[3] + "\",\"time\":\"" + Mes[4] + "\"}";
                    break;
            }
            return postStr;
        }


        /// <summary>
        /// 匹配短信发送结果
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetResultInfo2(string code)
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("OK", "请求成功");
                ht.Add("isp.RAM_PERMISSION_DENY", "RAM权限DENY");
                ht.Add("isp.isv.OUT_OF_SERVICE", "业务停机");
                ht.Add("isv.OUT_OF_SERVICE", "未开通云通信产品的阿里云客户");
                ht.Add("isv.PRODUCT_UNSUBSCRIBE", "产品未开通");
                ht.Add("isv.ACCOUNT_NOT_EXISTS", "账户不存在");
                ht.Add("isv.ACCOUNT_ABNORMAL", "账户异常");
                ht.Add("isv.SMS_TEMPLATE_ILLEGAL", "短信模板不合法");
                ht.Add("isv.SMS_SIGNATURE_ILLEGAL", "短信签名不合法");
                ht.Add("isv.INVALID_PARAMETERS", "参数异常");
                ht.Add("isp.SYSTEM_ERROR", "系统错误");
                ht.Add("isv.MOBILE_NUMBER_ILLEGAL", "非法手机号");
                ht.Add("isv.MOBILE_COUNT_OVER_LIMIT", "手机号码数量超过限制");
                ht.Add("isv.TEMPLATE_MISSING_PARAMETERS", "模板缺少变量");
                ht.Add("isv.BUSINESS_LIMIT_CONTROL", "业务限流");
                ht.Add("isv.INVALID_JSON_PARAM", "JSON参数不合法，只接受字符串值");
                ht.Add("isv.BLACK_KEY_CONTROL_LIMIT", "黑名单管控");
                ht.Add("isv.PARAM_LENGTH_LIMIT", "参数超出长度限制");
                ht.Add("isv.PARAM_NOT_SUPPORT_URL", "不支持URL");
                ht.Add("isv.AMOUNT_NOT_ENOUGH", "账户余额不足");
                return ht[code].ToString();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.ShortMsg, ex.Message);
                return "";
            }
        }

        #region 加密
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="mystr"></param>
        /// <returns></returns>
        public static string GetSHA1String(string mystr)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();

            //将mystr转换成byte[]
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(mystr);

            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "").ToLower();

            return hash;
        }

        /// <summary>
        /// SHA1(AppSecret + Nonce + CurTime),三个参数拼接的字符串，进行SHA1哈希计算，转化成16进制字符(String，小写)
        /// </summary>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string GetCheckSumString(string AppSecret, string Nonce, string CurTime)
        {
            return GetSHA1String(AppSecret + Nonce + CurTime);
        }
        #endregion

    }
}
