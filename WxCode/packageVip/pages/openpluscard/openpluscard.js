// packageVip/pages/openpluscard/openpluscard.js
import { get_opencardsdetail,get_userinfo } from '../../utils/server.js';
import { is_gologin } from '../../../utils/util.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    cardcode:'',
    levelname:'',
    arry:[],
    money:'0',
    tab_index:0,
    list:[],
    maxmoney:0,
    minmoney:0,
    cardinfo:'' ,//会员卡信息

    actsimagesshow:true
  },
  // 输入框输入
  inputmoney(e){
    let that=this;
    let value=e.detail.value;
    that.setData({
      money:value
    })
  },
  bin_tabindex(e){
    console.log(e);
    let that=this;
    let index=e.currentTarget.dataset.index;
    that.setData({
      tab_index:index
    })
    that.get_money();
  },
  // 去开卡确认
  go_opencardconfirm(){
    let that=this;
    if (!is_gologin()){
      wx.navigateTo({
        url: '/pages/login/login',
      })
      return
    }

    let coupons='';
    let pcode=''
    let money = that.data.money;
    let conmin = that.data.minmoney;
    let conmax = that.data.maxmoney;
    if (!money){
      wx.showToast({
        title: '请选择充值金额',
        icon:'none',
        duration:1500,
        mask:true
      })
      return
    }
    if (Number(money) < Number(conmin) || Number(money) > Number(conmax)){
      wx.showToast({
        title: '充值金额应在' + conmin + '-' + conmax+'之间',
        icon: 'none',
        duration: 1500,
        mask: true
      })
      return
    }
    let arry = that.data.list;
    let tab_index = that.data.tab_index;
    if (arry.length>0){
      pcode = arry[tab_index].pcode;
      let presentNum = arry[tab_index].presentNum;
      let num = 0;
      arry[tab_index].couponarry.map((item,index)=>{
        if (item.ischecked == true) {
          num++;
          if (coupons==''){
            coupons = item.name2 + ',' + item.name3 + ',' + item.name5
          }else{
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
    get_userinfo(data).then(res=>{
      if(res.status==0){
        let list = res.data;
        let name = list.cname;
        let card = list.IDNO;
        let tel = list.mobile;
        if (name && card && tel){
            wx.navigateTo({
              url: '../opencardconfirm2/opencardconfirm2?type=0' + '&levelcode=' + that.data.cardcode + '&money=' + that.data.money + '&pcode=' + pcode + '&coupons=' + coupons+'&card='+card+'&name='+name+'&tel='+tel,
            })
        }else{
          that.selectComponent("#showbox").show();
        }
      }else{
        wx.showToast({
          title: res.mes,
          icon:'none',
          duration:1500,
          mask:true
        })
      }
    })
  },
  get_data(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'getopencardvalidinfo',
      'parameters': {
        'GUID': '',
        'USER_ID': '',
        'memcode': memcode,
        'levelcode': that.data.cardcode,
        'way':'WX'
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_opencardsdetail(data).then(res=>{
      console.log(res);
      if(res.status==0){
        let cardinfo=res.data;
        if (cardinfo.imgPaths){
          cardinfo.imgPaths = cardinfo.imgPaths.split(',')[0]
        }
        let maxmoney = res.data.maxpay;
        let minmoney = res.data.minpay;
        let levelname = res.data.levelname;

        that.setData({
          maxmoney: maxmoney,
          minmoney: minmoney,
          levelname: levelname,
          money: minmoney,
          cardinfo: cardinfo
        })
        let arry=[];
        let list = res.data.acts;
        let actsimagesshow=true;
        if(list.length>0){
          list.map((time,i)=>{
            if (!time.smallimg || time.smallimg=='null'){
              actsimagesshow=false;
            }
            let arry=[];
            let presentNum = time.presentNum;
            if (time.coupons){
              let coupons = time.coupons;
              coupons = coupons.split(';');
              coupons.map((item, index) => {
                arry.push({});
                let result = item.split(',');
                result.map((ctim, idx) => {
                  arry[index]['name' + idx] = ctim
                })
                if (coupons.length == presentNum){
                  arry[index].ischecked = true
                }else{
                  arry[index].ischecked = false
                }
              })
            }
            time.couponarry = arry
          })
        }
        console.log(list);
        that.setData({
          list:list,
          actsimagesshow: actsimagesshow
        })
        console.log(list)
        that.get_money();
      }else{
        wx.showToast({
          title: res.mes,
          icon:'none',
          duration:1500,
          mask:true
        })
      }
    })
  },
  get_money(){
    let that=this;
    let list=that.data.list;
    let tab_index = that.data.tab_index;
    if (list.length>0){
      let maxmoney = list[tab_index].conmax;
      let minmoney = list[tab_index].conmin;
      let money = minmoney;
      that.setData({
        maxmoney: maxmoney,
        minmoney: minmoney,
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
    let that=this;
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

  }
})