using System.Collections.Generic;
using System.Data;
using System.Text;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
    /// <summary>
    /// 会员卡订单表业务类
    /// </summary>
    public class bllmemcardorders : bllBase
    {
        DAL.dalmemcardorders dal = new DAL.dalmemcardorders();
        memcardordersEntity Entity = new memcardordersEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string ID, string buscode, string stocode, string memcode, string cardcode, string otype, string regamount, string freeamount, string cardcost, string payamount, string remark, string status, string ucode, string uname, string ordercode, string paystatus, out string spanids)
        {
            string strRetuen = string.Empty;
            spanids = string.Empty;
            //要验证的实体属性
            List<string> EName = new List<string>() { };
            //要验证的实体属性值
            List<string> EValue = new List<string>() { };
            //错误信息
            List<string> errorCode = new List<string>();
            List<string> ControlName = new List<string>();
            //验证数据
            CheckValue<memcardordersEntity>(EName, EValue, ref errorCode, new memcardordersEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
                spanids = ListTostring(ControlName);
            }
            else//组合对象数据
            {
                Entity = new memcardordersEntity();
                Entity.ID = Helper.StringToLong(ID);
                Entity.buscode = buscode;
                Entity.stocode = stocode;
                Entity.memcode = memcode;
                Entity.cardcode = cardcode;
                Entity.otype = otype;
                Entity.regamount = Helper.StringToDecimal(regamount);
                Entity.freeamount = Helper.StringToDecimal(freeamount);
                Entity.cardcost = Helper.StringToDecimal(cardcost);
                Entity.payamount = Helper.StringToDecimal(payamount);
                Entity.remark = remark;
                Entity.status = status;
                Entity.ucode = ucode;
                Entity.uname = uname;

                Entity.ordercode = ordercode;
                Entity.paystatus = paystatus;
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID, out  string ID, string buscode, string stocode, string memcode, string cardtype, string cardcode, string otype, string regamount, string freeamount, string cardcost, string payamount, string remark, string status, string ucode, string uname, ref string ordercode, string paystatus, string pushemcode, string pushname, string shiftid, string pcname, string bak2, string bak3, opencardinfoEntity opencardinfo, List<opencardcouponEntity> opencardcoupon)
        {
            ID = "0";
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();

            StringBuilder sb = new StringBuilder();
            //添加会员卡开卡订单数据
            sb.AppendLine(" declare @ordercode VARCHAR(32);");
            sb.AppendLine(" declare @id int;");

            sb.AppendLine("begin tran tran1");
            sb.AppendFormat(" exec @id=[dbo].[p_memcardorders_Add] @ordercode=@ordercode output,@buscode='{0}',@stocode='{1}',@memcode='{2}',@cardcode='{3}'", buscode, stocode, memcode, cardcode);
            sb.AppendFormat(" ,@otype='{0}',@regamount='{1}',@freeamount='{2}',@cardcost='{3}',@payamount='0'", otype
                , (string.IsNullOrEmpty(regamount) || regamount.Trim() == "" ? "0" : regamount)
                , (string.IsNullOrEmpty(freeamount) || freeamount.Trim() == "" ? "0" : freeamount)
                , (string.IsNullOrEmpty(cardcost) || cardcost.Trim() == "" ? "0" : cardcost)
                );
            sb.AppendFormat(" ,@remark='{0}',@status='{1}',@ucode='{2}',@uname='{3}',@pushemcode='{4}',@pushname='{5}',@ctype='{6}',@shiftid='{7}',@pcname='{8}'", remark, status, ucode, uname, pushemcode, pushname, cardtype, shiftid, pcname);
            sb.AppendFormat(" ,@bak2='{0}',@bak3='{1}',@mobile='{2}',@memcname='{3}',@idtype='{4}',@idno='{5}'", bak2, bak3, opencardinfo == null ? "" : opencardinfo.mobile, opencardinfo == null ? "" : opencardinfo.cname, opencardinfo == null ? "" : opencardinfo.idtype, opencardinfo == null ? "" : opencardinfo.IDNO);

            //添加临时开卡信息数据
            if (opencardinfo != null)
            {
                sb.AppendLine(" if(@id=2) begin select @id,'';end else begin ");//等于2，卡号已经激活
                sb.AppendFormat(" exec [dbo].[p_opencardinfo_Add] @ordercode=@ordercode,@buscode='{0}',@stocode='{1}',@memcode='{2}',@cardcode='{3}'", buscode, stocode, memcode, cardcode);
                sb.AppendFormat(" ,@regamount='{0}',@freeamount='{1}',@cardcost='{2}',@payamount='0'"
                    , (string.IsNullOrEmpty(regamount) || regamount.Trim() == "" ? "0" : regamount)
                    , (string.IsNullOrEmpty(freeamount) || freeamount.Trim() == "" ? "0" : freeamount)
                    , (string.IsNullOrEmpty(cardcost) || cardcost.Trim() == "" ? "0" : cardcost)
                    );
                sb.AppendFormat(" ,@validate='{0}',@password='{1}',@nowritedoc='{2}',@cname='{3}'", opencardinfo.validate.ToString("yyyy-MM-dd"), opencardinfo.password, opencardinfo.nowritedoc, opencardinfo.cname);
                sb.AppendFormat(" ,@mobile='{0}',@idtype='{1}',@IDNO='{2}',@ucode='{3}',@uname='{4}'", opencardinfo.mobile, opencardinfo.idtype, opencardinfo.IDNO, ucode, uname);
                sb.AppendFormat(" ,@sex='{0}',@bak1='{1}',@bak2='{2}',@bak3='{3}'", opencardinfo.sex, opencardinfo.bak1, opencardinfo.bak2, opencardinfo.bak3);
            }
            //添加开卡优惠券赠送数据
            if (opencardcoupon != null)
            {
                sb.AppendLine(" DELETE FROM [dbo].[opencardcoupon] WHERE cardcode='" + cardcode + "';");
                for (int i = 0; i < opencardcoupon.Count; i++)
                {
                    sb.AppendFormat(" exec[dbo].[p_opencardcoupon_Add] @ordercode=@ordercode,@buscode='{0}',@stocode='{1}',@memcode='{2}',@cardcode='{3}'", buscode, stocode, memcode, cardcode);
                    sb.AppendFormat(" ,@pcode='{0}',@sumcode='{1}',@mccode='{2}',@num='{3}'", opencardcoupon[i].pcode, opencardcoupon[i].sumcode, opencardcoupon[i].mccode, opencardcoupon[i].num);
                }
            }
            if (opencardinfo != null)
            {
                sb.AppendLine(" end");
            }

            sb.AppendLine("if(@@error=0)begin commit tran tran1;select '0',@ordercode; end");
            sb.AppendLine("else begin rollback tran tran1;select '-1',''; end");

            string mescode = "Err_004";
            int intresult = -1;
            DataTable result = new bllPaging().GetDataTableInfoBySQL(sb.ToString());
            if (result != null && result.Rows.Count > 0)
            {

                intresult = Helper.StringToInt(result.Rows[0][0].ToString());
                switch (intresult)
                {
                    case 0:
                        ordercode = result.Rows[0][1].ToString();
                        break;
                    case -1:
                        mescode = "Err_004";
                        break;
                    case 2:
                        mescode = "memcardadd_notactive";
                        break;
                }

            }
            CheckDeleteResult(intresult, mescode);
            return dtBase;
        }

        /// <summary>
        /// 会员卡开卡
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="ID"></param>
        /// <param name="buscode"></param>
        /// <param name="stocode"></param>
        /// <param name="memcode"></param>
        /// <param name="cardtype"></param>
        /// <param name="cardcode"></param>
        /// <param name="otype"></param>
        /// <param name="regamount"></param>
        /// <param name="freeamount"></param>
        /// <param name="cardcost"></param>
        /// <param name="payamount"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        /// <param name="ucode"></param>
        /// <param name="uname"></param>
        /// <param name="ordercode"></param>
        /// <param name="paystatus"></param>
        /// <param name="pushemcode"></param>
        /// <param name="pushname"></param>
        /// <param name="shiftid"></param>
        /// <param name="pcname"></param>
        /// <param name="bak2"></param>
        /// <param name="bak3"></param>
        /// <param name="opencardinfo"></param>
        /// <param name="opencardcoupon"></param>
        /// <returns></returns>
        public DataTable AddmemCard(string GUID, string UID, out  string ID, string buscode, string stocode, string memcode, string cardtype, string levelcode, string otype, string regamount, string freeamount, string cardcost, string payamount, string remark, string status, string ucode, string uname, ref string ordercode, string paystatus, string pushemcode, string pushname, string shiftid, string pcname, string bak2, string bak3, opencardinfoEntity opencardinfo, List<opencardcouponEntity> opencardcoupon)
        {
            ID = "0";
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();

            StringBuilder sb = new StringBuilder();
            //添加会员卡开卡订单数据
            sb.AppendLine(" declare @ordercode VARCHAR(32);");
            sb.AppendLine(" declare @id int;");

            sb.AppendLine("begin tran tran1");
            sb.AppendFormat(" exec @id=[dbo].[p_memlevelcardorders_Add] @ordercode=@ordercode output,@buscode='{0}',@stocode='{1}',@memcode='{2}',@levelcode='{3}'", buscode, stocode, memcode, levelcode);
            sb.AppendFormat(" ,@otype='{0}',@regamount='{1}',@freeamount='{2}',@cardcost='{3}',@payamount='0'", otype
                , (string.IsNullOrEmpty(regamount) || regamount.Trim() == "" ? "0" : regamount)
                , (string.IsNullOrEmpty(freeamount) || freeamount.Trim() == "" ? "0" : freeamount)
                , (string.IsNullOrEmpty(cardcost) || cardcost.Trim() == "" ? "0" : cardcost)
                );
            sb.AppendFormat(" ,@remark='{0}',@status='{1}',@ucode='{2}',@uname='{3}',@pushemcode='{4}',@pushname='{5}',@ctype='{6}',@shiftid='{7}',@pcname='{8}'", remark, status, ucode, uname, pushemcode, pushname, cardtype, shiftid, pcname);
            sb.AppendFormat(" ,@bak2='{0}',@bak3='{1}',@mobile='{2}',@memcname='{3}',@idtype='{4}',@idno='{5}' ", bak2, bak3, opencardinfo.mobile, opencardinfo.cname, opencardinfo.idtype, opencardinfo.IDNO);

            //添加临时开卡信息数据
            if (opencardinfo != null)
            {
                sb.AppendLine(" if(@id=2) begin select @id,'';end else begin ");//等于2，卡号已经激活
                sb.AppendFormat(" exec [dbo].[p_openlevelcardinfo_Add] @ordercode=@ordercode,@buscode='{0}',@stocode='{1}',@memcode='{2}',@levelcode='{3}'", buscode, stocode, memcode, levelcode);
                sb.AppendFormat(" ,@regamount='{0}',@freeamount='{1}',@cardcost='{2}',@payamount='0'"
                    , (string.IsNullOrEmpty(regamount) || regamount.Trim() == "" ? "0" : regamount)
                    , (string.IsNullOrEmpty(freeamount) || freeamount.Trim() == "" ? "0" : freeamount)
                    , (string.IsNullOrEmpty(cardcost) || cardcost.Trim() == "" ? "0" : cardcost)
                    );
                sb.AppendFormat(" ,@validate='{0}',@password='{1}',@nowritedoc='{2}',@cname='{3}'", opencardinfo.validate.ToString("yyyy-MM-dd"), opencardinfo.password, opencardinfo.nowritedoc, opencardinfo.cname);
                sb.AppendFormat(" ,@mobile='{0}',@idtype='{1}',@IDNO='{2}',@ucode='{3}',@uname='{4}'", opencardinfo.mobile, opencardinfo.idtype, opencardinfo.IDNO, ucode, uname);
                sb.AppendFormat(" ,@sex='{0}',@bak1='{1}',@bak2='{2}',@bak3='{3}'", opencardinfo.sex, opencardinfo.bak1, opencardinfo.bak2, opencardinfo.bak3);
            }
            //添加开卡优惠券赠送数据
            if (opencardcoupon != null)
            {
                sb.AppendLine(" DELETE FROM [dbo].[opencardcoupon] WHERE levelcode='" + levelcode + "';");
                for (int i = 0; i < opencardcoupon.Count; i++)
                {
                    sb.AppendFormat(" exec[dbo].[p_openlevelcardcoupon_Add] @ordercode=@ordercode,@buscode='{0}',@stocode='{1}',@memcode='{2}',@levelcode='{3}'", buscode, stocode, memcode, levelcode);
                    sb.AppendFormat(" ,@pcode='{0}',@sumcode='{1}',@mccode='{2}',@num='{3}'", opencardcoupon[i].pcode, opencardcoupon[i].sumcode, opencardcoupon[i].mccode, opencardcoupon[i].num);
                }
            }
            if (opencardinfo != null)
            {
                sb.AppendLine(" end");
            }

            sb.AppendLine("if(@@error=0)begin commit tran tran1;select '0',@ordercode; end");
            sb.AppendLine("else begin rollback tran tran1;select '-1',''; end");

            string mescode = "Err_004";
            int intresult = -1;
            DataTable result = new bllPaging().GetDataTableInfoBySQL(sb.ToString());
            if (result != null && result.Rows.Count > 0)
            {

                intresult = Helper.StringToInt(result.Rows[0][0].ToString());
                switch (intresult)
                {
                    case 0:
                        ordercode = result.Rows[0][1].ToString();
                        break;
                    case -1:
                        mescode = "Err_004";
                        break;
                    case 2:
                        mescode = "memcardadd_notactive";
                        break;
                }

            }
            CheckDeleteResult(intresult, mescode);
            return dtBase;
        }



        /// <summary>
        /// 会员卡充值
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="ID"></param>
        /// <param name="buscode"></param>
        /// <param name="stocode"></param>
        /// <param name="memcode"></param>
        /// <param name="cardcode"></param>
        /// <param name="otype"></param>
        /// <param name="regamount"></param>
        /// <param name="freeamount"></param>
        /// <param name="cardcost"></param>
        /// <param name="payamount"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        /// <param name="ucode"></param>
        /// <param name="uname"></param>
        /// <param name="ordercode"></param>
        /// <param name="paystatus"></param>
        /// <param name="opencardcoupon"></param>
        /// <returns></returns>
        public DataTable Recharge(string GUID, string UID, out  string ID, string buscode, string stocode, string memcode, string cardtype, string cardcode, string otype, string regamount, string freeamount, string cardcost, string payamount, string remark, string status, string ucode, string uname, ref string ordercode, string paystatus, string pushemcode, string pushname, string shiftid, string pcname, string bak2, string bak3, List<opencardcouponEntity> opencardcoupon)
        {
            ID = "0";
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();

            StringBuilder sb = new StringBuilder();
            //添加会员卡开卡订单数据
            sb.AppendLine(" declare @ordercode VARCHAR(32);");
            sb.AppendLine(" declare @id int;");

            sb.AppendLine("begin tran tran1");
            sb.AppendFormat(" exec @id=[dbo].[p_memcardorders_Add] @ordercode=@ordercode output,@buscode='{0}',@stocode='{1}',@memcode='{2}',@cardcode='{3}'", buscode, stocode, memcode, cardcode);
            sb.AppendFormat(" ,@otype='{0}',@regamount='{1}',@freeamount='{2}',@cardcost='{3}',@payamount='0'", otype, regamount, freeamount, cardcost);
            sb.AppendFormat(" ,@remark='{0}',@status='{1}',@ucode='{2}',@uname='{3}',@pushemcode='{4}',@pushname='{5}',@ctype='{6}',@shiftid='{7}',@pcname='{8}'", remark, status, ucode, uname, pushemcode, pushname, cardtype, shiftid, pcname);
            sb.AppendFormat(" ,@bak2='{0}',@bak3='{1}',@mobile='',@memcname='',@idtype='',@idno='' ", bak2, bak3);
            sb.AppendLine(" if(@id=2) begin select @id,'';end else begin");//等于2，卡号已经激活

            //添加开卡优惠券赠送数据
            if (opencardcoupon != null)
            {
                sb.AppendLine(" DELETE FROM [dbo].[opencardcoupon] WHERE cardcode='" + cardcode + "';");
                for (int i = 0; i < opencardcoupon.Count; i++)
                {
                    sb.AppendFormat(" exec[dbo].[p_opencardcoupon_Add] @ordercode=@ordercode,@buscode='{0}',@stocode='{1}',@memcode='{2}',@cardcode='{3}'", buscode, stocode, memcode, cardcode);
                    sb.AppendFormat(" ,@pcode='{0}',@sumcode='{1}',@mccode='{2}',@num='{3}'", opencardcoupon[i].pcode, opencardcoupon[i].sumcode, opencardcoupon[i].mccode, opencardcoupon[i].num);
                }
            }

            sb.AppendLine(" end");

            sb.AppendLine("if(@@error=0)begin commit tran tran1;select '0',@ordercode; end");
            sb.AppendLine("else begin rollback tran tran1;select '-1',''; end");

            string mescode = "Err_004";
            int intresult = -1;
            DataTable result = new bllPaging().GetDataTableInfoBySQL(sb.ToString());
            if (result != null && result.Rows.Count > 0)
            {

                intresult = Helper.StringToInt(result.Rows[0][0].ToString());
                switch (intresult)
                {
                    case 0:
                        ordercode = result.Rows[0][1].ToString();
                        break;
                    case -1:
                        mescode = "Err_004";
                        break;
                    case 2:
                        mescode = "memcardadd_notactive";
                        break;
                }

            }
            CheckDeleteResult(intresult, mescode);
            return dtBase;
        }

        /// <summary>
        /// 退卡
        /// </summary>
        /// <returns></returns>
        public DataTable CardReturn(string GUID, string UID, out string ID, string buscode, string stocode, string memcode, string cardtype, string cardcode, string otype, string regamount, string freeamount, string cardcost, string payamount, string remark, string status, string ucode, string uname, ref string ordercode, string paystatus, string shiftid)
        {
            ID = "0";
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();

            StringBuilder sb = new StringBuilder();
            //添加会员卡开卡订单数据
            sb.AppendLine(" declare @ordercode VARCHAR(32);");
            sb.AppendLine(" declare @id int;");

            sb.AppendFormat(" exec @id=[dbo].[p_memcardorders_Return_Add] @ordercode=@ordercode output,@buscode='{0}',@stocode='{1}',@memcode='{2}',@cardcode='{3}'", buscode, stocode, memcode, cardcode);
            sb.AppendFormat(" ,@otype='{0}',@regamount='{1}',@freeamount='{2}',@cardcost='{3}',@payamount='0'", otype, regamount, freeamount, cardcost);
            sb.AppendFormat(" ,@remark='{0}',@status='{1}',@ucode='{2}',@uname='{3}',@ctype='{4}',@shiftid='{5}'", remark, status, ucode, uname, cardtype, shiftid);
            sb.AppendLine("select @id,@ordercode;");//等于2，卡号已经激活
            string mescode = "Err_004";
            int intresult = -1;
            DataTable result = new bllPaging().GetDataTableInfoBySQL(sb.ToString());
            if (result != null && result.Rows.Count > 0)
            {
                intresult = Helper.StringToInt(result.Rows[0][0].ToString());
                switch (intresult)
                {
                    case 0:
                        ordercode = result.Rows[0][1].ToString();
                        break;
                    case -1:
                        mescode = "Err_004";
                        break;
                    case 2:
                        mescode = "memcardreturn_004";
                        break;
                }

            }
            CheckDeleteResult(intresult, mescode);
            return dtBase;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ID">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public DataTable UpdateStatus(string GUID, string UID, string ids, string Status)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            int result = dal.UpdateStatus(ids, Status);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="filter">指定条件</param>
        /// <returns>返回第一行</returns>
        public DataTable GetPagingSigInfo(string GUID, string UID, string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            return GetPagingListInfo(GUID, UID, 1, 1, filter, string.Empty, out recnums, out pagenums);
        }

        /// <summary>
        /// 获取单条数据实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public memcardordersEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new memcardordersEntity();
        }

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentpage"></param>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="recnums"></param>
        /// <returns></returns>
        public DataTable GetPagingListInfo(string GUID, string UID, int pageSize, int currentpage, string filter, string order, out int recnums, out int pagenums)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                recnums = -1;
                pagenums = -1;
                return dtBase;
            }
            return new bllPaging().GetPagingInfo("memcardorders", "ID", "*,(select sum(payamount) from memcardorders " + filter + " ) as allmoney", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        public DataSet GetOpendCardInfo(string GUID, string UID, string ordercode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return null;
            }
            return new bllPaging().GetDataSetInfoBySQL("SELECT * FROM opencardinfo WHERE ordercode='" + ordercode + "';SELECT * FROM opencardcoupon WHERE ordercode='" + ordercode + "'");
        }

        public DataSet GetCardRechageInfo(string GUID, string UID, string ordercode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return null;
            }
            return new bllPaging().GetDataSetInfoBySQL("SELECT * FROM memcardorders WHERE ordercode='" + ordercode + "';SELECT * FROM opencardcoupon WHERE ordercode='" + ordercode + "'");
        }

        public DataTable GetCardOrderInfo(string GUID, string UID, string ordercode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return null;
            }
            return new bllPaging().GetDataTableInfoBySQL("SELECT * FROM memcardorders WHERE ordercode='" + ordercode + "';");
        }

        /// <summary>
        /// 获取实体信息集合（带明细）
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<memcardordersEntity> GetEntityInfos(string filter)
        {
            List<memcardordersEntity> lstOrders = null;

            StringBuilder sbSql = new StringBuilder();
            DataSet dsData = null;
            string strFileter = filter;
            if (!strFileter.ToLower().Contains(" where "))
            {
                strFileter = " where " + strFileter;
            }
            try
            {
                #region 损赔
                sbSql.AppendFormat("  SELECT * FROM memcardorders {0} ; ", strFileter);
                #endregion

                dsData = new bllPaging().GetDataSetInfoBySQL(sbSql.ToString());
                if (dsData != null && dsData.Tables.Count > 0)
                {
                    lstOrders = EntityHelper.GetEntityListByDT<memcardordersEntity>(dsData.Tables[0], null); //
                }
            }
            catch
            {
                return null;
            }
            finally
            {

                dsData = null;
            }

            return lstOrders;
        }

        /// <summary>
        /// 会员卡储值流水
        /// </summary>
        /// <param name="stocode">门店编号</param>
        /// <param name="btime">开始时间</param>
        /// <param name="etime">结束时间</param>
        /// <returns></returns>
        public DataSet GetCardRecharge(string stocode, string btime, string etime, string cardtype,string BusCode, int currentpage, int pageSize, out int recnums, out int pagenums)
        {
            recnums = 0;
            pagenums = 0;
            string where = " paystatus='2' AND A.otype IN('3','4','9','10','11')";
            if (stocode.Length > 0)
            {
                where += " and A.stocode='" + stocode + "'";
            }

            if (btime.Length > 0)
            {
                where += " and B.ctime>='" + btime + "'";
            }

            if (etime.Length > 0)
            {
                where += " and B.ctime<='" + etime + "'";
            }

            if (!string.IsNullOrEmpty(cardtype))
            {
                where += " and A.cardtype='" + cardtype + "'";
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                where += " and A.buscode='" + BusCode + "'";
            }
            DataSet ds = new DataSet();
            DataTable dtData = new bllPaging().GetPagingInfo("(SELECT * FROM memcardorders UNION SELECT * FROM dbo.TH_memcardorders) A INNER join (SELECT * FROM dbo.TB_Bill UNION SELECT * FROM dbo.TH_Bill) B ON A.stocode=B.StoCode and A.ordercode=B.OrderCodeList AND B.BillType='2' AND A.[paystatus] ='2' and B.TStatus='1'", "A.ctime", "B.PKCode,B.ctime,A.cardcode,cardcost=(CASE otype WHEN '9' then ISNULL(cardcost,0)*-1 ELSE ISNULL(cardcost,0) END),A.memcname,isnull(A.oldbalance,0.00)as oldbalance,regamount=(CASE otype WHEN '9' then ISNULL(regamount,0)*-1 ELSE ISNULL(regamount,0) END),A.freeamount,((CASE otype WHEN '9' then (ISNULL(regamount,0)+ISNULL(cardcost,0))*-1 ELSE ISNULL(regamount,0) END)+freeamount) as totalamount,isnull(A.balance,0.00)as balance,A.ucode,A.uname,stoname=catering.dbo.fnGetStoreName(A.stocode),A.stocode,A.pushemcode,A.pushname,A.otype,payremark=dbo.f_GetPaysinfohistory(A.ordercode)", pageSize, currentpage, where, "", "A.ctime", out recnums, out pagenums);
            if (dtData != null)
            {
                dtData.TableName = "Data";
                ds.Tables.Add(dtData);
            }
            string Sql = "SELECT count(1)as count,SUM(cardcost)AS cardcost,SUM(regamount)AS regamount,SUM(freeamount)AS freeamount,SUM(balance)AS balance,SUM(regamount+freeamount+tcardcost)AS totalamount FROM(SELECT cardcost=(CASE otype WHEN '9' then ISNULL(cardcost,0)*-1 ELSE ISNULL(cardcost,0) END),tcardcost=(CASE otype WHEN '9' then ISNULL(cardcost,0)*-1 ELSE 0  END),regamount=(CASE otype WHEN '9' then ISNULL(regamount,0)*-1 ELSE ISNULL(regamount,0) END),ISNULL(freeamount,0.00)AS freeamount,ISNULL(balance,0.00)AS balance FROM (SELECT * FROM memcardorders UNION SELECT * FROM dbo.TH_memcardorders) A INNER join (SELECT * FROM dbo.TB_Bill UNION SELECT * FROM dbo.TH_Bill) B ON A.stocode=B.StoCode and A.ordercode=B.OrderCodeList AND B.BillType='2' AND A.[paystatus] ='2' and B.TStatus='1' WHERE {0})AS XX";

            DataTable dtTotal = new bllPaging().GetDataTableInfoBySQL(string.Format(Sql, where));
            if (dtData != null)
            {
                dtData.TableName = "total";
                ds.Tables.Add(dtTotal);
            }
            return ds;
        }

        /// <summary>
        /// 会员卡储值流水导出
        /// </summary>
        /// <param name="stocode"></param>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <param name="currentpage"></param>
        /// <param name="pageSize"></param>
        /// <param name="recnums"></param>
        /// <param name="pagenums"></param>
        /// <returns></returns>
        public DataSet GetCardRechargeExport(string stocode, string btime, string etime, string cardtype,string BusCode)
        {
            string where = " where paystatus='2' AND A.otype IN('3','4','9','10','11')";
            if (stocode.Length > 0)
            {
                where += " and A.stocode='" + stocode + "'";
            }

            if (btime.Length > 0)
            {
                where += " and B.ctime>='" + btime + "'";
            }

            if (etime.Length > 0)
            {
                where += " and B.ctime<='" + etime + "'";
            }

            if (!string.IsNullOrEmpty(cardtype))
            {
                where += " and A.cardtype='" + cardtype + "'";
            }
            if (!string.IsNullOrEmpty(BusCode))
            {
                where += " and A.BusCode='" + BusCode + "'";
            }
            StringBuilder SQL = new StringBuilder();
            SQL.Append(@"declare @result table(id bigint identity,orderno varchar(32),detailcode varchar(32));
insert into @result 
select  B.PKCode,A.ordercode from (SELECT * FROM memcardorders UNION SELECT * FROM dbo.TH_memcardorders) A INNER join (SELECT * FROM dbo.TB_Bill UNION SELECT * FROM dbo.TH_Bill) B ON A.ordercode=B.OrderCodeList AND B.BillType='2' AND A.[paystatus] ='2' and B.TStatus='1' " + where);
            SQL.Append(@" select  ROW_NUMBER()OVER(order BY A.ctime)AS sortno,B.PKCode,B.ctime,A.cardcode,A.memcname,isnull(A.oldbalance,0.00)as oldbalance,cardcost=(CASE otype WHEN '9' then ISNULL(cardcost,0)*-1 ELSE ISNULL(cardcost,0) END),regamount=(CASE otype WHEN '9' then ISNULL(regamount,0)*-1 ELSE ISNULL(regamount,0) END),A.freeamount,((CASE otype WHEN '9' then (ISNULL(regamount,0)+ISNULL(cardcost,0))*-1 ELSE ISNULL(regamount,0) END)+freeamount) as totalamount,isnull(A.balance,0.00)as balance,A.ucode,A.uname
,stoname=catering.dbo.fnGetStoreName(A.stocode)
,A.stocode,A.pushemcode,A.pushname,A.otype
from (select * from(SELECT * FROM memcardorders UNION SELECT * FROM dbo.TH_memcardorders)X1 where X1.ordercode in(select detailcode from @result)) A INNER join (select * from (SELECT * FROM dbo.TB_Bill UNION SELECT * FROM dbo.TH_Bill)as X2 where X2.PKCode in(select orderno from @result)) B ON A.ordercode=B.OrderCodeList AND B.BillType='2' AND A.[paystatus] ='2' and B.TStatus='1';");
            SQL.Append(@" SELECT StoCode,BillCode,PayMethodName,convert(decimal(18,2),SUM(isnull(PayMoney,0))) AS paymoney  FROM 
(
select StoCode,BillCode,PayMethodName, PayMoney from TB_BillPay where TStatus='1'
union select StoCode,BillCode,PayMethodName, PayMoney from TH_BillPay where TStatus='1'
)as XX WHERE XX.BillCode in(select orderno from @result) GROUP BY  StoCode,BillCode,PayMethodName ;");
            return new bllPaging().GetDataSetInfoBySQL(SQL.ToString());
        }

        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private memcardordersEntity SetEntityInfo(DataRow dr)
        {
            memcardordersEntity Entity = new memcardordersEntity();
            Entity.ID = Helper.StringToLong(dr["ID"].ToString());
            Entity.buscode = dr["buscode"].ToString();
            Entity.stocode = dr["stocode"].ToString();
            Entity.memcode = dr["memcode"].ToString();
            Entity.cardcode = dr["cardcode"].ToString();
            Entity.otype = dr["otype"].ToString();
            Entity.regamount = Helper.StringToDecimal(dr["regamount"].ToString());
            Entity.freeamount = Helper.StringToDecimal(dr["freeamount"].ToString());
            Entity.cardcost = Helper.StringToDecimal(dr["cardcost"].ToString());
            Entity.payamount = Helper.StringToDecimal(dr["payamount"].ToString());
            Entity.remark = dr["remark"].ToString();
            Entity.status = dr["status"].ToString();
            Entity.ucode = dr["ucode"].ToString();
            Entity.uname = dr["uname"].ToString();
            Entity.ordercode = dr["ordercode"].ToString();
            Entity.paystatus = dr["paystatus"].ToString();
            Entity.oldcardcodes = dr["oldcardcodes"].ToString();
            Entity.oldbalance = dr["oldbalance"].ToString();
            Entity.balance = Helper.StringToDecimal(dr["balance"].ToString());
            Entity.ptime = Helper.StringToDateTime(dr["ptime"].ToString());

            Entity.shiftid = Helper.StringToLong(dr["shiftid"].ToString());
            Entity.cardname = dr["cardname"].ToString();
            Entity.cardtype = dr["cardtype"].ToString();
            Entity.pushemcode = dr["pushemcode"].ToString();
            Entity.pushname = dr["pushname"].ToString();
            Entity.stotel = dr["stotel"].ToString();
            Entity.amanagerid = dr["amanagerid"].ToString();
            Entity.amanagername = dr["amanagername"].ToString();

            Entity.memcname = dr["memcname"].ToString();//会员姓名
            Entity.mob = dr["mob"].ToString();//会员手机号
            Entity.pcname = dr["pcname"].ToString();//赠送礼包名称
            Entity.bak1 = dr["bak1"].ToString();//扩展信息1
            Entity.bak2 = dr["bak2"].ToString();//扩展信息2
            Entity.bak3 = dr["bak3"].ToString();//扩展信息3

            return Entity;
        }

        /// <summary>
        /// 退卡支付
        /// </summary>
        /// <param name="buscode"></param>
        /// <param name="stocode"></param>
        /// <param name="cardcode"></param>
        /// <param name="cardtype"></param>
        /// <param name="otype"></param>
        /// <param name="refundmoney"></param>
        /// <param name="postmac"></param>
        /// <param name="ordercode"></param>
        /// <param name="shiftid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int RefundMemcard(string buscode, string stocode, string cardcode, string cardtype, string otype, string refundmoney, string postmac, string ordercode, string shiftid, string userid, string terminaltype, ref string payordercode)
        {
            return dal.RefundMemcard(buscode, stocode, cardcode, cardtype, otype, refundmoney, postmac, ordercode, shiftid, userid, terminaltype, ref payordercode);
        }

        /// <summary>
        /// 更新合并卡信息
        /// </summary>
        /// <param name="ordercode"></param>
        /// <param name="oldcards"></param>
        /// <param name="oldbalance"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public int UpdateMemcardMerg(string ordercode, string oldcards, string oldbalance, string remark)
        {
            return new bllPaging().ExecuteNonQueryBySQL("update dbo.memcardorders set oldcardcodes='" + oldcards + "',oldbalance='" + oldbalance + "',remark='" + remark + "' where ordercode='" + ordercode + "'");
        }

        public int UpdateCardOrder(string cardcode, string ordercode, string oldcardcodes, string oldbalance, string balance, string memcname, string mob, string pcname, string bak1, string bak2, string bak3)
        {
            string updateCardcode = string.IsNullOrWhiteSpace(cardcode) ? "" : ("cardcode='" + cardcode + "',");
            string sql = "update dbo.memcardorders set " + updateCardcode + " oldcardcodes='" + oldcardcodes + "',oldbalance='" + oldbalance + "',balance='" + balance + "',ptime=getdate(),memcname='" + memcname + "',mob='" + mob + "',bak1='" + bak1 + "'  where ordercode='" + ordercode + "'";
            return new bllPaging().ExecuteNonQueryBySQL(sql);
        }

        /// <summary>
        /// 获取连锁端的会员卡类型(有效的)
        /// </summary>
        /// <returns></returns>
        public DataTable GetMemCardTypeList(string BusCode)
        {
            return dal.GetMemCardTypeList(BusCode);
        }


    }
}