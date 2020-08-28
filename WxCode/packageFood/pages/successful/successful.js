// packageFood/pages/successful/successful.js
import {
  getPTorderDetail
} from '../../utils/server.js';
import {
  baserURLcard
} from "../../../utils/api.js"; //域名引入
var util = require('../../../utils/util.js');

Page({
  /**
   * 页面的初始数据
   */
  data: {
    pkcode: "",
    tstatus: "", //待拼团 拼团中 拼团成功 拼团退款中 拼团已退款  取消(0, 1, 2, 3, 4, 5)
    tcstatus: "", //拼团支付状态（0：待支付 1：支付成功 2：取消支付 3：退款中 4：已退款 5 部分支付）
    colorder: '', //订单支付信息
    disInfo: "", //菜品信息 
    collorderinfo: "", //订单信息 
    colpeople: [], //拼团人信息
    successData: false,
    sign: false, //是否启用定时器
    ptStatus: "", //判断是否是从拼团订单页面支付完成后进入
  },
  // 00: 00: 00 
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    let that = this;
    //右上角移除分享功能
    wx.hideShareMenu();
    that.setData({
      pkcode: options.pkcode,
      ptStatus: options.status,
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
    getPTorderDetail(data).then(res => {
      if (res.status == 0) {
        var colorder = res.colorder[0]; //订单支付信息    
        var collorderinfo = res.collorderinfo[0]; //订单信息
        var colpeople = res.colpeople; //拼团人信息

        that.setData({
          colorder: colorder,
          collorderinfo: collorderinfo,
          colpeople: colpeople,
          tstatus: collorderinfo.tstatus,
          tcstatus: collorderinfo.tcstatus,
          successData: true,
          sign: true,
        })
        //倒计时
        that.timeDown(colorder)
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
      if (colorder.countDown != "00:00:00") {
        //延迟一秒执行自己
        setTimeout(function() {
          that.timeDown(that.data.colorder);
        }, 1000)
      }
    }
  },
  //订单规则
  orderRule() {
    wx.showModal({
      title: '如何参加拼团',
      content: '没有字段',
      confirmText: "我知道了",
      confirmColor: "#EA6248",
      showCancel: false,
      success(res) {
        if (res.confirm) {}
      }
    })
  },

  //点击跳到详情页
  godetail() {
    var that = this;
    wx.navigateTo({
      url: '/packageFood/pages/grorderdetail/grorderdetail?pkcode=' + that.data.pkcode,
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
    let that = this;
    let ptStatus = that.data.ptStatus;
    if (ptStatus == 1) {
      wx.switchTab({
        url: '/pages/wallet/wallet',
      })
    }
  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {

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
        // imageUrl: that.data.bannerlist[0],
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