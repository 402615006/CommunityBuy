// packageFood/pages/checkorder/checkorder.js
import {  get_orders } from '../../utils/server.js';
var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    list:[],
    money:0,
    stocode:'',
    buscode:'',
    billcode:'',
    logo:'',
    stoimg:''
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    let stocode = options.stocode;
    let buscode = options.buscode;
    let opencode = options.opencode;
    let stoimg = app.stoimg;
    let logo = app.logo;
    that.setData({
      stocode: stocode,
      buscode: buscode,
      opencode: opencode,
      stoimg: stoimg,
      logo: logo
    })
    if (options.billcode){
      that.setData({
        billcode: options.billcode
      })
    }
    that.get_order();
  },
  // 获取订单信息
  get_order(){
    let that=this;
    let data = {
      "actionname": "i_getorder",
      "parameters": {
        'key': '',
        'buscode': that.data.buscode,              //商户编号
        'stocode': that.data.stocode,             //门店编号
        'opencode': that.data.opencode                        //开台编号
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_orders(data).then(res=>{
      console.log(res);
      if(res.code==0){
        let list=res.data;
        list.map((item,index)=>{
          item.dishList = JSON.parse(item.dishList)
        })
        that.setData({
          list:list
        })
        that.get_price()
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
  // 计算价格
  get_price(){
    let that=this;
    let list=that.data.list;
    let money=0;
    list.map((item,index)=>{
      if(item.status!=3){
        money = Number(money) + Number(item.money);
      }
    })
    that.setData({
      money: money.toFixed(2)
    })
  },
  // 去付款
  go_fukuan(){
    let that=this;
    let list=that.data.list;
    let order='';
    if(!(list.length>0)){
      wx.showToast({
        title: '请先去下单',
        icon:'none',
        duration:1500,
        mask:true
      })
      return
    }
    list.map((item,index)=>{
      if(item.status==1){
        if (order==''){
          order = item.code
        }else{
          order = order + ',' + item.code;
        }
      }
    })
    wx.redirectTo({
      url: '../kuaicanzhifu/kuaicanzhifu?order=' + order + '&stocode=' + that.data.stocode + '&buscode=' + that.data.buscode + '&billcode=' + that.data.billcode,
    })
  },
  // 继续点菜
  fanhui(){
    wx.navigateBack();
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