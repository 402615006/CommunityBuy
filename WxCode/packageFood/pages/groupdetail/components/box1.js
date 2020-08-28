// packageFood/pages/groupdetail/components/box1.js

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    dataList:{
      type:Array,
      value:''
    },
    index: Number,
  },

  /**
   * 组件的初始数据
   */
  data: {
    show: false,
  },

  /**
   * 组件的方法列表
   */
  methods: {
    addorder() {
      var that = this;
      var data = that.data.dataList;
      var index = that.data.index;
      if (data[index].countDown == "00:00:00"){
        wx.showToast({
          title: "拼团时间已过，不可拼团！",
          mask: true,
          duration: 1500,
          icon: 'none'
        })
        return
      }
      that.triggerEvent('addorder', data[index])
    },
    showlogin() {
      let that = this;
      that.setData({
        show: true
      })
    },
    hidelogin() {
      let that = this;
      that.setData({
        show: false
      })
    }
  }
})