// packageFood/pages/addorderOk/addorderOk.js
Page({

  /**
   * 页面的初始数据
   */
  data: {
    orders:'',
    buscode:'',
    stocode:'',
    opencode:'',
    logo:'',
    stoimg: ''
  },
  fanhui(){
    wx.navigateBack();
  },
  // 去详情
  go_orderdetail(){
    let that=this;
    // let orders = that.data.orders;
    wx.redirectTo({
      url: '../checkorder/checkorder?buscode=' + that.data.buscode + '&stocode=' + that.data.stocode + '&opencode=' + that.data.opencode,
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options){
    let that=this;
    let stocode = options.stocode;
    let buscode = options.buscode;
    let opencode = options.opencode;
    that.setData({
      stocode: stocode,
      buscode: buscode,
      opencode: opencode
    })
    if (options.orders){
      that.setData({
        orders: options.orders
      })
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