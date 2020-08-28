// components/receivecoupon/receivecoupon.js
import { get_newpeoplecoupon } from '../../utils/server.js';

//获取应用实例
const app = getApp()

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    
  },

  /**
   * 组件的初始数据
   */
  data: {
    show: false,
    couponinfo:''
  },

  /**
   * 组件的方法列表
   */
  methods: {
    show() {
      let that = this;
      that.setData({
        show: true
      })
    },
    hide() {
      let that = this;
      that.setData({
        show: false
      })
    },
    preventTouchMove() {

    },
    // 获取新人优惠券领取列表
    get_newpeoplecoupon(){
      let that = this;
      let memcode = wx.getStorageSync('memcode');
      let unionid = wx.getStorageSync('unionid');
      let data = {
        "actionname": "getreceivenodecouponlist",
        "parameters": {
          'GUID': '',
          'USER_ID': unionid,
          "memcode": memcode
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_newpeoplecoupon(data).then(res => {
        app.globalData.is_newpeoplecoupon = true;
        if (res.code == 0) {
          let couponinfo = res.data;
          if (couponinfo[0].coupons.length > 0){
            that.setData({
              couponinfo: couponinfo[0]
            })
            that.show();
          }
        }else{
          // that.show();
        }
      })
    },
    // 立即领取优惠券
    go_newpeoplecoupon(){
      let that=this;
      let couponinfo = that.data.couponinfo;
      wx.navigateTo({
        url: '/packageVip/pages/newpeoplecoupon/newpeoplecoupon?couponinfo=' + JSON.stringify(couponinfo),
      })
      that.hide();
    }
  }
})
