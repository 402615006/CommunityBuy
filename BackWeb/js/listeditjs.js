var noload = $("body").attr("data-ischeck");

if (noload == undefined) {
    var topurl = top.location.href;
    topurl = topurl.substring(topurl.lastIndexOf('/') + 1);
    if (topurl != 'index.html') {
        top.location.href = '/index.html';
    }
}

window.onload = function () {
    if ($('.redline') != undefined) {
        $('.redline').eq(0).hide();
        console.log($('.redline'));
    }

    if ($('.righttitle') != undefined) {
        $('.righttitle').eq(0).hide();
    }
}

function checklist(type) {
    var showobj = document.getElementById('sp_showmes');
    var tbname = $("#form1").attr("data-tbname");
    var title = "";
    var s = document.getElementById('gv_list').getElementsByTagName('input');
    if (type == 'add') {
        title = "新增";
        ShowOpenpage(title, tbname + 'edit.aspx', '100%', '100%', true, true);
        return false;
    } 

    if (type == 'delete') {
        return confirm(getCommonInfo('delete_button_tip'));
    }

    var role_id = "";
    if (s != undefined) {
        for (var i = 0; i < s.length; i++) {
            if (s[i].type == 'checkbox' && s[i].checked) {
                if (s[i].id.indexOf('CB_Select') < 0) {
                    continue;
                }
                //num++;
                var table = document.getElementById('gv_list');
                var keyid = s[i + 1].id;
                role_id += $('#' + keyid).val() + ",";
            }
        }
    }
    role_id = role_id.toString().substr(0, role_id.length - 1);
    if (role_id.indexOf(',') >= 0) {
        $("#sp_showmes").html("一次只能操作一条数据");
        return false;
    }
    if (role_id == "") {
        $("#sp_showmes").html("请选择要操作的数据");
        return false;
    }
    ShowOpenpage("编辑", tbname + 'edit.aspx?type=' + type + '&id=' + role_id, '90%', '100%', true, true);
    return false;
}

$(document).ready(function () {
    if ($('.redline') != undefined) {
        $('.redline').eq(0).hide();
        console.log($('.redline'));
    }

    if ($('.righttitle') != undefined) {
        $('.righttitle').eq(0).hide();
    }

    $(".operlinkbtn").mousemove(function () {
        $(this).css("background-color", "#30a79c");
    })
    $(".operlinkbtn").mouseout(function () {
        $(this).css("background-color", "#009688");
    })
    //修改
    $(".graydiv").click(function () {
        $(".graydiv").css("background-image", "url(/img/toper1.png)");
        $(".graydiv").css("color", "#666");
        $(this).css("background-image", "url(/img/toper.png)");
        $(this).css("color", "#fff");
        $(".updatediv").hide();
        $("." + $(this).attr("title")).show();
    });
    //保存
    $(".savebtn").click(function () {
        var flag = FormDataValidationCheck();
        if (flag) {
            $("#Save_btn").click();
        }
    })
    //返回
    $(".gobackbtn").click(function () {
        //判断是否layer Open
        var _iframe = window.parent.document;
        var _btn = window.parent.document.getElementById("ToolBar1_LinkRefresh");
        if ($(_iframe).find(".layui-layer-title").attr("move") != undefined) {
            parent.layer.closeAll("iframe");
        } else {
            var tbname = $("#form1").attr("data-tbname");
            var url = tbname + "list.aspx";
            location.href = url;
        }
    });

    //返回上一级
    $(".backbtn").click(function () {
        location.href = document.referrer;
    });

    intchangecolor();
});

function auditclick(code, url, type) {
    //判断是否layer Open
    var _iframe = window.parent.document;
    if ($(_iframe).find(".layui-layer-title").attr("move") != undefined) {
        layer.alert(getNameByCode(code)
        , function () {
            if (type == "home") {
                parent.$("#btn_search").click();
            } else {
                parent.location.href = parent.location.href;
            }
            parent.layer.closeAll("iframe");
        });
    } else {
        layer.alert(getNameByCode(code)
        , function () {
            parent.location.href = url;
        });
    }
}

//Jquery 实现format 
//var text = "a{0}b{0}c{1}d\nqq{0}"; 
//var text2 = $.format(text, 1, 2); 
$.format = function (source, params) {
    if (arguments.length == 1)
        return function () {
            var args = $.makeArray(arguments);
            args.unshift(source);
            return $.format.apply(this, args);
        };
    if (arguments.length > 2 && params.constructor != Array) {
        params = $.makeArray(arguments).slice(1);
    }
    if (params.constructor != Array) {
        params = [params];
    }
    $.each(params, function (i, n) {
        source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
    });
    return source;
};

//省市区三级联动   
// 省 servercontrol  sel_provinceid  hidden hid_provinceid
// 市 servercontrol  sel_city  hidden hidcity
// 区  servercontrol  sel_area  hidden hidarea
function getprovice() {
    $.ajax({
        url: "/ajax/system/S_Province.ashx",
        type: "post",
        data: { "way": "provice" },
        dataType: "json",
        success: function (data) {
            var htmlstr = '<option value="">--全部--</option>';
            $(data.provincelist).each(function (i) {

                if (data.provincelist[i].provinceid == $("#hid_provinceid").val()) {
                    htmlstr += '<option value="' + data.provincelist[i].provinceid + '" selected="true">' + data.provincelist[i].province + '</option>';
                }
                else {
                    htmlstr += '<option value="' + data.provincelist[i].provinceid + '">' + data.provincelist[i].province + '</option>';
                }


            });
            $("#sel_provinceid").html(htmlstr);
            //特殊页面省
            if ($("#Ghostsel_provinceids").val() != undefined) {
                if ($("#Ghostsel_provinceids").val().length > 0) {
                    $("#sel_provinceid").val($("#Ghostsel_provinceids").val());
                }
            }
            getselcity();
        }
    });
}

function getselcity() {
    var pid = $("#sel_provinceid").val();
    if (pid == undefined || pid.length == 0) {
        return;
    }
    $.ajax({
        url: "/ajax/system/S_Province.ashx",
        type: "post",
        data: { "way": "city", "ids": pid },
        dataType: "json",
        success: function (data) {
            var htmlstr = '<option value="">--全部--</option>';
            $(data.citylist).each(function (i) {
                if (data.citylist[i].cityid == $("#hidcity").val()) {
                    htmlstr += '<option value="' + data.citylist[i].cityid + '" selected="true">' + data.citylist[i].city + '</option>';
                }
                else {
                    htmlstr += '<option value="' + data.citylist[i].cityid + '">' + data.citylist[i].city + '</option>';
                }
            });
            $("#sel_city").html(htmlstr);
            $("#hid_provinceid").val($("#sel_provinceid").val());
            //特殊页面市
            if ($("#Ghostsel_citys").val() != undefined) {
                if ($("#Ghostsel_citys").val().length > 0) {
                    $("#sel_city").val($("#Ghostsel_citys").val());
                }
            }
            getselarea();
        }
    });
}

function getselarea() {

    var pid = $("#sel_city").val();
    if (pid == undefined || pid.length == 0) {
        return;
    }
    $.ajax({
        url: "/ajax/system/S_Province.ashx",
        type: "post",
        data: { "way": "area", "ids": pid },
        dataType: "json",
        success: function (data) {
            var htmlstr = '<option value="">--全部--</option>';
            $(data.arealist).each(function (i) {

                if (data.arealist[i].areaid == $("#hidarea").val()) {
                    htmlstr += '<option value="' + data.arealist[i].areaid + '" selected="true">' + data.arealist[i].area + '</option>';
                }
                else {
                    htmlstr += '<option value="' + data.arealist[i].areaid + '">' + data.arealist[i].area + '</option>';
                }
            });
            $("#sel_area").html(htmlstr);
            $("#hidcity").val($("#sel_city").val());
            //特殊页面区
            if ($("#Ghostsel_areas").val() != undefined) {
                if ($("#Ghostsel_areas").val().length > 0) {
                    $("#sel_area").val($("#Ghostsel_areas").val());
                }
            }
        }
    });
}

function getSelect(elemid, hidelemid, name, value, url, data) {
    $.ajax({
        url: url,
        type: "post",
        data: data,//{ "way": "area", "ids": pid },
        dataType: "json",
        success: function (data) {
            var htmlstr = '<option value="-1">--请选择--</option>';
            $(data).each(function (i) {

                if (data[i][value] == $("#" + hidelemid).val()) {
                    htmlstr += '<option value="' + data[i][value] + '" selected="true">' + data[i][name] + '</option>';
                }
                else {
                    htmlstr += '<option value="' + data[i][value] + '">' + data[i][name] + '</option>';
                }
            });
            $("#" + elemid).html(htmlstr);
        }
    });
}

function setarea() {
    $("#hidarea").val($("#sel_area").val());
}

//显示大图片 w h 为可选参数，默认为100%
function showbigpic(img, w, h) {

    var width = arguments[1] ? arguments[1] : "100%";
    var height = arguments[2] ? arguments[2] : "100%";
    showlayer('/Showbigpic.html?url=' + img.src, '查看大图', width, height, 2);
}

function AddSaveAddbutton(id) {
    $("#" + id).parent().find("div").eq(0).after('<div class="addbtn" onclick="FormDataSaveAdd(\'' + id + '\');">保存并新建</div>');
}

function FormDataSaveAdd(id) {
    var flag = FormDataValidationCheck();
    if (flag) {
        $("#" + id + "").click();
        var obj = $("#hidId");
        if (obj != undefined) {
            if (obj.val().length > 0) {
                pcLayerMsg('数据已保存成功！');
                setTimeout(function () {
                    var a = '';
                }, 3000);
                location.href = location.href;
            }
        }
    }
}

function intchangecolor() {
    var noselect = $("body").attr("data-noselect");//此属性配置值则不加自动选择属性
    var nochangecolor = $("body").attr("data-nochangecolor");
    if (nochangecolor == undefined) {
        if (noselect == undefined) {
            $('.selectbtn').css('display', 'none');
        }
        var list = $('#gv_list');
        var txtsel = $('#txtsel');//选中名称控件
        var hid_sel = $('#hid_sel');//选中value控件
        if (list != undefined) {
            $('#gv_list tr').click(function () {
                var obj = $(this).find("input[type='checkbox']");
                var ctype = '';
                if ($(this).hasClass('changecolor')) {
                    $(this).removeClass("changecolor");
                    $(obj).removeAttr("checked");
                    ctype = '';
                } else {
                    if (noselect == undefined) {
                        var num = getUrlParam("num");
                        if (num > 0) {
                            var str = ($(hid_sel).val().slice($(hid_sel).val().length - 1) == ',') ? $(hid_sel).val().slice(0, -1) : $(hid_sel).val();
                            if (str.split(',').length > parseInt(num)) {
                                var hint = getCommonInfo('rerefernum');
                                hint = hint.format(num);
                                pcLayerMsg(hint);
                                return;
                            }
                        }
                    }
                    $(this).addClass("changecolor");
                    $(obj).attr("checked", "checked");
                    ctype = 'add';
                }
                if (noselect == undefined) {
                    if (txtsel != undefined && hid_sel != undefined) {
                        getListItemInfo(txtsel, hid_sel, ctype, this);
                    }
                }
            });
        }
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

function getListItemInfo(txtobj, valobj, ctype, thisobj) {
    var txtname = $(txtobj).val();//result names
    var txtval = $(valobj).val();//result 
    var dataselect = $("body").attr("data-select");
    if (txtname == undefined || txtval == undefined) {
        return;
    }
    var s = $(thisobj).find('input[type=hidden]');
    var val = '';
    var cname = '';
    if (s != undefined) {
        if (s.length >= 1) {
            val = $(s[0]).val();
        }

        if (s.length >= 2) {
            if (dataselect != undefined) {
                cname = $(s[0]).val() + " " + $(s[1]).val();
            } else {
                cname = $(s[1]).val();
            }
        }
    }

    if (ctype == 'add') {
        if (val.length > 0)//选中项值不为空
        {
            if ((',' + txtval).indexOf("," + val + ",") < 0) {
                if (txtval.lastIndexOf(',') != txtval.length - 1) {
                    txtval = txtval + ",";
                }
                txtval += val + ",";
                txtname += "," + cname;
            }
        }
    }
    else {
        if (val.length > 0)//选中项值不为空
        {
            if ((',' + txtval).indexOf("," + val + ",") >= 0) {
                txtval = (',' + txtval).replace(',' + val + ',', ',');
                txtname = (',' + txtname + ',').replace(',' + cname + ',', ',');
            }
            if (txtval == ',') {
                txtval = '';
                txtname = '';
            }
        }
    }

    txtname = txtname.replace(',,', ',');
    txtval = txtval.replace(',,', '');
    txtname = (txtname.slice(txtname.length - 1) == ',') ? txtname.slice(0, -1) : txtname;
    txtval = (0 == txtval.indexOf(',') ? txtval.substr(1) : txtval);
    $(txtobj).val(txtname);
    $(valobj).val(',' + txtval);
}
