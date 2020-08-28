using System.Data;
using System.Data.SqlClient;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
using System;

namespace CommunityBuy.DAL
{
    /// <summary>
    /// 会员信息表数据访问类
    /// </summary>
    public partial class dalmembers
    {
        MSSqlDataAccess DBHelper = new MSSqlDataAccess();
        int intReturn;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ref membersEntity Entity, ref string MesCode)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@memid", Entity.memid),
                new SqlParameter("@memcode" ,SqlDbType.VarChar,32),
				new SqlParameter("@wxaccount", Entity.wxaccount),
				new SqlParameter("@mobile", Entity.mobile)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.memid =sqlParameters[0].Value.ToString();
                Entity.memcode = sqlParameters[1].Value.ToString();
            }
            return intReturn;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(membersEntity Entity, ref string MesCode)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@memid", Entity.memid),
				new SqlParameter("@memcode", Entity.memcode),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@wxaccount", Entity.wxaccount),
				new SqlParameter("@mobile", Entity.mobile),
				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),
                new SqlParameter("@mescode",SqlDbType.VarChar,128)
             };

            sqlParameters[sqlParameters.Length - 1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_Update", CommandType.StoredProcedure, sqlParameters);
            if (intReturn != -1)
            {
                MesCode = sqlParameters[sqlParameters.Length-1].Value.ToString();
            }

            return intReturn;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="memcode">标识</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public int UpdateStatus(string ids, string Status)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@ids", ids),
				new SqlParameter("@status", Status)
             };
            return DBHelper.ExecuteNonQuery("dbo.p_members_UpdateStatus", CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID，多个用,分隔</param>
        /// <returns>返回操作结果</returns>
        public int Delete(string memcode, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@memcode", memcode),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,64,mescode)
             };
            sqlParameters[1].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_Delete", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[1].Value.ToString();
            return intReturn;
        }


        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="memcode"></param>
        /// <param name="mobile"></param>
        /// <param name="IDNO"></param>
        /// <param name="paypwd"></param>
        /// <param name="mescode"></param>
        /// <returns></returns>
        public int MembersChangePwd(string memcode, string mobile, string paypwd, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@memcode", memcode),
                 new SqlParameter("@mobile", mobile),
                 new SqlParameter("@paypwd", paypwd),
             };
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_changepwd", CommandType.StoredProcedure, sqlParameters);
            return intReturn;
        }




        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="memcode"></param>
        /// <param name="IDNO"></param>
        /// <param name="cname"></param>
        /// <param name="mescode"></param>
        /// <returns></returns>
        public int ApplyCashOut(string memcode, float money, float fee, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@memcode", memcode),
                 new SqlParameter("@money", money),
                 new SqlParameter("@fee", fee),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,128,mescode)
             };
            sqlParameters[3].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_earnApplyOut", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[3].Value.ToString();
            return intReturn;
        }

        #region 修改微信账号信息
        //设置手机号
        public int SetPhone(string openid, string mobile, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@openid", openid),
                 new SqlParameter("@mobile", mobile),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };

            sqlParameters[2].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_wx_setphone", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[2].Value.ToString();
            return intReturn;
        }

        //设置支付密码
        public int SetUPwd(string openid, string upwd, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@openid", openid),
                 new SqlParameter("@upwd", upwd),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };

            sqlParameters[2].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_wx_setpwd", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[2].Value.ToString();
            return intReturn;
        }

        //设置支付密码（新）
        public int SetUPwdNew(string openid, string mobile, string idno, string upwd, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@openid", openid),
                 new SqlParameter("@mobile",mobile),
                 new SqlParameter("@idno",idno),
                 new SqlParameter("@upwd", upwd),
                 new SqlParameter("@mescode",SqlDbType.VarChar ,128,mescode)
             };

            sqlParameters[4].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_wx_setpwdnew", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[4].Value.ToString();
            return intReturn;
        }
        #endregion
    }
}