using System.Drawing;
namespace CommunityBuy.WebControl
{
    sealed public class ControlHelp
    {
        /// <summary>
        /// 获取控件背景颜色
        /// </summary>
        /// <param name="KeyName">Key名称</param>
        /// <returns>背景颜色</returns>
        public static Color GetColor(string KeyName)
        {
            string strValue = GetappSettings(KeyName);
            if (strValue == null)
            {
                return Color.Yellow;
            }
            else
            {
                string[] FromArgb = strValue.Split(',');
                return Color.FromArgb(int.Parse(FromArgb[0]), int.Parse(FromArgb[1]), int.Parse(FromArgb[2]));
            }
        }

        #region 私有方法

        /// <summary>
        /// 获取App.config文件Key的值
        /// </summary>
        /// <param name="strKey">Key名称</param>
        /// <returns>Value</returns>
        private static string GetappSettings(string strKey)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[strKey].ToString();
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
