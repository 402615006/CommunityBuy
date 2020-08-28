// components/red/red.js
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
    showlogin:false,
    animationlogin:"",
    couponname:''
  },

  /**
   * 组件的方法列表
   */
  methods: {
    showlogin: function () {
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
        animationlogin: animation.export(),
        // 改变view里面的Wx：if
        showlogin: true
      })
      // 设置setTimeout来改变y轴偏移量，实现有感觉的滑动
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          animationlogin: animation.export()
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
        animationlogin: animation.export(),
      })
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          animationlogin: animation.export(),
          showlogin: false
        })
      }.bind(this), 200)
    },
    log(e){
      let couponname=e;
      this.setData({
        couponname: couponname
      })
      this.showlogin();
    }
  }
})
