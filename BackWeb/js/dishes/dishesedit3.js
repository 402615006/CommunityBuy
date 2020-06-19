//获取页面参数
var type = getUrlParam('type'); //操作类型
var discode = getUrlParam('discode');//主键
var stocode = getUrlParam('stocode');
var mlv = getUrlParam('mlv');

// API参数配置
var interfaceUrl = '../../ajax/dishes/WSdishes.ashx';

//详情接口请求默认参数
var detailDishesParameters = {
    GUID: "57d99d89-caab-482a-a0e9-a0a803eed3ba",
    USER_ID: getLoginUserInfo().userid,
    discode: discode,
    stocode: stocode
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



//初始化函数
$(document).ready(function () {
    // 判断页面类型
    var html = "";
    if (type == 'edit') { // 修改
        detailDishesParameters.discode = discode;
        var html = '<a href="javascript:void(0);" onclick="backform()">返回</a> &nbsp;<span data-code="Menu">出品管理</span> - 修改</span >';
        $('#PageTitle').html(html);
        if (detailDishesParameters.discode !== '') {
            commonAjax(interfaceUrl, getpostParameters('detail1', detailDishesParameters), true, setDataPageInfo);
        }
    } else if (type == 'copy') { // 复制
        html = '<a href="javascript:void(0);" onclick="backform()">返回</a> &nbsp;<span data-code="Menu">出品管理</span> - 添加</span >';
        $('#PageTitle').html(html);
        detailDishesParameters.discode = discode;
        if (detailDishesParameters.discode !== '') {
            commonAjax(interfaceUrl, getpostParameters('detail1', detailDishesParameters), true, setDataPageInfo);
        }
    }
    else if (type == "info") {
        detailDishesParameters.discode = discode;
        detailDishesParameters.stocode = stocode;
        html = '<a href="javascript:void(0);" onclick="backform()">返回</a> &nbsp;<span data-code="Menu">出品管理</span> - 详情</span >';
        $('#PageTitle').html(html);
        if (detailDishesParameters.discode !== '') {
            commonAjax(interfaceUrl, getpostParameters('detail1', detailDishesParameters), true, setDataPageInfo);
        }
        $("#btns").css("display", "none");
    }

    var tabNavLiList1 = $('#tabNav li');
    $(tabNavLiList1[3]).show();
});

// 填充页面数据
function setDataPageInfo(data) {
    var str = JSON.stringify(data);
    if (type == 'copy') {
        detailDishesParameters.discode = '';    //清空主键
    }
    if (data != undefined) {
        if (data.status == "0") {
            var objdata = data.data[0];
            $("#txt_disname").html(objdata.disname);
            $("#txt_discode").html(objdata.discode);
            $("#txt_selunit").html(objdata.unit);
            $("#txt_stocode").html(objdata.stoname);
            $("#txt_sel_warcode").html(objdata.warname);
            $("#txt_realprice").html(objdata.price);

            $("#txt_mlvprice").html(mlv);

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
                        var xprice = o.sjcb;
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
                                    xprice = q.sjcb;
                                    unitcodeselval = q.sjcb + '|' + unitcode;
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
                        DishesMatePrice = parseFloat(DishesMatePrice).toFixed(2);
                        if (isNaN(DishesMatePrice)) {
                            DishesMatePrice = '单位异常';
                        }
                        if (jlv != 100) {
                            jlv = jlv * 100;
                        }
                        var row = '<tr><td class="code">' + DishesMateCode + '</td>';
                        row += '<td style="width:200px;">{0}</td>';
                        row += '<td>{1}</td>';
                        row += '<td><select style="width:70px;" onchange="CountPriceInfo(this);">{2}</select></td>';
                        row += '<td><input id="jlnum" class="num empty" type="text" maxlength="10" style="width:70px;" value="' + jlnum + '" data-notempty="true" data-reg="Decimal" oninput="jlnuminput(this,' + DishesMatePrice + ',' + i + ')"></td>';
                        row += '<td><input id="jlv" class="num empty" type="text" maxlength="10" style="width:70px;" value="' + jlv.toFixed(2) + '"  data-notempty="true" data-reg="Decimal">%</td>';
                        row += '<td><input id=mlnum class="num empty" type="text" maxlength="10" style="width:70px;" value="' + mlnum + '"  data-notempty="true" data-reg="Decimal"></td>';
                        row += '<td><select id="mattype' + i + '" class="mattype" style="width:70px;"><option value="主料" selected>主料</option><option value="辅料">辅料</option><option value="调料">调料</option></select></td>';
                        row += '<td class="showcost">{3}</td>'; 
                        row += '<td><span id="money' + i + '">' + (mlnum * parseFloat(xprice)).toFixed(2) + '</span></td>';
                        row += '<td><input id="remark" type="text" maxlength="10" style="width:70px;" value="' + remark + '" onblur="CountPriceInfo(this);" data-notempty="true" data-reg="Decimal"></td>';
                        row += '</tr>';
                        row = row.format(DishesMateName, distypename, UnitInfo, parseFloat(xprice).toFixed(6));
                        $("#DishesMate tbody").append(row);
                        $("#DishesMate tbody").find("._sel").children().eq(0).attr("selected", "selected");
                        $("#mattype" + i + "").val(mattype);
                    });
                }
            }
        }
    }
}


var PriceWayInfo = '';


function setPriceWayInfo(stroption) {
    PriceWayInfo = stroption;
}

function isDigit(obj) {
    var test_value = $(obj).val();
    var patrn = /^([1-9]\d*|0)(\.\d*[1-9])?$/;
    if (!patrn.exec(test_value)) {
        $(obj).val("");
    }
}

//var UnitInfo = '';
//getDictOptionInfo(SystemDict.Unit, 0, 'zh-cn', setUnitInfoInfo);

function setUnitInfoInfo(stroption) {
    UnitInfo = stroption;
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