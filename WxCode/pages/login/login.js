
import { get_session_key, get_jiemi, login_mobile, set_userInfo } from '../../utils/server.js';
var app = getApp();
Page({
  /**
   * 页面的初始数据
   */
  data: {
    mobile:''
  },
  // 获取手机号
  getPhoneNumber(e){
    let that=this;
    let encryptedData = e.detail.encryptedData;
    let iv = e.detail.iv;
    if (encryptedData&&iv){
      // console.log("授权成功")
      let session_key=wx.getStorageSync('session_key');
      let data = {
        "actionname": "mpdecrypt",
        "parameters": {
          'encryptedData':encryptedData,
          'iv':iv,
          'sessionKey':session_key
        }
      };
      data.parameters = JSON.stringify(data.parameters);
      get_jiemi(data).then(res=>{
        // console.log(res);
        if (res.phoneNumber){
          let phoneNumber = res.phoneNumber;
          // 查看用户是否授权
          wx.getSetting({
            success(re) {
              if (re.authSetting['scope.userInfo']) {
                that.data.mobile = phoneNumber;
                // 已经授权，可以直接调用 getUserInfo 获取头像昵称
                wx.getUserInfo({
                  success: function (res) {
                    // console.log(res);
                    let userInfo = res.userInfo;
                    let encryptedData = res.encryptedData;
                    let iv = res.iv;
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
                    get_jiemi(data).then(res => {
                      // console.log(res);
                      if (res.openId) {
                        let openId = res.openId;
                        // let unionId = res.unionId;
                        let mobile = that.data.mobile;
                        // wx.setStorageSync('unionid', unionId);
                        wx.setStorageSync('openid', openId);
                        that.login_mobile(openId,mobile);
                      } else {
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
                })
              }else{
                wx.redirectTo({
                  url: '/pages/get_user/get_user?phoneNumber=' + phoneNumber,
                })
              }
            }
          })
          
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
          content: '用户手机号获取失败,请稍后再试',
          success(res) {
            if (res.confirm) {
              wx.navigateBack()
            }
          }
        })
      })
    }
  },
  // 页面初始化得到code
  get_code(){
    let that=this;
    wx.login({
      success: function (res) {
        let code = res.code;
        that.get_key(code);
      },
      fail: function (res) {
        wx.showModal({
          title: '提示',
          showCancel: false,
          content: '信息获取失败,请稍后再试',
          success:function(res){
            if (res.confirm) {
              wx.navigateBack()
            }
          }
        })
      }
    })
  },
  // 自动根据openid登录
  get_key(code){
    let that=this;
    let data = {
      "actionname": "getopenid",
      "parameters": {
        'code':code
      }
    };
    data.parameters = JSON.stringify(data.parameters);
    get_session_key(data).then(res=>{
      let session_key = res.session_key;
      if (session_key){
        wx.setStorageSync('session_key', session_key);
      }else{
        wx.showModal({
          title: '提示',
          showCancel: false,
          content: '服务器繁忙,请稍后再试',
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
        content: '服务器繁忙,请稍后再试',
        success(res) {
          if (res.confirm) {
            wx.navigateBack()
          }
        }
      })
    })
  },
  // 登录
  login_mobile(openid, mobile) {
    let that = this;
    let invitecode = '';
    if (app.globalData.shareId) {
      invitecode = app.globalData.shareId;
    }
    let data = {
      "actionname": "getmember",
      "parameters": {
        'openid': openid,
        'mobile': mobile
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    login_mobile(data).then(res => {
      if(!res)
      {
        wx.showToast({
          title: '登录失败',
          icon: 'fail',
          duration: 1000
        });
        return;
      }
        let memcode = res.memcode;
        let tel = res.mobile;
        // let invitecode = datas.invitecode;
        wx.setStorageSync('memcode', memcode);
        wx.setStorageSync('mobile', tel);
        // wx.setStorageSync('invitecode', invitecode);
        wx.setStorageSync('isonLoad', true);
        wx.showToast({
          title: '登录成功',
          icon: 'success',
          duration: 1000
        })
        // that.update_userInfo();
        setTimeout(() => {
          wx.navigateBack();    //返回上一页
          let pages = getCurrentPages();
          let prePage = pages[pages.length - 2];     //获取前一个页面
          try{
            prePage.onMyEvent();       //如果前一个页面有此方法则执行该方法
          }catch(err){}
        }, 1000)
    }).catch(err => {
      wx.showModal({
        title: '提示',
        showCancel: false,
        content: err.message,
        success(res) {
          if (res.confirm) {
            wx.navigateBack()
          }
        }
      })
    })
  },
  // 更新用户信息
  update_userInfo() {
    let that = this;
    let userInfo = wx.getStorageSync('userInfo');
    let data = {
      'actionname': 'updateuserinfo',
      'parameters': {
        'unionid': unionid,
        'userInfo': userInfo,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    set_userInfo(data).then(res => {
     
    })
  },
  // 查看服务协议
  go_detailxy(){
    wx.navigateTo({
      url: '/pages/Explanation/Explanation?type=0',
    })
  },
  go_detailys(){
    wx.navigateTo({
      url: '/pages/Explanation/Explanation?type=7',
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    that.get_code();
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