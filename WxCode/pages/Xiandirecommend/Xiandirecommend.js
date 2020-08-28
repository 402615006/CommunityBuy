// pages/coupon/coupon.js

Page({

  /**
   * 页面的初始数据
   */
  data: {
    tablist: ['美食', '休闲娱乐', '酒店', '教育培训'],
    tab_index: 0,
    
  },
  // 头部切换
  btn_tab(e) {
    let that = this;
    let index = e.currentTarget.dataset.index;
    if (index == that.data.tab_index) {
      return
    }
    that.setData({
      tab_index: index
    })
  },
  swiperChange(e) {
    let that = this;
    let source = e.detail.source;
    if (source == 'touch') {
      that.setData({
        tab_index: e.detail.current,
      })
    }
    let tab_index = e.detail.current;
    let id = "#text" + tab_index;
    that.selectComponent(id).onload();
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this;
    let tab_index = that.data.tab_index;
    let id = "#text" + tab_index;
    that.selectComponent(id).onload();
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
    var that = this;
    //上拉加载更多调用子组件上拉函数
      wx.stopPullDownRefresh();
      let tab_index = that.data.tab_index;
      let id = "#text" + tab_index;
      that.selectComponent(id).onPullDownRefresh();
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
    
  }
})