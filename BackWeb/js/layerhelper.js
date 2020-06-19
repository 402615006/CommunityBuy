/*
描述：layer插件封装
作者：CGD
时间：2016-11-18
*/

/*
    功能：confirm询问框
    参数：par1：提示信息，par2：回调函数名，par3：确认按钮名称，par4：取消按钮名称
*/
function layconfirm() {
    var msg = arguments[0];
    var fun = arguments[1];
    var but1 = arguments[2];
    var but2 = arguments[3];
    if (msg == undefined) {
        return;
    }
    if (but1 == undefined) {
        but1 = getCommonInfo('sure_layer_confirm');
    }
    if (but2 == undefined) {
        but2 = getCommonInfo('cancel_layer_confirm');
    }
    var title = getCommonInfo('title_layer_confirm');
    layer.confirm(msg + '？', {
        title: [title],
        btn: [but1, but2] //按钮
    }, function () {
        if (fun != undefined && fun.length > 0) {
            eval(fun + "();");
        }
    }, function () {
    });
}
/*
    功能：提示框
    参数：par1：#控件名或.css名称，par2：提示信息，par3：显示位置默认2（1-上，2-右，3-下，4-左），par4：背景颜色
*/
function laytips() {
    var id = arguments[0];
    if (id == undefined) {
        return;
    }
    var msg = arguments[1];
    if (msg == undefined) {
        return;
    }
    var position = arguments[2];
    if (position == undefined) {
        position = 2;
    }
    var bgcolor = arguments[3];
    if (bgcolor == undefined) {
        bgcolor = '#70789e';
    }
    //tips层-上
    layer.tips(msg, id, {
        tips: [position, bgcolor] //颜色
    });

    //layer.tips('tips的样式并非是固定的，您可自定义外观。', id, { 
    //    style: ['background-color:#78BA32; color:#fff', '#78BA32'],  
    //    maxWidth:185,  
    //    time: 3,  
    //    closeBtn:[0, true]  
    //});     
}

/*
    功能：弹出信息提示框
    参数：par1：提示信息，par2：图标类型1-9，par3：回调函数
*/
function pcLayerMsg() {
    var str = arguments[0];
    var status = arguments[1];
    if (status == undefined) {
        status = 1;
    }
    var funback = arguments[2];
    if (funback == undefined) {
        layer.msg(str, {
            icon: status,
            time: 3000
        });
    }
    else {
        layer.msg(str, {
            icon: 1,
            time: 3000, //5s后自动关闭
            end: funback
        });
    }
}

function pcLayerMsg2() {
    var str = arguments[0];
    var status = arguments[1];
    if (status == undefined) {
        status = 1;
    }
    var funback = arguments[2];
    if (funback == undefined) {
        layer.msg(str, {
            icon: status,
            time: 1000
        });
    }
    else {
        layer.msg(str, {
            icon: 1,
            time: 1000, //5s后自动关闭
            end: funback
        });
    }
}
/*
    功能：弹出参照页面
    参数：par1：value赋值控件ID，par2：名称赋值控件ID，par3：参照页标题，par4：参照页地址，par5：选择限制数量，par6：显示宽度，par7：显示高度
*/
function ShowReferPage() {
    var id = arguments[0];
    var name = arguments[1];
    var title = arguments[2];
    var linkurl = arguments[3];
    var choosenum = arguments[4];
    var showw = arguments[5];
    var showh = arguments[6];
    var formpage = arguments[7];
    var warcode = arguments[8];
    var templatetype = arguments[9];
    if (id == undefined || linkurl == undefined || name == undefined) {
        return;
    }

    var ids = $("#" + id).val();
    if (ids == undefined) {
        ids = "";
    }
    if (choosenum == undefined) {
        choosenum = 1;
    }
    if (formpage == undefined) {
        formpage = '';
    }
    if (warcode == undefined) {
        warcode = '';
    }
    if (templatetype == undefined) {
        templatetype = '';
    }
    if (linkurl.indexOf('?') < 0) {
        par = '?';
    }
    else {
        par = '&';
    }
    var linkstr = linkurl + par + 'id=' + ids + '&valid=' + id + '&nameid=' + name + '&num=' + choosenum + '&formpage=' + formpage + '&warcode=' + warcode + '&templatetype=' + templatetype;

    if (title == undefined) {
        title = '选择信息';
    }

    if (showw == undefined) {
        showw = '80%';
    }

    if (showh == undefined) {
        showh = '80%';
    }
    var index = layer.open({
        title: title,
        type: 2,
        shade: 0.6,
        maxmin: true,
        area: [showw, showh],
        content: linkstr
    });
    //layer.full(index);
}


function ShowOpenpage() {
    var title = arguments[0];
    var linkurl = arguments[1];
    var showw = arguments[2];
    var showh = arguments[3];
    var isfull = arguments[4];
    var isreload = arguments[5];

    if (linkurl == undefined) {
        return;
    }

    var linkstr = getrandombyurl(linkurl);

    if (title == undefined || title.length == 0) {
        title = false;
    }

    if (showw == undefined) {
        showw = '85%';
    }

    if (showh == undefined) {
        showh = '80%';
    }

    if (isfull == undefined) {
        isfull = false;
    }
    if (isreload == undefined) {
        isreload = false;
    }

    var index = layer.open({
        title: title,
        type: 2,
        shade: 0.6,
        area: [showw, showh],
        content: encodeURI(linkstr),
        cancel: function () {
            if (isreload) {
                var _btn = document.getElementById("ToolBar1_LinkRefresh");
                if (_btn != undefined) {
                    //_btn.click();
                }
            }
        }
    });
    if (isfull) {
        layer.full(index);
    }
}

function ShowOpenpage1() {
    var title = arguments[0];
    var linkurl = arguments[1];
    var showw = arguments[2];
    var showh = arguments[3];
    var isfull = arguments[4];
    var isreload = arguments[5];

    if (linkurl == undefined) {
        return;
    }

    var linkstr = getrandombyurl(linkurl);

    if (title == undefined || title.length == 0) {
        title = false;
    }

    if (showw == undefined) {
        showw = '85%';
    }

    if (showh == undefined) {
        showh = '80%';
    }

    if (isfull == undefined) {
        isfull = false;
    }
    if (isreload == undefined) {
        isreload = false;
    }

    var index = layer.open({
        title: title,
        type: 2,
        shade: 0.6,
        area: [showw, showh],
        closeBtn: 0,
        content: encodeURI(linkstr),
        cancel: function () {
            if (isreload) {
                var _btn = document.getElementById("ToolBar1_LinkRefresh");
                if (_btn != undefined) {
                    //_btn.click();
                }
            }
        }
    });
    if (isfull) {
        layer.full(index);
    }
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
//随机数
function random(min, max) {
    return String(Math.floor(min + Math.random() * (max - min)));
}

/*
    功能：弹出参照页面
    参数：par1：value赋值控件ID，par2：名称赋值控件ID，par3：参照页标题，par4：参照页地址，par5：选择限制数量，par6：显示宽度，par7：显示高度
*/
function ShowReferPagebycoupon() {
    var id = arguments[0];
    var name = arguments[1];
    var title = arguments[2];
    var linkurl = arguments[3];
    var choosenum = arguments[4];

    var selectnum = arguments[5];

    if (id == undefined || linkurl == undefined || name == undefined) {
        return;
    }

    var ids = $("#" + id).val();
    if (ids == undefined) {
        ids = "";
    }
    if (choosenum == undefined) {
        choosenum = 1;
    }
    var linkstr = linkurl + '?id=' + ids + '&valid=' + id + '&nameid=' + name + '&num=' + choosenum + '&selectnum=' + selectnum;;

    if (title == undefined) {
        title = '选择信息';
    }


    var index = layer.open({
        title: title,
        type: 2,
        shade: 0.6,
        maxmin: true,
        area: ['80%', '80%'],
        content: linkstr
    });
    //layer.full(index);
}


/*
    功能：大图显示控件
    
*/
function showlayer(url, title, wper, hper, type) {
    var index = layer.open({
        title: title,
        type: type,
        area: [wper, hper],
        fix: true, //不固定
        maxmin: false,
        content: url
    });
    return false;
}