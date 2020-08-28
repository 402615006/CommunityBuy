// packageVip/pages/vipcarddetail/vipcarddetail.js
import { get_memcardinfo } from '../../utils/server.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    cardcode: ''
  },
  // 去使用记录
  go_expensesrecord() {
    let that = this;
    wx.navigateTo({
      url: '../expensesrecord/expensesrecord?cardcode=' + that.data.cardcode
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this;
    let cardcode = options.cardcode;
    that.setData({
      cardcode: cardcode
    })
    that.get_data()
  },
  // 获取数据
  get_data() {
    let that = this;
    let data = {
      "actionname": "getmemcardinfo",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'cardcode': that.data.cardcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_memcardinfo(data).then(res => {
      if (res.status == 0) {
        console.log(res)
        let list = res.data;
        that.setData({
          list: list
        })
      } else {
        wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500,
          mask: true
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

  }
})