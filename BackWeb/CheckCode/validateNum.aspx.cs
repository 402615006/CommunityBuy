using System;
using System.Drawing;

namespace CommunityBuy.BackWeb.CheckCode
{
    public partial class validateNum : System.Web.UI.Page
    {
        #region 自定义随机颜色数组

        Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Brown, Color.DarkCyan, Color.Purple };
        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }
        #endregion

        #region 自定义字体数组

        string[] fonts = { "Arial", "Georgia" };
        public string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Cache.SetNoStore();
            DrawingAPic();
        }

        private void DrawingAPic()
        {
            string strCharAll = "0123456789";
            string chs = "";
            System.Random random = new Random();
            for (int index = 0; index < 4; index++)
            {
                chs += strCharAll[random.Next(strCharAll.Length - 1)].ToString();//Convert.ToChar(iChar).ToString();
            }
            //把生成的验证码存进session以进行对比

            Session["ValidateCode"] = chs;
            Session["ValidateCodeDateTime"] = DateTime.Now;

            // 实例化画布
            System.Drawing.Bitmap bitmap;
            bitmap = new Bitmap(90, 25);

            // 实例化Graphics
            System.Drawing.Graphics grahics;

            // 指定Graphics的操作对象
            grahics = Graphics.FromImage(bitmap);

            //填充背景色
            grahics.Clear(Color.FromArgb(255, 255, 255));

            // 随即数对象
            int i = 0;
            // 生成1000个干扰点
            for (i = 0; i < 10; i++)
            {
                bitmap.SetPixel(random.Next(90), random.Next(25), Color.Tan);
            }

            int cindex1, cindex2, findex;

            for (i = 0; i < 4; i++)
            {
                cindex1 = random.Next(Colors.Length - 1);
                cindex2 = random.Next(Colors.Length - 1);
                findex = random.Next(Fonts.Length - 1);
                Font font = new Font(Fonts[findex], 14, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, bitmap.Width, bitmap.Height), Colors[cindex1], Colors[cindex2], 1.2f, true);
                // 将随机产生的字母输出到画布上，字母坐标和字母颜色均是随机产生
                grahics.DrawString(chs.Substring(i, 1), font, brush, 22 * i, random.Next(0, 5));
            }

            //画图片的背景噪音线
            for (i = 0; i < 5; i++)
            {
                cindex1 = random.Next(Colors.Length - 1);
                int x1 = random.Next(bitmap.Width);
                int x2 = random.Next(bitmap.Width);
                int y1 = random.Next(bitmap.Height);
                int y2 = random.Next(bitmap.Height);

                grahics.DrawLine(new Pen(Colors[cindex1]), x1, y1, x2, y2);
            }

            //画图片的前景噪音点
            for (i = 0; i < 10; i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);

                bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            //生成18条干扰线
            System.Drawing.Pen pan;
            for (i = 0; i < 5; i++)
            {
                // 实例化画笔
                pan = new Pen(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255), random.Next(255)));
                // 将实例化的画笔操作，画笔起始点和终止点以及画笔颜色随机产生
                grahics.DrawLine(pan, random.Next(90), random.Next(500), random.Next(90), random.Next(25));
            }
            Response.Clear();
            // 流载体
            System.IO.Stream stream = Response.OutputStream;

            // 将生成的图像通过流输出到页面上
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            Response.End();
        }
    }
}