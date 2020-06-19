<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="membersEdit.aspx.cs" Inherits="CommunityBuy.BackWeb.membersEdit" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <link href="/css/switch.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/CWebControl.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="../js/jquery.uploadify/jquery.uploadify.v2.1.4.js" charset="gbk" type="text/javascript"></script>
    <script src="../js/jquery.uploadify/swfobject.js"></script>
    <link href="../js/jquery.uploadify/uploadify.css" rel="stylesheet" />
    <script type="text/javascript">
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        function showpic() {
            var imgpath = $("#hid_signature").val();
            $("#txt_signature").attr("src", imgpath);
            var idnopath = $("#hid_photo").val();
            $("#txt_photo").attr("src", idnopath);
        }

        $(function () {
            var id = 0;
            if (getQueryString("id") != null) {
                id = getQueryString("id");
            }
            showpic();
            //默认绑定新疆
            getprovice();
            $("#uploadifyaddress").uploadify({
                uploader: '../js/jquery.uploadify/uploadify.swf',
                cancelImg: '../js/jquery.uploadify/cancel.png',
                script: '../ajax/uploads.ashx',
                method: 'GET',
                scriptData: { 'type': '1', 'id': id },
                folder: '../uploads',
                queueID: 'div_backgroundimg',
                simUploadLimit: 1,//允许同时上传的个数
                buttonImg: '../img/selectimg.png',
                wmode: 'transparent',
                auto: true,
                fileTypeExts: '*.jpg; *.png; *.gif',
                fileSizeLimit: '2MB',
                onComplete: function (e, q, f, data, d) {
                    $("#hid_signature").val(data);
                    $("#txt_signature").attr("src", data);
                },
                onCancel: function (file) {
                    $("#txt_signature").attr("src", "");
                }
            }
           );

            $("#txt_signature").click(function () {
                if ($("#txt_signature").attr('src').length > 0) {
                    showbigpic(this);
                }
            });
            $("#txt_photo").click(function () {
                if ($("#txt_photo").attr('src').length > 0) {
                    showbigpic(this);
                }
            });

            $("#filelogo").uploadify({
                uploader: '../js/jquery.uploadify/uploadify.swf',
                cancelImg: '../js/jquery.uploadify/cancel.png',
                script: '../ajax/uploads.ashx',
                method: 'GET',
                scriptData: { 'type': '1', 'id': id },
                folder: '../uploads',
                queueID: 'div_logo',
                simUploadLimit: 1,//允许同时上传的个数
                buttonImg: '../img/selectimg.png',
                wmode: 'transparent',
                auto: true,
                fileTypeExts: '*.jpg; *.png; *.gif',
                fileSizeLimit: '2MB',
                onComplete: function (e, q, f, data, d) {
                    $("#hid_photo").val(data);
                    $("#txt_photo").attr("src", data);
                },
                onCancel: function (file) {
                    $("#txt_photo").attr("src", "");
                }
            });

            if ($('#hidbigcustomer').val() == "1") {
                $(obj).attr('src', '/img/on.png');
            }
        })

        function selectStore() {
            var ids = $("#hidstore").val();
            if (ids == undefined) {
                ids = "";
            }

            var title = getNameByCode('storerefer');
            var linkstr = '../manage/selectStroe.aspx?nameid=txt_stocode&valid=hidstore&id=' + ids;;
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

        function selectmemlikeclass() {
            var title = getNameByCode('memlikeclassrefer');
            var names = $("#txt_hobby").val();
            var linkstr = '/memberCard/memlikeclassrefer.aspx?nameid=txt_hobby&names=' + names;
            var index = layer.open({
                title: title,
                type: 2,
                area: ['80%', '80%'],
                fix: true, //不固定
                maxmin: false,
                content: linkstr
            });
            layer.full(index);
        }

        ////是否大客户
        //function clickbigcustomer(obj) {
        //    if ($(obj).hasClass('on')) {
        //        $(obj).removeClass('on');
        //        $(obj).attr('src', '/img/off.png');
        //        $('#hidbigcustomer').val('0');
        //    }
        //    else {
        //        $(obj).addClass('on');
        //        $(obj).attr('src', '/img/on.png');
        //        $('#hidbigcustomer').val('1');
        //    }
        //}

        //function readbigcustomer() {
        //    var bigcustomer = $('#hidbigcustomer').val();
        //    if (bigcustomer == "0") {
        //        $("#cb_bigcustomer").attr('src', '/img/off.png');
        //        if ($("#cb_bigcustomer").hasClass("on")) {
        //            $("#cb_bigcustomer").removeClass("on");
        //        }
        //    } else {
        //        $("#cb_bigcustomer").attr('src', '/img/on.png');
        //        $("#cb_bigcustomer").addClass("on");
        //    }
        //}

        function onblurCheck1() {
            onblurCheck('txt_IDNO');
            var idtype = $("#ddl_idtype").val();
            var idno = $("#txt_IDNO").val();
            var birthday = "";
            if (idtype == "IDNO") {
                var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
                if (reg.test(idno) == true) {
                    if (idno.length == 15) {
                        birthday = "19" + idno.substr(6, 6);
                    } else if (idno.length == 18) {
                        birthday = idno.substr(6, 8);
                    }

                    birthday = birthday.replace(/(.{4})(.{2})/, "$1-$2-");
                    $("#txt_birthday").val(birthday);
                } else {
                    pcLayerMsg("身份证号码格式不正确");
                    return false;
                }
            }
        }
    </script>

    <style type="text/css">
        #MenuList {
            width: 100%;
            margin-left: 50px;
        }

        .simg {
            width: 20px;
            height: 20px;
            vertical-align: middle;
            margin-left: 5px;
            margin-right: 5px;
            cursor: pointer;
        }

        .delete {
            /*background-image: url(../js/jquery.uploadify/cancel.png);*/
            height: 20px;
            width: 20px;
            line-height: 20px;
        }
    </style>
</head>
<body data-pagecode="members">
    <form id="form1" data-tbname="members" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="memberslist" PageType="Add" MainMenu="" SubMenu="会员信息表" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="会员信息表" Operate="Add" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
                <%--<div class="graydiv" data-code="cardinfo" id="clb" title="clb">会员卡信息</div>--%>
            </div>
            <div class="updatediv cla">
                <table>
                    <tr>
                        <td data-code="span_source" class="lefttd">会员来源：</td>
                        <td>
                            <cc1:CDropDownList ID="txt_source" CssClass="selstyle" IsRequired="False" Enabled="false" SelType="Normal" runat="server"></cc1:CDropDownList>
                        </td>
                        <%--<td data-code="span_strcode" class="lefttd">所属门店：</td>
                        <td>
                            <input type="hidden" id="hidstore" runat="server" />
                            <cc1:CTextBox ID="txt_stocode" data-code="strcode_placeholder" CssClass="txtstyle" MaxLength="8" IsRequired="False" TextType="Normal" ReadOnly="false" onfocus="selectStore()" placeholder="" runat="server" Enabled="False"></cc1:CTextBox><span class="redmess" id="txt_strcode_span"></span>
                            <img src="/img/search.png" onclick="selectStore()" class="simg" />
                        </td>--%>
                        <%--<td data-code="span_bigcustomer" class="lefttd">大客户：</td>
                        <td>
                            <img src="/img/off.png" id="cb_bigcustomer" class="btn-recyble" runat="server" onclick="clickbigcustomer(this);" />
                            <input type="hidden" id="hidbigcustomer" runat="server" />
                        </td>--%>
                        <td data-code="span_wxaccount" class="lefttd">微信账户：</td>
                        <td>
                            <cc1:CTextBox ID="txt_wxaccount" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" Enabled="false" runat="server"></cc1:CTextBox><span class="redmess" id="txt_wxaccount_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_cname" class="lefttd">姓名：</td>
                        <td>
                            <cc1:CTextBox ID="txt_cname" data-code="cname_placeholder" CssClass="txtstyle" MaxLength="32" IsRequired="true" TextType="Normal" onblur="onblurCheck('txt_cname')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_cname_span"></span></td>
                        <td data-code="span_sex" class="lefttd">性别：</td>
                        <td>
                            <cc1:CDropDownList ID="txt_sex" CssClass="selstyle" IsRequired="False" SelType="Sex" runat="server"></cc1:CDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_mobile" class="lefttd">手机号码：</td>
                        <td>
                            <cc1:CTextBox ID="txt_mobile" data-code="mobile_placeholder" CssClass="reqtxtstyle" MaxLength="12" IsRequired="true" TextType="Mobile" onblur="onblurCheck('txt_mobile')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_mobile_span"></span></td>
                        <td data-code="span_birthday" class="lefttd">生日：</td>
                        <td>
                            <cc1:CTextBox ID="txt_birthday" data-code="birthday_placeholder" Enabled="false" CssClass="txtstyle" MaxLength="10" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_birthday')" onfocus="ShowShortDate();" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_birthday_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_idtype" class="lefttd">证件类型：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_idtype" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>
                        </td>
                        <td data-code="span_IDNO" class="lefttd">证件号码：</td>
                        <td>
                            <cc1:CTextBox ID="txt_IDNO" data-code="IDNO_placeholder" CssClass="reqtxtstyle" MaxLength="20" IsRequired="true" TextType="Normal" onblur="onblurCheck1()" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_IDNO_span"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_email" class="lefttd">邮箱地址：</td>
                        <td>
                            <cc1:CTextBox ID="txt_email" data-code="email_placeholder" CssClass="txtstyle" MaxLength="128" IsRequired="False" TextType="Email" onblur="onblurCheck('txt_email')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_email_span"></span></td>
                        <td data-code="span_tel" class="lefttd">电话：</td>
                        <td>
                            <cc1:CTextBox ID="txt_tel" data-code="tel_placeholder" CssClass="txtstyle" MaxLength="64" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_tel')" placeholder="" runat="server" Width="300px"></cc1:CTextBox><span class="redmess" id="txt_tel_span"></span></td>
                    </tr>
                    <tr>
                        <td data-code="span_provinceid" class="lefttd">所属区域：</td>
                        <td colspan="3">
                            <select id="sel_provinceid" cssclass="selstyle" isrequired="False" seltype="Normal" runat="server" onchange="getselcity();" style="height: 30px" />
                            <input type="hidden" runat="server" id="hid_provinceid" />
                            <select id="sel_city" cssclass="selstyle" isrequired="False" seltype="Normal" runat="server" onchange="getselarea();" style="height: 30px" />
                            <input type="hidden" runat="server" id="hidcity" />
                            <select id="sel_area" cssclass="selstyle" isrequired="False" seltype="Normal" runat="server" style="height: 30px" />
                            <input type="hidden" runat="server" id="hidarea" />
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_photo" class="lefttd">照片：</td>
                        <td>
                            <div>
                                <div style="padding-top: 10px; float: left">
                                    <asp:Image runat="server" ID="txt_photo" width="100" height="100" style="float: left" />
                                </div>
                                <div style="padding-top: 50px; float: left; padding-left: 5px">
                                    <input type="file" style="vertical-align: middle;" name="filelogo" id="filelogo" />
                                    <div id="div_logo"></div>
                                    <input type="hidden" id="hid_photo" runat="server" style="float: left" />
                                </div>
                            </div>
                        </td>
                        <td data-code="span_signature" class="lefttd">电子签名：</td>
                        <td>
                            <div style="padding-top: 10px; float: left">
                                <asp:Image runat="server" id="txt_signature" width="100" height="100" style="float: left" />
                            </div>
                            <div style="padding-top: 50px; float: left; padding-left: 5px">
                                <input type="hidden" id="hid_signature" runat="server" />
                                <input type="file" style="height: 27px; vertical-align: middle; margin-top: 5px" name="uploadifyaddress" id="uploadifyaddress" />
                                <div id="div_backgroundimg"></div>
                            </div>
                            <div style="padding-top: 50px; float: left; padding-left: 20px">
                                <span data-code="span_imginfo" class="lefttd"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_address" class="lefttd">会员地址：</td>
                        <td colspan="3">
                            <cc1:CTextBox ID="txt_address" data-code="address_placeholder" CssClass="txtstyle" MaxLength="128" Width="80%" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_address')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_address_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_hobby" class="lefttd">特殊爱好：</td>
                        <td colspan="3">
                            <cc1:CTextBox ID="txt_hobby" data-code="hobby_placeholder" CssClass="txtstyle" MaxLength="128" Width="80%" IsRequired="False" TextType="Normal" onfocus="selectmemlikeclass();" placeholder="" runat="server"></cc1:CTextBox></td>
                    </tr>
                    <tr>
                        <td data-code="span_remark" class="lefttd">备注：</td>
                        <td colspan="3">
                            <cc1:CTextBox ID="txt_remark" data-code="remark_placeholder" CssClass="txtstyle" MaxLength="128" Width="80%" Height="150px" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_remark')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_remark_span"></span>						</td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" id="hidIdAdd" runat="server" />
                            <input type="hidden" id="hidId" runat="server" />
                            <input type="hidden" id="types" runat="server" />
                        </td>
                        <td>
                            <div class="savebtn">保存</div>
                            <div class="gobackbtn">返回</div>
                            <cc1:CButton ID="Save_btn" runat="server" Style="display: none" OnClick="Save_btn_Click" />
                            <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <%--<div class="updatediv clb" style="display: none">
                <iframe id="frame" src="<%=url%>" style="width: 100%; height: 500px; border: 0px;"></iframe>
            </div>--%>
            <div class="bottomline"></div>
        </div>
    </form>
</body>
</html>
