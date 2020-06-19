<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPwd.aspx.cs" Inherits="CommunityBuy.BackWeb.manage.ResetPwd" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script type="text/javascript" src="/js/jquery-1.7.2.min.js"></script>
    <script src="/js/CWebControl.js"></script>
    <script src="/js/listeditjs.js"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/xmlhelper.js"></script>
</head>
<body data-pagecode="resetpwd">
    <form id="form1" data-tbname="resetpwd" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="adminslist" PageType="Normal" MainMenu="系统管理" SubMenu="用户管理" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="用户管理" Operate="" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
            </div>
            <div class="updatediv cla">
                <table style="width:30%">
                    <tr>
                        <td data-code="span_uname" class="lefttd">用户名：</td>
                        <td>
                            <cc1:CTextBox ID="txt_uname" CssClass="reqtxtstyle" MaxLength="64" TextType="User" runat="server" Enabled="false"></cc1:CTextBox></td>
                    </tr>
                    <tr>
                        <td data-code="span_oldpwd" class="lefttd">原密码：</td>
                        <td>
                            <cc1:CTextBox ID="tex_oldpwd" data-code="oldpwd_placeholder" CssClass="selstyle" MaxLength="16" TextMode="Password" runat="server" IsRequired="true" TextType="Pwd" onblur="onblurCheck('tex_oldpwd')">    </cc1:CTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td data-code="span_pwd" class="lefttd">新密码：</td>
                        <td>
                            <cc1:CTextBox ID="txt_pwd" data-code="pwd_placeholder" CssClass="selstyle" MaxLength="16" IsRequired="true" TextMode="Password" TextType="Pwd" runat="server" placeholder="6-16位数字字母组合"></cc1:CTextBox></td>
                    </tr>
                    <tr>
                        <td data-code="span_repwd" class="lefttd">确认密码：</td>
                        <td>
                            <cc1:CTextBox ID="txt_repwd" data-code="repwd_placeholder" CssClass="reqtxtstyle" MaxLength="16" IsRequired="true" TextMode="Password" onblur="checkpwd()" runat="server" TextType="Pwd" placeholder="6-16位数字字母组合"></cc1:CTextBox></td>
                    </tr>

                    <tr>
                        <td>
                            <input type="hidden" id="hidId" runat="server" /></td>
                        <td colspan="5">
                            <div data-code="save_edit" class="savebtn">保存</div>

                            <cc1:CButton ID="Save_btn" runat="server" Style="display: none" OnClick="Save_btn_Click" />&nbsp;&nbsp; <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </form>

    <script>

        //onblur事件
        function checkpwd() {
            var pwd = $('#txt_pwd').val();
            var pwdstatus = false;
            var repwdstatus = false;
            if (pwd != '') {
                pwdstatus = onblurCheck('txt_pwd');
            }
            var repwd = $('#txt_repwd').val();;
            if (repwd != "") {
                repwdstatus = onblurCheck('txt_repwd');
            }

            if (repwdstatus && repwdstatus && pwd != repwd) {
                pcLayerMsg("两次输入密码不一致");
            }
            return true;
        }
    </script>
</body>
</html>
