//绑定菜谱
function BindMealInfo(index) {
    getListParameters.filter = "[status]='1' and stocode='" + $("#stocode").val() + "'";
    commonAjax('../../ajax/dishes/WSmeal.ashx', getpostParameters('getlist', getListParameters), true, function (data) {
        if (data != undefined) {
            BindSelect(data.data, 'sel_meal', 'melcode', 'melname', index);
        }
    });
}

//绑定菜品分类
function BindDisheTypeInfo(index) {
    getListParameters.filter = "[status]='1' and len(isnull(pdistypecode,''))=0 and stocode='" + $("#stocode").val() + "'";
    commonAjax('../../ajax/dishes/WSDisheType.ashx', getpostParameters('getlist', getListParameters), true, function (data) {
        if (data != undefined) {
            BindSelect(data.data, 'selDisheType', 'distypecode', 'distypename', index);
        }
    });

    $("#selDisheType").change(function () {
        $("#sel_dishetypetwo option").remove();
        if ($("#selDisheType").val() == "") {
            $("#sel_dishetypetwo").append("<option value=''>--全部--</option>");
        } else {
            //绑定菜品分类
            getListParameters.filter = "[status]='1' and pdistypecode='" + $("#selDisheType").val() + "' and stocode='" + $("#stocode").val() + "'";
            getListParameters.order = "";
            commonAjax('../../ajax/dishes/WSDisheType.ashx', getpostParameters('getlist', getListParameters), true, function (data) {
                if (data != undefined) {
                    BindSelect(data.data, 'sel_dishetypetwo', 'distypecode', 'distypename', 1);
                }
            });
        }
    });
}

//绑定制作厨房
function BindKitchenInfo(index) {
    getListParameters.filter = "[status]='1' and stocode='" + $("#stocode").val() + "'";
    commonAjax('../../ajax/dishes/WSkitchen.ashx', getpostParameters('getlist', getListParameters), false, function (data) {
        if (data != undefined) {
            BindSelect(data.data, 'selkitcode', 'kitcode', 'kitname', index);
        }
    });
}

//绑定制作厨师
function BindEmployeeInfo(index) {
    getListParameters.filter = "[status]='1' and strcode='" + $("#stocode").val() + "'";
    $("#sel_ecode").children().remove();
    if ($('#sel_ecode').val() == "") {
        $("#sel_ecode").append("<option value=''>--无--</option>")
    } else {
        commonAjax('../../ajax/dishes/WSEmployee.ashx', getpostParameters('getlist', getListParameters), false, function (data) {
            if (data != undefined) {
                BindSelect(data.data, 'sel_ecode', 'ecode', 'cname', index);
            }
        });
    }
}

//绑定单位下拉框
function BindUnitInfo(index) {
    getDictInfo(SystemDict.Unit, 'selunit', index, 'zh-cn');
}

//绑定财务类别
function BindFinanceTypeInfo(index) {
    getListParameters.filter = "[status]='1'";
    getListParameters.pageSize = 1000;
    commonAjax('../../ajax/dishes/WSFinanceType.ashx', getpostParameters('getlist', getListParameters), true, function (data) {
        if (data != undefined) {
            BindSelect(data.data, 'selfincode', 'fincode', 'finname', index);
        }
    });
}

//绑定出品部门
function BindDepartmentInfo(index) {
    if ($("#stocode").val() != null && $("#stocode").val() != undefined && $("#stocode").val().length > 0) {
        getListParameters.filter = "[status]='1' and pdcode <> '' and stocode='" + $("#stocode").val() + "'";
    } else {
        getListParameters.filter = "[status]='1' and pdcode <> ''";
    }
    commonAjax('../../ajax/dishes/WSDepartment.ashx', getpostParameters('getlist', getListParameters), false, function (data) {
        if (data != undefined) {
            BindSelect(data.data, 'seldcode', 'dcode', 'dname', index);
            setTimeout(BindEmployeeInfo, 1000);
        }
    });
}

//选择菜品类别
function ChooseDisheType() {
    ShowOpenpage('', 'DisheTyperefer.aspx?fun=setDisheTypeInfo&stocode=' + $("#stocode").val() + '', '80%', '550px', false);
}
function setDisheTypeInfo(code, name) {
    $('#distypecode').val(name);
    $('#hiddistypecode').val(code);
}

//选择菜品所属原料
function chooseStockMaterialrefer() {
    ShowOpenpage('', 'chooseStockMaterialrefer.aspx?fun=setStockMaterialInfo&stocode=' + $("#stocode").val() + '', '80%', '620px', false);
}
function setStockMaterialInfo(code, name) {
    $('#matcode').val(name);
    $('#hidmatcode').val(code);
    if ($("#iscaninventory").length > 0) {
        $('#iscaninventory').removeClass('active');
        $('#iscaninventory').toggleClass('active');
    }
}