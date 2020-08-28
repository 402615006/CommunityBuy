// pages/verification/verification.js
import { uploadfile_card, members_changeidno } from "../../utils/server.js"
Page({

  /**
   * 页面的初始数据
   */
  data: {
    IDcard: '',
    name: '',
    sex: '',
    cards: '',
    showLoading: false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    // this.get_data();
  },
  get_data() {
    let that = this;
    let unionid = wx.getStorageSync('unionid');
    let data = {
      "actionname": "mpuserinfo",
      "parameters": "{'GUID':'88888888','USER_ID':'" + unionid + "'}"
    }
    get_IDcard(data).then(res => {
      console.log("56666666666666666666")
      console.log(res)
      if (res[0].IDNO) {
        that.setData({
          IDcard: res[0].IDNO
        })
        console.log(res)
      } else {
        wx.showToast({
          mask: true,
          title: '您的身份信息不完整，请到柜台去修改',
          icon: 'none',
          duration: 2000
        })
      }
    })
  },
  //成功去修改手机号
  btn() {
    let that=this;
    let cards = that.data.cards;
    let memcode = wx.getStorageSync('memcode');
    if (!cards){
      wx.showToast({
        mask: true,
        title: '请先上传身份证识别',
        icon: 'none',
        duration: 2000
      })
      return
    }
    let data={
      "actionname": "memberschangeidno",
      "parameters": {
        "GUID": "",
        "USER_ID": "",
        "cname": "",
        "memcode": memcode,
        "IDNO": cards
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    members_changeidno(data).then(res=>{
      if(res.code==0){
        wx.navigateTo({
          url: '../changetel2/changetel2'
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
  //上传文件
  uploadfiles(e) {
    let that = this;
    uploadfile_card(e).then(res => {
      let data =JSON.parse(res);
      console.log(data)
      let cards = data.data[0].idCard;
      let name = data.data[0].name;
      let sex = data.data[0].sex;
      if (!cards) {
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
          sex: sex,
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
  go_paizhao() {
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
        that.uploadfiles(tempFilePaths)
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