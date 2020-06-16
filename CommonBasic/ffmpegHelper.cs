using System;
using System.Diagnostics;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.CommonBasic
{
    /// <summary>
    /// 视频转换类
    /// </summary>
    public class ffmpegHelper
    {
        /// <summary>
        /// 文件转换
        /// </summary>
        /// <param name="exe">ffmpeg EXE路径</param>
        /// <param name="arg">转换命令</param>
        /// <param name="output">输出参数</param>
        public static bool ExcuteProcess(string exe, string arg, DataReceivedEventHandler output)
        {
            try
            {
                using (var p = new Process())
                {
                    p.StartInfo.FileName = exe;
                    p.StartInfo.Arguments = arg;
                    p.StartInfo.UseShellExecute = false;    //输出信息重定向
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.OutputDataReceived += output;
                    p.ErrorDataReceived += output;
                    p.Start();                    //启动线程
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.WaitForExit();            //等待进程结束
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex);
                return false;
            }
        }
    }
}
