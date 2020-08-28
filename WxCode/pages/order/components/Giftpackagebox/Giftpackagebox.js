// pages/order/components/Giftpackagebox/Giftpackagebox.js
import { myordersbyvippro } from '../../../../utils/server.js'
import { baserURLOrganization } from '../../../../utils/api.js';
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
    show:false,
    list: [],
    isnextpage: 0,
    currentpage: 1,
    pagesize: 10,
    isloadmore: false,
    isnomore: false,
    isonload: true,   //页面是否第一次进入 
    isno: false,
    url: baserURLOrganization,
  },

  /**
   * 组件的方法列表
   */
  methods: {
    show(){
      let that = this;
      that.setData({
        show: true,
        list:[],
        currentpage:1
      })
      that.get_data();
    },
    hide() {
      let that = this;
      that.setData({
        show: false
      })
    },
    // 获取数据
    get_data(){
      let that=this;
      that.setData({
        isloadmore: true
      })
      let memcode = wx.getStorageSync('memcode');
        let data = {
          'actionname': "myordersbyvippro",
          'parameters': {
            "GUID": "",
            "USER_ID": "",
            "memcode":memcode,
            "currentpage":that.data.currentpage,
            "pagesize":that.data.pagesize
          }
        }
        data.parameters = JSON.stringify(data.parameters);
        myordersbyvippro(data).then(res => {
          console.log(res)
          that.setData({
            isloadmore:false,
          })
          if (res.code == 0) {
            let list = res.data;
            for(var i = 0;i<list.length;i++){
              list[i].simg = that.data.url+list[i].simg;
            }
            that.setData({
              list: that.data.list.concat(list),
              isnextpage:res.isnextpage
            })
          } else {
            that.setData({
              list:[],
              isno: true
            })
          }
        })
    },
    togetail(e){
      console.log(e)
      var that = this;
      var code =e.currentTarget.dataset.code;
      wx.navigateTo({
        url: '/pages/orderDetail/orderDetail?code=' + code+'&type=5',    
      })
    },
    // 上滑加载更多
    lower(){
      let that = this;
      let isnextpage = that.data.isnextpage;
      if (isnextpage == 0) {
        return
      }
      that.data.currentpage = that.data.currentpage + 1;
      that.get_data();
    }
  }
})
