
import { get_lookmemlist,attention } from '../../utils/server.js';
Page({

  /**
   * 页面的初始数据
   */
  data: {
    isloadmore: false,
    currentpage:1,
    pagesize:10,
    isnextpage:0,
    list: [],
    isno:false
  }, 
  previewImg: function (e) {
    let that = this;
    let src = e.currentTarget.dataset.src;
    src = src.replace(/\/132/, '/0');
    let str = [];
    str.push(src)
    wx.previewImage({
      current: src,     //当前图片地址
      urls: str
    })
  },
  btn_detail(e){
    let item = e.currentTarget.dataset.item;
    item=JSON.stringify(item);
    item= encodeURIComponent(item);
    wx.navigateTo({
      url: '../myFocusdetail/myFocusdetail?item='+item,
    })
  },
  // 取消关注
  cen_gaunzhu(e) {
    let that = this;
    let ymemcode = e.currentTarget.dataset.memcode;
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "lookcollemp",
      "parameters": {
        "GUID": "",
        "USER_ID": "",
        "ymemcode": ymemcode,
        "memcode": memcode,
        "type": "0"
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    attention(data).then(res => {
      if (res.code == 0) {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500,
        })
        that.setData({
          currentpage:1,
          list:[]
        })
        that.get_data();
      } else {
        wx.showToast({
          title: res.msg,
          icon: 'none',
          duration: 1500
        })
      }
    })
  },
  // 获取数据 
  get_data(){
    let that=this;
    that.setData({
      isloadmore: true
    })
    let memcode = wx.getStorageSync('memcode');
    let data = {
      "actionname": "lookmemlist",
      "parameters":{
          "GUID": "",
          "USER_ID": "",
          "memcode": memcode,
          "currentpage": that.data.currentpage,
          "pagesize": that.data.pagesize
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_lookmemlist(data).then(res=>{
      that.setData({
        isloadmore: false
      })
      if (res.code == 0) {
        let list = res.data;
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
        }
      } else if (res.code == 1) {
        switch (that.data.currentpage) {
          case 1:
            that.setData({
              isno: true
            })
            break;
          default:
            break
        }
      }
    }).catch(err=>{
      that.setData({
        isloadmore: false
      })
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that = this;
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
    let that = this;
    let isnextpage = that.data.isnextpage;
    let isloadmore = that.data.isloadmore;
    if (isnextpage <= 0 || isloadmore == true) {
      return
    }
    that.data.currentpage = that.data.currentpage + 1;
    that.get_data();
  }
})