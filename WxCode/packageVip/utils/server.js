import { req, upf } from "../../utils/req.js";   // 请求接口数据公共方法引入

//绑定会员卡
export function bind_vipcards(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在绑定...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//解绑会员卡
export function untied_vipcards(data){
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在解绑...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//会员卡详情
export function get_memcardinfo(data){
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//设置默认会员卡
export function setdefaultmemcard(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: '0'  //最低loading显示时间,没有要求写0
  })
}
//会员卡消费记录
export function getmemcardconsumption(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//二维码付款界面 我的会员卡
export function get_mywalletvipcardlist(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//二维码付款界面 生成的二维码信息
export function get_myaccountpaycode(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在刷新...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
//轮询查看支付状态
export function get_mypaystatus(data) {
  return req({
    url: `${baserURLcard}/memcardpay/memcardpay.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
//轮询查看支付密码验证
export function get_mypaypassword(data) {
  return req({
    url: `${baserURLcard}/memcardpay/memcardpay.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '密码验证中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}





//优惠券领取列表
export function get_receivecouponlist(data) {
  return req({
    url: `${baserURLcard}/Mywallet/Coupon.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 领取优惠券
export function receivecoupon(data) {
  return req({
    url: `${baserURLcard}/Mywallet/Coupon.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在领取...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 获取会员卡开卡列表
export function get_opencardslist(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

// 获取开卡详情
export function get_opencardsdetail(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

// 获取充值卡详情
export function get_cardinfodetail(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在加载...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 开卡调取
export function set_opencard(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在提交...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 充值调取
export function set_chargecardorder(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在提交...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 客服
export function getcustomerbytype(data) {
  return req({
    url: `${baserURLcard}/Mywallet/WSTB_Customer.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

// 获取是否免密和手机号
export function get_memberinfo(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 小额免密
export function no_password(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 修改密码
export function update_password(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 修改密码
export function update_tel(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在修改...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 身份认证
export function members_changeidno(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在认证...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}


//上传身份证识别出身份信息
export function uploadfile_card(data) {
  return upf({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx?actionname=memberscheckidno&parameters=`,
    data: data,
    showLoading: true,
    loadingtext: '上传识别中...',
    //最低loading显示时间
    loadingtimes: 1000
  })
}


// 获取个人信息
export function get_userinfo(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 添加个人信息
export function update_userinfo(data) {
  return req({
    url: `${baserURLcard}/memberCard/WSMemCardOnline.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 首页新人领取优惠券
export function receive_newpeoplecoupon(data) {
  return req({
    url: `${baserURLcard}/Mywallet/Coupon.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '正在领取...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}




//**************************** */

// 佣金收益页面
export function get_myearn(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// //收益页面、收入报表
export function get_myearnreport(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 收益页面、我的收入明细
export function get_myearndetail(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 佣金提现页面信息
export function get_mycashout(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 我的推广会员
export function get_mymembers(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 我的推广会员搜索
export function get_mymembersearch(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: false,  //数据请求时是否立即打开loading
    hideLoading: false,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 我的推广会员搜索
export function get_mymemberinfo(data){
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 300  //最低loading显示时间,没有要求写0
  })
}
// 佣金提现历史
export function get_mycashoutlist(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
// 佣金提现页面申请提现
export function applycashout(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//获取邀请码
export function get_invitecode(data) {
  return req({
    url: `${baserURLcard}/MyWallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}

//获取vip信息
export function get_myviplevel(data) {
  return req({
    url: `${baserURLcard}/Mywallet/UserCenter.ashx`,
    data: data,
    method: 'POST',
    datatype: 'json',
    showLoading: true,  //数据请求时是否立即打开loading
    hideLoading: true,  //数据请求完成是否立即关闭loading
    loadingtext: '加载中...', //loading中显示的文字
    loadingtimes: 0  //最低loading显示时间,没有要求写0
  })
}
