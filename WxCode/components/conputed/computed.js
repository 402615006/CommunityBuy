// components/conputed/computed.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    num: Number,
    maxNum: Number
  },

  /**
   * 组件的初始数据
   */
 
  data: {
    
 
  },
  /**
   * 组件的方法列表
   */
  methods:{
    //减号
    prev(){
      console.log("减")
      let that=this;
      if(that.data.num>0){
        that.data.num-=1;
        that.setData({
          num:that.data.num
        })
        this.triggerEvent('myevent', that.data.num);
        this.triggerEvent('numChange', "prev");
      }
    },
    // 加号
    add(){
      console.log("加")
      let that=this;
      if (that.data.num < that.data.maxNum){
        that.data.num += 1;
        that.setData({
          num: that.data.num
        })

        this.triggerEvent('myevent', that.data.num);
        this.triggerEvent('numChange', "add");

      }else{
        wx.showToast({
          title: "没有更多了！！！",
          icon: 'none',
          duration: 2000
        })
      } 
    }
  }
})
