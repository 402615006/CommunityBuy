//接口地址
var interfaceUrl = 'IStore/WSdishes.ashx';

//页面初始化函数
$(document).ready(function () {
    BindMeal();
    // 回收站状态切换
    var recyble = {
        el: $('#searchrecyble'),
        state: false,
        onFn: function () {
            console.log('开启回收站');
        },
        offFn: function () {
            console.log('关闭回收站');
        },
        changeState: function () {
            if (!recyble.state) {
                recyble.state = !recyble.state;
                recyble.el.addClass('on');
                recyble.onFn();
            } else {
                recyble.state = !recyble.state;
                recyble.el.removeClass('on');
                recyble.offFn();
            }
        }
    }

    // 回收站单击事件绑定
    recyble.el.on('click', function () {
        recyble.changeState();
    });

    //绑定菜谱下拉框
    BindMealInfo(1);

    //绑定菜品一级分类下拉框
    BindDisheTypeInfo(1);

    //绑定财务类别下拉框
    BindFinanceTypeInfo(1);

    //绑定状态下拉框
    getEnumInfo(SystemEnum.Status, 'selStatus', 1, 'zh-cn');

    // 首次请求菜品列表
    getListParameters.filter = " isdelete='0' and iscombo='0' and stocode='" + getLoginUserInfo().stocode + "'";
    getListParameters.order = " melcode,distypecode desc";
    getlistCallback(1);
});

//绑定菜谱
function BindMeal() {
    getListParameters.pageSize = 100000;
    getListParameters.filter = "[status]='1' and stocode='" + getLoginUserInfo().stocode + "'";
    getListParameters.stocode = getLoginUserInfo().stocode;
    commonAjax('../../ajax/dishes/WSmeal.ashx', getpostParameters('getlist', getListParameters), true, function (data) {
        if (data != undefined) {
            BindSelect(data.data, 'sel_meal', 'melcode', 'melname', 1);
        }
    });
}

var where = '1=1 and iscombo=\'0\' ';
//按钮点击事件
function PageClick(commondname) {
    switch (commondname) {
        //搜索
        case "search":
            where = "1=1 and iscombo='0' and stocode='" + getLoginUserInfo().stocode + "'";
            var flag = $('#searchrecyble').is('.on');
            //拼写where条件
            if (flag)//是否显示删除数据
            {
                where += " and isdelete='1'";
            }
            else {
                where += " and isdelete='0'";
            }
            //菜谱搜索
            var strmenu1 = $('#sel_meal').val();
            if (strmenu1.length > 0) {
                where += " and (melcode ='" + strmenu1 + "' or discode in(SELECT discode FROM dbo.DishesMeal WHERE melcode='" + strmenu1 + "')) and stocode='" + getLoginUserInfo().stocode + "'"
            }
            //菜品分类搜索
            var strmenu = $('#sel_dishetypetwo').val();
            if (strmenu.length > 0) {
                where += " and distypecode ='" + strmenu + "'"
            } else {
                strmenu = $('#selDisheType').val();
                if (strmenu.length > 0) {
                    where += " and distypecode in (SELECT distypecode FROM dbo.DisheType WHERE pdistypecode='" + strmenu + "')"
                }
            }
            //财务类别搜索
            var strfincode = $('#selfincode').val();
            if (strfincode.length > 0) {
                where += " and fincode='" + strfincode + "'"
            }
            //状态
            var strstatus = $('#selStatus').val();
            if (strstatus.length > 0) {
                where += " and status='" + strstatus + "'"
            }
            //名称/速查码/自定义编号
            var strcname = $('#txt_disname').val();
            if (strcname.length > 0) {
                where += " and (disname like '%" + strcname + "%' or quickcode like '%" + strcname + "%' or customcode like '%" + strcname + "%')"
            }
            getListParameters.filter = where;
            //从第一页开始
            getlistCallback(1);
            break;
            //添加
        case "add":
            ShowOpenpage('', 'dishesedit.html', "90%", "90%", true);
            break;
            //修改
        case "edit":
            if (arrChoose.length == 0) {
                layer.msg(getCommonInfo('nochoose_button_tip'));
                return;
            }
            else if (arrChoose.length > 1) {
                layer.msg(getCommonInfo('singlechoose_button_tip'));
                return;
            }
            ShowOpenpage('', 'dishesedit.html?type=details&discode=' + getArrChooseValue(0, 0), "90%", "90%", true);
            break;
            //删除
        case "delete":
            if (arrChoose.length == 0) {
                layer.msg(getCommonInfo('nochoose_button_tip'));
                return;
            }
            if (confirm(getCommonInfo('delete_button_tip'))) {
                var newdata = { "status": "-1", "mes": getCommonInfo('fail_status') };
                var Flag = true;
                for (var i = 0; i < arrChoose.length; i++) {
                    commonAjax(interfaceUrl, getpostParameters('delete', { "GUID": "", "USER_ID": "", "opeid": getArrChooseValue(i, 0), "discode": getArrChooseValue(0, 0) }), false, function (data) {
                        if (data == undefined) {
                            Flag = false;
                        }
                        else {
                            if (data.status == 0) {
                                newdata = data;
                                Flag = false;
                            }
                        }
                    });
                    if (Flag == false) {
                        break;
                    }
                }
                ShowResult(newdata);
                getlistCallback(getListParameters.currentPage);
            }
            break;
            //复制
        case "copy":
            if (arrChoose.length == 0) {
                layer.msg(getCommonInfo('nochoose_button_tip'));
                return;
            }
            else if (arrChoose.length > 1) {
                layer.msg(getCommonInfo('singlechoose_button_tip'));
                return;
            }
            ShowOpenpage('', 'dishesedit.html?type=copy&discode=' + getArrChooseValue(0, 0));
            break;
            //导出
        case "export":
            dataloading1("导出中，请稍后...");
            where = "1=1 and iscombo='0' and stocode='" + getLoginUserInfo().stocode + "'";
            var flag = $('#searchrecyble').is('.on');
            //拼写where条件
            if (flag)//是否显示删除数据
            {
                where += " and isdelete='1'";
            }
            else {
                where += " and isdelete='0'";
            }
            //菜谱搜索
            var strmenu1 = $('#sel_meal').val();
            if (strmenu1.length > 0) {
                where += " and (melcode ='" + strmenu1 + "' or discode in(SELECT discode FROM dbo.DishesMeal WHERE melcode='" + strmenu1 + "')) and stocode='" + getLoginUserInfo().stocode + "'"
            }
            //菜品分类搜索
            var strmenu = $('#sel_dishetypetwo').val();
            if (strmenu.length > 0) {
                where += " and distypecode ='" + strmenu + "'"
            } else {
                strmenu = $('#selDisheType').val();
                if (strmenu.length > 0) {
                    where += " and distypecode in (SELECT distypecode FROM dbo.DisheType WHERE pdistypecode='" + strmenu + "')"
                }
            }
            //财务类别搜索
            var strfincode = $('#selfincode').val();
            if (strfincode.length > 0) {
                where += " and fincode='" + strfincode + "'"
            }
            //状态
            var strstatus = $('#selStatus').val();
            if (strstatus.length > 0) {
                where += " and status='" + strstatus + "'"
            }
            //名称/速查码/自定义编号
            var strcname = $('#txt_disname').val();
            if (strcname.length > 0) {
                where += " and (disname like '%" + strcname + "%' or quickcode like '%" + strcname + "%' or customcode like '%" + strcname + "%')"
            }

            getListParameters.filter = where;
            //从第一页开始
            getExportlistCallback();
            break;
            //启用
        case "valid":
            updatestatus('1');
            break;
            //禁用
        case "invalid":
            updatestatus('0');
            break;
            //移除套餐
        case "delmeal":
            if (arrChoose.length == 0) {
                layer.msg(getCommonInfo('nochoose_button_tip'));
                return;
            }
            else if (arrChoose.length > 1) {
                layer.msg(getCommonInfo('singlechoose_button_tip'));
                return;
            }

            commonAjax(interfaceUrl, getpostParameters('deletepackage', { "GUID": getLoginUserInfo().GUID, "USER_ID": getLoginUserInfo().userid, "discode": getArrChooseValue(0, 0) }), true, CallBackPackage);

            break;
            //上传菜品
        case "uploadfood":
            break;
            //移除签送
        case "delrep":
            break;
            //下载模块
        case "downtemp":
            break;
            //上传模板
        case "uploadtemp":
            break;
    }
}

function CallBackPackage(data) {
    ShowResult(data);
}


//更新状态
function updatestatus(status) {
    var Flag = true;
    for (var i = 0; i < arrChoose.length; i++) {
        commonAjax(interfaceUrl, getpostParameters('updatestatus', { "GUID": "", "USER_ID": "", "discode": getArrChooseValue(i, 0), "status": status }), false, function (data) {
            if (data == undefined) {
                Flag = false;
            }
            else {
                if (data.status == 0) {
                    newdata = data;
                }
                else {
                    newdata = data;
                    Flag = false;
                }
            }
        });
        if (Flag == false) {
            break;
        }
    }
    ShowResult(newdata);
    getlistCallback(getListParameters.currentPage);
}

//分页方法
function getlistCallback(currentPage) {
    getListParameters.currentPage = currentPage;
    getListParameters.pageSize = 50;
    getListParameters.stocode = getLoginUserInfo().stocode;
    commonAjax(interfaceUrl, getpostParameters('getlist', getListParameters), true, bindlist);
}

//分页方法
function getExportlistCallback() {
    getListParameters.currentPage = 1;
    getListParameters.pageSize = 5000;
    getListParameters.order = " melcode,distypecode,ctime desc";
    getListParameters.stocode = getLoginUserInfo().stocode;
    getListParameters.type = "1";
    commonAjax(interfaceUrl, getpostParameters('dishesexport', getListParameters), true, bindExportlist);
}

function bindExportlist(data) {
    if (data != undefined) {
        if (data.status == 0) {
            closeloading();
            location.href = rooturl + data.data[0].filepath;
        } else {
            closeloading();
            layer.msg(data.mes);
        }
    }
}

function GetIsStatus(status) {
    var result = "否";
    if (status != "" && status != undefined) {
        if (status == "0") {
            result = "否";
        } else {
            result = "是";
        }
    }
    return result;
}

//绑定列表数据
function bindlist(data) {
    if (data != undefined) {
        if (data.status == 0) {
            // 拼接列表信息字符串
            var html = '';
            //表头
            var html = '<thead class="fixedThead"><tr>';
            html += '<th></th>';
            html += '<th>' + getCommonInfo('rowNumber') + '</th>';
            html += '<th>' + getNameByCode('Menu_list') + '</th>';
            html += '<th>' + getNameByCode('Departmen_list') + '</th>';
            html += '<th>' + getNameByCode('Kitchen_list') + '</th>';
            //html += '<th data-expression="discode">' + getNameByCode('discode_list') + '</th>';
            html += '<th data-expression="disname">' + getNameByCode('DisName_list') + '</th>';
            html += '<th>' + getNameByCode('quickcode_list') + '</th>';
            html += '<th>' + getNameByCode('customcode_list') + '</th>';
            html += '<th>' + getNameByCode('Classification_list') + '</th>';
            html += '<th>' + getNameByCode('Price_list') + '</th>';
            html += '<th>' + getNameByCode('CostPrice_list') + '</th>';
            html += '<th>' + getNameByCode('FinancialCategory_list') + '</th>';
            html += '<th>' + getNameByCode('unit_list') + '</th>';
            html += '<th>' + getNameByCode('memberprice_list') + '</th>';
            html += '<th>' + getNameByCode('kbj_list') + '</th>';//可变价
            html += '<th>' + getNameByCode('xcz_list') + '</th>';//需称重
            html += '<th>' + getNameByCode('zfbx_list') + '</th>';//做法必选
            html += '<th>' + getNameByCode('yjkrk_list') + '</th>';//烟酒(可入库)
            html += '<th>' + getNameByCode('kzdy_list') + '</th>';//可自定义
            html += '<th>' + getNameByCode('yxhyj_list') + '</th>';//允许会员价
            html += '<th>' + getNameByCode('cyfjf_list') + '</th>';//参与附加费计算
            html += '<th>' + getNameByCode('zcjyc_list') + '</th>';//支持使用消费券
            html += '<th>' + getNameByCode('kjc_list') + '</th>';//可寄存
            html += '<th>' + getNameByCode('yyw_list') + '</th>';//营业外收入
            html += '<th>' + getNameByCode('status_list') + '</th>';
            html += '<th data-expression="ctime">' + getNameByCode('ctime') + '</th>';
            html += '</tr></thead>';

            if (data.data != undefined) {
                //内容begin
                html += '<tbody>';
                for (var i = 0; i < data.data.length; i++) {
                    html += '<tr>';
                    html += '<td>' + data.data[i].discode + '</td>'; //
                    html += '<td>' + data.data[i].RowNumber + '</td>'; //序号
                    if (data.data[i].melname.length > 10) {
                        html += '<td title="' + data.data[i].melname + '">' + data.data[i].melname.substring(0, 10) + '...</td>'; // 菜谱
                    } else {
                        html += '<td>' + data.data[i].melname + '</td>'; // 菜谱
                    }
                    html += '<td>' + data.data[i].departname + '</td>'; // 部门
                    html += '<td>' + data.data[i].kitname + '</td>'; // 厨房
                    //html += '<td>' + data.data[i].discode + '</td>'; // 编号
                    html += '<td>' + data.data[i].disname + '</td>'; // 菜品名称
                    html += '<td>' + data.data[i].quickcode + '</td>'; //速查码
                    html += '<td>' + data.data[i].customcode + '</td>'; //自定义编号
                    html += '<td>' + data.data[i].distypename + '</td>'; // 分类
                    if (data.data[i].realprice.length > 10) {
                        html += '<td title="' + data.data[i].realprice + '">' + data.data[i].realprice.substring(0, 10) + '...</td>'; // 售价
                    } else {
                        html += '<td>' + data.data[i].realprice + '</td>'; // 售价
                    }
                    html += '<td>' + data.data[i].costprice + '</td>'; // 成本价
                    html += '<td>' + data.data[i].finname + '</td>'; // 财务类别
                    html += '<td>' + data.data[i].unit + '</td>'; //单位
                    if (data.data[i].realmemberprice.length > 10) {
                        html += '<td title="' + data.data[i].realmemberprice + '">' + data.data[i].realmemberprice.substring(0, 10) + '...</td>'; //会员价
                    } else {
                        html += '<td>' + data.data[i].realmemberprice + '</td>'; //会员价
                    }
                    html += "<td>" + GetIsStatus(data.data[i].iscanmodifyprice) + "</td>";
                    html += "<td>" + GetIsStatus(data.data[i].isneedweigh) + "</td>";
                    html += "<td>" + GetIsStatus(data.data[i].isneedmethod) + "</td>";
                    html += "<td>" + GetIsStatus(data.data[i].iscaninventory) + "</td>";
                    html += "<td>" + GetIsStatus(data.data[i].iscancustom) + "</td>";
                    html += "<td>" + GetIsStatus(data.data[i].isallowmemberprice) + "</td>";
                    html += "<td>" + GetIsStatus(data.data[i].isattachcalculate) + "</td>";
                    html += "<td>" + GetIsStatus(data.data[i].isclipcoupons) + "</td>";
                    html += "<td>" + GetIsStatus(data.data[i].iscandeposit) + "</td>";
                    html += "<td>" + GetIsStatus(data.data[i].isnonoperating) + "</td>";
                    html += '<td>' + data.data[i].statusname + '</td>'; // 状态
                    html += '<td>' + data.data[i].ctimestr + '</td>';
                    html += '</tr>';
                }
                html += '</tbody>';
                //内容end
            }

            // 填充列表信息
            $('#gridList').html(html);

            // 表格单击代理事件绑定
            addTableclick('gridList');

            //添加排序事件绑定
            addListOrder('gridList');

            // 重新渲染分页模块
            getpagelist(data.totalPage, getListParameters.currentPage, data.recordCount, 'getlistCallback');
        }
    }
}