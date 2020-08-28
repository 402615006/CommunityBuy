
import { get_mycashoutlist } from '../../utils/server.js';
Page({

  /**
   * 页面的初始数据
   */
  data: {
    isloadmore: false,
    isno: false,
    list:[]
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this;
    that.get_data();
  },
  // 获取数据
  get_data() {
    let that = this;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'mycashoutlist',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid,
        'memcode': memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_mycashoutlist(data).then(res => {
      if (res.status == 0) {
        let list = res.data;
        that.setData({
          list: list,
          isno: true
        })
      } else {
        that.setData({
          isno: true,
          list: []
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