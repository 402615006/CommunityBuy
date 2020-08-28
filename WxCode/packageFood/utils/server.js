import { req, upf } from "../../utils/req.js";   // 请求接口数据公共方法引入
import {serverURL} from "../../utils/api.js";    //域名引入




//获取美食首页商圈
export function get_shangquanlist(data){
  return req({
    url: `${serverURL}/WeiXinSercices/WXsqinfo.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//获取美食首页轮播图
export function get_bannerlist(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXsqinfo.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//获取美食首页门店
export function get_stocodelist(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXsqinfo.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//获取门店详情信息
export function get_stocodedetail(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXStore.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//添加排队
export function add_queue(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXQueue.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在取号...', //loading中显示的文字
    loadingtimes: 500  //最低loading显示时间,没有要求写0
  })
}
//添加预约
export function add_yuyue(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXReservation.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在预约...', //loading中显示的文字
    loadingtimes: 500  //最低loading显示时间,没有要求写0
  })
}
//预约记录
export function yuyue_jl(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXReservation.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//排队记录
export function paidui_jl(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXQueue.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 排队详情
export function paidui_detail(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXQueue.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//取消排队
export function cen_paidui(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXQueue.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在取消...', //loading中显示的文字
    loadingtimes: 500  //最低loading显示时间,没有要求写0
  })
}
//取消排队
export function cen_yuyue(data) {
  return req({
    url: `${serverURL}/WeiXinSercices/WXReservation.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在取消...', //loading中显示的文字
    loadingtimes: 500  //最低loading显示时间,没有要求写0
  })
}

// 获取会员卡列表
export function getmemcardslist(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 产品明细
export function getTJdetail(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTB_TopProducts.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//拼团明细
export function getcolagedetail(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTB_Collage.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//开团
export function addmaincollageorder(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_CollageOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//参团
export function addcollageorder(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_CollageOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//下单
export function addorder(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_ProductOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//推荐数量限制判断
export function checkaddorder(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_ProductOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//热门推荐支付
export function payorderbypcode(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_ProductOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//拼团支付
export function paycollagemoney(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_CollageOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//推荐订单详情
export function getTJorderDetail(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_ProductOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//推荐取消订单
export function cancelorderTJ(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_ProductOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//拼团订单详情
export function getPTorderDetail(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_CollageOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//推荐取消订单
export function cancelorderPT(data) {
  return req({
    url: `${baserURLcard}/Marketing/WSTM_CollageOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//门店预约时间获取
export function get_rtime(data) {
  return req({
    url: `${serverURL}/IServices/WSTB_Reservation.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//菜品获取
export function get_foots(data) {
  return req({
    url: `${serverURL}/App/WS_Store.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//菜品标签获取
export function get_foottag(data) {
  return req({
    url: `${serverURL}/ajax/dishes/WSDisheType.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//根据标签获取菜品
export function get_footlistdata(data) {
  return req({
    url: `${serverURL}/ajax/dishes/WSDishe.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 100  //最低loading显示时间,没有要求写0
  })
}


//做法加价获取
export function get_Method(data) {
  return req({
    url: `${serverURL}/ajax/dishes/WSDishe.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//获取档口
export function get_getdepart(data){
  return req({
    url: `${serverURL}/App/WS_Store.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//套餐获取
export function get_Pacage(data) {
  return req({
    url: `${serverURL}/Ar/WS_Dish.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//开台
export function open_table(data) {
  return req({
    url: `${serverURL}/App/WS_Table.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在开台...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//下单
export function add_order(data) {
  return req({
    url: `${serverURL}/ajax/order/WSOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在下单...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//获取订单信息
export function get_orders(data) {
  return req({
    url: `${serverURL}/ajax/order/WSOrder.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//待支付订单详细
export function get_Waitorders(data) {
  return req({
    url: `${serverURL}/App/WS_Order.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}


//添加账单
export function add_bill(data) {
  return req({
    url: `${serverURL}/App/WS_Bill.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//取消账单
export function cen_bill(data) {
  return req({
    url: `${serverURL}/App/WS_Bill.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在取消...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}


//获取优惠券
export function get_coupon(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//获取会员卡
export function get_vipcard(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//使用优惠券
export function refund_coupon(data) {
  return req({
    url: `${serverURL}/App/WS_Bill.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在提交...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//取消使用优惠券
export function cancel_coupon(data) {
  return req({
    url: `${serverURL}/App/WS_Bill.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在取消...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}

//取消部分支付
export function cancel_pay(data) {
  return req({
    url: `${serverURL}/App/WS_Bill.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在取消...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}


//会员卡支付
export function refund_cards(data){
  return req({
    url: `${serverURL}/App/WS_Bill.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//微信支付
export function wx_Pay(data){
  return req({
    url: `${serverURL}/App/WS_Bill.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//微信支付后刷新
export function new_wx_Pay(data){
  return req({
    url: `${serverURL}/App/WS_Bill.ashx`,
    data: data,
    method: 'POST',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在刷新', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}


