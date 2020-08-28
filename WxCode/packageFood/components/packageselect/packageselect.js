// packageFood/components/singleselect/singleselect.js
import { get_Pacage } from '../../utils/server.js'

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    buscode: {
      type: String,
      value: ''
    },
    stocode: {
      type: String,
      value: ''
    }
  },
  
  /**
   * 组件的初始数据
   */
  data: {
    animationData: '',
    show: false,
    packlist:[],
    object:''
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 点击加入购物车
    detailaddpack(){
      let that=this;
      let list = that.data.object;
      let packlist = that.data.packlist;
      console.log(packlist);
      let child=[];
      let flag=false;
      packlist.map((item,index)=>{
        item.dish.map((ctim,idx)=>{
          if (ctim.nums&&ctim.nums>0){
            let Method=[];
            if (ctim.IsMethod == 1 && ctim.Method){
              flag=true;
              Method = JSON.parse(ctim.Method);
            }
            child.push({
              'disname': ctim.DisName,
              'discode': ctim.DisCode,
              'disnum': ctim.nums,      //数量
              'ispackage': '2',
              'pdishcode': list.DisCode,     //套餐单品的父级discode
              'favor': '',       //口味
              'itemnum': '',       //条只数量
              'itemprice': 0,     //条只价格
              'cookname': '',      //做法名称
              'cookmoney': 0,      //做法加价
              'uptype': 1,         //上菜类型
              'remark': '',       //备注
              'discase': 1,       //菜品方案
              'kitcode': ctim.KitCode,   //厨房编号
              'typecode': ctim.TypeCode,    //菜品类别
              'pkkcode': '',   //父级类别编号
              'price': ctim.Price,           //菜品单价
              'SecPrice': 0,
              'IsSecPrice': '0',
              'IsMethod': ctim.IsMethod,      //是否做法必选
              'Method': Method,      //做法
              'Methodidx':'0'       //做法索引
            })
          }
        })
      })
      let data = {
        'disname': list.DisName,
        'discode': list.DisCode,
        'disnum': 1,      //数量
        'ispackage': list.IsCombo,
        'pdishcode': '',
        'favor': '',       //口味
        'itemnum': '',       //条只数量
        'itemprice': 0,     //条只价格
        'cookname': '',      //做法名称
        'cookmoney': 0,      //做法加价
        'uptype': 1,         //上菜类型
        'remark': '',       //备注
        'discase': 1,       //菜品方案
        'kitcode': list.CookerCode,   //厨房编号
        'typecode': list.TypeCode,    //菜品类别
        'pkkcode': list.pkkcode,   //一级类别编号
        'TypeCode': list.TypeCode,   //二级类别编号
        'price': list.Price ,          //菜品单价
        'SecPrice': list.SecPrice,
        'IsSecPrice': list.IsSecPrice,
        'Tag': list.Tag,
        'child': child
      };

      console.log(data);
      if (flag==true){
        // 点击加入购物车时判断套餐中的所有菜品是否有需要选择做法的，如果有则弹出选择做法
        that.selectComponent("#methodbox").show(data);
        return
      }
      that.hidelogin();
      that.triggerEvent('detailaddpack', data);

    },
    // 做法选择后子组件保存事件传递过来
    save(e){
      let that=this;
      let list=e.detail;
      that.hidelogin();
      that.triggerEvent('detailaddpack', list);
    },
    // 套餐选择
    btn_add(e){
      console.log(e);
      let that=this;
      let index=e.currentTarget.dataset.index;
      let idx=e.currentTarget.dataset.idx;
      let packlist = that.data.packlist;
      let arrey = packlist[index].dish[idx];
      if (packlist[index].CombinationType==0){
        let kindNum=0;
        let TNum=0;
        packlist[index].dish.map((item,index)=>{
          if (item.nums&&item.nums>0){
            kindNum = Number(kindNum)+1;
            TNum = Number(TNum)+Number(item.nums);
          }
        })
        console.log(TNum)
        if (Number(TNum) + 1 > Number(packlist[index].TotalOptNum)){
          wx.showToast({
            title: '最多可选' + packlist[index].TotalOptNum + '件',
            icon: 'none',
            duration: 1500,
            mask: true
          })
          return
        }
        if (arrey.nums>0){
          arrey.nums = Number(arrey.nums) + 1;
        }else{
          if (Number(kindNum) + 1 > Number(packlist[index].MaxOptNum)){
            wx.showToast({
              title: '最多可选' + packlist[index].MaxOptNum + '种',
              icon: 'none',
              duration: 1500,
              mask: true
            })
            return
          }else{
            arrey.nums = 1;
          }
        }
      } else if (packlist[index].CombinationType == 1){
        let price=0;
        packlist[index].dish.map((item,index)=>{
          if (item.nums && item.nums>0){
            price = Number(price) + Number(item.Price) * Number(item.nums);
          }
        })
        console.log(price);
        console.log(packlist[index].TotalOptMoney)
        if (Number(price) + Number(arrey.Price) > Number(packlist[index].TotalOptMoney)){
          wx.showToast({
            title: '最多可选' + packlist[index].TotalOptMoney + '元',
            icon: 'none',
            duration: 1500,
            mask: true
          })
          return
        }
        if (arrey.nums) {
          arrey.nums = Number(arrey.nums) + 1;
        } else {
          arrey.nums = 1;
        }
      }
      that.setData({
        packlist: packlist
      })
    },
    //点击x
    cen(e){
      let that = this;
      let index = e.currentTarget.dataset.index;
      let idx = e.currentTarget.dataset.idx;
      let packlist = that.data.packlist;
      let arrey = packlist[index].dish[idx];
      arrey.nums=0;
      that.setData({
        packlist: packlist
      })
    },
    // 获取数据
    get_data(e){
      let that = this;
      console.log(e);
      let data = {
        "actionname": "i_getdispacage",
        "parameters": {
          'key': '',
          'buscode': that.data.buscode, //商户编号
          'stocode': that.data.stocode, //门店编号
          'discode': e.DisCode
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_Pacage(data).then(res=>{
        console.log(e);
        if(res.code==0){
          let packlist=res.data;
          packlist.map((item,index)=>{
            if (item.dish){
              item.dish = JSON.parse(item.dish);
            }else{
              item.dish=[];
            }
            item.dish.map((ctim,idx)=>{
              ctim.nums = Number(ctim.DefaultNum);
              // if (item.GroupName =="必选菜品"){
              //   ctim.nums = Number(ctim.DefaultNum);
                
              // }else{
              //   ctim.nums=0
              // }
            })
          })
          that.setData({
            packlist: packlist,
            object: e
          })
          console.log(packlist);
          that.showlogin();
        }else{
          wx.showToast({
            title: res.msg,
            icon:'none',
            duration:1500,
            mask:true
          })
        }
      })
    },
    // 点击图片放大
    previewImg(e){
      console.log(e);
      let item = e.currentTarget.dataset.item;
      if (item.image) {
        let arry = [];
        arry.push(item.image);
        wx.previewImage({
          urls: arry,
        })
      }
    },
    showlogin() {
      let that=this;
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
      // that.get_data(e)
    },
    // 隐藏遮罩层
    hidelogin() {
      let that=this;
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
      that.setData({
        packlist: [],
        object: ''
      })
    }
  }
})
