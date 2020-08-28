// packageVip/pages/MemberBenefits/MemberBenefits.js
import { memuserprolist } from '../../utils/server.js';
import { baserURLOrganization } from '../../../utils/api.js';
var app = getApp();

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
  // 去升级会员
  go_Myrank(){
    app.globalData.memscroll=true;
    wx.switchTab({
      url: '/pages/member/member',
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that =this;
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
      'actionname': 'memuserprolist',
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "currentpage":that.data.currentpage,
        "pagesize":that.data.pagesize
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    memuserprolist(data).then(res => {
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
  // 详情页
  todetail(e){
    var procode = e.currentTarget.dataset.code;
    wx.navigateTo({
      url: '/pages/memDetail/memDetail?procode=' + procode+'&type=3',    
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
    var that = this;
    setTimeout(() => {
      wx.stopPullDownRefresh();
      that.setData({
        currentpage: 1,
        list: [],
      })
      that.getListData();
    }, 500);
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
    var that = this;
    if(that.data.isnextpage==1){
      that.setData({
        currentpage: that.data.currentpage+1,
      })
      that.getListData();
    }else{
      that.setData({
        noMore: true,
      })     
    }
  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})