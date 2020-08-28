// let wxparse = require("../../wxParse/wxParse.js");
import {
  getLBdetail
} from '../../utils/server.js';
import {
  baserURLcard
} from "../../utils/api.js"; //域名引入

Page({
  /**
   * 页面的初始数据
   */
  data: {
    img: [], //图片或视频Arr
    introduction: "",
    type: "", //类型 3 视频 其它图片
    id: "",
    nodes: "", //富文本
    videoindex: '',
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    var that = this;
    console.log(options)
    wx.showLoading({
      title: '加载中...',
    })
    var id = options.id;
    var type = options.type;
    that.setData({
      id: id,
      type: type,
      baserURLcard: baserURLcard
    })
    that.updata();
  },

  //加载数据
  updata() {
    let that = this;
    let data = {
      "actionname": "detail",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        "id": that.data.id
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getLBdetail(data).then(res => {
      console.log(res)
      if (res.code == 0) {
        var list = res.data[0];
        var imgArr = [];
        var type = list.Type;
        if (type == 3) {
          imgArr = list.Videos;
        } else {
          var contentList = list.Url;
          imgArr = contentList.split(",");
        }
        list.Description = list.Description.replace(/\<img/gi, '<img style="max-width:100%;height:auto"');
        that.setData({
          actname: list.ActivityName,
          img: imgArr,
          nodes: list.Description,
        })

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

  //点击播放,其它视频暂停
  bindplay: function(e) {
    var that = this;
    var id = e.currentTarget.dataset.pid,
      videoindex = e.currentTarget.dataset.index;
    var videoCtx = wx.createVideoContext(id); //获取点击的视频
    var videoArr = that.data.img;
    for (var i = 0; i < videoArr.length; i++) {
      if (i != videoindex) {
        var videoCtxPrev = wx.createVideoContext('myVideo' + i); //找到当前正在播放的视频
        videoCtxPrev.pause(); //暂停
      }else{
        videoCtx.play();
      }
    }
  },

  //图片加载完成后的回调函数
  imageLoad(ev) {
    console.log(ev)
    wx.hideLoading()
    // let src = ev.currentTarget.dataset.src,
    //   width = ev.detail.width,
    //   height = ev.detail.height
  },
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {},

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
  onUnload: function() {},

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {

  },

})