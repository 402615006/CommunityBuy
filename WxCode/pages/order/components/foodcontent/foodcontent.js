// pages/order/components/foodcontent/foodcontent.js

import { get_myorderlist } from '../../../../utils/server.js'
import { baserURLfood } from "../../../../utils/api.js"; 

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
    baserURLfood: baserURLfood,
    list:[],
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
    // 点击li到详情
    go_details(e){
      let that=this;
      let item = e.currentTarget.dataset.item;
      if (item.TStatus!=4){
        if (item.TStatus == 2){
          //去支付
          wx.navigateTo({
            url: '/packageFood/pages/kuaicanzhifu/kuaicanzhifu?billcode=' + item.PKCode + '&stocode=' + item.StoCode + '&buscode=' + item.BusCode,
          })
        } else {
          wx.navigateTo({
            url: '/packageFood/pages/orderdetail/orderdetail?detail=' + JSON.stringify(item)
          })
        }
      }else{
        return
      }
    },
    // 去门店
    go_stocode(e){
      // console.log(e);
      let stocode = e.currentTarget.dataset.result.StoCode;
      wx.navigateTo({
        url: '/packageFood/pages/stocode/stocode?stocode='+stocode,
      })
    },
    // 去付款
    go_fukuan(e){
      let result = e.currentTarget.dataset.result;
      wx.navigateTo({
        url: '/packageFood/pages/kuaicanzhifu/kuaicanzhifu?billcode=' + result.PKCode + '&stocode=' + result.StoCode + '&buscode=' + result.BusCode,
      })
    },
    // 去支付成功的详情
    go_detail(e){
      let result = e.currentTarget.dataset.result;
      // console.log(result)
      wx.navigateTo({
        url: '/packageFood/pages/orderdetail/orderdetail?detail=' + JSON.stringify(result),
      })
    },
    // 页面出现函数调取
    onload(){
      let that = this;
      if (that.data.isonload) {
        that.setData({
          isonload: false
        })
        that.get_data()
      }
    },
    // 获取数据
    get_data(){
      let that = this;
      that.setData({
        isloadmore: true
      })
      let memcode = wx.getStorageSync('memcode');
      let data = {
        "actionname": "i_mybill",
        "parameters": {
          'key':'',
          'buscode': '88888888',
          'memcode': memcode,
          'pagesize': that.data.pagesize,
          'page': that.data.currentpage,
          'status': that.data.status
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_myorderlist(data).then(res=>{
        // console.log(res);
        if(res.code==0){
          that.setData({
            isloadmore: false
          })
          let list = res.bill;
          list.map((item,index)=>{
              if (item.DishList){
                item.DishList = JSON.parse(item.DishList);
              }else{
                item.DishList=[];
              }
          })
          switch (that.data.currentpage) {
            case 1:
              that.setData({
                list: list,
                isnextpage: res.nextpage
              })
              break;
            default:
              that.setData({
                list: that.data.list.concat(list),
                isnextpage: res.nextpage
              })
          }
        } else if (res.code == 1){
          that.setData({
            isno:true,
            isloadmore: false
          })
        }else{
          that.setData({
            isloadmore: false
          })
        }
      }).catch(err=>{
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
