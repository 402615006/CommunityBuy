// packageVip/pages/opencardconfirm/opencardconfirm.js

import { set_opencard } from '../../utils/server.js'
import { uploadfileimg } from '../../../utils/server.js'
import { set_verificationcode } from '../../../utils/server.js';
import { wechat_payment } from '../../../utils/util.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    times: 0,
    type:'0',
    vcode:'', //获取到了后台传入验证码
    name:'',  //姓名
    idcard: '',//身份证
    tel: '',//手机
    code: '',  //输入的验证码号
    img1:'',
    img2:'',
    img3:'',
    imgpath1:'',
    imgpath2:'',
    imgpath3:'',


    levelcode:'', //会员卡code
    money:0, //开卡金额
    pcode: '', //开卡活动code
    coupons:'', //开卡赠送优惠券
  },
  // 开通会员卡
  set_opencard(){
    let that=this;
    let openid = wx.getStorageSync('openid');
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname':'opencardorder',
       'parameters':{ 
         "GUID":"", 
         "USER_ID":"", 
         "openid": openid, 
         "memcode": memcode, 
         "levelcode": that.data.levelcode, 
         "pcode": that.data.pcode, 
         "coupons": that.data.coupons, 
         "money":that.data.money, 
         "phoneno": that.data.tel, 
         "idno": that.data.idcard, 
         "cname":that.data.name, 
         "idimg1": that.data.imgpath2, 
         "idimg2": that.data.imgpath3,
         'way':'WX'
         }
    }
    data.parameters = JSON.stringify(data.parameters);
    set_opencard(data).then(res=>{
      if (res.timeStamp){
        wechat_payment(res.timeStamp, res.nonceStr, res.package, res.signType, res.paySign).then(res=>{
          wx.navigateTo({
            url: '../../../pages/success/success?title=开卡成功',
          })
        }).catch(err=>{
          wx.showToast({
            icon: "none",
            title: '支付失败',
            duration: 2000,
          })
        })
      }else{
        wx.showToast({
          icon: "none",
          title: res.mes,
          duration: 2000,
        })
      }
    })
  },
  // 点击获取验证码
  get_code() {
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
  // 点击第一个照相
  click_Avatar(){
    let that = this;
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
    
        let memcode = wx.getStorageSync('memcode');
        let search = 'type=1&memcode=' + memcode + '&imgtype=3';
        uploadfileimg(search, tempFilePaths).then(res => {
          res = JSON.parse(res);
          console.log(res);
          if (res.status == 0) {
            that.setData({
              img1: tempFilePaths,
              imgpath1: res.path
            })
          } else {
            wx.showToast({
              title: '上传失败',
              mask: true,
              duration: 1000,
              icon: 'none'
            })
          }
        }).catch(err => {
          wx.showToast({
            title: '上传失败',
            mask: true,
            duration: 1000,
            icon: 'none'
          })
        })   
      }
    })
  },
  // 点击上传身份证正面照相
  click_positive(){
    let that = this;
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
        
        let memcode = wx.getStorageSync('memcode');
        let search = 'type=1&memcode=' + memcode +'&imgtype=1';
        uploadfileimg(search,tempFilePaths).then(res=>{
          res=JSON.parse(res);
          console.log(res);
          if(res.status==0){
            that.setData({
              img2: tempFilePaths,
              imgpath2:res.path
            })
          }else{
            wx.showToast({
              title: '上传失败',
              mask:true,
              duration:1000,
              icon:'none'
            })
          }
        }).catch(err=>{
          wx.showToast({
            title: '上传失败',
            mask: true,
            duration: 1000,
            icon: 'none'
          })
        })
      }
    })
  },
  // 点击上传身份证背面照相
  click_back(){
    let that = this;
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
       
        let memcode = wx.getStorageSync('memcode');
        let search = 'type=1&memcode=' + memcode + '&imgtype=2';
        uploadfileimg(search, tempFilePaths).then(res => {
          res = JSON.parse(res);
          console.log(res);
          if (res.status == 0) {
            that.setData({
              img3: tempFilePaths,
              imgpath3: res.path
            })
          } else {
            wx.showToast({
              title: '上传失败',
              mask: true,
              duration: 1000,
              icon: 'none'
            })
          }
        }).catch(err => {
          wx.showToast({
            title: '上传失败',
            mask: true,
            duration: 1000,
            icon: 'none'
          })
        })
      }
    })
  },
  // 姓名输入
  input_name(e){
    let that = this;
    let value = e.detail.value;
    that.setData({
      name: value
    })
  },
  // 身份证输入
  input_idcard(e){
    let that = this;
    let value = e.detail.value;
    that.setData({
      idcard: value
    })
  },
  // 手机号码输入
  input_tel(e){
    let that = this;
    let value = e.detail.value;
    that.setData({
      tel: value
    })
  },
  // 验证码输入
  input_code(e){
    let that=this;
    let value=e.detail.value;
    that.setData({
      code: value
    })
  },
  btn_click(){
    let that=this;
    let name=that.data.name;  //姓名
    let idcard=that.data.idcard;//身份证
    let tel=that.data.tel ;//手机
    let code = that.data.code;  //输入的验证码号
    let vcode = that.data.vcode;
    let reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/; 

    let imgpath1 = that.data.imgpath1;
    let imgpath2 = that.data.imgpath2;
    let imgpath3 = that.data.imgpath3;
    if (!imgpath2){
      wx.showToast({
        title: '请先上传身份证正面照',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!imgpath3){
      wx.showToast({
        title: '请先上传身份证背面照',
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
    if (!idcard){
      wx.showToast({
        title: '请输入身份证号',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!reg.test(idcard)){
      wx.showToast({
        title: '身份证格式不正确',
        icon: 'none',
        duration: 1500,
        mask: true,
      })
      return
    }
    if (!tel){
      wx.showToast({
        title: '请输入手机号码',
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
    that.set_opencard();
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    console.log(options)
    let that=this;
    let type = options.type;
    if (type){
      that.setData({
        type: type
      })
      if (type==1){
        wx.setNavigationBarTitle({
          title: '领取会员卡'
        })
      }
    }
    let levelcode = options.levelcode;
    let money = options.money;
    let pcode = options.pcode;
    let coupons = options.coupons;
    that.setData({
      levelcode: levelcode,
      money: money,
      pcode: pcode,
      coupons: coupons
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