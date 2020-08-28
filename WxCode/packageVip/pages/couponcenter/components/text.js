
import { get_receivecouponlist } from '../../../utils/server.js'


Component({
  /**
   * 组件的属性列表
   */
  properties: {
    status:{
      type:String,
      value:''
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    list: [],
    isnextpage: 0,
    currentpage: 1,
    pagesize: 10,
    isloadmore: false,
    isnomore: false,
    isonload: true,   //页面是否第一次进入 
    isno:false
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 页面出现函数调取
    onload() {
      let that = this;
      if (that.data.isonload) {
        that.setData({
          isonload: false
        })
        that.get_data()
      }
    },
    go_detail(e){
      console.log(e);
      let result=e.currentTarget.dataset.result;
      result=JSON.stringify(result)
      wx.navigateTo({
        url: '../couponcenterdetail/couponcenterdetail?result='+result,
      })
    },
    // 获取数据
    get_data() {
      let that = this;
      // that.setData({
      //   isloadmore: true
      // })
      // var list=[
      //   {
      //     stoname: '恩佐的餐厅',
      //     price: '20',
      //     times: '2019.07.15',
      //     marke: '订单满199立减',
      //     status: '0'
      //   },
      //   {
      //     stoname: '恩佐的餐厅',
      //     price: '20',
      //     times: '2019.07.15',
      //     marke: '订单满199立减',
      //     status: '1'
      //   },
      //   {
      //     stoname: '恩佐的餐厅',
      //     price: '20',
      //     times: '2019.07.15',
      //     marke: '订单满199立减',
      //     status: '0'
      //   },
      //   {
      //     stoname: '恩佐的餐厅',
      //     price: '20',
      //     times: '2019.07.15',
      //     marke: '订单满199立减',
      //     status: '1'
      //   },
      //   {
      //     stoname: '恩佐的餐厅',
      //     price: '20',
      //     times: '2019.07.15',
      //     marke: '订单满199立减',
      //     status: '1'
      //   },
      //   {
      //     stoname: '恩佐的餐厅',
      //     price: '20',
      //     times: '2019.07.15',
      //     marke: '订单满199立减',
      //     status: '1'
      //   },
      //   {
      //     stoname: '恩佐的餐厅',
      //     price: '20',
      //     times: '2019.07.15',
      //     marke: '订单满199立减',
      //     status: '1'
      //   },
      //   {
      //     stoname: '恩佐的餐厅',
      //     price: '20',
      //     times: '2019.07.15',
      //     marke: '订单满199立减',
      //     status: '1'
      //   },
      //   {
      //     stoname: '恩佐的餐厅',
      //     price: '20',
      //     times: '2019.07.15',
      //     marke: '订单满199立减',
      //     status: '1'
      //   },
      // ]
      // setTimeout(() => {
      //   that.setData({
      //     list: list,
      //     isloadmore: false,
      //     isnomore: true
      //   })
      // }, 2000)
      let memcode = wx.getStorageSync('memcode');
      let unionid = wx.getStorageSync('unionid');
      let data = {
        "actionname": "getreceivecouponlist",
        "parameters": {
          'GUID':'',
          'USER_ID': unionid,
          'memcode': memcode,
          'sectype': that.data.status,
          'pagesize': that.data.pagesize,
          'currentpage': that.data.currentpage,
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      that.setData({
        isloadmore: true
      })
      get_receivecouponlist(data).then(res => {
        that.setData({
          isloadmore: false
        })
        if (res.code == 0) {
          let list = res.data;
          switch (that.data.currentpage) {
            case 1:
              that.setData({
                list: list,
                isnextpage: res.isnextpage
              })
              break;
            default:
              that.setData({
                list: that.data.list.concat(list),
                isnextpage: res.isnextpage
              })
          }
        }else if(res.code==1){
          switch (that.data.currentpage) {
            case 1:
              that.setData({
                isno: true
              })
              break;
            default:
              break
          }
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
    },
  }
})
