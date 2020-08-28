// packageFood/pages/stocode/components/toplist.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    toplist: {
      type: JSON,
      value: []
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 进入详情
    go_ToBuild(e){
      let that=this;
      let procode = e.currentTarget.dataset.code;
      wx.navigateTo({
        url: '/pages/memDetail/memDetail?procode=' + procode + '&type=2',
      })
    }
  }
})
