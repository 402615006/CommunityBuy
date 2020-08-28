// packageVip/pages/assets/assets.js
import { get_cardslist } from '../../../utils/server.js'

Page({

  /**
   * 页面的初始数据
   */
  data:{
    balance:0,
    list:[
     
    ]
  },
  // 获取会员卡列表
  get_data() {
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'getmemcardslistnew',
      'parameters': {
        'GUID': '',
        'USER_ID': '',
        'memcode': memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_cardslist(data,true).then(res => {
      console.log(res);
      if (res.status == 0) {
        let list = res.data;
        that.setData({
          list: list
        })
      }
    })
  },
  
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    let balance = options.balance;
    that.setData({
      balance: balance
    })
    that.get_data();
  },
  go_detail(e){
    console.log(e)
    let item = e.currentTarget.dataset.item;
    let cardcode = item.cardCode;
    let cardtype = item.cardtype;
    if (cardtype == 0) {
      //年费卡
      wx.navigateTo({
        url: '/packageVip/pages/vipcarddetail/vipcarddetail?cardcode=' + cardcode
      })
    } else if (cardtype == 1) {
      //充值卡
      wx.navigateTo({
        url: '/packageVip/pages/pluscarddetail/pluscarddetail?cardcode=' + cardcode
      })
    } else if (cardtype == 2) {
      // 次卡
      wx.navigateTo({
        url: '/packageVip/pages/timscarddetail/timscarddetail?cardcode=' + cardcode
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