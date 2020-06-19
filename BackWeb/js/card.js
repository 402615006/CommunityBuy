//卡等级 卡类型

//卡类型
function getcardtype() {
    $.ajax({
        url: "/ajax/card/CardReq.ashx",
        type: "post",
        data: { "way": "ctype" },
        dataType: "json",
        success: function (data) {
            var htmlstr = '<option value="">--全部--</option>';
            $(data.ctypelist).each(function (i) {
                if (data.ctypelist[i].mctcode == $("#hidctype").val()) {
                    htmlstr += '<option value="' + data.ctypelist[i].mctcode + '" selected="true">' + data.ctypelist[i].cname + '</option>';
                }
                else {
                    htmlstr += '<option value="' + data.ctypelist[i].mctcode + '">' + data.ctypelist[i].cname + '</option>';
                }

            });
            $("#sel_ctype").html(htmlstr);

            getcardlevel();
        }
    });
}

// 卡等级
function getcardlevel() {
    var pid = $("#sel_ctype").val();
    $.ajax({
        url: "/ajax/card/CardReq.ashx",
        type: "post",
        data: { "way": "level", "ids": pid },
        dataType: "json",
        success: function (data) {
            var htmlstr = '<option value="">--全部--</option>';
            $(data.ctypelist).each(function (i) {
                if (data.ctypelist[i].levelcode == $("#hidcracode").val()) {
                    htmlstr += '<option value="' + data.ctypelist[i].levelcode + '" selected="true">' + data.ctypelist[i].levelname + '</option>';
                }
                else {
                    htmlstr += '<option value="' + data.ctypelist[i].levelcode + '">' + data.ctypelist[i].levelname + '</option>';
                }
            });
            $("#sel_cracode").html(htmlstr);
            $("#hidctype").val($("#sel_ctype").val());
            $("#hidcracode").val("");
        }
    });
}

function setlevel() {
    $("#hidcracode").val($("#sel_cracode").val());
}

//可用余额
function showmemcardconsumptionList(cardcode) {
    var cardcode = cardcode;
    var linkstr = 'memcardconsumptionList.aspx?id=' + cardcode;;
    var title = '可用余额';
    showpage(title, linkstr);
}

//可用积分
function showmemintegralList(cardcode) {

    var cardcode = cardcode;
    var linkstr = 'memintegralList.aspx?id=' + cardcode;;
    var title = '可用积分';
    showpage(title, linkstr);
}

//可开发票
function showmemcardinvoiceList(cardcode) {
    var cardcode = cardcode;
    var linkstr = 'memcardinvoiceList.aspx?id=' + cardcode;;
    var title = '可开发票';
    showpage(title, linkstr);
}

//可用额度
function showmemcardicreditList(cardcode, ctype, isyes) {
    if (isyes == '1') {
        var cardcode = cardcode;
        var linkstr = 'memcardicreditList.aspx?ctype=' + ctype + '&id=' + cardcode;
        var title = getNameByCode('available_credit');
        if (ctype == '1') {
            title = getNameByCode('available_sign');
        }
        showpage(title, linkstr);
    }
}

function showpage(title, linkstr) {
    var index = layer.open({
        title: title,
        type: 2,
        area: ['100%', '100%'],
        fix: true, //不固定
        maxmin: false,
        content: linkstr
    });
    layer.full(index);
}