var noload = $("body").attr("data-ischeck");
if (noload = undefined) {
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
    if (type == 'add') {
        title = "新增";
        switch (tbname) {
            case "maincoupon"://活动优惠券信息
            case "maincouponN"://活动优惠券信息
            case "StockTransfer":
                //gotoURL(tbname + 'edit.aspx?' + $('#HidParameter').val());
                ShowOpenpage(title, tbname + 'edit.aspx?' + $('#HidParameter').val(), '100%', '100%', true, true);
                break;
            case "memcardlevelset":
                ShowOpenpage(title, tbname + 'edit.aspx?code=' + $('#hidlevelcode').val(), '100%', '100%', true, true);
                break;
            case "dishes":
                ShowOpenpage(title, tbname + 'edit.aspx?stocode=' + $('#ddl_stocode').val(), '100%', '100%', true, true);
                break;
            default:
                ShowOpenpage(title, tbname + 'edit.aspx', '100%', '100%', true, true);
                //gotoURL(tbname + 'edit.aspx');
                break;
        }
        return false;
    } else if (type == "audit" && tbname == "IncomeAdjust") { return; }
    else if (type == "edit" && tbname == "IncomeAdjust") { return; }
    else if (type == "edit" && tbname == "NoApplyStorage") { return; }
    else if (type == "edit" && tbname == "StockJKReturnlist") { return; }
    else if (type == 'edit' && tbname == 'StockMateClass') { return; }
    else if (type == "inportmate" && tbname == "dishes") {
        ShowOpenpage(title, tbname + 'inportmate.aspx', '90%', '100%', true, true);
        return false;
    } else if (type == 'edit' && tbname == 'StockMateClass') { return; }
    else if (type == 'dishesmate') {
        if ($("#stocode_ids").val().length > 0) {
            ShowOpenpage(title, tbname + 'StoreDishesCostVarianceReportDetail1.aspx?matcode=' + $("#stocode_ids").val() + '&date=' + $("#ddl_ctype").val() + '&stocode=' + $("#ddl_stocode").val() + '&type=' + $("#stoname_ids").val(), '90%', '100%', true, true);
        }
        else {
            $("#hidmess").html("未选中有效行");
        }
        return false;
    }
    else if (type == 'mateconsume') {
        ShowOpenpage(title, tbname + 'StoreDishesCostConsume.aspx?discode=' + $("#ddl_dishes").val() + '&date=' + $("#ddl_ctype").val() + '&stocode=' + $("#ddl_stocode").val() + '&type=' + $("#stoname_ids").val(), '90%', '100%', true, true);
        return false;
    }
    else if (type == "downtemp" && tbname == "dishes") {
        //var a = document.createElement('a'); // 创建a标签
        //a.setAttribute('download', '');// download属性
        //a.setAttribute('href', '../Template/成本卡模板.xlsx');// href链接
        //a.click();// 自执行点击事件
        return true;
    }
    else if (type == "edit" && tbname == "problemclass") {
        title = "修改";
        ShowOpenpage(title, tbname + 'edit.aspx?id=' + $('#hidtreid').val() + '&pid=' + $('#hidtrepid').val(), '100%', '100%', true, true);
        return false;
    }
    else if (type == "edit" && tbname == "DishesMate") {
        var s = document.getElementById('gv_list').getElementsByTagName('input');
        var role_id = "";
        var stocodes = "";
        if (s != undefined) {
            for (var i = 0; i < s.length; i++) {
                try {
                    if (s[i].type == 'checkbox' && s[i].attributes[3].value == 'checked') {
                        if (s[i].id.indexOf('CB_Select') < 0) {
                            continue;
                        }
                        num++;
                        var table = document.getElementById('gv_list');
                        var keyid = s[i + 1].id;
                        var stocode = s[i + 2].id;
                        role_id += $('#' + keyid).val() + ",";
                        stocodes = $('#' + stocode).val();
                    }
                } catch (e) {

                }

            }
        }
        role_id = role_id.toString().substr(0, role_id.length - 1);
        if (role_id.indexOf(',') >= 0) {
            $("#sp_showmes").html("一次只能修改或查看一条数据");
            return false;
        }
        if (role_id == "") {
            $("#sp_showmes").html("请选择要操作的数据");
            return false;
        }
        console.log(stocodes);
        ShowOpenpage(title, tbname + 'Edit.aspx?type=edit&discode=' + role_id + '&stocode=' + stocodes, '90%', '100%', true, true);
        return false;
    }
    else if (type == "info" && tbname == "DishesMate") {
        var s = document.getElementById('gv_list').getElementsByTagName('input');
        var role_id = "";
        var stocodes = "";
        if (s != undefined) {
            for (var i = 0; i < s.length; i++) {
                try {
                    if (s[i].type == 'checkbox' && s[i].attributes[3].value == 'checked') {
                        if (s[i].id.indexOf('CB_Select') < 0) {
                            continue;
                        }
                        num++;
                        var table = document.getElementById('gv_list');
                        var keyid = s[i + 1].id;
                        var stocode = s[i + 2].id;
                        role_id += $('#' + keyid).val() + ",";
                        stocodes = $('#' + stocode).val();
                    }
                } catch (e) {

                }

            }
        }
        role_id = role_id.toString().substr(0, role_id.length - 1);
        if (role_id.indexOf(',') >= 0) {
            $("#sp_showmes").html("一次只能修改或查看一条数据");
            return false;
        }
        if (role_id == "") {
            $("#sp_showmes").html("请选择要操作的数据");
            return false;
        }
        ShowOpenpage(title, tbname + 'Info.aspx?type=info&discode=' + role_id + '&stocode=' + stocodes, '90%', '100%', true, true);
        return false;
    }
    else if (type == "cardexport" && tbname == 'dishes') {
        var s = document.getElementById('gv_list').getElementsByTagName('input');
        var role_id = "";
        var stocodes = "";
        if (s != undefined) {
            for (var i = 0; i < s.length; i++) {
                if (s[i].type == 'checkbox' && s[i].checked) {
                    if (s[i].id.indexOf('CB_Select') < 0) {
                        continue;
                    }
                    num++;
                    var table = document.getElementById('gv_list');
                    var keyid = s[i + 1].id;
                    var stocode = s[i + 2].id;
                    role_id += $('#' + keyid).val() + ",";
                    stocodes += $('#' + stocode).val();
                }
            }
        }
        role_id = role_id.toString().substr(0, role_id.length - 1);
        $("#discodes").val(role_id);
        return true;
    }
    else if (type == "edit" && tbname == "dishes") {
        var s = document.getElementById('gv_list').getElementsByTagName('input');
        var role_id = "";
        var stocodes = "";
        if (s != undefined) {
            for (var i = 0; i < s.length; i++) {
                if (s[i].type == 'checkbox' && s[i].checked) {
                    if (s[i].id.indexOf('CB_Select') < 0) {
                        continue;
                    }
                    num++;
                    var table = document.getElementById('gv_list');
                    var keyid = s[i + 1].id;
                    var stocode = s[i + 2].id;
                    role_id += $('#' + keyid).val() + ",";
                    stocodes = $('#' + stocode).val();
                }
            }
        }
        role_id = role_id.toString().substr(0, role_id.length - 1);
        if (role_id.indexOf(',') >= 0) {
            $("#sp_showmes").html("一次只能修改或查看一条数据");
            return false;
        }
        if (role_id == "") {
            $("#sp_showmes").html("请选择要操作的数据");
            return false;
        }
        ShowOpenpage1(title, tbname + 'edit.aspx?type=edit&discode=' + role_id + '&stocode=' + stocodes, '90%', '100%', true, true);
        return false;
    }
    else if (type == "edit" && tbname == "ReportType") {
        var title = "修改";
        id = $('#hidtreid').val();
        if (id) {
            ShowOpenpage(title, tbname + type + '.aspx?id=' + id, '100%', '100%', true, true);
        } else {
            $("#sp_showmes").html("请选择要操作的数据");
        }
        return false;
    }
    else if (type == "delete" && tbname == "ReportType") {
        id = $('#hidtreid').val();
        if (id) {
            return confirm(getCommonInfo('delete_button_report'));
        } else {
            $("#sp_showmes").html("请选择要删除的数据");
        }
        return false;
    }
    else if ((type == 'recharge' && tbname == 'MemCard') || (type == 'recharge' && tbname == 'MemCardBig')) {
        return;
    }
    else if (type == 'free' || type == 'bactfree') {
        title = "赠送";
        ShowOpenpage(title, tbname + 'edit.aspx', '100%', '100%', true, true);
        //gotoURL(tbname + 'edit.aspx');
        return false;
    }
    else if (type == "scpandian" && tbname == 'SCStockCounting') {
        return;
    }
    else if (type == "scpandian" && tbname == 'SCStockCountingDay') { return; }
    else if (type == "audit" && tbname == "StockCounting") {
        return;
    }
    else if (type == "audit" && tbname == "memberCardCounting") {
        return;
    }
    else if (type == "audit" && tbname == "SupplierTicket") {
        return;
    }
    else if (type == "audit" && tbname == "StockSemiProOut") {
        return;
    }
    else if (type == "audit" && tbname == "StockTransfer") {
        return;
    }
    else if (type == "reaudit") { return confirm(getCommonInfo('reaudit_button_tip')); }
    else if (type == "sensitive" && tbname == 'MVComment') {
        return;
    }
    else if (type == "scorerel" && tbname == 'MVComment') {
        return;
    }
    else if (type == "audit" && tbname == "StockCostBalance") {
        return;
    } else if (type == "audit" && tbname == "StockSemiPro") {
        return;
    } else if (type == "audit" && tbname == "StockSemiProOut") {
        return;
    } else if ((type == "loguser" || type == "audit") && tbname == "ProAuthAudit") {
        return;
    } else if (type == "untying" && tbname == "ProAuthAudit") {
        return confirm(getCommonInfo('untying_button_tip'));
    }
    else if (type == "scpandian" && tbname == 'SCmemberCardCounting') {
        return;
    } else if (type == "info" && tbname == "MVmarketingactivity") {
        return;
    } else if (type == "audit" && tbname == "MVmarketingactivity") {
        return;
    }
        //广告活动
    else if (type == "edit" && tbname == "MVbanner") {
        return;
    }
    else if (type == "info" && tbname == "MVbanner") {
        return;
    }
        //生日赠送
    else if (type == "edit" && tbname == "birthsent") {
        return;
    }
    else if (type == "info" && tbname == "birthsent") {
        return;
    }
    else if (type == "edit" && tbname == "subcard") {
        return;
    }
    else if (type == "info" && tbname == "subcard") {
        return;
    }
    else if (type == "edit" && tbname == "coupontipsrule") {
        return;
    }
    else if (type == "info" && tbname == "coupontipsrule") {
        return;
    }
    else if (type == "audit" && tbname == "MVDraw") {
        return;
    }
    else if (type == "info" && tbname == "subcardinfo") {
        return;
    }
    else if (type == "info" && tbname == "dishes") {
        var s = document.getElementById('gv_list').getElementsByTagName('input');
        var role_id = "";
        var stocodes = "";
        if (s != undefined) {
            for (var i = 0; i < s.length; i++) {
                if (s[i].type == 'checkbox' && s[i].checked) {
                    if (s[i].id.indexOf('CB_Select') < 0) {
                        continue;
                    }
                    num++;
                    var table = document.getElementById('gv_list');
                    var keyid = s[i + 1].id;
                    var stocode = s[i + 2].id;
                    role_id += $('#' + keyid).val() + ",";
                    stocodes = $('#' + stocode).val() + ",";
                }
            }
        }
        role_id = role_id.toString().substr(0, role_id.length - 1);
        if (role_id.indexOf(',') >= 0) {
            $("#sp_showmes").html("一次只能修改一条数据");
            return false;
        }
        if (role_id == "") {
            $("#sp_showmes").html("请选择要操作的数据");
            return false;
        }
        ShowOpenpage1(title, tbname + 'edit.aspx?type=info&discode=' + role_id + '&stocode=' + stocodes, '90%', '100%', true, true);
        return false;
    }
    else if (type == "copy" && tbname == "dishes") {
        var s = document.getElementById('gv_list').getElementsByTagName('input');
        var role_id = "";
        var stocodes = "";
        if (s != undefined) {
            for (var i = 0; i < s.length; i++) {
                if (s[i].type == 'checkbox' && s[i].checked) {
                    if (s[i].id.indexOf('CB_Select') < 0) {
                        continue;
                    }
                    num++;
                    var table = document.getElementById('gv_list');
                    var keyid = s[i + 1].id;
                    var stocode = s[i + 2].id;
                    role_id += $('#' + keyid).val() + ",";
                    stocodes += $('#' + stocode).val() + ",";
                }
            }
        }
        role_id = role_id.toString().substr(0, role_id.length - 1);
        stocodes = stocodes.toString().substr(0, stocodes.length - 1);
        if (role_id.indexOf(',') >= 0) {
            $("#sp_showmes").html("一次只能修改一条数据");
            return false;
        }
        if (role_id == "") {
            $("#sp_showmes").html("请选择要操作的数据");
            return false;
        }
        ShowOpenpage(title, tbname + 'edit.aspx?type=copy&discode=' + role_id, '90%', '100%', true, true);
        return false;
    }
    else if (type == "edit" && tbname == "MVmarketingactivity") {
        return;
    }
    else if (type == "scpandian") {
        title = "盘点";
        ShowOpenpage(title, tbname + 'scpandian.aspx', '100%', '100%', true, true);
        //gotoURL(tbname + "scpandian.aspx");
        return false;
    } else if (type == "build") {
        return;
    } else if (type == "buildtotal") {
        return;
    } else if (type == "instore") {
        return;
    } else if (type == "total") {
        return;
    } else if (type == "outstock" || type == "downloadyltemplate" || type == "importyldata" || type == "viewerq") { return; }
    else if (type == "materialinfo" && tbname == "SupplierStore") {
        return;
    }
    else if (type == "materialinfo" || type == "modifysupp" || type == "storecon") {
        return;
    }

    if (tbname == 'Department' || tbname == 'ts_Dicts') {
        return;
    }
    var Flag = true;
    var num = 0;
    var id = '';
    var s = document.getElementById('gv_list').getElementsByTagName('input');
    if (s != undefined) {
        for (var i = 0; i < s.length; i++) {
            if (s[i].type == 'checkbox' && s[i].checked) {
                if (s[i].id.indexOf('CB_Select') < 0) {
                    continue;
                }
                num++;
                var table = document.getElementById('gv_list');
                var keyid = s[i + 1].id;
                id += $('#' + keyid).val() + ",";

            }
        }
    }
    id = id.toString().substr(0, id.length - 1);
    if (num < 1) {
        showobj.innerText = getCommonInfo('nochoose_button_tip');
        Flag = false;
    }
    else {
        if (type == 'oldactive') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            return getsyscard();
        }
        else if (type == 'printbalance') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            pageprint(id);
            return false;
        }
        else if (type == 'edit') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            switch (tbname) {
                case "maincoupon"://活动优惠券信息
                case "maincouponN"://活动优惠券信息
                    title = "优惠券信息";
                    ShowOpenpage(title, tbname + 'edit.aspx?id=' + id + "&" + $('#HidParameter').val(), '100%', '100%', true, true);
                    //gotoURL(tbname + 'edit.aspx?id=' + id + "&" + $('#HidParameter').val());
                    break;
                case "couponpresent":
                    return true;
                    break;
                case "StockApply":
                    break;
                case "StockReturn":
                    break;
                case "StockBack":
                    break;
                case "StockOut":
                    break;
                case "StockTransfer":
                    break;
                case "StockLoss":
                    break;
                case "StockStaffMeals":
                    break;
                case "memcardLoss":
                    break;
                case "memberCardSales":
                    break;
                case "sumcoupon":
                    break
                case "sumcouponN":
                    return true;
                    break
                case "StockStorage":
                    break;
                case "StockPreApply":
                    break;
                case "memberCardTransfer":
                    break;
                case "StockJKlist":
                    break;
                case "MemCardBig":
                    title = "会员卡信息";
                    ShowOpenpage(title, 'MemCardedit.aspx?id=' + id + '&types=big', '100%', '100%', true, true);
                    //gotoURL('MemCardedit.aspx?id=' + id + '&types=big');
                    break;
                default:
                    //gotoURL(tbname + type + '.aspx?id=' + id);
                    var title = "修改";
                    ShowOpenpage(title, tbname + type + '.aspx?id=' + id, '100%', '100%', true, true);
                    break;
            }
            if (tbname != "StockApply" && tbname != "StockReturn" && tbname != "StockTransfer" && tbname != "StockLoss" && tbname != "StockStaffMeals" && tbname != "memcardLoss" && tbname != "memberCardSales" && tbname != "sumcoupon" && tbname != "StockStorage" && tbname != "StockPreApply" && tbname != "StockBack" && tbname != "StockOut" && tbname != "memberCardTransfer" && tbname != "StockJKlist" && tbname != "NoApplyStorage") {
                return false;
            }
        }
        else if (type == 'info') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            var title = "详情";
            ShowOpenpage(title, tbname + type + '.aspx?id=' + id, '100%', '100%', true, true);
            return false;
        }
        else if (type == 'upload') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            var title = "上传";
            ShowOpenpage(title, tbname + type + '.aspx?id=' + id, '100%', '100%', true, true);
            return false;
        }
        else if (type == 'chargoff' && tbname == 'memcardcredit') {
            return;
        } else if (type == 'chargoff' && tbname == 'StockJKlist') {
            return;
        } else if (type == 'pay' && tbname == 'StockJKlist') {
            return;
        } else if (type == 'backpay' && tbname == 'StockJKlist') {
            return;
        }
        else if (type == 'chargoff' && tbname == 'memcardsign') {
            return;
        }
        else if (type == 'chargoff' && tbname != 'memcardcredit')//核销
        {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            gotoURL(tbname + 'Charg.aspx?id=' + id);
            return false;
        }
        else if (type == 'copying' && tbname == 'ReportTypeRelation') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            gotoURL(tbname + 'Edit.aspx?copying=1&id=' + id);
            return false;
        }
        else if (type == 'copying') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            gotoURL(tbname + 'Edit.aspx?copying=1&id=' + id);
            return false;
        }
        else if (type == 'memedit') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            if (tbname == "MemCardBig") {
                gotoURL('membersEdit.aspx?from=memcard&id=' + id + '&type=bigmemcard');
            } else {
                gotoURL('membersEdit.aspx?from=memcard&id=' + id);
            }
            return false;
        }
        else if (type == 'nullif' && tbname == "subcardinfo") {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            return confirm(getCommonInfo('nullifysubcard_button_tip'));
        }
        else if (type == 'nullifynotuse') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            return confirm(getCommonInfo('nullifynotuse_button_tip'));
        }
        else if (type == "delete" && tbname == "cbtotalmanagelist") {
            return;
        }
        else if (type == "delete" && tbname == "cbmanagelist") {
            return;
        }
        else if (type == "delete" && tbname == "ReportTypeRelation") {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            return confirm(getCommonInfo('delete_button_reportrelation'));
        }
        else if (type == 'delete') {
            if (tbname == "MVComment") {
                ShowReferPage('', '', "删除", '../common/ConfirmPage.aspx?content=' + getCommonInfo('deleteComment_button_tip') + '&change=confirmReturn&valid=valid', 1, '640px', '190px');
                return false;
            }
            return confirm(getCommonInfo('delete_button_tip'));
        }
        else if (type == 'clearphone') {
            if (tbname == "WXMembers") {
                if (num > 1) {
                    showobj.innerText = getCommonInfo('singlechoose_button_tip');
                    return false;
                }
                return confirm(getCommonInfo('clearphone_button_tip'));
            }
        }
        else if (type == 'untying') {
            if (tbname == "WXMembers") {
                if (num > 1) {
                    showobj.innerText = getCommonInfo('singlechoose_button_tip');
                    return false;
                }
                return true;
            }
            return false;
        }
        else if (type == 'cancel' && tbname == "MVticketsOrder") {
            return confirm(getCommonInfo('cancel_MVticketsOrder_tip'));
        }
        else if (type == "nullify") {
            if (tbname == "sumcoupon") {
                return confirm(getCommonInfo('nullify_button_tip'));
            }
            else {
                return confirm(getCommonInfo('nullify_button_tipC'));
            }
        } else if (type == 'releaseInfo') {
            return confirm(getCommonInfo('releaseinfo_button_tip'));
        } else if (type == 'repaircard') {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            var result = confirm(getCommonInfo('repaircard_button_tip'));
            if (result) {  //gotoURL('memCardRepair.aspx?id=' + id);
                var title = getNameByCode('pepair_refer');
                ShowReferPage(id, '', title, 'memCardRepair.aspx', 1, '70%', '80%');
                return false;
            }
        } else if (type == 'freeze') {
            //冻结
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            else {
                var title = getNameByCode('memcard_reffreeze');
                ShowReferPage(id, '', title, 'memCardfreeze.aspx?id=' + id, 1, '60%', '60%');
                return false;

            }
        } else if (type == 'merge') {
            //合并卡

            if (num <= 1) {
                //提示 数量不对
                showobj.innerText = getCommonInfo('minonechoose_button_tip');
                Flag = false;
            } else {
                var title = getCommonInfo('member_refer');

                gotoURL('memCardMerge.aspx?id=' + id);
                return false;

            }
        } else if (type == "changecard") {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            var result = confirm(getCommonInfo('changecard_button_tip'));
            if (result) {  //gotoURL('memCardRepair.aspx?id=' + id);
                var title = getCommonInfo('change_refer');
                ShowReferPage(id, '', title, 'memCardChange.aspx', 1, '70%', '80%');
                return false;
            }
        }
        else if (type == "invalid") {//无效
            /*if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }*/
            if (tbname == 'Employee') {
                if (confirm(getCommonInfo('invalid_button_tip'))) {
                    return confirm(getCommonInfo('invalid_button_tip2'));
                }
                else {
                    return false;
                }
            }
            else {
                return true;
            }
        }
        else if (type == "transfer") {//过户
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            var result = confirm(getNameByCode('memcard_comtransfer'));
            if (result) {  //gotoURL('memCardRepair.aspx?id=' + id);
                var title = getNameByCode('memcard_reftransfer');
                ShowReferPage(id, '', title, 'memCardtransfer.aspx?id=' + id, 1, '70%', '80%');
                return false;
            }
        }
        else if (type == "bindmember") {//重新绑定会员
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            var title = getNameByCode('memcard_refbindmember');
            ShowOpenpage(title, 'memCardbindmember.aspx?id=' + id, '80%', '80%', false);
            return false;
        }
            //处理
        else if (type == 'deal' && tbname == 'memcardreturn') {
            return;
        }
        else if (type == 'deal' && tbname != "memcardreturn") {
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            gotoURL(tbname + 'edit.aspx?id=' + id);
            return false;
        }
            //审核
        else if (type == 'audit' || type == 'auditing') {
            title = "审核";
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            switch (tbname) {
                case "maincoupon"://活动优惠券信息             
                    ShowOpenpage(title, tbname + 'edit.aspx?id=' + id + "&" + $('#HidParameter').val(), '100%', '100%', true, true);
                    break;
                case "birthsent"://生日赠送活动信息 
                case "subcard"://次卡设置活动信息 
                case "coupontipsrule": //优惠券过期提醒
                case "sumcouponN"://活动优惠券信息 
                case "marketingN":
                    return true;
                    break;
                case "sumcoupon"://活动优惠券信息                    
                    ShowOpenpage(title, 'maincouponaudit.aspx?id=' + id, '100%', '100%', true, true);
                    break;
                case "couponpresent"://赠送方案审核                 
                    ShowOpenpage(title, '/coupon/GoToAuditing.aspx?formpage=couponpresent&id=' + id, '600px', '400px', false, true);
                    break;
                case "StockPayOrder":
                    break;
                case "memberBind":
                    ShowOpenpage('会员绑定审核', 'memberbindAuding.aspx?id=' + id, '500px', '300px', false, true);
                    break;
                default:
                    ShowOpenpage(title, tbname + type + '.aspx?id=' + id, '100%', '100%', true, true);
                    break;
            }
            if (tbname != "StockPayOrder") {
                return false;
            }
        }
        else if (type == 'detail' && (tbname == "MachRecord" || tbname == "SupplierTicket")) {
            var title = "明细";
            if (num > 1) {
                showobj.innerText = getCommonInfo('singlechoose_button_tip');
                return false;
            }
            ShowOpenpage(title, tbname + type + '.aspx?type=detail&id=' + id, '100%', '100%', true, true);
            return false;
        }
    }
    return Flag;
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
            //if (_btn != undefined) {
            //    _btn.click();
            //}
            //parent.location.reload();
            parent.layer.closeAll("iframe");
        } else {
            var tbname = $("#form1").attr("data-tbname");
            var url = tbname + "list.aspx";
            switch (tbname) {
                case "maincoupon":
                case "maincouponN":
                    url = tbname + 'list.aspx?' + $('#HidParameter').val();
                    break;
                case "StockSemiProOut":
                case "StockTransfer":
                case "StockSemiPro":
                    url = tbname + 'list.aspx?type=' + $("#type_ids").val();
                    break;
                case "MemCard":
                    if ($("#isbigcusedit").val() == "big") {
                        url = tbname + 'biglist.aspx';
                    } else {
                        url = tbname + 'list.aspx';
                    }
                    break;
                case "members":
                    if ($("#types").val() == "big") {
                        url = 'MemCardbiglist.aspx';
                    } else {
                        url = 'MemCardlist.aspx';
                    }
                    break;
            }
            location.href = url;
        }
    })
    //返回上一级
    $(".backbtn").click(function () {
        location.href = document.referrer;
    });
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

//列表页事件
$(document).ready(function () {
    intchangecolor();
});

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

//网页读卡
function getreadcard(cardobj) {
    if (document.getElementById(cardobj).value.length == 0) {
        if (cardobj.length > 0) {
            var strresult = document.getElementById("readcard").GetCar();
            var arrres = strresult.split('$');
            if (arrres.length >= 2) {
                if (arrres[0] != "0") {
                    pcLayerMsg(arrres[1]);
                    return false;
                }
                else {
                    document.getElementById("hidreadcard").value = arrres[1];
                }
            }
        }
    }
    return true;
}