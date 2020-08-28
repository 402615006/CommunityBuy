// packageFood/pages/orderdetail/orderdetail.js
Page({
  /**
   * 页面的初始数据
   */
  data: {
    detail:'',
    list:[
      {
        name:'培根蔬菜水果沙拉',
        num:'5',
        price:'69'
      },
      {
        name:'培根蔬菜水果沙拉',
        num:'5',
        price:'69'
      },
      {
        name:'培根蔬菜水果沙拉',
        num:'5',
        price:'69'
      },
      {
        name:'培根蔬菜水果沙拉',
        num:'5',
        price:'69'
      }
    ]
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    console.log(options);
    if (options.detail){
      that.setData({
        detail: JSON.parse(options.detail)
      })
    }
    console.log(that.data.detail);
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