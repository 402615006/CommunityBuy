var intervalTime = 10000; //定时查询时间 5000毫秒(可修改,建议5秒以上)
var htmlcontent = "";
window.onresize = function () {
    $(".mainleft").height($(window).height() - 60);
    $("#btnlist").height($(window).height() - 245);//260
    $(".mainright").width($(window).width() - 220);
    $(".mainright").height($(window).height() - 95);
    $(".rightcontent").height($(window).height() - 217);
}
function gotologinout() {
    location.href = "/logout.aspx";
}

function confirmlogout() {
    layconfirm(getNameByCode('loginout_prompt'), 'gotologinout');
}

$(document).ready(function () {
    //TimingQuery();
    //设置初始
    if ($(window).width() > 1050) {
        $(".mainleft").height($(window).height() - 93);
        $("#btnlist").height($(window).height() - 248);
        $(".mainright").width($(window).width() - 220);
        $(".mainright").height($(window).height() - 95);
        $(".rightcontent").height($(window).height() - 212);
    }

    $.ajax({
        url: "/ajax/getuser.ashx",
        type: "post",
        data: "",
        dataType: "json",
        success: function (data) {
            $("#showname").html(data.uname);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            parent.location.href = "Login.aspx";
        }
    });

    $.ajax({
        url: "/ajax/toplist.ashx",
        type: "post",
        data: "",
        dataType: "json",
        success: function (data) {
            var html = "<div class=\"head_div home\" tid=\"home\"><img src=\"/img/menu/home.png\" /><p style='padding-top:4px;'>首页</p></div>"
            $(data).each(function (i) {
                html += "<div class=\"head_div\"  tid=\"" + data[i].id + "\"><img src=\"" + data[i].imgname + "\" /><p style='padding-top:4px;'>" + data[i].cname + "</p></div>"
            });
            $(".headList").html(html);
            clicktop();
            //$(".headList").children().eq(0).click(); //模拟点击首页事件
        }
    });

    //tabClose();
    //tabCloseEven();
})
function searchmess() {

}
//显示隐藏
function shrink() {
    htmlcontent = $(".mainleft").html();
    $(".mainleft").html("<div class=\"leftclose\"><img class=\"popcircle\" src=\"/img/Together1.png\" /></div>")
    $(".mainleft").width(20);
    $(".mainright").width($(window).width() + 220);
    $(".mainright").css("left", "20px");
    $(".layout-panel-west").width(20);
    $(".layout-panel-center").css("left", "20px");

    $(".layout-panel-center").width($(window).width() - 20);
    $(".tabs-container").width($(window).width() - 20);
    $(".tabs-header-noborder").width($(window).width() - 20);
    $(".tabs-wrap").width($(window).width() - 20);
    $(".tabs-panels-noborder").width($(window).width() - 20);
    $('.tabs-panels-noborder div').each(function (i) {
        $(this).width($(window).width() - 20);
    });
    //$(".").width($(window).width() + 220);

    $(".popcircle").click(function () {
        $(".mainleft").width(220);
        $(".mainleft").html(htmlcontent);
        $(".mainright").width($(window).width() - 220);
        $(".mainright").css("left", "0px");
        $(".layout-panel-west").width(220);
        $(".layout-panel-center").css("left", "220px");

        $(".layout-panel-center").width($(window).width() - 220);
        $(".tabs-container").width($(window).width() - 220);
        $(".tabs-header-noborder").width($(window).width() - 220);
        $(".tabs-wrap").width($(window).width() - 220);
        $(".tabs-panels-noborder").width($(window).width() - 220);
        $('.tabs-panels-noborder div').each(function (i) {
            $(this).width($(window).width() - 220);
        });
    })
}
//top点击变色
function clicktop() {
    $(".head_div").click(function () {
        $(".head_div").css("background-color", "#2c344c");
        $(this).css("background-color", "#3f4461");
        if ($(this).attr("tid") == "home") {

        }
        else {
            var headid = $(this).attr("tid");
            var html = "";
            $.ajax({
                url: "/ajax/leftlist.ashx",
                type: "post",
                data: { "parid": headid },
                dataType: "json",
                success: function (data) {
                    $(data).each(function (i) {
                        html += "<div class=\"mainbtn mainbtn" + i + "\" onclick=\"clicklefthead(" + i + ",'" + data[i].id + "')\">" + data[i].cname + "</div><div class=\"sec" + i + " thire\"></div>";//二级菜单
                    });
                    $("#btnlist").html(html);
                    $(".mainbtn0").css("background-image", "url(/img/up.png)");
                    if (data.length > 0) {
                        fistlist(0, data[0].id);
                    }
                }
            });
        }
    });
}

var chooseimg = "<div id='imgshow' class='imgshow'  ><img src='img/redpoint.png' style='vertical-align: middle;' /></div>";
//选中第一条
function fistlist(num, pid) {
    var html = "";
    $.ajax({
        url: "/ajax/leftlist.ashx",
        type: "post",
        data: { "parid": pid },
        dataType: "json",
        success: function (redata) {
            $(".mainbtn" + num).css("background-color", "#3f4461");
            $(".mainbtn" + num).css("color", "#fff");
            if (redata.length > 0) {
                $(".subclass").css("background-color", "#fff");
                $(".subclass").css("color", "#6e6e6e");
                $(redata).each(function (i) {
                    var numhtml = "";
                    var imghtml = "";
                    if (i == 0) {
                    }
                    html += "<div class=\"subclass\" onclick='clickleftmenu(this);'  " + numhtml + " tid=\"" + redata[i].url + "\"> " + imghtml + redata[i].cname + "   </div>";
                });
                $(".sec" + num).html(html);
                $(".sec" + num).css("display", "block");

            }
        },
        complete: function () {
            //clickleft();
        }
    });
}

function clickleftmenu(obj) {
    $("#btnlist .subclass").css("background-color", "#fff");
    $("#btnlist .subclass").css("color", "#6e6e6e");
    $("#btnlist .subclass div.imgshow").remove();
    $(obj).css("color", "#c02827");
    $(obj).prepend(chooseimg);
    //$("#frame").attr("src", $(obj).attr("tid"));
    addTab(trim($(obj).text()), $(obj).attr("tid"));
}

function trim(str) { //删除左右两端的空格
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

//回收，释放
function clicklefthead(num, id) {
    $(".thire").hide();
    var imgsrc = $(".mainbtn" + num).css("background-image");
    var sechtml = $(".sec" + num).html();
    $(".mainbtn").css("background-image", "url(/img/down.png)");
    $(".mainbtn").css("background-color", "#fff");
    $(".mainbtn").css("color", "#6e6e6e");
    if (imgsrc.indexOf("/img/down.png") >= 0) {
        $(".mainbtn" + num).css("background-image", "url(/img/up.png)");
        $(".mainbtn" + num).css("background-color", "#3f4461");
        $(".mainbtn" + num).css("color", "#fff");
        if (undefined != sechtml && sechtml.length > 0) {
            $(".sec" + num).show();
        }
        fistlist(num, id)
    }
    else {
        $(".mainbtn" + num).css("background-image", "url(/img/down.png)");
        $(".sec" + num).hide();
    }
}

var TimingInterVal;
//定时查询
function TimingQuery() {
    TimingInterVal = setInterval(OpenInterval, intervalTime); //开启定时器
}



var _layerMsgStatus = 0;
var _datas;
//右下角消息提醒 param:title 如果title为空则表头显示消息提示,传值表头为所传内容
function OpenMsg(title, data) {
    clearInterval(TimingInterVal); //清除定时器
    _layerMsgStatus = 0;
    var tempHtml = '<div style="width:90%; margin:5px 5px 5px 15px;">';
    tempHtml += '<p style="text-align:left; cursor:pointer;text-decoration:underline;" data-type="' + data[0].type + '" onclick="OpenMsgJump($(this))">' + "您有<span style='color:#FF8C00;font-weight:bold;'>" + data.length + "</span>条未读审批消息" + '</p>';
    //for (var i = 0; i < data.length; i++) {
    //    tempHtml += '<p style="text-align:left; cursor:pointer;" data-type="' + data[i].type + '" onclick="OpenMsgJump($(this))">' + (i + 1) + "." + data[i].content + '</p>';
    //}
    tempHtml += "</div>";
    //边缘弹出
    layer.open({
        type: 1
      , title: [title, 'text-align:center']
      , area: ['190px', '70px']
      , offset: 'rb' //具体配置参考：offset参数项
      , content: tempHtml//'<div style="width:90%; margin:10px 5% 10px 5%;"><p style="text-align:left;">' + data.content + '</p></div>'
      , shade: 0 //不显示遮罩
      , yes: function () {
          layer.closeAll();
      }, cancel: function () {
          _layerMsgStatus = 1;
          TimingQuery();
      }
    });
}

//点击未读消息跳转并更新数据
function OpenMsgJump(el) {
    var type = $(el).attr("data-type");
    TimingQuery();
    if (type != undefined && _datas != undefined) {
        if (type == "0") {
            $(".headList").children().eq(0).click(); //模拟点击首页事件
            layer.closeAll();
            updateMsgStatus("0", _datas);
            //$("#frame").attr("src", "../home/StockApproval.aspx");
        } else {
            $(".headList").children().eq(0).click(); //模拟点击首页事件
            layer.closeAll();
            $(".subclass").eq(1).click();
            updateMsgStatus("1", _datas);
            //$("#frame").attr("src", "../home/CardApproval.aspx");
        }
    }
}

//更新消息状态
function updateMsgStatus(type, data) {
    $.ajax({
        type: "post",
        url: "../ajax/AudMes.ashx?t=" + Math.random(),//请求地址 
        data: {//设置数据源 
            "actionname": "update",
            "auids": GetUpdateAuid(type, data)
        },
        dataType: "text",// 设置需要返回的数据类型 
        success: function (jsonstr) {

        },
        error: function (err) {

        }
    });
}

//根据类型获取唯一标识并用逗号分割
function GetUpdateAuid(type, data) {
    var tempAuid = "";
    if (data != undefined) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].type == type) {
                if (tempAuid.length > 0) {
                    tempAuid += "," + data[i].auid;
                } else {
                    tempAuid += data[i].auid;
                }
            }
        }
    }
    return tempAuid;
}

function addTab(subtitle, url) {
    if (!$('#tabs').tabs('exists', subtitle)) {
        $('#tabs').tabs('add', {
            title: subtitle,
            content: createFrame(url),
            closable: true,
            width: $('#mainPanle').width() - 10,
            height: $('#mainPanle').height() - 26
        });
    } else {
        $('#tabs').tabs('select', subtitle);
    }
    tabClose();
}

function createFrame(url) {
    var s = '<iframe name="mainFrame" scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:98%;"></iframe>';
    return s;
}

function tabClose() {
    /*双击关闭TAB选项卡*/
    $(".tabs-inner").dblclick(function () {
        var subtitle = $(this).children("span").text();
        $('#tabs').tabs('close', subtitle);
    })

    $(".tabs-inner").bind('contextmenu', function (e) {
        $('#mm').menu('show', {
            left: e.pageX,
            top: e.pageY
        });

        var subtitle = $(this).children("span").text();
        $('#mm').data("currtab", subtitle);

        return false;
    });
}
//绑定右键菜单事件
function tabCloseEven() {
    //关闭当前
    $('#mm-tabclose').click(function () {
        var currtab_title = $('#mm').data("currtab");
        $('#tabs').tabs('close', currtab_title);
    })
    //全部关闭
    $('#mm-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            $('#tabs').tabs('close', t);
        });
    });
    //关闭除当前之外的TAB
    $('#mm-tabcloseother').click(function () {
        var currtab_title = $('#mm').data("currtab");
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            if (t != currtab_title)
                $('#tabs').tabs('close', t);
        });
    });
    //关闭当前右侧的TAB
    $('#mm-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            //msgShow('系统提示','后边没有啦~~','error');
            alert('后边没有啦~~');
            return false;
        }
        nextall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            $('#tabs').tabs('close', t);
        });
        return false;
    });
    //关闭当前左侧的TAB
    $('#mm-tabcloseleft').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        if (prevall.length == 0) {
            alert('到头了，前边没有啦~~');
            return false;
        }
        prevall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            $('#tabs').tabs('close', t);
        });
        return false;
    });
}

