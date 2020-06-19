/*
描述：页面权限控制
作者：CGD
时间：2016-11-18
*/

var permissiondata; //连锁端同步表信息缓存

//系统所有按钮Html
//<button id="refreshButton" type="button" class="refresh" onclick="refreshclick();"><i></i><span>刷新</span></button>
var searchBut = '<button id="refreshButton" type="button" class="refresh" onclick="refreshclick();"><i></i><span>刷新</span></button><button id="searchButton" type="button" class="search" onclick="PageClick(\'search\');"><i></i><span data-code="search_button">搜索</span></button>';
var addBut = '<button id="addButton" type="button" class="add" onclick="PageClick(\'add\');"><i></i><span data-code="add_button">添加</span></button>';
var editBut = '<button id="editButton" type="button" class="edit" onclick="PageClick(\'edit\');"><i></i><span data-code="edit_button">修改</span></button>';
var deleteBut = '<button type="button" class="delete" onclick="PageClick(\'delete\');"><i></i><span data-code="delete_button">删除</span></button>';
var copyBut = '<button type="button" class="copy" onclick="PageClick(\'copy\');"><i></i><span data-code="copy_button">复制</span></button>';
var exportBut = '<button type="button" class="export" onclick="PageClick(\'export\');"><i></i><span  data-code="export_button">导出</span></button>';
var validBut = '<button type="button" class="active" onclick="PageClick(\'valid\');"><i></i><span data-code="valid_button">启用</span></button>';
var invalidBut = '<button type="button" class="invalid" onclick="PageClick(\'invalid\');"><i></i><span data-code="invalid_button">禁用</span></button>';
var allsync = '<button id="AllSynchro" type="button" class="active"><i></i><span data-code="AllSynchro">全部同步</span></button>';
var addsync = '<button id="TableSynchro" class="active" type="button"><i></i><span data-code="TableSynchro">增量同步</span></button>';
var depselBut = '<button class="edit" type="button" onclick="PageClick(\'depsel\');"><i></i><span data-code="depsel_button">部门修改</span></button>';
var delmealBut = '';
var uploadfoodBut = '';
var delrepBut = '';
var downtempBut = '';
var uploadtempBut = '';
//var delmealBut = '<button type="button" class="delmeal" onclick="PageClick(\'delmeal\');"><i></i><span data-code="delmeal_button">移除套餐</span></button>';
//var uploadfoodBut = '<button type="button" class="uploadfood" onclick="PageClick(\'uploadfood\');"><i></i><span data-code="uploadfood_button">上传菜品</span></button>';
//var delrepBut = '<button type="button" class="delrep" onclick="PageClick(\'delrep\');"><i></i><span data-code="delrep_button">移除签送</span></button>';
//var downtempBut = '<button type="button" class="downtemp" onclick="PageClick(\'downtemp\');"><i></i><span data-code="downtemp_button">下载模板</span></button>';
//var uploadtempBut = '<button type="button" class="uploadtemp" onclick="PageClick(\'uploadtemp\');"><i></i><span data-code="uploadtemp_button">上传模板</span></button>';

var acceptanceBut = '<button type="button" class="acceptance" onclick="PageClick(\'acceptance\');"><i></i><span data-code="acceptance_button">验收</span></button>';
var auditBut = '<button type="button" class="audit" onclick="PageClick(\'audit\');"><i></i><span data-code="audit_button">审核</span></button>';
var reauditBut = '<button type="button" class="reaudit" onclick="PageClick(\'reaudit\');"><i></i><span data-code="reaudit_button">复核</span></button>';
var paymentBut = '<button type="button" class="payment" onclick="PageClick(\'payment\');"><i></i><span data-code="payment_button">付款</span></button>';
var mypreviewBut = '<button type="button" class="mypreview" onclick="PageClick(\'mypreview\');"><i></i><span data-code="myprintpreview_button">打印预览</span></button>';
var myprintBut = '';//'<button type="button" class="myprint" onclick="PageClick(\'myprint\');"><i></i><span data-code="myprint_button">打印</span></button>';
var settimeBut = '<button type="button" class="settime" onclick="PageClick(\'settime\');"><i></i><span data-code="settime_button">到店时间</span></button>';//到店时间
var detailsBut = '<button type="button" class="details" onclick="PageClick(\'edit\');"><i></i><span data-code="details_button">详情</span></button>';//详情
//var detailsBut = '<button type="button" class="yuanliaoout" onclick="matsoutstore();"><i></i><span>原料出库</span></button>';//原料出库
//系统所有按钮数组(存放上面的按钮)
var _ButtonArray = [
    //数据库对应字段，对应变量名称
    ["add", addBut],
    ["edit", editBut],
    ["delete", deleteBut],
    ["copy", copyBut],
    ["export", exportBut],
    ["valid", validBut],
    ["invalid", invalidBut],
    ["delmeal", delmealBut],
    ["uploadfood", uploadfoodBut],
    ["delrep", delrepBut],
    ["downtemp", downtempBut],
    ["uploadtemp", uploadtempBut],
    ["acceptance", acceptanceBut],
    ["audit", auditBut],
    ["reaudit", reauditBut],
    ["payment", paymentBut],
    ["mypreview", mypreviewBut],
    ["myprint", myprintBut],
    ["settime", settimeBut],
    ["details", detailsBut],
    ["AllSync", allsync],
    //["AddSync", addsync]
    //这里要注意 字段里面PageClick里面的名称一定要和数据库对应字段相同
]

var hidebutton = $("body").attr("data-hidebutton");
var pagecode = $("body").attr("data-pagecode");
var loginuserid = getLoginUserInfo().userid;
var ButtonHtml = searchBut;
if (hidebutton != undefined) {
    ButtonHtml = '';
}

/**/
var paras = {
    "GUID": getLoginUserInfo().GUID,
    "USER_ID": getLoginUserInfo().userid,
    "pageSize": 10000,
    "currentPage": 1,
    "filter": "",
    "order": "",
    "roleid": ""
};
var flag = true;
var _permissiondatastr;
$(document).ready(function () {
    hidebutton = $("body").attr("data-hidebutton");
    pagecode = $("body").attr("data-pagecode");
    _permissondatastr = GetLocal("chainsynlist_data");
    var _permissiondata = undefined;
    if (_permissondatastr.length != 0) {
        _permissiondata = JSON.parse(_permissondatastr);
    }
    if (pagecode != undefined && pagecode.length > 0) {
        if (_permissiondata != undefined) {
            for (var i = 0; i < _permissiondata.length; i++) {
                if (pagecode == "discountpackage") {
                    flag = true;
                    break;
                }
                else if (_permissiondata[i].pagecode == pagecode) {
                    flag = false;
                    break;
                }
            }
            setPageHtml(flag);
        } else {
            //判断是否是连锁端同步表信息
            commonAjax('IStore/permission/WSchainsynlist.ashx', getpostParameters('getlist', paras), true, function (_data) {
                if (_data != undefined && _data != null) {
                    if (_data.data != undefined) {
                        _permissondatastr = SaveLocal("chainsynlist_data", JSON.stringify(_data.data));
                        for (var i = 0; i < _data.data.length; i++) {
                            if (_data.data[i].pagecode == pagecode) {
                                flag = false;
                                break;
                            }
                        }
                    }
                }
                setPageHtml(flag);
            });
        }
    }
    //$('.back').attr('onclick', 'javascript:history.back(-1);');
});

function setPageHtml(flag) {
    if (flag) {
        //获取页面权限按钮信息
        var interfaceUrl = 'IStore/WSsto_admins.ashx';
        commonAjax(interfaceUrl, getpostParameters('getUserRole', { "GUID": "", "USER_ID": "0", "userid": loginuserid, "pagecode": pagecode }), true, function (data) {
            if (data != undefined) {
                if (data.status == 0) {
                    for (var i = 0; i < data.data.length; i++) {
                        for (var j = 0; j < _ButtonArray.length; j++) {
                            if (data.data[i].btnname == null) {
                                continue;
                            }
                            if (_ButtonArray[j][0].toLowerCase() == data.data[i].btnname.toLowerCase()) {
                                ButtonHtml += _ButtonArray[j][1];
                            }
                        }
                    }

                    $('#PageButton').html(ButtonHtml);
                    if (ButtonHtml.indexOf("AllSynchro") != -1 && pagecode !="dishes") {
                        $("#PageButton .search").hide();
                    }
                }
            }
            else {
                $('#PageButton').html(ButtonHtml);
            }
        });
    } else {
        ButtonHtml = searchBut + detailsBut;
        if (pagecode == "paymethod") {
            ButtonHtml += validBut + invalidBut + depselBut;
        }

        $("#PageButton").html(ButtonHtml);
    }
}