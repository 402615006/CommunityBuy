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
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@wxaccount", Entity.wxaccount),
				new SqlParameter("@cname", Entity.cname),
				new SqlParameter("@birthday", Entity.birthday),
				new SqlParameter("@sex", Entity.sex),
				new SqlParameter("@mobile", Entity.mobile),


				new SqlParameter("@idtype", Entity.idtype),
				new SqlParameter("@IDNO", Entity.IDNO),
				new SqlParameter("@provinceid", Entity.provinceid),
				new SqlParameter("@cityid", Entity.cityid),
				new SqlParameter("@areaid", Entity.areaid),
				new SqlParameter("@photo", Entity.photo),

				new SqlParameter("@address", Entity.address),

				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),

				new SqlParameter("@cuser", Entity.cuser),

				new SqlParameter("@mescode",SqlDbType.VarChar,128)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[1].Direction = ParameterDirection.Output;
            sqlParameters[29].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.memid =sqlParameters[0].Value.ToString();
                Entity.memcode = sqlParameters[1].Value.ToString();
            }
            else
            {
                MesCode = sqlParameters[29].Value.ToString();
            }
            return intReturn;
        }

        /// <summary>
        /// 微信增加一条数据并开卡
        /// </summary>
        public int WxAdd(ref membersEntity Entity, ref string MesCode)
        {
            intReturn = 0;
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@memid", Entity.memid),
                new SqlParameter("@memcode" ,SqlDbType.VarChar,32),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@wxaccount", Entity.wxaccount),
				new SqlParameter("@cname", Entity.cname),
				new SqlParameter("@birthday", Entity.birthday),
				new SqlParameter("@sex", Entity.sex),
				new SqlParameter("@mobile", Entity.mobile),
				new SqlParameter("@idtype", Entity.idtype),
				new SqlParameter("@IDNO", Entity.IDNO),
				new SqlParameter("@provinceid", Entity.provinceid),
				new SqlParameter("@cityid", Entity.cityid),
				new SqlParameter("@areaid", Entity.areaid),
				new SqlParameter("@photo", Entity.photo),
				new SqlParameter("@address", Entity.address),
				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@cuser", Entity.cuser),

				new SqlParameter("@mescode",SqlDbType.VarChar,128)
             };
            sqlParameters[0].Direction = ParameterDirection.Output;
            sqlParameters[1].Direction = ParameterDirection.Output;
            sqlParameters[29].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_wx_Add", CommandType.StoredProcedure, sqlParameters);
            if (intReturn == 0)
            {
                Entity.memid =sqlParameters[0].Value.ToString();
                Entity.memcode = sqlParameters[1].Value.ToString();
                MesCode = sqlParameters[29].Value.ToString();
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
				new SqlParameter("@cname", Entity.cname),
				new SqlParameter("@birthday", Entity.birthday),
				new SqlParameter("@sex", Entity.sex),
				new SqlParameter("@mobile", Entity.mobile),
				new SqlParameter("@idtype", Entity.idtype),
				new SqlParameter("@IDNO", Entity.IDNO),
				new SqlParameter("@provinceid", Entity.provinceid),
				new SqlParameter("@cityid", Entity.cityid),
				new SqlParameter("@areaid", Entity.areaid),
				new SqlParameter("@photo", Entity.photo),
				new SqlParameter("@address", Entity.address),
				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@cuser", Entity.cuser),
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

        public int UpdatePhone(string memcode, string phoneno, string uuser, ref string MesCode)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@memcode", memcode),
				new SqlParameter("@phoneno", phoneno),
				new SqlParameter("@uuser", uuser),
                new SqlParameter("@mescode",SqlDbType.VarChar,128)
             };

            sqlParameters[3].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_UpdatePhone", CommandType.StoredProcedure, sqlParameters);
            if (intReturn != -1)
            {
                MesCode = sqlParameters[3].Value.ToString();
            }

            return intReturn;
        }

        /// <summary>
        /// 更新会员信息(微信专用)
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="MesCode"></param>
        /// <returns></returns>
        public int UpdateByWx(membersEntity Entity, ref string MesCode)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@memid", Entity.memid),
				new SqlParameter("@memcode", Entity.memcode),
				new SqlParameter("@buscode", Entity.buscode),
				new SqlParameter("@wxaccount", Entity.wxaccount),
				new SqlParameter("@cname", Entity.cname),
				new SqlParameter("@birthday", Entity.birthday),
				new SqlParameter("@sex", Entity.sex),
				new SqlParameter("@mobile", Entity.mobile),
				new SqlParameter("@idtype", Entity.idtype),
				new SqlParameter("@IDNO", Entity.IDNO),
				new SqlParameter("@provinceid", Entity.provinceid),
				new SqlParameter("@cityid", Entity.cityid),
				new SqlParameter("@areaid", Entity.areaid),
				new SqlParameter("@photo", Entity.photo),
				new SqlParameter("@address", Entity.address),
				new SqlParameter("@remark", Entity.remark),
				new SqlParameter("@status", Entity.status),
				new SqlParameter("@cuser", Entity.cuser),
                new SqlParameter("@mescode",SqlDbType.VarChar,128)
             };

            sqlParameters[27].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_Update_wx", CommandType.StoredProcedure, sqlParameters);
            if (intReturn != -1)
            {
                MesCode = sqlParameters[27].Value.ToString();
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
        /// 检测手机号码证件号码是否存在
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="idtype">证件号码</param>
        /// <param name="IDNO"></param>
        /// <param name="mescode">输出错误提示编码</param>
        /// <returns></returns>
        public int MembersCheck(string mobile, string idtype, string IDNO, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@mobile", mobile),
                 new SqlParameter("@idtype", idtype),
                 new SqlParameter("@IDNO", IDNO),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,128,mescode)
             };
            sqlParameters[3].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_check", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[3].Value.ToString();
            return intReturn;
        }

        public int MembersCheckID(string idtype, string IDNO, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@idtype", idtype),
                 new SqlParameter("@IDNO", IDNO),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,128,mescode)
             };
            sqlParameters[2].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_checkidno", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[2].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 检测手机号是否存在
        /// </summary>
        /// <param name="memcode"></param>
        /// <param name="mobile"></param>
        /// <param name="mescode"></param>
        /// <returns></returns>
        public int MembersCheckMobile(string memcode, string mobile, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@memcode", memcode),
                 new SqlParameter("@mobile", mobile),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,128,mescode)
             };
            sqlParameters[2].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_changebymob", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[2].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="memcode"></param>
        /// <param name="mobile"></param>
        /// <param name="ousercode"></param>
        /// <param name="ousername"></param>
        /// <param name="mescode"></param>
        /// <returns></returns>
        public int MembersChangeMobile(string memcode, string mobile, string ousercode, string ousername, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@memcode", memcode),
                 new SqlParameter("@mobile", mobile),
                 new SqlParameter("@ousercode", ousercode),
                 new SqlParameter("@ousername", ousername),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,128,mescode)
             };
            sqlParameters[4].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_changemobile", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[4].Value.ToString();
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
        public int MembersChangePwd(string memcode, string mobile, string IDNO, string paypwd, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@memcode", memcode),
                 new SqlParameter("@mobile", mobile),
                 new SqlParameter("@IDNO", IDNO),
                 new SqlParameter("@paypwd", paypwd),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,128,mescode)
             };
            sqlParameters[4].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_members_changepwd", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[4].Value.ToString();
            return intReturn;
        }

        /// <summary>
        /// 会员注册
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="pwd"></param>
        /// <param name="mescode"></param>
        /// <returns></returns>
        public DataTable MemberRegister(string source, string mobile, string weixin, string email, string qq, string pwd, ref string mescode)
        {
            mescode = "";
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@source", source),
                 new SqlParameter("@mobile", mobile),
                 new SqlParameter("@pwd", pwd),
                 new SqlParameter("@weixin", weixin),
                 new SqlParameter("@qq", qq),
                 new SqlParameter("@email", email),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,128,mescode)
            };
            sqlParameters[6].Direction = ParameterDirection.Output;
            DataTable dt = DBHelper.ExecuteDataTable("dbo.p_members_register", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[6].Value.ToString();
            return dt;
        }

        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="pwd"></param>
        /// <param name="mescode"></param>
        /// <returns></returns>
        public DataTable MemberLogin(string source, string mobile, string weixin, string email, string qq, string pwd, ref string mescode)
        {
            mescode = "";
            DataTable dt = null;
            SqlParameter[] sqlParameters = 
                {
                     new SqlParameter("@source", source),
                     new SqlParameter("@weixin", weixin),
                     new SqlParameter("@qq", qq),
                     new SqlParameter("@mobile", mobile),
                     new SqlParameter("@email", email),
                     new SqlParameter("@pwd", pwd),
                     new SqlParameter("@mescode",SqlDbType.VarChar,32)
                 };
            sqlParameters[6].Direction = ParameterDirection.Output;
            try
            {
                dt = DBHelper.ExecuteDataTable("dbo.p_members_login", CommandType.StoredProcedure, sqlParameters);
                mescode = sqlParameters[6].Value.ToString().Trim();
            }
            catch (Exception ex)
            {
                return null;
            }
            return dt;
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
        public int ModifyCityByOpenid(string openid, string proname, string cityname, string areaname)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@openid", openid),
                 new SqlParameter("@proname", proname),
                 new SqlParameter("@cityname", cityname),
                 new SqlParameter("@areaname", areaname)
             };

            return DBHelper.ExecuteNonQuery("p_modifycitybyopenid", CommandType.StoredProcedure, sqlParameters);
        }

        public int ModifyInfoByOpenid(string openid, string type, string value)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@openid", openid),
                 new SqlParameter("@type", type),
                 new SqlParameter("@value", value)
             };

            return DBHelper.ExecuteNonQuery("p_modifyinfobyopenid", CommandType.StoredProcedure, sqlParameters);
        }

        //设置小额免密
        public int SetnotPwd(string openid, string notpwd, string amount, ref string mescode)
        {
            SqlParameter[] sqlParameters = 
            {
                 new SqlParameter("@openid", openid),
                 new SqlParameter("@notpwd", notpwd),
                 new SqlParameter("@amount", amount),
                 new SqlParameter("@mescode",SqlDbType.NVarChar ,256,mescode)
             };

            sqlParameters[3].Direction = ParameterDirection.Output;
            intReturn = DBHelper.ExecuteNonQuery("dbo.p_wx_setnotpwd", CommandType.StoredProcedure, sqlParameters);
            mescode = sqlParameters[3].Value.ToString();
            return intReturn;
        }

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