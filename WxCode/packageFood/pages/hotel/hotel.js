// packageFood/pages/hotel/hotel.js
var util = require('../../../utils/util.js');

Page({

  /**
   * 页面的初始数据
   */
  data: {
    stocoordx:'43.817130',
    stocoordy:'87.622600',
    juli:'',
    address:'新疆维吾尔自治区乌鲁木齐市水磨沟区新民东街186号',
    bannerlist:[
      'http://back.xj-wz.com/uploads/wx/201912131921189574.jpg',
      'http://back.xj-wz.com/uploads/wx/201912131921223550.jpg',
      'http://back.xj-wz.com/uploads/wx/201912131921253181.jpg',
      'http://back.xj-wz.com/uploads/wx/201912131921286978.jpg'
    ]
  },
  // 打电话
  calling(){
    let that = this;
    wx.makePhoneCall({
      phoneNumber: '0991-6526666', //此号码并非真实电话号码，仅用于测试
      success: function () {
        console.log("拨打电话成功！")
      },
      fail: function () {
        console.log("拨打电话失败！")
      }
    })
  },
  // 打开地图
  go_map(){
    let that=this;
    wx.openLocation({
      latitude: Number(that.data.stocoordx),
      longitude: Number(that.data.stocoordy),
      name: '乌鲁木齐东街禧玥酒店',
      address: that.data.address,
      scale: 28
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    wx.getLocation({
      type: 'gcj02',
      success: function (res) {
        let latitude = that.data.stocoordx;
        let longitude = that.data.stocoordy;
        var juli = util.getDistance(latitude, longitude,res.latitude, res.longitude);
        that.setData({
          //保留两位小数
          juli: juli.toFixed(2),
        })
      }
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

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})