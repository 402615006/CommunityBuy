// packageFood/pages/index/index.js

import { get_shangquanlist, get_bannerlist, get_stocodelist } from '../../utils/server.js';

import { getDistance} from '../../../utils/util.js';
Page({
  /**
   * 页面的初始数据
   */
  data: {

    isloadmore: false,
    isnomore: false,
    
    bannerlist: [
      // { img: 'http://www.jingjiang.gov.cn/picture/0/eca8effbc88342848877e4a1be4dca64.jpg' },
      // { img: 'http://cs.xinpianchang.com/uploadfile/article/2018/08/19/5b7869213694f.jpeg@335w.jpg' },
      // { img: 'http://www.njyunhao.com/data/upload/image/201609/3196c98e91cff91b8b7e78c56c5e7e83.jpg' },
      // { img: 'http://img.mp.sohu.com/upload/20170622/33ab262f544c444eab82c1deeb15cc8f_th.png' },
      // { img: 'http://qcloud.dpfile.com/pc/Dgmrbz7TqbVo6uK0iMWfEKkA7yszk052laOQyMLEHtObBI9M86czpewAp1hb5qoLTYGVDmosZWTLal1WbWRW3A.jpg' },
    ],
    list:[],
    array: [],
    arrdata:[],
    title:'',
    index:0,

    typecode:'',  //所属类型
    currentpage: 1,  //页数
    pagesize: 10,    //每页多少条
    cname:'',  //输入框内容
  },

  // 点击轮播图
  previewImg: function (e) {
    let that = this;
    console.log(e);
    let index= e.currentTarget.dataset.index;
    let bannerlist = that.data.bannerlist;
    let src = bannerlist[index].img;
    let str=[];
    bannerlist.map((item,index)=>{
      str.push(item.img);
    })
    wx.previewImage({
      current: src,     //当前图片地址
      urls: str
    })
  },
  // 输入框输入完成
  go_search(){
    let that=this;
    that.setData({
      list:[],
      currentpage:1,
      isnextpage:-1
    })
    that.get_stocodelist();
  },
  bindPickerChange(e){
    let value=e.detail.value;
    let that=this;
    that.setData({
      index:value,
      list: [],
      currentpage: 1,
      isnextpage:-1
    })
    that.get_stocodelist();
  },
  // 去门店
  go_stocode(e){
    let that=this;
    let stocode=e.currentTarget.dataset.stocode;
    let title = that.data.title;
    if (title =='酒店'){
      wx.navigateTo({
        url: '/packageFood/pages/hotel/hotel',
      })
    }else{
      wx.navigateTo({
        url: '../stocode/stocode?stocode=' + stocode,
      })
    }
  },

  // input框输入函数
  input_cname(e){
    let that=this;
    that.setData({
      cname: e.detail.value
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options){
    let that=this;
    let title = options.title;
    let typecode = options.typecode;
    that.data.typecode = typecode;
    that.setData({
      title: title
    })
    wx.setNavigationBarTitle({
      title: title
    })
    if (title =='美食'){
      that.setData({
        bannerlist:[
          { img: 'http://www.jingjiang.gov.cn/picture/0/eca8effbc88342848877e4a1be4dca64.jpg' },
          { img: 'http://back.xj-wz.com/uploads/wx/201912131923527451.jpg' },
          { img: 'http://back.xj-wz.com/uploads/wx/201912131920264504.jpg' }
        ]
      })
    } else if (title == '休闲娱乐'){
      that.setData({
        bannerlist: [
          { img: 'http://img.zcool.cn/community/01fc4556f4a4a36ac72579485de0c8.jpg@1280w_1l_2o_100sh.jpg' },
          { img: 'http://img.zcool.cn/community/01621156f4a26a32f875a944604ebd.jpg@1280w_1l_2o_100sh.jpg' },
          { img: 'http://back.xj-wz.com/uploads/wx/201912131922124325.jpg' },
          { img: 'http://back.xj-wz.com/uploads/wx/201912131922091526.jpg' }
        ]
      })
    } else if (title == '酒店'){
      that.setData({
        bannerlist: [
          { img: 'http://back.xj-wz.com/uploads/wx/201912131921189574.jpg' },
          { img: 'http://back.xj-wz.com/uploads/wx/201912131921223550.jpg' },
          { img: 'http://back.xj-wz.com/uploads/wx/201912131921253181.jpg' },
          { img: 'http://back.xj-wz.com/uploads/wx/201912131921286978.jpg' }
        ]
      })
    }

    // 获取banner图
    // that.get_bannerlist();
    // 获取门店
    that.get_stocodelist();
    // that.get_shangquan();
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
    if (isnextpage <= 0) {
      return
    }
    that.data.currentpage = that.data.currentpage + 1;
    that.get_stocodelist();
  },
  // 获取轮播图
  get_bannerlist(){
    let that=this;
    let data = {
      "actionname": "getunimages",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        'modelcode':'3'
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    // get_bannerlist(data).then(res=>{
 
    //   if(res){

    //   }
    // })
  },  
  // 获取商圈
  get_shangquan(){
    let that=this;
    let data = {
      "actionname": "getnewgpslist",
      "parameters": {
        'GUID': '',
        'USER_ID': '',
        'typecode': that.data.typecode
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    get_shangquanlist(data).then(res=>{
      if(res.code==0){
        let list=res.data;
        let array=[];
        let position = wx.getStorageSync('position');
        list.map((item,index)=>{
          if (item.jwcodes){
            item.stocoordx = item.jwcodes.split(',')[0];
            item.stocoordy = item.jwcodes.split(',')[1];
            if (position){
              item.juli = getDistance(item.stocoordx, item.stocoordy, position.latitude, position.longitude);
            }else{
              item.juli=0;
            }
          }else{
            item.stocoordx = 0;
            item.stocoordy = 0;
            if (position) {
              item.juli = getDistance(item.stocoordx, item.stocoordy, position.latitude, position.longitude);
            } else {
              item.juli = 0;
            }
          }
          array.push(item.sqname)
        })
        var index=0;
        if(list.length>0){
          var juli=list[0].juli;
          list.map((item,i)=>{
            if (item.juli < juli){
              juli=item.juli;
              index=i;
            }
          })
        }
        that.setData({
          index: index,
          arrdata:list,
          array:array
        })
        that.get_stocodelist();
      }
    })
  },
  // 获取门店
  get_stocodelist(){
    let that=this;
    let index=that.data.index;
    let arrdata = that.data.arrdata;
    let filter=[];
    if (that.data.typecode){
      filter.push({ 'col': 'typecode', 'filter': that.data.typecode, 'exp': '=', 'cus': 'typecode' })
    }
    if(that.data.cname){
      filter.push({ 'col': 's.cname', 'filter': that.data.cname, 'exp': '%%', 'cus': '' })
    }
    // if (arrdata.length>0){
    //   filter.push({ 'col': 'gx.sqcode', 'filter': arrdata[index].sqcode, 'exp': '=', 'cus': '' })
    // }
    let position = wx.getStorageSync('position');
    let x=0,y=0;
    if (position){
      x = position.latitude;
      y = position.longitude;
    }
    let data = {
      "actionname": "getnewxdsqstolist",
      "parameters": {
        'limit': that.data.pagesize,
        'page': that.data.currentpage,
        'filters': filter ,
        'x':x,
        'y':y
      }
    }
    data.parameters = JSON.stringify(data.parameters);
    that.setData({
      isloadmore:true
    })
    get_stocodelist(data).then(res=>{
      that.setData({
        isloadmore: false
      })
      if(res.code==0){
        let list=res.data;
        if (position){
          list.map((item,index)=>{
            let juli = getDistance(item.stocoordx, item.stocoordy, position.latitude, position.longitude);
            item.juli=juli;
          })
        }
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
      }
    }).catch(err=>{
      that.setData({
        isloadmore: false
      })
    })
  }
})