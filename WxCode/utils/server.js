
import { req, upf } from "./req";   // 请求接口数据公共方法引入
import { serverURL} from "./api.js";    //域名引入


//记录页面
export function set_enterPageAct(name, actid, acttype, remark) {
  let unionid='';
  if (wx.getStorageSync('unionid')){
    unionid = wx.getStorageSync('unionid');
  }
  let data = {
    'actionname':'enterPageAct',
    'parameters': {
      "GUID": "",
      "USER_ID": unionid,
      "pagename": name,
      "actid": actid,
      "acttype": acttype,
      "remark": remark,
      "source": "WX"
    }
  }
  data.parameters = JSON.stringify(data.parameters);
  wx.request({
    url: `${serverURL}/CollectData.ashx`,
    data: data,
    header: {
      'content-type': 'application/x-www-form-urlencoded' // 默认值
    },
    method: 'POST',
    dataType: 'json',
    success: function (res) {

    }
  })
}


//************************************************************************************
//首页大屏弹窗
export function get_homeMode(data) {
  return req({
    url: `${serverURL}/OnlineService/ActivityAd.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

// 更新用户信息
export function set_userInfo(data){
  return req({
    url: `${serverURL}/UserCenter.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

// 发送验证码
export function set_verificationcode(data){
  return req({
    url: `${serverURL}/Other.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}


//获取session_key
export function get_session_key(data) {
  return req({
    url: `${serverURL}/ajax/member/member.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//解密
export function get_jiemi(data) {
  return req({
    url: `${serverURL}/ajax/member/member.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '数据请求中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//用手机号进行登录
export function login_mobile(data) {
  return req({
    url: `${serverURL}/ajax/member/member.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '登录中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//获取优惠券详情
export function get_coupondetail(data) {
  return req({
    url: `${serverURL}/Coupon.ashx`,
    data: {
      "actionname": "getcoupondes",
      "parameters": data
    },
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//赠送优惠券
export function sharecoupon(data) {
  return req({
    url: `${serverURL}/Coupon.ashx`,
    data: {
      "actionname": "sharecoupon",
      "parameters": data
    },
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//赠送优惠券领取
export function getsharecoupon(data) {
  return req({
    url: `${serverURL}/Coupon.ashx`,
    data: {
      "actionname": "getsharecoupon",
      "parameters": data
    },
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//获取我的页面
export function get_my(data) {
  return req({
    url: `${serverURL}/UserCenter.ashx`,
    data: {
      "actionname": "getusermsgtrue",
      "parameters": data
    },
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}


//订单页面公共需要的接口
// *************************************************************************************
//取消订单
export function cen_orders(data) {
  return req({
    url: `${serverURL}/Order.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在取消订单...', //loading中显示的文字
    loadingtimes: 1000  //最低loading显示时间,没有要求写0
  })
}

//订单提交
export function submit_orders(data) {
  return req({
    url: `${serverURL}/OrderN.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在支付...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//订单中获取优惠券
export function getmoviemaincouponbymoney(data) {
  return req({
    url: `${serverURL}/Coupon.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//待使用优惠券
export function get_couponlist(data) {
  return req({
    url: `${serverURL}/Coupon.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}


//单独购买商品信息
export function get_goodsdetail(data) {
  return req({
    url: `${serverURL}/Order.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//订单列表
export function get_dinglist(data) {
  return req({
    url: `${serverURL}/order.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//******************************************************
// 我的页面获取优惠券
export function get_couponnewlist(data) {
  return req({
    url: `${serverURL}/Mywallet/Coupon.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//我的优惠券详情
export function get_couponnewdetail(data) {
  return req({
    url: `${serverURL}/Mywallet/Coupon.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//我的优惠券详情赠送
export function send_couponnews(data) {
  return req({
    url: `${serverURL}/Mywallet/Coupon.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//我的优惠券详情赠送后进到首页领取
export function get_couponnews(data) {
  return req({
    url: `${serverURL}/Mywallet/Coupon.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

// 我的钱包页面获取信息
export function get_walletinfo(data){
  return req({
    url: `${serverURL}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 我的订单
export function get_myorderlist(data) {
  return req({
    url: `${baserURLfood}/App/WS_Bill.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//********************************************* */
//首页新人获取优惠券
export function get_newpeoplecoupon(data) {
  return req({
    url: `${serverURL}/Mywallet/Coupon.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//********************************************* */
//轮播图
export function getLBlist(data) {
  return req({
    url: `${serverURL}/ajax/advert/advert.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//首页公告
export function getnewslist(data) {
  return req({
    url: `${serverURL}/ajax/advert/advert.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

// 首页产品列表
export function getdishlist(data) {
  return req({
    url: `${serverURL}/ajax/dishes/WSdishe.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

// 推荐订单
export function getTJorderlist(data) {
  return req({
    url: `${serverURL}/Marketing/WSTM_ProductOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

// 拼团订单
export function getPTorderlist(data) {
  return req({
    url: `${serverURL}/Marketing/WSTM_CollageOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

// 获取首页模块信息列表
export function get_homeblock(data){
  return req({
    url: `${serverURL}/SystemManage/WXsqinfo.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

// 分类页面获取一级分类和附加商圈信息
export function getsqandhyinfolist(data) {
  return req({
    url: `${serverURL}/SystemManage/WXsqinfo.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

// 分类页面各种搜索
export function getselectstoreinfolist(data) {
  return req({
    url: `${serverURL}/ajax/store/store.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 200  //最低loading显示时间,没有要求写0
  })
}

//获取vip信息
export function get_myviplevel(data) {
  return req({
    url: `${serverURL}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}




//***********************会员********************** */
//新人专享列表
export function newuserprolist(data) {
  return req({
    url: `${serverURL}/manage/orders/aboutproorder.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//新人专享详情
export function newuserprodetail(data) {
  return req({
    url: `${serverURL}/manage/orders/aboutproorder.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//闲弟推荐列表
export function topprolist(data) {
  return req({
    url: `${serverURL}/manage/orders/aboutproorder.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//支付获取订单号
export function proorderadd(data) {
  return req({
    url: `${serverURL}/manage/orders/aboutproorder.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//支付
export function pay_collagemoney(data,type) {
  var url = '/manage/orders/aboutproorder.ashx';
  if(type==5){
    url = '/manage/orders/aboutviporder.ashx';
  }
  return req({
    url: `${serverURL}`+url,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '支付中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}


//取消订单（vip、组局除外）
export function can_celproorder(data) {
  return req({
    url: `${serverURL}/manage/orders/aboutproorder.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在取消...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//申请退款（组局、vip商品除外)
export function cen_proordrefundnew(data) {
  return req({
    url: `${serverURL}/manage/orders/aboutproorder.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在申请退款...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//获取用户可用佣金
export function get_usercomprice(data) {
  return req({
    url: `${serverURL}/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//查看协议
export function get_agree(data) {
  return req({
    url: `${serverURL}/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//vip会员生成海报
export function get_friendimg(data) {
  return req({
    url: `${serverURL}/Other.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在生成海报...', //loading中显示的文字
    loadingtimes:300  //最低loading显示时间,没有要求写0
  })
}






