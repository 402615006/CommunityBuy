// pages/coupondetail/coupondetail.js
import { get_couponnewdetail, send_couponnews } from '../../utils/server.js';
import { get_px } from '../../utils/util.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    checkcode:'',
    list:'',
    qrwidth:'0',
    qrcolor:'#000'
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    wx.hideShareMenu();
    let checkcode = options.checkcode;
    that.setData({
      checkcode: checkcode,
      qrwidth: get_px('280')
    })
    that.get_data();
  },
  // 点击二维码图片
  previewImage(){
    const that = this.selectComponent('#qrcode')
    wx.canvasToTempFilePath({
      canvasId: 'wux-qrcode',
      success: (res) => {
        wx.previewImage({
          urls: [res.tempFilePath]
        })
      }
    }, that)
  },
  // 获取数据
  get_data(){
    let that=this;
    let unionid = wx.getStorageSync('unionid');
    let data = {
      "actionname": "getcoupondes",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': unionid,
        'couponcode': that.data.checkcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_couponnewdetail(data).then(res=>{
      if(res.code==0){
        console.log(res);
        let list=res.data[0];
        that.setData({
          list:list
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
    
  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function (res) {
    let that = this;
    let list = that.data.list;
    let checkcode = list.checkcode;
    let couname = list.couname;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
   
    let data={
        "actionname": "sharecoupon",
        "parameters":{
          "GUID": "",
          "USER_ID": unionid,
          "couponcode": checkcode,
          "memcode": memcode
        } 
    }
    data.parameters = JSON.stringify(data.parameters);
    send_couponnews(data).then(res => {
      
    })
    return {
      title: couname,
      path: 'pages/index/index?checkcode=' + checkcode,
      imageUrl: '../../images/yhq.jpg',
      success: function () {

      }
    }
  }
})