// packageVip/pages/recharge/recharge.js;
import { get_mywalletvipcardlist, get_cardinfodetail, set_chargecardorder } from '../../utils/server.js';
import { wechat_payment } from '../../../utils/util.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    empid:'',  //服务码
    index:0,
    flag:false,
    cardcode:'',
    list: [],
    cardlist:[],
    array:[],

    arrycards:[],

    maxmoney:0,
    minmoney:0,
    money:0,
    tab_index:0,


    actsimagesshow:true
  },
  bin_tabindex(e) {
    console.log(e);
    let that = this;
    let index = e.currentTarget.dataset.index;
    that.setData({
      tab_index: index
    })
    that.get_money();
  },
  // 没有卡去开卡页面
  go_vip(){
    console.log('去')
    wx.redirectTo({
      url: '/packageVip/pages/cardlist/cardlist'
    })
  },
  // 输入服务码
  inputempid(e){
    let that = this;
    let value = e.detail.value;
    that.setData({
      empid: value
    })
  },
  // 输入框输入
  inputmoney(e) {
    let that = this;
    let value = e.detail.value;
    that.setData({
      money: value
    })
  },
  // 选完卡后
  bindPickerChange(e) {
    console.log(e);
    let that = this;
    let value = e.detail.value;
    // if (value==that.data.index){
    //   return
    // }
    that.setData({
      index: value
    })
    that.get_data();
  },
  // 获取会员卡
  get_cards(){
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "myaccountinfolist",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'memcode': memcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_mywalletvipcardlist(data).then(res => {
      if (res.status == 0) {
        let list = res.data;
        let array = [];
        list.map((item, index) => {
          array.push(item.levelname)
          
        })
        that.setData({
          cardlist: list,
          array: array,
          flag:true
        })
        if (that.data.cardcode) {
          for (var i = 0; i < list.length;i++){
            if (that.data.cardcode== list[i].cardcode){
              that.setData({
                index:i
              })
              break;
            }
          }
        }
        that.get_data();
      } else {
        wx.showToast({
          title: res.mes,
          icon: 'none',
          mask: true,
          duration: 1500
        })
      }
    })
  },
  // 获取充值信息
  get_data(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    if (that.data.cardlist.length==0){
      return
    }
    let cardcode = that.data.cardlist[that.data.index].cardcode;
    let data={
      'actionname':'memcardchargeinfo',
      'parameters':{ 
        "GUID": "", 
        "USER_ID": "", 
        "memcode": memcode, 
        "cardcode": cardcode, 
        "way": "WX" 
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_cardinfodetail(data).then(res=>{
      if(res.status==0){
        console.log(res)
        let maxmoney = res.data.maxpay;
        let minmoney = res.data.minpay;
        let levelname = res.data.levelname;
        that.setData({
          maxmoney: maxmoney,
          minmoney: minmoney,
          money: minmoney
        })
        let arry = [];
        let list = res.data.acts;
        let actsimagesshow=true;
        if (list.length > 0) {
          list.map((time, i) => {
            let arry = [];
            if (!time.smallimg || time.smallimg == 'null') {
              actsimagesshow = false;
            }
            let presentNum = time.presentNum;
            if (time.coupons){
              let coupons = time.coupons;
              coupons = coupons.split(';');
              coupons.map((item, index) => {
                arry.push({});
                let result = item.split(',');
                result.map((ctim, idx) => {
                  arry[index]['name' + idx] = ctim
                })
                if (coupons.length == presentNum){
                  arry[index].ischecked = true
                }else{
                  arry[index].ischecked=false

                }
              })
            }
            time.couponarry = arry
          })
        }
        that.setData({
          list: list,
          actsimagesshow: actsimagesshow
        })
        that.get_money();
      }else{
        wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
    })
  },
  get_money(){
    let that = this;
    let list = that.data.list;
    let tab_index = that.data.tab_index;
    if (list.length > 0) {
      let maxmoney = list[tab_index].conmax;
      let minmoney = list[tab_index].conmin;
      let money = minmoney;
      that.setData({
        maxmoney: maxmoney,
        minmoney: minmoney,
        money: money
      })
    }
  },
  btn(){
    let that = this;
    let coupons = '';
    let pcode='';
    let money = that.data.money;
    let conmin = that.data.minmoney;
    let conmax = that.data.maxmoney;
    if (!money) {
      wx.showToast({
        title: '请选择充值金额',
        icon: 'none',
        duration: 1500,
        mask: true
      })
      return
    }
    console.log(money);
    console.log(conmin);
    console.log(conmax);
    if (Number(money) < Number(conmin) || Number(money) > Number(conmax)) {
      wx.showToast({
        title: '充值金额应在' + conmin + '-' + conmax + '之间',
        icon: 'none',
        duration: 1500,
        mask: true
      })
      return
    }
    let arry = that.data.list;
    let tab_index=that.data.tab_index;
    if (arry.length > 0) {
      pcode = arry[tab_index].pcode;
      let presentNum = arry[tab_index].presentNum;
      let num=0;
      arry[tab_index].couponarry.map((item, index) => {
        if(item.ischecked==true){
          num++
          if (coupons =='') {
            coupons = item.name2 + ',' + item.name3 + ',' + item.name5
          } else {
            coupons = coupons + ';' + item.name2 + ',' + item.name3 + ',' + item.name5
          }
        }
      })

      if (Number(num) != Number(presentNum)){
        wx.showToast({
          title: '请选择' + presentNum+'种优惠券',
          icon:'none',
          duration:1500,
          mask:true
        })
        return
      }
    }
    console.log(coupons)
    let openid = wx.getStorageSync('openid');
    let memcode = wx.getStorageSync('memcode');
    let empid = that.data.empid;
    if (empid){
      empid = Number(empid);
    }else{
      empid=0
    }
    let data = {
      'actionname': 'chargecardorder',
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "openid": openid,
        "memcode": memcode,
        "cardcode": that.data.cardlist[that.data.index].cardcode,
        "pcode": pcode,
        "coupons": coupons,
        "money": that.data.money,
        "way":'WX',
        "empid":empid
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    set_chargecardorder(data).then(res => {
      if (res.timeStamp) {
        wechat_payment(res.timeStamp, res.nonceStr, res.package, res.signType, res.paySign).then(res => {
          wx.navigateTo({
            url: '../../../pages/success/success?title=充值成功' + '&go_back=1',
          })
        }).catch(err => {
          wx.showToast({
            icon: "none",
            title: '支付失败',
            duration: 2000,
          })
        })
      } else {
        wx.showToast({
          icon: "none",
          title: res.mes,
          duration: 2000,
        })
      }
    })
    // set_chargecardorder
  },
  // 点击优惠券
  click_box_li1(e){
    let that=this;
    let index=e.currentTarget.dataset.index;
    let list=that.data.list;
    let tab_index = that.data.tab_index;
    let ischecked = list[tab_index].couponarry[index].ischecked;
    if (ischecked==true){
      list[tab_index].couponarry[index].ischecked=false
    }else{
      let num=0;
      let presentNum = list[tab_index].presentNum;
      list[tab_index].couponarry.map((item,index)=>{
        if (item.ischecked==true){
          num++
        }
      })
      if (Number(num) < Number(presentNum)){
        list[tab_index].couponarry[index].ischecked = true
      }else{
        wx.showToast({
          title: '可选优惠券种类最多' + presentNum,
          icon:'none',
          duration:1500,
          mask:true
        })
      }
    }
    that.setData({
      list: list
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options){
    let that=this;
    if (options.cardcode){
      that.setData({
        cardcode: options.cardcode
      })
    }
    that.get_cards();
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