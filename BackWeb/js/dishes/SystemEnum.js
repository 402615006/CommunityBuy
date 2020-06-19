//字典接口实体定义
var SystemDict = {
    MemberCome: 'MemberCome',//会员来源
    IDType: 'IDType',//证件类型
    ZZStatus: 'ZZStatus',//在职状态 DutyStatus
    Unit: 'Unit',//单位
    TaxpayersType: 'TaxpayersType',//纳税人类型
    TemplateType: 'TemplateType',//模板类型
    LossType: 'LossType',//损耗类型
    EType: 'EType',//员工类型 EmployeeType
    StorageDivision: 'StorageDivision',//存储部门
    EduBackground: 'EduBackground',//学历
    FoodAttDepartment: 'FoodAttDepartment',//菜品归属部门
    RemarkType: 'RemarkType',//备注类型
    MoneyType: 'MoneyType',//货币类型
    ExchangeType: 'ExchangeType',//换菜类型
    OrderType: 'OrderType',//订单类型
    ConsumType: 'ConsumType',//消费类型
    POderType: 'POderType',//付款单据类型
    CouponSecType: 'CouponSecType',//优惠券二级类型
    DestineType: 'DestineType',//预定来源
    RepairCard: 'RepairCard',//补卡原因
    DepartmentType: 'DepartmentType',
    StaffJob: 'StaffJob',

};

//获取指定接口数据
//dictname:字典名称，bindid：绑定下拉框ID，index
function getDictInfo() {
    var diccode = arguments[0];
    var bindid = arguments[1];
    var index = arguments[2];
    var lng = arguments[3];
    if (diccode != undefined) {
        var interfaceUrl = '../../ajax/dishes/WSsto_ts_Dicts.ashx';
        commonAjax(interfaceUrl, getpostParameters('getlistbycode', { "GUID": "0", "USER_ID": "0", "buscode": "0", "stocode": $("#stocode").val() , "lng": lng, "diccode": diccode }), false, function (data) {
            if (bindid != undefined) {
                //绑定下拉框
                if (index == 1) {
                    $('#' + bindid).append('<option value="">--全部--</option>');
                }
                else if (index == 2) {
                    $('#' + bindid).append('<option value="">--无--</option>');
                }
                $(data.data).each(function (i, o) {
                    $('#' + bindid).append('<option value="' + o.diccode + '">' + o.dicname + '</option>');
                });
            }
            else {
                return data;
            }
        });
    }
}

//获取枚举项信息
function getDictOptionInfo() {
    var diccode = arguments[0];
    var index = arguments[1];
    var lng = arguments[2];
    var successFn = arguments[3];
    var str = '';
    if (diccode != undefined) {
        var interfaceUrl = '../../ajax/dishes/WSsto_ts_Dicts.ashx';
        commonAjax(interfaceUrl, getpostParameters('getlistbycode', { "GUID":  "0", "USER_ID":  "0", "buscode": "0", "stocode": $("#stocode").val(), "lng": lng, "diccode": diccode }), false, function (data) {
            if (data != undefined) {
                //绑定下拉框
                if (index == 1) {
                    str += '<option value="">--全部--</option>';
                }
                else if (index == 2) {
                    str += '<option value="">--无--</option>';
                }
                $(data.data).each(function (i, o) {
                    str += '<option value="' + o.diccode + '">' + o.dicname + '</option>';
                });
            }
            successFn(str);
        });
    }
}

//枚举接口实体定义
var SystemEnum = {
    LogOperateType: 'LogOperateType',//日志操作类型
    Status: 'Status',//状态
    MarriageTypes: 'MarriageTypes',//婚姻类型
    Sex: 'Sex',//性别
    Nation: 'Nation',//民族
    BackupCycle: 'BackupCycle',//备份周期
    SendType: 'SendType',//优惠券赠送方案 赠送类型
    CardPayMethod: 'CardPayMethod',//会员卡付款类型
    PayWay: 'PayWay',//会员卡充值途径
    MemCardStatus: 'MemCardStatus',//会员卡状态
    AffiliatedType: 'AffiliatedType',//附属卡类型
    DisposedStatus: 'DisposedStatus',//处理状态
    LostStatus: 'LostStatus',//招领状态
    Top: 'Top',//置顶状态
    LostType: 'LostType',//失物招领类型
    ActionType: 'ActionType',//活动类型
    InventoryCycle: 'InventoryCycle',//盘点周期
    AuditStatus: 'AuditStatus',//审核状态
    PriceWay: 'PriceWay',//加价方式
    CombiningScheme: 'CombiningScheme',//组合方案
    DiscountPackage: 'DiscountPackage',//折扣方案
    Week: 'Week',//星期
    FinancialSubject: 'FinancialSubject',//财务科目类别
    TablesDetailType: 'TablesDetailType',//桌台明细类型
    TableStatus: 'TableStatus',//桌台管理使用状态
    SurchargeCalculationMethod: 'SurchargeCalculationMethod',//附加费计算方法
    DepartmentType: 'DepartmentType',
    eOutofstype: 'eOutofstype',
    DefaultOrderMethod: 'DefaultOrderMethod', //点餐方式
    BookingSource: 'BookingSource',
    SysTerminalType: 'SysTerminalType', //终端类型
    RolesType: 'RolesType',//角色类型
    CarryOverType: 'CarryOverType'//抹零设置

};

//获取枚举数据
function getEnumInfo() {
    var enumcode = arguments[0];//枚举名称
    var bindid = arguments[1];//下拉框ID
    var index = arguments[2];//0不加仍和数
    var lng = arguments[3]//zh-cn
    if (enumcode != undefined) {
        var interfaceUrl = '../../ajax/dishes/WSSystemEnum.ashx';
        commonAjax(interfaceUrl, getpostParameters('getenumlist', { "GUID": "", "USER_ID": "", "enumcode": enumcode, "lng": lng }), false, function (data) {
            if (bindid != undefined) {
                //绑定下拉框
                if (index == 1) {
                    $('#' + bindid).append('<option value="">--全部--</option>');
                }
                else if (index == 2) {
                    $('#' + bindid).append('<option value="">--无--</option>');
                }
                $(data.data).each(function (i, o) {
                    $('#' + bindid).append('<option value="' + o.enumcode + '">' + o.enumname + '</option>');
                });
            }
            else {
                return data;
            }
        });
    }
}

//获取枚举项信息
function getEnumOptionInfo() {
    var enumcode = arguments[0];
    var index = arguments[1];
    var lng = arguments[2];
    var successFn = arguments[3];
    var str = '';
    if (enumcode != undefined) {
        var interfaceUrl = '../../ajax/dishes/WSSystemEnum.ashx';
        commonAjax(interfaceUrl, getpostParameters('getenumlist', { "GUID": "", "USER_ID": "", "enumcode": enumcode, "lng": lng }), false, function (data) {
            if (data != undefined) {
                //绑定下拉框
                if (index == 1) {
                    str += '<option value="">--全部--</option>';
                }
                else if (index == 2) {
                    str += '<option value="">--无--</option>';
                }
                $(data.data).each(function (i, o) {
                    str += '<option value="' + o.enumcode + '">' + o.enumname + '</option>';
                });
            }
            successFn(str);
        });
    }
}

//--李超 获取枚举的值和名称 动态生成button
function getEnumOptionValuesName_Button() {
    var enumcode = arguments[0];//枚举名称
    var bindid = arguments[1];//下拉框ID
    //var index = arguments[2];//0不加仍和数
    var lng = arguments[2]//zh-cn
    if (enumcode != undefined) {
        var interfaceUrl = '../../ajax/dishes/WSSystemEnum.ashx';
        commonAjax(interfaceUrl, getpostParameters('getenumlist', { "GUID": "", "USER_ID": "", "enumcode": enumcode, "lng": lng }), false, function (data) {
            $(data.data).each(function (i, o) {
                $('#' + bindid).append('<button id="' + o.enumfunctionname + '" data-code="span_' + o.enumfunctionname + '" class="other-choose" value="' + o.enumcode + '">' + o.enumname + '</button>');
            });
        });
    }
}

//--李超 获取表数据进行下拉框绑定
function getTableDataValueByID() {
    var bindid = arguments[0];//下拉框ID
    var interfacename = arguments[1];//接口名称
    var filter = arguments[2];//条件
    var keyname = arguments[3];//表主键名称
    var fieldvalue = arguments[4]//需要的列
    var interfaceUrl = '../../ajax/dishes/' + interfacename + '.ashx';
    getListParameters = { "GUID": "" +  "0" + "", "USER_ID": "" +  "0" + "", "pageSize": 0, "currentPage": 1, "filter": "" + filter + "", "order": "" };
    commonAjax(interfaceUrl, getpostParameters('getlist', getListParameters), false, function (data) {
        if (data != undefined) {
            $("#" + bindid + "").append("<option value=''>--全部--</option>");
            if (data.data != undefined) {
                $.each(data.data, function (index, value) {
                    $("#" + bindid + "").append("<option value='" + eval('value.' + keyname) + "'>" + eval('value.' + fieldvalue) + "</option>");
                });
            }
        }
    });
}