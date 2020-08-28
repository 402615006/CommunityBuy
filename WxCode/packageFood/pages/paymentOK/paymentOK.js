// packageFood/pages/paymentOK/paymentOK.js
Page({

  /**
   * 页面的初始数据
   */
  data: {
    ShiftCode:'',
    billcode:'',
    money:'',
    detail:''
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    if (options.ShiftCode){
      that.setData({
        ShiftCode: options.ShiftCode
      })
    }
    if (options.billcode){
      that.setData({
        billcode: options.billcode
      })
    }
    if (options.money){
      that.setData({
        money: options.money
      })
    }
    if (options.payName){
      that.setData({
        payName: options.payName
      })
    }
    if (options.detail){
      that.setData({
        detail: options.detail
      })
    }
  },
  // 去详情
  go_detail(){
    let that=this;
    wx.navigateTo({
      url: '/packageFood/pages/orderdetail/orderdetail?detail=' + that.data.detail,
    })
  },
  // 返回首页
  go_index(){
    wx.switchTab({
      url: '/pages/index/index',
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