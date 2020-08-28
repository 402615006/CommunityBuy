// packageVip/pages/grxx/grxx.js
import { uploadfile_card, get_userinfo,update_userinfo} from "../../utils/server.js";
import { set_verificationcode } from '../../../utils/server.js';
const app = getApp();
Page({
  /**
   * 页面的初始数据
   */
  data: {
    btnclick:true,
    falg:false,
    cards:"",
    name:"",
    tel:"",
    phone:'',
    code:'',   //验证码
    vcode:'',   //后台返回的yan
    times: 0,   //计时器
    block:0,
    isname:''
  },
  //上传文件
  uploadfiles(e) {
    let that = this;
    uploadfile_card(e).then(res => {
      let data = JSON.parse(res);
      console.log(data)
      let cards = data.data[0].idCard;
      let name = data.data[0].name;
      // let sex = data.data[0].sex;
      if (!cards || !name) {
        wx.showToast({
          mask: true,
          title: '未能识别成功',
          icon: 'none',
          duration: 2000
        })
      } else {
        that.setData({
          cards: cards,
          name: name,
          isname: new Array(name.length).join('*') + name.substr(-1)
        })
      }
    }).catch(res => {
      wx.showToast({
        mask: true,
        title: '未识别成功',
        icon: 'none',
        duration: 2000
      })
    })
  },
  //拍照
  click_positive() {
    let that = this;
    let btnclick = that.data.btnclick;
    if (btnclick==false){
      wx.showToast({
        title: '身份信息不能再修改',
        icon:'none',
        duration:1500
      })
      return
    }
    wx.chooseImage({
      count: 1, // 默认9
      sizeType: ['compressed'], // 可以指定是原图还是压缩图，默认二者都有
      sourceType: ['camera'], // 可以指定来源是相册还是相机，默认二者都有
      success: function (res) {
        // 返回选定照片的本地文件路径列表，tempFilePath可以作为img标签的src属性显示图片 
        let tempFilesSize = res.tempFiles[0].size;
        console.log(tempFilesSize);
        let tempFilePaths = res.tempFilePaths[0];
        console.log(tempFilePaths)
        that.uploadfiles(tempFilePaths)
      }
    })
  },
  // 姓名输入
  input_name(e) {
    let that = this;
    let value = e.detail.value;
    that.setData({
      name:value
    })
    // that.data.name = value
  },
  // 身份证号码输入
  input_idcard(e) {
    let that = this;
    let value = e.detail.value;
    that.data.cards = value
  },
  // 手机号码输入
  input_tel(e) {
    let that = this;
    let value = e.detail.value;
    that.setData({
      phone: value
    })
  },
  // 验证码输入
  input_code(e) {
    let that = this;
    let value = e.detail.value;
    that.setData({
      code: value
    })
  },
  // 点击获取验证码
  get_code() {
    let that = this;
    let tel_1 = /^1[0|1|2|3|4|5|6|7|8|9][0-9]{9}$/;
    let phone = that.data.phone;
    if (!phone) {
      wx.showToast({
        title: '请输入手机号',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!tel_1.test(phone)) {
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
        'mobile': phone,
        'descr': '绑定操作'
      }
    };
    data.parameters = JSON.stringify(data.parameters);
    set_verificationcode(data).then(res => {
      console.log(res)
      if (res.status == 0) {
        that.setData({
          vcode: res.mes
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
  // 确认按钮
  btn_click(){
    let that=this;
    let cards = that.data.cards;
    let name=that.data.name;
    let tel=that.data.tel;
    let phone=that.data.phone;
    let code=that.data.code;
    let vcode = that.data.vcode;
    let reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/; 
    if (!cards){
      wx.showToast({
        title: '请输入身份证号码',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!reg.test(cards)){
      wx.showToast({
        title: '身份证格式不正确',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!name){
      wx.showToast({
        title: '请输入姓名',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if(!tel){
      if(!phone){
        wx.showToast({
          title: '请填写手机号',
          icon: 'none',
          duration: 1500,
          mask: true,
        })
        return
      }
      if (!code){
        wx.showToast({
          title: '请输入验证码',
          icon: 'none',
          duration: 1500,
          mask: true,
        })
        return
      }
      if (code != vcode){
        wx.showToast({
          title: '验证码不正确',
          icon: 'none',
          duration: 1500,
          mask: true,
        })
        return
      }
    }
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let data = {
      'actionname': 'updatememberinfo',
      'parameters': {
        "GUID": "",
        "USER_ID": unionid,
        "memcode": memcode,
        "cname": name,
        "mobile": tel || phone,
        "idtype": "IDNO",
        "idno": cards
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    update_userinfo(data).then(res=>{
      console.log(res);
      if(res.status==0){
        console.log(res);
        app.globalData.isentercode = true;
        if (res.memcode){
          wx.setStorageSync('memcode', res.memcode);
        }
        if (res.hasPass==0){
          wx.showToast({
            title: '完善成功',
            icon: 'success',
            duration: 1500
          })
          setTimeout(()=>{
            wx.redirectTo({
              url: '../changepassword/changepassword?type=add',
            })
          },1000)
        }else{
          wx.redirectTo({
            url: '../../../pages/success/success?title=个人信息填写成功'+'&go_back=1',
          })
        }
      }else{
        wx.showToast({
          title:res.mes,
          icon:'none',
          duration:1500,
          mask:true
        })
      }
    })
  },
  get_data(){
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
        let list=res.data;
        let name = list.cname;
        let cards = list.IDNO;
        let tel = list.mobile;
        let isname = new Array(name.length).join('*') + name.substr(-1)
        that.setData({
          falg:true,
          name: list.cname,
          cards: list.IDNO,
          tel: list.mobile,
          isname:isname
        })
        if (name && cards && tel){
          console.log('都有')
          that.setData({
            block:'2'
          })
        }else{
          console.log("没哟")
          that.setData({
            block: '1'
          })
        }
        if (cards){
          that.setData({
            btnclick:false
          })
        }
      }else{
        wx:wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500,
          mask: true,
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this;
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