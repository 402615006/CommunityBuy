// packageFood/components/yypopup/yypopup.js
import { add_queue } from '../../../utils/server.js';
import { QueueId } from '../../../../utils/api.js';

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    stocode:{
      type: String,
      value: ''
    },
    buscode: {
      type: String,
      value: ''
    },
    numberlist: {
      type: JSON,
      value: ''
    },
  },

  /**
   * 组件的初始数据
   */
  data: {
    show: false,
    animationData: '',
    // numberlist: [
    //   '1-4人', '4-6人', '7-9人', '10人以上'
    // ],
    numindex: 0
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 禁止页面滑动
    preventTouchMove() {

    },
    // 点击取号
    btn_quhao(){
      let that=this;
      wx.requestSubscribeMessage({
        tmplIds: [QueueId],
        success(res) {
          that.click_quhao();
        }
      })
    },

    // 取号
    click_quhao(){
      let that=this;
      let mobile = wx.getStorageSync('mobile');
      let openid = wx.getStorageSync('openid');
      let index = that.data.numindex;
      let cusNum = that.data.numberlist[index].MaxNumber;

      let data = {
        "actionname": "add",
        "parameters": {
          'GUID': '',
          'USER_ID': '',
          'BusCode': that.data.buscode,
          'StoCode': that.data.stocode,
          'wxid': mobile,
          'CusNum': cusNum,
          'CCode': openid
        }
      }
      console.log(data)
      data.parameters = JSON.stringify(data.parameters);
      add_queue(data).then(res=>{
        if(res.code==0){
          that.hidelogin();
          wx.navigateTo({
            url: '../phdetail/phdetail?stocode=' + that.data.stocode,
          })
        }else if(res.code==5){
          that.hidelogin();
          wx.showModal({
            title: '提示',
            content: '您已存在排队,无法重复排队,去查看排队？',
            success(res) {
              if (res.confirm) {
                wx.navigateTo({
                  url: '../phdetail/phdetail?stocode=' + that.data.stocode
                })
              }else if (res.cancel) {
                
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
    // 人数选择点击
    btnnumber(e) {
      let that = this;
      let index = e.currentTarget.dataset.index;
      that.setData({
        numindex: index
      })
    },
    //取消遮罩层
    center() {
      let that = this;
      that.hidelogin();
    },
    showlogin() {
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
          show: false,
          animationData: animation.export(),
        })
      }.bind(this), 200)
    },
  }
})
