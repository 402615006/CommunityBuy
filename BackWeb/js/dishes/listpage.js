//选中项
var arrChoose = new Array();

var newdata =
{
    "status": '-1',
    "mes": ''
}

// 查询请求参数
var getListParameters = {
    "GUID": getLoginUserInfo().GUID,
    "USER_ID": getLoginUserInfo().userid,
    "pageSize": 10,
    "currentPage": 1,
    "filter": "",
    "order": "",
    "roleid": "",
    "stocode": "",
    "type": "0"
};

// 排序
var listOrder = {
    "IsNew": true,
    "Expression": "",
    "Order": ""
};

//列表刷新按钮
function refreshclick() {
    //getlistCallback(getListParameters.currentPage);
    PageClick('search');
}

//执行排序
function getOrderInfo(obj, Expression) {
    var strOrder = Expression + ' ';
    if (listOrder.Expression == Expression) {
        if (listOrder.Order == 'asc')//降序
        {
            listOrder.Order = 'desc';
        }
        else//升序
        {
            listOrder.Order = 'asc';
        }
    }
    else {
        listOrder.Order = 'asc';
    }
    if (Expression == listOrder.Expression) {
        listOrder.IsNew = false;
    }
    else {
        listOrder.IsNew = true;
    }
    listOrder.Expression = Expression;
    strOrder += listOrder.Order;
    getListParameters.order = strOrder;
    getlistCallback(getListParameters.currentPage);
}

//添加排序样式
function addListOrder(id) {
    if (id == undefined || id.length == 0) {
        id = '#gridList thead tr th';
    }
    else {
        id = '#' + id + ' thead tr th';
    }
    //添加排序函数
    var $thArr = $(id);
    $thArr.each(function (index, el) {
        var expression = $(el).attr('data-expression');
        if (expression !== undefined) {
            $(this).append('<i class="icon-order"></i>');
            $(this).addClass('icon-order-pointer');
            $(this).on('click', function () {
                getOrderInfo(this, expression);
            });
            var imgicon = $(this).children('i');
            if (imgicon != undefined) {
                imgicon.removeClass();
                if (expression == listOrder.Expression) {
                    imgicon.addClass('icon-order');
                    if (listOrder.IsNew == false) {
                        if (listOrder.Order == 'desc') {
                            $(imgicon).css('transform', 'rotate(180deg)');
                        }
                    }
                }
                else {
                    imgicon.addClass('icon-order-normal');
                }
            }
        }
    });
}

// 表格菜品单击代理事件绑定
function addTableclick(id) {
    if (id == undefined || id.length == 0) {
        id = '#gridList tbody tr';
    }
    else {
        id = '#' + id + ' tbody tr';
    }
    var $thArr = $(id);
    var url = window.location.href;
    if (url.indexOf("sto_rolesEdit_SelRdiscount22") != -1) {
        var tempArrchoose = "";
        $thArr.each(function (index, el) {
            var html = '<tr>' + $(el).html() + '</tr>';
            if (el !== undefined) {
                $(el).on('click', function () {
                    tempArrchoose = "";
                    for (var i = 0; i < arrChoose.length; i++) {
                        if (arrChoose[i].innerHTML != undefined && arrChoose[i].innerHTML.indexOf("<th>") == -1) {
                            tempArrchoose += arrChoose[i].firstChild.firstChild.textContent + "|";
                        }
                    }
                    var tempHtml = $(html).find("td").eq(0).html();
                    if (tempArrchoose.indexOf(tempHtml) < 0) {
                        $(el).toggleClass('current');
                        arrChoose.push(html); //不存在 插入进arrChoose

                    } else if (tempArrchoose.indexOf(tempHtml) >= 0) {
                        $(el).toggleClass('current');
                        if (tempArrchoose.length > 0)
                            for (var i = 0; i < tempArrchoose.split('|').length; i++) {
                                if (tempArrchoose.split('|')[i] == tempHtml) {
                                    arrChoose.splice(i, 1);// 存在 择删除
                                }
                            }

                    }
                });
            }
        });
        return;
    }
    $thArr.each(function (index, el) {
        var html = '<tr>' + $(el).html() + '</tr>';
        if (el !== undefined) {
            $(el).on('click', function () {
                if (arrChoose.indexOf(html) < 0) {
                    $(el).toggleClass('current');
                    arrChoose.push(html); //不存在 插入进arrChoose

                } else if (arrChoose.indexOf(html) >= 0) {
                    $(el).toggleClass('current');
                    arrChoose.splice(arrChoose.indexOf(html), 1);// 存在 择删除
                }
            });
        }
    });
}


// 选中arrChoose 按列和行查询 返回值
function getArrChooseValue(row, col) {
    if (arrChoose === undefined) {
        return;
    }
    var tdArr = $(arrChoose[row]).find('td');
    var tdValue = $(tdArr[col]).text();

    return tdValue;
}

function getArrChooseValues(row, col, key) {
    if (arrChoose === undefined) {
        return;
    }
    var tdArr = $(arrChoose[row]).find('td');
    var tdValue = $(tdArr[col]).attr(key);

    return tdValue;
}