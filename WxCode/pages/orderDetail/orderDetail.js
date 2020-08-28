var util = require('../../utils/util.js');
import { baserURLOrganization } from '../../utils/api.js';
import { wechat_payment } from '../../utils/util.js';
import { getdetail, Comgetdetail, get_vipcard, pay_collagemoney, can_celproorder, cen_proordrefundnew, get_usercomprice} from '../../utils/server.js';
Page({

  /**
   * 页面的初始数据
   */
  data: {
    title:"",//标题
    code:"",//code
    type:"",//1 新人专享
    url: baserURLOrganization,//url
    price:"0.00",//付款金额
    num:1,
    list:'',     //页面数据
    bannerlist:[],
    paytype:"",//支付方式
    cardlevel:'',  //卡等级限制
    cardlist:[],//支付列表
    orderno:"",//订单号
    mainorderno:"",
    VipOrPay:"",
    buscode:"",
    stocode:"",

    codejson:[]  //券码
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    wx.hideShareMenu();
    console.log(options)
    var code = options.code;
    var type = options.type;

    that.setData({
      code:code,
      type:type
    })

    if(type==1){
      that.setData({
        title: '新人专享详情',
        buscode : options.buscode,
        stocode : options.stocode,
      })
      that.getData();
    }else if(type==2){
      that.setData({
        title: '闲弟推荐详情',
        buscode : options.buscode,
        stocode : options.stocode,
      })
      that.getData();
    }else if(type==3){
      that.setData({
        title: '会员福利详情',
        buscode : options.buscode,
        stocode : options.stocode,
      })
      that.getData();
    }else{
      that.setData({
        title: '礼包详情',
      })
      that.getvipdata();
    }
  },

  //vip礼包详情
  getvipdata(){
    let that = this;
    let data = {
      'actionname': "getdetail",
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "code":that.data.code,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getdetail(data).then(res => {
      console.log(res)
      if (res.code == 0) {
        let list = res.TM_VipOrder[0];
        list.content=list.content.replace(/\<img/gi, '<img style="max-width:100%;height:auto" ');
        var bannerlist = "";
        if(list.bimg){
          bannerlist = list.bimg.split(",");
          for(var i = 0;i<bannerlist.length;i++){
            bannerlist[i] = that.data.url+bannerlist[i];
          }
        }   
        // 券码 
        let codejson=[];
        if (list.hxcode){
          codejson = res.TM_VipCoupon
        }
        that.vipcards();
        that.setData({
          list: list,
          price: Number(list.price).toFixed(2),
          bannerlist:bannerlist,
          paytype:'1',
          VipOrPay:res.TM_VipOrPay[0],
          codejson: codejson
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
  getData(){
    let that = this;
    let data = {
      'actionname': 'getdetail',
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "code":that.data.code,
        "buscode":that.data.buscode,
        "stocode":that.data.stocode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    Comgetdetail(data).then(res => {
      console.log(res)
      if (res.code == 0) {
        let list = res.TM_ProOrder[0];
        var bannerlist = list.bigimg.split(",");
        for(var i = 0;i<bannerlist.length;i++){
          bannerlist[i] = that.data.url+bannerlist[i];
        }
        that.data.cardlevel = list.cardlevel;
        that.vipcards();
        // 券码
        let codejson = [];
        if (list.hxcode) {
          let codedata = list.hxcode.split(',');
          codedata.map((item,index)=>{
            if(item){
              codejson.push({
                'checkcode':item,
                'status': list.hxstatus
              })
            }
          })
        }
        console.log(codejson);
        that.setData({
          list: list,
          price: Number(list.price).toFixed(2),
          bannerlist:bannerlist,
          paytype:res.TM_ProOrPay[0].paytype,
          VipOrPay:res.TM_ProOrPay[0],
          codejson: codejson
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
  vipcards(stocode){
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
      console.log(res)
      let list=res.data;
      let cardlist=[];
      if (that.data.paytype.indexOf('1') >= 0) {   // 是否允许使用微信
        cardlist.push({
          "levelname": '微信',
          "cardCode": 'wx'
        })
      }
      if(res.status==0){
        if (that.data.paytype.indexOf('2') >= 0) {  // 是否允许使用会员卡
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
      // 有佣金支付方式
      if (that.data.paytype.indexOf('3') >= 0){
        that.get_usercomprice();
      }
    })
  },
  // 获取佣金
  get_usercomprice() {
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "getusercomprice",
      "parameters": {
        "GUID": "",
        "USER_ID": "",
        "memcode": memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_usercomprice(data).then(res => {
      console.log('获取佣金')
      if (res.code == 0) {
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
    let memcode = wx.getStorageSync('memcode');
    if (!memcode) {
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
      that.buy_Pay(that.data.price,'proorderpay'); 
    }else{
      that.buy_Pay(that.data.price,'vipproordpay'); 
    }  
  },
    // 调用支付
    buy_Pay(collpaymoney,actionname){
      let that=this;
      let orderno=that.data.VipOrPay.pcode;
      let mainorderno=that.data.VipOrPay.code;
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
        console.log(res);
        if (paytype==1){
          if (res.appId){
            wechat_payment(res.timeStamp, res.nonceStr, res.package, res.signType, res.paySign).then(res => {
              wx.navigateBack();
            }).catch(err=>{
              wx.showToast({
                mask: true,
                title: '您已取消支付',
                icon: 'none'
              })
            })
          }else{
            wx.showToast({
              title: res.msg,
              duration: 1500,
              icon: 'none',
              mask: true
            })
            setTimeout(() => {
              wx.navigateBack();
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
            setTimeout(() => {
              wx.navigateBack();
            }, 1000)
          }else{
            wx.showToast({
              title: res.msg,
              duration:1500,
              icon:'none',
              mask: true
            })
          }
        }
      })
    },
    //详情
    todetail(e){
      var that = this;
      var procode = e.currentTarget.dataset.code;
      wx.navigateTo({
        url: '/pages/memDetail/memDetail?procode=' + procode+'&type='+that.data.type+'&stat=false',        
      })
    },
  // 取消订单
  cancel(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let orderno = that.data.VipOrPay.pcode;
    let data={
      "actionname": "cancelproorder",
      "parameters":{
        "GUID": "",
        "USER_ID": "",
        "memcode": memcode,
        "orderno": orderno
      } 
    }
    data.parameters = JSON.stringify(data.parameters);
    can_celproorder(data).then(res=>{
      if(res.code==0){
        wx.showToast({
          title: '取消订单成功',
          icon: 'success',
          duration: 1500
        })
        setTimeout(()=>{
          wx.navigateBack();
        },1000)
      }else{
        wx.showToast({
          title: res.msg,
          icon:'none',
          duration:1500
        })
      }
    })
  },
  // 申请退款
  cen_proordrefundnew(){
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    let orderno = that.data.VipOrPay.pcode;
    let mainorderno=that.data.VipOrPay.code;
    let data={
      "actionname": "proordrefundnew",
        "parameters":{
        "GUID": "",
        "USER_ID": "",
        "buscode": that.data.buscode,
        "stocode": that.data.stocode,
        "mainorderno": mainorderno,
        "orderno": orderno
      } 
    }
    data.parameters = JSON.stringify(data.parameters);
    cen_proordrefundnew(data).then(res=>{
      if(res.code==0){
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500,
          mask:true
        })
        setTimeout(() => {
          wx.navigateBack();
        }, 1500)
      }else{
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500
        })
      }
    })
  },
  // 去优惠券详情
  go_codedetail(e){
    let that=this;
    let item = e.currentTarget.dataset.item;
    let checkcode = item.checkcode;
    if (item.status==0){
      wx.navigateTo({
        url: '/pages/coupondetail/coupondetail?checkcode=' + checkcode,
      })
    }
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
    console.log(res)
    var that = this;
    if (res.from == 'button') {
      // 来自页面内转发按钮
      return {
        title: that.data.list.name,
        path: 'pages/memDetail/memDetail?procode=' + res.target.dataset.procode+'&type=' + res.target.dataset.type,
        // imageUrl: this.data.coverImage,
        success: (res) => {
          // 成功后要做的事情
          console.log(res)
          wx.showToast({
            title: "分享成功",
            icon: 'none',
            duration: 2000
          })
        },
        fail: function(res) {
          // 分享失败
          console.log(res)
          wx.showToast({
            title: "分享失败",
            icon: 'none',
            duration: 2000
          })
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
  }
})