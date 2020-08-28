
import { get_mymemberinfo } from '../../../utils/server.js';
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
    isshowbox:false,
    list: '',
    warp_show:true
  },
  
  /**
   * 组件的方法列表
   */
  methods: {
    cen_box() {
      let that = this;
      that.setData({
        isshowbox: !that.data.isshowbox
      })
    },
    show(lookmemcode, warp_show) {
      let that = this;
      that.get_data(lookmemcode);
      if (warp_show==false){
        that.setData({
          warp_show:false
        })
      }else{
        that.setData({
          warp_show: true
        })
      }
    },
    previewImg: function (e) {
      let that = this;
      let src = e.currentTarget.dataset.src;
      src = src.replace(/\/132/, '/0');
      let str=[];
      str.push(src)
      wx.previewImage({
        current: src,     //当前图片地址
        urls: str
      })
    },
    // 获取数据
    get_data(lookmemcode){
      let that=this;
      let unionid = wx.getStorageSync('unionid');
      let data = {
        "actionname": "mymemberinfo",
        "parameters": {
          "GUID": "",
          "USER_ID": unionid,
          "memcode": lookmemcode,
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_mymemberinfo(data).then(res=>{
        if(res.status==0){
          let list=res.data;
          that.setData({
            list: list,
            isshowbox:true
          })
        }else{
          wx.showToast({
            title: res.mes,
            icon: 'none',
            duration: 1500
          })
        }
      })
    }
  }
})
