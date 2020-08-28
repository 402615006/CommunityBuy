// pages/NewcomerExclusive/NewcomerExclusive.js
import { newuserprolist } from '../../utils/server.js';
import { baserURLOrganization } from '../../utils/api.js';
Page({

  /**
   * 页面的初始数据
   */
  data: {
    isloadmore:false,
    currentpage: 1,//页数
    pagesize: 10,//每页多少条
    isnextpage:"",//是否有下一页
    noMore:false,
    url: baserURLOrganization,
    list:[],
    isno:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this;
    wx.hideShareMenu();
    that.getListData();
  },

  // 获取数据
  getListData(){
    let that = this;
    that.setData({
      isloadmore:true
    })
    let data = {
      'actionname': 'newuserprolist',
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "currentpage":that.data.currentpage,
        "pagesize":that.data.pagesize
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    newuserprolist(data).then(res => {
      console.log(res)
      that.setData({
        isloadmore: false
      })
      if (res.code == 0) {
        let list = res.data;
        for(var i = 0;i<list.length;i++){
          list[i].smallimg = that.data.url+list[i].smallimg;
        }
        that.setData({
          list: that.data.list.concat(list),
          isnextpage:res.isnextpage,
        })
      } else if (res.code == 1){
        that.setData({
          isno: true
        })
      } else {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          mask: true,
          duration: 1500
        })
      }
    }).catch(err=>{
      that.setData({
        isloadmore: false
      })
    })
  },
  todetail(e){
    var procode = e.currentTarget.dataset.code;
    wx.navigateTo({
      url: '/pages/memDetail/memDetail?procode=' + procode+'&type=1',    
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
    let that=this;
    wx.stopPullDownRefresh();
    that.setData({
      currentpage: 1,
      list:[]
    })
    that.getListData();
  },
  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
    let that = this;
      let isnextpage = that.data.isnextpage;
      if (isnextpage == 0) {
        return
      }
      that.data.currentpage = that.data.currentpage + 1;
      that.getListData();
  }
})