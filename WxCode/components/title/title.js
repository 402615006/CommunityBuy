// components/title/title.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    background:{
      type:String,
      value:''
    },
    title: { // 设置标题
      type: String,
      value: ''
    },
    show_bol: { // 控制返回箭头是否显示
      type: Boolean,
      value: false
    },
    show_text: { // 弹窗文字描述
      type: String,
      value: ''
    },
    popUps: { // 控制返回箭头是否弹窗
      type: Boolean,
      value: false
    },
    my_class: { // 控制样式
      type: Boolean,
      value: false
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
    // 点击返回箭头
    goBack(){
      // 返回上一页
      let pages = getCurrentPages();
      if (pages.length>=2){
        wx.navigateBack()
      }else{
        wx.switchTab({
          url: '/pages/index/index',
        })
      }
    }
  }
})
