
import { get_jiemi, login_mobile, set_userInfo } from '../../utils/server.js';
var app = getApp();
Page({

  /**
   * 页面的初始数据
   */
  data: {
    mobile:'',    //手机号
  },
  // 获取是否授权
  get_userinfo() {
    let that = this;
    let userInfo = wx.getStorageSync('userInfo');
    if (userInfo){
      wx.navigateTo({
        url: '/pages/login/login',
      })
    }
    // 查看是否授权
    // wx.getSetting({
    //   success(re) {
    //     if (re.authSetting['scope.userInfo']) {
    //       // 已经授权，可以直接调用 getUserInfo 获取头像昵称
    //       wx.getUserInfo({
    //         success: function (res) {
    //           console.log(res);
    //           let userInfo = res.userInfo;
    //           wx.setStorageSync('userInfo', userInfo);
    //           wx.redirectTo({
    //             url: '/pages/login/login',
    //           })
    //         }
    //       })
    //     }
    //   }
    // })
  },
  // 授权微信用户信息
  bindGetUserInfo(e){
    let that = this;
    if (e.detail.errMsg == "getUserInfo:ok") {
      let userInfo = e.detail.userInfo;
      let encryptedData = e.detail.encryptedData;
      let iv = e.detail.iv;
      wx.setStorageSync('userInfo', userInfo);
      let session_key = wx.getStorageSync('session_key');
      let data = {
        "actionname": "mpdecrypt",
        "parameters": {
          'encryptedData': encryptedData,
          'iv': iv,
          'sessionKey': session_key
        }
      };
      data.parameters = JSON.stringify(data.parameters);
      get_jiemi(data).then(res=>{
        // console.log(res);
        if (res.unionId && res.openId){
          let openId = res.openId;
          let unionId = res.unionId;
          let mobile = that.data.mobile;
          wx.setStorageSync('unionid', unionId);
          wx.setStorageSync('openid', openId);
          that.login_mobile(openId, unionId, mobile);
        }else{
          wx.showModal({
            title: '提示',
            showCancel: false,
            content: '获取用户信息失败',
            success(res) {
              if (res.confirm) {
                wx.navigateBack()
              }
            }
          })
        }
      })
    }
  },
  // 登录
  login_mobile(openid, unionid, mobile){
    let that=this;
    let invitecode = '';
    if (app.globalData.shareId) {
      invitecode = app.globalData.shareId;
    }
    let data = {
      "actionname": "getuserstatus",
      "parameters": {
        'unionid': unionid,
        'openid': openid,
        'mobile': mobile,
        'invitecode': invitecode
      }
    }
    console.log(data);
    data.parameters = JSON.stringify(data.parameters);
    login_mobile(data).then(res=>{
      // console.log(res);
      if(res.status==0){
        let datas=res.data[0];
        if (!datas){
          wx.showModal({
            title: '提示',
            showCancel: false,
            content: '登录失败',
            success(res) {
              if (res.confirm) {
                wx.navigateBack()
              }
            }
          })
          return
        }
        if (!datas.memcode){
          wx.showModal({
            title: '提示',
            showCancel: false,
            content: '登录失败',
            success(res) {
              if (res.confirm) {
                wx.navigateBack()
              }
            }
          })
          return
        }
        let memcode = datas.memcode;
        let tel = datas.mobile;
        let invitecode = datas.invitecode;
        wx.setStorageSync('memcode', memcode);
        wx.setStorageSync('mobile', tel);
        wx.setStorageSync('invitecode', invitecode);
        wx.setStorageSync('isonLoad', true);
        wx.showToast({
          title: '登录成功',
          icon: 'success',
          duration: 1000
        })
        that.update_userInfo();
        setTimeout(()=>{
          wx.navigateBack();

          let pages = getCurrentPages();
          let prePage = pages[pages.length - 2];     //获取前一个页面
          try {
            prePage.onMyEvent();       //如果前一个页面有此方法则执行该方法
          } catch (err) {}
        },1000)
      }else{
        wx.showModal({
          title: '提示',
          showCancel: false,
          content: res.mes,
          success(res) {
            if (res.confirm) {
              wx.navigateBack()
            }
          }
        })
      }
    }).catch(err=>{
      wx.showModal({
        title: '提示',
        showCancel: false,
        content: res.mes,
        success(res) {
          if (res.confirm) {
            wx.navigateBack()
          }
        }
      })
    })
  },
  // 更新用户信息
  update_userInfo(){
    let that=this;
    let unionid = wx.getStorageSync('unionid');
    let userInfo = wx.getStorageSync('userInfo');
    let data = {
      'actionname': 'updateuserinfo',
      'parameters': {
        'unionid': unionid,
        'userInfo': userInfo,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    set_userInfo(data).then(res=> {
      // console.log(res);
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options){
    let that=this;
    if (options.phoneNumber){
      let mobile = options.phoneNumber;
      that.data.mobile = mobile;
    }
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