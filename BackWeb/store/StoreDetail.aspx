<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreDetail.aspx.cs" Inherits="CommunityBuy.BackWeb.StoreDetail" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <link href="/css/liststyle.css" rel="stylesheet" />

    <style>
        .container {
            position: relative;
            width: 720px;
            height: 200px;
            /*margin:100px auto 0 auto;*/
            box-shadow: 0 0 5px green;
            overflow: hidden;
        }

        a {
            text-decoration: none;
        }

        .container .wrap {
            position: absolute;
            width: 5040px;
            height: 200px;
            z-index: 1;
        }

            .container .wrap img {
                float: left;
                width: 720px;
                height: 200px;
            }

        .container .buttons {
            position: absolute;
            right: 5px;
            bottom: 30px;
            width: 400px;
            height: 10px;
            z-index: 2;
        }

            .container .buttons span {
                margin-left: 5px;
                display: inline-block;
                width: 12px;
                height: 12px;
                border-radius: 50%;
                background-color: rgb(211, 212, 211);
                text-align: center;
                color: white;
                cursor: pointer;
            }

                .container .buttons span.on {
                    background-color: rgb(151, 150, 150);
                }

        .container .arrow {
            position: absolute;
            top: 40%;
            color: rgb(179, 182, 179);
            padding: 0px 15px;
            border-radius: 50%;
            font-size: 50px;
            z-index: 2;
            display: none;
        }

        .container .arrow_left {
            left: 10px;
        }

        .container .arrow_right {
            right: 10px;
        }

        .container:hover .arrow {
            display: block;
        }

        .container .arrow:hover {
            background-color: rgba(0,0,0,0.2);
        }
    </style>

    <script>
        $(function () {
            showImages();
        });
        function addbtn(src) {
            var len = $("#gv_list tr").length;
            if (len > 0) {
                if (len % 2 == 0) {
                    $("#gv_list").append("<tr class='List_tab_alter' style='background-color:#EFF3FB;' id='tr_" + len + "'><td class='chooseclass'><input type='checkbox' name='tr_" + len + "'></td><td width='60%'><input isrequired='true' disabled='disabled' type='text' class='wheretxt' id='servicename" + len + "' style='width:500px'/></td><td><img width='90' height='60' id='img_" + len + "' src='" + src + "' /></td></tr>");
                }
                else {
                    $("#gv_list").append("<tr class='List_tab_alter' style='background-color:White;' id='tr_" + len + "'><td class='chooseclass'><input type='checkbox' name='tr_" + len + "'></td><td width='60%'><input isrequired='true' disabled='disabled' type='text' class='wheretxt' id='servicename" + len + "' style='width:500px'/></td><td><img width='90' height='60' id='img_" + len + "' src='" + src + "' /></td></tr>");
                }
            }
        }
        function showImages() {
            var stopath = $("#hidstopath").val();
            var services = $("#hidLvData").val();
            if (stopath.length > 0) {
                var arr1 = stopath.split(",");
                if (arr1 != null && arr1.length > 0) {
                    for (var i = 0; i < arr1.length; i++) {
                        var n = $("#images img").length;
                        $("#images").append("<img index='" + (n + 1).toString() + "' src='" + arr1[i] + "'>");
                        $("#buttons").append("<span onclick='show(this)' index='" + (n + 1).toString() + "'>" + (n + 1).toString() + "</span>");
                    }
                    show($("#buttons span:first")[0]);
                }
            }
            if (services.length > 0) {
                var arr1 = services.split(",");
                if (arr1 != null && arr1.length > 0) {
                    for (var i = 0; i < arr1.length; i++) {
                        var arr2 = arr1[i].split("|");
                        if (arr2.length == 2) {
                            addbtn(arr2[1]);
                            $("#servicename" + (i + 1).toString()).val(arr2[0]);
                        }
                    }
                }
            }
        }

        function show(span) {
            if (span == undefined) {
                return;
            }
            $("#buttons span").attr("class", "");
            $("#images img").attr("class", "");
            var index = span.attributes["index"].value;
            span.className = "on";
            //        
            var arr = $("#images img");
            for (var i = 0, count = arr.length; i < count; i++) {
                if (arr[i].attributes["index"].value == index) {
                    arr[i].className = "imageon";
                }
            }
            $("#images img")[index - 1]
            var wrap = document.getElementById("images");
            wrap.style.left = "-" + (720) * (index - 1).toString() + "px";
        }
    </script>
</head>
<body data-pagecode="Store">
    <form id="form1" data-tbname="Store" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="Storelist" PageType="Detail" MainMenu="" SubMenu="商家门店信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="商家门店信息" Operate="详情" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
                <div data-code="tab2" class="graydiv" title="clb">服务信息</div>
            </div>
            <div class="updatediv cla">
                <table>
                    <tr>
                        <td data-code="span_comcode" class="lefttd">所属公司名称：</td>
                        <td><span id="comcode" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_buscode" class="lefttd">所属商户：</td>
                        <td><span id="buscode" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_stocode" class="lefttd">门店编号：</td>
                        <td><span id="stocode" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_cname" class="lefttd">门店名称：</td>
                        <td><span id="cname" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_sname" class="lefttd">门店简称：</td>
                        <td><span id="sname" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_bcode" class="lefttd">门店简码：</td>
                        <td><span id="bcode" runat="server"></span></td>
                    </tr>

                    <tr>
                        <td data-code="span_provinceid" class="lefttd">所在区域：</td>
                        <td><span id="provinceid" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_address" class="lefttd">门店地址：</td>
                        <td><span id="address" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_stoprincipal" class="lefttd">负责人：</td>
                        <td><span id="stoprincipal" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_stoprincipaltel" class="lefttd">负责人联系电话：</td>
                        <td><span id="stoprincipaltel" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_tel" class="lefttd">门店电话：</td>
                        <td><span id="tel" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_stoemail" class="lefttd">门店邮箱：</td>
                        <td><span id="stoemail" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_stourl" class="lefttd">门店网址：</td>
                        <td><span id="stourl" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_stocoordx" class="lefttd">X坐标：</td>
                        <td><span id="stocoordx" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_stocoordy" class="lefttd">Y坐标：</td>
                        <td><span id="stocoordy" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_calcutime" class="lefttd">：</td>
                        <td><span id="calcutime" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_status" class="lefttd">有效状态（0无效，1有效）：</td>
                        <td><span id="status" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_logo" class="lefttd">门店Logo：</td>
                        <td>
                            <img runat="server" id="logo" width="200" height="200" /></td>
                    </tr>
                    <%--<tr style="visibility: collapse">
                        <td data-code="span_backgroundimg" class="lefttd">门店背景图：</td>
                        <td>
                            <img runat="server" id="backgroundimg" width="180" height="160" /></td>
                    </tr>--%>
                    <tr>
                        <td data-code="span_backgroundimg" class="lefttd">轮翻图：</td>
                        <td colspan="5">
                            <div class="container">
                                <div class="wrap lefttd" style="left: -1440px;" id="images">
                                </div>
                                <div class="buttons lefttd" id="buttons">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_descr" class="lefttd">门店描述：</td>
                        <td><span id="descr" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td class="lefttd">行业类型：</td>
                        <td><span id="storetype" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td class="lefttd">人均消费：</td>
                        <td><span id="jprice" runat="server"></span></td>
                    </tr>
                    <tr>
                        <td class="lefttd">支付类型：</td>
                        <td><span id="paytype" runat="server"></span></td>
                    </tr>
                </table>
            </div>
            <div class="updatediv clb" style="display: none">
                <table id="gv_list" class="List_tab" style="width: 100%; border-collapse: collapse;">
                    <tr class='list_tab_tit' style='font-weight: bold;'>
                        <td class='chooseclass'>
                            <input type='checkbox' id='cbAll' name='cbAll' onclick='CheckAll();' /></td>
                        <td data-code="servicename_list"></td>
                        <td data-code="serviceimg_list"></td>
                    </tr>
                </table>
            </div>
            <div style="margin-left: 30px">
                <table>
                    <tr>
                        <td>
                            <input type="hidden" id="hidId" runat="server" />
                            <input type="hidden" id="hidstopath" runat="server" />
                            <input type="hidden" id="hidLvData" runat="server" />
                        </td>
                        <td>
                            <div class="gobackbtn">返回</div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
