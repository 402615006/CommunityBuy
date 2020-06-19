var htmlcount = 0;
function showContent(txt) {

    var e = arguments.callee.caller.arguments[0] || window.event;
    var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft;
    var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
    var x = e.pageX || e.clientX + scrollX;
    var y = e.pageY || e.clientY + scrollY;
    var htmlstr = '<div class="mouseshow" style="position:absolute;left:' + (x-150) + 'px;top:' + (y + 20) + 'px;z-index:1000;background-color:white;width:300px;padding:5px 10px;border:1px solid #ccc;word-break:break-all;">' + txt + '</div>';
    if (htmlcount == 0) {
        $("#form1").append(htmlstr);
        htmlcount = 1;
    }
}
function hideContent() {
    $(".mouseshow").remove();
    htmlcount = 0;
}