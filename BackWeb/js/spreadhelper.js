var spreadObj = undefined;
var obj = null;
var nolinklist = [];
//sheet对象
var spread_sheet = {
    "setRowCount": 2,
    "setColumnCount": 1,
    "freezerow": 1,
    "freezecol": 2,
    "freezecolor": "#F8F8FF",
    "cols": 4,
    "frozenlineColor": "black",
    "fileName ": ''
};

//单元格对象
var spread_cell = {
    "val": "",
    "row": 1,
    "col": 1,
    "rowspan": 1,
    "colspan": 1,
    "width": 100
};

//列头对象
var spread_head = {
    "cname": "",
    "row": 1,
    "col": 1,
    "rowspan": 1,
    "colspan": 1,
    "width": 150,
    "code": ''

};

function intjs() {
    $('html').css("overflow", "hidden");
    $('body').css("overflow", "hidden");
    var exportbutton = $("#ToolBar1_LinkExport");
    if (exportbutton != undefined) {
        exportbutton.removeAttr("onclick");
        exportbutton.attr("onclick", "ExporSheet();");
    }
    if (document.getElementById("datacomtent") == undefined) {
        pcLayerMsg('没有datacomtent对象，请检查！');
        return;
    }
    $('#datacomtent').height(document.body.offsetHeight - 200);
    spreadObj = new GC.Spread.Sheets.Workbook(document.getElementById("datacomtent"));

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

function BindData(json, heards, cells) {
    intjs();
    if (json.length > 0) {
        obj = JSON.parse(json);
        var activeSheet = spreadObj.getActiveSheet();
        spreadObj.suspendPaint();
        activeSheet.setDataSource(obj.root);
        SetHead(page_head);
        SetCells(cells);
        //SetBgColor(obj.root.length);
        //SetHyperLink(page_head, obj.root.length);
        //SetBind();
        if (typeof needAddFunction != "undefined") {
            needAddFunction(page_head, obj.root);
        }
        spreadObj.resumePaint();
        $("#datacomtent_tabStrip").hide();
    }
}

//设置报表表头
function SetHead(heards) {
    var sheet = spreadObj.getSheet(0);
    if (sheet != undefined) {

        var spreadNS = GC.Spread.Sheets;
        sheet.suspendPaint();
        sheet.setRowCount(spread_sheet.setRowCount, spreadNS.SheetArea.colHeader);
        sheet.setColumnCount(spread_sheet.setColumnCount, spreadNS.SheetArea.rowHeader);
        sheet.options.colHeaderAutoTextIndex = 1;
        sheet.options.colHeaderAutoText = spreadNS.HeaderAutoText.numbers;

        //设置列信息
        for (var i = 0; i < heards.length; i++) {
            var head = heards[i];
            var size = '';
            sheet.addSpan(head.row, head.col, head.rowspan, head.colspan, GC.Spread.Sheets.SheetArea.colHeader);
            sheet.setValue(head.row, head.col, head.cname, GC.Spread.Sheets.SheetArea.colHeader);
            size = (head.col == 0 && head.row == 0) ? "18px" : "15px";
            SetTitleBold(head.col, head.row, size);
        }
        var cols = sheet.getColumnCount();
        var sw = 80;
        if (cols * 80 < $('#datacomtent').width()) {
            var sw = ($('#datacomtent').width() - 90) / cols;
        }
        for (var i = 0; i < cols; i++) {
            sheet.setColumnWidth(i, sw);
        }
        //var style = new GC.Spread.Sheets.Style();
        //style.foreColor = 'green';
        //style.font = "bold 11pt arial";  
        //style.hAlign = GC.Spread.Sheets.HorizontalAlign.center;
        //style.vAlign = GC.Spread.Sheets.VerticalAlign.center;
        //sheet.setStyle(0, 0, style, GC.Spread.Sheets.SheetArea.viewport);
        sheet.resumePaint();

        //冻结表
        //sheet.frozenRowCount(spread_sheet.freezerow);   //冻结行
        //sheet.frozenColumnCount(spread_sheet.freezecol);//冻结列
        //sheet.options.frozenlineColor = '#F8F8FF';
        //sheet.options.gridline.showHorizontalGridline = true;
        //sheet.options.gridline.showVerticalGridline = true;
        //sheet.options.frozenlineColor = spread_sheet.frozenlineColor;

    }
}
function Freeze(type) {//冻结行或列(type==1?行:列)
    var sheet = spreadObj.getSheet(0);
    if (type == 1) { sheet.frozenRowCount(spread_sheet.freezerow) } else {
        sheet.frozenColumnCount(spread_sheet.freezecol);
    }
}
function FreezeColor(color) {//设置冻结行或列颜色
    var sheet = spreadObj.getSheet(0);
    sheet.options.frozenlineColor = color;
}
function SetHyperLink(xlen,ylen,startx) {//设置超链接(仅做参考，具体功能代码需按情况实现)
    var spreadNS = GC.Spread.Sheets;
    var sheet = spreadObj.getSheet(0);
    for (var j = startx; j < xlen; j++) {
        for (var i = 0; i < ylen; i++) {
            var h = new spreadNS.CellTypes.HyperLink();
            sheet.setCellType(i, j, h, spreadNS.SheetArea.viewport);
        }
    }
}
//function SetHyperLink(head, datalength) { //设置超链接(实现参考)
//    var spreadNS = GC.Spread.Sheets;
//    var sheet = spreadObj.getSheet(0);
//    var codetype = 1;
//    var showtime = 0;
//    for (var j = 3; j < head.length; j++) {
//        for (var i = 0; i < datalength; i++) {
//            if (head[j].colspan == 1 && sheet.getValue(i, head[j].col) != "-") {
//                var h = new spreadNS.CellTypes.HyperLink();
//                switch ($('#HidType').val()) {
//                    case "Y":
//                        $('#fontlen').text(sheet.getValue(2, head[j].col, spreadNS.SheetArea.colHeader))
//                        sheet.setColumnWidth(i, $('#fontlen').width());
//                        console.log(sheet.getValue(2, head[j].col, spreadNS.SheetArea.colHeader));
//                        var str = head[j].code + '|' + $('#txt_BYear').val().split('-')[0] + '|Y';
//                        sheet.setBindingPath(i, head[j].col, str);
//                        sheet.setCellType(i, head[j].col, h, spreadNS.SheetArea.viewport);
//                        break;
//                    case "M":
//                        var str = $('#hidstocodes').val() + '|' + head[j].code + '|M';
//                        sheet.setBindingPath(i, head[j].col, str);
//                        sheet.setCellType(i, head[j].col, h, spreadNS.SheetArea.viewport);
//                        break;
//                    case "D":
//                        if (obj.codestr[i] != "none") {
//                            if (showtime == 2) {
//                                if (codetype == 2) { codetype = 1 } else {
//                                    codetype = 2;
//                                }
//                            }
//                            showtime = 0;
//                            var str = $('#hidstocodes').val() + '|' + $('#txt_Year').val() + '|D|' + obj.codestr[i] + '|' + codetype;
//                            sheet.setBindingPath(i, head[j].col, str);
//                            sheet.setCellType(i, head[j].col, h, spreadNS.SheetArea.viewport);
//                        } else {
//                            showtime++;
//                        }
//                        break;
//                }
//            }
//        }
//    }
//}
function SetBind() {//绑定报表点击事件(仅做参考，具体功能代码需按情况实现)
    var spreadNS = GC.Spread.Sheets;
    var sheet = spreadObj.getSheet(0);
    sheet.bind(spreadNS.Events.SelectionChanged, function (e, info) {
        //需要实现的点击操作
    })
}
//function SetBind() { //绑定报表点击事件(实现参考)
//    var spreadNS = GC.Spread.Sheets;
//    var sheet = spreadObj.getSheet(0);
//    sheet.bind(spreadNS.Events.SelectionChanged, function (e, info) {
//        var str = sheet.getBindingPath(info.newSelections[0].row, info.newSelections[0].col);
//        var url = '';
//        //alert(str);
//        if (str != null && str != "") {
//            var strlist = str.split('|');
//            switch (strlist[2]) {
//                case "Y":
//                    url = '/report/finance/cateringincomeM.aspx?stocode=' + strlist[0] + '&year=' + strlist[1];
//                    window.open(url, '_blank');
//                    break;
//                case "M":
//                    url = '/report/finance/cateringincomeD.aspx?stocode=' + strlist[0] + '&year=' + strlist[1];
//                    window.open(url, '_blank');
//                    break;
//                case "D":
//                    url = '/report/finance/cateringincomeT.aspx?stocode=' + strlist[0] + '&year=' + strlist[1] + '&itemcode=' + strlist[3] + '&dtype=' + strlist[4];
//                    window.open(url, '_blank');
//                    break;
//            }
//        }
//    })//点击事件
//}
function SetBgColor(ylen, x, where, color) {//设置行背景色
    var spreadNS = GC.Spread.Sheets;
    var sheet = spreadObj.getSheet(0);
    for (var i = 0; i < ylen; i++) {
        var style = new spreadNS.Style();
        style.isVerticalText = 'true';
        style.backColor = color;
        if (sheet.getValue(i, x) == where) {
            if (obj.codestr != null) {
                nolinklist.push(i)
            }
            sheet.setStyle(i, -1, style, GC.Spread.Sheets.SheetArea.viewport);
        }
    }
}
function SetFontSizeBold(len, xy, fontsize, fontfamily, type) {//设置行或列字体加粗（type==1?行:列）
    var sheet = spreadObj.getSheet(0);
    for (var i = 0; i < len; i++) {
        if (type == 1) {
            sheet.getCell(xy, i).font('bold normal ' + fontsize + ' ' + fontfamily + '');
        }
        else {
            sheet.getCell(i, xy).font('bold normal ' + fontsize + ' ' + fontfamily + '');
        }
    }
}
function SetFontSizeCel(x, y, fontsize, fontfamily) {//设置某个单元格字体加粗
    sheet.getCell(y, x).font('bold normal ' + fontsize + ' ' + fontfamily + '');
}
//function SetBgColor(length) {//设置行背景色
//    var spreadNS = GC.Spread.Sheets;
//    var sheet = spreadObj.getSheet(0);
//    for (var i = 0; i < length; i++) {
//        var style = new spreadNS.Style();
//        style.isVerticalText = 'true';
//        if (sheet.getValue(i, 0) == "合计：") {
//            style.backColor = "#CCFFCC"
//            if (obj.codestr != null) {
//                obj.codestr.splice(i, 0, "none");
//            }
//            sheet.setStyle(i, -1, style, GC.Spread.Sheets.SheetArea.viewport);
//        }
//        if (sheet.getValue(i, 1) == "小计：") {
//            if (obj.codestr != null) {
//                obj.codestr.splice(i, 0, "none");
//            }
//            style.backColor = "#FFCC99";
//            sheet.setStyle(i, -1, style, GC.Spread.Sheets.SheetArea.viewport);
//        }
//        sheet.getCell(i, 0).font('bold normal 15px 微软雅黑');
//        sheet.getCell(i, 1).font('bold normal 15px 微软雅黑');
//    }
//}
function SetTitleHeight(titleheight, rowheight) { //设置表格头高度
    var spreadNS = GC.Spread.Sheets;
    var sheet = spreadObj.getSheet(0);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
    for (var i = 0; i < sheet.getRowCount() ; i++) {
        sheet.setRowHeight(i, rowheight);
    }
    for (var i = 1; i < sheet.getRowCount(spreadNS.SheetArea.colHeader) ; i++) {
        sheet.setRowHeight(i, rowheight, spreadNS.SheetArea.colHeader);
    }
    sheet.setRowHeight(0, titleheight, spreadNS.SheetArea.colHeader)
}
function SetTitleBold(x, y, size) {//设置表格头字体加粗
    var spreadNS = GC.Spread.Sheets;
    var sheet = spreadObj.getSheet(0);
    sheet.getCell(y, x, spreadNS.SheetArea.colHeader).font('bold normal ' + size + ' 微软雅黑');
}
//设置单元格
function SetCells(cells) {
    var sheet = spreadObj.getSheet(0);
    if (sheet != undefined) {
        //设置列信息
        for (var i = 0; i < cells.length; i++) {
            var cell = cells[i];
            sheet.addSpan(cell.row, cell.col, cell.rowspan, cell.colspan);
            sheet.setValue(cell.row, cell.col, cell.val);
        }
    }

    var style;
    var rows = sheet.getRowCount();
    var cols = sheet.getColumnCount();
    for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++) {
            if (j == 0) {
                style = new GC.Spread.Sheets.Style();
                style.hAlign = GC.Spread.Sheets.HorizontalAlign.center;
                style.vAlign = GC.Spread.Sheets.VerticalAlign.center;
                sheet.setStyle(i, j, style, GC.Spread.Sheets.SheetArea.viewport);
            }
            else if (j == 1) {
                style = new GC.Spread.Sheets.Style();
                style.hAlign = GC.Spread.Sheets.HorizontalAlign.left;
                style.vAlign = GC.Spread.Sheets.VerticalAlign.center;
                sheet.setStyle(i, j, style, GC.Spread.Sheets.SheetArea.viewport);
            }
            else {
                style = new GC.Spread.Sheets.Style();
                style.hAlign = GC.Spread.Sheets.HorizontalAlign.right;
                style.vAlign = GC.Spread.Sheets.VerticalAlign.center;
                sheet.setStyle(i, j, style, GC.Spread.Sheets.SheetArea.viewport);
            }
        }
    //var lineStyle = GC.Spread.Sheets.LineStyle.dotted;
    //var lineBorder = new GC.Spread.Sheets.LineBorder('black', lineStyle);
    //var sheetArea = GC.Spread.Sheets.SheetArea.viewport
    //sheet.getRange(1, 1, 1, 1).setBorder(lineBorder, { left: true, right: true }, sheetArea);
    //sheet.resumePaint();
}

//导出
function ExporSheet() {
    var excelIo = new GC.Spread.Excel.IO();
    var fileName = $('#hidfilename').val();

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