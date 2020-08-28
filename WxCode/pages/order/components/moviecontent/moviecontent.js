// pages/dingdan/components/text.js
import { get_dinglist } from '../../../../utils/server.js';

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    status: {
      type: String,
      value: ''
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    list: [],
    isnextpage: 0,
    currentpage: 1,
    pagesize: 10,
    isloadmore: false,
    isnomore: false,

    loading: '',    //计时器
    isonload: true   //页面是否第一次进入  
  },
  /**
   * 组件的方法列表
   */
  methods: {
    onhide() {
      let that = this;
      that.setData({
        list: [],
        isnextpage: 0,
        currentpage: 1,
        pagesize: 10,
        isloadmore: false,
        isnomore: false,
        isonload: true
      })
      clearInterval(that.data.loading)
    },
    // 第一次进入组件要请求数据
    onload() {
      let that = this;
      if (that.data.isonload) {
        that.setData({
          isonload: false
        })
        that.get_data()
      }
    },
    // 数据请求
    get_data() {
      let that = this;
      that.setData({
        isloadmore: true
      })
      let memcode = wx.getStorageSync('memcode');
      let data = {
        "actionname": "userorder",
        "parameters": {
          'GUID': '88888888',
          'status': that.data.status,
          'memcode': memcode,
          'currentpage': that.data.currentpage,
          'pagesize': that.data.pagesize
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      get_dinglist(data).then(res => {
        that.setData({
          isloadmore: false
        })
        if (res.status == 0) {
          let list = res.data[0].orderInfo;
          let isnextpage = res.isnextpage;
          switch (that.data.currentpage) {
            case 1:
              that.setData({
                list: list,
                isnextpage: res.isnextpage
              })
              break;
            default:
              that.setData({
                list: that.data.list.concat(list),
                isnextpage: res.isnextpage
              })
              break;
          }
          if (isnextpage == 0) {
            that.setData({
              isnomore: true
            })
          }
          clearInterval(that.data.loading)
          that.count_down();
        } else {
          if (that.data.currentpage > 1) {
            that.data.currentpage = that.data.currentpage - 1;
          }
          that.setData({
            isnomore: true
          })
        }
      }).catch(err => {
        if (that.data.currentpage > 1) {
          that.data.currentpage = that.data.currentpage - 1;
        }
        that.setData({
          isloadmore: false
        })
      })
    },
    // 倒计时
    count_down() {
      let that = this;
      function newtim() {
        var result = that.data.list;
        // console.log(result)
        if (result != null) {
          for (var i = 0; i < result.length; i++) {
            if (result[i].bstatus == 0) {
              var t = result[i].ctime;
              var a = new Date(t).getTime();
              var c = 14 * 60 * 1000 + 58 * 1000;
              var date = new Date();
              var duringMs = a + c - date.getTime();
              if (duringMs > 0) {
                let mm, ss
                mm = Math.floor((duringMs / 1000 / 60) % 60);
                ss = Math.floor(duringMs / 1000 % 60);
                var str = mm + "分" + ss + "秒";
              } else {
                var str = 0;
              }
              result[i]['djs'] = str;
            }
          }
          that.setData({
            list: result
          })
        }
      }
      newtim();
      that.setData({
        loading: setInterval(function () {
          newtim();
        }, 1000)
      })
    },
    // 滑动到底部
    lower() {
      let that = this;
      let isnextpage = that.data.isnextpage;
      let isloadmore = that.data.isloadmore;
      if (isnextpage <= 0 || isloadmore == true) {
        return
      }
      that.data.currentpage = that.data.currentpage + 1;
      that.get_data();
    },
    //跳转到详情
    toddxq(e) {
      var that = this;
      let orderno = e.currentTarget.dataset.id.orderno;
      let gid = e.currentTarget.dataset.id.gId;
      let isgoods = e.currentTarget.dataset.id.isgoods;
      let checkcode = e.currentTarget.dataset.id.checkcode;
      wx.navigateTo({
        url: '/packageMovie/pages/ddxq/ddxq?orderno=' + orderno + '&gid=' + gid + '&isgoods=' + isgoods + '&checkcode=' + checkcode,
      })
    },
    //进入详情页
    go_detail(e) {
     
      let result = e.currentTarget.dataset.result;
      let ordernoex = e.currentTarget.dataset.ordernoex;
      let djs = e.currentTarget.dataset.djs;
      let orderno = result.orderno;
      let gid = result.gId;
      let isgoods = result.isgoods;
      let yingyuan = result.cinemaId;
      if (result.status == 0 && result.isgain == 0) {
        if (djs) {
          if (djs != 0) {
            wx.navigateTo({
              url: '/packageMovie/pages/ddzfxq/ddzfxq?yingyuan=' + yingyuan + '&ordernoex=' + ordernoex + '&ords=' + orderno,
            })
          }
        }
      } else if (result.status != 0) {
        if (result.status == 1 && result.checkcode) {
          wx.navigateTo({
            url: '/packageMovie/pages/ddxq/ddxq?orderno=' + orderno + '&gid=' + gid + '&isgoods=' + isgoods + '&checkcode=' + result.checkcode,
          })
        }
      }
    },
    //一键支付
    payment(e) {
      let ordernoex = e.currentTarget.dataset.id;
      let yingyuan = e.currentTarget.dataset.yingyuan;
      wx.navigateTo({
        url: '/packageMovie/pages/ddzfxq/ddzfxq?yingyuan=' + yingyuan + '&ordernoex=' + ordernoex + '&ords=',
      })
    },
    // 去找电影分类
    tofenlei() {
      this.triggerEvent('tofenlei');
    },
    //去支付
    to_zhifu(e) {
      let ordernoex = e.currentTarget.dataset.id;
      let yingyuan = e.currentTarget.dataset.yingyuan;
      let ords = e.currentTarget.dataset.ords;
      wx.navigateTo({
        url: '/packageMovie/pages/ddzfxq/ddzfxq?yingyuan=' + yingyuan + '&ordernoex=' + ordernoex + '&ords=' + ords,
      })
    },
    //支付超时重新选择
    new_yingyuan(e) {
      let yingyuan = e.currentTarget.dataset.id;
      wx.navigateTo({
        url: '/packageMovie/pages/yingyuan/yingyuan?yingyuan=' + yingyuan + '&id=' + '&day='
      })
    },
  },
  //组件初次加载
  attached: function () {
    // console.log("组件初始化")
  },
  detached: function () {
    // console.log("组件被销毁")
  },
})
