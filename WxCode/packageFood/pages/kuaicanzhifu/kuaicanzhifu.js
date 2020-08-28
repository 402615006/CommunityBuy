
import { add_bill, get_coupon, get_vipcard, refund_cards, new_wx_Pay, cancel_pay ,cen_bill} from '../../utils/server.js';

Page({
  /**
   * 页面的初始数据
   */
  data: {
    bill: '',
    table:'',
    cardlist: [{
      "levelname": "微信",
      "cardCode": "wx",
    }],
    paylist:[],
    coupon:[],

    stocode:'',
    buscode:'',
    order:'',
    billcode:'',

    tims:0  //刷新次数

  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    console.log(options)
    if (options.buscode){
      that.setData({
        buscode: options.buscode
      })
    }
    if (options.stocode){
      that.setData({
        stocode: options.stocode
      })
    }
    if (options.order){
      that.setData({
        order: options.order
      })
    }
    if (options.billcode){
      that.setData({
        billcode:options.billcode
      })
    }
    that.add_bill();
    that.get_vipcard();
  },
  // 取消账单
  cen_bill(){
    let that=this;
    let data = {
      "actionname": "i_cancelbill",
      "parameters": {
        'key': '',
        'buscode': that.data.buscode,
        'stocode': that.data.stocode,
        'billcode': that.data.billcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    cen_bill(data).then(res=>{
      console.log(res);
      if(res.code==0){
        wx.showToast({
          title: '取消成功',
          duration: 1500,
          mask: true
        })
        setTimeout(()=>{
          wx.navigateBack();
        },1000)
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
  // 添加账单
  add_bill(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "i_addbill",
      "parameters": {
        'key':'',
        'buscode': that.data.buscode,
        'stocode': that.data.stocode,
        'ordercodelist': that.data.order,
        'shiftcode': '',
        'memcode': memcode,
        'billcode': that.data.billcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    add_bill(data).then(res=>{
      console.log(res);
      if(res.code==0){
        let bill=res.bill[0];
        let table = res.table[0];
        let billcode = bill.PKCode;
        let paylist = res.pay;
        let dish = res.dish;
        let coupon = res.coupon;
        that.setData({
          bill:bill,
          table:table,
          billcode: billcode,
          paylist:paylist,
          dish:dish,
          coupon: coupon
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
  // 优惠券获取
  go_coupon(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    if (that.data.bill.PayMoney>0){
      wx.showToast({
        title: '已付款的账单不能再选择优惠券',
        icon:'none',
        duration:1500,
        mask:true
      })
      return
    }
    let billcode = that.data.bill.PKCode;
    wx.navigateTo({
      url: '../selectcoupon/selectcoupon?billcode=' + billcode + '&stocode=' + that.data.stocode + '&buscode=' + that.data.buscode
    })
    return
    // let data = {
    //   "actionname": "getcouponsvalid",
    //   "parameters": {
    //     'GUID': '',
    //     'USER_ID': '',
    //     'buscode': that.data.buscode,
    //     'stocode': that.data.stocode,
    //     'memcode': memcode,
    //   }
    // }
    // data.parameters = JSON.stringify(data.parameters);
    // get_coupon(data).then(res=>{
    //   console.log(res);
    //   if(res.status==0){
    //     let list=res.data;
    //     if(list.length>0){
    //       list.map((item,index)=>{
    //         item.checked=false;
    //       })
    //       let billcode=that.data.bill.PKCode;
    //        wx.navigateTo({
    //          url: '../selectcoupon/selectcoupon?billcode=' + billcode + '&stocode=' + that.data.stocode + '&buscode=' + that.data.buscode + '&list=' + JSON.stringify(list),
    //       })
    //     }else{
    //       wx.showToast({
    //         title: '没有可以使用的优惠券',
    //         icon:'none',
    //         duration:1500,
    //         mask:true
    //       })
    //     }
    //   }else{
    //     wx.showToast({
    //       title: res.mes,
    //       icon:'none',
    //       duration:1500,
    //       mask:true
    //     })
    //   }
    // })
  },
  // 会员卡获取
  get_vipcard(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "getmemcardslistvalid",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        'stocode':that.data.stocode,
        'memcode': memcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_vipcard(data).then(res=>{
      console.log(res)
      if(res.status==0){
        let cardlist=res.data;
        cardlist.push({
          "levelname": "微信",
          "cardCode": "wx",
        })
        that.setData({
          cardlist: cardlist
        })
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
  // 更新数据
  update(e){
    let that=this;
    let res=e.detail;
    let bill = res.bill[0];
    let paylist = res.pay;
    let coupon = res.coupon;
    that.setData({
      bill: bill,
      paylist: paylist,
      coupon: coupon
    })
    that.get_vipcard();
  },
  // 微信支付完成更新数据
  wx_update(){
    let that=this;
    let bill = that.data.bill;
    let data = {
      "actionname": "i_refreshBill",
      "parameters": {
        'key': '',
        'buscode': that.data.buscode,
        'stocode': that.data.stocode,
        'billcode': that.data.billcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
      that.setData({
        tims:Number(that.data.tims)+1
      })
      new_wx_Pay(data).then(res=>{
        console.log(res);
        if(res.code==0){
          let bill = res.bill[0];
          let paylist = res.pay;
          let coupon = res.coupon;
          if (Number(bill.ToPayMoney)>0){
            if (that.data.tims>4){
              wx.showModal({
                title: '提示',
                content: '支付出现错误,去我的订单查看？',
                showCancel: false,
                confirmText: '确定',
                success(res) {
                  if (res.confirm) {
                    wx.redirectTo({
                      url: '/pages/order/order'
                    })
                  }
                }
              })
            }else{
              setTimeout(()=>{
                that.wx_update();
              },500)
            }
          }else{
            that.setData({
              bill: bill,
              paylist: paylist,
              coupon: coupon
            })
          }
        }else{
          wx.showToast({
            title: res.msg,
            icon:'none',
            duration:1500,
            mask:true
          })
        }
      })
    // let money = Number(bill.PayMoney) + Number(bill.ToPayMoney);
    // money = parseFloat(money);
    // bill.ToPayMoney=0;
    // bill.PayMoney = money;
    // that.setData({
    //   bill:bill
    // })
  },
  // 支付完成跳转
  go_paymentOK(){
    let that=this;
    let bill=that.data.bill;
    let money = bill.PayMoney;
    if (bill.TStatus==1){
      let payName='';
      if (that.data.paylist.length>0){
        payName = that.data.paylist[0].PayMethodName;
      }else{
        payName = '微信支付';
      }
      bill.DishList=that.data.dish;
      wx.redirectTo({
        url: '../paymentOK/paymentOK?ShiftCode=' + bill.TackNo + '&billcode=' + bill.PKCode + '&money=' + money + '&payName=' + payName + '&detail=' + JSON.stringify(bill),
      })
    }else{
      wx.showToast({
        title: '订单未支付完成,请继续支付',
        icon:'none',
        duration:1500,
        mask:true
      })
    }
  },
  // 点击办卡跳转
  go_bindcard(){
    let that=this;
    let stocode = that.data.stocode;
    wx.navigateTo({
      url: '/packageVip/pages/cardlist/cardlist?stocode=' + stocode,
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

  }
})