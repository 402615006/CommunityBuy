
import { get_myviplevel, get_walletinfo ,set_enterPageAct } from '../../utils/server.js';
var { is_gologin } = require('../../utils/util.js');
var app = getApp();

Page({
  /**
   * 页面的初始数据
   */
  data: {
    tab_index:0,
    tab:['热销爆款','新品上架','即将下架'],
    fxMsg:"",      //点击分享时当前信息
    memcode: '',   //会员编号
    memlevel:'',   //会员等级
    expdate:'',     //到期时间
    list:''
  },
  // 获取佣金
  get_walletinfo() {
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let data = {
      'actionname': 'myaccountinfo',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid,
        'memcode': memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_walletinfo(data).then(res => {
      if (res.status == 0) {
        let list = res.data;
        that.setData({
          list: list
        })
      } else {
        wx.showToast({
          title: res.mes,
          icon: 'none',
          mask: true,
          duration: 1500
        })
      }
    })
  },
  // 获取信息
  get_data(){
    let that = this;
    if(!that.data.memcode){
      return
    }
    let data = {
      "actionname": "myviplevel",
      "parameters": {
        "GUID": "",
        "USER_ID": "",
        "memcode": that.data.memcode
      }
    };
    data.parameters = JSON.stringify(data.parameters);
    get_myviplevel(data).then(res => {
      if (res.status == 0) {
        let memlevel = res.memlevel;
        that.setData({
          memlevel: memlevel,
          expdate: res.expdate
        })
      }
    })
  },
  // 获取头部高度
  get_height() {
    let that = this;
    var query = wx.createSelectorQuery();
    //选择id
    query.select('.member_on').boundingClientRect(function (rect) {
      // console.log(rect.height);
    }).exec();
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    wx.hideShareMenu();
    let tab_index = that.data.tab_index;
    let id = "#text" + tab_index;
    that.selectComponent(id).onload();

    // 进入页面记录页面
    set_enterPageAct('会员','','2','');
  },
  // 点击tabbar
  click_tab(e){
    let that=this;
    let index = e.currentTarget.dataset.index;
    if (index == that.data.tab_index){
      return
    }
    that.setData({
      tab_index:index
    })
    let tab_index = that.data.tab_index;
    let id = "#text" + tab_index;
    that.selectComponent(id).onload();
  },
  //点击名字进入vip
  go_vip(){
    wx.navigateTo({
      url: '/packageVip/pages/Myrank/Myrank',
    })
  },
  // 去邀请码
  go_yaoqing() {
    let that = this;
    let list = that.data.list;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/invitecode/invitecode'
    })
  },
  // 推广攻略
  go_tuiguanggognlue(){
    wx.navigateTo({
      url: '/pages/Explanation/Explanation?type=1',
    })
  },

  // 去提现
  go_commission() {
    let that = this;
    let list = that.data.list;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/commission/commission'
    })
  },
  // 去卡包
  go_card() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '../vip/vip'
    })
  },
  //新人专享
  go_NewcomerExclusive(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    if (!memcode) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '../NewcomerExclusive/NewcomerExclusive',
    })
  },
  // 闲弟推荐
  go_Xiandirecommend(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    if (!memcode) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '../Xiandirecommend/Xiandirecommend',
    })
  },
  // 去会员福利
  go_MemberBenefits(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    if (!memcode) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/MemberBenefits/MemberBenefits',
    })
  },
  // 去积分商城
  go_jifen(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    if (!memcode) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/index/index',
    })
  },
  // 会员权益
  go_OrganizationDescription(){
    wx.navigateTo({
      url: '/pages/Explanation/Explanation?type=8',
    })
  },
  //会员特惠礼包分享
  fxjd(e){
    var that = this;
    that.setData({
      fxMsg:e.detail,
    })
  },

  //会员特惠礼包详情
  todetail(e){
    var that = this;
    var code =e.detail; 
    let memcode = wx.getStorageSync('memcode');
    if (!memcode) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/pages/memDetail/memDetail?procode=' + code+'&type=5',    
    })
  },
   // 登录
   btn_denglu() {
    let that = this;
     wx.navigateTo({
       url: '/pages/login/login',
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
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    if (memcode) {
      that.setData({
        memcode: memcode
      })
      that.get_data();
      that.get_walletinfo();
    } else {
      that.setData({
        memcode: '',
        list: ''
      })
    }
    if (app.globalData.memscroll==true){
      wx.pageScrollTo({
        scrollTop: 934 / 750 * wx.getSystemInfoSync().windowWidth,
        duration: 300
      })
    }
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {
    app.globalData.memscroll=false;
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
    //上拉加载更多调用子组件上拉函数
      wx.stopPullDownRefresh();
      let tab_index = that.data.tab_index;
      let id = "#text" + tab_index;
      that.selectComponent(id).onPullDownRefresh();
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
    var that = this;
    let tab_index = that.data.tab_index;
    let id = "#text" + tab_index;
    //上拉加载更多调用子组件上拉函数
    that.selectComponent(id).onReachBottom();
  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function (res) {
    var that = this;
    let unionid = wx.getStorageSync('unionid');
    if (!unionid) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    if (res.from == 'button'){
      // 来自页面内转发按钮
      return {
        title: res.stocode,
        path: 'packageActivity/pages/index/index?actid=' + res.actid+'&refid=' + unionid,
        // imageUrl: this.data.coverImage,
        success: (res) => {
          // 成功后要做的事情
          // console.log(res)
          wx.showToast({
            title: "分享成功",
            icon: 'none',
            duration: 2000
          })
        },
        fail: function(res) {
          // 分享失败
          // console.log(res)
          wx.showToast({
            title: "分享失败",
            icon: 'none',
            duration: 2000
          })
        }
      }
    }
  }
})