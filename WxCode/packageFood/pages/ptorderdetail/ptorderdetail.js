// packageFood/pages/ptorderdetail/ptorderdetail.js
import {
  getTJorderDetail,
  getmemcardslist,
  addorder,
  payorderbypcode,
  cancelorderTJ
} from '../../utils/server.js';
import {
  verificationcardpwd
} from '../../../utils/server.js';
import {
  baserURLcard
} from "../../../utils/api.js"; //域名引入
import {
  is_gologin,
  wechat_payment
} from '../../../utils/util.js';
Page({
  /**
   * 页面的初始数据
   */
  data: {
    pkcode:"",
    tstatus:"",//1已付款才会显示退款按钮
    proorder: '', //订单支付信息
    disInfo: "", //菜品信息 
    orderinfo: "", //订单信息 
    dishescombo: [], //必选套餐  
    dishesoptional: [], //套餐可选组合（iscombo等于1时有数据）
    bannerlist: [], //轮播图
    successData:false,
    cardsList: [], //会员卡
    btnIsTrue: false, //确认支付按钮
    cardIndex: 0, //选中卡下标
    cardsList: [], //会员卡列表
    defaultIndex: 0,//默认会员卡下标
    checkinfos:[],//券码集合
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    let that = this;
    that.setData({
      pkcode: options.pkcode,   
      baserURLcard: baserURLcard, //请求地址
    })
    that.getorderdetail();
  },
  //订单详情
  getorderdetail() {
    var that = this;
    var memcode = wx.getStorageSync("memcode");
    var data = {
      "actionname": "getorderdetail",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "memcode": memcode,
        "ordercode": that.data.pkcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getTJorderDetail(data).then(res => {
      console.log(res)
      if (res.status == 0) {
        var proorder = res.proorder[0]; //订单支付信息    
        var disInfo = res.disInfo[0]; // 菜品信息
        var orderinfo = res.orderinfo[0]; //订单信息
        var dishescombo = res.dishescombo; //必选套餐
        var dishesoptional = res.dishesoptional; //可选套餐
        var checkinfos = res.checkinfos;//券码

        var bannerlist = [];
        var imglist = orderinfo.proimgpath; //轮播图
        bannerlist = imglist.split(",");
        for (var i = 0; i < bannerlist.length; i++) {
          bannerlist[i] = that.data.baserURLcard + bannerlist[i];
        }
        that.setData({
          proorder: proorder,
          bannerlist: bannerlist,
          disInfo: disInfo,
          orderinfo: orderinfo,
          dishescombo: dishescombo,
          dishesoptional:dishesoptional,
          successData: true,
          tstatus: orderinfo.tstatus, 
          checkinfos:checkinfos, 
          // tstatus: 0, 
        })
      }
    })
  },
  //获取会员卡信息
  getCardList() {
    var that = this;
    var memcode = wx.getStorageSync("memcode");
    // var memcode = 'M153713';
    var stocode = that.data.orderinfo.stocode;
    var data = {
      "actionname": "getmemcardslistvalid",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "memcode": memcode,
        "stocode": stocode,
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
          btnIsTrue: true,
          defaultIndex: defaultIndex,
        })
        //弹框选择会员卡
        that.selectComponent("#CardsSelect").showlogin();
      }
    })
  },
  //会员卡notpwd== 1不需要输密码，notpwdAmount小于它时不需要输密码
  topay() {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var notpwdAmount = parseFloat(currentCard.notpwdAmount); //免密金额
    var price = parseFloat(that.data.orderinfo.pmoney);
    if (currentCard.cardCode != "wx") {
      if (currentCard.notpwd == 1 || price <= notpwdAmount) {
        that.getOrder();
      } else {
        this.selectComponent("#password").showModal();
      }
    } else {
      that.getOrder();
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
        that.getOrder();
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

  //获取单号
  getOrder() {
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
    that.selectComponent("#CardsSelect").hidelogin(); //隐藏卡列表
    var memcode = wx.getStorageSync("memcode");
    var disInfo = that.data.disInfo; //菜品信息
    var orderinfo = that.data.orderinfo;
    var orderstr = orderinfo.stocode + "," + disInfo.discode + "," + disInfo.disname + "," + 1 + "," + orderinfo.pmoney + ";"
    var data = {
      "actionname": "addorder",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "usercode": memcode,
        "orderstr": orderstr,
        "tpcode": orderinfo.pkcode,
        "pkcode": that.data.proorder.PKCode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    addorder(data).then(res => {
      if (res.status == 0) {
        var mainorderno = res.data[0].mainorderno; //主订单号
        var orderno = res.data[0].orderno; //副订单号
        //支付
        that.payorderbypcode(mainorderno, orderno);
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
  payorderbypcode(mainorderno, orderno) {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var paytype = 2; //1微信支付 2会员卡支付
    var cardcode = currentCard.cardCode; //会员卡卡号
    var balance = currentCard.balance; //余额
    var price = that.data.orderinfo.pmoney; //订单金额

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
      paytype = 1;
      cardcode = "";
    }
    var memcode = wx.getStorageSync("memcode");
    var openid = wx.getStorageSync("openid");
    var data = {
      "actionname": "payorderbypcode",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        'openid': openid,
        "mainorderno": mainorderno,
        "orderno": orderno,
        "paytype": paytype,
        "paymoney": price,
        "cardcode": cardcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    payorderbypcode(data).then(res => {
      if (paytype == 1) {//微信支付     
        wechat_payment(res.timeStamp, res.nonceStr, res.package, res.signType, res.paySign).then(res => {
          wx.showToast({
            title: res.mes,
            mask: true,
            duration: 1500,
            icon: 'success'
          })
          setTimeout(function () {
            // wx.switchTab({
            //   url: '/pages/wallet/wallet',
            // })
            wx.navigateBack()
          }, 1000)
        }).catch(err => {
          wx.showToast({
            icon: "none",
            title: '支付失败',
            duration: 2000,
          })
        })
      } else {//会员卡支付
        if (res.status == 0) {
          wx.showToast({
            title: res.mes,
            mask: true,
            duration: 1500,
            icon: 'success'
          })
          setTimeout(function () {
            // wx.switchTab({
            //   url: '/pages/wallet/wallet',
            // })
            wx.navigateBack()
          }, 1000)
        } else {
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
  cancelorder(){
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "cancelorder",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "memcode": memcode,
        "pkcode": that.data.proorder.PKCode,
      }
    }
    console.log(data)
    data.parameters = JSON.stringify(data.parameters);
    cancelorderTJ(data).then(res => {
      console.log(res);
      if (res.status == 0) {
        wx.showToast({
          title: res.mes,
          mask: true,
          duration: 1500,
          icon: 'none'
        })
        setTimeout(function () {
          // wx.switchTab({
          //   url: '/pages/wallet/wallet',
          // })
          wx.navigateBack()
        }, 1000)
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

  // 登陆成功后的事件
  onMyEvent() {
    var that = this;
    //获取订单信息
    that.getorderdetail();
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

  //点击阴影取消选择支付方式
  unclickCard() {
    var that = this;
    that.setData({
      btnIsTrue: false,
    })
  },
  //点击顶上菜品跳到详情页
  dishDetail() {
    var that = this;
    var pkcode = that.data.orderinfo.pkcode;
    wx.navigateTo({
      url: '/packageFood/pages/packagedetail/packagedetail?pkcode=' + pkcode,
    })
  },

  //轮播图点击预览
  imgYu(event) {
    var that = this;
    var src = event.currentTarget.dataset.src; //获取data-src
    var imgList = that.data.bannerlist; //获取data-list
    //图片预览
    wx.previewImage({
      current: src, // 当前显示图片的http链接
      urls: imgList // 需要预览的图片http链接列表
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

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {
    var that = this;
    wx.stopPullDownRefresh();
    //订单详情
    that.getorderdetail();
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {
    
  },
  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function() {

  }
})