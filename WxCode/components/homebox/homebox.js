// components/homebox/homebox.js
import { get_homeMode } from '../../utils/server.js';

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
    url: ''
  },

  /**
   * 组件的方法列表
   */
  methods: {
    get_data() {
      let that = this;
      let data = {
        "actionname": "homexf",
        "parameters": {}
      }
      data.parameters = JSON.stringify(data.parameters);
      get_homeMode(data).then(res => {
        // console.log(res);
        if (res.code == 0) {
          let img = res.wx_home_img;
          let url = res.wx_home_url;
          if (img && url) {
            that.setData({
              img: img,
              url: url
            })
            that.show();
          }
        }
      })
    },
    // 去开卡
    go_detail() {
      let that = this;
      wx.navigateTo({
        url: that.data.url,
      })
      that.hide();
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
    }
  }
})
