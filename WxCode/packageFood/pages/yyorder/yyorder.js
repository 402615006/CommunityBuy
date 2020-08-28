// packageFood/pages/yyorder/yyorder.js

import { yuyue_jl, cen_yuyue } from '../../utils/server.js';

Page({
  /**
   * 页面的初始数据
   */
  data: {
    currentpage: 1,  //页数
    pagesize: 10,    //每页多少条

    isloadmore: false,
    isnomore: false,
    flag:false,
    list:[]
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options){
    let that=this;
    that.get_data();
  },
  cen_yuyue(e){
    let that=this;
    wx.showModal({
      title: '温馨提示',
      content: '您确定要取消预订吗？',
      success:function(res){
        if (res.confirm) {
          let stocode = e.currentTarget.dataset.stocode;
          let id = e.currentTarget.dataset.id;
          let index = e.currentTarget.dataset.index;
          let list = that.data.list;
          let data = {
            "actionname": "updatereservationstatus",
            "parameters": {
              'BusCode': '88888888',
              'StoCode': stocode,
              'id': id,
              'status': '3'
            }
          }
          data.parameters = JSON.stringify(data.parameters);
          cen_yuyue(data).then(res => {
            if (res.code == 0) {
              wx.showToast({
                title: '取消成功',
                icon: 'success',
                duration: 1500,
                mask: true
              })
              list[index].AStatus = '3';
              that.setData({
                list: list
              })
            } else {
              wx.showToast({
                title: res.msg,
                icon: 'none',
                duration: 1500,
                mask: true
              })
            }
          })
        } else if (res.cancel) {
          console.log('用户点击取消')
        }
      }
    })
  },
  // 获取列表数据
  get_data(){
    let that = this;
    let filter = [];
    let mobile = wx.getStorageSync('mobile');
   
    filter.push({ 'col': 'r.CCode', 'filter': mobile, 'exp': '=', 'cus': '' })
    let data = {
      "actionname": "getlist",
      "parameters": {
        'limit': that.data.pagesize,
        'page': that.data.currentpage,
        'filters': filter
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    that.setData({
      isloadmore: true
    })
    yuyue_jl(data).then(res => {
      that.setData({
        isloadmore: false
      })
      if (res.code == 0) {
        let list = res.data;
        switch (that.data.currentpage) {
          case 1:
            that.setData({
              list: list,
              isnextpage: res.isnextpage,
              flag:true
            })
            break;
          default:
            that.setData({
              list: that.data.list.concat(list),
              isnextpage: res.isnextpage
            })
        }
      }
    }).catch(err => {
      that.setData({
        isloadmore: false
      })
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
    let that = this;
    let isnextpage = that.data.isnextpage;
    let isloadmore = that.data.isloadmore;
    if (isnextpage <= 0) {
      return
    }
    that.data.currentpage = that.data.currentpage + 1;
    that.get_data();
  }
})