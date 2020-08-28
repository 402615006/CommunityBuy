using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CommunityBuy.BLL;
using CommunityBuy.Model;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.IServices
{
    /// <summary>
    /// 菜品信息接口类
    /// </summary>
    public class WS_TB_Dish : ServiceBase
    {
        bllTB_Dish bll = new bllTB_Dish();
        DataTable dt = new DataTable();
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(HttpContext context)
        {
            if (CheckParameters(context))//检测是否合法
            {
                Dictionary<string, object> dicPar = GetParameters();
                if (dicPar != null)
                {
                    switch (actionname.ToLower())
                    {
                        case "getlist"://列表
                            GetList(dicPar);
                            break;
                        //case "add"://添加							
                        //    Add(dicPar);
                        //    break;
                        case "detail"://详细
                            Detail(dicPar);
                            break;
                        case "update"://修改							
                            Update(dicPar);
                            break;
                        case "delete"://删除
                            Delete(dicPar);
                            break;
                        case "updatestatus"://修改状态
                            UpdateStatus(dicPar);
                            break;
                    }
                }
            }
        }

        private void GetList(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "page", "limit", "filters", "orders" };

            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }

            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string userid = dicPar["userid"].ToString();
            int pageSize = StringHelper.StringToInt(dicPar["limit"].ToString());
            int currentPage = StringHelper.StringToInt(dicPar["page"].ToString());
            string depart = "";
            string filter = JsonHelper.ObjectToJSON(dicPar["filters"]);
            string opencode = "";
            object opencodeObj = "";
            string channel = "";
            dicPar.TryGetValue("opencode", out opencodeObj);
            if (opencodeObj != null)
            {
                opencode = opencodeObj.ToString();
            }
            DataTable dtFilter = new DataTable();
            if (filter.Length > 0 && filter != "[]")
            {
                filter = JsonHelper.JsonToFilterByString(filter, out dtFilter);
                if (dtFilter != null)
                {
                    DataRow[] drArr = dtFilter.Select("cus<>''");
                    foreach (DataRow dr in drArr)
                    {
                        string col = dr["col"].ToString();
                        switch (col)
                        {
                            case "sing":
                                string singCode = dr["filter"].ToString();
                                filter += " and (dis.DisCode in(select DisCode from TR_SignDish where SchCode='" + singCode + "') or dis.TypeCode in(select DisTypeCode from TR_SignDish where SchCode='" + singCode + "'))";
                                break;
                            case "dis.DisName":
                                string filter1 = filter.Replace("where", "");
                                filter1 = filter1.Replace("DisName", "QuickCode");
                                filter += " or (" + filter1 + ")";
                                filter += " or (" + filter1.Replace("QuickCode", "CusDisCode") + ")";
                                filter += " or (" + filter1.Replace("QuickCode", "QRCode") + ")";
                                break;
                            case "Depart":
                                depart = dr["filter"].ToString();
                                break;
                            case "Channel":
                                channel = dr["filter"].ToString();
                                break;
                            case "TypeCode":
                                string type = dr["cus"].ToString();
                                string value = dr["filter"].ToString();
                                if (type == "1")
                                {
                                    filter += " and (dis.typecode='" + value + "' or dis.typecode in(select PKCode from [dbo].[TB_DishType] where PKKCode='" + value + "'))";
                                }
                                else
                                {
                                    filter += " and dis.typecode='" + value + "'";
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                filter = "where 1=1";
            }
            string order = JsonHelper.ObjectToJSON(dicPar["orders"]);
            if (order.Length > 0)
            {
                order = JsonHelper.JsonToOrderByString(order);
            }
            if (string.IsNullOrEmpty(order))
            {
                order = " order by dis.ctime desc";
            }
            if (!string.IsNullOrWhiteSpace(depart))
            {
                filter += " and dis.MenuCode in(select PKCode from TB_DishMenu where IsDefault='1' and stocode=dis.stocode and DepCode='" + depart + "') ";
            }
            if (!string.IsNullOrWhiteSpace(channel))
            {
                filter += " and charindex('" + channel + "',dis.ChannelCodeList)>0 ";
            }


            if (!dicPar.ContainsKey("HT"))//门店后台独有参数
            {
                if (string.IsNullOrWhiteSpace(depart))
                {
                    filter += " and dis.MenuCode in(select PKCode from TB_DishMenu where IsDefault='1' and stocode=dis.stocode) ";
                }
                else
                {
                    filter += " and dis.MenuCode in(select PKCode from TB_DishMenu where IsDefault='1' and stocode=dis.stocode and DepCode='" + depart + "') ";
                }
            }

            int recordCount = 0;
            int totalPage = 0;
            //调用逻辑
            dt = bll.GetPagingListInfo(GUID, USER_ID, pageSize, currentPage, filter, order, out recordCount, out totalPage);
            ReturnListJson(dt, pageSize, recordCount, currentPage, totalPage);
        }

        //private void Add(Dictionary<string, object> dicPar)
        //{
        //    //要检测的参数信息
        //    List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "BusCode", "StoCode", "CCname", "UCname", "TStatus", "ChannelCodeList", "DisCode", "DisName", "OtherName", "TypeCode", "QuickCode", "CusDisCode", "Unit", "Price", "MenuCode", "MemPrice", "CostPrice", "RoyMoney", "ExtCode", "FinCode", "KitCode", "CookerCode", "MakeTime", "QRCode", "WarCode", "MatCode", "Descript", "IsCount", "DefCount", "CountPrice", "IsVarPrice", "IsWeight", "IsMethod", "IsStock", "IsPoint", "IsMemPrice", "IsCoupon", "IsKeep", "IsCombo", "CCode", "UCode", "ImageName", "dishesMethodsJson", "dishescombosJson", "dishescomboinfoJson" };
        //    //检测方法需要的参数
        //    if (!CheckActionParameters(dicPar, pra))
        //    {
        //        return;
        //    }
        //    //获取参数信息
        //    string userid = dicPar["userid"].ToString();
        //    string GUID = dicPar["GUID"].ToString();
        //    string USER_ID = dicPar["USER_ID"].ToString();
        //    string BusCode = dicPar["BusCode"].ToString();
        //    string StoCode = dicPar["StoCode"].ToString();
        //    string TStatus = dicPar["TStatus"].ToString();
        //    string ChannelCodeList = "";
        //    string DisCode = dicPar["DisCode"].ToString();
        //    string DisName = dicPar["DisName"].ToString();
        //    string OtherName = dicPar["OtherName"].ToString();
        //    string TypeCode = dicPar["TypeCode"].ToString();
        //    string QuickCode = dicPar["QuickCode"].ToString();
        //    string Unit = dicPar["Unit"].ToString();
        //    string Price = dicPar["Price"].ToString();
        //    string CostPrice = dicPar["CostPrice"].ToString();
        //    string QRCode = dicPar["QRCode"].ToString();
        //    string Descript = dicPar["Descript"].ToString();
        //    if (string.IsNullOrEmpty(QuickCode))
        //    {
        //        QuickCode = StringHelper.GetChineseSpell(DisName);
        //    }
        //    //调用逻辑
        //    bll.Add( GUID, USER_ID,  BusCode,  StoCode,  TStatus,  ChannelCodeList,  DisCode,  DisName,  OtherName,  TypeCode,  QuickCode,  Unit,  Price,  CostPrice,  QRCode,  Descript);
        //    ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        //}

        private void Update(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "BusCode", "StoCode", "CCname", "UCname", "TStatus", "ChannelCodeList", "DisCode", "DisName", "OtherName", "TypeCode", "QuickCode", "CusDisCode", "Unit", "Price", "MenuCode", "MemPrice", "CostPrice", "RoyMoney", "ExtCode", "FinCode", "KitCode", "CookerCode", "MakeTime", "QRCode", "WarCode", "MatCode", "Descript", "IsCount", "DefCount", "CountPrice", "IsVarPrice", "IsWeight", "IsMethod", "IsStock", "IsPoint", "IsMemPrice", "IsCoupon", "IsKeep", "IsCombo", "CCode", "UCode", "ImageName", "dishesMethodsJson", "dishescombosJson", "dishescomboinfoJson" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string userid = dicPar["userid"].ToString();
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string StoCode = dicPar["StoCode"].ToString();
            string ChannelCodeList = "";
            string DisCode = dicPar["DisCode"].ToString();
            string DisName = dicPar["DisName"].ToString();
            string OtherName = dicPar["OtherName"].ToString();
            string QuickCode = dicPar["QuickCode"].ToString();
            string Unit = dicPar["Unit"].ToString();
            string Price = dicPar["Price"].ToString();
            string CostPrice = dicPar["CostPrice"].ToString();
            string QRCode = dicPar["QRCode"].ToString();
            string Descript = dicPar["Descript"].ToString();

          
            if (string.IsNullOrEmpty(QuickCode))
            {
                QuickCode = StringHelper.GetChineseSpell(DisName);
            }

            //调用逻辑
            TB_DishEntity UEntity = bll.GetEntitySigInfo("Where DisCode='"+ DisCode + "' and Stocode='"+StoCode+"'");
            UEntity.ChannelCodeList = ChannelCodeList;
            UEntity.CostPrice =StringHelper.StringToDecimal(CostPrice);
            UEntity.Descript = Descript;
            UEntity.DisName = DisName;
            UEntity.OtherName = OtherName;
            UEntity.Price = StringHelper.StringToDecimal(Price);
            UEntity.QRCode = QRCode;
            UEntity.QuickCode = QuickCode;
            UEntity.Unit = Unit;

            bll.Update(GUID, USER_ID, UEntity);

            ReturnResultJson(bll.oResult.Code,bll.oResult.Msg);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="dicPar"></param>
        private void Detail(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "DisCode" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string DisCode = dicPar["DisCode"].ToString();
            //调用逻辑			
            TB_DishEntity DEntity = bll.GetEntitySigInfo("Where DisCode='" + DisCode + "'");
            ReturnJsonByT< TB_DishEntity>(DEntity);
        }

        /// <summary>
        /// 删除菜品
        /// </summary>
        /// <param name="dicPar"></param>
        private void Delete(Dictionary<string, object> dicPar)
        {
            //要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "userid", "id" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string DisCode = dicPar["id"].ToString();
            //调用逻辑
           bll.Delete(GUID, USER_ID, DisCode);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dicPar"></param>
        private void UpdateStatus(Dictionary<string, object> dicPar)
        {
            ///要检测的参数信息
            List<string> pra = new List<string>() { "GUID", "USER_ID", "id", "status" };
            //检测方法需要的参数
            if (!CheckActionParameters(dicPar, pra))
            {
                return;
            }
            //获取参数信息
            string GUID = dicPar["GUID"].ToString();
            string USER_ID = dicPar["USER_ID"].ToString();
            string ids = dicPar["id"].ToString();
            string status = dicPar["status"].ToString();
            string DisCode = dicPar["id"].ToString().Trim(',');
            TB_DishEntity UEntity = bll.GetEntitySigInfo("Where DisCode='" + DisCode + "'");
            UEntity.TStatus = status;

            bll.Update(GUID, USER_ID, UEntity);
            ReturnResultJson(bll.oResult.Code, bll.oResult.Msg);
        }

    }
}