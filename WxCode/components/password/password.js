// components/password/password.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    falg:{
      type:String,
      value:'1'
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    animationData: "",
    showModalStatus: false,
    // showlogin:true,
    items: [0, 1, 2, 3, 4, 5],
    keys: [1, 2, 3, 4, 5, 6, 7, 8, 9, 'y', 0],
    password:[]
  },

  /**
   * 组件的方法列表
   */
  methods: {
    preventTouchMove(){

    },
    //显示对话框
    showModal: function () {
      // 显示遮罩层
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
        showModalStatus: true
      })
      // 设置setTimeout来改变y轴偏移量，实现有感觉的滑动
      setTimeout(function () {
        animation.translateY(0).step();
        this.setData({
          animationData: animation.export()
        })
      }.bind(this), 200)
    },
    //隐藏对话框
    hideModal: function () {
      let that=this;
      // 隐藏遮罩层
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
          animationData: animation.export(),
          showModalStatus: false
        })
      }.bind(this), 200);
      that.triggerEvent('hiden');
    },
    //点击取消按钮
    backHandle() {
      this.clearPasswordHandle()  // 返回时清除password
      this.hideModal();
    },
    //清除密码
    clearPasswordHandle: function () {
      this.setData({
        password:[]
      })
    },
    //点击数字输密码
    keyUpHandle(e) {
      let that=this;
      let text=e.currentTarget.dataset.id;
      let password=that.data.password;
      // if (!text&&text!=0 || password.length >= 6) return;
      if (text == 'y') {
        return
      } else if (password.length >= 6) {
        return
      }
      password.push(text);
      that.setData({
        password: password
      })
      this.ajaxData()
    },
    //删除数字
    delHandle() {
      let that=this;
      let password = that.data.password;
      if (password.length <= 0) return false;
      password.pop();
      that.setData({
        password:password
      })
    },
    btn_password(){
      let that=this;
      that.hideModal();
      wx.navigateTo({
        url: '/packageVip/pages/safety/safety',
      })
    },
    //添加数字
    ajaxData() {
      let that=this;
      let password=that.data.password;
      if (password.length >= 6) {
        let number = (password.join(' ').replace(/\s/g, ''))
        that.triggerEvent('myevent', number);
        this.clearPasswordHandle();
        that.hideModal();
      }
      return false
    },
  }
})
