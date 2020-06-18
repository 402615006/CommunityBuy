using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommunityBuy.CommonBasic
{
    public class IpPrinter
    {
        private string ip;
        //热敏打印机默认9100端口
        private int port=9100;

        public string Ip
        {
            get { return this.ip; }
            set { this.ip = Ip; }
        }
        public int Port
        {
            get { return this.port; }
            set { this.port = Port; }
        }

        private Socket PrintSocket = null;
        private IPEndPoint PrintIpPoint = null;
        private bool CanConnected = false;
        private Thread ConnectThread = null;

        public IpPrinter(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            IPAddress iPAddress = IPAddress.Parse(ip);
            PrintIpPoint = new IPEndPoint(iPAddress, port);
            PrintSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            PrintSocket.SendTimeout = 1000;
            ConnectThread = new Thread(new ThreadStart(TryConnect));
        }

        /// <summary>
        /// 连接打印机
        /// </summary>
        private void TryConnect()
        {
            try
            {
                PrintSocket.Connect(PrintIpPoint);
                CanConnected = true;
            }
            catch (Exception ex) {
                ErrorLog.WriteErrorMessage(ErrorLog.LogType.baselog, ex.ToString());
            }
        }


        /// <summary>
        /// 直接打印图片
        /// </summary>
        /// <param name="bmp">待打印的图片</param>
        /// <param name="message">输出参数，打印结果</param>
        public void PrintBitmap(Bitmap bmp, out string message)
        {
            message = "";
            ConnectThread.Start();
            Thread.Sleep(1000);
            if (!CanConnected)
            {
                message = "打印机连接失败";
                return;
            }

            int newWidth = bmp.Width;
            int newHeight = bmp.Height;
            if (newWidth % 24 > 0)
            {
                newWidth = newWidth - (newWidth % 24);
            }
            if (newHeight % 24 > 0)
            {
                newHeight = newHeight - (newHeight % 24);
            }
            bmp = (Bitmap)bmp.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
            byte[] b_inital = GetPrintCommond(CommondType.Initial);
            byte[] b_align = GetPrintCommond(CommondType.Align_Center);
            byte[] b_line0 = GetPrintCommond(CommondType.LineHeight_Zero);
            byte[] b_bitmap = GetBitMapBytes(bmp);
            byte[] b_cute = GetPrintCommond(CommondType.Cut);
            PrintSocket.Send(b_line0, b_line0.Length, 0);
            PrintSocket.Send(b_bitmap, b_bitmap.Length, 0);
            PrintSocket.Send(b_cute, b_cute.Length, 0);

            if (PrintSocket.Connected)
            {
                PrintSocket.Close();
                ConnectThread.Abort();
            }
        }

        /// <summary>
        /// 获取常用的打印命令
        /// </summary>
        /// <returns></returns>
        private byte[] GetPrintCommond(CommondType commondType)
        {
            byte[] byteList = null;
            switch (commondType)
            {
                case CommondType.Initial:
                    byteList =new byte[]{0x1b, 0x40  };
                    break;
                case CommondType.Align_Center:
                    byteList = new byte[] { 0x1b, 0x61, (byte)1};
                    break;
                case CommondType.NextLine:
                    byteList = Encoding.GetEncoding("gb18030").GetBytes("\n");
                    break;
                case CommondType.LineHeight_Zero:
                    byteList = new byte[] { 0x1B, 0x33, 0x00};
                    break;
                case CommondType.Cut:
                    byteList = new byte[] { (byte)('\n'), (byte)29, (byte)86, (byte)66, (byte)1};
                    break;
            }

            return byteList;
        }

        /// <summary>
        /// 枚举打印命令
        /// </summary>
        public enum CommondType
         {
            Initial,                      //初始化打印机
            Align_Left,              //左对齐
            Align_Right,           //右对齐
            Align_Center,        //居中对齐
            NextLine,               //换行
            LineHeight_Zero,  //设置行距 为0
            Cut                      //切纸
        }


        /// <summary>
        /// 图片转换为打印命令
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private byte[] GetBitMapBytes( Bitmap bmp)
        {
            List<byte> listBytes = new List<byte>();
            // 逐行打印
            byte bw = (byte)(bmp.Width % 256);
            byte bh = (byte)(bmp.Width / 256);

            for (int j = 0; j < bmp.Height / 24 + 1; j++)
            {
                //打印图片的指令
                listBytes.Add(0x1B);
                listBytes.Add(0x2A);
                listBytes.Add(0x21);
                listBytes.Add(bw);
                listBytes.Add(bh);
                //对于每一行，逐列打印
                for (int i = 0; i < bmp.Width; i++)
                {
                    //每一列24个像素点，分为3个字节存储
                    for (int m = 0; m < 3; m++)
                    {
                        byte bytePoints = 0;
                        //每个字节表示8个像素点，0表示白色，1表示黑色
                        for (int n = 0; n < 8; n++)
                        {
                            byte b = px2Byte(i, j * 24 + m * 8 + n, bmp);
                            bytePoints +=(byte) (bytePoints+b);
                        }
                        listBytes.Add(bytePoints);
                    }
                }
                listBytes.Add((byte)10);//换行
            }

            byte[] data = listBytes.ToArray();
            return data;
        }

        /**
         * 灰度图片黑白化，黑色是1，白色是0
         *
         * @param x   横坐标
         * @param y   纵坐标
         * @param bit 位图
         * @return
         */
        private byte px2Byte(int x, int y, Bitmap bit)
        {
            if (x < bit.Width && y < bit.Height)
            {
                byte b;
                Color pixel = bit.GetPixel(x, y);
                int gray = RGB2Gray(pixel.R, pixel.G, pixel.B);
                if (gray < 160)
                {
                    b = 1;
                }
                else
                {
                    b = 0;
                }
                return b;
            }
            return 0;
        }

        /**
         * 图片灰度的转化
         */
        private int RGB2Gray(int r, int g, int b)
        {
            int gray = (int)(0.29900 * r + 0.58700 * g + 0.11400 * b);  //灰度转化公式
            return gray;
        }
    }
}
