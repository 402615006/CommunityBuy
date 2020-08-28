// packageVip/pages/expensesrecord/expensesrecord.js
import { getmemcardconsumption } from '../../utils/server.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    isnextpage: -1,
    currentpage: 1,
    pagesize: 10,
    isloadmore: false,
    isnomore: false,
    list:[],
    cardcode:''
  },
  // 获取消费记录
  get_data(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "getmemcardconsumption",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'memcode': memcode,
        'pageSize': that.data.pagesize,
        'currentPage': that.data.currentpage,
        'cardcode': that.data.cardcode
      }
    }
    that.setData({
      isloadmore: true
    })
    data.parameters = JSON.stringify(data.parameters);
    getmemcardconsumption(data).then(res => {
      that.setData({
        isloadmore: false
      })
      if (res.status == 0) {
        let list = res.data;
        let currentPage = res.currentPage;
        let totalPage = res.totalPage;
        let isnextpage=0;
        if (Number(totalPage) > Number(currentPage)){
          isnextpage=1;
        }
        switch (that.data.currentpage) {
          case 1:
            that.setData({
              list: list,
              isnextpage: isnextpage
            })
            break;
          default:
            that.setData({
              list: that.data.list.concat(list),
              isnextpage: isnextpage
            })
        }
      } else if (res.status == 1) {
        switch (that.data.currentpage) {
          case 1:
            that.setData({
              isno: true
            })
            break;
          default:
            break
        }
      }
    }).catch(err => {
      that.setData({
        isloadmore: false
      })
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    let cardcode = options.cardcode;
    that.setData({
      cardcode: cardcode
    })
    that.get_data();
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
    let that = this;
    let isnextpage = that.data.isnextpage;
    let isloadmore = that.data.isloadmore;
    if (isnextpage <= 0 || isloadmore == true) {
      return
    }
    that.data.currentpage = that.data.currentpage + 1;
    that.get_data();
  },

})