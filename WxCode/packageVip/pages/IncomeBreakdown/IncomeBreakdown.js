import { get_myearndetail } from '../../utils/server.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    isloadmore:false,
    isno:false,
    date: formatDateYearMonthDay(),
    enddate: formatDateYearMonthDay(),
    list:[]
  },  
  bindchange(e){
    console.log(e);
    let date=e.detail.value;
    let that=this;
    that.setData({
      date: date
    })
    that.get_data();
  },
  // 获取数据
  get_data(){
    let that=this;
    let unionid = wx.getStorageSync('unionid');
    let memcode = wx.getStorageSync('memcode');
    let data = {
      'actionname': 'myearndetail',
      'parameters': {
        'GUID': '',
        'USER_ID': unionid,
        'memcode': memcode,
        'date':that.data.date
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_myearndetail(data).then(res=>{
      if(res.status==0){
        let list=res.data;
        list.map((item,index)=>{
          if (Number(item.income)>=0){
            item.income = '+' + item.income
          }
        })
        that.setData({
          list:list,
          isno: true
        })
      }else{
        that.setData({
          isno:true,
          list:[]
        })
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
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

// 获取当前时间年月
function formatDateYearMonthDay() {
  var date = new Date();
  var Y = date.getFullYear() + '-';
  var M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1);
  return (Y + M )
}