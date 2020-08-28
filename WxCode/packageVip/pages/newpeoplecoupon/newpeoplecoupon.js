
import { receive_newpeoplecoupon } from '../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    show:false,
    status:0,
    couponinfo: {
      // "pcode": "P79014",
      // "pname": "节点礼包",
      // "actimg": "/uploads/stock/original/20191128/20191128170932.jpg",
      // "actDesc": "1.2019年11月1日起，小程序新注册用户可在活动页面领取新人礼包。",
      // "coupons": [
        // {
        //   "pcode": "P79014",
        //   "couname": "听装七喜券",
        //   "sdate": "2019.11.29",
        //   "edate": "2019.11.30",
        //   "sumcode": "SUM79010",
        //   "mccode": "MAI79012",
        //   "minsinglemoney": "0.00",
        //   "storename": "北晟-胡桃里餐吧,东街-美亚巨幕影城",
        //   "descr": ""
        // },
        // {
        //   "pcode": "P79014",
        //   "couname": "听装七喜券",
        //   "sdate": "2019.11.29",
        //   "edate": "2019.11.30",
        //   "sumcode": "SUM79010",
        //   "mccode": "MAI79012",
        //   "minsinglemoney": "0.00",
        //   "storename": "北晟-胡桃里餐吧,东街-美亚巨幕影城",
        //   "descr": ""
        // },
        // {
        //   "pcode": "P79014",
        //   "couname": "听装七喜券",
        //   "sdate": "2019.11.29",
        //   "edate": "2019.11.30",
        //   "sumcode": "SUM79010",
        //   "mccode": "MAI79012",
        //   "minsinglemoney": "0.00",
        //   "storename": "北晟-胡桃里餐吧,东街-美亚巨幕影城",
        //   "descr": ""
        // },
        // {
        //   "pcode": "P79014",
        //   "couname": "听装七喜券",
        //   "sdate": "2019.11.29",
        //   "edate": "2019.11.30",
        //   "sumcode": "SUM79010",
        //   "mccode": "MAI79012",
        //   "minsinglemoney": "0.00",
        //   "storename": "北晟-胡桃里餐吧,东街-美亚巨幕影城",
        //   "descr": ""
        // },
        // {
        //   "pcode": "P79014",
        //   "couname": "听装七喜券",
        //   "sdate": "2019.11.29",
        //   "edate": "2019.11.30",
        //   "sumcode": "SUM79010",
        //   "mccode": "MAI79012",
        //   "minsinglemoney": "0.00",
        //   "storename": "北晟-胡桃里餐吧,东街-美亚巨幕影城",
        //   "descr": ""
        // },
        // {
        //   "pcode": "P79014",
        //   "couname": "听装七喜券",
        //   "sdate": "2019.11.29",
        //   "edate": "2019.11.30",
        //   "sumcode": "SUM79010",
        //   "mccode": "MAI79012",
        //   "minsinglemoney": "0.00",
        //   "storename": "北晟-胡桃里餐吧,东街-美亚巨幕影城",
        //   "descr": ""
        // },
        // {
        //   "pcode": "P79014",
        //   "couname": "听装七喜券",
        //   "sdate": "2019.11.29",
        //   "edate": "2019.11.30",
        //   "sumcode": "SUM79010",
        //   "mccode": "MAI79012",
        //   "minsinglemoney": "0.00",
        //   "storename": "北晟-胡桃里餐吧,东街-美亚巨幕影城",
        //   "descr": ""
        // },
        // {
        //   "pcode": "P79014",
        //   "couname": "听装七喜券",
        //   "sdate": "2019.11.29",
        //   "edate": "2019.11.30",
        //   "sumcode": "SUM79010",
        //   "mccode": "MAI79012",
        //   "minsinglemoney": "0.00",
        //   "storename": "北晟-胡桃里餐吧,东街-美亚巨幕影城",
        //   "descr": ""
        // }
      // ],
    },
  },
  // 禁止页面滑动
  preventTouchMove(){

  },
  // 打开规则详情
  click_guizedetail(){
    let that=this;
    that.setData({
      show:true
    })
  },
  // 关闭详情
  click_cendetail(){
    let that = this;
    that.setData({
      show: false
    })
  },
  // 领取优惠券
  receive_newpeoplecoupon(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let data = {
      "actionname": "receivenodecoupon",
      "parameters": {
        'GUID': '',
        'USER_ID': unionid,
        'pcode': that.data.couponinfo.pcode,
        "memcode": memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    receive_newpeoplecoupon(data).then(res=>{
      if(res.code==0){
        wx.showToast({
          title: '领取成功',
          icon: 'success',
          duration: 1500,
          mask: true
        })
        that.setData({
          status:1
        })
      }else{
        wx.showToast({
          title: res.msg,
          icon:'none',
          duration:1500,
          mask:true
        })
      }
    })
  },
  // 去使用
  bo_back(){
    wx.navigateBack();
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    if (options.couponinfo){
      that.setData({
        couponinfo: JSON.parse(options.couponinfo)
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