<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisheTyperefer.aspx.cs" Inherits="CommunityBuy.BackWeb.manage.DisheTyperefer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../js/dishes/css/common.css" rel="stylesheet" />
    <script src="../js/dishes/jquery.min.js"></script>
    <script src="../js/dishes/common.js"></script>
    <style type="text/css">
        .divrow {
            width: 100%;
            clear: both;
        }

        .parentcontent {
            float: left;
            line-height: 42px;
            width: 15%;
            cursor: pointer;
            text-align: center;
            font-weight: bold;
            color: #000;
            margin-right: 8px;
        }

        .content {
            float: left;
            /*margin-left: 88px;*/
            text-align: left;
            width: 80%;
        }

            .content ul li {
                float: left;
                width: 150px;
                text-align: center;
                line-height: 42px;
                margin: 0px 8px 8px 0px;
                cursor: pointer;
            }

        ul {
            list-style: none;
            padding: 0;
            margin: 0;
        }

        .selectli {
            background: #def2fe;
        }

        #methodsdiv {
            height: 400px;
            border-top: 1px solid #bdbdbd;
            border-bottom: 1px solid #bdbdbd;
            overflow-y: auto;
        }

        .cancelbut {
            float: right;
            width: 80px;
            margin-right: 12px;
            background: #bdbdbd;
            color: #fff;
            line-height: 40px;
            height: 40px;
            text-align: center;
            cursor: pointer;
        }

        .cancelbut, .savebut {
            float: right;
            width: 80px;
            margin-right: 12px;
            color: #fff;
            line-height: 40px;
            height: 40px;
            text-align: center;
            cursor: pointer;
        }

        .cancelbut {
            background: #bdbdbd;
        }

        .savebut {
            background: #f74646;
        }

        .title {
            height: 40px;
            line-height: 40px;
            text-align: center;
            background-color: #ececec;
        }
    </style>
    <script type="text/javascript">
        //页面初始化函数
        $(document).ready(function () {
            var stocode = getUrlParam("stocode");
            if (getUrlParam("fun") == "setDisheTypeInfo") {
                $("#_title").text("所属分类");
            }
            //绑定菜品类别信息
            commonAjax('../../ajax/dishes/WSDisheType.ashx', getpostParameters('getlist', { "GUID": "0", "USER_ID": "0", "pageSize": 10000, "currentPage": 1, "filter": " [status]='1' and stocode='" + stocode + "'", "order": "" }), true, loadDishesType);
        });

        function addclick() {
            $('#methodsdiv').on('click', 'div div ul li', function (e) {
                $('#methodsdiv div div ul li').removeClass('selectli');
                $('#methodsdiv div').removeClass('selectli');
                $(e.target).toggleClass('selectli');
                if ($(e.target).is('.selectli')) {
                    var parentdiv = e.target.parentNode.parentNode.parentNode;
                    if (!$(parentdiv).find(".parentcontent").is('.selectli')) {
                        $(parentdiv).find(".parentcontent").addClass('selectli');
                    }
                }
            });
        }

        function loadDishesType(data) {
            if (data != undefined) {
                var listData = data.data;
                var levelone = new Array();
                var leveltwo = new Array();
                for (var i = 0; i < listData.length; i++) {
                    if (listData[i].pdistypecode.length == 0) {//level 1
                        levelone.push(listData[i]);
                    }
                    else {//level 2
                        leveltwo.push(listData[i]);
                    }
                }

                var html = '';
                $(levelone).each(function (i, o) {

                    html += '<div class="divrow">';
                    html += '<div class="parentcontent">' + o.distypename + '</div>';
                    html += '<div class="content"><ul>';
                    for (var j = 0; j < leveltwo.length; j++) {
                        var two = leveltwo[j];
                        if (o.distypecode == two.pdistypecode) {
                            html += '<li data-val="' + two.distypecode + '">' + two.distypename + '</li>';
                        };
                    }
                    html += '</ul></div>';
                    html += '</div>';
                    html += '</div>';
                });
                $('#methodsdiv').html(html);
                addclick();
            }
        }

        //
        function confirmok() {
            var index = parent.layer.getFrameIndex(window.name);
            var fun = getUrlParam('fun');
            //遍历选中项
            var code = '';
            var name = '';
            $('#methodsdiv').find('.content ul li').each(function (i, e) {
                if ($(e).is('.selectli')) {
                    code = $(e).attr('data-val');
                    name = $(e).text();
                }
            });
            eval("parent." + fun + "('" + code + "','" + name + "')");
            parent.layer.close(index);
        }
        //
        function closewindow() {
            var index = parent.layer.getFrameIndex(window.name);
            parent.layer.close(index);
        }
    </script>
</head>
<body>
    <div class="title">
        <h3>
            <label id="_title" data-code="">所属分类</label>
        </h3>
    </div>
    <div id="methodsdiv">
        <div class="divrow">
            <div class="parentcontent">醉月做法</div>
            <div class="content">
                <ul>
                    <li>1</li>
                    <li>2</li>
                    <li>3</li>
                    <li>4</li>
                    <li>5</li>
                    <li>6</li>
                    <li>7</li>
                    <li>8</li>
                    <li>9</li>
                </ul>
            </div>
        </div>
    </div>
    <div style="width: 100%; line-height: 60px; height: 60px; text-align: right; vertical-align: middle; margin-top: 20px;">
        <div data-code="Save" onclick="confirmok();" class="savebut">确定</div>
        <div data-code="cancel" onclick="closewindow();" class="cancelbut">取消</div>
    </div>
</body>
</html>
