// packageVip/pages/cardlist/components/vip1/vip1.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    tabindex:{
      type: Number,
      value: ''
    },
    pluscard:{
      type:JSON,
      value:[]
    },
    onLoad:{
      type:Boolean,
      value: false
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    list:[
      {
        name:'胡桃里餐厅会员卡'
      },
      {
        name:'胡桃里餐厅会员卡'
      },
      {
        name:'胡桃里餐厅会员卡'
      },
      {
        name:'胡桃里餐厅会员卡'
      },
      {
        name:'胡桃里餐厅会员卡'
      }
    ]
  },

  /**
   * 组件的方法列表
   */
  methods: {
    go_opencard(e){
      let cardcode = e.currentTarget.dataset.cardcode;
      wx.navigateTo({
        url: '../openpluscard/openpluscard?cardcode='+cardcode,
      })
    }
  }
})
