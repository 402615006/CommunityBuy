// components/badge/badge.js
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    number: {
      type: Number,
      value: 0
    },
    top:{
      type:Number,
      value:''
    },
    right: {
      type: Number,
      value: ''
    },
    PKCode:{
      type:String,
      value:'',
    },
    shopcardlist: {
      type: Array,
      value: [],
      observer: function (newVal, oldVal) {
        this.get_number()
      }
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    num:0
  },

  /**
   * 组件的方法列表
   */
  methods: {
    get_number(){
      let that=this;
      let num=0;
      let shopcardlist = that.data.shopcardlist;
      let PKCode = that.data.PKCode;
      shopcardlist.map((item,index)=>{
        if (item.typecode.indexOf(PKCode) >= 0){
          num = Number(num) + Number(item.disnum);
        }
      })
      that.setData({
        num: num
      })
    }
  }
})
