// AJAX POST请求 通用
var rooturl = 'http://192.168.5.10:8001/';
//显示图片站点根路径
var VRootPath = 'http://192.168.5.10:8001/';

//连锁端接口地址
var ChainUrl = 'http://192.168.5.10:7777';

//连锁端同步地址
var LSsynchronize = 'http://192.168.5.10:7777';

function commonAjax(path, parameters, isasync, successFn) {
    if (path.length <= 0) {
        return;
    }
    //alert(rooturl + path);
    $.ajax({
        type: 'POST',
        url: path,
        data: parameters,
        dataTpye: 'josn',
        async: isasync,
        error: function (data) {
            console.log('失败');
        },
        success: function (data) {
            var Jdata = data;
            if (data != undefined && data.length > 0) {
                Jdata = JSON.parse(data);
                successFn(Jdata);
            }
        }
    });
}

function commonAjaxsysn(path, parameters, isasync, successFn) {
    if (path.length <= 0) {
        return;
    }

    $.ajax({
        type: 'POST',
        url: LSsynchronize + path,
        data: parameters,
        dataTpye: 'josn',
        async: isasync,
        error: function (data) {
            console.log('失败');
        },
        success: function (data) {
            if (data.length > 0) {
                successFn(data);
            }
        }
    });
}

function ChaincommonAjax(path, parameters, isasync, successFn) {
    if (path.length <= 0) {
        return;
    }

    $.ajax({
        type: 'POST',
        url: LSsynchronize + path,
        data: parameters,
        dataTpye: 'josn',
        async: isasync,
        error: function (data) {
            console.log('失败');
        },
        success: function (data) {
            var Jdata = data;
            if (data != undefined && data.length > 0) {
                Jdata = JSON.parse(data);
                successFn(Jdata);
            }
        }
    });
}


//获取登录用户信息
function getLoginUserInfo() {
    var _loginInfo = GetLocal("_loginInfo");
    if (_loginInfo.length == 0) {
        return "";
    } else {
        var data = JSON.parse(_loginInfo);
        return data;
    }
}


//获取Jason参数
function getpostParameters(actionname, parameters) {
    return {
        "actionname": actionname,
        "parameters": JSON.stringify(parameters)
    };
}

//显示操作结果
function ShowResult(data) {
    if (data != undefined) {
        if (data.status == 0) {
            layer.alert("已操作成功", { closeBtn: 0 });
        }
        else {
            layer.alert(data.mes, { closeBtn: 0 });
        }
    }
    else {
        layer.alert("操作失败，请重试或联系管理员!", { closeBtn: 0 });
    }
}

//存储
function SaveLocal(skey, sval) {
    var storage = window.localStorage;
    storage[skey] = sval;
}


function SaveSession(key, value) {
    $.session.set(key, value);
}

function RemoveSession(key) {
    $.session.remove(key);
}

function GetSession(key) {
    return $.session.get(key);
}

function ClearSession(key) {
    $.session.clear();
}

//取出
function GetLocal(key) {
    if (GetSession("_login") == undefined) {
        SaveLocal("_loginInfo", "");
        return "";
    }
    if (GetSession("_login").length > 0) {
        var storage = window.localStorage;
        var returnStr = storage[key];
        if (undefined != returnStr && returnStr.length > 0) {
            return returnStr;
        }
        else {
            return "";
        }
    } else {
        SaveLocal("_loginInfo", "");
        return "";
    }
}

//截取链接中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) {
        return unescape(decodeURI(r[2]));
    }
    return "";
}

//随机数
function random(min, max) {
    return String(Math.floor(min + Math.random() * (max - min)));
}
function gotopagebyurl(url) {
    var par = '';
    if (url.indexOf('?') < 0) {
        par = '?';
    }
    else {
        par = '&';
    }
    location.href = url + par + 'v=' + String(random(1111, 999999));
}

function getrandombyurl(url) {
    var par = '';
    if (url.indexOf('?') < 0) {
        par = '?';
    }
    else {
        par = '&';
    }
    return url + par + 'v=' + String(random(1111, 999999));
}


function convertByTime(jsonTime, format) {
    if (jsonTime != undefined) {
        var date = new Date(parseInt(jsonTime.replace("/Date(", "").replace(")/", ""), 10));
        var formatDate = date.dataformat(format);
        return formatDate;
    }
    return '';
}

//日期格式格式化
Date.prototype.dataformat = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

String.prototype.format = function (args) {
    if (arguments.length > 0) {
        var result = this;
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                var reg = new RegExp("({" + key + "})", "g");
                result = result.replace(reg, args[key]);
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] == undefined) {
                    return "";
                }
                else {
                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
        return result;
    }
    else {
        return this;
    }
}

// 验证页面表单数据
function checkAllValue(id, callback) {
    var text = '#' + id + ' input';
    var $allArray = $(text);

    var checkflag = true;
    // 进行验证
    $allArray.each(function (index, el) {
        // 取得当前 el 的 value 值
        var value = $(el).val();
        var notempty = $(el).attr('data-notempty');
        if (notempty != undefined) {
            if (notempty) {
                if (value.length == 0) {
                    $(el).addClass('requirederror');

                    checkflag = false;
                }
                else {
                    $(el).removeClass('requirederror');
                }
            }
        }

        if (value != undefined && value.length > 0) {
            // 取得当前 el 的 reg 类型
            var reg = $(el).attr('data-reg');
            if (reg != undefined) {
                var flag = checkFormat(el, reg);
                if (!flag) {
                    var placeholder = $(el).attr('placeholder');
                    layer.tips(placeholder, "#" + $(el).attr('id'));
                    checkflag = false;

                    return checkflag;
                }
            }
        }
    });
    return checkflag;
}

//验证正则表达式
function checkFormat(el, Type) {
    var Reg;
    switch (Type) {
        //用户 密码格式(不包含*,&,<,>,,",',空格)
        case 'User':
            //Reg="^([^*&<>\"\'\\s]){6,10}$";："^[a-zA-Z]\w{5,17}$
            Reg = /^([a-zA-Z\u4e00-\u9fa5]{1}[a-zA-Z\u4e00-\u9fa50-9]{2,15})$/;
            break;
        case 'Pwd':
            //Reg = /^[a-zA-Z]\w{5,10}$/;
            Reg = /^[0-9a-zA-Z_]{6,16}$/;
            break;
            //邮箱格式
        case 'Email':
            //Reg=/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
            Reg = /^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/
            break;
            //联系电话
        case 'Tel':
            Reg = /^(((1[0-9]{1}[0-9]{1}))+\d{8})$/;
            break;
            //整型
        case 'Int':
            Reg = "^([0-9]){1,}$";
            break;
            //浮点型
        case 'Float':
            Reg = "^(-?[0-9]{1,})(.[0-9]{1,4})?$";
            break;

            //金额
        case 'Decimal':
            Reg = "^(-?[0-9]{1,})(.[0-9]{1,2})?$";
            break;
            //中文汉字
        case 'Chinese':
            Reg = "^[\u4e00-\u9fa5]{2,10}$";
            break;
            //中文汉字,字母，空格
        case 'CES':
            Reg = "^[\u4e00-\u9fa5A-Za-z\\s]*$";
            break;
            //英文字母
        case 'English':
            Reg = "^[A-Z]*$";
            break;
            //英文字母及空格
        case 'EnglishO':
            Reg = "^[A-Z\\s]*$";
            break;
            //身份证号
        case 'ChineseID':
            Reg = "^([0-9]{15}|[0-9]{17}[xX0-9]{1})$";
            break;
            //日期（格式为：19yy-mm-dd）
        case 'Date':
            Reg = /^((19)|(20))(\d{2})(-)(\d{1,2})(-)(\d{1,2})$/;
            break;
            //相对路径100
        case 'UrlInfo':
            Reg = "^[a-zA-Z\/\.]{1,100}$";
            break;
        case 'IP':
            Reg = "^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[0-9]{1,2})(\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[0-9]{1,2})){3}$";
            break;
        default:
            break;

    }
    return checkRegExp(el, Reg);
}

//正则表达式验证
function checkRegExp(el, Reg) {
    var value = $(el).val();

    var re = new RegExp(Reg);
    var r = value.search(re);
    if (r === -1) {
        $(el).addClass('requirederror');
        return false;
    } else {
        $(el).removeClass('requirederror');
        return true;
    }
}

//绑定下拉框
function BindSelect() {
    var data = arguments[0];//json数据
    var bindid = arguments[1];//绑定下拉框ID
    var selval = arguments[2];//value属性
    var selname = arguments[3];//name属性
    var index = arguments[4];//添加1-“全部”，2-“无”
    var defaultval = arguments[5];//默认值
    if (data != undefined) {
        $('#' + bindid).empty();
        if (index == 1) {
            $('#' + bindid).append('<option value="">--全部--</option>');
        }
        else if (index == 2) {
            $('#' + bindid).append('<option value="">--无--</option>');
        }

        var isSelectFirst = false;
        $(data).each(function (i, o) {
            $('#' + bindid).append('<option value="' + eval('o.' + selval) + '">' + eval('o.' + selname) + '</option>');
        });

        if (defaultval != undefined && defaultval != "" && defaultval != '') {
            $('#' + bindid).val(defaultval);
        }
    }
}


function deletePic(id, showid) {
    var url = $('#' + id).val();
    if (url == "") return;
    commonAjax('IStore/WSUploadFile.ashx', getpostParameters('delete', { "path": url }), true, function (data) {
        if (data != undefined) {
            if (data.status == '0') {
                pcLayerMsg(getCommonInfo('deleteFile_ok'));
                $('#' + id).val('');
                $('#' + showid).attr('src', '');
            }
        }
    });
}


