// packageFood/components/yypopup/yypopup.js

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
    animationData: '',
    list: [
      '50', '100', '200', '300'
    ],
    list_index: 0
  },
  /**
   * 组件的方法列表
   */
  methods: {
    // 防止事件穿透
    preventTouchMove() {

    },
    // 保存
    btn() {
      let that = this;
      let money = that.data.list[that.data.list_index];
      that.hidelogin();
      that.triggerEvent('save', money);
    },
    // 人数选择点击
    btnnumber(e) {
      let that = this;
      let index = e.currentTarget.dataset.index;
      that.setData({
        list_index: index
      })
    },
    //取消遮罩层
    center() {
      let that = this;
      that.hidelogin();
    },
    showlogin() {
      // 创建一个动画实例
      var animation = wx.createAnimation({
        // 动画持续时间
        duration: 200,
        // 定义动画效果，当前是匀速
        timingFunction: "linear",
        delay: 0
      })
      // 将该变量赋值给当前动画
      this.animation = animation
      // 先在y轴偏移，然后用step()完成一个动画
      animation.translateY(600).step()
      // 用setData改变当前动画
      this.setData({
        // 通过export()方法导出数据
        animationData: animation.export(),
        // 改变view里面的Wx：if
        show: true
      })
      // 设置setTimeout来改变y轴偏移量，实现有感觉的滑动
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          animationData: animation.export()
        })
      }.bind(this), 200)
    },
    // 隐藏遮罩层
    hidelogin() {
      var animation = wx.createAnimation({
        duration: 200,
        timingFunction: "linear",
        delay: 0
      })
      this.animation = animation
      animation.translateY(600).step()
      this.setData({
        animationData: animation.export(),
      })
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          show: false,
          animationData: animation.export(),
        })
      }.bind(this), 200)
    },
  }
})
