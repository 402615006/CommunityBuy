
import { no_password, get_memberinfo, get_userinfo} from '../../utils/server.js'
import { verificationcardpwd } from '../../../utils/server.js';
Page({

  /**
   * 页面的初始数据
   */
  data:{
    checked:false,
    tel:'',
    money:''
  },
  // 修改手机号
  go_changetel(){
    let that = this;
    that.get_userinfo('changetel');
  },
  // 修改密码
  go_changepassword(){
    let that=this;
    that.get_userinfo('changepassword')
  },
  // 获取完善信息
  get_userinfo(e){
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
    get_userinfo(data).then(res => {
      if (res.status == 0) {
        let list = res.data;
        let name = list.cname;
        let IDNO = list.IDNO;
        let tel = list.mobile;
        if (name && IDNO && tel) {
            if (e =='changepassword'){
              wx.navigateTo({
                url: '../changepassword/changepassword?type=update'
              })
            } else if (e =='changetel'){
              wx.navigateTo({
                url: '../changetel2/changetel2?IDNO=' + IDNO,
              })
            } else if (e == 'changechecked'){
              that.selectComponent("#text").showlogin();
            }
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

  // 小额免密开关点击
  btn_checked(){
    let that=this;
    let checked = that.data.checked;
    if (checked){
      that.no_password(0,'')
    }else{
      // that.selectComponent("#text").showlogin();
      that.get_userinfo('changechecked')
    }
  },
  no_password(i,money){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let data = {
      'actionname': 'memberschangeispwd',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid ,
        'memcode': memcode,
        "ispwd":i,
        'needmoney': money
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    no_password(data).then(res=>{
      if (res.status==0){
        that.setData({
          checked:!that.data.checked
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
  // 监听组件保存成功
  save(e){
    let that=this;
    console.log(e)
    that.setData({
      money: e.detail
    })
    this.selectComponent("#password").showModal();
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
        that.no_password(1,that.data.money)
      } else if (res.status == 2){
        wx.showModal({
          title: res.mes,
          content: '是否重试?',
          success(res) {
            if (res.confirm) {
              that.selectComponent("#password").showModal();
            } else if (res.cancel) {
              
            }
          }
        })
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
  get_data(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let unionid = wx.getStorageSync('unionid');
    let data = {
      "actionname": "getmemberinfo",
      "parameters": { 
        "GUID": "",
        "USER_ID": unionid,
        "memcode": memcode,
      }
    }
    console.log("执行")
    data.parameters = JSON.stringify(data.parameters);
    get_memberinfo(data).then(res=>{
      if(res.code==0){
        let list=res.data[0];
        console.log(list)
        let checked=false;
        if (list.notpwd == 0 || list.notpwd == '' || list.notpwd== 'null'){
          checked=false;
        }else{
          checked = true;
        }
        that.setData({
          checked: checked,
          tel: list.mobile
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
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    
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
    let that = this;
    that.get_data();
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