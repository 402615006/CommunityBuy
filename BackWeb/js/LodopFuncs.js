var CreatedOKLodop7766 = null;

function getLodop(oOBJECT, oEMBED) {
    /**************************
      本函数根据浏览器类型决定采用哪个页面元素作为Lodop对象：
      IE系列、IE内核系列的浏览器采用oOBJECT，
      其它浏览器(Firefox系列、Chrome系列、Opera系列、Safari系列等)采用oEMBED,
      如果页面没有相关对象元素，则新建一个或使用上次那个,避免重复生成。
      64位浏览器指向64位的安装程序install_lodop64.exe。
    **************************/
    var strHtmInstall = "<br><font color='#FF00FF'>打印控件未安装!点击这里<a href='/install_lodop32.exe' target='_self'>执行安装</a>,安装后请刷新页面或重新进入。</font>";
    var strHtmUpdate = "<br><font color='#FF00FF'>打印控件需要升级!点击这里<a href='/install_lodop32.exe' target='_self'>执行升级</a>,升级后请重新进入。</font>";
    var strHtm64_Install = "<br><font color='#FF00FF'>打印控件未安装!点击这里<a href='/install_lodop64.exe' target='_self'>执行安装</a>,安装后请刷新页面或重新进入。</font>";
    var strHtm64_Update = "<br><font color='#FF00FF'>打印控件需要升级!点击这里<a href='/install_lodop64.exe' target='_self'>执行升级</a>,升级后请重新进入。</font>";
    var strHtmFireFox = "<br><br><font color='#FF00FF'>（注意：如曾安装过Lodop旧版附件npActiveXPLugin,请在【工具】->【附加组件】->【扩展】中先卸它）</font>";
    var strHtmChrome = "<br><br><font color='#FF00FF'>(如果此前正常，仅因浏览器升级或重安装而出问题，需重新执行以上安装）</font>";
    var LODOP;
    try {
        //=====判断浏览器类型:===============
        var isIE = (navigator.userAgent.indexOf('MSIE') >= 0) || (navigator.userAgent.indexOf('Trident') >= 0);
        var is64IE = isIE && (navigator.userAgent.indexOf('x64') >= 0);
        //=====如果页面有Lodop就直接使用，没有则新建:==========
        if (oOBJECT != undefined || oEMBED != undefined) {
            if (isIE)
                LODOP = oOBJECT;
            else
                LODOP = oEMBED;
        } else {
            if (CreatedOKLodop7766 == null) {
                LODOP = document.createElement("object");
                LODOP.setAttribute("width", 0);
                LODOP.setAttribute("height", 0);
                LODOP.setAttribute("style", "position:absolute;left:0px;top:-100px;width:0px;height:0px;");
                if (isIE) LODOP.setAttribute("classid", "clsid:2105C259-1E0C-4534-8141-A753534CB4CA");
                else LODOP.setAttribute("type", "application/x-print-lodop");
                document.documentElement.appendChild(LODOP);
                CreatedOKLodop7766 = LODOP;
            } else
                LODOP = CreatedOKLodop7766;
        };
        //=====判断Lodop插件是否安装过，没有安装或版本过低就提示下载安装:==========
        if ((LODOP == null) || (typeof (LODOP.VERSION) == "undefined")) {
            if (navigator.userAgent.indexOf('Chrome') >= 0)
                document.documentElement.innerHTML = strHtmChrome + document.documentElement.innerHTML;
            if (navigator.userAgent.indexOf('Firefox') >= 0)
                document.documentElement.innerHTML = strHtmFireFox + document.documentElement.innerHTML;
            if (is64IE) document.write(strHtm64_Install); else
                if (isIE) document.write(strHtmInstall); else
                    document.documentElement.innerHTML = strHtmInstall + document.documentElement.innerHTML;
            return LODOP;
        } else
            if (LODOP.VERSION < "6.1.9.8") {
                if (is64IE) document.write(strHtm64_Update); else
                    if (isIE) document.write(strHtmUpdate); else
                        document.documentElement.innerHTML = strHtmUpdate + document.documentElement.innerHTML;
                return LODOP;
            };
        //=====如下空白位置适合调用统一功能(如注册码、语言选择等):====	     

        LODOP.SET_LICENSES("", "864607380837275858895969799998", "", "");
        //============================================================	     
        return LODOP;
    } catch (err) {
        if (is64IE)
            document.documentElement.innerHTML = "Error:" + strHtm64_Install + document.documentElement.innerHTML; else
            document.documentElement.innerHTML = "Error:" + strHtmInstall + document.documentElement.innerHTML;
        return LODOP;
    };
}

var LODOP; //声明为全局变量
function printinfo(title, id) {
    var prnhtml = $('#' + id).html();
    $('#' + id + " table tr").slice(0, 4).wrapAll("<thead></thead>");
    LODOP = getLodop();
    LODOP.PRINT_INIT(title);
    LODOP.SET_PRINT_PAGESIZE(1, "210mm", "140mm", '');
    LODOP.ADD_PRINT_TEXT(0, 0, 100, 39, "");
    LODOP.ADD_PRINT_TEXT(10, 700, 160, 20, "第#页/共&页");
    LODOP.SET_PRINT_STYLEA(2, "ItemType", 2);
    LODOP.SET_PRINT_STYLEA(2, "HOrient", 1);
    var prtstring = $('#' + id).html();
    prtstring = prtstring.replace('<!--[if supportMisalignedColumns]-->', '')
    prtstring = prtstring.replace('<!--[endif]-->', '')
    LODOP.ADD_PRINT_TABLE("3mm", "0mm", '100%', '100%', prtstring);
    //LODOP.ADD_PRINT_HTM('0%', '0%', '100%', '100%', $('#' + id).html());
    //LODOP.SET_PRINTER_INDEX(index); //设置打印机
    var flag = LODOP.PREVIEW();
    if (flag == '1') {
        //成功
    }
    else {
        //失败
    }
    //LODOP.PRINT();
}

function gotoprint() {
    var obj1 = arguments[0] == undefined ? '' : arguments[0];//类型
    var obj2 = arguments[1] == undefined ? '' : arguments[1];//申请单号
    var obj3 = arguments[2] == undefined ? '' : arguments[2];//类型（1：半成品原料出库 2：销售出库 3：回存酒出库 4：卡券销售出库单)
    var obj4 = arguments[3] == undefined ? '' : arguments[3];//验收单号
    $.ajax({
        url: "/ajax/stockprint.ashx",
        type: "post",
        data: { "ctype": obj1, "ordercode": obj2, "dtype": obj3, "ycode": obj4 },
        dataType: "text",
        error: function (val) {
            console.log('失败');
        },
        success: function (val) {
            $('#printDiv').html(val);
            printinfo('', 'printDiv');
        }
    });
}

function gotoprintmini() {
    var obj1 = arguments[0] == undefined ? '' : arguments[0];//类型
    var obj2 = arguments[1] == undefined ? '' : arguments[1];//申请单号
    var obj3 = arguments[2] == undefined ? '' : arguments[2];//类型（1：半成品原料出库 2：销售出库 3：回存酒出库 4：卡券销售出库单)
    var obj4 = arguments[3] == undefined ? '' : arguments[3];//验收单号
    $.ajax({
        url: "/ajax/stockprint.ashx",
        type: "post",
        data: { "ctype": obj1, "ordercode": obj2, "dtype": obj3, "ycode": obj4 },
        dataType: "text",
        error: function (val) {
            console.log('失败');
        },
        success: function (val) {
            $('#printDiv').html(val);
            printinfomini('', 'printDiv');
        }
    });
}

function printinfomini(title, id) {
    var prnhtml = $('#' + id).html();
    $('#' + id + " table tr").slice(0, 4).wrapAll("<thead></thead>");
    LODOP = getLodop();
    LODOP.PRINT_INIT(title);
    LODOP.SET_PRINT_PAGESIZE(1, "70mm", "80mm", '');
    LODOP.ADD_PRINT_TEXT(0, 0, 100, 39, "");
    LODOP.SET_PRINT_STYLEA(2, "ItemType", 2);
    LODOP.SET_PRINT_STYLEA(2, "HOrient", 1);
    var prtstring = $('#' + id).html();
    prtstring = prtstring.replace('<!--[if supportMisalignedColumns]-->', '')
    prtstring = prtstring.replace('<!--[endif]-->', '')
    LODOP.ADD_PRINT_TABLE("10mm", "0mm", '100%', '100%', prtstring);
    //LODOP.ADD_PRINT_HTM('0%', '0%', '100%', '100%', $('#' + id).html());
    //LODOP.SET_PRINTER_INDEX(index); //设置打印机
    var flag = LODOP.PREVIEW();
    if (flag == '1') {
        //成功
    }
    else {
        //失败
    }
    //LODOP.PRINT();
}