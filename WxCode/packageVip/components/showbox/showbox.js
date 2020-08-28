// packageVip/components/showbox/showbox.js
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
    isshow:false
  },

  /**
   * 组件的方法列表
   */
  methods: {
    show(){
      let that=this;
      that.setData({
        isshow:true
      })
    },
    hide(){
      let that=this;
      that.setData({
        isshow:false
      })
    },
    btn(){
      let that=this;
      wx.navigateTo({
        url: '/packageVip/pages/grxx/grxx',
      })
      that.setData({
        isshow:false
      })
    }
  }
})
