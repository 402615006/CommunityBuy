<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreEdit.aspx.cs" Inherits="CommunityBuy.BackWeb.StoreEdit" %>

<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <link href="/css/switch.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/CWebControl.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>
    <%--<script src="../js/Pinyin.js"></script>--%>
    <script src="../js/Pinyin/pinyin_dict_notone.js"></script>
    <script src="../js/Pinyin/pinyin_dict_withtone.js"></script>
    <script src="../js/Pinyin/pinyinUtil.js"></script>
    <!--图片上传控件-->
    <link rel="stylesheet" type="text/css" href="/js/webuploader/webuploader.css"/>
    <script type="text/javascript" src="/js/webuploader/webuploader.min.js"></script>
    <style>
        .resizeminwidth {
            width: 155px;
        }

        .resizeminwidthbytime {
            width: 55px;
        }

        #fileserviceUploader {
            width: 80px;
            height: 30px;
            border-radius: 5px;
            margin: 10px;
            float: left;
        }
    </style>
    <script>

        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        $(function () {
            $('.btn-recyble').click(function (e) {
                var obj = e.currentTarget;
                if ($(obj).hasClass('on')) {
                    $(obj).removeClass('on');
                    $(obj).attr('src', '/img/off.png');
                    $("#ddl_pstocode").attr("disabled", "disabled");
                    $("#ddl_pstocode").removeClass("selstyle");
                    $("#ddl_pstocode").val("");
                }
                else {
                    $(obj).addClass('on');
                    $(obj).attr('src', '/img/on.png');
                    $("#ddl_pstocode").removeAttr("disabled");
                    $("#ddl_pstocode").addClass("selstyle");
                }
            });

            var id = 0;
            if (getQueryString("id") != null) {
                id = getQueryString("id");
            }


            $(window).resize(function () {
                if ($(".atarea").width() < 550) {
                    $(".atarea select").addClass("resizeminwidth");
                    $("#txt_btime").addClass("resizeminwidthbytime");
                    $("#txt_etime").addClass("resizeminwidthbytime");
                } else {
                    $(".atarea select").removeClass("resizeminwidth");
                    $("#txt_btime").removeClass("resizeminwidthbytime");
                    $("#txt_etime").removeClass("resizeminwidthbytime");
                }
            });

            getPinyin();

            document.getElementById('txt_cname').addEventListener('input', getPinyin);
            if ($("#hidId").val().length <= 0) {
                addclick();
            }
            else {
                showImages();
            }

            if ($("#hid_logo").val()) {
                $("#logo").attr("src", $("#hid_logo").val());
            }

            if ($("#hid_backgroundimg").val()) {
                $("#backgroundimg").attr("src", $("#hid_backgroundimg").val());
            }

            InitalImagUploader();
        });

        function getPinyin() {
            var value = $('#txt_cname').val();
            var result = '';
            if (value) {
                result = pinyinUtil.getFirstLetter(value, false);
            }
            var html = result;
            if (result instanceof Array) {
                html = '<ol>';
                result.forEach(function (val) {
                    html += '<li>' + val + '</li>';
                });
                html += '</ol>';
            }
            $("#txt_bcode").val(html);
        }

        function addbtn(src) {
            var len = $("#gv_list tr").length;
            if (len > 0) {
                if (len % 2 == 0) {
                    $("#gv_list").append("<tr class='List_tab_alter' style='background-color:#EFF3FB;' id='tr_" + len + "'><td class='chooseclass'><input type='checkbox' name='tr_" + len + "'></td><td width='60%'><input data-code='请输入' placeholder='请输入' isrequired='true' type='text' class='wheretxt' id='servicename" + len + "' style='width:500px'/></td><td><img width='90' height='60' id='img_" + len + "' src='" + src + "' /></td></tr>");
                }
                else {
                    $("#gv_list").append("<tr class='List_tab_alter' style='background-color:White;' id='tr_" + len + "'><td class='chooseclass'><input type='checkbox' name='tr_" + len + "'></td><td width='60%'><input data-code='请输入' placeholder='请输入' isrequired='true' type='text' class='wheretxt' id='servicename" + len + "' style='width:500px'/></td><td><img width='90' height='60' id='img_" + len + "' src='" + src + "' /></td></tr>");
                }
            }
        }

        function delbtn() {
            var valu = "";
            $("#gv_list input[type=checkbox]").each(function () {
                if ($(this).is(':checked') && $(this).attr('name') != "cbAll") {
                    valu += $(this).attr('name') + ",";
                }
            });
            if (valu.length > 0) {
                var valu_arr = valu.substring(0, valu.length - 1);
                var valu_array = valu_arr.split(',');

                for (var i = 0; i < valu_array.length; i++) {
                    $("#" + valu_array[i]).remove();
                }
                $("#cbAll").attr("checked", false);
            }
        }

        function InitalImagUploader() {
            var uploader = WebUploader.create({
                auto: true,
                // swf文件路径
                swf: '/js/webuploader/Uploader.swf',
                //formData: { 'filetype': 'storelogo', 'pwd':'combuy'},
                // 文件接收服务端。
                server: '/ajax/UploadFile.ashx',
                // 选择文件的按钮。可选。
                // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                pick: '#logopicker',

                // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                resize: false
            });
        }

        function addclick() {
            var n = $("#images img").length;
            $("#images").append("<img index='" + (n + 1).toString() + "' class='imageon'>");
            $("#buttons").append("<span onclick='show(this)' index='" + (n + 1).toString() + "' class='on'>" + (n + 1).toString() + "</span>");
            show($("#buttons span:last")[0]);
        }

        function delclick() {
            var count = $("#images img").length;
            var index = $("#buttons span.on").attr("index");
            $("#buttons span.on").remove();
            $("#images img.imageon").remove();
            if (count > 1) {
                var i = 1;
                $("#images img").each(function () {
                    $(this).attr("index", i.toString());
                    i++;
                });
                i = 1;
                $("#buttons span").each(function () {
                    $(this).attr("index", i.toString());
                    $(this).html(i.toString());
                    i++;
                });
                show($("#buttons span:first")[0]);
            }
        }

        function saveImage() {
            var imgurls = "";
            $("#images img").each(function () {
                imgurls += ($(this).attr("src") + ",");
            });
            if (imgurls.length > 0) {
                imgurls = imgurls.substr(0, imgurls.length - 1);
            }
            $("#hidstopath").val(imgurls);

            var strData = "";
            var n = $("#gv_list tr").length;
            if (n > 1) {
                for (var i = 1; i < n; i++) {
                    strData += $("#servicename" + i).val();
                    strData += "|";
                    strData += $("#img_" + i).attr("src");
                    strData += ",";
                }
                if (strData.length > 0) {
                    strData = strData.substring(0, strData.length - 1);
                }
                $("#hidLvData").val(strData);
            }
            else {
                $("#hidLvData").val("");
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

        function savebtn() {
            var flag = FormDataValidationCheck();
            if (flag) {
                    $("#Save_btn").click();
            }
        }

    </script>

    <style>
        .delete {
            height: 20px;
            width: 20px;
            line-height: 20px;
        }
        /** {
            margin:0;
            padding:0;
          }*/
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
</head>
<body data-pagecode="Store">
    <form id="form1" data-tbname="Store" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="Storelist" PageType="Add" MainMenu="" SubMenu="商家门店信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="商家门店信息" Operate="Add" runat="server"></cc1:CPageTitle>
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
                        <td data-code="span_stocode" class="lefttd">门店编号：</td>
                        <td>
                            <cc1:CTextBox ID="txt_stocode" data-code="stocode_placeholder" CssClass="txtstyle" MaxLength="8" TextType="Normal" onblur="onblurCheck('txt_stocode')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_stocode_span"></span>						</td>
                        <td data-code="span_cname" class="lefttd">门店名称：</td>
                        <td>
                            <cc1:CTextBox ID="txt_cname" data-code="cname_placeholder" CssClass="txtstyle" MaxLength="64" IsRequired="true" TextType="Normal" onblur="onblurCheck('txt_cname');" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_cname_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_sname" class="lefttd">门店简称：</td>
                        <td>
                            <cc1:CTextBox ID="txt_sname" data-code="sname_placeholder" CssClass="txtstyle" MaxLength="16" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_sname')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_sname_span"></span>						</td>
                        <td data-code="span_bcode" class="lefttd">门店简码：</td>
                        <td>
                            <cc1:CTextBox ID="txt_bcode" data-code="bcode_placeholder" CssClass="txtstyle" MaxLength="16" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_bcode')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_bcode_span"></span>						</td>

                        <td data-code="span_tel" class="lefttd">门店电话：</td>
                        <td>
                            <cc1:CTextBox ID="txt_tel" data-code="tel_placeholder" CssClass="txtstyle" MaxLength="32" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_tel')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_tel_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_provinceid" class="lefttd">所在区域：</td>
                        <td class="atarea">
                            <cc1:CDropDownList ID="ddl_provinceid" data-code="status_placeholder" Descr="省" CssClass="selstyle" TextType="Normal" runat="server" OnSelectedIndexChanged="ddl_provinceid_SelectedIndexChanged" AutoPostBack="true">
                            </cc1:CDropDownList>
                            <cc1:CDropDownList ID="ddl_cityid" data-code="status_placeholder" Descr="市" CssClass="selstyle" TextType="Normal" runat="server" OnSelectedIndexChanged="ddl_cityid_SelectedIndexChanged" AutoPostBack="true">
                            </cc1:CDropDownList>
                            <cc1:CDropDownList ID="ddl_areaid" data-code="status_placeholder" Descr="区域" CssClass="selstyle" TextType="Normal" runat="server" OnSelectedIndexChanged="ddl_areaid_SelectedIndexChanged" AutoPostBack="true">
                            </cc1:CDropDownList>
                        </td>
                        <td class="lefttd">所属商圈：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_sq" data-code="storetype_placeholder" Descr="所属商圈" CssClass="selstyle" TextType="Normal" runat="server"  IsRequired="False" >
                            </cc1:CDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_address" class="lefttd">门店地址：</td>
                        <td colspan="5">
                            <cc1:CTextBox ID="txt_address" data-code="address_placeholder" CssClass="txtstyle" MaxLength="128" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_address')" placeholder="" runat="server" Width="90%"></cc1:CTextBox><span class="redmess" id="txt_address_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_stoprincipal" class="lefttd">负责人：</td>
                        <td>
                            <cc1:CTextBox ID="txt_stoprincipal" data-code="stoprincipal_placeholder" CssClass="txtstyle" MaxLength="32" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_stoprincipal')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_stoprincipal_span"></span>						</td>
                        <td data-code="span_stoprincipaltel" class="lefttd">负责人联系电话：</td>
                        <td>
                            <cc1:CTextBox ID="txt_stoprincipaltel" data-code="stoprincipaltel_placeholder" CssClass="txtstyle" MaxLength="32" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_stoprincipaltel')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_stoprincipaltel_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_stourl" class="lefttd">门店网址：</td>
                        <td>
                            <cc1:CTextBox ID="txt_stourl" data-code="stourl_placeholder" CssClass="txtstyle" MaxLength="128" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_stourl')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_stourl_span"></span>						</td>
                        <td data-code="span_stocoordx" class="lefttd">X坐标：</td>
                        <td>
                            <cc1:CTextBox ID="txt_stocoordx" data-code="stocoordx_placeholder" CssClass="txtstyle" MaxLength="8" IsRequired="true" TextType="Normal" onblur="onblurCheck('txt_stocoordx')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_stocoordx_span"></span>						</td>
                        <td data-code="span_stocoordy" class="lefttd">Y坐标：</td>
                        <td>
                            <cc1:CTextBox ID="txt_stocoordy" data-code="stocoordy_placeholder" CssClass="txtstyle" MaxLength="8" IsRequired="true" TextType="Normal" onblur="onblurCheck('txt_stocoordy')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_stocoordy_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_busHour" class="lefttd">营业时间：</td>
                        <td>
                            <cc1:CTextBox ID="txt_btime" data-code="btime_placeholder" CssClass="txtstyle" IsRequired="true" TextType="Normal" onblur="onblurCheck('txt_btime')" onclick="ShowHM();" onfocus="ShowHM();" placeholder="" runat="server" Width="50"></cc1:CTextBox>
                            <span>-</span>
                            <cc1:CTextBox ID="txt_etime" data-code="etime_placeholder" CssClass="txtstyle" IsRequired="true" TextType="Normal" onblur="onblurCheck('txt_etime')" onclick="ShowHM();" onfocus="ShowHM();" placeholder="" runat="server" Width="50"></cc1:CTextBox>
                        </td>
                        <td data-code="span_status" class="lefttd">状态：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_status" data-code="status_placeholder" Descr="状态" CssClass="selstyle" Width="60" TextType="Normal" runat="server" SelType="Status">
                            </cc1:CDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="lefttd">所属行业：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_storetype" data-code="storetype_placeholder" Descr="所属行业" CssClass="selstyle" TextType="Normal" runat="server">
                            </cc1:CDropDownList>
                        </td>
                        <td class="lefttd">人均消费：</td>
                        <td>
                            <cc1:CTextBox ID="txt_jprice" data-code="jprice_placeholder" CssClass="txtstyle" MaxLength="8" IsRequired="true" TextType="Decimal" placeholder="" runat="server"></cc1:CTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_recommended" class="lefttd">推荐：</td>
                        <td colspan="5">
                            <cc1:CTextBox ID="txt_recommended" data-code="recommended_placeholder" CssClass="txtstyle" MaxLength="128" Width="90%" IsRequired="False" TextType="Normal" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_recommended_span"></span>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_logo" class="lefttd">门店Logo：</td>
                        <td colspan="5">
                            <div>
                                <div style="padding-top: 10px; float: left">
                                    <img runat="server" id="logo" width="200" height="200" style="float: left" />
                                </div>
                                <div style="padding-top: 50px; float: left; padding-left: 20px">
                                    <div id="thelist" class="uploader-list"></div>
                                    <div id="logopicker">选择文件</div>
                                    <button id="ctlBtn" class="btn btn-default">开始上传</button>
                                    <input type="hidden" id="hid_logo" runat="server" />
                                </div>
                                <div style="float: left; padding-top: 50px">
                                    <span data-code="span_logoinfo" class="lefttd"></span>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_stopath" class="lefttd">轮翻图：</td>
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
                        <td></td>
                        <td colspan="5">
                            <input type="file" name="upimage" id="upimage" style="visibility: hidden" class="lefttd" />
                            <div style="float: left">
                                <span data-code="span_imgLFinfo"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td  colspan="5">
                            <div style="width: 600px">
                                <input class="addbtn" type="button" value="添加" onclick="addclick();" />
                                <input class="delbtn" type="button" value="删除" onclick="delclick();" />
                                <div id="div_upimage"></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_descr" class="lefttd">门店描述：</td>
                        <td colspan="5">
                            <div style="padding-top: 15px">
                                <cc1:CTextBox ID="txt_descr" data-code="descr_placeholder" CssClass="txtstyle" MaxLength="0" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_descr')" Width="90%" Height="200px" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_descr_span"></span>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="updatediv clb" style="display: none">
                <table>
                    <tr>
                        <td>
                            <div>
                                <input class="addbtn" type="file" name="fileservice" id="fileservice" />
                                <div class="delbtn" data-code="delete_edit" id="delbtn" onclick="delbtn();" style="font-size: 20px">删除</div>
                                <div>
                                    <span data-code="span_imgFWinfo" class="lefttd"></span>
                                </div>
                                <div id="div_upservice"></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                </table>
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
                            <input type="hidden" id="hidCheck" runat="server" />
                        </td>
                        <td>
                            <div id="savebtn" onclick="savebtn();">保存</div>
                            <div class="gobackbtn">返回</div>
                            <cc1:CButton ID="Save_btn" runat="server" Style="display: none" OnClick="Save_btn_Click" />
                            <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="bottomline"></div>
        </div>
    </form>
</body>
</html>
