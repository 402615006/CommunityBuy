var { is_gologin } = require('../../utils/util.js');
import { get_walletinfo, set_enterPageAct} from '../../utils/server.js';

//获取应用实例
var app = getApp();
Page({

  /**
   * 页面的初始数据
   */
  data: {
    memcode: '',
    tel: '',
    list: ''
  },
  
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {

    // 进入页面记录页面
    set_enterPageAct('钱包', '', '1', '');
  },
  // 登录
  btn_denglu() {
    let that = this;
    wx.navigateTo({
      url: '/pages/login/login',
    })
  },
  // 获取数据
  get_data() {
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
        let tel = list.mobile;
        that.setData({
          list: list,
          tel: tel
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
  // 退出
  clear() {
    let that = this;
    wx.showLoading({
      mask: true,
      title: '退出登录...',
    })
    wx.clearStorageSync();
    setTimeout(() => {
      wx.hideLoading()
      that.setData({
        list: '',
        memcode: ''
      })
    }, 1000)
  },
  // 去个人信息
  goto_grxx(){
    let that = this;
    let list = that.data.list;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/grxx/grxx',
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
  // 去付款码
  go_fukuan() {
    let that = this;
    let list = that.data.list;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/qrcode/qrcode'
    })
  },
  // 去提现
  go_commission(){
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
  //去总资产
  go_assets() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/assets/assets?balance=' + that.data.list.balance
    })
  },
  //去积分商城
  go_integral() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/index/index'
    })
  },
  // 去我的会员
  go_vip() {
    let that=this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/Mymember/Mymember'
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
  //去有优惠券
  go_coupon() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '../coupon/coupon'
    })
  },
  //订单
  go_order() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '../order/order'
    })
  },
  // 我的关注
  go_guanzhu(){
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/myFocus/myFocus'
    })
  },
  // 关注我的人
  go_guanzhumy(){
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageOrganization/pages/Focusmy/Focusmy'
    })
  },
  //去排队记录
  go_phjl() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageFood/pages/phorder/phorder'
    })
  },
  // 去预约记录
  go_yyjl() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageFood/pages/yyorder/yyorder'
    })
  },
  // 账户安全
  go_safety() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/safety/safety'
    })
  },
  //反馈
  go_feedback() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '../feedback/feedback'
    })
  },
  //客服
  go_kefu() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    wx.navigateTo({
      url: '/packageVip/pages/service/service'
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
    } else {
      that.setData({
        memcode: '',
        tel: '',
        list: ''
      })
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
  onShareAppMessage: function () {

  }
})