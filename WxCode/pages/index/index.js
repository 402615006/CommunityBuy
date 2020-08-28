//index.js
var util = require('../../utils/util.js');
var startPoint;
var {
  is_gologin
} = require('../../utils/util.js');
import {
  getLBlist,
  get_homeblock,
  set_enterPageAct
} from '../../utils/server.js';
import {
  serverURL
} from "../../utils/api.js"; //域名引入
//获取应用实例
const app = getApp()

Page({
  data: {
    bar_Height: wx.getSystemInfoSync().statusBarHeight,
    titleBarHeight:'',
    checkcode: '', //是否有好友分享优惠券号
    bannerlist: [], //轮播图图片集合
    bannerData: [], //轮播图所有数据
    tab_list: [
      {
        img: '/images/sy_002_dy.png',
        title: '水果蔬菜',
        typecode:'B4'
      },
      {
        img: '/images/sy_003_ms.png',
        title: '粮油副食',
        typecode:'B8'
      },
      {
        img: '/images/sy_004_yl.png',
        title: '肉肉水产',
        typecode:'B9'
      },
      {
        img: '/images/sy_005_jd.png',
        title: '零食饮料',
        typecode:'B12'
      },
      {
        img: '/images/sy_006_jy.png',
        title: '优惠券',
        typecode:'5'
      }
    ],
    noreadno:0,    //消息数量
    buttonLeft: '',
    buttonTop: '',
    windowHeight: '',
    windowWidth: ''
  },
  onLoad: function(options) {
    let that = this;
    if (options.checkcode) {
      that.setData({
        checkcode: options.checkcode
      })
    }
    that.getlist(); //轮播图
    that.get_homeblock();   //获取首页模块
    that.selectComponent("#content").get_news();
    // 有好友分享的优惠券码
    if (that.data.checkcode) {
      if (!is_gologin()) {
        wx.navigateTo({
          url: '/pages/login/login',
        })
      } else {
        that.selectComponent("#Receivecoupon").get_couponnews();
      }
    }

    // 延迟一秒后调用首页悬浮图
    setTimeout(()=>{
      that.selectComponent("#homebox").get_data();
    },1000)

    // 获取屏幕宽高
    wx.getSystemInfo({
      success: function (res) {
        // console.log(res);
        // 屏幕宽度、高度
        // console.log('height=' + res.windowHeight);
        // console.log('width=' + res.windowWidth);
        // 高度,宽度 单位为px
        that.setData({
          windowHeight: res.windowHeight,
          windowWidth: res.windowWidth,
          buttonLeft: res.windowWidth-55,
          buttonTop: res.windowHeight-130
        })
        if (res.platform == "devtools") {
          that.setData({
            titleBarHeight:48
          })
        } else if (res.platform == "ios") {
          that.setData({
            titleBarHeight: 44
          })
        } else if (res.platform == "android") {
          that.setData({
            titleBarHeight: 48
          })
        }
      }
    })


    // 进入页面记录页面
    set_enterPageAct('首页', '', '1', '');
  },
  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function() {
    let that = this;
    app.globalData.monitor = false;
    if (app.globalData.is_newpeoplecoupon == false) {
      if (is_gologin()) {
        that.selectComponent("#Newpeoplecoupon").get_newpeoplecoupon();
      }
    }
    that.getLonLat();
    that.selectComponent("#content").onPullDownRefresh();
  },

  // 登陆成功后的事件
  onMyEvent(){
    let that = this;
    if (that.data.checkcode){
      that.selectComponent("#Receivecoupon").get_couponnews();
    }
  },
  //清除checkcode
  clear_checkcode() {
    let that = this;
    that.setData({
      checkcode: ''
    })
  },
  //轮播图
  getlist() {
    let that = this;
    let data = {
      "actionname": "getlist",
      "parameters": {
        'key': '',
        'type': '1',
        'page': '1',
        'pagesize': '1',
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getLBlist(data).then(res=>{
      if(res.code){
        let list=res.data;
        list.map((item,index)=>{
          if (item.images){
            item.img = serverURL + '/UploadFiles/' + item.images;
          }
        })
        that.setData({
          bannerlist:list
        })
      }
    })
    return
  },
  // 获取首页模块
  get_homeblock(){
    return;
    let that=this;
    let data = {
      "actionname": "getnewapplist",
      "parameters": "{}"
    }
    get_homeblock(data).then(res=>{
      if(res.status==0){
        let list=res.data;
        that.setData({
          tab_list:list
        })
      }
    })
  },
  //点击轮播图进入详情
  bannerimgdetail(e) {
    let that = this;
    let url = e.currentTarget.dataset.url;
    if (url){
      wx.navigateTo({
        url: url ,
      })
    }
  },
  // 点击中间tab图标进入对应功能
  tabimgdetail(e) {
    let that = this;
    let item= e.currentTarget.dataset.item;
    that.selectComponent("#content").setData({type:item.typecode});
    that.selectComponent("#content").get_dishdata();
    return;
  },
  // 去领取优惠券
  receive_coupon() {
    wx.navigateTo({
      url: '/packageVip/pages/couponcenter/couponcenter'
    })
  },
  go_search(){
    wx.navigateTo({
      url: '../search/search',
    })
  },
  
  buttonEnd(){

  },
  buttonStart: function (e) {
    startPoint = e.touches[0];
  },
  buttonMove: function (e){
    var endPoint = e.touches[e.touches.length - 1]
    var translateX = endPoint.clientX - startPoint.clientX
    var translateY = endPoint.clientY - startPoint.clientY
    startPoint = endPoint
    var buttonTop = this.data.buttonTop + translateY
    var buttonLeft = this.data.buttonLeft + translateX
    //判断是移动否超出屏幕
    if (buttonLeft + 55 >= this.data.windowWidth) {
      buttonLeft = this.data.windowWidth - 55;
    }
    if (buttonLeft <= 0) {
      buttonLeft = 0;
    }
    if (buttonTop <= 0) {
      buttonTop = 0
    }
    if (buttonTop + 110 >= this.data.windowHeight) {
      buttonTop = this.data.windowHeight -110;
    }
    if (buttonTop<=this.data.bar_Height){
      buttonTop = this.data.bar_Height
    }
    this.setData({
      buttonTop: buttonTop,
      buttonLeft: buttonLeft
    })
  },
  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {
    var that = this;
    // 刷新轮播图
    that.getlist();
    //刷新首页模块
    that.get_homeblock();
    //上拉加载更多调用子组件上拉函数
    setTimeout(()=>{
      wx.stopPullDownRefresh();
      that.selectComponent("#content").onPullDownRefresh();
    },1000)
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {
    var that = this;
    //上拉加载更多调用子组件上拉函数
    that.selectComponent("#content").onReachBottom();
  },
  // 获取地理位置
  getLonLat: function(callback) {
    var that = this;
    wx.getLocation({
        type: 'gcj02',
        success: function(res) {
          //存缓存
          wx.setStorageSync('position', res);
        }
    });
  },
  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function (res) {
    
  }
})