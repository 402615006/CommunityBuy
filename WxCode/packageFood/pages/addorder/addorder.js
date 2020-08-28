// packageFood/pages/addorder/addorder.js

import { add_order, get_orders } from '../../utils/server.js';
import { get_shopcardlist, add_shopcard } from '../../../utils/util.js'

var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    shopcardlist:[],
    Tprice:0,
    stoname:'',
    stoimg:'',
    logo:'',
    stocode:'',
    list:[],
    remark:''
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    console.log(options)
    let that=this;
    let stocode = options.stocode;
    let stoname = options.stoname;
    that.setData({
      stocode: stocode,
      stoname: stoname
    })
    let shopcardlist=get_shopcardlist(that.data.stocode);
    that.setData({
      shopcardlist: shopcardlist
    })
    that.get_price();
  },
  // 获取已点餐的菜品
  get_orders(){
    let that=this;
    let data = {
      "actionname": "i_getorder",
      "parameters": {
        'key': '',            //商户编号
        'stocode': that.data.stocode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_orders(data).then(res=>{
      console.log(res)
      if(res.code==0){
        let list=res.data;
        that.setData({
          list:list
        })
      }else{
        wx.showToast({
          title: res.msg,
          icon:'none',
          duration:1200
        })
      }
    })
  },
  // 计算价格
  get_price(){
    let that=this;
    let shopcardlist = that.data.shopcardlist;
    let Tprice=0;
    shopcardlist.map((item,index)=>{
      Tprice = Number(Tprice)+Number(item.disnum) * Number(item.price) + Number(item.disnum) * Number(item.cookmoney);
    })
    that.setData({
      Tprice: Tprice.toFixed(2)
    })
  },
  get_Remark(e){
    this.setData({
      remark:e.detail.value
    })
  },
  // 下单
  set_addOrders(){
    let that=this;
    let shopcardlist = that.data.shopcardlist;
    let memcode = wx.getStorageSync('memcode');
    let openid= wx.getStorageSync('openid');
    let nickname= wx.getStorageSync('nickname');
    let remark= that.data.remark;
    let dishlist=[];
    let disnum=0;
    if (that.data.Tprice == 0) {
      wx.showToast({
        title: '订单错误',
        duration: 1500,
        mask: true,
        icon: 'none'
      })
      return
    }

    //设置菜品
    shopcardlist.map((item,index)=>{
      dishlist.push({
        'disname': item.disname,
        'discode': item.discode,
        'disnum': item.disnum,      //数量
        'cookname': item.cookname,      //做法名称
        'cookmoney': item.cookmoney,      //做法加价
        'price':item.price
      })
        disnum = Number(disnum) + Number(item.disnum)
    })
    //设置订单
    let data = {
      "actionname": "addorder",
      "parameters": {
        'key': '',
        'stocode':that.data.stocode ,             //门店编号
        'remark':remark,          //整体备注
        'ordermoney':that.data.Tprice,         //订单金额
        'memcode': memcode,           //顾客编号
        'openid':openid,
        'nickname':nickname,
        'dishlist':dishlist
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    add_order(data).then(res=>{
      if(res.code==1){
        let orders=res.msg;
          wx.redirectTo({
            url: '../kuaicanzhifu/kuaicanzhifu?order=' + orders + '&stocode=' + that.data.stocode
          })
        add_shopcard(that.data.stocode,[])//清空购物车
      }else{
        wx.showToast({
          title: res.msg,
          icon:'none',
          mask:true,
          duration:1500
        })
      }
    })
  },
  wxPay(ordercode,stocode){
    wechat_payment(list.timeStamp, list.nonceStr, list.package, list.signType, list.paySign).then(res =>{
      that.triggerEvent('wx_update');
    }).catch(err => {
      wx.showToast({
        icon: "none",
        title: '支付失败',
        duration: 2000,
      })
    })
  },
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {},
  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {},

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {},

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {},

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {},

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {}
})