// packageVip/pages/index/index.js
Page({

  /**
   * 页面的初始数据
   */
  data: {
    tablist:[
      '全部','电影','美食'
    ],
    tabindex:0,
    list:[
      {
        price:'20',
        name:'抵扣券',
        stocode:'胡桃里餐厅满减优惠券',
        num:'500',
        numprice:'199'
      },
      {
        price:'20',
        name:'抵扣券',
        stocode:'胡桃里餐厅满减优惠券',
        num:'500',
        numprice:'199'
      },
      {
        price:'20',
        name:'抵扣券',
        stocode:'胡桃里餐厅满减优惠券',
        num:'500',
        numprice:'199'
      },
      {
        price:'20',
        name:'抵扣券',
        stocode:'胡桃里餐厅满减优惠券',
        num:'500',
        numprice:'199'
      },
      {
        price:'20',
        name:'抵扣券',
        stocode:'胡桃里餐厅满减优惠券',
        num:'500',
        numprice:'199'
      }
    ]
  },
  // 积分明细
  go_pointsdetails() {
    wx.navigateTo({
      url: '../pointsdetails/pointsdetails',
    })
  },
  // 去积分抽奖
  go_lottery(){
    wx.navigateTo({
      url: '../lottery/lottery',
    })
  },
  //兑换记录
  go_redemptionrecord(){
    wx.navigateTo({
      url: '../redemptionrecord/redemptionrecord',
    })
  },
  go_detail(){
    wx.navigateTo({
      url: '../couponexchange/couponexchange',
    })
  },
  // 点击下方选项卡
  clicktab(e){
    let that=this;
    console.log(e)
    let index=e.currentTarget.dataset.index;
    that.setData({
      tabindex:index
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {

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