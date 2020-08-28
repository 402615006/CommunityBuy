
var { is_gologin } = require('../../utils/util.js');

import { get_cardslist } from '../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    count:false,
    islogin:false,
    list:[],
    stocode:''
  },
  // 去领卡确认
  go_opencardconfirm(){
    wx.navigateTo({
      url: '/packageVip/pages/opencardconfirm/opencardconfirm?type=1',
    })
  },
  // 去办理会员卡
  go_cadslist(){
    let that=this;
    wx.navigateTo({
      url: '/packageVip/pages/cardlist/cardlist?stocode=' + that.data.stocode
    })
  },
  // 绑定会员卡
  bindcard() {
    wx.navigateTo({
      url: '/packageVip/pages/bindcard/bindcard'
    })
  },
  // 点击会员卡进入卡详情
  go_carddetail(e){
    let item=e.currentTarget.dataset.item;
    let cardcode = item.cardCode;
    let cardtype = item.cardtype;
    if (cardtype==0){
      //年费卡
      wx.navigateTo({
        url: '/packageVip/pages/vipcarddetail/vipcarddetail?cardcode=' + cardcode
      })
    } else if (cardtype == 1){
      //充值卡
      wx.navigateTo({
        url: '/packageVip/pages/pluscarddetail/pluscarddetail?cardcode=' + cardcode
      })

    } else if (cardtype == 2){
      // 次卡
      wx.navigateTo({
        url: '/packageVip/pages/timscarddetail/timscarddetail?cardcode=' + cardcode
      })
    }
  },
  // 获取会员卡列表
  get_data(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data={
      'actionname':'getmemcardslistnew',
      'parameters':{
        'GUID':'',
        'USER_ID':'',
        'memcode': memcode,
        'stocode': that.data.stocode
      }
    }
    let showloading=true
    if (that.data.count==true){
      showloading=false;
    }else{
      showloading = true;
    }
    data.parameters = JSON.stringify(data.parameters);
    get_cardslist(data,showloading).then(res=>{
      if(res.status==0){
        let list=res.data;
        list.map((item,index)=>{
          item.cards.map((ctim,idx)=>{
            if (ctim.imgPaths){
              ctim.imgPaths = ctim.imgPaths.split(',')[0];
            }
          })
        })
        that.setData({
          list:list,
          count:true
        })
      }
    })
  },
  // 点击登录
  btn_log(){
    let that=this;
    wx.navigateTo({
      url: '/pages/login/login',
    })
  },
  // 登陆成功后的事件
  onMyEvent() {
    let that = this;
    that.setData({
      islogin:true
    })
    this.onShow();
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    if (options.stocode){
      that.setData({
        stocode: options.stocode
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
    if (is_gologin()) {
      that.setData({
        islogin: true
      })
    } else {
      that.setData({
        islogin: false
      })
    }
    let islogin = that.data.islogin;
    if (islogin){
      that.get_data();
    }
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
  onShareAppMessage: function (res) {
    return {
      path: 'pages/index/index',
      success: function () {

      }
    }
  }
})