import { getselectstoreinfolist, get_selectlist } from '../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    bar_Height: wx.getSystemInfoSync().statusBarHeight,
    titleBarHeight: '',
    value:'',
    list: [],
    tag: [],
    isloadmore:false,
    isno:false,
    focus:false,
    currentpage: 1,   //第几页
    pagesize: 100,     //一页多少
    isnextpage: 0, //是否有下一页
  },
  // 返回
  goBack(){
    wx.navigateBack();
  },
  // 输入框输入
  input_value(e){
    let that=this;
    let value=e.detail.value;
    that.setData({
      value: value
    })
  },
  // 点击tag
  btn_tag(e){
    console.log(e);
    let value = e.currentTarget.dataset.text;
    this.setData({
      value:value,
      focus:true
    })
  },
  //搜索
  btn_search(e){
    console.log(e);
    let that=this;
    let value=e.detail.value;
    if (!value){
      that.setData({
        list:[]
      })
      return
    }
    that.setData({
      isloadmore:true,
      isno:false,
      list:[]
    })
    let jcode = '0';
    let wcode = '0';
    let position = wx.getStorageSync('position');
    if (position) {
      jcode = position.latitude;
      wcode = position.longitude;
    }
    let data = {
      "actionname": "getselectstoreinfolist",
      "parameters": {
        "sqcode":"",
        "hytype":"",
        "order":"",
        "schname": that.data.value,
        "x": jcode,
        "y": wcode,
        "page": that.data.currentpage,
        "limit": that.data.pagesize
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getselectstoreinfolist(data).then(res=>{
      if(res.status==0){
        let list=res.data;
        that.setData({
          list:list,
          isloadmore: false
        })
        if (list.length==0){
          that.setData({
            isno:true
          })
        }
      }else{
        that.setData({
          list:[],
          isloadmore: false
        })
      }
    }).catch(err=>{
      that.setData({
        isloadmore: false
      })
    })
  },
  btn_tap(e) {
    console.log(e);
    let that = this;
    let item = e.currentTarget.dataset.item;
    let ismovie = item.ismovie;
    if (ismovie == 1) {
      let cinemaid = item.cinemaid;
      wx.navigateTo({
        url: '/packageMovie/pages/yingyuan/yingyuan?yingyuan=' + cinemaid + '&id=' + '&day=',
      })
    } else {
      let stocode = item.stocode;
      wx.navigateTo({
        url: '/packageFood/pages/stocode/stocode?stocode=' + stocode,
      })
    }
  },
  // 获取热搜词
  get_selectlist(){
    let that=this;
    let data = {
      "actionname": "getselectlist",
      "parameters": "{}"
    }
    get_selectlist(data).then(res=>{
      if(res.status==0){
        if (res.data){
          let tag=res.data;
          that.setData({
            tag: tag
          })
        }
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options){
    let that=this;
    that.get_selectlist();
    wx.getSystemInfo({
      success: function (res) {
        if (res.platform == "devtools") {
          that.setData({
            titleBarHeight: 48
          })
        } else if (res.platform == "ios") {
          that.setData({
            titleBarHeight: 44
          })
        } else if (res.platform == "android") {
          that.setData({
            titleBarHeight: 48
          })
        }
      }
    })
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