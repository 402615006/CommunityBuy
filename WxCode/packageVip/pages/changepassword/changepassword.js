
import { update_password,get_userinfo } from '../../utils/server.js';
import { set_verificationcode } from '../../../utils/server.js';
const app = getApp();
Page({

  /**
   * 页面的初始数据
   */
  data: {
    isentercode: false,
    tel:'',
    code:'',
    idcard:'',
    password:'',
    newpassword:'',

    verification_code:'',
    times:0,
    type:'',
    isshow:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    that.setData({
      isentercode: app.globalData.isentercode
    })
    if(options.type){
      let type = options.type;
      that.setData({
        type: type
      })
      if (type=='add'){
        wx.setNavigationBarTitle({
          title: '设置支付密码'
        })
      }else if(type=="update"){
        wx.setNavigationBarTitle({
          title: '修改支付密码'
        })
      }
    }else{
      wx.setNavigationBarTitle({
        title: '修改支付密码'
      })
    }

    that.get_data();
  },
  // 获取信息
  get_data(){
    let that = this;
    console.log('111')
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let data = {
      'actionname': 'memberinfo',
      'parameters': {
        "GUID": "",
        "USER_ID": unionid,
        "memcode": memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_userinfo(data).then(res=>{
      if (res.status == 0) {
        let list = res.data;
        let name = list.cname;
        let card = list.IDNO;
        let tel = list.mobile;
        if (name && card && tel){
          let type=that.data.type;
          if (type=='add'){
            that.setData({
              tel: tel,
              idcard:card,
              isshow:true
            })
          }else{
            that.setData({
              tel: tel,
              isshow:true
            })
          }
        }else{
          wx.showToast({
            title: '身份信息未完善,请先去完善',
            icon: 'none',
            duration: 1500,
            mask: true
          })
          setTimeout(()=>{
            wx.navigateBack()
          },1500)
        }
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
  // 点击获取验证码
  get_code(){
    let that = this;
    let tel_1 = /^1[0|1|2|3|4|5|6|7|8|9][0-9]{9}$/;
    let tel = that.data.tel;
    if (!tel) {
      wx.showToast({
        title: '请输入手机号',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!tel_1.test(tel)) {
      wx.showToast({
        title: '手机号格式不正确',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    let data = {
      "actionname": "sendmsg",
      "parameters": {
        'mobile': tel,
        'descr': '绑定操作'
      }
    };
    data.parameters = JSON.stringify(data.parameters);
    set_verificationcode(data).then(res => {
      console.log(res)
      if (res.status == 0) {
        that.setData({
          verification_code: res.mes
        })
      }
    })
    that.set_times();
  },
  // 定时器
  set_times(){
    let that=this;
    let times = 60;
    that.setData({
      times: times
    })
    let loading = setInterval(() => {
      if (times > 0) {
        times = times - 1
      } else {
        clearInterval(loading)
      }
      that.setData({
        times: times
      })
    }, 1000)
  },
  input_tel(e){
    let that=this;
    let tel=e.detail.value;
    that.setData({
      tel:tel
    })
  },
  input_code(e){
    let that = this;
    let code = e.detail.value;
    that.setData({
      code: code
    })
  },
  input_idcard(e){
    let that = this;
    let idcard = e.detail.value;
    that.setData({
      idcard: idcard
    })
  },
  input_password(e){
    let that = this;
    let password = e.detail.value;
    that.setData({
      password: password
    })
  },
  input_newpassword(e){
    let that = this;
    let newpassword = e.detail.value;
    that.setData({
      newpassword: newpassword
    })
  },
  // 确定
  btn(){
    let that=this;
    let tel=that.data.tel;
    let code = that.data.code;
    let idcard = that.data.idcard;
    let password = that.data.password;
    let newpassword = that.data.newpassword;
    let verification_code = that.data.verification_code;
    let reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/; 
    let reg_number = new RegExp("^[0-9]*$");
    if(!tel){
      wx.showToast({
        title: '请输入手机号',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (that.data.isentercode == false) {
      if (!verification_code){
        wx.showToast({
          title: '请先点击获取验证码',
          icon: 'none',
          duration: 1500,
          mask: true,
        })
        return
      }
      if(!code){
        wx.showToast({
          title: '请输入验证码',
          icon: 'none',
          duration: 1500,
          mask: true,
        })
        return
      }
      if (code != verification_code) {
        wx.showToast({
          title: '验证码错误',
          icon: 'none',
          duration: 1500,
          mask: true,
        })
        return
      }
    }
    if (!idcard){
      wx.showToast({
        title: '请输入身份证号',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!reg.test(idcard)) {
      wx.showToast({
        title: '身份证格式不正确',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    
    if (!password){
      wx.showToast({
        title: '请输入新密码',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!newpassword){
      wx.showToast({
        title: '请再次输入密码',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!reg_number.test(password)) {
      wx.showToast({
        title: '密码必须是数字',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (password!=newpassword){
      wx.showToast({
        title: '两次密码输入不一致',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    that.update_password()
  },
  // 修改密码
  update_password(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let data={
      "actionname": "memberschangepwd",
      "parameters":{
        "GUID": "",
        "USER_ID": unionid,
        "mobile": that.data.tel,
        "memcode": memcode,
        "IDNO": that.data.idcard,
        "paypwd": that.data.password
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    update_password(data).then(res=>{
      if(res.status==0){
        let type=that.data.type;
        if(type=='add'){
          wx.navigateTo({
            url: '../../../pages/success/success?title=设置成功' + '&go_back=2',
          })
        }else{
          wx.navigateTo({
            url: '../../../pages/success/success?title=修改成功' + '&go_back=2',
          })
        }
        app.globalData.isentercode = true;
      }else{
        wx.showToast({
          title: res.mes,
          icon:'none',
          mask:true,
          duration:1500
        })
      }
    })
  },
  // 获取个人信息
  get_userinfo(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'memberinfo',
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "memcode": memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_userinfo(data).then(res=>{
      if(res.status==0){
        let list = res.data;
        let name = list.cname;
        let card = list.IDNO;
        let tel = list.mobile;
        if (name && card && tel) {
          that.setData({
            flag:true
          })
        }else{
          wx.navigateTo({
            url: '../grxx/grxx',
          })
        }
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