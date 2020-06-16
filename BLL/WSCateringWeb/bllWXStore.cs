using System.Collections.Generic;
using System.Data;
using CommunityBuy.CommonBasic;
using CommunityBuy.Model;
using CommunityBuy.DAL;
namespace CommunityBuy.BLL
{
	/// <summary>
    /// 商家门店信息业务类
    /// </summary>
    public class bllWXStore : bllBase
    {
		dalWXStore dal = new dalWXStore();
        WXStoreEntity Entity = new WXStoreEntity();

		/// <summary>
        /// 检验表单数据
        /// </summary>
        /// <returns></returns>
        public string CheckPageInfo(string type, string stoid, string comcode, string buscode, string stocode, string cname, string sname, string bcode, string indcode, string provinceid, string cityid, string areaid, string address, string stoprincipal, string stoprincipaltel, string tel, string stoemail, string logo, string backgroundimg, string descr, string stourl, string stocoordx, string stocoordy, string netlinklasttime, string calcutime, string busHour, string recommended, string remark, string status, string cuser, string uuser, string btime, string etime, string TerminalNumber, string ValuesDate, string isfood, string pstocode, string sqcode, string storetype, string jprice)
        {
			string strRetuen = string.Empty;
            //要验证的实体属性
            List<string> EName = new List<string>() {  };
            //要验证的实体属性值
            List<string> EValue = new List<string>() {  };
            //错误信息
            List<string> errorCode = new List<string>();
            List<string> ControlName = new List<string>();
            //验证数据
            CheckValue<WXStoreEntity>(EName, EValue, ref errorCode, new WXStoreEntity());
            //特殊验证写在下面

            if (errorCode.Count > 0)
            {
                strRetuen = ErrMessage.GetMessageInfoByListCode(errorCode);
            }
            else//组合对象数据
            {
                Entity = new WXStoreEntity();
				Entity.stoid = Helper.StringToLong(stoid);
				Entity.comcode = comcode;
				Entity.buscode = buscode;
				Entity.stocode = stocode;
				Entity.cname = cname;
				Entity.sname = sname;
				Entity.bcode = bcode;
				Entity.indcode = indcode;
				Entity.provinceid = Helper.StringToInt(provinceid);
				Entity.cityid = Helper.StringToInt(cityid);
				Entity.areaid = Helper.StringToInt(areaid);
				Entity.address = address;
				Entity.stoprincipal = stoprincipal;
				Entity.stoprincipaltel = stoprincipaltel;
				Entity.tel = tel;
				Entity.stoemail = stoemail;
				Entity.logo = logo;
				Entity.backgroundimg = backgroundimg;
				Entity.descr = descr;
				Entity.stourl = stourl;
				Entity.stocoordx = stocoordx;
				Entity.stocoordy = stocoordy;
				Entity.netlinklasttime = Helper.StringToDateTime(netlinklasttime);
				Entity.calcutime = Helper.StringToDateTime(calcutime);
				Entity.busHour = busHour;
				Entity.recommended = recommended;
				Entity.remark = remark;
				Entity.status = status;
				Entity.cuser = Helper.StringToLong(cuser);
				
				Entity.uuser = Helper.StringToLong(uuser);
				
				
				Entity.btime = btime;
				Entity.etime = etime;
				Entity.TerminalNumber = Helper.StringToInt(TerminalNumber);
				Entity.ValuesDate = Helper.StringToDateTime(ValuesDate);
				Entity.isfood = isfood;
				Entity.pstocode = pstocode;
				Entity.sqcode = sqcode;
				Entity.storetype = storetype;
				Entity.jprice = Helper.StringToDecimal(jprice);
            }
            return strRetuen;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public DataTable Add(string GUID, string UID, out  string stoid, string comcode, string buscode, string stocode, string cname, string sname, string bcode, string indcode, string provinceid, string cityid, string areaid, string address, string stoprincipal, string stoprincipaltel, string tel, string stoemail, string logo, string backgroundimg, string descr, string stourl, string stocoordx, string stocoordy, string netlinklasttime, string calcutime, string busHour, string recommended, string remark, string status, string cuser, string uuser, string btime, string etime, string TerminalNumber, string ValuesDate, string isfood, string pstocode, string sqcode, string storetype, string jprice, operatelogEntity entity)
        {
			stoid = "0";
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }

            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("add",  stoid, comcode, buscode, stocode, cname, sname, bcode, indcode, provinceid, cityid, areaid, address, stoprincipal, stoprincipaltel, tel, stoemail, logo, backgroundimg, descr, stourl, stocoordx, stocoordy, netlinklasttime, calcutime, busHour, recommended, remark, status, cuser, uuser, btime, etime, TerminalNumber, ValuesDate, isfood, pstocode, sqcode, storetype, jprice);
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
        /// 更新一条数据
        /// </summary>
        public DataTable Update(string GUID, string UID,  string stoid, string comcode, string buscode, string stocode, string cname, string sname, string bcode, string indcode, string provinceid, string cityid, string areaid, string address, string stoprincipal, string stoprincipaltel, string tel, string stoemail, string logo, string backgroundimg, string descr, string stourl, string stocoordx, string stocoordy, string netlinklasttime, string calcutime, string busHour, string recommended, string remark, string status, string cuser, string uuser, string btime, string etime, string TerminalNumber, string ValuesDate, string isfood, string pstocode, string sqcode, string storetype, string jprice, operatelogEntity entity)
        {
			
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
            string spanids = string.Empty;
            string strReturn = CheckPageInfo("update",  stoid, comcode, buscode, stocode, cname, sname, bcode, indcode, provinceid, cityid, areaid, address, stoprincipal, stoprincipaltel, tel, stoemail, logo, backgroundimg, descr, stourl, stocoordx, stocoordy, netlinklasttime, calcutime, busHour, recommended, remark, status, cuser, uuser, btime, etime, TerminalNumber, ValuesDate, isfood, pstocode, sqcode, storetype, jprice);
            //数据页面验证
            if (!CheckControl(strReturn, spanids))
            {
                return dtBase;
            }
			//获取更新前的数据对象
            WXStoreEntity OldEntity = new WXStoreEntity();
            OldEntity = GetEntitySigInfo(" where stoid='" + stoid + "'");
			//更新数据
            int result = dal.Update(Entity);
            //检测执行结果
            if (CheckResult(result))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add<WXStoreEntity>(entity, Entity, OldEntity);
                }
            }
            return dtBase;
        }

		/// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="stoid">标识</param>
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
        /// 删除数据
        /// </summary>
        /// <param name="ID">主键ID</param>
        /// <returns>返回操作结果</returns>
        public DataTable Delete(string GUID, string UID, string stoid, operatelogEntity entity)
        {
			if (!CheckLogin(GUID, UID))//非法登录
            {
                return dtBase;
            }
            dtBase.Clear();
			string Mescode = string.Empty;
            int result = dal.Delete(stoid, ref Mescode);
            //检测执行结果
            if (CheckDeleteResult(result,Mescode))
            {
                //写日志
                if (entity != null)
                {
                    blllog.Add(entity);
                }

            }
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
            return GetPagingListInfo(GUID, UID, 1, 1, filter,"", out recnums, out pagenums);
        }

		/// <summary>
        /// 获取单条数据实体对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public WXStoreEntity GetEntitySigInfo(string filter)
        {
            int recnums = 0;
            int pagenums = 0;
            DataTable dt = GetPagingListInfo("", "0", 1, 1, filter, string.Empty, out recnums, out pagenums);
            if (dt != null && dt.Rows.Count > 0)
            {
                return SetEntityInfo(dt.Rows[0]);
            }
            return new WXStoreEntity();
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
            return new bllPaging().GetLSPagingInfo("Store s left join storegx sg on s.stocode=sg.stocode  left join ts_Dicts dic on sg.firtype=dic.diccode", "s.stoid", "'1' as isdc,s.*,dic.dicname,dic.diccode as firtype,cusername=dbo.fnGetUserName(s.cuser),uusername=dbo.fnGetUserName(s.uuser),areaname =dbo.fnGetAreaFullName(s.provinceid,s.cityid,s.areaid),netStatus=dbo.fnContrastMinute(s.stoid),busname=dbo.f_GetBusinessCname(s.buscode),sg.stopath,'' as stopathname,'' as sstatus,'0' as iscoupon,'0' as isyuding,'0' as ispaidui,sg.jprice,sg.ptype", pageSize, currentpage, filter, "", order, out recnums, out pagenums);
        }

		/// <summary>
        /// 单行数据转实体对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private WXStoreEntity SetEntityInfo(DataRow dr)
        {
            WXStoreEntity Entity = new WXStoreEntity();
			Entity.stoid = Helper.StringToLong(dr["stoid"].ToString());
			Entity.comcode = dr["comcode"].ToString();
			Entity.buscode = dr["buscode"].ToString();
			Entity.stocode = dr["stocode"].ToString();
			Entity.cname = dr["cname"].ToString();
			Entity.sname = dr["sname"].ToString();
			Entity.bcode = dr["bcode"].ToString();
			Entity.indcode = dr["indcode"].ToString();
			Entity.provinceid = Helper.StringToInt(dr["provinceid"].ToString());
			Entity.cityid = Helper.StringToInt(dr["cityid"].ToString());
			Entity.areaid = Helper.StringToInt(dr["areaid"].ToString());
			Entity.address = dr["address"].ToString();
			Entity.stoprincipal = dr["stoprincipal"].ToString();
			Entity.stoprincipaltel = dr["stoprincipaltel"].ToString();
			Entity.tel = dr["tel"].ToString();
			Entity.stoemail = dr["stoemail"].ToString();
			Entity.logo = dr["logo"].ToString();
			Entity.backgroundimg = dr["backgroundimg"].ToString();
			Entity.descr = dr["descr"].ToString();
			Entity.stourl = dr["stourl"].ToString();
			Entity.stocoordx = dr["stocoordx"].ToString();
			Entity.stocoordy = dr["stocoordy"].ToString();
			Entity.netlinklasttime = Helper.StringToDateTime(dr["netlinklasttime"].ToString());
			Entity.calcutime = Helper.StringToDateTime(dr["calcutime"].ToString());
			Entity.busHour = dr["busHour"].ToString();
			Entity.recommended = dr["recommended"].ToString();
			Entity.remark = dr["remark"].ToString();
			Entity.status = dr["status"].ToString();
			Entity.cuser = Helper.StringToLong(dr["cuser"].ToString());
			
			Entity.uuser = Helper.StringToLong(dr["uuser"].ToString());
			
			
			Entity.btime = dr["btime"].ToString();
			Entity.etime = dr["etime"].ToString();
			Entity.TerminalNumber = Helper.StringToInt(dr["TerminalNumber"].ToString());
			Entity.ValuesDate = Helper.StringToDateTime(dr["ValuesDate"].ToString());
			Entity.isfood = dr["isfood"].ToString();
			Entity.pstocode = dr["pstocode"].ToString();
			Entity.sqcode = dr["sqcode"].ToString();
			Entity.storetype = dr["storetype"].ToString();
			Entity.jprice = Helper.StringToDecimal(dr["jprice"].ToString());
            return Entity;
        }

        public DataTable GetTopProduct(string StoCode)
        {
            return dal.GetTopProduct(StoCode);
        }

    }
}