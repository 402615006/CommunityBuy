Page({
  /**
   * 页面的初始数据
   */
  data: {
    tab_index: 0,
    tab: ['首页', '分类', '发现', '我的'],
    video_index: 0,
    count: 10,
    isnextpage: 1,
    currentpage: 1,
    video_list: [
        {
          'video_src': 'https://stream7.iqilu.com/10339/upload_transcode/202002/18/20200218093206z8V1JuPlpe.mp4?sign=43cf344e83089348eeeea38d26ba51bb&t=1589851514',
          'name': '山东卫视',
          'muic_name': '自己去找',
          'text': '抗衰老的弗兰克看看了第三方律师代理费开始来的快疯了快递师傅冷酷么法暮色里发生可反馈肯菲菲老师能克服的那份可是当你妇科老妇女索拉非尼'
        },
        {
          'video_src': 'https://stream7.iqilu.com/10339/article/202002/17/c292033ef110de9f42d7d539fe0423cf.mp4',
          'name': '闪电视频',
          'muic_name': '自己去找',
          'text': '抗衰老的弗兰克看看了第三方律师代理费开始来的快疯了快递师傅冷酷么法暮色里发生可反馈肯菲菲老师能克服的那份可是当你妇科老妇女索拉非尼'
        },
        {
          'video_src': 'https://stream7.iqilu.com/10339/upload_transcode/202002/18/20200218025702PSiVKDB5ap.mp4',
          'name': '新冠肺炎',
          'muic_name': '自己去找',
          'text': '抗衰老的弗兰克看看了第三方律师代理费开始来的快疯了快递师傅冷酷么法暮色里发生可反馈肯菲菲老师能克服的那份可是当你妇科老妇女索拉非尼'
        },
        {
          'video_src': 'https://stream7.iqilu.com/10339/article/202002/18/2fca1c77730e54c7b500573c2437003f.mp4',
          'name': '新年好',
          'muic_name': '自己去找',
          'text': '抗衰老的弗兰克看看了第三方律师代理费开始来的快疯了快递师傅冷酷么法暮色里发生可反馈肯菲菲老师能克服的那份可是当你妇科老妇女索拉非尼'
        },
        {
          'video_src': 'https://stream7.iqilu.com/10339/upload_transcode/202002/18/20200218093206z8V1JuPlpe.mp4?sign=43cf344e83089348eeeea38d26ba51bb&t=1589851514',
          'name': '山东卫视',
          'muic_name': '自己去找',
          'text': '抗衰老的弗兰克看看了第三方律师代理费开始来的快疯了快递师傅冷酷么法暮色里发生可反馈肯菲菲老师能克服的那份可是当你妇科老妇女索拉非尼'
        }
    ]
  },
  // 轮播图滑动事件完成后触发
  swiperChange(e) {
    let index = e.detail.current;
    let that = this;
    let video_list = that.data.video_list;

    let videoContextPrev = wx.createVideoContext('myVideo' + this.data.video_index);
    console.log(this.data.video_index + 1);
    videoContextPrev.pause();    //停止上一个视频播放
    that.setData({
      video_index: index
    })

    let videoContext = wx.createVideoContext('myVideo' + this.data.video_index);
    console.log(this.data.video_index + 1)
    videoContext.play();      //播放当前视频
  },
  // 底部tab点击触发
  btn_tab(e) {
    let index = e.currentTarget.dataset.index;
    this.setData({
      tab_index: index
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {

  },
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {
    // 页面渲染完成播放第一个视频
    let videoContext = wx.createVideoContext('myVideo' + this.data.video_index);
    videoContext.play();
  }
})