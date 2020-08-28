// pages/index/components/top_search.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    titleBarHeight:{
      type:Number,
      value:''
    },
    noreadno:{
      type:String,
      value:''
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    bar_Height: wx.getSystemInfoSync().statusBarHeight
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 去搜索
    go_search() {
      wx.navigateTo({
        url: '/pages/search/search',
      })
    },
    // 去我的消息
    go_news() {
      wx.navigateTo({
        url: '/packageOrganization/pages/News/News',
      })
    },
  }
})
