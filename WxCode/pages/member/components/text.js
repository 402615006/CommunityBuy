// pages/member/components/text.js
import { getvipprolist } from '../../../utils/server.js';
import { baserURLOrganization } from '../../../utils/api.js';
Component({
  /**
   * 组件的属性列表
   */
  properties: {
    tab_index:{
      type: Number,
      value: ''
    },
    idx:{
      type: Number,
      value: ''
    }
  },

  /**
   * 组件的初始数据
   */
  data: {
    isonload: true,  
    isno:false,
    isloadmore:false,
    list:[],
    currentpage:1,
    pagesize:10,
    isnextpage:0,
    url: baserURLOrganization,
  },

  /**
   * 组件的方法列表
   */
  methods: {
    // 第一次加载
    onload(e) {
      let that = this;
      if (that.data.isonload) {
        that.setData({
          isonload: false
        })
        that.get_data()
      }
    },
    // 获取数据
    get_data(){
      let that=this;
      that.setData({
        isloadmore:true
      })
      let data = {
        "actionname": "getvipprolist",
        "parameters": {
          'GUID': '',
          'USER_ID': '',
          "type":that.data.idx+1,
          "currentpage":that.data.currentpage,
          "pagesize":that.data.pagesize
        }
      }
      data.parameters = JSON.stringify(data.parameters);
      getvipprolist(data).then(res=>{
        console.log(res)
        setTimeout(()=>{
          that.setData({
            isloadmore: false
          })
          if(res.code==0){
            let list=res.data;
            for(var i = 0;i<list.length;i++){
              list[i].smallimg = that.data.url+list[i].simg;
              list[i].etime = list[i].enddate.split(" ")[0];
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
          }else{
            that.setData({
              isloadmore:false,
              isno: true
            })
          }    
        },300)
      }).catch(err=>{
        that.setData({
          isloadmore: false
        })
      })
    },
    fenxiang(e){
      var that = this;
      var item = e.currentTarget.dataset.item;
      that.triggerEvent('fxjd',item);
    },

    todetail(e){
      var that = this;
      var code = e.currentTarget.dataset.code;
      that.triggerEvent('todetail',code);
    },
    // 上拉加载更多
    onReachBottom(){
      let that = this;
      let isnextpage = that.data.isnextpage;
      let isloadmore = that.data.isloadmore;
      if (isnextpage == 0 || isloadmore == true) {
        return
      }
      that.data.currentpage = that.data.currentpage + 1;
      that.get_data();
    },
    // 下拉刷新
    onPullDownRefresh(){
      let that=this;
      that.setData({
        currentpage: 1,
        list:[]
      })
      that.get_data();
    }
  }
})
