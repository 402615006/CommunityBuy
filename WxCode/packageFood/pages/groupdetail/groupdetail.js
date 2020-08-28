// packageFood/pages/groupdetail/groupdetail.js
//会员卡notpwd==1不需要输密码，notpwdAmount小于它时不需要输密码
import {
  getcolagedetail,
  addmaincollageorder,
  addcollageorder,
  paycollagemoney,
  getmemcardslist,
  cancelorderPT
} from '../../utils/server.js';
import {
  verificationcardpwd
} from '../../../utils/server.js';
import {
  baserURLcard
} from "../../../utils/api.js"; //域名引入
var util = require('../../../utils/util.js');
import {
  is_gologin,
  wechat_payment
} from '../../../utils/util.js';

/* 裁剪封面，
src为本地图片路径或临时文件路径，
imgW为原图宽度，
imgH为原图高度，
cb为裁剪成功后的回调函数
*/
const clipImage = (src, imgW, imgH, cb) => {
  // ‘canvas’为前面创建的canvas标签的canvas-id属性值
  let ctx = wx.createCanvasContext('canvas');
  let canvasW = 640,
    canvasH = imgH;
  // 长宽比大于5:4
  if (imgW / imgH > 5 / 4) {
    canvasW = imgH * 5 / 4;
  }
  // 将图片绘制到画布
  ctx.drawImage(src, (imgW - canvasW) / 2, 0, canvasW, canvasH, 0, 0, canvasW, canvasH)
  // draw()必须要用到，并且需要在绘制成功后导出图片
  ctx.draw(false, () => {
    setTimeout(() => {
      // 导出图片
      wx.canvasToTempFilePath({
        width: canvasW,
        height: canvasH,
        destWidth: canvasW,
        destHeight: canvasH,
        canvasId: 'canvas',
        fileType: 'jpg',
        success: (res) => {
          // res.tempFilePath为导出的图片路径
          typeof cb == 'function' && cb(res.tempFilePath);
        }
      })
    }, 500);
  })
}

Page({

  /**
   * 页面的初始数据
   */
  data: {
    collcode: '', //拼团订单编号
    proinfo: '', //餐品信息
    disInfo: "", //菜品信息 
    dishescombo: [], //套餐标配（iscombo等于1时有数据）信息    
    dishesoptional: [], //套餐可选组合（iscombo等于1时有数据）
    baserURLcard: "", //请求地址
    bannerlist: [], //轮播图
    collorder: [], //已拼团信息
    newcollorder: [], //部分拼团信息
    currentData: "", //点击参团当前数据
    currentIndex: 0, //当前选中下标
    sign: false, //是否启用定时器
    iconNum: [], //菜品可参团人数
    btnIsTrue: false, //按钮显示
    cardsList: [
      {
        "levelname": "微信",
        "cardCode": "wx",
      }
    ], //会员卡列表
    cardIndex: 0, //选中卡下标
    paytype: 2, //支付方式（购买或开团）
    paymoney: "", //支付金额
    pkcode: "", //参团的code
    stocode: "", //门店编号
    successData: false,
    memcode: "", //会员卡编号
    coverImage:"",//分享图片
    defaultIndex: 0,//默认会员卡下标
  },
  go_box1(e) {
    let that = this;
    var currentData = e.currentTarget.dataset.list; //当前选中的拼团信息
    var index = e.currentTarget.dataset.index; //选中下标
    if (currentData.countDown == "00:00:00") {
      wx.showToast({
        title: "拼团时间已过，不可拼团！",
        mask: true,
        duration: 1500,
        icon: 'none'
      })
      return
    }
    that.setData({
      currentData: currentData,
      currentIndex: index
    })
    this.selectComponent("#box1").showlogin();

  },
  go_box2() {
    let that = this;
    this.selectComponent("#box2").showlogin();
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    var that = this;
    var collcode = options.collcode;
    //右上角移除分享功能
    wx.hideShareMenu();
    that.setData({
      collcode: collcode,
      baserURLcard: baserURLcard, //请求地址
    })
    var memcode = wx.getStorageSync("memcode");
    that.setData({
      memcode: memcode
    })
    //拼团明细
    that.getcolagedetail();
  },
  //获取会员卡信息
  getCardList() {
    var that = this;
    var memcode = wx.getStorageSync("memcode");
    // var memcode = 'M153713';
    if (!memcode) {
      return
    }
    var data = {
      "actionname": "getmemcardslistvalid",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "memcode": memcode,
        "stocode": that.data.stocode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getmemcardslist(data).then(res => {
      if (res.status == 0) {
        var cardsList = res.data;
        var defaultIndex = 0;
        for (var i = 0; i < cardsList.length; i++) {
          if (cardsList[i].isdef == 1) {
            defaultIndex = i;
          }
        }
        var weixin = {
          "levelname": "微信",
          "cardCode": "wx",
        }
        cardsList.push(weixin)
        that.setData({
          cardsList: cardsList,
          defaultIndex: defaultIndex,
        })
      }
    })
  },
  //拼团明细
  getcolagedetail() {
    var that = this;
    var data = {
      "actionname": "getcolagedetail",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "collcode": that.data.collcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getcolagedetail(data).then(res => {
      console.log(res)
      if (res.status == 0) {
        //proinfo:餐品信息
        var proinfo = res.collageinfo[0];
        var collnum = proinfo.collnum;
        var iconNum = []; //菜品可参团人数
        for (var k = 0; k < collnum; k++) {
          iconNum.push(k);
        }
        //disInfo: 菜品信息
        var disInfo = res.disInfo[0];
        //dishescombo:套餐标配（iscombo等于1时有数据）信息
        var dishescombo = res.dishescombo;
        //dishesoptional：套餐可选组合（iscombo等于1时有数据）
        var dishesoptional = res.dishesoptional;
        //已拼团信息
        var collorder = res.collorder;

        var bannerlist = [];
        var imgliat = proinfo.collimgpaths; //轮播图
        bannerlist = imgliat.split(",");
        var stoname = proinfo.stoname;
        wx.setNavigationBarTitle({ //设置页面标题
          title: stoname
        })
        that.setData({
          proinfo: proinfo,
          bannerlist: bannerlist,
          disInfo: disInfo,
          dishescombo: dishescombo,
          dishesoptional: dishesoptional,
          collorder: collorder,
          sign: true, //可以倒计时
          iconNum: iconNum,
          stocode: proinfo.stocode,
          successData: true,
        })
        //获取会员卡信息
        // that.getCardList();
        //倒计时
        that.timeDown(collorder);
        //图片处理
        that.pictureProcessing();
      } else {
        wx.showToast({
          title: res.mes,
          mask: true,
          duration: 1500,
          icon: 'none'
        })
      }
    })
  },
  //倒计时
  timeDown(collorder) {
    var that = this;
    // var collorder = that.data.collorder;
    var newcollorder = [];
    for (var j = 0; j < collorder.length; j++) {
      collorder[j].countDown = util.addTime(collorder[j].ctime, that.data.proinfo.collInt)
    }
    if (collorder.length > 3) {
      for (var i = 0; i < 3; i++) {
        newcollorder.push(collorder[i])
      }
    } else {
      newcollorder = collorder;
    }
    that.setData({
      collorder: collorder,
      newcollorder: newcollorder,
    })
    var sign = that.data.sign;
    if (sign) {
      //延迟一秒执行自己
      setTimeout(function() {
        that.timeDown(that.data.collorder);
      }, 1000)
    }
  },

  //单独购买
  tobuy() {
    let that = this;
    if (!is_gologin()){
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    //弹框选择会员卡
    that.selectComponent("#CardsSelect").showlogin();
    that.setData({
      btnIsTrue: true,
      paytype: 1,
      paymoney: that.data.proinfo.costprice
    })
  },

  //开团
  kaituan() {
    let that = this;
    if (!is_gologin()){
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    //弹框选择会员卡
    that.selectComponent("#CardsSelect").showlogin();
    that.setData({
      btnIsTrue: true,
      paytype: 2,
      paymoney: that.data.proinfo.collprice
    })
  },

  //确认支付
  //会员卡notpwd== 1不需要输密码，notpwdAmount小于它时不需要输密码
  zhifu() {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var notpwdAmount = parseFloat(currentCard.notpwdAmount); //免密金额
    var price = parseFloat(that.data.price);
    if (currentCard.cardCode != "wx") {
      if (currentCard.notpwd == 1 || price <= notpwdAmount) {
        var paytype = that.data.paytype;
        if (paytype == 3) {
          var pkcode = that.data.pkcode;
          that.addcollageorder(pkcode);
        } else {
          var paymoney = that.data.paymoney;
          that.addorder(paytype, paymoney);
        }
      } else {
        this.selectComponent("#password").showModal();
      }
    } else {
      var paytype = that.data.paytype;
      if (paytype == 3) {
        var pkcode = that.data.pkcode;
        that.addcollageorder(pkcode);
      } else {
        var paymoney = that.data.paymoney;
        that.addorder(paytype, paymoney);
      }
    }
  },

  // 密码输入完成判断是否正确
  get_number_ok(e) {
    let that = this;
    let pwd = e.detail;
    let unionid = wx.getStorageSync('unionid');
    let data = {
      "actionname": "pwdcheck",
      "parameters": {
        'GUID': '88888888',
        'unionid': unionid,
        'pwd': pwd
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    verificationcardpwd(data).then(res => {
      if (res.status == 0) {
        var paytype = that.data.paytype;
        if (paytype == 3) {
          var pkcode = that.data.pkcode;
          that.addcollageorder(pkcode);
        } else {
          var paymoney = that.data.paymoney;
          that.addorder(paytype, paymoney);
        }
      } else {
        wx.showToast({
          title: res.mes,
          icon: 'none',
          mask: true,
          duration: 1500
        })
      }
    })
  },

  //开团或单独购买
  addorder(status, money) {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var cardcode = currentCard.cardCode; //会员卡卡号
    var balance = currentCard.balance; //余额
    if (currentCard.cardCode != "wx") {
      if (parseFloat(balance) == 0) {
        wx.showToast({
          title: "卡内余额为0，请充值后再付！",
          mask: true,
          duration: 1500,
          icon: 'none'
        })
        return
      }
    }

    var memcode = wx.getStorageSync("memcode");
    // var memcode = 'M153713';
    var data = {
      "actionname": "addmaincollageorder",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "collcode": that.data.collcode,
        "stocode": that.data.proinfo.stocode,
        "colltype": status,
        "collPeople": memcode,
        "collprice": money
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    addmaincollageorder(data).then(res => {
      console.log(res)
      if (res.status == 0) {
        var mainorderno = res.data[0].mainorderno; //主订单号
        var orderno = res.data[0].orderno; //副订单号
        //支付
        that.paycollagemoney(mainorderno, orderno, money);
      } else {
        wx.showToast({
          title: res.mes,
          mask: true,
          duration: 1500,
          icon: 'none'
        })
      }
    })
  },
  //参团
  cantuan(e) {
    var that = this;
    if (!is_gologin()){
      // 没有登录
      that.selectComponent("#box1").hidelogin();
      that.selectComponent("#box2").hidelogin();
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    var collDifNum = e.detail.CollDifNum;
    var collPeople = e.detail.CollPeople;
    var pkcode = e.detail.PKCode;
    var memcode = wx.getStorageSync("memcode");
    if (collPeople == memcode) {
      wx.showToast({
        title: "不可重复参团",
        mask: true,
        duration: 1500,
        icon: 'none'
      })
      return
    }
    that.selectComponent("#box1").hidelogin();
    that.selectComponent("#box2").hidelogin();
    //弹框选择会员卡
    that.selectComponent("#CardsSelect").showlogin();
    that.setData({
      btnIsTrue: true,
      paytype: 3,
      pkcode: pkcode,
      paymoney: that.data.proinfo.collprice
    })
  },

  //去参团
  addcollageorder(pkcode) {
    var that = this;
    var memcode = wx.getStorageSync("memcode");
    // var memcode = 'M153713';
    var money = that.data.proinfo.collprice;
    var data = {
      "actionname": "addcollageorder",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "stocode": that.data.proinfo.stocode,
        "pkcode": pkcode,
        "collPeople": memcode,
        "collprice": money
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    addcollageorder(data).then(res => {

      if (res.status == 0) {
        var mainorderno = res.data[0].mainorderno; // 主订单号
        var orderno = res.data[0].orderno; //副订单号
        //支付
        that.paycollagemoney(mainorderno, orderno, money);
      } else {
        wx.showToast({
          title: res.mes,
          mask: true,
          duration: 1500,
          icon: 'none'
        })
      }
    })
  },
  //支付
  paycollagemoney(mainorderno, orderno, money) {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var zfpaytype = 2; //1微信支付 2会员卡支付
    var cardcode = currentCard.cardCode; //会员卡卡号
    var balance = currentCard.balance; //余额
    var price = money; //订单金额
    var payMoney = 0; //实际支付金额

    if (parseFloat(price) > parseFloat(balance)) {
      wx.showToast({
        title: "卡内余额不足，请充值后再付！",
        mask: true,
        duration: 1500,
        icon: 'none'
      })
      return
    }
    if (currentCard.cardCode == "wx") {
      zfpaytype = 1;
      cardcode = "";
    }
    var memcode = wx.getStorageSync("memcode");
    var openid = wx.getStorageSync("openid");
    // var memcode = 'M153713';
    var data = {
      "actionname": "paycollagemoney",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        'openid': openid,
        "mainorderno": mainorderno,
        "orderno": orderno,
        "paytype": zfpaytype,
        "paymoney": price,
        "cardcode": cardcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    paycollagemoney(data).then(res => {
      if (zfpaytype == 1) { //微信支付   
        wechat_payment(res.timeStamp, res.nonceStr, res.package, res.signType, res.paySign).then(res => {
          //拼团明细
          that.getcolagedetail();
          wx.showToast({
            title: res.mes,
            mask: true,
            duration: 1500,
            icon: 'success'
          })
          var paytype = that.data.paytype;
          if (paytype == 1) {
            setTimeout(function() {
              wx.navigateTo({
                url: '/packageFood/pages/grorderdetail/grorderdetail?pkcode=' + mainorderno
              })
            }, 1000)
          } else {
            setTimeout(function() {
              wx.navigateTo({
                url: '/packageFood/pages/successful/successful?pkcode=' + mainorderno,
              })
            }, 1000)
          }
        }).catch(err => {
          that.cancelOrder(mainorderno);
          wx.showToast({
            icon: "none",
            title: '支付失败',
            duration: 2000,
          })        
        })
      } else { //会员卡支付
        if (res.status == 0) {
          var paytype = that.data.paytype;
          if (paytype == 2 || paytype == 3) {
            that.selectComponent("#CardsSelect").hidelogin();
            //拼团明细
            that.getcolagedetail();
            wx.showToast({
              title: res.mes,
              mask: true,
              duration: 1500,
              icon: 'success'
            })
            setTimeout(function() {
              wx.navigateTo({
                url: '/packageFood/pages/successful/successful?pkcode=' + mainorderno,
              })
            }, 1000)
          } else {
            that.selectComponent("#CardsSelect").hidelogin();
            //拼团明细
            that.getcolagedetail();
            wx.showToast({
              title: res.mes,
              mask: true,
              duration: 1500,
              icon: 'success'
            })
            setTimeout(function() {
              wx.navigateTo({
                url: '/packageFood/pages/grorderdetail/grorderdetail?pkcode=' + mainorderno
              })
            }, 1000)
          }
        } else {
          that.cancelOrder(mainorderno);
          wx.showToast({
            title: res.mes,
            mask: true,
            duration: 1500,
            icon: 'none'
          })         
        }
      }
    })
  },

  //取消订单
  cancelOrder(pkcode) {
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "cancelorder",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "memcode": memcode,
        "pkcode": pkcode,
      }
    }
    console.log(data)
    data.parameters = JSON.stringify(data.parameters);
    cancelorderPT(data).then(res => {

    })
  },
  //获取选中卡信息
  setCardcode(e) {
    var that = this;
    var cardIndex = e.detail;
    //获取卡下标
    that.setData({
      cardIndex: cardIndex
    })
  },
  // 登陆成功后的事件
  onMyEvent() {
    var that = this;
    //获取账单，会员卡
    that.getcolagedetail();
    var memcode = wx.getStorageSync("memcode");
    that.setData({
      memcode: memcode
    })
  },

  //点击阴影取消选择支付方式
  unclickCard() {
    var that = this;
    that.setData({
      btnIsTrue: false,
    })
  },
  //图片处理
  pictureProcessing() {
    var that = this;
    wx.downloadFile({
      url: that.data.baserURLcard + that.data.bannerlist[0],
      success(res) {
        // 只要服务器有响应数据，就会把响应内容写入文件并进入 success 回调，业务需要自行判断是否下载到了想要的内容
        if (res.statusCode === 200) {
          wx.getImageInfo({
            src: res.tempFilePath, // 这里填写网络图片路径
            success: (res) => {
              // 封装的裁剪图片方法
              clipImage(res.path, res.width, res.height, (img) => {
                // img为最终裁剪后生成的图片路径，我们可以用来做为转发封面图
                that.setData({
                  coverImage: img
                })
              });
            }
          })
        }
      }
    })
  },


  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function() {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function() {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function() {
    var that = this;
    //不可用倒计时
    that.setData({
      sign: false
    })
  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {

    var that = this;
    wx.stopPullDownRefresh();
    //拼团明细
    that.getcolagedetail();
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function(res) {
    var that = this;
    
    if (res.from === 'button') {
      // 来自页面内转发按钮
      return {
        title: that.data.proinfo.collname,
        path: '/packageFood/pages/groupdetail/groupdetail?collcode=' + that.data.collcode,
        imageUrl: that.data.coverImage,
        success: (res) => {
          // 成功后要做的事情
          wx.showToast({
            title: "分享成功",
            icon: 'none',
            duration: 2000
          })
        },
        fail: function(res) {
          // 分享失败
          wx.showToast({
            title: "分享失败",
            icon: 'none',
            duration: 2000
          })
        }
      }
    }
  }
})