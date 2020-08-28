
import { get_agree } from '../../utils/server.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    stocode: '0',
    list: '',
    type: ''
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this;
    if (options.stocode) {
      that.data.stocode = options.stocode
    }
    if (options.type) {
      that.data.type = options.type;
    }
    that.get_data();
  },
  get_data() {
    let that = this;
    let data = {
      "actionname": "getagree",
      "parameters": {
        "GUID": "",
        "USER_ID": "",
        "stocode": that.data.stocode,
        "type": that.data.type
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_agree(data).then(res => {
      console.log(res)
      if (res.code == 0) {
        let list = res.data[0];
        list.content = list.content.replace(/\<img/gi, '<img style="max-width:100%;height:auto"');
        that.setData({
          list: list
        })
        wx.setNavigationBarTitle({
          title: list.name
        })
      } else {
        wx.showToast({
          title: res.msg,
          duration: 1500,
          icon: 'none'
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