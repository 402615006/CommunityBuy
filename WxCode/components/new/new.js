// components/new/new.js
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
    showlogin: false,
    animationlogin: "",
    img:'',
    result:''
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
      console.log(e);
      let that=this;
      let result=e;
      that.setData({
        result:result
      })
      that.showlogin();
    },
    go(){
      console.log("跳转");
      let that=this;
      let id=that.data.result.actid;
      let acttype = that.data.result.acttype;
      console.log(acttype)
      if (acttype==1){
        wx.navigateTo({
          url: '../activity_detail/activity_detail?acttype=' + acttype + '&id=' + id
        })
      } else if (acttype == 2){
        wx.navigateTo({
          url: '../activity_detail2/activity_detail2?acttype=' + acttype + '&id=' + id
        })
      } else if (acttype == 3){
        wx.navigateTo({
          // url: '../activity_detail3/activity_detail3?memcode_id=M271931'
          url: '../activity_detail3/activity_detail3?id=' + id
        })
      } else if (acttype == 4){
        wx.navigateTo({
          url: '../activity_detail4/activity_detail4?acttype=' + acttype + '&id=' + id
        })
      } else if (acttype == 7){
        wx.navigateTo({
          url: '../lottery/lottery?acttype=' + acttype + '&actid=' + id
        })
      }
      that.cen();
    },
    cen(){
      this.hidelogin();
      this.triggerEvent('newcen');
    }
  }
})
