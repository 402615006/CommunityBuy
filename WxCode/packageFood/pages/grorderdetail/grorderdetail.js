// packageFood/pages/grorderdetail/grorderdetail.js
import {
  getPTorderDetail,
  getmemcardslist,
  addcollageorder,
  paycollagemoney,
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
    pkcode: "",
    tstatus: "", //支付状态（0：待支付 1：支付成功 2：取消支付 3：退款中 4：已退款 5 部分支付）
    tcstatus: "", //拼团状态 待拼团 拼团中 拼团成功 拼团退款中 拼团已退款  取消(0, 1, 2, 3, 4, 5)
    colorder: '', //订单支付信息
    disInfo: "", //菜品信息 
    collorderinfo: "", //订单信息 
    dishescombo: [], //必选套餐  
    dishesoptional: [], //套餐可选组合（iscombo等于1时有数据）
    bannerlist: [], //轮播图
    colpeople: [], //拼团人信息
    successData: false,
    sign: false, //是否启用定时器
    cardsList: [], //会员卡
    btnIsTrue: false, //确认支付按钮
    cardIndex: 0, //选中卡下标
    cardsList: [], //会员卡列表
    coverImage: "", //分享图片
    defaultIndex: 0, //默认会员卡下标
    buySeparately: false, //是否单独购买
    checkinfos:[],//券码集合  
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    let that = this;
    //右上角移除分享功能
    wx.hideShareMenu();
    that.setData({
      pkcode: options.pkcode,
      baserURLcard: baserURLcard, //请求地址
    })
    if (is_gologin()) {
      that.getorderdetail();
    }
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
    getPTorderDetail(data).then(res => {
      if (res.status == 0) {
        var colorder = res.colorder[0]; //订单支付信息    
        var disInfo = res.disInfo[0]; // 菜品信息
        var collorderinfo = res.collorderinfo[0]; //订单信息
        var dishescombo = res.dishescombo; //必选套餐
        var dishesoptional = res.dishesoptional; //可选套餐
        var colpeople = res.colpeople; //拼团人信息
        var checkinfos = res.checkinfos;

        var bannerlist = [];
        var imglist = collorderinfo.collimgpaths; //轮播图
        bannerlist = imglist.split(",");
        for (var i = 0; i < bannerlist.length; i++) {
          bannerlist[i] = that.data.baserURLcard + bannerlist[i];
        }
        var buySeparately = false;
        if (collorderinfo.colldifnum == 0 && colpeople.length == 1) {
          buySeparately = true;
        }
        that.setData({
          colorder: colorder,
          bannerlist: bannerlist,
          disInfo: disInfo,
          collorderinfo: collorderinfo,
          dishescombo: dishescombo,
          dishesoptional: dishesoptional,
          colpeople: colpeople,
          tstatus: collorderinfo.tstatus,
          tcstatus: collorderinfo.tcstatus,
          successData: true,
          sign: true,
          buySeparately: buySeparately,
          checkinfos: checkinfos,
        })
        //倒计时
        that.timeDown(colorder)
        //图片处理
        that.pictureProcessing();
      }else{
        wx.showToast({
          title: res.mes,
          duration:1500,
          icon:'none'
        })
      }
    })
  },

  //倒计时
  timeDown(colorder) {
    var that = this;
    colorder.countDown = util.addTime(colorder.ctime, that.data.collorderinfo.collint)
    that.setData({
      colorder: colorder,
    })
    var sign = that.data.sign;
    if (sign) {
      //延迟一秒执行自己
      setTimeout(function() {
        that.timeDown(that.data.colorder);
      }, 1000)
    }
  },

  //获取会员卡信息
  getCardList() {
    var that = this;
    var memcode = wx.getStorageSync("memcode");
    // var memcode = 'M153713';
    var stocode = that.data.collorderinfo.stocode;
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
  //支付
  topay() {
    var that = this;
    var countDown = that.data.colorder.countDown;
    if (countDown == "00:00:00") {
      wx.showToast({
        title: "拼团时间已过，不可拼团！",
        mask: true,
        duration: 1500,
        icon: 'none'
      })
      return
    }
    that.zhifu();
  },
  //会员卡notpwd== 1不需要输密码，notpwdAmount小于它时不需要输密码
  zhifu() {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var notpwdAmount = parseFloat(currentCard.notpwdAmount); //免密金额
    var price = parseFloat(that.data.collorderinfo.collmoney);
    if (currentCard.cardCode != "wx") {
      if (currentCard.notpwd == 1 || price <= notpwdAmount) {
        if (!that.data.buySeparately) {
          that.addcollageorder();
        } else {
          that.paycollagemoney(that.data.collorderinfo.pkcode, that.data.colorder.PKCode)
        }
      } else {
        this.selectComponent("#password").showModal();
      }
    } else {
      if (!that.data.buySeparately) {
        that.addcollageorder();
      } else {
        that.paycollagemoney(that.data.collorderinfo.pkcode, that.data.colorder.PKCode)
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
        if (!that.data.buySeparately) {
          that.addcollageorder();
        } else {
          that.paycollagemoney(that.data.collorderinfo.pkcode, that.data.colorder.PKCode)
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

  //去参团
  addcollageorder() {
    var that = this;
    var colpeople = that.data.colpeople;
    var pkcode = '';
    for (var i = 0; i < colpeople.length; i++) {
      if (colpeople[i].ismain == 1) {
        pkcode = colpeople[i].pkcode;
      }
    }
    var memcode = wx.getStorageSync("memcode");
    // var memcode = 'M153713';
    var data = {
      "actionname": "addcollageorder",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "stocode": that.data.collorderinfo.stocode,
        "pkcode": pkcode,
        "collPeople": memcode,
        "collprice": that.data.collorderinfo.collmoney
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    addcollageorder(data).then(res => {
      if (res.status == 0) {
        var mainorderno = res.data[0].mainorderno; // 主订单号
        var orderno = res.data[0].orderno; //副订单号
        //支付
        that.paycollagemoney(mainorderno, orderno);
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
  paycollagemoney(mainorderno, orderno) {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var zfpaytype = 2; //1微信支付 2会员卡支付
    var cardcode = currentCard.cardCode; //会员卡卡号
    var balance = currentCard.balance; //余额
    var price = that.data.collorderinfo.collmoney; //订单金额

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
          that.selectComponent("#CardsSelect").hidelogin();
          //订单明细
          that.getorderdetail();
          wx.showToast({
            title: res.mes,
            mask: true,
            duration: 1500,
            icon: 'success'
          })
          setTimeout(function() {
            wx.navigateTo({
              url: '/packageFood/pages/successful/successful?pkcode=' + mainorderno + '&status=1',
            })
          }, 1000)
        }).catch(err => {
          wx.showToast({
            icon: "none",
            title: '支付失败',
            duration: 2000,
          })
        })
      } else { //会员卡支付
        if (res.status == 0) {
          that.selectComponent("#CardsSelect").hidelogin();
          //订单明细
          that.getorderdetail();
          wx.showToast({
            title: res.mes,
            mask: true,
            duration: 1500,
            icon: 'success'
          })
          setTimeout(function() {
            wx.navigateTo({
              url: '/packageFood/pages/successful/successful?pkcode=' + mainorderno + '&status=1',
            })
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
  cancelOrder() {
    let that = this;
    let tstatus = that.data.tstatus;
    if (tstatus == 0) { //待支付
      that.truecancelOrder();
    } else if (tstatus == 1) { //已支付
      let colltimeout = that.data.collorderinfo.colltimeout;
      let text = '将自动取消订单并退款。';
      if (colltimeout == 0) {
        text = '将自动拼团。';
      }
      wx.showModal({
        title: '暂时无法取消订单',
        content: '发起拼团后,在拼团时间内未拼团成功,' + text,
        confirmText: "我知道了",
        confirmColor: "#EA6248",
        showCancel: false,
        success(res) {
          if (res.confirm) {}
        }
      })
    }
  },

  //取消订单
  truecancelOrder() {
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    var colpeople = that.data.colpeople;
    var pkcode = '';
    for (var i = 0; i < colpeople.length; i++) {
      if (colpeople[i].memcode == memcode) {
        pkcode = colpeople[i].pkcode;
      }
    }
    let data = {
      "actionname": "cancelorder",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "memcode": memcode,
        "pkcode": pkcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    cancelorderPT(data).then(res => {
      if (res.status == 0) {
        wx.showToast({
          title: res.mes,
          mask: true,
          duration: 1500,
          icon: 'none'
        })
        setTimeout(function() {
          wx.switchTab({
            url: '/pages/wallet/wallet',
          })
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

  //点击顶上菜品跳到详情页
  dishDetail() {
    var that = this;
    var collcode = that.data.collorderinfo.collcode;
    wx.navigateTo({
      url: '/packageFood/pages/groupdetail/groupdetail?collcode=' + collcode,
    })
  },
  //拼团详情
  groupDetails() {
    var that = this;
    var tcstatus = that.data.tcstatus;
    if (tcstatus == 0 || tcstatus == 1) {
      wx.navigateTo({
        url: '/packageFood/pages/successful/successful?pkcode=' + that.data.pkcode,
      })
    }
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
  //图片处理
  pictureProcessing() {
    var that = this;
    wx.downloadFile({
      url: that.data.bannerlist[0],
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
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function() {
    var that = this;
    if (!islogin()){
      wx.navigateTo({
        url: '/pages/login/login',
      })
    }
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
    //订单明细
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
  onShareAppMessage: function(res) {
    var that = this;
    if (res.from === 'button') {
      // 来自页面内转发按钮
      return {
        title: that.data.collorderinfo.collname,
        path: '/packageFood/pages/groupdetail/groupdetail?collcode=' + that.data.collorderinfo.collcode,
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