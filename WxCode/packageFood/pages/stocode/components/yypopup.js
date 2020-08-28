import { calcWeek, get_date } from '../../../../utils/util.js';
import { add_yuyue, get_rtime } from '../../../utils/server.js';

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    stocode: {
      type: String,
      value: ''
    },
    buscode: {
      type: String,
      value: ''
    },
    remacklist:{
      type:JSON,
      value:[]
    },
    stoname:{
      type:String,
      value:''
    }

  },

  /**
   * 组件的初始数据
   */
  data: {
    show: false,
    modshow:false,
    animationData: '',
    numberlist:[
      '1','2','3','4','5'
    ],
    numindex:0,
    remakeindex:0,
    // remacklist:['靠窗','儿童座椅','无烟环境'],
    date:'2019-07-27',
    stardate:'2019-07-25',
    enddate:'2030-12-12',
    day:'',
    weekday:'',

    time:'01:01',
    name:'',
    tel:'',
    timsindex:0,
    timsdata:[],
    timsarry:[]
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 姓名输入
    input_name(e){
      let that=this;
      let name=e.detail.value;
      that.setData({
        name: name
      })
    },
    input_tel(e){
      let that = this;
      let tel = e.detail.value;
      that.setData({
        tel: tel
      })
    },
    preventTouchMove(){

    },
    // 取消弹框
    cenmodclick(){
      let that = this;
      that.setData({
        modshow: false
      })
    },
    // 去预约记录
    go_yyorder(){
      let that=this;
      wx.navigateTo({
        url: '../yyorder/yyorder',
      })
      that.setData({
        modshow: false
      })
      that.hidelogin();
    },
    // 防止穿透
    no(){

    },
    // 点击预约按钮
    yuyueclick(){
      let that=this;
      let name=that.data.name;
      let tel=that.data.tel;
      let reg = /^1[0|1|2|3|4|5|6|7|8|9][0-9]{9}$/;
      if (!name){
        wx.showToast({
          title: '请输入姓名',
          icon:'none',
          mask:true,
          duration:1500
        })
        return
      }
      if (!tel){
        wx.showToast({
          title: '请输入电话',
          icon:'none',
          mask:true,
          duration:1500
        })
        return
      }
      if (!reg.test(tel)){
        wx.showToast({
          title: '手机号格式不正确',
          icon:'none',
          duration:1500
        })
        return
      }
      that.add_yuyue();
    },
    // 预约数据提交
    add_yuyue(){
      let that=this;
      let mobile = wx.getStorageSync('mobile');
      let numberlist = that.data.numberlist;
      let numindex = that.data.numindex
      let cusNum = numberlist[numindex];
      let remark='';
      if (that.data.remacklist.length>0){
        remark = that.data.remacklist[that.data.remakeindex].Remark;
      }
      let timsindex = that.data.timsindex;
      let timsarry = that.data.timsarry;
      if (timsarry.length==0){
        wx.showToast({
          title: '没有可预订时间,暂时无法预订',
          icon:'none',
          mask:true,
          duration:1500
        })
        return
      }
      let dinnerTime = that.data.date + ' ' + timsarry[timsindex];
      dinnerTime=dinnerTime.replace(/-/g, "/");
      if (new Date(dinnerTime).getTime() <= new Date().getTime()){
        wx.showToast({
          title: '预约时间应大于当前时间',
          icon: 'none',
          mask: true,
          duration: 1500
        })
        return
      }
      let data = {
        "actionname": "add",
        "parameters": {
          'GUID': '',
          'USER_ID': '',
          'BusCode': that.data.buscode,
          'StoCode': that.data.stocode,
          'DinnerTime': dinnerTime,
          'CusNum': cusNum, //用餐人数
          'CusPhone': that.data.tel,  //手机号码
          'CusName': that.data.name, //预定人姓名
          'wxPhoen': mobile,
          'Remark': remark   //备注
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      add_yuyue(data).then(res=>{
        if(res.code==0){
          that.setData({
            modshow: true
          })
        } else if (res.code==9){
          wx.showModal({
            title: '提示',
            content: '您已有预约,无法重复预约,去查看预约记录？',
            success(res) {
              if (res.confirm) {
                wx.navigateTo({
                  url: '/packageFood/pages/yyorder/yyorder'
                })
              }
            }
          })
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
    // day改变
    change_day(e){
      let that=this;
      let date=e.detail.value;
      console.log(date)
      let day = get_date(date);
      let weekday = calcWeek(date);
      that.setData({
        date: date,
        day: day,
        weekday: weekday
      })
    },  
    //时间改变
    change_time(e){
      // let that=this;
      // let time=e.detail.value;
      // that.setData({
      //   time:time
      // })
      let value = e.detail.value;
      let that = this;
      that.setData({
        timsindex: value,
      })
    },
    // 人数选择点击
    btnnumber(e){
      let that=this;
      let index=e.currentTarget.dataset.index;
      that.setData({
        numindex:index
      })
    },
    //备注选择
    btnremake(e){
      let that = this;
      let index = e.currentTarget.dataset.index;
      that.setData({
        remakeindex: index
      })
    },
    //取消遮罩层
    center(){
      let that=this;
      that.hidelogin();
    },
    // 获取预约时间
    get_tims(){
      let that=this;
      let data = {
        "actionname": "getrtime",
        "parameters": {
          'GUID': '',
          'USER_ID': '',
          'stocode': that.data.stocode
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_rtime(data).then(res=>{
        console.log(res);
        if(res.code==0){
          let list=res.data;
          let timsarry=[];
          list.map((item,index)=>{
            timsarry.push(item.MealTime)
          })
          that.setData({
            timsdata:list,
            timsarry: timsarry
          })
        }else{
          wx.showToast({
            title: res.msg,
            icon:none,
            duration:1500,
            mask:true
          })
        }
      })
    },
    showlogin() {
      let mobile = wx.getStorageSync('mobile');
      if (mobile){
        this.setData({
          tel:mobile
        })
      }
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
      this.get_tims();
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
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          show:false,
          animationData: animation.export(),
        })
      }.bind(this), 200)
    },
  },
  //组件初次加载
  attached: function () {
    let that=this;
    let date = new Date();
    let Y = date.getFullYear() + '-';
    let M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
    let D = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
    let h = date.getHours();
    h = h < 10 ? ('0' + h) : h;
    let minute = date.getMinutes();
    let second = date.getSeconds();
    minute = minute < 10 ? ('0' + minute) : minute;
    second = second < 10 ? ('0' + second) : second;

    let stardate = Y + M + D;
    let time = h + ':' + minute;
    let day=get_date(stardate);
    let weekday = calcWeek(stardate);
    that.setData({
      time:time,
      stardate: stardate,
      date: stardate,
      day: day,
      weekday: weekday
    })
  },
  detached: function () {
    console.log("组件被销毁")
  },
})
