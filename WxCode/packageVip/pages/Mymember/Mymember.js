import { get_mymembers, get_mymembersearch } from '../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    tab: ['专属会员','普通会员'],
    tab_index:0,
    list: [],
    nickname:'',
    isloadmore:false,
    zscount:'',
    ptcount:'',
    count:'',
    pmemcode:''
  },
  get_data(){
    let that=this;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'mymembers',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid,
        'memcode': memcode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_mymembers(data).then(res=>{
      if(res.status==0){
        let zscount = res.zscount;
        let ptcount = res.ptcount;
        let count = res.count;
        let pmemcode = res.pmemcode;
        that.setData({
          zscount: zscount,
          ptcount: ptcount,
          count: count,
          pmemcode: pmemcode
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
  // 点击搜索
  btn_search(){
    let that=this;
    that.get_mymembersearch();
  },
  get_mymembersearch(){
    let that=this;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let type='0';
    if (that.data.tab_index==0){
      type='1';
    }else{
      type = '0';
    }
    that.setData({
      isloadmore:true
    })
    let data = {
      'actionname': 'mymembersearch',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid,
        'memcode': memcode,
        'nickname': that.data.nickname,
        'type': type
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_mymembersearch(data).then(res=>{
      that.setData({
        isloadmore: false
      })
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
    }).catch(err=>{
      that.setData({
        isloadmore: false
      })
    })
  },
  input_value(e){
    let that=this;
    let value=e.detail.value;
    that.setData({
      nickname:value
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    that.get_data();
    that.get_mymembersearch();
  },
  // 点击tab
  btn_tab(e){
    let that=this;
    let index = e.currentTarget.dataset.index;
    if (index == that.data.tab_index){
      return
    }
    that.setData({
      tab_index:index
    })
    that.get_mymembersearch();
  },
  // 显示会员的详情
  btn_detail(e){
    let that=this;
    console.log(e);
    let memcode = e.currentTarget.dataset.memcode;
    that.selectComponent('#box').show(memcode,true);
  },
  show_myprev(e){
    let that = this;
    console.log(e);
    let memcode = e.currentTarget.dataset.memcode;
    that.selectComponent('#box').show(memcode,false);
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

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})