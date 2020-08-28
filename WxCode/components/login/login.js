// components/login/login.js
var { baseURL } = require('../../utils/api.js');
var { codeURL } = require('../../utils/api.js');
import { set_userInfo } from '../../utils/server.js'; 

var app = getApp();
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
    userInfo: {},
    canIUse: wx.canIUse('button.open-type.getUserInfo'),
    animationlogin: "",
    showlogin: false,
    showtip: false,

    animationlogin2: "",
    showlogin2: false,
    c2: 0,
    tel_value: "",
    code_value: "",

    code: "",
    tel: "",
    is_login: 2,
    unionid: '',
    user_info: false,
  },


  /**
   * 组件的方法列表
   */
  methods: {
    // 查看是否授权有就存入用户信息
    get_userinfo(){
      let that=this;
      // 查看是否授权
      wx.getSetting({
        success(re){
          if (re.authSetting['scope.userInfo']) {
            // 已经授权，可以直接调用 getUserInfo 获取头像昵称
            wx.getUserInfo({
              success: function (res){
                let userInfo = res.userInfo;
                wx.setStorageSync('userInfo', userInfo);
                let unionid = wx.getStorageSync('unionid');
                let data = {
                  'actionname': 'updateuserinfo',
                  'parameters': {
                    'unionid': unionid,
                    'userInfo': userInfo,
                  }
                }
                console.log('拿到用户信息')
                console.log(data);
                data.parameters = JSON.stringify(data.parameters);
                set_userInfo(data).then(res=>{
                  console.log(res);
                })
              }
            })
          }
        }
      })
    },
    // 获取失败
    code_false() {
      let that = this;
      let user_info = that.data.user_info;
      let showlogin2 = that.data.showlogin2;
      console.log(user_info)
      console.log(showlogin2)
      if (!user_info && !showlogin2) {
        that.hidelogin();
      }
    },
    //提示显示
    showtip(mes) {
      let that = this;
      that.setData({
        showtip: true,
      })
    },
    //提示隐藏
    hiddentip() {
      let that = this;
      that.setData({
        showtip: false
      })
    },
    btn_ok() {
      this.hiddentip();
      console.log(mes)
      setTimeout(() => {
        wx.showModal({
          title: '提示',
          showCancel: false,
          content: '登陆成功'
        })
      }, 1000)
    },
    //获取用户信息取消
    sele_user() {
      this.hidelogin();
      this.triggerEvent('mycen');
    },
    // 点击X
    hidden() {
      this.hidelogin2();
      this.hidelogin();
      this.triggerEvent('mycen');
    },

    bindGetUserInfo(e){
      let that = this;
      if (e.detail.errMsg == "getUserInfo:ok") {
        that.setData({
          user_info: false
        })
        let encryptedData = e.detail.encryptedData;
        let iv = e.detail.iv;
        let session_key = wx.getStorageSync('session_key');
        that.getNeededUserInfo(encryptedData, iv, session_key);
      } else {
        //点击拒绝授权按钮
      }
    },
    //调取后台获取unionid
    getNeededUserInfo(encryptedData, iv, session_key) {
      let that = this;
      let data = {
        "actionname": "mpdecrypt",
        "parameters": "{'encryptedData':'" + encryptedData + "','iv':'" + iv + "','sessionKey':'" + session_key + "'}"
      };
      console.log(data)
      wx.request({
        url: `${baseURL}/UserCenter.ashx`,
        method: 'POST',
        data: data,
        header: {
          "Content-Type": "application/x-www-form-urlencoded"
        },
        dataType: 'json',
        success: function (res) {
          console.log(res)
          if (res.data.unionId) {
            wx.setStorageSync('unionid', res.data.unionId);
            that.showlogin2();
            if (res.data.openId) {
              wx.setStorageSync('openid', res.data.openId);
            }
          } else {
            wx.showModal({
              title: '提示',
              showCancel: false,
              content: '获取用户信息失败',
              success(res) {
                if (res.confirm) {
                  that.hidelogin();
                  that.triggerEvent('mycen');
                }
              }
            })
          }
        },
        fail: function () {
          that.hidelogin();
          wx.showModal({
            title: '提示',
            showCancel: false,
            content: '获取信息失败,请稍后再试',
            success(res) {
              if (res.confirm) {

                that.triggerEvent('mycen');
              }
            }
          })
        }
      })
    },
    // 显示遮罩层
    showlogin: function () {
      let that = this;
      this.setData({
        is_login: 2
      })
      wx.showLoading({
        mask: true,
        title: '正在加载...',
      })
      wx.login({
        success: function (res) {
          let c = res.code;
          console.log(c)
          let data = {
            "actionname": "getmpuser",
            "parameters": "{'code':'" + c + "'}"
          }
          console.log(data);
          wx.request({
            url: `${baseURL}/UserCenter.ashx`,
            data: data,
            methods: 'GET',
            dataType: 'json',
            success: function (res) {
              console.log(res.data)
              wx.hideLoading();
              let session_key = res.data.session_key;
              let openid = res.data.openid;
              if (openid) {
                wx.setStorageSync('openid', openid);
              }
              if (session_key) {
                wx.setStorageSync('session_key', session_key);
              }
              if (res.data.unionid) {
                let unionid = res.data.unionid;
                wx.setStorageSync('unionid', unionid);
                // that.showlogin2();
              } 
              wx.getSetting({
                  success(re) {
                    if (re.authSetting['scope.userInfo']) {
                      console.log("进入到有授权")
                      wx.getUserInfo({
                        success: function (res) {
                          let encryptedData = res.encryptedData;
                          let iv = res.iv;
                          that.getNeededUserInfo(encryptedData, iv, session_key)
                        }
                      })
                    } else {
                      console.log("进入到这步")
                      that.setData({
                        user_info: true
                      })
                    }
                  }
              })
              
            },
            fail: function (res) {
              wx.hideLoading();
              that.hidelogin();
              wx.showModal({
                title: '提示',
                showCancel: false,
                content: '服务器繁忙,请稍后再试',
                success(res) {
                  if (res.confirm) {
                    that.hidelogin();
                    that.triggerEvent('mycen');
                  }
                }
              })
            }
          })
        },
        fail: function (res) {
          wx.hideLoading();
          that.hidelogin();
          wx.showModal({
            title: '提示',
            showCancel: false,
            content: '信息获取失败,请稍后再试'
          })
        }
      })


      // 创建一个动画实例
      var animation = wx.createAnimation({
        // 动画持续时间
        duration: 200,
        // 定义动画效果，当前是匀速
        timingFunction: "linear",
        delay: 0
      })
      // 将该变量赋值给当前动画
      this.animation = animation
      // 先在y轴偏移，然后用step()完成一个动画
      animation.translateY(600).step()
      // 用setData改变当前动画
      this.setData({
        // 通过export()方法导出数据
        animationlogin: animation.export(),
        // 改变view里面的Wx：if
        showlogin: true
      })
      // 设置setTimeout来改变y轴偏移量，实现有感觉的滑动
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          animationlogin: animation.export()
        })
      }.bind(this), 200)
    },
    // 隐藏遮罩层
    hidelogin() {
      this.hidelogin2();
      wx.hideLoading();
      var animation = wx.createAnimation({
        duration: 200,
        timingFunction: "linear",
        delay: 0
      })
      this.animation = animation
      animation.translateY(600).step()
      this.setData({
        animationlogin: animation.export(),
      })
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          animationlogin: animation.export(),
          showlogin: false,
          showlogin2: false,
          user_info: false,
          animationlogin2: ''
        })
      }.bind(this), 200)
    },
    showlogin2() {
      var animation = wx.createAnimation({
        // 动画持续时间
        duration: 200,
        // 定义动画效果，当前是匀速
        timingFunction: "linear",
        delay: 0
      })
      this.animation = animation
      // 先在y轴偏移，然后用step()完成一个动画
      animation.translateY(600).step()
      // 用setData改变当前动画
      this.setData({
        // 通过export()方法导出数据
        animationlogin2: animation.export(),
        // 改变view里面的Wx：if
        showlogin2: true
      })
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          animationlogin2: animation.export()
        })
      }.bind(this), 200)
    },
    hidelogin2() {
      var animation = wx.createAnimation({
        duration: 200,
        timingFunction: "linear",
        delay: 0
      })
      this.animation = animation
      animation.translateY(600).step()
      this.setData({
        animationlogin2: animation.export(),
      })
      setTimeout(function () {
        animation.translateY(0).step()
        this.setData({
          animationlogin2: animation.export(),
          showlogin2: false
        })
      }.bind(this), 200)
    },
    //获取验证码
    vcode() {
      var that = this;
      let wait = 60;
      let tel_1 = /^1[0|1|2|3|4|5|6|7|8|9][0-9]{9}$/;
      if (this.data.c2 == 0) {
        if (this.data.tel_value) {
          if (tel_1.test(this.data.tel_value)) {
            tim();
            let tel = this.data.tel_value;
            wx.request({
              url: `${baseURL}/Other.ashx`,
              data: {
                "actionname": "sendmsg",
                "parameters": "{'mobile':'" + tel + "','descr': '绑定操作'}"
              },
              methods: 'GET',
              dataType: 'json',
              success: function (res) {
                wx.hideLoading()
                if (res.data.status == 0) {
                  that.setData({
                    code: res.data.mes,
                    tel: tel
                  })
                  console.log(that.data.code, that.data.tel)
                } else {
                  wx.showModal({
                    title: '提示',
                    showCancel: false,
                    content: '验证码发送失败'
                  })
                }
              },
              fail: function (res) {
                wx.hideLoading();
                console.log(res)
                wx.showModal({
                  title: '提示',
                  showCancel: false,
                  content: '验证码发送失败'
                })
              }
            })
          } else {
            wx.showToast({
              mask: true,
              icon: "none",
              title: '手机号格式不正确',
              duration: 1500,
            })
          }
        } else {
          wx.showToast({
            mask: true,
            icon: "none",
            title: '手机号不能为空',
            duration: 1500,
          })
        }
      }
      //计时器
      function tim() {
        that.setData({
          c2: wait
        })
        wait--;
        setTimeout(() => {
          if (wait > 0) {
            tim()
          } else {
            that.setData({
              c2: 0
            })
          }
        }, 1000)
      }
    },
    //获取手机号input value
    tel_value(e) {
      this.data.tel_value = e.detail.value;
    },
    //获取验证码input value
    code_value(e) {
      this.data.code_value = e.detail.value;
    },
    //微信快速登陆
    // getUserInfo(e) {
    // var that = this
    // that.hidelogin();
    // 查看是否授权
    // wx.getSetting({
    //   success(res) {
    //     if (res.authSetting['scope.userInfo']) {
    //       // 已经授权，可以直接调用 getUserInfo 获取头像昵称
    //       wx.getUserInfo({
    //         success: function (res) {
    //           console.log(res.userInfo)
    //           wx.setStorageSync('userInfo', res.userInfo);
    //         }
    //       })
    //     }
    //   }
    // })
    // console.log(e)
    // console.log(e.detail)
    // app.globalData.userInfo = e.detail.userInfo
    // that.queryUsreInfo();
    // this.setData({
    //   userInfo: e.detail.userInfo,
    // })
    // },


    //登陆
    go_login() {
      let that = this;
      if (this.data.tel_value) {
        if (this.data.code_value) {
          if (this.data.tel) {
            if (this.data.tel == this.data.tel_value) {
              if (this.data.code == this.data.code_value) {
                that.hidelogin();
                let mobile = this.data.tel_value;
                let openid = wx.getStorageSync('openid');
                let unionid = wx.getStorageSync('unionid');
                that.get_request(openid, unionid, mobile)
              } else {
                wx.showToast({
                  mask: true,
                  icon: "none",
                  title: '验证码不正确',
                  duration: 1500,
                })
              }
            } else {
              wx.showToast({
                mask: true,
                icon: "none",
                title: '该手机号与发送验证码手机号不一致',
                duration: 1500,
              })
            }
          } else {
            wx.showToast({
              mask: true,
              icon: "none",
              title: '请先发验证码',
              duration: 1500,
            })
          }
        } else {
          wx.showToast({
            mask: true,
            icon: "none",
            title: '请输入验证码',
            duration: 1500,
          })
        }
      } else {
        wx.showToast({
          mask: true,
          icon: "none",
          title: '请输入手机号',
          duration: 1500,
        })
      }
      console.log(this.data.tel_value, this.data.code_value)
    },
    getPhoneNumber(e) {
      let that = this;
      let session_key = wx.getStorageSync('session_key');
      let encryptedData = e.detail.encryptedData;
      let iv = e.detail.iv;
      if (!encryptedData || !iv) {

      } else {
        that.hidelogin();
        // let session_key = wx.getStorageSync('session_key');
        // let encryptedData = e.detail.encryptedData;
        // let iv = e.detail.iv;
        wx.request({
          url: `${baseURL}/UserCenter.ashx`,
          data: {
            "actionname": "mpdecrypt",
            "parameters": "{'encryptedData':'" + encryptedData + "','iv':'" + iv + "','sessionKey':'" + session_key + "'}"
          },
          header: {
            "Content-Type": "application/x-www-form-urlencoded"
          },
          methods: 'POST',
          dataType: 'json',
          success: function (res) {
            wx.hideLoading();
            console.log(res.data)
            if (res.data.phoneNumber) {
              let mobile = res.data.phoneNumber;
              let openid = wx.getStorageSync('openid');
              let unionid = wx.getStorageSync('unionid');
              that.get_request(openid, unionid, mobile)
            } else {
              wx.showModal({
                title: '提示',
                showCancel: false,
                content: res.data.mes,
                success(res) {
                  if (res.confirm) {
                    // that.showlogin();
                  }
                }
              })
            }
            console.log(res.data.phoneNumber)
          },
          fail: function (res) {
            wx.hideLoading();
            wx.showModal({
              title: '提示',
              showCancel: false,
              content: '用户手机号获取失败,请稍后再试',
              success(res) {
                if (res.confirm) {
                  // that.showlogin();
                }
              }
            })
          }
        })
      }

      // if (e.detail.errMsg == 'getPhoneNumber:fail user deny' || e.detail.errMsg=='getPhoneNumber:user deny'){

      // }else{
      //   that.hidelogin();
      //   let session_key = wx.getStorageSync('session_key');
      //   let encryptedData = e.detail.encryptedData;
      //   let iv = e.detail.iv;
      //   wx.request({
      //     url: `${baseURL}/UserCenter.ashx`,
      //     data: {
      //       "actionname": "mpdecrypt",
      //       "parameters": "{'encryptedData':'" + encryptedData + "','iv':'" + iv + "','sessionKey':'" + session_key + "'}"
      //     },
      //     header:{
      //       "Content-Type": "application/x-www-form-urlencoded"
      //     },
      //       methods: 'POST',
      //       dataType: 'json',
      //       success: function (res){
      //         wx.hideLoading();
      //         console.log(res.data)
      //         if (res.data.phoneNumber){
      //             let mobile = res.data.phoneNumber;
      //             let openid = wx.getStorageSync('openid');
      //             let unionid = wx.getStorageSync('unionid');
      //             that.get_request(openid,unionid, mobile)
      //         }else{
      //           wx.showModal({
      //             title: '提示',
      //             showCancel: false,
      //             content: res.data.mes,
      //             success(res) {
      //               if (res.confirm) {
      //                 that.showlogin();
      //               }
      //             }
      //           })
      //         }
      //           console.log(res.data.phoneNumber)
      //         },
      //         fail: function (res){
      //           wx.hideLoading();
      //           wx.showModal({
      //           title: '提示',
      //           showCancel: false,
      //           content: '用户手机号获取失败',
      //           success(res){
      //             if (res.confirm){
      //               that.showlogin();
      //             }
      //           }
      //         })
      //       }
      //   })
      // }
    },
    //调取后台获取登录卡
    get_request(openid, unionid, mobile) {
      let that = this;
      wx.showLoading({
        mask: true,
        title: '登录中...',
      })
      console.log(openid, unionid, mobile)
      let invitecode='';
      if (app.globalData.shareId){
        invitecode = app.globalData.shareId;
      }
      let data = {
        "actionname": "getuserstatus",
        "parameters": {
          'unionid':unionid,
          'openid':openid,
          'mobile':mobile,
          'invitecode': invitecode
        }
      }
      console.log('登录时传过去的参数')
      console.log(data);
      wx.request({
        url: `${baseURL}/UserCenter.ashx`,
        data: data,
        methods: 'GET',
        dataType: 'json',
        success: function (res) {
          console.log(res.data)
          setTimeout(() => {
            wx.hideLoading();
            if (res.data.status == 0) {
              let datas = res.data.data[0];
              if (datas) {
                let memcode = datas.memcode;
                let tel = datas.mobile;
                let invitecode = datas.invitecode;
                if (memcode){
                  wx.setStorageSync('memcode', memcode);
                  wx.setStorageSync('mobile', tel);
                  wx.setStorageSync('invitecode', invitecode);
                  wx.setStorageSync('isonLoad', true);
                  wx.showToast({
                    title: '登录成功',
                    icon:'success',
                    duration:1000
                  })
                  that.triggerEvent('myevent');
                  that.setData({
                    tel: '',
                    code: ''
                  })
                  // 查看是否授权
                  that.get_userinfo();
                } else {
                  wx.showModal({
                    title: '提示',
                    showCancel: false,
                    content: '登录失败',
                    success(res) {
                      if (res.confirm) {
                        // that.showlogin();
                      }
                    }
                  })
                }
              } else {
                wx.showModal({
                  title: '提示',
                  showCancel: false,
                  content: '登录失败',
                  success(res) {
                    if (res.confirm) {
                      // that.showlogin();
                    }
                  }
                })
              }
            } else {
              wx.showModal({
                title: '提示',
                showCancel: false,
                content: res.data.mes,
                success(res) {
                  if (res.confirm) {
                    // that.showlogin();
                  }
                }
              })
            }





            // if (res.data.status == 0 || res.data.status==2){
            //   wx.setStorageSync('memcode', res.data.mes);
            //   wx.setStorageSync('mobile',mobile);
            //   that.setData({
            //     tel:'',
            //     code:''
            //   })
            //   that.triggerEvent('myevent');
            // } else if (res.data.status==1){
            //   wx.setStorageSync('memcode', res.data.mes);
            //   wx.setStorageSync('mobile', mobile);
            //   that.setData({
            //     tel: '',
            //     code: ''
            //   })
            //   that.showtip();
            //   that.triggerEvent('myevent');
            // }else{
            //   wx.showModal({
            //     title: '提示',
            //     showCancel: false,
            //     content: '登录失败',
            //     success(res) {
            //       if (res.confirm) {
            //         that.showlogin();
            //       }
            //     }
            //   })
            // }
          }, 1000)
        },
        fail: function (res) {
          setTimeout(() => {
            wx.hideLoading();
            wx.showModal({
              title: '提示',
              showCancel: false,
              content: '登录失败',
              success(res) {
                if (res.confirm) {
                  that.showlogin();
                }
              }
            })
          }, 1000)
        }
      })
    },
    getUserInfo() {
      let that = this;
      console.log("触发")
      wx.getUserInfo({
        withCredentials: true,
        success: function (res) {
          let encryptedData = res.encryptedData;
          let iv = res.iv;
          let session_key = wx.getStorageSync('session_key')
          that.getNeededUserInfo(encryptedData, iv, session_key)
        },
        fail: function (res) {

        }
      })
    },
    switch_tel() {
      this.setData({
        is_login: 1
      })
    },
    switch_wechat() {
      this.setData({
        is_login: 2
      })
    }
  }
})
