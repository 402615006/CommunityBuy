// packageFood/components/methodbox/methodbox.js
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
    show:false,
    list:'',
    addPrice:0
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 防止滑动穿透
    preventTouchMove(){
      return
    },
    show(list){
      console.log(list);
      let that=this;
      list.child.map((ctim, idx) => {
        ctim.Method.map((item,index)=>{
          if (item.RaiseType == 1) {
            item.addprice = Number(item.RaiseAmount);
          } else if (item.RaiseType == 0) {
            item.addprice = Number(item.RaiseAmount / 100 * ctim.price).toFixed(2);
          }
        })
      })
      console.log(list);
      that.setData({
        show:true,
        list: list
      })
      that.get_addprice();
    },
    cancel(){
      let that=this;
      that.setData({
        show:false
      })
    },
    // 选择做法索引
    btn_index(e){
      console.log(e);
      let that=this;
      let idx = e.currentTarget.dataset.idx;
      let discode = e.currentTarget.dataset.item.discode;
      let list=that.data.list;
      list.child.map((item,index)=>{
        if (discode = item.discode){
          item.Methodidx=idx;
        }
      })
      that.setData({
        list:list
      })
      that.get_addprice();
    },
    // 点击确定
    btn_ok(){
      let that=this;
      let list=that.data.list;
      that.setData({
        show:false
      })
      that.triggerEvent('save', list);
    },
    get_addprice(){
      let that=this;
      let list = that.data.list;
      let addPrice=0;
      list.child.map((item,index)=>{
        if (item.IsMethod == 1 && item.Method.length>0){
          let p = item.disnum * item.Method[item.Methodidx].addprice;
          item.cookname = item.Method[item.Methodidx].MetName;
          item.cookmoney=p;
          addPrice = addPrice+Number(p);
        }
      })
      list.cookmoney = addPrice;
      that.setData({
        addPrice: addPrice,
        list:list
      })
    }
  }
})
