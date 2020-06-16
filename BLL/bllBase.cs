
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 数据验证
    /// </summary>
    public class bllBase
    {
        public blloperatelog blllog = new blloperatelog();
        public DataTable dtBase = new DataTable();

        public bllBase()
        {
            dtBase = new DataTable("error");
            dtBase.Columns.Add("type", typeof(string));
            dtBase.Columns.Add("mes", typeof(string));
            dtBase.AcceptChanges();
        }

        /// <summary>
        /// List转string
        /// </summary>
        /// <param name="spans"></param>
        /// <returns></returns>
        public string ListTostring(List<string> spans)
        {
            string strReturn = string.Empty;
            foreach (string str in spans)
            {
                strReturn += str + "|";
            }
            return strReturn.TrimEnd('|');
        }

        /// <summary>
        /// 检测删除数据库执行结果
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="Mescode"></param>
        /// <returns></returns>
        public bool CheckDeleteResult(int Result, string Mescode)
        {
            bool Flag = false;
            DataRow dr = dtBase.NewRow();
            switch (Result)
            {
                case 0:
                    Flag = true;
                    dr["type"] = "0";
                    dr["mes"] = ErrMessage.GetMessageInfoByCode("Err_001").Body;
                    break;
                case 1:
                    dr["type"] = "1";
                    dr["mes"] = ErrMessage.GetMessageInfoByCode(Mescode).Body;
                    break;
                case 100:
                    dr["type"] = "2";
                    dr["mes"] = Mescode;
                    break;
                case 2:
                    dr["type"] = "2";
                    dr["mes"] = ErrMessage.GetMessageInfoByCode(Mescode).Body;
                    break;
                case 99:
                    Flag = true;
                    dr["type"] = "0";
                    dr["mes"] = Mescode;
                    break;
                default:
                    dr["type"] = "2";
                    dr["mes"] = ErrMessage.GetMessageInfoByCode(Mescode).Body;
                    break;
            }
            dtBase.Rows.Add(dr);
            dtBase.AcceptChanges();
            return Flag;
        }

        public bool CheckControl(string strReturn)
        {
            bool Flag = true;
            //数据验证
            if (strReturn.Length > 0)
            {
                Flag = false;
                DataRow DataVerify = dtBase.NewRow();
                DataVerify["type"] = "1";
                DataVerify["mes"] = strReturn;
                dtBase.Rows.Add(DataVerify);
            }
            return Flag;
        }

        public bool CheckControl(string strReturn, string spanids)
        {
            bool Flag = true;
            //数据验证
            if (strReturn.Length > 0)
            {
                Flag = false;
                DataRow DataVerify = dtBase.NewRow();
                DataVerify["type"] = "1";
                DataVerify["mes"] = strReturn;
                DataVerify["spanids"] = spanids;
                dtBase.Rows.Add(DataVerify);
            }
            return Flag;
        }

        /// <summary>
        /// 检测用户登录状态
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="UID"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool CheckLogin(string GUID, string UID)
        {
            bool Flag = true;
            //dtBase = LoginUniqueness.AppLoginedCheckFromPage(GUID, UID);
            ////if (dtBase.Rows.Count > 0)//非法登录
            ////{
            ////    Flag = false;
            ////}
            return Flag;
        }

        public bool APPCheckLogin(string GUID, string UID)
        {
            bool Flag = true;
            //dtBase = LoginUniqueness.AppLoginedCheckFromPage(GUID, UID);
            ////if (dtBase.Rows.Count > 0)//非法登录
            ////{
            ////    Flag = false;
            ////}
            return Flag;
        }

        public bool CheckResult(int Result)
        {
            bool Flag = false;
            DataRow dr = dtBase.NewRow();
            switch (Result)
            {
                case 0://已操作成功
                    Flag = true;
                    dr["type"] = "0";
                    dr["mes"] = ErrMessage.GetMessageInfoByCode("Err_001").Body;
                    break;
                case 1://记录已存在，请检查!
                    dr["type"] = "2";
                    dr["mes"] = ErrMessage.GetMessageInfoByCode("Err_002").Body;
                    break;
                case 2://操作失败，请重试或联系管理员!
                    dr["type"] = "2";
                    dr["mes"] = ErrMessage.GetMessageInfoByCode("Err_003").Body;
                    break;
                case 3://请选择要操作的记录
                    dr["type"] = "2";
                    dr["mes"] = ErrMessage.GetMessageInfoByCode("Err_004").Body;
                    break;
            }
            dtBase.Rows.Add(dr);
            dtBase.AcceptChanges();
            return Flag;
        }

        /// <summary>
        /// 检测删除数据库执行结果
        /// </summary>
        /// <param name="Result">执行情况</param>
        /// <param name="Mescode">提示信息</param>
        /// <param name="spanids">控件ID</param>
        /// <returns></returns>
        public bool CheckControlResult(int Result, string Mescode)
        {
            bool Flag = false;
            DataRow dr = dtBase.NewRow();
            switch (Result)
            {
                case 0:
                    Flag = true;
                    dr["type"] = "0";
                    dr["mes"] = ErrMessage.GetMessageInfoByCode("Err_001").Body;
                    break;
                case 99:
                    dr["type"] = "1";
                    dr["mes"] = Mescode;
                    break;
                default:
                    dr["type"] = "2";

                    dr["mes"] = ErrMessage.GetMessageInfoByCode(Mescode).Body;
                    break;
            }
            dtBase.Rows.Add(dr);
            dtBase.AcceptChanges();
            return Flag;
        }


        /// <summary>
        /// 检验表单数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="PropertyName">实体属性名称</param>
        /// <param name="value">待验证的属性值</param>
        /// <param name="errorCode">返回的错误List</param>
        /// <param name="t">实体对象</param>
        public void CheckValue<T>(List<string> EName, List<string> EValue, ref List<string> errorCode, T t)
        {
            if (EName == null && EValue == null && t == null)
            {
                return;
            }
            if (EName.Count != EValue.Count)
            {
                return;
            }
            ModelInfoAttribute ModelInfo = new ModelInfoAttribute();
            System.Reflection.MemberInfo propInfo = null;
            ModelInfoAttribute myAttr;
            string PropertyName = string.Empty;
            for (int i = 0; i < EName.Count; i++)
            {
                PropertyName = EName[i];
                if (PropertyName.Trim().Length > 0)
                {
                    propInfo = t.GetType().GetProperty(PropertyName);
                    if (propInfo != null)
                    {
                        myAttr = (ModelInfoAttribute)Attribute.GetCustomAttribute(propInfo, typeof(ModelInfoAttribute));
                        if (myAttr != null)
                        {
                            ModelInfo.Length = myAttr.Length;
                            ModelInfo.Name = myAttr.Name;
                            ModelInfo.NotEmpty = myAttr.NotEmpty;
                            ModelInfo.NotEmptyECode = myAttr.NotEmptyECode;
                            ModelInfo.RType = myAttr.RType;
                            ModelInfo.RTypeECode = myAttr.RTypeECode;
                            CheckValue(EValue[i], ModelInfo, ref errorCode);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ModelInfo"></param>
        /// <param name="errorCode"></param>
        private void CheckValue(string value, ModelInfoAttribute ModelInfo, ref List<string> ErrorCode)
        {
            if (value.Length == 0)
            {
                if (ModelInfo.NotEmpty)//不能为空
                {
                    ErrorCode.Add(ModelInfo.NotEmptyECode);
                }
            }
            else
            {
                if (value.Length > ModelInfo.Length && ModelInfo.Length != 0)//长度溢出
                {
                    ErrorCode.Add(ModelInfo.RTypeECode);
                }
                else
                {
                    if (ModelInfo.RType != RegularExpressions.RegExpType.Normal)
                    {
                        if (!RegularExpressions.IsRegExpType(value, ModelInfo.RType))//格式不正确
                        {
                            ErrorCode.Add(ModelInfo.RTypeECode);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证DataSet的有消息 如果有一个空都为false
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool CheckDataSet(DataSet ds)
        {
            bool status = true;
            foreach (DataTable dt in ds.Tables)
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    status = false;
                }
            }
            return status;
        }

        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="VCode"></param>
        /// <returns></returns>
        public DataTable CheckMobileVerificationCode(string GUID, string UID, string mobile, string VCode)
        {
            if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            int Flag = LoginUniqueness.MobileMesCheck(mobile, VCode);
            if (Flag > 0)
            {
                dtBase.Clear();
                DataRow LoginVerify = dtBase.NewRow();
                LoginVerify["type"] = "-1";
                if (Flag == 1)
                {
                    LoginVerify["mes"] = "验证码过期";
                }
                else if (Flag == 2)
                {
                    LoginVerify["mes"] = "验证码不正确";
                }
                dtBase.Rows.Add(LoginVerify);
            }
            else
            {
                DataRow LoginVerify = dtBase.NewRow();
                LoginVerify["type"] = "0";
                LoginVerify["mes"] = "";
                dtBase.Rows.Add(LoginVerify);
            }
            return dtBase;
        }

        /// <summary>
        /// 获取权限范围条件
        /// </summary>
        /// <param name="ColName">查询列名</param>
        /// <returns></returns>
        protected string GetAuthoritywhere(string ColName)
        {
            string where = string.Empty;
            if (LoginedUser.UserInfo.rolstocode.Length > 0)
            {
                where = " and " + ColName + " in('" + LoginedUser.UserInfo.rolstocode.Replace(",", "','") + "')";
            }
            else
            {
                //集团
                //分公司
            }
            return where;
        }
    }
}
