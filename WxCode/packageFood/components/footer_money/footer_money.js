// packageFood/components/footer/footer.js
import { verificationcardpwd } from '../../../utils/server.js';
import { refund_cards, wx_Pay, cancel_pay } from '../../utils/server.js';
import { wechat_payment } from '../../../utils/util.js'

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    bill:{
      type:Object,
      value:''
    },
    ToPayMoney:{
      type:String,
      value:'0'
    },
    PayMoney:{
      type:String,
      vlaue:'0'
    },
    billcode:{
      type:String,
      value:''
    },
    buscode:{
      type:String,
      value:''
    },
    stocode:{
      type:String,
      value:''
    },
    cardlist:{
      type:JSON,
      value:[],
      observer: function (newVal, oldVal) {
        console.log(newVal)
      }
    },
    paylist:{
      type:JSON,
      value:[],
      observer: function (newVal, oldVal) {
        console.log(newVal)
      }
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    animationData: '',
    show: false,
    animationData2:'',
    show2:false,
    card_index:'-1',

    discountmoney:0
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 选择支付方式
    btn_clickcard(e){
      let that=this;
      let index=e.currentTarget.dataset.index;
      let bill=that.data.bill;
      let cardlist=that.data.cardlist;
      // 点击微信方式选择
      if (cardlist[index].cardCode == 'wx'){
        // 所选择优惠券只能用会员卡支付
        if (bill.paymethod =='memcard'){
          wx.showToast({
            title: '您选择的优惠券不能使用微信支付',
            duration:2000,
            icon:'none'
          })
          return
        }
        that.setData({
          card_index:index,
          discountmoney:0
        })
      }else{
        // 点击会员卡方式选择
        if (bill.paymethod == 'wx') {
          // 所选择优惠券只能用微信支付
          wx.showToast({
            title: '您选择的优惠券不能使用会员卡支付',
            duration: 2000,
            icon: 'none'
          })
          return
        }
        that.get_Offerprice(index);
      }
    },
    // 选完支付方式点击确认
    btn_ok(){
      let that=this;
      let bill = that.data.bill;
      let card_index = that.data.card_index;
      let cardlist = that.data.cardlist;
      if (card_index=='-1'){
        wx.showToast({
          title: '请选择支付方式',
          icon:'none',
          duration:1500
        })
        return
      }
      if (!Number(that.data.ToPayMoney)>0){
        wx.showToast({
          title: '订单已支付完成',
          icon: 'none',
          duration: 1500,
          mask: true
        })
        return
      }
      // 选择微信支付
      if (cardlist[card_index].cardCode == 'wx') {
        // 选择的优惠券只能用会员卡支付
        if (bill.paymethod == 'memcard') {
          wx.showToast({
            title: '您选择的优惠券不能使用微信支付',
            duration: 2000,
            icon: 'none'
          })
          return
        }
        that.wx_Pay();
      } else {
        // 选择会员卡支付
        if (bill.paymethod == 'wx') {
          // 选择的优惠券只能用微信支付
          wx.showToast({
            title: '您选择的优惠券不能使用会员卡支付',
            duration: 2000,
            icon: 'none'
          })
          return
        }
        // let money = cardlist[card_index].balance;
        // if (!Number(money)>0){
        //   wx.showToast({
        //     title: '会员余额已为0',
        //     icon:'none',
        //     duration:1500,
        //     mask:true
        //   })
        //   return
        // }
        if (cardlist[card_index].isneedpwd == 0) {
          this.selectComponent("#password").showModal();
        } else {
          let notpwdAmount = cardlist[card_index].notpwdAmount;
          if (Number(notpwdAmount) > Number(that.data.ToPayMoney)){
            that.refund_cards();
          }else{
            this.selectComponent("#password").showModal();
          }
        }
      }
    },
    // 密码输入完成判断是否正确
    get_number_ok(e) {
      let that = this;
      let pwd = e.detail;
      let unionid = wx.getStorageSync('unionid');
      let data = {
        "actionname": "pwdcheck",
        "parameters": {
          'GUID': '88888888',
          'unionid': unionid,
          'pwd': pwd
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      verificationcardpwd(data).then(res => {
        console.log(res);
        if (res.status == 0) {
          that.refund_cards();
        }else{
          wx.showToast({
            title: res.mes,
            icon: 'none',
            mask: true,
            duration: 1500
          })
        }
      })
    },
    // 计算优惠
    get_Offerprice(index){
      let that=this;
      let cardlist = that.data.cardlist;
      let data = {
        "actionname": "i_addcardpay",
        "parameters": {
          'key': '',
          'billcode': that.data.billcode,
          'buscode': that.data.buscode,
          'stocode': that.data.stocode,
          'memcard': cardlist[index],
          'submit': '0'
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      refund_cards(data).then(res => {
        if (res.code == 0) {
            let discountmoney = res.bill[0].DiscountMoney
            that.setData({
              card_index: index,
              discountmoney: discountmoney
            })
        } else {
          wx.showToast({
            title: res.msg,
            icon: 'none',
            duration: 1500,
            mask: true
          })
        }
      })
    },
    // 会员卡支付
    refund_cards() {
      let that = this;
      let cardlist = that.data.cardlist;
      let card_index = that.data.card_index;
      let data = {
        "actionname": "i_addcardpay",
        "parameters": {
          'key':'',
          'billcode': that.data.billcode,
          'buscode': that.data.buscode,
          'stocode': that.data.stocode,
          'memcard': cardlist[card_index],
          'submit':'1'
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      refund_cards(data).then(res=>{
        if(res.code==0){
            wx.showToast({
              title: '付款成功',
              icon:'success',
              duration:1500
            })
            that.triggerEvent('update', res);
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
    // 微信支付
    wx_Pay(){
      let that=this;
      let openid = wx.getStorageSync('openid');
      let memcode = wx.getStorageSync('memcode');
      let data = {
        "actionname": "i_wxpay",
        "parameters": {
          'key': '',
          'billcode': that.data.billcode,
          'buscode': that.data.buscode,
          'stocode': that.data.stocode,
          'openid': openid,
          'paymoney': that.data.ToPayMoney,
          'memcode': memcode
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      wx_Pay(data).then(res=>{
        console.log(res);
        if(res.code==0){
          let list=res.msg;
          wechat_payment(list.timeStamp, list.nonceStr, list.package, list.signType, list.paySign).then(res =>{
            that.triggerEvent('wx_update');
          }).catch(err => {
            wx.showToast({
              icon: "none",
              title: '支付失败',
              duration: 2000,
            })
          })
        }else{
          wx.showToast({
            icon: "none",
            title: res.msg,
            duration: 2000,
          })
        }
      })
    },
    //显示卡列表
    showcard(){
      let that=this;
      that.hidelogin2();
      that.showlogin();
    },
    // 显示详细
    btn_detil(){
      let that=this;
      let money = that.data.PayMoney
      // if (!Number(money)>0){
      //   return
      // }
      let show2=that.data.show2;
      if (show2){
        return
      }else{
        that.hidelogin();
        that.showlogin2();
      }
    },
    // 取消支付
    cancel_pay(){
      let that=this;
      let money = that.data.ToPayMoney;
      if (Number(that.data.ToPayMoney)<=0){
        wx.showToast({
          title: '订单已完成不能再进行取消',
          icon: 'none',
          duration: 1500
        })
        return
      }
      let memcode = wx.getStorageSync('memcode');
      let data = {
        "actionname": "i_cancelpay",
        "parameters": {
          'key': '',
          'buscode': that.data.buscode,
          'stocode': that.data.stocode,
          'billcode': that.data.billcode,
          'memcode': memcode
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      cancel_pay(data).then(res=>{
        console.log(res)
        if(res.code==0){
          wx.showToast({
            title: '取消成功',
            icon: 'success',
            duration: 1500
          })
          that.hidelogin2();
          that.triggerEvent('update', res);
        }else{
          wx.showToast({
            icon: "none",
            title: res.msg,
            duration: 2000,
          })
        }
      })
    },
    // 支付完成
    go_ok(){
      let that=this;
      that.triggerEvent('go_paymentOK');
    },
    showlogin() {
      // 创建一个动画实例
      var animation = wx.createAnimation({
        // 动画持续时间
        duration: 200,
        // 定义动画效果，当前是匀速
        timingFunction: "linear",
        delay: 0
      })
      // 将该变量赋值给当前动画
      this.animation = animation
      // 先在y轴偏移，然后用step()完成一个动画
      animation.translateY(600).step()
      // 用setData改变当前动画
      this.setData({
        // 通过export()方法导出数据
        animationData: animation.export(),
        // 改变view里面的Wx：if
        show: true
      })
      // 设置setTimeout来改变y轴偏移量，实现有感觉的滑动
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          animationData: animation.export()
        })
      }.bind(this), 200)
    },
    // 隐藏遮罩层
    hidelogin() {
      var animation = wx.createAnimation({
        duration: 200,
        timingFunction: "linear",
        delay: 0
      })
      this.animation = animation
      animation.translateY(600).step()
      this.setData({
        animationData: animation.export(),
      })
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          show: false,
          animationData: animation.export(),
        })
      }.bind(this), 200)
    },
    showlogin2() {
      // 创建一个动画实例
      var animation = wx.createAnimation({
        // 动画持续时间
        duration: 200,
        // 定义动画效果，当前是匀速
        timingFunction: "linear",
        delay: 0
      })
      // 将该变量赋值给当前动画
      this.animation = animation
      // 先在y轴偏移，然后用step()完成一个动画
      animation.translateY(600).step()
      // 用setData改变当前动画
      this.setData({
        // 通过export()方法导出数据
        animationData2: animation.export(),
        // 改变view里面的Wx：if
        show2: true
      })
      // 设置setTimeout来改变y轴偏移量，实现有感觉的滑动
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          animationData2: animation.export()
        })
      }.bind(this), 200)
    },
    // 隐藏遮罩层
    hidelogin2() {
      var animation = wx.createAnimation({
        duration: 200,
        timingFunction: "linear",
        delay: 0
      })
      this.animation = animation
      animation.translateY(600).step()
      this.setData({
        animationData2: animation.export(),
      })
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          show2: false,
          animationData2: animation.export(),
        })
      }.bind(this), 200)
    },
  }
})