// packageFood/components/footer/footer.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    price: String,
    list: Array,
    isbtn:Boolean,

    shopcardlist:{
      type:Array,
      value:[],
      observer: function (newVal, oldVal){
        this.get_number()
      } 
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    animationData: '',
    show: false,
    dish_num:0,
    number:0,
    Tprice:0,

    classshow:false
  },

  /**
   * 组件的方法列表
   */
  methods: {
    get_number(){
      let that=this;
      let number=0;
      let Tprice=0;
      let shopcardlist = that.data.shopcardlist;
      shopcardlist.map((item,index)=>{
        number = Number(number) + Number(item.disnum);
        Tprice = Number(Tprice) + Number(item.price * item.disnum) + Number(item.disnum * item.cookmoney)
      })
      that.setData({
        number: number,
        Tprice: Tprice.toFixed(2)
      })
      if (shopcardlist.length==0){
        that.setData({
          show:false
        })
      }
    },
    // 立即下单
    // topay(){
    //   var that = this;
    //   that.triggerEvent('topay');
    // },
    addorder(){
      var that = this;
      that.hidelogin();
      that.triggerEvent('addorder');
    },
    // 点击购物车列表里的减号
    shopcardprev(e){
      let that=this;
      let index=e.currentTarget.dataset.index;
      this.triggerEvent('shopcardprev', index);
    },
    // 点击购物车列表里的加号
    shopcardadd(e){
      let that=this;
      let index=e.currentTarget.dataset.index;
      this.triggerEvent('shopcardadd', index);
    },
    // 清空购物车
    shopcordclear(){
      var that = this;
      that.triggerEvent('shopcordclear');
    },
    //点击购物车图标
    showbox() {
      let that = this;
      let show = that.data.show;

      that.triggerEvent('hideCards');
      if (that.data.shopcardlist.length==0){
        wx.showToast({
          title: '您的购物车空空如也~',
          icon:'none',
          duration:1500,
          mask:true
        })
        return
      }
      if (show) {
        that.hidelogin();
      } else {
        that.showlogin();
      }
    },
    hide_box(){
      let that=this;
      that.hidelogin();
    },
    showlogin(){
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
      setTimeout(function() {
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
      setTimeout(function() {
        animation.translateY(0).step()
        this.setData({
          show: false,
          animationData: animation.export(),
        })
      }.bind(this), 200)
    },

    // 立即下单
    btn(){
      wx.navigateTo({
        url: '../submitorder/submitorder',
      })
    }
  }
})