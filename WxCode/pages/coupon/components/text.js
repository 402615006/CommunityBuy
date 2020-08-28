// pages/order/components/foodcontent/foodcontent.js

import { get_couponnewlist } from '../../../utils/server.js';

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    status:{
      type:Number,
      value:''
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    isno:false,
    list: [],
    isnextpage: 0,
    currentpage: 1,
    pagesize: 10,
    isloadmore: false,
    isnomore: false,
    isonload: true,   //页面是否第一次进入 
    is_coupon:false,   //是否有可以领取的优惠券
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 页面出现函数调取
    onload() {
      let that = this;
      if (that.data.isonload) {
        that.setData({
          isonload: false
        })
        that.get_data()
      }
    },
    // 获取数据
    get_data() {
      let that = this;
      that.setData({
        isloadmore: true
      })
      let unionid = wx.getStorageSync('unionid');
      let memcode = wx.getStorageSync('memcode');
      let data = {
        "actionname": "mymoviecouponlist",
        "parameters": {
          'GUID': '888888888',
          'USER_ID': unionid,
          'memcode': memcode,
          'pagesize': that.data.pagesize,
          'currentpage': that.data.currentpage,
          'type': that.data.status
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_couponnewlist(data).then(res=>{
        that.setData({
          isloadmore: false
        })
        if (res.code == 0) {
          let list = res.data;
          switch (that.data.currentpage) {
            case 1:
              that.setData({
                list: list,
                isnextpage: res.isnextpage
              })
              break;
            default:
              that.setData({
                list: that.data.list.concat(list),
                isnextpage: res.isnextpage
              })
          }
        }else if(res.code==1){
          switch (that.data.currentpage) {
            case 1:
              that.setData({
                isno: true
              })
              break;
            default:
              break
          }
        }
      }).catch(err=>{
        that.setData({
          isloadmore: false
        })
      })
    },
    // 滑动到底部
    lower() {
      let that = this;
      let isnextpage = that.data.isnextpage;
      let isloadmore = that.data.isloadmore;
      if (isnextpage <= 0 || isloadmore == true) {
        return
      }
      that.data.currentpage = that.data.currentpage + 1;
      that.get_data();
    },
    // 去详情
    go_detail(e){
      let that=this;
      let item = e.currentTarget.dataset.item;
      let checkcode = item.checkcode;
      if (item.status==0){
        wx.navigateTo({
          url: '../coupondetail/coupondetail?checkcode=' + checkcode,
        })
      }
    }
  }
})
