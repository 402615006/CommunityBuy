
import {getnewslist, getdishlist } from '../../../utils/server.js';
import {serverURL,disImageURL} from "../../../utils/api.js";   //域名引入
var { is_gologin } = require('../../../utils/util.js');

Component({
  /**
   * 组件的属性列表
   */
  properties: {
    titleBarHeight:{
      type: Number,
      value: ''
    },
    TM_CollSucess:{
      type:Array,
      value:[]
    }
  },
  
  /**
   * 组件的初始数据
   */
  data: {
    bar_Height: wx.getSystemInfoSync().statusBarHeight,     //上方高度
    isloadmore:false,
    dishlist:[],
    newslist:[],
    currentpage:1,   //第几页
    pagesize:10,     //一页多少
    isnextpage: 0, //是否有下一页
    type:1,
    status:'1'
  },

  /**
   * 组件的方法列表
   */
  methods: {
    btn_times(){
      that.get_dishdata();
    },
    btn_num(){
      that.get_dishdata();
    },
    // 点击进入详情
    go_detail(e){
      let that=this;
      if (!is_gologin()) {
        wx.navigateTo({
          url: '/pages/login/login',
        })
        return
      }
      let collcode = e.currentTarget.dataset.collcode;
      wx.navigateTo({
        url: '/packageOrganization/pages/Senate/Senate?collcode=' + collcode,
      })
    },
    //获取公告
    get_news(){
      let that=this;
      let request = {
        "actionname": "getlist",
        "parameters": {
          'key': '',
          'type': '2',
          'page': '1',
          'pagesize': '1'
        }
      }
      request.parameters = JSON.stringify(request.parameters);
      getnewslist(request).then(res=>{
        if(res.code==1){
          let list=res.data;
          list.map((item, index) => {
            item.images=serverURL + '/UploadFiles/' + item.images;
          });
          that.setData({
            newslist: list
          });
        }
      });
    },
    // 获取数据
    get_dishdata(){
      let that=this;
      that.setData({
        isloadmore:true
      })

      let data = {
        "actionname": "getlist",
        "parameters": {
          "key": "",
          "ftype": that.data.type,
          "page": that.data.currentpage,
          "pagesize": that.data.pagesize,
          "name":""
        }
      }

      data.parameters = JSON.stringify(data.parameters);
      getdishlist(data).then(res=>{
        that.setData({
          isloadmore: false
        })
        if(res.code==1){
          let list=res.data;
          list.map((item, index) => {
            item.images=disImageURL+ item.images;
          });
          that.setData({
            dishlist: list,
            isnextpage: res.isnextpage
          })
        }else{
          if (that.data.currentpage==1){
            that.setData({
              dishlist: []
            })
          }
        }
      }).catch(err=>{
        that.setData({
          isloadmore: false
        })
      })
      return;
    },
    // 上拉加载更多
    onReachBottom(){
      let that = this;
      let isnextpage = that.data.isnextpage;
      let isloadmore = that.data.isloadmore;
      if (isnextpage <= 0 || isloadmore == true) {
        return
      }
      that.data.currentpage = that.data.currentpage + 1;
      that.get_dishdata();
    },
    // 下拉刷新
    onPullDownRefresh(){
      let that=this;
      that.setData({
        currentpage: 1,
        // status: '1',
        type_index: '0'
        // tab_index:1
      })
      that.get_dishdata();
      that.get_news();
    }
  }
})
