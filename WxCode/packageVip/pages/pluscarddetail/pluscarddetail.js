// packageVip/pages/pluscarddetail/pluscarddetail.js

import { get_memcardinfo, setdefaultmemcard, untied_vipcards } from '../../utils/server.js';
import { verificationcardpwd } from '../../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    checked:true,
    list:'',
    cardcode:''
  },
  // 默认会员卡按钮点击
  btn_checked(){
    let that=this;
    let type='';
    let list=that.data.list;
    if (list.isdefault==1){
      type=0
    }else{
      type=1
    }
    let data = {
      "actionname": "setdefaultmemcard",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'cardcode': that.data.cardcode,
        'type': type
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    setdefaultmemcard(data).then(res=>{
      if(res.status==0){
        if (list.isdefault == 1){
          list.isdefault = 0
        }else{
          list.isdefault = 1
        }
        that.setData({
          list:list
        })
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
  // 去使用记录
  go_expensesrecord(){
    let that=this;
    wx.navigateTo({
      url: '../expensesrecord/expensesrecord?cardcode=' + that.data.cardcode
    })
  },
  // 解绑会员卡
  go_unbindcard(){
    let that=this;
    let levelname = that.data.list;
    // wx.navigateTo({
    //   url: '../unbindcard/unbindcard?cardcode=' + that.data.cardcode + '&levelname=' + levelname
    // })
    this.selectComponent("#password").showModal();
  },
  // 密码输入完成判断是否正确
  get_number_ok(e){
    let that=this;
    let pwd=e.detail;
    // let unionid = wx.getStorageSync('unionid');
    // let data = {
    //   "actionname": "pwdcheck",
    //   "parameters": {
    //     'GUID': '88888888',
    //     'unionid':unionid,
    //     'pwd': pwd
    //   }
    // }
    // data.parameters = JSON.stringify(data.parameters);
    // verificationcardpwd(data).then(res=>{
    //   console.log(res) ;
    //   if(res.status==0){
    //     that.cen_bind();
    //   }else{
    //     wx.showToast({
    //       title: res.mes,
    //       icon:'none',
    //       mask:true,
    //       duration:1500
    //     })
    //   }
    // })
    that.cen_bind(pwd);
  },
  // 解绑
  cen_bind(password){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let data = {
      'actionname': 'unbindmemcard',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid ,
        'memcode': memcode,
        'cardcode': that.data.cardcode,
        'pass': password
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    untied_vipcards(data).then(res => {
      if (res.status == 0) {
        wx.redirectTo({
          url: '/pages/success/success?title=解绑成功'+'&go_back=2'
        })
      } else {
        wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
    })
  },
  // 去充值
  btn(){
    let that=this;
    let cardcode = that.data.cardcode
    wx.navigateTo({
      url: '../recharge/recharge?cardcode=' + cardcode,
    })
  },
  // 获取数据
  get_data(){
    let that=this;
    let data = {
      "actionname": "getmemcardinfo",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'cardcode': that.data.cardcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_memcardinfo(data).then(res => {
      if (res.status == 0) {
        console.log(res)
        let list = res.data;
        if (list.imgPaths){
          list.imgPaths = list.imgPaths.split(',')[0];
        }
        that.setData({
          list: list
        })
      } else {
        wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    let cardcode = options.cardcode;
    that.setData({
      cardcode: cardcode
    })
    that.get_data()
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