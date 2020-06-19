document.write("<script language=javascript src='/js/SpreadJS/gc.spread.sheets.all.11.0.0.min.js'></script>");
document.write("<script language=javascript src='/js/SpreadJS/FileSaver.min.js'></script>");
document.write("<script language=javascript src='/js/SpreadJS/gc.spread.excelio.11.0.0.min.js'></script>");
document.write("<script language=javascript src='/js/SpreadJS/gc.spread.sheets.resources.zh.11.0.0.min.js'></script>");


var spreadObj = undefined;
var obj = null;
var sheet = undefined;
var activeSheet = undefined;
var gcview = undefined;
var hyperLink;

function intjs() {
    $('html').css("overflow", "hidden");
    $('body').css("overflow", "hidden");
    var exportbutton = $("#ToolBar1_LinkExport");
    if (exportbutton != undefined) {
        exportbutton.removeAttr("onclick");
        exportbutton.removeAttr("href");
        exportbutton.attr("onclick", "ExporSheet();");
    }
    if (document.getElementById("ss") == undefined) {
        pcLayerMsg('没有表格对象，请检查！');
        return;
    }
    $('#ss').height(document.body.offsetHeight - 200);
    spreadObj = new GC.Spread.Sheets.Workbook(document.getElementById("ss"));
    hyperLink = new GC.Spread.Sheets.CellTypes.HyperLink();
    hyperLink.linkColor('red');
    hyperLink.visitedLinkColor('blue');

    spreadObj.options.tabStripVisible = false; //隐藏sheet

    //隐藏不需要的按钮
    var menuData = spreadObj.contextMenu.menuData;
    var newMenuData = [];
    menuData.forEach(function (item) {
        if (item) {
            if (item.name === "gc.spread.insertComment" || item.name === "gc.spread.insertSheet" || item.name === "gc.spread.insertRows" || item.name === "gc.spread.insertColumns" || item.name === "gc.spread.deleteRows" || item.name === "gc.spread.deleteColumns" || item.name === "gc.spread.deleteSheet") {
                return;
            }
            newMenuData.push(item);
        }
    });
}

//绑定数据
function BindData(json) {

    if (json.length > 0) {
        intjs();
        obj = JSON.parse(json);

        //循环所有的节点把为0的金额转换成-
        for (var p in obj.root) {
            for (var h in obj.root[p]) {
                var tempV = obj.root[p][h];
                if (tempV == 0 && tempV != "") {
                    obj.root[p][h] = "-";
                }
            }
        }

        activeSheet = spreadObj.getActiveSheet();
        sheet = spreadObj.getSheet(0);
        activeSheet.setDataSource(obj.root);
        gcview = GC.Spread.Sheets.SheetArea.viewport;
        initStyle();
    }
}

function initStyle() {

    if (sheet != undefined) {
        var rows = sheet.getRowCount();
        var cols = sheet.getColumnCount();
        var sw = 80;
        if (cols * 80 < $('#ss').width()) {
            var sw = ($('#ss').width() - 90) / cols;
        }

        var style = new GC.Spread.Sheets.Style();
        style.hAlign = GC.Spread.Sheets.HorizontalAlign.left;
        style.vAlign = GC.Spread.Sheets.VerticalAlign.center;

        var style1 = new GC.Spread.Sheets.Style();
        style1.hAlign = GC.Spread.Sheets.HorizontalAlign.right;
        style1.vAlign = GC.Spread.Sheets.VerticalAlign.center;

        var style2 = new GC.Spread.Sheets.Style();
        style2.backColor = "#CCFFCC";

        activeSheet.suspendPaint();

        activeSheet.defaults.colWidth = sw;  //默认列宽
        activeSheet.defaults.rowHeight = 27; //默认行高
        activeSheet.setStyle(-1, -1, style1, gcview);  //设置所有的数据垂直居中 水平居右
        activeSheet.setStyle(-1, 0, style, gcview);    //设置第一列的数据垂直居中 水平居左

        activeSheet.setStyle(rows - 1, -1, style2, GC.Spread.Sheets.SheetArea.viewport);  //设置最后一行高亮显示
        var headRows = sheet.As.rowCount; //头部行数
        var cHeight = 1 + ((headRows - 1) * 0.5);
        activeSheet.setRowHeight(0, cHeight * 38, GC.Spread.Sheets.SheetArea.colHeader);  //设置头部行高
        sheet.getCell(0, -1, GC.Spread.Sheets.SheetArea.colHeader).font('bold normal 16px 微软雅黑'); //设置头部样式  

        activeSheet.resumePaint();

        //隐藏页面空白部分
        var tableObj = GC.Spread.Sheets.findControl(document.getElementById('ss'));
        tableObj.options.scrollbarMaxAlign = true;
        tableObj.options.scrollbarShowMax = true;

    }
}


function SetBgColor() {//设置行背景色
    var spreadNS = GC.Spread.Sheets;
    for (var i = 0; i < sheet.getRowCount() ; i++) {
        var style = new spreadNS.Style();
        style.isVerticalText = 'true';
        if (sheet.getValue(i, 0) == "合计") {
            style.backColor = "#CCFFCC"
            if (obj.codestr != null) {
                obj.codestr.splice(i, 0, "none");
            }
            sheet.setStyle(i, -1, style, GC.Spread.Sheets.SheetArea.viewport);
        }
        if (sheet.getValue(i, 0) == "小计") {
            if (obj.codestr != null) {
                obj.codestr.splice(i, 0, "none");
            }
            style.backColor = "#FFCC99";
            sheet.setStyle(i, -1, style, GC.Spread.Sheets.SheetArea.viewport);
        }
        //sheet.getCell(i, 0).font('bold normal 15px 微软雅黑');
        //sheet.getCell(i, 1).font('bold normal 15px 微软雅黑');
    }
}

//导出
function ExporSheet() {
    var excelIo = new GC.Spread.Excel.IO();

    var excelfilename = "downfile";

    try {
        excelfilename = $("#PageTitle").children("span").eq(0).text();
    } catch (e) {

    }

    var fileName = excelfilename + ".xlsx";

    var serializationOption = {
        includeBindingSource: true,
        rowHeadersAsFrozenColumns: true,
        columnHeadersAsFrozenRows: true
    }
    var json = spreadObj.toJSON(serializationOption);
    // here is excel IO API
    excelIo.save(json, function (blob) {
        saveAs(blob, fileName);
    }, function (e) {
        // process error
        console.log(e);
    });
}

//设置超链接
function SetHypLink(row, col) {
    var style = new GC.Spread.Sheets.Style();
    style.foreColor = "blue";
    style.hAlign = GC.Spread.Sheets.HorizontalAlign.left;
    style.vAlign = GC.Spread.Sheets.VerticalAlign.center;

    activeSheet.setStyle(row, col, style, gcview);
    activeSheet.getCell(row, col).textDecoration(GC.Spread.Sheets.TextDecorationType.underline);
}


//初始化页面控件
function pageinit() {
    var url = "/ajax/SelectDown.ashx";

    if ($("#ddl_stocode").val().length > 0) {
        data = { code: "war", stocode: $("#ddl_stocode").val() };
        getSelect("ddl_warehouse", "Hidewarcode", "warname", "warcode", url, data);
    }

    $("#ddl_stocode").change(function () {

        $("#Hidewarcode").val("");
        var stocode = $(this).val();
        var data = { code: "war", stocode: stocode };
        $("#Hidestocode").val(stocode);
        getSelect("ddl_warehouse", "Hidewarcode", "warname", "warcode", url, data);

    })

    $("#ddl_warehouse").change(function () {
        $("#Hidewarcode").val($(this).val());
    })
}


//物料分类
function showfirst() {
    var Parameters = {
        "GUID": "",
        "USER_ID": "0",
        "buscode": "",
        "comcode": "",
        "filter": ""
    };

    //var cls = $("#Hidematclscode").val();
    //if (cls != "" && cls != "0") {
    //    Parameters.filter = " and pccode='" + cls + "'";
    //}

    if ($("#ddl_type").val() != "" && $("#ddl_type").val() != "0") {
        Parameters.filter = " and mcode='" + $("#ddl_type").val() + "' and (pccode='' or pccode=null)";
    }
    else {
        Parameters.filter = " and (pccode='' or pccode=null)";
    }

    var htmlstr = '<option value="">--全部--</option>';
    GpAjax('/ajax/stock/getmattype.ashx', getpostParameters('getlist', Parameters), false, function (data) {

        $(data.data).each(function (i) {
            if (data.data[i].ccode == $("#Hidematclscode").val()) {
                htmlstr += '<option value="' + data.data[i].ccode + '" selected="true">' + data.data[i].cname + '</option>';
            }
            else {
                htmlstr += '<option value="' + data.data[i].ccode + '">' + data.data[i].cname + '</option>';
            }
        });

    });
    $("#ddl_matclscode").html(htmlstr);
    showsecond();
}

function showsecond() {
    var Parameters = {
        "GUID": "",
        "USER_ID": "0",
        "buscode": "",
        "comcode": "",
        "filter": ""
    };

    if ($("#ddl_matclscode").val() != "" && $("#ddl_matclscode").val() != "0") {
        Parameters.filter = " and pccode='" + $("#ddl_matclscode").val() + "'";
    }
    else {
        if ($("#ddl_type").val() != "" && $("#ddl_type").val() != "0") {
            Parameters.filter = " and mcode='" + $("#ddl_type").val() + "' and (pccode<>'' or pccode<>null)";
        } else {
            Parameters.filter = " and (pccode<>'' or pccode<>null)";
        }
    }
    $("#Hidematclscode").val($("#ddl_matclscode").val());

    GpAjax('/ajax/stock/getmattype.ashx', getpostParameters('getlist', Parameters), true, function (data) {
        var htmlstr = '<option value="">--全部--</option>';
        $(data.data).each(function (i) {
            if (data.data[i].ccode == $("#Hidematclscode2").val()) {
                htmlstr += '<option value="' + data.data[i].ccode + '" selected="true">' + data.data[i].cname + '</option>';
            }
            else {
                htmlstr += '<option value="' + data.data[i].ccode + '">' + data.data[i].cname + '</option>';
            }
        });
        $("#ddl_matclscode2").html(htmlstr);
        $("#Hidematclscode2").val($("#ddl_matclscode2").val());
    });
}

function showthird() {
    $("#Hidematclscode2").val($("#ddl_matclscode2").val());
}