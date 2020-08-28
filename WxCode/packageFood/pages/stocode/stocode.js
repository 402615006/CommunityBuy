// packageFood/pages/stocode/stocode.js
var util = require('../../../utils/util.js');
var { is_gologin, getUrlParam } = require('../../../utils/util.js');
import { get_stocodedetail, open_table } from '../../utils/server.js';

import { topprolist } from '../../../utils/server.js';
import { baserURLOrganization } from '../../../utils/api.js';

//获取应用实例
var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    stocode: '', //门店编号
    logo:'',     //门店图标
    list:'',    //门店信息
    juli:'',    //距离
    url: baserURLOrganization,
    currentpage:1,  
    pagesize:10,
    isnextpage:0,
    toplist:[],    //门店商品集合

    bannerlist: [],  //轮播图
  },

  //点击轮播图放大
  previewImg(e){
    let that = this;
    console.log(e);
    let index = e.currentTarget.dataset.index;
    let bannerlist = that.data.bannerlist;
    let src = bannerlist[index];
    wx.previewImage({
      current: src,     //当前图片地址
      urls: bannerlist
    })
  },
  //拨打电话
  calling: function () {
    let that=this;
    wx.makePhoneCall({
      phoneNumber: that.data.list.tel, //此号码并非真实电话号码，仅用于测试
      success: function () {
        console.log("拨打电话成功！")
      },
      fail: function () {
        console.log("拨打电话失败！")
      }
    })
  },
  // 去门店详情
  go_stodetail(){
    wx.navigateTo({
      url: '/packageFood/pages/stodetail/stodetail',
    })
  },
  // 去点餐
  go_list(){
    let that = this;
    let list=that.data.list;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    } 
    // 不是美食城直接菜品列表
    wx.navigateTo({
      url: '../list/list?buscode=' + that.data.buscode + '&stocode=' + that.data.stocode + '&tablecode=' + that.data.tablecode + '&menucode=' + that.data.menucode + '&departcode=' + that.data.departcode + '&stoname=' + list.cname + '&ptype=' + that.data.list.ptype + '&opencode=' + that.data.opencode,
    })

  },

  //预约
  clickyy(){
    let that=this;
    let list = that.data.list;
    if (is_gologin()) {
      that.selectComponent("#yypopup").showlogin();
    } else {
      wx.navigateTo({
        url: '/pages/login/login',
      })
    }
  },
  //会员卡
  clickhuiyuanka(){
    let that=this;
    let stocode = that.data.stocode;
    wx.navigateTo({
      url: '/pages/vip/vip?stocode=' + stocode,
    })
  },
  // 领取优惠券
  receive_coupon(){
    wx.navigateTo({
      url: '/packageVip/pages/couponcenter/couponcenter',
    })
  },
  // 门店详情数据
  get_data(){
    let that=this;
    let memcode = '';
    let mobile = ''
    if (wx.getStorageSync('memcode')) {
      memcode = wx.getStorageSync('memcode');
    }
    if (wx.getStorageSync('mobile')) {
      mobile = wx.getStorageSync('mobile')
    }
    let data = {
      "actionname": "getwxstodetail",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        'stocode': that.data.stocode,
        'membercode': memcode,
        'phone': mobile,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_stocodedetail(data).then(res=>{
      if(res.code==0){
        let list=res.data1[0];
        let data2=res.data2;
        let data3=res.data3;
        let bannerlist=[];
        let logo='';
        if (list.cname){
          wx.setNavigationBarTitle({
            title: list.cname
          })
        }
        if (list.stopathname){
          bannerlist = list.stopathname.split(',')
        }
        if (list.logo){
          logo = list.logo;
        }
        that.setData({
          list:list,
          data2: data2,
          data3: data3,
          bannerlist: bannerlist,
          logo: logo
        })
        app.stoimg = that.data.bannerlist[0];
        app.logo = that.data.logo;
        that.get_juli();
      }
    })
  },
  // 开台
  open_table() {
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "i_opentable",
      "parameters": {
        'key': '',
        'buscode': that.data.buscode, //商户编号
        'stocode': that.data.stocode, //门店编号
        'tablecode': that.data.tablecode, //桌台编号
        'memcode': memcode, //会员编号
        'paytype': '1'
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    open_table(data).then(res => {
      if (res.code == 0) {
          that.data.opencode=res.msg;
        if (res.ordernum > 0 || res.billnum>0) {
          let billcode = '';
          if (billnum > 0) {
            billcode = bill[0].PKCode;
          }
          wx.navigateTo({
            url: '../checkorder/checkorder?buscode=' + that.data.buscode + '&stocode=' + that.data.stocode + '&opencode=' + that.data.opencode + '&billcode=' + billcode,
          })
        }
      } else {
        wx.showToast({
          title: res.msg,
          duration: 1500,
          mask: true,
          icon: 'none'
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
      let that=this;
      // 从门店列表进入有门店
      if (options.stocode){
        that.setData({
          stocode: options.stocode
        })
      }
      // 扫码进入有桌台门店信息
      if (options.scene){
        let scene = options.scene; 
        let stocode = scene.split('-')[0];
        let tablecode = scene.split('-')[1];
        if (stocode){
          that.setData({
            stocode:stocode
          })
        }
        if (tablecode){
          that.data.tablecode = tablecode;
        }
      }
      // 有桌台编号
      if (that.data.tablecode){
        if (is_gologin()){
          // 有桌台号调开台接口
          that.open_table();
        }
      }
      // 有门店编号
      if (that.data.stocode){
        // 获取门店数据
        that.get_data();
        // 获取闲弟推荐
        that.get_topprolist();
      }else{
        wx.showToast({
          title: '桌台码错误',
          duration:2000,
          icon:'none'
        })
      }
  },
  // 登录成功
  onMyEvent(){
    let that=this;
    if (that.data.tablecode){
        that.open_table();
    }
  },
  // 打开地图
  go_map(e){
    let that=this;
    let item = e.currentTarget.dataset.item;
    if (item.stocoordx && item.stocoordy){
      wx.openLocation({
        latitude: Number(item.stocoordx) ,
        longitude: Number(item.stocoordy),
        name: item.cname,
        address: item.address,
        scale: 28
      })
    }
  },
  // 闲弟推荐
  get_topprolist(){
    let that = this;
    let position = wx.getStorageSync('position');
    let jcode = '';
    let wcode = '';
    if (position) {
      jcode = position.longitude;
      wcode = position.latitude;
    }
    let data = {
      'actionname': 'topprolist',
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "stotype": 0,
        "order": '0',
        "status": '2',
        "jcode": wcode,
        "wcode": jcode,
        "currentpage": that.data.currentpage,
        "pagesize": that.data.pagesize,
        "stocode": that.data.stocode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    topprolist(data).then(res => {
      console.log(res)
      if (res.code == 0) {
        let list = res.data;
        for (var i = 0; i < list.length; i++) {
          list[i].smallimg = that.data.url + list[i].smallimg;
        }
        that.setData({
          toplist: that.data.toplist.concat(list),
          isnextpage: res.isnextpage,
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
    let that=this;
    let isnextpage = that.data.isnextpage;
    if (isnextpage <= 0) {
      return
    }
    that.data.currentpage = that.data.currentpage + 1;
    that.get_topprolist();
  }
})