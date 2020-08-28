
import { get_mywalletvipcardlist, get_myaccountpaycode, get_mypaystatus,get_mypaypassword } from '../../utils/server.js';
import { get_px } from '../../../utils/util.js';

var wxbarcode = require('../../../utils/wxbarcode.js');

Page({

  /**
   * 页面的初始数据
   */
  data: {

    timsInterval:'',
    tims:0,
    falg:true,
    index:0,
    array:[],
    flag:false,
    list:[],
    qrcolor:'#000',
    qrwidth: get_px('270'),
    value:'',
    cardpaycode:'',
    qrshow:true
  },
  bindPickerChange(e){
    let that=this;
    let value=e.detail.value;
    // if (value==that.data.index){
    //   return
    // }
    that.setData({
      index:value
    })
    that.new_code();
  },
  // 获取会员卡
  get_data(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "myaccountinfolist",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'memcode': memcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_mywalletvipcardlist(data).then(res=>{
      if(res.status==0){
        let list=res.data;
        let array=[];
        list.map((item,index)=>{
          array.push(item.levelname)
        })
        that.setData({
          list:list,
          array:array,
          flag:true
        })
        that.new_code();
      }else{
        wx.showToast({
          title: res.mes,
          icon:'none',
          mask:true,
          duration:1500
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options){
    let that=this;
    that.get_data();
  },
  newget_data(){
    let that=this;
    let falg = that.data.falg;
    if (!falg){
      that.new_code();
    }
  },
  // 计时器函数
  get_tims(){
    let that=this;
    let tims = that.data.tims;
    clearInterval(that.data.timsInterval);
    that.setData({
      tims:0
    })
    that.setData({
      timsInterval: setInterval(function () {
        that.timsonload();
      }, 1000)
    })
  },
  // 倒计时
  timsonload(){
    let that=this;
    let tims = that.data.tims;
    if (tims>2){
      that.get_paystatus();
      clearInterval(that.data.timsInterval);
    }else{
      tims = tims+1;
      that.setData({
        tims: tims
      })
    }
  },
  // 获取二维码
  new_code(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    if (that.data.list.length==0){
      return
    }
    let cardcode = that.data.list[that.data.index].cardcode;
    let data = {
      "actionname": "myaccountpaycode",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'memcode': memcode,
        'cardcode': cardcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_myaccountpaycode(data).then(res=>{
      if (res.status==0){
        clearInterval(that.data.timsInterval);
        // let value = res.data.cardpaycode;
        let cardpaycode = res.data.cardpaycode;
        that.setData({
          falg:true,
          value: cardpaycode,
          cardpaycode: cardpaycode
        })
        setTimeout(()=>{
          that.setData({
            falg:false
          })
        },5000)
        wxbarcode.barcode('barcode', cardpaycode, 495, 120);
        // wxbarcode.qrcode('qrcode', value, 270, 270);
        // that.get_tims();
        // 调取获取二维码支付状态
        that.get_paystatus();
      }else{
        wx.showToast({
          title: res.mes,
          icon:'none',
          duration:1500,
          mask:true
        })
      }
    })
  },
  //去办理会员卡
  go_vip(){
    wx.redirectTo({
      url: '/packageVip/pages/cardlist/cardlist'
    })
  },
  // 获取二维码支付状态
  get_paystatus(){
    let that=this;
    let data = {
      "actionname": "querypaycode",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'cardpaycode': that.data.cardpaycode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_mypaystatus(data).then(res=>{
      if(res.status==0){
      // 待支付状态继续轮询
        that.get_tims();
      }else if(res.status==1){
        // 需要密码调出密码输入框
        that.setData({
          qrshow: false
        })
        that.selectComponent("#password").showModal();
      }else if(res.status==2){
        // 支付成功跳转到成功页面
        let payresult = res.payresult;
        wx.redirectTo({
          url: '../scancodeOK/scancodeOK?payresult=' + payresult.expend,
        })
      } else if (res.status == 6){
        // 二维码失效重新刷新调取二维码接口
        // that.new_code();
      }else{
        // 其他错误提示
        wx.showToast({
          title: res.mes,
          duration:2000,
          icon:'none'
        })
      }
    })
  },
  // 输入密码完成之后验证
  get_number_ok(e){
    let that=this;
    let pwd = e.detail;
    let data = {
      "actionname": "paycodecheckpwd",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'cardpaycode': that.data.cardpaycode,
        'pwd': pwd
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_mypaypassword(data).then(res=>{
      if(res.status==2){
      // 密码输入完成支付成功跳转到成功页面
        let payresult = res.payresult;
        wx.redirectTo({
          url: '../scancodeOK/scancodeOK?payresult=' + payresult.expend,
        })
      } else if (res.status == 5){
        // 密码输入错误弹出是否重新输入密码确认框
        wx.showModal({
          title: res.mes,
          content: '是否重试?',
          success(res) {
            if (res.confirm) {
              that.setData({
                qrshow:false
              })
              that.selectComponent("#password").showModal();
            }
          }
        })
      } else {
        // 其他错误提示
        wx.showToast({
          title: res.mes,
          duration: 2000,
          icon: 'none'
        })
      }
    })
  },
  // 密码框消失
  hiden(){
    let that=this;
    setTimeout(()=>{
      that.setData({
        qrshow:true
      })
    },300)
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
    let that=this;
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
    let that=this;
    clearInterval(that.data.timsInterval);
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