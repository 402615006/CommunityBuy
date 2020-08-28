// packageVip/pages/cardlist/cardlist.js
import { get_opencardslist } from '../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    tabindex:0,
    tablist: ['plus会员卡','会员权益卡','次卡'],
    discard:[],
    pluscard:[],
    timescard:[],
    onLoad:false,
    stocode:''
  },
  btn_tab(e){
    console.log(e);
    let that=this;
    let index=e.currentTarget.dataset.index;
    if (index == that.data.tabindex){
      return
    }
    that.setData({
      tabindex: index
    })
  },
  // 滑动
  swiperChange(e){
    let that = this;
    let source = e.detail.source;
    if (source == 'touch') {
      that.setData({
        tabindex: e.detail.current,
      })
    }
  },
  // 获取数据
  get_data(){
    let that=this;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "getopencardsvalid",
      "parameters": {
        'GUID': '888888888',
        'USER_ID': '',
        'memcode': memcode,
        'way':'WX',
        'stocode': that.data.stocode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_opencardslist(data).then(res=>{
      console.log(res)
      if(res.status==0){
        let discard = res.discard;
        let pluscard = res.pluscard;
        let timescard = res.timescard;
        pluscard.map((item,index)=>{
          if (item.imgPaths){
            item.imgPaths = item.imgPaths.split(',')[0]
          }
        })
        that.setData({
          discard: discard,
          pluscard: pluscard,
          timescard: timescard,
          onLoad:true
        })
      }else{
        wx.showToast({
          title: res.mes,
          icon:'none',
          mask:true,
          duration:1500
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    if (options.stocode){
      that.setData({
        stocode: options.stocode
      })
    }
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