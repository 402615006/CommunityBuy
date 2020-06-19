//My97DatePicker
/*
%y	当前年
%M	当前月
%d	当前日
%ld	本月最后一天
%H	当前时
%m	当前分
%s	当前秒
{}	运算表达式,如:{%d+1}:表示明天
#F{}	{}之间是函数可写自定义JS代码
minDate:'%y-%M-01'

disabledDays:[0,6]
disabledDates:['0[4-7]$','1[1-5]$','2[58]$']
*/
//显示短日期
function ShowShortDate() {
    var fun = "WdatePicker({dateFmt: 'yyyy-MM-dd'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}

function ShowShortMaxDate() {
    var fun = "WdatePicker({dateFmt: 'yyyy-MM-dd',maxDate:'%y-%M-%d'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}

//显示日期+时间
function ShowLongDate() {
    var fun = "WdatePicker({dateFmt: 'yyyy-MM-dd HH:mm:ss'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}

function ShowLongMaxDate() {
    var fun = "WdatePicker({dateFmt: 'yyyy-MM-dd HH:mm:ss',maxDate:'%y-%M-%d HH:mm:ss'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}

function ShowDate() {
    var fun = "WdatePicker({dateFmt: 'yyyy-MM-dd HH:mm:ss'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}

//显示年月
function ShowYM() {
    var fun = "WdatePicker({dateFmt: 'yyyy-MM',maxDate:'%y-%M'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}
//显示月日
function ShowMD() {
    var fun = "WdatePicker({dateFmt: 'MM-dd'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}
//显示年
function ShowYear() {
    var fun = "WdatePicker({dateFmt: 'yyyy'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}

//时间
function ShowTime() {
    var fun = "WdatePicker({dateFmt: 'HH:mm:ss'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
    $(this).live('click', function () { eval(fun); });
}

function ShowHM() {
    var fun = "WdatePicker({dateFmt: 'HH:mm'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}

function ShowHour() {
    var fun = "WdatePicker({dateFmt: 'HH'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}

function ShowHourXX() {
    var fun = "WdatePicker({dateFmt: 'HH:00:00'";
    fun += GetParameterString(arguments);
    fun += "});";
    eval(fun);
}

function GetParameterString(obj) {
    var Parameter = obj[0];

    //var fun = ",readOnly:true";
    var fun = "";
    if (Parameter != undefined && Parameter.length > 0) {
        fun += "," + Parameter;
    }
    return fun;
}

function GetParameterString2(obj) {
    var minDate = obj[0];//最小日期
    var maxDate = obj[1];//最大日期
    var disabledDays = obj[2];//无效天限制,0至6 分别代表周日至周六,
    var disabledDates = obj[3];//无效日期限制

    var fun = ",readOnly:true";
    if (minDate != undefined && minDate.length > 0) {
        fun += ", minDate: '" + minDate + "'";
    }
    if (maxDate != undefined && maxDate.length > 0) {
        fun += ", maxDate: '" + maxDate + "'";
    }
    if (disabledDays != undefined && disabledDays.length > 0) {
        fun += ", disabledDays: [" + disabledDays + "]";
    }
    if (disabledDates != undefined && disabledDates.length > 0) {
        fun += ", disabledDates: ['" + disabledDates + "']";
    }
    return fun;
}

//$(document).ready(function () {
//    //累计充值金额、累计消费金额、累计消费次数开关
//    $('.datetxt').keyup(function (e) {
//        $(e.currentTarget).val('') ;
//    });
//});