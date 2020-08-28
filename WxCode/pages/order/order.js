// pages/order/order.js

//获取应用实例
var app = getApp();
Page({

  /**
   * 页面的初始数据
   */
  data: {
    tabindex:0,
    tablist: ['电影','美食','组局','套餐','礼包']
  },
  btn_tab(e){
    let that=this;
    let index=e.currentTarget.dataset.index;
    if (index == that.data.tabindex){
      return 
    }
    that.setData({
      tabindex:index
    })
    let tabindex = that.data.tabindex;
    let id = '#box' + tabindex;
    if (tabindex==0){
      that.selectComponent('#box1').hide();
      that.selectComponent('#box2').hide();
      that.selectComponent('#box3').hide();
      that.selectComponent('#box4').hide();
    } else if(tabindex == 1){
      that.selectComponent('#box0').hide();
      that.selectComponent('#box2').hide();
      that.selectComponent('#box3').hide();
      that.selectComponent('#box4').hide();
    } else if (tabindex == 2) {
      that.selectComponent('#box0').hide();
      that.selectComponent('#box1').hide();
      that.selectComponent('#box3').hide();
      that.selectComponent('#box4').hide();
    } else if (tabindex == 3) {
      that.selectComponent('#box0').hide();
      that.selectComponent('#box1').hide();
      that.selectComponent('#box2').hide();
      that.selectComponent('#box4').hide();
    } else if (tabindex == 4){
      that.selectComponent('#box0').hide();
      that.selectComponent('#box1').hide();
      that.selectComponent('#box2').hide();
      that.selectComponent('#box3').hide();
    }
    that.selectComponent(id).show();
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    app.globalData.monitor = false;
    if (options.tabindex){
      that.setData({
        tabindex: options.tabindex
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
    let that = this;
    let tabindex = that.data.tabindex;
    let id = '#box' + tabindex;
    that.selectComponent(id).show();
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {
    let that = this;
    that.selectComponent('#box0').onhide();
    that.selectComponent('#box1').onhide();
    that.selectComponent('#box2').onhide();
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