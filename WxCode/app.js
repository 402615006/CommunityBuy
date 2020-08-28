//app.js
App({
  onLaunch: function (options) {
    if (wx.getStorageSync('isonLoad')!=true){
      wx.clearStorageSync();
    }

    // 小程序启动时获取分享的人的id,小程序所有页面分享要带分享人的id都设置参数为shareId 如?shareId=5;

    // console.log("小程序启动");
    // console.log(options)
    if (options.query.shareId){
      // 如果小程序启动是通过分享获取分享的二维码扫描进入都会在小程序总内存里存入分享人id直到小程序从新启动或者小程序自动清理进程这个字段重新还原为空；
      this.globalData.shareId = options.query.shareId;
    }
    // 监听网络状态
    wx.onNetworkStatusChange(function (res) {
      if (res.networkType == 'none') {
        wx.showToast({
          title: '无网络',
          icon: 'none',
          duration: 2000
        })
      }
    });
    // 获取手机状态栏高度
    wx.getSystemInfo({
      success: res => {
        //导航高度
        this.globalData.navHeight = res.statusBarHeight + 46;
        this.bar_Height=res.statusBarHeight;		// 获取手机状态栏高度
        if (res.platform == "devtools"){
          this.deviceModel=='pc';
        } else if (res.platform == "ios"){
          this.deviceModel == 'ios';
        } else if (res.platform == "android"){
          this.deviceModel == 'android';
        }
      }
    })


    // 检测是否有小程序更新提示更新
    const updateManager = wx.getUpdateManager();
    updateManager.onCheckForUpdate(function (res) {
      // 请求完新版本信息的回调
      // console.log(res.hasUpdate)
    })
    updateManager.onUpdateReady(function () {
      wx.showModal({
        title: '更新提示',
        content: '新版本已经准备好，是否重启应用？',
        success: function (res) {
          if (res.confirm) {
            // 新的版本已经下载好，调用 applyUpdate 应用新版本并重启
            updateManager.applyUpdate()
          }
        }
      })
    })
    // 更新失败
    updateManager.onUpdateFailed(function () {
      // 新的版本下载失败
      wx.showModal({
        title: '更新提示',
        content: '新版本下载失败',
        showCancel: false
      })
    })
  },
  globalData: {
    "deviceModel":'',    //设备系统
    "shareId":'',    //分享人id默认为空
    "isentercode":false,    //判断是否输入过验证码
    "bar_Height":0,
    "navHeight":0,    //状态栏高度
    "userInfo": null,
    "tab_index":null,
    "isgodingdan":null,           
  }
})
