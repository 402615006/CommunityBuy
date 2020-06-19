//获取页面参数
var type = getUrlParam('type'); //操作类型
var discode = getUrlParam('discode');//主键
var stocode = getUrlParam('stocode');
var mlv = getUrlParam('mlv');

// API参数配置
var interfaceUrl = '../../ajax/dishes/WSdishes.ashx';
var interfaceTypeUrl = '../../ajax/dishes/WSDisheType.ashx';

//详情接口请求默认参数
var detailDishesParameters = {
    GUID: "57d99d89-caab-482a-a0e9-a0a803eed3ba",
    USER_ID: getLoginUserInfo().userid,
    discode: discode,
    stocode: stocode
};

//用于所属分类名称请求默认参数
var detailDishesTypeParameters = {
    GUID: getLoginUserInfo().GUID,
    USER_ID: getLoginUserInfo().userid,
    distypecode: ''
};

// 查询请求参数
var getListParameters = {
    "GUID": getLoginUserInfo().GUID,
    "USER_ID": getLoginUserInfo().userid,
    "pageSize": 10,
    "currentPage": 1,
    "filter": "",
    "order": "",
    "roleid": ""
};


//添加请求默认参数
var addDishesParameters = {
    GUID: getLoginUserInfo().GUID,
    USER_ID: getLoginUserInfo().userid,
    buscode: getLoginUserInfo().buscode,
    stocode: stocode,
    disid: '',
    discode: discode,
    disname: '',
    disothername: '',
    distypecode: '',
    quickcode: '',
    customcode: '',
    unit: '',
    price: '0',
    memberprice: '0',
    ismultiprice: '0',
    costprice: '0',
    iscostbyingredient: '',
    pushmoney: '',
    matclscode: '',
    matcode: '',
    extcode: '',
    fincode: '',
    dcode: '',
    kitcode: '',
    ecode: '',
    maketime: '0',
    qrcode: '',
    dispicture: '',
    remark: 1,
    isentity: '0',
    entitydefcount: 0,
    entityprice: 0,
    isdelete: "0",
    iscanmodifyprice: '0',
    isneedweigh: '0',
    isneedmethod: '0',
    iscaninventory: '0',
    iscancustom: '0',
    iscandeposit: '0',
    isallowmemberprice: '0',
    isattachcalculate: '0',
    isclipcoupons: '0',
    iscombo: '0',
    iscombooptional: '0',
    isnonoperating: '0',
    status: '1',
    busSort: '1',
    warcode: '',
    cuser: getLoginUserInfo().userid,
    uuser: getLoginUserInfo().userid,
    dishesMethodsJson: '',
    dishesMatesJson: '',
    dishesMealsJson: '',
    dishescombosJson: '',
    dishesoptionalsJson: '',
    melcode: ''
};

//绑定所属仓库
function bindwarcode() {
    getListParameters.pageSize = 1000;
    getListParameters.filter = " 1=1 and stocode='" + $("#stocode").val() + "'";
    commonAjax('../../ajax/dishes/WSStockWareHouse.ashx', getpostParameters('getlist', getListParameters), false, function (data) {
        if (data != undefined && data.data != undefined) {
            BindSelect(data.data, 'sel_warcode', 'warcode', 'warname', 2);
        }
    });
}

function BindMealInfo(index) {
    getListParameters.pageSize = 1000;
    getListParameters.filter = " 1=1 and stocode='" + $("#stocode").val() + "'";
    commonAjax('../../ajax/dishes/WSmeal.ashx', getpostParameters('getlist', getListParameters), true, function (data) {
        if (data != undefined && data.data != undefined) {
            if (type.length == 0) {
                BindSelect(data.data, 'sel_meal', 'melcode', 'melname', 0);
            } else {
                BindSelect(data.data, 'sel_meal', 'melcode', 'melname', index);
            }
        }
    });
}

//绑定下拉框
function BindSelect1() {
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
        if (stocode != null && stocode != "" && stocode != undefined) {
            $('#' + bindid).val(stocode);
        }
    }
}

function BindDepartmentInfo1(index) {
    getListParameters.pageSize = 1000;
    getListParameters.filter = "";
    commonAjax('../../ajax/dishes/WSStore.ashx', getpostParameters('getlist', getListParameters), false, function (data) {
        if (data != undefined && data.data != undefined) {
            if (type.length == 0) {
                BindSelect1(data.data, 'stocode', 'stocode', 'sname', 1);
            } else {
                BindSelect1(data.data, 'stocode', 'stocode', 'sname', index);
            }
        }
    });
}

var typeid = 'save';
//保存并新建
function saveaddclick() {
    typeid = 'saveadd';
    savego();
}

//保存
function saveclick() {
    typeid = 'save';
    savego();
}

// 绑定保存事件
function savego() {
    var stopSave = checkAllValue('tabContent');
    if (!stopSave) {
        return;
    }
    stopSave = getUserAddDishesData(addDishesParameters);
    //if ($("#multiprice").hasClass("active")) {
    //    if (addDishesParameters.dishesMealsJson.length == 2) {
    //        layer.alert("当多菜谱开启时请至少选择一个多菜谱", { closeBtn: 0 });
    //        return;
    //    }
    //}
    //if (!stopSave) {
    //    return;
    //}

    //if ($("#seldcode").val() == "" || $("#seldcode").val() == undefined) {
    //    layer.alert("请选择出品部门", { closeBtn: 0 });
    //    return false;
    //}

    //if ($("#selkitcode").val() == "" || $("#selkitcode").val() == undefined) {
    //    layer.alert("请选择制作厨房", { closeBtn: 0 });
    //    return false;
    //}

    //if ($("#sel_warcode").val() == "" || $("#sel_warcode").val() == undefined) {
    //    layer.alert("请选择所属仓库", { closeBtn: 0 });
    //    return false;
    //}
    ///*
    //var tempcaninventory = $("#iscaninventory").hasClass("active") ? 1 : 0;
    //var costByingRedient = $("#costByingRedient").hasClass("active") ? 1 : 0;
    //if (costByingRedient == 1 || tempcaninventory == 1) {
    //    if ($("#sel_warcode").val() == "" || $("#sel_warcode").val() == undefined) {
    //        layer.msg("请选择所属仓库");
    //        return false;
    //    }
    //}
    //*/

    //var iscontinue = true;
    //for (var i = 0; i < $("select").length; i++) {
    //    if ($("select").eq(i).find("option:selected").val() == undefined || $("select").eq(i).find("option:selected").val() == "null" || $("select").eq(i).find("option:selected").val() == null) {
    //        var content = $("select").eq(i).parent().find("label").text();
    //        layer.alert("请选择" + content.substring(0, content.length - 1), { closeBtn: 0 });
    //        iscontinue = false;
    //        break;
    //    }
    //}

    //if (!iscontinue) {
    //    return;
    //}

    if (detailDishesParameters.discode.length > 0) {
        // 更新            
        addDishesParameters.discode = detailDishesParameters.discode;
        addDishesParameters.status = "1";
        commonAjax(interfaceUrl, getpostParameters('update', addDishesParameters), true, savecallback);
    }
    else {
        commonAjax(interfaceUrl, getpostParameters('add', addDishesParameters), true, savecallback);
    }
}

function stoSelect() {
    BindMealInfo(2);

    //绑定财务类别
    BindFinanceTypeInfo(0);

    //绑定出品部门
    BindDepartmentInfo(2);

    //绑定制作厨房
    BindKitchenInfo(2);
    bindwarcode();
    //BindEmployeeInfo(2);
    getEnumOptionInfo(SystemEnum.PriceWay, 0, '', setPriceWayInfo);
    getDictOptionInfo(SystemDict.Unit, 0, 'zh-cn', setUnitInfoInfo);
}

//初始化函数
$(document).ready(function () {
    //绑定单位下拉框
    //BindUnitInfo(2);
    BindDepartmentInfo1(1);

    //绑定物料类别
    //commonAjax('', getpostParameters('update', addDishesParameters), true, savecallback);

    // 选项卡事件
    $('#tabNav').on('click', 'li', function (e) {
        // 获取 tab nav 的第几项
        var index = $(e.currentTarget).index();

        // 切换 tab content 内容
        var $liNavArr = $('#tabNav>li');
        var $liContentArr = $('#tabContent>li');
        // 重置 tab nav
        $liNavArr.removeClass();
        $($liNavArr[index]).addClass('active');
        // 重置 tab content
        $liContentArr.hide();
        $($liContentArr[index]).show();
    });

    // 选择与取消事件
    $('.radio').on('click', function (e) {
        toggleTabNav(e.target);
    });

    // other选项绑定事件
    $('#chooseList').on('click', 'button', function (e) {
        $(e.target).toggleClass('active');
    });

    $("#uploadify").uploadify({
        uploader: '/js/jquery.uploadify/uploadify.swf',
        cancelImg: '/js/jquery.uploadify/cancel.png',
        script: '../ajax/dishes/WSUploadFile.ashx',
        method: 'POST',
        scriptData: { "actionname": "dishesupload", "parameters": "{}" },
        //folder: '../uploads',
        queueID: 'showuploadsrc',
        simUploadLimit: 1,//允许同时上传的个数
        buttonImg: '/images/selectimg.png',
        wmode: 'transparent',
        auto: true,
        fileTypeExts: '*.jpg; *.png; *.gif',
        fileSizeLimit: '2MB',
        onComplete: function (e, q, f, data, d) {
            if (data != undefined) {
                var Jdata = JSON.parse(data);
                if (Jdata.status == '0') {
                    $("#uploadsrc").val(Jdata.mes);
                    $("#upimg").attr("src", VRootPath + Jdata.mes);
                    $("#showuploadsrc").show();
                }
                else {
                    layer.msg(Jdata.mes);
                    $("#showuploadsrc").hide();
                }
            }
        },
        onCancel: function (file) {
            deletePic('uploadsrc', 'upimg');
            //$("#upimg").attr("src", "");
        }
    });

    $("#multiprice").click(function () {
        if ($("#multiprice").hasClass("active")) {
            $("#price").removeAttr("data-notempty");
            $("#price").removeClass("empty");
            $("#price").removeClass("requirederror");
            $("#memberprice").removeAttr("data-notempty");
            $("#memberprice").removeClass("empty");
            $("#memberprice").removeClass("requirederror");
            $("#memberprice").removeClass("class", "reqtxtstyle");
            $("#sel_meal option").eq(0).before("<option value=\"\">---无---</option>");
            $("#sel_meal").val("");
            $("#sel_meal").attr("disabled", "disabled");
            $("#sel_meal").css("background", "#efefef");
            $("#memberprice").attr("class", "txtstyle");
            $("#price").attr("class", "txtstyle");
            $("#tab_dcp").css("display", "block");
        } else {
            $("#sel_meal option").eq(0).remove();
            $("#sel_meal").removeAttr("disabled", "disabled");
            $("#sel_meal").css("background", "#fff");
            $("#price").attr("data-notempty", "true");
            $("#memberprice").attr("data-notempty", "true");
            $("#memberprice").attr("class", "reqtxtstyle");
            $("#price").attr("class", "reqtxtstyle");
            $("#tab_dcp").css("display", "none");
        }
        noemptystyle('tabContent');
    });

    // 判断页面类型
    if (type == 'edit') { // 修改
        detailDishesParameters.discode = discode;
        var html = '<a href="javascript:void(0);" onclick="backform()">返回</a> &nbsp;<span data-code="Menu">出品管理</span> - 修改</span >';
        $('#PageTitle').html(html);
        if (detailDishesParameters.discode !== '') {
            commonAjax(interfaceUrl, getpostParameters('detail', detailDishesParameters), true, setDataPageInfo);
        }
    } else if (type == 'copy') { // 复制
        var html = '<a href="javascript:void(0);" onclick="backform()">返回</a> &nbsp;<span data-code="Menu">出品管理</span> - 添加</span >';
        $('#PageTitle').html(html);
        detailDishesParameters.discode = discode;
        if (detailDishesParameters.discode !== '') {
            commonAjax(interfaceUrl, getpostParameters('detail', detailDishesParameters), true, setDataPageInfo);
        }
    }
    else if (type == "info") {
        detailDishesParameters.discode = discode;
        detailDishesParameters.stocode = stocode;
        var html = '<a href="javascript:void(0);" onclick="backform()">返回</a> &nbsp;<span data-code="Menu">出品管理</span> - 详情</span >';
        $('#PageTitle').html(html);
        if (detailDishesParameters.discode !== '') {
            commonAjax(interfaceUrl, getpostParameters('detail', detailDishesParameters), true, setDataPageInfo);
        }
        $("#btns").css("display", "none");
    }

    //添加不能为空样式
    noemptystyle('tabContent');

    $(".full-row .radio").click(function () {
        if ($(this).hasClass("active")) {
            $("#entityprice").addClass("empty");
            $("#entityprice").attr("data-notempty", true);
        } else {
            $("#entityprice").removeClass("empty");
            $("#entityprice").removeAttr("data-notempty");
        }
    });
    initBatchingScheme();
    initiscaninventory();
    var tabNavLiList1 = $('#tabNav li');
    $(tabNavLiList1[3]).show();
});

//配料方案
function initBatchingScheme() {
    $("#costByingRedient").click(function () {
        if ($("#costByingRedient").hasClass("active")) {
            if ($("#matcode").val().length > 0) {
                layer.alert("所属原料已填写不可选择配料方案", { closeBtn: 0 });
                $(this).removeClass("active");
                $("#tab_pl").hide();
                $("#DishesMate").parent().hide();
                return false;
            }

            if ($("#iscaninventory").hasClass("active")) {
                layer.alert("已选择烟酒(可入库)不可选择配料方案", { closeBtn: 0 });
                $(this).removeClass("active");
                $("#tab_pl").hide();
                $("#DishesMate").parent().hide();
                return false;
            }
        }

        if ($("#costByingRedient").hasClass("active")) {
            $("#matcode").attr("disabled", "disabled"); //禁用所属原料
            $("#iscaninventory").attr("disabled", "disabled"); //禁用烟酒可入库
            $("#iscaninventory").css("background", "#efefef");
            $("#tab_pl").css("display", "block");
        } else {
            $("#matcode").removeAttr("disabled");
            $("#iscaninventory").removeAttr("disabled");
            $("#iscaninventory").css("background", "");
            $("#tab_pl").css("display", "none");
        }
    });
}

//初始化烟酒可入库
function initiscaninventory() {
    $("#iscaninventory").click(function () {
        if (!$("#iscaninventory").hasClass("active")) {
            $("#matcode").attr("data-notempty", "true");
        } else {
            $("#matcode").removeAttr("data-notempty");
            $("#matcode").removeClass("empty");
            $("#matcode").removeClass("requirederror");
        }
        //添加不能为空样式
        noemptystyle('tabContent');
    });
}

//添加不能为空样式
function noemptystyle(id) {
    var $allArray = $('#' + id + ' input');
    //不为空，添加样式
    $allArray.each(function (index, el) {
        var notempty = $(el).attr('data-notempty');
        if (notempty != undefined) {
            if (notempty) {
                $(el).addClass('empty');
            }
        }
    });
}

// 选项 关联 选项卡
function toggleTabNav(dom) {
    var str = $(dom).attr('id');
    var tabNavLiList = $('#tabNav li');

    switch (str) {
        case 'multiprice':
            $(dom).toggleClass('active');
            $(tabNavLiList[2]).toggle();
            break;
        case 'costByingRedient':
            $(dom).toggleClass('active');
            //$(tabNavLiList[3]).toggle();
            break;
        case 'entity':
            $(dom).toggleClass('active');
            $('#div_entity').toggle();
            break;
        default:
            return;
    }
}

// 用户请求数据
function getUserAddDishesData(typeParameters) {
    // 取值
    //addDishesParameters.discode = $('#discode').val();
    addDishesParameters.disname = $('#disname').val();
    addDishesParameters.disothername = $('#disothername').val();
    addDishesParameters.distypecode = $('#hiddistypecode').val();
    addDishesParameters.quickcode = $('#quickcode').val();
    addDishesParameters.customcode = $('#customcode').val();
    addDishesParameters.unit = $('#selunit').val();
    addDishesParameters.price = $('#price').val();
    addDishesParameters.memberprice = $('#memberprice').val();
    addDishesParameters.costprice = $('#costprice').val();
    addDishesParameters.pushmoney = $('#pushmoney').val();
    addDishesParameters.matcode = $('#hidmatcode').val();
    addDishesParameters.extcode = $('#extcode').val();
    addDishesParameters.fincode = $('#selfincode').val();
    addDishesParameters.dcode = $('#seldcode').val();
    addDishesParameters.kitcode = $('#selkitcode').val();
    addDishesParameters.ecode = $('#sel_ecode').val();
    addDishesParameters.maketime = $('#maketime').val();
    addDishesParameters.qrcode = $('#qrcode').val();
    addDishesParameters.dispicture = $('#uploadsrc').val();
    addDishesParameters.remark = $('#remark').val();
    addDishesParameters.warcode = $("#sel_warcode").val();
    addDishesParameters.stocode = $("#stocode").val();
    addDishesParameters.ismultiprice = '0';
    //做法加价
    var dishesMethodsJson = '[';
    $('#DishesMethods').find("tbody tr").each(function (i, o) {
        if (i != 0) {
            dishesMethodsJson += ',';
        }
        var methodcode = $($(o).find('.code')).text();//做法编号
        var selval = $($(o).find('select')).val();//加价方式
        var raiseamount = $($(o).find('input')).val();//加价金额或百分比

        dishesMethodsJson += '{"buscode":"0","stocode":"' + $("#stocode").val() + '","discode":"' + addDishesParameters.discode + '","methodcode":"' + methodcode + '","raisetype":"' + selval + '","raiseamount":"' + raiseamount + '","cuser":"' + getLoginUserInfo().userid + '","uuser":"' + getLoginUserInfo().userid + '"}';
    });
    dishesMethodsJson += ']';
    addDishesParameters.dishesMethodsJson = dishesMethodsJson;

    if (!$("#tab_dcp").is(":hidden"))//已启用多菜谱
    {
        var dishesMealsJson = '[';
        $('#DishesMeal').find("tbody tr").each(function (i, o) {
            if (i != 0) {
                dishesMealsJson += ',';
            }
            var melcode = $($(o).find('.code')).text();//菜谱编号
            var realprice = $($(o).find('.price')).val();//售价
            var memprice = $($(o).find('.memprice')).val();//会员价

            dishesMealsJson += '{"buscode":"0","stocode":"' + $("#stocode").val() + '","discode":"' + addDishesParameters.discode + '","melcode":"' + melcode + '","realprice":"' + realprice + '","realmemberprice":"' + memprice + '","cuser":"' + getLoginUserInfo().userid + '","uuser":"' + getLoginUserInfo().userid + '"}';
        });
        dishesMealsJson += ']';
        if (dishesMealsJson.length > 2) {
            addDishesParameters.ismultiprice = '1';
        }
        addDishesParameters.dishesMealsJson = dishesMealsJson;
    }

    if (!$("#tab_pl").is(":hidden"))//已启用多原料
    {
        addDishesParameters.iscostbyingredient = '1';
        var dishesMatesJson = '[';
        $('#DishesMate').find("tbody tr").each(function (i, o) {
            if (i != 0) {
                dishesMatesJson += ',';
            }
            var matcode = $($(o).find('.code')).text();//原料编号
            var val = $($(o).find('select')).val();//计量单位字典编号
            var useamount = $($(o).find('.num')).val();//使用数量
            var unitcode = '';
            var arrval = val.split('|');
            if (arrval != undefined) {
                unitcode = arrval[1];
            }

            var jlnum = $($(o).find('#jlnum')).val();
            var jlv = $($(o).find('#jlv')).val();
            var mlnum = $($(o).find('#mlnum')).val();
            var mattype = $($(o).find('.mattype')).val();
            var remark = $($(o).find('#remark')).val();

            dishesMatesJson += '{"buscode":"","stocode":"' + $("#stocode").val() + '","discode":"' + addDishesParameters.discode + '","jlnum":"' + jlnum + '","jlv":"' + jlv + '","mlnum":"' + mlnum + '","mattype":"' + mattype + '","remark":"' + remark+'","matcode":"' + matcode + '","unitcode":"' + unitcode + '","useamount":"' + useamount + '","cuser":"","uuser":""}';
        });
        dishesMatesJson += ']';
        addDishesParameters.dishesMatesJson = dishesMatesJson;
        //addDishesParameters.dishesMatesJson = JSON.parse(dishesMatesJson);
    }

    //条只方案
    if ($('#entity').is('.active')) {
        addDishesParameters.isentity = '1';
        addDishesParameters.entitydefcount = $('#entitydefcount').val();
        addDishesParameters.entityprice = $('#entityprice').val();
    }
    //可变价信息
    if ($('#iscanmodifyprice').is('.active')) {
        addDishesParameters.iscanmodifyprice = '1';
    }
    //需称重
    if ($('#isneedweigh').is('.active')) {
        addDishesParameters.isneedweigh = '1';
    }
    //做法必选
    if ($('#isneedmethod').is('.active')) {
        addDishesParameters.isneedmethod = '1';
    }
    //烟酒(可入库)
    if ($('#iscaninventory').is('.active')) {
        addDishesParameters.iscaninventory = '1';
    }
    //可自定义
    if ($('#iscancustom').is('.active')) {
        addDishesParameters.iscancustom = '1';
    }
    //允许会员价
    if ($('#isallowmemberprice').is('.active')) {
        addDishesParameters.isallowmemberprice = '1';
    }
    //参与附加费计算
    if ($('#isattachcalculate').is('.active')) {
        addDishesParameters.isattachcalculate = '1';
    }
    //支持使用消费券
    if ($('#isclipcoupons').is('.active')) {
        addDishesParameters.isclipcoupons = '1';
    }
    //可寄存
    if ($('#iscandeposit').is('.active')) {
        addDishesParameters.iscandeposit = '1';
    }
    //营业外收入
    if ($('#isnonoperating').is('.active')) {
        addDishesParameters.isnonoperating = '1';
    }
    addDishesParameters.melcode = $("#sel_meal").val();
    return true;
}

// 回调函数
function savecallback(data) {

    ShowResult(data);
    if (typeid == 'saveadd') {
        addDishesParameters.discode = '';
        location.href = '/productmanage/dishesedit.html?v=390590';
    }
}

// 填充页面数据
function setDataPageInfo(data) {
    var str = JSON.stringify(data);
    if (type == 'copy') {
        detailDishesParameters.discode = '';    //清空主键
    }
    if (data != undefined) {
        if (data.status == "0") {
            if (type != 'copy') {
                $("#price").attr("disabled", "disabled");
                $("#memberprice").attr("disabled", "disabled");
            }
            var objdata = data.data[0];
            $('#discode').val(objdata.discode);
            $('#disname').val(objdata.disname);
            $('#disothername').val(objdata.disothername);
            $('#distypecode').val(objdata.distypename);
            $('#hiddistypecode').val(objdata.distypecode);
            detailDishesTypeParameters.distypecode = objdata.distypecode;
            commonAjax(interfaceTypeUrl, getpostParameters('detail', detailDishesTypeParameters), true, function (data) {
                if (data != undefined) {
                    if (data.data[0] != undefined) {
                        $('#distypecode').val(data.data[0].distypename);
                    }
                }
            });

            $('#quickcode').val(objdata.quickcode);
            $('#customcode').val(objdata.customcode);
            $('#selunit').val(objdata.unit);
            $('#price').val(objdata.price);
            $('#memberprice').val(objdata.memberprice);
            $('#costprice').val(objdata.costprice);
            $('#pushmoney').val(objdata.pushmoney);
            $('#hidmatcode').val(objdata.matcode);
            $('#matcode').val(objdata.matname);
            $('#extcode').val(objdata.extcode);
            $('#selfincode').val(objdata.fincode);
            $('#seldcode').val(objdata.dcode);
            $('#selkitcode').val(objdata.kitcode);

            $('#maketime').val(objdata.maketime);
            $('#qrcode').val(objdata.qrcode);
            $('#selkitcode').val(objdata.kitcode);
            $('#upimg').attr("src", VRootPath + objdata.dispicture);
            $('#uploadsrc').val(objdata.dispicture);
            $('#remark').val(objdata.remark);
            $("#sel_warcode").val(objdata.warcode);
            $("#stocode").val(objdata.stocode);

            $("#txt_disname").html(objdata.disname);
            $("#txt_discode").html(objdata.discode); 
            $("#txt_selunit").html(objdata.unit); 
            $("#txt_stocode").html(objdata.stoname);
            $("#txt_sel_warcode").html(objdata.warname);
            $("#txt_realprice").html(objdata.price);
          
            $("#txt_mlvprice").html(mlv);
           
            stoSelect();

            //做法加价
            var PriceWayselval = '';
            if (objdata.DishesMethods != undefined) {
                $(objdata.DishesMethods).each(function (i, o) {
                    var MethodsCode = o.methodcode;
                    var MethodsName = o.methodname;
                    var raiseamount = o.raiseamount;
                    PriceWayselval += o.raisetype + ',';
                    var row = '<tr><td class="code">' + MethodsCode + '</td>';
                    row += '<td >{0}</td>';
                    row += '<td><select style="width:100px;" onchange="changePriceWay(this);" disabled>{1}</select></td>';
                    row += '<td style="text-align:left"><input class="empty" type="text" disabled maxlength="10" style="width:100px;" value="' + raiseamount + '" data-notempty="true" data-reg="Decimal" onblur="isDigit(this)"><span>&nbsp;%</span></td>';
                    //row += '<td><img alt="" onclick="TableInfoDelete(this,\'1\');" src="../images/btn_delete.png" class="list_button_delete" /></td></tr>';
                    row += '</tr>';
                    row = row.format(MethodsName, PriceWayInfo);
                    $("#DishesMethods tbody").append(row);
                });
                //设置做法加价下拉框值
                var splitPriceWayselval = PriceWayselval.split(',');
                $('#DishesMethods tbody tr').each(function (i, o) {
                    var per = '&nbsp;&nbsp;';
                    if (splitPriceWayselval[i] == '0')//加价比例
                    {
                        per = '&nbsp;%';
                    }
                    $(o).find('input[type=text]').next().html(per);
                    $(o).find('select').val(splitPriceWayselval[i]);
                });
            }

            //多菜谱 ismultiprice == '1'
            if (objdata.DishesMeals.length > 0) {
                $('#tab_dcp').toggle();
                if ($('#multiprice').hasClass('active')) {
                    $("#price").removeAttr("data-notempty");
                    $("#price").removeClass("empty");
                    $("#price").removeClass("requirederror");
                    $("#memberprice").removeAttr("data-notempty");
                    $("#memberprice").removeClass("empty");
                    $("#memberprice").removeClass("requirederror");

                } else {
                    $("#price").attr("data-notempty", "true");
                    $("#memberprice").attr("data-notempty", "true");
                }
                noemptystyle('tabContent');
                $('#multiprice').toggleClass('active');
                if (objdata.DishesMeals != undefined) {
                    $(objdata.DishesMeals).each(function (i, o) {
                        var MealCode = o.melcode;
                        var MealName = o.melname;
                        var realprice = o.realprice;
                        var realmemberprice = o.realmemberprice;
                        var isdefault = o.isdefault;
                        if (isdefault == "1") {
                            isdefault = '√';
                        } else {
                            isdefault = "";
                        }
                        var row = '<tr><td class="code">' + MealCode + '</td>';
                        row += '<td>{0}</td>';
                        row += '<td>{1}</td>';
                        row += '<td><input class="price empty" type="text" maxlength="18" disabled style="width:100px;" value="' + realprice + '" data-notempty="true" data-reg="Decimal" onblur="isDigit(this)"></td>';
                        row += '<td><input class="memprice empty" type="text" maxlength="18" disabled style="width:100px;" value="' + realmemberprice + '" data-notempty="true" data-reg="Decimal" onblur="isDigit(this)"></td>';
                        //row += '<td><img alt="" onclick="TableInfoDelete(this,\'2\');" src="../images/btn_delete.png" class="list_button_delete" /></td></tr>';
                        row += '</tr>';
                        row = row.format(MealName, isdefault, PriceWayInfo);
                        $("#DishesMeal tbody").append(row);
                    });
                }
            }
            if (objdata.ismultiprice == '0') {
                $("#sel_meal option").eq(0).remove();
                if (objdata.melcode != "" && objdata != null) {
                    $("#sel_meal").val(objdata.melcode);
                    if ($("#sel_meal").val() == null && $("#sel_meal option").length > 0) {
                        $("#sel_meal option").eq(0).attr("selected", "selected");
                    }
                }
                $("#multiprice").click(function () {
                    if ($("#multiprice").hasClass("active")) {
                        $("#price").removeAttr("data-notempty");
                        $("#price").removeClass("empty");
                        $("#price").removeClass("requirederror");
                        $("#memberprice").removeAttr("data-notempty");
                        $("#memberprice").removeClass("empty");
                        $("#memberprice").removeClass("requirederror");

                        $("#sel_meal option").eq(0).before("<option value=\"\">---无---</option>");
                        $("#sel_meal").val("");
                        $("#sel_meal").attr("disabled", "disabled");
                        $("#sel_meal").css("background", "#efefef");
                    } else {
                        $("#sel_meal option").eq(0).remove();
                        $("#sel_meal").removeAttr("disabled", "disabled");
                        $("#sel_meal").css("background", "#fff");
                        $("#price").attr("data-notempty", "true");
                        $("#memberprice").attr("data-notempty", "true");
                    }
                    noemptystyle('tabContent');
                });
            }

            if (objdata.ismultiprice == "1") {
                $("#sel_meal").val("");
                $("#sel_meal").attr("disabled", "disabled");
                $("#sel_meal").css("background", "#efefef");
            }

            //配料方案
            var unitcodeselval = '';
            if (objdata.DishesMates.length > 0) {
                $("#matcode").attr("disabled", "disabled");
                $("#iscaninventory").attr("disabled", "disabled");
                $("#iscaninventory").css("background", "#efefef");
                
                $('#costByingRedient').toggleClass('active');
                if (objdata.DishesMates != undefined) {
                    $(objdata.DishesMates).each(function (i, o) {
                        var DishesMateCode = o.matcode;
                        var DishesMateName = o.matname;
                        var distypename = o.matclassname;
                        var DishesMatePrice = 0;
                        var useamount = o.useamount;
                        var unitcode = o.unitcode;
                        var xprice = o.smxprice;
                        var unitname = o.unitname;
                        var UnitInfo = '';

                        var jlnum = o.jlnum;
                        var jlv = o.jlv;
                        var mlnum = o.mlnum;
                        var mattype = o.mattype;
                        var remark = o.remark;
                        if (i > 0) {
                            unitcodeselval += ',';
                        }
                        //单位信息
                        if (o.StockMateUnits != undefined && o.StockMateUnits.length > 0) {
                            UnitInfo = '';
                            $(o.StockMateUnits).each(function (j, q) {

                                if (q.matunitcode == unitcode)//获取指定单位的价格
                                {
                                    xprice = q.xprice;
                                    unitcodeselval = q.xprice + '|' + unitcode;
                                }
                                UnitInfo += '<option value="' + q.xprice + '|' + q.matunitcode + '">' + q.matunitname + '</option>';

                            });
                        }

                        if (UnitInfo.length == 0)//非多单位
                        {
                            UnitInfo = '<option value="' + xprice + '|' + unitcode + '">' + unitname + '</option>';
                            unitcodeselval += xprice + '|' + unitcode;
                        }

                        DishesMatePrice = useamount * xprice;
                        if (jlv != 100) {
                            jlv = jlv * 100;
                        }
                        var row = '<tr><td class="code">' + DishesMateCode + '</td>';
                        row += '<td style="width:200px;">{0}</td>';
                        row += '<td>{1}</td>';
                        row += '<td><select style="width:70px;" onchange="CountPriceInfo(this);">{2}</select></td>';
                        row += '<td><input id="jlnum" class="num empty" type="text" maxlength="10" style="width:70px;" value="' + jlnum + '" data-notempty="true" data-reg="Decimal" oninput="jlnuminput(this,' + parseFloat(DishesMatePrice).toFixed(2) + ',' + i +')"></td>';
                        row += '<td><input id="jlv" class="num empty" type="text" maxlength="10" style="width:70px;" value="' + jlv.toFixed(2) + '"  data-notempty="true" data-reg="Decimal"></td>';
                        row += '<td><input id=mlnum class="num empty" type="text" maxlength="10" style="width:70px;" value="' + mlnum + '"  data-notempty="true" data-reg="Decimal"></td>';
                        row += '<td><select id="mattype' + i +'" class="mattype" style="width:70px;"><option value="主料" selected>主料</option><option value="辅料">辅料</option><option value="调料">调料</option></select></td>';
                        row += '<td class="showcost">{3}</td>';
                        row += '<td><span id="money' + i + '">' + (mlnum * parseFloat(xprice)).toFixed(2) + '</span></td>';
                        row += '<td><input id="remark" type="text" maxlength="10" style="width:70px;" value="' + remark + '" onblur="CountPriceInfo(this);" data-notempty="true" data-reg="Decimal"></td>';
                        row += '<td><img alt="" onclick="TableInfoDelete(this,\'3\');" src="../images/btn_delete.png" class="list_button_delete" /></td></tr>';
                        row = row.format(DishesMateName, distypename, UnitInfo, parseFloat(xprice).toFixed(6));
                        $("#DishesMate tbody").append(row);
                        $("#DishesMate tbody").find("._sel").children().eq(0).attr("selected", "selected"); 
                        $("#mattype" + i + "").val(mattype);
                    });
                }
            }


            //设置配料下拉框值
            //var splitunitcodeselval = unitcodeselval.split(',');
            //$('#DishesMate tbody tr').each(function (i, o) {
            //    $(o).find('select').val(splitunitcodeselval[i]);
            //});

            //条只方案
            if (objdata.isentity == '1') {
                $('#entity').toggleClass('active');
                $('#div_entity').toggle();
                $('#entitydefcount').val(objdata.entitydefcount);
                $('#entityprice').val(objdata.entityprice);
            }
            //可变价信息
            if (objdata.iscanmodifyprice == '1') {
                $('#iscanmodifyprice').toggleClass('active');
            }
            //需称重
            if (objdata.isneedweigh == '1') {
                $('#isneedweigh').toggleClass('active');
            }
            //做法必选
            if (objdata.isneedmethod == '1') {
                $('#isneedmethod').toggleClass('active');
            }
            //烟酒(可入库)
            if (objdata.iscaninventory == '1') {
                $('#iscaninventory').toggleClass('active');
            }
            //可自定义
            if (objdata.iscancustom == '1') {
                $('#iscancustom').toggleClass('active');
            }
            //允许会员价
            if (objdata.isallowmemberprice == '1') {
                $('#isallowmemberprice').toggleClass('active');
            }
            //参与附加费计算
            if (objdata.isattachcalculate == '1') {
                $('#isattachcalculate').toggleClass('active');
            }
            //支持使用消费券
            if (objdata.isclipcoupons == '1') {
                $('#isclipcoupons').toggleClass('active');
            }
            //可寄存
            if (objdata.iscandeposit == '1') {
                $('#iscandeposit').toggleClass('active');
            }
            //营业外收入
            if (objdata.isnonoperating == '1') {
                $('#isnonoperating').toggleClass('active');
            }

            $('#selfincode').val(objdata.fincode);
            $('#seldcode').val(objdata.dcode);
            BindEmployeeInfo(2);
            $('#selkitcode').val(objdata.kitcode);
            $("#sel_warcode").val(objdata.warcode);
            $('#sel_ecode').val(objdata.ecode);

        }
    }
}


var PriceWayInfo = '';


function setPriceWayInfo(stroption) {
    PriceWayInfo = stroption;
}
//做法加价Add
function DishesMethodsAdd() {
    //选择做法
    ShowOpenpage('', 'methodsrefer.aspx?type=DishesMethods&fun=bindDishesMethods&stocode=' + $("#stocode").val() + '', '80%', '550px', false);
}

function isDigit(obj) {
    var test_value = $(obj).val();
    var patrn = /^([1-9]\d*|0)(\.\d*[1-9])?$/;
    if (!patrn.exec(test_value)) {
        $(obj).val("");
    }
}

function bindDishesMethods(ids, names) {
    var arrids = ids.split(',');
    var arrnames = names.split(',');
    if (arrids != undefined) {
        for (var i = 0; i < arrids.length; i++) {
            var val = ',' + $('#hidDishesMethods').val();
            if (val.indexOf(',' + arrids[i] + ',') < 0) {
                $('#hidDishesMethods').val($('#hidDishesMethods').val() + arrids[i] + ',');
                if (arrids[i].length == 0) {
                    continue;
                }
                var MethodsCode = arrids[i];
                var MethodsName = arrnames[i];
                var row = '<tr><td class="code">' + MethodsCode + '</td>';
                row += '<td >{0}</td>';
                row += '<td><select style="width:100px;" class="empty" onchange="changePriceWay(this);">{1}</select></td>';
                row += '<td style="text-align:left"><input class="empty" type="text" maxlength="10" style="width:100px;" value="" data-notempty="true" data-reg="Decimal" onblur="isDigit(this)"><span>&nbsp;%</span></td>';
                row += '<td><img alt="" onclick="TableInfoDelete(this,\'1\');" src="../images/btn_delete.png" class="list_button_delete" /></td>';
                row += '</tr>';
                row = row.format(MethodsName, PriceWayInfo);
                $("#DishesMethods tbody").append(row);
            }
        }
    }
}
//加价方式变更
function changePriceWay(obj) {
    var val = $(obj).val();
    var per = '&nbsp;&nbsp;';
    if (val == '0')//加价比例
    {
        per = '&nbsp;%';
    }
    var tr = obj.parentNode.parentNode;
    $(tr).find('td span').html(per);
}

function TableInfoDelete(obj, type) {
    if (confirm("确定删除吗?")) {
        //删除行数据
        var tr = obj.parentNode.parentNode;
        var code = $(tr).find('.code').html();
        var vals = '';
        var hidObj;
        switch (type) {
            case "1":
                hidObj = $('#hidDishesMethods');
                break;
            case "2":
                hidObj = $('#hidDishesMeal');
                break;
            case "3":
                hidObj = $('#hidDishesMate');
                break;
        }
        vals = $(hidObj).val();
        vals = (',' + vals).replace(',' + code + ',', '');
        $(hidObj).val(vals);

        var tbody = tr.parentNode;
        tbody.removeChild(tr);
    }
}

//多菜谱Add
function DishesMealAdd() {
    //选择多菜谱
    ShowOpenpage('', 'Mealrefer.aspx?type=Meal&fun=bindDishesMeal&stocode=' + $("#stocode").val() + '', '80%', '550px', false);
}

function bindDishesMeal(ids, names, isdefault) {
    var arrids = ids.split(',');
    var arrnames = names.split(',');
    var arrisdefault = isdefault.split(',');
    if (arrids != undefined) {
        for (var i = 0; i < arrids.length; i++) {
            var val = ',' + $('#hidDishesMeal').val();
            if (val.indexOf(',' + arrids[i] + ',') < 0) {
                $('#hidDishesMeal').val($('#hidDishesMeal').val() + arrids[i] + ',');
                if (arrids[i].length == 0) {
                    continue;
                }
                var MealCode = arrids[i];
                var MealName = arrnames[i];
                var isdefault = '';
                if (arrisdefault[i] == "1") {
                    isdefault = '√';
                }
                var row = '<tr><td class="code">' + MealCode + '</td>';
                row += '<td>{0}</td>';
                row += '<td>{1}</td>';
                row += '<td><input class="price empty" type="text" maxlength="18" style="width:100px;" value="" data-notempty="true" data-reg="Decimal" onblur="isDigit(this)"></td>';
                row += '<td><input class="memprice empty" type="text" maxlength="18" style="width:100px;" value="" data-notempty="true" data-reg="Decimal" onblur="isDigit(this)"></td>';
                row += '<td><img alt="" onclick="TableInfoDelete(this,\'2\');" src="../images/btn_delete.png" class="list_button_delete" /></td></tr>';
                row = row.format(MealName, isdefault, PriceWayInfo);
                $("#DishesMeal tbody").append(row);
            }
        }
    }
}

//var UnitInfo = '';
//getDictOptionInfo(SystemDict.Unit, 0, 'zh-cn', setUnitInfoInfo);

function setUnitInfoInfo(stroption) {
    UnitInfo = stroption;
}
//原料Add
function DishesMateAdd() {
    //选择原料
    ShowOpenpage('', 'DishesMaterefer.aspx?type=dishes&fun=bindDishesMate&stocode=' + getUrlParam('stocode'), '80%', '550px', false);
}


function bindDishesMate(ArrDishesMate) {
    if (ArrDishesMate != undefined) {
        for (var i = 0; i < ArrDishesMate.length; i++) {

            var obj = ArrDishesMate[i];
            var val = ',' + $('#hidDishesMate').val();
            if (val.indexOf(',' + obj.matcode + ',') < 0) {
                $('#hidDishesMate').val($('#hidDishesMate').val() + obj.matcode + ',');
                var DishesMateCode = obj.matcode;
                var DishesMateName = obj.matname;
                var distypename = obj.mattypename;
                var DishesMatePrice = '0';

                if (obj.isusemulticunit == '1') {
                    //单位信息
                    if (obj.StockMateUnits != undefined) {
                        UnitInfo = '';
                        for (var j = 0; j < obj.StockMateUnits.length; j++) {
                            var unitobj = obj.StockMateUnits[j];
                            if (j == 0) {
                                DishesMatePrice = unitobj.xprice;
                            }

                            UnitInfo += '<option value="' + unitobj.xprice + '|' + unitobj.matunitcode + '">' + unitobj.matunitname + '</option>';
                        }
                    }
                }
                else {
                    DishesMatePrice = obj.smxprice;
                    UnitInfo = '<option value="' + obj.smxprice + '|' + obj.smunitname + '">' + obj.matunitname + '</option>';
                }

                var row = '<tr><td class="code">' + DishesMateCode + '</td>';
                row += '<td>{0}</td>';
                row += '<td>{1}</td>';
                row += '<td><select style="width:70px;" onchange="CountPriceInfo(this);">{2}</select></td>';
                row += '<td><input id="jlnum" class="num empty" type="text" maxlength="10" style="width:70px;" value="0" data-notempty="true" data-reg="Decimal" oninput="jlnuminput(this,' + parseFloat(DishesMatePrice).toFixed(2)+','+i+')"></td>';
                row += '<td><input id="jlv" class="num empty" type="text" maxlength="10" style="width:70px;" value="0"  data-notempty="true" data-reg="Decimal"></td>';
                row += '<td><input id="mlnum" class="num empty" type="text" maxlength="10" style="width:70px;" value="0"  data-notempty="true" data-reg="Decimal"></td>';
                row += '<td><select id="mattype" class="mattype" style="width:70px;"><option value="主料" selected>主料</option><option value="辅料">辅料</option><option value="调料">调料</option></select></td>';
                row += '<td class="showcost">{3}</td>';
                row += '<td><span id="money'+i+'">0.00</span></td>';
                row += '<td><input id="remark" type="text" maxlength="10" style="width:70px;" value="" onblur="CountPriceInfo(this);" data-notempty="true" data-reg="Decimal"></td>';
                row += '<td><img alt="" onclick="TableInfoDelete(this,\'3\');" src="../images/btn_delete.png" class="list_button_delete" /></td></tr>';
                row = row.format(DishesMateName, distypename, UnitInfo, parseFloat(DishesMatePrice).toFixed(2));
                $("#DishesMate tbody").append(row);
            }
        }
    }
}

function jlnuminput(o, p, q) {
    $("#money" + q).html((parseFloat($(o).val()) * p).toFixed(2));
}

function CountPriceInfo(obj) {
    var tr = obj.parentNode.parentNode;
    var val = $(tr).find('select').val();
    var num = $(tr).find('.num').val();
    var price = val.split('|');
    if (price != undefined) {
        var cost = (num * price[0]).toFixed(2);
        $(tr).find('.cost').html(cost.toString());
        $(tr).find('.showcost').html(parseFloat(price[0]).toFixed(2).toString());
    }
}