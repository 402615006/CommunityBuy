/*
 **********************************************************************************************************************
 * 兼容ios  IOS只识别2018/01/01这种格式
 * .replace(/-/g, '/');  // 日期格式处理
 * *******************************************************************************************************************
 */
var app = getApp();

var { baseURL } = require('./api.js')




function is_gologin() {
  var memcode = wx.getStorageSync('memcode');
  let openid = wx.getStorageSync('openid');
  let mobile = wx.getStorageSync('mobile');
  let unionid = wx.getStorageSync('unionid');
  if (memcode && openid && mobile && unionid) {
    return true
  } else {
    return false
  }
}

// rpx转换成px
function get_px(value) {
  var systemInfo = wx.getSystemInfoSync();
  return value / 750 * systemInfo.windowWidth;
}

//根据'2018-09-26 09:16:15' 获取？月？日
function get_date(d) {
  var s = d.replace(/-/g, "/");
  s = new Date(s).getTime();
  var date = new Date(s);
  var y = date.getFullYear();
  var m = date.getMonth() + 1;
  var d = date.getDate();
  m = m < 10 ? ('0' + m) : m;
  d = d < 10 ? ('0' + d) : d;
  return m + '月' + d + '日';
}
//根据'2018-09-26 09:16:15' 获取？小时？分
function get_date1(d) {
  var s = d.replace(/-/g, "/");

  s = new Date(s).getTime();
  var date = new Date(s);

  var h = date.getHours();
  h = h < 10 ? ('0' + h) : h;
  var minute = date.getMinutes();
  minute = minute < 10 ? ('0' + minute) : minute;
  var second = date.getSeconds();

  return h + ':' + minute;
}

function get_tim(d) {
  var s = d;
  s = s.split(' ');
  return s[1];
}

//根据日期2018-09-26 判断周几
function calcWeek(dt) {
  var da = (dt.replace(/-/g, "/"))   // 把2018-09-26 转换为2018/09/26  
  var da = new Date(da)   //接收的格式da为2018/09/26 
  var weekday = new Array(7);
  weekday[0] = "日"
  weekday[1] = "一"
  weekday[2] = "二"
  weekday[3] = "三"
  weekday[4] = "四"
  weekday[5] = "五"
  weekday[6] = "六"
  return "周" + weekday[da.getDay()]
}

//根据已知的时间，判断是否是今天，明天，后天
//时间格式 2018-09-26 09:16:15
function getDayName(d) {
  var td = new Date();
  td = new Date(td.getFullYear(), td.getMonth(), td.getDate());
  var od = new Date(d.replace(/-/g, "/"));
  od = new Date(od.getFullYear(), od.getMonth(), od.getDate());
  var xc = (od - td) / 1000 / 60 / 60 / 24;
  if (xc < -2) {
    return -xc + "天前";
  } else if (xc < -1) {
    return "前天";
  } else if (xc < 0) {
    return "昨天";
  } else if (xc == 0) {
    return "今天";
  } else if (xc < 2) {
    return "明天";
  } else if (xc < 3) {
    return "后天";
  } else {
    return xc + "天后";
  }
}


function EarlyDate(d) {
  var time = new Date(d.replace(/-/g, "/"));
  var day = time.getHours();
  if (day < 6) {
    return '凌晨';
  } else if (day < 12) {
    return '早上';
  } else if (day < 18) {
    return '下午';
  } else if (day < 24) {
    return '晚上';
  }
}

/*
 * 时间戳转换为yyyy-MM-dd hh:mm:ss 格式  formatDate()
 * inputTime   时间戳
 */
function formatDate(inputTime) {
  console.log(inputTime)
  let t = inputTime;
  t = new Date(t).getTime() / 1000
  console.log(t)
  // 计算年月日
  // var n = inputTime * 1000;
  // var date = new Date(n);
  // var Y = date.getFullYear() + '/';
  // var M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '/';
  // var D = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
  // var h = date.getHours();
  // h = h < 10 ? ('0' + h) : h;
  // var minute = date.getMinutes();
  // var second = date.getSeconds();
  // minute = minute < 10 ? ('0' + minute) : minute;
  // second = second < 10 ? ('0' + second) : second;
  // return (Y + M + D + ' ' + h +':' +minute + ':' + second)


  //计算几天前，几小时前
  var d_minutes, d_hours, d_days;
  var timeNow = parseInt(new Date().getTime() / 1000);
  var d;
  d = timeNow - t;
  d_days = parseInt(d / 86400);
  d_hours = parseInt(d / 3600);
  d_minutes = parseInt(d / 60);
  if (d_days > 0 && d_days < 4) {
    return d_days + "天前";
  } else if (d_days <= 0 && d_hours > 0) {
    return d_hours + "小时前";
  } else if (d_hours <= 0 && d_minutes > 0) {
    return d_minutes + "分钟前";
  } else {
    var date = new Date(t * 1000);
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    m = m < 10 ? ('0' + m) : m;
    d = d < 10 ? ('0' + d) : d;
    return y + '-' + m + '-' + d;
  }

};
//计算时间还有几天或几小时或几分钟
function fdate(e) {
  let t = e;
  t = new Date(t).getTime() / 1000;
  var d_minutes, d_hours, d_days;
  var timeNow = parseInt(new Date().getTime() / 1000);
  var d;
  d = t - timeNow;
  d_days = parseInt(d / 86400);
  d_hours = parseInt(d / 3600);
  d_minutes = parseInt(d / 60);
  if (d_days > 0) {
    return d_days + "天";
  } else if (d_days <= 0 && d_hours > 0) {
    return d_hours + "小时";
  } else if (d_hours <= 0 && d_minutes > 0) {
    return d_minutes + "分钟";
  } else {
    return 0
  }
}

//计算两点位置距离
function getDistance(la1, lo1, la2, lo2) {
  var La1 = la1 * Math.PI / 180.0;
  var La2 = la2 * Math.PI / 180.0;
  var La3 = La1 - La2;
  var Lb3 = lo1 * Math.PI / 180.0 - lo2 * Math.PI / 180.0;
  var s = 2 * Math.asin(Math.sqrt(Math.pow(Math.sin(La3 / 2), 2) + Math.cos(La1) * Math.cos(La2) * Math.pow(Math.sin(Lb3 / 2), 2)));
  s = s * 6378.137; //地球半径
  s = Math.round(s * 10000) / 10000;
  return parseFloat(s.toFixed(2))
  // console.log("计算结果",s)
};
//随机函数的产生：
function createNonceStr() {
  return Math.random().toString(36).substr(2, 15)
};
// /* 随机数 */
// function randomString() {
//   var chars = 'ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678';    /****默认去掉了容易混淆的字符oOLl,9gq,Vv,Uu,I1****/
//   var maxPos = chars.length;
//   var pwd = '';
//   for (var i = 0; i < 32; i++) {
//     pwd += chars.charAt(Math.floor(Math.random() * maxPos));
//   }
//   return pwd;
// }
//时间戳产生的函数：

function createTimeStamp() {
  return parseInt(new Date().getTime() / 1000) + ''
};
//随机整数
// 多个不重复
function random(num, min, max) {
  // return Math.floor(Math.random() * (max - min)) + min;
  var arr = [];
  for (var i = 0; i < num; i++) {
    arr[i] = parseInt(Math.random() * (max - min + 1) + min);
    for (var j = 0; j < i; j++) {
      if (arr[i] == arr[j]) {
        i = i - 1;
        break;
      }
    }
  }
  return arr;
};
//随机整数
// 单个
function random_one(min, max) {
  return Math.floor(Math.random() * (max - min)) + min;
}

function fun(keyArr, valArr) {
  var data = {};
  for (var i = 0; i < keyArr.length; i++) {
    var key = keyArr[i];
    var val = valArr[i];
    data[key] = val;
  }
  return data;
}
// 整数相减求正数
function prev(x, y) {
  if (x - y >= 0) {
    return x - y
  } else {
    return 0
  }

}



//调取通知
function sendmsg(x, y) {
  let openid = x;
  let orderno = y;
  let formid = '';
  console.log(openid);
  console.log(orderno)
  console.log(baseURL)
  console.log("执行推送")
  var data = {
    "actionname": "moviebegintips",
    "parameters": "{'openid':'" + openid + "','orderno':'" + orderno + "','formid':'" + formid + "'}"
  }
  console.log(data)
  wx.request({
    url: `${baseURL}/MPPushMsg.ashx`,
    data: data,
    methods: 'POST',
    dataType: 'json',
    success: function(res) {
      console.log(res.data)
    },
    fail: function(res) {

    },
    complete: function(res) {

    },
  })
}

//改签调取通知
function changesuccess(x, y) {
  let openid = x;
  let orderno = y;
  let formid = '';
  console.log(openid);
  console.log(orderno)
  console.log(baseURL)
  console.log("改签执行推送")
  var data = {
    "actionname": "changesuccess",
    "parameters": "{'openid':'" + openid + "','orderno':'" + orderno + "','formid':'" + formid + "'}"
  }
  console.log(data)
  wx.request({
    url: `${baseURL}/MPPushMsg.ashx`,
    data: data,
    methods: 'POST',
    dataType: 'json',
    success: function(res) {
      console.log(res.data)
    },
    fail: function(res) {

    },
    complete: function(res) {

    },
  })
}


// 进入小程序记录
function enterAct(x, y) {
  let openid = x;
  let orderno = y;
  let formid = '';
  var data = {
    "actionname": "enterAct",
    "parameters": "{'GUID':'','USER_ID':'1','memcode':'" + formid + "','location':'" + formid + "','ctime':'" + formid + "','actid':'" + formid + "'}"
  }
  console.log(data)
  wx.request({
    url: `${baseURL}/CollectData.ashx`,
    data: data,
    methods: 'POST',
    dataType: 'json',
    success: function(res) {
      console.log(res.data)
    },
    fail: function(res) {

    },
    complete: function(res) {

    },
  })
}
// 进入小程序页面记录
function enterPageAct(x, y) {
  let openid = x;
  let orderno = y;
  let formid = '';
  var data = {
    "actionname": "enterPageAct",
    "parameters": "{'GUID':'','USER_ID':'1','pagename':'" + formid + "','otime':'" + formid + "','ctime':'" + formid + "','actid':'" + formid + "','acttype':'" + formid + "'}"
  }
  console.log(data)
  wx.request({
    url: `${baseURL}/CollectData.ashx`,
    data: data,
    methods: 'POST',
    dataType: 'json',
    success: function(res) {
      console.log(res.data)
    },
    fail: function(res) {

    },
    complete: function(res) {

    },
  })
}

// 获取当前时间戳
function get_formatDate() {
  let date = new Date();
  var Y = date.getFullYear() + '-';
  var M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
  var D = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
  var h = date.getHours();
  h = h < 10 ? ('0' + h) : h;
  var minute = date.getMinutes();
  var second = date.getSeconds();
  minute = minute < 10 ? ('0' + minute) : minute;
  second = second < 10 ? ('0' + second) : second;
  return (Y + M + D + ' ' + h + ':' + minute + ':' + second)
}

// 调起微信弹框
function wechat_payment(timeStamp1, nonceStr1, package1, signType1, paySign1) {
  return new Promise((resolve, reject) => {
    wx.requestPayment({
      timeStamp: timeStamp1,
      nonceStr: nonceStr1,
      package: package1,
      signType: signType1,
      paySign: paySign1,
      success: function(res) {
        resolve(res)
      },
      fail: function(res) {
        reject(res)
      }
    })
  })
}

//拼团时间加固定小时，显示倒计时
function addTime(x, y) {

  var a = new Date(x.replace(/-/g, '/'));
  var q = a.setHours(a.getHours() + parseFloat(y));
  var z = new Date();
  var s = z.getTime();
  var f = q - s;
  if (f > 0) {
    var t = formatDuring(f)
    return t
  } else {
    return "00:00:00"
  }
}

function formatDuring(mss) {
  var days = parseInt(mss / (1000 * 60 * 60 * 24));
  var hours = addZero(parseInt((mss % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)));
  var minutes = addZero(parseInt((mss % (1000 * 60 * 60)) / (1000 * 60)));
  var seconds = addZero(parseInt((mss % (1000 * 60)) / 1000));
  if (days > 0) {
    return days + "天" + hours + ":" + minutes + ":" + seconds;
  } else {
    return hours + ":" + minutes + ":" + seconds;
  }
}

function addZero(num) {
  if (num < 10)
    return "0" + num;
  return num;
}
// 获取购物车商品
function get_shopcardlist(stocode){
  if (wx.getStorageSync('ShoppCord')){
    let ShoppCord=wx.getStorageSync('ShoppCord');
    let flag=false;
    for (var i = 0; i < ShoppCord.length;i++){
      if (ShoppCord[i].stocode==stocode){
        flag=true;
        return ShoppCord[i].shopcardlist;
        break;
      }
    }
    if (flag==false){
      console.log('执行空')
      return []
    }
  }else{
    return []
  }
}
// 购物车存储商品
function add_shopcard(stocode,data){
  console.log(stocode)
  console.log(data)
  if (wx.getStorageSync('ShoppCord')){
    let ShoppCord = wx.getStorageSync('ShoppCord');
    let flag = false;
    for (var i = 0; i < ShoppCord.length;i++){
      if (ShoppCord[i].stocode == stocode) {
        flag = true;
        ShoppCord[i].shopcardlist=data;
        break;
      }
    }
    if (flag==false){
      ShoppCord.push({
        'stocode': stocode,
        'shopcardlist': data
      })
    }
    wx.setStorageSync('ShoppCord', ShoppCord);
  }else{
    let ShoppCord=[];
    ShoppCord.push({
      'stocode': stocode,
      'shopcardlist': data
    })
    wx.setStorageSync('ShoppCord', ShoppCord);
  }
}


//截取链接中的参数
function getUrlParam(name,search) {
  var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
  var r = search.substr(1).match(reg);  //匹配目标参数
  console.log('55');
  console.log(r)
  if (r != null) {
    return unescape(decodeURI(r[2]));
  }
  return "";
}


module.exports = {
  formatDate: formatDate,
  getDistance: getDistance,
  createNonceStr: createNonceStr,
  createTimeStamp: createTimeStamp,
  random: random,
  get_date: get_date,
  get_date1: get_date1,
  calcWeek: calcWeek,
  getDayName: getDayName,
  EarlyDate: EarlyDate,
  get_tim: get_tim,
  random_one: random_one,
  fun: fun,
  fdate: fdate,
  sendmsg: sendmsg,
  changesuccess: changesuccess,
  prev: prev,
  is_gologin: is_gologin,
  get_formatDate: get_formatDate,
  wechat_payment: wechat_payment,
  get_px: get_px,
  addTime: addTime,
  add_shopcard: add_shopcard,
  get_shopcardlist: get_shopcardlist,
  getUrlParam: getUrlParam
}