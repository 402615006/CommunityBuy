using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XJWZCatering.CommonBasic
{
    public class PrintService
    {
        static byte[] b_init = PrinterCmdUtils.init_printer();
        static byte[] b_nextLine = PrinterCmdUtils.nextLine(1);
        static byte[] b_next2Line = PrinterCmdUtils.nextLine(2);
        static byte[] b_next3Line = PrinterCmdUtils.nextLine(3);
        static byte[] b_center = PrinterCmdUtils.alignCenter();
        static byte[] b_left = PrinterCmdUtils.alignLeft();
        static byte[] b_breakPartial = PrinterCmdUtils.feedPaperCutPartial();

        /// <summary>
        /// 会员充值小票
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="num"></param>
        /// <param name="count"></param>
        /// <param name="way"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static byte[] charge(String shop, String num, String count, String way, String time)
        {
            String shop_f = "   会员充值（" + shop + "）";
            String num_f = "   会员号码：" + num;
            String count_f = "   充值金额：" + count;
            String way_f = "   充值方式：" + way;
            String time_f = "   充值时间：" + time;

            try
            {
                byte[] b_title = System.Text.Encoding.GetEncoding("gbk").GetBytes(shop_f);
                byte[] b_num = System.Text.Encoding.GetEncoding("gbk").GetBytes(num_f);
                byte[] b_count = System.Text.Encoding.GetEncoding("gbk").GetBytes(count_f);
                byte[] b_way = System.Text.Encoding.GetEncoding("gbk").GetBytes(way_f);
                byte[] b_time = System.Text.Encoding.GetEncoding("gbk").GetBytes(time_f);
                byte[] b_no = System.Text.Encoding.GetEncoding("gbk").GetBytes("   ----------------------------------------");
                byte[] b_breakPartial = PrinterCmdUtils.feedPaperCutPartial();

                byte[][] charge_0 = { 
                    b_init, 
                    b_center, b_title,  b_next2Line, 
                    b_left,   b_count,  b_nextLine, 
                    b_way,    b_nextLine, 
                    b_time,   b_nextLine, 
                    b_no,     b_nextLine,
                    b_breakPartial,
                    PrinterCmdUtils.open_money()
                    };

                byte[][] charge_1 = { 
                    b_init, 
                    b_center, b_title,  b_next2Line, 
                    b_left,   b_num,    b_nextLine, 
                    b_count,  b_nextLine, 
                    b_way,    b_nextLine, 
                    b_time,   b_nextLine, 
                    b_no,     b_nextLine,
                    b_breakPartial,
                    PrinterCmdUtils.open_money() 
                    };

                if (String.IsNullOrEmpty(num))
                {
                    return PrinterCmdUtils.byteMerger(charge_0);
                }
                else
                {
                    return PrinterCmdUtils.byteMerger(charge_1);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 收银小票  Json对象拼接
        /// </summary>
        /// <param name="head"></param>
        /// <param name="recv"></param>
        /// <param name="foot"></param>
        /// <returns></returns>
        public static byte[] jsonParse(String head, String recv, String foot, String url)
        {
            try
            {
                //二维码生成
                EscCommand esc = new EscCommand();
                esc.addSelectErrorCorrectionLevelForQRCode((byte)0x31);
                esc.addSelectSizeOfModuleForQRCode((byte)8);
                esc.addStoreQRCodeData(url);
                esc.addPrintQRCode();

                ArrayList datas = esc.getCommand();
                Byte[] Bytes = new Byte[datas.Count];
                for (int i = 0; i < datas.Count; i++)
                {
                    Bytes[i] = (Byte)datas[i];
                }

                byte[] bytes = EscCommand.toPrimitive(Bytes);

                //打印小票头部
                List<String> headString = new List<String>();
                List<TicketHead> mHeads = JsonConvert.DeserializeObject<List<TicketHead>>(head);

                headString.Add("  " + mHeads[0].Ordertime);
                headString.Add("  NO." + mHeads[0].Shopno);
                headString.Add(FileUtils.getFixedWidthString("  当班人员：" + mHeads[0].Shopuser, 36) + "餐牌号:" + mHeads[0].Card);
                headString.Add("  ---------------------------------------------");

                List<String> newString = new List<String>();
                List<Ticket> mTickets = JsonConvert.DeserializeObject<List<Ticket>>(recv);

                String nameTitle = "  名称              数量 售价    优惠    总额";
                newString.Add(nameTitle);

                for (int i = 0; i < mTickets.Count; i++)
                {
                    StringBuilder item = new StringBuilder();

                    if (("1".Equals(mTickets[i].Type) || "2".Equals(mTickets[i].Type)))
                    {
                        if ("".Equals(mTickets[i].Parentid))
                        {
                            if (mTickets[i].Name.Length > 8)
                            {
                                item.Append(FileUtils.getFixedWidthString(mTickets[i].Name.Substring(0, 8), 19));
                                item.Append(mTickets[i].Num.PadRight(4));
                                item.Append(String.Format("{0:F2}", mTickets[i].Price).PadRight(8));
                                item.Append(String.Format("{0:F2}", mTickets[i].Rebetmoney).PadRight(8));
                                item.Append(String.Format("{0:F2}", mTickets[i].Money).PadRight(6));

                                newString.Add("  " + item.ToString());
                                newString.Add("  " + mTickets[i].Name.Substring(8, mTickets[i].Name.Length - 8));
                            }
                            else
                            {
                                item.Append(FileUtils.getFixedWidthString(mTickets[i].Name, 19));
                                item.Append(mTickets[i].Num.PadRight(4));
                                item.Append(String.Format("{0:F2}", mTickets[i].Price).PadRight(8));
                                item.Append(String.Format("{0:F2}", mTickets[i].Rebetmoney).PadRight(8));
                                item.Append(String.Format("{0:F2}", mTickets[i].Money).PadRight(6));

                                newString.Add("  " + item.ToString());
                            }
                        }
                        else
                        {
                            newString.Add("  " + mTickets[i].Name);
                        }
                    }
                }

                newString.Add("  ---------------------------------------------");

                List<String> footString = new List<String>();
                List<TicketFoot> mFoots = JsonConvert.DeserializeObject<List<TicketFoot>>(foot);

                String moneytotal = ((float.Parse(mFoots[0].DecAmmBalance) + float.Parse(mFoots[0].DecGroupBuyMoney))).ToString("f2");
                String money = ((float.Parse(mFoots[0].DecRebeatBalance) + float.Parse(mFoots[0].DecGroupBuyMoney))).ToString("f2");

                footString.Add(FileUtils.getFixedWidthString("  餐品数量:", 20) + FileUtils.getFixedWidthString(mFoots[0].Counts, 10) + "点餐金额: " + moneytotal.PadRight(4));

                if (float.Parse(mFoots[0].DecWipeMoney) != 0.0f)
                    footString.Add(FileUtils.getFixedWidthString("  抹零优惠:", 40) + mFoots[0].DecWipeMoney);

                if (float.Parse(mFoots[0].DecMemberCoupon) != 0.0f)
                    footString.Add(FileUtils.getFixedWidthString("  会员优惠:", 40) + mFoots[0].DecMemberCoupon);

                if (float.Parse(money) != 0.0f)
                {
                    footString.Add(FileUtils.getFixedWidthString("  券类优惠:", 40) + money.PadRight(4));

                    for (int i = 0; i < mTickets.Count; i++)
                    {
                        if (!"1".Equals(mTickets[i].Type) && !"2".Equals(mTickets[i].Type))
                        {
                            footString.Add(FileUtils.getFixedWidthString("    " + mTickets[i].Name, 20) + mTickets[i].Num);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(mTickets[i].RebetName))
                                footString.Add(FileUtils.getFixedWidthString("    " + mTickets[i].RebetName, 20) + 1);
                        }
                    }
                }

                footString.Add("  ---------------------------------------------");
                if (float.Parse(mFoots[0].DecPaidBalance) != 0.0f)
                    footString.Add(FileUtils.getFixedWidthString("  折后支付:", 40) + mFoots[0].DecPaidBalance);

                if (float.Parse(mFoots[0].Membermoney) != 0.0f)
                    footString.Add(FileUtils.getFixedWidthString("  会员支付:", 40) + mFoots[0].Membermoney);

                if (float.Parse(mFoots[0].Cashmoney) != 0.0f)
                    footString.Add(FileUtils.getFixedWidthString("  现金支付:", 40) + mFoots[0].Cashmoney);

                if (!string.IsNullOrEmpty(mFoots[0].Bankmoney))
                    footString.Add(FileUtils.getFixedWidthString("  " + mFoots[0].Bankmoney.Split(':')[0], 40) + mFoots[0].Bankmoney.Split(':')[1]);

                if (float.Parse(mFoots[0].Givechange) != 0.0f)
                    footString.Add(FileUtils.getFixedWidthString("  现金找零:", 40) + mFoots[0].Givechange);

                if (float.Parse(mFoots[0].RechMoney) != 0.0f)
                    footString.Add(FileUtils.getFixedWidthString("  充值金额:", 40) + mFoots[0].RechMoney);

                /////////////////////////////////////////////////////////////////////////////////////////


                byte[][] headByte = new byte[15][];                                    //头部数组
                headByte[0] = b_init;
                headByte[1] = b_center;
                headByte[2] = System.Text.Encoding.GetEncoding("gbk").GetBytes("欢迎光临");
                headByte[3] = b_nextLine;
                headByte[4] = System.Text.Encoding.GetEncoding("gbk").GetBytes(mHeads[0].Shopbrand);
                headByte[5] = b_next2Line;
                headByte[6] = b_left;
                for (int i = 0; i < 8; i += 2)
                {
                    headByte[i + 7] = System.Text.Encoding.GetEncoding("gbk").GetBytes(headString[i / 2]);
                    headByte[i + 8] = b_nextLine;
                }

                byte[][] contentByte = new byte[newString.Count * 2][];            //中间体
                for (int i = 0; i < newString.Count * 2; i += 2)
                {
                    contentByte[i] = System.Text.Encoding.GetEncoding("gbk").GetBytes(newString[i / 2]);
                    contentByte[i + 1] = b_nextLine;
                }

                byte[][] footByte = new byte[footString.Count * 2 + 23][];            //底部数组
                for (int i = 0; i < footString.Count * 2; i += 2)
                {
                    footByte[i] = System.Text.Encoding.GetEncoding("gbk").GetBytes(footString[i / 2]);
                    footByte[i + 1] = b_nextLine;
                }

                footByte[footString.Count * 2] = System.Text.Encoding.GetEncoding("gbk").GetBytes("  ---------------------------------------------");
                if (string.IsNullOrEmpty(mHeads[0].Memberid))
                {
                    footByte[footString.Count * 2 + 1] = b_center;
                    footByte[footString.Count * 2 + 2] = b_center;
                }
                else
                {
                    footByte[footString.Count * 2 + 1] = b_nextLine;
                    footByte[footString.Count * 2 + 2] = System.Text.Encoding.GetEncoding("gbk").GetBytes("  会员号：" + mHeads[0].Memberid);
                }

                footByte[footString.Count * 2 + 3] = b_next2Line;
                footByte[footString.Count * 2 + 4] = b_center;
                footByte[footString.Count * 2 + 5] = System.Text.Encoding.GetEncoding("gbk").GetBytes("谢谢惠顾！欢迎再次光临！");
                footByte[footString.Count * 2 + 6] = b_nextLine;
                footByte[footString.Count * 2 + 7] = System.Text.Encoding.GetEncoding("gbk").GetBytes("如需开发票，请凭小票在一周内到门店开取");
                footByte[footString.Count * 2 + 8] = b_next2Line;

                footByte[footString.Count * 2 + 9] = b_left;
                footByte[footString.Count * 2 + 10] = System.Text.Encoding.GetEncoding("gbk").GetBytes("  门店地址：" + mHeads[0].Shopaddress);
                footByte[footString.Count * 2 + 11] = b_nextLine;
                footByte[footString.Count * 2 + 12] = System.Text.Encoding.GetEncoding("gbk").GetBytes("  联系电话：" + mHeads[0].Shoptel);
                footByte[footString.Count * 2 + 13] = b_nextLine;
                footByte[footString.Count * 2 + 14] = System.Text.Encoding.GetEncoding("gbk").GetBytes("  品牌网址：" + mHeads[0].Url);
                footByte[footString.Count * 2 + 15] = b_next2Line;
                footByte[footString.Count * 2 + 16] = b_center;

                footByte[footString.Count * 2 + 17] = b_next2Line;
                footByte[footString.Count * 2 + 18] = System.Text.Encoding.GetEncoding("gbk").GetBytes("微信扫一扫    畅享免费WiFi");
                footByte[footString.Count * 2 + 19] = b_next2Line;

                footByte[footString.Count * 2 + 20] = bytes;
                footByte[footString.Count * 2 + 21] = b_breakPartial;
                footByte[footString.Count * 2 + 22] = PrinterCmdUtils.open_money();

                byte[][] totalByte = new byte[38 + newString.Count * 2 + footString.Count * 2][];

                Array.Copy(headByte, totalByte, headByte.Length);
                Array.Copy(contentByte, 0, totalByte, headByte.Length, contentByte.Length);
                Array.Copy(footByte, 0, totalByte, headByte.Length + contentByte.Length, footByte.Length);

                return PrinterCmdUtils.byteMerger(totalByte);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class PrinterCmdUtils
    {

        public const byte ESC = 27;    // 换码
        public const byte FS = 28;    // 文本分隔符
        public const byte GS = 29;    // 组分隔符
        public const byte DLE = 16;    // 数据连接换码
        public const byte EOT = 4;    // 传输结束
        public const byte ENQ = 5;    // 询问字符
        public const byte SP = 32;    // 空格
        public const byte HT = 9;    // 横向列表
        public const byte LF = 10;    // 打印并换行（水平定位）
        public const byte CR = 13;    // 归位键
        public const byte FF = 12;    // 走纸控制（打印并回到标准模式（在页模式下） ）
        public const byte CAN = 24;    // 作废（页模式下取消打印数据 ）

        /**
         * 打印纸一行最大的字节
         */
        private const int LINE_BYTE_SIZE = 32;
        /**
         * 分隔符
         */
        private const String SEPARATOR = "$";
        private static StringBuilder sb = new StringBuilder();

        /**
         * 打印机初始化
         * 
         * @return
         */
        public static byte[] init_printer()
        {
            byte[] result = new byte[2];
            result[0] = ESC;
            result[1] = 64;
            return result;
        }

        /**
         * 打开钱箱
         * 
         * @return
         */
        public static byte[] open_money()
        {
            byte[] result = new byte[5];
            result[0] = ESC;
            result[1] = 112;
            result[2] = 48;
            result[3] = 64;
            result[4] = 0;
            return result;
        }

        /**
         * 换行
         * 
         * @param lineNum要换几行
         * @return
         */
        public static byte[] nextLine(int lineNum)
        {
            byte[] result = new byte[lineNum];
            for (int i = 0; i < lineNum; i++)
            {
                result[i] = LF;
            }

            return result;
        }


        /**
         * 绘制下划线（1点宽）
         * 
         * @return
         */
        public static byte[] underlineWithOneDotWidthOn()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 45;
            result[2] = 1;
            return result;
        }

        /**
         * 绘制下划线（2点宽）
         * 
         * @return
         */
        public static byte[] underlineWithTwoDotWidthOn()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 45;
            result[2] = 2;
            return result;
        }

        /**
         * 取消绘制下划线
         * 
         * @return
         */
        public static byte[] underlineOff()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 45;
            result[2] = 0;
            return result;
        }


        /**
         * 选择加粗模式
         * 
         * @return
         */
        public static byte[] boldOn()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 69;
            result[2] = 0xF;
            return result;
        }

        /**
         * 取消加粗模式
         * 
         * @return
         */
        public static byte[] boldOff()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 69;
            result[2] = 0;
            return result;
        }


        /**
         * 左对齐
         * 
         * @return
         */
        public static byte[] alignLeft()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 97;
            result[2] = 0;
            return result;
        }

        /**
         * 居中对齐
         * 
         * @return
         */
        public static byte[] alignCenter()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 97;
            result[2] = 1;
            return result;
        }

        /**
         * 右对齐
         * 
         * @return
         */
        public static byte[] alignRight()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 97;
            result[2] = 2;
            return result;
        }

        /**
         * 水平方向向右移动col列
         * 
         * @param col
         * @return
         */
        public static byte[] set_HT_position(byte col)
        {
            byte[] result = new byte[4];
            result[0] = ESC;
            result[1] = 68;
            result[2] = col;
            result[3] = 0;
            return result;
        }

        /**
         * 字体变大为标准的n倍
         * 
         * @param num
         * @return
         */
        public static byte[] fontSizeSetBig(int num)
        {
            byte realSize = 0;
            switch (num)
            {
                case 1:
                    realSize = 0;
                    break;
                case 2:
                    realSize = 17;
                    break;
                case 3:
                    realSize = 34;
                    break;
                case 4:
                    realSize = 51;
                    break;
                case 5:
                    realSize = 68;
                    break;
                case 6:
                    realSize = 85;
                    break;
                case 7:
                    realSize = 102;
                    break;
                case 8:
                    realSize = 119;
                    break;
            }
            byte[] result = new byte[3];
            result[0] = 29;
            result[1] = 33;
            result[2] = realSize;
            return result;
        }


        /**
         * 字体取消倍宽倍高
         * 
         * @return
         */
        public static byte[] fontSizeSetSmall()
        {
            byte[] result = new byte[3];
            result[0] = ESC;
            result[1] = 33;

            return result;
        }

        /**
         * 进纸并全部切割
         * 
         * @return
         */
        public static byte[] feedPaperCutAll()
        {
            byte[] result = new byte[4];
            result[0] = GS;
            result[1] = 86;
            result[2] = 65;
            result[3] = 0;
            return result;
        }

        /**
         * 进纸并切割（左边留一点不切）
         * 
         * @return
         */
        public static byte[] feedPaperCutPartial()
        {
            byte[] result = new byte[4];
            result[0] = GS;
            result[1] = 86;
            result[2] = 66;
            result[3] = 0;
            return result;
        }

        public static byte[] bmpToByte(Bitmap bmp)
        {
            int h = bmp.Height / 24 + 1;
            int w = bmp.Width;
            byte[][] all = new byte[4 + 2 * h + h * w][];

            all[0] = new byte[] { 0x1B, 0x33, 0x00 };

            Color pixelColor;
            // ESC * m nL nH 点阵图  
            byte[] escBmp = new byte[] { 0x1B, 0x2A, 0x21, (byte)(w % 256), (byte)(w / 256) };

            // 每行进行打印  
            for (int i = 0; i < h; i++)
            {
                all[i * (w + 2) + 1] = escBmp;
                for (int j = 0; j < w; j++)
                {
                    byte[] data = new byte[] { 0x00, 0x00, 0x00 };
                    for (int k = 0; k < 24; k++)
                    {
                        if (((i * 24) + k) < bmp.Height)
                        {
                            pixelColor = bmp.GetPixel(j, (i * 24) + k);
                            if (pixelColor.R == 0)
                            {
                                data[k / 8] += (byte)(128 >> (k % 8));
                            }
                        }
                    }
                    all[i * (w + 2) + j + 2] = data;
                }
                //换行  
                all[(i + 1) * (w + 2)] = PrinterCmdUtils.nextLine(1);
            }
            all[h * (w + 2) + 1] = PrinterCmdUtils.nextLine(2);
            all[h * (w + 2) + 2] = PrinterCmdUtils.feedPaperCutAll();
            all[h * (w + 2) + 3] = PrinterCmdUtils.open_money();

            return PrinterCmdUtils.byteMerger(all);
        }

        // ------------------------切纸-----------------------------
        public static byte[] byteMerger(byte[] byte_1, byte[] byte_2)
        {
            byte[] byte_3 = new byte[byte_1.Length + byte_2.Length];
            System.Array.Copy(byte_1, 0, byte_3, 0, byte_1.Length);
            System.Array.Copy(byte_2, 0, byte_3, byte_1.Length, byte_2.Length);
            return byte_3;
        }

        public static byte[] byteMerger(byte[][] byteList)
        {
            int Length = 0;
            for (int i = 0; i < byteList.Length; i++)
            {
                Length += byteList[i].Length;
            }
            byte[] result = new byte[Length];

            int index = 0;
            for (int i = 0; i < byteList.Length; i++)
            {
                byte[] nowByte = byteList[i];
                for (int k = 0; k < byteList[i].Length; k++)
                {
                    result[index] = nowByte[k];
                    index++;
                }
            }
            return result;
        }

        public static byte[][] byte20Merger(byte[] bytes)
        {
            int size = bytes.Length / 10 + 1;
            byte[][] result = new byte[size][];
            for (int i = 0; i < size; i++)
            {
                byte[] by = new byte[((i + 1) * 10) - (i * 10)];
                //从bytes中的第 i * 10 个位置到第 (i + 1) * 10 个位置;
                System.Array.Copy(bytes, i * 10, by, 0, (i + 1) * 10);
                result[i] = by;
            }
            return result;
        }
    }
    public class TicketFoot
    {
        private String decAmmBalance;        //点餐总额

        public String DecAmmBalance
        {
            get { return decAmmBalance; }
            set { decAmmBalance = value; }
        }
        private String decRebeatBalance;    //券内优惠

        public String DecRebeatBalance
        {
            get { return decRebeatBalance; }
            set { decRebeatBalance = value; }
        }
        private String decGroupBuyMoney;    //+

        public String DecGroupBuyMoney
        {
            get { return decGroupBuyMoney; }
            set { decGroupBuyMoney = value; }
        }
        private String decPaidBalance;        //折后支付

        public String DecPaidBalance
        {
            get { return decPaidBalance; }
            set { decPaidBalance = value; }
        }
        private String decMemberCoupon;        //会员优惠

        public String DecMemberCoupon
        {
            get { return decMemberCoupon; }
            set { decMemberCoupon = value; }
        }
        private String decWipeMoney;        //抹零优惠

        public String DecWipeMoney
        {
            get { return decWipeMoney; }
            set { decWipeMoney = value; }
        }
        private String wipe;                //

        public String Wipe
        {
            get { return wipe; }
            set { wipe = value; }
        }
        private String cashmoney;            //现金支付

        public String Cashmoney
        {
            get { return cashmoney; }
            set { cashmoney = value; }
        }
        private String bankmoney;            //银行卡支付

        public String Bankmoney
        {
            get { return bankmoney; }
            set { bankmoney = value; }
        }
        private String membermoney;            //会员支付

        public String Membermoney
        {
            get { return membermoney; }
            set { membermoney = value; }
        }
        private String counts;                //餐品数量

        public String Counts
        {
            get { return counts; }
            set { counts = value; }
        }

        private String givechange;            //找零

        public String Givechange
        {
            get { return givechange; }
            set { givechange = value; }
        }

        private string rechMoney;
        /// <summary>
        /// 充值金额
        /// </summary>
        public string RechMoney
        {
            get { return rechMoney; }
            set { rechMoney = value; }
        }
    }

    public class Ticket
    {
        private String id;

        public String Id
        {
            get { return id; }
            set { id = value; }
        }
        private String type;

        public String Type
        {
            get { return type; }
            set { type = value; }
        }
        private String parentid;

        public String Parentid
        {
            get { return parentid; }
            set { parentid = value; }
        }
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private String num;

        public String Num
        {
            get { return num; }
            set { num = value; }
        }
        private String price;

        public String Price
        {
            get { return price; }
            set { price = value; }
        }
        private String money;

        public String Money
        {
            get { return money; }
            set { money = value; }
        }
        private String rebetmoney;

        public String Rebetmoney
        {
            get { return rebetmoney; }
            set { rebetmoney = value; }
        }
        private String rebetName;

        public String RebetName
        {
            get { return rebetName; }
            set { rebetName = value; }
        }
        private String rebetid;

        public String Rebetid
        {
            get { return rebetid; }
            set { rebetid = value; }
        }
        private String rebetType;

        public String RebetType
        {
            get { return rebetType; }
            set { rebetType = value; }
        }
        private String grouptype;

        public String Grouptype
        {
            get { return grouptype; }
            set { grouptype = value; }
        }
        private String groupid;

        public String Groupid
        {
            get { return groupid; }
            set { groupid = value; }
        }
        private String serialnumber;

        public String Serialnumber
        {
            get { return serialnumber; }
            set { serialnumber = value; }
        }

        private String addonname;

        public String AddOnName
        {
            get { return addonname; }
            set { addonname = value; }
        }

    }

    public class TicketHead
    {
        private String memberid;        //会员号

        public String Memberid
        {
            get { return memberid; }
            set { memberid = value; }
        }
        private String card;            //餐牌号

        public String Card
        {
            get { return card; }
            set { card = value; }
        }
        private String shopname;        //店名

        public String Shopname
        {
            get { return shopname; }
            set { shopname = value; }
        }
        private String shopaddress;        //店地址

        public String Shopaddress
        {
            get { return shopaddress; }
            set { shopaddress = value; }
        }
        private String shoptel;            //店电话

        public String Shoptel
        {
            get { return shoptel; }
            set { shoptel = value; }
        }
        private String shopno;            //店编号

        public String Shopno
        {
            get { return shopno; }
            set { shopno = value; }
        }
        private String shopuser;        //当班人员

        public String Shopuser
        {
            get { return shopuser; }
            set { shopuser = value; }
        }
        private String ordertime;        //点单时间

        public String Ordertime
        {
            get { return ordertime; }
            set { ordertime = value; }
        }
        private String url;                //网址

        public String Url
        {
            get { return url; }
            set { url = value; }
        }

        private string shopbrand;

        public string Shopbrand
        {
            get { return shopbrand; }
            set { shopbrand = value; }
        }
    }

    public class EscCommand
    {
        ArrayList Command = null;

        public EscCommand()
        {
            this.Command = new ArrayList();
        }

        public ArrayList getCommand()
        {
            return this.Command;
        }

        private void addArrayToCommand(byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                this.Command.Add(array[i]);
            }
        }

        public void addSelectErrorCorrectionLevelForQRCode(byte n)
        {
            byte[] command = { 29, 40, 107, 3, 0, 49, 69, 3 };
            command[7] = n;
            addArrayToCommand(command);
        }

        public void addSelectSizeOfModuleForQRCode(byte n)
        {
            byte[] command = { 29, 40, 107, 3, 0, 49, 67, 3 };
            command[7] = n;
            addArrayToCommand(command);
        }

        public void addStoreQRCodeData(String content)
        {
            byte[] command = { 29, 40, 107, 0, 0, 49, 80, 48 };
            command[3] = ((byte)((content.Length + 3) % 256));
            command[4] = ((byte)((content.Length + 3) / 256));
            addArrayToCommand(command);
            addStrToCommand(content, content.Length);
        }

        private void addStrToCommand(String str, int length)
        {
            byte[] bs = null;
            if (!str.Equals(""))
            {
                try
                {
                    bs = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (length > bs.Length)
                {
                    length = bs.Length;
                }
                for (int i = 0; i < length; i++)
                {
                    this.Command.Add(bs[i]);
                }
            }
        }

        public void addPrintQRCode()
        {
            byte[] command = { 29, 40, 107, 3, 0, 49, 81, 48 };
            addArrayToCommand(command);
        }

        public static byte[] toPrimitive(Byte[] array)
        {
            if (array == null)
            {
                return null;
            }
            if (array.Length == 0)
            {
                return new byte[] { };
            }
            byte[] result = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }
            return result;
        }

    }

    public class PrintHelper
    {
        public void SendSocketMsg(String ip, int port, int times, byte[] data)
        {
            try
            {
                byte[] mData;
                if (times == 1)
                {
                    mData = new byte[data.Length];
                    Array.Copy(data, 0, mData, 0, data.Length);
                }
                else
                {
                    mData = new byte[data.Length * times];
                    byte[][] m = new byte[times][];
                    for (int i = 0; i < times; i++)
                    {
                        m[i] = data;
                    }
                    Array.Copy(PrinterCmdUtils.byteMerger(m), 0, mData, 0, PrinterCmdUtils.byteMerger(m).Length);
                }

                #region 同步 Socket
                Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

                mySocket.Connect(ipEndPoint);
                //mySocket.Send(data);
                mySocket.Send(mData);
                mySocket.Close();
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
                Logger.Trace(ex.StackTrace);
            }
        }
    }
}
