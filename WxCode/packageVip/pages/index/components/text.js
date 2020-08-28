// packageVip/pages/index/components/text.js
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
    list: [
      {
        price: '20',
        name: '抵扣券',
        stocode: '胡桃里餐厅满减优惠券',
        num: '500',
        numprice: '199'
      },
      {
        price: '20',
        name: '抵扣券',
        stocode: '胡桃里餐厅满减优惠券',
        num: '500',
        numprice: '199'
      },
      {
        price: '20',
        name: '抵扣券',
        stocode: '胡桃里餐厅满减优惠券',
        num: '500',
        numprice: '199'
      },
      {
        price: '20',
        name: '抵扣券',
        stocode: '胡桃里餐厅满减优惠券',
        num: '500',
        numprice: '199'
      },
      {
        price: '20',
        name: '抵扣券',
        stocode: '胡桃里餐厅满减优惠券',
        num: '500',
        numprice: '199'
      }
    ]
  },

  /**
   * 组件的方法列表
   */
  methods: {
    //优惠券详情
    go_detail() {
      wx.navigateTo({
        url: '../couponexchange/couponexchange',
      })
    },
  }
})
