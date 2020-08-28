// packageVip/pages/changetel/changetel.js

import { set_verificationcode } from '../../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    tel:'',
    code: '',
    verification_code: '',
    times:0,
  },
  // 点击获取验证码
  get_code() {
    let that = this;
    let tel = that.data.tel;
    let data = {
      "actionname": "sendmsg",
      "parameters": {
        'mobile': tel,
        'descr': '绑定操作'
      }
    };
    data.parameters = JSON.stringify(data.parameters);
    set_verificationcode(data).then(res => {
      console.log(res)
      if (res.status == 0) {
        that.setData({
          verification_code: res.mes
        })
      }
    })
    that.set_times();
  },
  // 定时器
  set_times() {
    let that = this;
    let times = 60;
    that.setData({
      times: times
    })
    let loading = setInterval(() => {
      if (times > 0) {
        times = times - 1
      } else {
        clearInterval(loading)
      }
      that.setData({
        times: times
      })
    }, 1000)
  },
  input_code(e) {
    let that = this;
    let code = e.detail.value;
    that.setData({
      code: code
    })
  },
  // 原号码没有了
  go_yanzheng(){
    wx.navigateTo({
      url: '../verification/verification'
    })
  },
  // 下一步
  btn() {
    let that = this;
    let code = that.data.code;
    let verification_code = that.data.verification_code;
   
    if (!verification_code) {
      wx.showToast({
        title: '请先点击获取验证码',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!code) {
      wx.showToast({
        title: '请输入验证码',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (code != verification_code){
      wx.showToast({
        title: '验证码错误',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    wx.navigateTo({
      url: '../changetel2/changetel2'
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    let tel = options.tel;
    that.setData({
      tel:tel
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

  }
})