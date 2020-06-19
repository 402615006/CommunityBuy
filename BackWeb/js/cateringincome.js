function SetHyperLinks(head, datalength) { //设置超链接
    var spreadNS = GC.Spread.Sheets;
    var sheet = spreadObj.getSheet(0);
    var codetype = 1;
    var showtime = 0;
    for (var j = 3; j < head.length; j++) {
        for (var i = 0; i < datalength; i++) {
            if (head[j].colspan == 1 && sheet.getValue(i, head[j].col) != "-") {
                var h = new spreadNS.CellTypes.HyperLink();
                switch ($('#HidType').val()) {
                    case "Y":
                        $('#fontlen').text(sheet.getValue(2, head[j].col, spreadNS.SheetArea.colHeader))
                        sheet.setColumnWidth(i, $('#fontlen').width());
                        console.log(sheet.getValue(2, head[j].col, spreadNS.SheetArea.colHeader));
                        var str = head[j].code + '|' + $('#txt_BYear').val().split('-')[0] + '|Y';
                        sheet.setBindingPath(i, head[j].col, str);
                        sheet.setCellType(i, head[j].col, h, spreadNS.SheetArea.viewport);
                        break;
                    case "M":
                        var str = $('#hidstocodes').val() + '|' + head[j].code + '|M';
                        sheet.setBindingPath(i, head[j].col, str);
                        sheet.setCellType(i, head[j].col, h, spreadNS.SheetArea.viewport);
                        break;
                    case "D":
                        if (obj.codestr[i] != "none") {
                            if (showtime == 2) {
                                if (codetype == 2) { codetype = 1 } else {
                                    codetype = 2;
                                }
                            }
                            showtime = 0;
                            var str = $('#hidstocodes').val() + '|' + $('#txt_Year').val() + '|D|' + obj.codestr[i] + '|' + codetype;
                            sheet.setBindingPath(i, head[j].col, str);
                            sheet.setCellType(i, head[j].col, h, spreadNS.SheetArea.viewport);
                        } else {
                            showtime++;
                        }
                        break;
                }
            }
        }
    }
}
function SetBinds() { //绑定报表点击事件
    var spreadNS = GC.Spread.Sheets;
    var sheet = spreadObj.getSheet(0);
    sheet.bind(spreadNS.Events.SelectionChanged, function (e, info) {
		event.stopPropagation();
        var str = sheet.getBindingPath(info.newSelections[0].row, info.newSelections[0].col);
        var url = '';
        //alert(str);
        if (str != null && str != "") {
            var strlist = str.split('|');
            switch (strlist[2]) {
                case "Y":
                    parent.opentab('/report/finance/cateringincomeM.aspx?stocode=' + strlist[0] + '&year=' + strlist[1], 'stocode' + strlist[0] + 'year' + strlist[1], '营业收入汇总(月)');
                    //ShowOpenpage('营业收入汇总(月)','/report/finance/cateringincomeM.aspx?stocode=' + strlist[0] + '&year=' + strlist[1]);
                    break;
                case "M":
				parent.opentab('/report/finance/cateringincomeD.aspx?stocode=' + strlist[0] + '&year=' + strlist[1], 'stocode' + strlist[0] + 'year' + strlist[1], '营业收入汇总(日)');
                    //ShowOpenpage('营业收入汇总(日)','/report/finance/cateringincomeD.aspx?stocode=' + strlist[0] + '&year=' + strlist[1]);
                    break;
                case "D":
                    parent.opentab('/report/finance/cateringincomeT.aspx?stocode=' + strlist[0] + '&year=' + strlist[1] + '&itemcode=' + strlist[3] + '&dtype=' + strlist[4], 'stocode' + strlist[0] + 'year' + strlist[1], '营业收入汇总(时)');
                    //ShowOpenpage('营业收入汇总(时)','/report/finance/cateringincomeT.aspx?stocode=' + strlist[0] + '&year=' + strlist[1] + '&itemcode=' + strlist[3] + '&dtype=' + strlist[4]);
                    break;
            }
        }
		return;
    })//点击事件
}
function needAddFunction(page_head, data) {
    SetTitleHeight(40, 25);
    SetBgColor(data.length, 1, "小计：", "#FFCC99")
    SetBgColor(data.length, 0, "合计：", "#CCFFCC")//设置行背景色
    var newlinklist = nolinklist.sort(function (a, b) {
        return a - b;
    })
    for (var i = 0; i < newlinklist.length; i++) {
        obj.codestr.splice(newlinklist[i], 0, "none")
    }
    SetFontSizeBold(data.length, 0, "15px", "微软雅黑", 2);//设置字体加粗
    SetFontSizeBold(data.length, 1, "15px", "微软雅黑", 2);
    spread_sheet.freezecol = 3;
    Freeze(2);
    FreezeColor("#F8F8FF")
    SetHyperLinks(page_head, data.length);
    SetBinds();
}