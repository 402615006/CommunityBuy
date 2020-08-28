//ajax请求根路径
var content_cusnum = 14;
var rooturl = '';
var overall_buscode = '88888888';
var overall_comcode = 'xjwz';
var overall_stocode = '12';
//ajax请求类型
var reqtype = 1;
//跳转页面
function gotoURL(url) {
    var par = '';
    if (url.indexOf('?') < 0) {
        par = '?';
    }
    else {
        par = '&';
    }
    location.href = url + par + 'v=' + String(random(1111, 999999));
}

//url+随机数
function getURLrandom(url) {
    var par = '';
    if (url.indexOf('?') < 0) {
        par = '?';
    }
    else {
        par = '&';
    }
    return url + par + 'v=' + String(random(1111, 999999));
}

//随机数
function random(min, max) {
    return String(Math.floor(min + Math.random() * (max - min)));
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


function OnKeyUp(el) {
    if (el.value.length == 1) {
        el.value = el.value.replace(/[^1-9]/g, '')
    }
    else {
        el.value = el.value.replace(/\D/g, '')
    }
}
function OnAfterPaste(el) {
    if (el.value.length == 1) {
        el.value = el.value.replace(/[^1-9]/g, '0')
    }
    else {
        el.value = el.value.replace(/\D/g, '')
    }
}

//获取Jason参数
function getpostParameters(actionname, parameters) {
    return {
        "actionname": actionname,
        "parameters": JSON.stringify(parameters)
    };
}

//存储
function SaveLocal(skey, sval) {
    var storage = window.localStorage;
    storage[skey] = sval;
}
//取出
function GetLocal(key) {
    var storage = window.localStorage;
    var returnStr = storage[key];
    if (undefined != returnStr && returnStr.length > 0) {
        return returnStr;
    }
    else {
        return "";
    }
}
//清除
function ClearLocal() {
    var storage = window.localStorage;
    storage.clear();
}

//验证值是否为空
function checkval(values) {
    if (values == null || undefined == values || values.length <= 0) {
        return false;
    }
    else {
        return true;
    }
}
//字符串截取
String.prototype.cutstring = function (num) {
    var result = this;
    if (result.length > 0) {
        if (result.length > num) {
            return result.toString().substring(0, num) + "...";
        }
        else {
            return result;
        }
    }
}
//字符串参数化替换
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
                    var reg = new RegExp("({[" + i + "]})", "g");
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

//写cookies
function setCookie(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value);
    //document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return '';
}

//日期转字符串（yyyy-mm-dd）
function DateToString(date) {
    return String(date.getFullYear()) + '-' + ((date.getMonth() + 1 > 9) ? String(date.getMonth() + 1) : '0' + String(date.getMonth() + 1)) + '-' + ((date.getDate() > 9) ? String(date.getDate()) : '0' + String(date.getDate()));
}
/*
* Javascript base64_decode() base64解密函数
用于解密base64加密的字符串
* 吴先成  www.51-n.com ohcc@163.com QQ:229256237
* @param string str base64加密字符串
* @return string 解密后的字符串
*/
function base64_decode(str) {
    var c1, c2, c3, c4;
    var base64DecodeChars = new Array(
                        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                        -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63, 52, 53, 54, 55, 56, 57,
                        58, 59, 60, 61, -1, -1, -1, -1, -1, -1, -1, 0, 1, 2, 3, 4, 5, 6,
                        7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
                        25, -1, -1, -1, -1, -1, -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36,
                        37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1,
                        -1, -1
                );
    var i = 0, len = str.length, string = '';

    while (i < len) {
        do {
            c1 = base64DecodeChars[str.charCodeAt(i++) & 0xff]
        } while (
                                i < len && c1 == -1
                        );

        if (c1 == -1) break;

        do {
            c2 = base64DecodeChars[str.charCodeAt(i++) & 0xff]
        } while (
                                i < len && c2 == -1
                        );

        if (c2 == -1) break;

        string += String.fromCharCode((c1 << 2) | ((c2 & 0x30) >> 4));

        do {
            c3 = str.charCodeAt(i++) & 0xff;
            if (c3 == 61)
                return string;

            c3 = base64DecodeChars[c3]
        } while (
                                i < len && c3 == -1
                        );

        if (c3 == -1) break;

        string += String.fromCharCode(((c2 & 0XF) << 4) | ((c3 & 0x3C) >> 2));

        do {
            c4 = str.charCodeAt(i++) & 0xff;
            if (c4 == 61) return string;
            c4 = base64DecodeChars[c4]
        } while (
                                i < len && c4 == -1
                        );

        if (c4 == -1) break;

        string += String.fromCharCode(((c3 & 0x03) << 6) | c4)
    }
    return utf8to16(string);
}
/* base64转换
* @param string str 原始字符串
* @return string 加密后的base64字符串
*/
function base64_encode(str) {
    return encode64(utf16to8(str));
}

function utf16to8(str) {
    var out, i, len, c;

    out = "";
    len = str.length;
    for (i = 0; i < len; i++) {
        c = str.charCodeAt(i);
        if ((c >= 0x0001) && (c <= 0x007F)) {
            out += str.charAt(i);
        } else if (c > 0x07FF) {
            out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
            out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        } else {
            out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        }
    }
    return out;
}

function utf8to16(str) {
    var out, i, len, c;
    var char2, char3;

    out = "";
    len = str.length;
    i = 0;
    while (i < len) {
        c = str.charCodeAt(i++);
        switch (c >> 4) {
            case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7:
                // 0xxxxxxx
                out += str.charAt(i - 1);
                break;
            case 12: case 13:
                // 110x xxxx   10xx xxxx
                char2 = str.charCodeAt(i++);
                out += String.fromCharCode(((c & 0x1F) << 6) | (char2 & 0x3F));
                break;
            case 14:
                // 1110 xxxx  10xx xxxx  10xx xxxx
                char2 = str.charCodeAt(i++);
                char3 = str.charCodeAt(i++);
                out += String.fromCharCode(((c & 0x0F) << 12) |
                ((char2 & 0x3F) << 6) |
                ((char3 & 0x3F) << 0));
                break;
        }
    }
    return out;
}
function encode64(str) {
    var c1, c2, c3;
    var base64EncodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    var i = 0, len = str.length, string = '';

    while (i < len) {
        c1 = str.charCodeAt(i++) & 0xff;
        if (i == len) {
            string += base64EncodeChars.charAt(c1 >> 2);
            string += base64EncodeChars.charAt((c1 & 0x3) << 4);
            string += "==";
            break;
        }
        c2 = str.charCodeAt(i++);
        if (i == len) {
            string += base64EncodeChars.charAt(c1 >> 2);
            string += base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
            string += base64EncodeChars.charAt((c2 & 0xF) << 2);
            string += "=";
            break;
        }
        c3 = str.charCodeAt(i++);
        string += base64EncodeChars.charAt(c1 >> 2);
        string += base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
        string += base64EncodeChars.charAt(((c2 & 0xF) << 2) | ((c3 & 0xC0) >> 6));
        string += base64EncodeChars.charAt(c3 & 0x3F)
    }
    return string
}

//弹出隐藏层
function ShowDiv(show_div, bg_div) {
    document.getElementById(show_div).style.display = 'block';
    document.getElementById(bg_div).style.display = 'block';
    var bgdiv = document.getElementById(bg_div);
    bgdiv.style.width = document.body.scrollWidth;
    // bgdiv.style.height = $(document).height();
    $("#" + bg_div).height($(document).height());
};

//关闭弹出层
function CloseDiv(show_div, bg_div) {
    document.getElementById(show_div).style.display = 'none';
    document.getElementById(bg_div).style.display = 'none';
};

//全选事件
function CheckAll() {
    var noselect = $("body").attr("data-noselect");//此属性配置值则不加自动选择属性
    var nowFlag = document.getElementById("cbAll").checked;
    var list = $('#gv_list');
    var txtsel = $('#txtsel');//选中名称控件
    var hid_sel = $('#hid_sel');//选中value控件

    if (list != undefined) {
        var arrTr = $('#gv_list tr');
        var ctype = '';
        for (var i = 1; i < arrTr.length; i++) {
            var obj = $(arrTr[i]).find("input[type='checkbox']");
            if (nowFlag)//true 选中增加颜色，同时选中所有项ID
            {
                var num = getUrlParam("num");
                if (num > 0) {
                    var str = ($(hid_sel).val().slice($(hid_sel).val().length - 1) == ',') ? $(hid_sel).val().slice(0, -1) : $(hid_sel).val();
                    if (str.split(',').length > parseInt(num)) {
                        var hint = getCommonInfo('rerefernum');
                        hint = hint.format(num);
                        pcLayerMsg(hint);
                        break;
                        return;
                    }

                }

                $(obj).attr("checked", "checked");
                $(arrTr[i]).addClass("changecolor");
                ctype = 'add';
                if (txtsel != undefined && hid_sel != undefined) {
                    getListItemInfo(txtsel, hid_sel, ctype, arrTr[i]);
                }
            }
            else {
                if ($(arrTr[i]).hasClass('changecolor')) {
                    $(arrTr[i]).removeClass("changecolor");
                    $(obj).removeAttr("checked");
                    ctype = '';
                }
                $(txtsel).val('');
                $(hid_sel).val('');
            }
        }
    }
}

function CheckAll1() {
    var nowFlag = document.getElementById("cbAll1").checked;
    var arrChkbox = document.getElementById("gv_list1").getElementsByTagName("input");
    for (var i = 0; i < arrChkbox.length; i++) {
        if (arrChkbox[i].type == "checkbox") {
            arrChkbox[i].checked = nowFlag;
        }
    }
}

function CheckAll2() {
    var nowFlag = document.getElementById("cbAll2").checked;
    var arrChkbox = document.getElementById("gv_list2").getElementsByTagName("input");
    for (var i = 0; i < arrChkbox.length; i++) {
        if (arrChkbox[i].type == "checkbox") {
            arrChkbox[i].checked = nowFlag;
        }
    }
}

function getimagesize(img, imgw, imgh) {
    // 定义返回的大小
    var imgSize = {};
    if (img == undefined) {
        imgSize["w"] = 0;
        imgSize["h"] = 0;
        return imgSize;
    }
    //获取图片
    // 定义新的宽高
    var neww = 0;
    var newh = 0;
    // 获取原本宽高
    var imgLayerW = img.width;
    var imgLayerH = img.height;
    var newper = 0;
    var imgper = imgw * 1.0 / imgh;

    if (imgLayerW > imgw || imgLayerH > imgh) {
        newper = imgLayerW * 1.0 / imgLayerH;
        if (imgper > newper) {
            neww = imgLayerW > imgw ? imgw : imgLayerW;
            newh = parseInt(neww / (imgLayerW * 1.0 / imgLayerH));
        }
        else {
            newh = imgLayerH > imgh ? imgh : imgLayerH;
            neww = parseInt(newh / (imgLayerH * 1.0 / imgLayerW));
        }
    }
    else {
        neww = imgLayerW;
        newh = imgLayerH;
    }

    imgSize["w"] = neww;
    imgSize["h"] = newh;
    return imgSize;
}

//ajax再封装,purl:地址,param:参数json,isasync:是否异步,funback回调方法
function GpAjax(path, param, isasync, successFn) {
    if (path.length <= 0) {
        return;
    }
    $.ajax({
        type: 'POST',
        url: rooturl + path,
        data: param,
        dataTpye: 'json',
        //dataType: 'JSONP',  // 处理Ajax跨域问题
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

function GpAjaxNormal(path, param, isasync, successFn) {
    if (path.length <= 0) {
        return;
    }
    $.ajax({
        type: 'POST',
        url: rooturl + path,
        data: param,
        dataTpye: 'json',
        //dataType: 'JSONP',  // 处理Ajax跨域问题
        async: isasync,
        error: function (data) {
            console.log('失败');
        },
        success: function (data) {
            successFn(data);
        }
    });
}


//获取Jason参数
function getpostParameters(actionname, parameters) {
    return {
        "actionname": actionname,
        "parameters": JSON.stringify(parameters)
    };
}

//获取Jason参数1
function getpostParameters1(actionname, parameters) {
    return {
        "way": actionname,
        "ids": parameters
    };
}

///get 请求
function GpAjaxGet(purl, param, isasync, funback) {
    $.ajax({
        url: getURLrandom(purl + "?" + param),
        type: "get",
        dataType: "json",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        async: isasync,
        success: eval(funback)
    });
}

//字符串TrimStart
String.prototype.TrimStart = function (trim) {
    var result = this;
    if (result.length > 0) {
        var str = result.replace(trim + trim, '');
        //去掉第一个逗号
        if (str.substr(0, 1) == trim) {
            str = str.substr(1);
        }
        return str;
    }
}

//字符串TrimEnd
String.prototype.TrimEnd = function (trim) {
    var result = this;
    if (result.length > 0) {
        var str = result.replace(trim + trim, '');
        //去掉最后一个逗号
        if (str.substr(str.length - 1) == trim) {
            str = str.substr(0, str.length - 1);
        }
        return str;
    }
}

function onlyNumber(obj) {
    //得到第一个字符是否为负号
    var t = obj.value.charAt(0);
    //先把非数字的都替换掉，除了数字和. 
    obj.value = obj.value.replace(/[^\d\.]/g, '');
    //必须保证第一个为数字而不是. 
    obj.value = obj.value.replace(/^\./g, '');
    //保证只有出现一个.而没有多个. 
    obj.value = obj.value.replace(/\.{2,}/g, '.');
    //保证.只出现一次，而不能出现两次以上 
    obj.value = obj.value.replace('.', '$#$').replace(/\./g, '').replace('$#$', '.');
    //如果第一位是负号，则允许添加
    if (t == '-') {
        obj.value = '-' + obj.value;
    }
}

//合并表格重复数据
function mc(tableId, startRow, endRow, col) {
    var tb = document.getElementById(tableId);
    if (col >= tb.rows[0].cells.length) {
        return;
    }
    if (col == 0) { endRow = tb.rows.length - 1; }
    for (var i = startRow; i < endRow; i++) {
        if (tb.rows[startRow].cells[col].innerHTML == tb.rows[i + 1].cells[0].innerHTML) {
            tb.rows[i + 1].removeChild(tb.rows[i + 1].cells[0]);
            tb.rows[startRow].cells[col].rowSpan = (tb.rows[startRow].cells[col].rowSpan | 0) + 1;
            if (i == endRow - 1 && startRow != endRow) {
                mc(tableId, startRow, endRow, col + 1);
            }
        } else {
            mc(tableId, startRow, i + 0, col + 1);
            startRow = i + 1;
        }
    }
}

function timeStamp(second_time) {
    var time = parseInt(second_time) + "秒";
    if (parseInt(second_time) > 60) {
        var second = parseInt(second_time) % 60;
        var min = parseInt(second_time / 60);
        time = min + "分" + second + "秒";

        if (min > 60) {
            min = parseInt(second_time / 60) % 60;
            var hour = parseInt(parseInt(second_time / 60) / 60);
            time = hour + "小时" + min + "分" + second + "秒";

            if (hour > 24) {
                hour = parseInt(parseInt(second_time / 60) / 60) % 24;
                var day = parseInt(parseInt(parseInt(second_time / 60) / 60) / 24);
                time = day + "天" + hour + "小时" + min + "分" + second + "秒";
            }
        }
    }
    return time;
}

function conpirestring(datas, val) {
    var datastr = ',' + datas + ',';
    var valstr = ',' + val + ',';
    if (datastr.indexOf(valstr) < 0) {
        return false;
    }
    return true;
}

function openWindow(url) {
    var index = layui.layer.open({
        type: 2,
        content: url,
        area: ['100%', '100%'],
        maxmin: false,
        cancel: function () {
            location.reload();
        }
    });
    layui.layer.full(index);
}

function openCouponWindow(url) {
    var index = layui.layer.open({
        type: 2,
        content: url,
        area: ['100%', '100%'],
        maxmin: false
    });
    layui.layer.full(index);
}

//$(document).keydown(function (event) {
//    switch (event.keyCode) {
//        case 13: return false;
//    }
//});

//添加自动查找选择事件
function Addautocomplete(txtid, hintid, selctionid, valid, url, actionname, searchname) {
    $('#' + txtid).autocomplete(
                {
                    lookup: function (query, done) {
                        var parameters = { "GUID": "", "USER_ID": "0", "filter": "" };
                        parameters.filter = " where " + searchname + " like '%" + $('#' + txtid).val() + "%'";
                        $.ajax({
                            type: 'POST',
                            url: url,
                            data: getpostParameters(actionname, parameters),
                            dataTpye: 'josn',
                            async: false,
                            error: function (data) {
                                console.log('失败');
                            },
                            success: function (data) {
                                if (data != undefined && data.length > 0) {
                                    var Jdata = data;
                                    Jdata = JSON.parse(data);
                                    var result = {
                                        suggestions: $.map(Jdata.data, function (dataItem) {
                                            return { value: dataItem.value, data: dataItem.data };
                                        })
                                    };
                                    done(result);
                                }
                            }
                        });
                    }
                    ,
                    delay: 500,
                    onSelect: function (suggestion) {
                        $('#' + valid).val(suggestion.data);
                        if (location.href.indexOf('SupplierStoreList') >= 0) {
                            getStocode();
                        }
                    },
                    onHint: function (hint) {
                        $('#' + hintid).val(hint);
                    },
                    onInvalidateSelection: function () {
                        $('#' + selctionid).html('You selected: none');
                    }
                }
            );
}

function autocomonblur(id, valid) {
    if ($('#' + id).val().length == 0) {
        $('#' + valid).val('');
    }
}

function stringTrim(str, tstr) {
    var rstr = tstr + tstr;
    str = str.replace(rstr, tstr);
    if (str.length > 0) {
        if (str.substring(0, 1) == tstr) {
            str = str.substring(1, str.length - 1);
        }

        if (str.substring(str.length - 1, 1) == tstr) {
            str = str.substring(0, str.length - 1);
        }
    }
    return str;
}