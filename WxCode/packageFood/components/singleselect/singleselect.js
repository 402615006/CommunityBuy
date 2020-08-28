// packageFood/components/singleselect/singleselect.js
import { get_Method } from '../../utils/server.js'

Component({
  /**
   * 组件的初始数据
   */
  data: {
    animationData:'',
    show:false,
    methodlist:[],
    object:'',
    seasoningindex:-1
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 加入购物车
    btn_add(){
      let that=this;
      let list = that.data.object;
      let methodlist = that.data.methodlist;
      let seasoningindex=this.data.seasoningindex;
      if(seasoningindex<0)
      {
        wx.showToast({
          title: '请选择规格',
          icon: 'none',
          duration: 2000
        })
        return;
      }
      list.method=methodlist[seasoningindex].Name;
      list.cookmoney=methodlist[seasoningindex].Money;
      list.cookname=methodlist[seasoningindex].Name;
      let data = {
        'disname': list.DisName,
        'discode': list.DisCode,
        'disnum': 1, //数量
        'typecode': list.TypeCode,    //菜品类别
        'pkkcode': list.pdistypecode,   //一级类别编号
        'price': list.Price,           //菜品单价
        'method': list.method,       //选规格
        'cookmoney':list.cookmoney,
        'images':list.images,
        'cookname':list.cookname
      };
      that.hidelogin();
      that.triggerEvent('detailaddMethod', data);
    },
    //做法选择
    btnseasoning(e){
      let that=this;
      let index=e.currentTarget.dataset.index;
      that.setData({
        seasoningindex:index
      })
    },
    get_data(e){
      let that=this;
      that.setData({object:e});
      let data = {
        "actionname": "getmethodlist",
        "parameters": {
          'key': '',
          'discode': e.DisCode
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_Method(data).then(res=>{
        if(res.code==1){
          let methodlist = res.data;
          methodlist.map((item,index)=>{
            if (item.Money>0){
              item.Money = Number(item.Money);
            } 
          });
          that.setData({methodlist: methodlist});
        }
        that.showlogin();
      })
    },
    // 点击图片放大
    previewImg(e) {
      console.log(e);
      let item = e.currentTarget.dataset.item;
      if (item.images) {
        let arry = [];
        arry.push(item.images);
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
      }.bind(this), 200);
      // that.get_data();
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
      }.bind(this), 200);
      that.setData({
        methodlist:[],
        flavorlist:[],
        object:''
      })
    }
  }
})
