
import { req, upf } from "../../utils/req.js";   // 请求接口数据公共方法引入
import { baseURL, baseURLwx } from "../../utils/api.js";    //域名引入

//************************************************************************************

//抽奖活动数据
export function get_hdlist(data) {
  return req({
    url: `${baseURL}/draw.ashx`,
    data: {
      "actionname": "getdrawinfo",
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

//抽奖剩余次数
export function get_number(data) {
  return req({
    url: `${baseURL}/draw.ashx`,
    data: {
      "actionname": "getmytimes",
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

//抽奖结果
export function set_lottery(data) {
  return req({
    url: `${baseURL}/draw.ashx`,
    data: {
      "actionname": "getdrawresult",
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
//我的奖品
export function Mylottery(data) {
  return req({
    url: `${baseURL}/draw.ashx`,
    data: {
      "actionname": "getmyprizes",
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