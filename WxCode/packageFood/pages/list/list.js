// packageFood/pages/list/list.js
import {get_foottag, get_footlistdata } from '../../utils/server.js';
import { get_shopcardlist , add_shopcard } from '../../../utils/util.js';
import {serverURL,disImageURL} from "../../../utils/api.js"; 

var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
      scrollTop:0,  //右边滚动条设置位置
      istrue:false,
      stoname:'',
      stoimg:'',
      logo:'',
      stocode: '', //门店编号
      leftlist:[],    //左边数组
      rightlist:[],   //右边数组
      leftlist_index:0, //左边选中的index
      shopcardlist:[]   //购物车本门店本地数据
  },
  // 点击左边
  btn_clickleft(e){
    let that=this;
    let index=e.currentTarget.dataset.index;
    if (index == that.data.leftlist_index){
      return
    }
    that.setData({
      leftlist_index:index,
      scrollTop:0,
      rightlist: []
    })
    that.get_footlistdata(index);
  },
  // 点击加号
  add(e){
    let that=this;
    let list=e.detail;
    let shopcardlist= that.data.shopcardlist;
    let data={
        'disname':list.DisName,
        'discode':list.DisCode,
        'disnum':1,      //数量
        'pdishcode':'',
        'pkkcode': list.pkkcode ,   //一级类别编号
        'TypeCode': list.TypeCode,
        'price': list.Price,
        'images':list.images,
        'unit':list.unit,
        'cookname':cookname
    };
    let flag=false;
    for (var i = 0; i < shopcardlist.length; i++) {
      if (shopcardlist[i].discode == list.DisCode) {
        flag=true;
        shopcardlist[i].disnum = Number(shopcardlist[i].disnum)+1;
        break
      }
    }
    if (flag==false){
      shopcardlist.push(data);
    }
    that.setData({
      shopcardlist: shopcardlist
    })
    add_shopcard(that.data.stocode,shopcardlist)
  },
  //点击减号
  prev(e){
    let that=this;
    let discode = e.detail.DisCode;
    let shopcardlist= that.data.shopcardlist;
    for (var i = 0; i < shopcardlist.length;i++){
      if (shopcardlist[i].discode == discode){
        if (Number(shopcardlist[i].disnum)>1){
          shopcardlist[i].disnum = shopcardlist[i].disnum-1;
        }else{
          shopcardlist.splice(i,1);
        }
        break
      }
    }
    that.setData({
      shopcardlist: shopcardlist
    })
    add_shopcard(that.data.stocode, shopcardlist)
  },
  // 添加做法
  addMethod(e){
    let item = e.detail;
    this.selectComponent("#singleselect").get_data(item);
  },
  // 清空购物车
  shopcordclear(){
    let that=this;
    let shopcardlist= that.data.shopcardlist;
    shopcardlist=[];
    that.setData({
      shopcardlist: shopcardlist
    })
    add_shopcard(that.data.stocode, shopcardlist)
  },
  // 购物车加号
  shopcardadd(e){
    let that=this;
    let index=e.detail;
    let shopcardlist= that.data.shopcardlist;
    shopcardlist[index].disnum = Number(shopcardlist[index].disnum)+1;
    that.setData({
      shopcardlist: shopcardlist
    })
    add_shopcard(that.data.stocode, shopcardlist)
  },
  // 购物车减号
  shopcardprev(e){
    let that=this;
    let index=e.detail;
    let shopcardlist = that.data.shopcardlist;
    if (Number(shopcardlist[index].disnum)>1){
      shopcardlist[index].disnum = Number(shopcardlist[index].disnum)-1;
    }else{
      shopcardlist.splice(index,1)
    }
    that.setData({
      shopcardlist: shopcardlist
    })
    add_shopcard(that.data.stocode, shopcardlist)
  },
  // 有做法的详情添加
  detailaddMethod(e){
    let that=this;
    let object=e.detail;
    let shopcardlist = that.data.shopcardlist;
    shopcardlist.push(object);
    that.setData({
      shopcardlist: shopcardlist
    })
    add_shopcard(that.data.stocode, shopcardlist)
  },
  // 去下单
  addorder(){
    let that=this;
      wx.navigateTo({
        url: '../addorder/addorder?stocode=' + that.data.stocode+'&stoname='+ that.data.stoname
    })
  },
  //判断开台后是否有订单
  get_Judge(order, ordernum) {
    let that = this;
      if (ordernum > 0) {
        // 快餐已有订单提示有订单,点击确定跳到订单付款页面
        wx.showModal({
          title: '提示',
          content: '您有未完成的订单',
          showCancel: false,
          confirmText: '去支付',
          success(res) {
            if (res.confirm) {
              let ordercode = '';
              if (ordernum > 0) {
                ordercode = order[0].PKCode;
              }
              wx.navigateTo({
                url: '../kuaicanzhifu/kuaicanzhifu?order=' + ordercode + '&stocode=' + that.data.stocode+'&stoname='+ that.data.stoname
              })
            }
          }
        })
      } else {
        // 快餐没有订单按正常页面跳转去下单页面
        wx.navigateTo({
          url: '../addorder/addorder?stocode=' + that.data.stocode+'&stoname='+ that.data.stoname
        })
      }

  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    let that=this;
    this.setData({
      stoname:options.stoname,
      stoimg:'',
      logo:options.logo,
      stocode: options.stocode,
    });
    that.get_data();
  },
  // 弹出弹框提示有点餐了去结账
  go_box(order, ordernum){
    let that=this;
      wx.showModal({
        title: '提示',
        content: '您有未完成的订单',
        showCancel:false,
        confirmText:'去支付',
        success(res) {
          if (res.confirm) {
            let billcode='';
            let ordercode='';
            if (ordernum>0){
              ordercode = order[0].PKCode;
            }
            if (billnum>0){
              billcode = bill[0].PKCode;
            }
            wx.redirectTo({
              url: '../kuaicanzhifu/kuaicanzhifu?order=' + ordercode + '&stocode=' + that.data.stocode + '&buscode=' + that.data.buscode + '&billcode=' + billcode,
            })
          } 
        }
      })
  },
  // 获取菜品分类
  get_data(){
    let that=this;
    let data = {
      "actionname": "getapplist",
      "parameters": {
        'key':'',
        'pageSize':99, 
        'currentPage': 1, 
        'filter': 'order', 
        'order': ''
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_foottag(data).then(res=>{
      if(res.code==1){
        let list=res.data;
        if(res.data.length>0){
          that.setData({
            leftlist: list,
            istrue:true
          })
          that.get_footlistdata(0);
        }else{
          wx.showModal({
            title: '提示',
            content: '该门店暂未上架菜品！',
            showCancel:false,
            success:function(res){
              if (res.confirm) {
                wx.navigateBack();
              }
            }
          })
        }
      }else{
        wx.showToast({
          title: res.msg,
          duration:1500,
          icon:'none'
        })
      }
    })
  },
  // 获取菜品列表
  get_footlistdata(index){
    let that=this;
    let leftlist = that.data.leftlist;
    let data = {
      "actionname": "getlist",
      "parameters": {
        'key': '',
        'page':1,
        'pagesize': 99,
        'stype': leftlist[index].PKCode,
        'name':that.data.name
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_footlistdata(data).then(res=>{
      console.log(res);
      if(res.code==1){
        let list = res.data;
        list.forEach(element => {
          element.images=disImageURL+ element.images;
        });
        that.setData({
          rightlist:list
        })
      }else{
        wx.showToast({
          title: res.msg,
          icon:'none',
          duration:1500
        })
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
    let that=this;
    let shopcardlist = get_shopcardlist(that.data.stocode);
    that.setData({
      shopcardlist: shopcardlist
    })
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