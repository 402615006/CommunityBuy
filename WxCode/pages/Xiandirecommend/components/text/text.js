// packageOrganization/pages/IWantToBuild/components/text/text.js
import { topprolist } from '../../../../utils/server.js';
import { baserURLOrganization } from '../../../../utils/api.js';
Component({
  /**
   * 组件的属性列表
   */
  properties: {

  },

  /**
   * 组件的初始数据
   */
  data: {
    isno: false,
    isloadmore: false,
    currentpage: 1,//页数
    pagesize: 10,//每页多少条
    isnextpage:"",//是否有下一页
    noMore:false,
    url: baserURLOrganization,
    isonload:true,
    list:[],
    status:2,
    typec:'0',
    typeorder:0,
  },

  /**
   * 组件的方法列表
   */
  methods: {
    onload() {
      let that = this;
      if (that.data.isonload) {
        that.setData({
          isonload: false
        })
        that.getListData();
      }
    },

    getListData(idx){
      let that = this;
      let position = wx.getStorageSync('position');
      var jcode = '';
      var wcode = '';
      if(position){
        jcode = position.longitude;
        wcode = position.latitude;
      }
      that.setData({
        isloadmore:true
      })
      let data = {
        'actionname': 'topprolist',
        'parameters': {
          "GUID": "",
          "USER_ID": "",
          "stotype":0,
          "order":that.data.typeorder,
          "status":that.data.status,
          "jcode":wcode,
          "wcode":jcode,
          "currentpage":that.data.currentpage,
          "pagesize":that.data.pagesize,
          "stocode":'0'
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      topprolist(data).then(res => {
        console.log(res)
        that.setData({
          isloadmore: false
        })
        if (res.code == 0) {
          let list = res.data;
          for(var i = 0;i<list.length;i++){
            list[i].smallimg = that.data.url+list[i].smallimg;
          }
          that.setData({
            list: that.data.list.concat(list),
            isnextpage:res.isnextpage,
          })
          if(list.length>=5&&list.length<that.data.pagesize){
            that.setData({
              noMore: true,
            })
          }
        }else if(res.code==1){
          that.setData({
            isno: true
          })
        } else {
            wx.showToast({
              title: res.msg,
              icon: 'none',
              mask: true,
              duration: 1500
            })
        }
      }).catch(err=>{
        that.setData({
          isloadmore: false
        })
      })
    },
    //价格排序
    btn_price(){
      let that = this;
      that.setData({
        typec: '1',
        currentpage: 1,
        typeorder:3,
        list: []
      })
      if (that.data.status == 1) {
        that.setData({
          status: '2'
        })
      }else{
        that.setData({
          status: '1'
        })
      }
      that.getListData();
    },
    //人数排序
    btn_num(){
      let that = this;
      that.setData({
        typec: '2',
        currentpage: 1,
        typeorder:1,
        list: []
      })
      if (that.data.status == 1) {
        that.setData({
          status: '2'
        })
      }else{
        that.setData({
          status: '1'
        })
      }
      that.getListData();
    },
    //距离排序
    btn_address(){
      let that = this;
      that.setData({
        typec: '0',
        currentpage: 1,
        typeorder:4,
        list: [],
        status: '1'
      })
      that.getListData();
    },
    go_ToBuild(e) {
      var procode = e.currentTarget.dataset.code;
      wx.navigateTo({
        url: '/pages/memDetail/memDetail?procode=' + procode+'&type=2',    
      })
    },
    // 滑动到底部
    lower(){
      let that = this;
      let isnextpage = that.data.isnextpage;
      let isloadmore = that.data.isloadmore;
      if (isnextpage == 0 || isloadmore==true) {
        return
      }
      that.data.currentpage = that.data.currentpage + 1;
      that.getListData();
    },
    // 下拉刷新
    onPullDownRefresh(){
      let that=this;
      that.setData({
        currentpage: 1,
        list:[]
      })
      that.getListData();
    }
  }
})
