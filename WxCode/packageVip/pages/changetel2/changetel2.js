// pages/phoneNumber2/phoneNumber2.js
import {  update_tel } from "../../utils/server.js";

import { set_verificationcode } from '../../../utils/server.js';
Page({

  /**
   * 页面的初始数据
   */
  data: {
    wait: 0,
    cardcode:'',
    tel: '',
    code: '',
    mobile: "",//手机号
    input_code: "",//验证码
    IDNO:''  //获取到了身份证号
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this
    if (!that.data.mobile) {
      this.setData({
        mobile: "****"
      })
    };
    if (options.IDNO){
      that.setData({
        IDNO: options.IDNO
      })
    }

  },

  //确定
  btn_submit() {
    let that = this
    let input_code = that.data.input_code;
    let tel = that.data.tel;
    let IDNO = that.data.IDNO;
    let cardcode = that.data.cardcode;
    let reg = /^1[0|1|2|3|4|5|6|7|8|9][0-9]{9}$/;
    let code = that.data.code;
    if (!cardcode){
      wx.showToast({
        icon: 'none',
        title: '请输入身份证号',
        duration: 1500
      })
      return
    }
    if (cardcode != IDNO){
      wx.showToast({

        title: '您输入的身份证号与您完善的身份证号不一致,请重新填写',
        icon: 'none',
        duration: 1500
      })
      return
    }
    if (!tel) {
      wx.showToast({
        mask: true,
        title: '请输入手机号',
        icon: 'none',
        duration: 1000
      })
      return
    }
    if (!input_code) {
      wx.showToast({
        mask: true,
        title: '请输入验证码',
        icon: 'none',
        duration: 1000
      })
      return
    }
    if (!reg.test(tel)) {
      wx.showToast({
        mask: true,
        title: '手机号格式不正确',
        icon: 'none',
        duration: 1000
      })
      return
    }
    if (!code) {
      wx.showToast({
        mask: true,
        title: '请先发送验证码',
        icon: 'none',
        duration: 1000
      })
      return
    }
    if (input_code != code) {
      wx.showToast({
        mask: true,
        title: '验证码不正确',
        icon: 'none',
        duration: 1000
      })
      return
    }
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let data={
        "actionname": "modifymobile",
        "parameters":{
          "GUID": "",
          "USER_ID": unionid,
          "newmobile": tel,
          "memcode": memcode,
        }
    }
    data.parameters = JSON.stringify(data.parameters) 
    update_tel(data).then((res) => {
      if (res.status == 0) {
        wx.navigateTo({
          url: '../../../pages/success/success?title=修改成功'+'&go_back=2',
        })
      } else {
        wx.showToast({
          mask: true,
          title: res.msg,
          icon: 'none',
          duration: 1000
        })
      }
    })
  },


  //输入身份证监听
  cardcode(e) {
    var cardcode = e.detail.value;
    this.setData({
      cardcode: cardcode
    })
  },
  //输入手机号监听
  phone(e) {
    var phone = e.detail.value;
    this.setData({
      tel: phone
    })
  },
  //输入验证码监听
  input_code(e) {
    var input_code = e.detail.value;
    this.setData({
      input_code: input_code
    })
  },
  //处理号码
  // mob_num(value) {
  //   console.log(value)
  //   if (!value) {
  //     return ''
  //   }
  //   return value.substring(0, 3) + "****" + value.substring(7);
  // },
  //倒计时
  time() {
    let that = this;
    let wait = 60;
    that.setData({
      wait: wait
    })
    let t = setInterval(() => {
      if (wait > 0) {
        wait = wait - 1;
        // console.log(wait)
        that.setData({
          wait: wait
        })
      } else {
        clearInterval(t);
      }
    }, 1000)
  },
  //获取手机号
  get_code() {
    let that = this;
    let tel = that.data.tel;
    let reg = /^1[0|1|2|3|4|5|6|7|8|9][0-9]{9}$/;
    console.log(tel)
    if (!tel) {
      wx.showToast({
        mask: true,
        title: '请输入手机号',
        icon: 'none',
        duration: 1000
      })
      return
    }
    if (!reg.test(tel)) {
      wx.showToast({
        mask: true,
        title: '手机号格式不正确',
        icon: 'none',
        duration: 1000
      })
      return
    }
    that.time();
    let data = {
      "actionname": "sendmsg",
      "parameters": {
        'mobile': tel,
        'descr': '绑定操作'
      }
    };
    data.parameters = JSON.stringify(data.parameters);
    set_verificationcode(data).then((res) => {
      if (res.status == 0) {
        that.setData({
          code: res.mes,
        })
      } else {
        wx.showToast({
          mask: true,
          title: '验证码发送失败',
          icon: 'none',
          duration: 1000
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

})