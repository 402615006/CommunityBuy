// packageFood/components/add_prev/add_prev.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    leftlist_index:{
      type:Number,
      value:'',
      observer: function (newVal, oldVal) {
        // this.get_number()
      }
    },
    right:{
      type:Number,
      value:''
    },
    bottom:{
      type:Number,
      vlaue:''
    },
    num:{
      type:Number,
      value:0
    },
    item:{
      type:Object,
      value:''
    },
    shopcardlist:{
      type: Array,
      value: [],
      observer: function (newVal, oldVal) {
        this.get_number()
      }
    }
    // maxNum: Number
  },

  /**
   * 组件的初始数据
   */
  data: {
    nums:0
  },

  /**
   * 组件的方法列表
   */
  methods: {
    get_number(){
      let that=this;
      let nums=0;
      let shopcardlist = that.data.shopcardlist;
      let object = that.data.item;
      shopcardlist.map((item,index)=>{
        if (object.DisCode==item.discode){
          nums = Number(nums) + Number(item.disnum)
        }
      })
      that.setData({
        nums: nums
      })
    },
    //减号
    prev() {
      let that = this;
      if (that.data.nums>0){
        this.triggerEvent('prev',that.data.item);
      }
    },
    // 加号
    add() {
      // console.log("加")
      let that = this;
      // if (that.data.num < that.data.maxNum) {
  
        this.triggerEvent('add',that.data.item);
      // } else {
      //   wx.showToast({
      //     title: "没有更多了！！！",
      //     icon: 'none',
      //     mask:true,
      //     duration: 1500
      //   })
      // }
    },
    //添加做法
    addMethod(){
      let that=this;
      this.triggerEvent('addMethod', that.data.item);
    }
    //添加套餐
    // addCombo(){
    //   let that=this;
    //   this.triggerEvent('addCombo', that.data.item);
    // }
  }
})
