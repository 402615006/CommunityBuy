// packageVip/pages/redemptionrecord/redemptionrecord.js
Page({

  /**
   * 页面的初始数据
   */
  data: {
    isloadmore:false,
    isnomore:false,
    list:[]
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    let list = [
      {
        name: '恩佐餐厅20元优惠券',
        time: '2018-07-18 10:15',
        number: '450'
      },
      {
        name: '恩佐餐厅20元优惠券',
        time: '2018-07-18 10:15',
        number: '450'
      },
      {
        name: '恩佐餐厅20元优惠券',
        time: '2018-07-18 10:15',
        number: '450'
      },
      {
        name: '恩佐餐厅20元优惠券',
        time: '2018-07-18 10:15',
        number: '450'
      }
    ];
    that.setData({
      isloadmore: true
    })
    setTimeout(() => {
      that.setData({
        list: list,
        isloadmore: false,
        isnomore: true
      })
    }, 1500)
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