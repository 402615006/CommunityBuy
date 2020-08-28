// packageFood/pages/groupdetail/components/box1.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    dataList:Array,
    memcode:String,
  },

  /**
   * 组件的初始数据
   */
  data: {
    show: false,
    currentData:'',
    currentIndex:0,
  },

  /**
   * 组件的方法列表
   */
  methods: {
    addorder(e) {
      var that = this;
      that.triggerEvent('addorder', e.detail)
    },
    go_box1(e) {
      let that = this;
      var currentData = e.currentTarget.dataset.list;
      var index = e.currentTarget.dataset.index;
      if (currentData.countDown == "00:00:00") {
        wx.showToast({
          title: "拼团时间已过，不可拼团！",
          mask: true,
          duration: 1500,
          icon: 'none'
        })
        return
      }
      that.setData({
        currentData: currentData,
        currentIndex: index
      })
      this.selectComponent("#box1").showlogin();
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
      this.selectComponent("#box1").hidelogin();
    }
  }
})
