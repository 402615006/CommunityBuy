// packageVip/pages/cardlist/components/vip2/vip2.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    tabindex: {
      type: Number,
      value: ''
    },
    discard:{
      type:JSON,
      vlaue:[]
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
    sldata: [
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      '10',
      '11',
      '12',
      '13',
      '14',
      '15',
      '16',
      '17',
      '18',
      '19',
      '20',
    ],
    list:[
      {
        'name': '西班牙餐厅会员卡',
        'price1':'149',
        'price2':'100',
        'price3':'50',
      },
      {
        'name': '西班牙餐厅会员卡',
        'price1':'149',
        'price2':'100',
        'price3':'50',
      },
      {
        'name': '西班牙餐厅会员卡',
        'price1':'149',
        'price2':'100',
        'price3':'50',
      }
    ]
  },

  /**
   * 组件的方法列表
   */
  methods: {
    go_openvipcard(e){
      let cardcode = e.currentTarget.dataset.cardcode;
      wx.navigateTo({
        url: '../openvipcard/openvipcard?cardcode=' + cardcode,
      })
    }
  }
})
