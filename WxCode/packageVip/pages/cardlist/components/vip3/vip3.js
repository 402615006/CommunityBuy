// packageVip/pages/cardlist/components/vip3/vip3.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    tabindex: {
      type: Number,
      value: ''
    },
    timescard:{
      type:JSON,
      value:[]
    },
    onLoad: {
      type: Boolean,
      value: false
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
      list:[
        {
          'name':'西班牙次卡',
          'number':'6',
          'price':'50'
        },
        {
          'name':'西班牙次卡',
          'number':'6',
          'price':'50'
        },
        {
          'name':'西班牙次卡',
          'number':'6',
          'price':'50'
        },
        {
          'name':'西班牙次卡',
          'number':'6',
          'price':'50'
        }
      ]
  },

  /**
   * 组件的方法列表
   */
  methods: {
    go_opentimscard(e){
      let cardcode = e.currentTarget.dataset.cardcode;
      wx.navigateTo({
        url: '../opentimscard/opentimscard?cardcode=' + cardcode,
      })
    }
  }
})
