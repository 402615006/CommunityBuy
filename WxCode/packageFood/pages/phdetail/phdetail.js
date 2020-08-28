
import { paidui_detail, cen_paidui } from '../../utils/server.js';

var app = getApp();

Page({
  /**
   * 页面的初始数据
   */
  data: {
    list:'',
    bg_img:''
  },
  // 去点餐
  go_stocode(){
    wx.navigateBack();
  },
  // 点击取消排队
  cen_paihao(e){
    let that=this;
    wx.showModal({
      title: '温馨提示',
      content: '您确定要取消排队吗？',
      success: function (res) {
        if (res.confirm) {
          that.cen_paihao_ok(e)
        }
      }
    })
  },
  // 取消排队调接口
  cen_paihao_ok(e){
    let that = this;
    let stocode = e.currentTarget.dataset.stocode;
    let id = e.currentTarget.dataset.id;
    let list = that.data.list;
    let data = {
      "actionname": "updatestatus",
      "parameters": {
        'BusCode': '88888888',
        'StoCode': stocode,
        'id': id,
        'status': '4'
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    cen_paidui(data).then(res => {
      if (res.code == 0) {
        wx.showToast({
          title: '取消成功',
          icon: 'success',
          duration: 1500,
          mask: true
        })
        list.TStatus = '4';
        that.setData({
          list: list
        })
        setTimeout(()=>{
          wx.navigateBack()
        },1500)
      } else {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500,
          mask: true
        })
      }
    })
  },
  // 获取数据
  get_data() {
    let that = this;
    let filter = [];
    let mobile = wx.getStorageSync('mobile');
    let stocode = that.data.stocode;
    filter.push({ 'col': 'wxid', 'filter': mobile, 'exp': '=', 'cus': '' }, { 'col': 'StoCode', 'filter': stocode, 'exp': '=', 'cus': '' })
    let data = {
      "actionname": "getlist",
      "parameters": {
        'limit': '10',
        'page': '1',
        'filters': filter
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    paidui_detail(data).then(res => {
      if (res.code == 0) {
        let list = res.data[0];
        that.setData({
          list:list
        })
        wx.setNavigationBarTitle({
          title: list.StoName
        })
      }else{
        wx:wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500,
          mask: true,
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    let bg_img = app.stoimg;
    let stocode=options.stocode;
    that.setData({
      stocode: stocode,
      bg_img: bg_img
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