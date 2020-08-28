

import { get_memcolllist, attention } from '../../utils/server.js';
import { baserURLOrganization } from '../../../utils/api.js'
Page({

  /**
   * 页面的初始数据
   */
  data: {
    info:'',
    json:[],
    isloadmore:false,
    baserURLOrganization: baserURLOrganization,
    isno:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    if (options.item){
      let item = decodeURIComponent(options.item);
      item = JSON.parse(item);
      that.setData({
        info:item
      })
      that.get_data(item.memcode);
    }
  },
  // 取消关注
  cen_gaunzhu(e) {
    let that = this;
    let ymemcode = e.currentTarget.dataset.memcode;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "lookcollemp",
      "parameters": {
        "GUID": "",
        "USER_ID": "",
        "ymemcode": ymemcode,
        "memcode": memcode,
        "type": "0"
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    attention(data).then(res => {
      if (res.code == 0) {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500,
        })
        let pages = getCurrentPages();
        let prePage = pages[pages.length - 2];
        prePage.setData({
          list:[],
          currentpage:1
        })
        prePage.get_data();
        setTimeout(()=>{
          wx.navigateBack();
        },1000)
      } else {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500
        })
      }
    })
  },
  // 获取数据
  get_data(memcode){
    let that=this;
    that.setData({
      isloadmore:true
    })
    let data = {
      "actionname": "memcolllist",
      "parameters":{
        "GUID": "",
        "USER_ID": "",
        "memcode": memcode,
        "jcode": "0",
        "wcode": "0",
        "currentpage": "1",
        "pagesize": "100"
      } 
    }
    data.parameters = JSON.stringify(data.parameters);
    get_memcolllist(data).then(res=>{
      if(res.code==0){
        let list=res.data;
        list.map((item, index) => {
          item.schedule = (item.cjnum / item.collcount * 100).toFixed(2) + '%';
          let date1 = new Date(item.startdate);
          let date2 = new Date(item.enddate);
          let y = date1.getMonth() + 1;
          let d = date1.getDate();
          let h = date1.getHours() < 10 ? '0' + date1.getHours() : date1.getHours();
          let m = date1.getMinutes() < 10 ? '0' + date1.getMinutes() : date1.getMinutes();
          let h2 = date2.getHours() < 10 ? '0' + date2.getHours() : date2.getHours();
          let m2 = date2.getMinutes() < 10 ? '0' + date2.getMinutes() : date2.getMinutes();
          let ly = y + '月' + d + '日';
          item.date = ly + ' ' + h + ':' + m + '-' + h2 + ':' + m2;
          item.imgaug = item.cjinfos.split(',');
        })
        that.setData({
          list:list,
          isloadmore:false
        })
      }else{
        that.setData({
          list:[],
          isloadmore: false,
          isno:true
        })
      }
    })
  },
  // 去详情
  go_detail(e) {
    let that = this;
    let collcode = e.currentTarget.dataset.collcode;
    wx.navigateTo({
      url: '/packageOrganization/pages/Senate/Senate?collcode=' + collcode,
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
  onShareAppMessage: function () {

  }
})