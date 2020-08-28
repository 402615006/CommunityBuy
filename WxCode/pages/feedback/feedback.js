
import { addFeedback, uploadfileimg } from '../../utils/server.js'

Page({

  /**
   * 页面的初始数据
   */
  data: {
    type:['功能异常','其他问题'],
    type_index:0,
    img1:'',
    img:'',
    mark:'',
    tel:'',
    name:''
  },
  btn_type(e){
    let that=this;
    console.log(e);
    let index=e.currentTarget.dataset.index;
    that.setData({
      type_index: index
    })
  },
  input_mark(e) {
    let that = this;
    let mark = e.detail.value;
    that.setData({
      mark: mark
    })
  },
  input_name(e) {
    let that = this;
    let name = e.detail.value;
    that.setData({
      name: name
    })
  },
  input_tel(e) {
    let that = this;
    let tel = e.detail.value;
    that.setData({
      tel: tel
    })
  },
  // 点击添加图片
  click_btn(){
    let that = this;
    wx.chooseImage({
      count: 1, // 默认9
      sizeType: ['compressed'], // 可以指定是原图还是压缩图，默认二者都有
      sourceType: ['camera','album'], // 可以指定来源是相册还是相机，默认二者都有
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
              img: res.path
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
  // 确定
  btn(){
    let that=this;
    let name=that.data.name;
    let tel = that.data.tel;
    let mark = that.data.mark;
    let img=that.data.img;
    let tel_1 = /^1[0|1|2|3|4|5|6|7|8|9][0-9]{9}$/;
    if (!mark){
      wx.showToast({
        title: '请填写反馈内容',
        icon:'none',
        duration:1500,
        mask:true
      })
      return
    }
    if (!name){
      wx.showToast({
        title: '请填写联系人',
        icon:'none',
        duration:1500,
        mask:true
      })
      return
    }
    if (!tel){
      wx.showToast({
        title: '请填写联系电话',
        icon:'none',
        duration:1500,
        mask:true
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
      "actionname": "add",
      "parameters": {
        "GUID": "",
        "USER_ID": "",
        "fcontent":mark,
        "fimg": img,
        "phone": tel,
        "CCode": "",
        "CCname": name,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    addFeedback(data).then(res=>{
      if (res.code==0){
        wx.navigateTo({
          url: '../success/success?title=反馈成功',
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