// components/homebox/homebox.js
import { get_friendimg} from '../../utils/server.js';
import { baserURLOrganization } from '../../utils/api.js';
Component({
  /**
   * 组件的属性列表
   */
  properties: {

  },

  /**
   * 组件的初始数据
   */
  data: {
    show: false,
    img: '',
    url: baserURLOrganization
  },

  /**
   * 组件的方法列表
   */
  methods: {
    get_data(code) {
      let that = this;
      if (that.data.img){
        that.show();
        return
      }
      let yqcode='';
      if (wx.getStorageSync('invitecode')){
        yqcode = wx.getStorageSync('invitecode');
      }
      let data = {
        "actionname": "vipspread",
        "parameters": {
          "GUID": "",
          "USER_ID": "",
          "code": code,
          "yqcode": yqcode
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_friendimg(data).then(res => {
        if (res.code == 0) {
          let img = res.msg;
          if (img) {
            that.setData({
              img: baserURLOrganization+img
            })
            that.show();
          }
        }else{
          wx.showToast({
            title: res.msg,
            duration:1500,
            icon:'none'
          })
        }
      })
    },
    // 显示
    show() {
      let that = this;
      that.setData({
        show: true
      })
    },
    // 关闭
    hide() {
      let that = this;
      that.setData({
        show: false
      })
    },
    // 保存
    saveImg(){
      this.saveImgToLocal();
    },
    handleSetting(e){
      let that=this;
      if (e.detail.authSetting['scope.writePhotosAlbum']){
        that.saveImgToLocal();
      }
    },
    saveImgToLocal: function () {
      let that = this;
      let img = that.data.img;
      wx.downloadFile({
        url: img,
        success: function (res) {
          console.log(res);
          //图片保存到本地
          wx.saveImageToPhotosAlbum({
            filePath: res.tempFilePath,
            success: function (data){
              wx.showToast({
                title: '保存成功',
                icon: 'success',
                duration: 2000
              })
              that.hide();
            },
          })
        }
      })
    },
  }
})
