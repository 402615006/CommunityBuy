
import { bind_vipcards, get_userinfo } from '../../utils/server.js';
import { set_verificationcode } from '../../../utils/server.js';

const app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    flag:false,
    tel: '',
    code: '',
    idcard: '',
    password: '',
    cards: '',

    verification_code: '',
    times: 0,
    isentercode: false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    that.setData({
      isentercode: app.globalData.isentercode
    })
  },
  get_userinfo() {
    let that = this;
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
    console.log(data)
    get_userinfo(data).then(res => {
      if (res.status == 0) {
        let list = res.data;
        let name = list.cname;
        let card = list.IDNO;
        let tel = list.mobile;
        if (name && card && tel) {
          that.setData({
            tel:tel,
            idcard: card,
            flag: true
          })
        } else {
          that.selectComponent("#showbox").show();
        }
      } else {
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
  get_code() {
    let that = this;
    let tel_1 = /^1[0|1|2|3|4|5|6|7|8|9][0-9]{9}$/;
    let tel=that.data.tel;
    if(!tel){
      wx.showToast({
        title: '请输入手机号',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!tel_1.test(tel)){
      wx.showToast({
        title: '手机号格式不正确',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    let data={
      "actionname":"sendmsg",
      "parameters":{
          'mobile':tel,
          'descr':'绑定操作'
      }
    };
    data.parameters = JSON.stringify(data.parameters);
    set_verificationcode(data).then(res=>{
      console.log(res)
      if(res.status==0){
        that.setData({
          verification_code:res.mes
        })
      }
    })
    that.set_times();
  },
  // 定时器
  set_times() {
    let that = this;
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
  input_tel(e) {
    let that = this;
    let tel = e.detail.value;
    that.setData({
      tel: tel
    })
  },
  input_code(e) {
    let that = this;
    let code = e.detail.value;
    that.setData({
      code: code
    })
  },
  input_idcard(e) {
    let that = this;
    let idcard = e.detail.value;
    that.setData({
      idcard: idcard
    })
  },
  input_password(e) {
    let that = this;
    let password = e.detail.value;
    that.setData({
      password: password
    })
  },
  input_cards(e) {
    let that = this;
    let cards = e.detail.value;
    that.setData({
      cards: cards
    })
  },
  // 确定
  btn() {
    let that = this;
    let tel = that.data.tel;
    let code = that.data.code;
    let idcard = that.data.idcard;
    let password = that.data.password;
    let cards = that.data.cards;
    let verification_code = that.data.verification_code;
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    if (!tel) {
      wx.showToast({
        title: '请输入手机号',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (that.data.isentercode==false){
      if (!verification_code) {
        wx.showToast({
          title: '请先点击获取验证码',
          icon: 'none',
          duration: 1500,
          mask: true,
        })
        return
      }
      if (!code) {
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
    if (!idcard) {
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
    if (!password) {
      wx.showToast({
        title: '请输入会员卡密码',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!cards) {
      wx.showToast({
        title: '请输入会员卡号',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    
    let data = {
      'actionname': 'bindmemcard',
      'parameters': {
        'GUID': '',
        'openid': unionid,
        'USER_ID': '',
        'memcode': memcode,
        'phoneno': tel,
        'idno': idcard,
        'cardcode': cards,
        'pass': password
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    bind_vipcards(data).then(res=>{
      if(res.status==0){
        if (res.newmemcode) {
          wx.setStorageSync('memcode', res.newmemcode);
        }
        wx.redirectTo({
          url: '/pages/success/success?title=绑定成功'+'&go_back=2'
        })
        app.globalData.isentercode = true;
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
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    let that=this;
    that.get_userinfo();
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