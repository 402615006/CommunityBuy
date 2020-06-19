/*******************************************
 * 
 * 创建人：Quber（qubernet@163.com）
 * 创建时间：2014年6月10日
 * 创建说明：Base=>页面加载（loading）效果
 * 
 * 修改人：
 * 修改时间：
 * 修改说明：
 * 
*********************************************/
//function dataloading() {
//    //获取浏览器页面可见高度和宽度
//    var _PageHeight = document.documentElement.clientHeight,
//        _PageWidth = document.documentElement.clientWidth;
//    //计算loading框距离顶部和左部的距离（loading框的宽度为225px，高度为61px）
//    var _LoadingTop = _PageHeight > 61 ? (_PageHeight - 61) / 2 : 0,
//        _LoadingLeft = _PageWidth > 225 ? (_PageWidth - 225) / 2 : 0;
//    //在页面未加载完毕之前显示的loading Html自定义内容
//    var _LoadingHtml = '<div id="loadingDiv" style="position:absolute;left:0;width:100%;height:' + _PageHeight + 'px;top:0;background:#efefef;opacity:0.8;filter:alpha(opacity=80);z-index:10000;"><div style="position: absolute; cursor1: wait; left: ' + _LoadingLeft + 'px; top:' + _LoadingTop + 'px; width: auto; height: 57px; line-height: 57px; padding-left: 20px; padding-right: 20px; border: 2px solid #95B8E7; color: #696969; font-family:\'Microsoft YaHei\';">页面加载中，请等待...</div></div>';
//    //呈现loading效果
//    $("body").append(_LoadingHtml);
//    return true;
//}

function dataloading() {
    //获取浏览器页面可见高度和宽度
    var _PageHeight = document.documentElement.clientHeight,
        _PageWidth = document.documentElement.clientWidth;
    //计算loading框距离顶部和左部的距离（loading框的宽度为225px，高度为160px）
    var _LoadingTop = _PageHeight > 60 ? (_PageHeight - 80) / 2 : 0,
        _LoadingLeft = _PageWidth > 225 ? (_PageWidth - 225) / 2 : 0;
    //var _LoadingTop = _PageHeight > 160 ? (_PageHeight - 160) / 2 : 0,
    //在页面未加载完毕之前显示的loading Html自定义内容
    //    var arr = ["从前有个人钓鱼，钓到了只鱿鱼。<br/>鱿鱼求他：你放了我吧，别把我烤来吃啊。<br/>那个人说：好的，那么我来考问你几个问题吧。<br/>鱿鱼很开心说：你考吧你考吧！<br/>然后这人就把鱿鱼给烤了..<br/>", "有一个人，<br/>走着走着觉得脚很酸，<br/>低头一看，<br/>他踩到了一个柠檬！<br/>", "不要小看自己<br/>因为人有无限的可能<br/>",
    //"口说好话<br/>心想好意<br/>身行好事<br/>", "原谅别人就是善待自己<br/>", "有一天,<br/>有只公鹿越跑越快，<br/>跑到最后 ，<br/>它就变成高速公鹿了。<br/>", "有个人长的像洋葱,<br/>走著走著就哭了..<br/>", "一个糖,在北极走著走著,觉得他好冷<br/>——于是就变成了冰糖。", "蝙蝠侠和超人为什么要穿紧身衣?<br/>因为救人要紧<br/>"];
    var arr = ["", "", ""]
    var index = Math.floor((Math.random() * arr.length));
    //var _LoadingHtml = '<div id="loadingDiv" style="position:absolute;left:0;width:100%;height:' + _PageHeight + 'px;top:0;background:#efefef;opacity:0.93;filter:alpha(opacity=93);z-index:10000;"><div style="background:white;position: absolute; cursor1: wait; left: ' + _LoadingLeft + 'px; top:' + _LoadingTop + 'px; width: auto;min-width:160px; height: 160px; line-height: 20px; padding-top:20px;padding-left: 20px; padding-right: 20px; border: 2px solid #95B8E7; color: #696969; font-family:\'Microsoft YaHei\';"><b>数据准备中..</b><br/><br/><span style="font-size:12px;">' + arr[index] + '</span></div></div>';
    var _LoadingHtml = '<div id="loadingDiv" style="position:absolute;left:0;width:100%;height:' + _PageHeight + 'px;top:0;background:#efefef;opacity:0.93;filter:alpha(opacity=93);z-index:10000;"><div style="background:white;position: absolute; cursor1: wait; left: ' + _LoadingLeft + 'px; top:' + _LoadingTop + 'px; width: auto;min-width:160px; height: 60px; line-height: 20px; padding-top:20px;padding-left: 20px; padding-right: 20px; border: 2px solid #95B8E7; color: #696969; font-family:\'Microsoft YaHei\';"><b>数据准备中..</b><br/><br/><span style="font-size:12px;">' + arr[index] + '</span></div></div>';
    //呈现loading效果
    $("body").append(_LoadingHtml);
    return true;
}

function dataloading1(str) {
    //获取浏览器页面可见高度和宽度
    var _PageHeight = document.documentElement.clientHeight,
        _PageWidth = document.documentElement.clientWidth;
    //计算loading框距离顶部和左部的距离（loading框的宽度为225px，高度为61px）
    var _LoadingTop = _PageHeight > 61 ? (_PageHeight - 61) / 2 : 0,
        _LoadingLeft = _PageWidth > 225 ? (_PageWidth - 225) / 2 : 0;
    //在页面未加载完毕之前显示的loading Html自定义内容
    var _LoadingHtml = '<div id="loadingDiv" style="position:absolute;left:0;width:100%;height:' + _PageHeight + 'px;top:0;background:#efefef;opacity:0.8;filter:alpha(opacity=80);z-index:10000;"><div style="position: absolute; cursor1: wait; left: ' + _LoadingLeft + 'px; top:' + _LoadingTop + 'px; width: auto; height: 57px; line-height: 57px; padding-left: 20px; padding-right: 20px; border: 2px solid #95B8E7; color: #696969; font-family:\'Microsoft YaHei\';">' + str + '</div></div>';
    //呈现loading效果
    $("body").append(_LoadingHtml);
    return true;
}

function closeloading() {
    if ($("#loadingDiv") != undefined) {
        $("#loadingDiv").remove();
    }
}

//window.onload = function () {
//    var loadingMask = document.getElementById('loadingDiv');
//    loadingMask.parentNode.removeChild(loadingMask);
//};

//监听加载状态改变
//document.onreadystatechange = completeLoading;

////加载状态为complete时移除loading效果
//function completeLoading() {
//    if (document.readyState == "complete") {
//        var loadingMask = document.getElementById('loadingDiv');
//        loadingMask.parentNode.removeChild(loadingMask);
//    }
//}
