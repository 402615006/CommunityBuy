
import { get_px, is_gologin } from '../../../utils/util.js'

import { receivecoupon } from '../../utils/server';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    checkcode: '',
    list: '',
    qrwidth: '0',
    qrcolor: '#000'
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this;
    let result = options.result;
    result = JSON.parse(result);
    console.log(result)
    that.setData({
      qrwidth: get_px('280'),
      list: result
    })
    // that.get_data();
  },
  // 点击二维码图片
  previewImage() {
    const that = this.selectComponent('#qrcode')
    wx.canvasToTempFilePath({
      canvasId: 'wux-qrcode',
      success: (res) => {
        wx.previewImage({
          urls: [res.tempFilePath]
        })
      }
    }, that)
  },
  // 领取优惠券
  receive() {
    let that = this;
    if (!is_gologin()){
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }

    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let list = that.data.list;
    let data = {
      "actionname": "receivecouponter",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': unionid,
        'pcode': that.data.list.pcode,
        'ccode': '',
        'ccname': '',
        'mccode': that.data.list.mccode,
        'couponcode': that.data.list.mccode,
        'memcode': memcode,
        'ffstocode': ''
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    receivecoupon(data).then(res => {
      console.log(res)
      if (res.code == 0) {
        wx.showToast({
          title: '领取成功',
          icon: 'none',
          duration: 1500,
          mask: true
        })
        list.checkcode = res.data[0].checkcode;
        that.setData({
          list: list
        })
      } else {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
    })
  },
  // 获取数据
  get_data() {
    let that = this;
    let unionid = wx.getStorageSync('unionid');
    let data = {
      "actionname": "getcoupondes",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': unionid,
        'couponcode': that.data.checkcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_couponnewdetail(data).then(res => {
      if (res.code == 0) {
        console.log(res);
        let list = res.data[0];
        that.setData({
          list: list
        })
      } else {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  // onShareAppMessage: function (res) {
  //   let that = this;
  //   let list = that.data.list;
  //   let checkcode = list.checkcode;
  //   let couname = list.couname;
  //   let unionid = wx.getStorageSync('unionid');
  //   let memcode = wx.getStorageSync('memcode');

  //   let data = {
  //     "actionname": "sharecoupon",
  //     "parameters": {
  //       "GUID": "",
  //       "USER_ID": unionid,
  //       "couponcode": checkcode,
  //       "memcode": memcode
  //     }
  //   }
  //   data.parameters = JSON.stringify(data.parameters);
  //   send_couponnews(data).then(res => {

  //   })
  //   return {
  //     title: couname,
  //     path: 'pages/wallet/wallet?checkcode=' + checkcode,
  //     imageUrl: '../../images/yhq.jpg',
  //     success: function () {

  //     }
  //   }
  // }
})