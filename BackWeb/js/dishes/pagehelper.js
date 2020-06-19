/*
  全局分页
  totalPage：//总页数
  currentPage：//当前页
  funname：//回调页面方法名称
*/

function getpagelist() {
    var totalPage = arguments[0];//总页数
    var currentPage = arguments[1];//当前页
    if (totalPage == 0) { reccount = 0; }
    var reccount = arguments[2];
    funname = arguments.length > 3 ? arguments[3] : "getpagelistCallback";//回调页面方法名称
    pageCalc(totalPage, currentPage, function (s, e, c) {
        var firstPage = c !== 1 ? 'onclick="'+ funname + '(' + 1 +');"' : '';
        var lastPage = c === totalPage ? '' : 'onclick="'+ funname + '(' + totalPage +');"';
        var str ='';

        str += '<div class="page-number">';
        str += '<span class="current-page">'+ currentPage +'</span>';
        str += '/';
        str += '<span class="total">' + totalPage + '</span>';
        str += '</div>';
        str += '<span class="number">' + getCommonInfo('ShowTotal') + reccount + getCommonInfo('ShowRec') + '</span>';
        str += '<nav class="page">';
        str += '<ul class="pagination">';

        var firstPageHtml = getCommonInfo('FirstPage');
        var lastPageHtml = getCommonInfo('LastPage');
        str += '<li><a class="first-page" ' + firstPage + ' data-code="FirstPage">' + firstPageHtml + '</a></li>';

        for (var i =s; i <=e; i++) {
          if (i == c) {
            str += '<li><a class="current">' + i + '</a></li>';
          } else {
            str += '<li><a onclick="' + funname + '(' + i + ')' + '">' + i + '</a></li>';
          }
        }

        str += '<li><a class="last-page" ' + lastPage + ' data-code="LastPage">'+ lastPageHtml +'</a></li>';
        str += '</ul>';
        str += '</nav>'

        //如果只有1页，就不显示分页了
        //if (e > 1) {
        //    $('.crBottom').html(str);
        //}
        $('.bottom').html(str);
        arrChoose = []; // 清空选择数组
    });
}

/*分页算法
总页数:totalpage,
显示页数:viewpage,
当前页数:currentPage
callback;(startIndex：开始页码, endIndex：结束页码, curIndex：当前第几页)回掉函数
*/
function pageCalc(totalpage, currentPage, callback) {
    var viewpage = 5;
    var startindex, endindex;
    if (viewpage >= totalpage) {
        startindex = 1;
        endindex = totalpage;
    } else {
        if (currentPage <= viewpage / 2) {
            startindex = 1;
            endindex = viewpage;
        } else if ((currentPage + viewpage / 2) > totalpage) {
            startindex = totalpage - viewpage + 1;
            endindex = totalpage;
        } else {
            startindex = currentPage - parseInt((viewpage - 1) / 2);
            endindex = currentPage + parseInt(viewpage / 2);
        }
    }
    var page = { startIndex: startindex, endIndex: endindex, curIndex: currentPage };
    callback(page.startIndex, page.endIndex, page.curIndex);
}


function gobackpage() {
    // window.history.back();
    window.history.go(-1);
}

