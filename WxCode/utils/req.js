
//公共请求数据方法
const req = function (data) {
  // console.log(data)
  let that = this;
  let url = data.url;
  let da = data.data;
  let me = data.method;
  let loadingtext = data.loadingtext;
  let datatype = data.datatype;
  let loadingtimes = data.loadingtimes;
  let hideLoading = data.hideLoading;
  let showLoading = false;
  if (data.showLoading) {
    showLoading = data.showLoading
  }
  return new Promise((resolve,reject) => {
    if (showLoading == true) {
      //弹出加载中loading
      wx.showLoading({
        mask: true,
        title: loadingtext,
      })
    }
    //正式请求数据
    wx.request({
      url: url,
      data: da,
      header: {
        'content-type': 'application/x-www-form-urlencoded' // 默认值
      },
      method: me,
      dataType: datatype,
      success: function (res){
        // console.log(res);
        if (showLoading) {
          setTimeout(() => {
            //关闭loading
            if (hideLoading) {
              wx.hideLoading();
            }
            //返回数据
            switch (res.statusCode) {
              case 200:
                resolve(res.data);
                break;
              case -1:
                wx.showToast({
                  mask: true,
                  title: '网络有点不给力~',
                  icon: 'none',
                  duration: 2000
                })
                reject('失败');
                break;
              case 500:
                wx.showToast({
                  mask: true,
                  title: '服务器内部错误',
                  icon: 'none',
                  duration: 2000
                })
                reject('失败');
                break;
              case 404:
                wx.showToast({
                  mask: true,
                  title: '请求地址不存在',
                  icon: 'none',
                  duration: 2000
                })
                reject('失败');
                break;
              default:
                wx.showToast({
                  mask: true,
                  title: '访问失败,出现未知错误！',
                  icon: 'none',
                  duration: 2000
                })
                reject("失败")
            }
          }, loadingtimes)
        } else {
          //关闭loading
          if (hideLoading) {
            wx.hideLoading();
          }
          //返回数据
          switch (res.statusCode) {
            case 200:
              resolve(res.data);
              break;
            case -1:
              wx.showToast({
                mask: true,
                title: '网络有点不给力~',
                icon: 'none',
                duration: 2000
              })
              reject('失败');
              break;
            case 500:
              wx.showToast({
                mask: true,
                title: '服务器内部错误',
                icon: 'none',
                duration: 2000
              })
              reject('失败');
              break;
            case 404:
              wx.showToast({
                mask: true,
                title: '请求地址不存在',
                icon: 'none',
                duration: 2000
              })
              reject('失败');
              break;
            default:
              wx.showToast({
                mask: true,
                title: '访问失败,出现未知错误！',
                icon: 'none',
                duration: 2000
              })
              reject("失败")
          }
        }
      },
      fail: function (res) {
        //关闭loading
        if (hideLoading) {
          wx.hideLoading();
        }
        // 请求出错返回
        wx.showToast({
          mask: true,
          title: res.errMsg,
          icon: 'none',
          duration: 2000
        })
        reject("失败")
      },
      complete: function (res) {

      },
    })
  })
}

//公共上传文件方法
const upf = function (data) {
  let that = this;
  let url = data.url;
  let da = data.data;
  let loadingtext = data.loadingtext;
  let loadingtimes = data.loadingtimes;
  let showLoading = false;
  if (data.showLoading) {
    showLoading = data.showLoading
  }
  return new Promise((resolve, reject) => {
    if (showLoading == true) {
      //弹出加载中loading
      wx.showLoading({
        mask: true,
        title: loadingtext,
      })
    }
    //正式请求数据
    wx.uploadFile({
      url: url,
      filePath: da,
      name: 'file',
      header: {
        "Content-Type": "multipart/form-data",
        'accept': 'application/json'
      },
      formData: {
        'user': 'test'  //其他额外的formdata，可不写
      },
      success: function (res) {
        if (showLoading) {
          if (res.statusCode == 200) {
            setTimeout(() => {
              wx.hideLoading();
              resolve(res.data);
            }, 500)
          } else {
            wx.showToast({
              mask: true,
              title: '上传图片失败',
              icon: 'none',
              duration: 2000
            })
            reject("上传图片失败")
          }
        } else {
          if (res.statusCode == 200) {
            wx.hideLoading();
            resolve(res.data);
          } else {
            wx.showToast({
              mask: true,
              title: '上传图片失败',
              icon: 'none',
              duration: 2000
            })
            reject("上传图片失败")
          }
        }
      },
      fail: function (res) {
        //关闭loading
        wx.hideLoading();
        // 请求出错返回
        wx.showToast({
          mask: true,
          title: res.errMsg,
          icon: 'none',
          duration: 2000
        })
        reject("请求出错接口报错")
      }
    })
  })
}
module.exports = {
  req, upf
}

