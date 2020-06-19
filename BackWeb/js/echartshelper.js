//// 基于准备好的dom，初始化echarts实例
//function clickdemo(myChart) {------echarts点击事件  可用作回调方法
//    myChart.on('click', function (parmas) {
//        console.log(parmas.name)
//    })
//}
//var data = {------仿造的数据源
//    status: 0,
//    msg: '',
//    data: [{ name: '销量', data: [{ 'name': '衬衫', 'value': 10 }, { 'name': '羊毛衫', 'value': 50 }, { 'name': '雪纺衫', 'value': 80 }, { 'name': '裤子', 'value': 60 }, { 'name': '高跟鞋', 'value': 90 }] }
//    , { name: '测试', data: [{ 'name': '衬衫', 'value': 20 }, { 'name': '羊毛衫', 'value': 80 }, { 'name': '雪纺衫', 'value': 30 }, { 'name': '裤子', 'value': 50 }, { 'name': '高跟鞋', 'value': 80 }] }]
//}
//var newdata = data.data;
//var tabletitlelist = [];
//var xaxis = [];
//var datalist = [];
//for (var i = 0; i < newdata.length; i++) {-------数据解析  让数据成为想要的形式
//    var datademo = JSON.parse(JSON.stringify(columnardatademo));----数据模型  所有的线形图和柱状图全部按照此模型格式，将数据按需求放入
//    var newdatalist = newdata[i]
//    tabletitlelist.push(newdatalist.name);
//    for (var j = 0; j < newdatalist.data.length; j++) {
//        if (i == 0) {
//            xaxis.push(newdatalist.data[j].name);
//        }
//        datademo.data.push(newdatalist.data[j].value)
//    }
//    datademo.name = newdatalist.name;
//    datalist.push(datademo);
//}
//var datalist=[{value:235, name:'视频广告'},{value:274, name:'联盟广告'},{value:310, name:'邮件营销'},{value:335, name:'直接访问'},{value:400, name:'搜索引擎'}];-------仿造的饼图数据源
//echelper.columnarwithevent('main', '测试demo', tabletitlelist, xaxis, datalist, clickdemo)
//echelper.pie('main',datalist)-----调用饼图方法






var piestyle = {//饼图样式  在调用饼图方法前先通过piestyle.name..来改成想要的样式
    name: '',//name
    bgcolor: '',//背景色
    radius: '55%',//图表占比
    rosetype: 'angle',//图标显示类型(为空时是饼图,angle(南丁格尔图),area(等角南丁格尔图))
    textcolor: '',//文字颜色
    linecolor: '',//线颜色
    shadowblur: 0,//阴影大小
    shadowcolor: ''//阴影位置
}
var columnarstyle = {//线形图、柱状图统一样式设置 在调用饼图方法前先通过columnarstyle.name..来改成想要的样式
    bgcolor: '',
    xisnzero: true,//x轴是否不从0刻度开始true(不从)false(从)
    containLabel: true,
    left: '',//设置图表距左位置
    right: '',//设置图表距右位置
    bottom: ''//设置图表距下位置
}
var columnardatademo = {//线形图、柱状图数据格式，调用
    name: '',//源数据名称
    type: 'bar',//line(线形图)bar(柱状图)
    smooth: false,//true是曲线图，false是线形图
    stack: '',//线形图数据堆叠时设置同一个名称但是显示数据曲线不是真实的，为空数据不堆叠，显示真实数据曲线
    //areaStyle: { color: ['rgba(20,25,20,0.4)']},//线形图时添加会有颜色填充可以根据color的值得到自己想要的颜色,按情况选择是否需要
    label: {
        normal: {
            show: true,
            position: 'top'
        }
    },
    itemStyle: {
        normal: {
            color: '',//设置柱状图或线形图颜色
        }
    },
    data: [],//数据源,例:[5, 20, 36, 10, 10, 20,100]
}

var echelper = {
    columnar: function (id, tabletitle, tabletitlelist, xaxis, datalist) {//柱状图：myChart(表格名),tabletitlelist(所属数据名),xaxis(节点名),datalist(数据源)
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            boundaryGap: columnarstyle.xisszreo,
            backgroundColor: columnarstyle.bgcolor,//背景色,看需求设置,可为空
            title: {
                text: tabletitle//可为空
            },
            tooltip: {},//鼠标移上去会有提示框
            grid: {
                left: columnarstyle.left,
                right: columnarstyle.right,
                bottom: columnarstyle.bottom,
                containLabel: columnarstyle.containLabel
            },
            legend: {
                data: tabletitlelist//['测试1','测试2']
            },
            xAxis: {
                data: xaxis//["衬衫","羊毛衫","雪纺衫","裤子","高跟鞋","袜子"]
            },
            yAxis: {},
            series: datalist//[{name:'测试1',type:'bar',data:[5, 20, 36, 10, 10, 20,100]},{name:'测试2',type:'bar',data:[5, 20, 36, 10, 10, 20,1]}](type:bar(柱状图),line(线形图))
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
    },
    columnarwithevent: function (id, tabletitle, tabletitlelist, xaxis, datalist, clickdemo) {//柱状图：myChart(表格名),tabletitlelist(所属数据名),xaxis(节点名),datalist(数据源),clickdemo(需要实现的操作,点击事件等)
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            boundaryGap: columnarstyle.xisszreo,
            backgroundColor: columnarstyle.bgcolor,//背景色,看需求设置,可为空
            title: {
                text: tabletitle//可为空
            },
            tooltip: {},//鼠标移上去会有提示框
            grid: {
                left: columnarstyle.left,
                right: columnarstyle.right,
                bottom: columnarstyle.bottom,
                containLabel: columnarstyle.containLabel
            },
            legend: {
                data: tabletitlelist//['测试1','测试2']
            },
            xAxis: {
                data: xaxis//["衬衫","羊毛衫","雪纺衫","裤子","高跟鞋","袜子"]
            },
            yAxis: {},
            series: datalist//[{name:'测试1',type:'bar',data:[5, 20, 36, 10, 10, 20,100]},{name:'测试2',type:'bar',data:[5, 20, 36, 10, 10, 20,1]}](type:bar(柱状图),line(线形图))
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
        clickdemo(myChart);
    },
    pie: function (id, datalist) {//id:表格ID,bgcolor:背景色,name:可为空
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            backgroundColor: piestyle.bgcolor,
            series: [
                {
                    name: piestyle.name,
                    type: 'pie',
                    radius: piestyle.radius,
                    data: datalist,//[{value:235, name:'视频广告'},{value:274, name:'联盟广告'},{value:310, name:'邮件营销'},{value:335, name:'直接访问'},{value:400, name:'搜索引擎'}];(数据格式)
                    roseType: piestyle.rosetype,
                    label: {
                        normal: {
                            textStyle: {
                                color: piestyle.textcolor
                            }
                        }
                    },
                    labelLine: {
                        normal: {
                            lineStyle: {
                                color: piestyle.linecolor
                            }
                        }
                    },
                    itemStyle: {
                        normal: {
                            shadowBlur: piestyle.shadowblur,
                            shadowColor: piestyle.shadowcolor
                        }
                    }
                }
            ]
        };
        myChart.setOption(option);
    }
}