import {
  get_hdlist,
  get_number,
  set_lottery
} from "../../utils/server.js";
var util = require('../../utils/utils.js');
var timer = null; //计时器
Page({

  /**
   * 页面的初始数据
   */
  data: {
    actid: "",//活动id
    refid: "",
    people: "", //参与活动人数
    records: [], //中奖纪录
    actname: "", //标题 
    awards: [], //奖品清单
    regulation: [], //规则
    isPossibleDraw: true, //是否可抽奖
    number: 0, //抽奖次数
    animationData: {}, //抽奖杆动画
    redEnvelopeList0: [],
    redEnvelopeList1: [],
    redEnvelopeList2: [],
    animation0: 0,
    time0: 4, //第一列转动时间
    time1: 5.2,
    time2: 6.2,
    isShowRaffleInfo: false, //是否显示抽奖结果
    raffleInfoanimation: {}, //抽奖结果动画
    rafflecontent: "", //抽中奖品内容
    raffleInfoUser: "", //抽中人头像
    raffleWin: true, //抽奖结果，成功、失败
    raffletitle: "", //中奖名称
    regulationHeight: "", //view高度
    winHeight: 120,//中奖名单高度
  },
  /* 
   *初始化加载数据
   */
  onLoad(options) {
    wx.hideShareMenu();
    var that = this;
    var refid = '';
    var actid = options.actid; 
    if (options.refid){
      refid = options.refid;
    }
    that.setData({
      actid: actid,
      refid: refid,
    })
    that.randomNumbers(); //生成三组随机数
    that.get_data(); //数据信息
    that.get_number(); //抽奖次数
    // that.timerStar(); //开计时器
  },

  randomNumbers() {
    var that = this;
    var arr1 = [];
    var arr2 = [];
    var arr3 = [];
    for (var i = 0; i < 10; i++) {
      arr1.push(util.random())
    }
    for (var i = 0; i < 10; i++) {
      arr2.push(util.random())
    }
    for (var i = 0; i < 10; i++) {
      arr3.push(util.random())
    }
    that.setData({
      redEnvelopeList0: arr1,
      redEnvelopeList1: arr2,
      redEnvelopeList2: arr3,
    })
  },

  //获取数据
  get_data() {
    let that = this;
    let unionid = wx.getStorageSync('unionid');
    let data = "{'GUID':'','USER_ID':'" + unionid + "','actid':'" + that.data.actid + "'}"
    get_hdlist(data).then(res => {
      console.log(res);
      if (res.status == 0) {
        // let prizes = res.data.prizes;
        let records = res.data.records;
        let people = res.data.people;
        var actname = res.data.actname;
        var regulation = res.data.activityexternalexplain.split(';');
        if (regulation.length == 1 || regulation.length == 2) {
          that.setData({
            regulationHeight: 120,
          })
        } else if (regulation.length == 3) {
          that.setData({
            regulationHeight: 200,
          })
        } else if (regulation.length == 4) {
          that.setData({
            regulationHeight: 280,
          })
        } else {
          that.setData({
            regulationHeight: 350,
          })
        }
        if (records.length == 0 || records.length == 1 || records.length == 2) {
          that.setData({
            winHeight: 120,
          })
        } else if (records.length == 3) {
          that.setData({
            winHeight: 200,
          })
        } else if (records.length == 4) {
          that.setData({
            winHeight: 280,
          })
        } else {
          that.setData({
            winHeight: 350,
          })
        }
        wx.setNavigationBarTitle({
          title: actname
        })
        that.setData({
          records: records,
          // awards: prizes,
          people: people,
          actname: actname,
          regulation: regulation
        })
      } else {
        wx.showToast({
          title: res.mes,
          duration: 1500,
          icon: 'none',
          mask: true
        })
      }
    })
  },


  // 获取剩余次数
  get_number() {
    let that = this;
    let unionid = wx.getStorageSync('unionid');
    let data = "{'GUID':'','USER_ID':'" + unionid + "','actid':'" + that.data.actid + "'}"
    get_number(data).then(res => {
      console.log(res);
      if (res.status == 0) {
        that.setData({
          number: res.times
        })
      }
    })
  },

  //抽奖
  playReward: function(a, b) {
    let that = this;
    let nickName = a;
    let nickavatarUrlName = b;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let openid = wx.getStorageSync('openid');
    let mobile = wx.getStorageSync('mobile');
    let data = "{'GUID':'','USER_ID':'" + unionid + "','actid':'" + that.data.actid + "','refid':'" + that.data.refid + "','username':'" + nickName + "','userhead':'" + nickavatarUrlName + "','mobile':'" + mobile + "'}"
    set_lottery(data).then(res => {
      console.log("******************")
      console.log(res);
      if (res.status == 0) {
        var title = res.data.name;
        var raffleWin = true;
        var status = 0;
        if (title == "一等奖") {
          status = 1;
        } else if (title == "二等奖") {
          status = 2;
        } else if (title == "三等奖") {
          status = 3;
        }else if (title == "特等奖") {
          status = "4";
        }
        that.start(status); //老虎机效果      
        that.get_number(); // 获取剩余次数
        that.lganima(); //拉杆动画
        setTimeout(function() {
          that.setData({
            isPossibleDraw: true, //可点击
            isShowRaffleInfo: true, //弹出层
            rafflecontent: res.data.content.split("|")[1].split("(")[0], //奖品信息
            raffletitle: res.data.name, //奖品信息
            raffleInfoUser: res.data.userimg, //抽奖人头像
            raffleWin: raffleWin, //是否中奖
          })
          that.raffleInfoanimation(); //抽奖结果
          that.get_data(); //刷新数据信息
        }, that.data.time2 * 1000)
      } else {
        wx.showToast({
          title: res.mes,
          duration: 1500,
          mask: true,
          icon: 'none'
        })
        that.setData({
          isPossibleDraw: true,
        })
      }
    }).catch(err => {
      that.setData({
        isPossibleDraw: true,
      })
    })
  },

  // 点击抽奖按钮
  bindGetUserInfo(e) { //获取用户信息
    var that = this;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let openid = wx.getStorageSync('openid');
    let mobile = wx.getStorageSync('mobile');
    if (!unionid || !memcode || !mobile || !openid) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }

    that.setData({
      isShowRaffleInfo: false,
      isPossibleDraw: false,
      raffletitle:"",
    })
    if (that.data.number < 1) {
      wx.showToast({
        title: "抽奖次数已用完",
        duration: 1500,
        mask: true,
        icon: 'none'
      })
      that.setData({
        isPossibleDraw: true,
      })
      return
    }
    if (e.detail.errMsg == "getUserInfo:ok") {
      console.log(e)
      // clearInterval(timer); //关闭计时器
      let nickName = e.detail.userInfo.nickName;
      let nickavatarUrlName = e.detail.userInfo.avatarUrl;
      that.randomNumbers(); //生成三组随机数
      that.playReward(nickName, nickavatarUrlName);
    } else {
      wx.showToast({
        title: "获取用户信息失败",
        duration: 1500,
        mask: true,
        icon: 'none'
      })
      that.setData({
        isPossibleDraw: true,
      })
    }
  },
  //拉杆动画
  lganima() {
    var that = this;
    var animation = wx.createAnimation({
      duration: 750,
      timingFunction: 'ease',
    })
    that.animation = animation;
    animation.right("8%").step()
    that.setData({
      animationData: animation.export()
    })
    setTimeout(function() {
      animation.right("10%").step()
      that.setData({
        animationData: animation.export()
      })
    }.bind(that), 500)
  },

  /**
   * @params start 抽奖事件
   */
  start(status) {
    var that = this;
    var list0 = that.data.redEnvelopeList0;
    var list1 = that.data.redEnvelopeList1;
    var list2 = that.data.redEnvelopeList2;

    if (status == 0) {
      var num1 = util.random();
      var num2 = util.nonRepeating(num1 + 1);
      var num3 = util.nonRepeating(num1 + 2);
      list0[0] = num1;
      list1[0] = num2;
      list2[0] = num3;
    } else {
      list0[0] = status;
      list1[0] = status;
      list2[0] = status;
    }
    console.log("*************")
    
    console.log(status)
    //  重置数组顺序后转动两圈
    that.setData({
      redEnvelopeList0: list0,
      redEnvelopeList1: list1,
      redEnvelopeList2: list2,
    }, () => {
      that.setData({
        animation0: that.data.animation0 + 720
      })
    })
  },

  //关闭抽奖信息弹窗
  closeRaffle() {
    var that = this;
    // that.timerStar(); //开计时器
    that.setData({
      isShowRaffleInfo: false
    })
  },

  //抽奖信息弹窗
  raffleInfoanimation() {
    var that = this;
    var animation = wx.createAnimation({
      duration: 0,
      timingFunction: "linear",
      delay: 0
    })
    that.animation = animation
    animation.translateY("50%").step()
    that.setData({
      raffleInfoanimation: animation.export(),
    })
    setTimeout(function() {
      animation.translateY("-50%").step()
      that.setData({
        raffleInfoanimation: animation.export(),
      })
    }.bind(that), 200)
  },

  onMyEvent() {
    var that = this;
    that.randomNumbers(); //生成三组随机数
    that.get_data(); //数据信息
    that.get_number(); //抽奖次数
  },

  //开计时器
  timerStar() {
    var that = this;
    clearInterval(timer);
    timer = setInterval(function() {
      that.randomNumbers();
    }, 10000);
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */

  onReady: function() {

  },
  /**
 * 页面相关事件处理函数--监听用户下拉动作
 */
  onPullDownRefresh: function () {
    var that = this;
    that.get_data(); //数据信息
    that.get_number(); //抽奖次数
    wx.stopPullDownRefresh();
  },
  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function() {
    clearInterval(timer);
  },
  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function(res) {
    var that = this;
    let unionid = wx.getStorageSync('unionid');
    if (!unionid) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }
    if (res.from === 'button') {
      // 来自页面内转发按钮
      return {
        title: this.data.actname,
        path: 'packageActivity/pages/index/index?actid=' + that.data.actid+'&refid=' + unionid,
        // imageUrl: this.data.coverImage,
        success: (res) => {
          // 成功后要做的事情
          console.log(res)
          wx.showToast({
            title: "分享成功",
            icon: 'none',
            duration: 2000
          })
        },
        fail: function(res) {
          // 分享失败
          console.log(res)
          wx.showToast({
            title: "分享失败",
            icon: 'none',
            duration: 2000
          })
        }
      }
    }
  }
})