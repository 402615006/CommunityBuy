// pages/order/components/hotbox/hotbox.js
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
    foodbox_index: 0,
    foodbox_text: ['全部', '待付款', '已付款', '退款/售后']
  },
  /**
   * 组件的方法列表
   */
  methods: {
    onhide() {
      let that = this;
      if (that.data.show){

      }
    },
    show() {
      let that = this;
      that.setData({
        show: true
      })
      let foodbox_index = that.data.foodbox_index;
      let id = "#text" + foodbox_index;
      that.selectComponent(id).onload();
    },
    hide() {
      let that = this;
      that.setData({
        show: false
      })
    },
    // 头部切换
    btn_tab(e) {
      let that = this;
      let index = e.currentTarget.dataset.index;
      that.setData({
        foodbox_index: index
      })
    },
    swiperChange(e){
      let that = this;
      let source = e.detail.source;
      if (source == 'touch') {
        that.setData({
          foodbox_index: e.detail.current,
        })
      }
      let foodbox_index = e.detail.current;
      let id = "#text" + foodbox_index;
      that.selectComponent(id).onload();
    },
  }
})
