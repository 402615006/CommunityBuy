// pages/order/components/groupcontent/groupcontent.js

import {
  getPTorderlist
} from '../../../../utils/server.js'
import {
  baserURLcard
} from "../../../../utils/api.js"; //域名引入
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    status: {
      type: String,
      value: ''
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    baserURLcard: baserURLcard,
    list: [],
    isnextpage: 0,
    currentpage: 1,
    pagesize: 10,
    isloadmore: false,
    isnomore: false,
    isonload: true, //页面是否第一次进入 
    isno: false
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 去详情
    go_detail(e) {
      let item = e.currentTarget.dataset.item;
      let pkcode = e.currentTarget.dataset.pkcode;
      if (item.tstatus==2){
        return
      }
      wx.navigateTo({
        url: '/packageFood/pages/grorderdetail/grorderdetail?pkcode=' + pkcode,
      })
    },
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
    // 获取数据
    get_data() {
      let that = this;
      that.setData({
        isloadmore: true
      })
      let memcode = wx.getStorageSync('memcode');
      let data = {
        "actionname": "getorderlist",
        "parameters": {
          'GUID': '',
          'USER_ID': '',
          'memcode': memcode,
          'type': that.data.status,
          'PageSize': that.data.pagesize,
          'CurrentPage': that.data.currentpage,
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      getPTorderlist(data).then(res => {
        if (res.status == 0) {
          that.setData({
            isloadmore: false
          })
          let list = res.data;
          for (var i = 0; i < list.length; i++) {
            list[i].shifu = (parseFloat(list[i].pmoney) - parseFloat(list[i].notamount)).toFixed(2);
          }
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
        } else {
          that.setData({
            isno: true,
            isloadmore: false
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
    },
  }
})