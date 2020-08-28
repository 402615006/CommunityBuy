import { get_mycashout, applycashout} from '../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    money:'',
    tel:'',
    input_money:'',
    fee:[],
    fprice:''
  },
  // 全部提现
  btn_all(){
    let that=this;
    let value = that.data.money;
    that.setData({
      input_money: value
    })
    let fee = that.data.fee;
    let fprice = 0;
    if (value == '') {
      that.setData({
        fprice: ''
      })
      return
    }
    for (var i = 0; i < fee.length; i++) {
      if (Number(value) >= Number(fee[i].bmoney) && Number(value) <= Number(fee[i].emoney)) {
        if (fee[i].ctype == 1) {
          fprice = Number(fee[i].val / 100 * value).toFixed(2);
        } else if (fee[i].ctype == 2) {
          fprice = Number(fee[i].val).toFixed(2)
        }
        break
      }
    }
    that.setData({
      fprice: fprice
    })
  },
  bindinput(e){
    let that = this;
    let value = e.detail.value;
    if (value.match(/\d+(\.\d{0,2})?/)) {
      value = value.match(/\d+(\.\d{0,2})?/)[0]
    } 
    that.setData({
      input_money: value
    })
    let fee = that.data.fee;
    let fprice=0;
    if (value==''){
      that.setData({
        fprice: ''
      })
      return
    }
    for (var i = 0; i < fee.length;i++){
      if (Number(value) >= Number(fee[i].bmoney) && Number(value) <= Number(fee[i].emoney)){
        if (fee[i].ctype==1){
          fprice = Number(fee[i].val / 100 * value).toFixed(2);
        } else if (fee[i].ctype == 2){
          fprice = Number(fee[i].val).toFixed(2);
        }
        break
      }
    }
    that.setData({
      fprice: fprice
    })
  },
  // 提现成功
  go_WithdrawalSuccess(){
    let that=this;
    if ((!that.data.input_money) || (Number(that.data.input_money) <= 0)){
      wx.showToast({
        title: '请正确输入提现金额',
        icon: 'none',
        duration: 1500
      })
      return
    }
    if (Number(that.data.input_money) > Number(that.data.money)){
      wx.showToast({
        title: '输入的提现金额超过可提现金额',
        icon: 'none',
        duration: 1500
      })
      return
    }

    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'applycashout',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid,
        'memcode': memcode,
        'money': that.data.input_money,
        'fee': that.data.fprice
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    applycashout(data).then(res=>{
      if(res.status==0){
        wx.redirectTo({
          url: '../WithdrawalSuccess/WithdrawalSuccess',
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
  get_data(){
    let that=this;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'mycashout',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid,
        'memcode': memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_mycashout(data).then(res=>{
      if(res.status==0){
        let money = Number(res.money.amount).toFixed(2);
        let tel = res.money.wxaccount;
        let fee = res.fee;
        that.setData({
          money: money,
          tel: tel,
          fee: fee
        })
      }else{
        wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500
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

  }
})