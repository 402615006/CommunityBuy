using System;
using System.ComponentModel;
using System.IO;

namespace CommunityBuy.CommonBasic
{
    /// <summary>
    /// 枚举描述类
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumAttribute : Attribute
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 描述：系统错误日志类
    /// 作者：CGD
    /// 日期：2019-04-28 Add
    /// </summary>
    public sealed class ErrorLog
    {
        [Description("日志类型")]
        public enum LogType
        {

            [EnumAttribute(Name = "默认文件")]
            baselog = 0,

  
            [EnumAttribute(Name = "短信服务")]
            ShortMsg = 1,


            [EnumAttribute(Name = "微信服务")]
            WxError = 1
        }

        private static string _logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\logs\\";

        /// <summary>
        /// 将错误信息写入日志文件
        /// </summary>
        /// <param name="ErrorMsg">错误信息字符串</param>
        public static void WriteErrorMessage(LogType LT, string ErrorMsg)
        {
            System.IO.StreamWriter sw = null;
            string filename = "log_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                if (!Directory.Exists(_logFilePath + LT.ToString()))
                {
                    Directory.CreateDirectory(_logFilePath + LT.ToString());
                }
                sw = new System.IO.StreamWriter(_logFilePath + LT.ToString() + "\\" + filename, true, System.Text.Encoding.Default);
                sw.WriteLine();
                sw.WriteLine("Time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "");
                sw.WriteLine("EMes:" + ErrorMsg);
            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
    }
}
