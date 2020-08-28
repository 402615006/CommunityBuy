// packageVip/pages/openvipcard/openvipcard.js
import { get_opencardsdetail, get_userinfo } from '../../utils/server.js';
import { is_gologin } from '../../../utils/util.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    cardcode:'',
    tabindex:0,
    list:[],
    arry:[],
    money:0,
    tab_index:0,
    levelname:'',
    cardinfo:''
  },
  // 选择年卡、月卡、季卡
  btn_select(e){
    let that=this;
    let index=e.currentTarget.dataset.index;
    that.setData({
      tabindex: index
    })
  },
  bin_tabindex(e) {
    console.log(e);
    let that = this;
    let index = e.currentTarget.dataset.index;
    that.setData({
      tab_index: index
    })
    that.get_money();
  },
  // 去开卡确认
  go_opencardconfirm() {
    let that = this;
    if (!is_gologin()) {
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }

    let coupons = '';
    let pcode='';
    let arry = that.data.list;
    let tab_index = that.data.tab_index;
    console.log(arry)
    if (arry.length > 0) {
      pcode = arry[tab_index].pcode;
      let presentNum = arry[tab_index].presentNum;
      let num = 0;
      arry[tab_index].couponarry.map((item, index) => {
        if (item.ischecked == true){
          num++;
          if (coupons == '') {
            coupons = item.name2 + ',' + item.name3 + ',' + item.name5
          } else {
            coupons = coupons + ';' + item.name2 + ',' + item.name3 + ',' + item.name5
          }
        }
      })
      if (Number(num) != Number(presentNum)) {
        wx.showToast({
          title: '请选择' + presentNum + '种优惠券',
          icon: 'none',
          duration: 1500,
          mask: true
        })
        return
      }
    }
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'memberinfo',
      'parameters': {
        "GUID": "",
        "USER_ID": "",
        "memcode": memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_userinfo(data).then(res => {
      if (res.status == 0){
        let list = res.data;
        let name = list.cname;
        let card = list.IDNO;
        let tel = list.mobile;
        if (name && card && tel){
          wx.navigateTo({
            url: '../opencardconfirm2/opencardconfirm2?type=0' + '&levelcode=' + that.data.cardcode + '&money=' + that.data.money + '&pcode=' + pcode + '&coupons=' + coupons + '&card=' + card + '&name=' + name + '&tel=' + tel,
          })
        } else {
          that.selectComponent("#showbox").show();
        }
      } else {
        wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
    })
    
  },
  get_data() {
    let that = this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'getopencardvalidinfo',
      'parameters': {
        'GUID': '',
        'USER_ID': '',
        'memcode': memcode,
        'levelcode': that.data.cardcode,
        'way': 'WX'
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_opencardsdetail(data).then(res => {
      if (res.status == 0) {
        let cardinfo=res.data;
        let money = res.data.maxpay;

        let levelname = res.data.levelname;
        that.setData({
          money: money,
          levelname: levelname,
          cardinfo: cardinfo
        })
        let arry = [];
        let list = res.data.acts;
        if (list.length > 0) {
          list.map((time, i) => {
            let arry = [];
            let presentNum = time.presentNum;
            if (time.coupons) {
              let coupons = time.coupons;
              coupons = coupons.split(';');
              coupons.map((item, index) => {
                arry.push({});
                let result = item.split(',');
                result.map((ctim, idx) => {
                  arry[index]['name' + idx] = ctim
                })
                if (coupons.length == presentNum) {
                  arry[index].ischecked = true
                } else {
                  arry[index].ischecked = false
                }
              })
            }
            time.couponarry = arry
          })
        }
        that.setData({
          list: list
        })
        console.log(list)
        that.get_money();
      } else {
        wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
    })
  },
  // 计算价格
  get_money() {
    let that = this;
    let list = that.data.list;
    let tab_index = that.data.tab_index;
    if (list.length > 0) {
      let money = list[tab_index].conmax;
      that.setData({
        money: money
      })
    }
  },
  // 点击优惠券
  click_box_li1(e) {
    let that = this;
    let index = e.currentTarget.dataset.index;
    let list = that.data.list;
    let tab_index = that.data.tab_index;
    let ischecked = list[tab_index].couponarry[index].ischecked;
    if (ischecked == true) {
      list[tab_index].couponarry[index].ischecked = false
    } else {
      let num = 0;
      let presentNum = list[tab_index].presentNum;
      list[tab_index].couponarry.map((item, index) => {
        if (item.ischecked == true) {
          num++
        }
      })
      if (Number(num) < Number(presentNum)) {
        list[tab_index].couponarry[index].ischecked = true
      } else {
        wx.showToast({
          title: '可选优惠券种类最多' + presentNum,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
    }
    that.setData({
      list: list
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this;
    let cardcode = options.cardcode;
    that.setData({
      cardcode: cardcode
    })
    that.get_data();
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },
})