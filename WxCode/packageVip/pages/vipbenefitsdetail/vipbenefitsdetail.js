// packageVip/pages/vipbenefitsdetail/vipbenefitsdetail.js
Page({

  /**
   * 页面的初始数据
   */
  data: {
    list_index:0,
    list: [
      {
        'name': '升级礼包',
        'price': '150',
        'number': '2'
      },
      {
        'name': '升级礼包',
        'price': '150',
        'number': '2'
      },
      {
        'name': '升级礼包',
        'price': '150',
        'number': '2'
      },
      {
        'name': '升级礼包',
        'price': '150',
        'number': '2'
      },
      {
        'name': '升级礼包',
        'price': '150',
        'number': '2'
      },
      {
        'name': '升级礼包',
        'price': '150',
        'number': '2'
      },
      {
        'name': '升级礼包',
        'price': '150',
        'number': '2'
      },
    ]
  },
  // 点击选择
  click_li(e){
    console.log(e);
    let that=this;
    let index=e.currentTarget.dataset.index;
    that.setData({
      list_index: index
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