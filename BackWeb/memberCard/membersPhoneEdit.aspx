<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="membersPhoneEdit.aspx.cs" Inherits="CommunityBuy.BackWeb.memberCard.membersPhoneEdit" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/CWebControl.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>

    <script src="../js/jquery.uploadify/jquery.uploadify.v2.1.4.js" charset="gbk" type="text/javascript"></script>
    <script src="../js/jquery.uploadify/swfobject.js"></script>
    <link href="../js/jquery.uploadify/uploadify.css" rel="stylesheet" />
    <style>
        .sign div {
            text-align: center;
        }

        .sign a {
            text-align: center;
            font-size: 25px;
        }

        .savebtn1 {
            font-size: 13px;
        }

        .gobackbtn1 {
            float: left;
            width: 80px;
            text-align: center;
            border-radius: 5px;
            height: 30px;
            line-height: 30px;
            color: white;
            cursor: pointer;
            background-color: #7A7A7A;
            margin: 10px;
        }

        #errormessage {
            visibility: visible!important;
        }
    </style>
    <script>
        function getYZMnew() {
            if ($("#YZMnew").html() != "获取验证码") {
                return;
            }
            if ($("#txt_newphone").val().length <= 0) {
                layer.alert("请输入新手机号", { icon: 1 });
            }
            else if ($("#txt_newphone").val().length != 11) {
                layer.alert("请输入正确的手机号", { icon: 1 });
            } else {
                if ($("#txt_newphone").val() == $("#txt_oldphone").val()) {
                    layer.alert("新旧手机号不能相同", { icon: 1 });
                    return;
                }
                var c = 90;
                var interval = setInterval(function () {
                    $("#YZMnew").html(c + "s之后重试");
                    c--;
                    if (c == 0) {
                        clearInterval(interval);
                        $("#YZMnew").html("获取验证码");
                        c = 90;
                    }
                }, 1000);
                $.ajax({
                    url: "../ajax/membercard/getmemberCard.ashx",
                    dataType: "json",
                    data: { "actionname": "sendmessage", "parameters": "{ \"type\":\"1\", \"phone\": \"" + $("#txt_newphone").val() + "\" }" },
                    type: "GET",
                    success: function (req) {
                        if (req.status == '0') {
                            $("#errormessage").html("发送成功");
                            $("#errormessage").css("visibility", "collapse");
                            $("#hidExists").val(req.mes);
                        }
                        else {
                            $("#errormessage").html("发送失败");
                        }
                    },
                    complete: function (req) {
                    },
                    error: function (req) {
                        $("#errormessage").html("发送失败");
                    }
                });
            }
            return false;
        }
        function getYZMold() {
            if ($("#YZMold").html() != "获取验证码") {
                return;
            }
            if ($("#txt_oldphone").val().length <= 0) {
                layer.alert("请输入旧手机号", { icon: 1 });
            }
            else if ($("#txt_oldphone").val().length != 11) {
                layer.alert("请输入正确的手机号", { icon: 1 });
            }
            else {
                var c = 90;
                var interval = setInterval(function () {
                    $("#YZMold").html(c + "s之后重试");
                    c--;
                    if (c == 0) {
                        clearInterval(interval);
                        $("#YZMold").html("获取验证码");
                        c = 90;
                    }
                }, 1000);
                $.ajax({
                    url: "../ajax/membercard/getmemberCard.ashx",
                    dataType: "json",
                    data: { "actionname": "sendmessage", "parameters": "{ \"type\":\"2\", \"phone\": \"" + $("#txt_oldphone").val() + "\" }" },
                    type: "GET",
                    success: function (req) {
                        if (req.status == '0') {
                            $("#errormessage").html("发送成功");
                            $("#errormessage").css("visibility", "collapse");
                        }
                        else {
                            $("#errormessage").html("发送失败");
                        }
                    },
                    complete: function (req) {
                    },
                    error: function (req) {
                        $("#errormessage").html("发送失败");
                    }
                });
            }
            return false;
        }
        function nouseold() {
            if ($("#hidSFZ").val() != "1") {
                $(".SFZ").css("display", "");
                $(".SFZ").css("visibility", "visible");
                $(".old").css("visibility", "collapse");
                $("#txt_oldYZM").attr("isrequired", "false");
                $("#hidSFZ").val("1");
            }
            else {
                $(".SFZ").css("display", "none");
                $(".SFZ").css("visibility", "collapse");
                $(".old").css("visibility", "visible");
                $("#txt_oldYZM").attr("isrequired", "true");
                $("#hidSFZ").val("0");
            }
        }
        function hideold() {
            $(".SFZ").css("display", "none");
            $(".old").css("visibility", "collapse");
            $("#txt_oldYZM").attr("isrequired", "false");
        }
        $(function () {
            $("#filetopImg1").uploadify({
                uploader: '../js/jquery.uploadify/uploadify.swf',
                cancelImg: '../js/jquery.uploadify/cancel.png',
                script: '../ajax/uploads.ashx',
                method: 'GET',
                scriptData: { 'type': '3' },
                folder: '../uploads',
                queueID: 'div_topImg1',
                simUploadLimit: 1,//允许同时上传的个数
                buttonImg: '../img/selectimg.png',
                wmode: 'transparent',
                auto: true,

                onComplete: function (e, q, f, data, d) {
                    $("#hid_topImg1").val(data);
                    $("#topImg1").attr("src", data);
                    $("#Img1").attr("src", data);
                },
                onCancel: function (file) {
                }
            });
            $("#filetopImg2").uploadify({
                uploader: '../js/jquery.uploadify/uploadify.swf',
                cancelImg: '../js/jquery.uploadify/cancel.png',
                script: '../ajax/uploads.ashx',
                method: 'GET',
                scriptData: { 'type': '3' },
                folder: '../uploads',
                queueID: 'div_topImg2',
                simUploadLimit: 1,//允许同时上传的个数
                buttonImg: '../img/selectimg.png',
                wmode: 'transparent',
                auto: true,

                onComplete: function (e, q, f, data, d) {
                    $("#hid_topImg2").val(data);
                    $("#topImg2").attr("src", data);
                    $("#Img2").attr("src", data);
                },
                onCancel: function (file) {
                }
            });
            $(".SFZ").css("display", "none");
            $(".SFZ2").css("visibility", "collapse");
            if ($("#txt_oldphone").val().length <= 0) {
                hideold();
            }
        });

        function save() {
            if ($(".old").css("visibility") == "visible" && $("#txt_oldphone").val().length <= 0) {
                layer.alert("请输入旧手机号", { icon: 1 });
                return;
            }
            else if ($(".old").css("visibility") == "visible" && $("#txt_oldphone").val().length != 11) {
                layer.alert("请输入正确的旧手机号", { icon: 1 });
                return;
            }
            if ($(".old").css("visibility") == "visible" && $("#txt_oldYZM").val().length <= 0) {
                layer.alert("请输入旧手机验证码", { icon: 1 });
                return;
            }
            if ($("#txt_newphone").val().length <= 0) {
                layer.alert("请输入新手机号", { icon: 1 });
                return;
            }
            else if ($("#txt_newphone").val().length != 11) {
                layer.alert("请输入正确的新手机号", { icon: 1 });
                return;
            }
            if ($("#txt_newYZM").val().length <= 0) {
                layer.alert("请输入新手机验证码", { icon: 1 });
                return;
            }
            if ($("#hidSFZ").val() == "1") {
                if ($("#hid_topImg1").val().length <= 0) {
                    layer.alert("请上传身份证正面", { icon: 1 });
                    return;
                }
                if ($("#hid_topImg2").val().length <= 0) {
                    layer.alert("请上传身份证背面", { icon: 1 });
                    return;
                }
            }
            if ($("#hidExists").val().length > 0) {
                layer.confirm('此手机号已经绑定会员' + $("#hidExists").val() + ' 是否将其替换？？', {
                    btn: ['确定', '取消']
                }, function () {
                    $("#Save_btn").click();
                });
            }
            else {
                $("#Save_btn").click();
            }
        }

        function sign() {
            if ($("#txt_newphone").val().length <= 0) {
                layer.alert("请输入新手机号", { icon: 1 });
                return;
            }
            else if ($("#txt_newphone").val().length != 11) {
                layer.alert("请输入正确的新手机号", { icon: 1 });
                return;
            } if ($("#hidSFZ").val() == "1") {
                if ($("#hid_topImg1").val().length <= 0) {
                    layer.alert("请上传身份证正面", { icon: 1 });
                    return;
                }
                if ($("#hid_topImg2").val().length <= 0) {
                    layer.alert("请上传身份证背面", { icon: 1 });
                    return;
                }
                $(".SFZ2").css("visibility", "visible");
            }
            $(".SFZ").css("visibility", "collapse");
            var old = $("#txt_oldphone").val();
            if(old.length<1)
            {
                old = "空";
            }
            $("#Img1").attr('src', $("#hid_topImg1").val());
            $("#Img2").attr('src', $("#hid_topImg2").val());
            $("#signContent").html($("#signContent").html().replace("OLDPHONENO", old).replace("ACCOUNT", $("#hidId").val()).replace("PHONENO", $("#txt_newphone").val()));
            $(".sign").css("visibility", "visible");
            $(".content").css("visibility", "collapse");
            $(".btnEdit").show();
        }
        function print1() {
            $(".btnEdit").hide();
            $("#errormessage").html("");
            window.print();

            setTimeout(function () {
                $(".sign").css("visibility", "collapse");
                $(".content").css("visibility", "visible");
                $(".SFZ2").css("visibility", "collapse");
                if ($("#hidSFZ").val() == "1") {
                    $(".SFZ").css("visibility", "visible");
                }
            }, 3000);
        }
        function back() {
            $(".btnEdit").hide();
            $(".sign").css("visibility", "collapse");
            $(".content").css("visibility", "visible");
            $(".SFZ2").css("visibility", "collapse");
            if ($("#hidSFZ").val() == "1") {
                $(".SFZ").css("visibility", "visible");
            }
        }
        function refresh() {
            $(".gobackbtn").click();
        }
    </script>
</head>
<body data-pagecode="members">
    <div class="content">
        <form id="form1" data-tbname="members" runat="server">
            <div class="fixtoph">&nbsp;</div>
            <div class="fixtop">
                <div class="currentpath">
                    <cc1:CPathBar ID="pathBar" PageCode="memberslist" PageType="Add" MainMenu="members" SubMenu="membersPhoneEdit" runat="server"></cc1:CPathBar>&nbsp;修改手机号
                </div>
                <%--<div class="redline"></div>--%>
                <div class="righttitle">
                    <cc1:CPageTitle ID="PageTitle" Menu="" Operate="updatephone" runat="server"></cc1:CPageTitle>
                </div>
            </div>
            <div class="rightcontent">
                <div class="height25 gray">&nbsp;</div>
                <div class="labelledlist">
                    <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
                </div>
                <div class="updatediv cla">
                    <table>
                        <tr class="old">
                            <td data-code="span_oldphone" class="lefttd">旧手机号：</td>
                            <td>
                                <cc1:CTextBox ID="txt_oldphone" data-code="oldphone_placeholder" CssClass="txtstyle" Enabled="false" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_oldphone')" placeholder="" runat="server"></cc1:CTextBox>&nbsp;<a data-code="span_yzm" visible="true" id="YZMold" href="javascript:void(0);" onclick="getYZMold()" runat="server">获取验证码</a>&nbsp;&nbsp;&nbsp;&nbsp;<a data-code="phonenouse" href="javascript:void(0);" onclick="nouseold()">原手机没用</a>
                            </td>
                        </tr>
                        <tr class="old oldYZM">
                            <td data-code="span_oldYZM" class="lefttd">验证码：</td>
                            <td>
                                <cc1:CTextBox ID="txt_oldYZM" data-code="oldYZM_placeholder" CssClass="txtstyle" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_oldYZM')" placeholder="" runat="server"></cc1:CTextBox>
                            </td>
                        </tr>
                        <tr class="SFZ">
                            <td data-code="span_sfz1" class="lefttd">身份证正面：</td>
                            <td>
                                <div class="SFZ">
                                    <img class="SFZ" runat="server" id="topImg1" src="" width="250" height="160" />
                                    <input type="file" style="vertical-align: middle;" name="filetopImg1" id="filetopImg1" />
                                    <div id="div_topImg1" />
                                    <input type="hidden" id="hid_topImg1" runat="server" style="float: left" />
                                </div>
                            </td>
                        </tr>
                        <tr class="SFZ">
                            <td data-code="span_sfz2" class="lefttd">身份证背面：</td>
                            <td>
                                <div class="SFZ">
                                    <img class="SFZ" runat="server" id="topImg2" src="" width="250" height="160" />
                                    <input type="file" style="vertical-align: middle;" name="filetopImg2" id="filetopImg2" />
                                    <div id="div_topImg2" />
                                    <input type="hidden" id="hid_topImg2" runat="server" style="float: left" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td data-code="span_newphone" class="lefttd">新手机号：</td>
                            <td>
                                <cc1:CTextBox ID="txt_newphone" data-code="newphone_placeholder" CssClass="txtstyle" IsRequired="false" TextType="Normal" onblur="onblurCheck('txt_newphone')" placeholder="" runat="server"></cc1:CTextBox>&nbsp;<a data-code="span_yzm" id="YZMnew" visible="true" href="javascript:void(0);" onclick="getYZMnew()" runat="server">获取验证码</a>
                            </td>
                        </tr>
                        <tr>
                            <td data-code="span_newYZM" class="lefttd">验证码：</td>
                            <td>
                                <cc1:CTextBox ID="txt_newYZM" data-code="newYZM_placeholder" CssClass="txtstyle" IsRequired="false" TextType="Normal" onblur="onblurCheck('txt_newYZM')" placeholder="" runat="server"></cc1:CTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="hidden" id="hidId" runat="server" /></td>
                            <td colspan="5">
                                <div class="savebtn1" data-code="save_edit" id="savebtn1" runat="server" onclick="save()">保存</div>
                                <div class="savebtn1" data-code="sign_edit" id="signbtn" runat="server" onclick="sign()">打印签字</div>
                                <div class="gobackbtn" data-code="back_edit">返回</div>
                                <cc1:CButton ID="Save_btn" runat="server" Style="display: none" OnClick="Save_btn_Click" />


                                <input type="hidden" id="hidoldphone" runat="server" />
                                <input type="hidden" id="hidSFZ" runat="server" value="0" />
                                <input type="hidden" id="hidExists" runat="server" value="" />
                                <span id="errormessage" runat="server"></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="updatediv clb" style="display: none"></div>
                <div class="bottomline"></div>
            </div>
        </form>
    </div>
    <div id="sign" style="visibility: collapse; width: 100%">
        <div class="sign">
            <table style="width: 100%">
                <tr>
                    <td colspan="4">
                        <div data-code="updatePhonesign" style="font-size: 30px;">会员修改手机号确认单</div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="signContent" data-code="updatePhoneContent" style="font-size: 20px;">您将自愿将账号 手机号修改为 </div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr class="SFZ2">
                    <td class="SFZ2" colspan="2">
                        <img runat="server" class="SFZ2" id="Img1" src="" width="250" height="160" style="float: right" />
                    </td>
                    <td class="SFZ2" colspan="2">
                        <img runat="server" class="SFZ2" id="Img2" src="" width="250" height="160" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <a style="float: right; font-size: 20px" id="txt_oname" runat="server"></a>
                        <div data-code="span_oname" style="float: right; font-size: 20px; font-weight: bold">操作人：</div>
                    </td>
                    <td colspan="2">&nbsp;&nbsp;&nbsp;<div data-code="span_memname" style="float: left; font-size: 20px; font-weight: bold">客户签字：</div>
                        <a style="float: left; font-size: 20px">_________________</a>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr class="btnEdit">
                    <td colspan="2">
                        <div class="savebtn1" data-code="print_edit" id="Div1" runat="server" onclick="print1()" style="text-align: center; float: right">打印</div>
                    </td>
                    <td colspan="2">
                        <div class="gobackbtn1" data-code="back_edit" onclick="back();">返回</div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</body>
</html>
