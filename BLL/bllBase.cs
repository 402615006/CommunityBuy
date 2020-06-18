
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CommunityBuy.CommonBasic;

namespace CommunityBuy.BLL
{
    /// <summary>
    /// 数据操作返回结果
    /// </summary>
    public class OperateResult
    {
        public string Code;
        public string Msg;
        public string Data;
    }

    /// <summary>
    /// 数据验证
    /// </summary>
    public class bllBase
    {
        public OperateResult oResult = new OperateResult();

        public void CheckResult(int result,string data)
        {
            switch (result)
            {
                case 0:
                    oResult.Code = "0";
                    oResult.Msg = "操作成功";
                    oResult.Data = data;
                    break;
                case -2:
                    oResult.Code = "-1";
                    oResult.Msg = "参数错误";
                    oResult.Data = data;
                    break;
                default:
                    oResult.Code = "-1";
                    oResult.Msg = "操作失败";
                    oResult.Data = "0";
                    break;
            }
        }



    }
}
