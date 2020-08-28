// packageFood/pages/groupdetail/groupdetail.js
import {
  getTJdetail,
  addorder,
  payorderbypcode,
  getmemcardslist,checkaddorder
} from '../../utils/server.js';
import {
  verificationcardpwd
} from '../../../utils/server.js';
import {
  baserURLcard
} from "../../../utils/api.js"; //域名引入
import {
  is_gologin,
  wechat_payment
} from '../../../utils/util.js';
Page({

  /**
   * 页面的初始数据
   */
  data: {
    pkcode: "", //产品编号
    num: 0, //默认数量
    // maxNum: Number.POSITIVE_INFINITY, //最大可选数
    dish_num: 0, //已选择数量
    selectList: [], //选择集合
    price: 0, //金额

    proinfo: '', //餐品信息
    disInfo: "", //菜品信息 
    dishescombo: [], //套餐标配（iscombo等于1时有数据）信息    
    dishesoptional: [], //套餐可选组合（iscombo等于1时有数据）
    bannerlist: [], //轮播图
    btnIsTrue: false, //按钮显示
    cardsList: [], //会员卡列表
    cardIndex: 0, //选中卡下标
    nums: 0, //选择数量
    animationData: '',
    show: false,
    shopCartselectList: [], //购物车选择
    stocode: "", //门店编号
    successData: false,
    defaultIndex:0,//默认会员卡下标
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    var that = this;
    var pkcode = options.pkcode; //产品编号
    that.setData({
      pkcode: pkcode,
      baserURLcard: baserURLcard, //请求地址
    })
    //产品详情
    that.getTJdetail();

  },
  //获取会员卡信息
  getCardList() {
    var that = this;
    var memcode = wx.getStorageSync("memcode");
    if (!memcode) {
      return
    }
    var data = {
      "actionname": "getmemcardslistvalid",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "memcode": memcode,
        "stocode": that.data.stocode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getmemcardslist(data).then(res => {
      if (res.status == 0) {
        var cardsList = res.data;
        var defaultIndex = 0;
        for (var i = 0; i < cardsList.length; i++) {
          if (cardsList[i].isdef == 1) {
            defaultIndex = i;
          }
        }
        var weixin = {
          "levelname": "微信",
          "cardCode": "wx",
        }
        cardsList.push(weixin)
        that.setData({
          cardsList: cardsList,
          defaultIndex: defaultIndex,
        })
      }
    })
  },
  //产品详情
  getTJdetail() {
    var that = this;
    var data = {
      "actionname": "getdetail",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "pkcode": that.data.pkcode,
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getTJdetail(data).then(res => {
      if (res.status == 0) {
        //proinfo:餐品信息
        var proinfo = res.proinfo[0];
        //disInfo: 菜品信息
        var disInfo = res.disInfo[0];
        //dishescombo:套餐标配（iscombo等于1时有数据）信息
        var dishescombo = res.dishescombo;
        //dishesoptional：套餐可选组合（iscombo等于1时有数据）
        var dishesoptional = res.dishesoptional;

        var bannerlist = [];
        var imglist = proinfo.proimgpath; //轮播图
        bannerlist = imglist.split(",");
        var stoname = proinfo.stoname;
        wx.setNavigationBarTitle({ //设置标题
          title: stoname
        })
        that.setData({
          proinfo: proinfo,
          bannerlist: bannerlist,
          disInfo: disInfo,
          dishescombo: dishescombo,
          dishesoptional: dishesoptional,
          stocode: proinfo.stocode,
          successData: true,
        })
        //获取会员卡信息
        that.getCardList();
      } else {
        wx.showToast({
          title: res.mes,
          mask: true,
          duration: 1500,
          icon: 'none'
        })
      }
    })
  },
  //下单
  addorder() {
    let that = this;
    if (!is_gologin()){
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    //弹框选择会员卡
    that.selectComponent("#CardsSelect").showlogin();
    that.hidelogin();
    that.setData({
      btnIsTrue: true,
    })
  },
  // 登陆成功后的事件
  onMyEvent() {
    var that = this;
    //获取会员卡信息
    that.getCardList();
  },
  //获取选中卡信息
  setCardcode(e) {
    var that = this;
    var cardIndex = e.detail;
    //获取卡下标
    that.setData({
      cardIndex: cardIndex
    })
  },

  //是否需要输入密码
  //会员卡notpwd== 1不需要输密码，notpwdAmount小于它时不需要输密码
  isInputPwd() {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var notpwdAmount = parseFloat(currentCard.notpwdAmount); //免密金额
    var Aprice = parseFloat(that.data.price);
    if (currentCard.cardCode != "wx") {
      that.Acheckaddorder(currentCard,Aprice,notpwdAmount);
    } else {
      //获取单号
      that.getOrder();
    }
  },

  Acheckaddorder(currentCard,Aprice,notpwdAmount){
    var that = this;
    var memcode = wx.getStorageSync("memcode");
    var selectList = that.data.shopCartselectList[0]; //已选菜品信息
    var proinfo = that.data.proinfo;
    var orderstr = proinfo.stocode + "," + selectList.discode + "," + selectList.disname + "," + selectList.num + "," + that.data.price + ";"
    var data = {
      "actionname": "checkaddorder",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "usercode": memcode,
        "orderstr": orderstr,
        "tpcode": that.data.pkcode,
      }
    }
    console.log(data);
    data.parameters = JSON.stringify(data.parameters);
    checkaddorder(data).then(res => {
      if (res.status == 0) {
        if (currentCard.notpwd == 1 || Aprice <= notpwdAmount) {
          //获取单号
          that.getOrder();
        } else {
          that.selectComponent("#password").showModal();
        }
      } else {
        wx.showToast({
          title: res.mes,
          mask: true,
          duration: 1500,
          icon: 'none'
        })
      }
    })
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
        //获取单号
        that.getOrder();
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

  //获取单号
  getOrder() {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var cardcode = currentCard.cardCode; //会员卡卡号
    var balance = currentCard.balance; //余额
    if (currentCard.cardCode != "wx") {
      if (parseFloat(balance) == 0) {
        wx.showToast({
          title: "卡内余额为0，请充值后再付！",
          mask: true,
          duration: 1500,
          icon: 'none'
        })
        return
      }
    }
    that.selectComponent("#CardsSelect").hidelogin(); //隐藏卡列表
    var memcode = wx.getStorageSync("memcode");
    var selectList = that.data.shopCartselectList[0]; //已选菜品信息
    var proinfo = that.data.proinfo;
    var orderstr = proinfo.stocode + "," + selectList.discode + "," + selectList.disname + "," + selectList.num + "," + that.data.price + ";"
    var data = {
      "actionname": "addorder",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "usercode": memcode,
        "orderstr": orderstr,
        "tpcode": that.data.pkcode,
        "pkcode": "",
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    addorder(data).then(res => {
      if (res.status == 0) {
        var mainorderno = res.data[0].mainorderno; //主订单号
        var orderno = res.data[0].orderno; //副订单号
        //支付
        that.payorderbypcode(mainorderno, orderno);
      } else {
        wx.showToast({
          title: res.mes,
          mask: true,
          duration: 1500,
          icon: 'none'
        })
      }
    })
  },
  //支付
  payorderbypcode(mainorderno, orderno) {
    var that = this;
    var cardsList = that.data.cardsList; //会员卡列表
    var cardIndex = that.data.cardIndex; //会员卡下标
    var currentCard = cardsList[cardIndex]; //当前选中会员卡
    var paytype = 2; //1微信支付 2会员卡支付
    var cardcode = currentCard.cardCode; //会员卡卡号
    var balance = currentCard.balance; //余额
    var price = that.data.price; //订单金额

    if (parseFloat(price) > parseFloat(balance)) {
      wx.showToast({
        title: "卡内余额不足，请充值后再付！",
        mask: true,
        duration: 1500,
        icon: 'none'
      })
      return
    }
    if (currentCard.cardCode == "wx") {
      paytype = 1;
      cardcode = "";
    }
    var memcode = wx.getStorageSync("memcode");
    var openid = wx.getStorageSync("openid");
    var data = {
      "actionname": "payorderbypcode",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        'openid': openid,
        "mainorderno": mainorderno,
        "orderno": orderno,
        "paytype": paytype,
        "paymoney": price,
        "cardcode": cardcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    payorderbypcode(data).then(res => {
      if (paytype == 1) { //微信支付     
        wechat_payment(res.timeStamp, res.nonceStr, res.package, res.signType, res.paySign).then(res => {
          wx.showToast({
            title: res.mes,
            mask: true,
            duration: 1500,
            icon: 'success'
          })
          setTimeout(function() {
            wx.navigateTo({
              url: '/pages/order/order?tabindex=' + 2,
            })
          }, 1000)
        }).catch(err => {
          wx.showToast({
            icon: "none",
            title: '支付失败',
            duration: 2000,
          })
          setTimeout(function () {
            wx.navigateTo({
              url: '/pages/order/order?tabindex=' + 2,
            })
          }, 1000)
        })
      } else { //会员卡支付
        if (res.status == 0) {
          wx.showToast({
            title: res.mes,
            mask: true,
            duration: 1500,
            icon: 'success'
          })
          setTimeout(function() {
            wx.navigateTo({
              url: '/pages/order/order?tabindex=' + 2,
            })
          }, 1000)
        } else {
          wx.showToast({
            title: res.mes,
            mask: true,
            duration: 1500,
            icon: 'none'
          })
        }

      }
    })
  },
  //点击阴影取消选择支付方式
  unclickCard() {
    var that = this;
    that.setData({
      btnIsTrue: false,
    })
  },

  //减号
  prev() {
    let that = this;
    if (that.data.nums > 0) {
      var num = that.data.nums;
      var price = that.data.proinfo.costprice;
      num--;
      that.setData({
        nums: num,
      })
      that.dndn(price, num);
    }
  },
  // 加号
  add() {
    let that = this;
    var num = that.data.nums;
    var price = that.data.proinfo.costprice;
    num++;
    that.setData({
      nums: num,
    })
    that.upup(price, num);
  },

  // 相加
  upup(price, num) {
    var that = this;
    if (num > 0) {
      that.setData({
        price: (Number(price) + Number(that.data.price)).toFixed(2),
      })
      that.selectDishes(that.data.price, num);
    }
  },
  // 相减
  dndn(price, num) {
    var that = this;
    that.setData({
      price: (Number(that.data.price) - Number(price)).toFixed(2),
    })
    if (num == 0) {
      that.shopcordclear();
    } else {
      that.selectDishes(that.data.price, num);
    }
  },

  //已选菜品
  selectDishes(price, num) {
    var that = this;
    var list = new Object();
    var selectList = []; //点击选择菜品
    list.discode = that.data.disInfo.discode;
    list.disname = that.data.disInfo.disname;
    list.price = price;
    list.num = num;
    selectList.push(list);
    that.setData({
      shopCartselectList: selectList,
    })
  },

  // 清空购物车
  shopcordclear() {
    var that = this;
    that.setData({
      shopCartselectList: [],
      nums: 0,
    })
    that.hidelogin();
  },

  //点击购物车图标
  showbox() {
    let that = this;
    let show = that.data.show;
    if (that.data.shopCartselectList.length == 0) {
      wx.showToast({
        title: '您的购物车空空如也~',
        icon: 'none',
        duration: 1500,
        mask: true
      })
      return
    }
    //取消选择支付方式
    that.unclickCard();
    //弹框选择会员卡
    that.selectComponent("#CardsSelect").hidelogin();
    if (show) {
      that.hidelogin();
    } else {
      that.showlogin();
    }

  },

  hide_box() {
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


  //底部组件内加减往页面传值
  // compontpass(e) {
  //   var that = this;
  //   that.setData({
  //     dish_num: e.detail
  //   })
  //   if (e.detail == 0) {
  //     that.setData({
  //       selectList: [],
  //     })
  //   }
  // },
  //底部组件内清空
  // clearList(e) {
  //   var that = this;
  //   that.setData({
  //     dish_num: 0,
  //     selectList: [],
  //   })
  // },

  // hideCards(){
  //   var that = this;
  //   //弹框选择会员卡
  //   that.selectComponent("#CardsSelect").hidelogin();
  // },
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function() {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function() {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function() {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {
    var that = this;
    wx.stopPullDownRefresh();
    //产品详情
    that.getTJdetail();
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function() {

  }
})