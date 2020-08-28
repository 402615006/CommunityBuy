// packageVip/pages/servicedetail/servicedetail.js;

import { getcustomerbytype } from '../../utils/server.js';


Page({

  /**
   * 页面的初始数据
   */
  data: {
    id:'',
    list:''
  },
  get_data(){
    let that=this;
    let data = {
      "actionname": "getcustomerbyid",
      "parameters": {
        "GUID": "",
        "USER_ID": "",
        "cuscode": that.data.id
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    getcustomerbytype(data).then(res=>{
      console.log(res)
      if(res.code==0){
        let list=res.data[0];
        that.setData({
          list:list
        })
      }else{
        wx.showToast({
          title: res.msg,
          icon:'none',
          duration:1500,
          mask:true
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    let id=options.id;
    that.setData({
      id:id
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