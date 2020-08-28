function CheckAll(obj) {

    var flag = false
    var chearr = document.getElementById('MenuList').getElementsByTagName("input");
    if (document.getElementById("chkAll").checked) {
        flag = true;
    }
    for (var k = 0; k < chearr.length; k++) {
        var t = chearr[k].type;
        if (t == "checkbox" && chearr[k].id != 'chkAll') {
            chearr[k].checked = flag;
        }
    }
}
function setMenuuMain(obj) {
    var id = obj.id;
    var strid = '';
    var checkval = false;
    if (document.getElementById(id).checked == true) {
        checkval = true;
        document.getElementById('chkAll').checked = true;

        var idsArr = id.split('_');
        for (var i = 0; i < idsArr.length - 1; i++) {
            strid += idsArr[i];
            document.getElementById(strid).checked = true;
            strid += '_';
        }
    }
    //设置菜单子项状态
    SetItemsMenuCheck(id, checkval);
}


function CheckCheckedAll(id, iid) {
    var chearr = document.getElementById('MenuList').getElementsByTagName("input");
    var flag = false;
    for (var k = 0; k < chearr.length; k++) {
        var t = chearr[k].type;
        if (t == "checkbox" && chearr[k].id != 'chkAll') {
            if (chearr[k].checked) {
                flag = true;
            }
        }
    }
    document.getElementById('chkAll').checked = flag;
}

function SetItemsMenuCheck(id, checked) {
    var chearr = document.getElementById('MenuList').getElementsByTagName("input");
    for (var k = 0; k < chearr.length; k++) {
        var id2 = chearr[k].id;
        var t = chearr[k].type;
        if (t == "checkbox") {
            if (id2 != id && id2.indexOf(id+'_') >= 0) {
                chearr[k].checked = checked;
            }
        }
    }
    if (checked == false)
    {
        while (id.lastIndexOf('_') >= 0)
        {
            id = id.substring(0, id.lastIndexOf('_'));
            falsecheckbox(id);
        }
    }
}

function falsecheckbox(pid)
{
    var chearr = document.getElementById('MenuList').getElementsByTagName("input");
    var flag = false;
    for (var k = 0; k < chearr.length; k++) {
        var id2 = chearr[k].id;
        var t = chearr[k].type;
        if (t == "checkbox") {
            if (id2 != pid && id2.indexOf(pid) >= 0) {
                if (chearr[k].checked == true)
                {
                    flag = true;
                }
            }
        }
    }
    if (!flag)
    {
        document.getElementById(pid).checked = false;
        if (pid.indexOf('_') < 0)
        {
            document.getElementById('chkAll').checked = false;
        }
    }
}

function GetFunIdStr() {
    var funId = "";
    var chearr = document.getElementById('MenuList').getElementsByTagName("input");
    for (var k = 0; k < chearr.length; k++) {
        var t = chearr[k].type;
        if (t == "checkbox" && chearr[k].id != 'chkAll') {
            if (chearr[k].checked) {
                funId += chearr[k].value + ',';
            }
        }
    }
    $("#HidfunIdStr").val(funId);
}
function clickrad() {
    var Pval = $("input[name='Rstatus']:checked").val();
    $("#rol").val(Pval);
}