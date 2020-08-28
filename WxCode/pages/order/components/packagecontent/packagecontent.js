
import { myordersbyproducts } from '../../../../utils/server.js'
import { baserURLOrganization } from "../../../../utils/api.js"; 

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    status: {
      type: String,
      value: ''
    },
    type: {
      type: String,
      value: ''
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    url: baserURLOrganization,
    list: [],
    isnextpage: 0,
    currentpage: 1,
    pagesize: 10,
    isloadmore: false,
    isnomore: false,
    isonload: true,   //页面是否第一次进入 
    isno: false
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 点击去详情
    togetail(e){
      console.log(e);
      let item = e.currentTarget.dataset.item;
      console.log(item)
      let code = item.code;
      let ordtype = item.ordtype;
      let buscode = item.buscode;
      let stocode = item.stocode;
      wx.navigateTo({
        url: '/pages/orderDetail/orderDetail?code=' + code+'&type='+ordtype+'&buscode='+buscode+'&stocode='+stocode, 
      })
    },
    // 页面出现函数调取
    onload(e) {
      let that = this;
        that.setData({
          list:[],
          currentpage:1,
          isonload: false
        })
        that.get_data()
    },
    // 获取数据
    get_data() {
      let that=this;
      that.setData({
        isloadmore: true,
        isno: false
      })
      let memcode = wx.getStorageSync('memcode');
        let data = {
          'actionname': "myordersbyproducts",
          'parameters': {
            "GUID": "",
            "USER_ID": "",
            "memcode":memcode,
            "currentpage":that.data.currentpage,
            "pagesize":that.data.pagesize,
            "status":that.data.status,
            "dtype":that.data.type, 
          }
        }
        data.parameters = JSON.stringify(data.parameters);
        myordersbyproducts(data).then(res => {
          console.log(res)
          that.setData({
            isloadmore:false,
          })
          if (res.code == 0) {
            let list = res.data;
            for(var i = 0;i<list.length;i++){
              list[i].smallimg = that.data.url+list[i].smallimg;
            }
            that.setData({
              list: that.data.list.concat(list),
              isnextpage:res.isnextpage,
              isno:false
            })
          } else {
            that.setData({
              list:[],
              isno: true
            })
          }
        }).catch(err => {
          that.setData({
            isloadmore: false
          })
        })
    },
    // 滑动到底部
    lower() {
      let that = this;
      let isnextpage = that.data.isnextpage;
      let isloadmore = that.data.isloadmore;
      if (isnextpage <= 0 || isloadmore == true) {
        return
      }
      that.data.currentpage = that.data.currentpage + 1;
      that.get_data();
    }
  }
})
