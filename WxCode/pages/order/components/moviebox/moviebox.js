// pages/order/components/foodbox/foodbox.js
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
    moviebox_index: 0,
    moviebox_text: ['全部', '待付款', '待使用']
  },
  /**
   * 组件的方法列表
   */
  methods: {
    onhide(){
      let that=this;
      if(that.data.show){
        that.selectComponent('#text0').onhide();
        that.selectComponent('#text1').onhide();
        that.selectComponent('#text2').onhide();
      }
    },
    show() {
      let that = this;
      that.setData({
        show: true
      })
      let moviebox_index = that.data.moviebox_index;
      let id = "#text" + moviebox_index;
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
        moviebox_index: index
      })
    },
    // 去分类电影
    tofenlei() {
      wx.navigateTo({
        url: '/packageMovie/pages/index/index',
      })
    },
    // swiper滑动事件
    swiperChange(e) {
      let that = this;
      let source = e.detail.source;
      if (source == 'touch') {
        that.setData({
          moviebox_index: e.detail.current,
        })
      }
      let moviebox_index = e.detail.current;
      let id = "#text" + moviebox_index;
      that.selectComponent(id).onload();
    },
  }
})
