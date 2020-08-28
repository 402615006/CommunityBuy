// packageFood/pages/Stall/Stall.js；

import { get_getdepart } from '../../utils/server.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    stoname:'',
    buscode: '',     //商户编号
    stocode:'',     //门店编号
    tablecode:'',   //桌台编号
    list:[],
    stoimg:'',
    logo:'',
    opencode:''
  },
  // 去点菜
  go_list(e){
    let that=this;
    let item = e.currentTarget.dataset.item;
    let departcode = '';
    let menucode = '';
    if (item.pdcode){
      departcode = item.pdcode
    }
    if (item.MenuCode){
      menucode = item.MenuCode
    }
    wx.navigateTo({
      url: '../list/list?stocode=' + that.data.stocode + '&buscode=' + that.data.buscode + '&tablecode=' + that.data.tablecode + '&menucode=' + menucode + '&departcode=' + departcode + '&stoname=' + that.data.stoname + '&ptype=' + that.data.list.ptype+'&opencode='+that.data.opencode,
    })
  },
  // 获取数据
  get_data(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "i_getdepart",
      "parameters": {
        'key':'',
        'buscode': that.data.buscode,
        'stocode': that.data.stocode,
        'page': that.data.tablecode,
        'pagesize': memcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_getdepart(data).then(res=>{
      if(res.code==0){
        let list=res.data;
        that.setData({
          list: list
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
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    console.log(options)
    let that=this;
    let buscode = options.buscode;
    let stocode = options.stocode;
    let tablecode = options.tablecode;
    let stoname = options.stoname;
    that.setData({
      buscode: buscode,
      stocode: stocode,
      tablecode: tablecode,
      stoname: stoname
    })
    if (options.opencode){
      that.data.opencode = options.opencode;
    }
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

  }
})