
import { verificationcardpwd } from '../../utils/server.js';

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    cardlist: {
      type: JSON,
      value: [],
      observer: function (newVal, oldVal) {
        console.log(newVal)
      }
    },
    money: {
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
    card_index: 0
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 点击确认付款
    go_ok() {
      let that = this;
      let cardlist = that.data.cardlist;
      let card_index = that.data.card_index;
      if (cardlist.length==0){
        wx.showToast({
          title: '没有支付方式选择,请确认！',
          icon: 'none',
          duration: 1500
        })
        return
      }
      if (that.data.money == '') {
        wx.showToast({
          title: '支付金额不正确请检查',
          icon: 'none',
          duration: 1500
        })
        return
      }
      if (!(Number(that.data.money) > 0)) {
        wx.showToast({
          title: '支付金额为0,请检查',
          icon: 'none',
          duration: 1500
        })
        return
      }
      if (cardlist[card_index].cardCode != 'wx' && cardlist[card_index].cardCode != 'yj') {
        let balance = cardlist[card_index].balance;
        if (!(Number(balance) >= Number(that.data.money))) {
          wx.showToast({
            title: '会员卡余额不足',
            icon: 'none',
            duration: 1500
          })
          return
        }
        if (cardlist[card_index].notpwd == 0) {
          // 没有免密支付直接调用密码弹窗
          that.hidelogin();
          that.selectComponent("#password").showModal();
          return
        } else {
          // 有免密额度判断免密额度大小
          let notpwdAmount = cardlist[card_index].notpwdAmount;
          if (Number(notpwdAmount) < Number(that.data.money)) {
            // 免密额度小于支付额度需要密码
            that.hidelogin();
            that.selectComponent("#password").showModal();
            return
          }
        }
      }
      if (cardlist[card_index].cardCode == 'yj') {
        let balance = cardlist[card_index].balance;
        if (!(Number(balance) >= Number(that.data.money))) {
          wx.showToast({
            title: '余额不足',
            icon: 'none',
            duration: 1500
          })
          return
        }
        that.selectComponent("#password").showModal();
        return
      }
      that.hidelogin();
      that.triggerEvent('pay_but', that.data.money);
    },
    // 密码输入完成判断是否正确
    get_number_ok(e) {
      let that = this;
      let pwd = e.detail;
      let unionid = wx.getStorageSync('unionid');
      let data = {
        "actionname": "pwdcheck",
        "parameters": {
          'GUID': '88888888',
          'unionid': unionid,
          'pwd': pwd
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      verificationcardpwd(data).then(res => {
        if (res.status == 0) {
          that.triggerEvent('pay_but', that.data.money);
        } else {
          wx.showToast({
            title: res.mes,
            icon: 'none',
            mask: true,
            duration: 1500
          })
        }
      })
    },
    // 选择卡
    btn_clickcard(e) {
      let that = this;
      let index = e.currentTarget.dataset.index;
      that.setData({
        card_index: index
      })

    },
    preventTouchMove(){
      
    },
    //显示卡列表
    showcard() {
      let that = this;
      that.showlogin();
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
