
import { refund_coupon, cancel_coupon, get_coupon} from '../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    stocode:'',
    buscode:'',
    billcode:'',
    isreq:false,
    list:[],
    couponlist:[],
    couponmoney:''
  },
  // 点击优惠券
  select_img(e){
    let that=this;
    console.log(e);
    let item = e.currentTarget.dataset.item;
    let index = e.currentTarget.dataset.index;
    let list=that.data.list;
    if(item.checked==true){
      // 取消优惠券
      that.cancel_coupon(e);
    }else{
      let data = {
        "actionname": "i_usecoupon",
        "parameters": {
          'key':'',
          'buscode': that.data.buscode,
          'stocode': that.data.stocode,
          'billcode': that.data.billcode,
          'couponinfo':item
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      refund_coupon(data).then(res=>{
        if(res.code==0){
          list[index].checked=true;
          that.setData({
            list:list
          })
          let bill=res.bill[0];
          let coupon = res.coupon;
          let paylist = res.pay;
          let pages = getCurrentPages();
          let prePage = pages[pages.length - 2];
          prePage.setData({
            bill: bill,
            coupon: coupon,
            paylist: paylist,
          })
          that.setData({
            couponlist: coupon,
            couponmoney: bill.CouponMoney
          })
        }else{
          wx.showToast({
            title: res.msg,
            icon:'none',
            duration:2000,
            mask:true
          })
        }
      })
    }
  },
  // 获取优惠券
  get_coupon(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "getcouponsvalid",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        'buscode': that.data.buscode,
        'stocode': that.data.stocode,
        'memcode': memcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_coupon(data).then(res => {
      let list=[];
      let couponlist = that.data.couponlist;
      couponlist.map((item,index)=>{
        item.couname = item.CouponName;
        item.checked=true;
        item.checkcode = item.CouponCode
      })
      if (res.status == 0) {
        list = res.data;
        if (list.length > 0) {
          list.map((item, index) => {
            item.checked = false;
          })
        }

      } else {
        wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
      that.setData({
        list: couponlist.concat(list),
        isreq:true
      })
    })
  },
  // 取消使用优惠券
  cancel_coupon(e){
    let that=this;
    let item = e.currentTarget.dataset.item;
    let index = e.currentTarget.dataset.index;
    let list = that.data.list;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "i_cancelcoupon",
      "parameters": {
        'key': '',
        'buscode': that.data.buscode,
        'stocode': that.data.stocode,
        'billcode': that.data.billcode,
        'couponcode': item.checkcode,
        'memcode': memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    cancel_coupon(data).then(res=>{
      if(res.code==0){
        let bill = res.bill[0];
        let coupon = res.coupon;
        let paylist = res.pay;
        let pages = getCurrentPages();
        let prePage = pages[pages.length - 2];
        that.setData({
          couponlist: coupon,
          couponmoney: bill.CouponMoney
        })
        prePage.setData({
          bill: bill,
          coupon:coupon,
          paylist: paylist
        })
        that.get_coupon();
        // list[index].checked = false;
        // that.setData({
        //   list: list
        // })
        // let bill = res.bill[0];
        // let pages = getCurrentPages();
        // let prePage = pages[pages.length - 2];
        // prePage.setData({
        //   bill: bill
        // })
      }else{
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 2000,
          mask: true
        })
      }
    })
  },
  // 确定
  submit(){
    wx.navigateBack();
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    console.log(options)
    // if(options.list){
    //   that.setData({
    //     list: JSON.parse(options.list)
    //   })
    // }
    if (options.stocode){
      that.setData({
        stocode: options.stocode
      })
    }
    if (options.buscode){
      that.setData({
        buscode: options.buscode
      })
    }
    if (options.billcode){
      that.setData({
        billcode: options.billcode
      })
    }
    let pages = getCurrentPages();
    let prePage = pages[pages.length - 2];
    let couponlist = prePage.data.coupon;
    let couponmoney = prePage.data.bill.CouponMoney;
    that.setData({
      couponlist: couponlist,
      couponmoney: couponmoney
    })
    that.get_coupon();
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

  }
})