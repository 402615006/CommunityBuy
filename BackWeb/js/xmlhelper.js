/*
描述：多语言页面名称解析，根据body的data-pagecode属性获取相应的xml文件
作者：CGD
时间：2016-11-18
*/
var xmlDoc;
var xmlDocCommon;
var pagecode;
$(document).ready(function () {
    var noload = $("body").attr("data-noload");
    if (noload == undefined) {
        setPageLanguageInfo();
    }
});

function setPageLanguageInfo() {
    $(document).attr('title', getNameByCode('pageTitle'));
    pagecode = $("body").attr("data-pagecode");
    $("body").hide();
    $.ajax({
        //url: xmlFile,
        url: '/xml/' + getlanguageFilePath() + '/' + pagecode + '.xml',
        dataType: 'xml',
        type: 'GET',
        timeout: 20000,
        async: false,
        error: function (xml) {
            console.log("Load XML Error！");
        },
        success: function (xml) {
            getCommonXml(xml);

        }
    });
}

function getCommonXml(xml) {
    //获取公用文件
    $.ajax({
        //url: xmlFile,
        url: '/xml/' + getlanguageFilePath() + '/common.xml',
        dataType: 'xml',
        type: 'GET',
        timeout: 20000,
        async: false,
        error: function (xmlCommon) {
            alert("加载XML 文件出错！");
        },
        success: function (xmlCommon) {
            xmlDocCommon = $(xmlCommon).find('Pages');
            xmlDoc = $(xml).find('Pages');
            if (xmlDoc != undefined && xmlDoc.length > 0) {
                setPageInfo();
                $("body").show();
            }
        }
    });
}

function setPageTitle() {
    var val = getNameByCode('PageTitle');
    $("title").html(val);
}

function setPageInfo() {
    setPageTitle();
    $("[data-code]").each(function () {
        var name = $(this).attr("data-code");
        var val = '';
        switch (name) {
            case 'position':
            case 'PageCount1':
            case 'PageCount2':
            case 'RecordCount':
            case 'FirstPage':
            case 'LastPage':
            case 'NextPage':
            case 'PrevPage':
            case 'save_edit':
            case 'back_edit':
            case 'refresh_button':
            case 'search_button':
            case 'add_button':
            case 'edit_button':
            case 'delete_button':
            case 'valid_button':
            case 'invalid_button':
            case 'cancel_button':
            case 'pause_button':
            case 'continue_button':
            case 'hide_button':
            case 'auditpass_button':
            case 'sensitive_button':
            case 'scorerel_button':
            case 'untying_button':
            case 'clearphone_button':
            case 'export_button':
            case 'import_button':
            case 'countexport_button':
            case 'delay_button':
            case 'downloadqctemplate':
            case 'importqcdate':
            case 'downloadyltemplate':
            case 'importyldata':
            case 'viewerq':
            case 'adjust_button':
            case 'backpay_button':
            case 'copy_button':
            case 'total_button':
            case 'diffhandle_button':
            case 'found_button':
            case 'relea_button':
            case 'releainfo_button':
            case 'deal_button':
            case 'info_button':
            case 'on_button':
            case 'off_button':
            case 'upload_button':
            case 'audit_button':
            case 'nullify_button':
            case 'prefabricate_button':
            case 'sendbatchcard_button':
            case 'exportpwd_button':
            case 'save_edit':
            case 'back_edit':
            case 'sendcard_btn':
            case 'changecard_btn':
            case 'repaircard_btn':
            case 'recharge_btn':
            case 'loss_btn':
            case 'merge_button':
            case 'pass_btn':
            case 'freeze_button':
            case 'thaw_button':
            case 'systemcode_placeholder':
            case 'build_button':
            case 'reabuild_button':
            case 'buildtotal_button':
            case 'seabuild_button':
            case 'free_button':
            case 'reject_btn':
            case 'payment_button':
            case 'pay_button':
            case 'pandian_button':
            case 'scpandian_button':
            case 'instore_button':
            case 'outstock_button':
            case 'review_button':
            case 'bactfree_button':
            case 'paysucess_result':
            case 'payerror_result':
            case 'prev_btn':
            case 'memedit_button':
            case 'transfer_button':
            case 'downloadtemplate':
            case 'materialimport':
            case 'nullifynotuse_button':
            case 'copyint_button':
            case 'chargoff_button':
            case 'bindmember_button':
            case 'unloss_button':
            case 'oldactive_button':
            case 'canoldactive_button':
            case "importk3_button":
            case 'templatedownload':
            case 'detail_button':
            case 'materialinfo_btn':
            case 'modifysupp_btn':
            case 'storecon_btn':
            case 'loguser_button':
            case 'reaudit_button':
            case 'recount_button':
            case 'analysis_button':
            case 'analysis1_button':
            case 'analysis2_button':
            case 'memeditphone_button':
            case 'membercard_button':
                val = getCommonInfo(name);
                if (name.indexOf('_placeholder') >= 0) {
                    $(this).attr("placeholder", val);
                }
                $(this).html(val);
                break;
            default:
                val = getNameByCode(name);
                if (val != "" && val != undefined) {
                    val = val.replace('\\r', '<br/>');
                }
                if (name.indexOf('_placeholder') >= 0) {
                    $(this).attr("placeholder", val);
                }
                else if (name.indexOf('_title') >= 0) {
                    $(this).attr("title", val);
                }
                else {
                    $(this).html(val);
                }
                break;
        }
    });
}

function getlanguageFilePath() {
    var storage = window.localStorage;
    var returnStr = storage["languagefilepath"];
    if (undefined != returnStr && returnStr.length > 0) {
        return returnStr;
    }
    else {
        return "zh-cn";
    }
}

/*
    功能：根据code获取公用common.xml对象的值
    参数：xml的code
*/
function getCommonInfo(code) {
    var val = '';
    if (xmlDocCommon != undefined && code != undefined && code.length > 0) {
        if (xmlDocCommon.length > 0) {
            if ($(xmlDocCommon).find(code).length > 0) {
                val = $(xmlDocCommon).find(code)[0].textContent;
            }
        }
        return val;
    }
}

/*
    功能：根据code获取页面xml对象的值
    参数：xml的code
*/
function getNameByCode(code) {
    var val = '';
    if (xmlDoc != null && code != null && code.length > 0) {
        if (xmlDoc.length > 0) {
            if ($(xmlDoc).find(code).length > 0) {
                val = $(xmlDoc).find(code)[0].textContent;
            }
        }
        return val;
    }
}

/*
    功能：设置当前语言
    参数：无
*/
function setlanguage() {
    var val = $('#language').val();
    Setlanguage(val);
    location.replace(location);
}
function Setlanguage(filename) {
    var storage = window.localStorage;
    storage["languagefilepath"] = filename;
}

$(document).ready(function () {
    textallselect();
}
);
function textallselect() {
    var parobj = arguments[0];
    if (parobj != undefined) {
        $(parobj).find("input[type='text']").bind('focus', function () { this.select(); });
    }
    else {
        $("body").find("input[type='text']").bind('focus', function () { this.select(); });
    }
}