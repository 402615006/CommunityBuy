// components/receivecoupon/receivecoupon.js
import { get_couponnews } from '../../utils/server.js';

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    checkcode:{
      type:String,
      value:''
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    show: false,
    dicname:'',
    couname:''
  },

  /**
   * 组件的方法列表
   */
  methods: {
    show(){
      let that=this;
      that.setData({
        show:true
      })
    },
    // 去我的优惠券
    go_coupon(){
      let that=this;
      that.hide();
      wx.navigateTo({
        url: '../coupon/coupon',
      })
    },
    hide(){
      let that = this;
      that.setData({
        show: false
      })
    },
    preventTouchMove(){
      
    },
    //领取好友赠送的优惠券
    get_couponnews() {
      let that = this;
      if (!that.data.checkcode) {
        return
      }
      let memcode = wx.getStorageSync('memcode');
      let unionid = wx.getStorageSync('unionid');
      let data = {
        "actionname": "getsharecoupon",
        "parameters": {
          "GUID": "",
          "USER_ID": unionid,
          "couponcode": that.data.checkcode,
          "memcode": memcode
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_couponnews(data).then(res => {
        console.log(res);
        if (res.status == 0) {
          that.triggerEvent('clear_checkcode');
          that.setData({
            dicname: res.dicname,
            couname: res.couname
          })
          that.show();
        } else {
          wx.showToast({
            title: res.mes,
            icon: 'none',
            duration: 2000
          })
        }
      })
    },
  }
})
