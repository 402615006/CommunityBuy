
import { get_myearn, get_myearnreport } from '../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    tab:['今日','昨日','近7日','近30日'],
    tab_index:0,
    list:'',
    list2:''
  },
  get_data(){
    let that=this;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'myearn',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid,
        'memcode': memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_myearn(data).then(res=>{
      if(res.status==0){
        let list=res.data;
        that.setData({
          list:list
        })
      }else{
        wx.showToast({
          title: res.mes,
          icon:'none',
          duration:1500
        })
      }
    })
  },
  get_myearnreport(){
    let that=this;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let type='0';
    let tab_index = that.data.tab_index;
    if (tab_index==1){
      type='1';
    } else if (tab_index == 2){
      type = '7';
    } else if (tab_index == 3){
      type = '30';
    }
    let data = {
      'actionname': 'myearnreport',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid,
        'memcode': memcode,
        'type':type
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_myearnreport(data).then(res=>{
      if(res.status==0){
        let list=res.data;
        that.setData({
          list2:list
        })
      }else{
        wx.showToast({
          title: res.mes,
          icon: 'none',
          duration: 1500
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    that.get_myearnreport();
  },
  // 去结算说明
  go_Billinginstructions(){
    let that=this;
    let title = that.data.list.note;
    wx.navigateTo({
      url: '../Billinginstructions/Billinginstructions?title=' + title,
    })
  },
  // 去提现
  go_withdraw(){
    wx.navigateTo({
      url: '../withdraw/withdraw'
    })
  },
  // 去余额明细
  go_IncomeBreakdown(){
    wx.navigateTo({
      url: '../IncomeBreakdown/IncomeBreakdown'
    })
  },
  //去提现记录
  go_tixianlist(){
    wx.navigateTo({
      url: '../tixianlist/tixianlist'
    })
  },
  // 点击tab
  btn_tab(e){
    let that=this;
    let index = e.currentTarget.dataset.index;
    that.setData({
      tab_index:index
    })
    that.get_myearnreport();
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
    let that=this;
    that.get_data();
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