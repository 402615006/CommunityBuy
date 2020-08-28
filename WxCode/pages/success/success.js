// pages/success/success.js
Page({

  /**
   * 页面的初始数据
   */
  data: {
    title:''
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    if (options.title){
      that.setData({
        title:options.title
      })
      wx.setNavigationBarTitle({
        title: options.title
      })
    }
    // 返回多少几页
    if (options.go_back){
      setTimeout(()=>{
        wx.navigateBack({
          delta: Number(options.go_back)
        });
      },1500)
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

  }
})