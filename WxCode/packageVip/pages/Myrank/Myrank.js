
import { get_myviplevel } from '../../utils/server.js';
var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    memlevel:'',
    pro:'',
    gift:''
  },
  // 去升级会员
  go_Myrank() {
    app.globalData.memscroll = true;
    wx.switchTab({
      url: '/pages/member/member',
    })
  },
  // 获取数据
  get_data(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "myviplevel",
      "parameters": {
        "GUID": "",
        "USER_ID": "",
        "memcode": memcode
      }
    };
    data.parameters = JSON.stringify(data.parameters);
    get_myviplevel(data).then(res=>{
      if(res.status==0){
        let memlevel = res.memlevel;
        let pro = res.pro;
        let gift = res.gift;
        let expdate = res.expdate;
        that.setData({
          memlevel: memlevel,
          pro: pro,
          gift: gift,
          expdate: expdate
        })
      }else{
        wx.showToast({
          title: res.mes,
          icon:'none',
          duration:1500
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    that.get_data();
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
  onShareAppMessage: function () {

  }
})