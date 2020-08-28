
import { myordersbyvippro } from '../../../../utils/server.js'
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
    show:false,
    foodbox_index: 0,
    foodbox_text: [
      {name:'全部',idx:3},
      {name:'待付款',idx:0},
      {name:'已付款',idx:1},
      {name:'退款/售后',idx:2}
    ],
    idx:"3",
    type:['全部','新人专享','闲弟推荐','会员福利'],
    type_index:0,
  },

  /**
   * 组件的方法列表
   */
  methods: {
    onhide() {
      let that = this;
      if (that.data.show) {
        that.setData({
          show: false
        })
      }
    },
    show() {
      let that = this;
      that.setData({
        show: true
      })
      let foodbox_index = that.data.foodbox_index;
      let id = "#text" + foodbox_index;
      that.selectComponent(id).onload();
    },
    hide() {
      let that = this;
      that.setData({
        show: false
      })
    },
    // 头部切换
    btn_tab(e) {
      let that = this;
      let index = e.currentTarget.dataset.index;
      let idx = e.currentTarget.dataset.idx;
      that.setData({
        foodbox_index: index,
        idx:idx
      })
    },
    // 点击筛选后调用
    bindPickerChangetype(e){
      let value = e.detail.value;
      let that = this;
      if (value == that.data.type_index){
        return
      }
      that.setData({
        type_index: value,
      })
      let foodbox_index = that.data.foodbox_index;
      let id = "#text" + foodbox_index;
      that.selectComponent(id).onload();
    },
    swiperChange(e) {
      console.log(e)
      let that = this;
      let source = e.detail.source;
      if (source == 'touch') {
        that.setData({
          foodbox_index: e.detail.current,
          idx:that.data.foodbox_text[e.detail.current].idx,
        })
      }
      let foodbox_index = e.detail.current;
      let id = "#text" + foodbox_index;
      that.selectComponent(id).onload();
    },
  }
})
