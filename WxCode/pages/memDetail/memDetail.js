var util = require('../../utils/util.js');
var { is_gologin } = require('../../utils/util.js');
import { baserURLOrganization } from '../../utils/api.js';
import { wechat_payment } from '../../utils/util.js';


import { 
  newuserprodetail, 
  proorderadd, 
  get_vipcard, 
  pay_collagemoney,
  getvipprodetail, 
  vipproordadd, 
  vipproordpay, 
  get_usercomprice,
  set_enterPageAct
} from '../../utils/server.js';
var app = getApp();
Page({

  /**
   * 页面的初始数据
   */
  data: {
    stocoordx:'43.817130',
    stocoordy:'87.622600',
    juli:'',
    stocode:'',    //门店code
    address:'新疆维吾尔自治区乌鲁木齐市水磨沟区新民东街186号',
    title:"",//标题
    procode:"",//code
    type:"",//1 新人专享
    url: baserURLOrganization,   //url
    price:"0.00",//付款金额
    num:1,
    bannerlist:[],
    paytype:"",//支付方式
    cardlevel:'',   //卡等级限制
    cardlist:[],//支付列表
    orderno:"",//订单号
    mainorderno:"",
    bottomShow:"true",
    btn_box:false,    //生成海报按钮

    Topheight: 400 / 750 * wx.getSystemInfoSync().windowWidth,
    background:'',
    my_class:true
  },
  // 打开地图
  go_map(){
    let that=this;
    wx.openLocation({
      latitude: Number(that.data.stocoordx),
      longitude: Number(that.data.stocoordy),
      name: '乌鲁木齐东街禧玥酒店',
      address: that.data.address,
      scale: 28
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options){
    let that=this;
    wx.hideShareMenu();
    var procode = options.procode;
    var type = options.type;
    that.setData({
      procode:procode,
      type:type
    })
    if(options.stat){
      that.setData({
        bottomShow:options.stat
      })
    }
    // 如果有分享人的邀请码
    if (options.shareId){
      app.globalData.shareId = options.shareId;
    }
    
    // 进入页面记录页面
    set_enterPageAct('商品详情', that.data.procode, '2', '');
  },

  //vip礼包详情
  getvipdata(){
    let that = this;
    let data = {
      'actionname': "getvipprodetail",
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "code":that.data.procode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getvipprodetail(data).then(res => {
      // console.log(res)
      if (res.code == 0) {
        let list = res.data[0];
        list.content=list.content.replace(/\<img/gi,'<img style="max-width:100%;height:auto" ');
        var bannerlist = "";
        if(list.bimg){
          bannerlist = list.bimg.split(",");
          for(var i = 0;i<bannerlist.length;i++){
            bannerlist[i] = that.data.url+bannerlist[i];
          }
        }   
        that.data.stocode = list.sybuscode;
        that.vipcards();
        that.setData({
          list: list,
          price: Number(list.price).toFixed(2),
          bannerlist:bannerlist,
          paytype:'1'
        })
  
      } else {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          mask: true,
          duration: 1500
        })
      }
    })
  },
  //新人专享、会员福利、闲弟推荐 详情
  getData(actionname){
    let that = this;
    let data = {
      'actionname': actionname,
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "procode":that.data.procode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    newuserprodetail(data).then(res => {
      // console.log(res)
      if (res.code == 0){
        let list = res.data[0];
        list.proinfo=list.proinfo.replace(/\<img/gi, '<img style="max-width:100%;height:auto" ');
        var bannerlist = list.bigimg.split(",");
        for(var i = 0;i<bannerlist.length;i++){
          bannerlist[i] = that.data.url+bannerlist[i];
        }
        that.data.stocode = list.stocode;
        that.data.buscode = list.buscode;
        that.data.cardlevel = list.cardlevel;
        that.vipcards();
        that.setData({
          list: list,
          price: Number(list.price).toFixed(2),
          bannerlist:bannerlist,
          paytype:list.paytype
        })
      } else {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          mask: true,
          duration: 1500
        })
      }
    })
  },

  //获取会员卡
  vipcards(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "getmemcardslistvalid",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        'stocode': that.data.stocode,
        'buscode': that.data.buscode,
        'memcode': memcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_vipcard(data).then(res=>{
      // console.log(res)
      let list=res.data;
      let cardlist=[];
      // 是否允许使用微信
      if (that.data.paytype.indexOf('1')>=0){
        cardlist.push({
          "levelname": '微信',
          "cardCode": 'wx'
        })
      }
      if(res.status==0){
        // 是否允许使用会员卡
        if (that.data.paytype.indexOf('2') >= 0){
          if (that.data.cardlevel) {      //是否有卡等级限制
            list.map((item, index) => {
              if (that.data.cardlevel.indexOf(item.cracode) >= 0) {
                cardlist.push(item);
              }
            })
          } else {
            cardlist = cardlist.concat(list);
          } 
        }
      }
      that.setData({
        cardlist: cardlist
      })
      // 是否允许使用佣金
      if (that.data.paytype.indexOf('3') >= 0){
        that.get_usercomprice();
      }
    })
  },
  // 获取佣金
  get_usercomprice(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data={
      "actionname": "getusercomprice",
      "parameters":{
          "GUID": "",
          "USER_ID": "",
          "memcode": memcode
      }  
    }
    data.parameters = JSON.stringify(data.parameters);
    get_usercomprice(data).then(res=>{
      // console.log('获取佣金')
      if(res.code==0){
        let cardlist = that.data.cardlist;
        cardlist.push({
          "levelname": '佣金',
          "cardCode": 'yj',
          "balance": res.msg
        })
        that.setData({
          cardlist: cardlist
        })
      }
    })
  },
  //确认付款
  confirmPayment(){
    let that = this;   
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    that.selectComponent("#button_pay").showcard();
  },
  //去支付
  pay_but(){
    var that = this;
    if(that.data.type!=5){
      that.comorders();
    }else{
      that.viporders();
    }  
  },
  //新人专享、会员福利、闲弟推荐 下单
  comorders(){
    var that = this;
    var Ldata = that.data.list;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': "proorderadd",
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "buscode":Ldata.buscode,
        "stocode":Ldata.stocode,
        "procode":Ldata.procode,
        "ordtype":that.data.type,
        "memcode":memcode,
        "price":that.data.price,
        "num":that.data.num,
        "dprice":Ldata.price,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    // console.log(data)
    proorderadd(data).then(res => {
      // console.log(res)
      if (res.code == 0) {
        let orderno = res.data[0].orderno;
        let mainorderno = res.data[0].mainorderno;
        that.data.orderno = orderno;
        that.data.mainorderno = mainorderno;
        that.buy_Pay(that.data.price,'proorderpay'); 
      } else if(res.code==2){
        wx.showModal({
          title: '温馨提示',
          content: res.msg,
          cancelText: '我知道了',
          confirmText: '升级会员',
          success: function (res) {
            if (res.confirm) {
              app.globalData.memscroll = true;
              wx.switchTab({
                url: '/pages/member/member',
              })
            }
          }
        })
      }else{
        wx.showToast({
          title: res.msg,
          icon: 'none',
          mask: true,
          duration: 1500
        })
      }
    })
  },
  //vip礼包下单
  viporders(){
    var that = this;
    var Ldata = that.data.list;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': "vipproordadd",
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "code":that.data.procode,
        "memcode":memcode,
        "num":"1",
        "dprice":Ldata.price,
        "price":that.data.price
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    // console.log(data)
    vipproordadd(data).then(res => {
      // console.log(res)
      if (res.code == 0) {
        let orderno = res.data[0].orderno;
        let mainorderno = res.data[0].mainorderno;
        that.data.orderno = orderno;
        that.data.mainorderno = mainorderno;
        that.buy_Pay(that.data.price,'vipproordpay'); 
      } else {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          mask: true,
          duration: 1500
        })
      }
    })
  },

    // 调用支付
    buy_Pay(collpaymoney,actionname){
      let that=this;
      let orderno=that.data.orderno;
      let mainorderno=that.data.mainorderno;
      let cardlist = that.data.cardlist;
      let card_index = that.selectComponent("#button_pay").data.card_index;
      let cardCode = cardlist[card_index].cardCode;
      let paytype='2';
      if (cardlist[card_index].cardCode=='wx'){
        paytype='1';
      }else if (cardlist[card_index].cardCode=='yj'){
        paytype='3';
      }
      let memcode = wx.getStorageSync('memcode');
      let openid = wx.getStorageSync('openid');
      let data = {
        "actionname": actionname,
        "parameters": {
          "GUID": "",
          "USER_ID": "",         
          "memcode": memcode,
          "mainorderno":mainorderno,
          "orderno": orderno,
          "paytype": paytype,
          "paymoney": collpaymoney,
          "cardcode": cardCode ,
          "openid": openid
        }
      }
      var orderTabIdx = '4';
      if(that.data.type!=5){
          data.parameters.buscode=that.data.list.buscode;
          data.parameters.stocode= that.data.list.stocode;
          orderTabIdx = '3';
      }
      data.parameters = JSON.stringify(data.parameters);
      pay_collagemoney(data,that.data.type).then(res=>{
        // console.log(res);
        if (paytype==1){
          if (res.appId){
            wechat_payment(res.timeStamp, res.nonceStr, res.package, res.signType, res.paySign).then(res => {
              wx.redirectTo({
                url: '/pages/order/order?tabindex='+orderTabIdx
              })
            }).catch(err=>{
              wx.showToast({
                mask: true,
                title: '您已取消支付',
                icon: 'none'
              })
              setTimeout(()=>{
                wx.redirectTo({
                  url: '/pages/order/order?tabindex='+orderTabIdx
                })
              },1000)
            })
          }else{
            wx.showToast({
              title: res.msg,
              duration: 1500,
              icon: 'none',
              mask: true
            })
            setTimeout(() => {
              wx.redirectTo({
                url: '/pages/order/order?tabindex='+orderTabIdx
              })
            }, 1000)
          }
        }else{
          if(res.code==0){
            wx.showToast({
              title: '支付成功',
              duration: 1500,
              icon: 'success',
              mask: true
            })
            setTimeout(()=>{
              wx.redirectTo({
                url: '/pages/order/order?tabindex='+orderTabIdx
              })
            },1000)
          }else{
            wx.showToast({
              title: res.msg,
              duration:1500,
              icon:'none',
              mask: true
            })
            setTimeout(() => {
              wx.redirectTo({
                url: '/pages/order/order?tabindex='+orderTabIdx
              })
            }, 1000)
          }
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
  onShow: function (){
    let that = this;
    let type=that.data.type;
    let actionname = '';
    if (type == 1) {
      that.setData({
        title: '新人专享详情',
      })
      actionname = 'newuserprodetail';
      that.getData(actionname);
    } else if (type == 2) {
      that.setData({
        title: '闲弟推荐详情',
      })
      actionname = 'topprodetail';
      that.getData(actionname);
    } else if (type == 3) {
      that.setData({
        title: '会员福利详情',
      })
      actionname = 'memuserprodetail';
      that.getData(actionname);
    } else {
      that.setData({
        title: '礼包详情',
      })
      that.getvipdata();
    }
  },
  onPageScroll(e) {
    // console.log(e);
    let that=this;
    let scrollTop = e.scrollTop;
    let Topheight = that.data.Topheight;
    if (scrollTop>0){
      if (scrollTop > Topheight){
        that.setData({
          background: 'rgba(255, 255, 255, 1)',
          my_class:false
        })
      }else{
        that.setData({
          background: `rgba(255, 255, 255,${scrollTop / Topheight})`,
          my_class:true
        })
      }
    }else{
      that.setData({
        background:'',
        my_class: true
      })
    }
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
    var that = this;
    setTimeout(() => {
      wx.stopPullDownRefresh();
      that.setData({
        currentpage: 1,
        list: [],
      })
      that.getListData();
    }, 500);
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
    var that = this;
    if(that.data.isnextpage==1){
      that.setData({
        currentpage: that.data.currentpage+1,
      })
      that.getListData();
    }else{
      that.setData({
        noMore: true,
      })     
    }
  },
  //取消分享按钮
  cen_btn_box(){
    let that=this;
    that.setData({
      btn_box: !that.data.btn_box
    })
  },
  //显示分享按钮
  show_btn_box(){
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    that.setData({
      btn_box: true
    })
  },
  //生成朋友圈海报
  show_generate_friend(){
    let that=this;
    that.setData({
      btn_box:false
    })
    let code = that.data.procode;
    that.selectComponent("#Friendbox").get_data(code);
  },
  preventTouchMove(){
    //防止滑动
  },
  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function (res) {
    // console.log(res);
    let that = this;
    that.setData({
      btn_box:false
    })
    let invitecode='';
    if (wx.getStorageSync('invitecode')){
      invitecode = wx.getStorageSync('invitecode')
    }
    // console.log(invitecode)
    if (res.from == 'button'){
      // 来自页面内转发按钮
      return {
        title: that.data.list.name,
        path: 'pages/memDetail/memDetail?procode=' + res.target.dataset.code + '&type=' + res.target.dataset.type + '&shareId=' + invitecode,
        // imageUrl: this.data.coverImage,
        success: (res) => {
          // 成功后要做的事情
        },
        fail: function(res) {
          // 分享失败
         
        }
      }
    }
  },

  
 

  //减号
  prev() {
    let that = this;
    if (that.data.num > 1) {
      var num = that.data.num;
      var price = that.data.list.price;
      num--;
      that.setData({
        num: num,
      })
      that.dndn(price, num);
    }
  },
  // 加号
  add() {
    let that = this;
    var num = that.data.num;
    var price = that.data.list.price;
    num++;
    that.setData({
      num: num,
    })
    that.upup(price, num);
  },

  // 相加
  upup(price, num) {
    var that = this;
    if (num > 0) {
      that.setData({
        price: (Number(price) + Number(that.data.price)).toFixed(2),
      })
    }
  },
  // 相减
  dndn(price, num) {
    var that = this;
    that.setData({
      price: (Number(that.data.price) - Number(price)).toFixed(2),
    })
  },

  // 复制
  copy(e) {
    let that = this;
    let text = e.currentTarget.dataset.text
    wx.setClipboardData({
      data: text,
      success: function (res) {
        wx.getClipboardData({
          success: function (res){
            // that.selectComponent("#Friendbox").get_data();
          }
        })
      }
    })
  }
})