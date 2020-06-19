<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncomingMessageList.aspx.cs" Inherits="CommunityBuy.BackWeb.IncomingMessageList" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
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
    <script>
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
            height: 20px;
            width: 20px;
            line-height: 20px;
        }
    </style>
    <script>
        function select() {
            frame.window.location.reload();
            Iframe1.window.location.reload();
        }

        //修改手机号
        function changePhoneNum() {
            var title = getNameByCode('employeematerial');
            ShowReferPage('MemberPhone', 'MemberPhone', title, '../store/ChangePhoneNumRefer.aspx', 100, '30%', '50%', 'stockpromotionEdit');
        }
    </script>
</head>
<body data-pagecode="IncomingMessage">
    <form id="form1" data-tbname="IncomingMessage" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="IncomingMessagelist" PageType="Normal" MainMenu="" SubMenu="会员信息表" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="来电信息" Operate="Add" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere" style="margin-bottom: 20px; margin-top: 20px; margin-left: 40px;">
                <span data-code="cellPhone">来电号码：</span>
                <cc1:CTextBox ID="phone_where" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" placeholder="" runat="server"></cc1:CTextBox>
                <asp:Button ID="Button1" runat="server" Text="搜索" Style="margin-left: 10px; width: 80px; height: 35px; background-color: #3E4460; color: white" OnClick="Button1_Click" BorderStyle="None" />
            </div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
                <div class="graydiv" data-code="cardinfo" id="clb" title="clb">会员卡信息</div>
                <div class="graydiv" data-code="Recordsofconsumptio" id="clc" title="clc">消费记录</div>
            </div>
            <div class="updatediv cla">
                <table>
                    <tr>
                        <td data-code="span_memcode" class="lefttd">会员编号：</td>
                        <td>
                            <cc1:CTextBox ID="MemberNumber" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" Enabled="false" runat="server"></cc1:CTextBox>
                        </td>

                        <td data-code="span_cname" class="lefttd">会员姓名：</td>
                        <td>
                            <cc1:CTextBox ID="MemberName" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" Enabled="false" runat="server"></cc1:CTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_photo" class="lefttd">照片：</td>
                        <td>
                            <div>
                                <div style="padding-top: 10px; float: left">
                                    <img runat="server" id="txt_photo" width="100" height="100" style="float: left" />
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
                                <img runat="server" id="txt_signature" width="100" height="100" style="float: left" />
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
                        <td data-code="span_sex" class="lefttd">会员性别：</td>
                        <td>
                            <cc1:CTextBox ID="MemberSex" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" Enabled="false" runat="server"></cc1:CTextBox>
                        </td>
                        <td data-code="span_mobile" class="lefttd">手机号码：</td>
                        <td>
                            <div style="display: block; float: left">
                                <cc1:CTextBox ID="MemberPhone" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" runat="server"></cc1:CTextBox>
                            </div>
                            <div style="display: block; float: left">
                                <div class="addbtn" data-code="changephonenum" style="width: 100px;" onclick="changePhoneNum();"></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_idtype" class="lefttd">证件类型：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_idtype" SelType="Normal" CssClass="selstyle" runat="server" Enabled="false"></cc1:CDropDownList>
                        </td>
                        <td data-code="span_IDNO" class="lefttd">证件号码：</td>
                        <td>
                            <cc1:CTextBox ID="txt_IDNO" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" runat="server" Enabled="false"></cc1:CTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_birthday" class="lefttd">会员生日：</td>
                        <td>
                            <cc1:CTextBox ID="MemberBirthday" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" Enabled="false" runat="server"></cc1:CTextBox>
                        </td>
                        <td data-code="span_email" class="lefttd">邮箱地址：</td>
                        <td>
                            <cc1:CTextBox ID="txt_email" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" runat="server"></cc1:CTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_provinceid" class="lefttd">所属区域：</td>
                        <td colspan="3">
                            <select id="sel_provinceid" cssclass="selstyle" isrequired="False" seltype="Normal" runat="server" onchange="getselcity();" style="height: 30px" />
                            <select id="sel_city" cssclass="selstyle" isrequired="False" seltype="Normal" runat="server" onchange="getselarea();" style="height: 30px" />
                            <select id="sel_area" cssclass="selstyle" isrequired="False" seltype="Normal" runat="server" style="height: 30px" />
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
                            <cc1:CTextBox ID="txt_hobby" data-code="hobby_placeholder" CssClass="txtstyle" MaxLength="128" Width="80%" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_hobby')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_hobby_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_remark" class="lefttd">备注：</td>
                        <td colspan="3">
                            <cc1:CTextBox ID="txt_remark" data-code="remark_placeholder" CssClass="txtstyle" MaxLength="128" Width="80%" Height="150px" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_remark')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_remark_span"></span>						</td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" id="hidIdAdd" runat="server" />
                            <input type="hidden" id="hidId" runat="server" /></td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="保存修改" Style="margin-left: 10px; width: 80px; height: 35px; background-color: #3E4460; color: white" BorderStyle="None" OnClick="Button2_Click" />
                            <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="updatediv clb" style="display: none">
                <iframe id="frame" src="<%=url%>" style="width: 100%; height: 500px; border: 0px;"></iframe>
            </div>
            <div class="updatediv clc" style="display: none">
                <iframe id="Iframe1" src="<%=url1%>" style="width: 100%; height: 500px; border: 0px;"></iframe>
            </div>
            <div class="bottomline"></div>
            <input id="HidWhere" runat="server" type="hidden" />
            <input id="HidOrder" type="hidden" runat="server" />
            <input id="HidSortExpression" type="hidden" runat="server" />
            <input id="Ghostsel_provinceids" type="hidden" runat="server" /><%--所属省--%>
            <input id="Ghostsel_citys" type="hidden" runat="server" /><%--所属城市--%>
            <input id="Ghostsel_areas" type="hidden" runat="server" /><%--所属区域--%>
            <input id="Ghostsel_memcardcodes" type="hidden" runat="server" /><%--会员卡号--%>
            <input id="Ghostsel_memid" type="hidden" runat="server" /><%--主键--%>
            <input id="Ghostsel_source" type="hidden" runat="server" /><%--会员来源--%>
            <input id="Ghostsel_wxaccount" type="hidden" runat="server" /><%--微信账号--%>
            <input id="Ghostsel_bigcustomer" type="hidden" runat="server" /><%--是否大客户--%>
            <input id="Ghostsel_tel" type="hidden" runat="server" /><%--客户电话--%>
            <input id="Ghostsel_PhoneNum" type="hidden" runat="server" /><%--会员手机号--%>
            <input id="Ghostsel_cuser" type="hidden" runat="server" />
            <div class="pagelist">
            </div>
        </div>
    </form>
</body>
</html>
