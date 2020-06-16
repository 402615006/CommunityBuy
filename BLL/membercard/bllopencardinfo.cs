using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
namespace CommunityBuy.BLL
{
    /// <summary>
    /// 发卡临时信息表业务类
    /// </summary>
    public class bllopencardinfo : bllBase
    {
        DAL.dalopencardinfo dal = new DAL.dalopencardinfo();
        opencardinfoEntity Entity = new opencardinfoEntity();

        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string oid, string ordercode, string buscode, string stocode, string memcode, string cardcode, string regamount, string freeamount, string cardcost, string payamount, string validate, string password, string nowritedoc, string cname, string mobile, string idtype, string IDNO, string ucode, string uname, out string spanids)
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
            CheckValue<opencardinfoEntity>(EName, EValue, ref errorCode, new opencardinfoEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
                spanids = ListTostring(ControlName);
            }
            else//组合对象数据
            {
                Entity = new opencardinfoEntity();
                Entity.oid = Helper.StringToLong(oid);
                Entity.ordercode = ordercode;
                Entity.buscode = buscode;
                Entity.stocode = stocode;
                Entity.memcode = memcode;
                Entity.cardcode = cardcode;
                Entity.regamount = Helper.StringToDecimal(regamount);
                Entity.freeamount = Helper.StringToDecimal(freeamount);
                Entity.cardcost = Helper.StringToDecimal(cardcost);
                Entity.payamount = Helper.StringToDecimal(payamount);
                Entity.validate = Helper.StringToDateTime(validate);
                Entity.password = password;
                Entity.nowritedoc = nowritedoc;
                Entity.cname = cname;
                Entity.mobile = mobile;
                Entity.idtype = idtype;
                Entity.IDNO = IDNO;
                Entity.ucode = ucode;
                Entity.uname = uname;

            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID, out  string oid, string ordercode, string buscode, string stocode, string memcode, string cardcode, string regamount, string freeamount, string cardcost, string payamount, string validate, string password, string nowritedoc, string cname, string mobile, string idtype, string IDNO, string ucode, string uname, operatelogEntity entity)
        {
            oid = "0";
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add", oid, ordercode, buscode, stocode, memcode, cardcode, regamount, freeamount, cardcost, payamount, validate, password, nowritedoc, cname, mobile, idtype, IDNO, ucode, uname, out spanids);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
            int result = dal.Add(ref Entity);
            //检测执行结果
            CheckResult(result);
            return dtBase;
        }

        /// <summary>
        /// 更新一条数据,bak1-存放押金
        /// </summary>
        public DataTable Update(string ordercode, string oldcardcodes, string oldbalance, string balance, string memcname, string mob, string pcname, string bak1, string bak2, string bak3, string newCardCode = "")
        {
            dtBase.Clear();
            string updateCardcode = string.IsNullOrWhiteSpace(newCardCode) ? "" : ("cardcode='" + newCardCode + "',");
            string sql = "update dbo.memcardorders set " + updateCardcode + " oldcardcodes='" + oldcardcodes + "',oldbalance='" + oldbalance + "',balance='" + balance + "',ptime=getdate(),memcname='" + memcname + "',mob='" + mob + "',bak1='" + bak1 + "'  where ordercode='" + ordercode + "'";
            int result = new bllPaging().ExecuteNonQueryBySQL(sql);
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
        public opencardinfoEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new opencardinfoEntity();
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
            return new bllPaging().GetPagingInfo("opencardinfo", "oid", "*", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

        public DataSet GetOpendCardInfo(string ordercode)
        {
            return new bllPaging().GetDataSetInfoBySQL("SELECT * FROM opencardinfo WHERE ordercode='" + ordercode + "';SELECT * FROM opencardcoupon WHERE ordercode='" + ordercode + "'");
        }

        public DataSet GetCardRechageInfo(string ordercode)
        {
            return new bllPaging().GetDataSetInfoBySQL("SELECT * FROM memcardorders WHERE ordercode='" + ordercode + "';SELECT * FROM opencardcoupon WHERE ordercode='" + ordercode + "'");
        }

        public DataTable GetCardOrderInfo(string ordercode)
        {
            return new bllPaging().GetDataTableInfoBySQL("SELECT * FROM memcardorders WHERE ordercode='" + ordercode + "';");
        }
        /// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private opencardinfoEntity SetEntityInfo(DataRow dr)
        {
            opencardinfoEntity Entity = new opencardinfoEntity();
            Entity.oid = Helper.StringToLong(dr["oid"].ToString());
            Entity.ordercode = dr["ordercode"].ToString();
            Entity.buscode = dr["buscode"].ToString();
            Entity.stocode = dr["stocode"].ToString();
            Entity.memcode = dr["memcode"].ToString();
            Entity.cardcode = dr["cardcode"].ToString();
            Entity.regamount = Helper.StringToDecimal(dr["regamount"].ToString());
            Entity.freeamount = Helper.StringToDecimal(dr["freeamount"].ToString());
            Entity.cardcost = Helper.StringToDecimal(dr["cardcost"].ToString());
            Entity.payamount = Helper.StringToDecimal(dr["payamount"].ToString());
            Entity.validate = Helper.StringToDateTime(dr["validate"].ToString());
            Entity.password = dr["password"].ToString();
            Entity.nowritedoc = dr["nowritedoc"].ToString();
            Entity.cname = dr["cname"].ToString();
            Entity.mobile = dr["mobile"].ToString();
            Entity.idtype = dr["idtype"].ToString();
            Entity.IDNO = dr["IDNO"].ToString();
            Entity.ucode = dr["ucode"].ToString();
            Entity.uname = dr["uname"].ToString();

            return Entity;
        }
    }
}